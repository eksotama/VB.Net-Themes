'Creator: Slurms Makenzi
'Thanks Aeon for theme class!
'Date: 7/7/2011
'Site: ***********
'Version: 1.0
'Thanks xZ3ROxPROJ3CTx

'

'\\\\\\\\\\\\\\\Use Credits Were Credits Are Needed///////////////////

'

#Region "Imports"
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Runtime.InteropServices
#End Region
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
Public Class Draw
    Shared Sub Gradient(ByVal g As Graphics, ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        Dim R As New Rectangle(x, y, width, height)
        Using T As New LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical)
            g.FillRectangle(T, R)
        End Using
    End Sub
    Shared Sub Gradient(ByVal g As Graphics, ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        Using T As New LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical)
            g.FillRectangle(T, R)
        End Using
    End Sub
    Shared Sub Blend(ByVal g As Graphics, ByVal c1 As Color, ByVal c2 As Color, ByVal c3 As Color, ByVal c As Single, ByVal d As Integer, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        Dim v As New ColorBlend(3)
        V.Colors = New Color() {c1, c2, c3}
        V.Positions = New Single() {0, c, 1}
        Dim R As New Rectangle(x, y, width, height)
        Using T As New LinearGradientBrush(R, c1, c1, CType(d, LinearGradientMode))
            T.InterpolationColors = v : g.FillRectangle(T, R)
        End Using
    End Sub
    Shared Function RoundedRectangle(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal cornerwidth As Integer, ByVal PenWidth As Integer) As GraphicsPath
        Dim p As New GraphicsPath
        p.StartFigure()
        p.AddArc(New Rectangle(x, y, cornerwidth, cornerwidth), 180, 90)
        p.AddLine(cornerwidth, y, width - cornerwidth - PenWidth, y)

        p.AddArc(New Rectangle(width - cornerwidth - PenWidth, y, cornerwidth, cornerwidth), -90, 90)
        p.AddLine(width - PenWidth, cornerwidth, width - PenWidth, height - cornerwidth - PenWidth)

        p.AddArc(New Rectangle(width - cornerwidth - PenWidth, height - cornerwidth - PenWidth, cornerwidth, cornerwidth), 0, 90)
        p.AddLine(width - cornerwidth - PenWidth, height - PenWidth, cornerwidth, height - PenWidth)

        p.AddArc(New Rectangle(x, height - cornerwidth - PenWidth, cornerwidth, cornerwidth), 90, 90)
        p.CloseFigure()

        Return p
    End Function

    Shared Sub BackGround(ByVal width As Integer, ByVal height As Integer, ByVal G As Graphics)

        Dim P1 As Color = Color.FromArgb(29, 25, 22)
        Dim P2 As Color = Color.FromArgb(35, 31, 28)

        For y As Integer = 0 To height Step 4
            For x As Integer = 0 To width Step 4
                G.FillRectangle(New SolidBrush(P1), New Rectangle(x, y, 1, 1))
                G.FillRectangle(New SolidBrush(P2), New Rectangle(x, y + 1, 1, 1))
                Try
                    G.FillRectangle(New SolidBrush(P1), New Rectangle(x + 2, y + 2, 1, 1))
                    G.FillRectangle(New SolidBrush(P2), New Rectangle(x + 2, y + 3, 1, 1))
                Catch
                End Try
            Next
        Next
    End Sub
End Class
Class PurityxTheme
    Inherits Theme

    Sub New()
        Resizable = False
        BackColor = Color.FromKnownColor(KnownColor.Control)
        MoveHeight = 25
        TransparencyKey = Color.Fuchsia
    End Sub

    Public Overrides Sub PaintHook()
        G.Clear(BackColor)   ' Clear the form first

        'DrawGradient(Color.FromArgb(64, 64, 64), Color.FromArgb(32, 32, 32), 0, 0, Width, Height, 90S)   ' Form Gradient
        G.Clear(Color.FromArgb(60, 60, 60))
        DrawGradient(Color.FromArgb(45, 40, 45), Color.FromArgb(32, 32, 32), 0, 0, Width, 25, 90S)   ' Form Top Bar

        G.DrawLine(Pens.Black, 0, 25, Width, 25)   ' Top Line
        'G.DrawLine(Pens.Black, 0, Height - 25, Width, Height - 25)   ' Bottom Line

        DrawCorners(Color.Fuchsia, ClientRectangle)   ' Then draw some clean corners
        DrawBorders(Pens.Black, Pens.DimGray, ClientRectangle)   ' Then we draw our form borders

        DrawText(HorizontalAlignment.Left, Color.Red, 7, 1)   ' Finally, we draw our text
    End Sub
End Class ' Theme Code
Class PurityxButton
    Inherits ThemeControl

    Overrides Sub PaintHook()
        Select Case MouseState
            Case State.MouseNone
                G.Clear(Color.Red)
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(38, 38, 38), 0, 0, Width, Height, 90S)
            Case State.MouseOver
                G.Clear(Color.Red)
                DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(38, 38, 38), 0, 0, Width, Height, 90S)
            Case State.MouseDown
                G.Clear(Color.DarkRed)
                DrawGradient(Color.FromArgb(38, 38, 38), Color.FromArgb(62, 62, 62), 0, 0, Width, Height, 90S)
        End Select

        DrawBorders(Pens.Black, Pens.DimGray, ClientRectangle)   ' Form Border
        DrawCorners(Color.Black, ClientRectangle)   ' Clean Corners
        DrawText(HorizontalAlignment.Center, Color.Red, 0)
    End Sub
End Class ' Button Code
Public Class PurityxLabel
    Inherits Label
    Sub New()
        Font = New Font("Arial", 8)
        ForeColor = Color.Red
        BackColor = Color.Transparent
    End Sub
End Class ' Label Code
Public Class PurityxSeperator
    Inherits Control

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                g.Clear(BackColor)

                Dim P1 As Color = Color.FromArgb(255, 255, 255)
                Dim P2 As Color = Color.FromArgb(255, 255, 255)
                g.FillRectangle(New SolidBrush(Color.FromArgb(62, 62, 62)), New Rectangle(0, 0, Width, Height))


                Dim GRec As New Rectangle(0, Height / 2, Width / 5, 2)
                Using GBrush As LinearGradientBrush = New LinearGradientBrush(GRec, Color.Transparent, P2, LinearGradientMode.Horizontal)
                    g.FillRectangle(GBrush, GRec)
                End Using
                g.DrawLine(New Pen(P2, 2), New Point(GRec.Width, GRec.Y + 1), New Point(Width - GRec.Width + 1, GRec.Y + 1))

                GRec = New Rectangle(Width - (Width / 5), Height / 2, Width / 5, 2)
                Using GBrush As LinearGradientBrush = New LinearGradientBrush(GRec, P2, Color.Transparent, LinearGradientMode.Horizontal)
                    g.FillRectangle(GBrush, GRec)
                End Using
                e.Graphics.DrawImage(b, 0, 0)
            End Using
        End Using
        MyBase.OnPaint(e)
    End Sub
End Class ' Seperator Code
Class PurityxGroupbox
    Inherits Panel
    Dim Bg As Color = Color.FromArgb(62, 62, 62)
    Dim PC2 As Color = Color.FromArgb(204, 204, 204)
    Dim FC As Color = Color.FromArgb(204, 204, 204)
    Dim p As Pen
    Dim sb As SolidBrush
    Sub New()
        BackColor = Bg
    End Sub
    Dim _t As String = ""
    Public Property Header() As String
        Get
            Return _t
        End Get
        Set(ByVal value As String)
            _t = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                p = New Pen(PC2)
                sb = New SolidBrush(Bg)
                Dim M As SizeF = g.MeasureString(_t, Font)
                Dim Outline As GraphicsPath = Draw.RoundedRectangle(0, M.Height / 2, Width - 1, Height - 1, 10, 1)
                g.Clear(BackColor)
                g.FillRectangle(sb, New Rectangle(0, M.Height / 2, Width - 1, Height - 1))
                g.DrawPath(p, Outline)

                g.FillRectangle(sb, New Rectangle(10, (M.Height / 2) - 2, M.Width + 10, M.Height))
                sb = New SolidBrush(FC)
                g.DrawString(MyBase.Text, MyBase.Font, Brushes.White, 7, 1)
                e.Graphics.DrawImage(b, 0, 0)
            End Using
        End Using
        MyBase.OnPaint(e)
    End Sub
End Class ' GroupBox Code