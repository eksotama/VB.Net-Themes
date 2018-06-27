Imports System.Drawing.Drawing2D, System.Drawing.Text, System.Drawing
Imports System.ComponentModel

Public Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
End Enum

Module Draw
    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
    Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
        Dim Rectangle As Rectangle = New Rectangle(X, Y, Width, Height)
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function
    Private Function ImageFromCode(ByRef str$) As Image
        Dim imageBytes As Byte() = Convert.FromBase64String(str)
        Dim ms As New IO.MemoryStream(imageBytes, 0, imageBytes.Length) : ms.Write(imageBytes, 0, imageBytes.Length)
        Dim i As Image = Image.FromStream(ms, True) : Return i
    End Function
    Public Function TiledTextureFromCode(ByVal str As String) As TextureBrush
        Return New TextureBrush(Draw.ImageFromCode(str), WrapMode.Tile)
    End Function
End Module

Public Class RedemptionButton
    Inherits Control
    Dim MouseState As MouseState = MouseState.None
    Enum HorizontalAlignment As Byte
        Left
        Center
        Right
    End Enum
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Center
    Public Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            Invalidate()
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Arial", 8.25F, FontStyle.Bold)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim curve As Integer = 5
        Dim b As New Bitmap(Width, Height)
        Dim g As Graphics = Graphics.FromImage(b)
        g.SmoothingMode = SmoothingMode.HighQuality
        g.TextRenderingHint = TextRenderingHint.AntiAlias
        MyBase.OnPaint(e)
        If Enabled Then
            g.Clear(BackColor)
            Select Case MouseState
                Case MouseState.None
                    Dim MainBody As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(55, 62, 70), Color.FromArgb(43, 44, 48), 90S)
                    g.FillPath(MainBody, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
                    Dim GlossPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(93, 98, 104), Color.Transparent, 90S)
                    g.DrawPath(New Pen(GlossPen), Draw.RoundRect(New Rectangle(0, 1, Width - 1, Height - 1), curve + 1))
                Case MouseState.Over
                    Dim MainBody As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(72, 79, 87), Color.FromArgb(48, 51, 56), 90S)
                    g.FillPath(MainBody, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
                    Dim GlossPen As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 3), Color.FromArgb(119, 124, 130), Color.FromArgb(64, 67, 72), 90S)
                    g.DrawPath(New Pen(GlossPen), Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), curve))
                Case MouseState.Down
                    Dim MainBody As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(43, 44, 48), Color.FromArgb(51, 54, 59), 90S)
                    g.FillPath(MainBody, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
                    Dim GlossPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(55, 56, 60), Color.Transparent, 90S)
                    g.DrawPath(New Pen(GlossPen), Draw.RoundRect(New Rectangle(0, 1, Width - 1, Height - 1), curve + 1))
            End Select

            g.DrawPath(New Pen(Color.FromArgb(31, 36, 42)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
        Else

        End If


        Dim sf As New StringFormat()
        Select Case TextAlign
            Case HorizontalAlignment.Center
                sf.Alignment = StringAlignment.Center
                sf.LineAlignment = StringAlignment.Center
                g.DrawString(Text, Font, New SolidBrush(Color.FromArgb(16, 20, 21)), New Rectangle(1, 2, Width - 1, Height - 1), sf)
                g.DrawString(Text, Font, Brushes.White, New Rectangle(0, 1, Width - 1, Height - 1), sf)
            Case HorizontalAlignment.Left
                sf.Alignment = StringAlignment.Near
                sf.LineAlignment = StringAlignment.Center
                g.DrawString(Text, Font, New SolidBrush(Color.FromArgb(16, 20, 21)), New Rectangle(6, 2, Width - 1, Height - 1), sf)
                g.DrawString(Text, Font, Brushes.White, New Rectangle(5, 1, Width - 1, Height - 1), sf)
            Case HorizontalAlignment.Right
                sf.Alignment = StringAlignment.Far
                sf.LineAlignment = StringAlignment.Center
                g.DrawString(Text, Font, New SolidBrush(Color.FromArgb(16, 20, 21)), New Rectangle(-3, 2, Width - 1, Height - 1), sf)
                g.DrawString(Text, Font, Brushes.White, New Rectangle(-4, 1, Width - 1, Height - 1), sf)
        End Select


        e.Graphics.DrawImage(b, New Point(0, 0))
        g.Dispose()
        b.Dispose()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        If Enabled Then
            MyBase.OnMouseEnter(e) : MouseState = MouseState.Over : Invalidate() : Cursor = Cursors.Hand
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If Enabled Then
            MyBase.OnMouseDown(e) : MouseState = MouseState.Down : Invalidate() : Cursor = Cursors.Hand
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        If Enabled Then
            MyBase.OnMouseUp(e) : MouseState = MouseState.Over : Invalidate() : Cursor = Cursors.Hand
        End If
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        If Enabled Then
            MyBase.OnMouseLeave(e) : MouseState = MouseState.None : Invalidate() : Cursor = Cursors.Default
        End If
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
End Class

Public Class RedemptionRoundButton
    Inherits Control
    Dim MouseState As MouseState = MouseState.None
    Enum HorizontalAlignment As Byte
        Left
        Center
        Right
    End Enum
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Center
    Public Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            Invalidate()
        End Set
    End Property

    Public Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Arial", 8.25F, FontStyle.Bold)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim curve As Integer = 13
        Dim b As New Bitmap(Width, Height)
        Dim g As Graphics = Graphics.FromImage(b)
        g.SmoothingMode = SmoothingMode.HighQuality
        g.TextRenderingHint = TextRenderingHint.AntiAlias
        MyBase.OnPaint(e)
        If Enabled Then
            g.Clear(BackColor)
            Select Case MouseState
                Case MouseState.None
                    Dim MainBody As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(131, 198, 240), Color.FromArgb(24, 121, 218), 90S)
                    g.FillPath(MainBody, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
                    Dim GlossPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(145, 212, 254), Color.Transparent, 90S)
                    g.DrawPath(New Pen(GlossPen), Draw.RoundRect(New Rectangle(0, 1, Width - 1, Height - 2), curve + 1))
                Case MouseState.Over
                    Dim MainBody As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(150, 203, 235), Color.FromArgb(35, 135, 220), 90S)
                    g.FillPath(MainBody, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
                    Dim GlossPen As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 3), Color.FromArgb(173, 226, 255), Color.FromArgb(54, 155, 235), 90S)
                    g.DrawPath(New Pen(GlossPen), Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), curve))
                Case MouseState.Down
                    Dim MainBody As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(30, 121, 210), Color.FromArgb(84, 172, 236), 90S)
                    g.FillPath(MainBody, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
                    Dim GlossPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(54, 145, 234), Color.Transparent, 90S)
                    g.DrawPath(New Pen(GlossPen), Draw.RoundRect(New Rectangle(0, 1, Width - 1, Height - 1), curve + 1))
            End Select
            g.DrawPath(New Pen(Color.FromArgb(21, 38, 56)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
            g.FillRectangle(New SolidBrush(Parent.BackColor), New Rectangle(-1, -1, 2, 10))
            g.FillRectangle(New SolidBrush(Parent.BackColor), New Rectangle(-2, -1, 4, 9))
        Else

        End If


        Dim sf As New StringFormat()
        Select Case TextAlign
            Case HorizontalAlignment.Center
                sf.Alignment = StringAlignment.Center
                sf.LineAlignment = StringAlignment.Center
                g.DrawString(Text, Font, New SolidBrush(Color.FromArgb(16, 20, 21)), New Rectangle(1, 2, Width - 1, Height - 1), sf)
                g.DrawString(Text, Font, Brushes.White, New Rectangle(0, 1, Width - 1, Height - 1), sf)
            Case HorizontalAlignment.Left
                sf.Alignment = StringAlignment.Near
                sf.LineAlignment = StringAlignment.Center
                g.DrawString(Text, Font, New SolidBrush(Color.FromArgb(16, 20, 21)), New Rectangle(6, 2, Width - 1, Height - 1), sf)
                g.DrawString(Text, Font, Brushes.White, New Rectangle(5, 1, Width - 1, Height - 1), sf)
            Case HorizontalAlignment.Right
                sf.Alignment = StringAlignment.Far
                sf.LineAlignment = StringAlignment.Center
                g.DrawString(Text, Font, New SolidBrush(Color.FromArgb(16, 20, 21)), New Rectangle(-3, 2, Width - 1, Height - 1), sf)
                g.DrawString(Text, Font, Brushes.White, New Rectangle(-4, 1, Width - 1, Height - 1), sf)
        End Select


        e.Graphics.DrawImage(b, New Point(0, 0))
        g.Dispose()
        b.Dispose()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        If Enabled Then
            MyBase.OnMouseEnter(e) : MouseState = MouseState.Over : Invalidate() : Cursor = Cursors.Hand
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If Enabled Then
            MyBase.OnMouseDown(e) : MouseState = MouseState.Down : Invalidate() : Cursor = Cursors.Hand
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        If Enabled Then
            MyBase.OnMouseUp(e) : MouseState = MouseState.Over : Invalidate() : Cursor = Cursors.Hand
        End If
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        If Enabled Then
            MyBase.OnMouseLeave(e) : MouseState = MouseState.None : Invalidate() : Cursor = Cursors.Default
        End If
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
End Class

Public Class RedemptionTextBox : Inherits Control
    Dim WithEvents txtbox As New TextBox
#Region " Control Help - Properties & Flicker Control "
    Private _UsePassword As Boolean = False
    Public Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _UsePassword
        End Get
        Set(ByVal v As Boolean)
            txtbox.UseSystemPasswordChar = UseSystemPasswordChar
            _UsePassword = v
            Invalidate()
        End Set
    End Property
    Private _MaxCharacters As Integer = 32767
    Public Shadows Property MaxLength() As Integer
        Get
            Return _MaxCharacters
        End Get
        Set(ByVal v As Integer)
            _MaxCharacters = v
            txtbox.MaxLength = MaxLength
            Invalidate()
        End Set
    End Property
    Private _TextAlignment As HorizontalAlignment
    Public Shadows Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlignment
        End Get
        Set(ByVal v As HorizontalAlignment)
            _TextAlignment = v
            Invalidate()
        End Set
    End Property
    Private _MultiLine As Boolean = False
    Public Shadows Property MultiLine() As Boolean
        Get
            Return _MultiLine
        End Get
        Set(ByVal value As Boolean)
            _MultiLine = value
            txtbox.Multiline = value
            OnResize(EventArgs.Empty)
            Invalidate()
        End Set
    End Property


    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnForeColorChanged(ByVal e As System.EventArgs)
        MyBase.OnForeColorChanged(e)
        txtbox.ForeColor = ForeColor
        Invalidate()
    End Sub
    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        txtbox.Font = Font
    End Sub
    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        MyBase.OnGotFocus(e)
        txtbox.Focus()
    End Sub
    Private Sub TextChngTxtBox() Handles txtbox.TextChanged
        Text = txtbox.Text
    End Sub
    Private Sub TextChng() Handles MyBase.TextChanged
        txtbox.Text = Text
    End Sub
    Sub NewTextBox()
        With txtbox
            .Multiline = False
            .BackColor = Color.FromArgb(49, 50, 54)
            .ForeColor = ForeColor
            .Text = String.Empty
            .TextAlign = HorizontalAlignment.Center
            .Borderstyle = BorderStyle.None
            .Location = New Point(5, 4)
            .Font = New Font("Arial", 8.25F, FontStyle.Bold)
            .Size = New Size(Width - 10, Height - 11)
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

    End Sub
#End Region

    Sub New()
        MyBase.New()

        NewTextBox()
        Controls.Add(txtbox)

        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        Text = ""
        BackColor = Color.Transparent
        ForeColor = Color.White
        Font = New Font("Arial", 8.25F, FontStyle.Bold)
        Size = New Size(135, 24)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Curve As Integer = 4
        G.SmoothingMode = SmoothingMode.HighQuality

        With txtbox
            .TextAlign = TextAlign
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

        G.Clear(Color.Transparent)
        G.FillPath(New SolidBrush(Color.FromArgb(49, 50, 54)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), Curve))
        Dim GradientPen As Color() = {Color.FromArgb(43, 44, 48), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), Color.FromArgb(46, 47, 51), Color.FromArgb(47, 48, 52), Color.FromArgb(48, 49, 53)}
        For i As Integer = 0 To 5
            G.DrawPath(New Pen(GradientPen(i)), Draw.RoundRect(New Rectangle(i + 1, i + 1, Width - ((2 * i) + 3), Height - ((2 * i) + 3)), Curve))
        Next
        Dim BorderPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.Transparent, Color.FromArgb(87, 88, 92), 90S)
        G.DrawPath(New Pen(BorderPen), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), Curve))
        G.DrawPath(New Pen(Color.FromArgb(32, 33, 37)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), Curve))

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        If Not MultiLine Then
            Dim TextBoxHeight As Integer = txtbox.Height
            txtbox.Location = New Point(10, (Height / 2) - (TextBoxHeight / 2) - 1)
            txtbox.Size = New Size(Width - 20, TextBoxHeight)
        Else
            Dim TextBoxHeight As Integer = txtbox.Height
            txtbox.Location = New Point(10, 10)
            txtbox.Size = New Size(Width - 20, Height - 20)
        End If
    End Sub
End Class

Public Class RedemptionProgressBar
    Inherits Control

#Region "Properties"
    Private val As Integer
    Public Property Value() As Integer
        Get
            Return val
        End Get
        Set(ByVal _value As Integer)
            If _value > max Then
                val = max
            ElseIf _value < 0 Then
                val = 0
            Else
                val = _value
            End If
            Invalidate()
        End Set
    End Property
    Private max As Integer
    Public Property Maximum() As Integer
        Get
            Return max
        End Get
        Set(ByVal _value As Integer)
            If _value < 1 Then
                max = 1
            Else
                max = _value
            End If

            If _value < val Then
                val = max
            End If

            Invalidate()
        End Set
    End Property
#End Region
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
    End Sub
    Sub New()
        max = 100
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim curve As Integer = 6
        Dim b As New Bitmap(Width, Height)
        Dim g As Graphics = Graphics.FromImage(b)
        g.SmoothingMode = SmoothingMode.HighQuality
        g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
        Dim Fill As Integer = CInt((Width - 1) * (val / max))

        g.Clear(Color.Transparent)
        g.FillPath(New SolidBrush(Color.FromArgb(49, 50, 54)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
        Dim GradientPen As Color() = {Color.FromArgb(43, 44, 48), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), Color.FromArgb(46, 47, 51), Color.FromArgb(47, 48, 52), Color.FromArgb(48, 49, 53)}
        For i As Integer = 0 To 5
            g.DrawPath(New Pen(GradientPen(i)), Draw.RoundRect(New Rectangle(i + 1, i + 1, Width - ((2 * i) + 3), Height - ((2 * i) + 3)), curve))
        Next

        If Fill > 4 Then
            g.FillPath(New SolidBrush(Color.FromArgb(80, 164, 234)), Draw.RoundRect(New Rectangle(0, 0, Fill, Height - 2), curve))
            Dim FillTexture As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.FromArgb(100, 26, 127, 217), Color.Transparent)
            Dim Gloss As New LinearGradientBrush(New Rectangle(0, 0, Fill, Height - 2), Color.FromArgb(75, Color.White), Color.FromArgb(65, Color.Black), 90S)
            g.FillPath(Gloss, Draw.RoundRect(New Rectangle(0, 0, Fill, Height - 2), curve))
            g.FillPath(FillTexture, Draw.RoundRect(New Rectangle(0, 0, Fill, Height - 2), curve))
            Dim FillGradientBorder As New LinearGradientBrush(New Rectangle(0, 0, Fill, Height - 2), Color.FromArgb(183, 223, 249), Color.FromArgb(41, 141, 226), 90S)
            g.DrawPath(New Pen(FillGradientBorder), Draw.RoundRect(New Rectangle(1, 1, Fill - 2, Height - 4), curve))
            g.DrawPath(New Pen(Color.FromArgb(1, 44, 76)), Draw.RoundRect(New Rectangle(0, 0, Fill, Height - 2), curve))

        End If

        Dim BorderPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.Transparent, Color.FromArgb(87, 88, 92), 90S)
        g.DrawPath(New Pen(BorderPen), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), curve))
        g.DrawPath(New Pen(Color.FromArgb(32, 33, 37)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), curve))


        e.Graphics.DrawImage(b.Clone, 0, 0)
        g.Dispose() : b.Dispose()
    End Sub
End Class

Public Class RedemptionLabel : Inherits Control
    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        ForeColor = Color.White
        BackColor = Color.FromArgb(51, 56, 60)
        Font = New Font("Arial", 8.25F, FontStyle.Bold)
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        MyBase.OnPaint(e)
        G.TextRenderingHint = TextRenderingHint.AntiAlias
        G.Clear(BackColor)
        G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(16, 20, 21)), New Rectangle(1, 1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(0, 0, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})


        e.Graphics.DrawImage(B, New Point(0, 0))
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class RedemptionTabControl
    Inherits TabControl

    Enum HorizontalAlignments
        Left
        Center
        Right
    End Enum
    Private _Align As HorizontalAlignments = HorizontalAlignments.Left
    Public Property TextAlign() As HorizontalAlignments
        Get
            Return _Align
        End Get
        Set(ByVal value As HorizontalAlignments)
            _Align = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        BackColor = Color.Transparent
        ItemSize = New Size(35, 100)
        Font = New Font("Arial", 8.25F, FontStyle.Bold)
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Curve As Integer = 6
        G.TextRenderingHint = TextRenderingHint.AntiAlias
        G.SmoothingMode = SmoothingMode.HighQuality
        Try : SelectedTab.BackColor = Color.FromArgb(47, 48, 52) : Catch : End Try
        G.Clear(Color.FromArgb(51, 56, 60))
        G.FillPath(New SolidBrush(Color.FromArgb(49, 50, 54)), Draw.RoundRect(New Rectangle(ItemSize.Height - 1, 0, Width - ItemSize.Height - 1 - 1, Height - 1), Curve))
        Dim GradientPen As Color() = {Color.FromArgb(43, 44, 48), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), Color.FromArgb(46, 47, 51), Color.FromArgb(47, 48, 52), Color.FromArgb(48, 49, 53)}
        For i As Integer = 0 To 5
            G.DrawPath(New Pen(GradientPen(i)), Draw.RoundRect(New Rectangle(ItemSize.Height - 1 + i + 1, i + 1, Width - ItemSize.Height - 1 - ((2 * i) + 3), Height - ((2 * i) + 3)), Curve))
        Next
        Dim BorderPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.Transparent, Color.FromArgb(87, 88, 92), 90S)
        G.DrawPath(New Pen(BorderPen), Draw.RoundRect(New Rectangle(ItemSize.Height - 1, 0, Width - ItemSize.Height - 1 - 1, Height - 1), Curve))
        G.DrawPath(New Pen(Color.FromArgb(32, 33, 37)), Draw.RoundRect(New Rectangle(ItemSize.Height - 1, 0, Width - ItemSize.Height - 1 - 1, Height - 2), Curve))

        For i = 0 To TabCount - 1
            If i = SelectedIndex Then
                Dim OuterBorder As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 1, GetTabRect(i).Location.Y + 3), New Size(GetTabRect(i).Width - 7, GetTabRect(i).Height - 7))
                Dim InnerBorder As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 1, GetTabRect(i).Location.Y + 4), New Size(GetTabRect(i).Width - 7, GetTabRect(i).Height - 8))
                Dim MainBody As New LinearGradientBrush(OuterBorder, Color.FromArgb(72, 79, 87), Color.FromArgb(48, 51, 56), 90S)
                G.FillPath(MainBody, Draw.RoundRect(OuterBorder, Curve))
                Dim GlossPen As New LinearGradientBrush(OuterBorder, Color.FromArgb(119, 124, 130), Color.FromArgb(64, 67, 72), 90S)
                G.DrawPath(New Pen(GlossPen), Draw.RoundRect(InnerBorder, Curve))
                G.DrawPath(New Pen(Color.FromArgb(31, 36, 42)), Draw.RoundRect(OuterBorder, Curve))
            End If

            Select Case TextAlign
                Case HorizontalAlignments.Center
                    Dim TextRectangle As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 4, GetTabRect(i).Location.Y + 4), New Size(GetTabRect(i).Width - 1, GetTabRect(i).Height - 7))
                    Dim TextShadow As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 3, GetTabRect(i).Location.Y + 5), New Size(GetTabRect(i).Width - 1, GetTabRect(i).Height - 7))
                    G.DrawString(TabPages(i).Text, Font, New SolidBrush(Color.FromArgb(150, Color.Black)), TextShadow, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                    G.DrawString(TabPages(i).Text, Font, Brushes.White, TextRectangle, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                Case HorizontalAlignments.Left
                    Dim TextRectangle As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X + 3, GetTabRect(i).Location.Y + 4), New Size(GetTabRect(i).Width - 1, GetTabRect(i).Height - 7))
                    Dim TextShadow As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X + 4, GetTabRect(i).Location.Y + 5), New Size(GetTabRect(i).Width - 1, GetTabRect(i).Height - 7))
                    G.DrawString(TabPages(i).Text, Font, New SolidBrush(Color.FromArgb(150, Color.Black)), TextShadow, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                    G.DrawString(TabPages(i).Text, Font, Brushes.White, TextRectangle, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                Case HorizontalAlignments.Right
                    Dim TextRectangle As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 9, GetTabRect(i).Location.Y + 4), New Size(GetTabRect(i).Width - 1, GetTabRect(i).Height - 7))
                    Dim TextShadow As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 8, GetTabRect(i).Location.Y + 5), New Size(GetTabRect(i).Width - 1, GetTabRect(i).Height - 7))
                    G.DrawString(TabPages(i).Text, Font, New SolidBrush(Color.FromArgb(150, Color.Black)), TextShadow, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Far})
                    G.DrawString(TabPages(i).Text, Font, Brushes.White, TextRectangle, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Far})
            End Select
        Next
        e.Graphics.DrawImage(B.Clone, 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class RedemptionNumericUpDown : Inherits Control

#Region " Properties & Flicker Control "
    Private State As New MouseState
    Private X As Integer
    Private Y As Integer
    Private _Value As Long
    Private _Max As Long
    Private _Min As Long
    Private Typing As Boolean
    Public Property Value() As Long
        Get
            Return _Value
        End Get
        Set(ByVal V As Long)
            If V <= _Max And V >= _Min Then _Value = V
            Invalidate()
        End Set
    End Property
    Public Property Maximum() As Long
        Get
            Return _Max
        End Get
        Set(ByVal V As Long)
            If V > _Min Then _Max = V
            If _Value > _Max Then _Value = _Max
            Invalidate()
        End Set
    End Property
    Public Property Minimum() As Long
        Get
            Return _Min
        End Get
        Set(ByVal V As Long)
            If V < _Max Then _Min = V
            If _Value < _Min Then _Value = _Min
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Y = e.Location.Y
        Invalidate()
        If e.X < Width - 23 Then Cursor = Cursors.IBeam Else Cursor = Cursors.Default
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Me.Height = 26
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseClick(e)
        If X > Me.Width - 17 AndAlso X < Me.Width - 3 Then
            If Y < 13 Then
                If (Value + 1) <= _Max Then _Value += 1
            Else
                If (Value - 1) >= _Min Then _Value -= 1
            End If
        Else
            Typing = Not Typing
            Focus()
        End If
        Invalidate()
    End Sub
    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        MyBase.OnKeyPress(e)
        Try
            If Typing Then _Value = CStr(CStr(_Value) & e.KeyChar.ToString)
            If _Value > _Max Then _Value = _Max
        Catch ex As Exception : End Try
    End Sub
    Protected Overrides Sub OnKeyup(ByVal e As System.Windows.Forms.KeyEventArgs)
        MyBase.OnKeyUp(e)
        If e.KeyCode = Keys.Up Then
            If (Value + 1) <= _Max Then _Value += 1
            Invalidate()
        ElseIf e.KeyCode = Keys.Down Then
            If (Value - 1) >= _Min Then _Value -= 1
        ElseIf e.KeyCode = Keys.Back Then
            Dim tmp As String = _Value.ToString()
            tmp = tmp.Remove(Convert.ToInt32(tmp.Length - 1))
            If (tmp.Length = 0) Then tmp = "0"
            _Value = Convert.ToInt32(tmp)
        End If
        Invalidate()
    End Sub
    Protected Sub DrawTriangle(ByVal Clr As Color, ByVal FirstPoint As Point, ByVal SecondPoint As Point, ByVal ThirdPoint As Point, ByVal G As Graphics)
        Dim points As New List(Of Point)()
        points.Add(FirstPoint)
        points.Add(SecondPoint)
        points.Add(ThirdPoint)
        G.FillPolygon(New SolidBrush(Clr), points.ToArray)
    End Sub
#End Region
    Sub New()
        _Max = 9999999
        _Min = 0
        Cursor = Cursors.IBeam
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.White
        DoubleBuffered = True
        Font = New Font("Arial", 8.25F, FontStyle.Bold)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Curve As Integer = 4
        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.AntiAlias
        G.FillPath(New SolidBrush(Color.FromArgb(49, 50, 54)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), Curve))
        Dim GradientPen As Color() = {Color.FromArgb(43, 44, 48), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), Color.FromArgb(46, 47, 51), Color.FromArgb(47, 48, 52), Color.FromArgb(48, 49, 53)}
        For i As Integer = 0 To 5
            G.DrawPath(New Pen(GradientPen(i)), Draw.RoundRect(New Rectangle(i + 1, i + 1, Width - ((2 * i) + 3), Height - ((2 * i) + 3)), Curve))
        Next
        G.SetClip(Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), Curve))
        Dim ButtonBackground As New LinearGradientBrush(New Rectangle(Width - 17, 0, 17, Height - 2), Color.FromArgb(75, 78, 87), Color.FromArgb(50, 51, 55), 90S)
        G.FillRectangle(ButtonBackground, ButtonBackground.Rectangle)
        G.ResetClip()
        Dim BorderPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.Transparent, Color.FromArgb(87, 88, 92), 90S)
        G.DrawPath(New Pen(BorderPen), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), Curve))
        G.DrawPath(New Pen(Color.FromArgb(32, 33, 37)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), Curve))
        DrawTriangle(Color.FromArgb(22, 23, 28), New Point(Width - 12, 8), New Point(Width - 6, 8), New Point(Width - 9, 5), G)
        DrawTriangle(Color.FromArgb(22, 23, 28), New Point(Width - 12, 17), New Point(Width - 6, 17), New Point(Width - 9, 20), G)
        G.SetClip(Draw.RoundRect(New Rectangle(Width - 17, 0, 17, Height - 2), Curve))
        G.DrawPath(New Pen(Color.FromArgb(82, 85, 92)), Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 4), Curve))
        G.ResetClip()
        G.DrawLine(New Pen(Color.FromArgb(29, 37, 40)), New Point(Width - 17, 0), New Point(Width - 17, Height - 2))
        G.DrawLine(New Pen(Color.FromArgb(85, 92, 98)), New Point(Width - 16, 1), New Point(Width - 16, Height - 3))
        G.DrawLine(New Pen(Color.FromArgb(29, 37, 40)), New Point(Width - 17, Height / 2 - 1), New Point(Width - 1, Height / 2 - 1))
        G.DrawLine(New Pen(Color.FromArgb(85, 92, 98)), New Point(Width - 16, Height / 2), New Point(Width - 2, Height / 2))

        G.DrawString(Value, Font, New SolidBrush(Color.FromArgb(16, 20, 21)), New Point(8, 8))
        G.DrawString(Value, Font, Brushes.White, New Point(7, 7))
        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class RedemptionComboBox : Inherits ComboBox
#Region " Control Help - Properties & Flicker Control "
    Private _StartIndex As Integer = 0
    Private Property StartIndex() As Integer
        Get
            Return _StartIndex
        End Get
        Set(ByVal value As Integer)
            _StartIndex = value
            Try
                MyBase.SelectedIndex = value
            Catch
            End Try
            Invalidate()
        End Set
    End Property
    Sub ReplaceItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()
        Try
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(59, 60, 64)), e.Bounds)
            Else
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(49, 50, 54)), e.Bounds)
            End If
            Using b As New SolidBrush(e.ForeColor)
                e.Graphics.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, b, New Rectangle(e.Bounds.X, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height))
            End Using
        Catch
        End Try
    End Sub
    Protected Sub DrawTriangle(ByVal Clr As Color, ByVal FirstPoint As Point, ByVal SecondPoint As Point, ByVal ThirdPoint As Point, ByVal G As Graphics)
        Dim points As New List(Of Point)()
        points.Add(FirstPoint)
        points.Add(SecondPoint)
        points.Add(ThirdPoint)
        G.FillPolygon(New SolidBrush(Clr), points.ToArray)
    End Sub

#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.DoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(182, 179, 171)
        DropDownstyle = ComboBoxStyle.DropDownList
        StartIndex = 0
        ItemHeight = 18
        DoubleBuffered = True
        Font = New Font("Arial", 8.25F, FontStyle.Bold)
    End Sub
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 26
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Curve As Integer = 4
        G.Clear(BackColor)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = TextRenderingHint.AntiAlias
        G.FillPath(New SolidBrush(Color.FromArgb(49, 50, 54)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), Curve))

        Dim BodyGradient As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(57, 62, 68), Color.FromArgb(42, 43, 47), 90S)
        G.FillPath(BodyGradient, Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), Curve))


        G.SetClip(Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), Curve))
        Dim ButtonBackground As New LinearGradientBrush(New Rectangle(Width - 17, 0, 17, Height - 2), Color.FromArgb(75, 78, 87), Color.FromArgb(50, 51, 55), 90S)
        G.FillRectangle(ButtonBackground, ButtonBackground.Rectangle)
        G.ResetClip()


        Dim BorderPen As New LinearGradientBrush(New Rectangle(0, 1, Width - 1, Height - 2), Color.FromArgb(92, 97, 103), Color.Transparent, 90S)
        G.DrawPath(New Pen(BorderPen), Draw.RoundRect(New Rectangle(0, 1, Width - 1, Height - 2), Curve))
        G.DrawPath(New Pen(Color.FromArgb(32, 33, 37)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), Curve))


        DrawTriangle(Color.FromArgb(22, 23, 28), New Point(Width - 12, 8), New Point(Width - 6, 8), New Point(Width - 9, 5), G)
        DrawTriangle(Color.FromArgb(22, 23, 28), New Point(Width - 12, 14), New Point(Width - 6, 14), New Point(Width - 9, 17), G)
        G.SetClip(Draw.RoundRect(New Rectangle(Width - 17, 0, 17, Height), Curve))
        Dim ButtonPen As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 3), Color.FromArgb(82, 85, 92), Color.FromArgb(66, 67, 72), 90S)
        G.DrawPath(New Pen(ButtonPen), Draw.RoundRect(New Rectangle(1, 1, Width - 3, Height - 3), Curve))
        G.ResetClip()
        G.DrawLine(New Pen(Color.FromArgb(29, 37, 40)), New Point(Width - 17, 0), New Point(Width - 17, Height - 2))
        G.DrawLine(New Pen(Color.FromArgb(85, 92, 98)), New Point(Width - 16, 1), New Point(Width - 16, Height - 3))

        Try
            G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(16, 20, 21)), New Rectangle(7, 0, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
            G.DrawString(Text, Font, New SolidBrush(Color.White), New Rectangle(7, 1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        Catch
        End Try

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Public Class RedemptionCheckBox : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Width = CreateGraphics().MeasureString(Text, Font).Width + (2 * 3) + Height
        Invalidate()
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
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 19
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        BackColor = Color.Transparent
        ForeColor = Color.White
        Size = New Size(147, 17)
        DoubleBuffered = True
        Font = New Font("Arial", 8.25F, FontStyle.Bold)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        Dim CheckBoxRectangle As New Rectangle(0, 0, Height - 1, Height - 1)
        Dim Curve As Integer = 1

        G.Clear(BackColor)

        G.Clear(Color.Transparent)
        G.FillPath(New SolidBrush(Color.FromArgb(49, 50, 54)), Draw.RoundRect(New Rectangle(0, 0, Height - 1, Height - 1), Curve))
        Dim GradientPen As Color() = {Color.FromArgb(43, 44, 48), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), Color.FromArgb(46, 47, 51), Color.FromArgb(47, 48, 52), Color.FromArgb(48, 49, 53)}
        For i As Integer = 0 To 5
            G.DrawPath(New Pen(GradientPen(i)), Draw.RoundRect(New Rectangle(i + 1, i + 1, Height - ((2 * i) + 3), Height - ((2 * i) + 3)), Curve))
        Next
        Dim BorderPen As New LinearGradientBrush(New Rectangle(0, 0, Height - 1, Height - 1), Color.Transparent, Color.FromArgb(87, 88, 92), 90S)
        G.DrawPath(New Pen(BorderPen), Draw.RoundRect(New Rectangle(0, 0, Height - 2, Height - 1), Curve))
        G.DrawPath(New Pen(Color.FromArgb(32, 33, 37)), Draw.RoundRect(New Rectangle(0, 0, Height - 2, Height - 2), Curve))


        If Checked Then
            Dim chkPoly As Rectangle = New Rectangle(CheckBoxRectangle.X + CheckBoxRectangle.Width / 4, CheckBoxRectangle.Y + CheckBoxRectangle.Height / 4, CheckBoxRectangle.Width \ 2, CheckBoxRectangle.Height \ 2)
            Dim Poly() As Point = {New Point(chkPoly.X + 1, chkPoly.Y + chkPoly.Height \ 2), New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height - 1), New Point(chkPoly.X + chkPoly.Width, chkPoly.Y)}
            For i = 0 To Poly.Length - 2 : G.DrawLine(New Pen(Color.White, 2), Poly(i), Poly(i + 1)) : Next
        End If

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(21, 3), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Public Class RedemptionRadioButton : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private R1 As Rectangle
    Private G1 As LinearGradientBrush

    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 19
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Width = CreateGraphics().MeasureString(Text, Font).Width + (2 * 3) + Height
        Invalidate()
    End Sub
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
    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        InvalidateControls()
    End Sub
    Private Sub InvalidateControls()
        Try
            If Not IsHandleCreated OrElse Not Checked Then Return
            For Each C As Control In Parent.Controls
                If C IsNot Me AndAlso TypeOf C Is RedemptionRadioButton Then
                    DirectCast(C, RedemptionRadioButton).Checked = False
                End If
            Next
        Catch : End Try
    End Sub
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.White
        DoubleBuffered = True
        Size = New Size(177, 17)
        Font = New Font("Arial", 8.25F, FontStyle.Bold)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim RadioBtnRectangle = New Rectangle(0, 0, Height - 1, Height - 1)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        G.Clear(BackColor)

        G.Clear(Color.Transparent)
        G.FillEllipse(New SolidBrush(Color.FromArgb(49, 50, 54)), New Rectangle(0, 0, Height - 1, Height - 1))
        Dim GradientPen As Color() = {Color.FromArgb(43, 44, 48), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), Color.FromArgb(46, 47, 51), Color.FromArgb(47, 48, 52), Color.FromArgb(48, 49, 53)}
        For i As Integer = 0 To 5
            G.DrawEllipse(New Pen(GradientPen(i)), New Rectangle(i + 1, i + 1, Height - ((2 * i) + 3), Height - ((2 * i) + 3)))
        Next
        Dim BorderPen As New LinearGradientBrush(New Rectangle(0, 0, Height - 1, Height - 1), Color.Transparent, Color.FromArgb(87, 88, 92), 90S)
        G.DrawEllipse(New Pen(BorderPen), New Rectangle(0, 0, Height - 2, Height - 1))
        G.DrawEllipse(New Pen(Color.FromArgb(32, 33, 37)), New Rectangle(0, 0, Height - 2, Height - 2))



        If Checked Then
            G.FillEllipse(New SolidBrush(Color.White), New Rectangle(5, 5, Height - 12, Height - 12))
        End If

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(21, 3), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class

<DefaultEvent("CheckedChanged")> Public Class RedemptionToggle : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
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
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Size = New Size(60, 26)
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Size = New Size(60, 26)
        BackColor = Color.Transparent
        ForeColor = Color.White
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        Dim Curve As Integer = 4

        G.Clear(BackColor)

        G.Clear(Color.Transparent)
        G.FillPath(New SolidBrush(Color.FromArgb(49, 50, 54)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), Curve))
        Dim GradientPen As Color() = {Color.FromArgb(43, 44, 48), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), Color.FromArgb(46, 47, 51), Color.FromArgb(47, 48, 52), Color.FromArgb(48, 49, 53)}
        For i As Integer = 0 To 5
            G.DrawPath(New Pen(GradientPen(i)), Draw.RoundRect(New Rectangle(i + 1, i + 1, Width - ((2 * i) + 3), Height - ((2 * i) + 3)), Curve))
        Next
        Dim BorderPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.Transparent, Color.FromArgb(87, 88, 92), 90S)
        G.DrawPath(New Pen(BorderPen), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), Curve))
        G.DrawPath(New Pen(Color.FromArgb(32, 33, 37)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 3), Curve))

        Select Case Checked
            Case False
                Dim CheckedBody As New LinearGradientBrush(New Rectangle(0, 0, 30, Height - 3), Color.FromArgb(72, 79, 87), Color.FromArgb(48, 52, 55), 90S)
                G.FillPath(CheckedBody, Draw.RoundRect(New Rectangle(0, 0, 30, Height - 3), Curve))
                Dim CheckedBorderPen As New LinearGradientBrush(New Rectangle(0, 0, 30, Height - 3), Color.FromArgb(29, 34, 40), Color.FromArgb(33, 34, 38), 90S)
                G.DrawPath(New Pen(CheckedBorderPen), Draw.RoundRect(New Rectangle(0, 0, 30, Height - 3), Curve))
                Dim CheckedBorderHighlight As New LinearGradientBrush(New Rectangle(1, 1, 28, Height - 5), Color.FromArgb(118, 123, 129), Color.FromArgb(66, 67, 71), 90S)
                G.DrawPath(New Pen(CheckedBorderHighlight), Draw.RoundRect(New Rectangle(1, 1, 28, Height - 5), Curve))
                For i As Integer = 0 To 2
                    G.DrawLine(New Pen(Color.FromArgb(82, 86, 95)), New Point(7, 7 + (i * 4)), New Point(22, 7 + (i * 4)))
                    G.DrawLine(New Pen(Color.FromArgb(47, 50, 57)), New Point(7, 7 + (i * 4) + 1), New Point(22, 7 + (i * 4) + 1))
                Next
            Case True
                Dim CheckedBody As New LinearGradientBrush(New Rectangle(29, 0, 30, Height - 3), Color.FromArgb(145, 204, 238), Color.FromArgb(35, 137, 222), 90S)
                G.FillPath(CheckedBody, Draw.RoundRect(New Rectangle(29, 0, 30, Height - 3), Curve))
                Dim CheckedBorderPen As New LinearGradientBrush(New Rectangle(29, 0, 30, Height - 3), Color.FromArgb(21, 37, 52), Color.FromArgb(18, 37, 54), 90S)
                G.DrawPath(New Pen(CheckedBorderPen), Draw.RoundRect(New Rectangle(29, 0, 30, Height - 3), Curve))
                Dim CheckedBorderHighlight As New LinearGradientBrush(New Rectangle(30, 1, 28, Height - 5), Color.FromArgb(169, 228, 255), Color.FromArgb(53, 155, 240), 90S)
                G.DrawPath(New Pen(CheckedBorderHighlight), Draw.RoundRect(New Rectangle(30, 1, 28, Height - 5), Curve))
                For i As Integer = 0 To 2
                    G.DrawLine(New Pen(Color.FromArgb(109, 188, 244)), New Point(36, 7 + (i * 4)), New Point(51, 7 + (i * 4)))
                    G.DrawLine(New Pen(Color.FromArgb(40, 123, 199)), New Point(36, 7 + (i * 4) + 1), New Point(51, 7 + (i * 4) + 1))
                Next
        End Select
        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Public Class RedemptionRoundedToggle : Inherits Control
#Region " Control Help - MouseState & Flicker Control"
    Private State As MouseState = MouseState.None
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
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
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Size = New Size(34, 21)
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        Size = New Size(34, 21)
        BackColor = Color.Transparent
        ForeColor = Color.White
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        Dim CheckBoxRectangle As New Rectangle(0, 0, Height - 1, Height - 1)
        Dim Curve As Integer = 9

        G.Clear(BackColor)

        G.Clear(Color.Transparent)
        G.FillPath(New SolidBrush(Color.FromArgb(49, 50, 54)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), Curve))
        Dim GradientPen As Color() = {Color.FromArgb(43, 44, 48), Color.FromArgb(44, 45, 49), Color.FromArgb(45, 46, 50), Color.FromArgb(46, 47, 51), Color.FromArgb(47, 48, 52), Color.FromArgb(48, 49, 53)}
        For i As Integer = 0 To 5
            G.DrawPath(New Pen(GradientPen(i)), Draw.RoundRect(New Rectangle(i + 1, i + 1, Width - ((2 * i) + 3), Height - ((2 * i) + 3)), Curve))
        Next
        Dim BorderPen As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.Transparent, Color.FromArgb(87, 88, 92), 90S)
        G.DrawPath(New Pen(BorderPen), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 2), Curve))
        G.DrawPath(New Pen(Color.FromArgb(32, 33, 37)), Draw.RoundRect(New Rectangle(0, 0, Width - 1, Height - 3), Curve))
        G.FillRectangle(New SolidBrush(Parent.BackColor), New Rectangle(-1, 0, 2, 7))
        Select Case Checked
            Case False
                Dim CheckedBody As New LinearGradientBrush(New Rectangle(0, 0, Height - 3, Height - 3), Color.FromArgb(72, 79, 87), Color.FromArgb(48, 52, 55), 90S)
                G.FillEllipse(CheckedBody, New Rectangle(0, 0, Height - 3, Height - 3))
                Dim CheckedBorderPen As New LinearGradientBrush(New Rectangle(0, 0, Height - 3, Height - 3), Color.FromArgb(29, 34, 40), Color.FromArgb(33, 34, 38), 90S)
                G.DrawEllipse(New Pen(CheckedBorderPen), New Rectangle(0, 0, Height - 3, Height - 3))
                Dim CheckedBorderHighlight As New LinearGradientBrush(New Rectangle(1, 1, Height - 5, Height - 4), Color.FromArgb(118, 123, 129), Color.FromArgb(66, 67, 71), 90S)
                G.DrawEllipse(New Pen(CheckedBorderHighlight), New Rectangle(1, 1, Height - 5, Height - 5))
            Case True
                Dim CheckedBody As New LinearGradientBrush(New Rectangle(15, 0, Height - 3, Height - 3), Color.FromArgb(138, 211, 254), Color.FromArgb(56, 157, 229), 90S)
                G.FillEllipse(CheckedBody, New Rectangle(15, 0, Height - 3, Height - 3))
                Dim CheckedBorderPen As New LinearGradientBrush(New Rectangle(15, 0, Height - 3, Height - 3), Color.FromArgb(7, 39, 64), Color.FromArgb(26, 35, 42), 90S)
                G.DrawEllipse(New Pen(CheckedBorderPen), New Rectangle(15, 0, Height - 3, Height - 3))
                Dim CheckedBorderHighlight As New LinearGradientBrush(New Rectangle(16, 1, Height - 5, Height - 4), Color.FromArgb(176, 206, 230), Color.FromArgb(30, 107, 175), 90S)
                G.DrawEllipse(New Pen(CheckedBorderHighlight), New Rectangle(16, 1, Height - 5, Height - 5))
        End Select
        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub
End Class