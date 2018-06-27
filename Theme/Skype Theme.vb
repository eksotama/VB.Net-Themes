Imports System, System.IO, System.Collections.Generic
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging

'------------------
'Creator: aeonhack
'Site: *****
'Created: 08/02/2011
'Changed: 12/06/2011
'Version: 1.5.4
'------------------

MustInherit Class ThemeContainer154
    Inherits ContainerControl

#Region " Initialization "

    Protected G As Graphics, B As Bitmap

    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)

        _ImageSize = Size.Empty
        Font = New Font("Verdana", 8S)

        MeasureBitmap = New Bitmap(1, 1)
        MeasureGraphics = Graphics.FromImage(MeasureBitmap)

        DrawRadialPath = New GraphicsPath

        InvalidateCustimization()
    End Sub

    Protected NotOverridable Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        If DoneCreation Then InitializeMessages()

        InvalidateCustimization()
        ColorHook()

        If Not _LockWidth = 0 Then Width = _LockWidth
        If Not _LockHeight = 0 Then Height = _LockHeight
        If Not _ControlMode Then MyBase.Dock = DockStyle.Fill

        Transparent = _Transparent
        If _Transparent AndAlso _BackColor Then BackColor = Color.Transparent

        MyBase.OnHandleCreated(e)
    End Sub

    Private DoneCreation As Boolean
    Protected NotOverridable Overrides Sub OnParentChanged(ByVal e As EventArgs)
        MyBase.OnParentChanged(e)

        If Parent Is Nothing Then Return
        _IsParentForm = TypeOf Parent Is Form

        If Not _ControlMode Then
            InitializeMessages()

            If _IsParentForm Then
                ParentForm.FormBorderStyle = _BorderStyle
                ParentForm.TransparencyKey = _TransparencyKey

                If Not DesignMode Then
                    AddHandler ParentForm.Shown, AddressOf FormShown
                End If
            End If

            Parent.BackColor = BackColor
        End If

        OnCreation()
        DoneCreation = True
        InvalidateTimer()
    End Sub

#End Region

    Private Sub DoAnimation(ByVal i As Boolean)
        OnAnimation()
        If i Then Invalidate()
    End Sub

    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return

        If _Transparent AndAlso _ControlMode Then
            PaintHook()
            e.Graphics.DrawImage(B, 0, 0)
        Else
            G = e.Graphics
            PaintHook()
        End If
    End Sub

    Protected Overrides Sub OnHandleDestroyed(ByVal e As EventArgs)
        RemoveAnimationCallback(AddressOf DoAnimation)
        MyBase.OnHandleDestroyed(e)
    End Sub

    Private HasShown As Boolean
    Private Sub FormShown(ByVal sender As Object, ByVal e As EventArgs)
        If _ControlMode OrElse HasShown Then Return

        If _StartPosition = FormStartPosition.CenterParent OrElse _StartPosition = FormStartPosition.CenterScreen Then
            Dim SB As Rectangle = Screen.PrimaryScreen.Bounds
            Dim CB As Rectangle = ParentForm.Bounds
            ParentForm.Location = New Point(SB.Width \ 2 - CB.Width \ 2, SB.Height \ 2 - CB.Width \ 2)
        End If

        HasShown = True
    End Sub


#Region " Size Handling "

    Private Frame As Rectangle
    Protected NotOverridable Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If _Movable AndAlso Not _ControlMode Then
            Frame = New Rectangle(7, 7, Width - 14, _Header - 7)
        End If

        InvalidateBitmap()
        Invalidate()

        MyBase.OnSizeChanged(e)
    End Sub

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        If Not _LockWidth = 0 Then width = _LockWidth
        If Not _LockHeight = 0 Then height = _LockHeight
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

#End Region

#Region " State Handling "

    Protected State As MouseState
    Private Sub SetState(ByVal current As MouseState)
        State = current
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If Not (_IsParentForm AndAlso ParentForm.WindowState = FormWindowState.Maximized) Then
            If _Sizable AndAlso Not _ControlMode Then InvalidateMouse()
        End If

        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
        If Enabled Then SetState(MouseState.None) Else SetState(MouseState.Block)
        MyBase.OnEnabledChanged(e)
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        SetState(MouseState.Over)
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        SetState(MouseState.Over)
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        SetState(MouseState.None)

        If GetChildAtPoint(PointToClient(MousePosition)) IsNot Nothing Then
            If _Sizable AndAlso Not _ControlMode Then
                Cursor = Cursors.Default
                Previous = 0
            End If
        End If

        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then SetState(MouseState.Down)

        If Not (_IsParentForm AndAlso ParentForm.WindowState = FormWindowState.Maximized OrElse _ControlMode) Then
            If _Movable AndAlso Frame.Contains(e.Location) Then
                Capture = False
                WM_LMBUTTONDOWN = True
                DefWndProc(Messages(0))
            ElseIf _Sizable AndAlso Not Previous = 0 Then
                Capture = False
                WM_LMBUTTONDOWN = True
                DefWndProc(Messages(Previous))
            End If
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Private WM_LMBUTTONDOWN As Boolean
    Protected Overrides Sub WndProc(ByRef m As Message)
        MyBase.WndProc(m)

        If WM_LMBUTTONDOWN AndAlso m.Msg = 513 Then
            WM_LMBUTTONDOWN = False

            SetState(MouseState.Over)
            If Not _SmartBounds Then Return

            If IsParentMdi Then
                CorrectBounds(New Rectangle(Point.Empty, Parent.Parent.Size))
            Else
                CorrectBounds(Screen.FromControl(Parent).WorkingArea)
            End If
        End If
    End Sub

    Private GetIndexPoint As Point
    Private B1, B2, B3, B4 As Boolean
    Private Function GetIndex() As Integer
        GetIndexPoint = PointToClient(MousePosition)
        B1 = GetIndexPoint.X < 7
        B2 = GetIndexPoint.X > Width - 7
        B3 = GetIndexPoint.Y < 7
        B4 = GetIndexPoint.Y > Height - 7

        If B1 AndAlso B3 Then Return 4
        If B1 AndAlso B4 Then Return 7
        If B2 AndAlso B3 Then Return 5
        If B2 AndAlso B4 Then Return 8
        If B1 Then Return 1
        If B2 Then Return 2
        If B3 Then Return 3
        If B4 Then Return 6
        Return 0
    End Function

    Private Current, Previous As Integer
    Private Sub InvalidateMouse()
        Current = GetIndex()
        If Current = Previous Then Return

        Previous = Current
        Select Case Previous
            Case 0
                Cursor = Cursors.Default
            Case 1, 2
                Cursor = Cursors.SizeWE
            Case 3, 6
                Cursor = Cursors.SizeNS
            Case 4, 8
                Cursor = Cursors.SizeNWSE
            Case 5, 7
                Cursor = Cursors.SizeNESW
        End Select
    End Sub

    Private Messages(8) As Message
    Private Sub InitializeMessages()
        Messages(0) = Message.Create(Parent.Handle, 161, New IntPtr(2), IntPtr.Zero)
        For I As Integer = 1 To 8
            Messages(I) = Message.Create(Parent.Handle, 161, New IntPtr(I + 9), IntPtr.Zero)
        Next
    End Sub

    Private Sub CorrectBounds(ByVal bounds As Rectangle)
        If Parent.Width > bounds.Width Then Parent.Width = bounds.Width
        If Parent.Height > bounds.Height Then Parent.Height = bounds.Height

        Dim X As Integer = Parent.Location.X
        Dim Y As Integer = Parent.Location.Y

        If X < bounds.X Then X = bounds.X
        If Y < bounds.Y Then Y = bounds.Y

        Dim Width As Integer = bounds.X + bounds.Width
        Dim Height As Integer = bounds.Y + bounds.Height

        If X + Parent.Width > Width Then X = Width - Parent.Width
        If Y + Parent.Height > Height Then Y = Height - Parent.Height

        Parent.Location = New Point(X, Y)
    End Sub

#End Region


#Region " Base Properties "

    Overrides Property Dock As DockStyle
        Get
            Return MyBase.Dock
        End Get
        Set(ByVal value As DockStyle)
            If Not _ControlMode Then Return
            MyBase.Dock = value
        End Set
    End Property

    Private _BackColor As Boolean
    <Category("Misc")> _
    Overrides Property BackColor() As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            If value = MyBase.BackColor Then Return

            If Not IsHandleCreated AndAlso _ControlMode AndAlso value = Color.Transparent Then
                _BackColor = True
                Return
            End If

            MyBase.BackColor = value
            If Parent IsNot Nothing Then
                If Not _ControlMode Then Parent.BackColor = value
                ColorHook()
            End If
        End Set
    End Property

    Overrides Property MinimumSize As Size
        Get
            Return MyBase.MinimumSize
        End Get
        Set(ByVal value As Size)
            MyBase.MinimumSize = value
            If Parent IsNot Nothing Then Parent.MinimumSize = value
        End Set
    End Property

    Overrides Property MaximumSize As Size
        Get
            Return MyBase.MaximumSize
        End Get
        Set(ByVal value As Size)
            MyBase.MaximumSize = value
            If Parent IsNot Nothing Then Parent.MaximumSize = value
        End Set
    End Property

    Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property

    Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            Invalidate()
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property ForeColor() As Color
        Get
            Return Color.Empty
        End Get
        Set(ByVal value As Color)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
        Set(ByVal value As Image)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImageLayout() As ImageLayout
        Get
            Return ImageLayout.None
        End Get
        Set(ByVal value As ImageLayout)
        End Set
    End Property

#End Region

#Region " Public Properties "

    Private _SmartBounds As Boolean = True
    Property SmartBounds() As Boolean
        Get
            Return _SmartBounds
        End Get
        Set(ByVal value As Boolean)
            _SmartBounds = value
        End Set
    End Property

    Private _Movable As Boolean = True
    Property Movable() As Boolean
        Get
            Return _Movable
        End Get
        Set(ByVal value As Boolean)
            _Movable = value
        End Set
    End Property

    Private _Sizable As Boolean = True
    Property Sizable() As Boolean
        Get
            Return _Sizable
        End Get
        Set(ByVal value As Boolean)
            _Sizable = value
        End Set
    End Property

    Private _TransparencyKey As Color
    Property TransparencyKey() As Color
        Get
            If _IsParentForm AndAlso Not _ControlMode Then Return ParentForm.TransparencyKey Else Return _TransparencyKey
        End Get
        Set(ByVal value As Color)
            If value = _TransparencyKey Then Return
            _TransparencyKey = value

            If _IsParentForm AndAlso Not _ControlMode Then
                ParentForm.TransparencyKey = value
                ColorHook()
            End If
        End Set
    End Property

    Private _BorderStyle As FormBorderStyle
    Property BorderStyle() As FormBorderStyle
        Get
            If _IsParentForm AndAlso Not _ControlMode Then Return ParentForm.FormBorderStyle Else Return _BorderStyle
        End Get
        Set(ByVal value As FormBorderStyle)
            _BorderStyle = value

            If _IsParentForm AndAlso Not _ControlMode Then
                ParentForm.FormBorderStyle = value

                If Not value = FormBorderStyle.None Then
                    Movable = False
                    Sizable = False
                End If
            End If
        End Set
    End Property

    Private _StartPosition As FormStartPosition
    Property StartPosition As FormStartPosition
        Get
            If _IsParentForm AndAlso Not _ControlMode Then Return ParentForm.StartPosition Else Return _StartPosition
        End Get
        Set(ByVal value As FormStartPosition)
            _StartPosition = value

            If _IsParentForm AndAlso Not _ControlMode Then
                ParentForm.StartPosition = value
            End If
        End Set
    End Property

    Private _NoRounding As Boolean
    Property NoRounding() As Boolean
        Get
            Return _NoRounding
        End Get
        Set(ByVal v As Boolean)
            _NoRounding = v
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            If value Is Nothing Then _ImageSize = Size.Empty Else _ImageSize = value.Size

            _Image = value
            Invalidate()
        End Set
    End Property

    Private Items As New Dictionary(Of String, Color)
    Property Colors() As Bloom()
        Get
            Dim T As New List(Of Bloom)
            Dim E As Dictionary(Of String, Color).Enumerator = Items.GetEnumerator

            While E.MoveNext
                T.Add(New Bloom(E.Current.Key, E.Current.Value))
            End While

            Return T.ToArray
        End Get
        Set(ByVal value As Bloom())
            For Each B As Bloom In value
                If Items.ContainsKey(B.Name) Then Items(B.Name) = B.Value
            Next

            InvalidateCustimization()
            ColorHook()
            Invalidate()
        End Set
    End Property

    Private _Customization As String
    Property Customization() As String
        Get
            Return _Customization
        End Get
        Set(ByVal value As String)
            If value = _Customization Then Return

            Dim Data As Byte()
            Dim Items As Bloom() = Colors

            Try
                Data = Convert.FromBase64String(value)
                For I As Integer = 0 To Items.Length - 1
                    Items(I).Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4))
                Next
            Catch
                Return
            End Try

            _Customization = value

            Colors = Items
            ColorHook()
            Invalidate()
        End Set
    End Property

    Private _Transparent As Boolean
    Property Transparent() As Boolean
        Get
            Return _Transparent
        End Get
        Set(ByVal value As Boolean)
            _Transparent = value
            If Not (IsHandleCreated OrElse _ControlMode) Then Return

            If Not value AndAlso Not BackColor.A = 255 Then
                Throw New Exception("Unable to change value to false while a transparent BackColor is in use.")
            End If

            SetStyle(ControlStyles.Opaque, Not value)
            SetStyle(ControlStyles.SupportsTransparentBackColor, value)

            InvalidateBitmap()
            Invalidate()
        End Set
    End Property

#End Region

#Region " Private Properties "

    Private _ImageSize As Size
    Protected ReadOnly Property ImageSize() As Size
        Get
            Return _ImageSize
        End Get
    End Property

    Private _IsParentForm As Boolean
    Protected ReadOnly Property IsParentForm As Boolean
        Get
            Return _IsParentForm
        End Get
    End Property

    Protected ReadOnly Property IsParentMdi As Boolean
        Get
            If Parent Is Nothing Then Return False
            Return Parent.Parent IsNot Nothing
        End Get
    End Property

    Private _LockWidth As Integer
    Protected Property LockWidth() As Integer
        Get
            Return _LockWidth
        End Get
        Set(ByVal value As Integer)
            _LockWidth = value
            If Not LockWidth = 0 AndAlso IsHandleCreated Then Width = LockWidth
        End Set
    End Property

    Private _LockHeight As Integer
    Protected Property LockHeight() As Integer
        Get
            Return _LockHeight
        End Get
        Set(ByVal value As Integer)
            _LockHeight = value
            If Not LockHeight = 0 AndAlso IsHandleCreated Then Height = LockHeight
        End Set
    End Property

    Private _Header As Integer = 24
    Protected Property Header() As Integer
        Get
            Return _Header
        End Get
        Set(ByVal v As Integer)
            _Header = v

            If Not _ControlMode Then
                Frame = New Rectangle(7, 7, Width - 14, v - 7)
                Invalidate()
            End If
        End Set
    End Property

    Private _ControlMode As Boolean
    Protected Property ControlMode() As Boolean
        Get
            Return _ControlMode
        End Get
        Set(ByVal v As Boolean)
            _ControlMode = v

            Transparent = _Transparent
            If _Transparent AndAlso _BackColor Then BackColor = Color.Transparent

            InvalidateBitmap()
            Invalidate()
        End Set
    End Property

    Private _IsAnimated As Boolean
    Protected Property IsAnimated() As Boolean
        Get
            Return _IsAnimated
        End Get
        Set(ByVal value As Boolean)
            _IsAnimated = value
            InvalidateTimer()
        End Set
    End Property

#End Region


#Region " Property Helpers "

    Protected Function GetPen(ByVal name As String) As Pen
        Return New Pen(Items(name))
    End Function
    Protected Function GetPen(ByVal name As String, ByVal width As Single) As Pen
        Return New Pen(Items(name), width)
    End Function

    Protected Function GetBrush(ByVal name As String) As SolidBrush
        Return New SolidBrush(Items(name))
    End Function

    Protected Function GetColor(ByVal name As String) As Color
        Return Items(name)
    End Function

    Protected Sub SetColor(ByVal name As String, ByVal value As Color)
        If Items.ContainsKey(name) Then Items(name) = value Else Items.Add(name, value)
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(a, r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal value As Color)
        SetColor(name, Color.FromArgb(a, value))
    End Sub

    Private Sub InvalidateBitmap()
        If _Transparent AndAlso _ControlMode Then
            If Width = 0 OrElse Height = 0 Then Return
            B = New Bitmap(Width, Height, PixelFormat.Format32bppPArgb)
            G = Graphics.FromImage(B)
        Else
            G = Nothing
            B = Nothing
        End If
    End Sub

    Private Sub InvalidateCustimization()
        Dim M As New MemoryStream(Items.Count * 4)

        For Each B As Bloom In Colors
            M.Write(BitConverter.GetBytes(B.Value.ToArgb), 0, 4)
        Next

        M.Close()
        _Customization = Convert.ToBase64String(M.ToArray)
    End Sub

    Private Sub InvalidateTimer()
        If DesignMode OrElse Not DoneCreation Then Return

        If _IsAnimated Then
            AddAnimationCallback(AddressOf DoAnimation)
        Else
            RemoveAnimationCallback(AddressOf DoAnimation)
        End If
    End Sub

#End Region


#Region " User Hooks "

    Protected MustOverride Sub ColorHook()
    Protected MustOverride Sub PaintHook()

    Protected Overridable Sub OnCreation()
    End Sub

    Protected Overridable Sub OnAnimation()
    End Sub

#End Region


#Region " Offset "

    Private OffsetReturnRectangle As Rectangle
    Protected Function Offset(ByVal r As Rectangle, ByVal amount As Integer) As Rectangle
        OffsetReturnRectangle = New Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2))
        Return OffsetReturnRectangle
    End Function

    Private OffsetReturnSize As Size
    Protected Function Offset(ByVal s As Size, ByVal amount As Integer) As Size
        OffsetReturnSize = New Size(s.Width + amount, s.Height + amount)
        Return OffsetReturnSize
    End Function

    Private OffsetReturnPoint As Point
    Protected Function Offset(ByVal p As Point, ByVal amount As Integer) As Point
        OffsetReturnPoint = New Point(p.X + amount, p.Y + amount)
        Return OffsetReturnPoint
    End Function

#End Region

#Region " Center "

    Private CenterReturn As Point

    Protected Function Center(ByVal p As Rectangle, ByVal c As Rectangle) As Point
        CenterReturn = New Point((p.Width \ 2 - c.Width \ 2) + p.X + c.X, (p.Height \ 2 - c.Height \ 2) + p.Y + c.Y)
        Return CenterReturn
    End Function
    Protected Function Center(ByVal p As Rectangle, ByVal c As Size) As Point
        CenterReturn = New Point((p.Width \ 2 - c.Width \ 2) + p.X, (p.Height \ 2 - c.Height \ 2) + p.Y)
        Return CenterReturn
    End Function

    Protected Function Center(ByVal child As Rectangle) As Point
        Return Center(Width, Height, child.Width, child.Height)
    End Function
    Protected Function Center(ByVal child As Size) As Point
        Return Center(Width, Height, child.Width, child.Height)
    End Function
    Protected Function Center(ByVal childWidth As Integer, ByVal childHeight As Integer) As Point
        Return Center(Width, Height, childWidth, childHeight)
    End Function

    Protected Function Center(ByVal p As Size, ByVal c As Size) As Point
        Return Center(p.Width, p.Height, c.Width, c.Height)
    End Function

    Protected Function Center(ByVal pWidth As Integer, ByVal pHeight As Integer, ByVal cWidth As Integer, ByVal cHeight As Integer) As Point
        CenterReturn = New Point(pWidth \ 2 - cWidth \ 2, pHeight \ 2 - cHeight \ 2)
        Return CenterReturn
    End Function

#End Region

#Region " Measure "

    Private MeasureBitmap As Bitmap
    Private MeasureGraphics As Graphics

    Protected Function Measure() As Size
        SyncLock MeasureGraphics
            Return MeasureGraphics.MeasureString(Text, Font, Width).ToSize
        End SyncLock
    End Function
    Protected Function Measure(ByVal text As String) As Size
        SyncLock MeasureGraphics
            Return MeasureGraphics.MeasureString(text, Font, Width).ToSize
        End SyncLock
    End Function

#End Region


#Region " DrawPixel "

    Private DrawPixelBrush As SolidBrush

    Protected Sub DrawPixel(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer)
        If _Transparent Then
            B.SetPixel(x, y, c1)
        Else
            DrawPixelBrush = New SolidBrush(c1)
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1)
        End If
    End Sub

#End Region

#Region " DrawCorners "

    Private DrawCornersBrush As SolidBrush

    Protected Sub DrawCorners(ByVal c1 As Color, ByVal offset As Integer)
        DrawCorners(c1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle, ByVal offset As Integer)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawCorners(ByVal c1 As Color)
        DrawCorners(c1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        If _NoRounding Then Return

        If _Transparent Then
            B.SetPixel(x, y, c1)
            B.SetPixel(x + (width - 1), y, c1)
            B.SetPixel(x, y + (height - 1), c1)
            B.SetPixel(x + (width - 1), y + (height - 1), c1)
        Else
            DrawCornersBrush = New SolidBrush(c1)
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1)
        End If
    End Sub

#End Region

#Region " DrawBorders "

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal offset As Integer)
        DrawBorders(p1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle, ByVal offset As Integer)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen)
        DrawBorders(p1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        G.DrawRectangle(p1, x, y, width - 1, height - 1)
    End Sub

#End Region

#Region " DrawText "

    Private DrawTextPoint As Point
    Private DrawTextSize As Size

    Protected Sub DrawText(ByVal b1 As Brush, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawText(b1, Text, a, x, y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If text.Length = 0 Then Return

        DrawTextSize = Measure(text)
        DrawTextPoint = New Point(Width \ 2 - DrawTextSize.Width \ 2, Header \ 2 - DrawTextSize.Height \ 2)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Center
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Right
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y)
        End Select
    End Sub

    Protected Sub DrawText(ByVal b1 As Brush, ByVal p1 As Point)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, p1)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal x As Integer, ByVal y As Integer)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, x, y)
    End Sub

#End Region

#Region " DrawImage "

    Private DrawImagePoint As Point

    Protected Sub DrawImage(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, a, x, y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        DrawImagePoint = New Point(Width \ 2 - image.Width \ 2, Header \ 2 - image.Height \ 2)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height)
            Case HorizontalAlignment.Center
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height)
            Case HorizontalAlignment.Right
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height)
        End Select
    End Sub

    Protected Sub DrawImage(ByVal p1 As Point)
        DrawImage(_Image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, x, y)
    End Sub

    Protected Sub DrawImage(ByVal image As Image, ByVal p1 As Point)
        DrawImage(image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        G.DrawImage(image, x, y, image.Width, image.Height)
    End Sub

#End Region

#Region " DrawGradient "

    Private DrawGradientBrush As LinearGradientBrush
    Private DrawGradientRectangle As Rectangle

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle)
    End Sub
    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, 90.0F)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, angle)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub


    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, 90.0F)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, angle)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub

#End Region

#Region " DrawRadial "

    Private DrawRadialPath As GraphicsPath
    Private DrawRadialBrush1 As PathGradientBrush
    Private DrawRadialBrush2 As LinearGradientBrush
    Private DrawRadialRectangle As Rectangle

    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, width \ 2, height \ 2)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal center As Point)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal cx As Integer, ByVal cy As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, cx, cy)
    End Sub

    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle)
        DrawRadial(blend, r, r.Width \ 2, r.Height \ 2)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal center As Point)
        DrawRadial(blend, r, center.X, center.Y)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal cx As Integer, ByVal cy As Integer)
        DrawRadialPath.Reset()
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1)

        DrawRadialBrush1 = New PathGradientBrush(DrawRadialPath)
        DrawRadialBrush1.CenterPoint = New Point(r.X + cx, r.Y + cy)
        DrawRadialBrush1.InterpolationColors = blend

        If G.SmoothingMode = SmoothingMode.AntiAlias Then
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3)
        Else
            G.FillEllipse(DrawRadialBrush1, r)
        End If
    End Sub


    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(c1, c2, DrawGradientRectangle)
    End Sub
    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(c1, c2, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        DrawRadialBrush2 = New LinearGradientBrush(r, c1, c2, 90.0F)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawRadialBrush2 = New LinearGradientBrush(r, c1, c2, angle)
        G.FillEllipse(DrawGradientBrush, r)
    End Sub

#End Region

#Region " CreateRound "

    Private CreateRoundPath As GraphicsPath
    Private CreateRoundRectangle As Rectangle

    Function CreateRound(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal slope As Integer) As GraphicsPath
        CreateRoundRectangle = New Rectangle(x, y, width, height)
        Return CreateRound(CreateRoundRectangle, slope)
    End Function

    Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

#End Region

End Class

MustInherit Class ThemeControl154
    Inherits Control


#Region " Initialization "

    Protected G As Graphics, B As Bitmap

    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)

        _ImageSize = Size.Empty
        Font = New Font("Verdana", 8S)

        MeasureBitmap = New Bitmap(1, 1)
        MeasureGraphics = Graphics.FromImage(MeasureBitmap)

        DrawRadialPath = New GraphicsPath

        InvalidateCustimization() 'Remove?
    End Sub

    Protected NotOverridable Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        InvalidateCustimization()
        ColorHook()

        If Not _LockWidth = 0 Then Width = _LockWidth
        If Not _LockHeight = 0 Then Height = _LockHeight

        Transparent = _Transparent
        If _Transparent AndAlso _BackColor Then BackColor = Color.Transparent

        MyBase.OnHandleCreated(e)
    End Sub

    Private DoneCreation As Boolean
    Protected NotOverridable Overrides Sub OnParentChanged(ByVal e As EventArgs)
        If Parent IsNot Nothing Then
            OnCreation()
            DoneCreation = True
            InvalidateTimer()
        End If

        MyBase.OnParentChanged(e)
    End Sub

#End Region

    Private Sub DoAnimation(ByVal i As Boolean)
        OnAnimation()
        If i Then Invalidate()
    End Sub

    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return

        If _Transparent Then
            PaintHook()
            e.Graphics.DrawImage(B, 0, 0)
        Else
            G = e.Graphics
            PaintHook()
        End If
    End Sub

    Protected Overrides Sub OnHandleDestroyed(ByVal e As EventArgs)
        RemoveAnimationCallback(AddressOf DoAnimation)
        MyBase.OnHandleDestroyed(e)
    End Sub

#Region " Size Handling "

    Protected NotOverridable Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If _Transparent Then
            InvalidateBitmap()
        End If

        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub

    Protected Overrides Sub SetBoundsCore(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal specified As BoundsSpecified)
        If Not _LockWidth = 0 Then width = _LockWidth
        If Not _LockHeight = 0 Then height = _LockHeight
        MyBase.SetBoundsCore(x, y, width, height, specified)
    End Sub

#End Region

#Region " State Handling "

    Private InPosition As Boolean
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        InPosition = True
        SetState(MouseState.Over)
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        If InPosition Then SetState(MouseState.Over)
        MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then SetState(MouseState.Down)
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        InPosition = False
        SetState(MouseState.None)
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(ByVal e As EventArgs)
        If Enabled Then SetState(MouseState.None) Else SetState(MouseState.Block)
        MyBase.OnEnabledChanged(e)
    End Sub

    Protected State As MouseState
    Private Sub SetState(ByVal current As MouseState)
        State = current
        Invalidate()
    End Sub

#End Region


#Region " Base Properties "

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property ForeColor() As Color
        Get
            Return Color.Empty
        End Get
        Set(ByVal value As Color)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
        Set(ByVal value As Image)
        End Set
    End Property
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Overrides Property BackgroundImageLayout() As ImageLayout
        Get
            Return ImageLayout.None
        End Get
        Set(ByVal value As ImageLayout)
        End Set
    End Property

    Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            Invalidate()
        End Set
    End Property
    Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            Invalidate()
        End Set
    End Property

    Private _BackColor As Boolean
    <Category("Misc")> _
    Overrides Property BackColor() As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            If Not IsHandleCreated AndAlso value = Color.Transparent Then
                _BackColor = True
                Return
            End If

            MyBase.BackColor = value
            If Parent IsNot Nothing Then ColorHook()
        End Set
    End Property

#End Region

#Region " Public Properties "

    Private _NoRounding As Boolean
    Property NoRounding() As Boolean
        Get
            Return _NoRounding
        End Get
        Set(ByVal v As Boolean)
            _NoRounding = v
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            If value Is Nothing Then
                _ImageSize = Size.Empty
            Else
                _ImageSize = value.Size
            End If

            _Image = value
            Invalidate()
        End Set
    End Property

    Private _Transparent As Boolean
    Property Transparent() As Boolean
        Get
            Return _Transparent
        End Get
        Set(ByVal value As Boolean)
            _Transparent = value
            If Not IsHandleCreated Then Return

            If Not value AndAlso Not BackColor.A = 255 Then
                Throw New Exception("Unable to change value to false while a transparent BackColor is in use.")
            End If

            SetStyle(ControlStyles.Opaque, Not value)
            SetStyle(ControlStyles.SupportsTransparentBackColor, value)

            If value Then InvalidateBitmap() Else B = Nothing
            Invalidate()
        End Set
    End Property

    Private Items As New Dictionary(Of String, Color)
    Property Colors() As Bloom()
        Get
            Dim T As New List(Of Bloom)
            Dim E As Dictionary(Of String, Color).Enumerator = Items.GetEnumerator

            While E.MoveNext
                T.Add(New Bloom(E.Current.Key, E.Current.Value))
            End While

            Return T.ToArray
        End Get
        Set(ByVal value As Bloom())
            For Each B As Bloom In value
                If Items.ContainsKey(B.Name) Then Items(B.Name) = B.Value
            Next

            InvalidateCustimization()
            ColorHook()
            Invalidate()
        End Set
    End Property

    Private _Customization As String
    Property Customization() As String
        Get
            Return _Customization
        End Get
        Set(ByVal value As String)
            If value = _Customization Then Return

            Dim Data As Byte()
            Dim Items As Bloom() = Colors

            Try
                Data = Convert.FromBase64String(value)
                For I As Integer = 0 To Items.Length - 1
                    Items(I).Value = Color.FromArgb(BitConverter.ToInt32(Data, I * 4))
                Next
            Catch
                Return
            End Try

            _Customization = value

            Colors = Items
            ColorHook()
            Invalidate()
        End Set
    End Property

#End Region

#Region " Private Properties "

    Private _ImageSize As Size
    Protected ReadOnly Property ImageSize() As Size
        Get
            Return _ImageSize
        End Get
    End Property

    Private _LockWidth As Integer
    Protected Property LockWidth() As Integer
        Get
            Return _LockWidth
        End Get
        Set(ByVal value As Integer)
            _LockWidth = value
            If Not LockWidth = 0 AndAlso IsHandleCreated Then Width = LockWidth
        End Set
    End Property

    Private _LockHeight As Integer
    Protected Property LockHeight() As Integer
        Get
            Return _LockHeight
        End Get
        Set(ByVal value As Integer)
            _LockHeight = value
            If Not LockHeight = 0 AndAlso IsHandleCreated Then Height = LockHeight
        End Set
    End Property

    Private _IsAnimated As Boolean
    Protected Property IsAnimated() As Boolean
        Get
            Return _IsAnimated
        End Get
        Set(ByVal value As Boolean)
            _IsAnimated = value
            InvalidateTimer()
        End Set
    End Property

#End Region


#Region " Property Helpers "

    Protected Function GetPen(ByVal name As String) As Pen
        Return New Pen(Items(name))
    End Function
    Protected Function GetPen(ByVal name As String, ByVal width As Single) As Pen
        Return New Pen(Items(name), width)
    End Function

    Protected Function GetBrush(ByVal name As String) As SolidBrush
        Return New SolidBrush(Items(name))
    End Function

    Protected Function GetColor(ByVal name As String) As Color
        Return Items(name)
    End Function

    Protected Sub SetColor(ByVal name As String, ByVal value As Color)
        If Items.ContainsKey(name) Then Items(name) = value Else Items.Add(name, value)
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SetColor(name, Color.FromArgb(a, r, g, b))
    End Sub
    Protected Sub SetColor(ByVal name As String, ByVal a As Byte, ByVal value As Color)
        SetColor(name, Color.FromArgb(a, value))
    End Sub

    Private Sub InvalidateBitmap()
        If Width = 0 OrElse Height = 0 Then Return
        B = New Bitmap(Width, Height, PixelFormat.Format32bppPArgb)
        G = Graphics.FromImage(B)
    End Sub

    Private Sub InvalidateCustimization()
        Dim M As New MemoryStream(Items.Count * 4)

        For Each B As Bloom In Colors
            M.Write(BitConverter.GetBytes(B.Value.ToArgb), 0, 4)
        Next

        M.Close()
        _Customization = Convert.ToBase64String(M.ToArray)
    End Sub

    Private Sub InvalidateTimer()
        If DesignMode OrElse Not DoneCreation Then Return

        If _IsAnimated Then
            AddAnimationCallback(AddressOf DoAnimation)
        Else
            RemoveAnimationCallback(AddressOf DoAnimation)
        End If
    End Sub
#End Region


#Region " User Hooks "

    Protected MustOverride Sub ColorHook()
    Protected MustOverride Sub PaintHook()

    Protected Overridable Sub OnCreation()
    End Sub

    Protected Overridable Sub OnAnimation()
    End Sub

#End Region


#Region " Offset "

    Private OffsetReturnRectangle As Rectangle
    Protected Function Offset(ByVal r As Rectangle, ByVal amount As Integer) As Rectangle
        OffsetReturnRectangle = New Rectangle(r.X + amount, r.Y + amount, r.Width - (amount * 2), r.Height - (amount * 2))
        Return OffsetReturnRectangle
    End Function

    Private OffsetReturnSize As Size
    Protected Function Offset(ByVal s As Size, ByVal amount As Integer) As Size
        OffsetReturnSize = New Size(s.Width + amount, s.Height + amount)
        Return OffsetReturnSize
    End Function

    Private OffsetReturnPoint As Point
    Protected Function Offset(ByVal p As Point, ByVal amount As Integer) As Point
        OffsetReturnPoint = New Point(p.X + amount, p.Y + amount)
        Return OffsetReturnPoint
    End Function

#End Region

#Region " Center "

    Private CenterReturn As Point

    Protected Function Center(ByVal p As Rectangle, ByVal c As Rectangle) As Point
        CenterReturn = New Point((p.Width \ 2 - c.Width \ 2) + p.X + c.X, (p.Height \ 2 - c.Height \ 2) + p.Y + c.Y)
        Return CenterReturn
    End Function
    Protected Function Center(ByVal p As Rectangle, ByVal c As Size) As Point
        CenterReturn = New Point((p.Width \ 2 - c.Width \ 2) + p.X, (p.Height \ 2 - c.Height \ 2) + p.Y)
        Return CenterReturn
    End Function

    Protected Function Center(ByVal child As Rectangle) As Point
        Return Center(Width, Height, child.Width, child.Height)
    End Function
    Protected Function Center(ByVal child As Size) As Point
        Return Center(Width, Height, child.Width, child.Height)
    End Function
    Protected Function Center(ByVal childWidth As Integer, ByVal childHeight As Integer) As Point
        Return Center(Width, Height, childWidth, childHeight)
    End Function

    Protected Function Center(ByVal p As Size, ByVal c As Size) As Point
        Return Center(p.Width, p.Height, c.Width, c.Height)
    End Function

    Protected Function Center(ByVal pWidth As Integer, ByVal pHeight As Integer, ByVal cWidth As Integer, ByVal cHeight As Integer) As Point
        CenterReturn = New Point(pWidth \ 2 - cWidth \ 2, pHeight \ 2 - cHeight \ 2)
        Return CenterReturn
    End Function

#End Region

#Region " Measure "

    Private MeasureBitmap As Bitmap
    Private MeasureGraphics As Graphics 'TODO: Potential issues during multi-threading.

    Protected Function Measure() As Size
        Return MeasureGraphics.MeasureString(Text, Font, Width).ToSize
    End Function
    Protected Function Measure(ByVal text As String) As Size
        Return MeasureGraphics.MeasureString(text, Font, Width).ToSize
    End Function

#End Region


#Region " DrawPixel "

    Private DrawPixelBrush As SolidBrush

    Protected Sub DrawPixel(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer)
        If _Transparent Then
            B.SetPixel(x, y, c1)
        Else
            DrawPixelBrush = New SolidBrush(c1)
            G.FillRectangle(DrawPixelBrush, x, y, 1, 1)
        End If
    End Sub

#End Region

#Region " DrawCorners "

    Private DrawCornersBrush As SolidBrush

    Protected Sub DrawCorners(ByVal c1 As Color, ByVal offset As Integer)
        DrawCorners(c1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle, ByVal offset As Integer)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawCorners(ByVal c1 As Color)
        DrawCorners(c1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        If _NoRounding Then Return

        If _Transparent Then
            B.SetPixel(x, y, c1)
            B.SetPixel(x + (width - 1), y, c1)
            B.SetPixel(x, y + (height - 1), c1)
            B.SetPixel(x + (width - 1), y + (height - 1), c1)
        Else
            DrawCornersBrush = New SolidBrush(c1)
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1)
        End If
    End Sub

#End Region

#Region " DrawBorders "

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal offset As Integer)
        DrawBorders(p1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle, ByVal offset As Integer)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen)
        DrawBorders(p1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        G.DrawRectangle(p1, x, y, width - 1, height - 1)
    End Sub

#End Region

#Region " DrawText "

    Private DrawTextPoint As Point
    Private DrawTextSize As Size

    Protected Sub DrawText(ByVal b1 As Brush, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawText(b1, Text, a, x, y)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal text As String, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If text.Length = 0 Then Return

        DrawTextSize = Measure(text)
        DrawTextPoint = Center(DrawTextSize)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Center
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Right
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y)
        End Select
    End Sub

    Protected Sub DrawText(ByVal b1 As Brush, ByVal p1 As Point)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, p1)
    End Sub
    Protected Sub DrawText(ByVal b1 As Brush, ByVal x As Integer, ByVal y As Integer)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, x, y)
    End Sub

#End Region

#Region " DrawImage "

    Private DrawImagePoint As Point

    Protected Sub DrawImage(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, a, x, y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        DrawImagePoint = Center(image.Size)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawImage(image, x, DrawImagePoint.Y + y, image.Width, image.Height)
            Case HorizontalAlignment.Center
                G.DrawImage(image, DrawImagePoint.X + x, DrawImagePoint.Y + y, image.Width, image.Height)
            Case HorizontalAlignment.Right
                G.DrawImage(image, Width - image.Width - x, DrawImagePoint.Y + y, image.Width, image.Height)
        End Select
    End Sub

    Protected Sub DrawImage(ByVal p1 As Point)
        DrawImage(_Image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal x As Integer, ByVal y As Integer)
        DrawImage(_Image, x, y)
    End Sub

    Protected Sub DrawImage(ByVal image As Image, ByVal p1 As Point)
        DrawImage(image, p1.X, p1.Y)
    End Sub
    Protected Sub DrawImage(ByVal image As Image, ByVal x As Integer, ByVal y As Integer)
        If image Is Nothing Then Return
        G.DrawImage(image, x, y, image.Width, image.Height)
    End Sub

#End Region

#Region " DrawGradient "

    Private DrawGradientBrush As LinearGradientBrush
    Private DrawGradientRectangle As Rectangle

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle)
    End Sub
    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(blend, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, 90.0F)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, Color.Empty, Color.Empty, angle)
        DrawGradientBrush.InterpolationColors = blend
        G.FillRectangle(DrawGradientBrush, r)
    End Sub


    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawGradientRectangle = New Rectangle(x, y, width, height)
        DrawGradient(c1, c2, DrawGradientRectangle, angle)
    End Sub

    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, 90.0F)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub
    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawGradientBrush = New LinearGradientBrush(r, c1, c2, angle)
        G.FillRectangle(DrawGradientBrush, r)
    End Sub

#End Region

#Region " DrawRadial "

    Private DrawRadialPath As GraphicsPath
    Private DrawRadialBrush1 As PathGradientBrush
    Private DrawRadialBrush2 As LinearGradientBrush
    Private DrawRadialRectangle As Rectangle

    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, width \ 2, height \ 2)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal center As Point)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, center.X, center.Y)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal cx As Integer, ByVal cy As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(blend, DrawRadialRectangle, cx, cy)
    End Sub

    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle)
        DrawRadial(blend, r, r.Width \ 2, r.Height \ 2)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal center As Point)
        DrawRadial(blend, r, center.X, center.Y)
    End Sub
    Sub DrawRadial(ByVal blend As ColorBlend, ByVal r As Rectangle, ByVal cx As Integer, ByVal cy As Integer)
        DrawRadialPath.Reset()
        DrawRadialPath.AddEllipse(r.X, r.Y, r.Width - 1, r.Height - 1)

        DrawRadialBrush1 = New PathGradientBrush(DrawRadialPath)
        DrawRadialBrush1.CenterPoint = New Point(r.X + cx, r.Y + cy)
        DrawRadialBrush1.InterpolationColors = blend

        If G.SmoothingMode = SmoothingMode.AntiAlias Then
            G.FillEllipse(DrawRadialBrush1, r.X + 1, r.Y + 1, r.Width - 3, r.Height - 3)
        Else
            G.FillEllipse(DrawRadialBrush1, r)
        End If
    End Sub


    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(c1, c2, DrawRadialRectangle)
    End Sub
    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        DrawRadialRectangle = New Rectangle(x, y, width, height)
        DrawRadial(c1, c2, DrawRadialRectangle, angle)
    End Sub

    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        DrawRadialBrush2 = New LinearGradientBrush(r, c1, c2, 90.0F)
        G.FillEllipse(DrawRadialBrush2, r)
    End Sub
    Protected Sub DrawRadial(ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle, ByVal angle As Single)
        DrawRadialBrush2 = New LinearGradientBrush(r, c1, c2, angle)
        G.FillEllipse(DrawRadialBrush2, r)
    End Sub

#End Region

#Region " CreateRound "

    Private CreateRoundPath As GraphicsPath
    Private CreateRoundRectangle As Rectangle

    Function CreateRound(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal slope As Integer) As GraphicsPath
        CreateRoundRectangle = New Rectangle(x, y, width, height)
        Return CreateRound(CreateRoundRectangle, slope)
    End Function

    Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

#End Region

End Class

Module ThemeShare

#Region " Animation "

    Private Frames As Integer
    Private Invalidate As Boolean
    Public ThemeTimer As New PrecisionTimer

    Private Const FPS As Integer = 50 '1000 / 50 = 20 FPS
    Private Const Rate As Integer = 10

    Public Delegate Sub AnimationDelegate(ByVal invalidate As Boolean)

    Private Callbacks As New List(Of AnimationDelegate)

    Private Sub HandleCallbacks(ByVal state As IntPtr, ByVal reserve As Boolean)
        Invalidate = (Frames >= FPS)
        If Invalidate Then Frames = 0

        SyncLock Callbacks
            For I As Integer = 0 To Callbacks.Count - 1
                Callbacks(I).Invoke(Invalidate)
            Next
        End SyncLock

        Frames += Rate
    End Sub

    Private Sub InvalidateThemeTimer()
        If Callbacks.Count = 0 Then
            ThemeTimer.Delete()
        Else
            ThemeTimer.Create(0, Rate, AddressOf HandleCallbacks)
        End If
    End Sub

    Sub AddAnimationCallback(ByVal callback As AnimationDelegate)
        SyncLock Callbacks
            If Callbacks.Contains(callback) Then Return

            Callbacks.Add(callback)
            InvalidateThemeTimer()
        End SyncLock
    End Sub

    Sub RemoveAnimationCallback(ByVal callback As AnimationDelegate)
        SyncLock Callbacks
            If Not Callbacks.Contains(callback) Then Return

            Callbacks.Remove(callback)
            InvalidateThemeTimer()
        End SyncLock
    End Sub

#End Region

End Module

Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum

Structure Bloom

    Public _Name As String
    ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Private _Value As Color
    Property Value() As Color
        Get
            Return _Value
        End Get
        Set(ByVal value As Color)
            _Value = value
        End Set
    End Property

    Property ValueHex() As String
        Get
            Return String.Concat("#", _
            _Value.R.ToString("X2", Nothing), _
            _Value.G.ToString("X2", Nothing), _
            _Value.B.ToString("X2", Nothing))
        End Get
        Set(ByVal value As String)
            Try
                _Value = ColorTranslator.FromHtml(value)
            Catch
                Return
            End Try
        End Set
    End Property


    Sub New(ByVal name As String, ByVal value As Color)
        _Name = name
        _Value = value
    End Sub
End Structure

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 11/30/2011
'Changed: 11/30/2011
'Version: 1.0.0
'------------------
Class PrecisionTimer
    Implements IDisposable

    Private _Enabled As Boolean
    ReadOnly Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
    End Property

    Private Handle As IntPtr
    Private TimerCallback As TimerDelegate

    <DllImport("kernel32.dll", EntryPoint:="CreateTimerQueueTimer")> _
    Private Shared Function CreateTimerQueueTimer( _
    ByRef handle As IntPtr, _
    ByVal queue As IntPtr, _
    ByVal callback As TimerDelegate, _
    ByVal state As IntPtr, _
    ByVal dueTime As UInteger, _
    ByVal period As UInteger, _
    ByVal flags As UInteger) As Boolean
    End Function

    <DllImport("kernel32.dll", EntryPoint:="DeleteTimerQueueTimer")> _
    Private Shared Function DeleteTimerQueueTimer( _
    ByVal queue As IntPtr, _
    ByVal handle As IntPtr, _
    ByVal callback As IntPtr) As Boolean
    End Function

    Delegate Sub TimerDelegate(ByVal r1 As IntPtr, ByVal r2 As Boolean)

    Sub Create(ByVal dueTime As UInteger, ByVal period As UInteger, ByVal callback As TimerDelegate)
        If _Enabled Then Return

        TimerCallback = callback
        Dim Success As Boolean = CreateTimerQueueTimer(Handle, IntPtr.Zero, TimerCallback, IntPtr.Zero, dueTime, period, 0)

        If Not Success Then ThrowNewException("CreateTimerQueueTimer")
        _Enabled = Success
    End Sub

    Sub Delete()
        If Not _Enabled Then Return
        Dim Success As Boolean = DeleteTimerQueueTimer(IntPtr.Zero, Handle, IntPtr.Zero)

        If Not Success AndAlso Not Marshal.GetLastWin32Error = 997 Then
            ThrowNewException("DeleteTimerQueueTimer")
        End If

        _Enabled = Not Success
    End Sub

    Private Sub ThrowNewException(ByVal name As String)
        Throw New Exception(String.Format("{0} failed. Win32Error: {1}", name, Marshal.GetLastWin32Error))
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Delete()
    End Sub
End Class

#Region "Skype Theme"
Class SkypeButton
    Inherits ThemeControl154
    Public Function RoundUp(ByVal d As Double) As Integer
        If d.ToString.Contains(",") Then
            Return CInt(d.ToString.Split(",")(0)) + 1
        Else
            Return CInt(d)
        End If
    End Function
    Private Function CodeToImage(ByVal Code As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(Code)))
    End Function
    Dim Side_Left As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAgCAIAAAC6rk4JAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAANVJREFUKFNVx81LwgAAhvG3PzqKltmnmnrw0tVD05xpERTkqUMEdQgKOr2VOvOj/GIjcOg8WFu94H6n51kpPi78qfftDOfzGfJ3k+w5y7ftvjtF7tL+Ld/3XddF5oy98WQUQrpKx3GGIRxW+VfBpCocSDB9QbLCL0HiNDqfgoMyexKdfYtdwZ7FjmDXYluis2PxQ7BdYkuCsQXxk+g0BVtFNgSx5dkssC4wCnyXYN4EhslXwYbJh+f/w2r+5eiqzhBSNW/dZO6C909E9maRvPaMkr12zB/HeDzVgnCjBwAAAABJRU5ErkJggg==")
    Dim Side_Right As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAgCAIAAAC6rk4JAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAANVJREFUKFNdyT1PwgAQxvFjQIyDiQMflZBYFXkRUIlxce0AKO9dMMHJgZA4OrBwKlLEAoUm7QAN6YLXJk+C/JbL/7lQQeNwOHJ8Ej04PCLT2dS6E5nU3pocx/E8T/q+rZMVMKzVbYtpGbBt+6aJkL6WWEC+wWSCH3PI7Ua2zjQDP6ZwVdsLAzJVpl9IS0wgtR8/kJTPGC4rTDr8i8QT0wj8+IaLx504lxjCWZnpCxSJT/DjA5QS4u2dTyU48PA8jKl9ku1OG8SLnH9xSY5S0XMdt/C6/QOl4zQlUbpikwAAAABJRU5ErkJggg==")
    Dim Side_Left_Hover As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAgCAIAAAC6rk4JAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAANdJREFUKFNdxzFPwmAYReHLT3WQAKKoKGwMstlqpTjg4GhcJAS2NyEkDsTFwYqLgAKCoAJplUZIF/AruUN9pnNCV/dL15kMe83F3EWhOk6fy03Vcn/nyFzW/8rzPMdxkMrL989s4sOhKbZtj31q1qXmwJQvUvNJSJryQdjPBWdE2DuTIQVn15B3QsKQAanpU3B2DHkjbJ9Kj9R0CfGT4HQIW7q8EmL/5oUQ1aVNalqEqCZNQkQT6/FpvdjIli6Kd88+ZCvTsCbm9e2D1YBeWx6Vp/F8ffNYVpzZMpYYLrVaAAAAAElFTkSuQmCC")
    Dim Side_Right_Hover As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAgCAIAAAC6rk4JAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAANVJREFUKFNlyrtvgXEUxvFjaxODwWTp1v/LIHGr1n0zsLm0WgYdbEYibL9EmMRKTG5t0dK65H2JkLwL5/cmT4J+lpPvk2MIZ8XNrdFyd280mWm3PxRrHZ5ytSWpqqppGnck36SVTt1sQxlBS52iKME3BLeMBQQ4/kDGL/jPw/cqaA4yZuBNX8UPPL0I+oZHjin8iwl4+G0MD8+CvuAi3ClBnyDjA1zJs3ByjEDGEBwcA5DRB0cC0e507Rw93XupZY2Vibd0oWGLi1h1TXzcmWa0sk7WjydC2yozBDZ0HwAAAABJRU5ErkJggg==")
    Dim Side_Left_Click As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAgCAIAAAC6rk4JAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAXtJREFUKFM90esvQmEcwPGf9/4O1iaq07nk9NLmvyFSnXJKF5dcy/2Su3ljXrhf4o2NjWHT6foYW4pTmAjltuE52rz8vP1+i+ZPfj7f88Xf91+fHzC1n5vfQQ6fn+XWYWEvbvf5S3S7pfo9mPMjrXlbxh3JnecSZMZDuetS6clKKOMFhVtU977BrB/JbTGl+1bCzD+8eZjeRuW2mMp9S2JMYVijqrY06cnB5BYq56Oq1jTZ84cKPkK0pKjuV5jYRBWNEaI5RXX9QWEJq10i3fkC4xtIYQ6rnSLd8QI+DC4kof0ZxtaR0hQiHSLjLsAYIu0i0/YMo2tIZQhSTTea1iyMYDQEKduNpiULwxh6gbJea5qfJBB6geavK10F1At0Y7LS+QhDq4ioExhLknU8wiCGLsCYE6w9AwMriKgNMFyCbcpA/wpS15wxpiut7QH6lguIs/ydhCrulDZcaLgkdC8GjcMHtP4cWwrfu4RMo8fV1ijgJdgDG3HvEvoFGSnkfAH6/cgAAAAASUVORK5CYII=")
    Dim Side_Right_Click As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAgCAIAAAC6rk4JAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAXtJREFUKFNjXLbnJisb+xcmUTYOLgbriq0Ns3Yu3X1z/pGvDOrFB9UK9tTP2rXq4AMG3aZbmhUnbSp3Lt51k8Gw/6Nu813NshMgjsnE7wYdz3RqLkE5hh0vdetuLALJTPgG4SwEckz7vxm1v9SrvbFgJ5DT99Wo7YVezfX5QI5Z31fjthf6Ndfn7QByer8Ytz7Xr7kG5vR8MW55rl99be72mwzm3Z9Nmp8ZVF0Fc7o+mzQ9M6i8OmcbkNP5CcSpuDIbyLHo+GTa+Myw/MqsrUBO+0fThqeGZZdBHMu2j2b1T41KL8/cAuS0fjSre2pUcnkGiNPywaz2iVHxpelAjlXzB/OaJ8ZQTtN78+rHxkVgGevG9xZVj40LL03bDOQ0vLOofGRccHEqiFP/zqLikXH+xSmbbjLY1L21KH9okndhMpBjXfPKovwBkDNpI9CeisfmpXdcys+COECWefGtsulHe9dcZnCvvV4+8xQ04CdsuDll2wNgFCw98x8ArkjdwxRv+JQAAAAASUVORK5CYII=")
    Dim ClickBG As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAD8AAAAgCAIAAAAqvTfGAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAL1JREFUWEfVzikOwlAARdHHYkiQCNr+tp8toUjqSJhn6AAsAY1EsAGW0KDQBEXngSAxJNi+m7OAWztefPBW75x5oWFdeaG5evBC6/DmBWVf8oKyK3lB8QpeUL3iyy1UKlDd/DcnVysPfxc1J68saHbGC9o24wWxSXlBrFNeEKuEF/Rlwgv6IuYFYx7zgjGLecGYRrxgTiJeMMchL8hRyAtyGPCCHAS80O6/eJHfy96TF4R15wWte+MF++Tz+gAX4QMt3+M70AAAAABJRU5ErkJggg==")
    Sub New()
        LockHeight = 32
    End Sub
    Private _Text_Margin_Left As Integer = 0
    Property Text_Margin_Left() As Integer
        Get
            Return _Text_Margin_Left
        End Get
        Set(ByVal value As Integer)
            _Text_Margin_Left = value
            Invalidate()
        End Set
    End Property
    Private _Image As Image = Nothing
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            _Image = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub ColorHook()
    End Sub
    Public Enum AllignMode
        Left = 1
        Right = 2
    End Enum
    Private _AllignModes As AllignMode = AllignMode.Left
    Property ImageAllignmentMode() As AllignMode

        Get
            Return _AllignModes
        End Get
        Set(ByVal value As AllignMode)
            _AllignModes = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub PaintHook()
        G.Clear(Color.Fuchsia)
        G.SmoothingMode = SmoothingMode.HighSpeed

        If State = MouseState.Down Then
            For i = 0 To RoundUp(Width / 63)
                G.DrawImage(ClickBG, i * 63, 0)
            Next
            G.DrawImage(Side_Left_Click, 0, 0, 4, 32)
            G.DrawImage(Side_Right_Click, Width - 4, 0, 4, 32)
            G.DrawString(Text, New Font("Arial", 10, FontStyle.Bold), Brushes.White, 5 + _Text_Margin_Left, 8)
        ElseIf State = MouseState.None Then
            DrawGradient(Color.FromArgb(140, 174, 217), Color.FromArgb(119, 162, 217), ClientRectangle)
            DrawGradient(Color.FromArgb(237, 237, 237), Color.FromArgb(217, 217, 217), New Rectangle(1, 2, Width - 2, Height - 3))
            G.DrawLine(Pens.White, 1, 1, Width, 1)
            G.DrawImage(Side_Left, 0, 0, 4, 32)
            G.DrawImage(Side_Right, Width - 4, 0, 4, 32)
            G.DrawString(Text, New Font("Arial", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(51, 51, 51)), 5 + _Text_Margin_Left, 8)
        ElseIf State = MouseState.Over Then
            DrawGradient(Color.FromArgb(119, 148, 185), Color.FromArgb(101, 138, 185), ClientRectangle)
            DrawGradient(Color.FromArgb(237, 237, 237), Color.FromArgb(217, 217, 217), New Rectangle(1, 2, Width - 2, Height - 3))
            G.DrawLine(Pens.White, 1, 1, Width, 1)
            G.DrawImage(Side_Left_Hover, 0, 0, 4, 32)
            G.DrawImage(Side_Right_Hover, Width - 4, 0, 4, 32)
            G.DrawString(Text, New Font("Arial", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(51, 51, 51)), 5 + _Text_Margin_Left, 8)
        End If
        Try
            If _AllignModes = AllignMode.Left Then
                G.DrawImage(_Image, 8, 8, 16, 16)
            Else
                G.DrawImage(_Image, Width - 24, 8, 16, 16)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
Class SkypeAdvertisement
    Inherits ThemeControl154
    Private Function CodeToImage(ByVal Code As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(Code)))
    End Function
    Dim CornerImg1 As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAMAAAADCAIAAADZSiLoAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAB9JREFUGFdj7Nn3X4iLQVGYgWHeif/H7oMQw/7bUBYA31ESKR4EWfAAAAAASUVORK5CYII=")
    Dim CornerImg2 As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAMAAAADCAIAAADZSiLoAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAB9JREFUGFdj3H/7//23DO++MTAcu/8fiOad+A9lAWUAOvkV2aJ25r4AAAAASUVORK5CYII=")
    Public Function FlipImage(ByVal img As Bitmap) As Bitmap
        Dim NewBMP As New Bitmap(img)
        For x = 0 To img.Width - 1
            For y = 0 To img.Height - 1
                Dim NewX As Integer = img.Width - x - 1
                Dim NewY As Integer = img.Height - y - 1
                NewBMP.SetPixel(NewX, NewY, img.GetPixel(x, y))
            Next
        Next
        Return NewBMP
    End Function
    Sub New()
    End Sub
    Private _Image As Image = Nothing
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            _Image = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(198, 223, 255))
        G.DrawImage(CornerImg1, 0, 0)
        G.DrawImage(CornerImg2, Width - 3, 0)
        G.DrawImage(FlipImage(CornerImg2), 0, Height - 3)
        G.DrawImage(FlipImage(CornerImg1), Width - 3, Height - 3)
        Try
            G.DrawImage(_Image, 5, 5, Width - 10, Height - 10)
        Catch ex As Exception

        End Try
    End Sub
End Class
Class SkypeButton2
    Inherits ThemeControl154
    Protected Sub DrawTextNew(ByVal b1 As Brush, ByVal text As String) ''Made by Aeonhack. Edited by me so it fits in the theme.
        If text.Length = 0 Then Return

        Dim DrawTextSize As Size = Measure(text)
        Dim DrawTextPoint As Point = New Point(Width \ 2 - DrawTextSize.Width \ 2, Height \ 2 - DrawTextSize.Height \ 2)


        G.DrawString(text, New Font("Arial", 10, FontStyle.Bold), b1, DrawTextPoint.X, DrawTextPoint.Y)

    End Sub
    Public Function RoundUp(ByVal d As Double) As Integer
        If d.ToString.Contains(",") Then
            Return CInt(d.ToString.Split(",")(0)) + 1
        Else
            Return CInt(d)
        End If
    End Function
    Private Function CodeToImage(ByVal Code As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(Code)))
    End Function
    Sub New()
        LockHeight = 25
        Font = New Font("Arial", 10, FontStyle.Bold)
    End Sub
    Protected Overrides Sub ColorHook()
    End Sub
    Public Enum ImgState
        Text = 1
        Image = 2
    End Enum
    Private _ImgState As ImgState = ImgState.Text
    Property ImageState() As ImgState
        Get
            Return _ImgState
        End Get
        Set(ByVal value As ImgState)
            _ImgState = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub PaintHook()
        Dim Normal_Left As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAsAAAAZCAIAAABo0EPhAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAASpJREFUOE99kz1vglAYhW///87kRBgIEcKALCwSB1gkfARDQpAmaEVtaWkLJTSBHoqJoFyf9T68ufecl6e2bckddV1//FOWJYExpKqq7XZr23aWZfCaphkZp9OpP/sdcDWOxyOOi6L4GXMx0jS1LOt7is7AN4Zh5Hn+NUVneJ632+0+KRC8Z7lcYgANEkVRHMePjNVqhUf2+UxCFosFAninQ2RZhvEAIknS20M643w+v9IhiqIgDEg0iGmavu/jOTTIfr+fz+eojQbBEqiqutlsUN4kXS9YGZ7nkyR5maIzMAbJiqII6Z7LfqAXXdcFQcC85zHXHYOEklmWXa/XQ2e0p5Bc10WGs9kMKQRBgNpvdx13OhwOjuNomsZxHMMwt0b/Z8BDl7h1GIZ/ySbf47asV7IAAAAASUVORK5CYII=")
        Dim Normal_Right As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAsAAAAZCAIAAABo0EPhAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAS9JREFUOE99kj1vglAUhk8XBqb+eicm40CIEAZhcYEw4KKIBmJCgCZA+WptbSslNsGeWxoDgeuz3JDzcD/Oex4sy2JZ9vEPhmFgSNM0l8vleDxut9sgCOq6vvaBnw6tVxRF14HvPufzGaU8z28SfI2x2WyyLGsl+BzjdDqZpom7E+ODQhzH+/2eGKjT0DStqqp7RhiGvu/DOx18tmEY8EYH26MoCuByB1mW4fUukiTBC52yLImBCw1syWKxALwwDcdxVqsVYEg05vN5kiSACY3iuq6qqjg68DxGFEU8z+NAkVzwY4goithN3IAYT33wP0EQdF3HOP/noyus1+vpdIqR3spkDwzwcDjgqziOw/7sdrtumRiTyWQ2my2XS9u20zRtz+5Nsud5eFMMb1hrvV/5S9RzkFpijAAAAABJRU5ErkJggg==")
        Dim Normal_BG As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAADYAAAAZCAIAAAD13UppAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAIpJREFUWEfl0T0KwkAUReETZv9NClc1GxCLCIJEY340goVM9DnltHnCFb8FHC7cKsbIaiEEa6SUVpfKwKf88rBkHqWyYVme8njI4y6PWR43eVzl/cLESR6jPAZ59PK4yKOTx1keJ3m08jjK4yCPvby/mdhk3/jDstQeNplHqWxYlq2HXeZRKhuWfQNxD9FuOCD9GwAAAABJRU5ErkJggg==")

        Dim Hover_Left As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAsAAAAZCAIAAABo0EPhAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAS9JREFUOE99kzuLg1AQRu/+/97GSrAQBLEwNjYRES0kqCnFNBp8Iivrxse6uJ9rdpP4OuWd4525M+PbMAxkQdd177/cbjcC45mmaS6Xi23baZoi3Pf9i4FTxLIsQ+D7j4eRJMnpdJq+e+ZuxHFsWVbbtl8LRqOua8MwqqpCgUtG43w+h2G4GsYhQeLj8YgnIMUqxPd9PG8rjHOiaVqe53vG4XBAjXuGKIo74TGLIAgodofRmCa0BZEkCc343IaYpuk4zp5xvV55ni/LEi9ahaCvsiyj8ZsG5oKechyH8X6sMU4O16CzyFUUBdLNuO8HTlVVxU0oa9rQfx47BglDZhgGq4RJrRhIB8l1XfSQpmld1z3PQ3HzXUdNURShQ4qisCxLUdTcmP4MeKg6CAJc8wOLkfGPTRlxyQAAAABJRU5ErkJggg==")
        Dim Hover_Right As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAsAAAAZCAIAAABo0EPhAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAATFJREFUOE99kjuLg0AURmcbu8D+dlsrwUIQxMKkSaOEYAoJPioRbaL4CkHZrEl2XdzPTSJm1Tnl3DN37mPeTNNcLBbvfzAMQ8a0bVvXdZ7nlmWFYXi9XnEyhPw8aZqmKAp4sF8MBIYg336/z7Ksl8j3iNvthuLSNL1L5GuK8/lsGMblcpk1cCuKItd1OwM5J0FTmqahrFkD19B8EAQ043g8bjYbmoF6l8slzcBDiqIQ1EJBlmWaUZZlZ3zOg5GoqkozbNve7XYEBU9SVZUkSXEczxoY+Wq1wvjJxxRYrCiKmGm3F2T7x+l0Qn5MEwk6Ay0NwcO4res6rj3+Rx/GFvBxeJ7HSvvwI4fv+9vtluM4zMdxnGG4M1iWFQRhvV6j+yRJ7m+//GTP8w6HA6obx+7eL3XU5i5sD4RGAAAAAElFTkSuQmCC")
        Dim Hover_BG As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAADYAAAAZCAIAAAD13UppAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAIxJREFUWEft0TsKgDAQBNCR3L+x8FS5gHgBS8UoRA1+Yr9uQGEivnoYJpvCWgsdY8wRDCHo4gmpm+ZNbY3U8YSg3HxehRyuFzDDQg8zvRwmenqgX+j/iU/8UQ5XnOiBfuGEkV4OEx090C90GOihp4eO3rcmttEbR5ebUapVkTqeEJSbUas1kTqeEJSbdz1AWt41xKtlAAAAAElFTkSuQmCC")

        Dim Click_Left As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAsAAAAZCAIAAABo0EPhAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAZFJREFUOE+FkymvwlAQRu/7sYgGj0I0ILAIMBVNUYQg2ASQsJMQ9n1fnigCiUTC6RsCpYj3uWbOLHfm68/9fldfOp/Pg8Fgv99fr1cF4ZZt25qmBQIBv9+fTCZrtdoHUSwWg8GgYRjlcrlerzebzXa7/SZKpZKu69lslhiBTqfT6/Xo9SQIh0IhUskj1u/3R6PRdDqdz+cOwVzhcDifz5NK3ng8JrBarTabzXa7dQifz5dIJAiTOplMlsslMR5yOByOx6OiQCQSoTfZlCV1t9sRO51Ov39SjBaNRrvdLtkSJs8tFYvFqtXqcDhcLBZ0lWy3FAUgKLBer+ntCfOp4vE4BMNLAep7a7BBCOaH8Ewgn8o0TSHkCd+QsizrHyKdTr+6UONbih1nMpnZbCZ7lFW6pW63WyqV4lq8llEEcsu5CzVwBiuXU8G55RCUAcJOrBVIBCp6+uNyueRyuVarxUBch44vvT0GVCgUcBDeYT0v7sOnQJitUqkwOBynAPV6nZmARI1GA9RLyJ8Bh7M4OMZ7AMVcm8qafdstAAAAAElFTkSuQmCC")
        Dim Click_Right As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAsAAAAZCAIAAABo0EPhAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAZtJREFUOE+FkjmOwkAQRXuOwAFIuQFHIkDkRAQIAlICSAgQRMgiYAsAiR0JYbOYfWcCCDgC4czz1MgyZqT5mf1f/66uqo9wOOzxeHw+n9/v93q96l2dTqdQKIRCIdBgMHi73b5epYbDYb/f73a7zWYzk8lEIpF6ve5k1GQy0XV9NBqBwpXL5Wg02mg0bEitVqvFYjGbzQzDgCOPsFgsZkPqcDjs9/vtdgs6n8/JI6xarcbj8fv9TpK6XC7n8/l0OsFtNhvTNAkDyufzgUDAIj5/dL1e4Y7HI2FcShI1JRIJYqwMpwQiaTweJ5NJClecdookKluv19PptN1up1IpNwFNTbvdjhgIYtyEFE4MT4NIp9PuOqQmCN4FQZf/IORRQmSz2X+IXC6nOPEu+5ZisegmeIj0d7lclkol5qDkl1MUwWuZoqZpz+dT8e2UDJLGsyVkWHPhlwhDRENZPGwCLIJAW0yE6weDQaVSeTweskS/BB4NYI/YoFqtZtsWgUGD8Sit1WqxWk7bIjB6vR6GSO5+2WTWiTGyKe+ecN8ZWpFliPhZJgAAAABJRU5ErkJggg==")
        Dim Click_BG As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAADYAAAAZCAIAAAD13UppAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAN9JREFUWEfdl70SgjAQhDcFL8wDamG0ERt1HBV/HiRFZJIUVJcNE+T0q26G3eUOwsxh2rYFgfe+aZpB6JwzxhAOVpJPXqkHWw5r7SYwFJyDVWWTceDoum4fGArOwaqyyTgTnALHQKwJEyVhknGjuQZoeYFQTsaT4zGCc7CqbDLeIi8R2VsrGXEIuZVlr6YXvWwT8t3Zs5g9WfMNWa3F7AyTBeksTvZ/wZhaHH/52upfaLFXD9R32OOunoI1omAvqCpF3DI0g4t6/qvFuEbP8dDlZOzUgzVN/AOk5QVCOfkDog29Jih87R0AAAAASUVORK5CYII=")

        G.Clear(Color.Fuchsia)

        If State = MouseState.Down Then
            For i = 0 To RoundUp(Width / 54)
                G.DrawImage(Click_BG, i * 54, 0)
            Next
            G.DrawImage(Click_Left, 0, 0, 11, 25)
            G.DrawImage(Click_Right, Width - 11, 0, 11, 25)
        ElseIf State = MouseState.None Then
            For i = 0 To RoundUp(Width / 54)
                G.DrawImage(Normal_BG, i * 54, 0)
            Next
            G.DrawImage(Normal_Left, 0, 0, 11, 25)
            G.DrawImage(Normal_Right, Width - 11, 0, 11, 25)
        ElseIf State = MouseState.Over Then
            For i = 0 To RoundUp(Width / 54)
                G.DrawImage(Hover_BG, i * 54, 0)
            Next
            G.DrawImage(Hover_Left, 0, 0, 11, 25)
            G.DrawImage(Hover_Right, Width - 11, 0, 11, 25)
        End If

        If _ImgState = ImageState.Image Then
            Try
                G.DrawImage(Image, 13, 4, 16, 16)
            Catch ex As Exception
            End Try
        Else
            DrawTextNew(New SolidBrush(Color.FromArgb(51, 51, 51)), Text)
        End If
    End Sub
End Class
Class SkypeCallButton
    Inherits ThemeControl154
    Protected Sub DrawTextNew(ByVal b1 As Brush, ByVal text As String) ''Made by Aeonhack. Edited by me so it fits in the theme.
        If text.Length = 0 Then Return

        Dim DrawTextSize As Size = Measure(text)
        Dim DrawTextPoint As Point = New Point(Width \ 2 - DrawTextSize.Width \ 2, Height \ 2 - DrawTextSize.Height \ 2)


        G.DrawString(text, New Font("Arial", 10, FontStyle.Bold), b1, DrawTextPoint.X, DrawTextPoint.Y)

    End Sub
    Public Function RoundUp(ByVal d As Double) As Integer
        If d.ToString.Contains(",") Then
            Return CInt(d.ToString.Split(",")(0)) + 1
        Else
            Return CInt(d)
        End If
    End Function
    Private Function CodeToImage(ByVal Code As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(Code)))
    End Function
    Sub New()
        LockHeight = 25
        Font = New Font("Arial", 10, FontStyle.Bold)
    End Sub
    Protected Overrides Sub ColorHook()
    End Sub
    Protected Overrides Sub PaintHook()
        Dim Normal_Left As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAABYAAAAZCAIAAAC6gEm5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAmJJREFUOE+VlE9IYlEYxa/9oQINAhGiVkGbCqpdVtIuopZFERFRULSICiOyIty0qKRVEJROViMkYzDPIorsUShm/58yDDKzSXDhKpBaCG3sPD55mZb2Hj/k3vu+c75zr3oV8XicpT2hlxAX4kDgKRB9jaYXfFiBRfLz+Pw4cD5QaCls5Br7+L4p39Tiw2JmWLLe+s+q2dHonDq9Tw/ZamB17c/a+t91gAGmWJy9nk3h3QL6YmtxL987czWzJCyZ/KYV/8qysIwxwABTLGJsuDaMe8clEhbQK7eUlNx4a1y4XZi/mf8UvDLeGRFz6GKIEC2wf/W2uvWoddQ9CotJ7+SEdyIDKNBf6lHc7eoGokX/WX+lvbLT1TnsHobr4MVgVlA24h6BpP24nSFCnjmvydnUc9YDy67Trm+CYkhaDlqY6d6k2dU0O5s7jjuwF1lAAiHT7mvLbGUNXAMmWk4rC92BDkKmsqhKf5bW7tfWOGqqHdVZqfpVJQEJhIxtsJLtkoq9CmSRS7mtHELRIt+cr95R43chF9WWCkKmNCvhUvSjCEa5m7lZydnMkYAEQlZvq6cg+GoxkAVUgE2fTEOG5gWWAll6xYYCEgiZ77+PlIn5t4MgdaJrLBZr22uDBXaIJXgn81UuFONOwadYgP8I7+fpULExuCRefB2HmomnQDWwQJC5wzmaU7wMR4tX6P+hgG6tSCRicBrIhZpQHykRBpTxk5jSxUcutCP6jkiDnoAcxfNP32Dy3QkXB++g05VByg2OcwkGg3aXfez3WN1unRQqk2OKBU1hFA6HBUHweDzn2Z437xmYkxCYqCUAAAAASUVORK5CYII=")
        Dim Normal_Right As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAwAAAAZCAIAAACKDFiYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAThJREFUOE9jZJjJIMAmoC+sH6AQAEQKvAoMmKDlXEvx8eLovdFWG6w45nDE74+//+n+f1TAUHmyEo6KjhfZbrQVWyg2/+Z8ZGUMeUfz0FDk3ki++XzI6hiSDiRhIqDtPPN44OoYwnaHYUVuW91EFohA3Mfgtd0LF1JdoRq7JxakyH6TPS5kvdGaZTYL0DAG47XGeJDYIrHus90MGis18CDpJdKWaywZZJfK4kGSiyV55/AyCC4QxI+A8cbAOpsVPwIpAmG8iGc2D2FFhksMCSsq3VFKWNHxW8cJKPJY7vH9+3d8ioBO3nthLyju8HitanMV0Bh8iio2Vjx//hySPrGYBLQFWQUWRUCXrt67Gm4GwiSgVoNFBjnrc1bsXnH9+nWIO1Aywv79+w8fPnz+/PnHjx9jSkOUAgBV+nww3EO/fQAAAABJRU5ErkJggg==")
        Dim Normal_BG As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAADYAAAAZCAIAAAD13UppAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAH9JREFUWEdjZJjJMNhBy7mWQY4YKk9WDnLEkHc0b5AjhqQDSYMcMYTtDhvkiMFru9cgRwz2m+wHOWIwXms8yBGDxkqNQY4YZJfKDnLEILhAcJAjBtbZrIMcMYCaEYMcDXb3gRpigzwIR51IpSgajWhqBORoKI6G4mBJA/sHPQAAhnkCIsAEp70AAAAASUVORK5CYII=")

        Dim Hover_Left As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAABYAAAAZCAIAAAC6gEm5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAqNJREFUOE+VlMlLW1EUxq/zhBFUSFwI/gVudOVC3KoIRtCNMWZjjHFlgrpIHBBUUIPiBJmamEcUbCuWUOOAsY1prWNMHaO2yVawIO0q3djvceU2RknM5Uc493LOd753Xt5NeHh4IM9W8E9wObgMfL9893/vnyc8OYFE+Ar8DjRvNWeZs6qd1aqvqjHfmPnSHB0SXm/xWwq4grr1uvHv49ZL68LNwuKPxaWfSwDB/M285dIyezYbwX8J1OfP5Xd965o5neGuOPu1nbvmENiubAABtvzhFTd9Nq3z6RiPEqjPs+b1HfRNnkyaLkyGc4P+XP8ixgsjEpA2eDRI4SXw/MI5oWJbMeobnTqdmjiZiAnSRo5HtHtawEtIN6XlH8q1+1qcDnuHozN0NERBsmZPo95RE1hIN6XLP8t793sBFY4Cyig9+z3Ib/e0E92xrvhtMaLu3W7VjiomHTsdjM7dTqVHScrelVU4KlrdrcptpcKtCAeHLyJ3yylt220t7hYieCOodFbKPsmkLmmTq+lFJJsSRuNmI0Piksi2ZIToSZWzqmGjQbwmjova1Vrxqrh+vZ6XyLXmFs0XiTjRaxDahAyRTVRoLyTZpuxUY2qOJQffRYYpIyZ4fWmmNEqmOVNgEZDShVIYSTGmJBuTEcQFqgDRuDQoSzIkQTWu+gR9AkpQSA6Dh7Tycf9qI3D92DUUCtW8r4FEoiERR9BmRDGFZAwFv3wOvhHPhQdDpRNho0KADEqEFm3GT4FahgSMDGwM0D3sMZhc+CFi9H8ye3pr3d3d9a/1UxXahPZhFhAwj5G+2MVHVegT0XdEa9CT/hew5ef/fN7hdydUVnZX6HTjIOIGx1wCgYDji0P9UV1iL2GmoilGSNAthG5vb/1+v9frPYi1/gEakOtUbEwcoQAAAABJRU5ErkJggg==")
        Dim Hover_Right As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAwAAAAZCAIAAACKDFiYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAASlJREFUOE9jZJjJIMAmoC+sH6AQAEQKvAoMmGDujbkQZLXBimMOR/z++Puf7v9HBQxTr05FRrYbbcUWis2/OR9ZGUPvxV5MxDefD1kdQ+u5VqyIZx4PXB1DzakaXEhkgQjEfQzFx4txIdUVqrF7YkGKso9k40Ess1mAhjEkHUzCg8QWiXWf7WaI3BuJH1musWQI3BmIH/HO4WXw2OqBHwHjjYF/Pj8Esc5mxYpAiuASIA42xDObB7sEsmrDJYaEFZXuKCWs6Pit4wQUeSz3+P79Oz5FQCfvvbAXFHe4PAUUr9pcBTQGn6KKjRXPnz+HpE8sJgFtQVaBRRHQpav3roabgTAJqNVgkUHO+pwVu1dcv34d4g6UjHDmzJnDhw+fP3/+8ePHmNIQpQAAmaPsN2aELAAAAABJRU5ErkJggg==")
        Dim Hover_BG As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAADYAAAAZCAIAAAD13UppAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAIFJREFUWEft1iEKgEAUhOExbPZa1o3WrUar1WjcICKewmAwCIIgIhgEjyPeYcMP7uM7wDBTXqJW9OufHk7+9nBqrgZO9VnDqdorOJVbCadiLeDkFgenfM7hZCcLp2zM4JQOKZxMZ+D0fTpw9HzfrwivMEYMNFEcOkSRscWftHjg7wWoQvM8R5DyyQAAAABJRU5ErkJggg==")

        Dim Click_Left As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAABYAAAAZCAIAAAC6gEm5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAA2VJREFUOE+NlF0sW2EYgN/q2SGcSDYTysiWzFQkZqQhfuIC1ZslbGEL0SiJLK0RpaddWWa6NmwI5meqNTtd43czuvqrvyqziyUuxdV+XLjaLTdL7D07QquGL8/FOd857/O97/ud8/EODg7Aa/zc+Wn/andsObZ/b+/92fPh+Xi/czyDCvfx49cPebs8qioq9Vnq3da7hT2Fsn5ZmbnsDMA9nrExscrY9Mb0hz0P5Ra5clSpmdDUTdXVT9XX2/7LscJit0Qpo3JacmSDssejj1WTqob5huaV5s71zs4vLM3O5oaFBnqGrrZXu3OoYD4zN6pviNvEUkZaPlqunlEbVg36Vb3OpWt0NXLo1nT6Nb1h3UAv0eXT5Uewiu+/vt9U3kxpSskfyC8ZKamdr32y/ES9rKadNL1K0y4P1C61dl1b46wpni0umilCWEVZe5nwqVDcK75vvS+fllc6KisWKxTLCsWKQuFUyJ1ybypXKx+tPMqbycudzgVM4WrV1YSXCdnm7AfjD0pnS6VzUg8WpFJPipeKpUtS2YqswFEgtouhbaQtXBue2JGYMZhRMFVw7/M9dJ9N7lwukjefl+/IT59Mh4zGjIjnEXFdcWnv0yQTkqzJrCzbOWTaMjly7Dkpn1IguDY4zBB2q/tWsjU5aSxJNC4SfTiPjyLRP5InkpMmkoCsIYObgq91X7ttvR0zFCMcFp5kRCj0JHo4miNmJCZuLA4IFUEZqMuvL18fvC54JxAwXlgEAk9CmdAQJgTBi0hrJATWBRJ6gmwng0xBlIkKMAUEmM/B3+zPQQ1QVwavwB39HXgB0AZkL8nv40MfXByinyD7SVANqDgFv5tPGsmLx+ObGM838mHj2wboAFqB18Xz6/fjveFd0OJj9PEz+fH6eLC/vy/pkMArgE4g+ghMBC3u/M/oa/LFQtin+I8suhapFgo6AHoAFZeMl9zxVuDKWALbBa5xqMBEtIwWa4EugF7WgmCep66P87j+cTynwLG7u6uxaKAdoJu1EEYC+4IiTBXXRDCS6z/Wf5j/0d4dHXys5b2G6qI4C7YD9xgtGMPB9Z/t34mNdz870TJmH5O8lbAV9bAieOPJqV/NiRMc+7K1tTVsG64Yqog3x1O9/5I6mxMK7hZFOzs7m5ubLpdr+bzxF18PkwwsoAUxAAAAAElFTkSuQmCC")
        Dim Click_Right As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAwAAAAZCAIAAACKDFiYAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAjlJREFUOE+Fk89P2mAYgN/V6YEAJszEpsmC0qAZh4o36o+DgoTE2MSLCTUxMVAPxNBtBsPYErMNtywOAgr+GIKuqCuijAHdzXDg4GUJR44aOfAncNwK3VgJM/vy3N4nz/cmX/tA69IqHiqGHw3PPJmZNc1qH2uh8yzHluldei4wN74xrmf1rpDr9u72Z/sB3zffemad5dmVzysL0YWJNxOG5wYuz8k1YPMsm2PZLOvOuF0p11JiybplxZ/hSSHZ8oARGKbAMDnGmXU6L50O3rF4vGgJWAaeDnCF3z2gv9O0QNMFms7R9q92e9puP7PPH86b3pvE3s3djdgDSqAa5JtkKeqConiK4qjpyPTQqyFHyNGQrIJVjiVjMafM5qR5KjY18mFE49aIMRjLjskhL0nynCRPSTJOGkNG7AUWTAXBeGGUQ5wTBE8QJwQRJwzbBmwDm3w9CXpeLwc/w/FTXMfpdIe6wZ3Bfn9/31ofYElMDsqhDY5RNIaiO6jmnaZnrQd6j3rlqBPqBnG1el+tCqsUm4ouTxd0x7rlIJ8Q5KBJFEGCCOJHVC9VAAf/Yh8gAhAE8MPo5uj/JU/Cc09GKgUA3sL1j+sOSRyL7AFsA2yBLWyr1+v3SLsAYVB+VF6Vrhpv17Z4K9O8y8f5xIxMksbSRVGAEHiT3lqtJn13zVK7oYwovSd/jabUCoh7RMB2ZEsL6VbjTykKyj2lMW5c/bLK5/lKpSLt0fYjFIvFUqlULper1WrnWFJ/AfiRdaa+FFYwAAAAAElFTkSuQmCC")
        Dim Click_BG As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAADYAAAAZCAIAAAD13UppAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAINJREFUWEft1CEOg1AQRdFbgcbjEHg0C0DjkSQYFJ5tVBDSVNRUIEgIINlad4BC3CZ/chbw8mYyj7RLkU/zauQY1kGOfuvlaPdWjvqo5aj2So5yK+UolkKOfM7lyL6ZHMknkSN+x3JEUyQHo16IeMeKQouhRcsNWHJc/L5/iPgEuVM/P1yHAgCiKbOGAAAAAElFTkSuQmCC")
        G.Clear(Color.Fuchsia)

        If State = MouseState.Down Then
            For i = 0 To RoundUp(Width / 54)
                G.DrawImage(Click_BG, i * 54, 0)
            Next
            G.DrawImage(Click_Left, 0, 0, 22, 25)
            G.DrawImage(Click_Right, Width - 11, 0, 11, 25)
        ElseIf State = MouseState.None Then
            For i = 0 To RoundUp(Width / 54)
                G.DrawImage(Normal_BG, i * 54, 0)
            Next
            G.DrawImage(Normal_Left, 0, 0, 22, 25)
            G.DrawImage(Normal_Right, Width - 12, 0, 12, 25)
        ElseIf State = MouseState.Over Then
            For i = 0 To RoundUp(Width / 54)
                G.DrawImage(Hover_BG, i * 54, 0)
            Next
            G.DrawImage(Hover_Left, 0, 0, 22, 25)
            G.DrawImage(Hover_Right, Width - 11, 0, 11, 25)
        End If
        G.SmoothingMode = SmoothingMode.AntiAlias
        G.DrawString(Text, New Font("Arial", 10, FontStyle.Bold), New SolidBrush(Color.White), 25, 4)
        Try
            G.DrawImage(Image, 7, 7, 12, 12)
        Catch ex As Exception

        End Try
    End Sub
End Class
Class SkypeTheme
    Inherits ThemeContainer154

    Sub New()
        TransparencyKey = Color.Fuchsia

    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Private RT1 As Rectangle

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(148, 195, 255))
        G.DrawRectangle(New Pen(Color.FromArgb(105, 142, 191)), 0, 0, Width - 1, Height - 1)

        DrawGradient(Color.FromArgb(241, 247, 255), Color.FromArgb(148, 195, 255), 1, 1, Width - 2, 25)
        DrawGradient(Color.FromArgb(211, 230, 255), Color.FromArgb(148, 195, 255), 2, 2, Width - 4, 25)

        G.DrawString(Text, New Font("Arial", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(51, 51, 51)), 5, 3)
    End Sub
End Class
Class SkypeGroupbox : Inherits ContainerControl
    Private Function CodeToImage(ByVal Code As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(Code)))
    End Function
    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.White
        DoubleBuffered = True
    End Sub
    Public Function RoundUp(ByVal d As Double) As Integer
        If d.ToString.Contains(",") Then
            Return CInt(d.ToString.Split(",")(0)) + 1
        Else
            Return CInt(d)
        End If
    End Function
    Private _BottomBar As Boolean = False
    Property BottomBar() As Boolean
        Get
            Return _BottomBar
        End Get
        Set(ByVal value As Boolean)
            _BottomBar = value
            Invalidate()
        End Set
    End Property
    Private _CaptionImage As Image
    Property CaptionImage() As Image
        Get
            Return _CaptionImage
        End Get
        Set(ByVal value As Image)
            _CaptionImage = value
            Invalidate()
        End Set
    End Property
    Private _Caption As Boolean = False
    Property CaptionText() As String
        Get
            Return _CaptionText
        End Get
        Set(ByVal value As String)
            _CaptionText = value
            Invalidate()
        End Set
    End Property
    Private _CaptionText As String = ""
    Property Caption() As Boolean
        Get
            Return _Caption
        End Get
        Set(ByVal value As Boolean)
            _Caption = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim Corner_Left_Top As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAAmkwkpAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAADxJREFUGFdj7Nn3/8/vn98+vf35/QtDx84fFYtvT9z84Oqjzwx1q58CWd9//v3//z9D+aKbVx59BrKAAAAObiiiSPYygwAAAABJRU5ErkJggg==")
        Dim Corner_Right_Top As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAAmkwkpAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAADlJREFUGFdjLF90k52Th4tPmIWVneHqo88TNz+oWHy7Y+cPhv///3//+RfIr1v9FMQBgiuPPgPVAwCozR/5eeWQ5wAAAABJRU5ErkJggg==")
        Dim Corner_Left_Bot As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAAmkwkpAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAACJJREFUGFdjKF908z8MMNStforgdOz8geD07PsP5APlgeoBWv0poRMgjd8AAAAASUVORK5CYII=")
        Dim Corner_Right_Bot As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAAmkwkpAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAClJREFUGFdj/P//PwMYVCy+xQDkQEDd6qcITsfOHwzli24CxYCsnn3/AWDMI6v3pdX3AAAAAElFTkSuQmCC")

        Dim Corner_Left_Bot_New As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAAmkwkpAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAADVJREFUGFdjLF90M9hCXJiXlQEIqpc/mLD5wd3nX4GIoX37N6AkkH/q1geGnn3/gXygPFAIAJkXHaCX6UBVAAAAAElFTkSuQmCC")
        Dim Corner_Right_Bot_New As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAQAAAAECAIAAAAmkwkpAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAADdJREFUGFdjvPv8KwMDw9vPv9eeeMkA5ADRhM0Pqpc/YDh16wOQVb7oZvv2bwxACigGZPXs+w8AgtEi2UaAmzcAAAAASUVORK5CYII=")

        Dim OnePxImg As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAADIAAAATCAIAAABdrcl1AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAEJJREFUSEtjvPv8K8MgBEBnDULEMAjdBIrAUWeRkFpGQ4uUvDUaWqOhRULmIqkkGk1bo2lrNG2RlGVopHg0J5KSEwFcyJWmO09UYwAAAABJRU5ErkJggg==")

        Dim CaptionCorner_Left As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAMAAAADCAIAAADZSiLoAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAACdJREFUGFdjXHrm/68f3z69f80w78jXnnU3Vx56zDBh8wMg9evXLwBQYRY9ipWOggAAAABJRU5ErkJggg==")
        Dim CaptionCorner_Right As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAAMAAAADCAIAAADZSiLoAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAACdJREFUGFdjnLj5AZ+gKBsHF8PKQ4971t2cd+Qrw69fv4CcCZsfAADNdxCJDpD+PwAAAABJRU5ErkJggg==")

        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Body As New Rectangle(4, 25, Width - 9, Height - 30)
        Dim Body2 As New Rectangle(0, 0, Width - 1, Height - 1)

        MyBase.OnPaint(e)

        G.Clear(Color.Transparent)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality

        G.DrawImage(Corner_Left_Top, 0, 0, 4, 4)
        G.DrawImage(Corner_Right_Top, Width - 4, 0, 4, 4)

        If _Caption = True Then
            Dim DrawGradientBrush = New LinearGradientBrush(New Rectangle(0, 2, Width - 1, 29), Color.FromArgb(241, 241, 241), Color.FromArgb(218, 218, 218), 90.0F)
            G.FillRectangle(DrawGradientBrush, New Rectangle(0, 2, Width - 1, 29))
            G.DrawLine(Pens.White, 1, 1, Width - 1, 1)
            G.DrawImage(CaptionCorner_Left, 0, 0, 3, 3)
            G.DrawImage(CaptionCorner_Right, Width - 3, 0, 3, 3)
            G.DrawLine(New Pen(Color.FromArgb(203, 203, 203)), 0, 31, Width - 1, 31)
            Try
                G.DrawImage(_CaptionImage, 9, 9, 16, 16)
                G.DrawString(_CaptionText, New Font("Arial", 10, FontStyle.Bold), New SolidBrush(Color.FromArgb(51, 51, 51)), 30, 8)
            Catch ex As Exception

            End Try
        End If

        If _BottomBar = True Then
            For i = 0 To RoundUp(Width / 50)
                G.DrawImage(OnePxImg, i * 50, Height - 20)
            Next
            G.DrawLine(New Pen(Color.FromArgb(217, 217, 217)), 1, Height - 21, Width - 2, Height - 21)
            G.DrawRectangle(New Pen(Color.FromArgb(119, 162, 217)), 0, 0, Width - 1, Height - 1)
            G.DrawImage(Corner_Left_Bot_New, 0, Height - 4, 4, 4)
            G.DrawImage(Corner_Right_Bot_New, Width - 4, Height - 4, 4, 4)
            G.DrawString(Text, New Font("Arial", 8), New SolidBrush(Color.FromArgb(153, 153, 153)), 4, Height - 17)
        Else
            G.DrawImage(Corner_Left_Bot, 0, Height - 4, 4, 4)
            G.DrawImage(Corner_Right_Bot, Width - 4, Height - 4, 4, 4)
            G.DrawRectangle(New Pen(Color.FromArgb(119, 162, 217)), 0, 0, Width - 1, Height - 1)
        End If

        B.SetPixel(0, Height - 1, Color.FromArgb(148, 195, 255))
        B.SetPixel(Width - 1, Height - 1, Color.FromArgb(148, 195, 255))
        B.SetPixel(Width - 1, 0, Color.FromArgb(148, 195, 255))
        B.SetPixel(0, 0, Color.FromArgb(148, 195, 255))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
<DefaultEvent("TextChanged")> _
Class SkypeTextbox
    Inherits ThemeControl154

    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If Base IsNot Nothing Then
                Base.TextAlign = value
            End If
        End Set
    End Property
    Private _MaxLength As Integer = 32767
    Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If Base IsNot Nothing Then
                Base.MaxLength = value
            End If
        End Set
    End Property
    Private _ReadOnly As Boolean
    Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If Base IsNot Nothing Then
                Base.ReadOnly = value
            End If
        End Set
    End Property
    Private _UseSystemPasswordChar As Boolean
    Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If Base IsNot Nothing Then
                Base.UseSystemPasswordChar = value
            End If
        End Set
    End Property
    Private _Multiline As Boolean
    Property Multiline() As Boolean
        Get
            Return _Multiline
        End Get
        Set(ByVal value As Boolean)
            _Multiline = value
            If Base IsNot Nothing Then
                Base.Multiline = value

                If value Then
                    LockHeight = 0
                    Base.Height = Height - 11
                Else
                    LockHeight = Base.Height + 11
                End If
            End If
        End Set
    End Property
    Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If Base IsNot Nothing Then
                Base.Text = value
            End If
        End Set
    End Property
    Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If Base IsNot Nothing Then
                Base.Font = value
                Base.Location = New Point(3, 5)
                Base.Width = Width - 6

                If Not _Multiline Then
                    LockHeight = Base.Height + 11
                End If
            End If
        End Set
    End Property

    Protected Overrides Sub OnCreation()
        If Not Controls.Contains(Base) Then
            Controls.Add(Base)
        End If
    End Sub

    Private Base As TextBox
    Sub New()
        Base = New TextBox

        Base.Font = Font
        Base.Text = Text
        Base.MaxLength = _MaxLength
        Base.Multiline = _Multiline
        Base.ReadOnly = _ReadOnly
        Base.UseSystemPasswordChar = _UseSystemPasswordChar

        Base.BorderStyle = BorderStyle.None

        Base.Location = New Point(4, 4)
        Base.Width = Width - 10

        If _Multiline Then
            Base.Height = Height - 11
        Else
            LockHeight = Base.Height + 11
        End If

        AddHandler Base.TextChanged, AddressOf OnBaseTextChanged
        AddHandler Base.KeyDown, AddressOf OnBaseKeyDown
    End Sub


    Protected Overrides Sub ColorHook()
        Base.ForeColor = Color.Gray
        Base.BackColor = Color.White
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.White)
        DrawBorders(New Pen(Color.FromArgb(194, 194, 194)))
        G.DrawLine(New Pen(Color.FromArgb(243, 243, 243)), 1, 1, Width - 2, 1)
        G.DrawLine(New Pen(Color.FromArgb(243, 243, 243)), 1, 2, Width - 2, 2)
        G.DrawLine(New Pen(Color.FromArgb(245, 245, 245)), 1, 3, Width - 2, 3)
        G.DrawLine(New Pen(Color.FromArgb(249, 249, 249)), 1, 4, Width - 2, 4)
        G.DrawLine(New Pen(Color.FromArgb(252, 252, 252)), 1, 5, Width - 2, 5)
    End Sub
    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
        Text = Base.Text
    End Sub
    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            Base.SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        Base.Location = New Point(4, 5)
        Base.Width = Width - 8

        If _Multiline Then
            Base.Height = Height - 5
        End If


        MyBase.OnResize(e)
    End Sub

End Class
<DefaultEvent("CheckedChanged")> _
Class SkypeCheckbox
    Inherits ThemeControl154
    Private Function CodeToImage(ByVal Code As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(Code)))
    End Function
    Sub New()
        LockHeight = 14
        LockWidth = 14
    End Sub

    Protected Overrides Sub ColorHook()
      
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        Invalidate()
    End Sub

    Protected Overrides Sub PaintHook()
        Dim ImgChecked As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAIAAACQKrqGAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAASlJREFUKFNj+E8I8DatVVNTkwpKYwCqZFhwm2HqGayIqfegop6xop4RSBlQXen+239wgOzsbAUFBZ6CqWClU8/8/v37MTawZMkSGRmZ6OhooBqo0p8/fz7EAJcvX9bW1lZUVLx//z5C6bdv33bv3i0lJeXh4XH37l2gHBDExsYKCwt3d3cDZRFKP336dOfOncmTJ/Px8WVkZADZc+fOBbIdHR2BbKAsQum7d+9ugUF6ejoXF1d1dTXQK4KCgnv37gUKAmURSl+9enUDDK5du2ZlZcUGBlVVVRBBoCxC6ZMnT67CwJEjR6SlpfX19YHegogBZRFKgZ4ASsDBypUrN2zYAOeihMDt27cv4gZAWZipC24nrz0JdCUuAJQFxigoCvCnAVDCAKsDAgBvkZJ5r5lP9QAAAABJRU5ErkJggg==")
        Dim ImgNoChecked As Image = CodeToImage("iVBORw0KGgoAAAANSUhEUgAAAA4AAAAOCAIAAACQKrqGAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAI9JREFUKFONkjEKhDAQRd2z7hk8in0usGewFmyWxUUcQWRTpUiRIhCyEL+iTPXBR7r3CAwzj1JKdZM9NVI1PX1G0Bw/Gqlb+XNg0Rxp0+ecfxxYNGeaUlo5sJrGGBcOrKYhhJkDq6n3fuLAauqcGzmwmlprBw6sphjpw4HVVETeHNgrNfJ8dV8O7Lmt+zewAahirSfxCAkLAAAAAElFTkSuQmCC")

        G.Clear(Color.FromArgb(51, 51, 51))
        If _Checked Then
            G.DrawImage(ImgChecked, 0, 0, 14, 14)
        Else
            G.DrawImage(ImgNoChecked, 0, 0, 14, 14)
        End If

    End Sub

    Private _Checked As Boolean
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnMouseDown(e)
    End Sub

    Event CheckedChanged(ByVal sender As Object)

End Class
#End Region