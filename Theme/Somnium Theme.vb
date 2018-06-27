#Region "Credits" 'Leave this here!
'Creator: Perplexity 
'Site: *************
'Make sure you give credits when the theme is used
#End Region
Imports System.Drawing.Drawing2D
Class SomniumThemeA
    Inherits ThemeContainer151
    Private C1 As Color 'G.Clear
    Private C2, C3, C4, C5 As Color 'Gradients
    Private P1 As Pen 'DrawRectangle
    Private B1 As Brush 'FillRectangle
    Private B2 As Brush 'Top Gloss
    Private B3 As Brush 'Gray Text
    Private B4 As Brush 'Bottom Gloss
    Private HB1 As HatchBrush 'Background
    Sub New()
        MoveHeight = 15
    End Sub
    Protected Overrides Sub ColorHook()
        C1 = Color.White
        C2 = Color.FromArgb(50, 50, 50)
        C3 = Color.FromArgb(0, 0, 0)
        C4 = Color.White
        C5 = Color.LightGray
        P1 = Pens.Black
        B1 = New SolidBrush(Color.FromArgb(15, 15, 15))
        B2 = New SolidBrush(Color.FromArgb(50, Color.White))
        B3 = New SolidBrush(Color.White)
        B4 = New SolidBrush(Color.FromArgb(30, Color.White))
        HB1 = New HatchBrush(HatchStyle.Trellis, Color.FromArgb(15, 15, 15))
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(C1)
        'BackGround'
        G.FillRectangle(HB1, 0, 0, Width, Height)

        'Top'
        DrawGradient(C2, C3, 0, 0, Width, 15, 90S)
        G.DrawRectangle(P1, 0, 0, Width, 15)

        'Bottom'
        G.FillRectangle(B1, 0, CInt(Height - 11), Width, 10)
        G.DrawRectangle(P1, 0, CInt(Height - 11), Width, 10)
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(30, 30, 30))), 14, CInt(Height - 10), CInt(Width - 29), 8)
        'Left Side'
        'Left'
        G.FillRectangle(B1, 0, 0, 5, CInt(Height - 1))
        G.DrawRectangle(P1, 0, 0, 5, CInt(Height - 1))
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(30, 30, 30))), 1, 1, 3, CInt(Height - 3))
        'Middle'
        DrawGradient(C4, C5, 5, 15, 3, CInt(Height - 16), 180S)
        G.DrawRectangle(P1, 5, 15, 3, CInt(Height - 16))
        'Right'
        G.FillRectangle(B1, 8, 15, 5, CInt(Height - 16))
        G.DrawRectangle(P1, 8, 15, 5, CInt(Height - 16))
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(30, 30, 30))), 9, 16, 3, CInt(Height - 18))

        'Right Side'
        'Right'
        G.FillRectangle(B1, CInt(Width - 6), 0, 5, CInt(Height - 1))
        G.DrawRectangle(P1, CInt(Width - 6), 0, 5, CInt(Height - 1))
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(30, 30, 30))), CInt(Width - 5), 1, 3, CInt(Height - 3))
        'Middle'
        DrawGradient(C4, C5, CInt(Width - 9), 15, 3, CInt(Height - 16), 180S)
        G.DrawRectangle(P1, CInt(Width - 9), 15, 3, CInt(Height - 16))
        'Left'
        G.FillRectangle(B1, CInt(Width - 14), 15, 5, CInt(Height - 16))
        G.DrawRectangle(P1, CInt(Width - 14), 15, 5, CInt(Height - 16))
        G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(30, 30, 30))), CInt(Width - 13), 16, 3, CInt(Height - 18))
        'Top Gloss'
        G.FillRectangle(B2, 0, 0, Width, 5)

        'Bottom Gloss'
        G.FillRectangle(B4, 0, CInt(Height - 3), Width, 3)

        DrawText(B3, HorizontalAlignment.Center, 0, 3)

    End Sub
End Class
Class SomniumButtonA
    Inherits ThemeControl151
    Private C1 As Color 'G.Clear
    Private C2, C3 As Color 'Lime Gradient
    Private B1 As Brush 'Gloss
    Private B2 As Brush 'Grey Text
    Private P1 As Pen 'DrawRectangle
    Protected Overrides Sub ColorHook()
        C1 = Color.White
        C2 = Color.FromArgb(50, 50, 50)
        C3 = Color.FromArgb(0, 0, 0)
        B1 = New SolidBrush(Color.FromArgb(60, Color.White))
        B2 = New SolidBrush(Color.White)
        P1 = Pens.White
    End Sub
    Sub New()
        Size = New Size(115, 20)
    End Sub
    Protected Overrides Sub PaintHook()
        G.Clear(C1)
        DrawGradient(C2, C3, 0, 0, Width, Height, 90S)
        'Gloss'
        If (State = MouseState.Over) Then
            G.FillRectangle(B1, 0, CInt(Height / 2), Width, CInt(Height / 2))
        ElseIf (State = MouseState.Down) Then
            G.FillRectangle(B1, 0, 0, Width, CInt(Height / 2))
        Else
            G.FillRectangle(B1, 0, 0, Width, CInt(Height / 2))
        End If
        DrawBorders(P1, 0, 0, Width, Height)
        DrawText(B2, HorizontalAlignment.Center, 0, 0)
    End Sub
End Class
Class SomniumGroupBox
    Inherits ThemeContainer151
    Private C1 As Color 'G.Clear
    Private C2, C3 As Color 'Lime Gradient
    Private P1 As Pen 'DrawRectangle
    Private B1 As Brush 'Gloss
    Private B2 As Brush 'Grey Text
    Private HB1 As Brush 'BackGround
    Sub New()
        ControlMode = True
        Size = New Size(205, 95)
    End Sub
    Protected Overrides Sub ColorHook()
        C1 = Color.FromArgb(15, 15, 15)
        C2 = Color.FromArgb(50, 50, 50)
        C3 = Color.FromArgb(0, 0, 0)
        P1 = Pens.White
        B1 = New SolidBrush(Color.FromArgb(60, Color.White))
        B2 = New SolidBrush(Color.White)
        HB1 = New HatchBrush(HatchStyle.Trellis, Color.FromArgb(15, 15, 15))
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(C1)
        G.FillRectangle(HB1, 0, 0, Width, Height)
        'BackGround'
        G.FillRectangle(HB1, 0, 0, Width, 15)
        'Outside'
        G.DrawRectangle(P1, 0, 15, Width - 1, CInt(Height - 16))
        'Green'
        DrawGradient(C2, C3, 5, 5, Width - 15, 20, 90S)
        'Gloss
        G.FillRectangle(B1, 5, 5, Width - 15, 10)
        G.DrawRectangle(P1, 5, 5, Width - 15, 20)
        DrawText(B2, HorizontalAlignment.Center, 0, 3)
    End Sub
End Class
Class SomniumCheckBox
    Inherits ThemeControl151
    Private C1 As Color 'G.Clear
    Private C2, C3 As Color 'Lime Gradient
    Private P1 As Pen 'DrawRectangle
    Private B1 As Brush 'Gloss
    Private B2 As Brush 'DrawText
    Private B3 As Brush 'Fill
    Private HB1 As HatchBrush 'BackGround
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
    Sub New()
        Size = New Size(136, 16)
    End Sub
    Sub changeChecked() Handles Me.Click
        Select Case _Checked
            Case False
                _Checked = True
            Case True
                _Checked = False
        End Select
    End Sub
    Protected Overrides Sub ColorHook()
        C1 = Color.FromArgb(15, 15, 15)
        C2 = Color.White
        C3 = Color.LightGray
        P1 = Pens.Black
        B1 = New SolidBrush(Color.FromArgb(15, Color.White))
        B2 = New SolidBrush(Color.White)
        B3 = New SolidBrush(Color.FromArgb(15, 15, 15))
        HB1 = New HatchBrush(HatchStyle.Trellis, Color.FromArgb(15, 15, 15))
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(C1)
        G.FillRectangle(HB1, 0, 0, Width, Height)
        G.FillRectangle(B3, 0, 0, 15, 15)
        G.DrawRectangle(P1, 0, 0, 15, 15)

        If (_Checked = False) Then
            If (State = MouseState.Over) Then
                DrawGradient(Color.FromArgb(50, 50, 50), Color.FromArgb(15, 15, 15), 2, 2, 11, 11, 90S)
            Else
                DrawGradient(Color.FromArgb(15, 15, 15), Color.FromArgb(50, 50, 50), 2, 2, 11, 11, 90S)
            End If
        Else
            If (State = MouseState.Over) Then
                DrawGradient(C3, C2, 2, 2, 11, 11, 90S)
            Else
                DrawGradient(C2, C3, 2, 2, 11, 11, 90S)
            End If

        End If
        G.FillRectangle(B1, 2, 2, 11, 11)
        G.DrawRectangle(P1, 2, 2, 11, 11)
        DrawText(B2, HorizontalAlignment.Left, 15, 0)
    End Sub
End Class
Class SomniumProgressBar
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
        G.Clear(C1)
        If (_Value > 0) Then
            DrawGradient(C2, C3, 0, 0, CInt((_Value / _Maximum) * Width), Height, 90S)
        End If
        G.FillRectangle(B1, 0, 0, Width, CInt(Height / 2))
        DrawBorders(P1, 0, 0, Width, Height)
    End Sub
End Class