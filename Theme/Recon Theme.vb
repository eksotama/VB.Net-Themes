Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Runtime.InteropServices
'.::Tweety Theme::.
'Author:   UnReLaTeDScript
'Credits:  Aeonhack [Themebase], Aeonhack (Menustrip Code)
'Version:  1.0
MustInherit Class Theme
    Inherits ContainerControl

#Region " Initialization "

    Protected G As Graphics
    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)
    End Sub

    Private ParentIsForm As Boolean
    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        Dock = DockStyle.Fill
        ParentIsForm = TypeOf Parent Is Form
        If ParentIsForm Then
            If Not _TransparencyKey = Color.Empty Then ParentForm.TransparencyKey = _TransparencyKey
            ParentForm.FormBorderStyle = FormBorderStyle.None
        End If
        MyBase.OnHandleCreated(e)
    End Sub

    Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal v As String)
            MyBase.Text = v
            Invalidate()
        End Set
    End Property
#End Region

#Region " Sizing and Movement "

    Private _Resizable As Boolean = True
    Property Resizable() As Boolean
        Get
            Return _Resizable
        End Get
        Set(ByVal value As Boolean)
            _Resizable = value
        End Set
    End Property

    Private _MoveHeight As Integer = 24
    Property MoveHeight() As Integer
        Get
            Return _MoveHeight
        End Get
        Set(ByVal v As Integer)
            _MoveHeight = v
            Header = New Rectangle(7, 7, Width - 14, _MoveHeight - 7)
        End Set
    End Property

    Private Flag As IntPtr
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If Not e.Button = MouseButtons.Left Then Return
        If ParentIsForm Then If ParentForm.WindowState = FormWindowState.Maximized Then Return

        If Header.Contains(e.Location) Then
            Flag = New IntPtr(2)
        ElseIf Current.Position = 0 Or Not _Resizable Then
            Return
        Else
            Flag = New IntPtr(Current.Position)
        End If

        Capture = False
        DefWndProc(Message.Create(Parent.Handle, 161, Flag, Nothing))

        MyBase.OnMouseDown(e)
    End Sub

    Private Structure Pointer
        ReadOnly Cursor As Cursor, Position As Byte
        Sub New(ByVal c As Cursor, ByVal p As Byte)
            Cursor = c
            Position = p
        End Sub
    End Structure

    Private F1, F2, F3, F4 As Boolean, PTC As Point
    Private Function GetPointer() As Pointer
        PTC = PointToClient(MousePosition)
        F1 = PTC.X < 7
        F2 = PTC.X > Width - 7
        F3 = PTC.Y < 7
        F4 = PTC.Y > Height - 7

        If F1 And F3 Then Return New Pointer(Cursors.SizeNWSE, 13)
        If F1 And F4 Then Return New Pointer(Cursors.SizeNESW, 16)
        If F2 And F3 Then Return New Pointer(Cursors.SizeNESW, 14)
        If F2 And F4 Then Return New Pointer(Cursors.SizeNWSE, 17)
        If F1 Then Return New Pointer(Cursors.SizeWE, 10)
        If F2 Then Return New Pointer(Cursors.SizeWE, 11)
        If F3 Then Return New Pointer(Cursors.SizeNS, 12)
        If F4 Then Return New Pointer(Cursors.SizeNS, 15)
        Return New Pointer(Cursors.Default, 0)
    End Function

    Private Current, Pending As Pointer
    Private Sub SetCurrent()
        Pending = GetPointer()
        If Current.Position = Pending.Position Then Return
        Current = GetPointer()
        Cursor = Current.Cursor
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If _Resizable Then SetCurrent()
        MyBase.OnMouseMove(e)
    End Sub

    Protected Header As Rectangle
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If Width = 0 OrElse Height = 0 Then Return
        Header = New Rectangle(7, 7, Width - 14, _MoveHeight - 7)
        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub

#End Region

#Region " Convienence "

    MustOverride Sub PaintHook()
    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return
        G = e.Graphics
        PaintHook()
    End Sub

    Private _TransparencyKey As Color
    Property TransparencyKey() As Color
        Get
            Return _TransparencyKey
        End Get
        Set(ByVal v As Color)
            _TransparencyKey = v
            Invalidate()
        End Set
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            _Image = value
            Invalidate()
        End Set
    End Property
    ReadOnly Property ImageWidth() As Integer
        Get
            If _Image Is Nothing Then Return 0
            Return _Image.Width
        End Get
    End Property

    Private _Size As Size
    Private _Rectangle As Rectangle
    Private _Gradient As LinearGradientBrush
    Private _Brush As SolidBrush

    Protected Sub DrawCorners(ByVal c As Color, ByVal rect As Rectangle)
        _Brush = New SolidBrush(c)
        G.FillRectangle(_Brush, rect.X, rect.Y, 1, 1)
        G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y, 1, 1)
        G.FillRectangle(_Brush, rect.X, rect.Y + (rect.Height - 1), 1, 1)
        G.FillRectangle(_Brush, rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), 1, 1)
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal p2 As Pen, ByVal rect As Rectangle)
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3)
    End Sub

    Protected Sub DrawText(ByVal a As HorizontalAlignment, ByVal c As Color, ByVal x As Integer)
        DrawText(a, c, x, 0)
    End Sub
    Protected Sub DrawText(ByVal a As HorizontalAlignment, ByVal c As Color, ByVal x As Integer, ByVal y As Integer)
        If String.IsNullOrEmpty(Text) Then Return
        _Size = G.MeasureString(Text, Font).ToSize
        _Brush = New SolidBrush(c)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(Text, Font, _Brush, x, _MoveHeight \ 2 - _Size.Height \ 2 + y)
            Case HorizontalAlignment.Right
                G.DrawString(Text, Font, _Brush, Width - _Size.Width - x, _MoveHeight \ 2 - _Size.Height \ 2 + y)
            Case HorizontalAlignment.Center
                G.DrawString(Text, Font, _Brush, Width \ 2 - _Size.Width \ 2 + x, _MoveHeight \ 2 - _Size.Height \ 2 + y)
        End Select
    End Sub

    Protected Sub DrawIcon(ByVal a As HorizontalAlignment, ByVal x As Integer)
        DrawIcon(a, x, 0)
    End Sub
    Protected Sub DrawIcon(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If _Image Is Nothing Then Return
        Select Case a
            Case HorizontalAlignment.Left
                G.DrawImage(_Image, x, _MoveHeight \ 2 - _Image.Height \ 2 + y)
            Case HorizontalAlignment.Right
                G.DrawImage(_Image, Width - _Image.Width - x, _MoveHeight \ 2 - _Image.Height \ 2 + y)
            Case HorizontalAlignment.Center
                G.DrawImage(_Image, Width \ 2 - _Image.Width \ 2, _MoveHeight \ 2 - _Image.Height \ 2)
        End Select
    End Sub

    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        _Rectangle = New Rectangle(x, y, width, height)
        _Gradient = New LinearGradientBrush(_Rectangle, c1, c2, angle)
        G.FillRectangle(_Gradient, _Rectangle)
    End Sub

#End Region

End Class
MustInherit Class ThemeControl
    Inherits Control

#Region " Initialization "

    Protected G As Graphics, B As Bitmap
    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)
        B = New Bitmap(1, 1)
        G = Graphics.FromImage(B)
    End Sub

    Sub AllowTransparent()
        SetStyle(ControlStyles.Opaque, False)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Overrides Property Text() As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal v As String)
            MyBase.Text = v
            Invalidate()
        End Set
    End Property
#End Region

#Region " Mouse Handling "

    Protected Enum State As Byte
        MouseNone = 0
        MouseOver = 1
        MouseDown = 2
    End Enum

    Protected MouseState As State
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        ChangeMouseState(State.MouseNone)
        MyBase.OnMouseLeave(e)
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        ChangeMouseState(State.MouseOver)
        MyBase.OnMouseEnter(e)
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        ChangeMouseState(State.MouseOver)
        MyBase.OnMouseUp(e)
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then ChangeMouseState(State.MouseDown)
        MyBase.OnMouseDown(e)
    End Sub

    Private Sub ChangeMouseState(ByVal e As State)
        MouseState = e
        Invalidate()
    End Sub

#End Region

#Region " Convienence "

    MustOverride Sub PaintHook()
    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return
        PaintHook()
        e.Graphics.DrawImage(B, 0, 0)
    End Sub

    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If Not Width = 0 AndAlso Not Height = 0 Then
            B = New Bitmap(Width, Height)
            G = Graphics.FromImage(B)
            Invalidate()
        End If
        MyBase.OnSizeChanged(e)
    End Sub

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
            _Image = value
            Invalidate()
        End Set
    End Property
    ReadOnly Property ImageWidth() As Integer
        Get
            If _Image Is Nothing Then Return 0
            Return _Image.Width
        End Get
    End Property
    ReadOnly Property ImageTop() As Integer
        Get
            If _Image Is Nothing Then Return 0
            Return Height \ 2 - _Image.Height \ 2
        End Get
    End Property

    Private _Size As Size
    Private _Rectangle As Rectangle
    Private _Gradient As LinearGradientBrush
    Private _Brush As SolidBrush

    Protected Sub DrawCorners(ByVal c As Color, ByVal rect As Rectangle)
        If _NoRounding Then Return

        B.SetPixel(rect.X, rect.Y, c)
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c)
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c)
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c)
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal p2 As Pen, ByVal rect As Rectangle)
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3)
    End Sub

    Protected Sub DrawText(ByVal a As HorizontalAlignment, ByVal c As Color, ByVal x As Integer)
        DrawText(a, c, x, 0)
    End Sub
    Protected Sub DrawText(ByVal a As HorizontalAlignment, ByVal c As Color, ByVal x As Integer, ByVal y As Integer)
        If String.IsNullOrEmpty(Text) Then Return
        _Size = G.MeasureString(Text, Font).ToSize
        _Brush = New SolidBrush(c)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(Text, Font, _Brush, x, Height \ 2 - _Size.Height \ 2 + y)
            Case HorizontalAlignment.Right
                G.DrawString(Text, Font, _Brush, Width - _Size.Width - x, Height \ 2 - _Size.Height \ 2 + y)
            Case HorizontalAlignment.Center
                G.DrawString(Text, Font, _Brush, Width \ 2 - _Size.Width \ 2 + x, Height \ 2 - _Size.Height \ 2 + y)
        End Select
    End Sub

    Protected Sub DrawIcon(ByVal a As HorizontalAlignment, ByVal x As Integer)
        DrawIcon(a, x, 0)
    End Sub
    Protected Sub DrawIcon(ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer)
        If _Image Is Nothing Then Return
        Select Case a
            Case HorizontalAlignment.Left
                G.DrawImage(_Image, x, Height \ 2 - _Image.Height \ 2 + y)
            Case HorizontalAlignment.Right
                G.DrawImage(_Image, Width - _Image.Width - x, Height \ 2 - _Image.Height \ 2 + y)
            Case HorizontalAlignment.Center
                G.DrawImage(_Image, Width \ 2 - _Image.Width \ 2, Height \ 2 - _Image.Height \ 2)
        End Select
    End Sub

    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        _Rectangle = New Rectangle(x, y, width, height)
        _Gradient = New LinearGradientBrush(_Rectangle, c1, c2, angle)
        G.FillRectangle(_Gradient, _Rectangle)
    End Sub
#End Region

End Class
MustInherit Class ThemeContainerControl
    Inherits ContainerControl

#Region " Initialization "

    Protected G As Graphics, B As Bitmap
    Sub New()
        SetStyle(DirectCast(139270, ControlStyles), True)
        B = New Bitmap(1, 1)
        G = Graphics.FromImage(B)
    End Sub

    Sub AllowTransparent()
        SetStyle(ControlStyles.Opaque, False)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
    End Sub

#End Region
#Region " Convienence "

    MustOverride Sub PaintHook()
    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        If Width = 0 OrElse Height = 0 Then Return
        PaintHook()
        e.Graphics.DrawImage(B, 0, 0)
    End Sub

    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If Not Width = 0 AndAlso Not Height = 0 Then
            B = New Bitmap(Width, Height)
            G = Graphics.FromImage(B)
            Invalidate()
        End If
        MyBase.OnSizeChanged(e)
    End Sub

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

    Private _Rectangle As Rectangle
    Private _Gradient As LinearGradientBrush

    Protected Sub DrawCorners(ByVal c As Color, ByVal rect As Rectangle)
        If _NoRounding Then Return
        B.SetPixel(rect.X, rect.Y, c)
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y, c)
        B.SetPixel(rect.X, rect.Y + (rect.Height - 1), c)
        B.SetPixel(rect.X + (rect.Width - 1), rect.Y + (rect.Height - 1), c)
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal p2 As Pen, ByVal rect As Rectangle)
        G.DrawRectangle(p1, rect.X, rect.Y, rect.Width - 1, rect.Height - 1)
        G.DrawRectangle(p2, rect.X + 1, rect.Y + 1, rect.Width - 3, rect.Height - 3)
    End Sub

    Protected Sub DrawGradient(ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal angle As Single)
        _Rectangle = New Rectangle(x, y, width, height)
        _Gradient = New LinearGradientBrush(_Rectangle, c1, c2, angle)
        G.FillRectangle(_Gradient, _Rectangle)
    End Sub
#End Region

End Class



Class ReconForm
    Inherits Theme
    Private _ShowIcon As Boolean
    Private TA As TextAlign
    Enum TextAlign As Integer
        Left = 0
        Center = 1
        Right = 2
    End Enum
    Public Property TextAlignment() As TextAlign
        Get
            Return TA
        End Get
        Set(ByVal value As TextAlign)
            TA = value
            Invalidate()
        End Set
    End Property
    Public Property ShowIcon As Boolean
        Get
            Return _ShowIcon
        End Get
        Set(ByVal value As Boolean)
            _ShowIcon = value
            Invalidate()
        End Set
    End Property
    Sub New()
        Color.FromArgb(45, 45, 45)
        MoveHeight = 30
        Me.ForeColor = Color.Olive
        TransparencyKey = Color.Fuchsia
        Me.BackColor = Color.FromArgb(41, 41, 41)
    End Sub
    Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(41, 41, 41))
        DrawGradient(Color.FromArgb(11, 11, 11), Color.FromArgb(26, 26, 26), 1, 1, ClientRectangle.Width, ClientRectangle.Height, 270S)
        DrawBorders(Pens.Black, New Pen(Color.FromArgb(52, 52, 52)), ClientRectangle)
        DrawGradient(Color.FromArgb(42, 42, 42), Color.FromArgb(40, 40, 40), 5, 30, Width - 10, Height - 35, 90S)
        G.DrawRectangle(New Pen(Color.FromArgb(18, 18, 18)), 5, 30, Width - 10, Height - 35)

        'Icon
        If _ShowIcon = False Then

            Select Case TA
                Case 0
                    DrawText(HorizontalAlignment.Left, Me.ForeColor, 6)
                Case 1
                    DrawText(HorizontalAlignment.Center, Me.ForeColor, 0)
                Case 2
                    MsgBox("Invalid Alignment, will not show text.")
            End Select
        Else

            Select Case TA
                Case 0
                    DrawText(HorizontalAlignment.Left, Me.ForeColor, 35)
                Case 1
                    DrawText(HorizontalAlignment.Center, Me.ForeColor, 0)
                Case 2
                    MsgBox("Invalid Alignment, will not show text.")
            End Select
            G.DrawIcon(Me.ParentForm.Icon, New Rectangle(New Point(6, 2), New Size(29, 29)))
        End If

        DrawCorners(Color.Fuchsia, ClientRectangle)
    End Sub
End Class
Class ReconButton
    Inherits ThemeControl
    Sub New()
        Me.ForeColor = Color.DarkOliveGreen
    End Sub
    Overrides Sub PaintHook()
        Select Case MouseState
            Case State.MouseNone
                G.Clear(Color.FromArgb(49, 49, 49))
                DrawGradient(Color.FromArgb(22, 22, 22), Color.FromArgb(34, 34, 34), 1, 1, ClientRectangle.Width, ClientRectangle.Height, 270S)
                DrawBorders(Pens.Black, New Pen(Color.FromArgb(52, 52, 52)), ClientRectangle)
                DrawText(HorizontalAlignment.Center, Me.ForeColor, 0)
                DrawCorners(Me.BackColor, ClientRectangle)
            Case State.MouseDown
                G.Clear(Color.FromArgb(49, 49, 49))
                DrawGradient(Color.FromArgb(28, 28, 28), Color.FromArgb(38, 38, 38), 1, 1, ClientRectangle.Width, ClientRectangle.Height, 270S)
                DrawGradient(Color.FromArgb(100, 0, 0, 0), Color.Transparent, 1, 1, ClientRectangle.Width, ClientRectangle.Height / 2, 90S)
                DrawBorders(Pens.Black, New Pen(Color.FromArgb(52, 52, 52)), ClientRectangle)
                DrawText(HorizontalAlignment.Center, Me.ForeColor, 1)
                DrawCorners(Me.BackColor, ClientRectangle)
            Case State.MouseOver
                G.Clear(Color.FromArgb(49, 49, 49))
                DrawGradient(Color.FromArgb(28, 28, 28), Color.FromArgb(38, 38, 38), 1, 1, ClientRectangle.Width, ClientRectangle.Height, 270S)
                DrawGradient(Color.FromArgb(100, 50, 50, 50), Color.Transparent, 1, 1, ClientRectangle.Width, ClientRectangle.Height / 2, 90S)
                DrawBorders(Pens.Black, New Pen(Color.FromArgb(52, 52, 52)), ClientRectangle)
                DrawText(HorizontalAlignment.Center, Me.ForeColor, -1)
                DrawCorners(Me.BackColor, ClientRectangle)
        End Select
        Me.Cursor = Cursors.Hand

    End Sub
End Class

Class TxtBox
    Inherits TextBox

    Protected Overrides Sub WndProc(ByRef m As Message)
        Select Case m.Msg
            Case 15
                Invalidate()
                MyBase.WndProc(m)
                Me.CustomPaint()
                Exit Select
            Case Else
                MyBase.WndProc(m)
                Exit Select
        End Select
    End Sub

    Sub New()
        Font = New Font("Microsoft Sans Serif", 8)
        ForeColor = Color.Black
        BackColor = Color.FromArgb(28, 28, 28)
        BorderStyle = Windows.Forms.BorderStyle.FixedSingle
    End Sub

    Private Sub CustomPaint()
        Dim p As New Pen(Color.Black)
        Dim o As New Pen(Color.FromArgb(45, 45, 45))
        CreateGraphics.DrawLine(p, 0, 0, Width, 0)
        CreateGraphics.DrawLine(p, 0, Height - 1, Width, Height - 1)
        CreateGraphics.DrawLine(p, 0, 0, 0, Height - 1)
        CreateGraphics.DrawLine(p, Width - 1, 0, Width - 1, Height - 1)

        CreateGraphics.DrawLine(o, 1, 1, Width - 2, 1)
        CreateGraphics.DrawLine(o, 1, Height - 2, Width - 2, Height - 2)
        CreateGraphics.DrawLine(o, 1, 1, 1, Height - 2)
        CreateGraphics.DrawLine(o, Width - 2, 1, Width - 2, Height - 2)
    End Sub
End Class

Class ReconBar
    Inherits ThemeControl
    Private _Maximum As Integer
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal v As Integer)
            Select Case v
                Case Is < _Value
                    _Value = v
            End Select
            _Maximum = v
            Invalidate()
        End Set
    End Property
    Private _Value As Integer
    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal v As Integer)
            Select Case v
                Case Is > _Maximum
                    v = _Maximum
            End Select
            _Value = v
            Invalidate()
        End Set
    End Property
    Overrides Sub PaintHook()

        G.Clear(Color.FromArgb(49, 49, 49))
        DrawGradient(Color.FromArgb(18, 18, 18), Color.FromArgb(28, 28, 28), 1, 1, ClientRectangle.Width, ClientRectangle.Height, 90S)

        'Fill
        Select Case _Value
            Case Is > 6
                Dim x As Integer = CInt(_Value / _Maximum * Width)
                Dim z As Integer = ClientRectangle.Height - 7

                DrawGradient(Color.FromArgb(28, 28, 28), Color.FromArgb(38, 38, 38), 1, 1, CInt(_Value / _Maximum * Width), ClientRectangle.Height, 270S)
                DrawGradient(Color.FromArgb(100, 50, 50, 50), Color.Transparent, 1, 1, CInt(_Value / _Maximum * Width), ClientRectangle.Height / 2, 90S)

                DrawGradient(Color.FromArgb(5, Me.ForeColor), Color.Transparent, 1, 1, CInt(_Value / _Maximum * Width), ClientRectangle.Height / 4, 90S)
                DrawGradient(Color.FromArgb(9, Me.ForeColor), Color.Transparent, 1, CInt(_Value / _Maximum * Width) / 2, ClientRectangle.Width, ClientRectangle.Height / 2, 270S)


                G.DrawRectangle(New Pen(Color.FromArgb(50, 50, 50)), 1, 1, x, z + 4)

                G.DrawRectangle(Pens.Black, 2, 2, x, z + 2)

            Case Is > 1
                DrawGradient(Color.FromArgb(109, 183, 255), Color.FromArgb(40, 154, 255), 3, 3, CInt(_Value / _Maximum * Width), Height / 2 - 3, 90S)
                DrawGradient(Color.FromArgb(30, 130, 245), Color.FromArgb(15, 100, 170), 3, Height / 2 - 1, CInt(_Value / _Maximum * Width), Height / 2 - 2, 90S)
        End Select

        'Borders
        DrawBorders(Pens.Black, New Pen(Color.FromArgb(52, 52, 52)), ClientRectangle)

    End Sub
    Public Sub Increment(ByVal Amount As Integer)
        If Me.Value + Amount > Maximum Then
            Me.Value = Maximum
        Else
            Me.Value += Amount
        End If
    End Sub
End Class

Class ReconGroupBox
    Inherits ThemeContainerControl
    Sub New()
        AllowTransparent()
    End Sub
    Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(25, 25, 25))
        Me.BackColor = Color.FromArgb(25, 25, 25)
        DrawGradient(Color.FromArgb(11, 11, 11), Color.FromArgb(26, 26, 26), 1, 1, ClientRectangle.Width, ClientRectangle.Height, 270S)
        DrawBorders(Pens.Black, New Pen(Color.FromArgb(52, 52, 52)), ClientRectangle)
        Try
            DrawGradient(Color.FromArgb(150, 32, 32, 32), Color.FromArgb(150, 31, 31, 31), 5, 23, Width - 10, Height - 28, 90S)
            G.DrawRectangle(New Pen(Color.FromArgb(130, 13, 13, 13)), 5, 23, Width - 10, Height - 28)

            G.DrawString(Text, Font, New SolidBrush(Me.ForeColor), 4, 6)

            DrawCorners(Color.Transparent, ClientRectangle)
        Catch
        End Try
    End Sub
End Class
Class ReconCheck
    Inherits ThemeControl

#Region " Properties "
    Private _CheckedState As Boolean
    Public Property CheckedState() As Boolean
        Get
            Return _CheckedState
        End Get
        Set(ByVal v As Boolean)
            _CheckedState = v
            Invalidate()
        End Set
    End Property
#End Region

    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(16, 16)
        MaximumSize = New Size(600, 16)
        CheckedState = False
    End Sub

    Public Overrides Sub PaintHook()
        G.Clear(BackColor)
        DrawGradient(Color.FromArgb(18, 18, 18), Color.FromArgb(28, 28, 28), 0, 0, 15, 15, 90S)

        Select Case CheckedState
            Case True

                DrawGradient(Color.FromArgb(18, 18, 18), Color.FromArgb(28, 28, 28), 0, 0, 15, 15, 270S)
                DrawGradient(Color.FromArgb(100, 40, 40, 40), Color.Transparent, 0, 0, 15, 15, 90S)

                DrawGradient(Color.FromArgb(5, Me.ForeColor), Color.Transparent, 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(9, Me.ForeColor), Color.Transparent, 0, 0, 15, 15, 270S)

                G.DrawRectangle(New Pen(Color.FromArgb(10, 10, 10)), 3, 3, 11, 11)
                DrawGradient(Color.FromArgb(50, 50, 50), Color.FromArgb(30, 30, 30), 0, 0, 15, 15, 90S)
        End Select

        DrawBorders(Pens.Black, New Pen(Color.FromArgb(52, 52, 52)), New Rectangle(0, 0, 15, 15))
        DrawText(HorizontalAlignment.Left, Me.ForeColor, 17, 0)
    End Sub

    Sub changeCheck() Handles Me.MouseDown
        Select Case CheckedState
            Case True
                CheckedState = False
            Case False
                CheckedState = True
        End Select
    End Sub
End Class
Class ReconCheckBox
    Inherits ThemeControl

#Region " Properties "
    Private _CheckedState As Boolean
    Public Property CheckedState() As Boolean
        Get
            Return _CheckedState
        End Get
        Set(ByVal v As Boolean)
            _CheckedState = v
            Invalidate()
        End Set
    End Property
#End Region

    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(16, 16)
        MaximumSize = New Size(600, 16)
        CheckedState = False
    End Sub

    Public Overrides Sub PaintHook()
        G.Clear(Me.Parent.BackColor)
        DrawGradient(Color.FromArgb(22, 22, 22), Color.FromArgb(32, 32, 32), 0, 0, 15, 15, 90S)
        Select Case CheckedState
            Case True
                DrawBorders(Pens.Black, New Pen(Color.FromArgb(52, 52, 52)), New Rectangle(0, 0, 15, 15))
                G.DrawString("a", New Font("Marlett", 12), Brushes.Black, New Point(-3, -1))
            Case False
                DrawBorders(Pens.Black, New Pen(Color.FromArgb(52, 52, 52)), New Rectangle(0, 0, 15, 15))
        End Select

        DrawText(HorizontalAlignment.Left, Me.ForeColor, 17, 0)
    End Sub

    Sub changeCheck() Handles Me.MouseDown
        Select Case CheckedState
            Case True
                CheckedState = False
            Case False
                CheckedState = True
        End Select
    End Sub
End Class

Class ReconSeperator
    Inherits Control

    Private _Orientation As Orientation
    Public Property Orientation() As Orientation
        Get
            Return _Orientation
        End Get
        Set(ByVal v As Orientation)
            _Orientation = v
            UpdateOffset()
            Invalidate()
        End Set
    End Property

    Dim G As Graphics, B As Bitmap, I As Integer
    Dim C1 As Color, P1, P2 As Pen
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        C1 = Me.BackColor
        P1 = New Pen(Color.FromArgb(22, 22, 22))
        P2 = New Pen(Color.FromArgb(49, 49, 49))
        MinimumSize = New Size(5, 2)
        MaximumSize = New Size(10000, 2)
    End Sub

    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        UpdateOffset()
        MyBase.OnSizeChanged(e)
    End Sub

    Sub UpdateOffset()
        I = Convert.ToInt32(If(_Orientation = 0, Height / 2 - 1, Width / 2 - 1))
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        B = New Bitmap(Width, Height)
        G = Graphics.FromImage(B)

        G.Clear(C1)

        If _Orientation = 0 Then
            G.DrawLine(P1, 0, I, Width, I)
            G.DrawLine(P2, 0, I + 1, Width, I + 1)
        Else
            G.DrawLine(P2, I, 0, I, Height)
            G.DrawLine(P1, I + 1, 0, I + 1, Height)
        End If

        e.Graphics.DrawImage(B, 0, 0)
        G.Dispose()
        B.Dispose()
    End Sub

    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub

End Class