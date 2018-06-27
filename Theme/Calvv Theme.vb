Imports System.Drawing.Drawing2D
Imports System.ComponentModel


'NOTES AND CREDITS


' Notes

' This theme looks best if your form has a background color of (75, 75, 85)


' Credits

' All controls were coded by Hawk HF

' This theme was recommended to me by Calvv

' Shoutout to Aeonhack for his themebase.
' It makes everything so much easier and faster to make.

' Thanks to Mavamaarten for all the generosity to new theme-makers.
' Your theme-making tutorials and open source themes are great for the HF community.


Class CalvvButton
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Sub New()
        Size = New Size(100, 30)
        Font = New Font("Verdana", 8, FontStyle.Bold)
        Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim outerRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim outerPath As GraphicsPath = CreateRound(outerRect, 10)
        G.FillPath(New SolidBrush(Color.FromArgb(145, 142, 150)), outerPath)

        Dim innerRect As New Rectangle(0, Height / 2 - 1, Width - 1, Height / 2 + 1)
        Dim innerPath As GraphicsPath = CreateRound(innerRect, 10)
        G.FillPath(New SolidBrush(Color.FromArgb(45, Color.Black)), innerPath)

        Dim textureHB As New HatchBrush(HatchStyle.Trellis, Color.FromArgb(15, Color.White), Color.Transparent)
        G.FillPath(textureHB, outerPath)

        Dim textX As Integer = (Width / 2) - (G.MeasureString(Text, Font).Width / 2)
        Dim textY As Integer = (Height / 2) - (G.MeasureString(Text, Font).Height / 2)
        G.DrawString(Text, Font, Brushes.Black, New Point(textX + 1, textY + 1))
        G.DrawString(Text, Font, Brushes.WhiteSmoke, New Point(textX, textY))

        Select Case State
            Case MouseState.Over
                G.FillPath(New SolidBrush(Color.FromArgb(40, Color.White)), outerPath)
            Case MouseState.Down
                G.FillPath(New SolidBrush(Color.FromArgb(80, Color.White)), outerPath)
        End Select

        Dim borderBrush As New LinearGradientBrush(outerRect, Color.Gray, Color.Black, 80.0F)
        G.DrawPath(New Pen(borderBrush), outerPath)

    End Sub

End Class

Class CalvvProgressbar
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Private percent As Double

    Private _Maximum As Integer = 100
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal V As Integer)
            If V < 1 Then V = 1
            If V < _Value Then _Value = V
            _Maximum = V
            Invalidate()
        End Set
    End Property

    Private _Value As Integer
    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal V As Integer)
            If V > _Maximum Then V = Maximum
            _Value = V
            Invalidate()
        End Set
    End Property

    Sub New()
        IsAnimated = True
        DoubleBuffered = True
        Size = New Size(200, 24)
        Font = New Font("Verdana", 8, FontStyle.Bold)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        percent = (_Value / _Maximum) * 100
        Dim slope As Integer = 8

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)

        Dim borderPath As GraphicsPath = CreateRound(mainRect, slope)
        G.FillPath(New SolidBrush(Color.FromArgb(75, 75, 75)), borderPath)

        Dim bar_cblend As New ColorBlend(3)
        bar_cblend.Colors(0) = Color.FromArgb(165, 162, 170)
        bar_cblend.Colors(1) = Color.FromArgb(195, 192, 200)
        bar_cblend.Colors(2) = Color.FromArgb(143, 140, 148)
        bar_cblend.Positions = New Single() {0, 0.5, 1}
        Dim barLGB As New LinearGradientBrush(mainRect, Color.Black, Color.Black, 90.0F)
        barLGB.InterpolationColors = bar_cblend
        Dim barHB As New HatchBrush(HatchStyle.LightUpwardDiagonal, Color.FromArgb(25, Color.Black), Color.Transparent)

        Dim hb As New HatchBrush(HatchStyle.Trellis, Color.FromArgb(8, Color.White), Color.Transparent)
        G.FillPath(hb, borderPath)

        Dim barRect As New Rectangle(1, 1, CInt(((Width / _Maximum) * _Value) - 3), Height - 3)
        Dim barPath As GraphicsPath = CreateRound(barRect, slope)
        If percent >= 1 Then
            G.FillPath(barLGB, barPath)
            G.FillPath(barHB, barPath)
        End If

        Dim displayPercent As String = CStr(CInt(percent)) & "%"
        Dim textX As Integer = Me.Width - G.MeasureString(displayPercent, Font).Width - 8
        Dim textY As Integer = (Me.Height / 2) - (G.MeasureString(displayPercent, Font).Height / 2)
        G.DrawString(displayPercent, Font, Brushes.Black, New Point(textX + 1, textY + 1))
        G.DrawString(displayPercent, Font, Brushes.WhiteSmoke, New Point(textX, textY))

        G.DrawPath(Pens.Black, borderPath)

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Class CalvvSwitch
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Event CheckedChanged(ByVal sender As Object)

    Private switchX As Integer = 1

    Private _checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value
            Invalidate()
        End Set
    End Property

    Sub New()
        IsAnimated = True
        DoubleBuffered = True
        LockWidth = 80
        LockHeight = 30
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 8

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim outerPath As GraphicsPath = CreateRound(mainRect, slope)
        G.FillPath(New SolidBrush(Color.FromArgb(75, 75, 85)), outerPath)
        Dim hb As New HatchBrush(HatchStyle.Trellis, Color.FromArgb(8, Color.White), Color.Transparent)
        G.FillPath(hb, outerPath)

        Dim onX, onY As Integer
        onX = (Width / 4) - (G.MeasureString("On", Font).Width / 2)
        onY = (Height / 2) - (G.MeasureString("On", Font).Height / 2)
        Dim offX, offY As Integer
        offX = ((Width / 4) * 3) - (G.MeasureString("Off", Font).Width / 2)
        offY = (Height / 2) - (G.MeasureString("Off", Font).Height / 2)
        G.DrawString("On", Font, Brushes.WhiteSmoke, onX, onY)
        G.DrawString("Off", Font, Brushes.WhiteSmoke, offX, offY)

        If DesignMode Then
            If _checked Then switchX = 40 Else switchX = 1
        End If

        Dim switchRect As New Rectangle(switchX, 1, Width - 42, Height - 3)
        Dim switchPath As GraphicsPath = CreateRound(switchRect, slope)
        G.FillPath(Brushes.Silver, switchPath)
        Dim lgb As New LinearGradientBrush(switchRect, Color.FromArgb(215, 212, 220), Color.FromArgb(148, 140, 148), LinearGradientMode.Vertical)
        G.FillPath(lgb, switchPath)
        G.DrawPath(Pens.Black, switchPath)

        Dim borderBrush As New LinearGradientBrush(mainRect, Color.Gray, Color.Black, 80.0F)
        G.DrawPath(New Pen(borderBrush), outerPath)

    End Sub

    Protected Overrides Sub OnAnimation()
        MyBase.OnAnimation()

        If _checked Then
            If switchX < 40 Then switchX += 1
        Else
            If switchX > 1 Then switchX -= 1
        End If

        Invalidate()

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        If _checked Then
            _checked = False
        Else
            _checked = True
        End If

        RaiseEvent CheckedChanged(Me)

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Class CalvvCheckbox
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Event CheckedChanged(ByVal sender As Object)

    Private _checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value
            Invalidate()
        End Set
    End Property

    Sub New()
        Size = New Size(140, 21)
        LockHeight = 21
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 8

        Dim box As New Rectangle(0, 0, 20, Height - 1)
        Dim boxPath As GraphicsPath = CreateRound(box, slope)
        Dim boxBrush As New LinearGradientBrush(box, Color.FromArgb(115, 112, 120), Color.FromArgb(95, 92, 100), 90.0F)
        G.FillPath(boxBrush, boxPath)
        Dim textureHB As New HatchBrush(HatchStyle.Trellis, Color.FromArgb(12, Color.White), Color.Transparent)
        G.FillPath(textureHB, boxPath)

        Dim borderBrush As New LinearGradientBrush(box, Color.FromArgb(65, 62, 70), Color.FromArgb(45, 42, 50), 80.0F)
        G.DrawPath(New Pen(borderBrush), boxPath)

        If _checked Then
            G.DrawString("b", New Font("Marlett", 16), Brushes.WhiteSmoke, New Point(-2, 0))
        End If

        Dim textY As Integer = (Height / 2) - (G.MeasureString(Text, Font).Height / 2)
        G.DrawString(Text, Font, Brushes.WhiteSmoke, New Point(24, textY))

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        If _checked Then
            _checked = False
        Else
            _checked = True
        End If

        RaiseEvent CheckedChanged(Me)

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Class CalvvRadioButton
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Event CheckedChanged(ByVal sender As Object)

    Private _checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value
            Invalidate()
        End Set
    End Property

    Sub New()
        Size = New Size(140, 21)
        LockHeight = 21
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 20

        Dim box As New Rectangle(0, 0, 20, Height - 1)
        Dim boxPath As GraphicsPath = CreateRound(box, slope)
        Dim boxBrush As New LinearGradientBrush(box, Color.FromArgb(115, 112, 120), Color.FromArgb(95, 92, 100), 90.0F)
        G.FillPath(boxBrush, boxPath)
        Dim textureHB As New HatchBrush(HatchStyle.Trellis, Color.FromArgb(12, Color.White), Color.Transparent)
        G.FillPath(textureHB, boxPath)

        Dim borderBrush As New LinearGradientBrush(box, Color.FromArgb(65, 62, 70), Color.FromArgb(45, 42, 50), 80.0F)
        G.DrawPath(New Pen(borderBrush), boxPath)

        If _checked Then
            Dim mark As New Rectangle(5, 5, 10, 10)
            Dim markPath As GraphicsPath = CreateRound(mark, mark.Width)
            Dim markBrush As New LinearGradientBrush(mark, Color.FromArgb(175, 172, 180), Color.FromArgb(155, 152, 160), 80.0F)
            G.FillPath(markBrush, markPath)
            G.DrawPath(New Pen(borderBrush), markPath)
        End If

        Dim textY As Integer = (Height / 2) - (G.MeasureString(Text, Font).Height / 2)
        G.DrawString(Text, Font, Brushes.WhiteSmoke, New Point(24, textY))

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is CalvvRadioButton Then
                DirectCast(C, CalvvRadioButton).Checked = False
            End If
        Next

        If _checked Then
            _checked = False
        Else
            _checked = True
        End If

        RaiseEvent CheckedChanged(Me)

    End Sub

End Class

Class CalvvBasicTextbox
    Inherits TextBox

    Sub New()
        BackColor = Color.FromArgb(85, 82, 90)
        Font = New Font("Verdana", 8)
        ForeColor = Color.WhiteSmoke
    End Sub

End Class

Class CalvvBasicLabel
    Inherits Label

    Sub New()
        BackColor = Color.Transparent
        Font = New Font("Verdana", 8)
        ForeColor = Color.WhiteSmoke
    End Sub

End Class