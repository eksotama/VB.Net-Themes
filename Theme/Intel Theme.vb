Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Class ReadMe

    'Intel Theme By HΛWK

    '========== =====  HELP?  ===== ==========

    'If you're getting tons of errors, it is probably because you haven't
    '   imported Aeonhack's ThemeBase 154. You can get that from Aeonhack's
    '   PasteBin at http://pastebin.com/axvdEAJc
    '       Once you've opened the PasteBin page, here's what you do:
    '           1. Scroll down to the bottom and copy the RAW Paste Data
    '           2. In Visual Studio, go to Project -> Add Class
    '           3. Name it ThemeBase.vb, click "Add"
    '           4. Clear any and all code from ThemeBase.vb
    '           5. Paste the themebase code into ThemeBase.vb

    'If you don't want the animations, there is an option to remove it on some controls
    '   Always look near the top of the events in the Properties window
    '   All of my custom events will start with an underscore

    'Please report any bugs or errors to me.
    '   > Skype:        hawk-hf
    '   > HackForums:   http://www.hackforums.net/member.php?action=profile&uid=1265187



    '========== ===== CREDITS ===== ==========

    'Thanks to Aeonhack for his wonderful themebase 1.5.4
    '   His themebase makes thememaking much easier
    '   You can download it at http://pastebin.com/axvdEAJc, as stated above

    'Thanks to Mavamaarten~
    '   Allowing the public to view his theme sources
    '   Special events on the controls are coded by him
    '   I used his Crystal Clear textbox and tab control in this theme
    '       I take no credit for those two controls
    '       They are completely coded by him, I just recolored them

    'All other controls were coded by me, HΛWK.
    '   Find me on HF at http://www.hackforums.net/member.php?action=profile&uid=1265187



    '========== ===== THANKS  ===== ==========

    'Thanks for downloading and I hope you enjoy it!
    'Feel free to use this in any of your projects, but be sure to give credit!

End Class

Class iContainer
    Inherits ThemeContainer154
    Protected Overrides Sub ColorHook()
    End Sub

    Private Icon As Icon
    Public Property _Icon As Icon
        Get
            Return Icon
        End Get
        Set(ByVal value As Icon)
            Icon = value
            Invalidate()
        End Set
    End Property

    Private ShowIcon As Boolean
    Public Property _ShowIcon As Boolean
        Get
            Return ShowIcon
        End Get
        Set(ByVal value As Boolean)
            ShowIcon = value
            Invalidate()
        End Set
    End Property

    Private WideBorders As Boolean
    Public Property _WideBorders As Boolean
        Get
            Return WideBorders
        End Get
        Set(ByVal value As Boolean)
            WideBorders = value
            Invalidate()
        End Set
    End Property

    Private tGlow As Integer = 0

    Sub New()
        TransparencyKey = Color.Fuchsia
        ShowIcon = False
    End Sub

    Protected Overrides Sub PaintHook()

        G.Clear(Color.Fuchsia)

        'Rounded Form
        Dim gp As GraphicsPath = CreateRound(New Rectangle(0, 0, Width - 1, Height - 1), 16)
        G.FillPath(New SolidBrush(Color.FromArgb(45, 45, 45)), gp)
        G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(60, 60, 60))), 0, 24, Width - 1, 24)
        G.DrawLine(Pens.DeepSkyBlue, 0, 25, Width - 1, 25)
        G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(60, 60, 60))), 0, 26, Width - 1, 26)
        G.DrawPath(Pens.DeepSkyBlue, gp)

        'Title Text and Icon
        Dim fontColor As Color = Color.WhiteSmoke
        If ShowIcon Then
            If Icon Is Nothing Then Icon = FindForm.Icon
            G.DrawIcon(_Icon, New Rectangle(6, 6, 16, 16))
            G.DrawString(Text, New Font("Verdana", 11), Brushes.Black, New Point(26, 5))
            G.DrawString(Text, New Font("Verdana", 11), New SolidBrush(fontColor), New Point(25, 4))
        Else
            G.DrawString(Text, New Font("Verdana", 11), Brushes.Black, New Point(5, 5))
            G.DrawString(Text, New Font("Verdana", 11), New SolidBrush(fontColor), New Point(4, 4))
        End If

        'Square Off Bottom Corners
        G.FillRectangle(New SolidBrush(Color.FromArgb(45, 45, 45)), New Rectangle(0, Height - 35, Width - 1, 34))
        G.DrawLine(Pens.DeepSkyBlue, New Point(0, Height - 35), New Point(0, Height - 1))
        G.DrawLine(Pens.DeepSkyBlue, New Point(Width - 1, Height - 35), New Point(Width - 1, Height - 1))
        G.DrawLine(Pens.DeepSkyBlue, New Point(0, Height - 1), New Point(Width - 1, Height - 1))

        'Border Lines
        If WideBorders Then
            G.DrawLine(Pens.DeepSkyBlue, New Point(6, 25), New Point(6, Height - 7))
            G.DrawLine(Pens.DeepSkyBlue, New Point(6, Height - 7), New Point(Width - 7, Height - 7))
            G.DrawLine(Pens.DeepSkyBlue, New Point(Width - 7, Height - 7), New Point(Width - 7, 25))
        End If

    End Sub

End Class

Class iControlBox
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Sub New()
        IsAnimated = True
        LockWidth = 52 : LockHeight = 16
        Anchor = AnchorStyles.Right Or AnchorStyles.Top
    End Sub

    Private overMin, overMax, overExit As Boolean

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Location = New Point(FindForm.Width - 59, 4)
    End Sub

    Private X As Integer
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.X
        If e.X > 0 AndAlso e.X < 17 Then
            overMin = True : overMax = False : overExit = False
        ElseIf e.X > 17 AndAlso e.X < 35 Then
            overMin = False : overMax = True : overExit = False
        ElseIf e.X > 35 AndAlso e.X < 52 Then
            overMin = False : overMax = False : overExit = True
        Else
            overMin = False : overMax = False : overExit = False
        End If
    End Sub

    Private Sub ControlBoxClicked() Handles MyBase.MouseClick
        With FindForm()
            If overMin Then
                .WindowState = FormWindowState.Minimized
            ElseIf overMax Then
                If .WindowState = FormWindowState.Normal Then
                    .WindowState = FormWindowState.Maximized
                ElseIf .WindowState = FormWindowState.Maximized Then
                    .WindowState = FormWindowState.Normal
                End If
            ElseIf overExit Then
                .Close()
            End If
        End With
    End Sub

    Private minGlow, maxGlow, exitGlow As Integer
    Protected Overrides Sub OnAnimation()
        MyBase.OnAnimation()
        If overMin Then
            If minGlow < 120 Then minGlow += 5
        Else
            If minGlow >= 3 Then minGlow -= 3
        End If
        If overMax Then
            If maxGlow < 120 Then maxGlow += 5
        Else
            If maxGlow >= 3 Then maxGlow -= 3
        End If
        If overExit Then
            If exitGlow < 120 Then exitGlow += 5
        Else
            If exitGlow >= 3 Then exitGlow -= 3
        End If
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        overMin = False : overMax = False : overExit = False
        Invalidate()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(45, 45, 45))
        G.DrawString("0", New Font("Marlett", 8), New SolidBrush(Color.FromArgb(120 + minGlow, Color.SteelBlue)), New Point(2, 3))
        If FindForm.WindowState <> FormWindowState.Maximized Then
            G.DrawString("1", New Font("Marlett", 9), New SolidBrush(Color.FromArgb(120 + maxGlow, Color.SteelBlue)), New Point(20, 4))
        Else
            G.DrawString("2", New Font("Marlett", 9), New SolidBrush(Color.FromArgb(120 + maxGlow, Color.SteelBlue)), New Point(20, 4))
        End If
        G.DrawString("r", New Font("Marlett", 10), New SolidBrush(Color.FromArgb(120 + exitGlow, Color.SteelBlue)), New Point(37, 3))
    End Sub
End Class

Class iButton
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

    Sub New()
        IsAnimated = True
        Size = New Size(100, 28)
        MinimumSize = New Size(40, 20)
        Me.Cursor = Cursors.Hand
    End Sub

    Private glow As Integer = 180
    Private overButton As Boolean = False

    Protected Overrides Sub PaintHook()

        G.Clear(Color.FromArgb(45, 45, 45))

        Dim gp As GraphicsPath = CreateRound(New Rectangle(0, 0, Width - 1, Height - 1), 8)
        Dim pgb As New PathGradientBrush(gp)

        G.FillPath(Brushes.Black, gp)

        pgb.CenterColor = Color.FromArgb(200, Color.SteelBlue)
        pgb.SurroundColors = {Color.FromArgb(glow, Color.SteelBlue)}
        pgb.CenterPoint = New Point((Me.Width - 1) / 2, (Me.Height - 1) / 2)

        G.FillPath(pgb, gp)
        G.DrawPath(Pens.DeepSkyBlue, gp)

        Dim textWidth As Integer = Me.CreateGraphics.MeasureString(Text, Font).Width, textHeight As Integer = Me.CreateGraphics.MeasureString(Text, Font).Height
        Dim textShadow As New SolidBrush(Color.FromArgb(30, 15, 0))
        Dim textRect As New Rectangle(3, 3, textWidth + 10, textHeight)
        Dim textPoint As Point = New Point((Width / 2) - (textWidth / 2), (Height / 2) - (textHeight / 2)), textShadowPoint As Point = New Point((Width / 2) - (textWidth / 2) + 1, (Height / 2) - (textHeight / 2) + 1)
        G.DrawString(Text, Font, textShadow, textShadowPoint)
        G.DrawString(Text, Font, Brushes.WhiteSmoke, textPoint)

    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        overButton = True
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        overButton = False
    End Sub

    Protected Overrides Sub OnAnimation()
        MyBase.OnAnimation()
        If overButton Then
            If glow < 230 Then glow += 1
        Else
            If glow > 182 Then
                glow -= 2
            ElseIf glow > 180 And glow < 182 Then
                glow = 180
            End If
        End If
    End Sub

End Class

Class iProgBar
    Inherits ThemeControl154

    Private _Maximum As Integer = 100
    Private _Value As Integer
    Private Progress As Integer

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
        Size = New Size(200, 25)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(45, 45, 45))
        'Background
        Dim bg_cblend As New ColorBlend(3)
        bg_cblend.Colors(0) = Color.FromArgb(30, 30, 30)
        bg_cblend.Colors(1) = Color.FromArgb(22, 22, 22)
        bg_cblend.Colors(2) = Color.FromArgb(30, 30, 30)
        bg_cblend.Positions = {0, 0.5, 1}
        DrawGradient(bg_cblend, New Rectangle(1, 1, Width - 2, Height - 2))
        'Bar
        Dim bar_cblend As New ColorBlend(3)
        bar_cblend.Colors(0) = Color.FromArgb(180, Color.DeepSkyBlue)
        bar_cblend.Colors(1) = Color.FromArgb(135, Color.DeepSkyBlue)
        bar_cblend.Colors(2) = bar_cblend.Colors(0)
        bar_cblend.Positions = {0, 0.5, 1}
        DrawGradient(bar_cblend, New Rectangle(1, 1, CInt(((Width / _Maximum) * _Value) - 2), Height - 2))
        'Border
        Dim borderPoints As Point() = {New Point(0, 2), New Point(2, 0), New Point(Width - 3, 0), New Point(Width - 1, 2), New Point(Width - 1, Height - 3), New Point(Width - 3, Height - 1), New Point(2, Height - 1), New Point(0, Height - 3)}
        G.DrawPolygon(Pens.Black, borderPoints)
    End Sub
End Class

Class iGroupBox
    Inherits ThemeContainer154
    Protected Overrides Sub ColorHook()
    End Sub

    Private showTitleText As Boolean
    Public Property _ShowTitleText As Boolean
        Get
            Return showTitleText
        End Get
        Set(ByVal value As Boolean)
            showTitleText = value
            Invalidate()
        End Set
    End Property

    Sub New()
        ControlMode = True
        showTitleText = True
    End Sub

    Protected Overrides Sub PaintHook()

        G.Clear(Color.FromArgb(45, 45, 45))

        Dim gp As GraphicsPath = CreateRound(New Rectangle(0, 0, Width - 1, Height - 1), 8)
        G.FillPath(New SolidBrush(Color.FromArgb(45, 45, 45)), gp)
        If showTitleText Then
            G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(60, 60, 60))), 0, 19, Width - 1, 19)
            G.DrawLine(Pens.DeepSkyBlue, 0, 20, Width - 1, 20)
            G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(60, 60, 60))), 0, 21, Width - 1, 21)
            G.DrawString(Text, New Font("Verdana", 9), Brushes.Black, New Point(5, 4))
            G.DrawString(Text, New Font("Verdana", 9), Brushes.Silver, New Point(4, 3))
        End If
        G.DrawPath(Pens.DeepSkyBlue, gp)

    End Sub

End Class

Class iCheckBox
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

#Region "Events"
    Private _Checked As Boolean = False
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal checked As Boolean)
            _Checked = checked
            Invalidate()
        End Set
    End Property
    Sub changeChecked() Handles Me.Click
        Select Case _Checked
            Case False
                _Checked = True
            Case True
                _Checked = False
        End Select
    End Sub
#End Region

    Sub New()
        Size = New Size(150, 17)
        LockHeight = 17
        Font = New Font("Arial", 9)
    End Sub

    Protected Overrides Sub PaintHook()

        G.Clear(Color.FromArgb(45, 45, 45))

        Dim xRect As New Rectangle(0, 0, 20, 16)
        Dim cRect As New Rectangle(21, 0, 20, 16)

        Dim bg_cblend As New ColorBlend(3)
        bg_cblend.Colors(0) = Color.FromArgb(35, 35, 35)
        bg_cblend.Colors(1) = Color.FromArgb(52, 52, 52)
        bg_cblend.Colors(2) = bg_cblend.Colors(0)
        bg_cblend.Positions = {0, 0.5, 1}
        Dim pBrush As New LinearGradientBrush(xRect, Color.Black, Color.Red, 90.0F)
        pBrush.InterpolationColors = bg_cblend
        G.FillRectangles(pBrush, {cRect, xRect})

        'On And Off Switches
        If _Checked Then
            'b
            G.DrawString("b", New Font("Marlett", 16), Brushes.ForestGreen, New Point(18, -1))
            G.DrawString("r", New Font("Marlett", 11), New SolidBrush(Color.FromArgb(70, 70, 70)), New Point(1, 1))
        Else
            G.DrawString("b", New Font("Marlett", 16), New SolidBrush(Color.FromArgb(70, 70, 70)), New Point(18, -1))
            G.DrawString("r", New Font("Marlett", 11), New SolidBrush(Color.FromArgb(180, 50, 50)), New Point(1, 1))
        End If
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, 40, 16))
        G.DrawLine(Pens.Black, New Point(20, 0), New Point(20, 17))

        'Text
        G.DrawString(Text, New Font("Arial", 8), Brushes.WhiteSmoke, 44, 2)

    End Sub

End Class

Class iRadioButton
    Inherits ThemeControl154
    Protected Overrides Sub ColorHook()
    End Sub

#Region "Events"
    Private _Checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal V As Boolean)
            _Checked = V
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        MyBase.OnClick(e)
        For Each C As Control In Parent.Controls
            If C.GetType.ToString = Replace(My.Application.Info.ProductName, " ", "_") & ".iRadioButton" Then
                Dim CC As iRadioButton
                CC = C
                CC.Checked = False
            End If
        Next
        _Checked = True
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Dim textSize As Integer
        textSize = Me.CreateGraphics.MeasureString(Text, Font).Width
        Me.Width = 25 + textSize
    End Sub
#End Region

    Private Animation As Boolean
    Public Property _Animation As Boolean
        Get
            Return Animation
        End Get
        Set(ByVal value As Boolean)
            Animation = value
            Invalidate()
        End Set
    End Property

    Private glow As Integer = 150
    Private inc As Boolean = True

    Sub New()
        Size = New Point(50, 17)
        LockHeight = 17
        IsAnimated = True
        Animation = True
    End Sub

    Protected Overrides Sub PaintHook()

        G.Clear(Color.FromArgb(45, 45, 45))
        G.SmoothingMode = SmoothingMode.AntiAlias

        'Check Box
        If _Checked Then
            If Animation Then
                G.FillEllipse(New SolidBrush(Color.FromArgb(glow, Color.SteelBlue)), New Rectangle(4, 4, 8, 8))
            Else
                G.FillEllipse(New SolidBrush(Color.FromArgb(205, Color.SteelBlue)), New Rectangle(4, 4, 8, 8))
            End If
        End If
        G.DrawEllipse(Pens.Black, New Rectangle(0, 0, 16, 16))

        'Text
        G.DrawString(Text, New Font("Verdana", 8), Brushes.Black, 21, 3)
        G.DrawString(Text, New Font("Verdana", 8), Brushes.WhiteSmoke, 20, 2)

    End Sub

    Protected Overrides Sub OnAnimation()
        MyBase.OnAnimation()
        If inc Then
            If glow < 240 Then glow += 1 Else inc = False
        Else
            If glow > 150 Then glow -= 2 Else inc = True
        End If
    End Sub

End Class

Class iLabel
    Inherits Label

    Sub New()
        BackColor = Color.Transparent
        ForeColor = Color.Silver
        Font = New Font("Verdana", 8)
    End Sub

End Class

'The remaining controls were coded by Mavamaarten~
<DefaultEvent("TextChanged")> Class CrystalClearTextBox
    Inherits ThemeControl154

    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If Base IsNot Nothing Then
                Base.TextAlign = value
            End If
        End Set
    End Property
    Private _MaxLength As Integer = 32767
    Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If Base IsNot Nothing Then
                Base.MaxLength = value
            End If
        End Set
    End Property
    Private _ReadOnly As Boolean
    Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If Base IsNot Nothing Then
                Base.ReadOnly = value
            End If
        End Set
    End Property
    Private _UseSystemPasswordChar As Boolean
    Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If Base IsNot Nothing Then
                Base.UseSystemPasswordChar = value
            End If
        End Set
    End Property
    Private _Multiline As Boolean
    Property Multiline() As Boolean
        Get
            Return _Multiline
        End Get
        Set(ByVal value As Boolean)
            _Multiline = value
            If Base IsNot Nothing Then
                Base.Multiline = value

                If value Then
                    LockHeight = 0
                    Base.Height = Height - 11
                Else
                    LockHeight = Base.Height + 11
                End If
            End If
        End Set
    End Property
    Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If Base IsNot Nothing Then
                Base.Text = value
            End If
        End Set
    End Property
    Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If Base IsNot Nothing Then
                Base.Font = value
                Base.Location = New Point(3, 5)
                Base.Width = Width - 6

                If Not _Multiline Then
                    LockHeight = Base.Height + 11
                End If
            End If
        End Set
    End Property
    Protected Overrides Sub OnCreation()
        If Not Controls.Contains(Base) Then
            Controls.Add(Base)
        End If
    End Sub

    Private Base As TextBox
    Sub New()
        Base = New TextBox
        Base.Font = Font
        Base.Text = Text
        Base.MaxLength = _MaxLength
        Base.Multiline = _Multiline
        Base.ReadOnly = _ReadOnly
        Base.UseSystemPasswordChar = _UseSystemPasswordChar
        Base.Borderstyle=BorderStyle.None
        Base.Location = New Point(4, 4)
        Base.Width = Width - 10
        If _Multiline Then
            Base.Height = Height - 11
        Else
            LockHeight = Base.Height + 11
        End If
        AddHandler Base.TextChanged, AddressOf OnBaseTextChanged
        AddHandler Base.KeyDown, AddressOf OnBaseKeyDown
    End Sub

    Private BG As Color
    Private P1 As Pen

    Protected Overrides Sub ColorHook()
        Base.ForeColor = Color.WhiteSmoke
        Base.BackColor = Color.FromArgb(35, 35, 35)
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(35, 35, 35))
        G.FillRectangle(New SolidBrush(Color.FromArgb(35, 35, 35)), New Rectangle(0, 0, Width - 1, Height - 1))
        DrawBorders(Pens.Black)
    End Sub
    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
        Text = Base.Text
    End Sub
    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            Base.SelectAll()
            e.SuppressKeyPress = True
        End If
    End Sub
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        Base.Location = New Point(4, 5)
        Base.Width = Width - 8

        If _Multiline Then
            Base.Height = Height - 5
        End If


        MyBase.OnResize(e)
    End Sub

End Class

Class CrystalClearTabControl
    Inherits TabControl

    Private _BG As Color
    Public Overrides Property Backcolor As Color
        Get
            Return _BG
        End Get
        Set(ByVal value As Color)
            _BG = value
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DoubleBuffered = True
        Backcolor = Color.FromArgb(45, 45, 45)
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub

    Function ToPen(ByVal color As Color) As Pen
        Return New Pen(color)
    End Function

    Function ToBrush(ByVal color As Color) As Brush
        Return New SolidBrush(color)
    End Function

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Try : SelectedTab.BackColor = Backcolor : Catch : End Try
        G.Clear(Backcolor)
        For i = 0 To TabCount - 1
            If i = SelectedIndex Then
                Dim x2 As Rectangle = New Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y, GetTabRect(i).Width, GetTabRect(i).Height - 2)
                Dim x3 As Rectangle = New Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y, GetTabRect(i).Width, GetTabRect(i).Height - 1)
                Dim x4 As Rectangle = New Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y, GetTabRect(i).Width, GetTabRect(i).Height)
                Dim G1 As New LinearGradientBrush(x3, Color.FromArgb(10, 0, 0, 0), Color.FromArgb(35, 35, 35), 90.0F)
                Dim HB As New HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(10, Color.Black), Color.Transparent)

                G.FillRectangle(HB, x3) : HB.Dispose()
                G.FillRectangle(G1, x3) : G1.Dispose()
                G.DrawLine(New Pen(Color.FromArgb(10, 10, 10)), x2.Location, New Point(x2.Location.X, x2.Location.Y + x2.Height))
                G.DrawLine(New Pen(Color.FromArgb(10, 10, 10)), New Point(x2.Location.X + x2.Width, x2.Location.Y), New Point(x2.Location.X + x2.Width, x2.Location.Y + x2.Height))
                G.DrawLine(New Pen(Color.FromArgb(10, 10, 10)), New Point(x2.Location.X, x2.Location.Y), New Point(x2.Location.X + x2.Width, x2.Location.Y))
                G.DrawString(TabPages(i).Text, Font, New SolidBrush(Color.DeepSkyBlue), x4, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            Else
                Dim x2 As Rectangle = New Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y + 3, GetTabRect(i).Width, GetTabRect(i).Height - 5)
                Dim G1 As New LinearGradientBrush(x2, Color.FromArgb(30, 30, 30), Color.FromArgb(35, 35, 35), -90.0F)
                G.FillRectangle(G1, x2) : G1.Dispose()
                G.DrawRectangle(New Pen(Color.FromArgb(15, 15, 15)), x2)
                G.DrawString(TabPages(i).Text, Font, New SolidBrush(Color.FromArgb(75, 75, 75)), x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            End If
        Next
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(10, 10, 10))), New Rectangle(0, 21, Width - 1, Height - 22))

        e.Graphics.DrawImage(B.Clone, 0, 0)
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))
        G.Dispose() : B.Dispose()
    End Sub
End Class