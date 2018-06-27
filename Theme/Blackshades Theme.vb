Imports System, System.IO, System.Collections.Generic, System.Runtime.InteropServices, System.ComponentModel
Imports System.Drawing, System.Drawing.Drawing2D, System.Drawing.Imaging, System.Windows.Forms

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

'--------------------- [ Theme ] --------------------
'Creator: Recuperare
'Contact: cschaefer2183 (Skype)
'Created: 09.22.2012
'Changed: 09.22.2012
'-------------------- [ /Theme ] ---------------------

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!


Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum

#Region " GLOBAL FUNCTIONS "

Class Draw
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
End Class

#End Region


Public Class BlackShadesNetForm : Inherits ContainerControl

#Region " Control Help - Movement & Flicker Control "
    Private MouseP As Point = New Point(0, 0)
    Private Cap As Boolean = False
    Private MoveHeight As Integer
    Private pos As Integer = 0

    Private Sub minimBtnClick() Handles minimBtn.Click
        ParentForm.FindForm.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub closeBtnClick() Handles closeBtn.Click
        If CloseButtonExitsApp Then
            System.Environment.Exit(0)
        Else
            ParentForm.FindForm.Close()
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Cap = True : MouseP = e.Location
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : Cap = False
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Cap Then
            Parent.Location = MousePosition - MouseP
        End If
    End Sub
    Protected Overrides Sub OnInvalidated(ByVal e As System.Windows.Forms.InvalidateEventArgs)
        MyBase.OnInvalidated(e)
        ParentForm.FindForm.Text = Text
    End Sub
    Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Me.ParentForm.FormBorderStyle = FormBorderStyle.None
        Me.ParentForm.TransparencyKey = Color.Fuchsia
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
    End Sub
    Private _closesEnv As Boolean = False
    Public Property CloseButtonExitsApp() As Boolean
        Get
            Return _closesEnv
        End Get
        Set(ByVal v As Boolean)
            _closesEnv = v
            Invalidate()
        End Set
    End Property

    Private _minimBool As Boolean = True
    Public Property MinimizeButton() As Boolean
        Get
            Return _minimBool
        End Get
        Set(ByVal v As Boolean)
            _minimBool = v
            Invalidate()
        End Set
    End Property

#End Region

    Dim WithEvents minimBtn As New BlackShadesNetTopButton With {.Location = New Point(Width - 44, 7)}
    Dim WithEvents closeBtn As New BlackShadesNetTopButton With {.Location = New Point(Width - 27, 7)}

    Sub New()
        MyBase.New()
        Dock = DockStyle.Fill
        MoveHeight = 25
        Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
        ForeColor = Color.FromArgb(142, 152, 156)
        DoubleBuffered = True

        Controls.Add(closeBtn)

        closeBtn.Refresh() : minimBtn.Refresh()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

        Const curve As Integer = 7
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        If _minimBool Then
            Controls.Add(minimBtn)
        Else
            Controls.Remove(minimBtn)
        End If

        minimBtn.Location = New Point(Width - 44, 7)
        closeBtn.Location = New Point(Width - 27, 7)

        G.SmoothingMode = SmoothingMode.Default
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim TransparencyKey As Color = Me.ParentForm.TransparencyKey
        Dim d As New Draw()
        MyBase.OnPaint(e)

        G.Clear(TransparencyKey)

        G.FillPath(New SolidBrush(Color.FromArgb(42, 47, 49)), d.RoundRect(ClientRectangle, curve))


        'DRAW GRADIENTED BORDER
        Dim innerGradLeft As New LinearGradientBrush(New Rectangle(1, 1, Width / 2 - 1, Height - 3), _
                                             Color.FromArgb(102, 108, 112), Color.FromArgb(204, 216, 224), 0S)
        Dim innerGradRight As New LinearGradientBrush(New Rectangle(1, 1, Width / 2 - 1, Height - 3), _
                                     Color.FromArgb(204, 216, 224), Color.FromArgb(102, 108, 112), 0S)
        G.DrawPath(New Pen(innerGradLeft), d.RoundRect(New Rectangle(1, 1, Width / 2 + 3, Height - 3), curve))
        G.DrawPath(New Pen(innerGradRight), d.RoundRect(New Rectangle(Width / 2 - 5, 1, Width / 2 + 3, Height - 3), curve))


        G.FillPath(New SolidBrush(Color.FromArgb(42, 47, 49)), d.RoundRect(New Rectangle(2, 2, Width - 5, Height - 5), curve))


        Dim topGradLeft As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, 25), Color.FromArgb(42, 46, 48), _
                                                   Color.FromArgb(93, 98, 101), 0S)
        Dim topGradRight As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, 25), Color.FromArgb(93, 98, 101), _
                                           Color.FromArgb(42, 46, 48), 0S)
        G.FillPath(topGradLeft, d.RoundRect(New Rectangle(2, 2, Width / 2 + 2, 25), curve))
        G.FillPath(topGradRight, d.RoundRect(New Rectangle(Width / 2 - 3, 2, Width / 2 - 1, 25), curve))

        G.FillRectangle(New SolidBrush(Color.FromArgb(42, 47, 49)), New Rectangle(2, 23, Width - 5, 10))

        G.DrawPath(New Pen(Color.FromArgb(42, 46, 48)), d.RoundRect(New Rectangle(2, 2, Width - 5, Height - 5), curve))
        G.DrawPath(New Pen(Color.FromArgb(9, 11, 12)), d.RoundRect(ClientRectangle, curve))

        G.DrawString(Text, Font, Brushes.White, New Rectangle(curve, curve - 2, Width - 1, 22), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class BlackShadesNetButton : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
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
    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Private _align As StringAlignment = StringAlignment.Center
    Public Shadows Property TextAlignment() As StringAlignment
        Get
            Return _align
        End Get
        Set(ByVal v As StringAlignment)
            _align = v
            Invalidate()
        End Set
    End Property

#End Region

    Sub New()
        MyBase.New()
        BackColor = Color.FromArgb(38, 38, 38)
        Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
        'ForeColor = Color.FromArgb(142, 152, 156)
        ForeColor = Color.White
        DoubleBuffered = True
        Size = New Size(75, 23)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Const curve As Integer = 3
        Dim ClientRectangle = New Rectangle(0, 0, Width - 1, Height - 1)

        Dim d As New Draw()
        G.SmoothingMode = SmoothingMode.HighQuality
        'G.Clear(Color.FromArgb(20, 20, 20))
        'Dim bg As Color = Parent.FindForm.BackColor
        G.Clear(Color.FromArgb(42, 47, 49))

        Select Case State
            Case MouseState.None 'Mouse None
                G.FillPath(New SolidBrush(Color.FromArgb(32, 36, 38)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), curve))
                Dim gloss As New LinearGradientBrush(New Rectangle(1, 1, Width - 5, Height / 2 - 3), Color.FromArgb(70, Color.White), Color.Transparent, 90S)
                G.FillPath(gloss, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2 - 3), curve))
                Dim borderRect As New LinearGradientBrush(ClientRectangle, Color.FromArgb(36, 31, 43), Color.FromArgb(61, 65, 68), 90S)
                G.DrawPath(New Pen(Color.FromArgb(99, 103, 105)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), curve))
                G.DrawPath(New Pen(borderRect), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), curve))
                G.DrawPath(New Pen(Color.FromArgb(27, 31, 33)), d.RoundRect(New Rectangle(1, 0, Width - 3, Height - 3), curve))
            Case MouseState.Over 'Mouse Hover
                G.FillPath(New SolidBrush(Color.FromArgb(32, 36, 38)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), curve))
                Dim gloss As New LinearGradientBrush(New Rectangle(1, 1, Width - 5, Height / 2 - 3), Color.FromArgb(70, Color.White), Color.Transparent, 90S)
                G.FillPath(gloss, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2 - 3), curve))
                Dim borderRect As New LinearGradientBrush(ClientRectangle, Color.FromArgb(36, 31, 43), Color.FromArgb(61, 65, 68), 90S)
                G.DrawPath(New Pen(Color.FromArgb(99, 103, 105)), d.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), curve))
                G.DrawPath(New Pen(Color.FromArgb(100, 99, 103, 105)), d.RoundRect(New Rectangle(2, 2, Width - 5, Height - 6), curve))
                G.DrawPath(New Pen(borderRect), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), curve))
                G.DrawPath(New Pen(Color.FromArgb(27, 31, 33)), d.RoundRect(New Rectangle(1, 0, Width - 3, Height - 3), curve))
                G.DrawPath(New Pen(Color.FromArgb(0, 186, 255)), d.RoundRect(New Rectangle(1, 1, Width - 3, Height - 4), curve))
            Case MouseState.Down 'Mouse Down
                G.FillPath(New SolidBrush(Color.FromArgb(32, 36, 38)), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), curve))
                Dim topGrad As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2 - 1), Color.FromArgb(32, 36, 38), Color.FromArgb(57, 57, 57), 90S)
                G.FillPath(topGrad, d.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2 + 1), curve))
                Dim botGrad As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2 - 1), Color.FromArgb(57, 57, 57), Color.FromArgb(32, 36, 38), 90S)
                G.FillPath(botGrad, d.RoundRect(New Rectangle(0, Height / 2 - 1, Width - 1, Height / 2 + 2), curve))
                G.DrawLine(New Pen(Color.FromArgb(57, 57, 57)), 0, Convert.ToInt32(Height / 2 - 1), Width - 1, Convert.ToInt32(Height / 2 - 1))

                Dim borderRect As New LinearGradientBrush(ClientRectangle, Color.FromArgb(36, 31, 43), Color.FromArgb(61, 65, 68), 90S)
                G.DrawPath(New Pen(borderRect), d.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), curve))
                G.DrawPath(New Pen(Color.FromArgb(27, 31, 33)), d.RoundRect(New Rectangle(1, 0, Width - 3, Height - 3), curve))

        End Select

        'G.DrawRectangle(Pens.Black, ClientRectangle)

        'G.DrawString(Text, Font, Brushes.Black, New Rectangle(-1, -2, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(-1, -1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class BlackShadesNetTopButton : Inherits Control
#Region " Control Help - MouseState & Flicker Control"

    Private State As MouseState = MouseState.None

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
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
    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
#End Region

    Sub New()
        MyBase.New()
        BackColor = Color.FromArgb(38, 38, 38)
        Font = New Font("Verdana", 8.25F)
        Size = New Size(15, 11)
        DoubleBuffered = True
        Focus()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim d As New Draw()

        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        Size = New Size(15, 11)
        G.Clear(Color.FromArgb(49, 53, 55))

        Select Case State
            Case MouseState.None 'Mouse None
                Dim border As New LinearGradientBrush(ClientRectangle, Color.FromArgb(200, 44, 47, 51), Color.FromArgb(80, 64, 69, 71), 90S)
                G.FillPath(border, d.RoundRect(ClientRectangle, 1))
                Dim bodyGrad As New LinearGradientBrush(New Rectangle(2, 2, Width - 6, Height - 6), Color.FromArgb(90, 97, 101), Color.FromArgb(63, 69, 73), 90S)
                G.FillPath(bodyGrad, d.RoundRect(New Rectangle(2, 2, Width - 6, Height - 6), 1))
                G.DrawPath(New Pen(Color.FromArgb(30, 32, 35)), d.RoundRect(New Rectangle(2, 2, Width - 6, Height - 6), 1))
            Case MouseState.Over
                Dim border As New LinearGradientBrush(ClientRectangle, Color.FromArgb(200, 44, 47, 51), Color.FromArgb(80, 64, 69, 71), 90S)
                G.FillPath(border, d.RoundRect(ClientRectangle, 1))
                Dim bodyGrad As New LinearGradientBrush(New Rectangle(2, 2, Width - 6, Height - 6), Color.FromArgb(90, 97, 101), Color.FromArgb(63, 69, 73), 90S)
                G.FillPath(bodyGrad, d.RoundRect(New Rectangle(2, 2, Width - 6, Height - 6), 1))
                G.DrawPath(New Pen(Color.FromArgb(30, 32, 35)), d.RoundRect(New Rectangle(2, 2, Width - 6, Height - 6), 1))
                G.DrawPath(New Pen(Color.FromArgb(200, 0, 186, 255)), d.RoundRect(New Rectangle(2, 2, Width - 6, Height - 6), 1))
            Case MouseState.Down
                Dim border As New LinearGradientBrush(ClientRectangle, Color.FromArgb(200, 44, 47, 51), Color.FromArgb(80, 64, 69, 71), 90S)
                G.FillPath(border, d.RoundRect(ClientRectangle, 1))
                Dim bodyGrad As New LinearGradientBrush(New Rectangle(2, 2, Width - 6, Height - 6), Color.FromArgb(90, 97, 101), Color.FromArgb(63, 69, 73), 135S)
                G.FillPath(bodyGrad, d.RoundRect(New Rectangle(2, 2, Width - 6, Height - 6), 1))
                G.DrawPath(New Pen(Color.FromArgb(30, 32, 35)), d.RoundRect(New Rectangle(2, 2, Width - 6, Height - 6), 1))
        End Select

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class BlackShadesNetMultiLineTextBox : Inherits Control
    Dim WithEvents txtbox As New TextBox

#Region " Control Help - Properties & Flicker Control "
    Private _maxchars As Integer = 32767
    Public Property MaxCharacters() As Integer
        Get
            Return _maxchars
        End Get
        Set(ByVal v As Integer)
            _maxchars = v
            Invalidate()
        End Set
    End Property
    Private _align As HorizontalAlignment
    Public Shadows Property TextAlignment() As HorizontalAlignment
        Get
            Return _align
        End Get
        Set(ByVal v As HorizontalAlignment)
            _align = v
            Invalidate()
        End Set
    End Property


    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        txtbox.BackColor = BackColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        txtbox.ForeColor = ForeColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        txtbox.Size = New Size(Width - 10, Height - 11)
    End Sub
    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        txtbox.Font = Font
    End Sub
    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
        txtbox.Focus()
    End Sub
    Sub TextChngTxtBox() Handles txtbox.TextChanged
        Text = txtbox.Text
    End Sub
    Sub TextChng() Handles MyBase.TextChanged
        txtbox.Text = Text
    End Sub
    Sub NewTextBox()
        With txtbox
            .Multiline = True
            .BackColor = BackColor
            .ForeColor = ForeColor
            .Text = String.Empty
            .TextAlign = HorizontalAlignment.Center
            .BorderStyle = BorderStyle.None
            .Location = New Point(3, 4)
            .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
            .Size = New Size(Width - 10, Height - 10)
        End With
        txtbox.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
    End Sub
#End Region

    Sub New()
        MyBase.New()

        NewTextBox()
        Controls.Add(txtbox)

        Text = ""
        BackColor = Color.FromArgb(36, 40, 42)
        ForeColor = Color.FromArgb(142, 152, 156)
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim d As New Draw()
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        txtbox.TextAlign = TextAlignment

        G.Clear(Color.FromArgb(36, 40, 42))

        G.FillRectangle(New SolidBrush(Color.FromArgb(36, 40, 42)), ClientRectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(53, 57, 60)), ClientRectangle)

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class BlackShadesNetTextBox : Inherits Control
    Dim WithEvents txtbox As New TextBox

#Region " Control Help - Properties & Flicker Control "
    Private _passmask As Boolean = False
    Public Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _passmask
        End Get
        Set(ByVal v As Boolean)
            txtbox.UseSystemPasswordChar = UseSystemPasswordChar
            _passmask = v
            Invalidate()
        End Set
    End Property
    Private _maxchars As Integer = 32767
    Public Shadows Property MaxLength() As Integer
        Get
            Return _maxchars
        End Get
        Set(ByVal v As Integer)
            _maxchars = v
            txtbox.MaxLength = MaxLength
            Invalidate()
        End Set
    End Property
    Private _align As HorizontalAlignment
    Public Shadows Property TextAlignment() As HorizontalAlignment
        Get
            Return _align
        End Get
        Set(ByVal v As HorizontalAlignment)
            _align = v
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        txtbox.BackColor = BackColor
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
    Sub TextChngTxtBox() Handles txtbox.TextChanged
        Text = txtbox.Text
    End Sub
    Sub TextChng() Handles MyBase.TextChanged
        txtbox.Text = Text
    End Sub
    Sub NewTextBox()
        With txtbox
            .Multiline = False
            .BackColor = Color.FromArgb(43, 43, 43)
            .ForeColor = ForeColor
            .Text = String.Empty
            .TextAlign = HorizontalAlignment.Center
            .BorderStyle = BorderStyle.None
            .Location = New Point(5, 4)
            .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
            .Size = New Size(Width - 10, Height - 11)
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

    End Sub
#End Region

    Sub New()
        MyBase.New()

        NewTextBox()
        Controls.Add(txtbox)

        Text = ""
        BackColor = Color.FromArgb(36, 40, 42)
        ForeColor = Color.FromArgb(142, 152, 156)
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim d As New Draw()
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        Height = txtbox.Height + 11
        With txtbox
            .Width = Width - 10
            .TextAlign = TextAlignment
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

        G.Clear(Color.FromArgb(36, 40, 42))

        G.FillRectangle(New SolidBrush(Color.FromArgb(36, 40, 42)), ClientRectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(53, 57, 60)), ClientRectangle)

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class BlackShadesNetRichTextBox : Inherits Control
    Dim WithEvents txtbox As New RichTextBox

#Region " Control Help - Properties & Flicker Control "


    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        txtbox.BackColor = BackColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        txtbox.ForeColor = ForeColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        txtbox.Size = New Size(Width - 10, Height - 11)
    End Sub
    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        txtbox.Font = Font
    End Sub
    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
        txtbox.Focus()
    End Sub
    Sub TextChngTxtBox() Handles txtbox.TextChanged
        Text = txtbox.Text
    End Sub
    Sub TextChng() Handles MyBase.TextChanged
        txtbox.Text = Text
    End Sub
    Sub NewTextBox()
        With txtbox
            .Multiline = True
            .BackColor = BackColor
            .ForeColor = ForeColor
            .Text = String.Empty
            .BorderStyle = BorderStyle.None
            .Location = New Point(3, 4)
            .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
            .Size = New Size(Width - 10, Height - 10)
        End With
        txtbox.Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
    End Sub
#End Region

    Sub New()
        MyBase.New()

        NewTextBox()
        Controls.Add(txtbox)

        Text = ""
        BackColor = Color.FromArgb(36, 40, 42)
        ForeColor = Color.FromArgb(142, 152, 156)
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim d As New Draw()
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        G.Clear(Color.FromArgb(36, 40, 42))

        G.DrawRectangle(New Pen(Color.FromArgb(30, 33, 35), 2), ClientRectangle)
        G.DrawLine(New Pen(Color.FromArgb(83, 90, 94)), Width - 1, 0, Width - 1, Height)
        G.DrawLine(New Pen(Color.FromArgb(83, 90, 94)), 0, Height - 1, Width - 1, Height - 1)

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Public Class BlackShadesNetCheckBox : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
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
    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Private _Checked As Boolean
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 14
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
#End Region

    Sub New()
        MyBase.New()
        BackColor = Color.FromArgb(20, 20, 20)
        ForeColor = Color.White
        Size = New Size(145, 16)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim d As New Draw()

        Dim checkBoxRectangle As New Rectangle(0, 0, Height - 1, Height - 1)

        G.Clear(Color.FromArgb(42, 47, 49))

        Dim bodyGrad As New LinearGradientBrush(checkBoxRectangle, Color.FromArgb(36, 40, 42), Color.FromArgb(64, 71, 74), 90S)
        G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(42, 47, 49)), New Rectangle(1, 1, Height - 3, Height - 3))
        G.DrawRectangle(New Pen(Color.FromArgb(102, 108, 112)), checkBoxRectangle)


        If Checked Then
            Dim chkPoly As Rectangle = New Rectangle(checkBoxRectangle.X + checkBoxRectangle.Width / 4, checkBoxRectangle.Y + checkBoxRectangle.Height / 4, checkBoxRectangle.Width \ 2, checkBoxRectangle.Height \ 2)
            Dim Poly() As Point = {New Point(chkPoly.X, chkPoly.Y + chkPoly.Height \ 2), _
                           New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height), _
                           New Point(chkPoly.X + chkPoly.Width, chkPoly.Y)}
            G.SmoothingMode = SmoothingMode.HighQuality
            Dim P1 As New Pen(Color.FromArgb(250, 255, 255, 255), 2)
            Dim chkGrad As New LinearGradientBrush(chkPoly, Color.FromArgb(200, 200, 200), Color.FromArgb(255, 255, 255), 0S)
            For i = 0 To Poly.Length - 2
                G.DrawLine(P1, Poly(i), Poly(i + 1))
            Next
        End If
        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(18, -1), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Public Class BlackShadesNetRadioButton : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private R1 As Rectangle
    Private G1 As LinearGradientBrush

    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
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
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 16
    End Sub
    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
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
    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        InvalidateControls()
    End Sub
    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is BlackShadesNetRadioButton Then
                DirectCast(C, BlackShadesNetRadioButton).Checked = False
            End If
        Next
    End Sub
#End Region

    Sub New()
        MyBase.New()
        BackColor = Color.FromArgb(42, 47, 49)
        ForeColor = Color.White
        Size = New Size(150, 16)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim d As New Draw()
        Dim radioBtnRectangle = New Rectangle(0, 0, Height - 1, Height - 1)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(BackColor)

        Dim bgGrad As New LinearGradientBrush(radioBtnRectangle, Color.FromArgb(36, 40, 42), Color.FromArgb(66, 70, 72), 90S)
        G.FillEllipse(bgGrad, radioBtnRectangle)

        G.DrawEllipse(New Pen(Color.FromArgb(44, 48, 50)), New Rectangle(1, 1, Height - 3, Height - 3))
        G.DrawEllipse(New Pen(Color.FromArgb(102, 108, 112)), radioBtnRectangle)

        If Checked Then
            Dim chkGrad As New LinearGradientBrush(New Rectangle(4, 4, Height - 9, Height - 8), Color.White, Color.Black, 90S)
            G.FillEllipse(chkGrad, New Rectangle(4, 4, Height - 9, Height - 9))
        End If

        G.DrawString(Text, Font, Brushes.White, New Point(18, 0), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class

Public Class BlackShadesNetGroupBox : Inherits ContainerControl
#Region " Control Help - Properties & Flicker Control"
    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
#End Region

    Sub New()
        MyBase.New()
        BackColor = Color.FromArgb(33, 33, 33)
        Size = New Size(200, 100)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.SmoothingMode = SmoothingMode.HighSpeed
        Const curve As Integer = 3
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim TransparencyKey As Color = Me.ParentForm.TransparencyKey
        Dim d As New Draw()
        MyBase.OnPaint(e)

        G.Clear(Color.FromArgb(42, 47, 49))



        G.DrawPath(New Pen(Color.FromArgb(67, 75, 78)), d.RoundRect(New Rectangle(2, 7, Width - 5, Height - 9), curve))
        Dim outerBorder As New LinearGradientBrush(ClientRectangle, Color.FromArgb(30, 32, 32), Color.Transparent, 90S)
        G.DrawPath(New Pen(outerBorder), d.RoundRect(New Rectangle(1, 6, Width - 3, Height - 9), curve))
        Dim innerBorder As New LinearGradientBrush(New Rectangle(3, 7, Width - 7, Height - 10), Color.Transparent, Color.FromArgb(30, 32, 32), 90S)
        G.DrawPath(New Pen(innerBorder), d.RoundRect(New Rectangle(3, 7, Width - 7, Height - 10), curve))

        G.FillRectangle(New SolidBrush(Color.FromArgb(42, 47, 49)), New Rectangle(8, 0, Text.Length * 6, 11))

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(8, 0, Width - 1, 11), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class