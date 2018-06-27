Imports System.Drawing.Drawing2D
'|===========================================================|
'|===|  Reactor (Theme used as reference)
'| Creator: ᒪeumonic
'| HF Account: http://www.hackforums.net/member.php?action=profile&uid=221681
'| Created: 11/22/2011
'|
'|
'|===|  Deimos Theme
'| Creator: LordPankake
'| HF Account: http://www.hackforums.net/member.php?action=profile&uid=1828119
'| Created: 6/20/2014, Last edited: 7/5/2014
'|===========================================================|
Public Class DeimosTheme : Inherits ContainerControl
#Region " Control Help - Movement & Flicker Control "
    Private State As MouseState = MouseState.None
    Private TopCap As Boolean = False
    Private SizeCap As Boolean = False
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Private MouseP As Point = New Point(0, 0)
    Private TopGrip As Integer
    Public Property DrawIcon As Boolean = True
    Public Property DrawIconSlot As Boolean = True
    Public Property InnerBox As Boolean = True
    Public Property InnerGradient As Boolean = True
    Public Property InnerRigid As Boolean = False
    Public Property AllowResize As Boolean = True
    Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If New Rectangle(0, 0, Width, TopGrip).Contains(e.Location) Then
                TopCap = True : MouseP = e.Location
            ElseIf New Rectangle(Width - 15, Height - 15, 15, 15).Contains(e.Location) And AllowResize Then
                SizeCap = True : MouseP = e.Location
            End If
        End If
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        TopCap = False
        SizeCap = False
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If TopCap Then
            Parent.Location = MousePosition - MouseP
        End If
        If SizeCap And AllowResize Then
            MouseP = e.Location
            Parent.Size = New Size(MouseP)
            Invalidate()
        End If
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Dock = DockStyle.Fill
        TopGrip = 45
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
        BackColor = Color.FromArgb(55, 55, 55)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim F As Form = FindForm()
        Dim D As New DrawUtils
        MyBase.OnPaint(e)

        G.Clear(F.TransparencyKey)

        '|========================================================================================|
        '|                                                                                        |
        '|      Drawing the form frame                                                            |
        '|                                                                                        |
        '| ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---|
        Dim FrameRectangle As New Rectangle(0, 40, F.Width - 1, F.Height - 41)
        G.FillRectangle(New SolidBrush(ColLow), FrameRectangle)
        G.DrawRectangle(New Pen(ColDark), FrameRectangle)
        If InnerBox Then
            Dim InternalRect As New Rectangle(15, 43, F.Width - 31, F.Height - 42 - 15)
            Dim InternalPath As GraphicsPath = D.RoundRect(InternalRect, 6)
            Dim InternalGradient As New LinearGradientBrush(New Point(0, 35), New Point(0, F.Height - 14), ColMed, Color.FromArgb(47, 49, 54))
            Dim InternalPathHighlight As GraphicsPath = D.RoundRect(New Rectangle(16, 44, F.Width - 33, F.Height - 42 - 15), 4)
            If Not InnerRigid Then
                G.SmoothingMode = SmoothingMode.HighQuality
            End If
            If InnerGradient Then
                G.FillPath(InternalGradient, InternalPath)
            Else
                G.FillPath(New SolidBrush(ColMed), InternalPath)
            End If

            G.DrawPath(New Pen(ColMed), InternalPathHighlight)
            G.DrawPath(New Pen(ColDark), InternalPath)
            G.SmoothingMode = SmoothingMode.None
        End If

        '| =======================================================================================|
        '|                                                                                        |
        '|      Drawing the middle bar                                                            |
        '|                                                                                        |
        '| ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---|
        '| Top Stuffs (Non-Outline Bar)
        Dim TopGripGradient As New LinearGradientBrush(New Point(0, 0), New Point(0, 25), ColMed, ColLow)
        Dim TopTexturedRect As New Rectangle(40, 5, F.Width - 80, 35)
        Dim TexturedTopBrush As New HatchBrush(HatchStyle.DarkDownwardDiagonal, ColLow, Color.Transparent)
        '| Middle bar
        Dim LinearGradientPen1 As New LinearGradientBrush(New Point(0, 5), New Point(F.Width / 2, 5), ColMed, ColHigh)
        Dim LinearGradientPen2 As New LinearGradientBrush(New Point(F.Width / 2, 5), New Point(F.Width, 5), ColHigh, ColMed)
        Dim LinearGradientPen3 As New LinearGradientBrush(New Point(0, 5), New Point(F.Width / 2, 5), ColDark, ColLow)
        Dim LinearGradientPen4 As New LinearGradientBrush(New Point(F.Width / 2, 5), New Point(F.Width, 5), ColLow, ColDark)
        '| Top Middle
        G.FillRectangle(TopGripGradient, TopTexturedRect)
        G.FillRectangle(TexturedTopBrush, TopTexturedRect)
        '| Middle Bar
        G.DrawLine(New Pen(LinearGradientPen3), New Point(51, 5), New Point((51 + F.Width - 52) / 2, 5))
        G.DrawLine(New Pen(LinearGradientPen4), New Point((51 + F.Width - 52) / 2, 5), New Point(F.Width - 52, 5))
        G.DrawLine(New Pen(LinearGradientPen1), New Point(51, 6), New Point((51 + F.Width - 52) / 2, 6))
        G.DrawLine(New Pen(LinearGradientPen2), New Point((51 + F.Width - 52) / 2, 6), New Point(F.Width - 52, 6))
        G.DrawLine(New Pen(ColHigh), New Point(F.Width / 2 - 1, 6), New Point(F.Width / 2 + 1, 6))
        '| =======================================================================================|
        '|                                                                                        |
        '|      Drawing the top right/left boxes                                                  |
        '|                                                                                        |
        '| ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---|

        '| Top Right
        Dim TopRightRect As New Rectangle(New Point((F.Width - 1) - 50, 0), New Size(New Point(50, 40)))
        Dim TopRightRectLight As New Rectangle(New Point((F.Width - 1) - 50, 1), New Size(New Point(50, 39)))
        Dim TopRightPath As GraphicsPath = D.RoundedTopRect(TopRightRect, 6)
        Dim TopRightPathLight As GraphicsPath = D.RoundedTopRect(TopRightRectLight, 6)
        '| Top Left 
        Dim TopLeftRect As New Rectangle(New Point(0, 0), New Size(New Point(50, 40)))
        Dim TopLeftRectLight As New Rectangle(New Point(0, 1), New Size(New Point(50, 39)))
        Dim TopLeftPath As GraphicsPath = D.RoundedTopRect(TopLeftRect, 6)
        Dim TopLeftPathLight As GraphicsPath = D.RoundedTopRect(TopLeftRectLight, 6)
        Dim TopGradient As New LinearGradientBrush(New Point(0, 0), New Point(0, 40), ColMed, ColLow)
        '| Top Left/Right
        G.FillPath(TopGradient, TopLeftPath)
        G.FillPath(TopGradient, TopRightPath)
        G.SmoothingMode = SmoothingMode.HighQuality
        G.DrawPath(New Pen(ColHigh), TopLeftPathLight)
        G.DrawPath(New Pen(ColHigh), TopRightPathLight)
        G.SmoothingMode = SmoothingMode.None
        G.DrawPath(New Pen(ColDark), TopRightPath)
        G.DrawPath(New Pen(ColDark), TopLeftPath)
        G.DrawLine(New Pen(ColLow), New Point(1, 40), New Point(F.Width - 2, 40))

        '| =======================================================================================|
        '|                                                                                        |
        '|      Drawing the middle connecter bar (attatched visually to main frame                |
        '|                                                                                        |
        '| ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---   ---|
        '| Top Middle (Bar)
        Dim Lefts As Point() = {New Point(49, 15), New Point(61, 27), New Point(49, 27)}
        Dim Rights As Point() = {New Point(F.Width - 49, 15), New Point(F.Width - 62, 29), New Point(F.Width - 49, 29)}
        Dim ConnectionRect As New Rectangle(50, 25, F.Width - 100, 15)
        '| Straight connection
        G.FillRectangle(TopGradient, ConnectionRect)
        G.DrawLine(New Pen(ColHigh), New Point(50, 26), New Point(F.Width - 62, 26))
        G.DrawLine(New Pen(ColDark), New Point(50, 25), New Point(F.Width - 51, 25))
        '| Diagonals
        G.SmoothingMode = SmoothingMode.AntiAlias
        G.FillPolygon(TopGradient, Lefts)
        G.DrawLine(New Pen(ColHigh), New Point(50, 16), New Point(60, 26))
        G.DrawLine(New Pen(ColDark), New Point(50, 15), New Point(60, 25))
        G.FillPolygon(TopGradient, Rights)
        G.DrawLine(New Pen(ColHigh), New Point(F.Width - 51, 16), New Point(F.Width - 61, 26))
        G.DrawLine(New Pen(ColDark), New Point(F.Width - 51, 15), New Point(F.Width - 61, 25))
        G.SmoothingMode = SmoothingMode.None
        If DrawIcon Then
            If DrawIconSlot Then
                Dim IconRect As GraphicsPath = D.RoundRect(New Rectangle(10, 4, 31, 31), 4)
                Dim InnerGradient As New LinearGradientBrush(New Point(5, 2), New Point(5, 40), ColLow, ColDark)
                G.SmoothingMode = SmoothingMode.HighQuality
                G.FillPath(InnerGradient, IconRect)
                G.DrawPath(New Pen(ColDark), IconRect)
            End If
            G.DrawIcon(F.Icon, New Rectangle(11, 5, 30, 30))
        End If
        G.SmoothingMode = SmoothingMode.None
        D.DrawTextWithShadow(G, New Rectangle(60, 0, Width - 55, 32), F.Text, Font, HorizontalAlignment.Left, ForeColor)
    End Sub
End Class
Public Class DeimosTopControl : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Private State As MouseState = MouseState.None
    Public Property xOffset As Integer
    Public Property yOffset As Integer
    Public Property Mode As String = "Close"
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        If New Rectangle(0, 0, Width, Height).Contains(e.Location) And Not IsNothing(Mode) Then
            If Mode = "Close" Then
                FindForm.Close()
            ElseIf Mode = "Minimize" Then
                FindForm.WindowState = FormWindowState.Minimized
            ElseIf Mode = "Maximized" Then
                If FindForm.WindowState = FormWindowState.Maximized Then
                    FindForm.WindowState = FormWindowState.Normal
                Else
                    FindForm.WindowState = FormWindowState.Maximized
                End If
            End If
        End If
        State = MouseState.Over
        SizeChangeExternal(Size)

        Dim B As New Bitmap(Width, Height)

        PaintExternal(Graphics.FromImage(B))
        Invalidate()
    End Sub
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        Invalidate()
    End Sub
    Public Sub PaintExternal(ByVal G As Graphics)
        PaintControl(G)
    End Sub
    Public Sub SizeChangeExternal(e As Size)
        Size = e
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Text = ""
        Size = New Size(New Point(21, 26))
        BackColor = Color.FromArgb(35, 37, 40)
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics

        MyBase.OnPaint(e)
        PaintControl(G)
    End Sub
    Private Sub PaintControl(ByVal G As Graphics)
        Dim D As New DrawUtils
        Top = 0
        Size = New Size(New Point(21, Height))
        G.Clear(Color.Fuchsia)
        Dim ContRect As GraphicsPath = D.RoundedTopRect(New Rectangle(0, 0, Width - 1, Height - 1), 5)
        Dim ContRectHighlight As GraphicsPath = D.RoundedTopRect(New Rectangle(0, 1, Width - 1, Height - 2), 5)

        Select Case State
            Case MouseState.None
                Dim ContGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColMed, ColLow)
                G.FillPath(ContGrad, ContRect)
                G.SmoothingMode = SmoothingMode.HighQuality
                G.DrawPath(New Pen(ColHigh), ContRectHighlight)
                G.SmoothingMode = SmoothingMode.None
                G.DrawPath(New Pen(ColDark), ContRect)

                G.DrawString(Text, Font, Brushes.Black, New Point(5 + xOffset, 1 + yOffset))
                G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(146, 149, 152)), New Point(4 + xOffset, 0 + yOffset))
            Case MouseState.Down
                Dim ContGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColLow, ColDark)
                G.FillPath(ContGrad, ContRect)
                G.SmoothingMode = SmoothingMode.HighQuality
                G.DrawPath(New Pen(ColHigh), ContRectHighlight)
                G.SmoothingMode = SmoothingMode.None
                G.DrawPath(New Pen(ColDark), ContRect)

                G.DrawString(Text, Font, Brushes.Black, New Point(5 + xOffset, 1 + yOffset))
                G.DrawString(Text, Font, New SolidBrush(ColHigh), New Point(4 + xOffset, 0 + yOffset))
            Case MouseState.Over
                Dim ContGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColHigh, ColMed)
                G.FillPath(ContGrad, ContRect)
                G.SmoothingMode = SmoothingMode.HighQuality
                G.DrawPath(New Pen(Color.FromArgb(144, 144, 144)), ContRectHighlight)
                G.SmoothingMode = SmoothingMode.None
                G.DrawPath(New Pen(ColDark), ContRect)

                G.DrawString(Text, Font, Brushes.Black, New Point(5 + xOffset, 1 + yOffset))
                G.DrawString(Text, Font, New SolidBrush(Color.White), New Point(4 + xOffset, 0 + yOffset))
        End Select
    End Sub
End Class
Public Class DeimosTopControlBar : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        For Each cont As DeimosTopControl In Controls
            cont.SizeChangeExternal(Size)
        Next
        Invalidate()
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Top = 0
        Size = New Size(New Point(61, Height))
        For Each cont As DeimosTopControl In Controls
            cont.PaintExternal(e.Graphics)
        Next
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Size = New Size(New Point(61, 26))

        Dim DeimostTopMin As New DeimosTopControl
        DeimostTopMin.Mode = "Minimize"
        DeimostTopMin.yOffset = -4
        DeimostTopMin.Text = "_"
        DeimostTopMin.Font = New Font("Segoe UI", 12.5F)
        DeimostTopMin.Location = New Point(0, 0)

        Dim DeimostTopMax As New DeimosTopControl
        DeimostTopMax.Mode = "Maximized"
        DeimostTopMax.Text = "□"
        DeimostTopMax.Font = New Font("Segoe UI", 11.0F)
        DeimostTopMax.Location = New Point(20, 0)

        Dim DeimostTopClose As New DeimosTopControl
        DeimostTopClose.Mode = "Close"
        DeimostTopClose.xOffset = 1
        DeimostTopClose.yOffset = 3
        DeimostTopClose.Font = New Font("Segoe UI", 9.0F)
        DeimostTopClose.Location = New Point(40, 0)
        DeimostTopClose.Text = "X"

        Controls.Add(DeimostTopMin)
        Controls.Add(DeimostTopMax)
        Controls.Add(DeimostTopClose)
        Invalidate()
    End Sub
End Class
Public Class DeimosButton : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Private State As MouseState = MouseState.None
    Public Property DrawRigid As Boolean = False
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        Invalidate()
    End Sub
    Public Sub PaintExternal(e As PaintEventArgs)
        OnPaint(e)
    End Sub
    Public Sub SizeChangeExternal(e As Size)
        Size = e
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Text = ""
        Size = New Size(New Point(80, 35))
        BackColor = Color.FromArgb(55, 58, 61)
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        MyBase.OnPaint(e)
        G.Clear(BackColor)
        If Not DrawRigid Then
            G.SmoothingMode = SmoothingMode.HighQuality
        End If
        Dim ContRect As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 5)
        Dim ContRectHighlight As GraphicsPath = D.RoundRect(New Rectangle(0, 1, Width - 1, Height - 2), 5)
        Select Case State
            Case MouseState.None
                Dim ContGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColMed, ColLow)

                G.FillPath(ContGrad, ContRect)
                G.DrawPath(New Pen(ColHigh), ContRectHighlight)
                G.DrawPath(New Pen(ColDark), ContRect)

                D.DrawTextWithShadow(G, New Rectangle(0, 0, Width, Height), Text, Font, HorizontalAlignment.Center, Color.FromArgb(146, 149, 152))
            Case MouseState.Down
                Dim ContGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColLow, ColDark)
                Dim ContRectHighlight2 As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 5)
                G.FillPath(ContGrad, ContRect)
                G.DrawPath(New Pen(ColMed), ContRectHighlight)
                G.DrawPath(New Pen(ColHigh), ContRectHighlight2)
                G.DrawPath(New Pen(ColDark), ContRect)

                D.DrawTextWithShadow(G, New Rectangle(0, 0, Width, Height), Text, Font, HorizontalAlignment.Center, Color.FromArgb(67, 69, 74))
            Case MouseState.Over
                Dim ContGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColHigh, ColMed)

                G.FillPath(ContGrad, ContRect)
                G.DrawPath(New Pen(Color.FromArgb(133, 133, 133)), ContRectHighlight)
                G.DrawPath(New Pen(ColDark), ContRect)

                D.DrawTextWithShadow(G, New Rectangle(0, 0, Width, Height), Text, Font, HorizontalAlignment.Center, Color.FromArgb(162, 165, 168))
        End Select
    End Sub
End Class
Public Class DeimosGroupbox : Inherits ContainerControl
#Region " Control Help - MouseState & Flicker Control"

    Private ColLow, ColMed, ColHigh, ColDark As Color
    Public Property BoxColor As Color = Color.FromArgb(57, 59, 64)
    Public Property DrawRigid As Boolean = False
    Public Property DrawBoxTitle As Boolean = False
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Text = ""
        Size = New Size(New Point(80, 35))
        BackColor = Color.FromArgb(55, 58, 61)
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        MyBase.OnPaint(e)
        G.Clear(BackColor)
        If Not DrawRigid Then
            G.SmoothingMode = SmoothingMode.HighQuality
        End If
        Dim ContRect As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 5)
        Dim ContRectHighlight As GraphicsPath = D.RoundRect(New Rectangle(0, 1, Width - 1, Height - 2), 5)
        Dim ContGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColMed, ColLow)

        G.FillPath(New SolidBrush(BoxColor), ContRect)
        G.DrawPath(New Pen(ColHigh), ContRectHighlight)
        G.DrawPath(New Pen(ColDark), ContRect)
        If DrawBoxTitle Then
            G.SmoothingMode = SmoothingMode.None
            G.DrawLine(New Pen(ColMed), New Point(1, 17), New Point(Width - 2, 17))
            G.DrawLine(New Pen(ColDark), New Point(0, 18), New Point(Width - 1, 18))
            D.DrawTextWithShadow(G, New Rectangle(3, 1, 15, 20), Text, Font, HorizontalAlignment.Left, Color.FromArgb(146, 149, 152))
            Dim Shade As New LinearGradientBrush(New Point(0, 18), New Point(0, 30), ColLow, Color.Transparent)
            G.FillRectangle(Shade, New Rectangle(0, 19, Width, 10))
            ' G.DrawString(Text, Font, Brushes.Black, New Point(5, 1)))
            ' G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(146, 149, 152)), New Point(4, 0))
        End If
    End Sub
End Class
Public Class DeimosLabel : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        Invalidate()
    End Sub
    Public Sub PaintExternal(e As PaintEventArgs)
        OnPaint(e)
    End Sub
    Public Sub SizeChangeExternal(e As Size)
        Size = e
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Text = ""
        Size = New Size(New Point(80, 35))
        BackColor = Color.FromArgb(55, 58, 61)
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        MyBase.OnPaint(e)
        G.Clear(BackColor)

        G.DrawString(Text, Font, Brushes.Black, New Point(5, -5))
        G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(146, 149, 152)), New Point(4, -4))

        '  D.DrawTextWithShadow(G, New Rectangle(3, 0, 15, 10), Text, Font, HorizontalAlignment.Left, Color.FromArgb(146, 149, 152))
    End Sub
End Class
Public Class DeimosTextbox : Inherits Control
    Private WithEvents txtbox As New TextBox
#Region " Control Help - Properties & Flicker Control "
    Public Property TxtReadOnly As Boolean
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Private _maxchars As Integer = 30000
    Private _SingleLineHeight As Integer = 21
    Public Property DrawRigid As Boolean = False
    Public Property MaxCharacters() As Integer
        Get
            Return _maxchars
        End Get
        Set(ByVal v As Integer)
            _maxchars = v
            Invalidate()
        End Set
    End Property
    Public Property IsMultiline() As Boolean
        Get
            Return txtbox.Multiline
        End Get
        Set(ByVal v As Boolean)
            txtbox.Multiline = v
            Invalidate()
        End Set
    End Property
    Public Property TextboxColor() As Color
        Get
            Return txtbox.BackColor
        End Get
        Set(ByVal v As Color)
            txtbox.BackColor = v
            Invalidate()
        End Set
    End Property
    Private _align As HorizontalAlignment
    Public Property TextAlign() As HorizontalAlignment
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
        Invalidate()
    End Sub
    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        txtbox.ForeColor = ForeColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        If Not Size.Height = _SingleLineHeight Then
            tempSize = Size
        End If
        txtbox.Size = New Size(Width - 6, Height - 6)
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
    Sub SetupTextbox()
        txtbox.Multiline = True
        txtbox.BackColor = Color.FromArgb(47, 49, 54)
        txtbox.ForeColor = ForeColor
        txtbox.Text = String.Empty
        txtbox.TextAlign = HorizontalAlignment.Center
        txtbox.BorderStyle = BorderStyle.None
        txtbox.Location = New Point(3, 3)
        Font = New Font("Segoe UI", 8.5F)
        txtbox.Size = New Size(Width - 6, Height - 6)
    End Sub
#End Region
    Sub New()
        MyBase.New()
        SetupTextbox()
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        BackColor = ColMed
        Controls.Add(txtbox)
        Text = ""
        ForeColor = Color.FromArgb(146, 149, 152)
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub
    Private tempSize As Size
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        txtbox.ReadOnly = TxtReadOnly
        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        MyBase.OnPaint(e)
        txtbox.TextAlign = TextAlign
        If Not txtbox.Multiline Then
            txtbox.Location = New Point(6, txtbox.Top)
            If (txtbox.Width + txtbox.Left) > Width - 3 And txtbox.Width > 5 Then
                txtbox.Width -= 1
            End If
            Size = New Size(New Point(Width, _SingleLineHeight))
        Else
            txtbox.Location = New Point(3, txtbox.Top)
            Size = tempSize
        End If
        G.Clear(BackColor)
        Dim ContRect As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 5)
        Dim ContRectHighlight As GraphicsPath = D.RoundRect(New Rectangle(0, 1, Width - 1, Height - 2), 5)
        If Not DrawRigid Then
            G.SmoothingMode = SmoothingMode.HighQuality
        End If
        G.FillPath(New SolidBrush(Color.FromArgb(47, 49, 54)), ContRect)
        G.DrawPath(New Pen(ColHigh), ContRectHighlight)
        G.DrawPath(New Pen(ColDark), ContRect)
    End Sub
End Class
Public Class DeimosTabControl : Inherits TabControl
#Region " Control Help - Movement & Flicker Control "
    Public Property TabRectOffset As Integer = 2
    Public Property TabRectRoundness As Integer = 6
    Public Property BGColor As Color = Color.FromArgb(35, 37, 40)
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        Invalidate()
    End Sub
#End Region
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        ItemSize = New Size(0, 25) '34
        Padding = New Size(13, 0) '24

        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
        BackColor = Color.FromArgb(57, 59, 64)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        Dim FontColor As New Color

        MyBase.OnPaint(e)
        G.Clear(BGColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        For i = 0 To TabCount - 1
            Dim TabRect As Rectangle = GetTabRect(i)
            Dim HighlightRect As Rectangle = GetTabRect(i)

            TabRect = New Rectangle(TabRect.X + TabRectOffset, 0, TabRect.Width - (TabRectOffset * 2), TabRect.Height)
            HighlightRect = New Rectangle(HighlightRect.X + TabRectOffset, 1, HighlightRect.Width - (TabRectOffset * 2), HighlightRect.Height)

            Dim RectPath As GraphicsPath = D.RoundedTopRect(TabRect, TabRectRoundness)
            Dim HighlightPath As GraphicsPath = D.RoundedTopRect(HighlightRect, TabRectRoundness)
            Dim TopGradient As New LinearGradientBrush(New Point(0, 0), New Point(0, TabRect.Height), ColMed, ColLow)
            G.FillPath(TopGradient, RectPath)
            G.DrawPath(New Pen(ColHigh), HighlightPath)
            G.DrawPath(New Pen(ColDark), RectPath)
            If i = SelectedIndex Then
                FontColor = Color.White
            Else
                FontColor = Color.FromArgb(146, 149, 152)
            End If
            Dim titleX As Integer = (TabRect.Location.X + TabRect.Width / 2) - (G.MeasureString(TabPages(i).Text, Font).Width / 2)
            Dim titleY As Integer = (TabRect.Location.Y + TabRect.Height / 2) - (G.MeasureString(TabPages(i).Text, Font).Height / 2)
            D.DrawTextWithShadow(G, New Rectangle(titleX, titleY, 666, 25), TabPages(i).Text, Font, HorizontalAlignment.Left, FontColor)
            ' G.DrawString(TabPages(i).Text, Font, New SolidBrush(FontColor), New Point(titleX, titleY))

            Try : TabPages(i).BackColor = Color.FromArgb(57, 59, 64) : Catch : End Try
        Next
        G.SmoothingMode = SmoothingMode.None
        Dim WorkRectangle As GraphicsPath = D.RoundRect(New Rectangle(0, 24, Width - 1, Height - 26), 2)
        G.FillPath(New SolidBrush(Color.FromArgb(45, 47, 50)), WorkRectangle)
        G.DrawPath(New Pen(ColDark), WorkRectangle)
        G.DrawRectangle(New Pen(ColDark), New Rectangle(3, 28, Width - 7, Height - 32))
    End Sub
End Class
Public Class DeimosTabControlAlt : Inherits TabControl
#Region " Control Help - Movement & Flicker Control "
    Public Property TabRectRoundness As Integer = 5
    Public Property BGColor As Color = Color.FromArgb(35, 37, 40)
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        Invalidate()
    End Sub
#End Region
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(30, 115)

        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
        BackColor = Color.FromArgb(57, 59, 64)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        Dim FontColor As New Color

        MyBase.OnPaint(e)
        G.Clear(BGColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        'Dim ContGrad As New LinearGradientBrush(New Point(0, (30 * i)), New Point(0, (30 * i) + 30), ColMed, ColLow)
        'G.FillRectangle(ContGrad, D.RoundRect(New Rectangle(5, (30 * i), 100, 30), TabRectRoundness))
        'G.DrawPath(New Pen(ColHigh), D.RoundRect(New Rectangle(5, (30 * i) + 1, 100, 30), TabRectRoundness))
        'G.DrawPath(New Pen(ColDark), D.RoundRect(New Rectangle(5, (30 * i), 100, 30), TabRectRoundness))

        For i = 0 To TabCount - 1
            'Dim x2 As Rectangle = New Rectangle(New Point(GetTabRect(i).+ (30 * i)+ (30 * i).X - 2, 10 + (i * 30)), New Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1))
            Dim textrectangle As New Rectangle(5, 30, 100, 30 + (60 * i))


            If SelectedIndex = i Then
                Dim ContGrad As New LinearGradientBrush(New Point(0, (30 * i)), New Point(0, (30 * i) + 30), ColMed, ColLow)
                G.FillPath(ContGrad, D.RoundRect(New Rectangle(5, (30 * i), 150, 30), TabRectRoundness))
                G.DrawPath(New Pen(ColHigh), D.RoundRect(New Rectangle(5, (30 * i) + 1, 150, 30), TabRectRoundness))
                G.DrawPath(Pens.Black, D.RoundRect(New Rectangle(5, (30 * i), 150, 30), TabRectRoundness))
            End If


            If ImageList IsNot Nothing Then
                Try
                    If ImageList.Images(TabPages(i).ImageIndex) IsNot Nothing Then
                        G.DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Point(textrectangle.Location.X + 8, textrectangle.Location.Y + 6))
                        D.DrawTextWithShadow(G, textrectangle, "      " & TabPages(i).Text, Font, HorizontalAlignment.Left, ColHigh)
                    Else
                        D.DrawTextWithShadow(G, textrectangle, TabPages(i).Text, Font, HorizontalAlignment.Left, ColHigh)
                        D.DrawTextWithShadow(G, textrectangle, TabPages(i).Text, Font, HorizontalAlignment.Left, ColHigh)
                    End If
                Catch ex As Exception
                    D.DrawTextWithShadow(G, textrectangle, TabPages(i).Text, Font, HorizontalAlignment.Left, ColHigh)
                End Try
            Else
                D.DrawTextWithShadow(G, textrectangle, TabPages(i).Text, Font, HorizontalAlignment.Center, ColHigh)
            End If

            Try : TabPages(i).BackColor = Color.FromArgb(57, 59, 64) : Catch : End Try
        Next

        G.SmoothingMode = SmoothingMode.None
        Dim WorkRectangle As GraphicsPath = D.RoundRect(New Rectangle(115, 0, Width - 116, Height - 1), 2)
        G.FillPath(New SolidBrush(Color.FromArgb(45, 47, 50)), WorkRectangle)
        G.DrawPath(New Pen(ColDark), WorkRectangle)
        G.DrawRectangle(New Pen(ColDark), New Rectangle(118, 3, Width - 122, Height - 7))
    End Sub
End Class
Public Class DeimosProgressBar : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Public Property DrawRigid As Boolean = False
    Public Property BarMin As Integer = 0
    Public Property BarMax As Integer = 100
    Public Property BarValue As Integer = 50
    Public Property BarBGColor As Color = Color.FromArgb(35, 37, 40)

    Public Sub AddValue(ByVal amount As Integer)
        If BarValue + amount <= BarMax Then
            BarValue += amount
        End If
        Invalidate()
    End Sub

    Private ColLow, ColMed, ColHigh, ColDark As Color
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        Invalidate()
    End Sub
    Public Sub PaintExternal(e As PaintEventArgs)
        OnPaint(e)
    End Sub
    Public Sub SizeChangeExternal(e As Size)
        Size = e
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Text = ""
        Size = New Size(New Point(100, 10))
        BackColor = Color.FromArgb(55, 58, 61)
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        MyBase.OnPaint(e)
        G.Clear(BackColor)

        Dim ContRect As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 5)
        Dim ContGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColMed, ColLow)
        Dim ContRectHighlight As GraphicsPath = D.RoundRect(New Rectangle(0, 1, Width - 1, Height - 2), 5)

        Dim ContRectInner As GraphicsPath = D.RoundRect(New Rectangle(0, 0, (Width - 1) * (BarValue / BarMax), (Height - 2)), 5)
        Dim ContGradInner As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColMed, ColLow)
        Dim ContRectInnerHighlight As GraphicsPath = D.RoundRect(New Rectangle(1, 0, (Width - 1) * (BarValue / BarMax), Height - 2), 5)
        If Not DrawRigid Then G.SmoothingMode = SmoothingMode.HighQuality
        G.FillPath(New SolidBrush(BarBGColor), ContRect)
        G.FillPath(ContGrad, ContRectInner)
        G.DrawPath(New Pen(ColMed), ContRectInnerHighlight)
        G.DrawPath(New Pen(ColDark), ContRectInner)
        G.DrawPath(New Pen(ColHigh), ContRectHighlight)
        G.DrawPath(New Pen(ColDark), ContRect)
        D.DrawTextWithShadow(G, New Rectangle(3, 0, Width, Height), Text, Font, HorizontalAlignment.Center, Color.FromArgb(146, 149, 152))
    End Sub
End Class
Public Class DeimosSwitch : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Public Property ToggleState As Boolean = False
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Public Property DrawRigid As Boolean = False
    Public Property DrawOnOffText As Boolean = True
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        ToggleState = Not ToggleState
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        Invalidate()
    End Sub
    Public Function IsChecked() As Boolean
        Return ToggleState
    End Function
    Public Sub SizeChangeExternal(e As Size)
        Size = e
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Text = ""
        Size = New Size(New Point(66, 26))
        BackColor = Color.FromArgb(57, 59, 64)
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        'Size = New Size(66, 26)

        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        MyBase.OnPaint(e)
        G.Clear(BackColor)
        If Not DrawRigid Then
            G.SmoothingMode = SmoothingMode.HighQuality
        End If
        Dim ContRect As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), 5)
        Dim ContRectHighlight As GraphicsPath = D.RoundRect(New Rectangle(0, 1, Width - 1, Height - 2), 5)

        If ToggleState Then
            G.FillPath(New SolidBrush(Color.FromArgb(45, 47, 55)), ContRect)
        Else
            G.FillPath(New SolidBrush(ColLow), ContRect)
        End If
        G.DrawPath(New Pen(ColHigh), ContRectHighlight)
        G.DrawPath(New Pen(ColDark), ContRect)

        Dim StateRect As New GraphicsPath
        Dim StateRectHighlight As New GraphicsPath
        Dim StateGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColMed, ColLow)
        If Not ToggleState Then
            StateRect = D.RoundRect(New Rectangle(0, 0, (Width / 2) - 1, Height - 2), 6)
            StateRectHighlight = D.RoundRect(New Rectangle(0, 1, (Width / 2) - 1, Height - 4), 6)
        Else
            StateRect = D.RoundRect(New Rectangle(Width / 2, 0, (Width / 2) - 1, Height - 2), 6)
            StateRectHighlight = D.RoundRect(New Rectangle(Width / 2, 1, (Width / 2) - 1, Height - 4), 6)
        End If

        G.FillPath(StateGrad, StateRect)
        G.DrawPath(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColHigh, ColDark)), StateRectHighlight)
        G.DrawPath(New Pen(ColDark), StateRect)

        If DrawOnOffText Then
            If ToggleState Then
                D.DrawTextWithShadow(G, New Rectangle(6, 0, Width - 1, Height - 2), "On", Font, HorizontalAlignment.Left, ColHigh)
            Else
                D.DrawTextWithShadow(G, New Rectangle(0, 0, Width - 1, Height - 2), "Off", Font, HorizontalAlignment.Right, ColHigh)
            End If
        End If
    End Sub
End Class
Public Class DeimosRadioButton : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Public Property Checked As Boolean = False
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        Checked = Not Checked
        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is DeimosRadioButton Then
                DirectCast(C, DeimosRadioButton).Checked = False
                DirectCast(C, DeimosRadioButton).Invalidate()
            End If
        Next
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        Invalidate()
    End Sub
    Public Function IsChecked() As Boolean
        Return Checked
    End Function
    Public Sub SizeChangeExternal(e As Size)
        Size = e
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Size = New Size(New Point(150, 26))
        BackColor = Color.FromArgb(57, 59, 64)
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        Font = New Font("Segoe UI", 10.0F)
        ForeColor = ColHigh
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Height = 16

        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        MyBase.OnPaint(e)
        G.Clear(BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim ContGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColMed, ColLow)
        Dim ContInnerGrad As New LinearGradientBrush(New Point(0, 1), New Point(0, Height - 1), ColHigh, ColMed)

        G.FillEllipse(ContGrad, New Rectangle(0, 0, 13, 13))
        ' G.DrawEllipse(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColHigh, ColMed)), New Rectangle(0, 1, 13, 13))
        G.DrawEllipse(New Pen(ColHigh), New Rectangle(0, 1, 13, 13))
        G.DrawEllipse(New Pen(Color.FromArgb(8, 11, 15)), New Rectangle(0, 0, 13, 13))

        If Checked Then
            G.FillEllipse(ContInnerGrad, New Rectangle(1, 1, 11, 11))
            'G.DrawEllipse(Pens.Black, New Rectangle(2, 2, 9, 9))
        End If
        D.DrawTextWithShadow(G, New Rectangle(20, 0, Width - 1, Height - 2), Text, Font, HorizontalAlignment.Left, ForeColor)
    End Sub
End Class
Public Class DeimosCheckbox : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Public Property Checked As Boolean = False
    Private ColLow, ColMed, ColHigh, ColDark As Color
    Public Property DrawRigid As Boolean = False
    Protected Overrides Sub OnPaintBackground(ByVal pevent As PaintEventArgs)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        Checked = Not Checked
        Invalidate()
    End Sub
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        MyBase.OnSizeChanged(e)
        Invalidate()
    End Sub
    Public Function IsChecked() As Boolean
        Return Checked
    End Function
    Public Sub SizeChangeExternal(e As Size)
        Size = e
        Invalidate()
    End Sub
#End Region
    Sub New()
        MyBase.New()
        Size = New Size(New Point(131, 26))
        BackColor = Color.FromArgb(57, 59, 64)
        ColHigh = Color.FromArgb(96, 98, 103)
        ColMed = Color.FromArgb(57, 59, 64)
        ColLow = Color.FromArgb(35, 37, 40)
        ColDark = Color.FromArgb(22, 22, 22)
        Font = New Font("Segoe UI", 10.0F)
        ForeColor = ColHigh
        DoubleBuffered = True
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Height = 16

        Dim G As Graphics = e.Graphics
        Dim D As New DrawUtils
        MyBase.OnPaint(e)
        G.Clear(BackColor)
        If Not DrawRigid Then
            G.SmoothingMode = SmoothingMode.HighQuality
        End If
        Dim ContRect As GraphicsPath = D.RoundRect(New Rectangle(0, 0, 13, 13), 2)
        Dim ContRectHighlight As GraphicsPath = D.RoundRect(New Rectangle(0, 1, 13, 13), 2)
        Dim StateGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColMed, ColLow)
        If Checked Then
            G.FillPath(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), ColHigh, ColMed), ContRect)
        Else
            G.FillPath(StateGrad, ContRect)
        End If
        G.DrawPath(New Pen(ColHigh), ContRectHighlight)
        G.DrawPath(New Pen(ColDark), ContRect)
        D.DrawTextWithShadow(G, New Rectangle(20, 0, Width - 1, Height - 2), Text, Font, HorizontalAlignment.Left, ForeColor)
    End Sub
End Class
'|===|  Theme Utility stuff
'|===========================================================|
Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum
Class DrawUtils
    Public Sub DrawTextWithShadow(ByVal G As Graphics, ByVal ContRect As Rectangle, ByVal Text As String, ByVal TFont As Font, ByVal TAlign As HorizontalAlignment, ByVal TColor As Color)
        DrawText(G, New Rectangle(ContRect.X, ContRect.Y + 2, ContRect.Width + 1, ContRect.Height + 2), Text, TFont, TAlign, Color.Black)
        DrawText(G, ContRect, Text, TFont, TAlign, TColor)
    End Sub
    Public Sub DrawText(ByVal G As Graphics, ByVal ContRect As Rectangle, ByVal Text As String, ByVal TFont As Font, ByVal TAlign As HorizontalAlignment, ByVal TColor As Color)
        If String.IsNullOrEmpty(Text) Then Return
        Dim TextSize As Size = G.MeasureString(Text, TFont).ToSize
        Dim _Brush As SolidBrush = New SolidBrush(TColor)

        Select Case TAlign
            Case HorizontalAlignment.Left
                G.DrawString(Text, TFont, _Brush, ContRect.X, ContRect.Height \ 2 - TextSize.Height \ 2)
            Case HorizontalAlignment.Right
                G.DrawString(Text, TFont, _Brush, ContRect.Width - TextSize.Width - 5, ContRect.Height \ 2 - TextSize.Height \ 2)
            Case HorizontalAlignment.Center
                G.DrawString(Text, TFont, _Brush, ContRect.Width \ 2 - TextSize.Width \ 2 + 3, ContRect.Height \ 2 - TextSize.Height \ 2)
        End Select
    End Sub
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
    Public Function RoundedTopRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddLine(New Point(Rectangle.X + Rectangle.Width, Rectangle.Y + ArcRectangleWidth), New Point(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height))
        P.AddLine(New Point(Rectangle.X, Rectangle.Height + Rectangle.Y), New Point(Rectangle.X, Rectangle.Y + Curve))
        Return P
    End Function
End Class