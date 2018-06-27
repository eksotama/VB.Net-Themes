Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Runtime.InteropServices

'Creator: DownFall.
'Credits: AeonHack (Theme Base Version 1.4)
'Site: ******
'Date 10/5/11
'Version 1.0

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

    Overrides Property Text As String
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

    Overrides Property Text As String
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

Class ZeusTheme
    Inherits Theme

#Region " Settings "

    Sub New()
        Me.Font = New Font("Candara", 8, FontStyle.Bold)
        Me.Resizable = False
        MoveHeight = 15
    End Sub

#End Region

#Region " PaintHook "

    Public Overrides Sub PaintHook()

        Dim C1 As Color = Color.FromArgb(38, 38, 38)
        Dim C2 As Color = Color.AliceBlue
        Dim P1 As Pen = Pens.Black
        Dim P2 As Pen = Pens.AliceBlue


        G.Clear(C1)
        G.DrawLine(P2, 50, 0, 0, 50)
        G.DrawLine(P2, Width - 50, 0, Width, 50)
        G.DrawLine(P2, 50, 20, Width - 50, 20)
        G.DrawLine(P2, 70, 0, 50, 20)
        G.DrawLine(P2, Width - 70, 0, Width - 50, 20)
        G.DrawLine(P2, 0, 75, 35, 40)
        G.DrawLine(P2, Width - 35, 40, Width, 75)
        G.DrawLine(P2, 35, 40, Width - 35, 40)
        G.DrawRectangle(P2, 15, 75, Width - 30, Height - 90)
        G.DrawString("<<>>", Me.Font, Brushes.AliceBlue, Width - 32, 0)
        G.DrawString("<<>>", Me.Font, Brushes.AliceBlue, 8, 0)
        DrawBorders(P1, P2, ClientRectangle)
        DrawText(HorizontalAlignment.Center, C2, 0)

    End Sub

#End Region

End Class
Class ZeusButton
    Inherits ThemeControl

#Region " PaintHook "

    Public Overrides Sub PaintHook()

        Dim C1 As Color = Color.FromArgb(38, 38, 38)
        Dim C2 As Color = Color.AliceBlue
        Dim C3 As Color = Color.FromArgb(150, 255, 255)
        Dim P1 As Pen = Pens.Black
        Dim P2 As Pen = Pens.AliceBlue

        Select Case MouseState

            Case State.MouseNone
                G.Clear(C1)
                DrawGradient(C2, C3, 0, 0, Width, Height, 90)
                DrawText(HorizontalAlignment.Center, C1, 0)
                DrawBorders(P1, P2, ClientRectangle)
            Case State.MouseOver
                G.Clear(C1)
                DrawGradient(C2, C3, 0, 0, Width, Height, 90)
                DrawText(HorizontalAlignment.Center, C1, 0)
                DrawBorders(P2, P1, ClientRectangle)
            Case State.MouseDown
                G.Clear(C1)
                DrawGradient(C2, C3, 0, 0, Width - 1, Height - 1, 90)
                G.DrawRectangle(P1, 2, 2, Width - 5, Height - 5)
                DrawText(HorizontalAlignment.Center, C1, 0)
                DrawBorders(P1, P2, ClientRectangle)
        End Select

    End Sub

#End Region

End Class
Class ZeusTopButton
    Inherits ThemeControl


#Region " Options "

    Sub New()
        Me.Size = New Size(15, 15)
        Me.MinimumSize = New Size(15, 15)
        Me.Text = "X"
    End Sub

#End Region

#Region " PaintHook "

    Public Overrides Sub PaintHook()

        Dim B1 As Brush = Brushes.Black
        Dim B2 As Brush = Brushes.LightCyan
        Dim B3 As Brush = Brushes.AliceBlue
        Dim C1 As Color = Color.FromArgb(45, 45, 45)
        Dim C2 As Color = Color.FromArgb(150, 255, 255)
        Dim C3 As Color = Color.AliceBlue
        Dim P1 As Pen = Pens.Black
        Dim P2 As Pen = Pens.LightCyan
        Dim P3 As Pen = Pens.AliceBlue


        G.Clear(C1)

        Select Case MouseState
            Case State.MouseNone
                G.DrawEllipse(P3, 0, 0, Width - 1, Height - 1)
                Me.Font = New Font("Cambria", 8, FontStyle.Bold)
                DrawText(HorizontalAlignment.Center, C3, 0)
            Case State.MouseOver
                G.DrawEllipse(P3, 0, 0, Width - 1, Height - 1)
                Me.Font = New Font("Cambria", 8, FontStyle.Bold)
                DrawText(HorizontalAlignment.Center, C3, 0)
            Case State.MouseDown
                G.DrawEllipse(P3, 1, 1, Width - 3, Height - 3)
                Me.Font = New Font("Cambria", 6, FontStyle.Bold)
                DrawText(HorizontalAlignment.Center, C3, 0)
        End Select



    End Sub

#End Region

End Class
Class ZeusProgressBar
    Inherits ThemeControl

#Region " Properties "


    Private _Maximum As Integer
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal v As Integer)
            If v < 1 Then v = 1
            If v < _Value Then _Value = v
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
            If v > _Maximum Then v = _Maximum
            Maximum = 100
            _Value = v
            Invalidate()
        End Set
    End Property


    Private _ShowText As Boolean
    Public Property ShowText() As Boolean
        Get
            Return _ShowText
        End Get
        Set(ByVal v As Boolean)
            _ShowText = v
            Invalidate()
        End Set
    End Property




#End Region

#Region " PaintHook "

    Public Overrides Sub PaintHook()

        Dim C1 As Color = Color.FromArgb(38, 38, 38)
        Dim C2 As Color = Color.AliceBlue
        Dim C3 As Color = Color.FromArgb(150, 255, 255)
        Dim P1 As Pen = Pens.Black
        Dim P2 As Pen = Pens.AliceBlue

        G.Clear(C1)

        Select Case _Value
            Case Is > 2
                DrawGradient(C2, C3, 3, 3, CInt((_Value / _Maximum) * Width) - 6, Height - 6, 90)
                Select Case ShowText
                    Case True
                        DrawText(HorizontalAlignment.Center, C1, 0)
                End Select
            Case Is > 0
                DrawGradient(C1, C1, 3, 3, CInt((_Value / _Maximum) * Width) - 6, Height - 6, 90)
        End Select

        G.DrawRectangle(P2, 0, 0, Width - 1, Height - 1)

    End Sub

#End Region

End Class
Class ZeusCheckBox
    Inherits ThemeControl

#Region " Properties "


    Private _Checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal v As Boolean)
            _Checked = v
            Invalidate()
        End Set
    End Property


    Private _ChangeCheck
    Sub ChangeCheck() Handles Me.Click
        Select Case Checked
            Case True
                Checked = False
            Case False
                Checked = True
        End Select
    End Sub

#End Region

#Region " Options "

    Sub New()
        Checked = False
        Me.Size = New Size(115, 23)
        Me.MinimumSize = New Size(0, 23)
        Me.MaximumSize = New Size(900, 23)
    End Sub

#End Region

#Region " PaintHook "

    Public Overrides Sub PaintHook()

        Dim C1 As Color = Color.FromArgb(38, 38, 38)
        Dim C2 As Color = Color.AliceBlue
        Dim C3 As Color = Color.FromArgb(150, 255, 255)
        Dim P1 As Pen = Pens.Black
        Dim P2 As Pen = Pens.AliceBlue

        G.Clear(C1)

        Select Case MouseState

            Case State.MouseNone
                DrawText(HorizontalAlignment.Center, C2, 0, 1)
            Case State.MouseOver
                DrawText(HorizontalAlignment.Center, C2, 0, 1)
                G.DrawRectangle(P2, 0, 0, Width - 1, Height - 1)
            Case State.MouseDown
                DrawText(HorizontalAlignment.Center, C2, 0, 1)
        End Select

        Select Case Checked
            Case True
                DrawGradient(C2, C3, 6, 6, 10, 10, 90)
                G.DrawRectangle(P2, 6, 6, 10, 10)
                G.DrawRectangle(P1, 5, 5, 11, 11)
            Case False
                DrawGradient(C1, C1, 6, 6, 5, 5, 90)
                G.DrawRectangle(P2, 6, 6, 10, 10)
        End Select

    End Sub

#End Region


End Class
