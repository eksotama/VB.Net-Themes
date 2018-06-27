Imports System.Drawing.Drawing2D
'|===========================================================|
'|===|  Piano Elements
'| Creator: LordPankake
'| HF Account: http://www.hackforums.net/member.php?action=profile&uid=1828119
'| Created: 9/25/2014, Last edited: 11/2/2014
'|          Wow, school made this take forever
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
        MinimumSize = New Size(20, 19)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
        Pal = New Palette
        Pal.ColHighest = Color.FromArgb(150, 150, 150)
        Pal.ColHigh = Color.FromArgb(63, 63, 63)
        Pal.ColMed = Color.FromArgb(35, 35, 35)
        Pal.ColDim = Color.FromArgb(16, 16, 16)
        Pal.ColDark = Color.FromArgb(5, 5, 5)
        BackColor = Pal.ColMed
    End Sub
End Class
Public Class ThemedTextbox : Inherits TextBox
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
        Pal.ColHighest = Color.FromArgb(150, 150, 150)
        Pal.ColHigh = Color.FromArgb(63, 63, 63)
        Pal.ColMed = Color.FromArgb(35, 35, 35)
        Pal.ColDim = Color.FromArgb(16, 16, 16)
        Pal.ColDark = Color.FromArgb(5, 5, 5)
        BackColor = Pal.ColMed
    End Sub
End Class
Public Class ThemedContainer : Inherits ContainerControl
    Public D As New DrawUtils
    Public Property Sizable As Boolean = True
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
        If Sizable And Drag And SizeCap Then
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
        Pal.ColHighest = Color.FromArgb(150, 150, 150)
        Pal.ColHigh = Color.FromArgb(63, 63, 63)
        Pal.ColMed = Color.FromArgb(35, 35, 35)
        Pal.ColDim = Color.FromArgb(16, 16, 16)
        Pal.ColDark = Color.FromArgb(5, 5, 5)
        BackColor = Pal.ColMed
    End Sub
End Class
Public Class ThemedTabControl : Inherits TabControl
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
        Pal.ColHighest = Color.FromArgb(150, 150, 150)
        Pal.ColHigh = Color.FromArgb(63, 63, 63)
        Pal.ColMed = Color.FromArgb(35, 35, 35)
        Pal.ColDim = Color.FromArgb(16, 16, 16)
        Pal.ColDark = Color.FromArgb(5, 5, 5)
        BackColor = Pal.ColMed
        Alignment = TabAlignment.Top
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
    End Sub
End Class
Public Class ThemedListControl : Inherits listbox
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
        Pal.ColHighest = Color.FromArgb(150, 150, 150)
        Pal.ColHigh = Color.FromArgb(63, 63, 63)
        Pal.ColMed = Color.FromArgb(35, 35, 35)
        Pal.ColDim = Color.FromArgb(16, 16, 16)
        Pal.ColDark = Color.FromArgb(5, 5, 5)
        BackColor = Pal.ColMed
    End Sub
End Class
#End Region

#Region "Theme"
Public Class PEForm : Inherits ThemedContainer
    Public Property TextYOffset As Integer = 0
    Public Property TextGradTopScale As Double = 1
    Public Property TextGradBottomScale As Double = 1
    Sub New()
        MyBase.New()
        MinimumSize = New Size(305, 150)
        Dock = DockStyle.Fill
        TopGrip = 70
        Font = New Font("Segoe UI Semibold", 35)
        BackColor = Color.FromArgb(21, 23, 25)
        ForeColor = Color.FromArgb(160, Color.White)
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

        '| Main/basic body
        Dim FormRoundness As Integer = 8
        Dim MainRect As New Rectangle(0, 0, Width, Height)
        Dim MainRectBorder As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim MainPath As GraphicsPath = D.RoundRect(MainRect, FormRoundness)
        Dim MainPathBorder As GraphicsPath = D.RoundRect(MainRectBorder, FormRoundness)
        G.FillPath(New SolidBrush(Pal.ColMed), MainPath)
        G.DrawPath(New Pen(Pal.ColMed), MainPathBorder)
        G.SmoothingMode = SmoothingMode.HighQuality
        G.DrawPath(New Pen(Color.FromArgb(20, Pal.ColHighest)), D.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), FormRoundness))
        '|=========| Top grip area |START|
        Dim CenterOffset As Integer = Width / 3
        Dim PathObjHeight As Integer = TopGrip / 3
        Dim TexturePath As New GraphicsPath
        Dim PathCutoffScaler As Double = 2.15
        With TexturePath
            .AddPolygon({
                        New Point(-2, PathObjHeight),
                        New Point(Width, PathObjHeight),
                        New Point(Width, PathObjHeight * PathCutoffScaler),
                        New Point((Width / 2) + CenterOffset, PathObjHeight * PathCutoffScaler),
                        New Point((Width / 2) + CenterOffset - 10, PathObjHeight * 3),
                        New Point((Width / 2) - CenterOffset + 10, PathObjHeight * 3),
                        New Point((Width / 2) - CenterOffset, PathObjHeight * PathCutoffScaler),
                        New Point(-2, PathObjHeight * PathCutoffScaler)})
        End With
        G.FillPath(New SolidBrush(Pal.ColDim), TexturePath)
        '| Shine behind the textures
        Dim CircleRect As New Rectangle((Width / 2) - CenterOffset * (2 / 3), PathObjHeight, (CenterOffset * (2 / 3)) * 2, PathObjHeight * 2)
        Dim CirclePath As New GraphicsPath : CirclePath.AddEllipse(CircleRect)
        Dim CircleGB As New PathGradientBrush(CirclePath)
        CircleGB.CenterPoint = New PointF(Width / 2, PathObjHeight + (PathObjHeight))
        CircleGB.CenterColor = Color.FromArgb(10, Color.White)
        CircleGB.SurroundColors = New Color() {Color.Transparent}
        G.FillEllipse(CircleGB, CircleRect)
        D.FillDualGradPath(G, Color.FromArgb(20, Color.Black), Color.FromArgb(5, Color.White), New Rectangle(0, 0, Width, TopGrip), TexturePath, GradientAlignment.Vertical)
        '| The texture overlay
        Dim TexturePathHB As New TextureBrush(New Bitmap(D.CodeToImage(D.Texture)))
        G.FillPath(TexturePathHB, TexturePath)
        G.DrawPath(New Pen(Color.FromArgb(Pal.ColDark.R + 10, Pal.ColDark.R + 10, Pal.ColDark.R + 13)), TexturePath)
        'G.DrawPath(New Pen(Color.Red, 5), TextPath)
        '|=========| Top grip area |END|
        '|=========| Top grip shines |START|
        Dim ShineLGBMid As LinearGradientBrush
        Dim ShineLGBMidShadow As LinearGradientBrush
        Dim ColBlend As New ColorBlend()
        '| Top Middle Shine
        ShineLGBMid = New LinearGradientBrush(New Point(0, 0), New Point(Width, 0), Color.Transparent, Color.Transparent)
        ShineLGBMidShadow = New LinearGradientBrush(New Point(0, 0), New Point(Width, 0), Color.Transparent, Color.Transparent)
        ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(100, Pal.ColHighest), Color.Transparent} : ShineLGBMid.InterpolationColors = ColBlend
        ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.FromArgb(25, Pal.ColDark), Color.FromArgb(200, Pal.ColDark), Color.FromArgb(25, Pal.ColDark)} : ShineLGBMidShadow.InterpolationColors = ColBlend
        G.DrawLine(New Pen(ShineLGBMid), New Point(0, PathObjHeight - 1), New Point(Width, PathObjHeight - 1))
        G.DrawLine(New Pen(ShineLGBMidShadow), New Point(0, PathObjHeight), New Point(Width, PathObjHeight))
        '| Bottom middle shine
        ShineLGBMid = New LinearGradientBrush(New Point((Width / 2) - CenterOffset, 0), New Point((Width / 2) + CenterOffset - 10, 0), Color.Transparent, Color.Transparent)
        ShineLGBMidShadow = New LinearGradientBrush(New Point((Width / 2) - CenterOffset, 0), New Point((Width / 2) + CenterOffset - 10, 0), Color.Transparent, Color.Transparent)
        ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(150, Pal.ColHighest), Color.Transparent} : ShineLGBMid.InterpolationColors = ColBlend
        ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.FromArgb(25, Pal.ColDark), Color.FromArgb(200, Pal.ColDark), Color.FromArgb(25, Pal.ColDark)} : ShineLGBMidShadow.InterpolationColors = ColBlend
        G.DrawLine(New Pen(ShineLGBMid), New Point((Width / 2) - CenterOffset + 10, PathObjHeight * 3 + 1), New Point((Width / 2) + CenterOffset - 10, PathObjHeight * 3 + 1))
        G.DrawLine(New Pen(ShineLGBMidShadow), New Point((Width / 2) - CenterOffset + 10, PathObjHeight * 3 + 2), New Point((Width / 2) + CenterOffset - 10, PathObjHeight * 3 + 2))
        '| Left shine
        ShineLGBMid = New LinearGradientBrush(New Point(0, 0), New Point((Width / 2) - CenterOffset, 0), Color.Transparent, Color.Transparent)
        ShineLGBMidShadow = New LinearGradientBrush(New Point(0, 0), New Point((Width / 2) - CenterOffset, 0), Color.Transparent, Color.Transparent)
        ColBlend.Positions = {0, 9 / 10, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(85, Pal.ColHighest), Color.Transparent} : ShineLGBMid.InterpolationColors = ColBlend
        ColBlend.Positions = {0, 9 / 10, 1} : ColBlend.Colors = {Color.FromArgb(25, Pal.ColDark), Color.FromArgb(110, Pal.ColDark), Color.FromArgb(25, Pal.ColDark)} : ShineLGBMidShadow.InterpolationColors = ColBlend
        G.DrawLine(New Pen(ShineLGBMid), New Point(0, PathObjHeight * PathCutoffScaler + 1), New Point((Width / 2) - CenterOffset, PathObjHeight * PathCutoffScaler + 1))
        G.DrawLine(New Pen(ShineLGBMidShadow), New Point(0, PathObjHeight * PathCutoffScaler + 2), New Point((Width / 2) - CenterOffset, PathObjHeight * PathCutoffScaler + 2))
        '| Right shine
        ShineLGBMid = New LinearGradientBrush(New Point((Width / 2) + CenterOffset, 0), New Point(Width - 1, 0), Color.Transparent, Color.Transparent)
        ShineLGBMidShadow = New LinearGradientBrush(New Point((Width / 2) + CenterOffset, 0), New Point(Width - 1, 0), Color.Transparent, Color.Transparent)
        ColBlend.Positions = {0, 1 / 10, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(85, Pal.ColHighest), Color.Transparent} : ShineLGBMid.InterpolationColors = ColBlend
        ColBlend.Positions = {0, 1 / 10, 1} : ColBlend.Colors = {Color.FromArgb(25, Pal.ColDark), Color.FromArgb(110, Pal.ColDark), Color.FromArgb(25, Pal.ColDark)} : ShineLGBMidShadow.InterpolationColors = ColBlend
        G.DrawLine(New Pen(ShineLGBMid), New Point((Width / 2) + CenterOffset, PathObjHeight * PathCutoffScaler + 1), New Point(Width - 1, PathObjHeight * PathCutoffScaler + 1))
        G.DrawLine(New Pen(ShineLGBMidShadow), New Point((Width / 2) + CenterOffset, PathObjHeight * PathCutoffScaler + 2), New Point(Width - 1, PathObjHeight * PathCutoffScaler + 2))
        '|=========| Top grip shines |END|
        '|=========| Text rendering |START|
        Dim TextPath As New GraphicsPath
        Dim StrFormat As New StringFormat
        StrFormat.LineAlignment = StringAlignment.Center
        StrFormat.Alignment = StringAlignment.Center
        Dim TextSize As Size = G.MeasureString(Text, Font).ToSize
        With TextPath
            .AddString(Text, Font.FontFamily, Font.Style, Font.Size, New Point((Width / 2), PathObjHeight * 2 + TextYOffset), StrFormat)
        End With
        G.DrawPath(New Pen(Color.FromArgb(100, 100, 255, 0), 5), TextPath)
        For i As Integer = 5 To 1 Step -1
            G.DrawPath(New Pen(Color.FromArgb(90, Color.Black), i), TextPath)
        Next
        Dim TextPathLGB As New LinearGradientBrush(New Point(0, ((PathObjHeight * 2) - (PathObjHeight / 2) - TextYOffset) * TextGradTopScale), New Point(0, ((PathObjHeight * 2) + (PathObjHeight / 2) + 3 + TextYOffset) * TextGradBottomScale), Color.FromArgb(180, 255, 20), Color.FromArgb(60, 180, 20))
        G.FillPath(TextPathLGB, TextPath)
        '|=========| Text rendering |STOP|
        '|=========| Border
        G.SmoothingMode = SmoothingMode.None
        G.DrawPath(New Pen(Color.FromArgb(100, Pal.ColDark)), MainPathBorder)
    End Sub
End Class
Public Class PEControlButton : Inherits ThemedControl
    Public Property ButtonColor As Color = Nothing
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI", 11.0F)
        MinimumSize = New Size(11, 11)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.BackColor)
        Size = New Size(16, 16)

        Dim Image As Bitmap = D.CodeToImage(D.Texture2)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim ColorRec2t As New Rectangle(0, 0, Image.Width - 1, Image.Height - 1)
        G.DrawEllipse(New Pen(Color.FromArgb(60, Color.Black)), ColorRec2t)

        If Not IsNothing(ButtonColor) Then
            Dim ColorRect As New Rectangle(1, 1, Image.Width - 3, Image.Height - 3)
            Select Case State
                Case MouseState.None
                    G.FillEllipse(New SolidBrush(Color.FromArgb(100, ButtonColor)), ColorRect)
                Case MouseState.Over
                    G.FillEllipse(New SolidBrush(Color.FromArgb(150, ButtonColor)), ColorRect)
                Case MouseState.Down
                    G.FillEllipse(New SolidBrush(Color.FromArgb(50, ButtonColor)), ColorRect)
            End Select
        End If

        G.DrawImage(Image, New Point(0, 0))
    End Sub
End Class
Public Class PEButtonAlt : Inherits ThemedControl
    Public Property Rounded As Boolean = True
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI", 10.0F)
        ForeColor = Pal.ColHighest
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.BackColor)

        Dim Roundness As Integer = 5
        Dim FillRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim HighlightRect As New Rectangle(1, 1, Width - 2, Height - 2)
        Dim ShadowRect As New Rectangle(1, 1, Width - 3, Height - 3)
        Dim FillLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(Pal.ColMed.R + 10, Pal.ColMed.R + 10, Pal.ColMed.R + 10), Color.FromArgb(Pal.ColMed.R - 10, Pal.ColMed.R - 10, Pal.ColMed.R - 10))
        If Rounded Then
            Dim FillPath As GraphicsPath = D.RoundRect(FillRect, Roundness)
            Dim HighlightPath As GraphicsPath = D.RoundRect(HighlightRect, Roundness)
            Dim ShadowPath As GraphicsPath = D.RoundRect(ShadowRect, Roundness)
            G.SmoothingMode = SmoothingMode.HighQuality
            G.FillPath(FillLGB, FillPath)
     
            G.DrawPath(New Pen(Pal.ColDim), ShadowPath)
            G.DrawPath(New Pen(Color.FromArgb(160, Pal.ColHigh)), HighlightPath)
            G.DrawPath(New Pen(Pal.ColDark), FillPath)
            '| Shine
            Dim ColBlend As New ColorBlend()
            Dim ShineLGBMid As New LinearGradientBrush(New Point(Roundness, 0), New Point(Width - Roundness - 1, 0), Color.Transparent, Color.Transparent)
            Dim ShineLGBMid2 As New LinearGradientBrush(New Point(Roundness, 0), New Point(Width - Roundness - 1, 0), Color.Transparent, Color.Transparent)
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(150, Pal.ColHighest), Color.Transparent} : ShineLGBMid.InterpolationColors = ColBlend
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(150, Pal.ColDark), Color.Transparent} : ShineLGBMid2.InterpolationColors = ColBlend
            G.DrawLine(New Pen(ShineLGBMid), New Point(Roundness, 1), New Point(Width - Roundness - 1, 1))
            G.DrawLine(New Pen(ShineLGBMid2), New Point(Roundness, 2), New Point(Width - Roundness - 1, 2))
            If State = MouseState.Over Then
                G.FillPath(New SolidBrush(Color.FromArgb(13, Color.WhiteSmoke)), FillPath)
            ElseIf State = MouseState.Down Then
                G.FillPath(New SolidBrush(Color.FromArgb(76, Color.Black)), FillPath)
            End If
        Else
            G.FillRectangle(FillLGB, FillRect)
            G.DrawRectangle(New Pen(Pal.ColDim), ShadowRect)
            G.DrawRectangle(New Pen(Color.FromArgb(160, Pal.ColHigh)), HighlightRect)
            G.DrawRectangle(New Pen(Pal.ColDark), FillRect)
            '| Shine
            Dim ColBlend As New ColorBlend()
            Dim ShineLGBMid As New LinearGradientBrush(New Point(1, 0), New Point(Width - 3, 0), Color.Transparent, Color.Transparent)
            Dim ShineLGBMid2 As New LinearGradientBrush(New Point(1, 0), New Point(Width - 3, 0), Color.Transparent, Color.Transparent)
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(150, Pal.ColHighest), Color.Transparent} : ShineLGBMid.InterpolationColors = ColBlend
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(150, Pal.ColDark), Color.Transparent} : ShineLGBMid2.InterpolationColors = ColBlend
            G.DrawLine(New Pen(ShineLGBMid), New Point(1, 1), New Point(Width - 3, 1))
            G.DrawLine(New Pen(ShineLGBMid2), New Point(1, 2), New Point(Width - 3, 2))
            If State = MouseState.Over Then
                G.FillRectangle(New SolidBrush(Color.FromArgb(13, Color.WhiteSmoke)), FillRect)
            ElseIf State = MouseState.Down Then
                G.FillRectangle(New SolidBrush(Color.FromArgb(76, Color.Black)), FillRect)
            End If
        End If
        D.DrawText(G, FillRect, Text, Font, HorizontalAlignment.Center, ForeColor)
    End Sub
End Class
Public Class PEProgressBar : Inherits ThemedControl
    Private PValue As Integer = 50
    Public Property Rounded As Boolean = True
    Public Property DisplayMode As PEProgressBar.Type = Type.Ticked
    Public Property Value() As Integer
        Get
            Return PValue
        End Get
        Set(ByVal value As Integer)
            PValue = value
            Invalidate()
        End Set
    End Property
    Public Property Minimum As Integer = 0
    Public Property Maximum As Integer = 100
    Public Property TickWidth = 5
    Public Property TickStart = 2
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI", 11.0F)
        Height = 24

    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.BackColor)
        Dim blackness As Integer = 100
        Dim roundness As Integer = 3
        If Rounded Then
            G.SmoothingMode = SmoothingMode.HighQuality
        End If
        If DisplayMode = Type.Full Then
            Dim MainRect As New Rectangle(0, 0, Width - 1, Height - 1)
            Dim Value As Integer = ValueToPercentage(PValue) * Width - 1
            Dim ValueRect As New Rectangle(0, 0, Value, Height - 1)
            If Value > 0 Then
                If Rounded Then
                    G.FillPath(New SolidBrush(Color.FromArgb(blackness, Color.Black)), D.RoundRect(MainRect, roundness))
                    G.FillPath(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(200, 255, 2), Color.FromArgb(121, 172, 2)), D.RoundRect(ValueRect, roundness))
                    G.DrawPath(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(111, Color.Black), Color.FromArgb(111, Color.Black))), D.RoundRect(New Rectangle(0, 1, Value, Height - 3), 1))
                Else
                    G.FillRectangle(New SolidBrush(Color.FromArgb(30, Color.Black)), MainRect)
                    G.FillRectangle(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(200, 255, 2), Color.FromArgb(121, 172, 2)), ValueRect)
                End If
            End If
            If Rounded Then
                G.DrawPath(New Pen(Pal.ColDark), D.RoundRect(MainRect, roundness))
            Else
                G.DrawRectangle(New Pen(Pal.ColDark), MainRect)
            End If
        ElseIf DisplayMode = Type.Ticked Then
            Dim MainRect As New Rectangle(0, 0, Width - 1, Height - 6)
            Dim Value As Integer = ValueToPercentage(PValue) * Width - 1
            Dim ValueRect As New Rectangle(0, 0, Value, Height - 6)
            If Value > 0 Then
                If Rounded Then
                    G.FillPath(New SolidBrush(Color.FromArgb(blackness, Color.Black)), D.RoundRect(MainRect, roundness))
                    G.FillPath(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(200, 255, 2), Color.FromArgb(121, 172, 2)), D.RoundRect(ValueRect, roundness))
                    G.DrawPath(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(111, Color.Black), Color.FromArgb(111, Color.Black))), D.RoundRect(New Rectangle(0, 1, Value, Height - 8), 1))
                Else
                    G.FillRectangle(New SolidBrush(Color.FromArgb(30, Color.Black)), MainRect)
                    G.FillRectangle(New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 6), Color.FromArgb(200, 255, 2), Color.FromArgb(121, 172, 2)), ValueRect)
                End If
            End If
            If Rounded Then
                G.DrawPath(New Pen(Pal.ColDark), D.RoundRect(MainRect, roundness))
            Else
                G.DrawRectangle(New Pen(Pal.ColDark), MainRect)
            End If

            For i As Integer = TickStart To Width Step TickWidth
                G.DrawLine(New Pen(Color.FromArgb(100, Color.Gray)), New Point(i, Height - 5), New Point(i, Height - 1))
            Next
        End If


    End Sub
    Private Function ValueToPercentage(val As Integer) As Single
        Dim min = Minimum
        Dim max = Maximum
        Return (val - min) / (max - min)
    End Function

    Enum Type
        Full
        Ticked
    End Enum
End Class
Public Class PECheckbox : Inherits ThemedControl
    Public Property Checked As Boolean
    Public Property Rounded As Boolean = True

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        Checked = Not Checked
    End Sub
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI", 10.0F)
        Height = 20
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.BackColor)
        Dim CheckedGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(200, 255, 2), Color.FromArgb(121, 172, 2))
        Dim CheckedGradOutline As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(9, 16, 1), Color.FromArgb(1, 3, 0))
        If Rounded Then
            G.SmoothingMode = SmoothingMode.HighQuality
            Dim MainPath As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Height - 2, Height - 2), 3)
            Dim BorderPath As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Height - 1, Height - 1), 3)
            G.DrawPath(New Pen(Color.FromArgb(160, Pal.ColHigh)), BorderPath)
            G.DrawPath(New Pen(Color.Black), MainPath)
            If Checked Then
                G.FillPath(CheckedGrad, MainPath)
                G.DrawPath(New Pen(CheckedGradOutline), BorderPath)
            Else
                G.FillPath(New SolidBrush(Color.FromArgb(100, Color.Black)), MainPath)
            End If
        Else
            G.DrawRectangle(New Pen(Color.FromArgb(160, Pal.ColHigh)), New Rectangle(0, 0, Height - 1, Height - 1))
            G.DrawRectangle(New Pen(Pal.ColDark), New Rectangle(0, 0, Height - 2, Height - 2))
            If Checked Then
                Dim RectShadeBrush As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(100, 9, 16, 1), Color.FromArgb(100, 1, 3, 0))
                G.FillRectangle(CheckedGrad, New Rectangle(0, 0, Height - 1, Height - 1))
                G.DrawRectangle(New Pen(CheckedGradOutline), New Rectangle(0, 0, Height - 1, Height - 1))
                G.DrawRectangle(New Pen(RectShadeBrush), New Rectangle(0, 0, Height - 2, Height - 2))
            Else
                G.FillRectangle(New SolidBrush(Color.FromArgb(100, Color.Black)), New Rectangle(0, 0, Height - 2, Height - 2))
            End If
        End If
        D.DrawTextWithShadow(G, New Rectangle(Height, 0, Width, Height - 1), Text, Font, HorizontalAlignment.Left, Color.FromArgb(155, 155, 160), Color.Black)
    End Sub
End Class
Public Class PERadiobutton : Inherits ThemedControl
    Public Property Checked As Boolean
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        For Each Cont As Control In Parent.Controls
            If TypeOf Cont Is PERadiobutton Then
                DirectCast(Cont, PERadiobutton).Checked = False
                Cont.Invalidate()
            End If
        Next
        Checked = True
    End Sub
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI", 10.0F)
        Height = 20
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.BackColor)

        Dim CheckedGrad As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(200, 255, 2), Color.FromArgb(121, 172, 2))
        Dim CheckedGradOutline As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(9, 16, 1), Color.FromArgb(1, 3, 0))

        G.SmoothingMode = SmoothingMode.HighQuality
        G.DrawEllipse(New Pen(Color.FromArgb(160, Pal.ColHigh)), New Rectangle(0, 0, Height - 1, Height - 1))
        G.DrawEllipse(New Pen(Pal.ColDark), New Rectangle(0, 0, Height - 2, Height - 2))
        If Checked Then
            Dim RectShadeBrush As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(100, 9, 16, 1), Color.FromArgb(100, 1, 3, 0))
            G.FillEllipse(CheckedGrad, New Rectangle(0, 0, Height - 1, Height - 1))
            G.DrawEllipse(New Pen(CheckedGradOutline), New Rectangle(0, 0, Height - 1, Height - 1))
            G.DrawEllipse(New Pen(RectShadeBrush), New Rectangle(0, 0, Height - 2, Height - 2))
        Else
            G.FillEllipse(New SolidBrush(Color.FromArgb(100, Color.Black)), New Rectangle(0, 0, Height - 2, Height - 2))
        End If
        D.DrawTextWithShadow(G, New Rectangle(Height, 0, Width, Height - 1), Text, Font, HorizontalAlignment.Left, Color.FromArgb(155, 155, 160), Color.Black)

    End Sub
End Class
Public Class PEGroupboxEmbossed : Inherits ThemedContainer
    Public Property Rounded As Boolean = True
    Sub New()
        MyBase.New()
        MinimumSize = New Size(10, 10)
        TopGrip = 20
        Font = New Font("Segoe UI", 10.0F)
        BackColor = Pal.ColMed
        ForeColor = Color.FromArgb(160, Color.White)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        If Rounded Then
            Dim OuterPath As GraphicsPath = D.RoundRect(New Rectangle(1, 1, Width - 2, Height - 2), 5)
            Dim OuterPathHighlight As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 5)
            Dim OuterPathShadow As GraphicsPath = D.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), 5)
            Dim InnerPath As GraphicsPath = D.RoundRect(New Rectangle(9, 9, Width - 19, Height - 19), 5)
            Dim InnerPathHighlight As GraphicsPath = D.RoundRect(New Rectangle(9, 9, Width - 18, Height - 18), 5)
            Dim InnerPathShadow As GraphicsPath = D.RoundRect(New Rectangle(10, 10, Width - 20, Height - 20), 5)
            Dim Hat As New HatchBrush(HatchStyle.Percent30, Color.FromArgb(100, Pal.ColDim), Color.Transparent)
            '|Fills
            G.FillPath(New SolidBrush(Pal.ColMed), OuterPath)
            G.FillPath(Hat, OuterPath)
            G.FillPath(New SolidBrush(Pal.ColDim), InnerPath)
            '| Borders
            G.DrawPath(New Pen(Color.FromArgb(100, Pal.ColHigh)), OuterPathHighlight)
            G.DrawPath(New Pen(Color.FromArgb(200, Pal.ColDark)), OuterPath)
            G.DrawPath(New Pen(Color.FromArgb(100, Pal.ColHigh)), InnerPathHighlight)
            G.DrawPath(New Pen(Color.FromArgb(100, Pal.ColDark)), InnerPathShadow)
            G.DrawPath(New Pen(Color.FromArgb(200, Pal.ColDark)), InnerPath)
        Else
            Dim OuterRect As Rectangle = New Rectangle(1, 1, Width - 2, Height - 2)
            Dim OuterRectHighlight As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
            Dim OuterRectShadow As Rectangle = New Rectangle(1, 1, Width - 3, Height - 3)
            Dim InnerRect As Rectangle = New Rectangle(9, 9, Width - 19, Height - 19)
            Dim InnerRectHighlight As Rectangle = New Rectangle(9, 9, Width - 18, Height - 18)
            Dim InnerRectShadow As Rectangle = New Rectangle(10, 10, Width - 20, Height - 20)
            Dim Hat As New HatchBrush(HatchStyle.Percent30, Color.FromArgb(100, Pal.ColDim), Color.Transparent)
            '|Fills
            G.FillRectangle(New SolidBrush(Pal.ColMed), OuterRect)
            G.FillRectangle(Hat, OuterRect)
            G.FillRectangle(New SolidBrush(Pal.ColDim), InnerRect)
            '| Borders
            G.DrawRectangle(New Pen(Color.FromArgb(100, Pal.ColHigh)), OuterRectHighlight)
            G.DrawRectangle(New Pen(Color.FromArgb(200, Pal.ColDark)), OuterRect)
            G.DrawRectangle(New Pen(Color.FromArgb(100, Pal.ColHigh)), InnerRectHighlight)
            G.DrawRectangle(New Pen(Color.FromArgb(100, Pal.ColDark)), InnerRectShadow)
            G.DrawRectangle(New Pen(Color.FromArgb(200, Pal.ColDark)), InnerRect)
        End If

    End Sub
End Class
Public Class PEGroupboxEngraved : Inherits ThemedContainer
    Public Property Rounded As Boolean = True
    Sub New()
        MyBase.New()
        MinimumSize = New Size(10, 10)
        TopGrip = 20
        Font = New Font("Segoe UI", 10.0F)
        BackColor = Pal.ColMed
        ForeColor = Color.FromArgb(160, Color.White)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        If Rounded Then
            Dim TexturePathHB As New TextureBrush(New Bitmap(D.CodeToImage(D.Texture)))
            Dim OuterRect1 As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 2, Height - 2), 5)
            Dim OuterRect2 As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 5)
            '|=========| OUTER |START|
            G.FillPath(New SolidBrush(Pal.ColDim), OuterRect1)
            D.FillGradientBeam(G, Color.FromArgb(27, Color.Black), Color.FromArgb(8, Color.White), New Rectangle(0, 0, Width, Height), GradientAlignment.Vertical)
            G.FillPath(TexturePathHB, OuterRect1)
            G.DrawPath(New Pen(Color.FromArgb(160, Pal.ColHigh)), OuterRect2)
            G.DrawPath(New Pen(Pal.ColDark), OuterRect1)
            '| Shine
            Dim ShineLGBTop As New LinearGradientBrush(New Point(5, 0), New Point(Width - 5, 0), Color.Transparent, Color.Transparent)
            Dim ColBlend As New ColorBlend()
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(75, Pal.ColHighest), Color.Transparent} : ShineLGBTop.InterpolationColors = ColBlend
            G.DrawLine(New Pen(ShineLGBTop), New Point(5, Height - 1), New Point(Width - 5, Height - 1))
            '|=========| Inner |END|
            '|=========| Inner |START|
            Dim InnerRect1 As GraphicsPath = D.RoundRect(New Rectangle(9, 9, Width - 19, Height - 19), 5)
            Dim InnerRect2 As GraphicsPath = D.RoundRect(New Rectangle(10, 10, Width - 20, Height - 20), 5)
            Dim InnerRect3 As GraphicsPath = D.RoundRect(New Rectangle(10, 10, Width - 21, Height - 21), 5)
            G.FillPath(New SolidBrush(Pal.ColMed), InnerRect1)
            G.DrawPath(New Pen(Pal.ColDim), InnerRect3)
            G.DrawPath(New Pen(Color.FromArgb(160, Pal.ColHigh)), InnerRect2)
            G.DrawPath(New Pen(Pal.ColDark), InnerRect1)
            '| Shine
            Dim ShineLGBMid As New LinearGradientBrush(New Point(14, 0), New Point(Width - 29, 0), Color.Transparent, Color.Transparent)
            Dim ShineLGBMid2 As New LinearGradientBrush(New Point(14, 0), New Point(Width - 29, 0), Color.Transparent, Color.Transparent)
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(150, Pal.ColHighest), Color.Transparent} : ShineLGBMid.InterpolationColors = ColBlend
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(150, Pal.ColDark), Color.Transparent} : ShineLGBMid2.InterpolationColors = ColBlend
            G.DrawLine(New Pen(ShineLGBMid), New Point(14, 10), New Point(Width - 29, 10))
            G.DrawLine(New Pen(ShineLGBMid2), New Point(14, 11), New Point(Width - 29, 11))
            '|=========| Inner |END|
        Else
            Dim TexturePathHB As New TextureBrush(New Bitmap(D.CodeToImage(D.Texture)))
            Dim OuterRect1 As New Rectangle(0, 0, Width - 2, Height - 2)
            Dim OuterRect2 As New Rectangle(0, 0, Width - 1, Height - 1)
            '|=========| OUTER |START|
            G.FillRectangle(New SolidBrush(Pal.ColDim), OuterRect1)
            D.FillGradientBeam(G, Color.FromArgb(27, Color.Black), Color.FromArgb(8, Color.White), New Rectangle(0, 0, Width, Height), GradientAlignment.Vertical)
            G.FillRectangle(TexturePathHB, OuterRect1)
            G.DrawRectangle(New Pen(Color.FromArgb(160, Pal.ColHigh)), OuterRect2)
            G.DrawRectangle(New Pen(Pal.ColDark), OuterRect1)
            '| Shine
            Dim ShineLGBTop As New LinearGradientBrush(New Point(0, 0), New Point(Width, 0), Color.Transparent, Color.Transparent)
            Dim ColBlend As New ColorBlend()
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(75, Pal.ColHighest), Color.Transparent} : ShineLGBTop.InterpolationColors = ColBlend
            G.DrawLine(New Pen(ShineLGBTop), New Point(10, Height - 1), New Point(Width - 1, Height - 1))
            '|=========| Inner |END|
            '|=========| Inner |START|
            Dim InnerRect1 As New Rectangle(9, 9, Width - 19, Height - 19)
            Dim InnerRect2 As New Rectangle(10, 10, Width - 20, Height - 20)
            Dim InnerRect3 As New Rectangle(10, 10, Width - 21, Height - 21)
            G.FillRectangle(New SolidBrush(Pal.ColMed), InnerRect1)
            G.DrawRectangle(New Pen(Pal.ColDim), InnerRect3)
            G.DrawRectangle(New Pen(Color.FromArgb(160, Pal.ColHigh)), InnerRect2)
            G.DrawRectangle(New Pen(Pal.ColDark), InnerRect1)
            '| Shine
            Dim ShineLGBMid As New LinearGradientBrush(New Point(9, 0), New Point(Width - 20, 0), Color.Transparent, Color.Transparent)
            Dim ShineLGBMid2 As New LinearGradientBrush(New Point(9, 0), New Point(Width - 20, 0), Color.Transparent, Color.Transparent)
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(150, Pal.ColHighest), Color.Transparent} : ShineLGBMid.InterpolationColors = ColBlend
            ColBlend.Positions = {0, 1 / 2, 1} : ColBlend.Colors = {Color.Transparent, Color.FromArgb(150, Pal.ColDark), Color.Transparent} : ShineLGBMid2.InterpolationColors = ColBlend
            G.DrawLine(New Pen(ShineLGBMid), New Point(10, 10), New Point(Width - 20, 10))
            G.DrawLine(New Pen(ShineLGBMid2), New Point(10, 11), New Point(Width - 20, 11))
            '|=========| Inner |END|
        End If

    End Sub
   
End Class
Public Class PEGroupboxEngraved_Simple : Inherits ThemedContainer
    Public Property Rounded As Boolean = True
    Public Property FillColor As Color = Pal.ColDim
    Sub New()
        MyBase.New()
        MinimumSize = New Size(10, 10)
        TopGrip = 20
        Font = New Font("Segoe UI", 10.0F)
        BackColor = Pal.ColMed
        ForeColor = Color.FromArgb(160, Color.White)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
         If Rounded Then
            Dim OuterRect1 As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 2, Height - 2), 5)
            Dim OuterRect2 As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 5)
            '|=========| OUTER |START|
            G.FillPath(New SolidBrush(FillColor), OuterRect1)
            G.DrawPath(New Pen(Color.FromArgb(160, Pal.ColHigh)), OuterRect2)
            G.DrawPath(New Pen(Pal.ColDark), OuterRect1)
        Else
            G.FillRectangle(New SolidBrush(FillColor), New Rectangle(0, 0, Width - 2, Height - 2))
            G.DrawRectangle(New Pen(Color.FromArgb(160, Pal.ColHigh)), New Rectangle(0, 0, Width - 1, Height - 1))
            G.DrawRectangle(New Pen(Pal.ColDark), New Rectangle(0, 0, Width - 2, Height - 2))
        End If

    End Sub
End Class
Public Class PETextbox : Inherits ThemedTextbox
    Public Property Rounded As Boolean = True
    Property IsMultiline As Boolean
        Get
            Return Multiline
        End Get
        Set(ByVal value As Boolean)
            Multiline = value
            Invalidate()
        End Set
    End Property
    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        BorderStyle = Windows.Forms.BorderStyle.None
        Multiline = IsMultiline
        ForeColor = Pal.ColHighest
        Font = New Font("Segoe UI", 10.0F)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        If Not IsMultiline Then
            ' Height = 25
        End If

        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality
        If Rounded Then
            Dim OuterRect1 As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 2, Height - 2), 5)
            Dim OuterRect2 As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 5)
            '|=========| OUTER |START|
            G.FillPath(New SolidBrush(Color.FromArgb(21, 21, 21)), OuterRect1)
            G.DrawPath(New Pen(Color.FromArgb(160, Pal.ColHigh)), OuterRect2)
            G.DrawPath(New Pen(Pal.ColDark), OuterRect1)
        Else
            G.FillRectangle(New SolidBrush(Color.FromArgb(21, 21, 21)), New Rectangle(0, 0, Width - 2, Height - 2))
            G.DrawRectangle(New Pen(Color.FromArgb(160, Pal.ColHigh)), New Rectangle(0, 0, Width - 1, Height - 1))
            G.DrawRectangle(New Pen(Pal.ColDark), New Rectangle(0, 0, Width - 2, Height - 2))
        End If
        If Multiline Then
            Dim ccount As Integer = CountCharacter(Text, CChar(vbNewLine))
            D.DrawTextWithShadow(G, New Rectangle(3, 0, Width, Height - 1), Text, Font, Me.TextAlign, Color.FromArgb(155, 155, 160), Color.Black)
        Else
            D.DrawTextWithShadow(G, New Rectangle(3, 0, Width, Height - 1), Text, Font, Me.TextAlign, Color.FromArgb(155, 155, 160), Color.Black)
        End If
    End Sub
    Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
        Dim cnt As Integer = 0
        For Each c As Char In value
            If c = ch Then cnt += 1
        Next
        Return cnt
    End Function
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
Public Enum ImageMode As Byte
    Normal = 0
    Scaled = 1
End Enum
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
    Public Texture As String = "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"
    Public Texture2 As String = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAPCAYAAADtc08vAAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAIGNIUk0AAHolAACAgwAA+f8AAIDpAAB1MAAA6mAAADqYAAAXb5JfxUYAAADySURBVHjapNPJSkNBEAXQUxk04kIDZuEAov6af+Vv+QcOiANk4YBPfeWmHoQXo0IKiobue2/fqq6OzLRODKwZo1/OjnCMT9zi6r8CU5zjpLd/jQs8/CVwivvKfpz1BaJrYkSAzNzAASYLuESDm4hoCrcskJlRhGF3QZHhq3D5o0CBB1XWuNYB2iI31dA2IrLjjXpPOsYWtiuHRX7DC17RZGbbOVsUiCJPMcMuNvGBOR4L15abJQdt3TrDIfaqkU3tB94rVwo8Y6fIswWBqBKeylGuGuU5LqvuSS8Td+VgeQ56MaxJ3C+xmxrltg+MdX/j9wCsYFw8P56nMgAAAABJRU5ErkJggg=="
    Public Sub FillGradientBeam(ByVal g As Graphics, ByVal Col1 As Color, ByVal Col2 As Color, ByVal rect As Rectangle, ByVal align As GradientAlignment)
        Dim stored As SmoothingMode = g.SmoothingMode
        Dim Blend As New ColorBlend
        g.SmoothingMode = SmoothingMode.HighQuality
        Select Case align
            Case GradientAlignment.Vertical
                Dim PathGradient As New LinearGradientBrush(New Point(rect.X, rect.Y), New Point(rect.X + rect.Width - 1, rect.Y), Color.Black, Color.Black)
                Blend.Positions = {0, 1 / 2, 1}
                Blend.Colors = {Col1, Col2, Col1}
                PathGradient.InterpolationColors = Blend
                g.FillRectangle(PathGradient, rect)
            Case GradientAlignment.Horizontal
                Dim PathGradient As New LinearGradientBrush(New Point(rect.X, rect.Y), New Point(rect.X, rect.Y + rect.Height), Color.Black, Color.Black)
                Blend.Positions = {0, 1 / 2, 1}
                Blend.Colors = {Col1, Col2, Col1}
                PathGradient.InterpolationColors = Blend
                PathGradient.RotateTransform(0)
                g.FillRectangle(PathGradient, rect)
        End Select
        g.SmoothingMode = stored
    End Sub  
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
    Public Sub DrawTextWithShadow(ByVal G As Graphics, ByVal ContRect As Rectangle, ByVal Text As String, ByVal TFont As Font, ByVal TAlign As HorizontalAlignment, ByVal TColor As Color, ByVal BColor As Color)
        DrawText(G, New Rectangle(ContRect.X + 1, ContRect.Y + 1, ContRect.Width, ContRect.Height), Text, TFont, TAlign, BColor)
        DrawText(G, ContRect, Text, TFont, TAlign, TColor)
    End Sub
    Public Sub DrawText(ByVal G As Graphics, ByVal ContRect As Rectangle, ByVal Text As String, ByVal TFont As Font, ByVal TAlign As HorizontalAlignment, ByVal TColor As Color)
        If String.IsNullOrEmpty(Text) Then Return
        Dim TextSize As Size = G.MeasureString(Text, TFont).ToSize
        Dim CenteredY As Integer = ContRect.Height \ 2 - TextSize.Height \ 2
        Select Case TAlign
            Case HorizontalAlignment.Left
                Dim sf As New StringFormat
                sf.LineAlignment = StringAlignment.Near
                sf.Alignment = StringAlignment.Near
                G.DrawString(Text, TFont, New SolidBrush(TColor), New Rectangle(ContRect.X, ContRect.Y, ContRect.Width, ContRect.Height), sf)
            Case HorizontalAlignment.Right
                Dim sf As New StringFormat
                sf.LineAlignment = StringAlignment.Far
                sf.Alignment = StringAlignment.Far
                G.DrawString(Text, TFont, New SolidBrush(TColor), New Rectangle(ContRect.X, ContRect.Y, ContRect.Width, ContRect.Height / 2 + TextSize.Height / 2), sf)
            Case HorizontalAlignment.Center
                Dim sf As New StringFormat
                sf.LineAlignment = StringAlignment.Center
                sf.Alignment = StringAlignment.Center
                G.DrawString(Text, TFont, New SolidBrush(TColor), ContRect, sf)
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
    Public Function CodeToImage(ByVal Code As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(Code)))
    End Function
End Class
#End Region