Option Strict On

Imports System, System.IO, System.Collections.Generic
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging
Imports System.Reflection

'########|CREDITS|##########
'# Theme: Blink            #
'# Themebase: Aeonhack     #
'###########################
'# Other: Mavamaarten~     #
'# Controls based on his   #
'###########################

'##########|INFO|###########
'# Theme: Influx           #
'# Creator: Blink          #
'# Version: 1.1            #
'# Created: 29/09/2012     #
'# Changed: 30/09/2012     #
'###########################


#Region "Themebase"
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
#End Region

Class InfluxThemeControl
    Inherits ThemeContainer154


    Sub New()
        'TransparencyKey = Color.Fuchsia
        BackColor = Color.FromArgb(77, 77, 77)
        Font = New Font("Verdana", 8S)
        MinimumSize = New Size(80, 55)
        SetColor("Border", Color.FromArgb(53, 53, 53))
        SetColor("Text", Color.FromArgb(229, 229, 229))
    End Sub

    Dim Border As Color
    Dim TextBrush As Brush
    Protected Overrides Sub ColorHook()
        Border = GetColor("Border")
        TextBrush = GetBrush("Text")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Border)
        'Draw form border
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(53, 53, 53))), New Rectangle(0, 0, Width - 1, Height - 1))
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(66, 66, 66))), New Rectangle(1, 1, Width - 3, Height - 3))
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(79, 79, 79)), 2), New Rectangle(3, 3, Width - 6, Height - 6))
        'Draw form content
        Dim HB As New HatchBrush(HatchStyle.Percent20, Color.FromArgb(83, 83, 83), BackColor)
        G.FillRectangle(HB, New Rectangle(4, 15, Width - 8, Height - 19))
        'Draw title bar
        Dim TitleTopGradient As New LinearGradientBrush(New Rectangle(4, 4, Width - 8, 11), Color.FromArgb(79, 79, 79), Color.FromArgb(87, 87, 87), 0)
        TitleTopGradient.SetBlendTriangularShape(0.5F, 1.0F)
        G.FillRectangle(TitleTopGradient, New Rectangle(4, 4, Width - 8, 11))
        Dim TitleBottomGradient As New LinearGradientBrush(New Rectangle(4, 15, Width - 8, 11), Color.FromArgb(150, 67, 67, 67), Color.FromArgb(150, 73, 73, 73), 0)
        TitleBottomGradient.SetBlendTriangularShape(0.5F, 1.0F)
        G.FillRectangle(TitleBottomGradient, New Rectangle(4, 15, Width - 8, 11))
        G.DrawString(FindForm.Text, Font, TextBrush, New Point(30, 7))
        G.DrawIcon(FindForm.Icon, New Rectangle(9, 6, 16, 16))
        'DrawCorners(Color.Fuchsia)
    End Sub
End Class

Class InfluxButton
    Inherits ThemeControl154

    Dim ButtonColor As Color
    Dim TextBrush As Brush
    Dim Border As Pen

    Sub New()
        Width = 120
        Height = 22
        SetColor("Button", Color.FromArgb(77, 77, 77))
        SetColor("Text", Color.FromArgb(229, 229, 229))
        SetColor("Border", Color.FromArgb(60, 60, 60))
    End Sub

    Protected Overrides Sub ColorHook()
        ButtonColor = GetColor("Button")
        TextBrush = GetBrush("Text")
        Border = GetPen("Border")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(ButtonColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim ButtonTop As New Rectangle(1, 1, Width - 2, CInt((Height / 2) - 1))
        Dim ButtonBottom As New Rectangle(1, CInt(Height / 2), Width - 2, CInt((Height / 2) - 1))
        Dim BorderPen As Pen = Nothing
        Dim TopGradient1, TopGradient2, Bottomgradient1, BottomGradient2 As Color
        Select Case State
            Case MouseState.None
                BorderPen = New Pen(New SolidBrush(Color.FromArgb(60, 60, 60)))
                TopGradient1 = Color.FromArgb(82, 82, 82)
                TopGradient2 = Color.FromArgb(78, 78, 78)
                Bottomgradient1 = Color.FromArgb(66, 66, 66)
                BottomGradient2 = Color.FromArgb(73, 73, 73)
            Case MouseState.Over
                BorderPen = New Pen(New SolidBrush(Color.FromArgb(62, 62, 62)))
                TopGradient1 = Color.FromArgb(93, 93, 93)
                TopGradient2 = Color.FromArgb(84, 84, 84)
                Bottomgradient1 = Color.FromArgb(71, 71, 71)
                BottomGradient2 = Color.FromArgb(77, 77, 77)
            Case MouseState.Down
                BorderPen = New Pen(New SolidBrush(Color.FromArgb(67, 67, 67)))
                TopGradient1 = Color.FromArgb(111, 111, 111)
                TopGradient2 = Color.FromArgb(101, 101, 101)
                Bottomgradient1 = Color.FromArgb(84, 84, 84)
                BottomGradient2 = Color.FromArgb(90, 90, 90)
        End Select
        Dim TopGradient As New LinearGradientBrush(ButtonTop, TopGradient1, TopGradient2, 90)
        G.FillRectangle(TopGradient, ButtonTop)
        Dim BottomGradient As New LinearGradientBrush(ButtonBottom, Bottomgradient1, BottomGradient2, 90)
        G.FillRectangle(BottomGradient, ButtonBottom)
        'Draw border
        G.DrawPath(BorderPen, CreateRound(New Rectangle(0, 0, Width - 1, Height - 1), 2))
        G.DrawPath(New Pen(Color.FromArgb(84, 84, 84)), CreateRound(New Rectangle(1, 1, Width - 3, Height - 3), 2))
        'Draw text
        DrawText(TextBrush, HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

<DefaultEvent("CheckedChanged")> _
Class InfluxRadioButton
    Inherits ThemeControl154
    Private X As Integer
    Private _Checked As Boolean

    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            InvalidateControls()
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnCreation()
        InvalidateControls()
    End Sub

    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is InfluxRadioButton Then
                DirectCast(C, InfluxRadioButton).Checked = False
            End If
        Next
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.X
        Invalidate()
    End Sub

    Dim TextBrush As Brush

    Protected Overrides Sub ColorHook()
        TextBrush = GetBrush("Text")
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Dim textSize As Integer
        textSize = CInt(Me.CreateGraphics.MeasureString(Text, Font).Width)
        Me.Width = 20 + textSize
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        'HatchBrush
        Dim HB As New HatchBrush(HatchStyle.Percent20, Color.FromArgb(83, 83, 83), BackColor)
        G.FillRectangle(HB, New Rectangle(0, 0, Width, Height))

        If _Checked Then
            If State = MouseState.Over And X <= 11 Then
                G.FillEllipse(New SolidBrush(Color.FromArgb(100, 100, 100)), New Rectangle(0, 3, 11, 11))
            Else
                G.FillEllipse(New SolidBrush(Color.FromArgb(95, 95, 95)), New Rectangle(0, 3, 11, 11))
            End If
            G.FillEllipse(New SolidBrush(Color.FromArgb(214, 214, 214)), New Rectangle(3, 6, 5, 5))
            G.DrawEllipse(New Pen(New SolidBrush(Color.FromArgb(68, 68, 68))), New Rectangle(0, 3, 11, 11))
        Else
            If State = MouseState.Over And X <= 11 Then
                G.FillEllipse(New SolidBrush(Color.FromArgb(94, 94, 94)), New Rectangle(0, 3, 11, 11))
                G.FillEllipse(New SolidBrush(Color.FromArgb(88, 88, 88)), New Rectangle(2, 5, 8, 9))
            Else
                G.FillEllipse(New SolidBrush(Color.FromArgb(89, 89, 89)), New Rectangle(0, 3, 11, 11))
                G.FillEllipse(New SolidBrush(Color.FromArgb(83, 83, 83)), New Rectangle(2, 5, 8, 9))
            End If
            G.DrawEllipse(New Pen(New SolidBrush(Color.FromArgb(68, 68, 68))), New Rectangle(0, 3, 11, 11))
        End If
        G.DrawString(Text, Font, TextBrush, New Point(17, 2))
    End Sub

    Public Sub New()
        Me.Size = New Size(50, 16)
        SetColor("Text", Color.FromArgb(229, 229, 229))
        BackColor = Color.FromArgb(77, 77, 77)
    End Sub
End Class

<DefaultEvent("CheckedChanged")> _
Class InfluxCheckbox
    Inherits ThemeControl154
    Private _Checked As Boolean
    Private X As Integer

    Event CheckedChanged(ByVal sender As Object)

    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Dim textSize As Integer
        textSize = CInt(Me.CreateGraphics.MeasureString(Text, Font).Width)
        Me.Width = 20 + textSize
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.X
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        _Checked = Not _Checked
    End Sub

    Dim CheckBoxColor As Color
    Dim TextBrush As Brush
    Protected Overrides Sub ColorHook()
        CheckBoxColor = GetColor("CheckBoxColor")
        TextBrush = GetBrush("Text")
    End Sub

    Dim CheckButtonBorder As New Rectangle(0, 3, 11, 11)
    Dim CheckPoints() As Point = {New Point(2, 7), New Point(5, 10), New Point(7, 6), New Point(10, 1)}
    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(77, 77, 77))
        G.SmoothingMode = SmoothingMode.HighQuality

        'HatchBrush
        Dim HB As New HatchBrush(HatchStyle.Percent20, Color.FromArgb(83, 83, 83), BackColor)
        G.FillRectangle(HB, New Rectangle(0, 0, Width, Height))

        If _Checked Then
            If State = MouseState.Over And X <= 11 Then
                Dim Gradient As New LinearGradientBrush(CheckButtonBorder, Color.FromArgb(108, 108, 108), Color.FromArgb(95, 95, 95), 90)
                G.FillRectangle(Gradient, CheckButtonBorder)
            Else
                Dim Gradient As New LinearGradientBrush(CheckButtonBorder, Color.FromArgb(103, 103, 103), Color.FromArgb(90, 90, 90), 90)
                G.FillRectangle(Gradient, CheckButtonBorder)
            End If
            G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(67, 67, 67))), CreateRound(CheckButtonBorder, 2))
            G.DrawLines(New Pen(TextBrush, 2), CheckPoints)
        Else
            G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(59, 59, 59))), CreateRound(CheckButtonBorder, 2))
            If State = MouseState.Over And X <= 11 Then
                Dim Gradient As New LinearGradientBrush(CheckButtonBorder, Color.FromArgb(95, 95, 95), Color.FromArgb(70, 70, 70), 90)
                G.FillRectangle(Gradient, CheckButtonBorder)
            Else
                Dim Gradient As New LinearGradientBrush(CheckButtonBorder, Color.FromArgb(90, 90, 90), Color.FromArgb(65, 65, 65), 90)
                G.FillRectangle(Gradient, CheckButtonBorder)
            End If
        End If
        G.DrawString(Text, Font, TextBrush, New Point(17, 2))
    End Sub

    Public Sub New()
        Me.Size = New Size(50, 16)
        BackColor = Color.FromArgb(77, 77, 77)
        SetColor("CheckBoxColor", Color.FromArgb(77, 77, 77))
        SetColor("Text", Color.FromArgb(229, 229, 229))
    End Sub
End Class

<DefaultEvent("TextChanged")> _
Class InfluxTextBox
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

        Width = 125

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


        SetColor("Text", Color.FromArgb(229, 229, 229))
        SetColor("TextBackColor", Color.FromArgb(73, 73, 73))
        SetColor("OuterBorder", Color.FromArgb(81, 81, 81))
        SetColor("InnerBorder", Color.FromArgb(60, 60, 60))
    End Sub

    Private TextBackColor As Color
    Private InnerBorder, OuterBorder As Pen

    Protected Overrides Sub ColorHook()
        TextBackColor = GetColor("TextBackColor")

        InnerBorder = GetPen("InnerBorder")
        OuterBorder = GetPen("OuterBorder")

        Base.ForeColor = GetColor("Text")
        Base.BackColor = GetColor("TextBackColor")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(77, 77, 77))

        G.SmoothingMode = SmoothingMode.HighQuality
        G.FillRectangle(New SolidBrush(TextBackColor), New Rectangle(1, 1, Width - 3, Height - 3))
        G.DrawPath(OuterBorder, CreateRound(New Rectangle(0, 0, Width - 1, Height - 1), 4))
        G.DrawPath(InnerBorder, CreateRound(New Rectangle(1, 1, Width - 3, Height - 3), 4))
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
        Base.Width = Width - 9

        If _Multiline Then
            Base.Height = Height - 10
        End If

        MyBase.OnResize(e)
    End Sub
End Class

Class InfluxTabControl
    Inherits TabControl

    Private _BG As Color
    Private _TabCloseButton As Boolean
    Private _BackgroundPattern As Boolean
    Public Overrides Property Backcolor As Color
        Get
            Return _BG
        End Get
        Set(ByVal value As Color)
            _BG = value
        End Set
    End Property

    Public Property TabCloseButton As Boolean
        Get
            Return _TabCloseButton
        End Get
        Set(ByVal value As Boolean)
            _TabCloseButton = value
            InvokePaint(Me, New PaintEventArgs(MyBase.CreateGraphics, MyBase.Bounds))
        End Set
    End Property

    Public Property BackgroundPattern As Boolean
        Get
            Return _BackgroundPattern
        End Get
        Set(ByVal value As Boolean)
            _BackgroundPattern = value
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DoubleBuffered = True
        Backcolor = Color.FromArgb(77, 77, 77)
        _TabCloseButton = False
        _BackgroundPattern = True
        Try
            SelectedTab = TabPages(0)
        Catch : End Try
        InvokePaint(Me, New PaintEventArgs(MyBase.CreateGraphics, MyBase.Bounds))
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub

    Function ToPen(ByVal color As Color) As Pen
        Return New Pen(color)
    End Function

    Function ToBrush(ByVal color As Color) As Brush
        Return New SolidBrush(color)
    End Function

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        If _TabCloseButton And TabCount > 1 Then
            Dim intOldIndex As Integer = intHoveredIndex
            intHoveredIndex = GetHoveredCloseButtonIndex(e.Location)
            If intHoveredIndex <> intOldIndex Then
                'Repaint
                InvokePaint(Me, New PaintEventArgs(MyBase.CreateGraphics, MyBase.Bounds))
            End If
        End If
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseClick(ByVal e As System.Windows.Forms.MouseEventArgs)
        If _TabCloseButton And TabCount > 1 Then
            Dim intHoveredIndex As Integer = GetHoveredCloseButtonIndex(e.Location)
            If intHoveredIndex <> -1 Then
                TabPages.RemoveAt(intHoveredIndex)
            End If
        End If
        MyBase.OnMouseClick(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        InvokePaint(Me, New PaintEventArgs(MyBase.CreateGraphics, MyBase.Bounds))
        MyBase.OnMouseDown(e)
    End Sub

    Private Function GetCloseButtonPositionByIndex(ByVal intIndex As Integer) As Point
        Return GetCloseButtonBoundsByIndex(intIndex).Location
    End Function

    Private Function GetCloseButtonBoundsByIndex(ByVal intTabIndex As Integer) As Rectangle
        Dim TabTopBounds As Rectangle = GetTabRect(intTabIndex)
        Return New Rectangle(TabTopBounds.X + TabTopBounds.Width - 1 - 7, 3, 7, 7)
    End Function

    Private Function MouseIsOverCloseButton(ByVal intTabIndex As Integer, ByVal pntMouseLocation As Point) As Boolean
        Return GetCloseButtonBoundsByIndex(intTabIndex).Contains(pntMouseLocation)
    End Function

    Private Function GetHoveredCloseButtonIndex(ByVal pntMouseLocation As Point) As Integer
        For i As Integer = 0 To TabCount - 1
            If MouseIsOverCloseButton(i, pntMouseLocation) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Dim intHoveredIndex As Integer = -1
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        If Not _BackgroundPattern Then Try : SelectedTab.BackColor = Backcolor : Catch : End Try
        G.Clear(Backcolor)

        'HatchBrush
        Dim HB_Container As New HatchBrush(HatchStyle.Percent20, Color.FromArgb(83, 83, 83), Backcolor)
        G.FillRectangle(HB_Container, New Rectangle(0, 0, Width, Height))
        'Selected panel hatchbrush
        If Not SelectedTab Is Nothing And _BackgroundPattern Then
            Dim ST_B As New Bitmap(SelectedTab.Width, SelectedTab.Height)
            Dim ST_G As Graphics = Graphics.FromImage(ST_B)
            ST_G.FillRectangle(HB_Container, New Rectangle(0, 0, SelectedTab.Width, SelectedTab.Height))
            SelectedTab.CreateGraphics.DrawImage(ST_B, 0, 0)
        End If
        'Draw panel borders
        Dim TopHorizontal As New Rectangle(3, 24, Width - 6, 1)
        Dim BottomHorizontal As New Rectangle(3, Height - 4, Width - 6, 1)
        Dim LeftVertical As New Rectangle(3, 24, 1, Height - 27)
        Dim RightVertical As New Rectangle(Width - 4, 24, 1, Height - 27)
        G.FillRectangle(New SolidBrush(Color.FromArgb(65, 65, 65)), TopHorizontal)
        G.FillRectangle(New SolidBrush(Color.FromArgb(65, 65, 65)), BottomHorizontal)
        G.FillRectangle(New SolidBrush(Color.FromArgb(65, 65, 65)), LeftVertical)
        G.FillRectangle(New SolidBrush(Color.FromArgb(65, 65, 65)), RightVertical)

        For i = 0 To TabCount - 1
            Dim TabTopBounds As Rectangle = GetTabRect(i)
            If i = SelectedIndex Then
                If i = 0 Then
                    'Gradient
                    TabTopBounds = New Rectangle(TabTopBounds.X + 1, TabTopBounds.Y + 1, TabTopBounds.Width - 1, TabTopBounds.Height)
                    Dim GradientBounds As New Rectangle(TabTopBounds.X + 1, TabTopBounds.Y, TabTopBounds.Width - 2, TabTopBounds.Height - 1)
                    Dim SelectedGradient As New LinearGradientBrush(GradientBounds, Color.FromArgb(100, 111, 111, 111), Color.Transparent, 90)
                    G.FillRectangle(SelectedGradient, GradientBounds)
                    'Tab top border
                    G.DrawPath(ToPen(Color.FromArgb(65, 65, 65)), RoundRect(TabTopBounds, 2))
                    Dim BottomLine As New Rectangle(TabTopBounds.X + 1, TabTopBounds.Y + TabTopBounds.Height - 1, TabTopBounds.Width, 1)
                    G.DrawRectangle(ToPen(Color.FromArgb(77, 77, 77)), BottomLine)
                Else
                    'Gradient
                    TabTopBounds = New Rectangle(TabTopBounds.X, TabTopBounds.Y + 1, TabTopBounds.Width, TabTopBounds.Height)
                    Dim GradientBounds As New Rectangle(TabTopBounds.X + 1, TabTopBounds.Y, TabTopBounds.Width - 2, TabTopBounds.Height - 1)
                    Dim SelectedGradient As New LinearGradientBrush(GradientBounds, Color.FromArgb(100, 111, 111, 111), Color.Transparent, 90)
                    G.FillRectangle(SelectedGradient, GradientBounds)
                    'Top border
                    G.DrawPath(ToPen(Color.FromArgb(65, 65, 65)), RoundRect(TabTopBounds, 2))
                    Dim BottomLine As New Rectangle(TabTopBounds.X + 1, TabTopBounds.Y + TabTopBounds.Height - 1, TabTopBounds.Width - 2, 1)
                    G.DrawRectangle(ToPen(Color.FromArgb(77, 77, 77)), BottomLine)
                End If
                If _TabCloseButton And TabCount > 1 Then
                    If intHoveredIndex = i Then
                        G.DrawString("x", New Font("Segoe UI", 6), New SolidBrush(Color.White), New Point((TabTopBounds.X + TabTopBounds.Width - 1) - 5))
                    Else
                        G.DrawString("x", New Font("Segoe UI", 6), New SolidBrush(Color.FromArgb(200, 200, 200)), New Point((TabTopBounds.X + TabTopBounds.Width - 1) - 5))
                    End If
                End If
            Else
                If i = 0 Then
                    TabTopBounds = New Rectangle(TabTopBounds.X + 1, TabTopBounds.Y + 1, TabTopBounds.Width - 1, TabTopBounds.Height)
                    G.DrawPath(ToPen(Color.FromArgb(65, 65, 65)), RoundRect(TabTopBounds, 2))
                Else
                    TabTopBounds = New Rectangle(TabTopBounds.X, TabTopBounds.Y + 1, TabTopBounds.Width, TabTopBounds.Height)
                    G.DrawPath(ToPen(Color.FromArgb(65, 65, 65)), RoundRect(TabTopBounds, 2))
                End If
                If _TabCloseButton And TabCount > 1 Then
                    If intHoveredIndex = i Then
                        G.DrawString("x", New Font("Segoe UI", 6), New SolidBrush(Color.White), New Point((TabTopBounds.X + TabTopBounds.Width - 1) - 5))
                    Else
                        G.DrawString("x", New Font("Segoe UI", 6), New SolidBrush(Color.FromArgb(150, 150, 150)), New Point((TabTopBounds.X + TabTopBounds.Width - 1) - 5))
                    End If
                End If
            End If
            If _TabCloseButton Then
                G.DrawString(TabPages(i).Text, Font, ToBrush(Color.FromArgb(229, 229, 229)), New Point(CInt(TabTopBounds.X + (TabTopBounds.Width / 2)) - 2, CInt(TabTopBounds.Y + (TabTopBounds.Height / 2))), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            Else
                G.DrawString(TabPages(i).Text, Font, ToBrush(Color.FromArgb(229, 229, 229)), New Point(CInt(TabTopBounds.X + (TabTopBounds.Width / 2)), CInt(TabTopBounds.Y + (TabTopBounds.Height / 2))), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            End If

        Next

        e.Graphics.DrawImage(CType(B.Clone, Image), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
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

Class InfluxListbox
    Inherits ListBox

    Sub New()
        SetStyle(ControlStyles.DoubleBuffer, True)
        Font = New Font("Verdana", 8S)
        BorderStyle = Windows.Forms.BorderStyle.None
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        ItemHeight = 22
        ForeColor = Color.Black
        BackColor = Color.FromArgb(65, 65, 65)
        IntegralHeight = False
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
        If m.Msg = 15 Then CustomPaint()
    End Sub

    Private _Image As Image
    Public Property ItemImage As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            _Image = value
        End Set
    End Property

    Protected Overrides Sub OnDrawItem(ByVal e As System.Windows.Forms.DrawItemEventArgs)
        Try
            If e.Index >= 0 Then
                Dim ItemBounds As Rectangle = e.Bounds
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 255)), ItemBounds)
                If InStr(e.State.ToString, "Selected,") > 0 Then
                    'Draw item backcolor
                    If IsEven(e.Index) Then
                        e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(75, 75, 75)), ItemBounds)
                    Else
                        e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(72, 72, 72)), ItemBounds)
                    End If
                    Dim ItemBoundsTop As New Rectangle(ItemBounds.X + 2, ItemBounds.Y + 1, ItemBounds.Width - 5, CInt(ItemBounds.Height / 2) - 1)
                    Dim ItemBoundsBottom As New Rectangle(ItemBounds.X + 2, ItemBounds.Y + CInt(ItemBounds.Height / 2), ItemBounds.Width - 5, CInt(ItemBounds.Height / 2))
                    Dim TopGradient As New LinearGradientBrush(ItemBoundsTop, Color.FromArgb(115, 115, 115), Color.FromArgb(120, 120, 120), 90)
                    e.Graphics.FillRectangle(TopGradient, ItemBoundsTop)
                    Dim BottomGradient As New LinearGradientBrush(ItemBoundsBottom, Color.FromArgb(90, 90, 90), Color.FromArgb(85, 85, 85), 90)
                    e.Graphics.FillRectangle(BottomGradient, ItemBoundsBottom)
                    'Draw selected item bounds
                    Dim SelectedRectangle As New Rectangle(ItemBounds.X + 2, ItemBounds.Y, ItemBounds.Width - 5, ItemBounds.Height - 1)
                    e.Graphics.DrawRectangle(New Pen(Color.FromArgb(60, 60, 60)), SelectedRectangle)
                    'Draw text
                    e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, New SolidBrush(Color.FromArgb(229, 229, 229)), 5, CInt(e.Bounds.Y + (e.Bounds.Height / 2) - 7))
                Else
                    'Draw item backcolor
                    If IsEven(e.Index) Then
                        e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(77, 77, 77)), ItemBounds)
                    Else
                        e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(73, 73, 73)), ItemBounds)
                    End If
                    'Draw item border
                    e.Graphics.DrawLine(New Pen(Color.FromArgb(65, 65, 65)), ItemBounds.X, ItemBounds.Y, ItemBounds.X + ItemBounds.Width - 1, ItemBounds.Y)
                    'Draw text
                    e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, New SolidBrush(Color.FromArgb(229, 229, 229)), 5, CInt(e.Bounds.Y + (e.Bounds.Height / 2) - 7))
                End If
            End If
            'Draw borders
            e.Graphics.DrawRectangle(New Pen(Color.FromArgb(65, 65, 65)), New Rectangle(0, 0, Width - 1, Height - 1))
            e.Graphics.DrawRectangle(New Pen(Color.FromArgb(80, 80, 80)), New Rectangle(1, 1, Width - 3, Height - 3))
            MyBase.OnDrawItem(e)
        Catch ex As Exception : End Try
    End Sub

    Private Function IsEven(ByVal intNumber As Integer) As Boolean
        Return (intNumber Mod 2 = 0)
    End Function

    Sub CustomPaint()
        CreateGraphics.DrawRectangle(New Pen(Color.FromArgb(65, 65, 65)), New Rectangle(0, 0, Width - 1, Height - 1))
        CreateGraphics.DrawRectangle(New Pen(Color.FromArgb(80, 80, 80)), New Rectangle(1, 1, Width - 3, Height - 3))
    End Sub
End Class

Class InfluxComboBox
    Inherits ComboBox
    Private X As Integer
    Private Over As Boolean

    Sub New()
        MyBase.New()
        Font = New Font("Verdana", 8S)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        ItemHeight = 18
        DropDownStyle = ComboBoxStyle.DropDownList
        BackColor = Color.FromArgb(77, 77, 77)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        Over = True
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        Over = False
        Invalidate()
    End Sub

    Dim TextBrush As New SolidBrush(Color.FromArgb(229, 229, 229))
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Not DropDownStyle = ComboBoxStyle.DropDownList Then DropDownStyle = ComboBoxStyle.DropDownList
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.Clear(Color.FromArgb(77, 77, 77))

        'Draw background
        If Over Then
            Dim TopRectangle As New Rectangle(2, 2, Width - 4, CInt(Height / 2))
            Dim BottomRectangle As New Rectangle(2, CInt(Height / 2), Width - 4, CInt(Height / 2))
            Dim TopGradient As New LinearGradientBrush(TopRectangle, Color.FromArgb(78, 78, 78), Color.FromArgb(82, 82, 82), 90)
            G.FillRectangle(TopGradient, TopRectangle)
            Dim BottomGradient As New LinearGradientBrush(BottomRectangle, Color.FromArgb(70, 70, 70), Color.FromArgb(75, 75, 75), 90)
            G.FillRectangle(BottomGradient, BottomRectangle)
        Else
            Dim TopRectangle As New Rectangle(2, 2, Width - 4, CInt(Height / 2))
            Dim BottomRectangle As New Rectangle(2, CInt(Height / 2), Width - 4, CInt(Height / 2))
            Dim TopGradient As New LinearGradientBrush(TopRectangle, Color.FromArgb(73, 73, 73), Color.FromArgb(74, 74, 74), 90)
            G.FillRectangle(TopGradient, TopRectangle)
            Dim BottomGradient As New LinearGradientBrush(BottomRectangle, Color.FromArgb(68, 68, 68), Color.FromArgb(72, 72, 72), 90)
            G.FillRectangle(BottomGradient, BottomRectangle)
        End If

        'Draw border
        Dim p As New Pen(Color.FromArgb(60, 60, 60))
        G.DrawPath(p, RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
        G.DrawPath(New Pen(Color.FromArgb(84, 84, 84)), RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 2))

        'Draw dropdown triangle
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim pntCheckPointOne As New Point(Width - 16, CInt(Height / 2) - 3)
        Dim CheckPoints() As Point = {pntCheckPointOne, New Point(pntCheckPointOne.X + 4, pntCheckPointOne.Y + 4), New Point(pntCheckPointOne.X + 8, pntCheckPointOne.Y)}
        If X >= Width - 23 Then
            'Hover triangle
            G.DrawLines(New Pen(Color.White, 2), CheckPoints)
        Else
            G.DrawLines(New Pen(Color.FromArgb(220, 220, 220), 2), CheckPoints)
        End If

        'Draw text
        Dim S1 As Integer = CInt(G.MeasureString(" ... ", Font).Height)
        If SelectedIndex <> -1 Then
            G.DrawString(Items(SelectedIndex).ToString, Font, TextBrush, 4, CInt(Height \ 2 - S1 \ 2))
        Else
            If Not Items Is Nothing And Items.Count > 0 Then
                G.DrawString(Items(0).ToString, Font, TextBrush, 4, Height \ 2 - S1 \ 2)
            Else
                G.DrawString(" ... ", Font, TextBrush, 4, Height \ 2 - S1 \ 2)
            End If
        End If

        e.Graphics.DrawImage(CType(B.Clone, Image), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)
        If e.Index >= 0 Then
            Dim ItemBounds As Rectangle = e.Bounds
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 255)), ItemBounds)
            If e.State = 785 Or e.State = 17 Then
                'Draw item backcolor
                If IsEven(e.Index) Then
                    e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(75, 75, 75)), ItemBounds)
                Else
                    e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(72, 72, 72)), ItemBounds)
                End If
                Dim ItemBoundsTop As New Rectangle(ItemBounds.X + 2, ItemBounds.Y + 1, ItemBounds.Width - 5, CInt(ItemBounds.Height / 2) - 1)
                Dim ItemBoundsBottom As New Rectangle(ItemBounds.X + 2, ItemBounds.Y + CInt(ItemBounds.Height / 2), ItemBounds.Width - 5, CInt(ItemBounds.Height / 2))
                Dim TopGradient As New LinearGradientBrush(ItemBoundsTop, Color.FromArgb(150, 115, 115, 115), Color.FromArgb(150, 120, 120, 120), 90)
                e.Graphics.FillRectangle(TopGradient, ItemBoundsTop)
                Dim BottomGradient As New LinearGradientBrush(ItemBoundsBottom, Color.FromArgb(150, 90, 90, 90), Color.FromArgb(150, 85, 85, 85), 90)
                e.Graphics.FillRectangle(BottomGradient, ItemBoundsBottom)
                'Draw selected item bounds
                Dim SelectedRectangle As New Rectangle(ItemBounds.X + 2, ItemBounds.Y, ItemBounds.Width - 5, ItemBounds.Height - 1)
                e.Graphics.DrawRectangle(New Pen(Color.FromArgb(200, 60, 60, 60)), SelectedRectangle)
                'Draw text
                e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, New SolidBrush(Color.FromArgb(229, 229, 229)), 5, CInt(e.Bounds.Y + (e.Bounds.Height / 2) - 8))
            Else
                'Draw item backcolor
                If IsEven(e.Index) Then
                    e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(77, 77, 77)), ItemBounds)
                Else
                    e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(73, 73, 73)), ItemBounds)
                End If
                'Draw item border
                e.Graphics.DrawLine(New Pen(Color.FromArgb(65, 65, 65)), ItemBounds.X, ItemBounds.Y, ItemBounds.X + ItemBounds.Width - 1, ItemBounds.Y)
                'Draw text
                e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, New SolidBrush(Color.FromArgb(229, 229, 229)), 5, CInt(e.Bounds.Y + (e.Bounds.Height / 2) - 8))
            End If
        End If
        MyBase.OnDrawItem(e)
    End Sub

    Private Sub InfluxComboBox_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DropDownClosed
        DropDownStyle = ComboBoxStyle.Simple
        Application.DoEvents()
        DropDownStyle = ComboBoxStyle.DropDownList
        Invalidate()
    End Sub

    Private Sub InfluxComboBox_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.TextChanged
        Invalidate()
    End Sub

    Private Function IsEven(ByVal intNumber As Integer) As Boolean
        Return (intNumber Mod 2 = 0)
    End Function

    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
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

Class InfluxControlBox
    Inherits ThemeControl154
    Private _Min As Boolean = True
    Private _Max As Boolean = True
    Private X As Integer

    Protected Overrides Sub ColorHook()
    End Sub

    Public Property MinButton As Boolean
        Get
            Return _Min
        End Get
        Set(ByVal value As Boolean)
            _Min = value
            Dim tempwidth As Integer = 40
            If _Min Then tempwidth += 25
            If _Max Then tempwidth += 25
            Me.Width = tempwidth + 1
            Me.Height = 16
            Invalidate()
        End Set
    End Property

    Public Property MaxButton As Boolean
        Get
            Return _Max
        End Get
        Set(ByVal value As Boolean)
            _Max = value
            Dim tempwidth As Integer = 40
            If _Min Then tempwidth += 25
            If _Max Then tempwidth += 25
            Me.Width = tempwidth + 1
            Me.Height = 16
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        Size = New Size(92, 16)
        Me.Location = New Point(100, 7)
        Me.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Transparent = True
        Me.BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub OnMove(ByVal e As System.EventArgs)
        MyBase.OnMove(e)
        Me.Top = 7
    End Sub


    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        MyBase.OnClick(e)
        If _Min And _Max Then
            If X > 0 And X < 25 Then
                FindForm.WindowState = FormWindowState.Minimized
            ElseIf X > 25 And X < 50 Then
                If FindForm.WindowState = FormWindowState.Maximized Then FindForm.WindowState = FormWindowState.Normal Else FindForm.WindowState = FormWindowState.Maximized
            ElseIf X > 50 And X < 90 Then
                FindForm.Close()
            End If
        ElseIf _Min Then
            If X > 0 And X < 25 Then
                FindForm.WindowState = FormWindowState.Minimized
            ElseIf X > 25 And X < 65 Then
                FindForm.Close()
            End If
        ElseIf _Max Then
            If X > 0 And X < 25 Then
                If FindForm.WindowState = FormWindowState.Maximized Then FindForm.WindowState = FormWindowState.Normal Else FindForm.WindowState = FormWindowState.Maximized
            ElseIf X > 25 And X < 65 Then
                FindForm.Close()
            End If
        Else
            If X > 0 And X < 40 Then
                FindForm.Close()
            End If
        End If
    End Sub

    Dim NormalBrush As New SolidBrush(Color.FromArgb(200, 200, 200))
    Dim OverBrush As New SolidBrush(Color.White)
    Dim ControlFont As New Font("Marlett", 8)
    Protected Overrides Sub PaintHook()
        G.Clear(Color.Transparent)
        If _Min And _Max Then
            G.DrawString("r", ControlFont, NormalBrush, New Point(63, 2))
            If FindForm.WindowState = FormWindowState.Normal Then
                G.DrawString("1", ControlFont, NormalBrush, New Point(32, 2))
            Else
                G.DrawString("2", ControlFont, NormalBrush, New Point(32, 2))
            End If
            G.DrawString("0", ControlFont, NormalBrush, New Point(6, 2))
            If State = MouseState.Over Then
                If X > 0 And X < 25 Then
                    'mouse over close
                    G.DrawString("0", ControlFont, OverBrush, New Point(6, 2))
                End If
                If X > 25 And X < 50 Then
                    'mouse over maximize
                    If FindForm.WindowState = FormWindowState.Normal Then
                        G.DrawString("1", ControlFont, OverBrush, New Point(32, 2))
                    Else
                        G.DrawString("2", ControlFont, OverBrush, New Point(32, 2))
                    End If
                End If
                If X > 50 And X < 90 Then
                    'mouse over minimize
                    G.DrawString("r", ControlFont, OverBrush, New Point(63, 2))
                End If
            End If
        ElseIf _Min Then
            G.DrawString("0", ControlFont, NormalBrush, New Point(6, 2))
            G.DrawString("r", ControlFont, NormalBrush, New Point(38, 2))
            If State = MouseState.Over Then
                If X > 0 And X < 25 Then
                    'mouse over close
                    G.DrawString("0", ControlFont, OverBrush, New Point(6, 2))
                End If
                If X > 25 And X < 65 Then
                    'mouse over minimize
                    G.DrawString("r", ControlFont, OverBrush, New Point(38, 2))
                End If
            End If
        ElseIf _Max Then
            If FindForm.WindowState = FormWindowState.Normal Then
                G.DrawString("1", ControlFont, NormalBrush, New Point(6, 2))
            Else
                G.DrawString("2", ControlFont, NormalBrush, New Point(6, 2))
            End If
            G.DrawString("r", ControlFont, NormalBrush, New Point(38, 2))
            If State = MouseState.Over Then
                If X > 0 And X < 25 Then
                    'mouse over maximize
                    If FindForm.WindowState = FormWindowState.Normal Then
                        G.DrawString("1", ControlFont, OverBrush, New Point(6, 2))
                    Else
                        G.DrawString("2", ControlFont, OverBrush, New Point(6, 2))
                    End If
                End If
                If X > 25 And X < 65 Then
                    'Mouse over close
                    G.DrawString("r", ControlFont, OverBrush, New Point(38, 2))
                End If
            End If
        Else
            G.DrawString("r", ControlFont, NormalBrush, New Point(13, 2))
            If State = MouseState.Over Then
                If X > 0 And X < 40 Then
                    'mouse over close
                    G.DrawString("r", ControlFont, OverBrush, New Point(13, 2))
                End If
            End If
        End If
    End Sub
End Class

Class InfluxGroupBox
    Inherits ContainerControl

    Sub New()
        Size = New Size(250, 150)
        BackColor = Color.FromArgb(77, 77, 77)
        ForeColor = Color.FromArgb(229, 229, 229)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim HB As New HatchBrush(HatchStyle.Percent20, Color.FromArgb(83, 83, 83), Color.FromArgb(77, 77, 77))
        G.FillRectangle(HB, New Rectangle(0, 0, Width, Height))

        G.DrawPath(New Pen(Color.FromArgb(65, 65, 65)), RoundRect(New Rectangle(0, 6, Width - 1, Height - 7), 2))
        G.DrawLine(New Pen(Color.FromArgb(77, 77, 77)), 6, 6, CInt(G.MeasureString(Text, Font).Width + 8), 6)
        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(6, 0, Width - 1, 11), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})

        e.Graphics.DrawImage(CType(B.Clone(), Image), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
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

<DefaultEvent("TextChanged")> _
Class InfluxNumericUpDown
    Inherits ThemeControl154
    Private pntMouseLocation As Point
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
    Property Value As Integer
        Get
            If MyBase.Text = "" Then
                Return 0
            ElseIf MyBase.Text.StartsWith("InfluxNumericUpDown") Then
                Return 0
            Else
                Return CInt(MyBase.Text)
            End If
        End Get
        Set(ByVal v As Integer)
            MyBase.Text = v.ToString
            If Base IsNot Nothing Then
                If v >= _MinValue And v <= _MaxValue Then
                    Base.Text = v.ToString
                ElseIf v < _MinValue Then
                    Base.Text = _MinValue.ToString
                    Base.Select(Base.Text.Length, 0)
                ElseIf v > _MaxValue Then
                    Base.Text = _MaxValue.ToString
                    Base.Select(Base.Text.Length, 0)
                End If
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

            End If
        End Set
    End Property
    Private _MaxValue As Integer = 100
    Property MaxValue() As Integer
        Get
            Return _MaxValue
        End Get
        Set(ByVal value As Integer)
            _MaxValue = value
            If Base IsNot Nothing Then
                Dim intBaseValue As Integer = CInt(Base.Text)
                If intBaseValue > value Then
                    Base.Text = value.ToString
                End If
            End If
        End Set
    End Property
    Private _MinValue As Integer = 0
    Property MinValue() As Integer
        Get
            Return _MinValue
        End Get
        Set(ByVal value As Integer)
            _MinValue = value
            If Base IsNot Nothing Then
                Dim intBaseValue As Integer = CInt(Base.Text)
                If intBaseValue < value Then
                    Base.Text = value.ToString
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

        Width = 125
        Text = ""
        Base.Text = ""
        Value = 0
        Base.Font = Font
        Base.Text = Value.ToString
        Base.ReadOnly = _ReadOnly

        Base.BorderStyle = BorderStyle.None

        Base.Location = New Point(4, 4)
        Base.Width = Width - 10


        AddHandler Base.TextChanged, AddressOf OnBaseTextChanged
        AddHandler Base.KeyDown, AddressOf OnBaseKeyDown
        AddHandler Base.LostFocus, AddressOf OnBaseFocusLost


        SetColor("Text", Color.FromArgb(229, 229, 229))
        SetColor("TextBackColor", Color.FromArgb(73, 73, 73))
        SetColor("OuterBorder", Color.FromArgb(81, 81, 81))
        SetColor("InnerBorder", Color.FromArgb(60, 60, 60))
    End Sub

    Private TextBackColor As Color
    Private InnerBorder, OuterBorder As Pen

    Protected Overrides Sub ColorHook()
        TextBackColor = GetColor("TextBackColor")

        InnerBorder = GetPen("InnerBorder")
        OuterBorder = GetPen("OuterBorder")

        Base.ForeColor = GetColor("Text")
        Base.BackColor = GetColor("TextBackColor")
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        pntMouseLocation = e.Location
        Invalidate()
    End Sub

    Private Function GetButtonBounds() As Rectangle
        Return New Rectangle(Width - 20, 3, 17, Height - 7)
    End Function

    Private Function GetUpButtonBounds() As Rectangle
        Dim ButtonBounds As Rectangle = GetButtonBounds()
        Return New Rectangle(ButtonBounds.Location, New Size(16, CInt(Height / 2) - 5))
    End Function

    Private Function GetDownButtonBounds() As Rectangle
        Dim ButtonBounds As Rectangle = GetButtonBounds()
        Return New Rectangle(ButtonBounds.Location.X, ButtonBounds.Location.Y + CInt(Height / 2) - 3, 16, CInt(Height / 2) - 5)
    End Function

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        If GetUpButtonBounds.Contains(e.Location) Then
            If Value + 1 <= _MaxValue Then
                Value += 1
            End If
        ElseIf GetDownButtonBounds.Contains(e.Location) Then
            If Value - 1 >= _MinValue Then
                Value -= 1
            End If
        End If
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(77, 77, 77))

        G.SmoothingMode = SmoothingMode.HighQuality
        G.FillRectangle(New SolidBrush(TextBackColor), New Rectangle(1, 1, Width - 3, Height - 3))
        'Draw up/down buttons
        If GetUpButtonBounds.Contains(pntMouseLocation) Then
            If State = MouseState.Down Then
                Dim ButtonGradient As New LinearGradientBrush(GetUpButtonBounds, Color.FromArgb(110, 110, 110), Color.FromArgb(97, 97, 97), 90)
                G.FillPath(ButtonGradient, CreateRound(GetUpButtonBounds, 2))
            Else
                Dim ButtonGradient As New LinearGradientBrush(GetUpButtonBounds, Color.FromArgb(100, 100, 100), Color.FromArgb(87, 87, 87), 90)
                G.FillPath(ButtonGradient, CreateRound(GetUpButtonBounds, 2))
            End If
        Else
            Dim ButtonGradient As New LinearGradientBrush(GetUpButtonBounds, Color.FromArgb(90, 90, 90), Color.FromArgb(77, 77, 77), 90)
            G.FillPath(ButtonGradient, CreateRound(GetUpButtonBounds, 2))
        End If
        If GetDownButtonBounds.Contains(pntMouseLocation) Then
            If State = MouseState.Down Then
                Dim ButtonGradient As New LinearGradientBrush(GetUpButtonBounds, Color.FromArgb(110, 110, 110), Color.FromArgb(97, 97, 97), 90)
                G.FillPath(ButtonGradient, CreateRound(GetDownButtonBounds, 2))
            Else
                Dim ButtonGradient As New LinearGradientBrush(GetUpButtonBounds, Color.FromArgb(100, 100, 100), Color.FromArgb(87, 87, 87), 90)
                G.FillPath(ButtonGradient, CreateRound(GetDownButtonBounds, 2))
            End If
        Else
            Dim ButtonGradient As New LinearGradientBrush(GetUpButtonBounds, Color.FromArgb(77, 77, 77), Color.FromArgb(90, 90, 90), 90)
            G.FillPath(ButtonGradient, CreateRound(GetDownButtonBounds, 2))
        End If
        G.DrawPath(New Pen(Color.FromArgb(65, 65, 65)), CreateRound(GetUpButtonBounds, 2))
        G.DrawPath(New Pen(Color.FromArgb(65, 65, 65)), CreateRound(GetDownButtonBounds, 2))
        'Draw up sign
        G.SmoothingMode = SmoothingMode.AntiAlias
        Dim pntCheckPointOneTop As New Point(GetUpButtonBounds.X + 6, GetUpButtonBounds.Y + GetUpButtonBounds.Height - 3)
        Dim CheckPointsTop() As Point = {pntCheckPointOneTop, New Point(pntCheckPointOneTop.X + 3, pntCheckPointOneTop.Y - 2), New Point(pntCheckPointOneTop.X + 6, pntCheckPointOneTop.Y)}
        If GetUpButtonBounds.Contains(pntMouseLocation) Then
            G.DrawLines(New Pen(Color.White), CheckPointsTop)
        Else
            G.DrawLines(New Pen(Color.FromArgb(220, 220, 220)), CheckPointsTop)
        End If
        'Draw down sign
        Dim pntCheckPointOneBottom As New Point(GetDownButtonBounds.X + 6, GetDownButtonBounds.Y + 3)
        Dim CheckPointsBottom() As Point = {pntCheckPointOneBottom, New Point(pntCheckPointOneBottom.X + 3, pntCheckPointOneBottom.Y + 2), New Point(pntCheckPointOneBottom.X + 6, pntCheckPointOneBottom.Y)}
        If GetDownButtonBounds.Contains(pntMouseLocation) Then
            G.DrawLines(New Pen(Color.White), CheckPointsBottom)
        Else
            G.DrawLines(New Pen(Color.FromArgb(220, 220, 220)), CheckPointsBottom)
        End If
        'Draw border
        G.DrawPath(OuterBorder, CreateRound(New Rectangle(0, 0, Width - 1, Height - 1), 4))
        G.DrawPath(InnerBorder, CreateRound(New Rectangle(1, 1, Width - 3, Height - 3), 4))
    End Sub
    Private Sub OnBaseFocusLost(ByVal s As Object, ByVal e As EventArgs)
        If Base.Text = "" Then
            Base.Text = "0"
            Value = 0
        End If
    End Sub
    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
        If Base.Text <> "-" Then
            If Base.Text.Contains("-") And Not Base.Text.StartsWith("-") Then
                Base.Text = Base.Text.Substring(Base.Text.IndexOf("-")) & Base.Text.Substring(0, Base.Text.IndexOf("-"))
                Base.Select(Base.Text.Length, 0)
            End If
            If Base.Text <> "" Then
                If CInt(Base.Text) <= _MaxValue Then
                    If CInt(Base.Text) >= _MinValue Then
                        Value = CInt(Base.Text)
                    Else
                        Value = _MinValue
                        Base.Text = _MinValue.ToString
                    End If
                Else
                    Value = _MaxValue
                    Base.Text = _MaxValue.ToString
                End If
            End If
        End If
    End Sub
    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            Base.SelectAll()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.OemMinus Or e.KeyCode = Keys.Subtract Then
            e.SuppressKeyPress = True
            If Not Base.Text.StartsWith("-") Then
                Base.Text = "-" & Base.Text
                Base.Select(Base.Text.Length, 0)
            End If
        End If
        If Not IsNumeric(e) Then
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Function IsNumeric(ByVal e As KeyEventArgs) As Boolean
        Dim k As Windows.Forms.Keys = e.KeyCode
        Return (k = Keys.NumPad0 Or k = Keys.NumPad1 Or k = Keys.NumPad2 Or k = Keys.NumPad3 Or k = Keys.NumPad4 _
                Or k = Keys.NumPad5 Or k = Keys.NumPad6 Or k = Keys.NumPad7 Or k = Keys.NumPad8 Or k = Keys.NumPad9 _
                Or (e.Shift And (k = Keys.D0 Or k = Keys.D1 Or k = Keys.D2 Or k = Keys.D3 Or k = Keys.D4 Or k = Keys.D5 Or k = Keys.D6 _
                Or k = Keys.D7 Or k = Keys.D8 Or k = Keys.D9)) Or _
                k = Keys.Back Or k = Keys.Delete Or k = Keys.Right Or k = Keys.Left Or k = Keys.OemMinus Or k = Keys.Subtract)
    End Function
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        Base.Location = New Point(5, 4)
        Base.Width = Width - 31

        MyBase.OnResize(e)
    End Sub
End Class

Class InfluxLabel
    Inherits Label

    Sub New()
        ForeColor = Color.FromArgb(229, 229, 229)
        BackColor = Color.FromArgb(77, 77, 77)
    End Sub

    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
        Dim HB As New HatchBrush(HatchStyle.Percent20, Color.FromArgb(83, 83, 83), BackColor)
        pevent.Graphics.FillRectangle(HB, New Rectangle(0, 0, Width, Height))
    End Sub
End Class
