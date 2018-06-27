Imports System.Drawing.Drawing2D

Class WhiteForm
    Inherits ThemeContainer153

    Sub New()
        BackColor = Color.FromArgb(250, 250, 250)
        TransparencyKey = Color.Fuchsia
        Header = 20

        SetColor("Border1", 225, 225, 225)
        SetColor("Border2", 150, 150, 150)

        SetColor("Grad1", 225, 225, 225)
        SetColor("Grad2", 185, 185, 185)
    End Sub

    Dim P1, P2 As Pen
    Dim c1, c2, tr As Color

    Protected Overrides Sub ColorHook()
        P1 = New Pen(GetColor("Border1"))
        P2 = New Pen(GetColor("Border2"))

        c1 = GetColor("Grad1")
        c2 = GetColor("Grad2")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        DrawBorders(P1, 1)

        DrawGradient(c1, c2, 0, 0, Width, 20)
        Dim DarkDown As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.FromArgb(75, 75, 75)))
        Dim DarkUp As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.FromArgb(75, 75, 75)))
        'G.FillRectangle(DarkDown, New Rectangle(0, 0, ClientRectangle.Width, Header))
        G.FillRectangle(DarkUp, New Rectangle(0, 0, ClientRectangle.Width, Header))


        DrawBorders(P2, 0)

        G.DrawLine(P2, 0, 20, Width, 20)
        G.DrawLine(P1, 0, 21, Width, 21)
        G.DrawLine(P1, 0, 22, Width, 22)
        G.DrawLine(P2, 0, 23, Width, 23)

        DrawText(Brushes.DarkBlue, HorizontalAlignment.Left, 5, 1)

        DrawCorners(TransparencyKey)
    End Sub
End Class

Class WhiteButton
    Inherits ThemeControl153
    Sub New()
        BackColor = Color.FromArgb(250, 250, 250)

        SetColor("Border1", 225, 225, 225)
        SetColor("Border2", 150, 150, 150)

        SetColor("Grad1", 225, 225, 225)
        SetColor("Grad2", 185, 185, 185)

        Me.Size = New Size(100, 23)
    End Sub

    Dim P1, P2 As Pen
    Dim c1, c2, tr As Color

    Protected Overrides Sub ColorHook()
        P1 = New Pen(GetColor("Border1"))
        P2 = New Pen(GetColor("Border2"))

        c1 = GetColor("Grad1")
        c2 = GetColor("Grad2")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        DrawBorders(P1, 1)

        Select Case State

            Case MouseState.Down
                DrawGradient(c1, c2, 0, 0, Width, Height)
            Case MouseState.None
                DrawGradient(c2, c1, 0, 0, Width, Height)
          Case MouseState.Over
                DrawGradient(c2, c1, 0, 0, Width, Height)
        End Select

        Dim DarkUp As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.Black))
        G.FillRectangle(DarkUp, New Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height))
        
        DrawBorders(P2, 0)

        'G.DrawLine(P2, 0, 20, Width, 20)
        'G.DrawLine(P1, 0, 21, Width, 21)
        'G.DrawLine(P1, 0, 22, Width, 22)
        'G.DrawLine(P2, 0, 23, Width, 23)

        DrawText(Brushes.DarkBlue, HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

Class WhiteGroupBox
    Inherits ThemeContainer153

    Sub New()
        ControlMode = True
        BackColor = Color.FromArgb(225, 225, 225)

        SetColor("Grad1", 225, 225, 225)
        SetColor("Grad2", 185, 185, 185)
        SetColor("Grad3", 80, Color.White)
        SetColor("Transp", Color.Transparent)
        SetColor("Text", Color.FromArgb(0, 133, 157))
        SetColor("BG", Color.FromArgb(41, 41, 41))
        SetColor("Border1", 225, 225, 225)
        SetColor("Border2", 150, 150, 150)


        Me.Size = New Size(200, 150)

    End Sub

    Dim C1, C2, C4 As Color
    Dim P1, P2 As Pen
    Dim B1 As Brush

    Protected Overrides Sub ColorHook()
        C1 = GetColor("Grad1")
        C2 = GetColor("Grad2")
        C4 = GetColor("Transp")

        B1 = New SolidBrush(GetColor("Text"))

        P1 = New Pen(GetColor("Border1"))
        P2 = New Pen(GetColor("Border2"))
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)

        DrawGradient(C2, C1, 0, 0, Width, 20)
        Dim DarkUp As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.FromArgb(75, 75, 75)))
        G.FillRectangle(DarkUp, New Rectangle(0, 0, ClientRectangle.Width, 20))

        G.DrawLine(P1, 0, 20, Width, 20)
        G.DrawLine(P2, 0, 21, Width, 21)

        DrawBorders(P1)
        DrawBorders(P2, 1)


        DrawText(Brushes.DarkBlue, HorizontalAlignment.Center, 0, 0)
    End Sub
End Class

Class WhiteCheckBox
    Inherits ThemeControl153
#Region " Properties "
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
#End Region
    Sub New()
        BackColor = Color.FromArgb(255, 255, 255)
        Size = New Size(120, 16)
        MinimumSize = New Size(16, 16)
        MaximumSize = New Size(600, 16)

        SetColor("Text", Color.DarkBlue)

        SetColor("Border1", 225, 225, 225)
        SetColor("Border2", 150, 150, 150)

        SetColor("Grad1", 225, 225, 225)
        SetColor("Grad2", 185, 185, 185)
    End Sub
    Dim P1, P2 As Pen
    Dim B1 As Brush
    Dim c1, c2 As Color
    Protected Overrides Sub ColorHook()
        P1 = New Pen(GetColor("Border1"))
        P2 = New Pen(GetColor("Border2"))
        B1 = New SolidBrush(GetColor("Text"))

        c1 = GetColor("Grad1")
        c2 = GetColor("Grad2")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Select Case CheckedState
            Case True
                DrawGradient(c2, c1, 0, 0, 14, 14)
            Case False
                G.FillRectangle(New SolidBrush(BackColor), 0, 0, 14, 14)
        End Select
        G.DrawRectangle(P2, 0, 0, 14, 14)
        G.DrawRectangle(P1, 1, 1, 12, 12)
        DrawText(B1, HorizontalAlignment.Left, 17, 0)
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

Class aCButton
    Inherits ThemeControl153

    Sub New()
        Me.Size = New Size(21, 20)

        'BEGIN BUTTON-STATE GRADIENTS
        SetColor("DownGradient2", 185, 185, 185)
        SetColor("DownGradient1", 225, 225, 225)

        SetColor("NoneGradient2", 225, 225, 225)
        SetColor("NoneGradient1", 185, 185, 185)
        'END BUTTON-STATE GRADIENTS


        SetColor("Grad1", 225, 225, 225)
        SetColor("Grad2", 185, 185, 185)

        SetColor("Text", Color.DarkBlue)
        SetColor("Border1", 12, Color.White)
        SetColor("Border2", Color.SlateGray)
    End Sub

    Private C1, C2, C3, C4 As Color
    Private B1 As SolidBrush
    Private P1, P2 As Pen

    Protected Overrides Sub ColorHook()
        C1 = GetColor("DownGradient1")
        C2 = GetColor("DownGradient2")
        C3 = GetColor("NoneGradient1")
        C4 = GetColor("NoneGradient2")

        B1 = New SolidBrush(GetColor("Text"))

        P1 = New Pen(GetColor("Border1"))
        P2 = New Pen(GetColor("Border2"))
    End Sub

    Protected Overrides Sub PaintHook()
        Select Case State
            Case MouseState.None
                DrawGradient(C3, C4, ClientRectangle, 90.0F)
            Case MouseState.Over
                DrawGradient(C3, C4, ClientRectangle, 90.0F)
            Case MouseState.Down
                DrawGradient(C1, C2, ClientRectangle, 90.0F)
        End Select

        
        Dim DarkUp As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.White))
        G.FillRectangle(DarkUp, New Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height))


        DrawText(Brushes.DarkBlue, "X", HorizontalAlignment.Center, 0, 0)
        DrawBorders(P2)


        'DrawBorders(New Pen(Color.FromArgb(0, 168, 198)))
        'DrawCorners(BackColor)
    End Sub

    Sub IveBeenClicked__ItTicklesLOL() Handles Me.Click
        FindForm().Close()
    End Sub
End Class

Class WhiteTextBox
    Inherits TextBox

    Sub New()
        Me.ForeColor = Color.DarkBlue
        Me.BorderStyle = Windows.Forms.BorderStyle.FixedSingle

        Me.Text = Me.Name
    End Sub
End Class