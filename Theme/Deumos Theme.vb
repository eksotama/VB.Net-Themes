Imports System, System.Collections
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms

'------------------
'Creator: aeonhack
'Site: *********
'Created: 7/8/2011
'Changed: 11/2/2011
'Version: 1.2.0
'Theme Base: 1.5.3
'------------------
Class DeumosTheme
    Inherits ThemeContainer153


    Private _HeaderOffset As Integer = 1
    Property HeaderOffset() As Integer
        Get
            Return _HeaderOffset
        End Get
        Set(ByVal value As Integer)
            If value < 0 OrElse value > 2 Then Return

            _HeaderOffset = value
            Invalidate()
        End Set
    End Property


    Private Blend As ColorBlend

    Sub New()
        Header = 24
        TransparencyKey = Color.Fuchsia

        Path = New GraphicsPath

        Blend = New ColorBlend
        Blend.Positions = New Single() {0, 0.4F, 0.6F, 1}

        SetColor("BackColor", 14, 14, 14)
        SetColor("CornerGradient1", 48, 48, 48)
        SetColor("CornerGradient2", 4, 4, 4)
        SetColor("TextShadow", Color.Black)
        SetColor("Text", Color.White)
        SetColor("BorderHighlight", 26, Color.White)
        SetColor("Border", 45, 45, 45)
        SetColor("Outline", Color.Black)
        SetColor("CornerHighlight", 15, Color.White)
        SetColor("TitleShine", 100, 100, 100)
        SetColor("TitleGloss", 42, Color.White)
        SetColor("CornerGloss", 15, Color.White)
        SetColor("TitleGradient1", 14, 14, 14)
        SetColor("TitleGradient2", 41, 41, 41)

        BackColor = Color.FromArgb(14, 14, 14)
    End Sub

    Protected Overrides Sub ColorHook()
        Blend.Colors = New Color() {GetColor("TitleGradient1"), GetColor("TitleGradient2"), GetColor("TitleGradient2"), GetColor("TitleGradient1")}

        C1 = GetColor("BackColor")
        C2 = GetColor("CornerGradient1")
        C3 = GetColor("CornerGradient2")

        P1 = New Pen(GetColor("Border"), 8)
        P1.Alignment = PenAlignment.Inset

        P2 = New Pen(GetColor("Outline"))
        P3 = New Pen(GetColor("CornerHighlight"), 2)
        P4 = New Pen(GetColor("TitleShine"))
        P5 = New Pen(GetColor("BorderHighlight"))

        B1 = New SolidBrush(GetColor("TitleGloss"))
        B2 = New SolidBrush(GetColor("CornerGloss"))
        B3 = New SolidBrush(GetColor("TextShadow"))
        B4 = New SolidBrush(GetColor("Text"))

        BackColor = C1
    End Sub

    Private C1, C2, C3 As Color
    Private P1, P2, P3, P4, P5 As Pen
    Private B1, B2, B3, B4 As SolidBrush

    Private R1 As Rectangle
    Private G1 As LinearGradientBrush

    Protected Overrides Sub PaintHook()
        'Draw background
        G.Clear(C1)
        G.FillRectangle(Brushes.Fuchsia, 0, 0, Width, 3)

        'Draw border
        DrawBorders(P1, 0, 17, Width + 1, Height - 16) 'Border

        'Draw outline
        DrawBorders(P5, 1, 23, Width - 2, Height - 24)
        DrawBorders(P2, 0, 24, Width, Height - 24) 'InnerOutline
        DrawBorders(P2, 7, 23, Width - 14, Height - 30) 'OuterOutline

        'Draw title gradient
        R1 = New Rectangle(30, _HeaderOffset, Width - 67, 24 - _HeaderOffset)
        G1 = New LinearGradientBrush(R1, Color.Empty, Color.Empty, 0.0F)
        G1.InterpolationColors = Blend
        G.FillRectangle(G1, R1)

        'Draw title gloss
        G.FillRectangle(B1, 30, _HeaderOffset, Width - 67, 12 - _HeaderOffset)
        G.DrawLine(P4, 30, _HeaderOffset + 1, Width - 67, _HeaderOffset + 1)

        'Draw title outline
        G.DrawLine(P2, 30, _HeaderOffset, Width - 67, _HeaderOffset) 'TitleOutline

        'Draw corner gradient
        G.SetClip(Path)
        R1 = New Rectangle(0, 0, Width, 24)
        G1 = New LinearGradientBrush(R1, C2, C3, 90S)
        G.FillRectangle(G1, R1)

        'Draw corner gloss
        G.FillRectangle(B2, 0, 0, Width, 11)
        G.DrawPath(P3, PathClone)
        G.ResetClip()

        'Draw corner outline
        G.DrawPath(P2, Path) 'CornerOutline

        'Draw title elements
        DrawText(B3, HorizontalAlignment.Center, -21, _HeaderOffset + 1)
        DrawText(B4, HorizontalAlignment.Center, -20, _HeaderOffset)
        DrawImage(HorizontalAlignment.Left, 11, 0)
    End Sub

    Protected Overrides Sub OnCreation()
        Parent.MinimumSize = New Size(120, 80)
    End Sub

    Private Path, PathClone As GraphicsPath
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        Path.Reset()

        Path.AddLine(3, 0, 31, 0)
        Path.AddLine(55, 24, 0, 24)
        Path.AddLine(0, 24, 0, 3)
        Path.CloseFigure()

        Path.AddLine(Width - 68, 0, Width - 4, 0)
        Path.AddLine(Width - 1, 3, Width - 1, 24)
        Path.AddLine(Width - 1, 24, Width - 92, 24)
        Path.CloseFigure()

        PathClone = DirectCast(Path.Clone, GraphicsPath)
        PathClone.Widen(Pens.Black)

        MyBase.OnResize(e)
    End Sub

End Class

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 7/8/2011
'Changed: 8/13/2011
'Version: 1.2.0
'Theme Base: 1.5.3
'------------------
Class DeumosButton
    Inherits ThemeControl153

    Sub New()
        SetColor("Back", 14, 14, 14)
        SetColor("DownGradient1", 14, 14, 14)
        SetColor("DownGradient2", 41, 41, 41)
        SetColor("OverShine", 5, Color.White)
        SetColor("GlossGradient1", 30, Color.White)
        SetColor("GlossGradient2", 5, Color.White)
        SetColor("Highlight1", 62, 62, 62)
        SetColor("Highlight2", 15, Color.White)
        SetColor("Border", Color.Black)
        SetColor("Corners", 16, 16, 16)
        SetColor("Text", Color.White)
    End Sub

    Protected Overrides Sub ColorHook()
        C1 = GetColor("Back")
        C2 = GetColor("DownGradient1")
        C3 = GetColor("DownGradient2")
        C4 = GetColor("GlossGradient1")
        C5 = GetColor("GlossGradient2")
        C6 = GetColor("Corners")

        B1 = New SolidBrush(GetColor("OverShine"))
        B2 = New SolidBrush(GetColor("Text"))

        P1 = New Pen(GetColor("Highlight1"))
        P2 = New Pen(GetColor("Highlight2"))
        P3 = New Pen(GetColor("Border"))
    End Sub

    Private C1, C2, C3, C4, C5, C6 As Color
    Private B1, B2 As SolidBrush
    Private P1, P2, P3 As Pen

    Protected Overrides Sub PaintHook()
        G.Clear(C1)

        If State = MouseState.Down Then
            DrawGradient(C2, C3, 0, 0, Width, Height, 90S)
        End If

        If State = MouseState.Over Then
            G.FillRectangle(B1, ClientRectangle)
        End If

        DrawGradient(C4, C5, 0, 0, Width, Height \ 2, 90S)

        G.DrawLine(P1, 0, 1, Width, 1)
        DrawBorders(P2, ClientRectangle, 1)

        DrawBorders(P3, ClientRectangle)

        DrawCorners(C6, New Rectangle(1, 1, Width - 2, Height - 2))
        DrawCorners(BackColor, ClientRectangle)

        DrawText(B2, HorizontalAlignment.Center, 0, 0)
    End Sub

End Class

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 8/13/2011
'Changed: 11/2/2011
'Version: 1.2.0
'Theme Base: 1.5.3
'------------------
<DefaultEvent("CheckedChanged")> _
Class DeumosCheckBox
    Inherits ThemeControl153

    Sub New()
        LockHeight = 16

        SetColor("Border", 26, 26, 26)
        SetColor("Gloss1", 35, Color.White)
        SetColor("Gloss2", 5, Color.White)
        SetColor("Checked1", Color.Transparent)
        SetColor("Checked2", 40, Color.White)
        SetColor("Unchecked1", 8, 8, 8)
        SetColor("Unchecked2", 16, 16, 16)
        SetColor("Glow", 5, Color.White)
        SetColor("Text", Color.White)
        SetColor("InnerOutline", Color.Black)
        SetColor("OuterOutline", Color.Black)
    End Sub

    Private C1, C2, C3, C4, C5, C6 As Color
    Private P1, P2, P3 As Pen
    Private B1, B2 As SolidBrush

    Protected Overrides Sub ColorHook()
        C1 = GetColor("Gloss1")
        C2 = GetColor("Gloss2")
        C3 = GetColor("Checked1")
        C4 = GetColor("Checked2")
        C5 = GetColor("Unchecked1")
        C6 = GetColor("Unchecked2")

        P1 = New Pen(GetColor("Border"))
        P2 = New Pen(GetColor("InnerOutline"))
        P3 = New Pen(GetColor("OuterOutline"))

        B1 = New SolidBrush(GetColor("Glow"))
        B2 = New SolidBrush(GetColor("Text"))
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)

        DrawBorders(P1, 0, 0, _Field, _Field, 1)
        DrawGradient(C1, C2, 0, 0, _Field, _Field \ 2)

        If _Checked Then
            DrawGradient(C3, C4, 2, 2, _Field - 4, _Field - 4)
        Else
            DrawGradient(C5, C6, 2, 2, _Field - 4, _Field - 4, 90)
        End If

        If State = MouseState.Over Then
            G.FillRectangle(B1, 0, 0, _Field, _Field)
        End If

        DrawText(B2, HorizontalAlignment.Left, _Field + 3, 0)

        DrawBorders(P2, 0, 0, _Field, _Field, 2)
        DrawBorders(P3, 0, 0, _Field, _Field)

        DrawCorners(BackColor, 0, 0, _Field, _Field)
    End Sub

    Private _Field As Integer = 16
    Property Field() As Integer
        Get
            Return _Field
        End Get
        Set(ByVal value As Integer)
            If value < 4 Then Return
            _Field = value
            LockHeight = value
            Invalidate()
        End Set
    End Property

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

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 8/17/2011
'Changed: 11/2/2011
'Version: 1.2.0
'Theme Base: 1.5.3
'------------------
<DefaultEvent("CheckedChanged")> _
Class DeumosRadioButton
    Inherits ThemeControl153

    Sub New()
        LockHeight = 16

        SetColor("Gloss1", 38, Color.White)
        SetColor("Gloss2", 5, Color.White)
        SetColor("Checked1", Color.Transparent)
        SetColor("Checked2", 40, Color.White)
        SetColor("Unchecked1", 8, 8, 8)
        SetColor("Unchecked2", 16, 16, 16)
        SetColor("Glow", 5, Color.White)
        SetColor("Text", Color.White)
        SetColor("InnerOutline", Color.Black)
        SetColor("OuterOutline", 15, Color.White)
    End Sub

    Protected Overrides Sub ColorHook()
        C1 = GetColor("Gloss1")
        C2 = GetColor("Gloss2")
        C3 = GetColor("Checked1")
        C4 = GetColor("Checked2")
        C5 = GetColor("Unchecked1")
        C6 = GetColor("Unchecked2")

        B1 = New SolidBrush(GetColor("Glow"))
        B2 = New SolidBrush(GetColor("Text"))

        P1 = New Pen(GetColor("InnerOutline"))
        P2 = New Pen(GetColor("OuterOutline"))
    End Sub

    Private C1, C2, C3, C4, C5, C6 As Color
    Private P1, P2 As Pen
    Private B1, B2 As SolidBrush

    Private R1, R2 As Rectangle
    Private G1 As LinearGradientBrush

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality
        R1 = New Rectangle(4, 2, _Field - 8, (_Field \ 2) - 1)
        R2 = New Rectangle(4, 2, _Field - 8, (_Field \ 2))

        G1 = New LinearGradientBrush(R2, C1, C2, 90S)
        G.FillEllipse(G1, R1)

        R1 = New Rectangle(2, 2, _Field - 4, _Field - 4)

        If _Checked Then
            G1 = New LinearGradientBrush(R1, C3, C4, 90S)
        Else
            G1 = New LinearGradientBrush(R1, C5, C6, 90S)
        End If
        G.FillEllipse(G1, R1)

        If State = MouseState.Over Then
            R1 = New Rectangle(2, 2, _Field - 4, _Field - 4)
            G.FillEllipse(B1, R1)
        End If

        DrawText(B2, HorizontalAlignment.Left, _Field + 3, 0)

        G.DrawEllipse(P1, 2, 2, _Field - 4, _Field - 4)
        G.DrawEllipse(P2, 1, 1, _Field - 2, _Field - 2)

    End Sub

    Private _Field As Integer = 16
    Property Field() As Integer
        Get
            Return _Field
        End Get
        Set(ByVal value As Integer)
            If value < 4 Then Return
            _Field = value
            LockHeight = value
            Invalidate()
        End Set
    End Property

    Private _Checked As Boolean
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            InvalidateControls()
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnMouseDown(e)
    End Sub

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnCreation()
        InvalidateControls()
    End Sub

    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is DeumosRadioButton Then
                DirectCast(C, DeumosRadioButton).Checked = False
            End If
        Next
    End Sub

End Class

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 8/13/2011
'Changed: 10/7/2011
'Version: 1.2.0
'Theme Base: 1.5.3
'------------------
Class DeumosGroupBox
    Inherits ThemeContainer153

    Sub New()
        ControlMode = True

        SetColor("Back", 14, 14, 14)
        SetColor("MainFill", 20, 20, 20)
        SetColor("MainOutline1", 32, 32, 32)
        SetColor("MainOutline2", Color.Black)
        SetColor("TitleShadow", 50, Color.Black)
        SetColor("TitleFill", 20, 20, 20)
        SetColor("Text", Color.White)
        SetColor("TitleOutline1", 32, 32, 32)
        SetColor("TitleOutline2", Color.Black)

        BackColor = Color.FromArgb(20, 20, 20)
    End Sub

    Protected Overrides Sub ColorHook()
        C1 = GetColor("Back")

        B1 = New SolidBrush(GetColor("MainFill"))
        B2 = New SolidBrush(GetColor("TitleFill"))
        B3 = New SolidBrush(GetColor("Text"))

        P1 = New Pen(GetColor("MainOutline1"))
        P2 = New Pen(GetColor("MainOutline2"))
        P3 = New Pen(GetColor("TitleShadow"))
        P4 = New Pen(GetColor("TitleOutline1"))
        P5 = New Pen(GetColor("TitleOutline2"))

        BackColor = GetColor("MainFill")
    End Sub

    Private C1 As Color
    Private B1, B2, B3 As SolidBrush
    Private P1, P2, P3, P4, P5 As Pen

    Private S1 As Size
    Private R1 As Rectangle

    Protected Overrides Sub PaintHook()
        G.Clear(C1)

        If Me.Text.Length = 0 Then
            G.FillRectangle(B1, ClientRectangle)
            DrawBorders(P1, 1)
            DrawBorders(P2)
        Else
            G.FillRectangle(B1, 0, 13, Width, Height - 13)
            DrawBorders(P1, 0, 13, Width, Height - 13, 1)
            DrawBorders(P2, 0, 13, Width, Height - 13)


            S1 = Measure()
            R1 = New Rectangle(8, 0, S1.Width + 16, 26)

            DrawBorders(P3, 7, 0, R1.Width + 1, 27)

            G.FillRectangle(B2, R1)

            DrawText(B3, Center(R1, S1))

            DrawBorders(P4, R1, 1)
            DrawBorders(P5, R1)

            DrawCorners(C1, 0, 13, Width, Height - 13)
        End If

    End Sub

End Class

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 11/2/2011
'Changed: 11/2/2011
'Version: 1.2.0
'Theme Base: 1.5.3
'------------------
<DefaultEvent("TextChanged")> _
Class DeumosTextBox
    Inherits ThemeControl153

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

        Base.Location = New Point(5, 5)
        Base.Width = Width - 10

        If _Multiline Then
            Base.Height = Height - 11
        Else
            LockHeight = Base.Height + 11
        End If

        AddHandler Base.TextChanged, AddressOf OnBaseTextChanged
        AddHandler Base.KeyDown, AddressOf OnBaseKeyDown


        SetColor("Text", Color.White)
        SetColor("Back", 7, 7, 7)
        SetColor("Border1", Color.Black)
        SetColor("Border2", 20, 20, 20)
    End Sub

    Private C1 As Color
    Private P1, P2 As Pen

    Protected Overrides Sub ColorHook()
        C1 = GetColor("Back")

        P1 = GetPen("Border1")
        P2 = GetPen("Border2")

        Base.ForeColor = GetColor("Text")
        Base.BackColor = C1
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(C1)

        DrawBorders(P1, 1)
        DrawBorders(P2)
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
        Base.Location = New Point(5, 5)
        Base.Width = Width - 10

        If _Multiline Then
            Base.Height = Height - 11
        End If


        MyBase.OnResize(e)
    End Sub

End Class

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 11/2/2011
'Changed: 11/2/2011
'Version: 1.2.0
'Theme Base: 1.5.3
'------------------
Class DeumosProgressBar
    Inherits ThemeControl153

    Private _Minimum As Integer
    Property Minimum() As Integer
        Get
            Return _Minimum
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                Throw New Exception("Property value is not valid.")
            End If

            _Minimum = value
            If value > _Value Then _Value = value
            If value > _Maximum Then _Maximum = value
            Invalidate()
        End Set
    End Property

    Private _Maximum As Integer = 100
    Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then
                Throw New Exception("Property value is not valid.")
            End If

            _Maximum = value
            If value < _Value Then _Value = value
            If value < _Minimum Then _Minimum = value
            Invalidate()
        End Set
    End Property

    Private _Value As Integer
    Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value > _Maximum OrElse value < _Minimum Then
                Throw New Exception("Property value is not valid.")
            End If

            _Value = value
            Invalidate()
        End Set
    End Property

    Private Sub Increment(ByVal amount As Integer)
        Value += amount
    End Sub

    Sub New()
        SetColor("Frame", 18, 18, 18)
        SetColor("Border1", Color.Black)
        SetColor("Border2", Color.Black)
        SetColor("Gloss1", 30, Color.White)
        SetColor("Gloss2", 5, Color.White)
        SetColor("Back1", 2, 2, 2)
        SetColor("Back2", 8, 8, 8)
        SetColor("Progress1", 38, 38, 38)
        SetColor("Progress2", 52, 52, 52)
        SetColor("ProgressShine", 16, Color.White)
        SetColor("ProgressGloss1", 32, Color.White)
        SetColor("ProgressGloss2", 12, Color.White)
    End Sub

    Private C1, C2, C3, C4, C5, C6, C7, C8, C9 As Color
    Private P1, P2, P3 As Pen

    Protected Overrides Sub ColorHook()
        C1 = GetColor("Frame")
        C2 = GetColor("Gloss1")
        C3 = GetColor("Gloss2")
        C4 = GetColor("Back1")
        C5 = GetColor("Back2")
        C6 = GetColor("Progress1")
        C7 = GetColor("Progress2")
        C8 = GetColor("ProgressGloss1")
        C9 = GetColor("ProgressGloss2")

        P1 = GetPen("Border1")
        P2 = GetPen("Border2")
        P3 = GetPen("ProgressShine")
    End Sub

    Private R1 As Rectangle
    Private I1 As Integer

    Protected Overrides Sub PaintHook()
        G.Clear(C1)
        DrawGradient(C2, C3, 0, 0, Width, Height \ 2)

        DrawGradient(C4, C5, 2, 2, Width - 4, Height - 4)
        DrawBorders(P1, 2)

        I1 = CInt((_Value - _Minimum) / (_Maximum - _Minimum) * (Width - 6))

        If Not I1 = 0 Then
            R1 = New Rectangle(3, 3, I1, Height - 6)

            DrawGradient(C6, C7, R1, 35.0F)
            DrawBorders(P3, R1)

            DrawGradient(C8, C9, 3, 3, I1, Height \ 2 - 3)
        End If

        DrawBorders(P2)
        DrawCorners(BackColor)
    End Sub

End Class

'------------------
'Creator: aeonhack
'Site: elitevs.net
'Created: 10/7/2011
'Changed: 10/7/2011
'Version: 1.2.0
'Theme Base: 1.5.3
'------------------
Class DeumosSeperator
    Inherits ThemeControl153


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
        LockHeight = 14

        SetColor("Line1", Color.Black)
        SetColor("Line2", 22, 22, 22)
    End Sub

    Private P1, P2 As Pen

    Protected Overrides Sub ColorHook()
        P1 = GetPen("Line1")
        P2 = GetPen("Line2")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)

        If _Orientation = Windows.Forms.Orientation.Vertical Then
            G.DrawLine(P1, 6, 0, 6, Height)
            G.DrawLine(P2, 7, 0, 7, Height)
        Else
            G.DrawLine(P1, 0, 6, Width, 6)
            G.DrawLine(P2, 0, 7, Width, 7)
        End If

    End Sub
End Class