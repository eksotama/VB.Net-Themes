Imports System.Drawing.Drawing2D
'|===========================================================|
'|===|  White UI
'| Creator: LordPankake
'| Created: 8/13/2014, Last edited: 8/19/2014
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
        Pal.ColHighest = Color.White
        Pal.ColHigh = Color.FromArgb(215, 215, 215)
        Pal.ColMed = Color.FromArgb(180, 180, 180)
        Pal.ColDim = Color.FromArgb(100, 100, 100)
        Pal.ColDark = Color.FromArgb(50, 50, 50)
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
        Pal.ColHighest = Color.White
        Pal.ColHigh = Color.FromArgb(215, 215, 215)
        Pal.ColMed = Color.FromArgb(180, 180, 180)
        Pal.ColDim = Color.FromArgb(100, 100, 100)
        Pal.ColDark = Color.FromArgb(50, 50, 50)
        BackColor = Pal.ColDim
    End Sub
End Class
#End Region
#Region "Theme"
Public Class WhiteUIForm : Inherits ThemedContainer
    Sub New()
        MyBase.New()
        MinimumSize = New Size(305, 150)
        Dock = DockStyle.Fill
        TopGrip = 30
        Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
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
        '| Background static noise
        Dim BGStatic As Bitmap = D.CodeToImage(D.BGWhiteStatic)
        Dim BGTextureBrush As New TextureBrush(BGStatic, WrapMode.TileFlipXY)
        Dim MainPath As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 10)


        G.FillPath(BGTextureBrush, MainPath)
        G.DrawPath(New Pen(Pal.ColHigh), MainPath)
        G.DrawLine(New Pen(Pal.ColHigh), New Point(0, TopGrip), New Point(Width - 1, TopGrip))
        D.DrawTextWithShadow(G, New Rectangle(8, 0, Width - 17, TopGrip), Text, Font, HorizontalAlignment.Left, Pal.ColDim, Color.FromArgb(200, 200, 200))
    End Sub
End Class
Public Class WhiteUITopButton : Inherits ThemedControl
    Public Property Action As BtnAction = BtnAction.Close
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
        If Action = BtnAction.Close Then
            FindForm.Close()
        Else
            FindForm.WindowState = FormWindowState.Minimized
        End If
    End Sub
    Enum BtnAction
        Close
        Minimize
    End Enum
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim BGStatic As Bitmap = D.CodeToImage(D.BGWhiteStatic)
        Dim BGTextureBrush As New TextureBrush(BGStatic, WrapMode.TileFlipXY)
        Dim TextRect As New Rectangle(0, 0, Width + 1, Height - 3)
        Dim MainPath As GraphicsPath = D.RoundRect(New Rectangle(2, 2, Width - 6, Height - 6), 5)
        Dim DropShadowPath As GraphicsPath = D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 5)
        G.FillRectangle(BGTextureBrush, New Rectangle(-1, -1, Width + 1, Height + 1))


        '| Dropshadow
        Dim ColBlend As ColorBlend = New ColorBlend(3)
        ColBlend.Colors = {Color.Transparent, Color.FromArgb(30, Color.DimGray), Color.FromArgb(60, Color.DimGray)}
        ColBlend.Positions = {0, 1 / 2, 1}
        D.DrawShadowPath(G, ColBlend, DropShadowPath)

        '| Drawing the button
        Dim BtnText As String
        If Action = BtnAction.Close Then
            BtnText = "x"
        Else
            BtnText = "_"
        End If
        Select Case State
            Case MouseState.None
                Dim ButtonLGB As New LinearGradientBrush(New Point(0, 2), New Point(0, Height * (3 / 2)), Color.FromArgb(240, 240, 240), Color.FromArgb(220, 220, 220))
                G.FillPath(ButtonLGB, MainPath)
                D.DrawText(G, TextRect, BtnText, Font, HorizontalAlignment.Center, Color.FromArgb(200, 200, 200))
            Case MouseState.Over
                Dim ButtonLGB As New LinearGradientBrush(New Point(0, 2), New Point(0, Height * (3 / 2)), Color.White, Color.FromArgb(230, 230, 230))
                G.FillPath(ButtonLGB, MainPath)
                D.DrawText(G, TextRect, BtnText, Font, HorizontalAlignment.Center, Color.FromArgb(210, 210, 210))
            Case MouseState.Down
                Dim ButtonLGB As New LinearGradientBrush(New Point(0, 2), New Point(0, Height * (3 / 2)), Color.FromArgb(230, 230, 230), Color.FromArgb(210, 210, 210))
                G.FillPath(ButtonLGB, MainPath)
                D.DrawText(G, TextRect, BtnText, Font, HorizontalAlignment.Center, Color.FromArgb(170, 170, 170))
        End Select
        G.DrawPath(New Pen(Pal.ColHigh), MainPath)
    End Sub
End Class
Public Class WhiteUIButton : Inherits ThemedControl
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim BGStatic As Bitmap = D.CodeToImage(D.BGWhiteStatic)
        Dim BGTextureBrush As New TextureBrush(BGStatic, WrapMode.TileFlipXY)
        Dim TextRect As New Rectangle(0, 0, Width + 1, Height - 3)
        Dim MainPath As GraphicsPath = D.RoundRect(New Rectangle(2, 2, Width - 6, Height - 6), 5)
        Dim DropShadowPath As GraphicsPath = D.RoundRect(New Rectangle(-2, 0, Width + 1, Height), 5)
        G.FillRectangle(BGTextureBrush, New Rectangle(-1, -1, Width + 1, Height + 1))


        '| Dropshadow
        Dim ColBlend As ColorBlend = New ColorBlend(3)
        ColBlend.Colors = {Color.Transparent, Color.FromArgb(30, Color.DimGray), Color.FromArgb(60, Color.DimGray)}
        ColBlend.Positions = {0, 1 / 10, 1}
        D.DrawShadowPath(G, ColBlend, DropShadowPath)

        '| Main button drawing
        Select Case State
            Case MouseState.None
                Dim ButtonLGB As New LinearGradientBrush(New Point(0, 2), New Point(0, Height * (3 / 2)), Color.WhiteSmoke, Pal.ColHigh)
                G.FillPath(ButtonLGB, MainPath)
            Case MouseState.Over
                Dim ButtonLGB As New LinearGradientBrush(New Point(0, 2), New Point(0, Height * (3 / 2)), Color.White, Color.FromArgb(230, 230, 230))
                G.FillPath(ButtonLGB, MainPath)
            Case MouseState.Down
                Dim ButtonLGB As New LinearGradientBrush(New Point(0, 2), New Point(0, Height), Color.FromArgb(240, 240, 240), Pal.ColHigh)
                G.FillPath(ButtonLGB, MainPath)
        End Select

        D.DrawTextWithShadow(G, TextRect, Text, Font, HorizontalAlignment.Center, Pal.ColDim, Color.FromArgb(200, 200, 200))
        G.DrawPath(New Pen(Pal.ColHigh), MainPath)
    End Sub
End Class
Public Class WhiteUICircularButton : Inherits ThemedControl
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim BGStatic As Bitmap = D.CodeToImage(D.BGWhiteStatic)
        Dim BGTextureBrush As New TextureBrush(BGStatic, WrapMode.TileFlipXY)
        Dim TextRect As New Rectangle(0, 0, Width + 1, Height - 3)
        Dim MainPath As Rectangle = New Rectangle(2, 2, Width - 6, Height - 6)
        Dim DropShadowPath As Rectangle = New Rectangle(-2, 0, Width + 1, Height)
        G.FillRectangle(BGTextureBrush, New Rectangle(-1, -1, Width + 1, Height + 1))
        '| Dropshadow
        D.DrawShadowEllipse(G, Color.FromArgb(30, Color.DimGray), DropShadowPath)

        '| Main button drawing
        Select Case State
            Case MouseState.None
                Dim ButtonLGB As New LinearGradientBrush(New Point(0, 2), New Point(0, Height * (3 / 2)), Color.WhiteSmoke, Pal.ColHigh)
                G.FillEllipse(ButtonLGB, MainPath)
            Case MouseState.Over
                Dim ButtonLGB As New LinearGradientBrush(New Point(0, 2), New Point(0, Height * (3 / 2)), Color.White, Color.FromArgb(230, 230, 230))
                G.FillEllipse(ButtonLGB, MainPath)
            Case MouseState.Down
                Dim ButtonLGB As New LinearGradientBrush(New Point(0, 2), New Point(0, Height), Color.FromArgb(240, 240, 240), Pal.ColHigh)
                G.FillEllipse(ButtonLGB, MainPath)
        End Select

        D.DrawTextWithShadow(G, TextRect, Text, Font, HorizontalAlignment.Center, Pal.ColDim, Color.FromArgb(200, 200, 200))
        G.DrawEllipse(New Pen(Pal.ColHigh), MainPath)
    End Sub
End Class
Public Class WhiteUICheckbox : Inherits ThemedControl
    Public Property Checked As Boolean
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        Checked = Not Checked
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
        Height = 31
        Dim BGStatic As Bitmap = D.CodeToImage(D.BGWhiteStatic)
        Dim BGTextureBrush As New TextureBrush(BGStatic, WrapMode.TileFlipXY)
        Dim TextRect As New Rectangle(30, 0, Width + 1, Height - 1)
        Dim CheckboxPath As GraphicsPath = D.RoundRect(New Rectangle(2, 2, Height - 6, Height - 6), 5)
        Dim CheckboxLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height * (3 / 2)), Color.FromArgb(200, 200, 200), Color.FromArgb(245, 245, 245))
        Dim DropShadowPath As GraphicsPath = D.RoundRect(New Rectangle(-1, 0, Height, Height - 1), 5)
        G.FillRectangle(BGTextureBrush, New Rectangle(-1, -1, Width + 1, Height + 1))


        '| Dropshadow
        Dim ShadowBlend As ColorBlend = New ColorBlend(3)
        ShadowBlend.Colors = {Color.Transparent, Color.FromArgb(30, Pal.ColDim), Color.FromArgb(60, Color.DimGray)}
        ShadowBlend.Positions = {0, 1 / 8, 1}
        D.DrawShadowPath(G, ShadowBlend, DropShadowPath)

        '| Main checkbox drawing
        G.FillPath(CheckboxLGB, CheckboxPath)

        '| Check drawing
        If Checked Then
            Dim chkPoly As Rectangle = New Rectangle(2 + (Height - 6) / 4, 2 + (Height - 6) / 4, (Height - 6) \ 2, (Height - 6) \ 2)
            Dim Poly, PolyShad As New GraphicsPath
            With Poly
                .AddLine(New Point(chkPoly.X, chkPoly.Y + chkPoly.Height \ 2), New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height))
                .AddLine(New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height), New Point(chkPoly.X + chkPoly.Width + 1, chkPoly.Y - 2))
            End With
            With PolyShad
                .AddLine(New Point(chkPoly.X, 2 + chkPoly.Y + chkPoly.Height \ 2), New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + 2 + chkPoly.Height))
                .AddLine(New Point(chkPoly.X + chkPoly.Width \ 2, 2 + chkPoly.Y + chkPoly.Height), New Point(chkPoly.X + chkPoly.Width + 1, chkPoly.Y))
            End With
            G.DrawPath(New Pen(Color.FromArgb(180, 180, 180), 3), PolyShad)
            G.DrawPath(New Pen(Color.FromArgb(255, 255, 255), 3), Poly)
        End If

        D.DrawTextWithShadow(G, TextRect, Text, Font, HorizontalAlignment.Left, Pal.ColDim, Color.FromArgb(200, 200, 200))
        G.DrawPath(New Pen(Color.FromArgb(50, Pal.ColDim)), CheckboxPath)
    End Sub
End Class
Public Class WhiteUIRadiobutton : Inherits ThemedControl
    Public Property Checked As Boolean
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        For Each Cont As Control In Parent.Controls
            If TypeOf Cont Is WhiteUIRadiobutton Then
                DirectCast(Cont, WhiteUIRadiobutton).Checked = False
                Cont.Invalidate()
            End If
        Next
        Checked = True
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
        Height = 31
        Dim BGStatic As Bitmap = D.CodeToImage(D.BGWhiteStatic)
        Dim BGTextureBrush As New TextureBrush(BGStatic, WrapMode.TileFlipXY)
        Dim TextRect As New Rectangle(30, 0, Width + 1, Height - 1)
        Dim RadiobuttonRect As Rectangle = New Rectangle(2, 2, Height - 6, Height - 6)
        Dim RadiobuttonLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height * (3 / 2)), Color.FromArgb(200, 200, 200), Color.FromArgb(245, 245, 245))
        Dim DropShadowCircle As Rectangle = New Rectangle(-1, 0, Height, Height - 1)
        G.FillRectangle(BGTextureBrush, New Rectangle(-1, -1, Width + 1, Height + 1))


        '| Dropshadow
        D.DrawShadowEllipse(G, Color.FromArgb(30, Color.DimGray), DropShadowCircle)

        '| Main checkbox drawing
        G.FillEllipse(RadiobuttonLGB, RadiobuttonRect)

        '| Check drawing
        If Checked Then
            Dim CheckedLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height * (3 / 2)), Color.FromArgb(230, 230, 230), Color.FromArgb(255, 255, 255))
            Dim CheckedCircle As New Rectangle(6, 6, Height - 14, Height - 14)
            G.FillEllipse(CheckedLGB, CheckedCircle)
            G.DrawEllipse(New Pen(Color.FromArgb(50, Pal.ColDim)), CheckedCircle)
        End If

        D.DrawTextWithShadow(G, TextRect, Text, Font, HorizontalAlignment.Left, Pal.ColDim, Color.FromArgb(200, 200, 200))
        G.DrawEllipse(New Pen(Color.FromArgb(50, Pal.ColDim)), RadiobuttonRect)
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
        Dim BGStatic As Bitmap = D.CodeToImage(D.BGWhiteStatic)
        Dim BGTextureBrush As New TextureBrush(BGStatic, WrapMode.TileFlipXY)
        Dim MainPath As GraphicsPath = D.RoundRect(New Rectangle(2, 4, Width - 6, Height - 14), 5)
        Dim MainPathhighlight As GraphicsPath = D.RoundRect(New Rectangle(2, 6, Width - 6, Height - 12), 5)
        Dim MainPathLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 1), Color.FromArgb(225, 225, 225), Color.FromArgb(240, 240, 240))
        Dim MainPathHighlightLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(185, 185, 185), Color.White)
        G.FillRectangle(BGTextureBrush, New Rectangle(-1, -1, Width + 1, Height + 1))

        '| Main shape
        G.FillPath(MainPathLGB, MainPath)
        G.DrawPath(New Pen(MainPathHighlightLGB, 2), MainPathhighlight)

        '| Grip
        Dim GripX As Integer = ValueToPercentage(Value) * Width - 15
        Dim GripRect As New Rectangle(GripX, 0, 30, Height - 5)
        Dim GripPath As GraphicsPath = D.RoundRect(GripRect, 6)
        Dim GripShadowRect As New Rectangle(GripX - 2, -1, 34, Height)
        Dim GripShadowPath As GraphicsPath = D.RoundRect(GripShadowRect, 6)
        Dim GripLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.White, Color.FromArgb(220, 220, 220))

        '| Grip dropshadow
        Dim ColBlend As ColorBlend = New ColorBlend(3)
        ColBlend.Colors = {Color.Transparent, Color.FromArgb(30, Color.DimGray), Color.FromArgb(60, Color.DimGray)}
        ColBlend.Positions = {0, 1 / 10, 1}
        D.DrawShadowPath(G, ColBlend, GripShadowPath)

        '| Grip drawing
        G.FillPath(GripLGB, GripPath)
        G.DrawPath(New Pen(Color.FromArgb(100, Pal.ColDim)), GripPath)
        D.DrawTextWithShadow(G, GripRect, Value, Font, HorizontalAlignment.Center, Pal.ColDim, Color.FromArgb(200, 200, 200))
    End Sub
    Private Function ValueToPercentage(value As Integer) As Single
        Dim min = Minimum - 10
        Dim max = Maximum + 10
        Return (value - min) / (max - min)
        'vertical:  Return 1 - (value - min) / (max - min)
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
    Public BGWhiteStatic As String = "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"
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
    Public Sub DrawShadowPath(ByVal G As Graphics, ByVal ColBlend As ColorBlend, ByVal Path As GraphicsPath)
        Using ShadowBrush As PathGradientBrush = New PathGradientBrush(Path)
            ShadowBrush.WrapMode = WrapMode.Clamp
            ShadowBrush.InterpolationColors = ColBlend
            G.FillPath(ShadowBrush, Path)
        End Using
    End Sub
    Public Sub DrawShadowEllipse(ByVal G As Graphics, ByVal col As Color, ByVal Path As Rectangle)
        Dim gp As New GraphicsPath()
        gp.AddEllipse(Path)

        Dim pgb As New PathGradientBrush(gp)

        pgb.CenterPoint = New PointF(Path.Width / 2, Path.Height / 2)
        pgb.CenterColor = col
        pgb.SurroundColors = New Color() {Color.Transparent}
        pgb.SetBlendTriangularShape(0.1F, 1.0F)
        pgb.FocusScales = New PointF(0.0F, 0.0F)

        G.FillPath(pgb, gp)
    End Sub
    Public Function CodeToImage(ByVal Code As String) As Image
        Return Image.FromStream(New System.IO.MemoryStream(Convert.FromBase64String(Code)))
    End Function
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
                G.DrawString(Text, TFont, New SolidBrush(TColor), ContRect.X + ContRect.Width \ 2 - TextSize.Width \ 2, CenteredY)
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