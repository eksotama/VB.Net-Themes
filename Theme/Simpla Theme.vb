Imports System.Drawing.Drawing2D
Imports System.ComponentModel

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

'--------------------- [ Theme ] --------------------
'Creator: Mephobia
'Contact: Mephobia.HF (Skype)
'Created: 4.15.2013
'Changed: 4.15.2013
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
Public Class SimplaTheme : Inherits ContainerControl

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.FromArgb(25, 25, 25)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim mainTop As New Rectangle(0, 0, Width - 1, 40)
        Dim mainBottom As New Rectangle(0, 40, Width, Height)

        MyBase.OnPaint(e)


        G.Clear(Color.Fuchsia)
        G.SetClip(Draw.RoundRect(New Rectangle(0, 0, Width, Height), 9))

        Dim gradientBackground As New SolidBrush(Color.FromArgb(34, 34, 34))
        G.FillRectangle(gradientBackground, mainBottom)

        Dim gradientBackground2 As New LinearGradientBrush(mainTop, Color.FromArgb(23, 23, 23), Color.FromArgb(17, 17, 17), 90S)
        G.FillRectangle(gradientBackground2, mainTop)

        G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(56, 56, 56))), 0, 40, Width - 1, 40)
        G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(42, 42, 42))), 0, 41, Width - 1, 41)

        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(12, 12, 12))), New Rectangle(0, 0, Width - 1, Height - 1))

        Dim drawFont As New Font("Calibri (Body)", 15, FontStyle.Bold)
        G.DrawString(FindForm.Text, drawFont, New SolidBrush(Color.FromArgb(225, 225, 225)), 3, 18)

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
    Private MouseP As Point = New Point(0, 0)
    Private Cap As Boolean = False
    Private MoveHeight% = 40 : Private pos% = 0
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
        Me.ParentForm.FormBorderstyle = FormBorderStyle.None
        Me.ParentForm.TransparencyKey = Color.Fuchsia
        Dock = DockStyle.Fill
    End Sub
End Class
Public Class SimplaButton : Inherits Control
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

    Enum ColorSchemes
        DarkGray
        Green
        Blue
        White
        Red
    End Enum
    Private _ColorScheme As ColorSchemes
    Public Property ColorScheme() As ColorSchemes
        Get
            Return _ColorScheme
        End Get
        Set(ByVal value As ColorSchemes)
            _ColorScheme = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim InnerRectangle As New Rectangle(1, 1, Width - 3, Height - 3)

        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Calibri (Body)", 10, FontStyle.Bold)

        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case ColorScheme
            Case ColorSchemes.DarkGray
                Dim gradientBackground As New LinearGradientBrush(ClientRectangle, Color.FromArgb(23, 23, 23), Color.FromArgb(17, 17, 17), 90S)
                G.FillPath(gradientBackground, Draw.RoundRect(ClientRectangle, 4))

                Dim p As New Pen(New SolidBrush(Color.FromArgb(56, 56, 56)))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 4))

                Dim p2 As New Pen(New SolidBrush(Color.FromArgb(5, 240, 240, 240)))
                G.DrawPath(p2, Draw.RoundRect(InnerRectangle, 4))
            Case ColorSchemes.Green
                Dim gradientBackground As New LinearGradientBrush(ClientRectangle, Color.FromArgb(121, 185, 0), Color.FromArgb(94, 165, 1), 90S)
                G.FillPath(gradientBackground, Draw.RoundRect(ClientRectangle, 4))

                Dim p As New Pen(New SolidBrush(Color.FromArgb(159, 207, 1)))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 4))

                Dim p2 As New Pen(New SolidBrush(Color.FromArgb(30, 240, 240, 240)))
                G.DrawPath(p2, Draw.RoundRect(InnerRectangle, 4))
            Case ColorSchemes.Blue
                Dim gradientBackground As New LinearGradientBrush(ClientRectangle, Color.FromArgb(0, 124, 186), Color.FromArgb(0, 97, 166), 90S)
                G.FillPath(gradientBackground, Draw.RoundRect(ClientRectangle, 4))

                Dim p As New Pen(New SolidBrush(Color.FromArgb(0, 161, 207)))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 4))

                Dim p2 As New Pen(New SolidBrush(Color.FromArgb(10, 240, 240, 240)))
                G.DrawPath(p2, Draw.RoundRect(InnerRectangle, 4))
            Case ColorSchemes.White
                Dim gradientBackground As New LinearGradientBrush(ClientRectangle, Color.FromArgb(245, 245, 245), Color.FromArgb(246, 246, 246), 90S)
                G.FillPath(gradientBackground, Draw.RoundRect(ClientRectangle, 4))

                Dim p As New Pen(New SolidBrush(Color.FromArgb(254, 254, 254)))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 4))

                Dim p2 As New Pen(New SolidBrush(Color.FromArgb(10, 240, 240, 240)))
                G.DrawPath(p2, Draw.RoundRect(InnerRectangle, 4))
            Case ColorSchemes.Red
                Dim gradientBackground As New LinearGradientBrush(ClientRectangle, Color.FromArgb(185, 0, 0), Color.FromArgb(170, 0, 0), 90S)
                G.FillPath(gradientBackground, Draw.RoundRect(ClientRectangle, 4))

                Dim p As New Pen(New SolidBrush(Color.FromArgb(209, 1, 1)))
                G.DrawPath(p, Draw.RoundRect(ClientRectangle, 4))

                Dim p2 As New Pen(New SolidBrush(Color.FromArgb(2, 240, 240, 240)))
                G.DrawPath(p2, Draw.RoundRect(InnerRectangle, 4))
            Case Else

        End Select

        Select Case State
            Case MouseState.None
                If ColorScheme = ColorSchemes.White Then
                    G.DrawString(Text, drawFont, New SolidBrush(Color.FromArgb(51, 51, 51)), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Else
                    Dim textGradient As New LinearGradientBrush(ClientRectangle, Color.FromArgb(235, 235, 235), Color.FromArgb(212, 212, 212), 90S)
                    G.DrawString(Text, drawFont, textGradient, New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                End If
            Case MouseState.Over
                G.DrawString(Text, drawFont, New SolidBrush(Color.Silver), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case MouseState.Down
                G.DrawString(Text, drawFont, New SolidBrush(Color.DimGray), New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
<DefaultEvent("CheckedChanged")> Public Class SimplaCheckBox : Inherits Control

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
        Height = 16
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
#End Region

    Enum ColorSchemes
        Green
        Blue
        White
        Red
    End Enum
    Private _ColorScheme As ColorSchemes
    Public Property ColorScheme() As ColorSchemes
        Get
            Return _ColorScheme
        End Get
        Set(ByVal value As ColorSchemes)
            _ColorScheme = value
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.OptimizedDoubleBuffer, True)
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
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        G.Clear(Color.Transparent)

        Dim bodyGrad As New LinearGradientBrush(checkBoxRectangle, Color.FromArgb(40, 40, 40), Color.FromArgb(30, 30, 30), 90S)
        G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(56, 56, 56)), checkBoxRectangle)

        If Checked Then
            Dim t As New Font("Marlett", 20, FontStyle.Regular)
            Select Case ColorScheme
                Case ColorSchemes.Green
                    G.DrawString("a", t, New SolidBrush(Color.FromArgb(159, 207, 1)), -9, -7)
                Case ColorSchemes.Blue
                    G.DrawString("a", t, New SolidBrush(Color.FromArgb(0, 161, 207)), -9, -7)
                Case ColorSchemes.White
                    G.DrawString("a", t, New SolidBrush(Color.FromArgb(254, 254, 254)), -9, -7)
                Case ColorSchemes.Red
                    G.DrawString("a", t, New SolidBrush(Color.FromArgb(209, 1, 1)), -9, -7)
            End Select
        End If

        Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(200, 200, 200))
        G.DrawString(Text, drawFont, nb, New Point(19, 9), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class
<DefaultEvent("CheckedChanged")> Public Class SimplaRadioButton : Inherits Control

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
            If C IsNot Me AndAlso TypeOf C Is SimplaRadioButton Then
                DirectCast(C, SimplaRadioButton).Checked = False
            End If
        Next
    End Sub
#End Region

    Enum ColorSchemes
        Green
        Blue
        White
        Red
    End Enum
    Private _ColorScheme As ColorSchemes
    Public Property ColorScheme() As ColorSchemes
        Get
            Return _ColorScheme
        End Get
        Set(ByVal value As ColorSchemes)
            _ColorScheme = value
            Invalidate()
        End Set
    End Property

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
        Dim radioBtnRectangle As New Rectangle(0, 0, Height - 1, Height - 1)
        Dim InnerRect As New Rectangle(3, 3, Height - 7, Height - 7)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.CompositingQuality = CompositingQuality.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        G.Clear(BackColor)

        Dim bgGrad As New LinearGradientBrush(radioBtnRectangle, Color.FromArgb(40, 40, 40), Color.FromArgb(30, 30, 30), 90S)
        G.FillRectangle(bgGrad, radioBtnRectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(56, 56, 56)), radioBtnRectangle)

        If Checked Then
            Select Case ColorScheme
                Case ColorSchemes.Green
                    Dim fillGradient As New LinearGradientBrush(InnerRect, Color.FromArgb(121, 185, 0), Color.FromArgb(94, 165, 1), 90S)
                    G.FillRectangle(fillGradient, InnerRect)
                    G.DrawRectangle(New Pen(Color.FromArgb(159, 207, 1)), InnerRect)
                Case ColorSchemes.Blue
                    Dim fillGradient As New LinearGradientBrush(InnerRect, Color.FromArgb(0, 124, 186), Color.FromArgb(0, 97, 166), 90S)
                    G.FillRectangle(fillGradient, InnerRect)
                    G.DrawRectangle(New Pen(Color.FromArgb(0, 161, 207)), InnerRect)
                Case ColorSchemes.White
                    Dim fillGradient As New LinearGradientBrush(InnerRect, Color.FromArgb(245, 245, 245), Color.FromArgb(246, 246, 246), 90S)
                    G.FillRectangle(fillGradient, InnerRect)
                    G.DrawRectangle(New Pen(Color.FromArgb(254, 254, 254)), InnerRect)
                Case ColorSchemes.Red
                    Dim fillGradient As New LinearGradientBrush(InnerRect, Color.FromArgb(185, 0, 0), Color.FromArgb(170, 0, 0), 90S)
                    G.FillRectangle(fillGradient, InnerRect)
                    G.DrawRectangle(New Pen(Color.FromArgb(209, 1, 1)), InnerRect)
            End Select
        End If

        Dim drawFont As New Font("Tahoma", 10, FontStyle.Bold)
        Dim nb As Brush = New SolidBrush(Color.FromArgb(200, 200, 200))
        G.DrawString(Text, drawFont, nb, New Point(19, 8), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class
Public Class SimplaControlBox : Inherits Control
    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(205, 205, 205)
        Size = New Size(52, 26)
        DoubleBuffered = True
    End Sub
#Region " MouseStates "
    Dim State As MouseState = MouseState.None
    Dim X As Integer
    Dim MinBtn As New Rectangle(0, 0, 32, 25)
    Dim CloseBtn As New Rectangle(33, 0, 65, 25)
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If X > MinBtn.X And X < MinBtn.X + 32 Then
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
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle As New Rectangle(0, 0, 64, 25)
        Dim InnerRect As New Rectangle(1, 1, 62, 23)


        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Dim drawFont As New Font("Marlett", 10, FontStyle.Bold)

        'G.CompositingQuality = CompositingQuality.HighQuality
        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case State
            Case MouseState.None
                G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
            Case MouseState.Over
                If X > MinBtn.X And X < MinBtn.X + 32 Then
                    G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                    G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(95, Color.Green)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                    G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                Else
                    G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(95, Color.Red)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                    G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                End If
            Case MouseState.Down
                G.DrawString("r", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(17, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                G.DrawString("0", drawFont, New SolidBrush(Color.FromArgb(178, 178, 178)), New Rectangle(8, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
        End Select


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class SimplaProgressBar : Inherits Control

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

    Enum ColorSchemes
        DarkGray
        Green
        Blue
        White
        Red
    End Enum
    Private _ColorScheme As ColorSchemes
    Public Property ColorScheme() As ColorSchemes
        Get
            Return _ColorScheme
        End Get
        Set(ByVal value As ColorSchemes)
            _ColorScheme = value
            Invalidate()
        End Set
    End Property

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


        G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(26, 26, 26))), Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 2))

        Dim percentColor As SolidBrush = New SolidBrush(Color.White)
        '//// Bar Fill
        If Not intValue = 0 Then
            Select Case ColorScheme
                Case ColorSchemes.DarkGray
                    Dim g1 As New LinearGradientBrush(New Rectangle(2, 2, intValue - 5, Height - 5), Color.FromArgb(23, 23, 23), Color.FromArgb(17, 17, 17), 90S)
                    G.FillPath(g1, Draw.RoundRect(New Rectangle(2, 2, intValue - 5, Height - 5), 2))
                    percentColor = New SolidBrush(Color.White)
                Case ColorSchemes.Green
                    Dim g1 As New LinearGradientBrush(New Rectangle(2, 2, intValue - 5, Height - 5), Color.FromArgb(121, 185, 0), Color.FromArgb(94, 165, 1), 90S)
                    G.FillPath(g1, Draw.RoundRect(New Rectangle(2, 2, intValue - 5, Height - 5), 2))
                    percentColor = New SolidBrush(Color.White)
                Case ColorSchemes.Blue
                    Dim g1 As New LinearGradientBrush(New Rectangle(2, 2, intValue - 5, Height - 5), Color.FromArgb(0, 124, 186), Color.FromArgb(0, 97, 166), 90S)
                    G.FillPath(g1, Draw.RoundRect(New Rectangle(2, 2, intValue - 5, Height - 5), 2))
                    percentColor = New SolidBrush(Color.White)
                Case ColorSchemes.White
                    Dim g1 As New LinearGradientBrush(New Rectangle(2, 2, intValue - 5, Height - 5), Color.FromArgb(245, 245, 245), Color.FromArgb(246, 246, 246), 90S)
                    G.FillPath(g1, Draw.RoundRect(New Rectangle(2, 2, intValue - 5, Height - 5), 2))
                    percentColor = New SolidBrush(Color.FromArgb(20, 20, 20))
                Case ColorSchemes.Red
                    Dim g1 As New LinearGradientBrush(New Rectangle(2, 2, intValue - 5, Height - 5), Color.FromArgb(185, 0, 0), Color.FromArgb(170, 0, 0), 90S)
                    G.FillPath(g1, Draw.RoundRect(New Rectangle(2, 2, intValue - 5, Height - 5), 2))
                    percentColor = New SolidBrush(Color.White)
            End Select
        End If

        '//// Outer Rectangle
        G.DrawPath(New Pen(Color.FromArgb(190, 56, 56, 56)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))

        '//// Bar Size
        G.DrawPath(New Pen(Color.FromArgb(150, 97, 94, 90)), Draw.RoundRect(New Rectangle(2, 2, intValue - 5, Height - 5), 2))

        If _ShowPercentage Then
            G.DrawString(Convert.ToString(String.Concat(Value, "%")), New Font("Tahoma", 9, FontStyle.Bold), percentColor, New Rectangle(0, 1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class
Public Class SimplaTextBox : Inherits TextBox

    Sub New()
        Borderstyle = Windows.Forms.BorderStyle.FixedSingle
        Font = New Font("Tahoma", 9, FontStyle.Bold)
        BackColor = Color.FromArgb(35, 35, 35)
        ForeColor = Color.WhiteSmoke
    End Sub
End Class
Public Class SimplaTabControl : Inherits TabControl

    Private mainBackground As Color
    Private topBackground As Color
    Private activeTabColor As Color

    Enum ColorSchemes
        DarkGray
        Green
        Blue
        White
        Red
    End Enum
    Private _ColorScheme As ColorSchemes
    Public Property ColorScheme() As ColorSchemes
        Get
            Return _ColorScheme
        End Get
        Set(ByVal value As ColorSchemes)
            _ColorScheme = value
            Invalidate()
        End Set
    End Property
    Private _textColor As Color
    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True

        BackColor = Color.FromArgb(34, 34, 34)
        topBackground = Color.FromArgb(34, 34, 34)
        mainBackground = Color.FromArgb(34, 34, 34)
        ForeColor = Color.White
        _textColor = ForeColor
        activeTabColor = Color.FromArgb(20, 20, 20)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        MyBase.OnPaint(e)

        Try : SelectedTab.BackColor = mainBackground : Catch : End Try

        G.Clear(topBackground)
        G.SmoothingMode = SmoothingMode.HighQuality

        Select Case ColorScheme
            Case ColorSchemes.DarkGray
                activeTabColor = Color.FromArgb(10, 10, 10)
                _textColor = Color.White
            Case ColorSchemes.Green
                activeTabColor = Color.FromArgb(94, 165, 1)
                _textColor = Color.White
            Case ColorSchemes.Blue
                activeTabColor = Color.FromArgb(0, 97, 166)
                _textColor = Color.White
            Case ColorSchemes.White
                activeTabColor = Color.FromArgb(245, 245, 245)
                _textColor = Color.FromArgb(36, 36, 36)
            Case ColorSchemes.Red
                activeTabColor = Color.FromArgb(170, 0, 0)
                _textColor = Color.White
        End Select

        For i As Integer = 0 To TabPages.Count - 1
            Dim TabRect As New Rectangle(GetTabRect(i).X + 3, GetTabRect(i).Y, GetTabRect(i).Width - 5, GetTabRect(i).Height)
            G.FillRectangle(New SolidBrush(mainBackground), TabRect)
            G.DrawString(TabPages(i).Text, New Font("Tahoma", 9, FontStyle.Bold), New SolidBrush(ForeColor), TabRect, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
        Next

        G.FillRectangle(New SolidBrush(mainBackground), 0, ItemSize.Height, Width, Height)

        If Not SelectedIndex = -1 Then
            Dim TabRect As New Rectangle(GetTabRect(SelectedIndex).X + 3, GetTabRect(SelectedIndex).Y, GetTabRect(SelectedIndex).Width - 5, GetTabRect(SelectedIndex).Height)
            G.FillPath(New SolidBrush(activeTabColor), Draw.RoundRect(TabRect, 4))
            G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(20, 20, 20))), Draw.RoundRect(TabRect, 4))
            G.DrawString(TabPages(SelectedIndex).Text, New Font("Tahoma", 9, FontStyle.Bold), New SolidBrush(_textColor), TabRect, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If

        e.Graphics.DrawImage(B, New Point(0, 0))
        G.Dispose() : B.Dispose()
    End Sub
End Class