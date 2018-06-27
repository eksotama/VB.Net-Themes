Imports System.ComponentModel
Imports System.Drawing.Drawing2D

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

'--------------------- [ Theme ] --------------------
'Creator: Mephobia
'Contact: Mephobia.HF (Skype)
'Created: 10.10.2012
'Changed: 10.10.2012
'-------------------- [ /Theme ] ---------------------

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

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
    'Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
    '    Dim Rectangle As Rectangle = New Rectangle(X, Y, Width, Height)
    '    Dim P As GraphicsPath = New GraphicsPath()
    '    Dim ArcRectangleWidth As Integer = Curve * 2
    '    P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
    '    P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
    '    P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
    '    P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
    '    P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
    '    Return P
    'End Function
End Module

Public Class UbuntuControlBox : Inherits Control

#Region " MouseStates "
    Dim State As MouseState = MouseState.None
    Dim X As Integer
    Dim CloseBtn As New Rectangle(43, 2, 17, 17)
    Dim MinBtn As New Rectangle(3, 2, 17, 17)
    Dim MaxBtn As New Rectangle(23, 2, 17, 17)
    Dim bgr As New Rectangle(0, 0, 62.5, 21)

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If X > 3 AndAlso X < 20 Then
            FindForm.WindowState = FormWindowState.Minimized
        ElseIf X > 23 AndAlso X < 40 Then
            If FindForm.WindowState = FormWindowState.Maximized Then
                FindForm.WindowState = FormWindowState.Minimized
                FindForm.WindowState = FormWindowState.Normal
            Else
                FindForm.WindowState = FormWindowState.Minimized
                FindForm.WindowState = FormWindowState.Maximized
            End If

        ElseIf X > 43 AndAlso X < 60 Then
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
        Font = New Font("Marlett", 7)
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        MyBase.OnPaint(e)

        G.SmoothingMode = SmoothingMode.HighQuality

        G.Clear(BackColor)

        Dim bg0 As New LinearGradientBrush(bgr, Color.FromArgb(60, 59, 55), Color.FromArgb(60, 59, 55), 90S)
        G.FillPath(bg0, Draw.RoundRect(bgr, 10))

        Dim lgb10 As New LinearGradientBrush(MinBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
        G.FillEllipse(lgb10, MinBtn)
        G.DrawEllipse(Pens.DimGray, MinBtn)
        G.DrawString("0", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(5.5, 6, 0, 0))

        Dim lgb20 As New LinearGradientBrush(MaxBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
        G.FillEllipse(lgb20, MaxBtn)
        G.DrawEllipse(Pens.DimGray, MaxBtn)
        G.DrawString("1", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(26, 7, 0, 0))

        Dim lgb30 As New LinearGradientBrush(CloseBtn, Color.FromArgb(247, 150, 116), Color.FromArgb(223, 81, 6), 90S)
        G.FillEllipse(lgb30, CloseBtn)
        G.DrawEllipse(Pens.DimGray, CloseBtn)
        G.DrawString("r", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(46, 7, 0, 0))

        Select Case State
            Case MouseState.None
                Dim bg As New LinearGradientBrush(bgr, Color.FromArgb(60, 59, 55), Color.FromArgb(60, 59, 55), 90S)
                G.FillPath(bg, Draw.RoundRect(bgr, 10))

                Dim lgb1 As New LinearGradientBrush(MinBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
                G.FillEllipse(lgb1, MinBtn)
                G.DrawEllipse(Pens.DimGray, MinBtn)
                G.DrawString("0", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(5.5, 6, 0, 0))

                Dim lgb2 As New LinearGradientBrush(MaxBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
                G.FillEllipse(lgb2, MaxBtn)
                G.DrawEllipse(Pens.DimGray, MaxBtn)
                G.DrawString("1", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(26, 7, 0, 0))

                Dim lgb3 As New LinearGradientBrush(CloseBtn, Color.FromArgb(247, 150, 116), Color.FromArgb(223, 81, 6), 90S)
                G.FillEllipse(lgb3, CloseBtn)
                G.DrawEllipse(Pens.DimGray, CloseBtn)
                G.DrawString("r", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(46, 7, 0, 0))
            Case MouseState.Over
                If X > 3 AndAlso X < 20 Then
                    Dim bg As New LinearGradientBrush(bgr, Color.FromArgb(60, 59, 55), Color.FromArgb(60, 59, 55), 90S)
                    G.FillPath(bg, Draw.RoundRect(bgr, 10))

                    Dim lgb1 As New LinearGradientBrush(MinBtn, Color.FromArgb(172, 171, 166), Color.FromArgb(76, 75, 71), 90S)
                    G.FillEllipse(lgb1, MinBtn)
                    G.DrawEllipse(Pens.DimGray, MinBtn)
                    G.DrawString("0", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(5.5, 6, 0, 0))

                    Dim lgb2 As New LinearGradientBrush(MaxBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
                    G.FillEllipse(lgb2, MaxBtn)
                    G.DrawEllipse(Pens.DimGray, MaxBtn)
                    G.DrawString("1", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(26, 7, 0, 0))

                    Dim lgb3 As New LinearGradientBrush(CloseBtn, Color.FromArgb(247, 150, 116), Color.FromArgb(223, 81, 6), 90S)
                    G.FillEllipse(lgb3, CloseBtn)
                    G.DrawEllipse(Pens.DimGray, CloseBtn)
                    G.DrawString("r", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(46, 7, 0, 0))
                ElseIf X > 23 AndAlso X < 40 Then
                    Dim bg As New LinearGradientBrush(bgr, Color.FromArgb(60, 59, 55), Color.FromArgb(60, 59, 55), 90S)
                    G.FillPath(bg, Draw.RoundRect(bgr, 10))

                    Dim lgb1 As New LinearGradientBrush(MinBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
                    G.FillEllipse(lgb1, MinBtn)
                    G.DrawEllipse(Pens.DimGray, MinBtn)
                    G.DrawString("0", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(5.5, 6, 0, 0))

                    Dim lgb2 As New LinearGradientBrush(MaxBtn, Color.FromArgb(172, 171, 166), Color.FromArgb(76, 75, 71), 90S)
                    G.FillEllipse(lgb2, MaxBtn)
                    G.DrawEllipse(Pens.DimGray, MaxBtn)
                    G.DrawString("1", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(26, 7, 0, 0))

                    Dim lgb3 As New LinearGradientBrush(CloseBtn, Color.FromArgb(247, 150, 116), Color.FromArgb(223, 81, 6), 90S)
                    G.FillEllipse(lgb3, CloseBtn)
                    G.DrawEllipse(Pens.DimGray, CloseBtn)
                    G.DrawString("r", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(46, 7, 0, 0))
                ElseIf X > 43 AndAlso X < 60 Then
                    Dim bg As New LinearGradientBrush(bgr, Color.FromArgb(60, 59, 55), Color.FromArgb(60, 59, 55), 90S)
                    G.FillPath(bg, Draw.RoundRect(bgr, 10))

                    Dim lgb1 As New LinearGradientBrush(MinBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
                    G.FillEllipse(lgb1, MinBtn)
                    G.DrawEllipse(Pens.DimGray, MinBtn)
                    G.DrawString("0", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(5.5, 6, 0, 0))

                    Dim lgb2 As New LinearGradientBrush(MaxBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
                    G.FillEllipse(lgb2, MaxBtn)
                    G.DrawEllipse(Pens.DimGray, MaxBtn)
                    G.DrawString("1", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(26, 7, 0, 0))

                    Dim lgb3 As New LinearGradientBrush(CloseBtn, Color.FromArgb(255, 170, 136), Color.FromArgb(243, 101, 26), 90S)
                    G.FillEllipse(lgb3, CloseBtn)
                    G.DrawEllipse(Pens.DimGray, CloseBtn)
                    G.DrawString("r", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(46, 7, 0, 0))
                End If
            Case Else
                Dim bg As New LinearGradientBrush(bgr, Color.FromArgb(60, 59, 55), Color.FromArgb(60, 59, 55), 90S)
                G.FillPath(bg, Draw.RoundRect(bgr, 10))

                Dim lgb1 As New LinearGradientBrush(MinBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
                G.FillEllipse(lgb1, MinBtn)
                G.DrawEllipse(Pens.DimGray, MinBtn)
                G.DrawString("0", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(5.5, 6, 0, 0))

                Dim lgb2 As New LinearGradientBrush(MaxBtn, Color.FromArgb(152, 151, 146), Color.FromArgb(56, 55, 51), 90S)
                G.FillEllipse(lgb2, MaxBtn)
                G.DrawEllipse(Pens.DimGray, MaxBtn)
                G.DrawString("1", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(26, 7, 0, 0))

                Dim lgb3 As New LinearGradientBrush(CloseBtn, Color.FromArgb(247, 150, 116), Color.FromArgb(223, 81, 6), 90S)
                G.FillEllipse(lgb3, CloseBtn)
                G.DrawEllipse(Pens.DimGray, CloseBtn)
                G.DrawString("r", Font, New SolidBrush(Color.FromArgb(58, 57, 53)), New Rectangle(46, 7, 0, 0))
        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class UbuntuTheme : Inherits ContainerControl
    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.FromArgb(25, 25, 25)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim TopBar As New Rectangle(0, 0, Width - 1, 30)
        Dim FixBottom As New Rectangle(0, 26, Width - 1, 0)
        Dim Body As New Rectangle(0, 5, Width - 1, Height - 6)

        MyBase.OnPaint(e)

        G.Clear(Color.Fuchsia)

        G.SmoothingMode = SmoothingMode.HighSpeed

        Dim lbb As New LinearGradientBrush(Body, Color.FromArgb(242, 241, 240), Color.FromArgb(240, 240, 238), 90S)
        G.FillPath(lbb, Draw.RoundRect(Body, 1))
        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(60, 60, 60))), Draw.RoundRect(Body, 1))

        Dim lgb As New LinearGradientBrush(TopBar, Color.FromArgb(87, 86, 81), Color.FromArgb(60, 59, 55), 90S)
        'Dim tophatch As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(20, 20, 20), Color.Transparent)
        G.FillPath(lgb, Draw.RoundRect(TopBar, 4))
        'G.FillPath(tophatch, Draw.RoundRect(TopBar, 4))
        G.DrawPath(Pens.Black, Draw.RoundRect(TopBar, 3))
        G.DrawPath(Pens.Black, Draw.RoundRect(FixBottom, 1))
        G.FillRectangle(Brushes.White, 1, 27, Width - 2, Height - 29)

        Dim drawFont As New Font("Tahoma", 10, FontStyle.Regular)
        G.DrawString(Text, drawFont, New SolidBrush(Color.WhiteSmoke), New Rectangle(25, 0, Width - 1, 27), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        G.DrawIcon(FindForm.Icon, New Rectangle(5, 5, 16, 16))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Private MouseP As Point = New Point(0, 0)
    Private Cap As Boolean = False
    Private MoveHeight% = 26 : Private pos% = 0
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
    Private Sub UbuntuTheme1_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
       
    End Sub
End Class

Public Class UbuntuButtonOrange : Inherits Control
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
        ForeColor = Color.FromArgb(86, 109, 109)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        MyBase.OnPaint(e)

        G.SmoothingMode = SmoothingMode.HighQuality

        G.Clear(BackColor)
        Dim drawFont As New Font("Tahoma", 11, FontStyle.Regular)
        Dim p As New Pen(Color.FromArgb(157, 118, 103), 1)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(86, 109, 109))
        Select Case State
            Case MouseState.None

                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(249, 163, 128), Color.FromArgb(237, 139, 99), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 3))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 3))

                G.DrawString(Text, drawFont, nb, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 186, 153), Color.FromArgb(255, 171, 135), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 3))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 3))

                G.DrawString(Text, drawFont, nb, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Down
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(200, 116, 83), Color.FromArgb(194, 101, 65), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 3))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 3))

                G.DrawString(Text, drawFont, nb, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class UbuntuButtonGray : Inherits Control
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
        ForeColor = Color.FromArgb(90, 84, 82)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        MyBase.OnPaint(e)

        G.SmoothingMode = SmoothingMode.HighQuality

        G.Clear(BackColor)
        Dim drawFont As New Font("Tahoma", 11, FontStyle.Regular)
        Dim p As New Pen(Color.FromArgb(166, 166, 166), 1)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(80, 84, 82))
        Select Case State
            Case MouseState.None

                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(223, 223, 223), Color.FromArgb(197, 197, 197), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 3))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 3))

                G.DrawString(Text, drawFont, nb, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(243, 243, 243), Color.FromArgb(217, 217, 217), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 3))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 3))

                G.DrawString(Text, drawFont, nb, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Down
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(212, 211, 216), Color.FromArgb(156, 155, 151), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 3))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 3))

                G.DrawString(Text, drawFont, nb, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Public Class UbuntuCheckBox : Inherits Control

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
        BackColor = Color.WhiteSmoke
        ForeColor = Color.Black
        Size = New Size(145, 16)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim checkBoxRectangle As New Rectangle(0, 0, Height - 1, Height - 1)

        G.Clear(BackColor)

        Dim bodyGrad As New LinearGradientBrush(checkBoxRectangle, Color.FromArgb(102, 101, 96), Color.FromArgb(76, 75, 71), 90S)
        G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
        G.DrawRectangle(New Pen(Color.Gray), New Rectangle(1, 1, Height - 3, Height - 3))
        G.DrawRectangle(New Pen(Color.FromArgb(42, 47, 49)), checkBoxRectangle)

        Dim drawFont As New Font("Tahoma", 10, FontStyle.Regular)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(86, 83, 87))
        G.DrawString(Text, drawFont, nb, New Point(16, 7), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})


        If Checked Then
            Dim chkPoly As Rectangle = New Rectangle(checkBoxRectangle.X + checkBoxRectangle.Width / 4, checkBoxRectangle.Y + checkBoxRectangle.Height / 4, checkBoxRectangle.Width \ 2, checkBoxRectangle.Height \ 2)
            Dim Poly() As Point = {New Point(chkPoly.X, chkPoly.Y + chkPoly.Height \ 2), _
                           New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height), _
                           New Point(chkPoly.X + chkPoly.Width, chkPoly.Y)}
            G.SmoothingMode = SmoothingMode.HighQuality
            Dim P1 As New Pen(Color.FromArgb(247, 150, 116), 2)
            Dim chkGrad As New LinearGradientBrush(chkPoly, Color.FromArgb(200, 200, 200), Color.FromArgb(255, 255, 255), 0S)
            For i = 0 To Poly.Length - 2
                G.DrawLine(P1, Poly(i), Poly(i + 1))
            Next
        End If

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Public Class UbuntuRadioButton : Inherits Control

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
            If C IsNot Me AndAlso TypeOf C Is UbuntuRadioButton Then
                DirectCast(C, UbuntuRadioButton).Checked = False
            End If
        Next
    End Sub
#End Region

    Sub New()
        MyBase.New()
        BackColor = Color.WhiteSmoke
        ForeColor = Color.Black
        Size = New Size(150, 16)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim radioBtnRectangle = New Rectangle(0, 0, Height - 1, Height - 1)

        G.SmoothingMode = SmoothingMode.HighQuality

        G.Clear(BackColor)

        Dim bgGrad As New LinearGradientBrush(radioBtnRectangle, Color.FromArgb(102, 101, 96), Color.FromArgb(76, 75, 71), 90S)
        G.FillEllipse(bgGrad, radioBtnRectangle)

        G.DrawEllipse(New Pen(Color.Gray), New Rectangle(1, 1, Height - 3, Height - 3))
        G.DrawEllipse(New Pen(Color.FromArgb(42, 47, 49)), radioBtnRectangle)

        If Checked Then
            Dim chkGrad As New LinearGradientBrush(New Rectangle(4, 4, Height - 9, Height - 9), Color.FromArgb(247, 150, 116), Color.FromArgb(197, 100, 66), 90S)
            G.FillEllipse(chkGrad, New Rectangle(4, 4, Height - 9, Height - 9))
        End If

        Dim drawFont As New Font("Tahoma", 10, FontStyle.Regular)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(86, 83, 87))
        G.DrawString(Text, drawFont, nb, New Point(16, 1), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class

Public Class UbuntuGroupBox : Inherits ContainerControl

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim TopBar As New Rectangle(0, 0, Width - 1, 20)
        Dim box As New Rectangle(0, 0, Width - 1, Height - 10)

        MyBase.OnPaint(e)

        G.Clear(Color.Transparent)

        G.SmoothingMode = SmoothingMode.HighQuality

        Dim bodygrade As New LinearGradientBrush(ClientRectangle, Color.White, Color.White, 120S)
        G.FillPath(bodygrade, Draw.RoundRect(New Rectangle(0, 12, Width - 1, Height - 15), 1))

        Dim outerBorder As New LinearGradientBrush(ClientRectangle, Color.FromArgb(50, 50, 50), Color.DimGray, 90S)
        G.DrawPath(New Pen(outerBorder), Draw.RoundRect(New Rectangle(0, 12, Width - 1, Height - 15), 1))

        Dim lbb As New LinearGradientBrush(TopBar, Color.FromArgb(87, 86, 81), Color.FromArgb(60, 59, 55), 90S)
        G.FillPath(lbb, Draw.RoundRect(TopBar, 1))

        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(50, 50, 50))), Draw.RoundRect(TopBar, 2))

        Dim drawFont As New Font("Tahoma", 9, FontStyle.Regular)

        G.DrawString(Text, drawFont, New SolidBrush(Color.White), TopBar, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class UbuntuTextBox : Inherits Control
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
        BackColor = Color.White
        ForeColor = Color.FromArgb(102, 102, 102)
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)

        Height = txtbox.Height + 11
        Dim drawFont As New Font("Tahoma", 9, FontStyle.Regular)
        With txtbox
            .Width = Width - 12
            .ForeColor = Color.FromArgb(102, 102, 102)
            .Font = drawFont
            .TextAlign = TextAlignment
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality

        G.FillRectangle(New SolidBrush(Color.White), ClientRectangle)
        G.DrawPath(New Pen(Color.FromArgb(255, 207, 188)), Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 1))
        G.DrawPath(New Pen(Color.FromArgb(255, 207, 188)), Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 2))
        G.DrawPath(New Pen(Color.FromArgb(205, 87, 40)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
        G.DrawPath(New Pen(Color.FromArgb(205, 87, 40)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 3))


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class