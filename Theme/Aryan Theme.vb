Imports System.Drawing.Drawing2D

'Theme
Class AryanTheme
    Inherits ThemeContainer151

    Sub New()
        TransparencyKey = Color.Fuchsia
        MoveHeight = 35

        SetColor("BackColor", Color.FromArgb(25, 25, 25))
        SetColor("BorderInner", Color.FromArgb(25, 25, 25))
        SetColor("BorderColor", Color.FromArgb(25, 25, 25))
        SetColor("TextColor", Color.Black)
    End Sub


    Dim C1, BC, BA, T1 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("BackColor")
        BC = GetColor("BorderColor")
        BA = GetColor("BorderInner")
        T1 = GetColor("TextColor")
    End Sub


    Protected Overrides Sub PaintHook()
        G.Clear(C1)
        DrawGradient(Color.FromArgb(255, 0, 0), Color.FromArgb(255, 0, 0), New Rectangle(0, 0, Width, 35), 90S)

        Dim T As New HatchBrush(HatchStyle.Trellis, Color.FromArgb(25, 25, 25), Color.FromArgb(35, 35, 35))
        G.FillRectangle(T, New Rectangle(11, 25, Width - 23, Height - 36))

        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(22, 22, 22))), New Rectangle(11, 25, Width - 23, Height - 36))
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(40, 40, 40))), New Rectangle(12, 26, Width - 25, Height - 38))
        DrawCorners(Color.FromArgb(40, 40, 40), New Rectangle(11, 25, Width - 22, Height - 35))

        DrawBorders(New Pen(New SolidBrush(BA)), 1)
        DrawBorders(New Pen(New SolidBrush(BC)))
        DrawCorners(TransparencyKey)
        DrawText(New SolidBrush(T1), HorizontalAlignment.Left, 15, -3)
    End Sub
End Class

'Button
Class AryanButton
    Inherits ThemeControl151

    Sub New()
        SetColor("BackColor", Color.FromArgb(25, 25, 25))
        SetColor("TextColor", Color.Red)
    End Sub


    Dim C1, T1 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("BackColor")
        T1 = GetColor("TextColor")
    End Sub


    Protected Overrides Sub PaintHook()
        G.Clear(C1)
        Select Case State
            Case 0 'None
                DrawGradient(Color.FromArgb(25, 25, 25), Color.FromArgb(50, 50, 50), ClientRectangle, 90S)
                Cursor = Cursors.Arrow
            Case 1 'Down
                DrawGradient(Color.FromArgb(50, 50, 50), Color.FromArgb(42, 42, 42), ClientRectangle, 90S)
                Cursor = Cursors.Hand
            Case 2 'Over
                DrawGradient(Color.FromArgb(42, 42, 42), Color.FromArgb(50, 50, 50), ClientRectangle, 90S)
                Cursor = Cursors.Hand
        End Select
        DrawBorders(New Pen(New SolidBrush(Color.FromArgb(59, 59, 59))), 1)
        DrawBorders(New Pen(New SolidBrush(Color.FromArgb(25, 25, 25))))
        DrawCorners(Color.FromArgb(35, 35, 35))
        DrawText(New SolidBrush(T1), HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

'CheckBox
Class AryanCheckBox
    Inherits ThemeControl151
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
        Size = New Size(100, 15)
        MinimumSize = New Size(16, 16)
        MaximumSize = New Size(600, 16)
        CheckedState = False
        SetColor("CheckBorderOut", Color.FromArgb(25, 25, 25))
        SetColor("CheckBorderIn", Color.FromArgb(59, 59, 59))
        SetColor("TextColor", Color.Red)
        SetColor("CheckBackground1", Color.FromArgb(25, 25, 25))
        SetColor("CheckBackground2", Color.Red)
        SetColor("CheckForecolor1", Color.FromArgb(25, 25, 25))
        SetColor("CheckForecolor2", Color.FromArgb(25, 25, 25))
        SetColor("ColorUncheck", Color.FromArgb(35, 35, 35))
        SetColor("BackColor", Color.FromArgb(25, 25, 25))
    End Sub
    Dim C1, C2, C3, C4, C5, C6, P1, P2, B1 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("CheckBackground1")
        C2 = GetColor("CheckBackground2")
        C3 = GetColor("CheckForecolor1")
        C4 = GetColor("CheckForecolor2")
        C5 = GetColor("ColorUncheck")
        C6 = GetColor("BackColor")
        P1 = GetColor("CheckBorderOut")
        P2 = GetColor("CheckBorderIn")
        B1 = GetColor("TextColor")
    End Sub
    Protected Overrides Sub PaintHook()
        G.Clear(C6)

        Dim R As New HatchBrush(HatchStyle.Trellis, Color.FromArgb(25, 25, 25), Color.FromArgb(35, 35, 35))
        G.FillRectangle(R, ClientRectangle)

        Select Case CheckedState
            Case True
                DrawGradient(C1, C2, 3, 3, 9, 9, 90S)
                DrawGradient(C3, C4, 4, 4, 7, 7, 90S)
            Case False
                DrawGradient(C5, C5, 0, 0, 15, 15, 90S)
        End Select
        G.DrawRectangle(New Pen(New SolidBrush(P1)), 0, 0, 14, 14)
        G.DrawRectangle(New Pen(New SolidBrush(P2)), 1, 1, 12, 12)
        DrawText(New SolidBrush(B1), 17, 0)
        DrawCorners(C6, New Rectangle(0, 0, 13, 13))
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
Class AryanProgessBar
    Inherits ThemeControl151
    Private _maximum As Integer
    Public Property maximum() As Integer
        Get
            Return _maximum
        End Get
        Set(ByVal v As Integer)
            If v < 100 Then v = 100
            If v < _value Then _value = v


            _maximum = v
            Invalidate()
        End Set
    End Property
    Private _value As Integer
    Public Property value() As Integer
        Get
            Return _value
        End Get
        Set(ByVal v As Integer)
            If v > _maximum Then v = maximum
            _value = v
            Invalidate()
        End Set
    End Property
    Sub New()
        SetColor("BackColor", Color.FromArgb(25, 25, 25))
    End Sub
    Dim c1 As Color
    Protected Overrides Sub ColorHook()
        c1 = GetColor("BackColor")
        BackColor = c1
    End Sub

    Protected Overrides Sub PaintHook()

        G.Clear(Color.FromArgb(25, 25, 25))



        G.FillRectangle(Brushes.Red, 0, 0, CInt((_value / _maximum) * Width), CInt((Height / 3)))
        G.FillRectangle(Brushes.Red, 0, CInt((Height / 3)), CInt((_value / _maximum) * Width), Height)
        G.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1)

    End Sub
End Class