Imports System.Drawing.Drawing2D
Imports System.ComponentModel

'--------------------- [ THEME ] ---------------------
'Name: Elementary Theme
'Creator: Ashlanfox
'Contact: Ashlanfox (Skype)
'Created: 03.01.2015
'Changed: 03.01.2015
'Thanks to: UnReLaTeDScript (Resize & Move events), Sempiternal (TextBox)!
'Inspired of: Flat design (by Hongkiat).
'-------------------- [ /THEME ] ---------------------

'
Class ElTheme
    Inherits ContainerControl

#Region "Functions | Create control"
    Private CreateRoundPath As GraphicsPath
    Function CreateTopRightRound(ByVal r As Rectangle, ByVal slope As Integer, ByVal size As Single) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)

        CreateRoundPath.AddLine(r.Right - size, r.Y, r.Right - slope, r.Y)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddLine(r.Right, r.Y + slope, r.Right, r.Bottom)
        CreateRoundPath.AddLine(r.Right - size, r.Bottom, r.Right - slope, r.Bottom)
        CreateRoundPath.AddLine(r.Right - size, r.Bottom, r.Right - size, r.Y)

        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

    Function CreateTopLeftRound(ByVal r As Rectangle, ByVal slope As Integer, ByVal size As Single) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)

        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddLine(r.X + slope, r.Y, size, r.Y)
        CreateRoundPath.AddLine(size, r.Y, size, r.Bottom)
        CreateRoundPath.AddLine(size, r.Bottom, r.X, r.Bottom)
        CreateRoundPath.AddLine(r.X, r.Bottom, r.X, r.Y + slope)

        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
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

#Region "Subs | Events Move & Resize"
    Private _TransparencyKey As Color = Color.Empty
    Private _MoveHeight As Integer = 38

    Private ParentIsForm As Boolean
    Protected Overrides Sub OnTextChanged(e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Refresh()
    End Sub

    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        Dock = DockStyle.Fill
        ParentIsForm = TypeOf Parent Is Form
        If ParentIsForm Then
            If Not _TransparencyKey = Color.Empty Then ParentForm.TransparencyKey = _TransparencyKey
            ParentForm.FormBorderStyle = FormBorderStyle.None
        End If
        MyBase.OnHandleCreated(e)
    End Sub

    Private _Resizable As Boolean = True
    Property Resizable() As Boolean
        Get
            Return _Resizable
        End Get
        Set(ByVal value As Boolean)
            _Resizable = value
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

    Sub New()
        DoubleBuffered = True
        _TransparencyKey = Color.Fuchsia

        BackColor = Color.White
        ForeColor = Color.FromArgb(115, 115, 115)
        Font = New Font("Tahoma", 9, FontStyle.Regular)
        SetStyle(DirectCast(139270, ControlStyles), True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.Clear(Color.Fuchsia)

        G.SmoothingMode = SmoothingMode.Default
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        G.InterpolationMode = InterpolationMode.HighQualityBicubic

        Dim size As Single = Width / 8
        Dim high As Integer = 8
        Dim greyHigh As Integer = 28
        Dim slope As Integer = 7

        G.FillPath(Brushes.White, CreateRound(New Rectangle(0, 0, Width, Height), slope))
        G.FillRectangle(New SolidBrush(Color.FromArgb(247, 247, 247)), New Rectangle(0, high, Width, greyHigh))
        G.DrawLine(New Pen(Color.FromArgb(238, 238, 238)), New Point(0, greyHigh + high), New Point(Width, greyHigh + high))

        'I multiplied by 1.5 the width to delete possible blank between each rectangle
        G.FillPath(New SolidBrush(Color.FromArgb(195, 225, 127)), CreateTopLeftRound(New Rectangle(0, 0, Width, high), slope, size))
        G.FillRectangle(New SolidBrush(Color.FromArgb(247, 254, 202)), New Rectangle(size * 1, 0, size * 1.5, high))
        G.FillRectangle(New SolidBrush(Color.FromArgb(255, 205, 114)), New Rectangle(size * 2, 0, size * 1.5, high))
        G.FillRectangle(New SolidBrush(Color.FromArgb(241, 119, 106)), New Rectangle(size * 3, 0, size * 1.5, high))
        G.FillRectangle(New SolidBrush(Color.FromArgb(217, 158, 190)), New Rectangle(size * 4, 0, size * 1.5, high))
        G.FillRectangle(New SolidBrush(Color.FromArgb(196, 120, 219)), New Rectangle(size * 5, 0, size * 1.5, high))
        G.FillRectangle(New SolidBrush(Color.FromArgb(103, 153, 222)), New Rectangle(size * 6, 0, size * 1.5, high))
        G.FillPath(New SolidBrush(Color.FromArgb(100, 194, 228)), CreateTopRightRound(New Rectangle(0, 0, Width, high), slope, size))

        Dim TextW As Integer = e.Graphics.MeasureString(Text, Font).Width
        Dim TextH As Integer = e.Graphics.MeasureString(Text, Font).Height

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point((Width / 2) - (TextW / 2), high + (greyHigh / 2) - (TextH / 2) + 1))

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.High
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class ElButton
    Inherits Control

#Region "Function | Create control"
    Private CreateRoundPath As GraphicsPath
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

#Region "Subs | Events"
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Cursor = Cursors.Hand
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
#End Region

#Region "Properties | Settings"
    Private State As MouseState = MouseState.None

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True

        BackColor = Color.Transparent
        Font = New Font("Tahoma", 8, FontStyle.Regular)
        Size = New Size(120, 40)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.Clear(BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        If Enabled = False Then State = MouseState.Block
        Dim slope As Integer = 7

        Select Case State
            Case MouseState.None
                G.FillPath(New SolidBrush(Color.FromArgb(214, 107, 97)), CreateRound(New Rectangle(0, 0, Width, Height), slope))
                G.FillPath(New SolidBrush(Color.FromArgb(241, 119, 108)), CreateRound(New Rectangle(0, 0, Width, Height - 4), slope))
            Case MouseState.Over

                Cursor = Cursors.Hand
                G.FillPath(New SolidBrush(Color.FromArgb(214, 107, 97)), CreateRound(New Rectangle(0, 0, Width, Height), slope))
                G.FillPath(New SolidBrush(Color.FromArgb(241, 129, 119)), CreateRound(New Rectangle(0, 0, Width, Height - 4), slope))
            Case MouseState.Down

                Cursor = Cursors.Arrow
                G.FillPath(New SolidBrush(Color.FromArgb(214, 107, 97)), CreateRound(New Rectangle(0, 0, Width, Height), slope))
                G.FillPath(New SolidBrush(Color.FromArgb(241, 119, 108)), CreateRound(New Rectangle(0, 0, Width, Height - 2), slope))
            Case MouseState.Block

                Cursor = Cursors.Arrow
                G.FillPath(New SolidBrush(Color.FromArgb(214, 107, 97)), CreateRound(New Rectangle(0, 0, Width, Height), slope))
                G.FillPath(New SolidBrush(Color.FromArgb(241, 119, 108)), CreateRound(New Rectangle(0, 0, Width, Height - 2), slope))
        End Select

        G.DrawString(Text, Font, New SolidBrush(Color.White), New Rectangle(-2, 0, Width, Height), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.High
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()

    End Sub
End Class

Public Class ElTextBox
    Inherits Control

    Dim WithEvents txtbox As New TextBox

#Region "Function | Create control"
    Private CreateRoundPath As GraphicsPath
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

#Region "Control Help | Properties & Flicker Control"
    Private _UsePassword As Boolean = False
    Public Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _UsePassword
        End Get
        Set(ByVal v As Boolean)
            txtbox.UseSystemPasswordChar = UseSystemPasswordChar
            _UsePassword = v
            Invalidate()
        End Set
    End Property
    Private _MaxCharacters As Integer = 32767
    Public Shadows Property MaxLength() As Integer
        Get
            Return _MaxCharacters
        End Get
        Set(ByVal v As Integer)
            _MaxCharacters = v
            txtbox.MaxLength = MaxLength
            Invalidate()
        End Set
    End Property
    Private _TextAlignment As HorizontalAlignment
    Public Shadows Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlignment
        End Get
        Set(ByVal v As HorizontalAlignment)
            _TextAlignment = v
            Invalidate()
        End Set
    End Property
    Private _MultiLine As Boolean = False
    Public Shadows Property MultiLine() As Boolean
        Get
            Return _MultiLine
        End Get
        Set(ByVal value As Boolean)
            _MultiLine = value
            txtbox.Multiline = value
            OnResize(EventArgs.Empty)
            Invalidate()
        End Set
    End Property


    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        txtbox.ForeColor = ForeColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        txtbox.Font = Font
    End Sub
    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
        txtbox.Focus()
    End Sub
    Private Sub TextChngTxtBox() Handles txtbox.TextChanged
        Text = txtbox.Text
    End Sub
    Private Sub TextChng() Handles MyBase.TextChanged
        txtbox.Text = Text
    End Sub
    Sub NewTextBox()
        With txtbox
            .Multiline = False
            .BackColor = Color.White
            .ForeColor = ForeColor
            .Text = String.Empty
            .TextAlign = HorizontalAlignment.Center
            .BorderStyle = BorderStyle.None
            .Location = New Point(5, 4)
            .Font = New Font("Tahoma", 8.25F, FontStyle.Regular)
            .Size = New Size(Width - 10, Height - 11)
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

    End Sub
#End Region

    Sub New()
        MyBase.New()

        NewTextBox()
        Controls.Add(txtbox)

        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        Text = String.Empty
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 203, 206)
        Font = New Font("Tahoma", 8.25F, FontStyle.Regular)
        Size = New Size(135, 30)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Curve As Integer = 4
        G.SmoothingMode = SmoothingMode.HighQuality

        With txtbox
            .TextAlign = TextAlign
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

        G.Clear(Color.Transparent)

        Dim slope As Integer = 5
        G.FillPath(New SolidBrush(Color.FromArgb(225, 225, 225)), CreateRound(New Rectangle(0, 0, Width, Height), slope))
        G.FillPath(New SolidBrush(Color.White), CreateRound(New Rectangle(0, 0, Width - 1, Height - 3), slope))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If Not MultiLine Then
            Dim TextBoxHeight As Integer = txtbox.Height
            txtbox.Location = New Point(10, (Height / 2) - (TextBoxHeight / 2) - 1)
            txtbox.Size = New Size(Width - 20, TextBoxHeight)
        Else
            Dim TextBoxHeight As Integer = txtbox.Height
            txtbox.Location = New Point(10, 10)
            txtbox.Size = New Size(Width - 20, Height - 20)
        End If
    End Sub
End Class