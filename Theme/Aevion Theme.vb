Imports System.Drawing.Drawing2D

Module Helpers

    Public AevionFont As New Font("Segoe UI", 9)
    Public AevionFontBold As New Font("Segoe UI", 9, FontStyle.Bold)
    Public AevionBack As Color = Color.FromArgb(48, 57, 65)

    Public Enum MouseState As Byte
        None
        Hover
        Down
    End Enum

    Public Enum Direction
        Up = 1
        Down = 2
        Left = 3
        Right = 4
    End Enum

    Public Sub RoundRect(ByVal G As Graphics, _
                              ByVal X As Integer, _
                              ByVal Y As Integer, _
                              ByVal Width As Integer, _
                              ByVal Height As Integer, _
                              ByVal Curve As Integer, _
                              ByVal Draw As Color)


        Dim BaseRect As New RectangleF(X, Y, Width,
                                      Height)
        Dim ArcRect As New RectangleF(BaseRect.Location,
                                  New SizeF(Curve, Curve))

        G.DrawArc(New Pen(Draw), ArcRect, 180, 90)
        G.DrawLine(New Pen(Draw), X + CInt(Curve / 2),
                             Y,
                             Y + Width - CInt(Curve / 2),
                             Y)

        ArcRect.X = BaseRect.Right - Curve
        G.DrawArc(New Pen(Draw), ArcRect, 270, 90)
        G.DrawLine(New Pen(Draw), X + Width,
                             Y + CInt(Curve / 2),
                             X + Width,
                             Y + Height - CInt(Curve / 2))

        ArcRect.Y = BaseRect.Bottom - Curve
        G.DrawArc(New Pen(Draw), ArcRect, 0, 90)
        G.DrawLine(New Pen(Draw), X + CInt(Curve / 2),
                             Y + Height,
                             X + Width - CInt(Curve / 2),
                             Y + Height)

        ArcRect.X = BaseRect.Left
        G.DrawArc(New Pen(Draw), ArcRect, 90, 90)
        G.DrawLine(New Pen(Draw),
                             X, Y + CInt(Curve / 2),
                             X,
                             Y + Height - CInt(Curve / 2))

    End Sub

    Public Sub CenterString(G As Graphics, text As String, font As Font, brush As Brush, rect As Rectangle, Optional shadow As Boolean = False, Optional yOffset As Integer = 0)

        Dim textSize As SizeF = G.MeasureString(text, font)
        Dim textX As Integer = rect.X + (rect.Width / 2) - (textSize.Width / 2)
        Dim textY As Integer = rect.Y + (rect.Height / 2) - (textSize.Height / 2) + yOffset

        If shadow Then G.DrawString(text, font, Brushes.Black, textX + 1, textY + 1)
        G.DrawString(text, font, brush, textX, textY + 1)

    End Sub

    Public Sub DrawTriangle(G As Graphics, Rect As Rectangle, direction__1 As Direction, Draw As Color)
        Dim halfWidth As Integer = Rect.Width / 2
        Dim halfHeight As Integer = Rect.Height / 2
        Dim p0 As Point = Point.Empty
        Dim p1 As Point = Point.Empty
        Dim p2 As Point = Point.Empty

        Select Case direction__1
            Case Direction.Up
                p0 = New Point(Rect.Left + halfWidth, Rect.Top)
                p1 = New Point(Rect.Left, Rect.Bottom)
                p2 = New Point(Rect.Right, Rect.Bottom)
                Exit Select
            Case Direction.Down
                p0 = New Point(Rect.Left + halfWidth, Rect.Bottom)
                p1 = New Point(Rect.Left, Rect.Top)
                p2 = New Point(Rect.Right, Rect.Top)
                Exit Select
            Case Direction.Left
                p0 = New Point(Rect.Left, Rect.Top + halfHeight)
                p1 = New Point(Rect.Right, Rect.Top)
                p2 = New Point(Rect.Right, Rect.Bottom)
                Exit Select
            Case Direction.Right
                p0 = New Point(Rect.Right, Rect.Top + halfHeight)
                p1 = New Point(Rect.Left, Rect.Bottom)
                p2 = New Point(Rect.Left, Rect.Top)
                Exit Select
        End Select

        G.FillPolygon(New SolidBrush(Draw), New Point() {p0, p1, p2})

    End Sub

End Module

Class AevionForm
    Inherits Control

    Sub New()
        DoubleBuffered = True
        Font = AevionFont
        ForeColor = Color.White
        BackColor = AevionBack
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Dock = DockStyle.Fill
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        MyBase.OnPaint(e)

        G.Clear(AevionBack)

    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

End Class

Class AevionButton
    Inherits Button

    Private State As MouseState

    Enum Style
        DefaultStyle = 1
        GreenStyle = 2
        RedStyle = 3
    End Enum

    Private _ButtonStyle As Style
    Public Property ButtonStyle As Style
        Get
            Return _ButtonStyle
        End Get
        Set(value As Style)
            _ButtonStyle = value
            Invalidate()
        End Set
    End Property

    Private _ImagePath As Image
    Public Property ImagePath As Image
        Get
            Return _ImagePath
        End Get
        Set(value As Image)
            _ImagePath = value
            Invalidate()
        End Set
    End Property

    Private _ShowIcon As Boolean
    Public Property ShowIcon As Boolean
        Get
            Return _ShowIcon
        End Get
        Set(value As Boolean)
            _ShowIcon = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        ButtonStyle = Style.DefaultStyle
        ShowIcon = False
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality

        MyBase.OnPaint(e)

        G.Clear(AevionBack)

        If ButtonStyle = Style.RedStyle Then

            Select Case State

                Case MouseState.Down

                    Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(173, 83, 74), Color.Black, LinearGradientMode.Vertical)
                    G.FillRectangle(Linear, New Rectangle(1, 1, Width - 2, Height - 2))

                Case MouseState.Hover
                    Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(193, 103, 94), Color.Black, LinearGradientMode.Vertical)
                    G.FillRectangle(Linear, New Rectangle(1, 1, Width - 2, Height - 2))

                Case Else

                    Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(183, 93, 84), Color.Black, LinearGradientMode.Vertical)
                    G.FillRectangle(Linear, New Rectangle(1, 1, Width - 2, Height - 2))

            End Select

        ElseIf ButtonStyle = Style.GreenStyle Then

            Select Case State

                Case MouseState.Down

                    Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(127, 177, 80), Color.Black, LinearGradientMode.Vertical)
                    G.FillRectangle(Linear, New Rectangle(1, 1, Width - 2, Height - 2))

                Case MouseState.Hover
                    Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(157, 197, 100), Color.Black, LinearGradientMode.Vertical)
                    G.FillRectangle(Linear, New Rectangle(1, 1, Width - 2, Height - 2))

                Case Else

                    Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(147, 187, 90), Color.Black, LinearGradientMode.Vertical)
                    G.FillRectangle(Linear, New Rectangle(1, 1, Width - 2, Height - 2))

            End Select

        Else

            Select Case State

                Case MouseState.Down

                    Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(88, 105, 123), Color.Black, LinearGradientMode.Vertical)
                    G.FillRectangle(Linear, New Rectangle(1, 1, Width - 2, Height - 2))

                Case MouseState.Hover
                    Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(108, 125, 143), Color.Black, LinearGradientMode.Vertical)
                    G.FillRectangle(Linear, New Rectangle(1, 1, Width - 2, Height - 2))

                Case Else

                    Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(98, 115, 133), Color.Black, LinearGradientMode.Vertical)
                    G.FillRectangle(Linear, New Rectangle(1, 1, Width - 2, Height - 2))

            End Select

        End If

        RoundRect(G, 0, 0, Width - 1, Height - 1, 3, Color.FromArgb(38, 38, 38))

        If ShowIcon Then
            G.DrawImage(ImagePath, New Point(Width / 8, Height / 2 - 8))
        End If

        CenterString(G, Text, AevionFontBold, Brushes.White, New Rectangle(0, 0, Width, Height))


    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Hover : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Hover : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

End Class

Class AevionRadioButton
    Inherits Control

    Event CheckedChanged(sender As Object)

    Private _Checked As Boolean
    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Size = New Point(Width, 16)
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality

        MyBase.OnPaint(e)

        G.Clear(AevionBack)

        Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(98, 115, 133), Color.Black, LinearGradientMode.Vertical)

        G.FillEllipse(Linear, New Rectangle(1, 1, 14, 14))
        G.DrawEllipse(New Pen(Color.FromArgb(35, 35, 40)), New Rectangle(0, 0, 15, 15))

        If Checked Then
            G.FillEllipse(New SolidBrush(Color.FromArgb(220, 220, 255)), New Rectangle(5, 5, 5, 5))
        End If

        G.DrawString(Text, AevionFont, Brushes.White, New Point(20, 0))

    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If Checked Then
            Checked = False
        Else
            Checked = True
        End If

    End Sub

End Class

Class AevionCheckBox
    Inherits Control

    Event CheckedChanged(sender As Object)

    Private _Checked As Boolean
    Public Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Size = New Point(Width, 16)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality

        MyBase.OnPaint(e)

        G.Clear(AevionBack)

        Dim Linear As New LinearGradientBrush(New Rectangle(0, 0, Width, Height + 35), Color.FromArgb(98, 115, 133), Color.Black, LinearGradientMode.Vertical)
        G.FillRectangle(Linear, New Rectangle(1, 1, 13, 13))
        RoundRect(G, 0, 0, 14, 14, 3, Color.FromArgb(35, 35, 40))

        If Checked Then
            CenterString(G, "b", New Font("Marlett", 10), Brushes.White, New Rectangle(2, 1, 13, 13))
        End If

        G.DrawString(Text, AevionFont, Brushes.White, New Point(20, -1))

    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)

        If Checked Then
            Checked = False
        Else
            Checked = True
        End If

    End Sub

End Class

Class AevionProgressBar
    Inherits Control

    Private _Maximum As Integer
    Public Property Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(value As Integer)
            _Maximum = value
            Invalidate()
        End Set
    End Property

    Private _Minimum As Integer
    Public Property Minimum As Integer
        Get
            Return _Minimum
        End Get
        Set(value As Integer)
            _Minimum = value
            Invalidate()
        End Set
    End Property

    Private _Value As Integer
    Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(value As Integer)
            _Value = value
            Invalidate()
        End Set
    End Property

    Private _Showtext As Boolean
    Public Property Showtext As Boolean
        Get
            Return _Showtext
        End Get
        Set(value As Boolean)
            _Showtext = value
            Invalidate()
        End Set
    End Property

    Sub New()
        DoubleBuffered = True
        Maximum = 100
        Minimum = 0
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality

        MyBase.OnPaint(e)

        G.Clear(AevionBack)

        Dim Linear As New LinearGradientBrush(New Point(0, 0), New Point(Width + Value + 50, Height), Color.FromArgb(98, 115, 133), Color.Black)

        G.FillRectangle(Linear, New Rectangle(0, 0, CInt(Value / Maximum * Width), Height))

        RoundRect(G, 0, 0, Width - 1, Height - 1, 3, Color.FromArgb(38, 38, 38))

        If Showtext Then
            CenterString(G, Text, AevionFont, Brushes.White, New Rectangle(0, 0, Width, Height))
        End If

    End Sub

End Class

Class AevionNotice
    Inherits TextBox

    Sub New()
        DoubleBuffered = True
        Enabled = False
        Multiline = True
        BorderStyle = Windows.Forms.BorderStyle.None
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
    End Sub  

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality

        MyBase.OnPaint(e)

        G.Clear(AevionBack)

        G.FillRectangle(New SolidBrush(Color.FromArgb(38, 38, 38)), New Rectangle(1, 1, Width - 2, Height - 2))
        RoundRect(G, 0, 0, Width - 1, Height - 1, 3, Color.FromArgb(35, 35, 40))

        G.DrawString(Text, AevionFont, Brushes.White, New Point(12, 8))


    End Sub


End Class

Class AevionLabel
    Inherits Label

    Sub New()
        DoubleBuffered = True
        Font = AevionFont
        ForeColor = Color.White
        BackColor = AevionBack
    End Sub

End Class