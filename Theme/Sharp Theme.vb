Imports System.ComponentModel, System.Drawing, System.Drawing.Drawing2D
Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum

Class SharpButton

    Inherits Control

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
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

#End Region
    Public Property Color2() As Boolean
        Get
            Return _Color2
        End Get
        Set(ByVal value As Boolean)
            _Color2 = value
            Me.Refresh()
        End Set
    End Property
    Private _Color2 As Boolean = False
    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        ForeColor = Color.FromArgb(210, 220, 230)
        DoubleBuffered = True
        Font = New Font("Verdana", 8.5F, FontStyle.Regular)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim bmp As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(bmp)
        MyBase.OnPaint(e)

        Select Case State
            Case MouseState.None
                If _Color2 Then
                    G.Clear(Color.FromArgb(43, 53, 63))
                    Dim BTNLGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(37, 47, 57), Color.FromArgb(140, 149, 155), 180)
                    G.FillRectangle(BTNLGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim BTNLGB1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(140, 149, 155), Color.FromArgb(37, 47, 57), 180)
                    G.FillRectangle(BTNLGB1, New Rectangle(1, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(23, 33, 43))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(113, 123, 133), Color.FromArgb(50, 50, 50), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(63, 73, 83))), 2, Height - 2, Width - 2, Height - 2)
                    G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(0, -1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                Else
                    G.Clear(Color.FromArgb(43, 53, 63))
                    Dim BTNLGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(37, 47, 57), Color.FromArgb(140, 149, 155), LinearGradientMode.Horizontal)
                    G.FillRectangle(BTNLGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim BTNLGB1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(140, 149, 155), Color.FromArgb(37, 47, 57), LinearGradientMode.Horizontal)
                    G.FillRectangle(BTNLGB1, New Rectangle(1, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(33, 43, 53))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(153, 163, 173), Color.FromArgb(50, 50, 50), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(73, 83, 93))), 1, Height - 2, Width - 2, Height - 2)
                    G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(0, -1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                End If
            Case MouseState.Over
                If _Color2 Then
                    G.Clear(Color.FromArgb(47, 57, 67))
                    Dim LGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(43, 53, 63), Color.FromArgb(157, 166, 172), 180)
                    G.FillRectangle(LGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim LGBOver1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(157, 166, 172), Color.FromArgb(43, 53, 63), 180)
                    G.FillRectangle(LGBOver1, New Rectangle(1, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(29, 39, 49))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(153, 163, 173), Color.FromArgb(50, 50, 50), 90S)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(73, 83, 93))), 1, Height - 2, Width - 2, Height - 2)
                    G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(200, 210, 220)), New Rectangle(0, -1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                Else
                    G.Clear(Color.FromArgb(51, 61, 71))
                    Dim LGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(43, 53, 63), Color.FromArgb(154, 163, 169), LinearGradientMode.Horizontal)
                    G.FillRectangle(LGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim LGBOver1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(154, 163, 169), Color.FromArgb(43, 53, 63), LinearGradientMode.Horizontal)
                    G.FillRectangle(LGBOver1, New Rectangle(0, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(43, 53, 63))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(163, 173, 183), Color.FromArgb(60, 60, 60), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(83, 93, 103))), 1, Height - 2, Width - 2, Height - 2)
                    G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(200, 210, 220)), New Rectangle(0, -1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                End If
            Case MouseState.Down
                If _Color2 Then
                    G.Clear(Color.FromArgb(37, 47, 57))
                    Dim BTNLGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(35, 45, 55), Color.FromArgb(132, 141, 147), 180)
                    G.FillRectangle(BTNLGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim BTNLGB1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(132, 141, 147), Color.FromArgb(35, 45, 55), 180)
                    G.FillRectangle(BTNLGB1, New Rectangle(0, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(33, 43, 53))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(93, 103, 113), Color.FromArgb(45, 45, 45), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(53, 63, 73))), 1, Height - 2, Width - 2, Height - 2)
                    G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(1, -1, Width - 1, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                Else
                    G.Clear(Color.FromArgb(43, 53, 63))
                    Dim BTNLGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(27, 37, 47), Color.FromArgb(130, 139, 145), LinearGradientMode.Horizontal)
                    G.FillRectangle(BTNLGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim BTNLGB1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(130, 139, 145), Color.FromArgb(27, 37, 47), LinearGradientMode.Horizontal)
                    G.FillRectangle(BTNLGB1, New Rectangle(0, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(33, 43, 53))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(123, 133, 143), Color.FromArgb(55, 55, 55), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(73, 83, 93))), 2, Height - 2, Width - 2, Height - 2)
                    G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(1, -1, Width - 1, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                End If
        End Select

        e.Graphics.DrawImage(bmp, 0, 0)
        G.Dispose()
        bmp.Dispose()

    End Sub
End Class

Class SharpForm
    Inherits ContainerControl
    Private Header As Integer
    Private Cap As Boolean
    Private MouseP As Point = New Point(0, 0)
    Private path As GraphicsPath
    Protected G As Graphics, bmp As Bitmap
    Private Function RoundRect(ByVal r As RectangleF, ByVal r1 As Single, ByVal r2 As Single, ByVal r3 As Single, ByVal r4 As Single) As GraphicsPath
        Dim x As Single = r.X, y As Single = r.Y, w As Single = r.Width, h As Single = r.Height
        Dim rr5 As GraphicsPath = New GraphicsPath
        rr5.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y)
        rr5.AddLine(x + r1, y, x + w - r2, y)
        rr5.AddBezier(x + w - r2, y, x + w, y, x + w, y + r2, x + w, y + r2)
        rr5.AddLine(x + w, y + r2, x + w, y + h - r3)
        rr5.AddBezier(x + w, y + h - r3, x + w, y + h, x + w - r3, y + h, x + w - r3, y + h)
        rr5.AddLine(x + w - r3, y + h, x + r4, y + h)
        rr5.AddBezier(x + r4, y + h, x, y + h, x, y + h - r4, x, y + h - r4)
        rr5.AddLine(x, y + h - r4, x, y + r1)
        Return rr5
    End Function
#Region " Control Help - Movement & Flicker Control "
    Protected Overrides Sub OnInvalidated(ByVal e As System.Windows.Forms.InvalidateEventArgs)
        MyBase.OnInvalidated(e)
        ParentForm.FindForm.Text = Text
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
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, Header).Contains(e.Location) Then
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


    Dim WithEvents minimBtn As New SharpTopButton With {.TopButtonTXT = SharpTopButton.TxtState.Minim, .Location = New Point(Width - 60, 0)}
    Dim WithEvents closeBtn As New SharpTopButton With {.TopButtonTXT = SharpTopButton.TxtState.Close, .Location = New Point(Width - 40, 0)}


    Public Property Color2() As Boolean
        Get
            Return _Color2
        End Get
        Set(ByVal value As Boolean)
            _Color2 = value
            Me.Refresh()
        End Set
    End Property
    Private _Color2 As Boolean = True


    Sub New()
        MyBase.new()
        Header = 25
        Dock = DockStyle.Fill
        DoubleBuffered = True

        Controls.Add(closeBtn)

        closeBtn.Refresh() : minimBtn.Refresh()
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        bmp = New Bitmap(Width, Height)
        G = Graphics.FromImage(bmp)
        Dim TransparencyKey As Color = Me.ParentForm.TransparencyKey


        MyBase.OnPaint(e)

        If _minimBool Then
            Controls.Add(minimBtn)
        Else
            Controls.Remove(minimBtn)
        End If

        minimBtn.Location = New Point(Width - 60, 0)
        closeBtn.Location = New Point(Width - 40, 0)


        G.Clear(Color.FromArgb(43, 53, 63))
        '---- Sides--
        Dim LGBunderGrdrect As Rectangle = New Rectangle(1, Header, Width, 130)
        Dim LGBunderGrd As New LinearGradientBrush(LGBunderGrdrect, Color.FromArgb(43, 53, 63), Color.FromArgb(70, 79, 85), 90)
        G.FillRectangle(LGBunderGrd, LGBunderGrdrect)


        If _Color2 Then
            Dim BTNLGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Header / 2), Color.FromArgb(20, 30, 40), Color.FromArgb(135, Color.White), 360)
            G.FillRectangle(BTNLGBOver, New Rectangle(2, 1, Width - 3, Header / 2))
            Dim BTNLGB1 As New LinearGradientBrush(New Rectangle(-1, 1, Width, Header / 2), Color.FromArgb(100, Color.White), Color.FromArgb(20, 30, 40), 360)
            G.FillRectangle(BTNLGB1, New Rectangle(-1, 1, Width / 2, Header / 2))
            Dim txtbrushCL2 As Brush = New SolidBrush(Color.FromArgb(250, 250, 250))
            G.DrawString(Text, Font, txtbrushCL2, New Rectangle(16, 6, Width - 1, 22), New StringFormat With {.LineAlignment = StringAlignment.Near, .Alignment = StringAlignment.Near})
        Else
            Dim BTNLGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Header / 2), Color.FromArgb(35, 45, 55), Color.FromArgb(155, Color.White), 180)
            G.FillRectangle(BTNLGBOver, New Rectangle(2, 1, Width - 3, Header / 2))
            Dim BTNLGB1 As New LinearGradientBrush(New Rectangle(-1, 1, Width, Header / 2), Color.FromArgb(120, Color.White), Color.FromArgb(35, 45, 55), 180)
            G.FillRectangle(BTNLGB1, New Rectangle(-1, 1, Width / 2, Header / 2))
            Dim txtbrush As Brush = New SolidBrush(Color.FromArgb(210, 220, 230))
            G.DrawString(Text, Font, txtbrush, New Rectangle(16, 7, Width - 1, 22), New StringFormat With {.LineAlignment = StringAlignment.Near, .Alignment = StringAlignment.Near})
        End If





        Dim InerRecLGB As Rectangle = New Rectangle(11, 28, Width - 22, Height - 37)
        Dim InnerRecLGB As New LinearGradientBrush(InerRecLGB, Color.FromArgb(57, 67, 77), Color.FromArgb(60, 69, 75), 90)
        G.FillRectangle(InnerRecLGB, InerRecLGB)

        '----- InnerRect
        Dim P1 As Pen = New Pen(New SolidBrush(Color.FromArgb(23, 33, 43)))
        G.DrawRectangle(P1, 12, 29, Width - 25, Height - 40)
        Dim P2 As Pen = New Pen(New SolidBrush(Color.FromArgb(93, 103, 113)))
        G.DrawRectangle(P2, 11, 28, Width - 23, Height - 38)



        Dim LGBunderGrd3 As New LinearGradientBrush(New Rectangle(0, Height - 9, Width \ 2, 50), Color.FromArgb(40, 50, 60), Color.FromArgb(50, Color.White), 360)
        G.FillRectangle(LGBunderGrd3, 0, Height - 9, Width \ 2, 50)
        Dim LGBunderGrd2 As New LinearGradientBrush(New Rectangle(Width \ 2, Height - 9, Width \ 2, Height), Color.FromArgb(40, 50, 60), Color.FromArgb(50, Color.White), 180)
        G.FillRectangle(LGBunderGrd2, Width \ 2, Height - 9, Width \ 2, Height)
        G.DrawLine(New Pen(Color.FromArgb(90, 90, 90)), Width \ 2, Height - 9, Width \ 2, Height)

        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(137, 147, 157))), 1, 1, Width - 3, Height - 3)

        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        G.DrawPath(Pens.Black, RoundRect(ClientRectangle, 0, 0, 0, 0))

        G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(163, 173, 183))), 2, 1, Width - 3, 1)
        e.Graphics.DrawImage(bmp.Clone, 0, 0)
        bmp.Dispose()
        G.Dispose()

    End Sub

End Class

Class SharpTopButton
    Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum
    Private State As MouseState
    Dim X As Integer

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
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.X
        Invalidate()
    End Sub

#End Region
    Enum TxtState As Byte
        Close = 1
        Minim = 2
    End Enum
    Private BtnTxt As TxtState
    Public Property TopButtonTXT() As TxtState
        Get
            Return BtnTxt
        End Get
        Set(ByVal v As TxtState)
            BtnTxt = v
            Invalidate()
        End Set
    End Property
    Sub New()
        MyBase.New()
        BackColor = Color.FromArgb(38, 38, 38)
        Font = New Font("Verdana", 8.25F)
        Size = New Size(30, 20)

        DoubleBuffered = True
        Focus()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim bmp As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(bmp)
        MyBase.OnPaint(e)


        Select Case State
            Case MouseState.Over

                If BtnTxt = TxtState.Close Then
                    G.Clear(Color.FromArgb(170, 18, 32))
                    Dim LGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(170, 18, 32), Color.FromArgb(147, 156, 162), 360)
                    G.FillRectangle(LGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim LGBOver1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(147, 156, 162), Color.FromArgb(170, 18, 32), 360)
                    G.FillRectangle(LGBOver1, New Rectangle(0, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(39, 49, 59))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(159, 169, 179), Color.FromArgb(90, 90, 90), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(123, 133, 143))), 1, Height - 2, Width - 2, Height - 2)

                ElseIf BtnTxt = TxtState.Minim Then
                    G.Clear(Color.FromArgb(0, 102, 175))
                    Dim LGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(0, 102, 175), Color.FromArgb(157, 166, 172), 360)
                    G.FillRectangle(LGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim LGBOver1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(157, 166, 172), Color.FromArgb(0, 102, 175), 360)
                    G.FillRectangle(LGBOver1, New Rectangle(0, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(39, 49, 59))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(159, 169, 179), Color.FromArgb(90, 90, 90), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(123, 133, 143))), 1, Height - 2, Width - 2, Height - 2)

                End If
            Case MouseState.None

                If BtnTxt = TxtState.Close Then
                    G.Clear(Color.FromArgb(140, 18, 32))
                    Dim LGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(140, 18, 32), Color.FromArgb(117, 126, 132), 360)
                    G.FillRectangle(LGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim LGBOver1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(117, 126, 132), Color.FromArgb(140, 18, 32), 360)
                    G.FillRectangle(LGBOver1, New Rectangle(0, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(33, 43, 53))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(153, 163, 173), Color.FromArgb(85, 85, 85), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(103, 113, 123))), 1, Height - 2, Width - 2, Height - 2)

                ElseIf BtnTxt = TxtState.Minim Then
                    G.Clear(Color.FromArgb(0, 102, 156))
                    Dim LGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(0, 102, 156), Color.FromArgb(117, 126, 132), 360)
                    G.FillRectangle(LGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim LGBOver1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(117, 126, 132), Color.FromArgb(0, 102, 156), 360)
                    G.FillRectangle(LGBOver1, New Rectangle(0, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(33, 43, 53))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(153, 163, 173), Color.FromArgb(85, 85, 85), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(103, 113, 123))), 1, Height - 2, Width - 2, Height - 2)

                End If
            Case MouseState.Down
                If BtnTxt = TxtState.Close Then
                    G.Clear(Color.FromArgb(130, 18, 32))
                    Dim LGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(130, 18, 32), Color.FromArgb(117, 126, 132), 360)
                    G.FillRectangle(LGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim LGBOver1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(117, 126, 132), Color.FromArgb(130, 18, 32), 360)
                    G.FillRectangle(LGBOver1, New Rectangle(0, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality
                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(30, 40, 50))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 2), Color.FromArgb(151, 161, 171), Color.FromArgb(83, 83, 83), 90)
                    G.DrawRectangle(New Pen(InnerRect), New Rectangle(1, 1, Width - 3, Height - 3))
                    G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(97, 107, 117))), 1, Height - 2, Width - 2, Height - 2)

                ElseIf BtnTxt = TxtState.Minim Then
                    G.Clear(Color.FromArgb(0, 102, 146))
                    Dim LGBOver As New LinearGradientBrush(New Rectangle(2, 1, Width - 3, Height / 2 - 2), Color.FromArgb(0, 102, 154), Color.FromArgb(132, 141, 147), 360)
                    G.FillRectangle(LGBOver, New Rectangle(2, 1, Width - 3, Height / 2 - 2))
                    Dim LGBOver1 As New LinearGradientBrush(New Rectangle(0, 1, Width - 2, Height / 2 - 2), Color.FromArgb(132, 141, 147), Color.FromArgb(0, 102, 154), 360)
                    G.FillRectangle(LGBOver1, New Rectangle(0, 1, Width / 2, Height / 2 - 2))

                    G.SmoothingMode = SmoothingMode.HighQuality

                    '----- Borders -----
                    G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(37, 47, 57))), New Rectangle(0, 0, Width - 1, Height - 1))
                    Dim InnerRect1 As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 3), Color.FromArgb(150, 150, 150), Color.FromArgb(127, 137, 147), 90S)
                    G.DrawRectangle(New Pen(InnerRect1), New Rectangle(1, 1, Width - 3, Height - 3))
                End If

        End Select
        Select Case BtnTxt
            Case TxtState.Close
                Size = New Size(30, 20)
                G.DrawString("r", New Font("Marlett", 8.75), New SolidBrush(Color.FromArgb(220, Color.White)), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case TxtState.Minim
                Size = New Size(25, 20)
                G.DrawString("0", New Font("Marlett", 12), New SolidBrush(Color.FromArgb(220, Color.White)), New Rectangle(1, -1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select

        '  G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(40, 40, 40))), 0, 0, 29, 19)

        e.Graphics.DrawImage(bmp.Clone(), 0, 0)
        G.Dispose() : bmp.Dispose()
    End Sub

End Class

Class SharpGroupBOx
    Inherits ContainerControl
#Region " Control Help - Properties & Flicker Control"
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Size = New Size(200, 100)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        'BackColor = Color.Transparent
        DoubleBuffered = True
        ForeColor = Color.FromArgb(210, 220, 230)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim bmp As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(bmp)

        MyBase.OnPaint(e)
        G.SmoothingMode = SmoothingMode.HighQuality

        G.Clear(Color.FromArgb(43, 53, 63))

        Dim s2 As New LinearGradientBrush(New Rectangle(0, 2, Width - 3, 25), Color.FromArgb(35, 45, 55), Color.FromArgb(50, Color.White), 90S)
        G.FillRectangle(s2, New Rectangle(1, 14, Width - 3, 13))
        Dim s1 As New LinearGradientBrush(New Rectangle(0, 2, Width - 3, 25), Color.FromArgb(90, Color.White), Color.FromArgb(35, 45, 55), 90S)
        G.FillRectangle(s1, New Rectangle(1, 1, Width - 3, 13))


        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(30, 40, 50))), 0, 0, Width - 1, 28)
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(111, 121, 131))), 1, 1, Width - 3, 26)
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(30, 40, 50))), 0, 30, Width - 1, Height - 31)
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(60, 70, 80))), 1, 31, Width - 3, Height - 33)

        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(30, 30, 30))), New Rectangle(0, 0, Width - 1, Height - 1))
        G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(20, 30, 40))), 1, 29, Width - 2, 29)

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(1, 4, Width, 20), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

        e.Graphics.DrawImage(bmp.Clone(), 0, 0)
        G.Dispose() : bmp.Dispose()
    End Sub
End Class

Class SharpProgreSsBar
    Inherits Control
    Private GlowAnimation As Timer = New Timer
    'Private _GlowColor As Color = Color.FromArgb(55, 65, 75)
    Private _GlowColor As Color = Color.FromArgb(50, 255, 255, 255)
    Private _Animate As Boolean = True
    Private _Value As Int32 = 0
    Private _HighlightColor As Color = Color.Silver
    Private _BackgroundColor As Color = Color.FromArgb(150, 150, 150)
    Private _StartColor As Color = Color.FromArgb(110, 110, 110)
#Region "Properties"
    Public Property Color() As Color
        Get
            Return _StartColor
        End Get
        Set(ByVal value As Color)
            _StartColor = value
            Me.Invalidate()
        End Set
    End Property
    Public Property Animate() As Boolean
        Get
            Return _Animate
        End Get
        Set(ByVal value As Boolean)
            _Animate = value
            If value = True Then
                GlowAnimation.Start()
            Else
                GlowAnimation.Stop()
            End If
            Me.Invalidate()
        End Set
    End Property
    Public Property GlowColor() As Color
        Get
            Return _GlowColor
        End Get
        Set(ByVal value As Color)
            _GlowColor = value
            Me.Invalidate()
        End Set
    End Property
    Public Property Value() As Int32
        Get
            Return _Value
        End Get
        Set(ByVal value As Int32)
            If value < 0 Then Return
            _Value = value
            If value < 100 Then GlowAnimation.Start()

            Me.Invalidate()
        End Set
    End Property
    Public Property BackgroundColor() As Color
        Get
            Return _BackgroundColor
        End Get
        Set(ByVal value As Color)
            _BackgroundColor = value
            Me.Invalidate()
        End Set
    End Property
    Public Property HighlightColor() As Color
        Get
            Return _HighlightColor
        End Get
        Set(ByVal value As Color)
            _HighlightColor = value
            Me.Invalidate()
        End Set
    End Property

#End Region
    Private Function InDesignMode() As Boolean
        Return (LicenseManager.UsageMode = LicenseUsageMode.Designtime)
    End Function
    Sub New()
        MyBase.New()
        DoubleBuffered = True
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)

        If Not InDesignMode() Then
            GlowAnimation.Interval = 15
            If Value < 100 Then GlowAnimation.Start()

            AddHandler GlowAnimation.Tick, AddressOf GlowAnimation_Tick
        End If
    End Sub
    Private _mGlowPosition As Integer = -100

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality

        G.Clear(Color.FromArgb(43, 53, 63))
        '   -------------------Draw Background for the MBProgressBar--------------------

        Dim s2 As New LinearGradientBrush(New Rectangle(0, 2, Width - 3, 50), Color.FromArgb(35, 45, 55), Color.FromArgb(50, Color.White), 90S)

        Dim s1 As New LinearGradientBrush(New Rectangle(0, 2, Width - 3, 50), Color.FromArgb(75, Color.White), Color.FromArgb(35, 45, 55), 90)

        Dim BackRectangle As Rectangle = Me.ClientRectangle
        BackRectangle.Width = BackRectangle.Width - 1
        BackRectangle.Height = BackRectangle.Height - 1
        Dim GrafP As GraphicsPath = RoundRect(BackRectangle, 2, 2, 2, 2)
        G.FillPath(s1, GrafP)


        '--------------------Draw Background Shadows for MBProgrssBar------------------
        Dim BGSH As Rectangle = New Rectangle(2, 2, 10, Me.Height - 5)
        Dim LGBS As LinearGradientBrush = New LinearGradientBrush(BGSH, Color.FromArgb(70, 80, 90), Color.FromArgb(95, 105, 115), LinearGradientMode.Horizontal)
        G.FillRectangle(LGBS, BGSH)
        Dim BGSRectangle As Rectangle = New Rectangle(Me.Width - 12, 2, 10, Me.Height - 5)
        Dim LG As LinearGradientBrush = New LinearGradientBrush(BGSRectangle, Color.FromArgb(80, 90, 100), Color.FromArgb(75, 85, 95), LinearGradientMode.Horizontal)
        G.FillRectangle(LG, BGSRectangle)


        '----------------------Draw MBProgressBar--------------------	
        Dim ProgressRect As Rectangle = New Rectangle(1, 2, Me.Width - 3, Me.Height - 3)
        ProgressRect.Width = CInt((Value * 1.0F / (100) * Me.Width))
        G.FillRectangle(s2, ProgressRect)


        '----------------------Draw Shadows for MBProgressBar------------------
        Dim SHRect As Rectangle = New Rectangle(1, 2, 15, Me.Height - 3)
        Dim LGSHP As LinearGradientBrush = New LinearGradientBrush(SHRect, Color.Black, Color.Black, LinearGradientMode.Horizontal)

        Dim BColor As ColorBlend = New ColorBlend(3)
        BColor.Colors = New Color() {Color.Gray, Color.FromArgb(40, 0, 0, 0), Color.Transparent}
        BColor.Positions = New Single() {0.0F, 0.2F, 1.0F}
        LGSHP.InterpolationColors = BColor

        SHRect.X = SHRect.X - 1
        G.FillRectangle(LGSHP, SHRect)

        Dim Rect1 As Rectangle = New Rectangle(Me.Width - 3, 2, 15, Me.Height - 3)
        Rect1.X = CInt((Value * 1.0F / (100) * Me.Width) - 14)
        Dim LGSH1 As LinearGradientBrush = New LinearGradientBrush(Rect1, Color.Black, Color.Black, LinearGradientMode.Horizontal)

        Dim BColor1 As ColorBlend = New ColorBlend(3)
        BColor1.Colors = New Color() {Color.Transparent, Color.FromArgb(70, 0, 0, 0), Color.Transparent}
        BColor1.Positions = New Single() {0.0F, 0.8F, 1.0F}
        LGSH1.InterpolationColors = BColor1

        G.FillRectangle(LGSH1, Rect1)


        '-------------------------Draw Highlight for MBProgressBar-----------------
        Dim HLRect As Rectangle = New Rectangle(1, 1, Me.Width - 1, 6)
        Dim HLGPa As GraphicsPath = RoundRect(HLRect, 2, 2, 0, 0)
        'G.SetClip(HLGPa)
        Dim HLGBS As LinearGradientBrush = New LinearGradientBrush(HLRect, Color.FromArgb(190, 190, 190), Color.FromArgb(150, 150, 150), LinearGradientMode.Vertical)
        G.FillPath(HLGBS, HLGPa)
        G.ResetClip()
        Dim HLrect2 As Rectangle = New Rectangle(1, Me.Height - 8, Me.Width - 1, 6)
        Dim bp1 As GraphicsPath = RoundRect(HLrect2, 0, 0, 2, 2)
        ' G.SetClip(bp1)
        Dim bg1 As LinearGradientBrush = New LinearGradientBrush(HLrect2, Color.Transparent, Color.FromArgb(150, Me.HighlightColor), LinearGradientMode.Vertical)
        G.FillPath(bg1, bp1)
        G.ResetClip()


        '--------------------Draw Inner Sroke for MBProgressBar--------------
        Dim Rect20 As Rectangle = Me.ClientRectangle
        Rect20.X = Rect20.X + 1
        Rect20.Y = Rect20.Y + 1
        Rect20.Width -= 3
        Rect20.Height -= 3
        Dim Rect15 As GraphicsPath = RoundRect(Rect20, 2, 2, 2, 2)
        G.DrawPath(New Pen(Color.FromArgb(55, 65, 75)), Rect15)

        '-----------------------Draw Outer Stroke on the Control----------------------------
        Dim StrokeRect As Rectangle = Me.ClientRectangle
        StrokeRect.Width = StrokeRect.Width - 1
        StrokeRect.Height = StrokeRect.Height - 1
        Dim GGH As GraphicsPath = RoundRect(StrokeRect, 2, 2, 2, 2)
        G.DrawPath(New Pen(Color.FromArgb(122, 122, 122)), GGH)

        '------------------------Draw Glow for MBProgressBar-----------------------
        Dim GlowRect As Rectangle = New Rectangle(_mGlowPosition, 6, 60, 60)
        Dim GlowLGBS As LinearGradientBrush = New LinearGradientBrush(GlowRect, Color.FromArgb(127, 137, 147), Color.FromArgb(75, 85, 95), LinearGradientMode.Horizontal)
        Dim BColor3 As ColorBlend = New ColorBlend(4)
        BColor3.Colors = New Color() {Color.Transparent, Me.GlowColor, Me.GlowColor, Color.Transparent}
        BColor3.Positions = New Single() {0.0F, 0.5F, 0.6F, 1.0F}
        GlowLGBS.InterpolationColors = BColor3
        Dim clip As Rectangle = New Rectangle(1, 2, Me.Width - 3, Me.Height - 3)
        clip.Width = CInt((Value * 1.0F / (100) * Me.Width))
        G.SetClip(clip)
        G.FillRectangle(GlowLGBS, GlowRect)
        G.ResetClip()

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Private Function RoundRect(ByVal r As RectangleF, ByVal r1 As Single, ByVal r2 As Single, ByVal r3 As Single, ByVal r4 As Single) As GraphicsPath
        Dim x As Single = r.X, y As Single = r.Y, w As Single = r.Width, h As Single = r.Height
        Dim rr5 As GraphicsPath = New GraphicsPath
        rr5.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y)
        rr5.AddLine(x + r1, y, x + w - r2, y)
        rr5.AddBezier(x + w - r2, y, x + w, y, x + w, y + r2, x + w, y + r2)
        rr5.AddLine(x + w, y + r2, x + w, y + h - r3)
        rr5.AddBezier(x + w, y + h - r3, x + w, y + h, x + w - r3, y + h, x + w - r3, y + h)
        rr5.AddLine(x + w - r3, y + h, x + r4, y + h)
        rr5.AddBezier(x + r4, y + h, x, y + h, x, y + h - r4, x, y + h - r4)
        rr5.AddLine(x, y + h - r4, x, y + r1)
        Return rr5
    End Function
    Private Sub GlowAnimation_Tick(ByVal sender As Object, ByVal e As EventArgs)
        If Me.Animate Then
            _mGlowPosition += 4
            If _mGlowPosition > Me.Width Then
                _mGlowPosition = -10
                Me.Invalidate()
            End If

        Else

            GlowAnimation.Stop()

            _mGlowPosition = -50
        End If
    End Sub

End Class

Class SharpTextBox : Inherits Control
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
            .Location = New Point(5, 5)
            .Font = New Font("Verdana", 8)
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
        BackColor = Color.FromArgb(35, 45, 55)
        ForeColor = Color.FromArgb(162, 172, 182)
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        MyBase.OnPaint(e)

        Height = txtbox.Height + 11
        With txtbox
            .Width = Width - 10
            .TextAlign = TextAlignment
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

        G.Clear(Color.FromArgb(35, 45, 55))
        Dim txtRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim InnerRect As New LinearGradientBrush(txtRect, Color.FromArgb(74, 84, 94), Color.FromArgb(94, 104, 114), 90S)

        G.DrawRectangle(New Pen(Color.FromArgb(12, 22, 32), 2), txtRect)
        G.DrawLine(New Pen(InnerRect), Width - 1, 0, Width - 1, Height)
        G.DrawLine(New Pen(InnerRect), 0, Height - 1, Width - 1, Height - 1)


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class

Class SharpCheckBox : Inherits Control

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

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 18
    End Sub
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Size = New Size(145, 16)
        ForeColor = Color.FromArgb(210, 210, 222)
        DoubleBuffered = True
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        MyBase.OnPaint(e)
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(BackColor)
        If Checked = False Then
            Dim BTNLGBOver As New LinearGradientBrush(New Rectangle(3, 3, 12, 11), Color.FromArgb(37, 47, 57), Color.FromArgb(62, 68, 74), 90)
            G.FillRectangle(BTNLGBOver, New Rectangle(3, 3, 12, 11))


        Else

            Dim BTNLGBOver As New LinearGradientBrush(New Rectangle(3, 3, 12, 11), Color.FromArgb(73, 83, 93), Color.FromArgb(40, 50, 60), 90)
            G.FillRectangle(BTNLGBOver, New Rectangle(3, 3, 12, 11))

            Dim chkRec12 As New Rectangle(3, 2, Height - 5, Height - 6)
            Dim chkPoly As Rectangle = New Rectangle(chkRec12.X + chkRec12.Width / 4, chkRec12.Y + chkRec12.Height / 4, chkRec12.Width \ 2, chkRec12.Height \ 2)
            Dim Poly() As Point = {New Point(chkPoly.X, chkPoly.Y + chkPoly.Height \ 2), _
                           New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height), _
                           New Point(chkPoly.X + chkPoly.Width, chkPoly.Y)}

            Dim P1 As Pen = New Pen((Color.White), 2)

            For i = 0 To Poly.Length - 2
                G.DrawLine(P1, Poly(i), Poly(i + 1))
            Next


        End If
        G.DrawRectangle(New Pen(Color.FromArgb(93, 103, 113)), 1, 1, 16, 15)
        G.DrawRectangle(New Pen(Color.FromArgb(13, 23, 33)), 2, 2, 14, 13)
        G.DrawRectangle(New Pen(Color.FromArgb(113, 123, 133)), 3, 3, 12, 11)
        Dim txtbrush As Brush = New SolidBrush(Color.FromArgb(210, 220, 230))
        G.DrawString(Text, Font, txtbrush, New Point(18, 2), New StringFormat With {.LineAlignment = StringAlignment.Near, .Alignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Class SharpRadioButton : Inherits Control
#Region " Control Help - MouseState & Flicker Control"

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum
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
            If C IsNot Me AndAlso TypeOf C Is SharpRadioButton Then
                DirectCast(C, SharpRadioButton).Checked = False
            End If
        Next
    End Sub
#End Region
    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Size = New Size(150, 16)
        ForeColor = Color.FromArgb(210, 210, 222)
        DoubleBuffered = True
        Font = New Font("Verdana", 8)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.SmoothingMode = SmoothingMode.HighQuality

        MyBase.OnPaint(e)

        G.Clear(BackColor)


        If Checked = False Then
            Dim BTNLGBOver As New LinearGradientBrush(New Rectangle(3, 3, 12, 11), Color.FromArgb(37, 47, 57), Color.FromArgb(62, 68, 74), 90)
            G.FillEllipse(BTNLGBOver, New Rectangle(3, 3, 12, 11))
            G.DrawEllipse(New Pen(Color.FromArgb(0, 0, 0)), 2, 2, 14, 13)
        Else

            Dim CKelGrd As New LinearGradientBrush(New Rectangle(3, 3, 12, 11), Color.FromArgb(65, 71, 77), Color.FromArgb(0, 0, 0), 90)
            G.FillEllipse(CKelGrd, New Rectangle(3, 3, 12, 11))
            G.DrawEllipse(New Pen(Color.FromArgb(13, 23, 33)), 2, 2, 14, 13)
        End If



        G.DrawEllipse(New Pen(Color.FromArgb(93, 103, 113)), 1, 1, 15, 14)

        G.DrawEllipse(New Pen(Color.FromArgb(113, 123, 133)), 3, 3, 11, 10)
        Dim txtbrush As Brush = New SolidBrush(Color.FromArgb(210, 220, 230))
        G.DrawString(Text, Font, txtbrush, New Point(18, 2), New StringFormat With {.LineAlignment = StringAlignment.Near, .Alignment = StringAlignment.Near})


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class