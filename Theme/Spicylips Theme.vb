Imports System, System.Collections
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms

'********************
'Creator: Spicylips
'Site: ************
'Created: 01/17/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackTheme
    Inherits ThemeContainer154
    Sub New()
        TransparencyKey = Color.Fuchsia
        Height = 30
        BackColor = Color.FromArgb(20, 20, 20)
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(20, 20, 20))

        Dim T As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(9, 9, 9), Color.FromArgb(15, 15, 15))
        G.FillRectangle(T, ClientRectangle)
        G.FillRectangle(New SolidBrush(Color.FromArgb(20, Color.White)), ClientRectangle)
        DrawBorders(New Pen(Color.FromArgb(7, 7, 7)), 0, 0, Width, Height)

        G.FillRectangle(New SolidBrush(Color.FromArgb(22, 22, 22)), 12, 22, Width - 24, Height - 27)
        DrawBorders(New Pen(Color.FromArgb(7, 7, 7)), 12, 22, Width - 24, Height - 27)

        DrawText(New SolidBrush(Color.White), HorizontalAlignment.Center, 0, 2)
        DrawCorners(TransparencyKey)

    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/17/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackCloseButton
    Inherits ThemeControl154
    Sub New()
        Size = New Size(18, 18)
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(101, 101, 101))
        Select Case State
            Case 0
                DrawGradient(Color.FromArgb(40, 40, 40), Color.FromArgb(4, 4, 4), 0, 0, Width, Height, 90S)
            Case 1
                DrawGradient(Color.FromArgb(40, 40, 40), Color.FromArgb(4, 4, 4), 0, 0, Width, Height, 90S)
            Case 2
                DrawGradient(Color.FromArgb(4, 4, 4), Color.FromArgb(78, 0, 0), 0, 0, Width, Height, 90S)
        End Select
        DrawBorders(Pens.Black)
        DrawBorders(Pens.Black, 2)
        DrawCorners(Color.FromArgb(30, 30, 30), ClientRectangle)
        G.DrawString("X", Font, Brushes.White, 3, 3)
        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/17/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackMinimizeButton
    Inherits ThemeControl154
    Sub New()
        Size = New Size(18, 18)
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(101, 101, 101))
        Select Case State
            Case 0
                DrawGradient(Color.FromArgb(40, 40, 40), Color.FromArgb(4, 4, 4), 0, 0, Width, Height, 90S)
            Case 1
                DrawGradient(Color.FromArgb(40, 40, 40), Color.FromArgb(4, 4, 4), 0, 0, Width, Height, 90S)
            Case 2
                DrawGradient(Color.FromArgb(4, 4, 4), Color.FromArgb(3, 65, 0), 0, 0, Width, Height, 90S)
        End Select
        DrawBorders(Pens.Black)
        DrawBorders(Pens.Black, 2)
        DrawCorners(Color.FromArgb(30, 30, 30), ClientRectangle)
        G.DrawString("_", Font, Brushes.White, 4, -2)
        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/17/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackButton
    Inherits ThemeControl154
    Sub New()
        Size = New Size(75, 23)
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(1, 1, 1))
        Select Case State
            Case 0
                DrawGradient(Color.FromArgb(40, 40, 40), Color.FromArgb(28, 28, 28), 0, 0, Width, Height, 90S)
            Case 1
                DrawGradient(Color.FromArgb(40, 40, 40), Color.FromArgb(28, 28, 28), 0, 0, Width, Height, 90S)
            Case 2
                DrawGradient(Color.FromArgb(4, 4, 4), Color.FromArgb(20, 20, 20), 0, 0, Width, Height, 90S)
        End Select
        G.FillRectangle(New SolidBrush(Color.FromArgb(6, Color.White)), 0, 0, Width, 12)
        DrawBorders(Pens.Black)
        DrawBorders(Pens.Black, 2)
        DrawCorners(Color.FromArgb(20, 20, 20), ClientRectangle)
        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/17/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackButton1
    Inherits ThemeControl154
    Sub New()
        Size = New Size(75, 23)
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(7, 7, 7))
        Select Case State
            Case 0
                DrawGradient(Color.FromArgb(79, 79, 79), Color.FromArgb(4, 4, 4), 0, 0, Width, Height, 90S)
            Case 1
                DrawGradient(Color.FromArgb(79, 79, 79), Color.FromArgb(4, 4, 4), 0, 0, Width, Height, 90S)
            Case 2
                DrawGradient(Color.FromArgb(4, 4, 4), Color.FromArgb(79, 79, 79), 0, 0, Width, Height, 90S)
        End Select
        G.FillRectangle(New SolidBrush(Color.FromArgb(6, Color.White)), 0, 0, Width, 12)
        DrawBorders(Pens.Black)
        DrawBorders(Pens.Black, 2)
        DrawCorners(Color.FromArgb(30, 30, 30), ClientRectangle)
        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)

    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/17/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackButton2
    Inherits ThemeControl154
    Sub New()
        Size = New Size(75, 23)
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Select Case State
            Case 0
                DrawGradient(Color.FromArgb(40, 40, 40), Color.FromArgb(4, 4, 4), 0, 0, Width, Height, 90S)
            Case 1
                DrawGradient(Color.FromArgb(40, 40, 40), Color.FromArgb(4, 4, 4), 0, 0, Width, Height, 90S)
            Case 2
                DrawGradient(Color.FromArgb(4, 4, 4), Color.FromArgb(40, 40, 40), 0, 0, Width, Height, 90S)
        End Select
        G.FillRectangle(New SolidBrush(Color.FromArgb(6, Color.White)), 0, 0, Width, 12)
        DrawBorders(Pens.Black)
        DrawBorders(Pens.Black, 2)
        DrawCorners(Color.FromArgb(20, 20, 20), ClientRectangle)
        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/17/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackCheckBox
    Inherits ThemeControl154
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
        Size = New Size(158, 16)
        MinimumSize = New Size(16, 16)
        MaximumSize = New Size(600, 16)
        CheckedState = False
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        Dim FontColor As Color
        FontColor = Color.White

        G.Clear(Color.FromArgb(22, 22, 22))
        Select Case CheckedState
            Case True
                DrawGradient(Color.FromArgb(40, 40, 40), Color.FromArgb(30, 30, 30), 3, 3, 9, 9, 90S)
                DrawGradient(Color.FromArgb(65, 65, 65), Color.FromArgb(20, 20, 20), 4, 4, 7, 7, 90S)
            Case False
                DrawGradient(Color.FromArgb(34, 34, 34), Color.FromArgb(34, 34, 34), 0, 0, 15, 15, 90S)
        End Select
        G.DrawRectangle(Pens.Black, 0, 0, 14, 14)
        G.DrawRectangle(Pens.Black, 2, 2, 10, 10)
        DrawText(Brushes.White, HorizontalAlignment.Left, 17, 0)
    End Sub
    Sub changeCheck() Handles Me.Click
        Select Case CheckedState
            Case True
                CheckedState = False
            Case False
                CheckedState = True
        End Select
    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/17/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackSeperator
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
                LockWidth = 12
            Else
                LockHeight = 12
                LockWidth = 0
            End If

            Invalidate()
        End Set
    End Property
    Sub New()
        LockHeight = 12
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(22, 22, 22))

        If _Orientation = Windows.Forms.Orientation.Horizontal Then
            G.DrawLine(New Pen(Color.FromArgb(2, 2, 2)), 0, 6, Width, 6)
            G.DrawLine(New Pen(Color.FromArgb(22, 22, 22)), 0, 7, Width, 7)
        Else
            G.DrawLine(New Pen(Color.FromArgb(2, 2, 2)), 6, 0, 6, Height)
            G.DrawLine(New Pen(Color.FromArgb(22, 22, 22)), 7, 0, 7, Height)
        End If

    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/17/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackPanel
    Inherits ThemeControl154
    Sub New()
        Size = New Size(120, 120)
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(26, 26, 26))
        DrawBorders(New Pen(New SolidBrush(Color.FromArgb(7, 7, 7))), ClientRectangle)
        DrawCorners(Color.FromArgb(22, 22, 22), ClientRectangle)
        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/18/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackGroupBox
    Inherits ThemeContainer154
    Sub New()
        ControlMode = True
        BackColor = Color.FromArgb(20, 20, 20)
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(26, 26, 26))

        Dim T As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(9, 9, 9), Color.FromArgb(15, 15, 15))
        G.FillRectangle(T, ClientRectangle)
        G.FillRectangle(New SolidBrush(Color.FromArgb(20, Color.White)), ClientRectangle)
        DrawText(Brushes.White, HorizontalAlignment.Center, -1, -1)
        G.FillRectangle(New SolidBrush(Color.FromArgb(22, 22, 22)), 10, 20, Width - 21, Height)
        DrawBorders(New Pen(Color.FromArgb(7, 7, 7)), 0, 0, Width, Height)
        DrawCorners(Color.FromArgb(22, 22, 22), ClientRectangle)
        DrawBorders(Pens.Black, 10, 20, Width - 21, Height)
    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/18/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackProgressBar
    Inherits ThemeControl154

    Private Blend As ColorBlend

    Sub New()
        Blend = New ColorBlend
        Blend.Colors = New Color() {Color.FromArgb(37, 37, 37), Color.FromArgb(64, 66, 68), Color.FromArgb(64, 66, 68), Color.FromArgb(37, 37, 37)}
        Blend.Positions = New Single() {0.0F, 0.4F, 0.6F, 1.0F}

        IsAnimated = True

        P1 = New Pen(Color.FromArgb(32, 32, 32))
        P2 = New Pen(Color.FromArgb(15, Color.White))
        P3 = Pens.Black
        P4 = Pens.Black

        B1 = New SolidBrush(Color.FromArgb(50, 50, 50))
        B2 = New SolidBrush(Color.FromArgb(37, 37, 37))
        B3 = New SolidBrush(Color.FromArgb(13, Color.White))

        C1 = Color.FromArgb(8, 8, 8)
        C2 = Color.FromArgb(23, 23, 23)
    End Sub

    Private GlowPosition As Single = -1.0F
    Protected Overrides Sub OnAnimation()
        GlowPosition += 0.05F
        If GlowPosition >= 1.0F Then GlowPosition = -1.0F
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

    Private P1, P2, P3, P4 As Pen
    Private B1, B2, B3 As SolidBrush
    Private C1, C2 As Color

    Private R1 As Rectangle

    Private Progress As Integer
    Protected Overrides Sub PaintHook()
        DrawBorders(P1, 1)
        G.FillRectangle(B1, 0, 0, Width, 8)

        DrawGradient(C1, C2, 2, 2, Width - 4, Height - 4, 90.0F)

        Progress = CInt((_Value / _Maximum) * Width)

        If Not Progress = 0 Then
            R1 = New Rectangle(3, 3, Progress - 6, Height - 6)

            G.SetClip(R1)
            G.FillRectangle(B2, 0, 0, Progress, Height)

            DrawGradient(Blend, CInt(GlowPosition * Progress), 0, Progress, Height, 0.0F)
            DrawBorders(P2, 3, 3, Progress - 6, Height - 6)

            G.FillRectangle(B3, 3, 3, Width - 6, 5)
            G.ResetClip()
        End If

        DrawBorders(P3, 2)
        DrawBorders(P4)
    End Sub
End Class

'********************
'Creator: Spicylips
'Site: hackforums.net
'Created: 01/18/2102
'Changed: N / A
'Version: 1.0.0.0
'Theme Base: 1.5.4
'********************
Class BlackTextBox
    Inherits ThemeControl154
    Private WithEvents Txt As New TextBox
    Sub New()
        Txt.TextAlign = HorizontalAlignment.Left
        Txt.BorderStyle = BorderStyle.None
        Txt.Location = New Point(2, 2)
        Controls.Add(Txt)
        Txt.Text = " "
        Text = " "
        Size = New Size(100, 20)
        LockHeight = 20
    End Sub
    Protected Overrides Sub ColorHook()
        Txt.ForeColor = Color.White
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(9, 9, 9))
        Txt.BackColor = Color.FromArgb(9, 9, 9)
        G.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1)
        G.DrawRectangle(Pens.Black, 1, 1, Width - 3, Height - 3)
        Txt.Size = New Size(Me.Width - 4, Txt.Height - 4)
    End Sub
    Sub TextBox_TextChanged() Handles Txt.TextChanged
        Text = Txt.Text
    End Sub
    Sub PropertyTextChanged() Handles MyBase.TextChanged
        Txt.Text = Text
    End Sub
End Class