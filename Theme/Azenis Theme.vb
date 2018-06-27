Imports System.Drawing.Drawing2D
'|===========================================================|
'|===|  Azenis 
'| Creator: LordPankake
'| HF Account: http://www.hackforums.net/member.php?action=profile&uid=1828119
'| Created: 8/13/2014, Last edited: 8/17/2014
'|===========================================================|
#Region "Base Classes"
Public Class ThemedControl : Inherits Control
    Public D As New DrawUtils
    Public State As MouseState = MouseState.None
    Public Pal As Palette
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
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
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
    Sub New()
        MyBase.New()
        MinimumSize = New Size(20, 20)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
        Pal = New Palette
        Pal.ColHighest = Color.FromArgb(100, 105, 110)
        Pal.ColHigh = Color.FromArgb(65, 67, 69)
        Pal.ColMed = Color.FromArgb(40, 42, 44)
        Pal.ColDim = Color.FromArgb(30, 32, 34)
        Pal.ColDark = Color.FromArgb(15, 16, 17)
        BackColor = Pal.ColDim
    End Sub
End Class
Public Class ThemedComboBox : Inherits ComboBox
    Public D As New DrawUtils
    Public State As MouseState = MouseState.None
    Public Pal As Palette
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
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
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
    Sub New()
        MyBase.New()
        MinimumSize = New Size(20, 20)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
        Pal = New Palette
        Pal.ColHighest = Color.FromArgb(100, 105, 110)
        Pal.ColHigh = Color.FromArgb(65, 67, 69)
        Pal.ColMed = Color.FromArgb(40, 42, 44)
        Pal.ColDim = Color.FromArgb(30, 32, 34)
        Pal.ColDark = Color.FromArgb(15, 16, 17)
        BackColor = Pal.ColDim
    End Sub
End Class
Public Class ThemedContainer : Inherits ContainerControl
    Public D As New DrawUtils
    Protected Drag As Boolean = True
    Public State As MouseState = MouseState.None
    Protected TopCap As Boolean = False
    Protected SizeCap As Boolean = False
    Public Pal As Palette
    Protected MouseP As Point = New Point(0, 0)
    Protected TopGrip As Integer
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
            ElseIf Drag And New Rectangle(Width - 15, Height - 15, 15, 15).Contains(e.Location) Then
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
        If Drag Then
            SizeCap = False
        End If

    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)

        If TopCap Then
            Parent.Location = MousePosition - MouseP
        End If
        If Drag And SizeCap Then
            MouseP = e.Location
            Parent.Size = New Size(MouseP)
            Invalidate()
        End If

    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Sub New()
        MyBase.New()
        MinimumSize = New Size(20, 20)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Trebuchet MS", 10.0F)
        DoubleBuffered = True
        Pal = New Palette
        Pal.ColHighest = Color.FromArgb(100, 105, 110)
        Pal.ColHigh = Color.FromArgb(65, 67, 69)
        Pal.ColMed = Color.FromArgb(40, 42, 44)
        Pal.ColDim = Color.FromArgb(30, 32, 34)
        Pal.ColDark = Color.FromArgb(15, 16, 17)
        BackColor = Pal.ColDim
    End Sub
End Class
#End Region
#Region "Theme"
Public Class AzenisForm : Inherits ThemedContainer
    Sub New()
        MyBase.New()
        MinimumSize = New Size(305, 150)
        Dock = DockStyle.Fill
        TopGrip = 30
        Font = New Font("Segoe UI", 10.0F)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        Try
            Me.ParentForm.TransparencyKey = Color.Fuchsia
            Me.ParentForm.MinimumSize = MinimumSize
            If Not Me.ParentForm.FormBorderStyle = FormBorderStyle.None Then
                Me.ParentForm.FormBorderStyle = FormBorderStyle.None
            End If
        Catch ex As Exception : End Try
        G.Clear(Me.ParentForm.TransparencyKey)

        '|====| Main shape
        Dim MainShape As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 8)
        Dim DoStuffPath As GraphicsPath = D.RoundRect(New Rectangle(5, TopGrip + 2, Width - 11, Height - TopGrip - 8), 8)
        G.FillPath(New SolidBrush(Pal.ColHighest), MainShape)
        '| Interior shading (behind all else)
        Dim ColBlend As ColorBlend = New ColorBlend(3)
        Dim BlurScale As Integer = Math.Sqrt((Width * Width) + (Height * Height)) / 10
        ColBlend.Colors = {Color.Transparent, Color.FromArgb(255, Pal.ColDim), Color.FromArgb(255, Pal.ColDim)}
        ColBlend.Positions = {0, 1 / BlurScale, 1}
        '| Looks best when square. Not sure how I would approach scaling for rectangular shapes.
        D.DrawShadowPath(G, ColBlend, MainShape)
        '| Adds a border inside the light blending on the edges
        G.DrawPath(New Pen(Pal.ColDark), DoStuffPath)
        '|====| Top bar
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim TopPath As GraphicsPath = D.RoundedTopRect(New Rectangle(0, 0, Width - 2, TopGrip + 3), 8)
        Dim TopPathLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, TopGrip + 5), Pal.ColHighest, Color.Transparent)
        Dim LineLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, TopGrip + 5), Pal.ColHighest, Color.Transparent)
        Dim LineLGBAlt As New LinearGradientBrush(New Point(0, 0), New Point(0, TopGrip + 5), Pal.ColDim, Color.Transparent)
        G.FillPath(TopPathLGB, TopPath)
        G.DrawLine(New Pen(LineLGB), New Point(9, 0), New Point(9, TopGrip + 5))
        G.DrawLine(New Pen(LineLGB), New Point(Width - 10, 0), New Point(Width - 10, TopGrip + 5))
        G.DrawLine(New Pen(Pal.ColDim), New Point(8, 0), New Point(8, TopGrip + 5))
        G.DrawLine(New Pen(Pal.ColDim), New Point(Width - 9, 0), New Point(Width - 9, TopGrip + 5))
        '|====| Top bar - Inner
        Dim BaRct As New Rectangle(15, 4, Width - 31, TopGrip * (4 / 5))
        Dim DP As GraphicsPath = D.RoundRect(BaRct, 5)
        Dim TextureBrush As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Pal.ColMed, Color.Transparent)
        G.DrawPath(New Pen(Pal.ColDark, 3), DP)
        G.FillPath(New SolidBrush(Pal.ColDark), DP)
        G.FillPath(TextureBrush, DP)
        D.FillDualGradPath(G, Pal.ColDark, Color.Transparent, BaRct, DP, GradientAlignment.Horizontal)
        G.DrawPath(New Pen(Color.FromArgb(100, Pal.ColHigh)), DP)
        '|====|  Border
        G.SmoothingMode = SmoothingMode.None
        G.DrawPath(New Pen(Pal.ColDim), MainShape)
        D.DrawTextWithShadow(G, New Rectangle(0, 0, Width - 1, TopGrip), Text, Font, HorizontalAlignment.Center, Pal.ColHighest, Pal.ColDark)
    End Sub
End Class
Public Class AzenisButton : Inherits ThemedControl
    Sub New()
        MyBase.New()
        Font = New Font("Trebuchet MS", 10.0F)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim MainShape As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2)
        Dim LGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Pal.ColHigh, Pal.ColDim)
        G.FillPath(LGB, MainShape)

        Dim TextCol, TopColor As Color
        Select Case State
            Case MouseState.None
                TopColor = Pal.ColHighest
                TextCol = Color.FromArgb(Pal.ColHighest.R + 10, Pal.ColHighest.R + 10, Pal.ColHighest.R + 10)
            Case MouseState.Over
                TopColor = Color.FromArgb(Pal.ColHighest.R + 20, Pal.ColHighest.R + 20, Pal.ColHighest.R + 20)
                G.FillPath(New SolidBrush(Color.FromArgb(10, Color.WhiteSmoke)), MainShape)
                TextCol = Color.FromArgb(Pal.ColHighest.R + 60, Pal.ColHighest.R + 60, Pal.ColHighest.R + 60)
            Case MouseState.Down
                TopColor = Pal.ColDark
                G.FillPath(New SolidBrush(Color.FromArgb(100, Pal.ColDark)), MainShape)
                TextCol = Color.FromArgb(Pal.ColHighest.R - 20, Pal.ColHighest.R - 20, Pal.ColHighest.R - 20)
        End Select
        Dim LGB2 As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), TopColor, Pal.ColDark)
        G.DrawPath(New Pen(LGB2), MainShape)
        D.DrawTextWithShadow(G, New Rectangle(0, 0, Width - 1, Height - 1), Text, Font, HorizontalAlignment.Center, TextCol, Pal.ColDark)
    End Sub
End Class
Public Class AzenisCheckbox : Inherits ThemedControl
    Public Property Checked As Boolean
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        Checked = Not Checked
    End Sub
    Sub New()
        MyBase.New()
        Font = New Font("Trebuchet MS", 10.0F)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(BackColor)
        Height = 21
        G.SmoothingMode = SmoothingMode.HighQuality
        '|====| Main Shape
        '| Fill
        Dim MainShape As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Height - 1, Height - 1), 2)
        Dim LGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Pal.ColHigh, Pal.ColDim)
        G.FillPath(LGB, MainShape)
        '| Border
        Dim TextCol As Color = Color.FromArgb(Pal.ColHighest.R + 10, Pal.ColHighest.R + 10, Pal.ColHighest.R + 10)
        Dim LGB2 As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Pal.ColHighest, Pal.ColDark)
        G.DrawPath(New Pen(LGB2), MainShape)
        '|====| Inner Shape
        '| Fill
        Dim InnerShape As GraphicsPath = D.RoundRect(New Rectangle(2, 2, Height - 5, Height - 5), 2)
        Dim LGB3 As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Pal.ColDark, Pal.ColHighest)
        G.FillPath(LGB3, InnerShape)
        '| Checked
        If Checked Then
            Dim InnerCheckShape As GraphicsPath = D.RoundRect(New Rectangle(3, 3, Height - 7, Height - 8), 2)
            G.FillPath(New SolidBrush(Color.CadetBlue), InnerCheckShape)
            Dim ColBlend As ColorBlend = New ColorBlend(2)
            ColBlend.Colors = {Color.FromArgb(255, Color.FromArgb(11, 20, 70)), Color.Transparent}
            ColBlend.Positions = {0, 1}
            D.DrawShadowPath(G, ColBlend, InnerCheckShape)
            G.DrawPath(Pens.Black, InnerCheckShape)
        End If
        D.DrawTextWithShadow(G, New Rectangle(Height + 3, 0, Width - 1, Height - 4), Text, Font, HorizontalAlignment.Left, TextCol, Pal.ColDark)
    End Sub
End Class
Public Class AzenisRadiobutton : Inherits ThemedControl
    Public Property Checked As Boolean
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        For Each Cont As Control In Parent.Controls
            If TypeOf Cont Is AzenisRadiobutton Then
                DirectCast(Cont, AzenisRadiobutton).Checked = False
                Cont.Invalidate()
            End If
        Next
        Checked = True
    End Sub
    Sub New()
        MyBase.New()
        Font = New Font("Trebuchet MS", 10.0F)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(BackColor)
        Height = 21
        G.SmoothingMode = SmoothingMode.HighQuality
        '|====| Main Shape
        '| Fill
        Dim MainShape As New Rectangle(0, 0, Height - 1, Height - 1)
        Dim LGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Pal.ColHigh, Pal.ColDim)
        G.FillEllipse(LGB, MainShape)
        '| Border
        Dim TextCol As Color = Color.FromArgb(Pal.ColHighest.R + 10, Pal.ColHighest.R + 10, Pal.ColHighest.R + 10)
        Dim LGB2 As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Pal.ColHighest, Pal.ColDark)
        G.DrawEllipse(New Pen(LGB2), MainShape)
        '|====| Inner Shape
        '| Fill
        Dim InnerShape As New Rectangle(2, 2, Height - 5, Height - 5)
        Dim LGB3 As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Pal.ColDark, Pal.ColHighest)
        G.FillEllipse(LGB3, InnerShape)
        '| Checked
        If Checked Then
            Dim InnerCheckShape As New Rectangle(3, 3, Height - 8, Height - 8)
            G.FillEllipse(New SolidBrush(Color.CadetBlue), InnerCheckShape)
            D.DrawShadowEllipse(G, Color.FromArgb(11, 20, 70), InnerCheckShape)
            G.DrawEllipse(Pens.Black, InnerCheckShape)
        End If
        D.DrawTextWithShadow(G, New Rectangle(Height + 3, 0, Width - 1, Height - 4), Text, Font, HorizontalAlignment.Left, TextCol, Pal.ColDark)
    End Sub
End Class
Public Class WhiteUIHorizontalBar : Inherits ThemedControl
    Public Property Minimum As Integer = 0
    Public Property Value As Integer = 50
    Public Property Maximum As Integer = 100
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim NewMin = Minimum - 10
            Dim NewMax = Maximum + 10
            Dim Offset As Integer = 24
            If e.X > Offset And e.X < Width - Offset Then
                Value = (NewMin + (e.X * ((NewMax - NewMin) / Width)))
            ElseIf e.X <= Offset Then
                Value = (NewMin + (Offset * ((NewMax - NewMin) / Width)))
            ElseIf e.X >= Width - Offset Then
                Value = (NewMin + ((Width - Offset) * ((NewMax - NewMin) / Width)))
            End If
            Invalidate()
        End If
    End Sub
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim MainPath As GraphicsPath = D.RoundRect(New Rectangle(2, 4, Width - 6, Height - 14), 4)
        Dim MainPathHighlight As GraphicsPath = D.RoundRect(New Rectangle(2, 4, Width - 6, Height - 14), 4)
        Dim MainPathShadow As GraphicsPath = D.RoundRect(New Rectangle(2, 6, Width - 6, Height - 14), 4)
        Dim MainPathLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 1), Color.FromArgb(20, 100, 180), Color.FromArgb(15, 70, 140))
        Dim MainPathHighlightLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(80, 80, 80), Color.FromArgb(22, 22, 22))
        '|====| Main shape
        G.FillPath(MainPathLGB, MainPath)
        G.DrawPath(New Pen(Color.FromArgb(60, Color.Black), 2), MainPathShadow)
        G.DrawPath(New Pen(MainPathHighlightLGB, 2), MainPathhighlight)

        '|====| Grip
        Dim GripX As Integer = ValueToPercentage(Value) * Width - 15
        Dim GripRect As New Rectangle(GripX, 0, 30, Height - 5)
        Dim GripPath As GraphicsPath = D.RoundRect(GripRect, 6)
        Dim GripShadowRect As New Rectangle(GripX - 2, -1, 34, Height)
        Dim GripInnerRect As New Rectangle(GripX + 2, 2, 26, Height - 7)
        Dim GripShadowPath As GraphicsPath = D.RoundRect(GripShadowRect, 6)
        Dim GripInnerPath As GraphicsPath = D.RoundRect(GripInnerRect, 6)
        Dim GripLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(80, 80, 80), Color.FromArgb(22, 22, 22))
        Dim GripLGB2 As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(22, 22, 22), Color.FromArgb(80, 80, 80))
        '| dropshadow
        Dim ColBlend As ColorBlend = New ColorBlend(2)
        ColBlend.Colors = {Color.Transparent, Color.FromArgb(70, Color.Black)}
        ColBlend.Positions = {0, 1}
        D.DrawShadowPath(G, ColBlend, GripShadowPath)
        '| drawing
        G.FillPath(GripLGB, GripPath)
        G.FillPath(GripLGB2, GripInnerPath)
        G.DrawPath(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 3), Color.FromArgb(130, 130, 130), Color.FromArgb(22, 22, 22))), GripPath)
    End Sub
    Private Function ValueToPercentage(value As Integer) As Single
        Dim min = Minimum - 10
        Dim max = Maximum + 10
        Return (value - min) / (max - min)
        'vertical:  Return 1 - (value - min) / (max - min)
    End Function
End Class
Public Class AzenisCombobox : Inherits ThemedComboBox
    Private StrtIndex As Integer = 0
    Public Property StartIndex As Integer
        Get
            Return StrtIndex
        End Get
        Set(ByVal value As Integer)
            StrtIndex = value
            Try
                MyBase.SelectedIndex = value
            Catch
            End Try
            Invalidate()
        End Set
    End Property
    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DropDownStyle = ComboBoxStyle.DropDownList
        Font = New Font("Trebuchet MS", 10.0F)
    End Sub
    Protected Sub OnItemPaint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        Dim G As Graphics = e.Graphics
        e.DrawBackground()
        Dim BorderRect As Rectangle = e.Bounds
        Try
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                G.FillRectangle(New SolidBrush(Color.FromArgb(0, 145, 235)), BorderRect)
            End If
            G.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, New SolidBrush(e.ForeColor), e.Bounds)
        Catch
        End Try
        e.DrawFocusRectangle()
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim MainShape As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2)
        Dim TextRect As GraphicsPath = D.RoundRect(New Rectangle(3, 3, Width - 21, Height - 7), 2)

        Dim LGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Pal.ColHigh, Pal.ColDim)
        G.FillPath(LGB, MainShape)
        G.FillPath(New SolidBrush(Pal.ColDim), TextRect)
        G.DrawPath(New Pen(Pal.ColDark), TextRect)
        Dim TopColor As Color = Color.FromArgb(Pal.ColHighest.R + 10, Pal.ColHighest.R + 10, Pal.ColHighest.R + 10)
        Dim LGB2 As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Pal.ColHighest, Pal.ColDark)
        G.DrawPath(New Pen(LGB2), MainShape)

        G.DrawLine(New Pen(Pal.ColDark, 3), New Point(Width - 15, 7), New Point(Width - 3, 7))
        G.DrawLine(New Pen(Pal.ColHighest, 1), New Point(Width - 15, 7), New Point(Width - 3, 7))

        G.DrawLine(New Pen(Pal.ColDark, 3), New Point(Width - 15, Height - 9), New Point(Width - 3, Height - 9))
        G.DrawLine(New Pen(Pal.ColHighest, 1), New Point(Width - 15, Height - 9), New Point(Width - 3, Height - 9))

        G.DrawLine(New Pen(Pal.ColDark, 3), New Point(Width - 15, Height - 13), New Point(Width - 3, Height - 13))
        G.DrawLine(New Pen(Pal.ColHighest, 1), New Point(Width - 15, Height - 13), New Point(Width - 3, Height - 13))

        D.DrawTextWithShadow(G, New Rectangle(5, 0, Width - 11, Height - 1), Text, Font, HorizontalAlignment.Left, Pal.ColHighest, Pal.ColDark)
    End Sub
End Class
#End Region
#Region "Theme Utility Stuff"
Public Class Palette
    Public ColHighest As Color
    Public ColHigh As Color
    Public ColMed As Color
    Public ColDim As Color
    Public ColDark As Color
End Class
Public Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum
Public Enum GradientAlignment As Byte
    Vertical = 0
    Horizontal = 1
End Enum
Public Class DrawUtils    
    Public Sub FillDualGradPath(ByVal g As Graphics, ByVal Col1 As Color, ByVal Col2 As Color, ByVal rect As Rectangle, ByVal gp As GraphicsPath, ByVal align As GradientAlignment)
        Dim stored As SmoothingMode = g.SmoothingMode
        Dim Blend As New ColorBlend
        g.SmoothingMode = SmoothingMode.HighQuality
        Select Case align
            Case GradientAlignment.Vertical
                Dim PathGradient As New LinearGradientBrush(New Point(rect.X, rect.Y), New Point(rect.X + rect.Width - 1, rect.Y), Color.Black, Color.Black)
                Blend.Positions = {0, 1 / 2, 1}
                Blend.Colors = {Col1, Col2, Col1}
                PathGradient.InterpolationColors = Blend
                g.FillPath(PathGradient, gp)
            Case GradientAlignment.Horizontal
                Dim PathGradient As New LinearGradientBrush(New Point(rect.X, rect.Y), New Point(rect.X, rect.Y + rect.Height), Color.Black, Color.Black)
                Blend.Positions = {0, 1 / 2, 1}
                Blend.Colors = {Col1, Col2, Col1}
                PathGradient.InterpolationColors = Blend
                PathGradient.RotateTransform(0)
                g.FillPath(PathGradient, gp)
        End Select
        g.SmoothingMode = stored
    End Sub
    Public Sub DrawShadowPath(ByVal G As Graphics, ByVal ColBlend As ColorBlend, ByVal Path As GraphicsPath)
        Using ShadowBrush As PathGradientBrush = New PathGradientBrush(Path)
            ShadowBrush.InterpolationColors = ColBlend
            G.FillPath(ShadowBrush, Path)
        End Using
    End Sub
    Public Sub DrawShadowEllipse(ByVal G As Graphics, ByVal col As Color, ByVal Path As Rectangle)
        Dim GPath As New GraphicsPath()
        GPath.AddEllipse(Path)
        Dim PathGradBrush As New PathGradientBrush(GPath)
        With PathGradBrush
            .CenterPoint = New PointF(Path.X + Path.Width / 2, Path.Y + Path.Height / 2)
            .CenterColor = col
            .SurroundColors = New Color() {Color.Transparent}
            .SetBlendTriangularShape(0.1F, 1.0F)
            .FocusScales = New PointF(0.0F, 0.0F)
        End With
        G.FillPath(PathGradBrush, GPath)
    End Sub
    Public Sub DrawTextWithShadow(ByVal G As Graphics, ByVal ContRect As Rectangle, ByVal Text As String, ByVal TFont As Font, ByVal TAlign As HorizontalAlignment, ByVal TColor As Color, ByVal BColor As Color)
        DrawText(G, New Rectangle(ContRect.X + 1, ContRect.Y + 2, ContRect.Width + 1, ContRect.Height + 2), Text, TFont, TAlign, BColor)
        DrawText(G, ContRect, Text, TFont, TAlign, TColor)
    End Sub
    Public Sub DrawText(ByVal G As Graphics, ByVal ContRect As Rectangle, ByVal Text As String, ByVal TFont As Font, ByVal TAlign As HorizontalAlignment, ByVal TColor As Color)
        If String.IsNullOrEmpty(Text) Then Return
        Dim TextSize As Size = G.MeasureString(Text, TFont).ToSize
        Dim CenteredY As Integer = ContRect.Height \ 2 - TextSize.Height \ 2
        Select Case TAlign
            Case HorizontalAlignment.Left
                G.DrawString(Text, TFont, New SolidBrush(TColor), ContRect.X, CenteredY)
            Case HorizontalAlignment.Right
                G.DrawString(Text, TFont, New SolidBrush(TColor), ContRect.X + ContRect.Width - TextSize.Width - 5, CenteredY)
            Case HorizontalAlignment.Center
                G.DrawString(Text, TFont, New SolidBrush(TColor), ContRect.X + 4 + ContRect.Width \ 2 - TextSize.Width \ 2, CenteredY)
        End Select
    End Sub
    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim Path As New GraphicsPath
        Dim ArcRectangleWidth As Integer = Curve * 2
        With Path
            .AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
            .AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
            .AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
            .AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
            .AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        End With
        Return Path
    End Function
    Public Function RoundedTopRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim Path As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        With Path
            .AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
            .AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
            .AddLine(New Point(Rectangle.X + Rectangle.Width, Rectangle.Y + ArcRectangleWidth), New Point(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height))
            .AddLine(New Point(Rectangle.X, Rectangle.Height + Rectangle.Y), New Point(Rectangle.X, Rectangle.Y + Curve))
        End With
        Return Path
    End Function
End Class
#End Region