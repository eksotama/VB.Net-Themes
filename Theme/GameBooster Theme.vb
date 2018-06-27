Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Class GameBoosterButton
    Inherits ThemeControl154

    Sub New()
    End Sub
    Private TopGradient, BotGradient As Color
    Private TopGradientClick, BotGradientClick As Color
    Private TopGradientHover, BotGradientHover As Color
    Private InnerBorder, OuterBorder, InnerBorderHover, OuterBorderHover, InnerBorderClick, OuterBorderClick As Pen
    Private TextCol As SolidBrush
    Protected Overrides Sub ColorHook()
        TopGradient = Color.FromArgb(55, 55, 55)
        BotGradient = Color.FromArgb(32, 32, 32)

        TopGradientHover = Color.FromArgb(66, 66, 66)
        BotGradientHover = Color.FromArgb(41, 41, 41)

        TopGradientClick = Color.FromArgb(60, 60, 60)
        BotGradientClick = Color.FromArgb(37, 37, 37)

        TextCol = New SolidBrush(Color.FromArgb(204, 204, 204))

        OuterBorder = New Pen(Color.Black)
        InnerBorder = New Pen(Color.FromArgb(76, 76, 76))

        OuterBorderHover = New Pen(Color.Black)
        InnerBorderHover = New Pen(Color.FromArgb(87, 87, 87))

        OuterBorderClick = New Pen(Color.Black)
        InnerBorderClick = New Pen(Color.FromArgb(71, 71, 71))
    End Sub

    Protected Overrides Sub PaintHook()

        If State = MouseState.Down Then
            DrawGradient(TopGradientClick, BotGradientClick, New Rectangle(2, 1, Width - 4, Height - 3), 90.0F)
            G.DrawRectangle(InnerBorderClick, 1, 1, ClientRectangle.Width - 3, ClientRectangle.Height - 3)
            'TOPLEFT
            DrawPixel(OuterBorderClick.Color, 1, 1)
            DrawPixel(InnerBorderClick.Color, 2, 2)
            'TOPRIGHT
            DrawPixel(OuterBorderClick.Color, Width - 2, 1)
            DrawPixel(InnerBorderClick.Color, Width - 3, 2)
            'BOTTOMLEFT
            DrawPixel(OuterBorderClick.Color, 1, Height - 2)
            DrawPixel(InnerBorderClick.Color, 1, Height - 3)
            'BOTTOMRIGHT
            DrawPixel(OuterBorderClick.Color, Width - 2, Height - 2)
            DrawPixel(InnerBorderClick.Color, Width - 3, Height - 3)
            DrawBorders(OuterBorderClick)
        Else
            DrawGradient(TopGradient, BotGradient, New Rectangle(2, 1, Width - 4, Height - 3), 90.0F)
            G.DrawRectangle(InnerBorder, 1, 1, ClientRectangle.Width - 3, ClientRectangle.Height - 3)
            'TOPLEFT
            DrawPixel(OuterBorder.Color, 1, 1)
            DrawPixel(InnerBorder.Color, 2, 2)
            'TOPRIGHT
            DrawPixel(OuterBorder.Color, Width - 2, 1)
            DrawPixel(InnerBorder.Color, Width - 3, 2)
            'BOTTOMLEFT
            DrawPixel(OuterBorder.Color, 1, Height - 2)
            DrawPixel(InnerBorder.Color, 1, Height - 3)
            'BOTTOMRIGHT
            DrawPixel(OuterBorder.Color, Width - 2, Height - 2)
            DrawPixel(InnerBorder.Color, Width - 3, Height - 3)
            DrawBorders(OuterBorder)
        End If

        If State = MouseState.Over Then
            DrawGradient(TopGradientHover, BotGradientHover, New Rectangle(2, 1, Width - 4, Height - 3), 90.0F)
            G.DrawRectangle(InnerBorderHover, 1, 1, ClientRectangle.Width - 3, ClientRectangle.Height - 3)
            'TOPLEFT
            DrawPixel(OuterBorderHover.Color, 1, 1)
            DrawPixel(InnerBorderHover.Color, 2, 2)
            'TOPRIGHT
            DrawPixel(OuterBorderHover.Color, Width - 2, 1)
            DrawPixel(InnerBorderHover.Color, Width - 3, 2)
            'BOTTOMLEFT
            DrawPixel(OuterBorderHover.Color, 1, Height - 2)
            DrawPixel(InnerBorderHover.Color, 1, Height - 3)
            'BOTTOMRIGHT
            DrawPixel(OuterBorderHover.Color, Width - 2, Height - 2)
            DrawPixel(InnerBorderHover.Color, Width - 3, Height - 3)
            DrawBorders(OuterBorderHover)
        End If

        DrawText(TextCol, HorizontalAlignment.Center, 0, 0)

        DrawCorners(Color.FromArgb(51, 51, 51))
    End Sub
End Class
Class GameBoosterSideButton

    Inherits ThemeControl154
    Public Enum _Icon
        Square = 1
        Circle = 2
        Custom_Image = 3
    End Enum
    Public Enum _Color
        Red = 1
        Green = 2
        Yellow = 3
    End Enum
    Private _DisplayIcon As _Icon
    Private _Col As _Color
    Property DisplayIcon As _Icon
        Get
            Return _DisplayIcon
        End Get
        Set(ByVal value As _Icon)
            _DisplayIcon = value
            Invalidate()
        End Set
    End Property
    Property SideColor As _Color
        Get
            Return _Col
        End Get
        Set(ByVal value As _Color)
            _Col = value
            Invalidate()
        End Set
    End Property
    Sub New()
        LockHeight = 30
        Width = 227
    End Sub
    Private GrayGradient1, GrayGradient2, GrayGradient3, GrayGradient4, RedGradient1, RedGradient2, RedGradient3, RedGradient4 As Color
    Private OuterBorder, InnerBorderGray, InnerBorderRed As Pen
    Private InnerBorderGreen, InnerBorderYellow As Pen
    Private InnerBorderGrayHover As Pen
    Private InnerBorderGrayClick As Pen
    Private GreenGradient1, GreenGradient2, GreenGradient3, GreenGradient4 As Color
    Private YellowGradient1, YellowGradient2, YellowGradient3, YellowGradient4 As Color
    Private ExtraPixelRed, ExtraPixelGreen, ExtraPixelYellow As Color
    Private GrayGradientHover1, GrayGradientHover2, GrayGradientHover3, GrayGradientHover4 As Color
    Private GrayGradientClick1, GrayGradientClick2, GrayGradientClick3, GrayGradientClick4 As Color
    Private TextCol As SolidBrush
    Private CircleColor As Color
    Private SquareColor As Color
    Protected Overrides Sub ColorHook()
        OuterBorder = New Pen(Color.Black)
        InnerBorderRed = New Pen(Color.FromArgb(216, 70, 70))
        InnerBorderGray = New Pen(Color.FromArgb(87, 87, 87))
        InnerBorderGreen = New Pen(Color.FromArgb(68, 204, 2))
        InnerBorderYellow = New Pen(Color.FromArgb(247, 219, 17))
        InnerBorderGrayHover = New Pen(Color.FromArgb(104, 104, 104))
        InnerBorderGrayClick = New Pen(Color.FromArgb(79, 79, 79))
        TextCol = New SolidBrush(Color.White)
        ExtraPixelRed = Color.FromArgb(133, 37, 37)
        ExtraPixelGreen = Color.FromArgb(1, 58, 11)
        ExtraPixelYellow = Color.FromArgb(206, 111, 4)
        SquareColor = Color.White

        GrayGradient1 = Color.FromArgb(59, 59, 59)
        GrayGradient2 = Color.FromArgb(45, 45, 45)
        GrayGradient3 = Color.FromArgb(33, 33, 33)
        GrayGradient4 = Color.FromArgb(24, 24, 24)

        GrayGradientHover1 = Color.FromArgb(78, 78, 78)
        GrayGradientHover2 = Color.FromArgb(64, 64, 64)
        GrayGradientHover3 = Color.FromArgb(48, 48, 48)
        GrayGradientHover4 = Color.FromArgb(38, 38, 38)

        GrayGradientClick1 = Color.FromArgb(48, 48, 48)
        GrayGradientClick2 = Color.FromArgb(35, 35, 35)
        GrayGradientClick3 = Color.FromArgb(33, 33, 33)
        GrayGradientClick4 = Color.FromArgb(24, 24, 24)

        RedGradient1 = Color.FromArgb(206, 38, 38)
        RedGradient2 = Color.FromArgb(157, 25, 25)
        RedGradient3 = Color.FromArgb(147, 12, 12)
        RedGradient4 = Color.FromArgb(104, 2, 2)

        GreenGradient1 = Color.FromArgb(52, 155, 2)
        GreenGradient2 = Color.FromArgb(43, 129, 1)
        GreenGradient3 = Color.FromArgb(2, 100, 19)
        GreenGradient4 = Color.FromArgb(1, 78, 15)

        YellowGradient1 = Color.FromArgb(232, 151, 10)
        YellowGradient2 = Color.FromArgb(236, 167, 12)
        YellowGradient3 = Color.FromArgb(228, 141, 5)
        YellowGradient4 = Color.FromArgb(223, 122, 4)

        CircleColor = Color.White
    End Sub

    Protected Overrides Sub PaintHook()
        ''---GRAY---
        If State = MouseState.Down Then
            DrawGradient(GrayGradientClick3, GrayGradientClick4, New Rectangle(1, Height / 2 - 1, Width, Height / 2 + 2)) 'BOT
            DrawGradient(GrayGradientClick1, GrayGradientClick2, New Rectangle(1, 1, Width, Height / 2 - 1)) 'TOP
        ElseIf State = MouseState.Over Then
            DrawGradient(GrayGradientHover3, GrayGradientHover4, New Rectangle(1, Height / 2 - 1, Width, Height / 2 + 2)) 'BOT
            DrawGradient(GrayGradientHover1, GrayGradientHover2, New Rectangle(1, 1, Width, Height / 2 - 1)) 'TOP
        Else
            DrawGradient(GrayGradient3, GrayGradient4, New Rectangle(1, Height / 2 - 1, Width, Height / 2 + 2)) 'BOT
            DrawGradient(GrayGradient1, GrayGradient2, New Rectangle(1, 1, Width, Height / 2 - 1)) 'TOP
        End If
        ''---COLOR---
        If _Col = _Color.Green Then
            DrawGradient(GreenGradient3, GreenGradient4, New Rectangle(1, Height / 2 - 1, 23, Height / 2 + 2)) 'BOT
            DrawGradient(GreenGradient1, GreenGradient2, New Rectangle(1, 1, 23, Height / 2 - 1)) 'TOP
        ElseIf _Col = _Color.Yellow Then
            DrawGradient(YellowGradient3, YellowGradient4, New Rectangle(1, Height / 2 - 1, 23, Height / 2 + 2)) 'BOT
            DrawGradient(YellowGradient1, YellowGradient2, New Rectangle(1, 1, 23, Height / 2 - 1)) 'TOP
        Else
            DrawGradient(RedGradient3, RedGradient4, New Rectangle(1, Height / 2 - 1, 23, Height / 2 + 2)) 'BOT
            DrawGradient(RedGradient1, RedGradient2, New Rectangle(1, 1, 23, Height / 2 - 1)) 'TOP
        End If
        ''---------
        If _Col = _Color.Green Then
            G.DrawRectangle(InnerBorderGreen, New Rectangle(1, 1, 22, Height - 3))
        ElseIf _Col = _Color.Yellow Then
            G.DrawRectangle(InnerBorderYellow, New Rectangle(1, 1, 22, Height - 3))
        Else
            G.DrawRectangle(InnerBorderRed, New Rectangle(1, 1, 22, Height - 3))
        End If
        If State = MouseState.Down Then
            G.DrawRectangle(InnerBorderGrayClick, New Rectangle(24, 1, Width - 26, Height - 3))
        ElseIf State = MouseState.Over Then
            G.DrawRectangle(InnerBorderGrayHover, New Rectangle(24, 1, Width - 26, Height - 3))
        Else
            G.DrawRectangle(InnerBorderGray, New Rectangle(24, 1, Width - 26, Height - 3))
        End If
        DrawBorders(OuterBorder)
        '---TOPLEFT---

        If _Col = _Color.Green Then
            DrawPixel(ExtraPixelGreen, 1, 2)
            DrawPixel(ExtraPixelGreen, 2, 1)
            DrawPixel(InnerBorderGreen.Color, 2, 2)
        ElseIf _Col = _Color.Yellow Then
            DrawPixel(ExtraPixelYellow, 1, 2)
            DrawPixel(ExtraPixelYellow, 2, 1)
            DrawPixel(InnerBorderYellow.Color, 2, 2)
        Else
            DrawPixel(ExtraPixelRed, 1, 2)
            DrawPixel(ExtraPixelRed, 2, 1)
            DrawPixel(InnerBorderRed.Color, 2, 2)
        End If
        DrawPixel(OuterBorder.Color, 1, 1)
        '---BOTLEFT---
        DrawPixel(Color.FromArgb(51, 51, 51), 0, Height - 1)
        DrawPixel(Color.FromArgb(51, 51, 51), 1, Height - 1)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, Height - 2)

        If _Col = _Color.Green Then
            DrawPixel(InnerBorderGreen.Color, 2, Height - 3)
            DrawPixel(ExtraPixelGreen, 1, Height - 3)
            DrawPixel(ExtraPixelGreen, 2, Height - 2)
        ElseIf _Col = _Color.Yellow Then
            DrawPixel(InnerBorderYellow.Color, 2, Height - 3)
            DrawPixel(ExtraPixelYellow, 1, Height - 3)
            DrawPixel(ExtraPixelYellow, 2, Height - 2)
        Else
            DrawPixel(InnerBorderRed.Color, 2, Height - 3)
            DrawPixel(ExtraPixelRed, 1, Height - 3)
            DrawPixel(ExtraPixelRed, 2, Height - 2)
        End If
        DrawPixel(OuterBorder.Color, 1, Height - 2)

        '---ICON---
        If DisplayIcon = _Icon.Square Then
            DrawGradient(SquareColor, SquareColor, New Rectangle(7, 9, 5, 5))
            DrawGradient(SquareColor, SquareColor, New Rectangle(13, 9, 5, 5))
            DrawGradient(SquareColor, SquareColor, New Rectangle(7, 15, 5, 5))
            DrawGradient(SquareColor, SquareColor, New Rectangle(13, 15, 5, 5))
        ElseIf DisplayIcon = _Icon.Circle Then
            G.SmoothingMode = SmoothingMode.HighQuality
            G.DrawEllipse(New Pen(CircleColor), 6, 8, 12, 12)
            G.FillEllipse(New SolidBrush(CircleColor), 8, 10, 8, 8)
        ElseIf DisplayIcon = _Icon.Custom_Image Then
            G.DrawImage(Image, 5, 8, 16, 16)
        Else
            DrawGradient(SquareColor, SquareColor, New Rectangle(7, 9, 5, 5))
            DrawGradient(SquareColor, SquareColor, New Rectangle(13, 9, 5, 5))
            DrawGradient(SquareColor, SquareColor, New Rectangle(7, 15, 5, 5))
            DrawGradient(SquareColor, SquareColor, New Rectangle(13, 15, 5, 5))
        End If

        DrawText(TextCol, HorizontalAlignment.Left, 31, 0)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, 0)
        DrawPixel(Color.FromArgb(51, 51, 51), 1, 0)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, 1)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, Height - 1)
        DrawPixel(Color.FromArgb(51, 51, 51), 1, Height - 1)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, Height - 2)

    End Sub
End Class
<DefaultEvent("CheckedChanged")> _
Class GameBoosterCheckBox
    Inherits ThemeControl154

    Sub New()
        LockHeight = 17
        Width = 160
    End Sub

    Private X As Integer
    Private TextColor, G1, G2, Edge As Color

    Protected Overrides Sub ColorHook()
        TextColor = Color.White
        G1 = Color.FromArgb(65, 65, 65)
        G2 = Color.FromArgb(122, 122, 122)
        Edge = Color.Black
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Invalidate()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(51, 51, 51))
        If _Checked Then
            Dim LGB As New LinearGradientBrush(New Rectangle(New Point(0, 0), New Size(14, 16)), G1, G2, 90.0F)
            DrawGradient(Color.Black, Color.FromArgb(100, 100, 100), New Rectangle(2, 2, Height - 7, Height - 7), 45.0F)
            G.DrawRectangle(New Pen(Color.FromArgb(102, 102, 102)), 1, 1, Height - 5, Height - 5)
            G.FillRectangle(LGB, New Rectangle(New Point(3, 3), New Size(Height - 8, Height - 8)))
            DrawPixel(Color.FromArgb(42, 42, 42), 2, Height - 5)
            DrawPixel(Color.FromArgb(42, 42, 42), Height - 5, 2)
            DrawPixel(Color.FromArgb(42, 42, 42), 2, 2)
        Else
            Dim LGB As New LinearGradientBrush(New Rectangle(New Point(0, 0), New Size(14, 16)), G1, G2, 90.0F)
            DrawGradient(Color.Black, Color.FromArgb(101, 101, 101), New Rectangle(2, 2, Height - 7, Height - 7), 45.0F)
            G.DrawRectangle(New Pen(Color.FromArgb(102, 102, 102)), 1, 1, Height - 5, Height - 5)
            G.FillRectangle(LGB, New Rectangle(New Point(3, 3), New Size(Height - 8, Height - 8)))
            DrawPixel(Color.FromArgb(42, 42, 42), 2, Height - 5)
            DrawPixel(Color.FromArgb(42, 42, 42), Height - 5, 2)
            DrawPixel(Color.FromArgb(42, 42, 42), 2, 2)
        End If

        If State = MouseState.Over And X < 15 Then
            Dim SB As New SolidBrush(Color.FromArgb(70, Color.White))
            G.FillRectangle(SB, New Rectangle(New Point(0, 0), New Size(14, 14)))
        ElseIf State = MouseState.Down And X < 15 Then
            Dim SB As New SolidBrush(Color.FromArgb(10, Color.Black))
            G.FillRectangle(SB, New Rectangle(New Point(0, 0), New Size(14, 14)))
        End If

        Dim HB As New HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(7, Color.Black), Color.Transparent)
        G.FillRectangle(HB, New Rectangle(New Point(0, 0), New Size(14, 14)))
        G.DrawRectangle(New Pen(Edge), New Rectangle(New Point(0, 0), New Size(14, 14)))

        If _Checked Then G.DrawString("a", New Font("Marlett", 12), New SolidBrush(Color.FromArgb(214, 214, 214)), New Point(-3, -1))
        DrawText(New SolidBrush(TextColor), HorizontalAlignment.Left, 19, -1)
    End Sub

    Private _Checked As Boolean
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnMouseDown(e)
    End Sub

    Event CheckedChanged(ByVal sender As Object)

End Class
Class GameBoosterTheme
    Inherits ThemeContainer154

    Sub New()
        Me.BackColor = Color.FromArgb(51, 51, 51)
        TransparencyKey = Color.Fuchsia

        SetColor("Sides", 232, 232, 232)
        SetColor("Gradient1", 252, 252, 252)
        SetColor("Gradient2", 242, 242, 242)
        SetColor("TextShade", Color.White)
        SetColor("Text", 80, 80, 80)
        SetColor("Back", Color.White)
        SetColor("Border1", Color.Black)
        SetColor("Border2", Color.White)
        SetColor("Border3", Color.White)
        SetColor("Border4", 150, 150, 150)
    End Sub

    Private C1, C2, C3 As Color
    Private B1, B2, B3 As SolidBrush
    Private P1, P2, P3, P4 As Pen

    Protected Overrides Sub ColorHook()
        C1 = GetColor("Sides")
        C2 = GetColor("Gradient1")
        C3 = GetColor("Gradient2")

        B1 = New SolidBrush(GetColor("TextShade"))
        B2 = New SolidBrush(GetColor("Text"))
        B3 = New SolidBrush(GetColor("Back"))

        P1 = New Pen(Color.FromArgb(24, 24, 24))
        P2 = New Pen(Color.FromArgb(126, 126, 126))

        P3 = New Pen(Color.FromArgb(92, 92, 92))
        P4 = New Pen(Color.FromArgb(24, 24, 24))

        BackColor = B3.Color
    End Sub

    Private RT1 As Rectangle

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(51, 51, 51))

        DrawGradient(C2, C3, 0, 0, Width, 15)

        DrawText(B1, HorizontalAlignment.Left, 13, 1)
        DrawText(B2, HorizontalAlignment.Left, 12, 0)

        DrawGradient(Color.FromArgb(92, 92, 92), Color.FromArgb(49, 49, 49), 0, 0, Width, 26)
        G.DrawLine(New Pen(P1.Color), New Point(0, 26), New Point(Width, 26))
        G.DrawRectangle(P1, 0, 0, Width - 1, Height - 1)
        G.DrawRectangle(P2, 1, 1, Width - 3, Height - 3)
        DrawPixel(P1.Color, 1, 1)
        DrawPixel(P2.Color, 2, 2)
        DrawPixel(P1.Color, Width - 2, 1)
        DrawPixel(P2.Color, Width - 3, 2)
        DrawPixel(P1.Color, 1, Height - 2)
        DrawPixel(P2.Color, 2, Height - 3)
        DrawPixel(P1.Color, Width - 2, Height - 2)
        DrawPixel(P2.Color, Width - 3, Height - 3)
        DrawText(New SolidBrush(Color.FromArgb(61, 61, 61)), HorizontalAlignment.Center, 0, 1)
        DrawText(New SolidBrush(Color.White), HorizontalAlignment.Center, 0, 2)

    End Sub
End Class
Class GameBoosterMiddleBar
    Inherits ThemeControl154

    Sub New()
        LockHeight = 31
        Height = 31
    End Sub
    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(54, 54, 54))
        G.DrawLine(New Pen(Color.FromArgb(24, 24, 24)), 0, 0, Width, 0)
        G.DrawLine(New Pen(Color.FromArgb(69, 69, 69)), 0, 1, Width, 1)
        G.DrawLine(New Pen(Color.FromArgb(24, 24, 24)), 0, Height - 2, Width, Height - 2)
        G.DrawLine(New Pen(Color.FromArgb(69, 69, 69)), 0, Height - 1, Width, Height - 1)
    End Sub
End Class
Class GameBoosterListBox
    Inherits ListBox

    Sub New()
        ItemHeight = 20
        SetStyle(ControlStyles.DoubleBuffer, True)
        Font = New Font("Microsoft Sans Serif", 9)
        BorderStyle = Windows.Forms.BorderStyle.None
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        ItemHeight = 21
        ForeColor = Color.White
        BackColor = Color.FromArgb(51, 51, 51)
        IntegralHeight = False
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        MyBase.WndProc(m)
        If m.Msg = 15 Then CustomPaint()
    End Sub

    Private _Image As Image
    Public Property ItemImage As Image
        Get
            Return _Image
        End Get
        Set(ByVal value As Image)
            _Image = value
        End Set
    End Property

    Protected Overrides Sub OnDrawItem(ByVal e As System.Windows.Forms.DrawItemEventArgs)
        Try
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(51, 51, 51)), e.Bounds)
            If e.Index < 0 Then Exit Sub
            e.DrawBackground()
            Dim rect As New Rectangle(New Point(e.Bounds.Left, e.Bounds.Top + 2), New Size(Bounds.Width, 16))
            e.DrawFocusRectangle()
            If InStr(e.State.ToString, "Selected,") > 0 Then
                Dim x2 As Rectangle = e.Bounds
                Dim x3 As Rectangle = New Rectangle(x2.Location, New Size(x2.Width, (x2.Height / 2)))
                Dim G1 As New LinearGradientBrush(New Point(x2.X, x2.Y), New Point(x2.X, x2.Y + x2.Height), Color.FromArgb(31, 31, 31), Color.FromArgb(18, 18, 18))
                e.Graphics.FillRectangle(G1, x2.X + 1, x2.Y + 1, x2.Width, x2.Height) : G1.Dispose()
                e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, Brushes.White, 5, e.Bounds.Y + (e.Bounds.Height / 2) - 9)
                e.Graphics.DrawLine(New Pen(Color.FromArgb(51, 51, 51)), 2, SelectedIndex * 20, Width - 2, SelectedIndex * 20)
            Else
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(51, 51, 51)), e.Bounds)
                Dim x2 As Rectangle = e.Bounds
                e.Graphics.DrawString(" " & Items(e.Index).ToString(), Font, Brushes.White, 5, e.Bounds.Y + (e.Bounds.Height / 2) - 9)
                For i = 1 To Items.Count
                    e.Graphics.DrawLine(New Pen(Color.FromArgb(51, 51, 51)), 2, 20 * i, Width - 2, 20 * i)
                Next
            End If
            e.Graphics.DrawRectangle(New Pen(Color.FromArgb(24, 24, 24)), New Rectangle(0, 0, Width - 1, Height - 1))
            MyBase.OnDrawItem(e)
            CreateGraphics.DrawRectangle(New Pen(Color.FromArgb(69, 69, 69)), New Rectangle(0, 0, ClientRectangle.Width - 1, ClientRectangle.Height - 1))
            CreateGraphics.DrawRectangle(New Pen(Color.FromArgb(24, 24, 24)), New Rectangle(1, 1, ClientRectangle.Width - 3, ClientRectangle.Height - 3))
        Catch ex As Exception : End Try
    End Sub

    Sub CustomPaint()

    End Sub
End Class
<DefaultEvent("TextChanged")> _
Class GameBoosterTextBoxDark
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

        Base.BorderStyle = BorderStyle.None

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


    Protected Overrides Sub ColorHook()
        Base.ForeColor = Color.FromArgb(204, 204, 204)
        Base.BackColor = Color.FromArgb(107, 107, 107)
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(107, 107, 107))
        DrawBorders(Pens.Black)
        DrawBorders(New Pen(Color.FromArgb(102, 102, 102)), 1)
        DrawBorders(Pens.Black, 2)
        DrawPixel(Color.FromArgb(46, 46, 46), 2, 2)
        DrawPixel(Color.FromArgb(46, 46, 46), Width - 3, 2)
        DrawPixel(Color.FromArgb(46, 46, 46), 2, Height - 3)
        DrawPixel(Color.FromArgb(46, 46, 46), Width - 3, Height - 3)
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
<DefaultEvent("TextChanged")> _
Class GameBoosterTextBoxLight
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

        Base.BorderStyle = BorderStyle.None

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


    Protected Overrides Sub ColorHook()
        Base.ForeColor = Color.FromArgb(204, 204, 204)
        Base.BackColor = Color.FromArgb(153, 153, 153)
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(153, 153, 153))
        DrawBorders(Pens.Black)
        DrawBorders(New Pen(Color.FromArgb(102, 102, 102)), 1)
        DrawBorders(Pens.Black, 2)
        DrawPixel(Color.FromArgb(46, 46, 46), 2, 2)
        DrawPixel(Color.FromArgb(46, 46, 46), Width - 3, 2)
        DrawPixel(Color.FromArgb(46, 46, 46), 2, Height - 3)
        DrawPixel(Color.FromArgb(46, 46, 46), Width - 3, Height - 3)
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
<DefaultEvent("TextChanged")> _
Class GameBoosterTextBoxRound
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

        Base.BorderStyle = BorderStyle.None

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


    Protected Overrides Sub ColorHook()
        Base.ForeColor = Color.FromArgb(204, 204, 204)
        Base.BackColor = Color.FromArgb(32, 32, 32)
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(32, 32, 32))

        G.DrawLine(New Pen(Color.FromArgb(23, 23, 23)), 0, 0, Width, 0)
        G.DrawLine(New Pen(Color.FromArgb(28, 28, 28)), 0, 1, Width, 1)
        G.DrawLine(New Pen(Color.FromArgb(31, 31, 31)), 0, 2, Width, 2)

        G.DrawLine(New Pen(Color.FromArgb(29, 29, 29)), 0, Height - 2, Width, Height - 2)
        G.DrawLine(New Pen(Color.FromArgb(70, 70, 70)), 0, Height - 1, Width, Height - 1)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, 0)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, 1)
        DrawPixel(Color.FromArgb(44, 44, 44), 0, 2)
        DrawPixel(Color.FromArgb(28, 28, 28), 0, 3)
        DrawPixel(Color.FromArgb(24, 24, 24), 0, 4)
        DrawPixel(Color.FromArgb(22, 22, 22), 0, 5)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 6)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 7)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 8)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 9)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 10)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 11)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 12)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 13)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 14)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 15)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 16)
        DrawPixel(Color.FromArgb(23, 23, 23), 0, 17)
        DrawPixel(Color.FromArgb(25, 25, 25), 0, 18)
        DrawPixel(Color.FromArgb(29, 29, 29), 0, 19)
        DrawPixel(Color.FromArgb(44, 44, 44), 0, 20)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, 21)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, 22)
        DrawPixel(Color.FromArgb(51, 51, 51), 0, 23)
        DrawPixel(Color.FromArgb(51, 51, 51), 1, 0)
        DrawPixel(Color.FromArgb(37, 37, 37), 1, 1)
        DrawPixel(Color.FromArgb(23, 23, 23), 1, 2)
        DrawPixel(Color.FromArgb(24, 24, 24), 1, 3)
        DrawPixel(Color.FromArgb(27, 27, 27), 1, 4)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 5)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 6)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 7)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 8)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 9)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 10)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 11)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 12)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 13)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 14)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 15)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 16)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 17)
        DrawPixel(Color.FromArgb(28, 28, 28), 1, 18)
        DrawPixel(Color.FromArgb(27, 27, 27), 1, 19)
        DrawPixel(Color.FromArgb(26, 26, 26), 1, 20)
        DrawPixel(Color.FromArgb(40, 40, 40), 1, 21)
        DrawPixel(Color.FromArgb(51, 51, 51), 1, 22)
        DrawPixel(Color.FromArgb(51, 51, 51), 1, 23)
        DrawPixel(Color.FromArgb(44, 44, 44), 2, 0)
        DrawPixel(Color.FromArgb(22, 22, 22), 2, 1)
        DrawPixel(Color.FromArgb(26, 26, 26), 2, 2)
        DrawPixel(Color.FromArgb(28, 28, 28), 2, 3)
        DrawPixel(Color.FromArgb(29, 29, 29), 2, 4)
        DrawPixel(Color.FromArgb(30, 30, 30), 2, 5)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 6)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 7)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 8)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 9)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 10)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 11)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 12)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 13)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 14)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 15)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 16)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 17)
        DrawPixel(Color.FromArgb(31, 31, 31), 2, 18)
        DrawPixel(Color.FromArgb(30, 30, 30), 2, 19)
        DrawPixel(Color.FromArgb(29, 29, 29), 2, 20)
        DrawPixel(Color.FromArgb(26, 26, 26), 2, 21)
        DrawPixel(Color.FromArgb(51, 51, 51), 2, 22)
        DrawPixel(Color.FromArgb(51, 51, 51), 2, 23)
        DrawPixel(Color.FromArgb(31, 31, 31), 3, 0)
        DrawPixel(Color.FromArgb(24, 24, 24), 3, 1)
        DrawPixel(Color.FromArgb(28, 28, 28), 3, 2)
        DrawPixel(Color.FromArgb(30, 30, 30), 3, 3)
        DrawPixel(Color.FromArgb(31, 31, 31), 3, 4)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 5)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 6)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 7)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 8)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 9)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 10)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 11)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 12)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 13)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 14)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 15)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 16)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 17)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 18)
        DrawPixel(Color.FromArgb(32, 32, 32), 3, 19)
        DrawPixel(Color.FromArgb(31, 31, 31), 3, 20)
        DrawPixel(Color.FromArgb(29, 29, 29), 3, 21)
        DrawPixel(Color.FromArgb(40, 40, 40), 3, 22)
        DrawPixel(Color.FromArgb(56, 56, 56), 3, 23)
        DrawPixel(Color.FromArgb(25, 25, 25), 4, 0)
        DrawPixel(Color.FromArgb(26, 26, 26), 4, 1)
        DrawPixel(Color.FromArgb(29, 29, 29), 4, 2)
        DrawPixel(Color.FromArgb(31, 31, 31), 4, 3)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 4)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 5)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 6)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 7)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 8)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 9)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 10)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 11)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 12)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 13)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 14)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 15)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 16)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 17)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 18)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 19)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 20)
        DrawPixel(Color.FromArgb(31, 31, 31), 4, 21)
        DrawPixel(Color.FromArgb(32, 32, 32), 4, 22)
        DrawPixel(Color.FromArgb(64, 64, 64), 4, 23)

        DrawPixel(Color.FromArgb(51, 51, 51), Width - 0, 0)
        DrawPixel(Color.FromArgb(51, 51, 51), Width - 0, 1)
        DrawPixel(Color.FromArgb(44, 44, 44), Width - 0, 2)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 0, 3)
        DrawPixel(Color.FromArgb(24, 24, 24), Width - 0, 4)
        DrawPixel(Color.FromArgb(22, 22, 22), Width - 0, 5)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 6)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 7)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 8)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 9)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 10)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 11)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 12)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 13)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 14)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 15)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 16)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 0, 17)
        DrawPixel(Color.FromArgb(25, 25, 25), Width - 0, 18)
        DrawPixel(Color.FromArgb(29, 29, 29), Width - 0, 19)
        DrawPixel(Color.FromArgb(44, 44, 44), Width - 0, 20)
        DrawPixel(Color.FromArgb(51, 51, 51), Width - 0, 21)
        DrawPixel(Color.FromArgb(51, 51, 51), Width - 0, 22)
        DrawPixel(Color.FromArgb(51, 51, 51), Width - 0, 23)
        DrawPixel(Color.FromArgb(51, 51, 51), Width - 1, 0)
        DrawPixel(Color.FromArgb(37, 37, 37), Width - 1, 1)
        DrawPixel(Color.FromArgb(23, 23, 23), Width - 1, 2)
        DrawPixel(Color.FromArgb(24, 24, 24), Width - 1, 3)
        DrawPixel(Color.FromArgb(27, 27, 27), Width - 1, 4)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 5)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 6)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 7)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 8)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 9)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 10)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 11)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 12)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 13)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 14)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 15)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 16)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 17)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 1, 18)
        DrawPixel(Color.FromArgb(27, 27, 27), Width - 1, 19)
        DrawPixel(Color.FromArgb(26, 26, 26), Width - 1, 20)
        DrawPixel(Color.FromArgb(40, 40, 40), Width - 1, 21)
        DrawPixel(Color.FromArgb(51, 51, 51), Width - 1, 22)
        DrawPixel(Color.FromArgb(51, 51, 51), Width - 1, 23)
        DrawPixel(Color.FromArgb(44, 44, 44), Width - 2, 0)
        DrawPixel(Color.FromArgb(22, 22, 22), Width - 2, 1)
        DrawPixel(Color.FromArgb(26, 26, 26), Width - 2, 2)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 2, 3)
        DrawPixel(Color.FromArgb(29, 29, 29), Width - 2, 4)
        DrawPixel(Color.FromArgb(30, 30, 30), Width - 2, 5)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 6)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 7)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 8)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 9)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 10)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 11)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 12)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 13)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 14)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 15)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 16)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 17)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 2, 18)
        DrawPixel(Color.FromArgb(30, 30, 30), Width - 2, 19)
        DrawPixel(Color.FromArgb(29, 29, 29), Width - 2, 20)
        DrawPixel(Color.FromArgb(26, 26, 26), Width - 2, 21)
        DrawPixel(Color.FromArgb(51, 51, 51), Width - 2, 22)
        DrawPixel(Color.FromArgb(51, 51, 51), Width - 2, 23)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 3, 0)
        DrawPixel(Color.FromArgb(24, 24, 24), Width - 3, 1)
        DrawPixel(Color.FromArgb(28, 28, 28), Width - 3, 2)
        DrawPixel(Color.FromArgb(30, 30, 30), Width - 3, 3)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 3, 4)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 5)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 6)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 7)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 8)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 9)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 10)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 11)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 12)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 13)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 14)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 15)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 16)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 17)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 18)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 3, 19)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 3, 20)
        DrawPixel(Color.FromArgb(29, 29, 29), Width - 3, 21)
        DrawPixel(Color.FromArgb(40, 40, 40), Width - 3, 22)
        DrawPixel(Color.FromArgb(56, 56, 56), Width - 3, 23)
        DrawPixel(Color.FromArgb(25, 25, 25), Width - 4, 0)
        DrawPixel(Color.FromArgb(26, 26, 26), Width - 4, 1)
        DrawPixel(Color.FromArgb(29, 29, 29), Width - 4, 2)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 4, 3)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 4)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 5)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 6)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 7)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 8)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 9)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 10)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 11)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 12)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 13)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 14)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 15)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 16)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 17)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 18)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 19)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 20)
        DrawPixel(Color.FromArgb(31, 31, 31), Width - 4, 21)
        DrawPixel(Color.FromArgb(32, 32, 32), Width - 4, 22)
        DrawPixel(Color.FromArgb(64, 64, 64), Width - 4, 23)


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