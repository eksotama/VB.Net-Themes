'IMPORTANT:
'Please leave these comments in place as they help protect intellectual rights and allow
'developers to determine the version of the theme they are using. 
'Name: SLC Theme
'Created: Not defined yet
'Version: 1.0.0.0 beta
'Site: http://sliceproducts.pw/

'Copyright © 2013 Slice Software
'Special Thanks to : Aeonhack http://nimoru.com/


#Region "Theme Container"
Imports System, System.IO, System.Collections.Generic
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging

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
                ParentForm.FormBorderstyle = _BorderStyle
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
            _Borderstyle = value

            If _IsParentForm AndAlso Not _ControlMode Then
                ParentForm.FormBorderstyle = value

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
#End Region
#Region "SLCTheme"
Class SLCTheme
    Inherits ThemeContainer154

    Private P1, P2 As Pen
    Private topc, botc, topc2, botc2 As Color
    Private B1 As SolidBrush




    Sub New()

        TransparencyKey = Color.Fuchsia
        Header = 30
        SetColor("top", Color.FromArgb(21, 18, 37))
        SetColor("bottom", Color.FromArgb(32, 35, 54))
        '(21, 18, 37)
        SetColor("top2", Color.FromArgb(32, 35, 54))
        SetColor("bottom2", Color.FromArgb(21, 18, 37))
        BackColor = Color.FromArgb(0, 57, 72)
        P1 = New Pen(Color.FromArgb(35, 35, 35))
        P2 = New Pen(Color.FromArgb(60, 60, 60))
        B1 = New SolidBrush(Color.FromArgb(50, 50, 50))

    End Sub

    Protected Overrides Sub ColorHook()
        topc = GetColor("top")
        botc = GetColor("bottom")
        topc2 = GetColor("top2")
        botc2 = GetColor("bottom2")
    End Sub

    Private Function PrepareBorder() As GraphicsPath
        Dim P As New GraphicsPath()

        Dim PS As New List(Of Point)
        PS.Add(New Point(0, 2))
        PS.Add(New Point(2, 0))
        PS.Add(New Point(100, 0))
        PS.Add(New Point(115, 15))
        PS.Add(New Point(Width - 1 - 115, 15))
        PS.Add(New Point(Width - 1 - 100, 0))
        PS.Add(New Point(Width - 2, 0))
        PS.Add(New Point(Width - 1, 3))


        'PS.Add(New Point(Width - 1, Height - 1))

        'bottom
        PS.Add(New Point(Width - 1, Height - 3))
        PS.Add(New Point(Width - 3, Height - 1))
        PS.Add(New Point(Width - 100, Height - 1))
        PS.Add(New Point(Width - 115, Height - 15 - 1))
        PS.Add(New Point(116, Height - 15 - 1))
        PS.Add(New Point(101, Height - 1))
        PS.Add(New Point(2, Height - 1))
        PS.Add(New Point(0, Height - 2))

        P.AddPolygon(PS.ToArray())
        Return P
    End Function

    'Private Function FormInsideSQ()
    '    Dim P As New GraphicsPath()
    '    Dim PS As New List(Of Point)

    '    PS.Add(New Point(6, Height - 310))
    '    PS.Add(New Point(Width - 6, 64))
    '    PS.Add(New Point(Width - 6, Height - 6))
    '    PS.Add(New Point(Width - 100, Height - 6))
    '    PS.Add(New Point(Width - 116, Height - 22))
    '    PS.Add(New Point(Width - 522, Height - 22))
    '    PS.Add(New Point(Width - 538, Height - 6))
    '    PS.Add(New Point(6, Height - 6))
    '    P.AddPolygon(PS.ToArray())
    '    Return p
    'End Function



    Protected Overrides Sub PaintHook()
        TransparencyKey = Color.Fuchsia

        G.Clear(Color.Fuchsia)




        Dim HB As New HatchBrush(HatchStyle.Trellis, Color.FromArgb(50, Color.FromArgb(38, 138, 201)), Color.FromArgb(80, Color.FromArgb(12, 40, 57)))
        Dim linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(108, 137, 184), Color.FromArgb(13, 20, 25), 20.0F)

        G.FillRectangle(linear, New Rectangle(3, 3, Width - 5, Height - 3))

        G.FillRectangle(HB, New Rectangle(3, 3, Width - 5, Height - 3))


        G.DrawLine(Pens.Fuchsia, 1, 0, Width - 1, 0)
        G.DrawLine(Pens.Fuchsia, 1, 1, Width - 1, 1)
        G.DrawLine(New Pen(Color.FromArgb(26, 47, 59)), 1, 2, Width - 1, 2)

        G.DrawLine(Pens.Fuchsia, 1, Height - 1, Width - 1, Height - 1)
        G.DrawLine(Pens.Fuchsia, 1, Height - 2, Width - 1, Height - 2)
        G.DrawLine(New Pen(Color.FromArgb(26, 47, 59)), 1, Height - 2, Width - 4, Height - 2)




        Dim GPF As GraphicsPath = PrepareBorder()


        Dim PB2 As PathGradientBrush
        PB2 = New PathGradientBrush(GPF)
        PB2.CenterColor = Color.FromArgb(250, 250, 250)
        PB2.SurroundColors = {Color.FromArgb(237, 237, 237)}
        PB2.FocusScales = New PointF(0.9F, 0.5F)

        G.SetClip(GPF)

        G.FillPath(PB2, GPF)
        G.DrawPath(New Pen(Color.White, 3), GPF)
        G.ResetClip()

        Dim tmpG As GraphicsPath = PrepareBorder()

        G.DrawPath(Pens.Gray, tmpG)



        Dim linear2 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(13, 59, 85), Color.FromArgb(22, 35, 43), 180.0F)



        Dim barGP As New GraphicsPath

        Dim PB3 As PathGradientBrush
        PB3 = New PathGradientBrush(GPF)
        PB3.CenterColor = Color.FromArgb(39, 60, 73)
        PB3.SurroundColors = {Color.FromArgb(31, 105, 152)}
        PB3.FocusScales = New PointF(0.5F, 0.5F)
        PB3.CenterPoint = New Point(Width / 2, 10)

        barGP.AddRectangle(New Rectangle(0, 39, Width - 1, 20))

        G.FillPath(PB3, barGP)
        G.FillPath(New HatchBrush(HatchStyle.NarrowHorizontal, Color.FromArgb(20, 34, 45), Color.Transparent), barGP)

        '// get rid of some pixels
        G.DrawRectangle(Pens.Fuchsia, New Rectangle(Width - 4, 40, 3, 17))
        G.FillRectangle(Brushes.Fuchsia, New Rectangle(Width - 4, 40, 3, 17))

        G.DrawRectangle(Pens.Fuchsia, New Rectangle(0, 40, 3, 17))
        G.FillRectangle(Brushes.Fuchsia, New Rectangle(0, 40, 3, 17))


        '// inside square

        'Dim SQpth As GraphicsPath = FormInsideSQ()
        ' G.DrawPath(Pens.Red, SQpth)



        DrawText(New SolidBrush(Color.FromArgb(30, Color.Black)), HorizontalAlignment.Left, 12, 6)
        DrawText(New SolidBrush(Color.FromArgb(20, Color.Black)), HorizontalAlignment.Left, 11, 5)
        DrawText(Brushes.Black, HorizontalAlignment.Left, 10, 4)





    End Sub
End Class
#End Region
#Region "SLCbtn"
Class SLCbtn
    Inherits ThemeControl154


    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Color.White)

        Select Case State
            Case MouseState.None

                '// bnt form

                Dim linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height / 2), Color.FromArgb(100, Color.FromArgb(207, 207, 207)), Color.FromArgb(250, 250, 250), 90.0F)
                Dim gp As New GraphicsPath
                gp = CreateRound(0, 0, Width - 1, Height - 1, 7)
                G.FillPath(linear, gp)
                G.DrawPath(New Pen(Color.FromArgb(105, 112, 115)), gp)
                G.SetClip(gp)
                Dim btninsideborder As GraphicsPath = CreateRound(1, 1, Width - 3, Height - 3, 7)
                G.DrawPath(New Pen(Color.White, 1), btninsideborder)
                G.ResetClip()

                '// circle



                'Dim GPF As New GraphicsPath
                'GPF.AddEllipse(New Rectangle(4, Height / 2 - 2.5, 6, 6))
                'Dim PB2 As PathGradientBrush
                'PB2 = New PathGradientBrush(GPF)
                'PB2.CenterColor = Color.FromArgb(69, 128, 156)
                'PB2.SurroundColors = {Color.FromArgb(8, 25, 33)}
                'PB2.FocusScales = New PointF(0.9F, 0.9F)


                'G.FillPath(PB2, GPF)

                'G.DrawPath(New Pen(Color.FromArgb(49, 63, 86)), GPF)

                'G.DrawEllipse(New Pen(Color.LightGray), New Rectangle(3, Height / 2 - 3.1, 8, 8))

                DrawText(New SolidBrush(Color.FromArgb(1, 75, 124)), HorizontalAlignment.Left, 5, 1)
            Case MouseState.Down
                '// bnt form

                Dim linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height / 2), Color.FromArgb(100, Color.FromArgb(207, 207, 207)), Color.FromArgb(250, 250, 250), 90.0F)
                Dim gp As New GraphicsPath
                gp = CreateRound(0, 0, Width - 1, Height - 1, 7)
                G.FillPath(linear, gp)
                G.DrawPath(New Pen(Color.FromArgb(105, 112, 115)), gp)
                G.SetClip(gp)
                Dim btninsideborder As GraphicsPath = CreateRound(1, 1, Width - 3, Height - 3, 7)
                G.DrawPath(New Pen(Color.White, 1), btninsideborder)
                G.ResetClip()

                '// circle



                'Dim GPF As New GraphicsPath
                'GPF.AddEllipse(New Rectangle(4, Height / 2 - 2.5, 6, 6))
                'Dim PB2 As PathGradientBrush
                'PB2 = New PathGradientBrush(GPF)
                'PB2.CenterColor = Color.FromArgb(86, 161, 196)
                'PB2.SurroundColors = {Color.FromArgb(94, 176, 215)}
                'PB2.FocusScales = New PointF(0.9F, 0.9F)


                'G.FillPath(PB2, GPF)

                'G.DrawPath(New Pen(Color.FromArgb(105, 194, 236)), GPF)

                'G.DrawEllipse(New Pen(Color.LightGray), New Rectangle(3, Height / 2 - 3.1, 8, 8))

                DrawText(New SolidBrush(Color.FromArgb(86, 161, 196)), HorizontalAlignment.Left, 5, 1)

            Case MouseState.Over
                '// bnt form

                Dim linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height / 2), Color.FromArgb(100, Color.FromArgb(207, 207, 207)), Color.FromArgb(250, 250, 250), 90.0F)
                Dim gp As New GraphicsPath
                gp = CreateRound(0, 0, Width - 1, Height - 1, 7)
                G.FillPath(linear, gp)
                G.DrawPath(New Pen(Color.FromArgb(105, 112, 115)), gp)
                G.SetClip(gp)
                Dim btninsideborder As GraphicsPath = CreateRound(1, 1, Width - 3, Height - 3, 7)
                G.DrawPath(New Pen(Color.FromArgb(50, Color.Gray), 1), btninsideborder)
                G.ResetClip()

                '// circle


                'Dim GPF As New GraphicsPath
                'GPF.AddEllipse(New Rectangle(4, Height / 2 - 2.5, 6, 6))
                'Dim PB2 As PathGradientBrush
                'PB2 = New PathGradientBrush(GPF)
                'PB2.CenterColor = Color.FromArgb(69, 128, 156)
                'PB2.SurroundColors = {Color.FromArgb(8, 25, 33)}
                'PB2.FocusScales = New PointF(0.9F, 0.9F)


                'G.FillPath(PB2, GPF)

                'G.DrawPath(New Pen(Color.FromArgb(49, 63, 86)), GPF)
                'G.DrawEllipse(New Pen(Color.LightGray), New Rectangle(3, Height / 2 - 3.1, 8, 8))
                DrawText(New SolidBrush(Color.FromArgb(1, 75, 124)), HorizontalAlignment.Left, 5, 1)
        End Select

    End Sub
End Class
#End Region
#Region "SLCTabControl"
Class SLCTabControl
    Inherits TabControl

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(30, 120)

    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
    End Sub

    Public Function Borderpts() As GraphicsPath
        Dim P As New GraphicsPath()
        Dim PS As New List(Of Point)

        PS.Add(New Point(0, 0))
        PS.Add(New Point(Width - 1, 0))
        PS.Add(New Point(Width - 1, Height - 1))
        PS.Add(New Point(0, Height - 1))



        P.AddPolygon(PS.ToArray())
        Return P
    End Function

    Public Function BorderptsInside() As GraphicsPath
        Dim P As New GraphicsPath()
        Dim PS As New List(Of Point)

        PS.Add(New Point(1, 1))
        PS.Add(New Point(122, 1))
        PS.Add(New Point(122, Height - 2))
        PS.Add(New Point(1, Height - 2))



        P.AddPolygon(PS.ToArray())
        Return P
    End Function

    Public Function BigBorder() As GraphicsPath
        Dim P As New GraphicsPath()
        Dim PS As New List(Of Point)

        PS.Add(New Point(50, 1))
        PS.Add(New Point(349, 50))
        PS.Add(New Point(349, 50))
        PS.Add(New Point(50, 349))

        P.AddPolygon(PS.ToArray())
        Return P
    End Function

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)



        Dim b As New Bitmap(Width, Height)
        Dim g As Graphics = Graphics.FromImage(b)
        g.Clear(Color.White)




        '//Big square shadow







        Dim GP1 As GraphicsPath = Borderpts()
        g.DrawPath(Pens.LightGray, GP1)


        '// small border
        Dim tmp1 As GraphicsPath = BorderptsInside()

        Dim PB2 As PathGradientBrush
        PB2 = New PathGradientBrush(tmp1)
        PB2.CenterColor = Color.FromArgb(250, 250, 250)
        PB2.SurroundColors = {Color.FromArgb(237, 237, 237)}
        PB2.FocusScales = New PointF(0.9F, 0.9F)

        g.FillPath(PB2, tmp1)
        g.DrawPath(Pens.Gray, tmp1)




        '// items






        For i = 0 To TabCount - 1

            Dim rec As Rectangle = GetTabRect(i)
            Dim rec2 As Rectangle = rec


            '//inside small 
            rec2.Width -= 3
            rec2.Height -= 3
            rec2.Y += 1
            rec2.X += 1




            If i = SelectedIndex Then



                Dim linear As New LinearGradientBrush(New Rectangle(rec2.X + 108, rec2.Y + 1, 10, rec2.Height - 1), Color.FromArgb(227, 227, 227), Color.Transparent, 180.0F)
                Dim linear3 As New LinearGradientBrush(New Rectangle(rec2.X, rec2.Y + 1, 10, rec2.Height - 1), Color.FromArgb(227, 227, 227), Color.Transparent, 180.0F)


                g.FillRectangle(New SolidBrush(Color.FromArgb(242, 242, 242)), rec2)
                g.DrawRectangle(Pens.White, rec2)
                g.DrawRectangle(New Pen(Color.FromArgb(70, Color.FromArgb(39, 93, 127)), 2), rec)
                g.FillRectangle(linear, New Rectangle(rec2.X + 113, rec2.Y + 1, 6, rec2.Height - 1))
                g.FillRectangle(linear3, New Rectangle(rec2.X, rec2.Y + 1, 6, rec2.Height - 1))
                '// circle


                g.SmoothingMode = SmoothingMode.HighQuality
                '    g.DrawEllipse(New Pen(Color.FromArgb(200, 200, 200), 3), New Rectangle(rec2.X + 6.5, rec2.Y + 7, 14, 14))
                ' g.DrawEllipse(New Pen(Color.FromArgb(150, 150, 150), 1), New Rectangle(rec2.X + 6.5, rec2.Y + 7, 14, 14))


                Dim GPF As New GraphicsPath
                GPF.AddEllipse(New Rectangle(rec2.X + 8, rec2.Y + 8, 12, 12))
                Dim PB3 As PathGradientBrush
                PB3 = New PathGradientBrush(GPF)
                PB3.CenterPoint = New Point(rec2.X - 10, rec2.Y - 10)
                PB3.CenterColor = Color.FromArgb(56, 142, 196)
                PB3.SurroundColors = {Color.FromArgb(64, 106, 140)}
                PB3.FocusScales = New PointF(0.9F, 0.9F)


                g.FillPath(PB3, GPF)

                g.DrawPath(New Pen(Color.FromArgb(49, 63, 86)), GPF)
                g.SetClip(GPF)
                g.FillEllipse(New SolidBrush(Color.FromArgb(40, Color.WhiteSmoke)), New Rectangle(rec2.X + 10.5, rec2.Y + 11, 6, 6))
                g.ResetClip()



                g.SmoothingMode = SmoothingMode.None

            Else

                g.SmoothingMode = SmoothingMode.HighQuality
                Dim linear As New LinearGradientBrush(New Rectangle(rec2.X + 108, rec2.Y + 1, 10, rec2.Height - 1), Color.FromArgb(227, 227, 227), Color.Transparent, 180.0F)
                Dim linear3 As New LinearGradientBrush(New Rectangle(rec2.X, rec2.Y + 1, 10, rec2.Height - 1), Color.FromArgb(227, 227, 227), Color.Transparent, 180.0F)


                g.FillRectangle(New SolidBrush(Color.FromArgb(242, 242, 242)), rec2)
                g.DrawRectangle(Pens.White, rec2)
                g.DrawRectangle(New Pen(Color.FromArgb(70, Color.FromArgb(39, 93, 127)), 2), rec)
                g.FillRectangle(linear, New Rectangle(rec2.X + 113, rec2.Y + 1, 6, rec2.Height - 1))
                g.FillRectangle(linear3, New Rectangle(rec2.X, rec2.Y + 1, 6, rec2.Height - 1))


                g.FillEllipse(Brushes.LightGray, New Rectangle(rec2.X + 8, rec2.Y + 8, 12, 12))
                g.DrawEllipse(New Pen(Color.FromArgb(100, 100, 100), 1), New Rectangle(rec2.X + 8, rec2.Y + 8, 12, 12))
                g.SmoothingMode = SmoothingMode.None

            End If

            g.DrawString(TabPages(i).Text, Font, New SolidBrush(Color.FromArgb(56, 106, 137)), rec, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        Next




        e.Graphics.DrawImage(b.Clone, 0, 0)
        g.Dispose()
        b.Dispose()
        MyBase.OnPaint(e)
    End Sub
End Class
#End Region
#Region "SLCTextbox"
<DefaultEvent("TextChanged")> Class SLCTextBox : Inherits Control

#Region " Variables"
    Private TB As Windows.Forms.TextBox
    Private State As MouseState = MouseState.None
#End Region

#Region " Properties"
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : TB.Focus() : Invalidate()
    End Sub

    Protected Overrides Sub OnEnter(ByVal e As EventArgs)
        MyBase.OnEnter(e) : TB.Focus() : Invalidate()
    End Sub
    Protected Overrides Sub OnLeave(ByVal e As EventArgs)
        MyBase.OnLeave(e) : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e) : Invalidate()
    End Sub

    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If TB IsNot Nothing Then
                TB.TextAlign = value
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
            If TB IsNot Nothing Then
                TB.MaxLength = value
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
            If TB IsNot Nothing Then
                TB.ReadOnly = value
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
            If TB IsNot Nothing Then
                TB.UseSystemPasswordChar = value
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
            If TB IsNot Nothing Then
                TB.Multiline = value

                If value Then
                    TB.Height = Height - 11
                Else
                    Height = TB.Height + 11
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
            If TB IsNot Nothing Then
                TB.Text = value
            End If
        End Set
    End Property
    Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If TB IsNot Nothing Then
                TB.Font = value
                TB.Location = New Point(3, 5)
                TB.Width = Width - 6

                If Not _Multiline Then
                    Height = TB.Height + 11
                End If
            End If
        End Set
    End Property
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(TB) Then
            Controls.Add(TB)
        End If
    End Sub
    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
        Text = TB.Text
    End Sub
    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            TB.SelectAll()
            e.SuppressKeyPress = True
        End If
        If e.Control AndAlso e.KeyCode = Keys.C Then
            TB.Copy()
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        TB.Location = New Point(5, 5)
        TB.Width = Width - 10

        If _Multiline Then
            TB.Height = Height - 11
        Else
            Height = TB.Height + 11
        End If

        MyBase.OnResize(e)
    End Sub
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
              ControlStyles.UserPaint Or _
              ControlStyles.OptimizedDoubleBuffer Or _
              ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.Selectable, True)

        DoubleBuffered = True

        TB = New Windows.Forms.TextBox
        TB.Font = New Font("Tahoma", 8)
        TB.BackColor = Color.White
        TB.ForeColor = Color.FromArgb(1, 75, 124)
        TB.MaxLength = _MaxLength
        TB.Multiline = _Multiline
        TB.ReadOnly = _ReadOnly
        TB.UseSystemPasswordChar = _UseSystemPasswordChar
        TB.Borderstyle = BorderStyle.None
        TB.Location = New Point(5, 5)
        TB.Width = Width - 10

        TB.Cursor = Cursors.IBeam

        If _Multiline Then
            TB.Height = Height - 11
        Else
            Height = TB.Height + 11
        End If

        AddHandler TB.TextChanged, AddressOf OnBaseTextChanged
        AddHandler TB.KeyDown, AddressOf OnBaseKeyDown
    End Sub
    Public Function Borderpts() As Rectangle
        Dim P As New Rectangle


        P = New Rectangle(2, 2, Width - 5, Height - 5)
        Return P
    End Function

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)


        Dim PB1 As PathGradientBrush
        Dim GP1 As GraphicsPath = RoundRec(Borderpts, 2)





        With G
            .SmoothingMode = 2
            .TextRenderingHint = 1
            .Clear(Color.White)

            PB1 = New PathGradientBrush(GP1)
            PB1.CenterColor = Color.White
            PB1.SurroundColors = {Color.FromArgb(234, 234, 234)}
            PB1.FocusScales = New PointF(0.9F, 0.5F)



            .FillPath(PB1, GP1)
            .DrawPath(New Pen(Color.FromArgb(125, 125, 125)), GP1)


            MyBase.OnPaint(e)
            G.Dispose()
            e.Graphics.InterpolationMode = 7
            e.Graphics.DrawImageUnscaled(B, 0, 0)
            B.Dispose()
        End With
    End Sub

    Public Function RoundRec(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
End Class
#End Region
#Region "SLCRadioButton"
<DefaultEvent("CheckedChanged")> _
Class SLCRadionButton
    Inherits Control

    Event CheckedChanged(ByVal sender As Object)

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        BackColor = Color.White

        P1 = New Pen(Color.FromArgb(55, 55, 55))
        P2 = New Pen(Brushes.Red)
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
            If Not (C Is Me) AndAlso (TypeOf C Is SLCRadionButton) Then
                DirectCast(C, SLCRadionButton).Checked = False
            End If
        Next
    End Sub

    Private GP1 As GraphicsPath

    Private SZ1 As SizeF
    Private PT1 As PointF

    Private P1, P2 As Pen

    Private PB1 As PathGradientBrush

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics
        G = e.Graphics


        G.Clear(BackColor)
        G.SmoothingMode = SmoothingMode.AntiAlias

        GP1 = New GraphicsPath
        GP1.AddEllipse(0, 2, Height - 5, Height - 5)

        PB1 = New PathGradientBrush(GP1)
        PB1.CenterColor = Color.FromArgb(50, 50, 50)
        PB1.SurroundColors = {Color.FromArgb(45, 45, 45)}
        PB1.FocusScales = New PointF(0.3F, 0.3F)

        ' G.FillPath(PB1, GP1)

        G.DrawEllipse(P1, 4, 4, Height - 11, Height - 11)
        ' G.DrawEllipse(P2, 1, 3, Height - 7, Height - 7)

        If _Checked Then
            Dim GPF As New GraphicsPath
            GPF.AddEllipse(New Rectangle(Height - 18.5, Height - 19, 12, 12))
            Dim PB3 As PathGradientBrush
            PB3 = New PathGradientBrush(GPF)
            PB3.CenterPoint = New Point(Height - 18.5, Height - 20)
            PB3.CenterColor = Color.FromArgb(56, 142, 196)
            PB3.SurroundColors = {Color.FromArgb(64, 106, 140)}
            PB3.FocusScales = New PointF(0.9F, 0.9F)


            G.FillPath(PB3, GPF)

            G.DrawPath(New Pen(Color.FromArgb(49, 63, 86)), GPF)
            G.SetClip(GPF)
            G.FillEllipse(New SolidBrush(Color.FromArgb(40, Color.WhiteSmoke)), New Rectangle(Height - 16, Height - 18, 6, 6))
            G.ResetClip()
        End If

        SZ1 = G.MeasureString(Text, Font)
        PT1 = New PointF(Height - 3, Height \ 2 - SZ1.Height / 2)


        G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(1, 75, 124)), PT1)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        Checked = True
        MyBase.OnMouseDown(e)
    End Sub

End Class
#End Region
#Region "SLCComboBox"
Class SLCComboBox
    Inherits ComboBox

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)

        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DropDownstyle = ComboBoxStyle.DropDownList


        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed


    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G.Clear(Color.White)





        'inside borders



        Dim GP As GraphicsPath = RoundRec(New Rectangle(0, 0, Width - 1, Height - 1), 5)
        Dim GP2 As GraphicsPath = RoundRec(New Rectangle(1, 1, Width - 3, Height - 3), 5)
        Dim GP3 As GraphicsPath = RoundRec(New Rectangle(2, 2, Width - 5, Height - 5), 5)


        G.SmoothingMode = SmoothingMode.HighQuality

        G.FillPath(New SolidBrush(Color.FromArgb(250, 250, 250)), GP3)
        G.DrawPath(New Pen(Color.FromArgb(60, Color.LightGray), 4), GP2)
        G.DrawPath(New Pen(Color.FromArgb(100, Color.FromArgb(15, 15, 15))), GP)
        ' G.DrawPath(New Pen(Color.FromArgb(60, Color.LightGray), 4), GP3)
        G.SmoothingMode = SmoothingMode.None

        Dim rect1 As New Rectangle(Width - 26, 0, 1, Height)
        Dim rect2 As New Rectangle(Width - 27, 0, 2, Height)
        G.FillRectangle(New SolidBrush(Color.FromArgb(30, Color.FromArgb(1, 75, 124))), rect1)
        G.DrawRectangle(New Pen(Color.FromArgb(60, Color.FromArgb(61, 113, 153))), rect1)



        'little arrow shit

        G.SmoothingMode = SmoothingMode.HighQuality


        G.DrawArc(New Pen(Color.FromArgb(97, 152, 195)), New Rectangle(Width - 18, Height - 19, 8, 8), 20, 140)

        G.DrawArc(New Pen(Color.LightGray), New Rectangle(Width - 18, Height - 18, 8, 8), 10, 160)
        G.DrawArc(New Pen(Color.FromArgb(78, 121, 154), 1.5), New Rectangle(Width - 18, Height - 20, 8, 8), 20, 140)



        G.DrawArc(New Pen(Color.FromArgb(97, 152, 195)), New Rectangle(Width - 19, Height - 16, 10, 10), 20, 140)

        G.DrawArc(New Pen(Color.LightGray), New Rectangle(Width - 19, Height - 15, 10, 10), 10, 160)
        G.DrawArc(New Pen(Color.FromArgb(78, 121, 154), 1.5), New Rectangle(Width - 19, Height - 17, 10, 10), 20, 140)
        G.SmoothingMode = SmoothingMode.None


        Dim PT1 As New PointF(3, Height - 18)

        G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(1, 75, 124)), PT1)
    End Sub

    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)

        If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds)
        Else
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(250, 250, 250)), e.Bounds)
        End If

        If Not e.Index = -1 Then
            e.Graphics.DrawString(GetItemText(Items(e.Index)), e.Font, New SolidBrush(Color.FromArgb(1, 75, 124)), e.Bounds)
        End If
    End Sub

    Public Function RoundRec(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
End Class
#End Region
#Region "SLCProgrssBar"
Class SLCProgrssBar
    Inherits Control

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

    Private Sub Increment(ByVal amount As Integer)
        Value += amount
    End Sub

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)


        P2 = New Pen(Color.FromArgb(55, 55, 55))
        B1 = New SolidBrush(Color.FromArgb(0, 214, 37))
    End Sub

    Private GP1, GP2, GP3 As GraphicsPath

    Private R1, R2 As Rectangle

    Private P2 As Pen
    Private B1 As SolidBrush
    Private GB1, GB2 As LinearGradientBrush

    Private I1 As Integer

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim R3 As New Rectangle
        Dim HB As HatchBrush

        G.Clear(Color.White)
        G.SmoothingMode = SmoothingMode.HighQuality

        GP1 = RoundRec(New Rectangle(0, 0, Width - 1, Height - 1), 4)
        GP2 = RoundRec(New Rectangle(1, 1, Width - 3, Height - 3), 4)

        R1 = New Rectangle(0, 2, Width - 1, Height - 1)
        GB1 = New LinearGradientBrush(R1, Color.FromArgb(255, 255, 255), Color.FromArgb(230, 230, 230), 90.0F)





        G.SetClip(GP1)
        'gloss
        Dim PB As New PathGradientBrush(GP1)
        PB.CenterColor = Color.FromArgb(230, 230, 230)
        PB.SurroundColors = {Color.FromArgb(255, 255, 255)}
        PB.CenterPoint = New Point(0, Height)
        PB.FocusScales = New PointF(1, 0)

        G.FillRectangle(PB, R1)

        G.FillPath(New SolidBrush(Color.FromArgb(250, 250, 250)), RoundRec(New Rectangle(1, 1, Width - 3, Height / 2 - 2), 4))

        I1 = CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 3))

        If I1 > 1 Then





            GP3 = RoundRec(New Rectangle(1, 1, I1, Height - 3), 4)

            'grad
            HB = New HatchBrush(HatchStyle.Trellis, Color.FromArgb(50, Color.Black), Color.Transparent)



            'bar
            R2 = New Rectangle(1, 1, I1, Height - 3)
            GB2 = New LinearGradientBrush(R2, Color.FromArgb(20, 34, 45), Color.FromArgb(27, 84, 121), 90.0F)

            G.FillPath(GB2, GP3)
            G.DrawPath(New Pen(Color.FromArgb(120, 134, 145)), GP3)

            G.SetClip(GP3)
            G.SmoothingMode = SmoothingMode.None



            G.FillRectangle(New SolidBrush(Color.FromArgb(32, 100, 144)), R2.X, R2.Y + 1, R2.Width, R2.Height \ 2)

            R3 = New Rectangle(1, 1, I1, Height - 1)

            G.FillRectangle(HB, R3)

            G.SmoothingMode = SmoothingMode.AntiAlias
            G.ResetClip()
        End If



        G.DrawPath(New Pen(Color.FromArgb(125, 125, 125)), GP2)
        G.DrawPath(New Pen(Color.FromArgb(80, Color.LightGray)), GP1)
    End Sub
    Public Function RoundRec(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function

End Class
#End Region
#Region "SLCCheckbox"
<DefaultEvent("CheckedChanged")> _
Class SLCCheckbox
    Inherits Control

    Event CheckedChanged(ByVal sender As Object)

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)

        P11 = New Pen(Color.LightGray)
        P22 = New Pen(Color.FromArgb(35, 35, 35))
        P3 = New Pen(Color.Black, 2.0F)
        P4 = New Pen(Color.White, 2.0F)
    End Sub

    Private _Checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)

            Invalidate()
        End Set
    End Property

    Private GP1, GP2 As GraphicsPath

    Private SZ1 As SizeF
    Private PT1 As PointF

    Private P11, P22, P3, P4 As Pen

    Private PB1 As PathGradientBrush

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim g As Graphics = e.Graphics


        g.Clear(Color.White)
        g.SmoothingMode = SmoothingMode.AntiAlias

        GP1 = RoundRec(New Rectangle(0, 2, Height - 5, Height - 5), 1)
        GP2 = RoundRec(New Rectangle(1, 3, Height - 7, Height - 7), 1)

        Dim GPF As New GraphicsPath
        GPF.AddPath(RoundRec(New Rectangle(Height - 18.5, Height - 20, 14, 14), 2), True)
        Dim PB3 As PathGradientBrush
        PB3 = New PathGradientBrush(GPF)
        '  PB3.CenterPoint = New Point(Height - 18.5, Height - 21)
        PB3.CenterColor = Color.FromArgb(240, 240, 240)
        PB3.SurroundColors = {Color.FromArgb(90, 90, 90)}
        PB3.FocusScales = New PointF(0.4F, 0.4F)

        Dim hb As New HatchBrush(HatchStyle.Trellis, Color.FromArgb(70, Color.FromArgb(15, 54, 80)), Color.Transparent)

        g.FillPath(PB3, GPF)
        g.FillPath(hb, GPF)


        g.DrawPath(New Pen(Color.FromArgb(49, 63, 86)), GPF)
        g.SetClip(GPF)
        g.FillEllipse(New SolidBrush(Color.FromArgb(70, Color.DarkGray)), New Rectangle(Height - 16, Height - 18, 6, 6))
        g.DrawPath(Pens.White, RoundRec(New Rectangle(5, 4, Height - 11, Height - 11), 2))
        g.ResetClip()



        If _Checked Then

            'g.DrawLine(Pens.Red, New PointF(7, Height - 10), New PointF(Height - 10, 5))
            g.DrawLine(New Pen(New SolidBrush(Color.FromArgb(1, 75, 124)), 2), New Point(8, Height - 9), New Point(Height - 8, 6))


            g.DrawLine(New Pen(New SolidBrush(Color.FromArgb(1, 75, 124)), 2), 7, Height - 13, 8, Height - 10)


        End If

        SZ1 = g.MeasureString(Text, Font)
        PT1 = New PointF(Height - 3, Height \ 2 - SZ1.Height / 2)


        g.DrawString(Text, Font, New SolidBrush(Color.FromArgb(1, 75, 124)), PT1)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        Checked = Not Checked
    End Sub

    Public Function RoundRec(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function

End Class

#End Region
#Region "SLCListview"
Class SLCListView
    Inherits Control

    Class NSListViewItem
        Property Text As String
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        Property SubItems As New List(Of NSListViewSubItem)

        Protected UniqueId As Guid

        Sub New()
            UniqueId = Guid.NewGuid()
        End Sub

        Public Overrides Function ToString() As String
            Return Text
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is NSListViewItem Then
                Return (DirectCast(obj, NSListViewItem).UniqueId = UniqueId)
            End If

            Return False
        End Function


    End Class

    Class NSListViewSubItem
        Property Text As String

        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class

    Class NSListViewColumnHeader
        Property Text As String
        Property Width As Integer = 60

        Public Overrides Function ToString() As String
            Return Text
        End Function
    End Class

    Private _Items As New List(Of NSListViewItem)
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Property Items() As NSListViewItem()
        Get
            Return _Items.ToArray()
        End Get
        Set(ByVal value As NSListViewItem())
            _Items = New List(Of NSListViewItem)(value)
            InvalidateScroll()
        End Set
    End Property

    Private _SelectedItems As New List(Of NSListViewItem)
    Public ReadOnly Property SelectedItems() As NSListViewItem()
        Get
            Return _SelectedItems.ToArray()
        End Get
    End Property

    Private _Columns As New List(Of NSListViewColumnHeader)
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    Public Property Columns() As NSListViewColumnHeader()
        Get
            Return _Columns.ToArray()
        End Get
        Set(ByVal value As NSListViewColumnHeader())
            _Columns = New List(Of NSListViewColumnHeader)(value)
            InvalidateColumns()
        End Set
    End Property

    Private _MultiSelect As Boolean = True
    Public Property MultiSelect() As Boolean
        Get
            Return _MultiSelect
        End Get
        Set(ByVal value As Boolean)
            _MultiSelect = value

            If _SelectedItems.Count > 1 Then
                _SelectedItems.RemoveRange(1, _SelectedItems.Count - 1)
            End If

            Invalidate()
        End Set
    End Property

    Private ItemHeight As Integer = 24
    Public Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            ItemHeight = CInt(Graphics.FromHwnd(Handle).MeasureString("@", Font).Height) + 6

            If VS IsNot Nothing Then
                VS.SmallChange = ItemHeight
                VS.LargeChange = ItemHeight
            End If

            MyBase.Font = value
            InvalidateLayout()
        End Set
    End Property

#Region " Item Helper Methods "

    'Ok, you've seen everything of importance at this point; I am begging you to spare yourself. You must not read any further!

    Public Sub AddItem(ByVal text As String, ByVal ParamArray subItems As String())
        Dim Items As New List(Of NSListViewSubItem)
        For Each I As String In subItems
            Dim SubItem As New NSListViewSubItem()
            SubItem.Text = I
            Items.Add(SubItem)
        Next

        Dim Item As New NSListViewItem()
        Item.Text = text
        Item.SubItems = Items

        _Items.Add(Item)
        InvalidateScroll()
    End Sub

    Public Sub RemoveItemAt(ByVal index As Integer)
        _Items.RemoveAt(index)
        InvalidateScroll()
    End Sub

    Public Sub RemoveItem(ByVal item As NSListViewItem)
        _Items.Remove(item)
        InvalidateScroll()
    End Sub

    Public Sub RemoveItems(ByVal items As NSListViewItem())
        For Each I As NSListViewItem In items
            _Items.Remove(I)
        Next

        InvalidateScroll()
    End Sub

#End Region

    Private VS As SLCScrollBar

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, True)

        P1 = New Pen(Color.FromArgb(55, 55, 55))
        P2 = New Pen(Color.FromArgb(35, 35, 35))
        P3 = New Pen(Color.FromArgb(65, 65, 65))

        B1 = New SolidBrush(Color.FromArgb(62, 62, 62))
        B2 = New SolidBrush(Color.FromArgb(65, 65, 65))
        B3 = New SolidBrush(Color.FromArgb(47, 47, 47))
        B4 = New SolidBrush(Color.FromArgb(50, 50, 50))

        VS = New SLCScrollBar
        VS.SmallChange = ItemHeight
        VS.LargeChange = ItemHeight

        AddHandler VS.Scroll, AddressOf HandleScroll
        AddHandler VS.MouseDown, AddressOf VS_MouseDown
        Controls.Add(VS)

        InvalidateLayout()
    End Sub

    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        InvalidateLayout()
        MyBase.OnSizeChanged(e)
    End Sub

    Private Sub HandleScroll(ByVal sender As Object)
        Invalidate()
    End Sub

    Private Sub InvalidateScroll()
        VS.Maximum = (_Items.Count * ItemHeight)
        Invalidate()
    End Sub

    Private Sub InvalidateLayout()
        VS.Location = New Point(Width - VS.Width - 1, 1)
        VS.Size = New Size(18, Height - 2)

        Invalidate()
    End Sub

    Private ColumnOffsets As Integer()
    Private Sub InvalidateColumns()
        Dim Width As Integer = 3
        ColumnOffsets = New Integer(_Columns.Count - 1) {}

        For I As Integer = 0 To _Columns.Count - 1
            ColumnOffsets(I) = Width
            Width += Columns(I).Width
        Next

        Invalidate()
    End Sub

    Private Sub VS_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        Focus()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        Focus()

        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim Offset As Integer = CInt(VS.Percent * (VS.Maximum - (Height - (ItemHeight * 2))))
            Dim Index As Integer = ((e.Y + Offset - ItemHeight) \ ItemHeight)

            If Index > _Items.Count - 1 Then Index = -1

            If Not Index = -1 Then
                'TODO: Handle Shift key

                If ModifierKeys = Keys.Control AndAlso _MultiSelect Then
                    If _SelectedItems.Contains(_Items(Index)) Then
                        _SelectedItems.Remove(_Items(Index))
                    Else
                        _SelectedItems.Add(_Items(Index))
                    End If
                Else
                    _SelectedItems.Clear()
                    _SelectedItems.Add(_Items(Index))
                End If
            End If

            Invalidate()
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Private P1, P2, P3 As Pen
    Private B1, B2, B3, B4 As SolidBrush
    Private GB1 As LinearGradientBrush

    'I am so sorry you have to witness this. I tried warning you. ;.;

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G = e.Graphics

        G.Clear(Color.White)

        Dim X, Y As Integer
        Dim H As Single

        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(50, Color.LightGray))), 1, 1, Width - 3, Height - 3)

        Dim R1 As Rectangle
        Dim CI As NSListViewItem

        Dim Offset As Integer = CInt(VS.Percent * (VS.Maximum - (Height - (ItemHeight * 2))))

        Dim StartIndex As Integer
        If Offset = 0 Then StartIndex = 0 Else StartIndex = (Offset \ ItemHeight)

        Dim EndIndex As Integer = Math.Min(StartIndex + (Height \ ItemHeight), _Items.Count - 1)

        For I As Integer = StartIndex To EndIndex
            CI = Items(I)

            R1 = New Rectangle(0, ItemHeight + (I * ItemHeight) + 1 - Offset, Width, ItemHeight - 1)

            H = G.MeasureString(CI.Text, Font).Height
            Y = R1.Y + CInt((ItemHeight / 2) - (H / 2))

            If _SelectedItems.Contains(CI) Then
                If I Mod 2 = 0 Then
                    G.FillRectangle(New SolidBrush(Color.FromArgb(40, Color.LightGray)), R1)
                Else
                    G.FillRectangle(New SolidBrush(Color.FromArgb(40, Color.LightGray)), R1)
                End If
            Else
                If I Mod 2 = 0 Then
                    G.FillRectangle(Brushes.White, R1)
                Else
                    G.FillRectangle(Brushes.White, R1)
                End If
            End If

            G.DrawLine(Pens.LightGray, 0, R1.Bottom, Width, R1.Bottom)

            If Columns.Length > 0 Then
                R1.Width = Columns(0).Width
                G.SetClip(R1)
            End If

            'TODO: Ellipse text that overhangs seperators.
            G.DrawString(CI.Text, Font, New SolidBrush(Color.FromArgb(1, 75, 124)), 10, Y + 1)


            If CI.SubItems IsNot Nothing Then
                For I2 As Integer = 0 To Math.Min(CI.SubItems.Count, _Columns.Count) - 1
                    X = ColumnOffsets(I2 + 1) + 4

                    R1.X = X
                    R1.Width = Columns(I2).Width
                    G.SetClip(R1)

                    G.DrawString(CI.SubItems(I2).Text, Font, New SolidBrush(Color.FromArgb(1, 75, 124)), X + 1, Y + 1)

                Next
            End If

            G.ResetClip()
        Next

        R1 = New Rectangle(0, 0, Width, ItemHeight)

        GB1 = New LinearGradientBrush(R1, Color.FromArgb(255, 255, 255), Color.FromArgb(245, 245, 245), 90.0F)
        G.FillRectangle(GB1, R1)
        G.SetClip(R1)
        G.FillRectangle(Brushes.White, New Rectangle(0, 0, R1.Width - 1, R1.Height / 2 - 1))
        G.ResetClip()
        G.DrawRectangle(Pens.White, 1, 1, Width - 22, ItemHeight - 2)

        Dim LH As Integer = Math.Min(VS.Maximum + ItemHeight - Offset, Height)

        Dim CC As NSListViewColumnHeader
        For I As Integer = 0 To _Columns.Count - 1
            CC = Columns(I)

            H = G.MeasureString(CC.Text, Font).Height
            Y = CInt((ItemHeight / 2) - (H / 2))
            X = ColumnOffsets(I)

            G.DrawString(CC.Text, Font, New SolidBrush(Color.FromArgb(1, 75, 124)), X + 1, Y + 1)


            G.DrawLine(Pens.LightGray, X - 3, 0, X - 3, LH)
            G.DrawLine(Pens.White, X - 2, 0, X - 2, ItemHeight)
        Next

        G.DrawRectangle(Pens.LightGray, 0, 0, Width - 1, Height - 1)

        G.DrawLine(New Pen(New SolidBrush(Color.LightGray)), 0, ItemHeight, Width, ItemHeight)
        G.DrawLine(New Pen(Brushes.LightGray), VS.Location.X - 1, 0, VS.Location.X - 1, Height)

        G.FillRectangle(Brushes.White, Width - 19, 0, Width, Height)
    End Sub

    Protected Overrides Sub OnMouseWheel(ByVal e As MouseEventArgs)
        Dim Move As Integer = -((e.Delta * SystemInformation.MouseWheelScrollLines \ 120) * (ItemHeight \ 2))

        Dim Value As Integer = Math.Max(Math.Min(VS.Value + Move, VS.Maximum), VS.Minimum)
        VS.Value = Value

        MyBase.OnMouseWheel(e)
    End Sub

End Class

#End Region
#Region "SLCScrollBar"
<DefaultEvent("Scroll")> _
Class SLCScrollBar
    Inherits Control

    Event Scroll(ByVal sender As Object)

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

            InvalidateLayout()
        End Set
    End Property

    Private _Maximum As Integer = 100
    Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1

            _Maximum = value
            If value < _Value Then _Value = value
            If value < _Minimum Then _Minimum = value

            InvalidateLayout()
        End Set
    End Property

    Private _Value As Integer
    Property Value() As Integer
        Get
            If Not ShowThumb Then Return _Minimum
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value = _Value Then Return

            If value > _Maximum OrElse value < _Minimum Then
                Throw New Exception("Property value is not valid.")
            End If

            _Value = value
            InvalidatePosition()

            RaiseEvent Scroll(Me)
        End Set
    End Property

    Property _Percent As Double
    Public ReadOnly Property Percent As Double
        Get
            If Not ShowThumb Then Return 0
            Return GetProgress()
        End Get
    End Property

    Private _SmallChange As Integer = 1
    Public Property SmallChange() As Integer
        Get
            Return _SmallChange
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then
                Throw New Exception("Property value is not valid.")
            End If

            _SmallChange = value
        End Set
    End Property

    Private _LargeChange As Integer = 10
    Public Property LargeChange() As Integer
        Get
            Return _LargeChange
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then
                Throw New Exception("Property value is not valid.")
            End If

            _LargeChange = value
        End Set
    End Property

    Private ButtonSize As Integer = 16
    Private ThumbSize As Integer = 24 ' 14 minimum

    Private TSA As Rectangle
    Private BSA As Rectangle
    Private Shaft As Rectangle
    Private Thumb As Rectangle

    Private ShowThumb As Boolean
    Private ThumbDown As Boolean

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)

        Width = 18


    End Sub

    Private GP1, GP2, GP3, GP4 As GraphicsPath



    Dim I1 As Integer

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G = e.Graphics
        G.Clear(Color.FromArgb(255, 255, 255))

        GP1 = DrawArrow(4, 6, False)
        GP2 = DrawArrow(5, 7, False)

        G.FillPath(New SolidBrush(Color.LightGray), GP2)
        G.FillPath(New SolidBrush(Color.FromArgb(83, 123, 168)), GP1)

        GP3 = DrawArrow(4, Height - 11, True)
        GP4 = DrawArrow(5, Height - 10, True)

        G.FillPath(New SolidBrush(Color.LightGray), GP4)
        G.FillPath(New SolidBrush(Color.FromArgb(83, 123, 168)), GP3)

        If ShowThumb Then
            G.FillRectangle(New SolidBrush(Color.FromArgb(250, 250, 250)), Thumb)
            G.DrawRectangle(Pens.LightGray, Thumb)
            G.DrawRectangle(Pens.White, Thumb.X + 1, Thumb.Y + 1, Thumb.Width - 2, Thumb.Height - 2)

            Dim Y As Integer
            Dim LY As Integer = Thumb.Y + (Thumb.Height \ 2) - 3

            For I As Integer = 0 To 2
                Y = LY + (I * 3)

                G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(68, 95, 127))), Thumb.X + 5, Y, Thumb.Right - 5, Y)
                G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(50, Color.FromArgb(68, 95, 127)))), Thumb.X + 5, Y + 1, Thumb.Right - 5, Y + 1)
            Next
        End If
        G.SmoothingMode = SmoothingMode.HighQuality
        'G.DrawRectangle(New Pen(New SolidBrush(Color.Red)), 0, 0, Width - 1, Height - 1)
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(59, 122, 165))), RoundRec(New Rectangle(1, 1, Width - 3, Height - 3), 4))
        G.SmoothingMode = SmoothingMode.None
    End Sub

    Private Function DrawArrow(ByVal x As Integer, ByVal y As Integer, ByVal flip As Boolean) As GraphicsPath
        Dim GP As New GraphicsPath()

        Dim W As Integer = 9
        Dim H As Integer = 5

        If flip Then
            GP.AddLine(x + 1, y, x + W + 1, y)
            GP.AddLine(x + W, y, x + H, y + H - 1)
        Else
            GP.AddLine(x, y + H, x + W, y + H)
            GP.AddLine(x + W, y + H, x + H, y)
        End If

        GP.CloseFigure()
        Return GP
    End Function

    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        InvalidateLayout()
    End Sub

    Private Sub InvalidateLayout()
        TSA = New Rectangle(0, 0, Width, ButtonSize)
        BSA = New Rectangle(0, Height - ButtonSize, Width, ButtonSize)
        Shaft = New Rectangle(0, TSA.Bottom + 1, Width, Height - (ButtonSize * 2) - 1)

        ShowThumb = ((_Maximum - _Minimum) > Shaft.Height)

        If ShowThumb Then
            'ThumbSize = Math.Max(0, 14) 'TODO: Implement this.
            Thumb = New Rectangle(1, 0, Width - 3, ThumbSize)
        End If

        RaiseEvent Scroll(Me)
        InvalidatePosition()
    End Sub

    Private Sub InvalidatePosition()
        Thumb.Y = CInt(GetProgress() * (Shaft.Height - ThumbSize)) + TSA.Height
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left AndAlso ShowThumb Then
            If TSA.Contains(e.Location) Then
                I1 = _Value - _SmallChange
            ElseIf BSA.Contains(e.Location) Then
                I1 = _Value + _SmallChange
            Else
                If Thumb.Contains(e.Location) Then
                    ThumbDown = True
                    MyBase.OnMouseDown(e)
                    Return
                Else
                    If e.Y < Thumb.Y Then
                        I1 = _Value - _LargeChange
                    Else
                        I1 = _Value + _LargeChange
                    End If
                End If
            End If

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum)
            InvalidatePosition()
        End If

        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If ThumbDown AndAlso ShowThumb Then
            Dim ThumbPosition As Integer = e.Y - TSA.Height - (ThumbSize \ 2)
            Dim ThumbBounds As Integer = Shaft.Height - ThumbSize

            I1 = CInt((ThumbPosition / ThumbBounds) * (_Maximum - _Minimum)) + _Minimum

            Value = Math.Min(Math.Max(I1, _Minimum), _Maximum)
            InvalidatePosition()
        End If

        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        ThumbDown = False
        MyBase.OnMouseUp(e)
    End Sub

    Private Function GetProgress() As Double
        Return (_Value - _Minimum) / (_Maximum - _Minimum)
    End Function

    Public Function RoundRec(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
End Class
#End Region
#Region "SLCOnOffBox"
<DefaultEvent("CheckedChanged")> _
Class SLCOnOffBox
    Inherits Control

    Event CheckedChanged(ByVal sender As Object)

    Protected State As MouseState


    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)

        P1 = New Pen(Color.FromArgb(55, 55, 55))
        P2 = New Pen(Color.FromArgb(35, 35, 35))
        P3 = New Pen(Color.FromArgb(65, 65, 65))

        B1 = New SolidBrush(Color.FromArgb(35, 35, 35))
        B2 = New SolidBrush(Color.FromArgb(85, 85, 85))
        B3 = New SolidBrush(Color.FromArgb(65, 65, 65))
        B4 = New SolidBrush(Color.FromArgb(205, 150, 0))
        B5 = New SolidBrush(Color.FromArgb(40, 40, 40))

        SF1 = New StringFormat()
        SF1.LineAlignment = StringAlignment.Center
        SF1.Alignment = StringAlignment.Near

        SF2 = New StringFormat()
        SF2.LineAlignment = StringAlignment.Center
        SF2.Alignment = StringAlignment.Far

        Size = New Size(56, 24)
        MinimumSize = Size
        MaximumSize = Size
    End Sub

    Private _Checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)

            Invalidate()
        End Set
    End Property

    Private GP1, GP2, GP3, GP4 As GraphicsPath

    Private P1, P2, P3 As Pen
    Private B1, B2, B3, B4, B5 As SolidBrush

    Private PB1 As PathGradientBrush
    Private GB1 As LinearGradientBrush

    Private R1, R2, R3 As Rectangle
    Private SF1, SF2 As StringFormat

    Private Offset As Integer

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

        Dim G As Graphics = e.Graphics

        G.Clear(Color.White)
        G.SmoothingMode = SmoothingMode.AntiAlias

        GP1 = RoundRec(New Rectangle(0, 0, Width - 1, Height - 1), 7)
        GP2 = RoundRec(New Rectangle(1, 1, Width - 3, Height - 3), 7)

        PB1 = New PathGradientBrush(GP1)
        PB1.CenterColor = Color.FromArgb(250, 250, 250)
        PB1.SurroundColors = {Color.FromArgb(245, 245, 245)}
        PB1.FocusScales = New PointF(0.3F, 0.3F)

        G.FillPath(PB1, GP1)
        G.DrawPath(Pens.LightGray, GP1)
        G.DrawPath(Pens.White, GP2)

        R1 = New Rectangle(5, 0, Width - 10, Height + 2)
        R2 = New Rectangle(6, 1, Width - 10, Height + 2)

        R3 = New Rectangle(1, 1, (Width \ 2) - 1, Height - 3)



        If _Checked Then
            ' G.DrawString("On", Font, Brushes.Black, R2, SF1)
            G.DrawString("On", Font, New SolidBrush(Color.FromArgb(1, 75, 124)), R1, SF1)

            R3.X += (Width \ 2) - 1
        Else
            'G.DrawString("Off", Font, B1, R2, SF2)
            G.DrawString("Off", Font, New SolidBrush(Color.FromArgb(1, 75, 124)), R1, SF2)
        End If

        GP3 = RoundRec(R3, 7)
        GP4 = RoundRec(New Rectangle(R3.X + 1, R3.Y + 1, R3.Width - 2, R3.Height - 2), 7)

        GB1 = New LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 255, 255), Color.FromArgb(245, 245, 245), 90.0F)

        G.FillPath(GB1, GP3)
        G.DrawPath(Pens.LightGray, GP3)
        G.DrawPath(Pens.White, GP4)


        Offset = R3.X + (R3.Width \ 2) - 3

        For I As Integer = 0 To 1
            If _Checked Then
                'G.FillRectangle(Brushes.LightGray, Offset + (I * 5), 7, 2, Height - 14)
            Else
                ' G.FillRectangle(Brushes.LightGray, Offset + (I * 5), 7, 2, Height - 14)
            End If

            G.SmoothingMode = SmoothingMode.None

            If _Checked Then

                G.SmoothingMode = SmoothingMode.HighQuality

                Dim GPF As New GraphicsPath
                GPF.AddEllipse(New Rectangle(Width - 20, Height - 17, 10, 10))
                Dim PB3 As PathGradientBrush
                PB3 = New PathGradientBrush(GPF)
                PB3.CenterPoint = New Point(Height - 18.5, Height - 20)
                PB3.CenterColor = Color.FromArgb(53, 152, 74)
                PB3.SurroundColors = {Color.FromArgb(86, 216, 114)}
                PB3.FocusScales = New PointF(0.9F, 0.9F)


                G.FillPath(PB3, GPF)

                G.DrawPath(New Pen(Color.FromArgb(85, 200, 109)), GPF)
                G.SetClip(GPF)
                G.FillEllipse(New SolidBrush(Color.FromArgb(40, Color.WhiteSmoke)), New Rectangle(Width - 20, Height - 18, 6, 6))
                G.ResetClip()


                '  G.FillRectangle(New SolidBrush(Color.FromArgb(85, 158, 203)), Offset + (I * 5), 7, 2, Height - 14)
            Else
                G.SmoothingMode = SmoothingMode.HighQuality

                Dim GPF As New GraphicsPath
                GPF.AddEllipse(New Rectangle(Height - 15, Height - 17, 10, 10))
                Dim PB3 As PathGradientBrush
                PB3 = New PathGradientBrush(GPF)
                PB3.CenterPoint = New Point(Height - 18.5, Height - 20)
                PB3.CenterColor = Color.FromArgb(185, 65, 65)
                PB3.SurroundColors = {Color.Red}
                PB3.FocusScales = New PointF(0.9F, 0.9F)


                G.FillPath(PB3, GPF)

                G.DrawPath(New Pen(Color.FromArgb(152, 53, 53)), GPF)
                G.SetClip(GPF)
                G.FillEllipse(New SolidBrush(Color.FromArgb(40, Color.WhiteSmoke)), New Rectangle(Height - 16, Height - 18, 6, 6))
                G.ResetClip()



                '  G.FillRectangle(Brushes.LightGray, Offset + (I * 5), 7, 2, Height - 14)
            End If

            G.SmoothingMode = SmoothingMode.AntiAlias
        Next
    End Sub

    Public Function RoundRec(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        Checked = Not Checked
        MyBase.OnMouseDown(e)
    End Sub

End Class
#End Region
#Region "SLCGroupBox"
Class SLCGroupBox
    Inherits ContainerControl

    Private _DrawSeperator As Boolean
    Public Property DrawSeperator() As Boolean
        Get
            Return _DrawSeperator
        End Get
        Set(ByVal value As Boolean)
            _DrawSeperator = value
            Invalidate()
        End Set
    End Property

    Private _Title As String = "GroupBox"
    Public Property Title() As String
        Get
            Return _Title
        End Get
        Set(ByVal value As String)
            _Title = value
            Invalidate()
        End Set
    End Property

    Private _SubTitle As String = "Details"
    Public Property SubTitle() As String
        Get
            Return _SubTitle
        End Get
        Set(ByVal value As String)
            _SubTitle = value
            Invalidate()
        End Set
    End Property

    Private _TitleFont As Font
    Private _SubTitleFont As Font

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)

        _TitleFont = New Font("Verdana", 10.0F)
        _SubTitleFont = New Font("Verdana", 6.5F)


    End Sub

    Private GP1, GP2 As GraphicsPath

    Private PT1 As PointF
    Private SZ1, SZ2 As SizeF




    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G = e.Graphics


        G.Clear(Color.White)
        G.SmoothingMode = SmoothingMode.AntiAlias

        GP1 = RoundRec(New Rectangle(0, 0, Width - 1, Height - 1), 7)
        GP2 = RoundRec(New Rectangle(1, 1, Width - 3, Height - 3), 7)


        G.FillPath(New SolidBrush(Color.FromArgb(250, 250, 250)), GP2)
        G.SetClip(GP2)
        Dim PB As New PathGradientBrush(GP2)
        PB.CenterColor = Color.FromArgb(255, 255, 255)
        PB.SurroundColors = {Color.FromArgb(250, 250, 250)}
        PB.FocusScales = New PointF(0.95, 0.95)
        G.FillPath(PB, GP2)
        G.ResetClip()

        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(70, Color.LightGray))), GP1)
        G.DrawPath(Pens.Gray, GP2)

        SZ1 = G.MeasureString(_Title, _TitleFont, Width, StringFormat.GenericTypographic)
        SZ2 = G.MeasureString(_SubTitle, _SubTitleFont, Width, StringFormat.GenericTypographic)


        G.DrawString(_Title, _TitleFont, New SolidBrush(Color.FromArgb(1, 75, 124)), 5, 5)

        PT1 = New PointF(6.0F, SZ1.Height + 4.0F)


        G.DrawString(_SubTitle, _SubTitleFont, New SolidBrush(Color.Black), PT1.X, PT1.Y)


    End Sub

    Public Function RoundRec(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function

End Class
#End Region
#Region "SLCContextMenu"
Class SLCContextMenu
    Inherits ContextMenuStrip

    Sub New()
        Renderer = New ToolStripProfessionalRenderer(New SLCColorTable())
        ForeColor = Color.FromArgb(1, 75, 124)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.Clear(Color.White)
        MyBase.OnPaint(e)
    End Sub

End Class

#End Region
#Region "SCLCT"

Class SLCColorTable
    Inherits ProfessionalColorTable

    Private BackColor As Color = Color.White

    Public Overrides ReadOnly Property ButtonSelectedBorder() As Color
        Get
            Return BackColor
        End Get
    End Property

    Public Overrides ReadOnly Property CheckBackground() As Color
        Get
            Return BackColor
        End Get
    End Property

    Public Overrides ReadOnly Property CheckPressedBackground() As Color
        Get
            Return BackColor
        End Get
    End Property

    Public Overrides ReadOnly Property CheckSelectedBackground() As Color
        Get
            Return BackColor
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
        Get
            Return BackColor
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
        Get
            Return BackColor
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
        Get
            Return BackColor
        End Get
    End Property

    Public Overrides ReadOnly Property MenuBorder() As Color
        Get
            Return Color.FromArgb(1, 75, 124)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemBorder() As Color
        Get
            Return BackColor
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelected() As Color
        Get
            Return Color.FromArgb(50, Color.LightGray)
        End Get
    End Property

    Public Overrides ReadOnly Property SeparatorDark() As Color
        Get
            Return Color.FromArgb(35, 35, 35)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
        Get
            Return BackColor
        End Get
    End Property

End Class
#End Region
#Region "SLCLABEL"
Class SLCLabel
    Inherits Control
    Dim lb As New Windows.Forms.Label
    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)

        Font = New Font("Verdana", 8.0F, FontStyle.Regular)
        Size = New Size(39, 13)


    End Sub

    Private _text As String = "SLCLabel"
    Public Overrides Property Text As String
        Get
            Return _text
        End Get
        Set(ByVal value As String)
            _text = value
            Invalidate()
        End Set
    End Property


    Private PT1 As PointF
    Private SZ1 As SizeF

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G = e.Graphics

        G.Clear(Color.White)


        PT1 = New PointF(0, 0)

        G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(1, 75, 124)), PT1)

    End Sub

End Class
#End Region
#Region "SLCClose"
Class SLCClose
    Inherits ThemeControl154
    Dim LB As New Label
    Protected Overrides Sub ColorHook()

    End Sub

    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Size = New Size(20, 20)


    End Sub

    Protected Overrides Sub PaintHook()
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Color.White)
        G.FillRectangle(New SolidBrush(Color.FromArgb(239, 239, 239)), New Rectangle(-1, -1, Width + 1, Height + 1))



        Select Case State
            Case MouseState.None


                '// circle

                G.SmoothingMode = SmoothingMode.HighQuality

                Dim GPF As New GraphicsPath
                GPF.AddEllipse(New Rectangle(Width - 20, Height - 19, 15, 15))
                Dim PB3 As PathGradientBrush
                PB3 = New PathGradientBrush(GPF)
                PB3.CenterPoint = New Point(Height - 18.5, Height - 20)
                PB3.CenterColor = Color.FromArgb(193, 26, 26)
                PB3.SurroundColors = {Color.FromArgb(229, 110, 110)}
                PB3.FocusScales = New PointF(0.6F, 0.6F)


                G.FillPath(PB3, GPF)

                G.DrawPath(New Pen(Color.FromArgb(159, 41, 41)), GPF)
                G.SetClip(GPF)
                G.FillEllipse(New SolidBrush(Color.FromArgb(40, Color.WhiteSmoke)), New Rectangle(Width - 20, Height - 18, 6, 6))
                G.ResetClip()
            Case MouseState.Down
                '// circle

                G.SmoothingMode = SmoothingMode.HighQuality

                Dim GPF As New GraphicsPath
                GPF.AddEllipse(New Rectangle(Width - 20, Height - 19, 15, 15))
                Dim PB3 As PathGradientBrush
                PB3 = New PathGradientBrush(GPF)
                PB3.CenterPoint = New Point(Height - 18.5, Height - 20)
                PB3.CenterColor = Color.FromArgb(221, 32, 32)
                PB3.SurroundColors = {Color.FromArgb(229, 110, 110)}
                PB3.FocusScales = New PointF(0.6F, 0.6F)


                G.FillPath(PB3, GPF)

                G.DrawPath(New Pen(Color.White), GPF)
                G.SetClip(GPF)
                G.FillEllipse(New SolidBrush(Color.FromArgb(40, Color.WhiteSmoke)), New Rectangle(Width - 20, Height - 18, 6, 6))
                G.ResetClip()
            Case MouseState.Over
                '// circle

                G.SmoothingMode = SmoothingMode.HighQuality

                Dim GPF As New GraphicsPath
                GPF.AddEllipse(New Rectangle(Width - 20, Height - 19, 15, 15))
                Dim PB3 As PathGradientBrush
                PB3 = New PathGradientBrush(GPF)
                PB3.CenterPoint = New Point(Height - 18.5, Height - 20)
                PB3.CenterColor = Color.FromArgb(221, 32, 32)
                PB3.SurroundColors = {Color.FromArgb(229, 110, 110)}
                PB3.FocusScales = New PointF(0.6F, 0.6F)


                G.FillPath(PB3, GPF)

                G.DrawPath(New Pen(Color.FromArgb(159, 41, 41)), GPF)
                G.SetClip(GPF)
                G.FillEllipse(New SolidBrush(Color.FromArgb(40, Color.WhiteSmoke)), New Rectangle(Width - 20, Height - 18, 6, 6))
                G.ResetClip()
        End Select

    End Sub
End Class
#End Region