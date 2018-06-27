'------------------
'Creator: Androcir
'Site:
'Created: 15/9/2011
'Changed:
'Version: 1.0.0
'------------------
 
 
'This Theme Requires Aeonhacks ThemeBase 1.5.1
'Credits of ThemeBase 1.5.1 goes to Aeonhack.




Class CCTheme
    Inherits ThemeContainer151
    Sub New()
        TransparencyKey = Color.Fuchsia
        MoveHeight = 24
    End Sub
    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()

        G.Clear(Color.Crimson)
        DrawGradient(Color.Brown, Color.Firebrick, 0, 0, Width, 24, 315S)
        DrawGradient(Color.Brown, Color.Crimson, 0, 24, Width, Height, 67S)
        DrawCorners(Color.RosyBrown, ClientRectangle)

        DrawBorders(New Pen(New SolidBrush(Color.RosyBrown)))
        DrawBorders(New Pen(New SolidBrush(Color.RosyBrown)), 1)
        DrawBorders(New Pen(New SolidBrush(Color.RosyBrown)), 2)

        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)

    End Sub
End Class
Class CCButton
    Inherits ThemeControl151

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()

        G.Clear(Color.FromArgb(192, 0, 0))
        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)
        DrawBorders(New Pen(New SolidBrush(Color.LightGray)))
        DrawBorders(New Pen(New SolidBrush(Color.RosyBrown)), 1)
        DrawCorners(Color.RosyBrown, ClientRectangle)
        If State = MouseState.Over Then
            DrawBorders(New Pen(New SolidBrush(Color.Goldenrod)))
            DrawBorders(New Pen(New SolidBrush(Color.Goldenrod)), 1)
            DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)
        ElseIf State = MouseState.Down Then
            G.Clear(Color.FromArgb(204, 0, 5))
            DrawBorders(New Pen(New SolidBrush(Color.Gold)))
            DrawBorders(New Pen(New SolidBrush(Color.Gold)), 1)
            DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)
        End If


    End Sub
End Class

Class CCProgressBar
    Inherits ThemeControl151
    Sub New()

    End Sub
    Private _maximum As Integer
    Public Property maximum() As Integer
        Get
            Return _maximum
        End Get
        Set(ByVal v As Integer)
            If v < 1 Then v = 1
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

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()

        G.Clear(Color.FromArgb(192, 0, 0))
        DrawBorders(New Pen(New SolidBrush(Color.RosyBrown)))
        DrawBorders(New Pen(New SolidBrush(Color.RosyBrown)), 1)

        G.FillRectangle(Brushes.White, 0, 0, CInt((_value / _maximum) * Width), Height)
        G.FillRectangle(Brushes.WhiteSmoke, 0, CInt((Height / 4)), CInt((_value / _maximum) * Width), Height)
        G.FillRectangle(Brushes.LightGray, 0, CInt((Height / 3)), CInt((_value / _maximum) * Width), Height)
        G.FillRectangle(Brushes.LightGray, 0, CInt((Height / 3)), CInt((_value / _maximum) * Width), Height)
        G.FillRectangle(Brushes.Silver, 0, CInt((Height / 2)), CInt((_value / _maximum) * Width), Height)
        G.DrawRectangle(Pens.RosyBrown, 0, 0, Width - 1, Height - 1)

    End Sub
End Class
Class CCCheckBox
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
        Size = New Size(90, 15)
        MinimumSize = New Size(16, 16)
        MaximumSize = New Size(600, 16)


        CheckedState = False
    End Sub
    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(192, 0, 0))

        Dim Grad As Integer
        Dim FontColor As Color
        Select Case 1
            Case True
                Grad = 51
                FontColor = Color.White
            Case False
                Grad = 200
                FontColor = Color.White
        End Select

        Select Case CheckedState
            Case True
                'DrawGradient(Color.FromArgb(62, 62, 62), Color.FromArgb(38, 38, 38), 0, 0, 15, 15, 90S)
                DrawGradient(Color.White, Color.Silver, 3, 3, 9, 9, 90S)
                DrawGradient(Color.WhiteSmoke, Color.Gray, 4, 4, 7, 7, 90S)
            Case False
                DrawGradient(Color.FromArgb(192, 0, 0), Color.FromArgb(216, 216, 216), 0, 0, 15, 15, 90S)
        End Select
        G.DrawRectangle(Pens.RosyBrown, 0, 0, 14, 14)
        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 0)
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
End Class
Class CCXButton
    Inherits ThemeControl151

    Sub New()
        SetColor("BackColor", Color.FromArgb(192, 0, 0))
        SetColor("TextColor", Color.Green)
        Size = New Size(19, 12)
    End Sub


    Dim C1, T1 As Color
    Dim C2, C3, C4, C5, C6, P1, P2, B1 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("BackColor")
        T1 = GetColor("TextColor")
    End Sub


    Protected Overrides Sub PaintHook()
        G.Clear(C1)
        Select Case State
            Case 0 'None
                DrawGradient(Color.FromArgb(192, 0, 0), Color.FromArgb(192, 0, 0), ClientRectangle, 90S)
                Cursor = Cursors.Arrow

        End Select

        DrawBorders(New Pen(New SolidBrush(Color.RosyBrown)))
        DrawBorders(New Pen(New SolidBrush(Color.RosyBrown)))
        DrawBorders(New Pen(New SolidBrush(Color.RosyBrown)), New Rectangle(1, 2, -2, Height))
        DrawText(Brushes.White, "X", HorizontalAlignment.Center, 0, 0)

    End Sub
End Class