#Region "Comments"
'Classes Theme, ThemeControl, and ThemeContainerControl were created by AeonHack
'   Creator: Aeonhack
'   Date: 4/23/2011
'   Site: *******
'   Version: 1.4
'
'SimplyGray theme created by Hydrogen
'
'Please include my name in the credits if you use this theme in a project
#End Region

Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Runtime.InteropServices

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

Class SimplyGrayTheme
    Inherits Theme

    Public Shared _BorderColor1 As Pen = Pens.DarkGray
    Public Shared _BorderColor2 As Pen = Pens.Black
    Private _SubText As String = "Subtext"

    Public Property InnerBorderColor() As Color
        Get
            Return _BorderColor1.Color
        End Get
        Set(ByVal value As Color)
            _BorderColor1 = New Pen(value)
            Invalidate()
        End Set
    End Property

    Public Property OuterBorderColor() As Color
        Get
            Return _BorderColor2.Color
        End Get
        Set(ByVal value As Color)
            _BorderColor2 = New Pen(value)
            Invalidate()
        End Set
    End Property

    Public Property SubText() As String
        Get
            Return _SubText
        End Get
        Set(ByVal value As String)
            _SubText = value
            Invalidate()
        End Set
    End Property

    Sub New()
        MoveHeight = 19
        TransparencyKey = Color.Fuchsia
        BackColor = Color.Gray
        ForeColor = Color.White
    End Sub

    Dim F As New System.Drawing.Font("Verdana", 8)
    Dim B As New SolidBrush(Color.DimGray)
    Dim Gr As Color = Color.Gray : Dim LG As Color = Color.LightGray : Dim Fc As Color = Color.Fuchsia : Dim TextColor As Color = Color.FromArgb(60, 60, 60)

    Overrides Sub PaintHook()
        G.Clear(Color.Gray)
        DrawGradient(LG, Gr, 0, 0, Width, 20, 90S)

        DrawBorders(_BorderColor2, _BorderColor1, ClientRectangle)
        DrawCorners(Fc, ClientRectangle)

        DrawText(HorizontalAlignment.Left, TextColor, 3, 0)
        G.DrawString(_SubText, F, B, 4, 19)
    End Sub
End Class

Class SimplyGrayButton
    Inherits ThemeControl

    Dim Gr As Color = Color.Gray : Dim LG As Color = Color.LightGray : Dim O As Color = Color.FromArgb(244, 244, 244) : Dim D As Color = Color.FromArgb(183, 183, 183)
    Dim P As Pen = Pens.DarkGray : Dim PG As Pen = Pens.Gray

    Sub New()
        ForeColor = Color.Black
    End Sub


    Overrides Sub PaintHook()
        G.Clear(Color.DarkGray)

        If MouseState = State.MouseNone Then
            DrawGradient(LG, Gr, 0, 0, Width, Height, 90)
        ElseIf MouseState = State.MouseOver Then
            DrawGradient(O, Gr, 0, 0, Width, Height, 90)
        ElseIf MouseState = State.MouseDown Then
            DrawGradient(D, Gr, 0, 0, Width, Height, 90)
        End If

        DrawText(HorizontalAlignment.Center, ForeColor, 0)

        DrawBorders(P, PG, ClientRectangle)
        DrawCorners(BackColor, ClientRectangle)
    End Sub
End Class


Class SimplyGrayLightProgressBar
    Inherits ThemeControl


    Private _Maximum As Integer = 100
    Property Maximum() As Integer
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
    Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal v As Integer)
            If v > _Maximum Then v = _Maximum

            _Value = v
            Invalidate()
        End Set
    End Property

    Sub New()
        AllowTransparent()
    End Sub

    Dim Gr As Color = Color.Gray : Dim P As Pen = Pens.DarkGray : Dim Br As Brush = Brushes.DarkGray : Dim T As Color = Color.Transparent


    Overrides Sub PaintHook()
        G.Clear(Gr)

        G.DrawRectangle(P, 0, 0, Width - 1, Height - 1)
        G.FillRectangle(Br, 0, 0, CInt((_Value / _Maximum) * Width), Height)

        DrawCorners(T, ClientRectangle)
    End Sub
End Class

Class SimplyGrayProgressBar
    Inherits ThemeControl


    Private _Maximum As Integer = 100
    Property Maximum() As Integer
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
    Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal v As Integer)
            If v > _Maximum Then v = _Maximum

            _Value = v
            Invalidate()
        End Set
    End Property

    Sub New()
        AllowTransparent()
    End Sub

    Dim Gr As Color = Color.Gray : Dim P As New Pen(Color.FromArgb(60, 60, 60)) : Dim Br As Brush = Brushes.DarkGray : Dim T As Color = Color.Transparent

    Overrides Sub PaintHook()
        G.Clear(Gr)

        G.FillRectangle(Br, 1, 1, CInt((_Value / _Maximum) * Width), Height - 2)
        G.DrawRectangle(P, 0, 0, Width - 1, Height - 1)

        DrawCorners(T, ClientRectangle)
    End Sub
End Class

Class SimplyGray3DBarLight
    Inherits ThemeControl

    Private _Maximum As Integer = 100
    Property Maximum() As Integer
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
    Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal v As Integer)
            If v > _Maximum Then v = _Maximum

            _Value = v
            Invalidate()
        End Set
    End Property

    Sub New()
        AllowTransparent()
        ForeColor = Color.White
    End Sub

    Dim Gr As Color = Color.Gray : Dim SBr As Brush = Brushes.Silver : Dim P As Pen = Pens.DarkGray : Dim Br As Brush = Brushes.DarkGray : Dim T As Color = Color.Transparent


    Overrides Sub PaintHook()
        G.Clear(Gr)

        G.DrawRectangle(P, 0, 0, Width - 1, Height - 1)
        G.FillRectangle(Br, 0, 0, CInt((_Value / _Maximum) * Width), Height)
        G.FillRectangle(SBr, 0, 0, CInt((_Value / _Maximum) * Width), Height \ 2)

        DrawText(HorizontalAlignment.Center, ForeColor, 0)

        DrawCorners(T, ClientRectangle)
    End Sub
End Class

Class SimplyGray3DBar
    Inherits ThemeControl


    Private _Maximum As Integer = 100
    Property Maximum() As Integer
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
    Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal v As Integer)
            If v > _Maximum Then v = _Maximum

            _Value = v
            Invalidate()
        End Set
    End Property

    Sub New()
        AllowTransparent()
        ForeColor = Color.White
    End Sub

    Dim Gr As Color = Color.Gray : Dim SBr As Brush = Brushes.Silver : Dim P As New Pen(Color.FromArgb(60, 60, 60)) : Dim Br As Brush = Brushes.DarkGray : Dim T As Color = Color.Transparent

    Overrides Sub PaintHook()
        G.Clear(Gr)

        G.FillRectangle(Br, 1, 1, CInt((_Value / _Maximum) * Width), Height - 2)
        G.FillRectangle(SBr, 1, 1, CInt((_Value / _Maximum) * Width), (Height - 2) \ 2)
        G.DrawRectangle(P, 0, 0, Width - 1, Height - 1)

        DrawText(HorizontalAlignment.Center, ForeColor, 0)

        DrawCorners(T, ClientRectangle)
    End Sub
End Class

Class SimplyGraySeperator
    Inherits ThemeControl

    Private _Color1 As Color = Color.DarkGray
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value
        End Set
    End Property

    Private _Color2 As Color = Color.FromArgb(50, 50, 50)
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value
        End Set
    End Property

    Sub New()
        AllowTransparent()
        BackColor = Color.Transparent
        Dim S As New Size(150, 10) : Size = S
    End Sub

    Dim Gr As Color = Color.Gray : Dim DGr As Color = Color.DarkGray : Dim T As Color = Color.Transparent : Dim PDGr As Pen = Pens.DarkGray

    Overrides Sub PaintHook()
        G.Clear(Gr)

        G.DrawLine(New Pen(_Color1), 0, Height \ 2, Width, Height \ 2)
        G.DrawLine(New Pen(_Color2), 0, Height \ 2 + 1, Width, Height \ 2 + 1)
    End Sub
End Class

Class SimplyGrayMenuButton
    Inherits ThemeControl

    Sub New()
        AllowTransparent()
        ForeColor = Color.White
        Dim S As New Size(15, 20) : Size = S
    End Sub

    Dim T As Color = Color.Transparent : Dim LG As Color = Color.LightGray : Dim Gr As Color = Color.Gray
    Dim P As Pen = Pens.DarkGray : Dim Pb As Pen = Pens.Black

    Overrides Sub PaintHook()
        G.Clear(Color.DarkGray)

        DrawGradient(LG, Gr, 0, 0, Width, Height, 90)

        G.DrawLine(Pb, 0, 0, Width, 0)
        G.DrawLine(P, 0, 1, Width, 1)

        DrawText(HorizontalAlignment.Center, ForeColor, 0)
    End Sub
End Class