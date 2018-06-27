'             Thief3 VB.Net Theme

' #############################################
' #          Creator: iZ3RO (ZerO)            #
' #           Date: 30 July, 2011             #
' #         Site: ******************          #
' #               Version: 3.0                #
' #############################################

'     Credits to AeonHack for the theme base


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
    Protected Sub DrawText(ByVal a As HorizontalAlignment, ByVal c As Color, ByVal x As Integer, ByVal y As Integer, ByVal f As Font)
        If String.IsNullOrEmpty(Text) Then Return
        _Size = G.MeasureString(Text, Font).ToSize
        _Brush = New SolidBrush(c)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(Text, f, _Brush, x, _MoveHeight \ 2 - _Size.Height \ 2 + y)
            Case HorizontalAlignment.Right
                G.DrawString(Text, f, _Brush, Width - _Size.Width - x, _MoveHeight \ 2 - _Size.Height \ 2 + y)
            Case HorizontalAlignment.Center
                G.DrawString(Text, f, _Brush, Width \ 2 - _Size.Width \ 2 + x, _MoveHeight \ 2 - _Size.Height \ 2 + y)
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


Class Thief3Theme
    Inherits Theme

    Private _bStripes As Boolean
    Public Property HatchEnable() As Boolean
        Get
            Return _bStripes
        End Get
        Set(ByVal v As Boolean)
            _bStripes = v
            Invalidate()
        End Set
    End Property
    Private _aColor As Color
    Public Property AccentColor() As Color
        Get
            Return _aColor
        End Get
        Set(ByVal value As Color)
            _aColor = value
            Invalidate()
        End Set
    End Property
    Private _cStyle As Boolean
    Public Property DarkTheme() As Boolean
        Get
            Return _cStyle
        End Get
        Set(ByVal v As Boolean)
            _cStyle = v
            Invalidate()
        End Set
    End Property
    Private _tAlign As HorizontalAlignment
    Public Property TitleTextAlign() As HorizontalAlignment
        Get
            Return _tAlign
        End Get
        Set(ByVal v As HorizontalAlignment)
            _tAlign = v
            Invalidate()
        End Set
    End Property
    Sub New()
        MoveHeight = 19
        TransparencyKey = Color.Fuchsia
        Me.Resizable = False
        Me.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Regular)
        'Custom Settings
        HatchEnable = True
        AccentColor = Color.DodgerBlue
        DarkTheme = True
        TitleTextAlign = Left
    End Sub
    Public Overrides Sub PaintHook()
        Dim ClientPtA, ClientPtB, GradA, GradB As Integer
        Dim PenColor As Pen
        Select Case HatchEnable
            Case True
                ClientPtA = 38
                ClientPtB = 37
            Case False
                ClientPtA = 21
                ClientPtB = -1
        End Select
        Select Case DarkTheme
            Case True
                GradA = 51
                GradB = 30
                PenColor = Pens.Black
            Case False
                GradA = 200
                GradB = 160
                PenColor = Pens.DimGray
        End Select
        G.Clear(Color.FromArgb(GradA, GradA, GradA))
        DrawGradient(Color.FromArgb(GradA, GradA, GradA), Color.FromArgb(GradB, GradB, GradB), 0, 0, Width, 19, 90S)
        Select Case HatchEnable
            Case True
                DrawGradient(Color.FromArgb(GradB, GradB, GradB), Color.FromArgb(GradA, GradA, GradA), 0, 19, Width, 18, 90S)
        End Select
        G.DrawLine(PenColor, 0, 20, Width, 20)
        G.DrawLine(PenColor, 0, ClientPtB, Width, ClientPtB)
        Select Case HatchEnable
            Case True
                For I As Integer = 0 To Width + 17 Step 4
                    G.DrawLine(PenColor, I, 21, I - 17, ClientPtA)
                    G.DrawLine(PenColor, I - 1, 21, I - 18, ClientPtA)
                Next
        End Select
        G.DrawLine(New Pen(New SolidBrush(AccentColor)), 0, ClientPtA, Width, ClientPtA)
        G.DrawLine(New Pen(New SolidBrush(AccentColor)), 0, Height - 2, Width, Height - 2)
        G.DrawLine(New Pen(New SolidBrush(AccentColor)), 1, ClientPtA, 1, Height - 2)
        G.DrawLine(New Pen(New SolidBrush(AccentColor)), Width - 2, ClientPtA, Width - 2, Height - 1)
        G.DrawString(".", Me.Parent.Font, Brushes.Black, -2, Height - 12)
        G.DrawString(".", Me.Parent.Font, Brushes.Black, Width - 5, Height - 12)
        DrawText(TitleTextAlign, AccentColor, 6, 0)
        DrawBorders(Pens.Black, Pens.Transparent, ClientRectangle)
        DrawCorners(Color.Fuchsia, ClientRectangle)
    End Sub

End Class
Class Thief3TopButton
    Inherits ThemeControl
    Private _aColor As Color
    Public Property AccentColor() As Color
        Get
            Return _aColor
        End Get
        Set(ByVal value As Color)
            _aColor = value
            Invalidate()
        End Set
    End Property
    Private _cStyle As Boolean
    Public Property DarkTheme() As Boolean
        Get
            Return _cStyle
        End Get
        Set(ByVal v As Boolean)
            _cStyle = v
            Invalidate()
        End Set
    End Property
    Sub New()
        Size = New Size(11, 5)
        AccentColor = Color.DodgerBlue
        DarkTheme = True
    End Sub
    Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(102, 102, 102))
        Dim GradA, GradB, GradC As Integer
        Select Case DarkTheme
            Case True
                GradA = 61
                GradB = 49
                GradC = 51
            Case False
                GradA = 200
                GradB = 155
                GradC = 155
        End Select
        Select Case MouseState
            Case State.MouseNone
                DrawGradient(Color.FromArgb(GradA, GradA, GradA), Color.FromArgb(GradB, GradB, GradB), 0, 0, Width, Height, 90S)
            Case State.MouseOver
                DrawGradient(Color.FromArgb(GradA, GradA, GradA), Color.FromArgb(GradB, GradB, GradB), 0, 0, Width, Height, 90S)
            Case State.MouseDown
                DrawGradient(Color.FromArgb(GradB, GradB, GradB), Color.FromArgb(GradA, GradA, GradA), 0, 0, Width, Height, 90S)
        End Select
        DrawBorders(New Pen(New SolidBrush(AccentColor)), Pens.Transparent, ClientRectangle)
        DrawCorners(Color.FromArgb(GradC, GradC, GradC), ClientRectangle)
    End Sub
End Class
Class Thief3Button
    Inherits ThemeControl
    Private _aColor As Color
    Public Property AccentColor() As Color
        Get
            Return _aColor
        End Get
        Set(ByVal value As Color)
            _aColor = value
            Invalidate()
        End Set
    End Property
    Private _cStyle As Boolean
    Public Property DarkTheme() As Boolean
        Get
            Return _cStyle
        End Get
        Set(ByVal v As Boolean)
            _cStyle = v
            Invalidate()
        End Set
    End Property
    Sub New()
        Select Case DarkTheme
            Case True
                BackColor = Color.FromArgb(51, 51, 51)
            Case False
                BackColor = Color.FromArgb(200, 200, 200)
        End Select
        AccentColor = Color.DodgerBlue
    End Sub
    Overrides Sub PaintHook()
        Dim GradA, GradB As Integer
        Dim PenColor As Pen
        Select Case DarkTheme
            Case True
                GradA = 61
                GradB = 49
                PenColor = Pens.DimGray
            Case False
                GradA = 200
                GradB = 155
                PenColor = Pens.White
        End Select
        G.Clear(Color.FromArgb(102, 102, 102))
        Select Case MouseState
            Case State.MouseNone
                DrawGradient(Color.FromArgb(GradA, GradA, GradA), Color.FromArgb(GradB, GradB, GradB), 0, 0, Width, Height, 90S)
                G.DrawLine(PenColor, 1, 1, Width - 1, 1)
            Case State.MouseOver
                DrawGradient(Color.FromArgb(GradA, GradA, GradA), Color.FromArgb(GradB, GradB, GradB), 0, 0, Width, Height, 90S)
                G.DrawLine(PenColor, 1, 1, Width - 1, 1)
            Case State.MouseDown
                DrawGradient(Color.FromArgb(GradB, GradB, GradB), Color.FromArgb(GradA, GradA, GradA), 0, 0, Width, Height, 90S)
                G.DrawLine(PenColor, 1, Height - 2, Width - 1, Height - 2)
        End Select
        DrawBorders(Pens.Black, Pens.Transparent, ClientRectangle)
        DrawText(HorizontalAlignment.Center, AccentColor, -1, 0)
    End Sub
End Class
Class Thief3Check
    Inherits ThemeControl
    Private _cStyle As Boolean
    Public Property DarkTheme() As Boolean
        Get
            Return _cStyle
        End Get
        Set(ByVal v As Boolean)
            _cStyle = v
            Invalidate()
        End Set
    End Property
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
    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(16, 16)
        MaximumSize = New Size(600, 16)

        DarkTheme = True
        CheckedState = False
    End Sub
    Public Overrides Sub PaintHook()
        Dim Grad As Integer
        Dim FontColor As Color
        Select Case DarkTheme
            Case True
                Grad = 51
                FontColor = Color.White
            Case False
                Grad = 200
                FontColor = Color.Black
        End Select
        G.Clear(Color.FromArgb(Grad, Grad, Grad))
        Select Case CheckedState
            Case True
                'DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(38, 38, 38), 0, 0, 15, 15, 90S)
                DrawGradient(Color.FromArgb(132, 192, 240), Color.FromArgb(78, 123, 168), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(98, 159, 220), Color.FromArgb(62, 102, 147), 4, 4, 7, 7, 90S)
            Case False
                DrawGradient(Color.FromArgb(80, 80, 80), Color.FromArgb(80, 80, 80), 0, 0, 15, 15, 90S)
        End Select
        G.DrawRectangle(Pens.Black, 0, 0, 14, 14)
        G.DrawRectangle(Pens.DimGray, 1, 1, 12, 12)
        DrawText(HorizontalAlignment.Left, FontColor, 17, 0)
    End Sub
    Sub changeCheck() Handles Me.Click
        Select Case CheckedState
            Case True
                CheckedState = False
            Case False
                CheckedState = True
        End Select
    End Sub
End Class
Class Thief3ProgressBar
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
            Select Case _Value
                Case 0
                    Return 1
                Case Else
                    Return _Value
            End Select
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
        'Fill
        Select Case _Value
            Case Is > 2
                DrawGradient(Color.FromArgb(132, 192, 240), Color.FromArgb(78, 123, 168), 3, 3, CInt(_Value / _Maximum * Width) - 6, Height - 6, 90S)
                DrawGradient(Color.FromArgb(98, 159, 220), Color.FromArgb(62, 102, 147), 4, 4, CInt(_Value / _Maximum * Width) - 8, Height - 8, 90S)
            Case Is > 0
                DrawGradient(Color.FromArgb(132, 192, 240), Color.FromArgb(78, 123, 168), 3, 3, CInt(_Value / _Maximum * Width), Height - 6, 90S)
                DrawGradient(Color.FromArgb(98, 159, 220), Color.FromArgb(62, 102, 147), 4, 4, CInt(_Value / _Maximum * Width) - 2, Height - 8, 90S)
        End Select

        'Borders
        G.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1)
        G.DrawRectangle(Pens.Gray, 1, 1, Width - 3, Height - 3)
    End Sub
End Class