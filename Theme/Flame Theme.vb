Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Drawing.Text

Module Draw

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

    Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
        Dim Rectangle As Rectangle = New Rectangle(X, Y, Width, Height)
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function

    Public Sub InnerGlow(ByVal G As Graphics, ByVal Rectangle As Rectangle, ByVal Colors As Color())
        Dim SubtractTwo As Integer = 1
        Dim AddOne As Integer = 0
        For Each c In Colors
            G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(c.R, c.B, c.G))), Rectangle.X + AddOne, Rectangle.Y + AddOne, Rectangle.Width - SubtractTwo, Rectangle.Height - SubtractTwo)
            SubtractTwo += 2
            AddOne += 1
        Next
    End Sub

    Public Sub InnerGlowRounded(ByVal G As Graphics, ByVal Rectangle As Rectangle, ByVal Degree As Integer, ByVal Colors As Color())
        Dim SubtractTwo As Integer = 1
        Dim AddOne As Integer = 0
        For Each c In Colors
            G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(c.R, c.B, c.G))), Draw.RoundRect(Rectangle.X + AddOne, Rectangle.Y + AddOne, Rectangle.Width - SubtractTwo, Rectangle.Height - SubtractTwo, Degree))
            SubtractTwo += 2
            AddOne += 1
        Next
    End Sub

End Module

Class flameTheme
    Inherits ContainerControl

#Region " Declarations "
    Private _Header As Integer = 30
    Private _Down As Boolean = False
    Private _MousePoint As Point

    Private _fromHeight As Integer = 60
#End Region

#Region " MouseStates "

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        _Down = False
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Location.Y < _Header AndAlso e.Button = Windows.Forms.MouseButtons.Left Then
            _Down = True
            _MousePoint = e.Location
        End If
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If _Down = True Then
            ParentForm.Location = MousePosition - _MousePoint
        End If
    End Sub

#End Region

#Region " Properties "

    Private _SubHeader As Boolean = True
    <Category("Settings")>
    Public Property SubHeader() As Boolean
        Get
            Return _SubHeader
        End Get
        Set(ByVal value As Boolean)
            _SubHeader = value
        End Set
    End Property

    Private _SubText As String
    Public Property SubText() As String
        Get
            Return _SubText
        End Get
        Set(ByVal value As String)
            _SubText = value
            Invalidate()
        End Set
    End Property

#End Region

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        ParentForm.FormBorderStyle = FormBorderStyle.None
        ParentForm.TransparencyKey = Color.Fuchsia
        Dock = DockStyle.Fill
        Invalidate()
        _SubText = "Insert Sub Text Here!"
    End Sub

    Sub New()
        BackColor = Color.Fuchsia
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G = e.Graphics
        Dim mainRect As New Rectangle(0, 0, Width, Height)
        Dim _StringF As New StringFormat
        _StringF.LineAlignment = StringAlignment.Center
        _StringF.Alignment = StringAlignment.Center
        G.Clear(Color.Fuchsia)

        Dim c As Color() = New Color() {Color.FromArgb(50, 50, 50), Color.FromArgb(45, 45, 45), Color.FromArgb(40, 40, 40), Color.FromArgb(35, 35, 35), Color.FromArgb(30, 30, 30), Color.FromArgb(25, 25, 25), Color.FromArgb(20, 20, 20), Color.FromArgb(15, 15, 15), Color.FromArgb(10, 10, 10)}
        G.FillRectangle(New SolidBrush(Color.FromArgb(0, 0, 0)), mainRect)
        InnerGlow(G, mainRect, c)


        'Main Bar
        Dim MainBar As New LinearGradientBrush(New Rectangle(0, 0, Width, _Header), Color.DarkRed, Color.Black, LinearGradientMode.Vertical)
        G.FillRectangle(MainBar, New Rectangle(0, 0, Width, _Header))
        G.DrawString(Text, New Font("Segoe UI", 12), Brushes.WhiteSmoke, New RectangleF(0, 0, Width, _Header), _StringF)

        'Sub Bar
        If _SubHeader = True Then
            Dim SubBar As New LinearGradientBrush(New Rectangle(0, _Header, Width, _Header), Color.Black, Color.Black, LinearGradientMode.Vertical)
            G.FillRectangle(SubBar, New Rectangle(0, _Header, Width, _Header))
            InnerGlow(G, New Rectangle(0, _Header, Width - 1, _Header), c)
            G.DrawRectangle(New Pen(Brushes.DarkRed), 0, _Header, Width, _Header)
            G.DrawString(_SubText, New Font("Segoe UI", 10), Brushes.DarkRed, New RectangleF(0, _Header, Width - 1, _Header - 1), _StringF)
        Else
            G.DrawLine(Pens.DarkRed, 0, 30, Width, 30)
        End If

    End Sub

End Class

Class flameButton
    Inherits Control

#Region " MouseStates "

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum

    Dim State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
#End Region

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Black
        ForeColor = Color.FromArgb(205, 205, 205)
        DoubleBuffered = True
        Size = New Point(100, 35)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim drawFont As New Font("Segoe UI", 10)
        Dim _StringF As New StringFormat
        _StringF.LineAlignment = StringAlignment.Center
        _StringF.Alignment = StringAlignment.Center
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        MyBase.OnPaint(e)
        G.Clear(BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim MainBar As New LinearGradientBrush(New Rectangle(2, 2, Width - 2, Height - 2), Color.DarkRed, Color.Black, LinearGradientMode.Vertical)
        G.FillPath(MainBar, Draw.RoundRect(ClientRectangle, 1))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(15, 15, 15))), Draw.RoundRect(ClientRectangle, 1))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(40, 40, 40))), Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 1))

        Select Case State
            Case MouseState.None
                G.DrawString(Text, drawFont, Brushes.WhiteSmoke, New Rectangle(0, 0, Width - 1, Height - 1), _StringF)
            Case MouseState.Over
                G.DrawString(Text, drawFont, Brushes.White, New Rectangle(0, 0, Width - 1, Height - 1), _StringF)
            Case MouseState.Down
                G.DrawString(Text, drawFont, Brushes.Gray, New Rectangle(0, 0, Width - 1, Height - 1), _StringF)
        End Select

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Class flameGroupBox
    Inherits ContainerControl

    Public Enum colors
        LightGrey
        DarkGrey
    End Enum

#Region " Properties "

    Private _Center As Boolean = True
    <Category("Settings")>
    Public Property TitleCenter() As Boolean
        Get
            Return _Center
        End Get
        Set(ByVal value As Boolean)
            _Center = value
        End Set
    End Property

#End Region

    Sub New()
        Size = New Size(200, 100)
        BackColor = Color.White
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G = e.Graphics
        Dim mainRect As New Rectangle(0, 30, Width, Height - 30)
        G.Clear(BackColor)

        'Main Panel
        Dim c As Color() = New Color() {Color.FromArgb(50, 50, 50), Color.FromArgb(45, 45, 45), Color.FromArgb(40, 40, 40), Color.FromArgb(35, 35, 35), Color.FromArgb(30, 30, 30), Color.FromArgb(25, 25, 25), Color.FromArgb(20, 20, 20), Color.FromArgb(15, 15, 15), Color.FromArgb(10, 10, 10)}
        G.FillRectangle(New SolidBrush(Color.FromArgb(0, 0, 0)), mainRect)
        InnerGlow(G, mainRect, c)

        'Main Bar
        Dim MainBar As New LinearGradientBrush(New Rectangle(0, 0, Width, 30), Color.DarkRed, Color.Black, LinearGradientMode.Vertical)
        G.FillRectangle(MainBar, New Rectangle(0, 0, Width, 30))
        G.DrawRectangle(New Pen(Brushes.DarkRed), -1, 30, Width + 1, Height - 31)

        'Main Text
        Dim _StringF As New StringFormat
        _StringF.LineAlignment = StringAlignment.Center
        If _Center = True Then
            _StringF.Alignment = StringAlignment.Center
        Else
            _StringF.Alignment = StringAlignment.Near
        End If
        G.DrawString(Text, New Font("Segoe UI", 10), Brushes.White, New RectangleF(0, 0, Width, 30), _StringF)
    End Sub

End Class

<DefaultEvent("TextChanged")>
Class flameTextBox
    Inherits Control

#Region " Declarations "
    Private WithEvents _TextBox As Windows.Forms.TextBox
#End Region

#Region " Properties "

    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    <Category("Settings")> _
    Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If _TextBox IsNot Nothing Then
                _TextBox.TextAlign = value
            End If
        End Set
    End Property

    Private _MaxLength As Integer = 32767
    <Category("Settings")> _
    Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If _TextBox IsNot Nothing Then
                _TextBox.MaxLength = value
            End If
        End Set
    End Property

    Private _ReadOnly As Boolean
    <Category("Settings")> _
    Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If _TextBox IsNot Nothing Then
                _TextBox.ReadOnly = value
            End If
        End Set
    End Property

    Private _UseSystemPasswordChar As Boolean
    <Category("Settings")> _
    Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If _TextBox IsNot Nothing Then
                _TextBox.UseSystemPasswordChar = value
            End If
        End Set
    End Property

    Private _Multiline As Boolean
    <Category("Settings")> _
    Property Multiline() As Boolean
        Get
            Return _Multiline
        End Get
        Set(ByVal value As Boolean)
            _Multiline = value
            If _TextBox IsNot Nothing Then
                _TextBox.Multiline = value

                If value Then
                    _TextBox.Height = Height - 11
                Else
                    Height = _TextBox.Height + 11
                End If

            End If
        End Set
    End Property

    <Category("Settings")> _
    Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If _TextBox IsNot Nothing Then
                _TextBox.Text = value
            End If
        End Set
    End Property

    <Category("Settings")> _
    Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If _TextBox IsNot Nothing Then
                _TextBox.Font = value
                _TextBox.Location = New Point(3, 5)
                _TextBox.Width = Width - 6

                If Not _Multiline Then
                    Height = _TextBox.Height + 11
                End If
            End If
        End Set
    End Property

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(_TextBox) Then
            Controls.Add(_TextBox)
        End If
    End Sub

    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
        Text = _TextBox.Text
    End Sub

    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            _TextBox.SelectAll()
            e.SuppressKeyPress = True
        End If
        If e.Control AndAlso e.KeyCode = Keys.C Then
            _TextBox.Copy()
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        _TextBox.Location = New Point(5, 5)
        _TextBox.Width = Width - 10

        If _Multiline Then
            _TextBox.Height = Height - 11
        Else
            Height = _TextBox.Height + 11
        End If
        MyBase.OnResize(e)
    End Sub

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True

        BackColor = Color.Transparent

        _TextBox = New Windows.Forms.TextBox
        _TextBox.Font = New Font("Segoe UI", 10)
        _TextBox.Text = Text
        _TextBox.BackColor = Color.Black
        _TextBox.ForeColor = Color.DarkRed
        _TextBox.MaxLength = _MaxLength
        _TextBox.Multiline = _Multiline
        _TextBox.ReadOnly = _ReadOnly
        _TextBox.UseSystemPasswordChar = _UseSystemPasswordChar
        _TextBox.BorderStyle = BorderStyle.None
        _TextBox.Location = New Point(5, 5)
        _TextBox.Width = 100
        _TextBox.Height = 30

        _TextBox.Cursor = Cursors.IBeam

        If _Multiline Then
            _TextBox.Height = Height - 11
        Else
            Height = _TextBox.Height + 11
        End If

        AddHandler _TextBox.TextChanged, AddressOf OnBaseTextChanged
        AddHandler _TextBox.KeyDown, AddressOf OnBaseKeyDown
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G = e.Graphics
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim GP, GP1 As New GraphicsPath
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim BorderBase As New Rectangle(0, 0, Width - 1, Height - 1)
        GP = Draw.RoundRec(Base, 1)
        GP1 = Draw.RoundRec(BorderBase, 1)
        G.FillPath(New SolidBrush(Color.Black), GP)

        Dim c As Color() = New Color() {Color.FromArgb(50, 50, 50), Color.FromArgb(45, 45, 45), Color.FromArgb(40, 40, 40), Color.FromArgb(35, 35, 35), Color.FromArgb(30, 30, 30), Color.FromArgb(25, 25, 25), Color.FromArgb(20, 20, 20), Color.FromArgb(15, 15, 15), Color.FromArgb(10, 10, 10)}
        Dim SubBar As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.Black, Color.Black, LinearGradientMode.Vertical)
        G.FillRectangle(SubBar, New Rectangle(0, 0, Width, Height))
        InnerGlow(G, New Rectangle(-3, 0, Width + 6, Height), c)

        'G.DrawPath(New Pen(Brushes.DarkRed), GP1)
        G.DrawRectangle(New Pen(Brushes.DarkRed), 0, 0, Width - 1, Height - 1)
        _TextBox.ForeColor = Color.DarkRed

    End Sub

End Class

<DefaultEvent("CheckedChanged")>
Class flameCheckBox
    Inherits Control

#Region " Declarations "
    Private _Checked As Boolean
#End Region

#Region " Properties "
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
        If Not _Checked Then
            Checked = True
        Else
            Checked = False
        End If
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 16
    End Sub

#End Region

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G = e.Graphics

        G.Clear(Color.Black)
        G.FillRectangle(New SolidBrush(Color.Black), New Rectangle(0, 0, 14, 14))
        G.DrawRectangle(New Pen(Brushes.DarkRed), 0, 0, 14, 14)
        G.DrawString(Text, New Font("Segoe UI", 10), Brushes.White, New Point(18, -2))

        If Checked Then
            G.FillRectangle(New SolidBrush(Color.DarkRed), New Rectangle(4, 4, 7, 7))
        End If

    End Sub
End Class

Class flameProgressBar
    Inherits Control

#Region " Properties "

    Private _Maximum As Integer
    <Category("Settings")>
    Public Property Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            _Maximum = value
            Invalidate()
        End Set
    End Property

    Private _Minimum As Integer
    <Category("Settings")>
    Public Property Minimum As Integer
        Get
            Return _Minimum
        End Get
        Set(ByVal value As Integer)
            _Minimum = value
            Invalidate()
        End Set
    End Property

    Private _Value As Integer
    <Category("Settings")>
    Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            _Value = value
            Invalidate()
        End Set
    End Property

#End Region

    Sub New()
        Maximum = 100
        Minimum = 0
        Value = 0
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G = e.Graphics

        G.DrawRectangle(New Pen(Color.DarkRed), New Rectangle(0, 0, Width, Height))
        G.FillRectangle(New SolidBrush(Color.Black), New Rectangle(1, 1, Width - 2, Height - 2))

        Dim MainBar As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.DarkRed, Color.Black, LinearGradientMode.Vertical)

        Select Case _Value
            Case Is > 2
                G.FillRectangle(MainBar, New Rectangle(0, 0, CInt(Value / Maximum * Width), Height))
            Case Is > 0
                G.FillRectangle(MainBar, New Rectangle(0, 0, CInt(Value / Maximum * Width), Height))
        End Select

        Dim c As Color() = New Color() {Color.FromArgb(50, 50, 50), Color.FromArgb(45, 45, 45), Color.FromArgb(40, 40, 40), Color.FromArgb(35, 35, 35), Color.FromArgb(30, 30, 30), Color.FromArgb(25, 25, 25), Color.FromArgb(20, 20, 20), Color.FromArgb(15, 15, 15), Color.FromArgb(10, 10, 10)}
        InnerGlow(G, New Rectangle(-4, -2, Width + 8, Height + 4), c)
        G.DrawRectangle(New Pen(Brushes.DarkRed), 0, 0, Width - 1, Height - 1)

    End Sub

End Class