Imports System.Drawing.Drawing2D
Class PatrickTheme
    Inherits ThemeContainer151
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
    End Sub

    Protected Overrides Sub PaintHook()
        Dim hb2 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(15, 15, 15))
        G.Clear(BackColor)
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(71, 71, 71))), New Rectangle(0, 0, Width, 20))
        G.FillRectangle(hb2, New Rectangle(0, 0, Width, 20))
        For i = 1 To 30
            G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(CInt(255 / (i * 8)), Color.Black))), 0, 20 + i, Width, 20 + i)
        Next
        Dim hbi As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(15, 15, 15))
        G.FillRectangle(hbi, New Rectangle(0, 20, Width, 20))
        G.DrawString(Text, Font, Brushes.DarkGray, New Point(5, 5))
        Dim SubFont As Font = New Font(DefaultFont.FontFamily, Font.Size - 1)
        G.DrawString(_subtext, SubFont, New SolidBrush(Color.FromArgb(130, 130, 130)), New Point(8, 21))
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))
        DrawCorners(Color.Magenta)
    End Sub
End Class
'Buttons
Class PatrickButton
    Inherits ThemeControl151
    Protected Overrides Sub ColorHook()
    End Sub
    Private B1 As Brush
    Protected Overrides Sub PaintHook()
        Dim hb1 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(15, 15, 15))
        Dim hb2 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(35, 35, 35))
        Dim hb3 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(0, 0, 0))
        B1 = New SolidBrush(Color.FromArgb(50, Color.White))
        G.Clear(BackColor)
        Select Case State
            Case MouseState.None
                G.FillRectangle(hb1, ClientRectangle)
            Case MouseState.Over
                G.FillRectangle(hb2, ClientRectangle)
            Case MouseState.Down
                G.FillRectangle(hb3, ClientRectangle)
        End Select
        G.DrawString(Text, Font, Brushes.White, New Point(CInt((Me.Width / 2) - (Measure(Text).Width / 2)), CInt((Me.Height / 2) - (Measure(Text).Height / 2))))
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))
        DrawCorners(Color.White)
    End Sub
End Class
Class PatrickTopButton
    Inherits ThemeControl151
    Protected Overrides Sub ColorHook()
    End Sub
    Protected Overrides Sub PaintHook()
        Dim hb1 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(35, 35, 35))
        Dim hb2 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(35, 35, 35))
        Dim hb3 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(0, 0, 0))
        G.Clear(BackColor)
        Select Case State
            Case MouseState.None
                G.FillRectangle(hb1, ClientRectangle)
            Case MouseState.Over
                G.FillRectangle(hb2, ClientRectangle)
            Case MouseState.Down
                G.FillRectangle(hb3, ClientRectangle)
        End Select
        G.DrawRectangle(Pens.Black, New Rectangle(0, 0, Width - 1, Height - 1))
        DrawCorners(Color.FromArgb(15, 15, 15))
    End Sub
End Class
'ProgressBar
Class PatrickProgressBar
    Inherits ThemeControl151
    Private _Maximum As Integer = 100
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If value < 1 Then value = 1
            If value < _Value Then _Value = value
            _Maximum = value
            Invalidate()
        End Set
    End Property

    Private _Value As Integer = 50
    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value > _Maximum Then value = _Maximum
            _Value = value
            Invalidate()
        End Set
    End Property

    Private C1 As Color 'G.Clear
    Private C2, C3 As Color 'Lime Gradient
    Private P1 As Pen 'DrawRectangle
    Private B1 As Brush 'Gloss
    Sub New()
        Size = New Size(75, 18)
    End Sub
    Protected Overrides Sub ColorHook()
        C1 = Color.FromArgb(15, 15, 15)
        C2 = Color.FromArgb(50, 50, 50)
        C3 = Color.FromArgb(0, 0, 0)
        P1 = Pens.Black
        B1 = New SolidBrush(Color.FromArgb(50, Color.White))
    End Sub

    Protected Overrides Sub PaintHook()
        Dim hb1 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(15, 15, 15))
        Dim hb2 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(35, 35, 35))
        Dim hb3 As New Drawing2D.HatchBrush(Drawing2D.HatchStyle.Trellis, Color.FromArgb(0, 0, 0))
        G.Clear(C1)
        If (_Value > 0) Then
            G.FillRectangle(hb1, 0, 0, CInt((_Value / _Maximum) * Width), Height)
        End If
        DrawBorders(P1, 0, 0, Width, Height)
        DrawCorners(Color.Magenta)
    End Sub
End Class
'GroupBox
Class PatrickGroupBox
    Inherits ThemeContainer151
    Private C1 As Color 'G.Clear
    Private C2, C3 As Color 'Lime Gradient
    Private P1 As Pen 'DrawRectangle
    Private B1 As Brush 'Gloss
    Private B2 As Brush 'Grey Text
    Private B5 As Brush
    Private HB1 As Brush 'BackGround
    Sub New()
        ControlMode = True
        Size = New Size(205, 95)
    End Sub
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
        G.FillRectangle(B5, 0, 0, Width, Height)
        'BackGround'
        G.FillRectangle(B5, 0, 0, Width, 15)
        'Outside'
        G.DrawRectangle(P1, 0, 15, Width - 1, CInt(Height - 16))
        'Green'
        G.FillRectangle(hb1, 5, 5, Width - 15, 20)
        'Gloss
        G.FillRectangle(hb1, 5, 5, Width - 15, 10)
        G.DrawRectangle(P1, 5, 5, Width - 15, 20)
        DrawText(B2, HorizontalAlignment.Center, 0, 3)
    End Sub
End Class