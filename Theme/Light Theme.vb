Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Runtime.InteropServices
'.::Light Theme::.
'Author:   UnReLaTeDScript
'Credits:  Aeonhack [Themebase]
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


Class lTheme
    Inherits Theme
    Private _ShowIcon As Boolean
    Public Property ShowIcon() As Boolean
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
        Me.ForeColor = Color.FromArgb(30, 144, 255)
        TransparencyKey = Color.Fuchsia
        Me.BackColor = Color.FromArgb(235, 235, 235)
    End Sub
    Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(176, 176, 176))
        Me.ForeColor = Color.FromArgb(45, 45, 45)
        Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
        Dim hb2 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(35, Color.White), Color.Transparent)
        DrawGradient(Color.FromArgb(176, 176, 176), Color.FromArgb(215, 215, 215), 0, 0, Width, 30, 270S)
        G.FillRectangle(hb, 1, 1, Width, Height)
        G.FillRectangle(New SolidBrush(Color.FromArgb(235, 235, 235)), 5, 30, Width - 10, Height - 35)
        G.FillRectangle(hb2, 5, 30, Width - 10, Height - 35)
        DrawGradient(Color.FromArgb(30, Color.White), Color.Transparent, 3, 3, Width - 6, 14, 60S)
        G.DrawRectangle(New Pen(Color.FromArgb(195, 195, 195)), 5, 30, Width - 10, Height - 35)
        DrawBorders(New Pen(Color.FromArgb(109, 109, 109)), Pens.LightGray, ClientRectangle)
        DrawCorners(Color.Fuchsia, ClientRectangle)
        G.DrawIcon(FindForm.Icon, New Rectangle(5, 6, 20, 20))
        G.DrawString(FindForm.Text, Me.Font, New SolidBrush(Me.ForeColor), New Point(25, 10))
    End Sub
End Class
Class lButton
    Inherits ThemeControl
    Overrides Sub PaintHook()
        DrawText(HorizontalAlignment.Center, Color.Lime, 0)
        Me.ForeColor = Color.FromArgb(45, 45, 45)
        Select Case MouseState
            Case State.MouseNone
                Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
                Dim hb2 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(35, Color.White), Color.Transparent)
                G.FillRectangle(New SolidBrush(Color.FromArgb(196, 196, 196)), 0, 0, Width, Height)
                DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, Width, 30, 270S)
                G.FillRectangle(hb, 1, 1, Width, Height)
                DrawBorders(Pens.Gray, Pens.White, ClientRectangle)
                DrawGradient(Color.FromArgb(50, Color.White), Color.Transparent, 1, 1, Width - 2, Height / 2 - 3, 270S)
                DrawText(HorizontalAlignment.Center, Me.ForeColor, 0)
                DrawCorners(Me.Parent.BackColor, ClientRectangle)
            Case State.MouseDown
                Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
                Dim hb2 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(35, Color.White), Color.Transparent)
                G.FillRectangle(New SolidBrush(Color.FromArgb(196, 196, 196)), 0, 0, Width, Height)
                DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, Width, 30, 270S)
                G.FillRectangle(hb, 1, 1, Width, Height)
                DrawBorders(Pens.Gray, Pens.LightGray, ClientRectangle)
                DrawText(HorizontalAlignment.Center, Me.ForeColor, 1)
                DrawGradient(Color.FromArgb(60, Color.RoyalBlue), Color.Transparent, 0, 0, Width, Height, 90S)
                DrawGradient(Color.FromArgb(25, Color.Black), Color.Transparent, 0, 0, Width, Height, 270S)
                DrawGradient(Color.FromArgb(20, Color.White), Color.Transparent, 1, 1, Width - 2, Height / 2, 270S)
                DrawCorners(Me.Parent.BackColor, ClientRectangle)
            Case State.MouseOver
                Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
                Dim hb2 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(35, Color.White), Color.Transparent)
                G.FillRectangle(New SolidBrush(Color.FromArgb(196, 196, 196)), 0, 0, Width, Height)
                DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, Width, 30, 270S)
                G.FillRectangle(hb, 1, 1, Width, Height)
                DrawBorders(Pens.Gray, Pens.LightGray, ClientRectangle)
                DrawText(HorizontalAlignment.Center, Me.ForeColor, -1)
                DrawGradient(Color.FromArgb(35, Color.RoyalBlue), Color.Transparent, 0, 0, Width, Height, 90S)
                DrawGradient(Color.FromArgb(35, Color.White), Color.Transparent, 1, 1, Width - 2, Height / 2 - 5, 270S)
                DrawCorners(Me.Parent.BackColor, ClientRectangle)
        End Select
        Me.Cursor = Cursors.Hand
        DrawCorners(Me.Parent.BackColor, ClientRectangle)
    End Sub
End Class
Class lProgressBar
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
        G.Clear(Color.FromArgb(51, 51, 51))
        Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(15, Color.White), Color.Transparent)
        Dim hb2 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(35, Color.White), Color.Transparent)
        G.FillRectangle(New SolidBrush(Color.FromArgb(196, 196, 196)), 0, 0, Width, Height)
        DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, Width, 30, 270S)
        DrawGradient(Color.FromArgb(40, Color.White), Color.FromArgb(30, Color.White), 1, 1, Width, Height / 2 - 4, 270S)
        'Fill
        Select Case _Value
            Case Is > 6
                DrawGradient(Color.FromArgb(109, 183, 255), Color.FromArgb(40, 154, 255), 3, 3, CInt(_Value / _Maximum * Width) - 6, Height / 2 - 3, 90S)
                DrawGradient(Color.FromArgb(30, 130, 245), Color.FromArgb(15, 100, 170), 3, Height / 2 - 1, CInt(_Value / _Maximum * Width) - 6, Height / 2 - 2, 90S)

            Case Is > 1
                DrawGradient(Color.FromArgb(109, 183, 255), Color.FromArgb(40, 154, 255), 3, 3, CInt(_Value / _Maximum * Width), Height / 2 - 3, 90S)
                DrawGradient(Color.FromArgb(30, 130, 245), Color.FromArgb(15, 100, 170), 3, Height / 2 - 1, CInt(_Value / _Maximum * Width), Height / 2 - 2, 90S)
        End Select

        'Borders
        G.DrawRectangle(Pens.DarkGray, 3, 3, Width - 7, Height - 7)
        G.FillRectangle(hb, 1, 1, Width, Height)
        DrawBorders(Pens.Gray, Pens.White, ClientRectangle)

    End Sub
    Public Sub Increment(ByVal Amount As Integer)
        If Me.Value + Amount > Maximum Then
            Me.Value = Maximum
        Else
            Me.Value += Amount
        End If
    End Sub
End Class

Class lCheck
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
        Me.ForeColor = Me.Parent.ForeColor
        Select Case CheckedState
            Case True
                Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
                DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, 15, 15, 270S)
                DrawGradient(Color.FromArgb(35, Color.Black), Color.Transparent, 0, 0, 15, 15, 180S)
                G.FillRectangle(hb, 0, 0, 15, 15)
                G.DrawString("a", New Font("Marlett", 12), Brushes.Black, New Point(-3, -1))
            Case False
                Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
                DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, 15, 15, 270S)
                DrawGradient(Color.FromArgb(35, Color.Black), Color.Transparent, 0, 0, 15, 15, 180S)
                G.FillRectangle(hb, 0, 0, 15, 15)
        End Select

        DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))

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
Class lCheckBox
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
        Me.ForeColor = Me.Parent.ForeColor
        Select Case CheckedState
            Case True
                Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
                DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, 15, 15, 270S)
                DrawGradient(Color.FromArgb(35, Color.Black), Color.Transparent, 0, 0, 15, 15, 180S)
                G.FillRectangle(hb, 0, 0, 15, 15)
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(34, 64, 160), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(34, 64, 160), Color.FromArgb(27, 81, 255), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(27, 81, 255), Color.FromArgb(34, 64, 160), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case False
                Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
                DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, 15, 15, 270S)
                DrawGradient(Color.FromArgb(35, Color.Black), Color.Transparent, 0, 0, 15, 15, 180S)
                G.FillRectangle(hb, 0, 0, 15, 15)
        End Select

        DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))

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
Class lSwitch
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
        Me.ForeColor = Me.Parent.ForeColor
        Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
        DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, 30, 15, 270S)
        DrawGradient(Color.FromArgb(35, Color.Black), Color.Transparent, 0, 0, 15, 15, 180S)
        DrawGradient(Color.FromArgb(35, Color.Black), Color.Transparent, 15, 0, 16, 15, 0S)
        G.FillRectangle(hb, 1, 1, Width, Height)
        Select Case CheckedState
            Case True
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(4, 128, 7), 0, 0, 13, 15, 90S)
                DrawGradient(Color.FromArgb(4, 128, 7), Color.FromArgb(17, 196, 21), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(17, 196, 21), Color.FromArgb(4, 128, 7), 4, 4, 7, 7, 90S)
                G.DrawRectangle(Pens.LightGray, New Rectangle(0, 0, 13, 14))
            Case False
                DrawGradient(Color.FromArgb(160, 0, 0), Color.FromArgb(109, 16, 16), 15, 0, 13, 15, 90S)
                DrawGradient(Color.FromArgb(109, 16, 16), Color.FromArgb(212, 20, 20), 18, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(212, 20, 20), Color.FromArgb(109, 16, 16), 19, 4, 7, 7, 90S)
                G.DrawRectangle(Pens.LightGray, New Rectangle(15, 0, 13, 14))
        End Select

        DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 30, 15))
        DrawText(HorizontalAlignment.Left, Me.ForeColor, 32, 0)
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
Class lRadiobutton
    Inherits ThemeControl

    Private _Checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal V As Boolean)
            _Checked = V
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        MyBase.OnClick(e)
        For Each C As Control In Parent.Controls
            If C.GetType.ToString = Replace(My.Application.Info.ProductName, " ", "_") & ".lRadiobutton" Then
                Dim CC As lRadiobutton
                CC = C
                CC.Checked = False
            Else

            End If
        Next
        _Checked = True
    End Sub

    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(17, 17)
        MaximumSize = New Size(600, 17)
    End Sub

    Overrides Sub PaintHook()
        G.Clear(BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(20, Color.White), Color.Transparent)
        Dim hb2 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(35, Color.White), Color.Transparent)
        Dim a As New LinearGradientBrush(New Rectangle(1, 1, 14, 14), Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 90.0F)
        If _Checked = False Then
            G.FillEllipse(New SolidBrush(Color.Gray), 0, 0, 16, 16)
            G.FillEllipse(New SolidBrush(Color.LightGray), 1, 1, 14, 14)
            G.FillEllipse(a, New Rectangle(2, 2, 12, 12))
            G.FillEllipse(hb, New Rectangle(2, 2, 12, 12))

        Else
            G.FillEllipse(New SolidBrush(Color.Gray), 0, 0, 16, 16)
            G.FillEllipse(New SolidBrush(Color.LightGray), 1, 1, 14, 14)
            G.FillEllipse(a, New Rectangle(2, 2, 12, 12))
            G.FillEllipse(hb, New Rectangle(2, 2, 12, 12))
            G.FillEllipse(Brushes.Black, New Rectangle(5, 6, 5, 5))
            G.FillEllipse(New SolidBrush(Color.FromArgb(60, 60, 60)), New Rectangle(5, 5, 5, 5))
        End If
        DrawText(HorizontalAlignment.Left, Me.ForeColor, 18, 1)
    End Sub
End Class

Class lControlRed
    Inherits ThemeControl
    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(14, 14)
        MaximumSize = New Size(15, 15)
    End Sub
    Overrides Sub PaintHook()


        Select Case MouseState
            Case State.MouseNone
                DrawGradient(Color.FromArgb(160, 0, 0), Color.FromArgb(109, 16, 16), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(109, 16, 16), Color.FromArgb(212, 20, 20), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(212, 20, 20), Color.FromArgb(109, 16, 16), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseDown
                DrawGradient(Color.FromArgb(160, 0, 0), Color.FromArgb(109, 16, 16), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(109, 16, 16), Color.FromArgb(212, 20, 20), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(212, 20, 20), Color.FromArgb(109, 16, 16), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseOver
                DrawGradient(Color.FromArgb(160, 0, 0), Color.FromArgb(249, 50, 50), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(249, 50, 50), Color.FromArgb(212, 20, 20), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(212, 20, 20), Color.FromArgb(249, 50, 50), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
        End Select
        Me.Cursor = Cursors.Hand

    End Sub
End Class
Class lControlGreen
    Inherits ThemeControl
    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(14, 14)
        MaximumSize = New Size(15, 15)
    End Sub
    Overrides Sub PaintHook()


        Select Case MouseState
            Case State.MouseNone
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(4, 128, 7), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(4, 128, 7), Color.FromArgb(17, 196, 21), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(17, 196, 21), Color.FromArgb(4, 128, 7), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseDown
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(4, 128, 7), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(4, 128, 7), Color.FromArgb(17, 196, 21), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(17, 196, 21), Color.FromArgb(4, 128, 7), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseOver
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(22, 234, 27), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(22, 234, 27), Color.FromArgb(17, 196, 21), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(17, 196, 21), Color.FromArgb(22, 234, 27), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
        End Select
        Me.Cursor = Cursors.Hand

    End Sub
End Class
Class lControlYellow
    Inherits ThemeControl
    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(14, 14)
        MaximumSize = New Size(15, 15)
    End Sub
    Overrides Sub PaintHook()


        Select Case MouseState
            Case State.MouseNone
                DrawGradient(Color.FromArgb(160, 160, 0), Color.FromArgb(162, 154, 18), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(162, 154, 18), Color.FromArgb(237, 225, 25), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(237, 225, 25), Color.FromArgb(162, 154, 18), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseDown
                DrawGradient(Color.FromArgb(160, 160, 0), Color.FromArgb(162, 154, 18), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(162, 154, 18), Color.FromArgb(237, 225, 25), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(237, 225, 25), Color.FromArgb(162, 154, 18), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseOver
                DrawGradient(Color.FromArgb(160, 160, 0), Color.FromArgb(244, 234, 68), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(244, 234, 68), Color.FromArgb(237, 225, 25), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(237, 225, 25), Color.FromArgb(244, 234, 68), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
        End Select
        Me.Cursor = Cursors.Hand

    End Sub
End Class
Class lControlBlue
    Inherits ThemeControl
    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(14, 14)
        MaximumSize = New Size(15, 15)
    End Sub
    Overrides Sub PaintHook()


        Select Case MouseState
            Case State.MouseNone
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(34, 64, 160), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(34, 64, 160), Color.FromArgb(27, 81, 255), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(27, 81, 255), Color.FromArgb(34, 64, 160), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseDown
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(34, 64, 160), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(34, 64, 160), Color.FromArgb(27, 81, 255), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(27, 81, 255), Color.FromArgb(34, 64, 160), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseOver
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(110, 170, 255), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(110, 170, 255), Color.FromArgb(94, 128, 235), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(94, 128, 235), Color.FromArgb(110, 170, 255), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
        End Select
        Me.Cursor = Cursors.Hand

    End Sub
End Class

Class dControldark
    Inherits ThemeControl
    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(14, 14)
        MaximumSize = New Size(15, 15)
    End Sub
    Overrides Sub PaintHook()


        Select Case MouseState
            Case State.MouseNone
                DrawGradient(Color.FromArgb(30, 30, 30), Color.FromArgb(50, 50, 50), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(50, 50, 50), Color.FromArgb(130, 130, 130), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(130, 130, 130), Color.FromArgb(50, 50, 50), 4, 4, 7, 7, 90S)
                DrawBorders(New Pen(Color.FromArgb(105, 105, 105)), Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseDown
                DrawGradient(Color.FromArgb(30, 30, 30), Color.FromArgb(50, 50, 50), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(50, 50, 50), Color.FromArgb(130, 130, 130), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(130, 130, 130), Color.FromArgb(50, 50, 50), 4, 4, 7, 7, 90S)
                DrawBorders(New Pen(Color.FromArgb(105, 105, 105)), Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseOver
                DrawGradient(Color.FromArgb(30, 30, 30), Color.FromArgb(160, 160, 160), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(160, 160, 160), Color.FromArgb(130, 130, 130), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(130, 130, 130), Color.FromArgb(160, 160, 160), 4, 4, 7, 7, 90S)
                DrawBorders(New Pen(Color.FromArgb(105, 105, 105)), Pens.LightGray, New Rectangle(0, 0, 15, 15))
        End Select
        Me.Cursor = Cursors.Hand

    End Sub
End Class
Class dControlGray
    Inherits ThemeControl
    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(14, 14)
        MaximumSize = New Size(15, 15)
    End Sub
    Overrides Sub PaintHook()


        Select Case MouseState
            Case State.MouseNone
                DrawGradient(Color.FromArgb(60, 60, 60), Color.FromArgb(80, 80, 80), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(80, 80, 80), Color.FromArgb(140, 140, 140), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(140, 140, 140), Color.FromArgb(80, 80, 80), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseDown
                DrawGradient(Color.FromArgb(60, 60, 60), Color.FromArgb(80, 80, 80), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(80, 80, 80), Color.FromArgb(140, 140, 140), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(140, 140, 140), Color.FromArgb(80, 80, 80), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseOver
                DrawGradient(Color.FromArgb(60, 60, 60), Color.FromArgb(160, 160, 160), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(160, 160, 160), Color.FromArgb(130, 130, 130), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(130, 130, 130), Color.FromArgb(160, 160, 160), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
        End Select
        Me.Cursor = Cursors.Hand

    End Sub
End Class
Class dControlLight
    Inherits ThemeControl
    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(14, 14)
        MaximumSize = New Size(15, 15)
    End Sub
    Overrides Sub PaintHook()


        Select Case MouseState
            Case State.MouseNone
                DrawGradient(Color.FromArgb(60, 60, 60), Color.FromArgb(150, 150, 150), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(150, 150, 150), Color.FromArgb(230, 230, 230), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(230, 230, 230), Color.FromArgb(150, 150, 150), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseDown
                DrawGradient(Color.FromArgb(60, 60, 60), Color.FromArgb(150, 150, 150), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(150, 150, 150), Color.FromArgb(230, 230, 230), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(230, 230, 230), Color.FromArgb(150, 150, 150), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
            Case State.MouseOver
                DrawGradient(Color.FromArgb(60, 60, 60), Color.FromArgb(235, 235, 235), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(235, 235, 235), Color.FromArgb(215, 215, 215), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(215, 215, 215), Color.FromArgb(235, 235, 235), 4, 4, 7, 7, 90S)
                DrawBorders(Pens.Gray, Pens.LightGray, New Rectangle(0, 0, 15, 15))
        End Select
        Me.Cursor = Cursors.Hand

    End Sub
End Class

Class lTextBox
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
        BackColor = Color.FromArgb(230, 230, 230)
        BorderStyle = Windows.Forms.BorderStyle.FixedSingle
    End Sub

    Private Sub CustomPaint()
        Dim p As New Pen(Color.Gray)
        CreateGraphics.DrawLine(p, 0, 0, Width, 0)
        CreateGraphics.DrawLine(p, 0, Height - 1, Width, Height - 1)
        CreateGraphics.DrawLine(p, 0, 0, 0, Height - 1)
        CreateGraphics.DrawLine(p, Width - 1, 0, Width - 1, Height - 1)
    End Sub
End Class
Class lListBox
    Inherits ThemeControl
    Public WithEvents LBox As New ListBox
    Private __Items As String() = {""}
    Public Property Items() As String()
        Get
            Return __Items
            Invalidate()
        End Get
        Set(ByVal value As String())
            __Items = value
            LBox.Items.Clear()
            Invalidate()
            LBox.Items.AddRange(value)
            Invalidate()
        End Set
    End Property

    Public ReadOnly Property SelectedItem() As String
        Get
            Return LBox.SelectedItem
        End Get
    End Property

    Sub New()
        Controls.Add(LBox)
        Size = New Size(131, 101)

        LBox.BackColor = Color.FromArgb(240, 240, 240)
        LBox.BorderStyle = BorderStyle.None
        LBox.DrawMode = Windows.Forms.DrawMode.OwnerDrawVariable
        LBox.Location = New Point(2, 2)
        LBox.ForeColor = Color.Black
        LBox.ItemHeight = 20
        LBox.Items.Clear()
        LBox.IntegralHeight = False
        Invalidate()
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        LBox.Width = Width - 4
        LBox.Height = Height - 4
    End Sub

    Public Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(224, 240, 240))
        LBox.Size = New Size(Width - 3, Height - 4)

        DrawBorders(Pens.Gray, Pens.LightGray, ClientRectangle)
        DrawCorners(Me.Parent.BackColor, ClientRectangle)
    End Sub
    Sub DrawItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles LBox.DrawItem
        If e.Index < 0 Then Exit Sub
        e.DrawBackground()
        e.DrawFocusRectangle()
        If InStr(e.State.ToString, "Selected,") > 0 Then
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(224, 240, 240)), e.Bounds)
            Dim x2 As Rectangle = New Rectangle(e.Bounds.Location, New Size(e.Bounds.Width - 1, e.Bounds.Height))
            Dim x3 As Rectangle = New Rectangle(x2.Location, New Size(x2.Width, (x2.Height / 2) - 2))
            Dim G1 As New LinearGradientBrush(New Point(x2.X, x2.Y), New Point(x2.X, x2.Y + x2.Height), Color.FromArgb(60, 60, 60), Color.FromArgb(50, 50, 50))

            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(28, 126, 236)), x2) : G1.Dispose()
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(66, 165, 255)), x3)

            e.Graphics.DrawString(" " & LBox.Items(e.Index).ToString(), Me.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y + 2)
        Else
            e.Graphics.DrawString(" " & LBox.Items(e.Index).ToString(), Me.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y + 2)
        End If
    End Sub
    Sub AddRange(ByVal Items As Object())
        LBox.Items.Remove("")
        LBox.Items.AddRange(Items)
        Invalidate()
    End Sub
    Sub AddItem(ByVal Item As Object)
        LBox.Items.Remove("")
        LBox.Items.Add(Item)
        Invalidate()
    End Sub
End Class
Class tGroupBox
    Inherits ThemeContainerControl
    Sub New()
        AllowTransparent()
    End Sub
    Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(199, 199, 199))
        Me.BackColor = Color.FromArgb(196, 196, 196)

        Dim hb As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.FromArgb(15, Color.White), Color.Transparent)
        Dim hb2 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(35, Color.White), Color.Transparent)
        G.FillRectangle(New SolidBrush(Color.FromArgb(196, 196, 196)), 0, 0, Width, Height)
        DrawGradient(Color.FromArgb(196, 196, 196), Color.FromArgb(230, 230, 230), 0, 0, Width, 30, 270S)
        G.FillRectangle(hb, 1, 1, Width, Height)

        G.DrawRectangle(Pens.LightGray, 1, 1, Width - 3, 30)
        G.DrawRectangle(Pens.LightGray, 1, 1, Width - 3, 32)
        G.DrawRectangle(Pens.Gray, 0, 0, Width, 32)

        G.DrawString(Text, Me.Font, New SolidBrush(Me.ForeColor), 5, 10)

        DrawBorders(Pens.Gray, Pens.LightGray, ClientRectangle)
        DrawCorners(Me.Parent.BackColor, ClientRectangle)
    End Sub
End Class