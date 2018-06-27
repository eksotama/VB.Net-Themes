Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.ComponentModel

''============================
''Author: Mr.TwEaK
''Publish Date: 07;05;2012
''Credits: Aeonhack
''============================
''========= Include =========''
''Aeonhack 1.5.4 ThemeBase
''========= End ''========= 

Class BCEvoTheme
    Inherits ThemeContainer154
    Protected Overrides Sub ColorHook()
    End Sub


    Private _ForeColor As Color
    Public Property TextColor() As Color
        Get
            Return _ForeColor
        End Get
        Set(ByVal value As Color)
            _ForeColor = value
        End Set
    End Property


    Private Blend As ColorBlend
    Sub New()
        TextColor = Color.Gray
        TransparencyKey = Color.Maroon
        Blend = New ColorBlend
        Blend.Colors = New Color() {Color.FromArgb(16, 16, 16), Color.FromArgb(46, 46, 46), Color.FromArgb(46, 46, 46), Color.FromArgb(16, 16, 16)}
        Blend.Positions = New Single() {0.0F, 0.45F, 0.65F, 1.0F}
    End Sub
    Protected Overrides Sub OnCreation()
        If Not DesignMode Then
            Dim G As New Threading.Thread(AddressOf MoveGlow)
            Dim T As New Threading.Thread(AddressOf MoveText)
            G.IsBackground = True
            T.IsBackground = True
            G.Start()
            T.Start()
        End If
    End Sub

    Private GlowPosition As Single = -1.0F
    Private Sub MoveGlow()
        While True
            GlowPosition += 0.01F
            If GlowPosition >= 1.0F Then GlowPosition = -1.0F
            Invalidate()
            Threading.Thread.Sleep(60)
        End While
    End Sub
    Private TextPosition As Single = -1.0F
    Private Sub MoveText()
        While True
            TextPosition += 0.01F
            If TextPosition >= 1.0F Then TextPosition = -1.0F
            Invalidate()
            Threading.Thread.Sleep(60)
        End While
    End Sub
    Private BasePoints As Point()
    Private BaseBrush As LinearGradientBrush
    Private BaseCol, BaseCol2 As Color
    Private TBox, TIBox, TRPBox As Point()
    Private HBrush As HatchBrush
    Protected Overrides Sub PaintHook()
        TBox = New Point() {New Point(98, 1), New Point(ClientRectangle.Width - 98, 1), New Point(ClientRectangle.Width - 98, 22), New Point(98, 22), New Point(98, 1)}
        TIBox = New Point() {New Point(101, 4), New Point(ClientRectangle.Width - 101, 4), New Point(ClientRectangle.Width - 101, 19), New Point(101, 19), New Point(101, 4)}
        TRPBox = New Point() {New Point(100, 3), New Point(ClientRectangle.Width - 100, 3), New Point(ClientRectangle.Width - 100, 20), New Point(100, 20), New Point(100, 3)}
        BaseCol = Color.FromArgb(255, 170, 0, 0)
        BaseCol2 = Color.FromArgb(255, 150, 0, 0)
        BaseBrush = New LinearGradientBrush(ClientRectangle, BaseCol, BaseCol2, LinearGradientMode.Vertical)
        G.FillRectangle(BaseBrush, ClientRectangle)
        HBrush = New HatchBrush(HatchStyle.Trellis, Color.FromArgb(24, 24, 24), Color.FromArgb(8, 8, 8))

        G.FillRectangle(HBrush, 11, 30, Width - 22, Height - 41)
        G.DrawLines(New Pen(Color.FromArgb(32, 32, 32), 1), TBox)
        G.FillRectangle(New SolidBrush(Color.FromArgb(50, 50, 50)), 98, 1, ClientRectangle.Width - 196, 8)
        DrawGradient(Color.FromArgb(8, 8, 8), Color.FromArgb(23, 23, 23), 98, 1, ClientRectangle.Width - 196, 22, 90.0F)
        G.SetClip(New Rectangle(101, 3, ClientRectangle.Width - 201, 17))
        G.FillRectangle(New SolidBrush(Color.FromArgb(16, 16, 16)), 98, 1, ClientRectangle.Width - 196, 22)
        DrawGradient(Blend, CInt(GlowPosition * ClientRectangle.Width + 50), 1, ClientRectangle.Width - 196, 22, 0.0F)
        G.DrawLines(New Pen(Color.FromArgb(15, Color.White), 1), TIBox)
        DrawText(New SolidBrush(TextColor), HorizontalAlignment.Center, -CInt(TextPosition * ClientRectangle.Width + 50), 0)
        G.ResetClip()
        G.FillRectangle(New SolidBrush(Color.FromArgb(26, Color.White)), 98, 1, ClientRectangle.Width - 196, 8)
        G.DrawLines(Pens.Black, TRPBox)
        G.DrawLines(Pens.Black, TBox)
        DrawBorders(Pens.Maroon, 0)
        DrawCorners(Color.Red, 0)
    End Sub
End Class
'Credits to Aeonhack for Seperator
Class BCEvoTSeperator
    Inherits ThemeControl154

    Private _Orientation As Orientation
    Property Orientation() As Orientation
        Get
            Return _Orientation
        End Get
        Set(ByVal value As Orientation)
            _Orientation = value

            If value = Windows.Forms.Orientation.Vertical Then
                LockHeight = 0
                LockWidth = 14
            Else
                LockHeight = 14
                LockWidth = 0
            End If

            Invalidate()
        End Set
    End Property

    Sub New()
        Transparent = True
        BackColor = Color.Transparent

        LockHeight = 14
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)

        Dim BL1, BL2 As New ColorBlend
        BL1.Positions = New Single() {0.0F, 0.15F, 0.85F, 1.0F}
        BL2.Positions = New Single() {0.0F, 0.15F, 0.5F, 0.85F, 1.0F}

        BL1.Colors = New Color() {Color.Transparent, Color.Black, Color.Black, Color.Transparent}
        BL2.Colors = New Color() {Color.Transparent, Color.FromArgb(35, 35, 35), Color.FromArgb(45, 45, 45), Color.FromArgb(35, 35, 35), Color.Transparent}

        If _Orientation = Windows.Forms.Orientation.Vertical Then
            DrawGradient(BL1, 6, 0, 1, Height)
            DrawGradient(BL2, 7, 0, 1, Height)
        Else
            DrawGradient(BL1, 0, 6, Width, 1, 0.0F)
            DrawGradient(BL2, 0, 7, Width, 1, 0.0F)
        End If

    End Sub

End Class

Class BCEvoCtrlBox
    Inherits ThemeControl154
    Private _Min As Boolean = True
    Private _Max As Boolean = True
    Private X As Integer

    Protected Overrides Sub ColorHook()
    End Sub

    Private _Orientation As Orientation
    Property Orientation() As Orientation
        Get
            Return _Orientation
        End Get
        Set(ByVal value As Orientation)
            _Orientation = value

            If value = Windows.Forms.Orientation.Vertical Then
                LockHeight = 0
                LockWidth = 14
            Else
                LockHeight = 14
                LockWidth = 0
            End If

            Invalidate()
        End Set
    End Property

    Public Property MinButton() As Boolean
        Get
            Return _Min
        End Get
        Set(ByVal value As Boolean)
            _Min = value
            Dim tempwidth As Integer = 40
            If _Min Then tempwidth += 25
            If _Max Then tempwidth += 25
            Me.Width = tempwidth + 1
            Me.Height = 16
            Invalidate()
        End Set
    End Property

    Public Property MaxButton() As Boolean
        Get
            Return _Max
        End Get
        Set(ByVal value As Boolean)
            _Max = value
            Dim tempwidth As Integer = 40
            If _Min Then tempwidth += 25
            If _Max Then tempwidth += 25
            Me.Width = tempwidth + 1
            Me.Height = 16
            Invalidate()
        End Set
    End Property

    Sub New()
        Transparent = True
        BackColor = Color.Transparent
        LockHeight = 14
        LockWidth = 72
        Location = New Point(50, 2)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        MyBase.OnClick(e)
        If _Min And _Max Then
            If X > 0 And X < 20 Then
                FindForm.WindowState = FormWindowState.Minimized
            ElseIf X > 26 And X < 45 Then
                If FindForm.WindowState = FormWindowState.Maximized Then FindForm.WindowState = FormWindowState.Normal Else FindForm.WindowState = FormWindowState.Maximized
            ElseIf X > 51 And X < 70 Then
                FindForm.Close()
            End If
        ElseIf _Min Then
            If X > 0 And X < 20 Then
                FindForm.WindowState = FormWindowState.Minimized
            ElseIf X > 26 And X < 45 Then
                FindForm.Close()
            End If
        ElseIf _Max Then
            If X > 0 And X < 20 Then
                If FindForm.WindowState = FormWindowState.Maximized Then FindForm.WindowState = FormWindowState.Normal Else FindForm.WindowState = FormWindowState.Maximized
            ElseIf X > 26 And X < 45 Then
                FindForm.Close()
            End If
        Else
            If X > 0 And X < 20 Then
                FindForm.Close()
            End If
        End If
    End Sub

    Private ExBrush, SBrush, MinBrush, MNone As LinearGradientBrush
    Private MinPath, SPath, ExPath As GraphicsPath
    Protected Overrides Sub PaintHook()
        G.SmoothingMode = SmoothingMode.HighSpeed
        MinPath = CreateRound(New Rectangle(0, 5, 17, 6), 6)
        SPath = CreateRound(New Rectangle(25, 5, 17, 6), 6)
        ExPath = CreateRound(New Rectangle(50, 5, 17, 6), 6)
        MinBrush = New LinearGradientBrush(New Rectangle(0, 5, 17, 6), Color.FromArgb(190, 244, 236, 0), Color.FromArgb(255, 218, 211, 0), LinearGradientMode.Vertical)
        SBrush = New LinearGradientBrush(New Rectangle(0, 5, 17, 6), Color.FromArgb(255, 236, 144, 0), Color.FromArgb(255, 218, 133, 0), LinearGradientMode.Vertical)
        ExBrush = New LinearGradientBrush(New Rectangle(0, 5, 17, 6), Color.FromArgb(255, 238, 0, 0), Color.FromArgb(255, 167, 0, 0), LinearGradientMode.Vertical)
        MNone = New LinearGradientBrush(New Rectangle(0, 5, 17, 6), Color.FromArgb(255, 43, 43, 43), Color.FromArgb(255, 10, 10, 10), LinearGradientMode.Vertical)

        If _Min And _Max Then
            LockHeight = 14
            LockWidth = 72
            If State = MouseState.Over Then
                If X > 0 And X < 20 Then
                    G.FillPath(MinBrush, MinPath)
                    G.FillPath(MNone, SPath)
                    G.FillPath(MNone, ExPath)
                    G.DrawPath(Pens.Black, ExPath)
                    G.DrawPath(Pens.Black, SPath)
                    G.DrawPath(Pens.Black, MinPath)
                End If
                If X > 26 And X < 45 Then
                    G.FillPath(SBrush, SPath)
                    G.FillPath(MNone, MinPath)
                    G.FillPath(MNone, ExPath)
                    G.DrawPath(Pens.Black, ExPath)
                    G.DrawPath(Pens.Black, MinPath)
                    G.DrawPath(Pens.Black, SPath)
                End If
                If X > 51 And X < 70 Then
                    G.FillPath(ExBrush, ExPath)
                    G.FillPath(MNone, MinPath)
                    G.FillPath(MNone, SPath)
                    G.DrawPath(Pens.Black, MinPath)
                    G.DrawPath(Pens.Black, SPath)
                    G.DrawPath(Pens.Black, ExPath)
                End If
            ElseIf State = MouseState.None Then
                G.FillPath(MNone, ExPath)
                G.FillPath(MNone, MinPath)
                G.FillPath(MNone, SPath)
                G.DrawPath(Pens.Black, MinPath)
                G.DrawPath(Pens.Black, SPath)
                G.DrawPath(Pens.Black, ExPath)
            End If
        ElseIf _Min Then
            LockHeight = 14
            LockWidth = 47
            If State = MouseState.Over Then
                If X > 0 And X < 20 Then
                    ExPath = CreateRound(New Rectangle(25, 5, 17, 6), 6)
                    G.FillPath(MinBrush, MinPath)
                    G.FillPath(MNone, ExPath)
                    G.DrawPath(Pens.Black, ExPath)
                    G.DrawPath(Pens.Black, MinPath)
                End If
                If X > 26 And X < 45 Then
                    ExPath = CreateRound(New Rectangle(25, 5, 17, 6), 6)
                    G.FillPath(ExBrush, ExPath)
                    G.FillPath(MNone, MinPath)
                    G.DrawPath(Pens.Black, MinPath)
                    G.DrawPath(Pens.Black, ExPath)
                End If
            ElseIf State = MouseState.None Then
                ExPath = CreateRound(New Rectangle(25, 5, 17, 6), 6)
                G.FillPath(MNone, MinPath)
                G.FillPath(MNone, ExPath)
                G.DrawPath(Pens.Black, ExPath)
                G.DrawPath(Pens.Black, MinPath)
            End If
        ElseIf _Max Then
            LockHeight = 14
            LockWidth = 47
            If State = MouseState.Over Then
                If X > 0 And X < 20 Then
                    SPath = CreateRound(New Rectangle(0, 5, 17, 6), 6)
                    ExPath = CreateRound(New Rectangle(25, 5, 17, 6), 6)
                    G.FillPath(SBrush, SPath)
                    G.FillPath(MNone, ExPath)
                    G.DrawPath(Pens.Black, ExPath)
                    G.DrawPath(Pens.Black, SPath)
                End If
                If X > 26 And X < 45 Then
                    ExPath = CreateRound(New Rectangle(25, 5, 17, 6), 6)
                    SPath = CreateRound(New Rectangle(0, 5, 17, 6), 6)
                    G.FillPath(ExBrush, ExPath)
                    G.FillPath(MNone, SPath)
                    G.DrawPath(Pens.Black, SPath)
                    G.DrawPath(Pens.Black, ExPath)
                End If
            ElseIf State = MouseState.None Then
                SPath = CreateRound(New Rectangle(0, 5, 17, 6), 6)
                ExPath = CreateRound(New Rectangle(25, 5, 17, 6), 6)
                G.FillPath(MNone, SPath)
                G.FillPath(MNone, ExPath)
                G.DrawPath(Pens.Black, ExPath)
                G.DrawPath(Pens.Black, SPath)
            End If
        Else
            LockHeight = 14
            LockWidth = 19
            If State = MouseState.Over Then
                If X > 0 And X < 20 Then
                    ExPath = CreateRound(New Rectangle(0, 5, 17, 6), 6)
                    G.FillPath(ExBrush, ExPath)
                    G.DrawPath(Pens.Black, ExPath)
                End If
            ElseIf State = MouseState.None Then
                ExPath = CreateRound(New Rectangle(0, 5, 17, 6), 6)
                G.FillPath(MNone, ExPath)
                G.DrawPath(Pens.Black, ExPath)
            End If
        End If
    End Sub
End Class

Class BCEvoProgressBar
    Inherits ThemeControl154

    Private Blend As ColorBlend

    Sub New()
        Blend = New ColorBlend
        Blend.Colors = New Color() {Color.FromArgb(150, 0, 0), Color.FromArgb(190, 0, 0), Color.FromArgb(190, 0, 0), Color.FromArgb(150, 0, 0)}
        Blend.Positions = New Single() {0.0F, 0.4F, 0.6F, 1.0F}
    End Sub

    Protected Overrides Sub OnCreation()
        If Not DesignMode Then
            Dim T As New Threading.Thread(AddressOf MoveGlow)
            T.IsBackground = True
            T.Start()
        End If
    End Sub

    Private GlowPosition As Single = -1.0F
    Private Sub MoveGlow()
        While True
            GlowPosition += 0.01F
            If GlowPosition >= 1.0F Then GlowPosition = -1.0F
            Invalidate()
            Threading.Thread.Sleep(10)
        End While
    End Sub

    Private _Value As Integer
    Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value > _Maximum Then value = _Maximum
            If value < 0 Then value = 0

            _Value = value
            Invalidate()
        End Set
    End Property

    Private _Maximum As Integer = 100
    Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If _Value > value Then _Value = value

            _Maximum = value
            Invalidate()
        End Set
    End Property

    Sub Increment(ByVal amount As Integer)
        Value += amount
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Private Progress As Integer
    Protected Overrides Sub PaintHook()
        G.SmoothingMode = SmoothingMode.HighQuality
        DrawBorders(New Pen(Color.FromArgb(32, 32, 32)), 1)
        G.FillRectangle(New SolidBrush(Color.FromArgb(50, 50, 50)), 0, 0, Width, 8)

        DrawGradient(Color.FromArgb(8, 8, 8), Color.FromArgb(23, 23, 23), 2, 2, Width - 4, Height - 4, 90.0F)

        Progress = CInt((_Value / _Maximum) * Width)

        If Not Progress = 0 Then
            G.SetClip(New Rectangle(3, 3, Progress - 6, Height - 6))
            G.FillRectangle(New SolidBrush(Color.FromArgb(150, 0, 0)), 0, 0, Progress, Height)

            DrawGradient(Blend, CInt(GlowPosition * Progress), 0, Progress, Height, 0.0F)
            DrawBorders(New Pen(Color.FromArgb(15, Color.White)), 3, 3, Progress - 6, Height - 6)

            G.FillRectangle(New SolidBrush(Color.FromArgb(13, Color.White)), 3, 3, Width - 6, CInt(Height / 2 - 3))
            G.ResetClip()
        End If

        DrawBorders(Pens.Black, 2)
        DrawBorders(Pens.Black)
    End Sub
End Class

Class BCEvoButton
    Inherits ThemeControl154

    Protected Overrides Sub ColorHook()
    End Sub
    Private _ForeColor As Color
    Public Property TextColor() As Color
        Get
            Return _ForeColor
        End Get
        Set(ByVal value As Color)
            _ForeColor = value
        End Set
    End Property

    Sub New()
        Transparent = True
        BackColor = Color.Transparent
        TextColor = Color.Gray
        Size = New Size(97, 23)
        Location = New Point(40, 30)
    End Sub
    Private BPath, TPath As GraphicsPath
    Private BITPoints As Point()
    Private BRect, TRect, BIRect As Rectangle
    Private BBrush, BIBrush As LinearGradientBrush
    Protected Overrides Sub PaintHook()
        G.SmoothingMode = SmoothingMode.HighSpeed
        BRect = New Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1)
        TRect = New Rectangle(0, 0, ClientRectangle.Width - 2, CInt(ClientRectangle.Height / 2))
        BITPoints = New Point() {New Point(4, 4), New Point(ClientRectangle.Width - 4, 4), New Point(ClientRectangle.Width - 4, ClientRectangle.Height - 4), New Point(4, ClientRectangle.Height - 4), New Point(4, 4)}
        BIRect = New Rectangle(3, 3, ClientRectangle.Width - 4, ClientRectangle.Height - 4)
        BBrush = New LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 55, 55, 55), Color.FromArgb(255, 22, 22, 22), LinearGradientMode.Vertical)
        BIBrush = New LinearGradientBrush(BIRect, Color.FromArgb(255, 100, 0, 0), Color.FromArgb(255, 60, 0, 0), LinearGradientMode.Vertical)

        Select Case State
            Case MouseState.Over
                BIBrush = New LinearGradientBrush(BIRect, Color.FromArgb(255, 60, 0, 0), Color.FromArgb(255, 100, 0, 0), LinearGradientMode.Vertical)
                G.FillRectangle(BBrush, BRect)
                G.DrawRectangle(Pens.Black, BRect)
                G.FillPolygon(BIBrush, BITPoints)
                DrawBorders(Pens.Black, 3)
                G.FillRectangle(New SolidBrush(Color.FromArgb(25, 255, 255, 255)), TRect)
            Case MouseState.Down
                G.FillRectangle(BBrush, BRect)
                G.DrawRectangle(Pens.Black, BRect)
                G.FillPolygon(BIBrush, BITPoints)
                DrawBorders(Pens.Black, 3)
                G.FillRectangle(New SolidBrush(Color.FromArgb(25, 255, 255, 255)), TRect)
            Case MouseState.None
                G.FillRectangle(BBrush, BRect)
                G.DrawRectangle(Pens.Black, BRect)
                G.FillPolygon(BIBrush, BITPoints)
                DrawBorders(Pens.Black, 3)
                G.FillRectangle(New SolidBrush(Color.FromArgb(25, 255, 255, 255)), TRect)
        End Select
        DrawText(New SolidBrush(TextColor), HorizontalAlignment.Center, 0, 0)

        If Enabled = False Then
            BIBrush = New LinearGradientBrush(BIRect, Color.FromArgb(255, 40, 40, 40), Color.FromArgb(255, 20, 20, 20), LinearGradientMode.Vertical)
            G.FillRectangle(BBrush, BRect)
            G.DrawRectangle(Pens.Black, BRect)
            G.FillPolygon(BIBrush, BITPoints)
            DrawBorders(Pens.Black, 3)
            G.FillRectangle(New SolidBrush(Color.FromArgb(13, 255, 255, 255)), TRect)
            DrawText(Brushes.Gray, HorizontalAlignment.Center, 0, 0)
        Else
        End If
    End Sub
End Class

Class BCEvoCheckBox
    Inherits ThemeControl154

    Private _ForeColor As Color = Color.Black
    Public Property TextColor() As Color
        Get
            Return _ForeColor
        End Get
        Set(ByVal v As Color)
            _ForeColor = v
            Invalidate()
        End Set
    End Property

    Private _CheckedState As Boolean
    Public Property CheckedState() As Boolean
        Get
            Return _CheckedState
        End Get
        Set(ByVal v As Boolean)
            _CheckedState = v
            Invalidate()
        End Set
    End Property

    Sub New()
        Transparent = True
        BackColor = Color.Transparent
        TextColor = Color.Gray
        Size = New Size(125, 19)
        MinimumSize = New Size(16, 19)
        MaximumSize = New Size(600, 19)
        CheckedState = False
    End Sub

    Sub changeCheck() Handles Me.Click
        Select Case CheckedState
            Case True
                CheckedState = False
            Case False
                CheckedState = True
        End Select
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub
    Private _OBRect, _OBGRect, _IBRect As Rectangle
    Private _FCBrush As SolidBrush
    Private _OBPen As Pen
    Private _IBBrush, _LBCBrush, _LBG, _BHighlightBrush As LinearGradientBrush
    Private _LBColor, _LBColor2, _IBColor, _IBColor2, _IBCColor, _IBCColor2, _BHColor, _BHColor2 As Color
    Protected Overrides Sub PaintHook()
        G.Clear(Color.Transparent)
        ' Declare variable values
        _OBRect = New Rectangle(0, 1, 15, 15)
        _OBGRect = New Rectangle(0, 1, 15, 7)
        _IBRect = New Rectangle(2, 3, 11, 11)
        _LBColor = Color.FromArgb(50, 255, 255, 255)
        _LBColor2 = Color.FromArgb(100, 255, 255, 255)
        _OBPen = New Pen(Color.FromArgb(255, 120, 120, 120))
        _IBColor = Color.FromArgb(255, 20, 20, 20)
        _IBColor2 = Color.FromArgb(255, 60, 60, 60)
        _IBCColor = Color.FromArgb(255, 100, 0, 0)
        _IBCColor2 = Color.FromArgb(255, 60, 0, 0)
        _BHColor = Color.FromArgb(30, 196, 196, 196)
        _BHColor2 = Color.FromArgb(13, 226, 226, 226)
        _LBG = New LinearGradientBrush(_OBRect, _LBColor, _LBColor2, LinearGradientMode.Vertical)
        _IBBrush = New LinearGradientBrush(_IBRect, _IBColor, _IBColor2, LinearGradientMode.Vertical)
        _LBCBrush = New LinearGradientBrush(_IBRect, _IBCColor, _IBCColor2, LinearGradientMode.Vertical)
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        _FCBrush = New SolidBrush(_ForeColor)

        ' Draw Checkbox BG
        G.FillRectangle(_LBG, _OBGRect)
        G.FillRectangle(_IBBrush, _IBRect)
        G.DrawRectangle(Pens.Black, _OBRect)

        Select Case State
            Case MouseState.Over
                _BHighlightBrush = New LinearGradientBrush(_OBRect, _BHColor2, _BHColor, LinearGradientMode.Vertical)
                G.FillRectangle(_BHighlightBrush, _OBRect)
            Case MouseState.Down
                _BHighlightBrush = New LinearGradientBrush(_OBRect, _BHColor, _BHColor2, LinearGradientMode.Vertical)
                G.FillRectangle(_BHighlightBrush, _OBRect)
        End Select

        Select Case CheckedState
            Case True
                G.FillRectangle(_LBCBrush, _IBRect)
            Case False
                G.FillRectangle(_IBBrush, _IBRect)
        End Select
        DrawText(New SolidBrush(TextColor), HorizontalAlignment.Left, 19, 1)
    End Sub
End Class
Class BCEvoTextBox
    Inherits TextBox

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer Or _
        ControlStyles.Opaque, True)
        BorderStyle = Windows.Forms.BorderStyle.FixedSingle
    End Sub
    Private B As Bitmap
    Private TBBrush, TBIBrush As LinearGradientBrush
    Private TBRect As Point()
    Private G As Graphics
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        G = e.Graphics
        G.Clear(Color.Black)
        DrawBorders(Pens.White, 0)
        DrawCorners(Color.Transparent, 0)
    End Sub
#Region " DrawBorders "

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal offset As Integer)
        DrawBorders(p1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle, ByVal offset As Integer)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen)
        DrawBorders(p1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        G.DrawRectangle(p1, x, y, width - 1, height - 1)
    End Sub

#End Region
#Region " DrawCorners "

    Private DrawCornersBrush As SolidBrush

    Protected Sub DrawCorners(ByVal c1 As Color, ByVal offset As Integer)
        DrawCorners(c1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle, ByVal offset As Integer)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height, offset)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawCorners(c1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawCorners(ByVal c1 As Color)
        DrawCorners(c1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal r1 As Rectangle)
        DrawCorners(c1, r1.X, r1.Y, r1.Width, r1.Height)
    End Sub
    Protected Sub DrawCorners(ByVal c1 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        If _NoRounding Then Return

        If _Transparent Then
            B.SetPixel(x, y, c1)
            B.SetPixel(x + (width - 1), y, c1)
            B.SetPixel(x, y + (height - 1), c1)
            B.SetPixel(x + (width - 1), y + (height - 1), c1)
        Else
            DrawCornersBrush = New SolidBrush(c1)
            G.FillRectangle(DrawCornersBrush, x, y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y, 1, 1)
            G.FillRectangle(DrawCornersBrush, x, y + (height - 1), 1, 1)
            G.FillRectangle(DrawCornersBrush, x + (width - 1), y + (height - 1), 1, 1)
        End If
    End Sub
    Private _Transparent As Boolean
    Property Transparent() As Boolean
        Get
            Return _Transparent
        End Get
        Set(ByVal value As Boolean)
            _Transparent = value
            If Not (IsHandleCreated OrElse _ControlMode) Then Return

            If Not value AndAlso Not BackColor.A = 255 Then
                Throw New Exception("Unable to change value to false while a transparent BackColor is in use.")
            End If

            SetStyle(ControlStyles.Opaque, Not value)
            SetStyle(ControlStyles.SupportsTransparentBackColor, value)

            InvalidateBitmap()
            Invalidate()
        End Set
    End Property
    Private Sub InvalidateBitmap()
        If _Transparent AndAlso _ControlMode Then
            If Width = 0 OrElse Height = 0 Then Return
            B = New Bitmap(Width, Height, PixelFormat.Format32bppPArgb)
            G = Graphics.FromImage(B)
        Else
            G = Nothing
            B = Nothing
        End If
    End Sub
    Private _BackColor As Boolean
    <Category("Misc")> _
    Overrides Property BackColor() As Color
        Get
            Return MyBase.BackColor
        End Get
        Set(ByVal value As Color)
            If value = MyBase.BackColor Then Return

            If Not IsHandleCreated AndAlso _ControlMode AndAlso value = Color.Transparent Then
                _BackColor = True
                Return
            End If

            MyBase.BackColor = value
            If Parent IsNot Nothing Then
                If Not _ControlMode Then Parent.BackColor = value

            End If
        End Set
    End Property
    Private _ControlMode As Boolean
    Protected Property ControlMode() As Boolean
        Get
            Return _ControlMode
        End Get
        Set(ByVal v As Boolean)
            _ControlMode = v

            Transparent = _Transparent
            If _Transparent AndAlso _BackColor Then BackColor = Color.Transparent

            InvalidateBitmap()
            Invalidate()
        End Set
    End Property
    Private _NoRounding As Boolean
    Property NoRounding() As Boolean
        Get
            Return _NoRounding
        End Get
        Set(ByVal v As Boolean)
            _NoRounding = v
            Invalidate()
        End Set
    End Property
#End Region
End Class
Class BCEvoTabControl
    Inherits TabControl

    Private Light As Color = Color.FromArgb(180, 180, 180)
    Private Lighter As Color = Color.FromArgb(230, 230, 230)
    Private DrawGradientBrush As LinearGradientBrush
    Private _ControlBColor As Color
    Private G As Graphics
    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer, True)
    End Sub
    Private TCBrush, TCIBrush As LinearGradientBrush, TCTBrush As New SolidBrush(Color.Empty), TCIGBrush As SolidBrush
    Private TCItemB, TCItemR, TCItemG As Rectangle
    Private TCTAT As New StringFormat
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        G = e.Graphics
        TCBrush = New LinearGradientBrush(ClientRectangle, Color.FromArgb(255, 55, 55, 55), Color.FromArgb(255, 22, 22, 22), LinearGradientMode.Vertical)
        TCIBrush = New LinearGradientBrush(ClientRectangle, Color.FromArgb(100, 0, 0), Color.FromArgb(60, 0, 0), LinearGradientMode.Vertical)
        TCIGBrush = New SolidBrush(Color.FromArgb(25, Color.White))
        TCTAT.LineAlignment = StringAlignment.Center
        TCTAT.Alignment = StringAlignment.Center

        G.FillRectangle(TCBrush, ClientRectangle)
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        For TabItemIndex As Integer = 0 To Me.TabCount - 1
            TCItemB = GetTabRect(TabItemIndex)

            'If TabItemIndex = 0 Then
            '    TCItemR = New Rectangle(10, TCItemB.Y + 1, TCItemB.Width - 18, TCItemB.Height - 5)
            '    TCItemG = New Rectangle(10, TCItemB.Y + 1, TCItemB.Width - 18, CInt(TCItemB.Height / 2 - 2))
            'Else
            '    TCItemR = New Rectangle(TCItemB.X + 8, TCItemB.Y + 1, TCItemB.Width - 17, TCItemB.Height - 5)
            '    TCItemG = New Rectangle(TCItemB.X + 8, TCItemB.Y + 1, TCItemB.Width - 17, CInt(TCItemB.Height / 2 - 2))
            'End If

            If TabItemIndex = SelectedIndex Then
            Else
            End If

            If SelectedIndex = TabItemIndex Then
                TCTBrush.Color = Color.FromArgb(255, 255, 255)
                '####################################
                'G.FillRectangle(TCIBrush, TCItemR)
                'G.FillRectangle(TCIGBrush, TCItemG) TabPage Outline + Gradient + Gloss
                'G.DrawRectangle(Pens.Black, TCItemR)
                '####################################
            Else
                TCTBrush.Color = Color.FromArgb(100, 100, 100)
            End If
            Try
                G.DrawString( _
                TabPages(TabItemIndex).Text, _
                Font, TCTBrush, New Rectangle(GetTabRect(TabItemIndex).Location + New Point(0, -2), GetTabRect(TabItemIndex).Size), TCTAT)
                TabPages(TabItemIndex).BackColor = Color.FromArgb(90, 90, 90)
            Catch : End Try
        Next
        DrawBorders(Pens.Black)
        DrawBorders(Pens.Black, 2)
    End Sub
#Region " DrawBorders "

    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal offset As Integer)
        DrawBorders(p1, 0, 0, Width, Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle, ByVal offset As Integer)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height, offset)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal offset As Integer)
        DrawBorders(p1, x + offset, y + offset, width - (offset * 2), height - (offset * 2))
    End Sub

    Protected Sub DrawBorders(ByVal p1 As Pen)
        DrawBorders(p1, 0, 0, Width, Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal r As Rectangle)
        DrawBorders(p1, r.X, r.Y, r.Width, r.Height)
    End Sub
    Protected Sub DrawBorders(ByVal p1 As Pen, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        G.DrawRectangle(p1, x, y, width - 1, height - 1)
    End Sub

#End Region

End Class

Class BCEvoGroupBox
    Inherits ThemeContainer154

    Sub New()
        ControlMode = True
        Header = 26
        TextColor = Color.Gray
        Size = New Size(205, 120)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub
    Private _ForeColor As Color
    Public Property TextColor() As Color
        Get
            Return _ForeColor
        End Get
        Set(ByVal value As Color)
            _ForeColor = value
        End Set
    End Property
    Private LBlend, LBlend2 As New ColorBlend
    Protected Overrides Sub PaintHook()
        LBlend.Positions = New Single() {0.0F, 0.15F, 0.85F, 1.0F}
        LBlend2.Positions = New Single() {0.0F, 0.15F, 0.5F, 0.85F, 1.0F}
        LBlend.Colors = New Color() {Color.Transparent, Color.Black, Color.Black, Color.Transparent}
        LBlend2.Colors = New Color() {Color.Transparent, Color.FromArgb(35, 35, 35), Color.FromArgb(45, 45, 45), Color.FromArgb(35, 35, 35), Color.Transparent}

        G.Clear(Color.FromArgb(24, 24, 24))

        If Text = Nothing Then
        Else
            DrawGradient(LBlend, 0, 23, Width, 1, 0.0F)
            DrawGradient(LBlend2, 0, 24, Width, 1, 0.0F)
        End If

        DrawBorders(Pens.Black, 3)
        DrawBorders(Pens.Black)

        G.DrawLine(New Pen(Color.FromArgb(48, 48, 48)), 1, 1, Width - 2, 1)

        DrawText(New SolidBrush(TextColor), HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

Class DotNetBarTabcontrol
    Inherits TabControl

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(44, 136)
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
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
        Try : SelectedTab.BackColor = Color.White : Catch : End Try
        G.Clear(Color.White)
        G.FillRectangle(New SolidBrush(Color.FromArgb(246, 248, 252)), New Rectangle(0, 0, ItemSize.Height + 4, Height))
        G.DrawLine(New Pen(Color.FromArgb(170, 187, 204)), New Point(ItemSize.Height + 3, 0), New Point(ItemSize.Height + 3, 999))
        For i = 0 To TabCount - 1
            If i = SelectedIndex Then
                Dim x2 As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), New Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1))
                Dim myBlend As New ColorBlend()
                myBlend.Colors = New Color() {Color.FromArgb(232, 232, 240), Color.FromArgb(232, 232, 240), Color.FromArgb(232, 232, 240)}
                myBlend.Positions = New Single() {0.0F, 0.5F, 1.0F}
                Dim lgBrush As New LinearGradientBrush(x2, Color.Black, Color.Black, 90.0F)
                lgBrush.InterpolationColors = myBlend
                G.FillRectangle(lgBrush, x2)
                G.DrawRectangle(New Pen(Color.FromArgb(170, 187, 204)), x2)

                G.SmoothingMode = SmoothingMode.HighQuality
                Dim p() As Point = {New Point(ItemSize.Height - 3, GetTabRect(i).Location.Y + 20), New Point(ItemSize.Height + 4, GetTabRect(i).Location.Y + 14), New Point(ItemSize.Height + 4, GetTabRect(i).Location.Y + 27)}
                G.FillPolygon(Brushes.White, p)
                G.DrawPolygon(New Pen(Color.FromArgb(170, 187, 204)), p)

                If ImageList IsNot Nothing Then
                    Try
                        If ImageList.Images(TabPages(i).ImageIndex) IsNot Nothing Then

                            G.DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Point(x2.Location.X + 8, x2.Location.Y + 6))
                            G.DrawString("  " & TabPages(i).Text, Font, Brushes.Black, x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                        Else
                            G.DrawString(TabPages(i).Text, New Font(Font.FontFamily, Font.Size, FontStyle.Bold), Brushes.Black, x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                        End If
                    Catch ex As Exception
                        G.DrawString(TabPages(i).Text, New Font(Font.FontFamily, Font.Size, FontStyle.Bold), Brushes.Black, x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                    End Try
                Else
                    G.DrawString(TabPages(i).Text, New Font(Font.FontFamily, Font.Size, FontStyle.Bold), Brushes.Black, x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                End If

                G.DrawLine(New Pen(Color.FromArgb(200, 200, 250)), New Point(x2.Location.X - 1, x2.Location.Y - 1), New Point(x2.Location.X, x2.Location.Y))
                G.DrawLine(New Pen(Color.FromArgb(200, 200, 250)), New Point(x2.Location.X - 1, x2.Bottom - 1), New Point(x2.Location.X, x2.Bottom))
            Else
                Dim x2 As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), New Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1))
                G.FillRectangle(New SolidBrush(Color.FromArgb(246, 248, 252)), x2)
                G.DrawLine(New Pen(Color.FromArgb(170, 187, 204)), New Point(x2.Right, x2.Top), New Point(x2.Right, x2.Bottom))
                If ImageList IsNot Nothing Then
                    Try
                        If ImageList.Images(TabPages(i).ImageIndex) IsNot Nothing Then
                            G.DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Point(x2.Location.X + 8, x2.Location.Y + 6))
                            G.DrawString("  " & TabPages(i).Text, Font, Brushes.DimGray, x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                        Else
                            G.DrawString(TabPages(i).Text, Font, Brushes.DimGray, x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                        End If
                    Catch ex As Exception
                        G.DrawString(TabPages(i).Text, Font, Brushes.DimGray, x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                    End Try
                Else
                    G.DrawString(TabPages(i).Text, Font, Brushes.DimGray, x2, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                End If
            End If
        Next

        e.Graphics.DrawImage(B.Clone, 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Dim OldIndex As Integer

    Private _Speed As Integer = 5
    Property Speed() As Integer
        Get
            Return _Speed
        End Get
        Set(ByVal value As Integer)
            If value > 20 Or value < -20 Then
                MsgBox("Speed needs to be in between -20 and 20.")
            Else
                _Speed = value
            End If
        End Set
    End Property

    Sub DoAnimationScrollLeft(ByVal Control1 As Control, ByVal Control2 As Control)
        Dim G As Graphics = Control1.CreateGraphics()
        Dim P1 As New Bitmap(Control1.Width, Control1.Height)
        Dim P2 As New Bitmap(Control2.Width, Control2.Height)
        Control1.DrawToBitmap(P1, New Rectangle(0, 0, Control1.Width, Control1.Height))
        Control2.DrawToBitmap(P2, New Rectangle(0, 0, Control2.Width, Control2.Height))

        For Each c As Control In Control1.Controls
            c.Hide()
        Next

        Dim Slide As Integer = Control1.Width - (Control1.Width Mod _Speed)

        Dim a As Integer
        For a = 0 To Slide Step _Speed
            G.DrawImage(P1, New Rectangle(a, 0, Control1.Width, Control1.Height))
            G.DrawImage(P2, New Rectangle(a - Control2.Width, 0, Control2.Width, Control2.Height))
        Next
        a = Control1.Width
        G.DrawImage(P1, New Rectangle(a, 0, Control1.Width, Control1.Height))
        G.DrawImage(P2, New Rectangle(a - Control2.Width, 0, Control2.Width, Control2.Height))

        SelectedTab = Control2

        For Each c As Control In Control2.Controls
            c.Show()
        Next

        For Each c As Control In Control1.Controls
            c.Show()
        Next
    End Sub

    Protected Overrides Sub OnSelecting(ByVal e As System.Windows.Forms.TabControlCancelEventArgs)
        If OldIndex < e.TabPageIndex Then
            DoAnimationScrollRight(TabPages(OldIndex), TabPages(e.TabPageIndex))
        Else
            DoAnimationScrollLeft(TabPages(OldIndex), TabPages(e.TabPageIndex))
        End If
    End Sub

    Protected Overrides Sub OnDeselecting(ByVal e As System.Windows.Forms.TabControlCancelEventArgs)
        OldIndex = e.TabPageIndex
    End Sub

    Sub DoAnimationScrollRight(ByVal Control1 As Control, ByVal Control2 As Control)
        Dim G As Graphics = Control1.CreateGraphics()
        Dim P1 As New Bitmap(Control1.Width, Control1.Height)
        Dim P2 As New Bitmap(Control2.Width, Control2.Height)
        Control1.DrawToBitmap(P1, New Rectangle(0, 0, Control1.Width, Control1.Height))
        Control2.DrawToBitmap(P2, New Rectangle(0, 0, Control2.Width, Control2.Height))

        For Each c As Control In Control1.Controls
            c.Hide()
        Next

        Dim Slide As Integer = Control1.Width - (Control1.Width Mod _Speed)

        Dim a As Integer
        For a = 0 To -Slide Step -_Speed
            G.DrawImage(P1, New Rectangle(a, 0, Control1.Width, Control1.Height))
            G.DrawImage(P2, New Rectangle(a + Control2.Width, 0, Control2.Width, Control2.Height))
        Next
        a = Control1.Width
        G.DrawImage(P1, New Rectangle(a, 0, Control1.Width, Control1.Height))
        G.DrawImage(P2, New Rectangle(a + Control2.Width, 0, Control2.Width, Control2.Height))

        SelectedTab = Control2

        For Each c As Control In Control2.Controls
            c.Show()
        Next

        For Each c As Control In Control1.Controls
            c.Show()
        Next
    End Sub
End Class