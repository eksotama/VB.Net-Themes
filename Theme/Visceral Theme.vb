Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum

Module Draw
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
End Module

Public Class VisceralButton : Inherits Control
#Region " MouseStates "
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
        BackColor = Color.Transparent
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Arial", 8, FontStyle.Bold)
        Select Case State
            Case MouseState.None
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(61, 61, 63), Color.FromArgb(14, 14, 14), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 3))
                Dim gloss As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(100, Color.FromArgb(61, 61, 63)), Color.FromArgb(12, 255, 255, 255), 90S)
                G.FillPath(gloss, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2), 3))
                G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 3))
                G.DrawString(Text, drawFont, New SolidBrush(ForeColor), New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(245, 61, 61, 63), Color.FromArgb(245, 14, 14, 14), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 3))
                Dim gloss As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(75, Color.FromArgb(61, 61, 63)), Color.FromArgb(20, 255, 255, 255), 90S)
                G.FillPath(gloss, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2), 3))
                G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 3))
                G.DrawString(Text, drawFont, New SolidBrush(ForeColor), New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Down
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(51, 51, 53), Color.FromArgb(4, 4, 4), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 3))
                Dim gloss As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height / 2), Color.FromArgb(75, Color.FromArgb(61, 61, 63)), Color.FromArgb(5, 255, 255, 255), 90S)
                G.FillPath(gloss, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height / 2), 3))
                G.DrawPath(Pens.Black, Draw.RoundRect(ClientRectangle, 3))
                G.DrawString(Text, drawFont, New SolidBrush(ForeColor), New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class VisceralTheme : Inherits ContainerControl
    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.FromArgb(25, 25, 25)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim TopBar As New Rectangle(0, 0, Width - 1, 30)
        Dim Body As New Rectangle(0, 10, Width - 1, Height - 1)

        MyBase.OnPaint(e)

        G.Clear(Color.Fuchsia)

        'G.SmoothingMode = SmoothingMode.HighQuality

        Dim lbb As New LinearGradientBrush(Body, Color.FromArgb(19, 19, 19), Color.FromArgb(17, 17, 17), 90S)
        Dim bodyhatch As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(20, 20, 20), Color.Transparent)
        G.FillPath(lbb, Draw.RoundRect(Body, 5))
        G.FillPath(bodyhatch, Draw.RoundRect(Body, 5))
        G.DrawPath(Pens.Black, Draw.RoundRect(Body, 5))


        Dim lgb As New LinearGradientBrush(TopBar, Color.FromArgb(60, 60, 62), Color.FromArgb(25, 25, 25), 90S)
        'Dim tophatch As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(20, 20, 20), Color.Transparent)
        G.FillPath(lgb, Draw.RoundRect(TopBar, 4))
        'G.FillPath(tophatch, Draw.RoundRect(TopBar, 4))
        G.DrawPath(Pens.Black, Draw.RoundRect(TopBar, 4))
        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(33, 0, Width - 1, 30), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        G.DrawIcon(FindForm.Icon, New Rectangle(11, 8, 16, 16))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Private MouseP As Point = New Point(0, 0)
    Private Cap As Boolean = False
    Private MoveHeight% = 30 : Private pos% = 0
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

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Me.ParentForm.FormBorderStyle = FormBorderStyle.None
        Me.ParentForm.TransparencyKey = Color.Fuchsia
        Dock = DockStyle.Fill
    End Sub
End Class

Public Class VisceralTextBox : Inherits Control
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
        BackColor = Color.FromArgb(15, 15, 15)
        ForeColor = Color.Silver
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        Height = txtbox.Height + 11
        With txtbox
            .Width = Width - 10
            .TextAlign = TextAlignment
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

        G.Clear(BackColor)

        G.FillRectangle(New SolidBrush(Color.FromArgb(10, 10, 10)), ClientRectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(53, 57, 60)), ClientRectangle)

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class VisceralGroupBox : Inherits ContainerControl

    Private _ImageSize As Size
    Protected ReadOnly Property ImageSize() As Size
        Get
            Return _ImageSize
        End Get
    End Property

    Private _Image As Image
    Property Image() As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            If value Is Nothing Then
                _ImageSize = Size.Empty
            Else
                _ImageSize = value.Size
            End If

            _Image = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim TopBar As New Rectangle(10, 0, 130, 25)
        Dim box As New Rectangle(0, 0, Width - 1, Height - 10)

        MyBase.OnPaint(e)

        G.Clear(Color.Transparent)

        G.SmoothingMode = SmoothingMode.HighQuality

        Dim bodygrade As New LinearGradientBrush(ClientRectangle, Color.FromArgb(15, 15, 15), Color.FromArgb(22, 22, 22), 120S)
        G.FillPath(bodygrade, Draw.RoundRect(New Rectangle(1, 12, Width - 3, box.Height - 1), 1))

        Dim outerBorder As New LinearGradientBrush(ClientRectangle, Color.DimGray, Color.Gray, 90S)
        G.DrawPath(New Pen(outerBorder), Draw.RoundRect(New Rectangle(1, 12, Width - 3, Height - 13), 1))
        Dim outerBorder2 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), 90S)
        G.DrawPath(New Pen(outerBorder2), Draw.RoundRect(New Rectangle(2, 13, Width - 5, Height - 15), 1))
        'Dim outerBorder3 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(0, 0, 0), Color.FromArgb(0, 0, 0), 90S)
        'G.DrawPath(New Pen(outerBorder2), Draw.RoundRect(New Rectangle(3, 14, Width - 7, Height - 17), 1))

        Dim lbb As New LinearGradientBrush(TopBar, Color.FromArgb(30, 30, 32), Color.FromArgb(25, 25, 25), 90S)
        G.FillPath(lbb, Draw.RoundRect(TopBar, 1))

        G.DrawPath(Pens.DimGray, Draw.RoundRect(TopBar, 2))

        If Not Image Is Nothing Then
            G.InterpolationMode = InterpolationMode.HighQualityBicubic
            G.DrawImage(Image, New Rectangle(TopBar.Width - 115, 5, 16, 16))
            G.DrawString(Text, Font, Brushes.White, 35, 5)
        Else
            G.DrawString(Text, Font, New SolidBrush(Color.White), TopBar, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class VisceralControlBox : Inherits Control
#Region " MouseStates "
    Dim State As MouseState = MouseState.None
    Dim X As Integer
    Dim MinBtn As New Rectangle(0, 0, 35, 20)
    Dim CloseBtn As New Rectangle(35, 0, 35, 20)
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If X > MinBtn.X And X < MinBtn.X + 35 Then
            FindForm.WindowState = FormWindowState.Minimized
        Else
            FindForm.Close()
        End If
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
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Invalidate()
    End Sub
#End Region

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Merlett", 8, FontStyle.Bold)

        Select Case State
            Case MouseState.None
                Dim lgb As New LinearGradientBrush(MinBtn, Color.FromArgb(50, 50, 50), Color.FromArgb(45, 45, 45), 90S)
                G.FillPath(lgb, Draw.RoundRect(MinBtn, 2.5))
                G.DrawPath(Pens.Black, Draw.RoundRect(MinBtn, 2.5))
                G.DrawString("_", drawFont, New SolidBrush(Color.Silver), MinBtn, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Dim lgb2 As New LinearGradientBrush(CloseBtn, Color.FromArgb(50, 50, 50), Color.FromArgb(45, 45, 45), 90S)
                G.FillPath(lgb2, Draw.RoundRect(CloseBtn, 2.5))
                G.DrawPath(Pens.Black, Draw.RoundRect(CloseBtn, 2.5))
                G.DrawString("x", drawFont, New SolidBrush(Color.Silver), CloseBtn, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                If X > MinBtn.X And X < MinBtn.X + 35 Then
                    Dim lgb As New LinearGradientBrush(MinBtn, Color.FromArgb(50, 85, 255, 85), Color.FromArgb(45, 45, 45), 90S)
                    G.FillPath(lgb, Draw.RoundRect(MinBtn, 2.5))
                    G.DrawPath(Pens.Black, Draw.RoundRect(MinBtn, 2.5))
                    G.DrawString("_", drawFont, New SolidBrush(Color.Silver), MinBtn, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    Dim lgb2 As New LinearGradientBrush(CloseBtn, Color.FromArgb(50, 50, 50), Color.FromArgb(45, 45, 45), 90S)
                    G.FillPath(lgb2, Draw.RoundRect(CloseBtn, 2.5))
                    G.DrawPath(Pens.Black, Draw.RoundRect(CloseBtn, 2.5))
                    G.DrawString("x", drawFont, New SolidBrush(Color.Silver), CloseBtn, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Else
                    Dim lgb2 As New LinearGradientBrush(CloseBtn, Color.FromArgb(50, 30, 30), Color.FromArgb(45, 45, 45), 90S)
                    G.FillPath(lgb2, Draw.RoundRect(CloseBtn, 2.5))
                    G.DrawPath(Pens.Black, Draw.RoundRect(CloseBtn, 2.5))
                    G.DrawString("x", drawFont, New SolidBrush(Color.Silver), CloseBtn, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    Dim lgb As New LinearGradientBrush(MinBtn, Color.FromArgb(50, 50, 50), Color.FromArgb(45, 45, 45), 90S)
                    G.FillPath(lgb, Draw.RoundRect(MinBtn, 2.5))
                    G.DrawPath(Pens.Black, Draw.RoundRect(MinBtn, 2.5))
                    G.DrawString("_", drawFont, New SolidBrush(Color.Silver), MinBtn, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                End If
        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Public Class VisceralCheckBox : Inherits Control   'HELP FROM RECUPERARE

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
        MyBase.OnPaintBackground(pevent)
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
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.White
        Size = New Size(145, 16)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim checkBoxRectangle As New Rectangle(0, 0, Height - 1, Height - 1)

        G.Clear(BackColor)

        Dim bodyGrad As New LinearGradientBrush(checkBoxRectangle, Color.FromArgb(25, 25, 25), Color.FromArgb(35, 35, 35), 120S)
        G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(42, 47, 49)), New Rectangle(1, 1, Height - 3, Height - 3))
        G.DrawRectangle(New Pen(Color.FromArgb(87, 87, 89)), checkBoxRectangle)

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