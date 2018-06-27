Class BorderTheme
    Inherits ThemeContainer152


    Protected Overrides Sub ColorHook()

    End Sub
    Private hb1 As Brush
    Protected Overrides Sub PaintHook()
        Dim hb2 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.LightUpwardDiagonal, Color.FromArgb(35, 35, 35))
        G.Clear(BackColor)
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(71, 71, 71))), New Rectangle(0, 0, Width, Height))
        G.FillRectangle(New SolidBrush(Color.FromArgb(5, 5, 5)), New Rectangle(0, 0, Width, Height))

        G.DrawString(Text, Font, Brushes.Black, New Point(10, 9))
        G.DrawString(Text, Font, Brushes.White, New Point(8, 6))

        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))

        G.FillRectangle(New SolidBrush(Color.FromArgb(55, Color.White)), New Rectangle(0, 0, Width - 1, Height))
        G.FillRectangle(New SolidBrush(Color.FromArgb(5, Color.White)), New Rectangle(0, 0, Width - 1, 17))
        G.DrawRectangle(New Pen(New SolidBrush(Color.Black)), New Rectangle(11, 28, Width - 23, Height - 41))
        G.FillRectangle(New SolidBrush(Color.FromArgb(15, 15, 15)), New Rectangle(12, 29, Width - 24, Height - 42))

        DrawCorners(Color.Magenta)
    End Sub
End Class
Class BorderButton
    Inherits ThemeControl152
    Protected Overrides Sub ColorHook()
    End Sub
    Private B1 As Brush 'Gloss
    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Select Case State
            Case MouseState.None
                G.FillRectangle(New SolidBrush(Color.FromArgb(35, 35, 35)), ClientRectangle)
            Case MouseState.Over
                G.FillRectangle(New SolidBrush(Color.FromArgb(30, 30, 30)), ClientRectangle)
            Case MouseState.Down
                G.FillRectangle(New SolidBrush(Color.FromArgb(38, 38, 38)), ClientRectangle)
        End Select
        G.DrawString(Text, Font, Brushes.White, New Point(CInt((Me.Width / 2) - (Measure(Text).Width / 2)), CInt((Me.Height / 2) - (Measure(Text).Height / 2))))
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))
        DrawCorners(Color.FromArgb(15, 15, 15))
    End Sub
End Class
Class BorderButtonSubText
    Inherits ThemeControl152
    Protected Overrides Sub ColorHook()
    End Sub
    Private _subtext As String
    Public Property TextSub() As String
        Get
            Return _subtext
        End Get
        Set(ByVal value As String)
            _subtext = value
            Invalidate()
        End Set
    End Property
    Private B1 As Brush
    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Select Case State
            Case MouseState.None
                G.FillRectangle(New SolidBrush(Color.FromArgb(35, 35, 35)), ClientRectangle)
            Case MouseState.Over
                G.FillRectangle(New SolidBrush(Color.FromArgb(30, 30, 30)), ClientRectangle)
            Case MouseState.Down
                G.FillRectangle(New SolidBrush(Color.FromArgb(38, 38, 38)), ClientRectangle)
        End Select
        G.DrawString(Text, Font, Brushes.White, New Point(6, 8))
        Dim SubFont As Font = New Font(DefaultFont.FontFamily, Font.Size - 1)
        G.DrawString(_subtext, SubFont, New SolidBrush(Color.FromArgb(48, 48, 48)), New Point(6, 21))
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))
        DrawCorners(Color.FromArgb(15, 15, 15))
    End Sub
End Class
Class BorderGroupBox
    Inherits ThemeContainer152
    Private C1 As Color
    Private C2, C3 As Color
    Private P1 As Pen
    Private B1 As Brush
    Private B2 As Brush
    Private B5 As Brush
    Private HB1 As Brush
    Sub New()
        ControlMode = True
        Size = New Size(205, 95)
    End Sub

    Private _subtext As String
    Public Property TextSub() As String
        Get
            Return _subtext
        End Get
        Set(ByVal value As String)
            _subtext = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub ColorHook()
        C1 = Color.FromArgb(15, 15, 15)
        C2 = Color.FromArgb(50, 50, 50)
        C3 = Color.FromArgb(0, 0, 0)
        P1 = Pens.Black
        B1 = New SolidBrush(Color.FromArgb(60, Color.White))
        B2 = New SolidBrush(Color.White)
        B5 = New SolidBrush(Color.White)

    End Sub

    Protected Overrides Sub PaintHook()
        Dim hb1 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(15, 15, 15))
        Dim hb2 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(35, 35, 35))
        Dim hb3 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(0, 0, 0))
        G.Clear(C1)
        G.FillRectangle(New SolidBrush(Color.Transparent), 0, 0, Width, Height)
        G.FillRectangle(New SolidBrush(Color.Transparent), 0, 0, Width, 15)
        G.DrawRectangle(P1, 0, 5, Width - 1, CInt(Height - 16))
        G.FillRectangle(New SolidBrush(Color.FromArgb(80, 35, 35, 35)), 1, 5, Width, 40)
        G.FillRectangle(New SolidBrush(Color.Transparent), 5, 1, Width - 6, 10)
        G.DrawRectangle(P1, 0, 5, Width - 1, 40)
        G.DrawString(Text, Font, Brushes.White, New Point(6, 12))
        Dim SubFont As Font = New Font(DefaultFont.FontFamily, Font.Size - 1)
        G.DrawString(_subtext, SubFont, New SolidBrush(Color.FromArgb(48, 48, 48)), New Point(6, 26))
    End Sub
End Class

