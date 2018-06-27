#Region "Themebase"
Imports System, System.IO, System.Collections.Generic
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging
Imports System.Drawing.Text

'------------------
'Creator: aeonhack
'Site: elitevs.net
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

Module ThemeModule

    Friend G As Graphics

    Sub New()
        TextBitmap = New Bitmap(1, 1)
        TextGraphics = Graphics.FromImage(TextBitmap)
    End Sub

    Private TextBitmap As Bitmap
    Private TextGraphics As Graphics

    Friend Function MeasureString(ByVal text As String, ByVal font As Font) As SizeF
        Return TextGraphics.MeasureString(text, font)
    End Function

    Friend Function MeasureString(ByVal text As String, ByVal font As Font, ByVal width As Integer) As SizeF
        Return TextGraphics.MeasureString(text, font, width, StringFormat.GenericTypographic)
    End Function

    Private CreateRoundPath As GraphicsPath
    Private CreateRoundRectangle As Rectangle

    Friend Function CreateRound(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal slope As Integer) As GraphicsPath
        CreateRoundRectangle = New Rectangle(x, y, width, height)
        Return CreateRound(CreateRoundRectangle, slope)
    End Function

    Friend Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

End Module
#End Region

'/------------------------------------------------------------------------------------------------------------------\
'| 88888888888                                  888888888888 88                                                     |
'| 88                                         ,d     88      88                                                     |
'| 88                                         88     88      88                                                     |
'| 88aaaaa 8b,dPPYba,  ,adPPYba,  ,adPPYba, MM88MMM  88      88,dPPYba,   ,adPPYba, 88,dPYba,,adPYba,   ,adPPYba,   |
'| 88""""" 88P'   "Y8 a8"     "8a I8[    ""   88     88      88P'    "8a a8P_____88 88P'   "88"    "8a a8P_____88   |
'| 88      88         8b       d8  `"Y8ba,    88     88      88       88 8PP""""""" 88      88      88 8PP"""""""   |
'| 88      88         "8a,   ,a8" aa    ]8I   88,    88      88       88 "8b,   ,aa 88      88      88 "8b,   ,aa   |
'| 88      88          `"YbbdP"'  `"YbbdP"'   "Y888  88      88       88  `"Ybbd8"' 88      88      88  `"Ybbd8"'   |
'|                                                                                                                  |
'|                                                                                                                  |
'| Creator: Hoody                                                                                                   |
'| Created: 2/25/2014                                                                                               |
'| Changed: 3/16/2014                                                                                               |
'| Version: 1.2.1                                                                                                   |
'\------------------------------------------------------------------------------------------------------------------/
Class FrostButton
    Inherits ThemeControl154

    Private _FontColor As Color = Color.FromArgb(132, 112, 255)
    Public Property FontColor() As Color
        Get
            Return _FontColor
        End Get
        Set(ByVal value As Color)
            _FontColor = value
            Invalidate()
        End Set
    End Property

#Region "None Colors"

    Private _ColorNone1 As Color = Color.FromArgb(47, 48, 52)
    Public Property ColorNone1() As Color
        Get
            Return _ColorNone1
        End Get
        Set(ByVal value As Color)
            _ColorNone1 = value
            Invalidate()
        End Set
    End Property

    Private _ColorNone2 As Color = Color.FromArgb(29, 29, 29)
    Public Property ColorNone2() As Color
        Get
            Return _ColorNone2
        End Get
        Set(ByVal value As Color)
            _ColorNone2 = value
            Invalidate()
        End Set
    End Property

    Private _ColorNoneOutline As Color = Color.FromArgb(70, 70, 70)
    Public Property ColorNoneOutline() As Color
        Get
            Return _ColorNoneOutline
        End Get
        Set(ByVal value As Color)
            _ColorNoneOutline = value
            Invalidate()
        End Set
    End Property

#End Region

#Region "Over Colors"

    Private _ColorOver1 As Color = Color.FromArgb(67, 68, 72)
    Public Property ColorOver1() As Color
        Get
            Return _ColorOver1
        End Get
        Set(ByVal value As Color)
            _ColorOver1 = value
            Invalidate()
        End Set
    End Property

    Private _ColorOver2 As Color = Color.FromArgb(49, 49, 49)
    Public Property ColorOver2() As Color
        Get
            Return _ColorOver2
        End Get
        Set(ByVal value As Color)
            _ColorOver2 = value
            Invalidate()
        End Set
    End Property

    Private _ColorOverOutline As Color = Color.FromArgb(100, 100, 100)
    Public Property ColorOverOutline() As Color
        Get
            Return _ColorOverOutline
        End Get
        Set(ByVal value As Color)
            _ColorOverOutline = value
            Invalidate()
        End Set
    End Property

#End Region

#Region "Down Colors"

    Private _ColorDown1 As Color = Color.FromArgb(27, 28, 32)
    Public Property ColorDown1() As Color
        Get
            Return _ColorDown1
        End Get
        Set(ByVal value As Color)
            _ColorDown1 = value
            Invalidate()
        End Set
    End Property

    Private _ColorDown2 As Color = Color.FromArgb(9, 9, 9)
    Public Property ColorDown2() As Color
        Get
            Return _ColorDown2
        End Get
        Set(ByVal value As Color)
            _ColorDown2 = value
            Invalidate()
        End Set
    End Property

    Private _ColorDownOutline As Color = Color.FromArgb(40, 40, 40)
    Public Property ColorDownOutline() As Color
        Get
            Return _ColorDownOutline
        End Get
        Set(ByVal value As Color)
            _ColorDownOutline = value
            Invalidate()
        End Set
    End Property

#End Region


    Sub New()
        BackColor = Color.FromArgb(0, 0, 0)
        Font = New Font("Segoe UI", 10)
    End Sub
    Dim TextBrush As Brush
    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Select Case State
            Case MouseState.None
                G.FillRectangle(New SolidBrush(_ColorNone1), New Rectangle(0, 0, Width, Height))
                G.FillRectangle(New SolidBrush(_ColorNone2), New Rectangle(0, Height / 2, Width, Height))
                G.DrawRectangle(New Pen(_ColorNoneOutline), New Rectangle(1, 1, Width - 3, Height - 3))
            Case MouseState.Over
                G.FillRectangle(New SolidBrush(_ColorOver1), New Rectangle(0, 0, Width, Height))
                G.FillRectangle(New SolidBrush(_ColorOver2), New Rectangle(0, Height / 2, Width, Height))
                G.DrawRectangle(New Pen(_ColorOverOutline), New Rectangle(1, 1, Width - 3, Height - 3))
            Case MouseState.Down
                G.FillRectangle(New SolidBrush(_ColorDown1), New Rectangle(0, 0, Width, Height))
                G.FillRectangle(New SolidBrush(_ColorDown2), New Rectangle(0, Height / 2, Width, Height))
                G.DrawRectangle(New Pen(_ColorDownOutline), New Rectangle(1, 1, Width - 3, Height - 3))
        End Select
        DrawText(New SolidBrush(FontColor), HorizontalAlignment.Center, 0, 0)
        G.DrawRectangle(New Pen(BackColor), New Rectangle(0, 0, Width - 1, Height - 1))
    End Sub
End Class
<DefaultEvent("CheckedChanged")> _
Class FrostCheckBox
    Inherits ThemeControl154


    Private _FontColor As Color = Color.FromArgb(132, 112, 255)
    Public Property FontColor() As Color
        Get
            Return _FontColor
        End Get
        Set(ByVal value As Color)
            _FontColor = value
            Invalidate()
        End Set
    End Property

    Private _Color1 As Color = Color.FromArgb(70, 70, 70)
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
            Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(132, 112, 255)
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
            Invalidate()
        End Set
    End Property

    Private _Color3 As Color = Color.FromArgb(122, 102, 245)
    Public Property Color3() As Color
        Get
            Return _Color3
        End Get
        Set(ByVal value As Color)
            _Color3 = value
            Invalidate()
        End Set
    End Property

    Private _Color4 As Color = Color.FromArgb(30, 30, 30)
    Public Property Color4() As Color
        Get
            Return _Color4
        End Get
        Set(ByVal value As Color)
            _Color4 = value
            Invalidate()
        End Set
    End Property

    Private _Color5 As Color = Color.FromArgb(25, 25, 25)
    Public Property Color5() As Color
        Get
            Return _Color5
        End Get
        Set(ByVal value As Color)
            _Color5 = value
            Invalidate()
        End Set
    End Property

    Event CheckedChanged(ByVal sender As Object)

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)

        BackColor = Color.FromArgb(21, 21, 21)
        Font = New Font("Segoe UI", 10)

    End Sub

    Private _Checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value

            If _Checked Then
                InvalidateParent()
            End If

            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    Private Sub InvalidateParent()
        If Parent Is Nothing Then Return
    End Sub


    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If Checked Then
            Checked = False
        Else
            Checked = True
        End If
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        G.Clear(BackColor)
        G.DrawRectangle(New Pen(_Color1), New Rectangle(1, 1, Height - 3, Height - 3))
        If _Checked Then
            G.FillRectangle(New SolidBrush(_Color2), New Rectangle(3, 3, Height - 6, Height / 2))
            G.FillRectangle(New SolidBrush(_Color3), New Rectangle(3, Height / 2, Height - 6, Height / 2 - 4))
        Else
            G.FillRectangle(New SolidBrush(_Color4), New Rectangle(3, 3, Height - 6, Height / 2))
            G.FillRectangle(New SolidBrush(_Color5), New Rectangle(3, Height / 2, Height - 6, Height / 2 - 4))
        End If
        DrawText(New SolidBrush(_FontColor), Height + 5, Height / 2 - Me.Font.SizeInPoints)
    End Sub
End Class
Class FrostCloseWindow
    Inherits ThemeControl154

#Region "Color None"

    Private _ColorNone1 As Color = Color.FromArgb(40, 40, 40)
    Public Property ColorNone1() As Color
        Get
            Return _ColorNone1
        End Get
        Set(ByVal value As Color)
            _ColorNone1 = value
            Invalidate()
        End Set
    End Property

    Private _ColorNone2 As Color = Color.FromArgb(35, 35, 35)
    Public Property ColorNone2() As Color
        Get
            Return _ColorNone2
        End Get
        Set(ByVal value As Color)
            _ColorNone2 = value
            Invalidate()
        End Set
    End Property

    Private _ColorNoneOutline As Color = Color.FromArgb(0, 0, 0)
    Public Property ColorNoneOutline() As Color
        Get
            Return _ColorNoneOutline
        End Get
        Set(ByVal value As Color)
            _ColorNoneOutline = value
            Invalidate()
        End Set
    End Property

#End Region

#Region "Color Over"

    Private _ColorOver1 As Color = Color.FromArgb(60, 60, 60)
    Public Property ColorOver1() As Color
        Get
            Return _ColorOver1
        End Get
        Set(ByVal value As Color)
            _ColorOver1 = value
            Invalidate()
        End Set
    End Property

    Private _ColorOver2 As Color = Color.FromArgb(55, 55, 55)
    Public Property ColorOver2() As Color
        Get
            Return _ColorOver2
        End Get
        Set(ByVal value As Color)
            _ColorOver2 = value
            Invalidate()
        End Set
    End Property

    Private _ColorOverOutline As Color = Color.FromArgb(0, 0, 0)
    Public Property ColorOverOutline() As Color
        Get
            Return _ColorOverOutline
        End Get
        Set(ByVal value As Color)
            _ColorOverOutline = value
            Invalidate()
        End Set
    End Property

#End Region

    Sub New()
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
    End Sub
    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.Transparent)
        Me.Width = 30
        Me.Height = 7
        Me.MaximumSize = New Size(New Point(25, 7))
        Select Case State
            Case MouseState.None
                G.FillRectangle(New SolidBrush(_ColorNone1), New Rectangle(0, 0, Width, Height / 2))
                G.FillRectangle(New SolidBrush(_ColorNone1), New Rectangle(0, Height / 2, Width, Height / 2))
                G.DrawRectangle(New Pen(_ColorNoneOutline), New Rectangle(0, 0, Width - 1, Height - 1))
            Case MouseState.Over
                G.FillRectangle(New SolidBrush(_ColorOver1), New Rectangle(0, 0, Width, Height / 2))
                G.FillRectangle(New SolidBrush(_ColorOver2), New Rectangle(0, Height / 2, Width, Height / 2))
                G.DrawRectangle(New Pen(_ColorOverOutline), New Rectangle(0, 0, Width - 1, Height - 1))

                ' I would do MouseState.Down : FindForm.Close() but it closes the theme then the form and it looks ugly. Just do it manually. Sorry
        End Select
    End Sub
End Class
Class FrostComboBox
    Inherits ComboBox

    Private _FontColor As Color = Color.FromArgb(132, 112, 255)
    Public Property FontColor() As Color
        Get
            Return _FontColor
        End Get
        Set(ByVal value As Color)
            _FontColor = value
            Invalidate()
        End Set
    End Property

    Private _Color1 As Color = Color.FromArgb(27, 28, 32)
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
            Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(9, 9, 9)
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
            Invalidate()
        End Set
    End Property

    Private _Color3 As Color = Color.FromArgb(70, 70, 70)
    Public Property Color3() As Color
        Get
            Return _Color3
        End Get
        Set(ByVal value As Color)
            _Color3 = value
            Invalidate()
        End Set
    End Property

    Private _Color4 As Color = Color.FromArgb(100, 100, 100)
    Public Property Color4() As Color
        Get
            Return _Color4
        End Get
        Set(ByVal value As Color)
            _Color4 = value
            Invalidate()
        End Set
    End Property

    Private _Color5 As Color = Color.FromArgb(29, 29, 29)
    Public Property Color5() As Color
        Get
            Return _Color5
        End Get
        Set(ByVal value As Color)
            _Color5 = value
            Invalidate()
        End Set
    End Property

    Private _Color6 As Color = Color.FromArgb(21, 21, 21)
    Public Property Color6() As Color
        Get
            Return _Color6
        End Get
        Set(ByVal value As Color)
            _Color6 = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DropDownStyle = ComboBoxStyle.DropDownList
        Font = New Font("Segoe UI", 10)
        BackColor = Color.FromArgb(0, 0, 0)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G.Clear(BackColor)
        G.FillRectangle(New SolidBrush(_Color1), New Rectangle(2, 2, Width, (Height / 2) - 2))
        G.FillRectangle(New SolidBrush(_Color2), New Rectangle(2, (Height / 2), Width, (Height / 2) - 2.5))
        G.DrawRectangle(New Pen(BackColor), New Rectangle(0, 0, Width - 1, Height - 1))
        G.DrawRectangle(New Pen(_Color3), New Rectangle(1, 1, Width - 3, Height - 3))
        G.FillPolygon(New SolidBrush(_Color4), {New Point(Width - 13, Height / 2 - 2), New Point(Width - 6, Height / 2 - 2), New Point(Width - 10, Height / 2 + 2)})
        G.DrawString(Text, Font, New SolidBrush(_FontColor), New Point(5, 3))
    End Sub
    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(New SolidBrush(_Color5), e.Bounds)
        Else
            e.Graphics.FillRectangle(New SolidBrush(_Color6), e.Bounds)
        End If
        If Not e.Index = -1 Then
            e.Graphics.DrawString(GetItemText(Items(e.Index)), e.Font, New SolidBrush(_FontColor), e.Bounds)
        End If
    End Sub
End Class
Class FrostGroupBox
    Inherits ThemeContainer154

    Private _Color1 As Color = Color.FromArgb(21, 21, 21)
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
            Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(9, 9, 9)
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
            Invalidate()
        End Set
    End Property

    Private _Color3 As Color = Color.FromArgb(27, 28, 32)
    Public Property Color3() As Color
        Get
            Return _Color3
        End Get
        Set(ByVal value As Color)
            _Color3 = value
            Invalidate()
        End Set
    End Property

    Private _Color4 As Color = Color.FromArgb(100, 100, 100)
    Public Property Color4() As Color
        Get
            Return _Color4
        End Get
        Set(ByVal value As Color)
            _Color4 = value
            Invalidate()
        End Set
    End Property

    Private _Color5 As Color = Color.FromArgb(0, 0, 0)
    Public Property Color5() As Color
        Get
            Return _Color5
        End Get
        Set(ByVal value As Color)
            _Color5 = value
            Invalidate()
        End Set
    End Property

    Private _FontColor As Color = Color.FromArgb(132, 112, 255)
    Public Property FontColor() As Color
        Get
            Return _FontColor
        End Get
        Set(ByVal value As Color)
            _FontColor = value
            Invalidate()
        End Set
    End Property

    Sub New()
        ControlMode = True
        BackColor = Color.FromArgb(0, 0, 0)
        Font = New Font("Segoe UI", 10)
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(_Color1)

        G.FillRectangle(New SolidBrush(_Color3), New Rectangle(0, 0, Width, 24))
        G.FillRectangle(New SolidBrush(_Color2), New Rectangle(0, 14, Width, 10))

        G.FillPolygon(New SolidBrush(_Color2), {New Point(0, 34), New Point(0, 24), New Point(10, 24)})
        G.FillPolygon(New SolidBrush(_Color2), {New Point(Width, 34), New Point(Width, 24), New Point(Width - 10, 24)})

        G.DrawRectangle(New Pen(_Color4), New Rectangle(0, 0, Width - 1, Height - 1))

        G.FillPolygon(New SolidBrush(_Color5), {New Point(0, 8), New Point(0, 0), New Point(8, 0)})
        G.FillPolygon(New SolidBrush(_Color5), {New Point(Width, 7), New Point(Width, 0), New Point(Width - 8, 0)})
        G.FillPolygon(New SolidBrush(_Color5), {New Point(0, Height - 8), New Point(0, Height), New Point(8, Height)})
        G.FillPolygon(New SolidBrush(_Color5), {New Point(Width, Height - 8), New Point(Width, Height), New Point(Width - 9, Height)})

        G.DrawLine(New Pen(_Color4), New Point(0, 7), New Point(7, 0))
        G.DrawLine(New Pen(_Color4), New Point(Width - 2, 6), New Point(Width - 8, 0))
        G.DrawLine(New Pen(_Color4), New Point(0, Height - 8), New Point(8, Height))
        G.DrawLine(New Pen(_Color4), New Point(Width - 1, Height - 8), New Point(Width - 8, Height - 1))

        DrawText(New SolidBrush(_FontColor), HorizontalAlignment.Left, 10, 2)
    End Sub
End Class
Class FrostMessageBox
    Inherits System.Windows.Forms.Form
    Private _Message As String = ""
    Private _Title As String = ""
    Property Title As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
        End Set
    End Property
    Property Message As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property

    Public Sub Display()
        Dim Theme As New FrostTheme
        Dim Textbox As New FrostTextBox
        Textbox.Text = _Message
        Textbox.Multiline = True
        Textbox.Location = New Point(23, 47)
        Textbox.Size = New Point(350, 72)
        Textbox.ReadOnly = True
        Me.Text = _Title
        Dim Button As New FrostButton
        Button.Text = "Ok"
        Button.Size = New Point(210, 24)
        Button.Location = New Point(93, 128)
        Me.Controls.AddRange({Theme, Textbox, Button})
        AddHandler Button.Click, AddressOf quit
        Theme.SendToBack()
        Me.Size = New Point(396, 175)
        Me.TransparencyKey = Color.Fuchsia
        Me.Show()
    End Sub
    Private Sub quit()
        Me.Close()
    End Sub
End Class
Class FrostMinWindow
    Inherits ThemeControl154

#Region "Color None"

    Private _ColorNone1 As Color = Color.FromArgb(40, 40, 40)
    Public Property ColorNone1() As Color
        Get
            Return _ColorNone1
        End Get
        Set(ByVal value As Color)
            _ColorNone1 = value
            Invalidate()
        End Set
    End Property

    Private _ColorNone2 As Color = Color.FromArgb(35, 35, 35)
    Public Property ColorNone2() As Color
        Get
            Return _ColorNone2
        End Get
        Set(ByVal value As Color)
            _ColorNone2 = value
            Invalidate()
        End Set
    End Property

    Private _ColorNoneOutline As Color = Color.FromArgb(0, 0, 0)
    Public Property ColorNoneOutline() As Color
        Get
            Return _ColorNoneOutline
        End Get
        Set(ByVal value As Color)
            _ColorNoneOutline = value
            Invalidate()
        End Set
    End Property

#End Region

#Region "Color Over"

    Private _ColorOver1 As Color = Color.FromArgb(60, 60, 60)
    Public Property ColorOver1() As Color
        Get
            Return _ColorOver1
        End Get
        Set(ByVal value As Color)
            _ColorOver1 = value
            Invalidate()
        End Set
    End Property

    Private _ColorOver2 As Color = Color.FromArgb(55, 55, 55)
    Public Property ColorOver2() As Color
        Get
            Return _ColorOver2
        End Get
        Set(ByVal value As Color)
            _ColorOver2 = value
            Invalidate()
        End Set
    End Property

    Private _ColorOverOutline As Color = Color.FromArgb(0, 0, 0)
    Public Property ColorOverOutline() As Color
        Get
            Return _ColorOverOutline
        End Get
        Set(ByVal value As Color)
            _ColorOverOutline = value
            Invalidate()
        End Set
    End Property

#End Region

    Sub New()
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
    End Sub
    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.Transparent)
        G.FillRectangle(New SolidBrush(Color.Transparent), New Rectangle(0, 0, Width, Height))
        Me.Width = 30
        Me.Height = 7
        Me.MaximumSize = New Size(New Point(25, 7))
        Select Case State
            Case MouseState.None
                G.FillRectangle(New SolidBrush(_ColorNone1), New Rectangle(0, 0, Width, Height / 2))
                G.FillRectangle(New SolidBrush(_ColorNone1), New Rectangle(0, Height / 2, Width, Height / 2))
                G.DrawRectangle(New Pen(_ColorNoneOutline), New Rectangle(0, 0, Width - 1, Height - 1))
            Case MouseState.Over
                G.FillRectangle(New SolidBrush(_ColorOver1), New Rectangle(0, 0, Width, Height / 2))
                G.FillRectangle(New SolidBrush(_ColorOver2), New Rectangle(0, Height / 2, Width, Height / 2))
                G.DrawRectangle(New Pen(_ColorOverOutline), New Rectangle(0, 0, Width - 1, Height - 1))
            Case MouseState.Down
                FindForm.WindowState = FormWindowState.Minimized
        End Select
    End Sub
End Class
Class FrostProgressBar
    Inherits ThemeControl154
    Private _Minimum As Integer
    Property Minimum() As Integer
        Get
            Return _Minimum
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                Throw New Exception("Property value is not valid.")
            End If

            _Minimum = value
            If value > _Value Then _Value = value
            If value > _Maximum Then _Maximum = value
            Invalidate()
        End Set
    End Property

    Private _Maximum As Integer = 100
    Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                Throw New Exception("Property value is not valid.")
            End If

            _Maximum = value
            If value < _Value Then _Value = value
            If value < _Minimum Then _Minimum = value
            Invalidate()
        End Set
    End Property

    Private _Value As Integer
    Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value > _Maximum OrElse value < _Minimum Then
                Throw New Exception("Property value is not valid.")
            End If

            _Value = value
            Invalidate()
        End Set
    End Property

    Public Sub Increment(ByVal amount As Integer)
        If Value + amount > Maximum Then
            Value = Maximum
        Else
            Value += amount
        End If
    End Sub

    Private _FontColor As Color = Color.FromArgb(132, 112, 255)
    Public Property FontColor() As Color
        Get
            Return _FontColor
        End Get
        Set(ByVal value As Color)
            _FontColor = value
            Invalidate()
        End Set
    End Property

    Private _Color1 As Color = Color.FromArgb(0, 0, 0)
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
            Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(70, 70, 70)
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
            Invalidate()
        End Set
    End Property

    Private _Color3 As Color = Color.FromArgb(40, 40, 40)
    Public Property Color3() As Color
        Get
            Return _Color3
        End Get
        Set(ByVal value As Color)
            _Color3 = value
            Invalidate()
        End Set
    End Property

    Private _Color4 As Color = Color.FromArgb(21, 21, 21)
    Public Property Color4() As Color
        Get
            Return _Color4
        End Get
        Set(ByVal value As Color)
            _Color4 = value
            Invalidate()
        End Set
    End Property

    Private _Color5 As Color = Color.FromArgb(27, 28, 32)
    Public Property Color5() As Color
        Get
            Return _Color5
        End Get
        Set(ByVal value As Color)
            _Color5 = value
            Invalidate()
        End Set
    End Property

    Private _Color6 As Color = Color.FromArgb(9, 9, 9)
    Public Property Color6() As Color
        Get
            Return _Color6
        End Get
        Set(ByVal value As Color)
            _Color6 = value
            Invalidate()
        End Set
    End Property

    Sub New()
        BackColor = Color.FromArgb(30, 30, 30)
        Font = New Font("Segoe UI", 10)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        Dim HB As New HatchBrush(HatchStyle.LightUpwardDiagonal, _Color3, _Color4)

        G.Clear(BackColor)
        G.DrawRectangle(New Pen(_Color1), New Rectangle(0, 0, Width - 1, Height - 1))
        G.DrawRectangle(New Pen(_Color2), New Rectangle(1, 1, Width - 3, Height - 3))
        G.FillRectangle(HB, New Rectangle(2, 2, Width - 4, Height - 4))
        G.FillRectangle(New SolidBrush(_Color5), New Rectangle(2, 2, Int((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 4)), (Height / 2) - 2))
        G.FillRectangle(New SolidBrush(_Color6), New Rectangle(2, (Height / 2), CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 4)), (Height / 2) - 2.5))
        G.DrawString(_Value.ToString + "%", Font, New SolidBrush(_FontColor), (Width / 2) - (_Value.ToString + "%").Length * 5, 3)
    End Sub
End Class
<DefaultEvent("CheckedChanged")> _
Class FrostRadioButton
    Inherits ThemeControl154

    Private _FontColor As Color = Color.FromArgb(132, 112, 255)
    Public Property FontColor() As Color
        Get
            Return _FontColor
        End Get
        Set(ByVal value As Color)
            _FontColor = value
            Invalidate()
        End Set
    End Property

    Private _Color1 As Color = Color.FromArgb(70, 70, 70)
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
            Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(132, 112, 255)
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
            Invalidate()
        End Set
    End Property

    Private _Color3 As Color = Color.FromArgb(122, 102, 245)
    Public Property Color3() As Color
        Get
            Return _Color3
        End Get
        Set(ByVal value As Color)
            _Color3 = value
            Invalidate()
        End Set
    End Property

    Private _Color4 As Color = Color.FromArgb(30, 30, 30)
    Public Property Color4() As Color
        Get
            Return _Color4
        End Get
        Set(ByVal value As Color)
            _Color4 = value
            Invalidate()
        End Set
    End Property

    Private _Color5 As Color = Color.FromArgb(25, 25, 25)
    Public Property Color5() As Color
        Get
            Return _Color5
        End Get
        Set(ByVal value As Color)
            _Color5 = value
            Invalidate()
        End Set
    End Property

    Event CheckedChanged(ByVal sender As Object)

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)

        BackColor = Color.FromArgb(21, 21, 21)
        Font = New Font("Segoe UI", 10)
    End Sub

    Private _Checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value

            If _Checked Then
                InvalidateParent()
            End If

            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    Private Sub InvalidateParent()
        If Parent Is Nothing Then Return

        For Each C As Control In Parent.Controls
            If Not (C Is Me) AndAlso (TypeOf C Is FrostRadioButton) Then
                DirectCast(C, FrostRadioButton).Checked = False
            End If
        Next
    End Sub


    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        Checked = True
        MyBase.OnMouseDown(e)
    End Sub

    Dim TextBrush As Brush
    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        G.Clear(BackColor)
        G.DrawRectangle(New Pen(_Color1), New Rectangle(1, 1, Height - 3, Height - 3))
        If _Checked Then
            G.FillRectangle(New SolidBrush(_Color2), New Rectangle(3, 3, Height - 6, Height / 2))
            G.FillRectangle(New SolidBrush(_Color3), New Rectangle(3, Height / 2, Height - 6, Height / 2 - 4))
        Else
            G.FillRectangle(New SolidBrush(_Color4), New Rectangle(3, 3, Height - 6, Height / 2))
            G.FillRectangle(New SolidBrush(_Color5), New Rectangle(3, Height / 2, Height - 6, Height / 2 - 4))
        End If
        DrawText(New SolidBrush(_FontColor), Height + 5, Height / 2 - Me.Font.SizeInPoints)
    End Sub
End Class
Class FrostTabControl
    Inherits TabControl
    Dim G As Graphics

    Private _FontColor As Color = Color.FromArgb(132, 112, 255)
    Public Property FontColor() As Color
        Get
            Return _FontColor
        End Get
        Set(ByVal value As Color)
            _FontColor = value
            Invalidate()
        End Set
    End Property

    Private _Color1 As Color = Color.FromArgb(21, 21, 21)
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
            Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(100, 100, 100)
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
            Invalidate()
        End Set
    End Property

    Private _Color3 As Color = Color.FromArgb(0, 0, 0)
    Public Property Color3() As Color
        Get
            Return _Color3
        End Get
        Set(ByVal value As Color)
            _Color3 = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)

        SizeMode = TabSizeMode.Fixed
        Alignment = TabAlignment.Left
        ItemSize = New Size(28, 115)
        BackColor = _Color1
        DrawMode = TabDrawMode.OwnerDrawFixed

        Font = New Font("Segoe UI", 10)

        SF1 = New StringFormat()
        SF1.LineAlignment = StringAlignment.Center
    End Sub

    Protected Overrides Sub OnControlAdded(ByVal e As ControlEventArgs)
        If TypeOf e.Control Is TabPage Then
            e.Control.BackColor = _Color1
        End If

        MyBase.OnControlAdded(e)
    End Sub

    Private R1, R2 As Rectangle
    Private TP1 As TabPage
    Private SF1 As StringFormat
    Private Offset, ItemHeight As Integer

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        G = e.Graphics

        G.Clear(_Color1)

        G.DrawLine(New Pen(_Color2), New Point(118, 0), New Point(118, Height))
        For I As Integer = 0 To TabCount - 1
            R1 = GetTabRect(I)
            R1.Y += 2
            R1.Height -= 3
            R1.Width += 1
            R1.X -= 1

            TP1 = TabPages(I)
            Offset = 0

            If SelectedIndex = I Then
                G.DrawLine(New Pen(_Color2), New Point(1, R1.Y), New Point(118, R1.Y))
                G.DrawLine(New Pen(_Color2), New Point(1, R1.Y + R1.Height), New Point(118, R1.Y + R1.Height))
                G.DrawLine(New Pen(_Color1), New Point(R1.X + R1.Width + 1, R1.Y + 1), New Point(R1.X + R1.Width + 1, R1.Y + R1.Height - 1))
            End If
            Offset += 10
            R1.X += 5 + Offset

            R2 = R1
            R2.Y += 1
            R2.X += 1

            G.DrawString(TP1.Text, Font, New SolidBrush(_FontColor), R1, SF1)
        Next

        G.DrawRectangle(New Pen(_Color2), New Rectangle(0, 0, Width - 1, Height - 1))

        G.FillPolygon(New SolidBrush(_Color3), {New Point(0, 8), New Point(0, 0), New Point(8, 0)})
        G.FillPolygon(New SolidBrush(_Color3), {New Point(Width, 7), New Point(Width, 0), New Point(Width - 8, 0)})
        G.FillPolygon(New SolidBrush(_Color3), {New Point(0, Height - 8), New Point(0, Height), New Point(8, Height)})
        G.FillPolygon(New SolidBrush(_Color3), {New Point(Width, Height - 8), New Point(Width, Height), New Point(Width - 9, Height)})

        G.DrawLine(New Pen(_Color2), New Point(0, 7), New Point(7, 0))
        G.DrawLine(New Pen(_Color2), New Point(Width - 2, 6), New Point(Width - 8, 0))
        G.DrawLine(New Pen(_Color2), New Point(0, Height - 8), New Point(8, Height))
        G.DrawLine(New Pen(_Color2), New Point(Width - 1, Height - 8), New Point(Width - 8, Height - 1))

    End Sub

End Class
<DefaultEvent("TextChanged")> _
Class FrostTextBox
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

    Private _Color1 As Color = Color.FromArgb(0, 0, 0)
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
            Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(70, 70, 70)
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
            Invalidate()
        End Set
    End Property

    Private _FontColor As Color = Color.FromArgb(132, 112, 255)
    Public Property FontColor() As Color
        Get
            Return _FontColor
        End Get
        Set(ByVal value As Color)
            _FontColor = value
            Base.ForeColor = _FontColor
            Invalidate()
        End Set
    End Property

    Private Base As TextBox
    Sub New()
        Base = New TextBox
        Font = New Font("Segoe UI", 10)
        Base.Font = New Font("Segoe UI", 10)
        Base.Text = Text
        Base.MaxLength = _MaxLength
        Base.Multiline = _Multiline
        Base.ReadOnly = _ReadOnly
        Base.UseSystemPasswordChar = _UseSystemPasswordChar
        Base.BorderStyle = BorderStyle.None
        Base.Location = New Point(5, 5)
        Base.Width = Width - 10
        BackColor = Color.FromArgb(35, 35, 35)
        If _Multiline Then
            Base.Height = Height - 11
        Else
            LockHeight = Base.Height + 11
        End If
        AddHandler Base.TextChanged, AddressOf OnBaseTextChanged
        AddHandler Base.KeyDown, AddressOf OnBaseKeyDown
    End Sub

    Protected Overrides Sub ColorHook()
        Base.ForeColor = _FontColor
        Base.BackColor = BackColor
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Base.BackColor)
        G.DrawRectangle(New Pen(_Color1), New Rectangle(0, 0, Width - 1, Height - 1))
        G.DrawRectangle(New Pen(_Color2), New Rectangle(1, 1, Width - 3, Height - 3))

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
        Base.Location = New Point(5, 5)
        Base.Width = Width - 10

        If _Multiline Then
            Base.Height = Height - 11
        End If


        MyBase.OnResize(e)
    End Sub

End Class
Class FrostTheme
    Inherits ThemeContainer154

    Private _Color1 As Color = Color.FromArgb(21, 21, 21)
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
            Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(35, 35, 35)
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
            Invalidate()
        End Set
    End Property

    Private _Color3 As Color = Color.FromArgb(9, 9, 9)
    Public Property Color3() As Color
        Get
            Return _Color3
        End Get
        Set(ByVal value As Color)
            _Color3 = value
            Invalidate()
        End Set
    End Property

    Private _Color4 As Color = Color.FromArgb(27, 28, 32)
    Public Property Color4() As Color
        Get
            Return _Color4
        End Get
        Set(ByVal value As Color)
            _Color4 = value
            Invalidate()
        End Set
    End Property

    Private _Color5 As Color = Color.FromArgb(42, 42, 42)
    Public Property Color5() As Color
        Get
            Return _Color5
        End Get
        Set(ByVal value As Color)
            _Color5 = value
            Invalidate()
        End Set
    End Property

    Private _Color6 As Color = Color.FromArgb(132, 112, 255)
    Public Property Color6() As Color
        Get
            Return _Color6
        End Get
        Set(ByVal value As Color)
            _Color6 = value
            Invalidate()
        End Set
    End Property

    Sub New()
        BackColor = Color.Black
        TransparencyKey = Color.Fuchsia
        Font = New Font("Segoe UI", 10)
    End Sub
    Protected Overrides Sub ColorHook()
    End Sub
    Protected Overrides Sub PaintHook()
        G.Clear(TransparencyKey)
        G.FillRectangle(New SolidBrush(_Color6), New Rectangle(20, 4, Width - 40, Height - 8))
        G.FillRectangle(New SolidBrush(_Color6), New Rectangle(4, 20, Width - 8, Height - 40))
        G.FillRectangle(New SolidBrush(_Color1), New Rectangle(10, 10, Width - 20, Height - 20))
        G.FillRectangle(New SolidBrush(_Color2), New Rectangle(10, 15, Width - 20, Height - 30))
        G.FillRectangle(New SolidBrush(_Color2), New Rectangle(15, 10, Width - 30, Height - 20))
        G.FillRectangle(New SolidBrush(BackColor), New Rectangle(15, 10, Width - 30, Height - 24))

        'corners
        G.FillPolygon(New SolidBrush(_Color1), {New Point(20, 0), New Point(Width / 3, 0), New Point((Width / 3) + 10, 10), New Point(10, 10)})
        G.FillPolygon(New SolidBrush(_Color2), {New Point(20, 5), New Point(Width / 3, 5), New Point((Width / 3) + 10, 10), New Point(10, 15)})
        G.FillPolygon(New SolidBrush(_Color1), {New Point(0, 20), New Point(0, Height / 3), New Point(10, (Height / 3) + 10), New Point(10, 10)})
        G.FillPolygon(New SolidBrush(_Color2), {New Point(5, 20), New Point(5, Height / 3), New Point(10, (Height / 3) + 10), New Point(10, 15)})

        G.FillPolygon(New SolidBrush(_Color1), {New Point(Width - 20, 0), New Point(Width - (Width / 3), 0), New Point(Width - ((Width / 3) + 10), 10), New Point(Width - 10, 10)})
        G.FillPolygon(New SolidBrush(_Color2), {New Point(Width - 20, 5), New Point(Width - (Width / 3), 5), New Point(Width - ((Width / 3) + 10), 10), New Point(Width - 10, 15)})
        G.FillPolygon(New SolidBrush(_Color1), {New Point(Width, 20), New Point(Width, (Height / 3)), New Point(Width - 10, ((Height / 3) + 10)), New Point(Width - 10, 10)})
        G.FillPolygon(New SolidBrush(_Color2), {New Point(Width - 5, 20), New Point(Width - 5, (Height / 3)), New Point(Width - 10, ((Height / 3) + 10)), New Point(Width - 10, 15)})

        G.FillPolygon(New SolidBrush(_Color1), {New Point(Width - 20, Height), New Point(Width - (Width / 3), Height), New Point(Width - ((Width / 3) + 10), Height - 10), New Point(Width - 10, Height - 10)})
        G.FillPolygon(New SolidBrush(_Color2), {New Point(Width - 20, Height - 5), New Point(Width - (Width / 3), Height - 5), New Point(Width - ((Width / 3) + 10), Height - 10), New Point(Width - 10, Height - 15)})
        G.FillPolygon(New SolidBrush(_Color1), {New Point(Width, Height - 20), New Point(Width, Height - (Height / 3)), New Point(Width - 10, Height - ((Height / 3) + 10)), New Point(Width - 10, Height - 10)})
        G.FillPolygon(New SolidBrush(_Color2), {New Point(Width - 5, Height - 20), New Point(Width - 5, Height - (Height / 3)), New Point(Width - 10, Height - ((Height / 3) + 10)), New Point(Width - 10, Height - 15)})

        G.FillPolygon(New SolidBrush(_Color1), {New Point(20, Height), New Point(Width / 3, Height), New Point((Width / 3) + 10, Height - 10), New Point(10, Height - 10)})
        G.FillPolygon(New SolidBrush(_Color2), {New Point(20, Height - 5), New Point(Width / 3, Height - 5), New Point((Width / 3) + 10, Height - 10), New Point(10, Height - 15)})
        G.FillPolygon(New SolidBrush(_Color1), {New Point(0, Height - 20), New Point(0, Height - (Height / 3)), New Point(10, Height - ((Height / 3) + 10)), New Point(10, Height - 10)})
        G.FillPolygon(New SolidBrush(_Color2), {New Point(5, Height - 20), New Point(5, Height - (Height / 3)), New Point(10, Height - ((Height / 3) + 10)), New Point(10, Height - 15)})

        'outer lines

        G.DrawLine(New Pen(_Color1), New Point((Width / 3) + 9, 9), New Point(Width - ((Width / 3) + 9), 9))
        G.DrawLine(New Pen(_Color1), New Point(9, (Height / 3) + 9), New Point(9, Height - ((Height / 3) + 9)))
        G.DrawLine(New Pen(_Color1), New Point((Width / 3) + 9, Height - 10), New Point(Width - ((Width / 3) + 10), Height - 10))
        G.DrawLine(New Pen(_Color1), New Point(Width - 10, (Height / 3) + 9), New Point(Width - 10, Height - ((Height / 3) + 9)))

        G.FillRectangle(New SolidBrush(_Color2), New Rectangle(10, 36, Width - 20, 2))
        G.DrawLine(New Pen(_Color5), New Point(10, 36), New Point(Width - 11, 36))

        'inner corners
        G.DrawLine(New Pen(_Color2), New Point(15, 38), New Point(18, 38))
        G.DrawLine(New Pen(_Color2), New Point(15, 39), New Point(16, 39))
        G.DrawLine(New Pen(_Color2), New Point(15, 40), New Point(15, 41))

        G.DrawLine(New Pen(_Color2), New Point(Width - 15, 38), New Point(Width - 18, 38))
        G.DrawLine(New Pen(_Color2), New Point(Width - 15, 39), New Point(Width - 16, 39))
        G.DrawLine(New Pen(_Color2), New Point(Width - 15, 40), New Point(Width - 15, 41))

        G.DrawLine(New Pen(_Color2), New Point(10, Height - 15), New Point(18, Height - 15))
        G.DrawLine(New Pen(_Color2), New Point(15, Height - 16), New Point(16, Height - 16))
        G.DrawLine(New Pen(_Color2), New Point(15, Height - 17), New Point(15, Height - 18))

        G.DrawLine(New Pen(_Color2), New Point(Width - 11, Height - 15), New Point(Width - 19, Height - 15))
        G.DrawLine(New Pen(_Color2), New Point(Width - 16, Height - 16), New Point(Width - 17, Height - 16))
        G.DrawLine(New Pen(_Color2), New Point(Width - 16, Height - 17), New Point(Width - 16, Height - 18))

        G.FillRectangle(New SolidBrush(_Color4), New Rectangle(14, 11, Width - 29, 13))
        G.FillRectangle(New SolidBrush(_Color3), New Rectangle(14, 24, Width - 29, 12))
        G.DrawRectangle(New Pen(_Color5), New Rectangle(14, 11, Width - 29, 25))


        G.DrawString(FindForm.Text, Font, New SolidBrush(_Color6), New Point(17, 13))
    End Sub
End Class