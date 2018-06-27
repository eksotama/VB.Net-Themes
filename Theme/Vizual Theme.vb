Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Class VizualBigButton
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Sub New()
        Font = New Font("Verdana", 9, FontStyle.Bold)
        Size = New Size(100, 40)
        Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 12
        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim mainPath As GraphicsPath = CreateRound(mainRect, slope)

        Dim bgLGB As New LinearGradientBrush(mainRect, Color.FromArgb(180, 200, 215), Color.FromArgb(160, 180, 205), 90.0F)
        G.FillPath(bgLGB, mainPath)

        Dim textX As Integer = (Width / 2) - (G.MeasureString(Text, Font).Width / 2)
        Dim textY As Integer = (Height / 2) - (G.MeasureString(Text, Font).Height / 2)
        G.DrawString(Text, Font, Brushes.WhiteSmoke, textX, textY)

        If State = MouseState.Over Then
            G.FillPath(New SolidBrush(Color.FromArgb(25, Color.White)), mainPath)
        ElseIf State = MouseState.Down Then
            G.FillPath(New SolidBrush(Color.FromArgb(60, Color.White)), mainPath)
        End If

        Dim borderBrush As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(205, 225, 240), Color.FromArgb(120, 145, 170), 90.0F)
        G.DrawPath(New Pen(borderBrush), mainPath)

    End Sub

End Class

Class VizualSmallButton
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Sub New()
        Font = New Font("Verdana", 8, FontStyle.Bold)
        Size = New Size(80, 30)
        Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 12
        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim mainPath As GraphicsPath = CreateRound(mainRect, slope)

        Dim bgLGB As New LinearGradientBrush(mainRect, Color.FromArgb(250, 250, 250), Color.FromArgb(235, 235, 235), 90.0F)
        G.FillPath(bgLGB, mainPath)

        Dim textX As Integer = (Width / 2) - (G.MeasureString(Text, Font).Width / 2)
        Dim textY As Integer = (Height / 2) - (G.MeasureString(Text, Font).Height / 2)
        G.DrawString(Text, Font, Brushes.Black, textX, textY)

        If State = MouseState.Over Then
            G.FillPath(New SolidBrush(Color.FromArgb(10, Color.Black)), mainPath)
        ElseIf State = MouseState.Down Then
            G.FillPath(New SolidBrush(Color.FromArgb(20, Color.Black)), mainPath)
        End If

        Dim borderBrush As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.Silver, Color.DimGray, 90.0F)
        G.DrawPath(New Pen(borderBrush), mainPath)

    End Sub

End Class

Class VizualProgressbar
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Private percent As Double

    Private _Maximum As Integer = 100
    Public Property Maximum As Integer
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
    Public Property Value As Integer
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
        Size = New Size(200, 16)
        LockHeight = 16
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        percent = (_Value / _Maximum) * 100
        Dim slope As Integer = 14

        Dim mainRect As New Rectangle(1, 1, Width - 3, Height - 3)
        Dim borderPath As GraphicsPath = CreateRound(mainRect, slope)
        Dim bgLGB As New LinearGradientBrush(mainRect, Color.FromArgb(155, 160, 170), Color.FromArgb(175, 180, 185), 90.0F)
        G.FillPath(bgLGB, borderPath)
        Dim borderLGB As New LinearGradientBrush(mainRect, Color.FromArgb(130, 140, 150), Color.FromArgb(160, 170, 175), 90.0F)
        G.DrawPath(New Pen(borderLGB), borderPath)

        Dim barRect As New Rectangle(0, 0, CInt(((Width / _Maximum) * _Value) - 1), Height - 1)
        Dim barPath As GraphicsPath = CreateRound(barRect, slope)
        Dim barLGB As New LinearGradientBrush(barRect, Color.White, Color.FromArgb(235, 240, 245), 90.0F)
        Dim barBorderLGB As New LinearGradientBrush(barRect, Color.FromArgb(220, 220, 220), Color.FromArgb(170, 170, 170), 90.0F)
        If percent >= 3.5 Then
            G.FillPath(barLGB, barPath)
            G.DrawPath(New Pen(barBorderLGB), barPath)
        ElseIf percent < 3.5 AndAlso Value >= 1 Then
            Dim fakeBar As New Rectangle(0, 0, Height - 1, Height - 1)
            Dim fakeLGB As New LinearGradientBrush(fakeBar, Color.White, Color.FromArgb(235, 240, 245), 90.0F)
            G.FillEllipse(fakeLGB, fakeBar)
            G.DrawEllipse(New Pen(barBorderLGB), fakeBar)
        End If

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Class VizualSwitch1
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
        LockWidth = 70
        LockHeight = 30
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 8
        Dim switchX As Integer = 3

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim outerPath As GraphicsPath = CreateRound(mainRect, slope)
        Dim bgLGB As LinearGradientBrush = New LinearGradientBrush(mainRect, Color.Black, Color.Black, 90.0F)
        If _checked Then
            switchX = 34
            bgLGB = New LinearGradientBrush(mainRect, Color.FromArgb(180, 200, 215), Color.FromArgb(160, 180, 205), 90.0F)
        Else
            switchX = 3
            bgLGB = New LinearGradientBrush(mainRect, Color.FromArgb(150, 155, 160), Color.FromArgb(180, 185, 190), 90.0F)
        End If
        G.FillPath(bgLGB, outerPath)

        Dim onX, onY As Integer
        onX = (Width / 4) - (G.MeasureString("ON", Font).Width / 2)
        onY = (Height / 2) - (G.MeasureString("ON", Font).Height / 2)
        Dim offX, offY As Integer
        offX = (((Width - 1) / 4) * 3) - (G.MeasureString("OFF", Font).Width / 2)
        offY = (Height / 2) - (G.MeasureString("OFF", Font).Height / 2)
        G.DrawString("ON", Font, Brushes.WhiteSmoke, onX, onY)
        G.DrawString("OFF", Font, Brushes.Black, offX, offY)

        Dim switchRect As New Rectangle(switchX, 3, Width - 38, Height - 7)
        Dim switchPath As GraphicsPath = CreateRound(switchRect, slope)
        G.FillPath(Brushes.Silver, switchPath)
        Dim lgb As New LinearGradientBrush(switchRect, Color.FromArgb(245, 245, 245), Color.FromArgb(230, 230, 230), LinearGradientMode.Vertical)
        G.FillPath(lgb, switchPath)
        G.DrawPath(Pens.Gray, switchPath)

        Dim borderBrush As New LinearGradientBrush(mainRect, Color.FromArgb(130, 140, 150), Color.FromArgb(165, 170, 175), 90.0F)
        G.DrawPath(New Pen(borderBrush), outerPath)

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

<DefaultEvent("CheckedChanged")> Class VizualSwitch2
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
        Size = New Size(120, 19)
        LockHeight = 19
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim switchFont As New Font("Arial", 8)
        Dim textY As Integer = (Me.Height / 2) - (G.MeasureString("O", switchFont).Height / 2)
        G.DrawString("OFF", switchFont, Brushes.Black, New Point(0, textY))
        G.DrawString("ON", switchFont, Brushes.Black, New Point(Width - G.MeasureString("ON", switchFont).Width - 1, textY))

        Dim bgBrush As New SolidBrush(Color.Black)
        If _checked Then
            bgBrush = New SolidBrush(Color.FromArgb(170, 190, 210))
        Else
            bgBrush = New SolidBrush(Color.FromArgb(170, 175, 180))
        End If

        Dim leftArea As New Rectangle(G.MeasureString("OFF", switchFont).Width + 3, 0, Height - 1, Height - 1)
        Dim rightArea As New Rectangle(Me.Width - (G.MeasureString("ON", switchFont).Width) - Height - 3, 0, Height - 1, Height - 1)
        Dim connector As New Rectangle(leftArea.X + (Height - 1) - 1, ((Height - 1) / 2) - 2, rightArea.X - (leftArea.X + (Height - 1)) + 2, 4)
        Dim drawThrough As New Rectangle(connector.X - 1, connector.Y + 1, connector.Width + 2, connector.Height - 2)

        Dim borderBrush As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(125, 132, 140), Color.FromArgb(150, 155, 160), 90.0F)
        G.FillEllipse(bgBrush, leftArea)
        G.DrawEllipse(New Pen(borderBrush), leftArea)
        G.FillEllipse(bgBrush, rightArea)
        G.DrawEllipse(New Pen(borderBrush), rightArea)
        G.FillRectangle(bgBrush, connector)
        G.DrawRectangle(New Pen(borderBrush), connector)
        G.FillRectangle(bgBrush, drawThrough)

        Dim circleBrush As New LinearGradientBrush(leftArea, Color.FromArgb(250, 250, 250), Color.FromArgb(225, 230, 235), 90.0F)
        If _checked Then
            G.FillEllipse(circleBrush, New Rectangle(rightArea.X + 1, rightArea.Y + 1, rightArea.Width - 2, rightArea.Height - 2))
            Dim innerDot As New Rectangle(rightArea.X + 7, rightArea.Y + 7, rightArea.Width - 14, rightArea.Height - 14)
            G.FillEllipse(bgBrush, innerDot)
            G.DrawEllipse(New Pen(borderBrush), innerDot)
        Else
            G.FillEllipse(circleBrush, New Rectangle(leftArea.X + 1, leftArea.Y + 1, rightArea.Width - 2, rightArea.Height - 2))
            Dim innerDot As New Rectangle(leftArea.X + 7, leftArea.Y + 7, leftArea.Width - 14, leftArea.Height - 14)
            G.FillEllipse(bgBrush, innerDot)
            G.DrawEllipse(New Pen(borderBrush), innerDot)
        End If

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

<DefaultEvent("CheckedChanged")> Class VizualSwitch3
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
        Size = New Size(35, 19)
        LockHeight = 19
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = Height - 3

        Dim mainRect As New Rectangle(1, 1, Width - 3, Height - 3)
        Dim mainPath As GraphicsPath = CreateRound(mainRect, slope)

        Dim borderPen As New Pen(New LinearGradientBrush(mainRect, Color.FromArgb(120, 130, 140), Color.FromArgb(155, 165, 175), 90.0F))
        Dim bgBrush As New LinearGradientBrush(mainRect, Color.Black, Color.Black, 90.0F)
        If _checked Then
            bgBrush = New LinearGradientBrush(mainRect, Color.FromArgb(165, 185, 205), Color.FromArgb(185, 205, 225), 90.0F)
        Else
            bgBrush = New LinearGradientBrush(mainRect, Color.FromArgb(150, 155, 160), Color.FromArgb(165, 170, 175), 90.0F)
        End If

        G.FillPath(bgBrush, mainPath)
        G.DrawPath(borderPen, mainPath)

        Dim leftMark As New Rectangle(0, 0, Height - 1, Height - 1)
        Dim rightMark As New Rectangle((Width - 1) - (Height - 1), 0, Height - 1, Height - 1)
        Dim circleBrush As New LinearGradientBrush(leftMark, Color.FromArgb(250, 250, 250), Color.FromArgb(225, 230, 235), 90.0F)

        If _checked Then
            G.FillEllipse(circleBrush, rightMark)
            Dim innerRect As New Rectangle(rightMark.X + 7, rightMark.Y + 7, rightMark.Width - 14, rightMark.Height - 14)
            G.FillEllipse(bgBrush, innerRect)
            G.DrawEllipse(borderPen, rightMark)
            G.DrawEllipse(borderPen, innerRect)
        Else
            G.FillEllipse(circleBrush, leftMark)
            Dim innerRect As New Rectangle(leftMark.X + 7, leftMark.Y + 7, leftMark.Width - 14, leftMark.Height - 14)
            G.FillEllipse(bgBrush, innerRect)
            G.DrawEllipse(borderPen, leftMark)
            G.DrawEllipse(borderPen, innerRect)
        End If

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

<DefaultEvent("CheckedChanged")> Class VizualCheckbox
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
        Size = New Size(140, 19)
        LockHeight = 20
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 10

        Dim box As New Rectangle(0, 0, Height - 1, Height - 1)
        Dim boxPath As GraphicsPath = CreateRound(box, slope)
        Dim boxBrush As New LinearGradientBrush(box, Color.FromArgb(248, 248, 248), Color.FromArgb(235, 240, 245), 90.0F)
        G.FillPath(boxBrush, boxPath)

        Dim borderBrush As New LinearGradientBrush(box, Color.FromArgb(220, 220, 220), Color.FromArgb(160, 165, 170), 90.0F)
        G.DrawPath(New Pen(borderBrush), boxPath)

        Dim markPen As New Pen(Color.FromArgb(150, 155, 160))
        Dim lightMarkPen As New Pen(Color.FromArgb(170, 175, 180))
        If _checked Then
            G.DrawLine(markPen, New Point(5, 8), New Point(10, 13))
            G.DrawLine(markPen, New Point(6, 8), New Point(10, 12))
            G.DrawLine(markPen, New Point(6, 8), New Point(10, 11))
            G.DrawLine(markPen, New Point(7, 8), New Point(10, 10))
            Dim x As Integer = 20
            G.DrawLine(markPen, New Point(10, 10), New Point(x, 0))
            G.DrawLine(markPen, New Point(10, 11), New Point(x, 0))
            G.DrawLine(markPen, New Point(10, 12), New Point(x, 1))
            G.DrawLine(markPen, New Point(10, 13), New Point(x, 2))
        End If

        Dim textY As Integer = (Height / 2) - (G.MeasureString(Text, Font).Height / 2)
        G.DrawString(Text, Font, Brushes.Black, New Point(24, textY))

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

<DefaultEvent("CheckedChanged")> Class VizualRadioButton
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
        Size = New Size(140, 19)
        LockHeight = 20
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = Height - 1

        Dim box As New Rectangle(0, 0, Height - 1, Height - 1)
        Dim boxPath As GraphicsPath = CreateRound(box, slope)
        Dim boxBrush As New LinearGradientBrush(box, Color.FromArgb(248, 248, 248), Color.FromArgb(235, 240, 245), 90.0F)
        G.FillPath(boxBrush, boxPath)

        Dim borderBrush As New LinearGradientBrush(box, Color.FromArgb(220, 220, 220), Color.FromArgb(160, 165, 170), 90.0F)
        G.DrawPath(New Pen(borderBrush), boxPath)

        If _checked Then
            Dim innerMark As New Rectangle(7, 7, Height - 15, Height - 15)
            Dim innerBrush As New LinearGradientBrush(innerMark, Color.FromArgb(145, 165, 185), Color.FromArgb(165, 185, 205), 90.0F)
            G.FillEllipse(innerBrush, innerMark)
            G.DrawEllipse(New Pen(borderBrush), innerMark)
        End If

        Dim textY As Integer = (Height / 2) - (G.MeasureString(Text, Font).Height / 2)
        G.DrawString(Text, Font, Brushes.Black, New Point(24, textY))

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is VizualRadioButton Then
                DirectCast(C, VizualRadioButton).Checked = False
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

Class VizualFileBrowser
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Private mouseX As Integer

    Private _ofd As OpenFileDialog
    Public Property FileDialog As OpenFileDialog
        Get
            Return _ofd
        End Get
        Set(ByVal value As OpenFileDialog)
            _ofd = value
            Invalidate()
        End Set
    End Property

    Private _showFileLocation As Boolean
    Public Property ShowFileLocation As Boolean
        Get
            Return _showFileLocation
        End Get
        Set(ByVal value As Boolean)
            _showFileLocation = value
            Invalidate()
        End Set
    End Property

    Sub New()
        Size = New Size(250, 30)
        MinimumSize = New Size(140, 20)
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 12

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim mainPath As GraphicsPath = CreateRound(mainRect, slope)

        Dim textAreaBrush As New SolidBrush(Color.FromArgb(245, 245, 245))
        G.FillPath(textAreaBrush, mainPath)
        Dim textBorderPen As New Pen(Color.FromArgb(190, 195, 200))
        G.DrawPath(textBorderPen, mainPath)

        Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Text, New Font("Arial", 9)).Height / 2) + 1
        If _ofd Is Nothing Then
            G.DrawString("Select a FileDialog", Font, Brushes.DimGray, New Point(10, textY))
        Else
            G.DrawString(Text, New Font("Arial", 9), Brushes.DimGray, New Point(10, textY))
        End If

        Dim buttonRect As New Rectangle(Width - 81, 0, 80, Height - 1)
        Dim buttonPath As GraphicsPath = CreateRound(buttonRect, slope)

        Dim buttonBrush As New LinearGradientBrush(mainRect, Color.FromArgb(180, 200, 215), Color.FromArgb(160, 180, 205), 90.0F)
        G.FillPath(buttonBrush, buttonPath)
        Dim buttonBorderBrush As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(205, 225, 240), Color.FromArgb(100, 125, 150), 75.0F)
        G.DrawPath(New Pen(buttonBorderBrush), buttonPath)

        If State = MouseState.Over Then
            If mouseX >= (Width - 81) Then G.FillPath(New SolidBrush(Color.FromArgb(25, Color.White)), buttonPath)
        End If

        Dim browseFont As New Font("Verdana", 9, FontStyle.Bold)
        Dim browseX As Integer = ((Me.Width - 1) - buttonRect.Width) + (buttonRect.Width / 2) - (G.MeasureString("Browse", browseFont).Width / 2)
        Dim browseY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString("Browse", browseFont).Height / 2)
        G.DrawString("Browse", browseFont, Brushes.Silver, New Point(browseX + 1, browseY + 1))
        G.DrawString("Browse", browseFont, Brushes.WhiteSmoke, New Point(browseX, browseY))

    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        mouseX = e.X
        If mouseX > (Width - 81) Then
            Cursor = Cursors.Hand
        Else
            Cursor = Cursors.Arrow
        End If
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)

        If mouseX >= Width - 81 Then
            If _ofd IsNot Nothing Then
                If _ofd.ShowDialog = DialogResult.OK Then
                    If _showFileLocation Then
                        Text = _ofd.FileName
                    Else
                        Text = _ofd.FileName.Split("\")(_ofd.FileName.Split("\").Length - 1)
                    End If
                End If
            Else
                Dim msg As String = "Please select an OpenFileDialog by" & Environment.NewLine & _
                                "editing the FileDialog property." & Environment.NewLine & _
                                Environment.NewLine & "~ Hawk HF"
                MessageBox.Show(msg, "No OpenFileDialog Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If

        MyBase.OnMouseDown(e)

    End Sub

End Class

<DefaultEvent("ValueChanged")> Class VizualNumericUpDown
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Event ValueChanged(ByVal sender As Object)

    Private overUp As Boolean
    Private overDown As Boolean

    Private _value As Integer
    Public Property Value() As Integer
        Get
            Return _value
        End Get
        Set(ByVal v As Integer)
            If v > _maximum Then v = _maximum
            If v < _minimum Then v = _minimum
            _value = v
            Invalidate()
        End Set
    End Property

    Private _maximum As Integer
    Public Property Maximum() As Integer
        Get
            Return _maximum
        End Get
        Set(ByVal v As Integer)
            _maximum = v
        End Set
    End Property

    Private _minimum As Integer
    Public Property Minimum() As Integer
        Get
            Return _minimum
        End Get
        Set(ByVal v As Integer)
            _minimum = v
        End Set
    End Property

    Private _increment As Integer
    Public Property Increment As Integer
        Get
            Return _increment
        End Get
        Set(ByVal v As Integer)
            If v < 1 Then v = 1
            _increment = v
        End Set
    End Property

    Sub New()
        Maximum = 100
        LockHeight = 30
    End Sub

    Protected Overrides Sub PaintHook()

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim slope As Integer = 12

        Dim borderPen As New Pen(Color.FromArgb(180, 185, 190))
        Dim interiorBrush As New SolidBrush(Color.FromArgb(245, 248, 250))
        Dim arrowBrush As New SolidBrush(Color.FromArgb(130, 132, 135))
        Dim arrowBorder As New Pen(Color.FromArgb(90, 90, 90))

        Dim buttonBrush As SolidBrush = Brushes.Black
        If State = MouseState.Over Then
            buttonBrush = New SolidBrush(Color.FromArgb(180, 200, 215))
        ElseIf State = MouseState.Down Then
            buttonBrush = New SolidBrush(Color.FromArgb(160, 180, 200))
        End If

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim mainPath As GraphicsPath = CreateRound(mainRect, slope)
        G.FillPath(interiorBrush, mainPath)

        Dim basicUpDownRect As New Rectangle(Width - 19, 0, 18, (Height - 1) / 2)
        If overUp And (State = MouseState.Over Or State = MouseState.Down) And _value <> _maximum Then
            Dim upRect As Rectangle = basicUpDownRect
            Dim upPath As GraphicsPath = CreateRound(upRect, slope)
            G.FillPath(buttonBrush, upPath)
            Dim upperLeftFiller As New Rectangle(Width - 19, 0, 6, 6)
            Dim lowerLeftFiller As New Rectangle(Width - 19, ((Height - 1) / 2) - 6, 6, 6)
            Dim lowerRightFiller As New Rectangle((Width - 1) - 6, ((Height - 1) / 2) - 6, 6, 6)
            G.FillRectangles(buttonBrush, New Rectangle() {upperLeftFiller, lowerLeftFiller, lowerRightFiller})
        ElseIf overDown And (State = MouseState.Over Or State = MouseState.Down) And _value <> _minimum Then
            Dim downRect As Rectangle = New Rectangle(Width - 19, (Height - 1) / 2, 18, (Height - 1) / 2)
            Dim downPath As GraphicsPath = CreateRound(downRect, slope)
            G.FillPath(buttonBrush, downPath)
            Dim upperLeftFiller As New Rectangle(Width - 19, (Height - 1) / 2, 6, 6)
            Dim upperRightFiller As New Rectangle(Width - 6 - 1, (Height - 1) / 2, 6, 6)
            Dim lowerLeftFiller As New Rectangle(Width - 19, Height - 6 - 1, 6, 6)
            G.FillRectangles(buttonBrush, New Rectangle() {upperLeftFiller, upperRightFiller, lowerLeftFiller})
        End If

        Dim topPoints As Point() = {New Point(Width - 13, 9), New Point(Width - 7, 9), New Point(Width - 10, 5)}
        G.FillPolygon(arrowBrush, topPoints)
        G.DrawPolygon(arrowBorder, topPoints)
        Dim bottomPoints As Point() = {New Point(Width - 13, 19), New Point(Width - 7, 19), New Point(Width - 10, 23)}
        G.FillPolygon(arrowBrush, bottomPoints)
        G.DrawPolygon(arrowBorder, bottomPoints)

        G.DrawLine(borderPen, New Point(Width - 19, 0), New Point(Width - 19, Height - 1))
        G.DrawLine(borderPen, New Point(Width - 19, (Height - 1) / 2), New Point(Width - 1, (Height - 1) / 2))

        Dim valX As Integer = ((Me.Width - 19) / 2) - (G.MeasureString(CStr(_value), New Font("Verdana", 8)).Width / 2)
        Dim valY As Integer = (Me.Height / 2) - (G.MeasureString(CStr(_value), New Font("Verdana", 8)).Height / 2)
        G.DrawString(_value, New Font("Verdana", 8), Brushes.Black, valX, valY)

        G.DrawPath(borderPen, mainPath)

    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)

        If e.X > (Width - 19) AndAlso e.Y < ((Height - 1) / 2) Then
            overUp = True
            overDown = False
        ElseIf e.X > (Width - 19) AndAlso e.Y > ((Height - 1) / 2) Then
            overUp = False
            overDown = True
        Else
            overUp = False
            overDown = False
        End If

        Invalidate()

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        Dim preValue As Integer = _value

        If overUp = True Then
            If (_value + _increment) < _maximum Then
                _value += _increment
            ElseIf (_value + _increment) >= _maximum AndAlso _value >= (_maximum - _increment) Then
                _value = _maximum
            End If
        ElseIf overDown = True Then
            If (_value - _increment) > _minimum Then
                _value -= _increment
            ElseIf (_value - _increment) <= _minimum AndAlso _value <= (_minimum + _increment) Then
                _value = _minimum
            End If
        End If

        Dim postValue As Integer = _value

        If postValue <> preValue Then RaiseEvent ValueChanged(Me)

    End Sub

End Class

Class VizualTabControl
    Inherits TabControl

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.ResizeRedraw Or _
                 ControlStyles.UserPaint Or _
                 ControlStyles.DoubleBuffer, True)
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
        SizeMode = TabSizeMode.Fixed
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        If ItemSize <> New Size(40, 100) Then ItemSize = New Size(40, 100)

        Dim G As Graphics = e.Graphics

        Dim bgColor As Color = Color.FromArgb(240, 242, 245)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim selectedLGB As New LinearGradientBrush(New Rectangle(0, 0, 100, 40), Color.FromArgb(155, 162, 170), Color.FromArgb(180, 185, 190), 90.0F)
        Dim selectedTabBorderPen As New Pen(New LinearGradientBrush(GetTabRect(0), Color.FromArgb(130, 140, 150), Color.FromArgb(165, 170, 175), 90.0F))
        Dim selectedFont As New Font("Verdana", 8, FontStyle.Bold)
        Dim bgLGB As New LinearGradientBrush(New Rectangle(0, 0, 100, 40), Color.FromArgb(250, 250, 250), Color.FromArgb(235, 240, 245), 90.0F)
        Dim regFont As New Font("Verdana", 8, FontStyle.Regular)
        Dim tabBorderPen As New Pen(New LinearGradientBrush(GetTabRect(0), Color.FromArgb(220, 220, 220), Color.FromArgb(160, 165, 170), 90.0F))

        For i = 0 To TabCount - 1

            Dim mainRect As New Rectangle(GetTabRect(i).X, GetTabRect(i).Y + 2, GetTabRect(i).Width, GetTabRect(i).Height - 4)
            Dim titleX As Integer = (mainRect.Location.X + mainRect.Width / 2) - (G.MeasureString(TabPages(i).Text, Font).Width / 2 + 4)
            Dim titleY As Integer = (mainRect.Location.Y + mainRect.Height / 2) - (G.MeasureString(TabPages(i).Text, Font).Height / 2)

            If i = SelectedIndex Then
                G.FillPath(selectedLGB, RoundHalfRect(mainRect, 5))
                G.DrawString(TabPages(i).Text, selectedFont, New SolidBrush(Color.WhiteSmoke), New Point(titleX, titleY))
                G.DrawPath(selectedTabBorderPen, RoundHalfRect(mainRect, 5))
                G.DrawLine(New Pen(selectedLGB), New Point(mainRect.X + 5, mainRect.Y + 1), New Point(mainRect.X + 5, mainRect.Y + mainRect.Height - 1))
            Else
                G.FillPath(bgLGB, RoundHalfRect(mainRect, 5))
                G.DrawString(TabPages(i).Text, regFont, New SolidBrush(Color.Black), New Point(titleX, titleY))
                G.DrawPath(tabBorderPen, RoundHalfRect(mainRect, 5))
                G.DrawLine(New Pen(bgLGB), New Point(mainRect.X + 5, mainRect.Y + 1), New Point(mainRect.X + 5, mainRect.Y + mainRect.Height - 1))
            End If

            Try : TabPages(i).BackColor = bgColor : Catch : End Try

        Next

        If Dock = DockStyle.Fill Then
            Dim mainBorderRect As New Rectangle(ItemSize.Height, -1, (Width - 1) - ItemSize.Height + 1, Height + 2)
            Dim mainBorderBrush As New LinearGradientBrush(mainBorderRect, Color.FromArgb(130, 140, 150), Color.FromArgb(165, 170, 175), 90.0F)
            Dim borderPen As New Pen(mainBorderBrush)
            G.FillRectangle(New SolidBrush(bgColor), mainBorderRect)
            G.DrawRectangle(borderPen, mainBorderRect)
        Else
            Dim mainBorderRect As New Rectangle(ItemSize.Height, 0, (Width - 1) - ItemSize.Height, Height - 1)
            Dim mainBorderBrush As New LinearGradientBrush(mainBorderRect, Color.FromArgb(130, 140, 150), Color.FromArgb(165, 170, 175), 90.0F)
            Dim borderPen As New Pen(mainBorderBrush)
            G.FillPath(New SolidBrush(bgColor), RoundRect(mainBorderRect, 6))
            G.DrawPath(borderPen, RoundRect(mainBorderRect, 6))
        End If

    End Sub

    'Shoutout to Tedd for helping me with this.
    Private Function RoundHalfRect(ByVal r As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim rectPath As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        rectPath.AddArc(New Rectangle(r.X, r.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        rectPath.AddArc(New Rectangle(r.X, r.Height - ArcRectangleWidth + r.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        Dim rest As New Rectangle(r.X + Curve, r.Y, r.Width - Curve, r.Height)
        rectPath.AddRectangle(rest)
        rectPath.CloseAllFigures()
        Return rectPath
    End Function

    Public Function RoundRect(ByVal r As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(r.X, r.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(r.Width - ArcRectangleWidth + r.X, r.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(r.Width - ArcRectangleWidth + r.X, r.Height - ArcRectangleWidth + r.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(r.X, r.Height - ArcRectangleWidth + r.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(r.X, r.Height - ArcRectangleWidth + r.Y), New Point(r.X, Curve + r.Y))
        P.CloseAllFigures()
        Return P
    End Function

End Class

Class VizualCombo
    Inherits ComboBox

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()

        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DropDownStyle = ComboBoxStyle.DropDownList
        DoubleBuffered = True
        ItemHeight = 20

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim G As Graphics = e.Graphics

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim _font As New Font("Verdana", 8)

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim bgLGB As New LinearGradientBrush(mainRect, Color.FromArgb(250, 250, 250), Color.FromArgb(235, 240, 245), 90.0F)
        Dim mainPath As GraphicsPath = RoundRect(mainRect, 6)
        G.FillPath(bgLGB, mainPath)
        Dim mainBorderPen As New Pen(New LinearGradientBrush(mainRect, Color.FromArgb(220, 220, 220), Color.FromArgb(160, 165, 170), 90.0F))
        G.DrawPath(mainBorderPen, mainPath)

        Dim topTriangle As Point() = New Point() {New Point(Width - 15, 6), New Point(Width - 19, 11), New Point(Width - 11, 11)}
        Dim bottomTriangle As Point() = New Point() {New Point(Width - 15, 19), New Point(Width - 19, 14), New Point(Width - 11, 14)}
        G.FillPolygon(Brushes.DimGray, topTriangle)
        G.FillPolygon(Brushes.DimGray, bottomTriangle)

        Dim seperatorCB As New ColorBlend(3)
        seperatorCB.Colors(0) = Color.Transparent
        seperatorCB.Colors(1) = Color.FromArgb(200, 200, 200)
        seperatorCB.Colors(2) = Color.Transparent
        seperatorCB.Positions = New Single() {0.0, 0.5, 1.0}
        Dim seperatorLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 1), Color.Black, Color.Black)
        seperatorLGB.InterpolationColors = seperatorCB
        G.DrawLine(New Pen(seperatorLGB), New Point(Width - 27, 0), New Point(Width - 27, Height - 1))

        Try
            If Items.Count > 0 Then
                If Not SelectedIndex = -1 Then
                    Dim textX As Integer = 6
                    Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Items(SelectedIndex), _font).Height / 2) + 1
                    G.DrawString(Items(SelectedIndex), _font, Brushes.DimGray, New Point(textX, textY))
                Else
                    Dim textX As Integer = 6
                    Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Items(0), _font).Height / 2) + 1
                    G.DrawString(Items(0), _font, Brushes.DimGray, New Point(textX, textY))
                End If
            End If
        Catch : End Try

    End Sub

    Sub replaceItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim _font As New Font("Verdana", 8)

        Try
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                Dim selectedLGB As New LinearGradientBrush(e.Bounds, Color.FromArgb(180, 200, 215), Color.FromArgb(160, 180, 205), 90.0F)
                G.FillRectangle(selectedLGB, New Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width + 2, e.Bounds.Height + 2))
                G.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), _font, Brushes.White, New Rectangle(e.Bounds.X + 6, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height))
            Else
                G.FillRectangle(New SolidBrush(Color.FromArgb(235, 240, 245)), e.Bounds)
                G.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), _font, Brushes.Black, New Rectangle(e.Bounds.X + 6, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height))
            End If

        Catch : End Try

    End Sub

    Protected Overrides Sub OnSelectedItemChanged(ByVal e As System.EventArgs)
        MyBase.OnSelectedItemChanged(e)
        Invalidate()
    End Sub

    Public Function RoundRect(ByVal r As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(r.X, r.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(r.Width - ArcRectangleWidth + r.X, r.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(r.Width - ArcRectangleWidth + r.X, r.Height - ArcRectangleWidth + r.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(r.X, r.Height - ArcRectangleWidth + r.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(r.X, r.Height - ArcRectangleWidth + r.Y), New Point(r.X, Curve + r.Y))
        P.CloseAllFigures()
        Return P
    End Function

End Class

Class VizualLabel
    Inherits Label

    Sub New()
        BackColor = Color.Transparent
        Font = New Font("Verdana", 8)
        ForeColor = Color.Black
    End Sub

End Class