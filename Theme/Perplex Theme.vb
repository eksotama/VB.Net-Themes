Imports System.Drawing.Drawing2D
Imports System.ComponentModel

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

'--------------------- [ Theme ] --------------------
'Creator: Mephobia
'Contact: Mephobia.HF (Skype)
'Created: 10.27.2012
'Changed: 10.27.2012
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
Public Class PerplexButton : Inherits Control
#Region " MouseStates "
    Dim State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseDown(e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
#End Region

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)


        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Tahoma", 8, FontStyle.Bold)

        'G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case State
            Case MouseState.None
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 2))
                Dim gloss As New LinearGradientBrush(New Rectangle(0, 0, Width - 125, 35 / 2), Color.FromArgb(75, Color.FromArgb(26, 26, 26)), Color.FromArgb(3, 255, 255, 255), 90S)
                G.FillPath(gloss, Draw.RoundRect(ClientRectangle, 2))

                Dim botbar As New LinearGradientBrush(New Rectangle(5, Height - 10, Width - 11, 5), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), 90S)
                G.FillPath(botbar, Draw.RoundRect(New Rectangle(5, Height - 10, Width - 11, 5), 1))

                Dim fill As New LinearGradientBrush(New Rectangle(6, Height - 9, Width - 13, 3), Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90S)
                G.FillPath(fill, Draw.RoundRect(New Rectangle(6, Height - 9, Width - 13, 3), 1))
                Dim o As New Pen(Color.FromArgb(50, 50, 50), 1)
                G.DrawPath(o, Draw.RoundRect(ClientRectangle, 2))
                G.DrawPath(Pens.Black, Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 2))
            Case MouseState.Over
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 2))
                Dim gloss As New LinearGradientBrush(New Rectangle(0, 0, Width - 125, 35 / 2), Color.FromArgb(15, Color.FromArgb(26, 26, 26)), Color.FromArgb(1, 255, 255, 255), 90S)
                G.FillPath(gloss, Draw.RoundRect(ClientRectangle, 2))

                Dim botbar As New LinearGradientBrush(New Rectangle(5, Height - 10, Width - 11, 5), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), 90S)
                G.FillPath(botbar, Draw.RoundRect(New Rectangle(5, Height - 10, Width - 11, 5), 1))

                Dim fill As New LinearGradientBrush(New Rectangle(6, Height - 9, Width - 13, 3), Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90S)
                G.FillPath(fill, Draw.RoundRect(New Rectangle(6, Height - 9, Width - 13, 3), 1))
                Dim o As New Pen(Color.FromArgb(50, 50, 50), 1)
                G.DrawPath(o, Draw.RoundRect(ClientRectangle, 2))
                G.DrawPath(Pens.Black, Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 2))
            Case MouseState.Down
                Dim lgb As New LinearGradientBrush(ClientRectangle, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                G.FillPath(lgb, Draw.RoundRect(ClientRectangle, 2))
                Dim gloss As New LinearGradientBrush(New Rectangle(0, 0, Width - 125, 35 / 2), Color.FromArgb(100, Color.FromArgb(26, 26, 26)), Color.FromArgb(1, 255, 255, 255), 90S)
                G.FillPath(gloss, Draw.RoundRect(ClientRectangle, 2))

                Dim botbar As New LinearGradientBrush(New Rectangle(5, Height - 10, Width - 11, 5), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), 90S)
                G.FillPath(botbar, Draw.RoundRect(New Rectangle(5, Height - 10, Width - 11, 5), 1))

                Dim fill As New LinearGradientBrush(New Rectangle(6, Height - 9, Width - 13, 3), Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90S)
                G.FillPath(fill, Draw.RoundRect(New Rectangle(6, Height - 9, Width - 13, 3), 1))
                Dim o As New Pen(Color.FromArgb(50, 50, 50), 1)
                G.DrawPath(o, Draw.RoundRect(ClientRectangle, 2))
                G.DrawPath(Pens.Black, Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 2))
        End Select

        G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(5, 5, 5)), New Rectangle(1, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(205, 205, 205)), New Rectangle(0, -1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class PerplexTheme : Inherits ContainerControl
    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.FromArgb(25, 25, 25)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim TopLeft As New Rectangle(0, 0, Width - 125, 28)
        Dim TopRight As New Rectangle(Width - 82, 0, 81, 28)
        Dim Body As New Rectangle(10, 10, Width - 21, Height - 16)
        Dim Body2 As New Rectangle(5, 5, Width - 11, Height - 6)

        MyBase.OnPaint(e)

        G.Clear(Color.Fuchsia)

        Dim BodyBrush As New LinearGradientBrush(Body2, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 35, 48), 90S)
        G.FillPath(BodyBrush, Draw.RoundRect(Body2, 3))
        G.DrawPath(New Pen(Brushes.Black), Draw.RoundRect(Body2, 3))

        Dim BodyBrush2 As New LinearGradientBrush(Body, Color.FromArgb(46, 46, 46), Color.FromArgb(50, 55, 58), 120S)
        G.FillPath(BodyBrush2, Draw.RoundRect(Body, 3))
        G.DrawPath(New Pen(Brushes.Black), Draw.RoundRect(Body, 3))

        Dim gloss As New LinearGradientBrush(New Rectangle(0, 0, Width - 125, 28 / 2), Color.FromArgb(240, Color.FromArgb(26, 26, 26)), Color.FromArgb(5, 255, 255, 255), 90S)
        Dim mainbrush As New LinearGradientBrush(TopLeft, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90S)
        G.FillPath(mainbrush, Draw.RoundRect(TopLeft, 3))
        G.FillPath(gloss, Draw.RoundRect(TopLeft, 3))
        G.DrawPath(New Pen(Brushes.Black), Draw.RoundRect(TopLeft, 3))

        Dim gloss2 As New LinearGradientBrush(New Rectangle(Width - 82, 0, Width - 205, 28 / 2), Color.FromArgb(240, Color.FromArgb(26, 26, 26)), Color.FromArgb(5, 255, 255, 255), 90S)
        Dim mainbrush2 As New LinearGradientBrush(TopRight, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90S)
        G.FillPath(mainbrush, Draw.RoundRect(TopRight, 3))
        G.FillPath(gloss2, Draw.RoundRect(TopRight, 3))
        G.DrawPath(New Pen(Brushes.Black), Draw.RoundRect(TopRight, 3))

        Dim p1 As New Pen(Color.FromArgb(174, 195, 30), 2)
        G.DrawLine(p1, 14, 9, 14, 22)
        Dim p2 As New Pen(Color.FromArgb(174, 195, 30), 2)
        G.DrawLine(p2, 17, 6, 17, 25)
        Dim p3 As New Pen(Color.FromArgb(174, 195, 30), 2)
        G.DrawLine(p3, 20, 9, 20, 22)

        Dim p4 As New Pen(Color.FromArgb(174, 195, 30), 2)
        G.DrawLine(p4, 11, 12, 11, 19)
        Dim p5 As New Pen(Color.FromArgb(174, 195, 30), 2)
        G.DrawLine(p4, 23, 12, 23, 19)

        Dim p6 As New Pen(Color.FromArgb(174, 195, 30), 2)
        G.DrawLine(p6, 8, 14, 8, 17)
        Dim p7 As New Pen(Color.FromArgb(174, 195, 30), 2)
        G.DrawLine(p7, 26, 14, 26, 17)


        Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)
        G.DrawString(Text, drawFont, New SolidBrush(Color.WhiteSmoke), New Rectangle(32, 1, Width - 1, 27), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Private MouseP As Point = New Point(0, 0)
    Private Cap As Boolean = False
    Private MoveHeight% = 29 : Private pos% = 0
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
Public Class PerplexControlBox : Inherits Control

#Region " MouseStates "
    Dim State As MouseState = MouseState.None
    Dim MinBtn As New Rectangle(0, 0, 20, 20)
    Dim MaxBtn As New Rectangle(25, 0, 20, 20)
    Dim X As Integer

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Location.X > 0 AndAlso e.Location.X < 20 Then
            FindForm.WindowState = FormWindowState.Minimized
        ElseIf e.Location.X > 25 AndAlso e.Location.X < 45 Then
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
    Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Invalidate()
    End Sub
#End Region

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        MyBase.OnPaint(e)

        G.Clear(BackColor)

        'G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case State
            Case MouseState.None
                Dim mlgb As New LinearGradientBrush(MinBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                G.FillPath(mlgb, Draw.RoundRect(MinBtn, 4))
                G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MinBtn, 4))
                Dim mf As New Font("Marlett", 9)
                Dim mfb As New SolidBrush(Color.FromArgb(174, 195, 30))
                G.DrawString("0", mf, mfb, 4, 4)

                Dim lgb As New LinearGradientBrush(MaxBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                G.FillPath(lgb, Draw.RoundRect(MaxBtn, 4))
                G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MaxBtn, 4))
                Dim f As New Font("Marlett", 9)
                Dim fb As New SolidBrush(Color.FromArgb(174, 195, 30))
                G.DrawString("r", f, fb, 28, 4)
            Case MouseState.Over
                If X > 0 AndAlso X < 20 Then
                    Dim mlgb As New LinearGradientBrush(MinBtn, Color.FromArgb(100, 66, 67, 70), Color.FromArgb(100, 43, 44, 48), 90S)
                    G.FillPath(mlgb, Draw.RoundRect(MinBtn, 4))
                    G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MinBtn, 4))
                    Dim mf As New Font("Marlett", 9)
                    Dim mfb As New SolidBrush(Color.FromArgb(174, 195, 30))
                    G.DrawString("0", mf, mfb, 4, 4)

                    Dim lgb As New LinearGradientBrush(MaxBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                    G.FillPath(lgb, Draw.RoundRect(MaxBtn, 4))
                    G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MaxBtn, 4))
                    Dim f As New Font("Marlett", 9)
                    Dim fb As New SolidBrush(Color.FromArgb(174, 195, 30))
                    G.DrawString("r", f, fb, 28, 4)

                ElseIf X > 25 AndAlso X < 45 Then
                    Dim mlgb As New LinearGradientBrush(MinBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                    G.FillPath(mlgb, Draw.RoundRect(MinBtn, 4))
                    G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MinBtn, 4))
                    Dim mf As New Font("Marlett", 9)
                    Dim mfb As New SolidBrush(Color.FromArgb(174, 195, 30))
                    G.DrawString("0", mf, mfb, 4, 4)

                    Dim lgb As New LinearGradientBrush(MaxBtn, Color.FromArgb(100, 66, 67, 70), Color.FromArgb(100, 43, 44, 48), 90S)
                    G.FillPath(lgb, Draw.RoundRect(MaxBtn, 4))
                    G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MaxBtn, 4))
                    Dim f As New Font("Marlett", 9)
                    Dim fb As New SolidBrush(Color.FromArgb(174, 195, 30))
                    G.DrawString("r", f, fb, 28, 4)
                Else
                    Dim mlgb As New LinearGradientBrush(MinBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                    G.FillPath(mlgb, Draw.RoundRect(MinBtn, 4))
                    G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MinBtn, 4))
                    Dim mf As New Font("Marlett", 9)
                    Dim mfb As New SolidBrush(Color.FromArgb(174, 195, 30))
                    G.DrawString("0", mf, mfb, 4, 4)

                    Dim lgb As New LinearGradientBrush(MaxBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                    G.FillPath(lgb, Draw.RoundRect(MaxBtn, 4))
                    G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MaxBtn, 4))
                    Dim f As New Font("Marlett", 9)
                    Dim fb As New SolidBrush(Color.FromArgb(174, 195, 30))
                    G.DrawString("r", f, fb, 28, 4)
                End If
            Case MouseState.Down
                Dim mlgb As New LinearGradientBrush(MinBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                G.FillPath(mlgb, Draw.RoundRect(MinBtn, 4))
                G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MinBtn, 4))
                Dim mf As New Font("Marlett", 9)
                Dim mfb As New SolidBrush(Color.FromArgb(174, 195, 30))
                G.DrawString("0", mf, mfb, 4, 4)

                Dim lgb As New LinearGradientBrush(MaxBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                G.FillPath(lgb, Draw.RoundRect(MaxBtn, 4))
                G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MaxBtn, 4))
                Dim f As New Font("Marlett", 9)
                Dim fb As New SolidBrush(Color.FromArgb(174, 195, 30))
                G.DrawString("r", f, fb, 28, 4)
            Case Else
                Dim mlgb As New LinearGradientBrush(MinBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                G.FillPath(mlgb, Draw.RoundRect(MinBtn, 4))
                G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MinBtn, 4))
                Dim mf As New Font("Marlett", 9)
                Dim mfb As New SolidBrush(Color.FromArgb(174, 195, 30))
                G.DrawString("0", mf, mfb, 4, 4)

                Dim lgb As New LinearGradientBrush(MaxBtn, Color.FromArgb(66, 67, 70), Color.FromArgb(43, 44, 48), 90S)
                G.FillPath(lgb, Draw.RoundRect(MaxBtn, 4))
                G.DrawPath(New Pen(Color.FromArgb(21, 21, 21), 1), Draw.RoundRect(MaxBtn, 4))
                Dim f As New Font("Marlett", 9)
                Dim fb As New SolidBrush(Color.FromArgb(174, 195, 30))
                G.DrawString("r", f, fb, 28, 4)
        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class PerplexGroupBox : Inherits ContainerControl

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Body As New Rectangle(4, 25, Width - 9, Height - 30)
        Dim Body2 As New Rectangle(0, 0, Width - 1, Height - 1)

        MyBase.OnPaint(e)

        G.Clear(Color.Transparent)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality

        Dim p As New Pen(Color.Black)
        Dim BodyBrush As New LinearGradientBrush(Body2, Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90S)
        G.FillPath(BodyBrush, Draw.RoundRect(Body2, 3))
        G.DrawPath(New Pen(Brushes.Black), Draw.RoundRect(Body2, 3))

        Dim BodyBrush2 As New LinearGradientBrush(Body, Color.FromArgb(46, 46, 46), Color.FromArgb(50, 55, 58), 120S)
        G.FillPath(BodyBrush2, Draw.RoundRect(Body, 3))
        G.DrawPath(p, Draw.RoundRect(Body, 3))

        Dim drawFont As New Font("Tahoma", 9, FontStyle.Bold)
        G.DrawString(Text, drawFont, New SolidBrush(Color.WhiteSmoke), 67, 14, New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class PerplexProgressBar : Inherits Control

#Region " Control Help - Properties & Flicker Control "
    Private OFS As Integer = 0
    Private Speed As Integer = 50
    Private _Maximum As Integer = 100

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
    Private _Value As Integer = 0
    Public Property Value() As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
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
    Private _ShowPercentage As Boolean = False
    Public Property ShowPercentage() As Boolean
        Get
            Return _ShowPercentage
        End Get
        Set(ByVal v As Boolean)
            _ShowPercentage = v
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        ' Dim tmr As New Timer With {.Interval = Speed}
        ' AddHandler tmr.Tick, AddressOf Animate
        ' tmr.Start()
        Dim T As New Threading.Thread(AddressOf Animate)
        T.IsBackground = True
        'T.Start()
    End Sub
    Sub Animate()
        While True
            If OFS <= Width Then : OFS += 1
            Else : OFS = 0
            End If
            Invalidate()
            Threading.Thread.Sleep(Speed)
        End While
    End Sub
#End Region

    Sub New()
        MyBase.New()
        DoubleBuffered = True
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.SmoothingMode = SmoothingMode.HighQuality

        Dim intValue As Integer = CInt(_Value / _Maximum * Width)
        G.Clear(BackColor)

        Dim gB As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(26, 26, 26), Color.FromArgb(30, 30, 30), 90S)
        G.FillPath(gB, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 1))
        Dim g1 As New LinearGradientBrush(New Rectangle(2, 2, intValue - 1, Height - 2), Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90S)
        G.FillPath(g1, Draw.RoundRect(New Rectangle(0, 0, intValue - 1, Height - 2), 1))
        Dim h1 As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(50, 174, 195, 30), Color.FromArgb(25, 141, 153, 16))
        G.FillPath(h1, Draw.RoundRect(New Rectangle(0, 0, intValue - 1, Height - 2), 1))

        'G.DrawPath(New Pen(Color.FromArgb(125, 97, 94, 90)), Draw.RoundRect(New Rectangle(0, 1, Width - 1, Height - 3), 2))
        G.DrawPath(New Pen(Color.FromArgb(0, 0, 0)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))

        G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), Draw.RoundRect(New Rectangle(0, 0, intValue - 1, Height - 1), 2))
        G.DrawPath(New Pen(Color.FromArgb(0, 0, 0)), Draw.RoundRect(New Rectangle(0, 0, intValue - 1, Height - 1), 2))

        If _ShowPercentage Then
            G.DrawString(Convert.ToString(String.Concat(Value, "%")), Font, Brushes.White, New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
<DefaultEvent("CheckedChanged")> Public Class PerplexCheckBox : Inherits Control

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
        BackColor = Color.Transparent
        ForeColor = Color.Black
        Size = New Size(145, 16)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim checkBoxRectangle As New Rectangle(0, 0, Height - 1, Height - 1)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality

        G.Clear(BackColor)

        Dim bodyGrad As New LinearGradientBrush(checkBoxRectangle, Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90S)
        G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
        G.DrawRectangle(New Pen(Color.Gray), New Rectangle(1, 1, Height - 3, Height - 3))
        G.DrawRectangle(New Pen(Color.Black), checkBoxRectangle)

        Dim drawFont As New Font("Tahoma", 9, FontStyle.Bold)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(205, 205, 205))
        G.DrawString(Text, drawFont, Brushes.Black, New Point(17, 9), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
        G.DrawString(Text, drawFont, nb, New Point(16, 8), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        If Checked Then
            Dim chkPoly As Rectangle = New Rectangle(checkBoxRectangle.X + checkBoxRectangle.Width / 4, checkBoxRectangle.Y + checkBoxRectangle.Height / 4, checkBoxRectangle.Width \ 2, checkBoxRectangle.Height \ 2)
            Dim Poly() As Point = {New Point(chkPoly.X, chkPoly.Y + chkPoly.Height \ 2), _
                           New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height), _
                           New Point(chkPoly.X + chkPoly.Width, chkPoly.Y)}
            G.SmoothingMode = SmoothingMode.HighQuality
            Dim P1 As New Pen(Color.FromArgb(12, 12, 12), 2)
            Dim chkGrad As New LinearGradientBrush(chkPoly, Color.White, Color.White, 0S)
            For i = 0 To Poly.Length - 2
                G.DrawLine(P1, Poly(i), Poly(i + 1))
            Next
        End If

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class
<DefaultEvent("CheckedChanged")> Public Class PerplexRadioButton : Inherits Control

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
            If C IsNot Me AndAlso TypeOf C Is PerplexRadioButton Then
                DirectCast(C, PerplexRadioButton).Checked = False
            End If
        Next
    End Sub
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.Black
        Size = New Size(150, 16)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim radioBtnRectangle = New Rectangle(0, 0, Height - 1, Height - 1)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality

        G.Clear(BackColor)

        Dim bgGrad As New LinearGradientBrush(radioBtnRectangle, Color.FromArgb(174, 195, 30), Color.FromArgb(141, 153, 16), 90S)
        G.FillEllipse(bgGrad, radioBtnRectangle)

        'G.DrawEllipse(New Pen(Color.Gray), New Rectangle(1, 1, Height - 3, Height - 3))
        G.DrawEllipse(New Pen(Color.Black), radioBtnRectangle)

        If Checked Then
            Dim chkGrad As New LinearGradientBrush(New Rectangle(4, 4, Height - 9, Height - 9), Color.FromArgb(250, 15, 15, 15), Color.FromArgb(250, 15, 15, 15), 90S)
            G.FillEllipse(chkGrad, New Rectangle(4, 4, Height - 9, Height - 9))
        End If

        Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(205, 205, 205))
        G.DrawString(Text, drawFont, Brushes.Black, New Point(17, 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
        G.DrawString(Text, drawFont, nb, New Point(16, 1), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class
Public Class PerplexLabel : Inherits Control

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)


        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Tahoma", 9, FontStyle.Bold)

        G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality

        G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(5, 5, 5)), New Rectangle(1, 0, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
        G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(205, 205, 205)), New Rectangle(0, -1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class