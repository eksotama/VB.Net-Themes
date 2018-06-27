

Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Drawing.Text
 
'     DO NOT REMOVE CREDITS! IF YOU USE PLEASE CREDIT!    '''
 
''' <summary>
''' Elegant GDI Theme
''' Creator: Xertz (HF)
''' Version: 1.0
''' Control Count: 17
''' Date Created: 25/12/2013
''' UID: 1602992
''' For any bugs / errors, PM me.
''' </summary>
''' <remarks></remarks>
 
Module DrawHelpers
 
#Region "Functions"
 
    Public Function RoundRectangle(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
 
    Public Function RoundRect(x!, y!, w!, h!, Optional r! = 0.3, Optional TL As Boolean = True, Optional TR As Boolean = True, Optional BR As Boolean = True, Optional BL As Boolean = True) As GraphicsPath
        Dim d! = Math.Min(w, h) * r, xw = x + w, yh = y + h
        RoundRect = New GraphicsPath
 
        With RoundRect
            If TL Then .AddArc(x, y, d, d, 180, 90) Else .AddLine(x, y, x, y)
            If TR Then .AddArc(xw - d, y, d, d, 270, 90) Else .AddLine(xw, y, xw, y)
            If BR Then .AddArc(xw - d, yh - d, d, d, 0, 90) Else .AddLine(xw, yh, xw, yh)
            If BL Then .AddArc(x, yh - d, d, d, 90, 90) Else .AddLine(x, yh, x, yh)
 
            .CloseFigure()
        End With
    End Function
 
    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum
 
#End Region
 
End Module
 
Public Class ElegantThemeContainer
    Inherits ContainerControl
 
#Region "Declarations"
 
    Private _FontSize As Integer = 12
    Private State As MouseState = MouseState.None
    Private MouseXLoc As Integer
    Private MouseYLoc As Integer
    Private CaptureMovement As Boolean = False
    Private MoveHeight As Integer = 26
    Private MouseP As Point = New Point(0, 0)
    Private _ControlBoxColour As Color = Color.FromArgb(123, 150, 106)
    Private _ControlBaseColour As Color = Color.FromArgb(247, 249, 248)
    Private _TopStripBorderColour As Color = Color.FromArgb(183, 210, 166)
    Private _TopStripColour As Color = Color.FromArgb(240, 242, 241)
    Private _BaseColour As Color = Color.FromArgb(250, 250, 250)
    Private _BorderColour As Color = Color.FromArgb(10, 10, 10)
    Private _ControlBoxButtonSplitColour As Color = Color.FromArgb(210, 210, 210)
    Private _IconColour As Color = Color.FromArgb(90, 90, 90)
    Private _AllowClose As Boolean = True
    Private _AllowMinimize As Boolean = True
    Private _AllowMaximize As Boolean = True
    Private _Font As Font = New Font("Segoe UI", _FontSize)
 
#End Region
 
#Region "Properties & Events"
 
    <Category("Colours")>
    Public Property IconColour As Color
        Get
            Return _IconColour
        End Get
        Set(value As Color)
            _IconColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property ControlBoxButtonSplitColour As Color
        Get
            Return _ControlBoxButtonSplitColour
        End Get
        Set(value As Color)
            _ControlBoxButtonSplitColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property ControlBaseColour As Color
        Get
            Return _ControlBaseColour
        End Get
        Set(value As Color)
            _ControlBaseColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property ControlBoxColour As Color
        Get
            Return _ControlBoxColour
        End Get
        Set(value As Color)
            _ControlBoxColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property TopStripBorderColour As Color
        Get
            Return _TopStripBorderColour
        End Get
        Set(value As Color)
            _TopStripBorderColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property TopStripColour As Color
        Get
            Return _TopStripColour
        End Get
        Set(value As Color)
            _TopStripColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property
    Public Property AllowClose As Boolean
        Get
            Return _AllowClose
        End Get
        Set(value As Boolean)
            _AllowClose = value
        End Set
    End Property
    Public Property AllowMinimize As Boolean
        Get
            Return _AllowMinimize
        End Get
        Set(value As Boolean)
            _AllowMinimize = value
        End Set
    End Property
    Public Property AllowMaximize As Boolean
        Get
            Return _AllowMaximize
        End Get
        Set(value As Boolean)
            _AllowMaximize = value
        End Set
    End Property
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : CaptureMovement = False
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        MouseXLoc = e.Location.X
        MouseYLoc = e.Location.Y
        Invalidate()
        If CaptureMovement Then
            Parent.Location = MousePosition - MouseP
        End If
        If e.X < Width - 90 AndAlso e.Y > 28 Then Cursor = Cursors.Arrow Else Cursor = Cursors.Hand
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If MouseXLoc > Width - 26 AndAlso MouseYLoc < 25 Then
            If _AllowClose Then
                Environment.Exit(0)
            End If
        ElseIf MouseXLoc > Width - 56 AndAlso MouseXLoc < Width - 26 AndAlso MouseYLoc < 25 Then
            If _AllowMaximize Then
                Select Case FindForm.WindowState
                    Case FormWindowState.Maximized
                        FindForm.WindowState = FormWindowState.Normal
                    Case FormWindowState.Normal
                        FindForm.WindowState = FormWindowState.Maximized
                End Select
            End If
        ElseIf MouseXLoc > Width - 84 AndAlso MouseXLoc < Width - 56 AndAlso MouseYLoc < 25 Then
            If _AllowMinimize Then
                Select Case FindForm.WindowState
                    Case FormWindowState.Normal
                        FindForm.WindowState = FormWindowState.Minimized
                    Case FormWindowState.Maximized
                        FindForm.WindowState = FormWindowState.Minimized
                End Select
            End If
        ElseIf e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width - 84, MoveHeight).Contains(e.Location) Then
            CaptureMovement = True : MouseP = e.Location
        Else
            Focus()
        End If
        Invalidate()
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        Me.DoubleBuffered = True
        Me.BackColor = _BaseColour
        Me.Dock = DockStyle.Fill
    End Sub
 
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        ParentForm.FormBorderStyle = FormBorderStyle.None
        ParentForm.AllowTransparency = False
        ParentForm.TransparencyKey = Color.Fuchsia
        ParentForm.FindForm.StartPosition = FormStartPosition.CenterScreen
        Dock = DockStyle.Fill
        Invalidate()
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
            Dim LeftBorderPoints() As Point = {New Point(0, .MeasureString(Text, _Font).Height + 10), New Point(.MeasureString(Text, _Font).Width + 3, .MeasureString(Text, _Font).Height + 10), New Point(.MeasureString(Text, _Font).Width + 16, 0)}
            Dim LeftBorderPoints1() As Point = {New Point(0, 0), New Point(0, .MeasureString(Text, _Font).Height + 10), New Point(.MeasureString(Text, _Font).Width + 3, .MeasureString(Text, _Font).Height + 10), New Point(.MeasureString(Text, _Font).Width + 16, 0)}
            Dim MiddleStripPoints() As Point = {New Point(.MeasureString(Text, _Font).Width + 4, .MeasureString(Text, _Font).Height + 3), New Point(.MeasureString(Text, _Font).Width + 16, 0), New Point(Width - 84, 0), New Point(Width - 84, .MeasureString(Text, _Font).Height + 3)}
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, Width, Height))
            .FillPolygon(New SolidBrush(_TopStripBorderColour), MiddleStripPoints)
            .FillPolygon(New SolidBrush(_TopStripColour), LeftBorderPoints1)
            .FillRectangle(New SolidBrush(_ControlBaseColour), Width - 84, 0, Width, 25)
            .DrawLine(New Pen(_IconColour, 2), Width - 19, 7, Width - 7, 19)
            .DrawLine(New Pen(_IconColour, 2), Width - 19, 19, Width - 7, 7)
            .DrawLine(New Pen(_IconColour, 2), Width - 76, 18, Width - 64, 18)
            .DrawLine(New Pen(_IconColour, 2), Width - 48, 19, Width - 48, 6)
            .DrawLine(New Pen(_IconColour, 2), Width - 48, 19, Width - 34, 19)
            .DrawLine(New Pen(_IconColour, 4), Width - 48, 8, Width - 34, 8)
            .DrawLine(New Pen(_IconColour, 2), Width - 34, 6, Width - 34, 19)
            .DrawLine(New Pen(_ControlBoxColour), Width, 25, Width - 84, 25)
            .DrawLine(New Pen(_TopStripBorderColour), Width - 84, 25, Width - 84, 0)
            .DrawLine(New Pen(_ControlBoxButtonSplitColour), Width - 56, 25, Width - 56, 0)
            .DrawLine(New Pen(_ControlBoxButtonSplitColour), Width - 26, 25, Width - 26, 0)
            .DrawLines(New Pen(_TopStripBorderColour, 2), LeftBorderPoints)
            .DrawRectangle(New Pen(_BorderColour), New Rectangle(0, 0, Width, Height))
            .DrawString(Text, _Font, New SolidBrush(Color.FromArgb(120, 120, 120)), New Point(5, 5))
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeTextBox
    Inherits Control
 
#Region "Declarations"
    Private _BaseColour As Color = Color.FromArgb(255, 255, 255)
    Private _BorderColour As Color = Color.FromArgb(163, 190, 146)
    Private _LineColour As Color = Color.FromArgb(221, 221, 221)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _LeftPolygonColour As Color = Color.FromArgb(248, 250, 249)
    Private WithEvents TB As TextBox
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean
    Private _UseSystemPasswordChar As Boolean
    Private _Multiline As Boolean
    Private State As MouseState = MouseState.None
    Private _Pictures As Pictures = Pictures.Username
#End Region
 
#Region "Properties & Events"
 
    Public Sub SelectAll()
        TB.Focus()
        TB.SelectAll()
        Invalidate()
    End Sub
 
    <Category("Control")>
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property LineColour As Color
        Get
            Return _LineColour
        End Get
        Set(value As Color)
            _LineColour = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property LeftPolygonColour As Color
        Get
            Return _LeftPolygonColour
        End Get
        Set(value As Color)
            _LeftPolygonColour = value
        End Set
    End Property
 
    Public Property Picture As Pictures
        Get
            Return _Pictures
        End Get
        Set(value As Pictures)
            _Pictures = value
        End Set
    End Property
 
    <Category("Control")>
    Enum Pictures
        Username
        Password
        None
    End Enum
 
    <Category("Control")>
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
 
    <Category("Control")>
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
 
    <Category("Control")>
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
 
    <Category("Control")>
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
 
    <Category("Control")>
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
 
    <Category("Control")>
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
 
    <Category("Control")>
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
        If _Pictures = Pictures.Password Or _Pictures = Pictures.Username Then
            TB.Location = New Point(40, 6)
            TB.Width = Width - 45
        Else
            TB.Location = New Point(5, 6)
            TB.Width = Width - 10
        End If
        If _Multiline Then
            TB.Height = Height - 11
        Else
            Height = TB.Height + 11
        End If
        MyBase.OnResize(e)
    End Sub
 
#End Region
 
#Region "Mouse States"
 
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
               ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
               ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        TB = New Windows.Forms.TextBox
        TB.Height = 190
        TB.Font = New Font("Segoe UI", 9)
        TB.Text = Text
        TB.BackColor = _BaseColour
        TB.ForeColor = _TextColour
        TB.MaxLength = _MaxLength
        TB.Multiline = False
        TB.ReadOnly = _ReadOnly
        TB.UseSystemPasswordChar = _UseSystemPasswordChar
        TB.BorderStyle = BorderStyle.None
        TB.Location = New Point(40, 6)
        TB.Width = Width - 45
        AddHandler TB.TextChanged, AddressOf OnBaseTextChanged
        AddHandler TB.KeyDown, AddressOf OnBaseKeyDown
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim GP As GraphicsPath
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            Dim P() As Point = {New Point(0, 0), New Point(28, 0), New Point(28, Height / 2 - 6), New Point(34, Height / 2), _
                               New Point(28, Height / 2 + 6), New Point(28, Height), New Point(0, Height)}
            Dim P1() As Point = {New Point(28, 0), New Point(28, Height / 2 - 6), New Point(34, Height / 2), _
                               New Point(28, Height / 2 + 6), New Point(28, Height)}
            GP = RoundRectangle(Base, 1)
            If _Pictures = Pictures.Username Then
                TB.Location = New Point(40, 6)
                TB.Width = Width - 45
                .FillPath(New SolidBrush(_BaseColour), GP)
                .FillPolygon(New SolidBrush(_LeftPolygonColour), P)
                .DrawLines(New Pen(_LineColour, 1), P1)
                .DrawPath(New Pen((_BorderColour), 2), GP)
                .FillEllipse(New SolidBrush(_TextColour), New Rectangle(10, 5, 8, 10))
                Dim P2() As Point = {New Point(5, 22), New Point(9, 17)}
                Dim SecondLine() As Point = {New Point(19, 17), New Point(23, 22)}
                .DrawLines(New Pen(_TextColour), P2)
                .DrawLines(New Pen(_TextColour), SecondLine)
                Dim CurvePoints() As PointF = {New Point(9, 17), New Point(14, 19), New Point(19, 17)}
                .DrawCurve(New Pen(_TextColour), CurvePoints)
            ElseIf _Pictures = Pictures.Password Then
                TB.Location = New Point(40, 6)
                TB.Width = Width - 45
                .FillPath(New SolidBrush(_BaseColour), GP)
                .FillPolygon(New SolidBrush(_LeftPolygonColour), P)
                .DrawLines(New Pen(_LineColour, 1), P1)
                .DrawPath(New Pen((_BorderColour), 2), GP)
                .FillEllipse(New SolidBrush(_TextColour), New Rectangle(14, 5, 9, 9))
                .FillEllipse(New SolidBrush(_LeftPolygonColour), New Rectangle(18, 7, 3, 3))
                Dim BaseKey() As Point = {New Point(18, 7), New Point(6, 18), New Point(6, 21), New Point(9, 21), _
                                          New Point(9, 18), New Point(11, 19), New Point(10, 18), New Point(12, 18), _
                                          New Point(11, 17), New Point(13, 17), New Point(12, 16), New Point(14, 16), _
                                          New Point(13, 15), New Point(15, 15), New Point(15, 14), New Point(16, 14), _
                                          New Point(15, 13)}
                .DrawLines(New Pen(_TextColour), BaseKey)
            Else
                .FillPath(New SolidBrush(_BaseColour), GP)
                .DrawPath(New Pen((_BorderColour), 2), GP)
                TB.Location = New Point(5, 6)
                TB.Width = Width - 10
            End If
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeButton
    Inherits Control
 
#Region "Declarations"
    Private State As MouseState = MouseState.None
    Private _Font As New Font("Segoe UI", 9)
    Private _BaseColour As Color = Color.FromArgb(245, 245, 245)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _PressedTextColour As Color = Color.FromArgb(42, 42, 42)
    Private _BorderColour As Color = Color.FromArgb(163, 190, 146)
#End Region
 
#Region "Properties"
 
    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property PressedTextColour As Color
        Get
            Return _PressedTextColour
        End Get
        Set(value As Color)
            _PressedTextColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
    Overrides Property Font As Font
        Get
            Return _Font
        End Get
        Set(value As Font)
            _Font = value
        End Set
    End Property
 
#End Region
 
#Region "Mouse States"
 
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
              ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
              ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(75, 30)
        BackColor = Color.Transparent
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim GP, GP1 As New GraphicsPath
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            Select Case State
                Case MouseState.None
                    .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, Width, Height))
                    .DrawRectangle(New Pen(_BorderColour, 1), New Rectangle(0, 0, Width, Height))
                    .DrawString(Text, _Font, New SolidBrush(_TextColour), New Rectangle(0, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Case MouseState.Over
                    .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, Width, Height))
                    .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                    .DrawString(Text, _Font, New SolidBrush(_TextColour), New Rectangle(0, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Case MouseState.Down
                    .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, Width, Height))
                    .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
                    .DrawString(Text, _Font, New SolidBrush(_PressedTextColour), New Rectangle(0, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            End Select
        End With
        MyBase.OnPaint(e)
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeSeperator
    Inherits Control
 
#Region "Declarations"
    Private _SeperatorColour As Color = Color.FromArgb(163, 190, 146)
    Private _FontSize As Integer = 11
    Private _Font As New Font("Tahoma", _FontSize)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _Alignment As Style = Style.Horizontal
    Private _Thickness As Single = 1
    Private _ShowText As Boolean = True
#End Region
 
#Region "Properties"
 
    Enum Style
        Horizontal
        Verticle
    End Enum
 
    <Category("Control")>
    Public Property Thickness As Single
        Get
            Return _Thickness
        End Get
        Set(value As Single)
            _Thickness = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property Alignment As Style
        Get
            Return _Alignment
        End Get
        Set(value As Style)
            _Alignment = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property SeperatorColour As Color
        Get
            Return _SeperatorColour
        End Get
        Set(value As Color)
            _SeperatorColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property ShowText As Boolean
        Get
            Return _ShowText
        End Get
        Set(value As Boolean)
            _ShowText = value
        End Set
    End Property
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Size = New Size(20, 40)
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        With G
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .Clear(Color.FromArgb(250, 250, 250))
            Select Case _Alignment
                Case Style.Horizontal
                    If _ShowText = True Then
                        .DrawString(Text, _Font, New SolidBrush(_TextColour), 0, Height / 2 - _FontSize + 1)
                        .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(.MeasureString(Text, _Font).Width + 2, Height / 2), New Point(Width, Height / 2))
                    Else
                        .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(0, Height / 2), New Point(Width, Height / 2))
                    End If
                Case Style.Verticle
                    .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(Width / 2, 0), New Point(Width / 2, Height))
            End Select
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
<DefaultEvent("CheckedChanged")>
Public Class ElegantThemeCheckBox
    Inherits Control
 
#Region "Declarations"
 
    Private _Checked As Boolean = False
    Private State As MouseState = MouseState.None
    Private _Font As Font = New Font("Segoe UI", 9)
    Private _CheckedColour As Color = Color.FromArgb(173, 173, 174)
    Private _BorderColour As Color = Color.FromArgb(193, 210, 176)
    Private _BackColour As Color = Color.Transparent
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _DefaultSqaureColour As Color = Color.FromArgb(230, 230, 230)
    Private _HoverSqaureColour As Color = Color.FromArgb(220, 220, 220)
 
#End Region
 
#Region "Colour & Other Properties"
 
    <Category("Colours")>
    Public Property DefaultSqaureColour As Color
        Get
            Return _DefaultSqaureColour
        End Get
        Set(value As Color)
            _DefaultSqaureColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property HoverSqaureColour As Color
        Get
            Return _HoverSqaureColour
        End Get
        Set(value As Color)
            _HoverSqaureColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BackColour
        End Get
        Set(value As Color)
            _BackColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property CheckedColour As Color
        Get
            Return _CheckedColour
        End Get
        Set(value As Color)
            _CheckedColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property FontColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
 
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property
 
    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
 
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 22
    End Sub
 
#End Region
 
#Region "Mouse States"
 
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        Size = New Size(100, 22)
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, 22, 22)
        With G
            .TextRenderingHint = TextRenderingHint.AntiAliasGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(Color.FromArgb(250, 250, 250))
            .FillRectangle(New SolidBrush(_BackColour), Base)
            .FillRectangle(New SolidBrush(_DefaultSqaureColour), New Rectangle(0, 0, 22, 22))
            .DrawRectangle(New Pen(_BorderColour), New Rectangle(1, 1, 22, 20))
            Select Case State
                Case MouseState.Over
                    If Checked Then
                        .FillRectangle(New SolidBrush(_HoverSqaureColour), New Rectangle(0, 0, 22, 22))
                        .DrawLine(New Pen(_BorderColour, 2), 1, 1, 22, 20)
                        .DrawLine(New Pen(_BorderColour, 2), 2, 20, 21, 2)
                        .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(1, 1, 22, 20))
                    Else
                        .FillRectangle(New SolidBrush(_HoverSqaureColour), New Rectangle(0, 0, 22, 22))
                        .DrawRectangle(New Pen(_BorderColour), New Rectangle(1, 1, 22, 20))
                    End If
            End Select
            If Checked Then
                .DrawLine(New Pen(_BorderColour, 2), 1, 1, 22, 20)
                .DrawLine(New Pen(_BorderColour, 2), 2, 20, 22, 2)
                .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(1, 1, 22, 20))
                .FillRectangle(New SolidBrush(_BorderColour), New Rectangle(7, 6, 10, 10))
                .DrawString(Text, _Font, New SolidBrush(Color.FromArgb(45, 45, 45)), New Rectangle(27, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
            Else
                .DrawString(Text, _Font, New SolidBrush(_TextColour), New Rectangle(27, 0, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
            End If
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeGroupBox
    Inherits ContainerControl
 
#Region "Declarations"
    Private _MainColour As Color = Color.FromArgb(255, 255, 255)
    Private _HeaderColour As Color = Color.FromArgb(248, 250, 249)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _BorderColour As Color = Color.FromArgb(163, 190, 146)
#End Region
 
#Region "Properties"
 
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property HeaderColour As Color
        Get
            Return _HeaderColour
        End Get
        Set(value As Color)
            _HeaderColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property MainColour As Color
        Get
            Return _MainColour
        End Get
        Set(value As Color)
            _MainColour = value
        End Set
    End Property
 
#End Region
 
#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
               ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
               ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(160, 110)
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim Circle As New Rectangle(8, 8, 10, 10)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            .FillRectangle(New SolidBrush(_MainColour), New Rectangle(0, 28, Width, Height))
            .FillRectangle(New SolidBrush(_HeaderColour), New Rectangle(0, 0, Width, 28))
            .DrawString(Text, Font, New SolidBrush(_TextColour), New Point(5, 5))
            .DrawRectangle(New Pen(_BorderColour), New Rectangle(0, 0, Width, Height))
            .DrawLine(New Pen(_BorderColour), 0, 28, Width, 28)
            .DrawLine(New Pen(_BorderColour, 3), New Point(0, 27), New Point(.MeasureString(Text, Font).Width + 7, 27))
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region
 
End Class
 
Public Class ElegantThemeDropDownSeperator
    Inherits ContainerControl
 
#Region "Declaration"
 
    Private _Checked As Boolean
    Private X As Integer
    Private Y As Integer
    Private Up As Boolean
    Private SpecifiedHeight As Integer
    Private _OpenHeight As Integer = 200
    Private _ClosedHeight As Integer = 30
    Private _OpenWidth As Integer = 160
    Private _CaptureHeightChange As Boolean = False
    Private _SeperatorColour As Color = Color.FromArgb(163, 190, 146)
    Private _FontSize As Integer = 11
    Private _Font As New Font("Tahoma", _FontSize)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _Thickness As Single = 1
    Private _Animate As Boolean = False
 
#End Region
 
#Region "Properties & Events"
 
    <Category("Colours")>
    Public Property SeperatorColour As Color
        Get
            Return _SeperatorColour
        End Get
        Set(value As Color)
            _SeperatorColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    Public Property OpenHeight As Integer
        Get
            Return _OpenHeight
        End Get
        Set(value As Integer)
            _OpenHeight = value
        End Set
    End Property
 
    Public Property Thickness As Single
        Get
            Return _Thickness
        End Get
        Set(value As Single)
            _Thickness = value
        End Set
    End Property
 
    Public Property Animation As Boolean
        Get
            Return _Animate
        End Get
        Set(ByVal Value As Boolean)
            _Animate = Value
            Invalidate()
        End Set
    End Property
 
    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal V As Boolean)
            _Checked = V
            Invalidate()
        End Set
    End Property
 
    Public Property CaptureHeightChange As Boolean
        Get
            Return _CaptureHeightChange
        End Get
        Set(ByVal Value As Boolean)
            _CaptureHeightChange = Value
            Invalidate()
        End Set
    End Property
 
    Protected Overrides Sub OnResize(e As EventArgs)
        If _CaptureHeightChange = True Then
            _OpenHeight = Height
        End If
        MyBase.OnResize(e)
    End Sub
 
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.X
        Y = e.Y
        Invalidate()
    End Sub
 
    Sub Animate(ByVal Closing As Boolean)
        Select Case Closing
            Case True
                Dim HT As Integer = _OpenHeight
                Do Until HT = 30
                    Me.Height -= 1
                    HT -= 1
                Loop
                Up = True
                _Checked = False
            Case False
                Do Until Me.Height = _OpenHeight
                    Me.Height += 1
                    Update()
                Loop
                Up = False
                _Checked = True
        End Select
    End Sub
 
    Sub ChangeCheck() Handles Me.MouseDown
        If X >= Width - 22 Then
            If Y <= 30 Then
                Select Case Checked
                    Case True
                        If _Animate Then
                            Animate(True)
                        Else
                            Me.Height = 30
                            Up = True
                            _Checked = False
                        End If
                    Case False
                        If _Animate Then
                            Animate(False)
                        Else
                            Me.Height = _OpenHeight
                            Up = False
                            _Checked = True
                        End If
                End Select
            End If
        End If
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
            ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
            ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(90, 30)
        MinimumSize = New Size(5, 30)
        Me.Font = New Font("Tahoma", 10)
        Me.ForeColor = Color.FromArgb(40, 40, 40)
        _Checked = True
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            If _Checked = True Then
                .Clear(Color.FromArgb(250, 250, 250))
                .DrawString(Text, _Font, New SolidBrush(_TextColour), 13, 30 / 2 - _FontSize + 1)
                .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(0, 30 / 2), New Point(10, 30 / 2))
                .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(.MeasureString(Text, _Font).Width + 13, 30 / 2), New Point(Width - 25, 30 / 2))
                .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(Width - 7, 30 / 2), New Point(Width, 30 / 2))
                .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(1, 30 / 2), New Point(1, Height))
                .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(Width - 1, 30 / 2), New Point(Width - 1, Height))
                .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(1, Height - 1), New Point(Width, Height - 1))
            Else
                .Clear(Color.FromArgb(250, 250, 250))
                .DrawString(Text, _Font, New SolidBrush(_TextColour), 13, 30 / 2 - _FontSize + 1)
                .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(0, 30 / 2), New Point(10, 30 / 2))
                .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(.MeasureString(Text, _Font).Width + 13, 30 / 2), New Point(Width - 25, 30 / 2))
                .DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(Width - 7, 30 / 2), New Point(Width, 30 / 2))
            End If
            Select Case _Checked
                Case False
                    .DrawLine(New Pen(Color.DarkGray, 2), New Point(Width - 21, 11), New Point(Width - 16, 19))
                    .DrawLine(New Pen(Color.DarkGray, 2), New Point(Width - 16, 19), New Point(Width - 11, 11))
                Case True
                    .DrawLine(New Pen(Color.DarkGray, 2), New Point(Width - 21, 19), New Point(Width - 16, 11))
                    .DrawLine(New Pen(Color.DarkGray, 2), New Point(Width - 16, 11), New Point(Width - 11, 19))
            End Select
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeStatusBar
    Inherits Control
 
#Region "Variables"
    Private _BaseColour As Color = Color.FromArgb(240, 242, 241)
    Private _BorderColour As Color = Color.FromArgb(183, 210, 166)
    Private _TextColour As Color = Color.FromArgb(120, 120, 120)
    Private _RectColour As Color = Color.FromArgb(21, 117, 149)
    Private _SeperatorColour As Color = Color.FromArgb(110, 110, 110)
    Private _ShowLine As Boolean = True
    Private _LinesToShow As LinesCount = LinesCount.One
    Private _NumberOfStrings As AmountOfStrings = AmountOfStrings.One
    Private _ShowBorder As Boolean = True
    Private _FirstLabelStringFormat As StringFormat
    Private _FirstLabelText As String = "Label1"
    Private _FirstLabelAlignment As Alignments = Alignments.Center
    Private _SecondLabelStringFormat As StringFormat
    Private _SecondLabelText As String = "Label2"
    Private _SecondLabelAlignment As Alignments = Alignments.Center
    Private _ThirdLabelStringFormat As StringFormat
    Private _ThirdLabelText As String = "Label3"
    Private _ThirdLabelAlignment As Alignments = Alignments.Center
#End Region
 
#Region "Properties"
 
    <Category("First Label Options")>
    Public Property FirstLabelText As String
        Get
            Return _FirstLabelText
        End Get
        Set(value As String)
            _FirstLabelText = value
        End Set
    End Property
 
    <Category("First Label Options")>
    Public Property FirstLabelAlignment As Alignments
        Get
            Return _FirstLabelAlignment
        End Get
        Set(value As Alignments)
            Select Case value
                Case Alignments.Left
                    _FirstLabelStringFormat = New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}
                    _FirstLabelAlignment = value
                Case Alignments.Center
                    _FirstLabelStringFormat = New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                    _FirstLabelAlignment = value
                Case Alignments.Right
                    _FirstLabelStringFormat = New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
                    _FirstLabelAlignment = value
            End Select
        End Set
    End Property
 
    <Category("Second Label Options")>
    Public Property SecondLabelText As String
        Get
            Return _SecondLabelText
        End Get
        Set(value As String)
            _SecondLabelText = value
        End Set
    End Property
 
    <Category("Second Label Options")>
    Public Property SecondLabelAlignment As Alignments
        Get
            Return _SecondLabelAlignment
        End Get
        Set(value As Alignments)
            Select Case value
                Case Alignments.Left
                    _SecondLabelStringFormat = New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}
                    _SecondLabelAlignment = value
                Case Alignments.Center
                    _SecondLabelStringFormat = New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                    _SecondLabelAlignment = value
                Case Alignments.Right
                    _SecondLabelStringFormat = New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
                    _SecondLabelAlignment = value
            End Select
        End Set
    End Property
 
    <Category("Third Label Options")>
    Public Property ThirdLabelText As String
        Get
            Return _ThirdLabelText
        End Get
        Set(value As String)
            _ThirdLabelText = value
        End Set
    End Property
 
    <Category("Third Label Options")>
    Public Property ThirdLabelAlignment As Alignments
        Get
            Return _ThirdLabelAlignment
        End Get
        Set(value As Alignments)
            Select Case value
                Case Alignments.Left
                    _ThirdLabelStringFormat = New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center}
                    _ThirdLabelAlignment = value
                Case Alignments.Center
                    _ThirdLabelStringFormat = New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                    _ThirdLabelAlignment = value
                Case Alignments.Right
                    _ThirdLabelStringFormat = New StringFormat With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
                    _ThirdLabelAlignment = value
            End Select
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    Enum LinesCount As Integer
        None = 0
        One = 1
        Two = 2
    End Enum
 
    Enum AmountOfStrings
        One
        Two
        Three
    End Enum
 
    Enum Alignments
        Left
        Center
        Right
    End Enum
 
    <Category("Control")>
    Public Property AmountOfString As AmountOfStrings
        Get
            Return _NumberOfStrings
        End Get
        Set(value As AmountOfStrings)
            _NumberOfStrings = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property LinesToShow As LinesCount
        Get
            Return _LinesToShow
        End Get
        Set(value As LinesCount)
            _LinesToShow = value
        End Set
    End Property
 
    Public Property ShowBorder As Boolean
        Get
            Return _ShowBorder
        End Get
        Set(value As Boolean)
            _ShowBorder = value
        End Set
    End Property
 
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Dock = DockStyle.Bottom
    End Sub
 
    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub
 
    <Category("Colours")> _
    Public Property RectangleColor As Color
        Get
            Return _RectColour
        End Get
        Set(value As Color)
            _RectColour = value
        End Set
    End Property
 
    Public Property ShowLine As Boolean
        Get
            Return _ShowLine
        End Get
        Set(value As Boolean)
            _ShowLine = value
        End Set
    End Property
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 9)
        ForeColor = Color.White
        Size = New Size(Width, 20)
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .Clear(BaseColour)
            .FillRectangle(New SolidBrush(BaseColour), Base)
            Select Case _LinesToShow
                Case LinesCount.None
                    If _NumberOfStrings = AmountOfStrings.One Then
                        .DrawString(_FirstLabelText, Font, New SolidBrush(_TextColour), New Rectangle(5, 1, Width - 5, Height), _FirstLabelStringFormat)
                    ElseIf _NumberOfStrings = AmountOfStrings.Two Then
                        .DrawString(_FirstLabelText, Font, New SolidBrush(_TextColour), New Rectangle(5, 1, (Width / 2 - 6), Height), _FirstLabelStringFormat)
                        .DrawString(_SecondLabelText, Font, New SolidBrush(_TextColour), New Rectangle(Width - (Width / 2 + 5), 1, Width / 2 - 4, Height), _SecondLabelStringFormat)
                        .DrawLine(New Pen(_SeperatorColour, 1), New Point(Width / 2, 6), New Point(Width / 2, Height - 6))
                    Else
                        .DrawString(_FirstLabelText, Font, New SolidBrush(_TextColour), New Rectangle(5, 1, (Width - (Width / 3) * 2) - 6, Height), _FirstLabelStringFormat)
                        .DrawString(_SecondLabelText, Font, New SolidBrush(_TextColour), New Rectangle(Width - (Width / 3) * 2 + 5, 1, Width - (Width / 3) * 2 - 6, Height), _SecondLabelStringFormat)
                        .DrawString(_ThirdLabelText, Font, New SolidBrush(_TextColour), New Rectangle(Width - (Width / 3) + 5, 1, Width / 3 - 6, Height), _ThirdLabelStringFormat)
                        .DrawLine(New Pen(_SeperatorColour, 1), New Point(Width - (Width / 3) * 2, 6), New Point(Width - (Width / 3) * 2, Height - 6))
                        .DrawLine(New Pen(_SeperatorColour, 1), New Point(Width - (Width / 3), 6), New Point(Width - (Width / 3), Height - 6))
                    End If
                Case LinesCount.One
                    If _NumberOfStrings = AmountOfStrings.One Then
                        .DrawString(_FirstLabelText, Font, New SolidBrush(_TextColour), New Rectangle(22, 1, Width, Height), _FirstLabelStringFormat)
                        .FillRectangle(New SolidBrush(_RectColour), New Rectangle(5, 9, 14, 3))
                    ElseIf _NumberOfStrings = AmountOfStrings.Two Then
                        .DrawString(_FirstLabelText, Font, New SolidBrush(_TextColour), New Rectangle(22, 1, (Width / 2 - 24), Height), _FirstLabelStringFormat)
                        .DrawString(_SecondLabelText, Font, New SolidBrush(_TextColour), New Rectangle((Width / 2 + 5), 1, Width / 2 - 10, Height), _SecondLabelStringFormat)
                        .DrawLine(New Pen(_SeperatorColour, 1), New Point(Width / 2, 6), New Point(Width / 2, Height - 6))
                    Else
                    End If
                    .FillRectangle(New SolidBrush(_SeperatorColour), New Rectangle(5, 10, 14, 3))
                Case LinesCount.Two
                    If _NumberOfStrings = AmountOfStrings.One Then
                        .DrawString(_FirstLabelText, Font, New SolidBrush(_TextColour), New Rectangle(22, 1, Width - 44, Height), _FirstLabelStringFormat)
                    ElseIf _NumberOfStrings = AmountOfStrings.Two Then
                        .DrawString(_FirstLabelText, Font, New SolidBrush(_TextColour), New Rectangle(22, 1, (Width - 46) / 2, Height), _FirstLabelStringFormat)
                        .DrawString(_SecondLabelText, Font, New SolidBrush(_TextColour), New Rectangle((Width / 2 + 5), 1, Width / 2 - 28, Height), _SecondLabelStringFormat)
                        .DrawLine(New Pen(_SeperatorColour, 1), New Point(Width / 2, 6), New Point(Width / 2, Height - 6))
                    Else
                        .DrawString(_FirstLabelText, Font, New SolidBrush(_TextColour), New Rectangle(22, 1, (Width - 78) / 3, Height), _FirstLabelStringFormat)
                        .DrawString(_SecondLabelText, Font, New SolidBrush(_TextColour), New Rectangle(Width - (Width / 3) * 2 + 5, 1, Width - (Width / 3) * 2 - 12, Height), _SecondLabelStringFormat)
                        .DrawString(_ThirdLabelText, Font, New SolidBrush(_TextColour), New Rectangle(Width - (Width / 3) + 6, 1, Width / 3 - 22, Height), _ThirdLabelStringFormat)
                        .DrawLine(New Pen(_SeperatorColour, 1), New Point(Width - (Width / 3) * 2, 6), New Point(Width - (Width / 3) * 2, Height - 6))
                        .DrawLine(New Pen(_SeperatorColour, 1), New Point(Width - (Width / 3), 6), New Point(Width - (Width / 3), Height - 6))
 
                    End If
                    .FillRectangle(New SolidBrush(_SeperatorColour), New Rectangle(5, 10, 14, 3))
                    .FillRectangle(New SolidBrush(_SeperatorColour), New Rectangle(Width - 20, 10, 14, 3))
            End Select
            If _ShowBorder Then
                .DrawRectangle(New Pen(_BorderColour, 2), New Rectangle(0, 0, Width, Height))
            Else
            End If
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeTitledListBox
    Inherits Control
 
#Region "Variables"
 
    Private WithEvents ListB As New ListBox
    Private _Items As String() = {""}
    Private _BaseColour As Color = Color.FromArgb(255, 255, 255)
    Private _SelectedColour As Color = Color.FromArgb(55, 55, 55)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _BorderColour As Color = Color.FromArgb(183, 210, 166)
    Private _TitleFont As New Font("Segeo UI", 10, FontStyle.Bold)
 
#End Region
 
#Region "Properties"
 
    <Category("Control")>
    Public Property TitleFont As Font
        Get
            Return _TitleFont
        End Get
        Set(value As Font)
            _TitleFont = value
        End Set
    End Property
 
    <Category("Control")> _
    Public Property Items As String()
        Get
            Return _Items
        End Get
        Set(value As String())
            _Items = value
            ListB.Items.Clear()
            ListB.Items.AddRange(value)
            Invalidate()
        End Set
    End Property
 
    <Category("Colours")> _
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Colours")> _
    Public Property SelectedColour As Color
        Get
            Return _SelectedColour
        End Get
        Set(value As Color)
            _SelectedColour = value
        End Set
    End Property
 
    <Category("Colours")> _
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property
 
    <Category("Colours")> _
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    Public ReadOnly Property SelectedItem() As String
        Get
            Return ListB.SelectedItem
        End Get
    End Property
 
    Public ReadOnly Property SelectedIndex() As Integer
        Get
            Return ListB.SelectedIndex
            If ListB.SelectedIndex < 0 Then Exit Property
        End Get
    End Property
 
    Public Sub Clear()
        ListB.Items.Clear()
    End Sub
 
    Public Sub ClearSelected()
        For i As Integer = (ListB.SelectedItems.Count - 1) To 0 Step -1
            ListB.Items.Remove(ListB.SelectedItems(i))
        Next
    End Sub
 
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(ListB) Then
            Controls.Add(ListB)
        End If
    End Sub
 
    Sub AddRange(ByVal items As Object())
        ListB.Items.Remove("")
        ListB.Items.AddRange(items)
    End Sub
 
    Sub AddItem(ByVal item As Object)
        ListB.Items.Remove("")
        ListB.Items.Add(item)
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub Drawitem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListB.DrawItem
        If e.Index < 0 Then Exit Sub
        e.DrawBackground()
        e.DrawFocusRectangle()
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .InterpolationMode = InterpolationMode.HighQualityBicubic
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            If InStr(e.State.ToString, "Selected,") > 0 Then
                .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
                .DrawLine(New Pen(_BorderColour), e.Bounds.X, e.Bounds.Y + e.Bounds.Height, e.Bounds.Width, e.Bounds.Y + e.Bounds.Height)
                .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 9, FontStyle.Bold), New SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2)
            Else
                .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
                .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 8), New SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2)
            End If
            .Dispose()
        End With
    End Sub
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
            ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        ListB.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        ListB.ScrollAlwaysVisible = False
        ListB.HorizontalScrollbar = False
        ListB.BorderStyle = BorderStyle.None
        ListB.BackColor = _BaseColour
        ListB.Location = New Point(3, 30)
        ListB.Font = New Font("Segoe UI", 8)
        ListB.ItemHeight = 20
        ListB.Items.Clear()
        ListB.IntegralHeight = False
        Size = New Size(130, 100)
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 28, Width, Height)
        With G
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .Clear(Color.FromArgb(248, 250, 249))
            ListB.Size = New Size(Width - 6, Height - 32)
            .FillRectangle(New SolidBrush(BaseColour), Base)
            .DrawRectangle(New Pen((_BorderColour), 1), New Rectangle(0, 0, Width, Height - 1))
            .DrawLine(New Pen((_BorderColour), 2), New Point(0, 27), New Point(Width - 1, 27))
            .DrawString(Text, _TitleFont, New SolidBrush(_TextColour), New Rectangle(2, 5, Width - 5, 20), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
            .DrawLine(New Pen(_BorderColour, 3), New Point(0, 26), New Point(.MeasureString(Text, _TitleFont).Width + 10, 26))
 
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeListBox
    Inherits Control
 
#Region "Declarations"
 
    Private WithEvents ListB As New ListBox
    Private _Items As String() = {""}
    Private _BaseColour As Color = Color.FromArgb(255, 255, 255)
    Private _SelectedColour As Color = Color.FromArgb(55, 55, 55)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _BorderColour As Color = Color.FromArgb(183, 210, 166)
 
#End Region
 
#Region "Properties"
 
    <Category("Control")> _
    Public Property Items As String()
        Get
            Return _Items
        End Get
        Set(value As String())
            _Items = value
            ListB.Items.Clear()
            ListB.Items.AddRange(value)
            Invalidate()
        End Set
    End Property
 
    <Category("Colours")> _
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Colours")> _
    Public Property SelectedColour As Color
        Get
            Return _SelectedColour
        End Get
        Set(value As Color)
            _SelectedColour = value
        End Set
    End Property
 
    <Category("Colours")> _
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property
 
    <Category("Colours")> _
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    Public ReadOnly Property SelectedItem() As String
        Get
            Return ListB.SelectedItem
        End Get
    End Property
 
    Public ReadOnly Property SelectedIndex() As Integer
        Get
            Return ListB.SelectedIndex
            If ListB.SelectedIndex < 0 Then Exit Property
        End Get
    End Property
 
    Public Sub Clear()
        ListB.Items.Clear()
    End Sub
 
    Public Sub ClearSelected()
        For i As Integer = (ListB.SelectedItems.Count - 1) To 0 Step -1
            ListB.Items.Remove(ListB.SelectedItems(i))
        Next
    End Sub
 
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(ListB) Then
            Controls.Add(ListB)
        End If
    End Sub
 
    Sub AddRange(ByVal items As Object())
        ListB.Items.Remove("")
        ListB.Items.AddRange(items)
    End Sub
 
    Sub AddItem(ByVal item As Object)
        ListB.Items.Remove("")
        ListB.Items.Add(item)
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub Drawitem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ListB.DrawItem
        If e.Index < 0 Then Exit Sub
        e.DrawBackground()
        e.DrawFocusRectangle()
        With e.Graphics
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .InterpolationMode = InterpolationMode.HighQualityBicubic
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            If InStr(e.State.ToString, "Selected,") > 0 Then
                .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
                .DrawLine(New Pen(_BorderColour), e.Bounds.X, e.Bounds.Y + e.Bounds.Height, e.Bounds.Width, e.Bounds.Y + e.Bounds.Height)
                .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 9, FontStyle.Bold), New SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2)
            Else
                .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height))
                .DrawString(" " & ListB.Items(e.Index).ToString(), New Font("Segoe UI", 8), New SolidBrush(_TextColour), e.Bounds.X, e.Bounds.Y + 2)
            End If
            .Dispose()
        End With
    End Sub
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
            ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        ListB.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        ListB.ScrollAlwaysVisible = False
        ListB.HorizontalScrollbar = False
        ListB.BorderStyle = BorderStyle.None
        ListB.BackColor = _BaseColour
        ListB.Location = New Point(3, 3)
        ListB.Font = New Font("Segoe UI", 8)
        ListB.ItemHeight = 20
        ListB.Items.Clear()
        ListB.IntegralHeight = False
        Size = New Size(130, 100)
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .Clear(Color.FromArgb(248, 250, 249))
            ListB.Size = New Size(Width - 6, Height - 7)
            .FillRectangle(New SolidBrush(BaseColour), Base)
            .DrawRectangle(New Pen((_BorderColour), 1), New Rectangle(0, 0, Width, Height - 1))
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeLabel
    Inherits Label
 
#Region "Declaration"
    Private _FontColour As Color = Color.FromArgb(163, 163, 163)
#End Region
 
#Region "Property & Event"
 
    <Category("Colours")>
    Public Property FontColour As Color
        Get
            Return _FontColour
        End Get
        Set(value As Color)
            _FontColour = value
        End Set
    End Property
 
    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Font = New Font("Segoe UI", 9)
        ForeColor = _FontColour
        BackColor = Color.Transparent
        Text = Text
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeRichTextBox
    Inherits Control
 
#Region "Declarations"
    Private WithEvents TB As New RichTextBox
    Private _BaseColour As Color = Color.FromArgb(255, 255, 255)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _BorderColour As Color = Color.FromArgb(183, 210, 166)
#End Region
 
#Region "Properties"
 
    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
#End Region
 
#Region "Events"
 
    Overrides Property Text As String
        Get
            Return TB.Text
        End Get
        Set(value As String)
            TB.Text = value
            Invalidate()
        End Set
    End Property
 
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        TB.BackColor = _BaseColour
        Invalidate()
    End Sub
 
    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        TB.ForeColor = ForeColor
        Invalidate()
    End Sub
 
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        TB.Size = New Size(Width - 10, Height - 11)
    End Sub
 
    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        TB.Font = Font
    End Sub
 
    Sub TextChanges() Handles MyBase.TextChanged
        TB.Text = Text
    End Sub
 
    Sub AppendText(ByVal Text As String)
        TB.Focus()
        TB.AppendText(Text)
        Invalidate()
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        With TB
            .Multiline = True
            .ForeColor = _TextColour
            .Text = String.Empty
            .BorderStyle = BorderStyle.None
            .Location = New Point(5, 5)
            .Font = New Font("Segeo UI", 9)
            .Size = New Size(Width - 10, Height - 10)
        End With
        Controls.Add(TB)
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub
 
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(_BaseColour)
            .DrawRectangle(New Pen(_BorderColour, 1), ClientRectangle)
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeColourTable
    Inherits ProfessionalColorTable
 
#Region "Declarations"
 
    Private _BackColour As Color = Color.FromArgb(220, 220, 220)
    Private _BorderColour As Color = Color.FromArgb(183, 210, 166)
    Private _SelectedColour As Color = Color.FromArgb(230, 230, 230)
 
#End Region
 
#Region "Properties"
 
    <Category("Colours")>
    Public Property SelectedColour As Color
        Get
            Return _SelectedColour
        End Get
        Set(value As Color)
            _SelectedColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BackColour As Color
        Get
            Return _BackColour
        End Get
        Set(value As Color)
            _BackColour = value
        End Set
    End Property
 
    Public Overrides ReadOnly Property ButtonSelectedBorder() As Color
        Get
            Return _BackColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property CheckBackground() As Color
        Get
            Return _BackColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property CheckPressedBackground() As Color
        Get
            Return _BackColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property CheckSelectedBackground() As Color
        Get
            Return _BackColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
        Get
            Return _BackColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
        Get
            Return _BackColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
        Get
            Return _BackColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property MenuBorder() As Color
        Get
            Return _BorderColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property MenuItemBorder() As Color
        Get
            Return _BackColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property MenuItemSelected() As Color
        Get
            Return _SelectedColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property SeparatorDark() As Color
        Get
            Return _BorderColour
        End Get
    End Property
 
    Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
        Get
            Return _BackColour
        End Get
    End Property
 
#End Region
 
End Class
 
Public Class ElegantThemeContextMenu
    Inherits ContextMenuStrip
 
#Region "Draw Control"
 
    Sub New()
        Renderer = New ToolStripProfessionalRenderer(New ElegantThemeColourTable())
        ShowCheckMargin = False
        ShowImageMargin = False
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
        MyBase.OnPaint(e)
    End Sub
 
#End Region
 
End Class
 
Public Class ElegantThemeTabControlVertical
    Inherits TabControl
 
#Region "Declarations"
    Private _PressedTabColour As Color = Color.FromArgb(238, 240, 239)
    Private _HoverColour As Color = Color.FromArgb(230, 230, 230)
    Private _NormalColour As Color = Color.FromArgb(250, 249, 251)
    Private _BorderColour As Color = Color.FromArgb(163, 190, 146)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private HoverIndex As Integer = -1
#End Region
 
#Region "Colour & Other Properties"
    <Category("Colours")>
    Public Property NormalColour As Color
        Get
            Return _NormalColour
        End Get
        Set(value As Color)
            _NormalColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property HoverColour As Color
        Get
            Return _HoverColour
        End Get
        Set(value As Color)
            _HoverColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property PressedTabColour As Color
        Get
            Return _PressedTabColour
        End Get
        Set(value As Color)
            _PressedTabColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
#End Region
 
#Region "Draw Control"
    Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint, True)
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(45, 95)
        Font = New Font("Segoe UI", 9, FontStyle.Bold)
        DrawMode = TabDrawMode.OwnerDrawFixed
    End Sub
 
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
    End Sub
 
    Protected Overrides Sub OnControlAdded(ByVal e As ControlEventArgs)
        If TypeOf e.Control Is TabPage Then
            For Each i As TabPage In Me.Controls
                i = New TabPage
            Next
            e.Control.BackColor = Color.FromArgb(255, 255, 255)
        End If
        MyBase.OnControlAdded(e)
    End Sub
 
    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        For I As Integer = 0 To TabPages.Count - 1
            If GetTabRect(I).Contains(e.Location) Then
                HoverIndex = I
                Exit For
            End If
        Next
        Invalidate()
        MyBase.OnMouseMove(e)
    End Sub
 
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        HoverIndex = -1
        Invalidate()
        MyBase.OnMouseLeave(e)
 
    End Sub
 
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
        G.SmoothingMode = SmoothingMode.HighQuality
        G.PixelOffsetMode = PixelOffsetMode.HighQuality
        G.Clear(BackColor)
        Try : SelectedTab.BackColor = _NormalColour : Catch : End Try
        With G
            .FillRectangle(New SolidBrush(_NormalColour), New Rectangle(-2, 0, ItemSize.Height + 4, Height + 22))
            For i As Integer = 0 To TabCount - 1
                Dim tabRect1 As New Rectangle(GetTabRect(i).Location.X + 5, GetTabRect(i).Location.Y + 2, GetTabRect(i).Size.Width - 20, GetTabRect(i).Size.Height - 11)
                If i = SelectedIndex Then
                    Dim tabRect As New Rectangle(GetTabRect(i).Location.X + 5, GetTabRect(i).Location.Y + 2, GetTabRect(i).Size.Width + 10, GetTabRect(i).Size.Height - 11)
                    .FillRectangle(New SolidBrush(_PressedTabColour), New Rectangle(tabRect.X + 1, tabRect.Y + 1, tabRect.Width - 1, tabRect.Height - 2))
                    .DrawRectangle(New Pen(_BorderColour), tabRect)
                    .FillRectangle(New SolidBrush(_BorderColour), GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y + 1, GetTabRect(i).Location.X + 2, GetTabRect(i).Size.Height - 10)
                    .FillRectangle(New SolidBrush(_BorderColour), GetTabRect(i).Location.X + 2, GetTabRect(i).Location.Y + 6, GetTabRect(i).Location.X + 2, GetTabRect(i).Size.Height - 19)
                    .SmoothingMode = SmoothingMode.AntiAlias
                    .DrawString(TabPages(i).Text, New Font("Segoe UI", 9, FontStyle.Bold), New SolidBrush(_TextColour), tabRect1, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                Else
                    If HoverIndex = i Then
                        Dim x21 As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y + 2), New Size(GetTabRect(i).Width, GetTabRect(i).Height - 11))
                        .FillRectangle(New SolidBrush(_HoverColour), x21)
                    End If
                    .DrawString(TabPages(i).Text, New Font("Segoe UI", 9, FontStyle.Regular), New SolidBrush(_TextColour), tabRect1, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
 
                End If
                .FillRectangle(New SolidBrush(_NormalColour), New Rectangle(97, 0, Width - 97, Height))
                .DrawLine(New Pen((_BorderColour), 1), New Point(96, 0), New Point(96, Height))
            Next
        End With
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region
 
End Class
 
Public Class ElegantThemeProgressBar
    Inherits Control
 
#Region "Declarations"
 
    Private _ProgressColour As Color = Color.FromArgb(163, 190, 146)
    Private _BorderColour As Color = Color.FromArgb(210, 210, 210)
    Private _BaseColour As Color = Color.FromArgb(255, 255, 255)
    Private _SecondColour As Color = Color.FromArgb(148, 190, 131)
    Private _Value As Integer = 0
    Private _Maximum As Integer = 100
    Private _TwoColour As Boolean = True
 
#End Region
 
#Region "Properties"
 
    <Category("Colours")>
    Public Property SecondColour As Color
        Get
            Return _SecondColour
        End Get
        Set(value As Color)
            _SecondColour = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property TwoColour As Boolean
        Get
            Return _TwoColour
        End Get
        Set(value As Boolean)
            _TwoColour = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(V As Integer)
            Select Case V
                Case Is < _Value
                    _Value = V
            End Select
            _Maximum = V
            Invalidate()
        End Set
    End Property
 
    <Category("Control")>
    Public Property Value() As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
                    Invalidate()
                Case Else
                    Return _Value
                    Invalidate()
            End Select
        End Get
        Set(V As Integer)
            Select Case V
                Case Is > _Maximum
                    V = _Maximum
                    Invalidate()
            End Select
            _Value = V
            Invalidate()
        End Set
    End Property
 
    <Category("Colours")>
    Public Property ProgressColour As Color
        Get
            Return _ProgressColour
        End Get
        Set(value As Color)
            _ProgressColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
 
#End Region
 
#Region "Events"
 
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If Height < 25 Then
            Height = 25
        End If
    End Sub
 
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Height = 25
    End Sub
 
    Public Sub Increment(ByVal Amount As Integer)
        Value += Amount
    End Sub
 
#End Region
 
#Region "Draw Control"
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B = New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            Dim ProgVal As Integer = CInt(_Value / _Maximum * Width)
            Select Case Value
                Case 0
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    .DrawRectangle(New Pen(_BorderColour, 3), Base)
                Case _Maximum
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    If _TwoColour Then
                        G.SetClip(New Rectangle(0, -10, Width * _Value / _Maximum - 1, Height - 5))
                        For i = 0 To (Width - 1) * _Maximum / _Value Step 20
                            G.DrawLine(New Pen(New SolidBrush(_SecondColour), 7), New Point(i, 0), New Point(i - 15, Height))
                        Next
                        G.ResetClip()
                    Else
                    End If
                    .DrawRectangle(New Pen(_BorderColour, 3), Base)
                Case Else
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    If _TwoColour Then
                        .SetClip(New Rectangle(0, 0, Width * _Value / _Maximum - 1, Height - 1))
                        For i = 0 To (Width - 1) * _Maximum / _Value Step 20
                            .DrawLine(New Pen(New SolidBrush(_SecondColour), 7), New Point(i, 0), New Point(i - 10, Height))
                        Next
                        .ResetClip()
                    Else
                    End If
                    .DrawRectangle(New Pen(_BorderColour, 3), Base)
            End Select
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
 
#End Region
 
End Class
 
<DefaultEvent("CheckedChanged")>
Public Class ElegantRadioButton
    Inherits Control
 
#Region "Declarations"
    Private _Checked As Boolean
    Private State As MouseState = MouseState.None
    Private _HoverColour As Color = Color.FromArgb(240, 240, 240)
    Private _CheckedColour As Color = Color.FromArgb(163, 190, 146)
    Private _BorderColour As Color = Color.FromArgb(210, 210, 210)
    Private _BackColour As Color = Color.FromArgb(255, 255, 255)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
#End Region
 
#Region "Colour & Other Properties"
 
    <Category("Colours")>
    Public Property HighlightColour As Color
        Get
            Return _HoverColour
        End Get
        Set(value As Color)
            _HoverColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BackColour
        End Get
        Set(value As Color)
            _BackColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property CheckedColour As Color
        Get
            Return _CheckedColour
        End Get
        Set(value As Color)
            _CheckedColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property FontColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    Event CheckedChanged(ByVal sender As Object)
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            InvalidateControls()
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property
 
    Protected Overrides Sub OnClick(e As EventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnClick(e)
    End Sub
    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return
        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is ElegantRadioButton Then
                DirectCast(C, ElegantRadioButton).Checked = False
                Invalidate()
            End If
        Next
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        InvalidateControls()
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 22
    End Sub
#End Region
 
#Region "Mouse States"
 
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
 
#End Region
 
#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        Size = New Size(100, 22)
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(1, 1, Height - 2, Height - 2)
        Dim Circle As New Rectangle(6, 6, Height - 12, Height - 12)
        Dim SecondBorder As New Rectangle(4, 3, 14, 14)
        With G
            .TextRenderingHint = TextRenderingHint.AntiAliasGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(Color.Transparent)
            .FillEllipse(New SolidBrush(_BackColour), Base)
            .DrawEllipse(New Pen(_BorderColour, 2), Base)
            If Checked Then
                Select Case State
                    Case MouseState.Over
                        .FillEllipse(New SolidBrush(_HoverColour), New Rectangle(2, 2, Height - 4, Height - 4))
                End Select
                .FillEllipse(New SolidBrush(_CheckedColour), Circle)
            Else
                Select Case State
                    Case MouseState.Over
                        .FillEllipse(New SolidBrush(_HoverColour), New Rectangle(2, 2, Height - 4, Height - 4))
                End Select
            End If
            .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(24, 3, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region
 
End Class
 
Public Class ElegantThemeRadialProgressBar
    Inherits Control
 
#Region "Declarations"
    Private _BorderColour As Color = Color.FromArgb(210, 210, 210)
    Private _BaseColour As Color = Color.FromArgb(255, 255, 255)
    Private _ProgressColour As Color = Color.FromArgb(163, 190, 146)
    Private _TextColour As Color = Color.FromArgb(163, 163, 163)
    Private _Value As Integer = 0
    Private _Maximum As Integer = 100
    Private _StartingAngle As Integer = 110
    Private _RotationAngle As Integer = 255
    Private _Font As Font = New Font("Segoe UI", 20)
#End Region
 
#Region "Properties"
 
    <Category("Control")>
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(V As Integer)
            Select Case V
                Case Is < _Value
                    _Value = V
            End Select
            _Maximum = V
            Invalidate()
        End Set
    End Property
 
    <Category("Control")>
    Public Property Value() As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
                    Invalidate()
                Case Else
                    Return _Value
                    Invalidate()
            End Select
        End Get
 
        Set(V As Integer)
            Select Case V
                Case Is > _Maximum
                    V = _Maximum
                    Invalidate()
            End Select
            _Value = V
            Invalidate()
        End Set
    End Property
 
    Public Sub Increment(ByVal Amount As Integer)
        Value += Amount
    End Sub
 
    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property ProgressColour As Color
        Get
            Return _ProgressColour
        End Get
        Set(value As Color)
            _ProgressColour = value
        End Set
    End Property
 
    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property StartingAngle As Integer
        Get
            Return _StartingAngle
        End Get
        Set(value As Integer)
            _StartingAngle = value
        End Set
    End Property
 
    <Category("Control")>
    Public Property RotationAngle As Integer
        Get
            Return _RotationAngle
        End Get
        Set(value As Integer)
            _RotationAngle = value
        End Set
    End Property
 
#End Region
 
#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(78, 78)
        BackColor = Color.Transparent
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        With G
            .TextRenderingHint = TextRenderingHint.AntiAliasGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            Select Case _Value
                Case 0
                    .DrawArc(New Pen(New SolidBrush(_BorderColour), 1 + 6), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle - 3, _RotationAngle + 5)
                    .DrawArc(New Pen(New SolidBrush(_BaseColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle)
                    .DrawString(_Value, _Font, New SolidBrush(_TextColour), New Point(Width / 2, Height / 2 - 1), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Case _Maximum
                    .DrawArc(New Pen(New SolidBrush(_BorderColour), 1 + 6), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle - 3, _RotationAngle + 5)
                    .DrawArc(New Pen(New SolidBrush(_BaseColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle)
                    .DrawArc(New Pen(New SolidBrush(_ProgressColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle)
                    .DrawString(_Value, _Font, New SolidBrush(_TextColour), New Point(Width / 2, Height / 2 - 1), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Case Else
                    .DrawArc(New Pen(New SolidBrush(_BorderColour), 1 + 6), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle - 3, _RotationAngle + 5)
                    .DrawArc(New Pen(New SolidBrush(_BaseColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, _RotationAngle)
                    .DrawArc(New Pen(New SolidBrush(_ProgressColour), 1 + 3), CInt(3 / 2) + 1, CInt(3 / 2) + 1, Width - 3 - 4, Height - 3 - 3, _StartingAngle, CInt((_RotationAngle / _Maximum) * _Value))
                    .DrawString(_Value, _Font, New SolidBrush(_TextColour), New Point(Width / 2, Height / 2 - 1), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            End Select
        End With
        MyBase.OnPaint(e)
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region
 
End Class

