' All credits goes to Aeonhack
' All credits goes to HawkHF
' All credits goes to Leumonic
' All credits goes to Ashlanfox

' Edited by Huracan
' Hurrycan theme

Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Drawing.Text
Imports Hurrycan_theme.HurrycanButton



Module HurrycanModule

#Region " G"
    Friend G As Graphics, B As Bitmap
#End Region


    Sub New()
        TextBitmap = New Bitmap(1, 1)
        TextGraphics = Graphics.FromImage(TextBitmap)
    End Sub

    Private TextBitmap As Bitmap
    Private TextGraphics As Graphics

    Friend Function MeasureString(text As String, font As Font) As SizeF
        Return TextGraphics.MeasureString(text, font)
    End Function

    Friend Function MeasureString(text As String, font As Font, width As Integer) As SizeF
        Return TextGraphics.MeasureString(text, font, width, StringFormat.GenericTypographic)
    End Function

    Private CreateRoundPath As GraphicsPath
    Private CreateRoundRectangle As Rectangle

    Friend Function CreateRound(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal slope As Integer) As GraphicsPath
        CreateRoundRectangle = New Rectangle(x, y, width, height)
        Return CreateRound(CreateRoundRectangle, slope)
    End Function

    Friend Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

End Module

Class HurrycanForm
    Inherits ThemeContainer154

#Region "Variables | Colors"
    Private BGC As Color = Color.FromArgb(219, 219, 219) 'BACKGROUND COLOR
    Private LC As Color = Color.FromArgb(32, 175, 206) 'LINE COLOR
#End Region

    Sub New()
        BackColor = Color.FromArgb(255, 255, 255)
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BGC)
        Dim Font As New Font("Verdana", 10)
        'Close
        G.DrawString(FindForm.Text, Font, New SolidBrush(Color.FromArgb(130, 130, 130)), New Point(15, 13))
    End Sub
End Class

Public Class HurrycanControlBox : Inherits Control
    Enum ColorSchemes
        Dark
    End Enum
    Event ColorSchemeChanged()
    Private _ColorScheme As ColorSchemes
    Public Property ColorScheme() As ColorSchemes
        Get
            Return _ColorScheme
        End Get
        Set(ByVal value As ColorSchemes)
            _ColorScheme = value
            RaiseEvent ColorSchemeChanged()
        End Set
    End Property
    Protected Sub OnColorSchemeChanged() Handles Me.ColorSchemeChanged
        Invalidate()
        Select Case ColorScheme
            Case ColorSchemes.Dark
                BackColor = Color.FromArgb(219, 219, 219)
                ForeColor = Color.White
                AccentColor = Color.FromArgb(225, 225, 225)
        End Select
    End Sub
    Private _AccentColor As Color
    Public Property AccentColor() As Color
        Get
            Return _AccentColor
        End Get
        Set(ByVal value As Color)
            _AccentColor = value
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        DoubleBuffered = True
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        ForeColor = Color.FromArgb(50, 50, 50)
        BackColor = Color.FromArgb(50, 50, 50)
        AccentColor = Color.DodgerBlue
        ColorScheme = ColorSchemes.Dark
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
    End Sub
    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(100, 25)
    End Sub
    Enum ButtonHover
        Minimize
        Maximize
        Close
        None
    End Enum
    Dim ButtonState As ButtonHover = ButtonHover.None
    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        Dim X As Integer = e.Location.X
        Dim Y As Integer = e.Location.Y
        If Y > 0 AndAlso Y < (Height - 2) Then
            If X > 0 AndAlso X < 34 Then
                ButtonState = ButtonHover.Minimize
            ElseIf X > 33 AndAlso X < 65 Then
                ButtonState = ButtonHover.Maximize
            ElseIf X > 64 AndAlso X < Width Then
                ButtonState = ButtonHover.Close
            Else
                ButtonState = ButtonHover.None
            End If
        Else
            ButtonState = ButtonHover.None
        End If
        Invalidate()
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        MyBase.OnPaint(e)

        G.Clear(BackColor)
        Select Case ButtonState
            Case ButtonHover.None
                G.Clear(BackColor)
            Case ButtonHover.Minimize
                G.FillRectangle(New SolidBrush(_AccentColor), New Rectangle(3, 0, 30, Height))
            Case ButtonHover.Maximize
                G.FillRectangle(New SolidBrush(_AccentColor), New Rectangle(34, 0, 30, Height))
            Case ButtonHover.Close
                G.FillRectangle(New SolidBrush(_AccentColor), New Rectangle(65, 0, 35, Height))
        End Select

        Dim ButtonFont As New Font("Marlett", 9.75F)
        'Close
        G.DrawString("r", ButtonFont, New SolidBrush(Color.FromArgb(150, 150, 150)), New Point(Width - 16, 7), New StringFormat With {.Alignment = StringAlignment.Center})
        'Maximize
        Select Case Parent.FindForm().WindowState
            Case FormWindowState.Maximized
                G.DrawString("2", ButtonFont, New SolidBrush(Color.FromArgb(150, 150, 150)), New Point(51, 7), New StringFormat With {.Alignment = StringAlignment.Center})
            Case FormWindowState.Normal
                G.DrawString("1", ButtonFont, New SolidBrush(Color.FromArgb(150, 150, 150)), New Point(51, 7), New StringFormat With {.Alignment = StringAlignment.Center})
        End Select
        'Minimize
        G.DrawString("0", ButtonFont, New SolidBrush(Color.FromArgb(150, 150, 150)), New Point(20, 7), New StringFormat With {.Alignment = StringAlignment.Center})


        e.Graphics.DrawImage(B, New Point(0, 0))
        G.Dispose() : B.Dispose()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        Select Case ButtonState
            Case ButtonHover.Close
                Parent.FindForm().Close()
            Case ButtonHover.Minimize
                Parent.FindForm().WindowState = FormWindowState.Minimized
            Case ButtonHover.Maximize
                If Parent.FindForm().WindowState = FormWindowState.Normal Then
                    Parent.FindForm().WindowState = FormWindowState.Maximized
                Else
                    Parent.FindForm().WindowState = FormWindowState.Normal
                End If

        End Select
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        ButtonState = ButtonHover.None : Invalidate()
    End Sub
End Class

Class HurrycanButton
    Inherits ThemeControl154

#Region "Variables | Colors"
    Private WL As Color = Color.FromArgb(55, Color.White) 'WHITE LINE
    Private SC As Color = Color.FromArgb(31, 147, 174) 'SURROUND COLOR
    Private TC As Color = Color.White 'TEXT COLOR
#End Region

#Region "Themebase | DrawText functions"
    Private DrawTextPoint As Point
    Private DrawTextSize As Size

    Protected Sub DrawSomeText(ByVal b1 As Brush, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer, Optional ByVal Upper As Boolean = False)
        DrawSomeText(b1, Text, a, x, y, Upper)
    End Sub


    Protected Sub DrawSomeText(ByVal b1 As Brush, ByVal text As String, ByVal a As HorizontalAlignment, ByVal x As Integer, ByVal y As Integer, Optional ByVal Upper As Boolean = False)
        If text.Length = 0 Then Return
        If Upper = True Then text = text.ToUpper

        DrawTextSize = Measure(text)
        DrawTextPoint = Center(DrawTextSize)

        Select Case a
            Case HorizontalAlignment.Left
                G.DrawString(text, Font, b1, x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Center
                G.DrawString(text, Font, b1, DrawTextPoint.X + x, DrawTextPoint.Y + y)
            Case HorizontalAlignment.Right
                G.DrawString(text, Font, b1, Width - DrawTextSize.Width - x, DrawTextPoint.Y + y)
        End Select
    End Sub

    Protected Sub DrawSomeText(ByVal b1 As Brush, ByVal p1 As Point)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, p1)
    End Sub
    Protected Sub DrawSomeText(ByVal b1 As Brush, ByVal x As Integer, ByVal y As Integer)
        If Text.Length = 0 Then Return
        G.DrawString(Text, Font, b1, x, y)
    End Sub

#End Region

    Sub New()
        Font = New Font("Segoe UI", 9)
    End Sub

    Protected Overrides Sub ColorHook()

    End Sub

    Protected Overrides Sub PaintHook()
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Color.FromArgb(219, 219, 219))

        Select Case State
            Case MouseState.None
                Dim BR As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(37, 191, 208), Color.FromArgb(29, 174, 198), 90.0F)
                Dim GP As GraphicsPath = CreateRound(0, 1, Width - 2, Height - 2, 5)

                G.FillPath(BR, GP)
                G.DrawPath(New Pen(SC), GP)
                G.DrawLine(New Pen(WL, 2), New Point(2, 2), New Point(Width - 2, 2))

            Case MouseState.Over
                Dim BR As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(69, 200, 214), Color.FromArgb(63, 186, 206), 90.0F)
                Dim GP As GraphicsPath = CreateRound(0, 1, Width - 2, Height - 2, 5)

                G.FillPath(BR, GP)
                G.DrawPath(New Pen(SC), GP)
                G.DrawLine(New Pen(WL, 2), New Point(2, 2), New Point(Width - 2, 2))

            Case MouseState.Down
                Dim BR As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(36, 190, 214), Color.FromArgb(29, 174, 198), 90.0F)
                Dim GP As GraphicsPath = CreateRound(0, 1, Width - 2, Height - 2, 5)

                G.FillPath(BR, GP)
                G.DrawPath(New Pen(SC), GP)
                G.DrawLine(New Pen(WL, 2), New Point(2, 2), New Point(Width - 2, 2))

        End Select

        DrawSomeText(New SolidBrush(TC), HorizontalAlignment.Center, 0, 0, False)
    End Sub

End Class

Class HurrycanGroupBox
    Inherits ContainerControl

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        BackColor = Color.FromArgb(225, 225, 225)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim G As Graphics = e.Graphics

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        G.FillRectangle(New SolidBrush(Color.FromArgb(225, 225, 225)), mainRect)
        G.DrawRectangle(New Pen(Color.FromArgb(170, 170, 170)), mainRect)

    End Sub

End Class

Public Class HurrycanCheckBox
    Inherits Control

#Region "Declarations"
    Private _Checked As Boolean
    Private State As MouseState = MouseState.None
    Private _CheckedColour As Color = Color.FromArgb(63, 189, 220)
    Private _BorderColour As Color = Color.FromArgb(170, 170, 170)
    Private _BackColour As Color = Color.FromArgb(219, 219, 219)
    Private _TextColour As Color = Color.FromArgb(140, 140, 140)
#End Region

#Region "Colour & Other Properties"

    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BackColour
        End Get
        Set(value As Color)
            _BackColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property CheckedColour As Color
        Get
            Return _CheckedColour
        End Get
        Set(value As Color)
            _CheckedColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property FontColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property

    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 22
    End Sub
#End Region

#Region "Mouse States"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        Size = New Size(100, 22)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Dim Base As New Rectangle(0, 0, 20, 20)
        With g
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(Color.FromArgb(219, 219, 219))
            .FillRectangle(New SolidBrush(_BackColour), Base)
            .DrawRectangle(New Pen(_BorderColour), New Rectangle(1, 1, 18, 18))
            Select Case State
                Case MouseState.Over
                    .FillRectangle(New SolidBrush(Color.FromArgb(225, 225, 225)), Base)
                    .DrawRectangle(New Pen(_BorderColour), New Rectangle(1, 1, 18, 18))
            End Select
            If Checked Then
                Dim P() As Point = {New Point(4, 11), New Point(6, 8), New Point(9, 12), New Point(15, 3), New Point(17, 6), New Point(9, 16)}
                .FillPolygon(New SolidBrush(_CheckedColour), P)
            End If
            .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(24, 1, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
            .InterpolationMode = CType(7, InterpolationMode)
        End With
    End Sub
#End Region

End Class

<System.ComponentModel.DefaultEvent("TextChanged")> _
Public Class HurrycanTextBox
    Inherits Control

#Region "Declarations"
    Private State As MouseState = MouseState.None
    Private WithEvents TB As Windows.Forms.TextBox
    Private _BaseColour As Color = Color.FromArgb(219, 219, 219)
    Private _TextColour As Color = Color.FromArgb(140, 140, 140)
    Private _BorderColour As Color = Color.FromArgb(200, 200, 200)
    Private _Style As Styles = Styles.Normal
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean
    Private _UseSystemPasswordChar As Boolean
    Private _Multiline As Boolean
#End Region

#Region "TextBox Properties"

    Enum Styles
        Normal
    End Enum

    <Category("Options")>
    Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If TB IsNot Nothing Then
                TB.TextAlign = value
            End If
        End Set
    End Property

    <Category("Options")>
    Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If TB IsNot Nothing Then
                TB.MaxLength = value
            End If
        End Set
    End Property

    <Category("Options")>
    Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If TB IsNot Nothing Then
                TB.ReadOnly = value
            End If
        End Set
    End Property

    <Category("Options")>
    Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If TB IsNot Nothing Then
                TB.UseSystemPasswordChar = value
            End If
        End Set
    End Property

    <Category("Options")>
    Property Multiline() As Boolean
        Get
            Return _Multiline
        End Get
        Set(ByVal value As Boolean)
            _Multiline = value
            If TB IsNot Nothing Then
                TB.Multiline = value

                If value Then
                    TB.Height = Height - 11
                Else
                    Height = TB.Height + 11
                End If

            End If
        End Set
    End Property

    <Category("Options")>
    Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If TB IsNot Nothing Then
                TB.Text = value
            End If
        End Set
    End Property

    <Category("Options")>
    Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If TB IsNot Nothing Then
                TB.Font = value
                TB.Location = New Point(3, 5)
                TB.Width = Width - 6

                If Not _Multiline Then
                    Height = TB.Height + 11
                End If
            End If
        End Set
    End Property

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(TB) Then
            Controls.Add(TB)
        End If
    End Sub

    Private Sub OnBaseTextChanged(ByVal s As Object, ByVal e As EventArgs)
        Text = TB.Text
    End Sub

    Private Sub OnBaseKeyDown(ByVal s As Object, ByVal e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            TB.SelectAll()
            e.SuppressKeyPress = True
        End If
        If e.Control AndAlso e.KeyCode = Keys.C Then
            TB.Copy()
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        TB.Location = New Point(5, 5)
        TB.Width = Width - 10

        If _Multiline Then
            TB.Height = Height - 11
        Else
            Height = TB.Height + 11
        End If

        MyBase.OnResize(e)
    End Sub

    Public Property Style As Styles
        Get
            Return _Style
        End Get
        Set(value As Styles)
            _Style = value
        End Set
    End Property

    Public Sub SelectAll()
        TB.Focus()
        TB.SelectAll()
    End Sub


#End Region

#Region "Colour Properties"

    <Category("Colours")>
    Public Property BackgroundColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property

#End Region

#Region "Mouse States"

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : TB.Focus() : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        TB = New Windows.Forms.TextBox
        TB.Height = 190
        TB.Font = New Font("Segoe UI", 10)
        TB.Text = Text
        TB.BackColor = Color.FromArgb(219, 219, 219)
        TB.ForeColor = Color.FromArgb(140, 140, 140)
        TB.MaxLength = _MaxLength
        TB.Multiline = False
        TB.ReadOnly = _ReadOnly
        TB.UseSystemPasswordChar = _UseSystemPasswordChar
        TB.BorderStyle = BorderStyle.None
        TB.Location = New Point(5, 5)
        TB.Width = Width - 35
        AddHandler TB.TextChanged, AddressOf OnBaseTextChanged
        AddHandler TB.KeyDown, AddressOf OnBaseKeyDown
    End Sub


    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        Dim Base As New Rectangle(0, 0, Width, Height)
        With g
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            TB.BackColor = Color.FromArgb(219, 219, 219)
            TB.ForeColor = Color.FromArgb(140, 140, 140)
            Select Case _Style
                Case Styles.Normal
                    .FillRectangle(New SolidBrush(Color.FromArgb(219, 219, 219)), New Rectangle(0, 0, Width - 1, Height - 1))
                    .DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(170, 170, 170)), 2), New Rectangle(0, 0, Width, Height))
            End Select
            .InterpolationMode = CType(7, InterpolationMode)
        End With
    End Sub

#End Region

End Class

Public Class HurrycanComboBox : Inherits ComboBox
#Region " Control Help - Properties & Flicker Control "
    Enum ColorSchemes
        Light
        Dark
    End Enum
    Private _ColorScheme As ColorSchemes
    Public Property ColorScheme() As ColorSchemes
        Get
            Return _ColorScheme
        End Get
        Set(ByVal value As ColorSchemes)
            _ColorScheme = value
            Invalidate()
        End Set
    End Property

    Private _AccentColor As Color
    Public Property AccentColor() As Color
        Get
            Return _AccentColor
        End Get
        Set(ByVal value As Color)
            _AccentColor = value
            Invalidate()
        End Set
    End Property

    Private _StartIndex As Integer = 0
    Private Property StartIndex As Integer
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
                e.Graphics.FillRectangle(New SolidBrush(_AccentColor), e.Bounds)
            Else
                Select Case ColorScheme
                    Case ColorSchemes.Dark
                        e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(225, 225, 225)), e.Bounds)
                    Case ColorSchemes.Light
                        e.Graphics.FillRectangle(New SolidBrush(Color.White), e.Bounds)
                End Select
            End If
            Select Case ColorScheme
                Case ColorSchemes.Dark
                    e.Graphics.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, Brushes.Gray, e.Bounds)
                Case ColorSchemes.Light
                    e.Graphics.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, Brushes.Gray, e.Bounds)
            End Select
        Catch
        End Try
    End Sub
    Protected Sub DrawTriangle(ByVal Clr As Color, ByVal FirstPoint As Point, ByVal SecondPoint As Point, ByVal ThirdPoint As Point, ByVal G As Graphics)
        Dim points As New List(Of Point)()
        points.Add(FirstPoint)
        points.Add(SecondPoint)
        points.Add(ThirdPoint)
        G.FillPolygon(New SolidBrush(Clr), points.ToArray())
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
        BackColor = Color.FromArgb(219, 219, 219)
        Size = New Size(Width, 30)
        ForeColor = Color.FromArgb(130, 130, 130)
        AccentColor = Color.FromArgb(63, 189, 220)
        ColorScheme = ColorSchemes.Dark
        DropDownStyle = ComboBoxStyle.DropDownList
        Font = New Font("Verdana", 8)
        StartIndex = 0
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        Size = New Size(Width, 30)
        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Curve As Integer = 2


        G.SmoothingMode = SmoothingMode.HighQuality


        Select Case ColorScheme
            Case ColorSchemes.Dark
                G.Clear(Color.FromArgb(219, 219, 219))
                G.DrawLine(New Pen(Color.FromArgb(63, 189, 220), 2), New Point(Width - 18, 10), New Point(Width - 14, 14))
                G.DrawLine(New Pen(Color.FromArgb(63, 189, 220), 2), New Point(Width - 14, 14), New Point(Width - 10, 10))
                G.DrawLine(New Pen(Color.FromArgb(63, 189, 220)), New Point(Width - 14, 15), New Point(Width - 14, 14))
            Case ColorSchemes.Light
                G.Clear(Color.White)
                G.DrawLine(New Pen(Color.FromArgb(63, 189, 220), 2), New Point(Width - 18, 10), New Point(Width - 14, 14))
                G.DrawLine(New Pen(Color.FromArgb(63, 189, 220), 2), New Point(Width - 14, 14), New Point(Width - 10, 10))
                G.DrawLine(New Pen(Color.FromArgb(63, 189, 220)), New Point(Width - 14, 15), New Point(Width - 14, 14))
        End Select
        G.DrawRectangle(New Pen(Color.FromArgb(170, 170, 170)), New Rectangle(0, 0, Width - 1, Height - 1))


        Try
            Select Case ColorScheme
                Case ColorSchemes.Dark
                    G.DrawString(Text, Font, Brushes.Gray, New Rectangle(7, 0, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                Case ColorSchemes.Light
                    G.DrawString(Text, Font, Brushes.Gray, New Rectangle(7, 0, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
            End Select
        Catch
        End Try

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class HurrycanProgressBar
    Inherits Control

#Region "Declarations"
    Private _ProgressColour As Color = Color.FromArgb(23, 180, 220)
    Private _BorderColour As Color = Color.FromArgb(170, 170, 170)
    Private _BaseColour As Color = Color.FromArgb(225, 225, 225)
    Private _FontColour As Color = Color.FromArgb(130, 130, 130)
    Private _SecondColour As Color = Color.FromArgb(0, 170, 250)
    Private _Value As Integer = 0
    Private _Maximum As Integer = 100
    Private _TwoColour As Boolean = True
#End Region

#Region "Properties"

    Public Property SecondColour As Color
        Get
            Return _SecondColour
        End Get
        Set(value As Color)
            _SecondColour = value
        End Set
    End Property

    <Category("Control")>
    Public Property TwoColour As Boolean
        Get
            Return _TwoColour
        End Get
        Set(value As Boolean)
            _TwoColour = value
        End Set
    End Property

    <Category("Control")>
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(V As Integer)
            Select Case V
                Case Is < _Value
                    _Value = V
            End Select
            _Maximum = V
            Invalidate()
        End Set
    End Property

    <Category("Control")>
    Public Property Value() As Integer
        Get
            Select Case _Value
                Case 0
                    Return 0
                    Invalidate()
                Case Else
                    Return _Value
                    Invalidate()
            End Select
        End Get
        Set(V As Integer)
            Select Case V
                Case Is > _Maximum
                    V = _Maximum
                    Invalidate()
            End Select
            _Value = V
            Invalidate()
        End Set
    End Property

    <Category("Colours")>
    Public Property ProgressColour As Color
        Get
            Return _ProgressColour
        End Get
        Set(value As Color)
            _ProgressColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property BorderColour As Color
        Get
            Return _BorderColour
        End Get
        Set(value As Color)
            _BorderColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property FontColour As Color
        Get
            Return _FontColour
        End Get
        Set(value As Color)
            _FontColour = value
        End Set
    End Property

#End Region

#Region "Events"

    Public Sub Increment(ByVal Amount As Integer)
        Value += Amount
    End Sub

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim G = e.Graphics
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            Dim ProgVal As Integer = CInt(_Value / _Maximum * Width)
            Select Case Value
                Case 0
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    .DrawRectangle(New Pen(_BorderColour, 2), Base)
                Case _Maximum
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    If _TwoColour Then
                        For i = 0 To (Width - 1) * _Maximum / _Value Step 25
                            G.DrawLine(New Pen(New SolidBrush(_SecondColour), 7), New Point(CInt(i), 0), New Point(CInt(i - 15), Height))
                        Next
                        G.ResetClip()
                    Else
                    End If
                    .DrawRectangle(New Pen(_BorderColour, 2), Base)
                Case Else
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    If _TwoColour Then
                        .SetClip(New Rectangle(0, 0, CInt(Width * _Value / _Maximum - 1), Height - 1))
                        For i = 0 To (Width - 1) * _Maximum / _Value Step 25
                            .DrawLine(New Pen(New SolidBrush(_SecondColour), 7), New Point(CInt(i), 0), New Point(CInt(i - 10), Height))
                        Next
                        .ResetClip()
                    Else
                    End If
                    .DrawRectangle(New Pen(_BorderColour, 2), Base)
            End Select
            .InterpolationMode = CType(7, InterpolationMode)
        End With
    End Sub

#End Region

End Class

Class HurrycanAlertBox
    Inherits Control

    Private exitLocation As Point
    Private overExit As Boolean

    Enum Style
        Simple
        Success
        Notice
        Warning
        Informations
    End Enum

    Private _alertStyle As Style
    Public Property AlertStyle As Style
        Get
            Return _alertStyle
        End Get
        Set(ByVal value As Style)
            _alertStyle = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw, True)
        Font = New Font("Verdana", 8)
        Size = New Size(200, 40)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim G As Graphics = e.Graphics

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        Dim borderColor, innerColor, textColor As Color
        Select Case _alertStyle
            Case Style.Simple
                borderColor = Color.FromArgb(170, 170, 170)
                innerColor = Color.FromArgb(225, 225, 225)
                textColor = Color.FromArgb(130, 130, 130)
            Case Style.Success
                borderColor = Color.FromArgb(60, 98, 79)
                innerColor = Color.FromArgb(60, 85, 79)
                textColor = Color.FromArgb(35, 169, 110)
            Case Style.Notice
                borderColor = Color.FromArgb(70, 91, 107)
                innerColor = Color.FromArgb(70, 91, 94)
                textColor = Color.FromArgb(97, 185, 186)
            Case Style.Warning
                borderColor = Color.FromArgb(100, 71, 71)
                innerColor = Color.FromArgb(87, 71, 71)
                textColor = Color.FromArgb(254, 142, 122)
            Case Style.Informations
                borderColor = Color.FromArgb(133, 133, 71)
                innerColor = Color.FromArgb(120, 120, 71)
                textColor = Color.FromArgb(254, 224, 122)
        End Select

        Dim mainRect As New Rectangle(0, 0, Width - 1, Height - 1)
        G.FillRectangle(New SolidBrush(innerColor), mainRect)
        G.DrawRectangle(New Pen(borderColor), mainRect)

        Dim styleText As String = Nothing

        Select Case _alertStyle

        End Select

        Dim styleFont As New Font(Font.FontFamily, Font.Size, FontStyle.Bold)
        Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Text, Font).Height / 2)
        G.DrawString(styleText, styleFont, New SolidBrush(textColor), New Point(10, textY))
        G.DrawString(Text, Font, New SolidBrush(textColor), New Point(10 + G.MeasureString(styleText, styleFont).Width + 4, textY))

        Dim exitFont As New Font("Marlett", 6)
        Dim exitY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString("r", exitFont).Height / 2) + 1
        exitLocation = New Point(Width - 26, exitY - 3)
        G.DrawString("r", exitFont, New SolidBrush(textColor), New Point(Width - 23, exitY))

    End Sub


    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)

        If e.X >= Width - 26 AndAlso e.X <= Width - 12 AndAlso e.Y > exitLocation.Y AndAlso e.Y < exitLocation.Y + 12 Then
            If Cursor <> Cursors.Hand Then Cursor = Cursors.Hand
            overExit = True
        Else
            If Cursor <> Cursors.Arrow Then Cursor = Cursors.Arrow
            overExit = False
        End If

        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If overExit Then Me.Visible = False
    End Sub
End Class

Class HurrycanMainTab
    Inherits TabControl

#Region "Properties | TabControl"
    Private _HighlightTab As Boolean = True
    Public Property HighlightTab() As Boolean
        Get
            Return _HighlightTab
        End Get
        Set(ByVal v As Boolean)
            _HighlightTab = v
            Invalidate()
        End Set
    End Property

    Private _ShowBorders As Boolean = False
    Public Property ShowBorders() As Boolean
        Get
            Return _ShowBorders
        End Get
        Set(ByVal v As Boolean)
            _ShowBorders = v
            Invalidate()
        End Set
    End Property

    Private _DisableTabIndex As String
    Public Property DisableTabIndex() As String
        Get
            Return _DisableTabIndex
        End Get
        Set(ByVal v As String)
            _DisableTabIndex = v
            Invalidate()
        End Set
    End Property

    Private _BGColor As Color = Color.FromArgb(219, 219, 219)
    Public Property BGColor() As Color
        Get
            Return _BGColor
        End Get
        Set(ByVal v As Color)
            _BGColor = v
            Invalidate()
        End Set
    End Property
#End Region

#Region "Variables | Colors, Integer, Point"
    Private BC As Color = Color.FromArgb(32, 175, 206) 'BACK COLOR
    Private AC As Color = Color.FromArgb(225, 225, 225) 'ACTIVE COLOR
    Private _MOUSEPOS As Point
    Private LSI As Integer 'LAST SELECTED INDEX
    Private MOUSE_STATE As MouseState
#End Region

#Region "Functions | Disabled tab page(s)"
    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MOUSE_STATE = MouseState.None
        _MOUSEPOS.X = New Integer
        _MOUSEPOS.Y = New Integer
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MOUSE_STATE = MouseState.Over
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnSelecting(ByVal e As System.Windows.Forms.TabControlCancelEventArgs)
        If DisabledTabList.Contains(e.TabPageIndex) = True Then e.Cancel = True
        MyBase.OnSelecting(e)
    End Sub

    Private Function DisabledTabList() As List(Of Integer)
        Dim l As New List(Of Integer)
        If IsNumeric(DisableTabIndex.Replace(":", Nothing)) = False Then Return l
        Try
            Dim x As Integer = 0
            Do
                l.Add(DisableTabIndex.Split(":")(x))
                x += 1
            Loop
        Catch
        End Try
        Return l
    End Function

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        _MOUSEPOS.X = e.X
        _MOUSEPOS.Y = e.Y
        MyBase.OnMouseMove(e)
    End Sub

    Private Function IsMouseOver(ByVal r As Rectangle) As Boolean
        If _MOUSEPOS.X >= r.X And _MOUSEPOS.Y >= r.Y And _MOUSEPOS.X < (r.Width + r.X) And _MOUSEPOS.Y < (r.Height + r.Y) Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True

        ShowBorders = True
        HighlightTab = True

        DisableTabIndex = "example: '0:1'"
        BackColor = Color.White
        BGColor = Color.FromArgb(219, 219, 219)
        Font = New Font("Segoe UI", 9)
        SizeMode = TabSizeMode.FillToRight
        ItemSize = New Size(140, 47)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.Clear(Color.FromArgb(73, 169, 188))
        G.SmoothingMode = SmoothingMode.HighQuality

        G.FillRectangle(New SolidBrush(BGColor), New Rectangle(-1, -1, Width + 1, ItemSize.Height - 6))

        G.DrawLine(New Pen(BC, 2), New Point(-1, ItemSize.Height - 6), New Point(Width, ItemSize.Height - 6))

        If ShowBorders = True Then
            G.DrawLine(New Pen(BC, 3), New Point(1, ItemSize.Height - 6), New Point(1, Height))
            G.DrawLine(New Pen(BC, 3), New Point(1, Height - 2), New Point(Width, Height - 2))
            G.DrawLine(New Pen(BC, 3), New Point(Width - 2, Height - 2), New Point(Width - 2, ItemSize.Height - 6))
        End If

        For i = 0 To TabCount - 1

            Dim Base As Rectangle = GetTabRect(i)
            Dim Custom As Rectangle = Base

            Custom.Width -= 2
            Custom.Height -= 10
            Custom.Y += 1
            Custom.X += 1

            Dim P As New GraphicsPath
            Dim PS As New List(Of Point)

            PS.Add(New Point(Custom.X + 3, Custom.Y))
            PS.Add(New Point(Custom.X + Custom.Width - 3, Custom.Y))
            PS.Add(New Point(Custom.X + Custom.Width, Custom.Y + 3))

            If DisabledTabList.Contains(i) = True Then
                PS.Add(New Point(Custom.X + Custom.Width, Custom.Y + Custom.Height))
                PS.Add(New Point(Custom.X, Custom.Y + Custom.Height))
                PS.Add(New Point(Custom.X, Custom.Y + 3))

                P.AddPolygon(PS.ToArray)
                Dim BR As New LinearGradientBrush(New Rectangle(0, 0, ItemSize.Width, ItemSize.Height), Color.FromArgb(57, 139, 154), Color.FromArgb(21, 122, 144), 90S)
                G.FillPath(BR, P)

                Custom.Height += 3
                G.DrawString(Space(0) & TabPages(i).Text.ToUpper & Space(6), Font, New SolidBrush(Color.FromArgb(0, 0, 0)), Custom, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
                GoTo 1
            End If

            If i = SelectedIndex Then
2:
                LSI = i
                Custom.Height -= 1

                PS.Add(New Point(Custom.X + Custom.Width, Custom.Y + Custom.Height + 2))
                PS.Add(New Point((Custom.X + (Custom.Width / 2)) + 8, Custom.Y + Custom.Height + 2))
                PS.Add(New Point((Custom.X + (Custom.Width / 2)), Custom.Y + Custom.Height + 9))
                PS.Add(New Point((Custom.X + (Custom.Width / 2)) - 8, Custom.Y + Custom.Height + 2))
                PS.Add(New Point(Custom.X, Custom.Y + Custom.Height + 2))
                PS.Add(New Point(Custom.X, Custom.Y + 3))

                P.AddPolygon(PS.ToArray)

                Dim BR As New LinearGradientBrush(New Rectangle(0, 0, ItemSize.Width, ItemSize.Height + 7), Color.FromArgb(225, 225, 225), Color.FromArgb(225, 225, 225), 90S)
                G.FillPath(BR, P)

                Dim L As New List(Of Point)
                L.Add(New Point(Custom.X + Custom.Width - 1, Custom.Y + 4))
                L.Add(New Point(Custom.X + Custom.Width - 1, Custom.Y + Custom.Height + 1))
                L.Add(New Point((Custom.X + (Custom.Width / 2)) + 7, Custom.Y + Custom.Height + 1))
                L.Add(New Point((Custom.X + (Custom.Width / 2)), Custom.Y + Custom.Height + 8))
                L.Add(New Point((Custom.X + (Custom.Width / 2)) - 7, Custom.Y + Custom.Height + 1))
                L.Add(New Point(Custom.X + 1, Custom.Y + Custom.Height + 1))
                L.Add(New Point(Custom.X + 1, Custom.Y + 4))

                G.DrawLines(New Pen(New SolidBrush(AC), 2), L.ToArray)

                L.Clear()

                L.Add(New Point(Custom.X, Custom.Y + Custom.Height + 3))
                L.Add(New Point((Custom.X + (Custom.Width / 2)) - 8, Custom.Y + Custom.Height + 3))
                L.Add(New Point((Custom.X + (Custom.Width / 2)), Custom.Y + Custom.Height + 10))
                L.Add(New Point((Custom.X + (Custom.Width / 2)) + 8, Custom.Y + Custom.Height + 3))
                L.Add(New Point(Custom.X + Custom.Width, Custom.Y + Custom.Height + 3))

                G.DrawLines(New Pen(New SolidBrush(AC), 2), L.ToArray)
                G.DrawLines(New Pen(New SolidBrush(Color.FromArgb(20, Color.Black))), L.ToArray)
                G.DrawLines(New Pen(New SolidBrush(Color.FromArgb(10, Color.Black)), 4), L.ToArray)

                Custom.Height += 3
                G.DrawString(Space(0) & TabPages(i).Text & Space(6), Font, New SolidBrush(Color.FromArgb(130, 130, 130)), Custom, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Else

                For Each k In DisabledTabList()
                    If k > TabCount - 1 Then Exit For
                    If IsMouseOver(GetTabRect(k)) = True And LSI = i Then
                        GoTo 2
                    End If
                Next

                PS.Add(New Point(Custom.X + Custom.Width, Custom.Y + Custom.Height))
                PS.Add(New Point(Custom.X, Custom.Y + Custom.Height))
                PS.Add(New Point(Custom.X, Custom.Y + 3))

                P.AddPolygon(PS.ToArray)

                Dim BR As New LinearGradientBrush(New Rectangle(0, 0, ItemSize.Width, ItemSize.Height), Color.FromArgb(79, 198, 219), Color.FromArgb(32, 175, 206), 90S)
                If IsMouseOver(Base) = True And MOUSE_STATE = MouseState.Over And HighlightTab = True Then BR = New LinearGradientBrush(New Rectangle(0, 0, ItemSize.Width, ItemSize.Height), Color.FromArgb(135, 215, 230), Color.FromArgb(109, 203, 223), 90S)

                G.FillPath(BR, P)

                Custom.Height += 3
                G.DrawString(Space(0) & TabPages(i).Text & Space(6), Font, New SolidBrush(AC), Custom, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            End If
1:
        Next

        e.Graphics.DrawImage(B.Clone, 0, 0)
        G.Dispose()
        B.Dispose()
        MyBase.OnPaint(e)
    End Sub

End Class

Class HurrycanSecondTab
    Inherits TabControl

#Region "Properties | TabControl"
    Private _BGColor As Color = Color.FromArgb(219, 219, 219)
    Public Property BGColor() As Color
        Get
            Return _BGColor
        End Get
        Set(ByVal v As Color)
            _BGColor = v
            Invalidate()
        End Set
    End Property
#End Region

#Region "Variables | Colors"
    Private TC As Color = Color.FromArgb(145, 145, 145) 'TEXT COLOR
    Private AC As Color = Color.FromArgb(230, 230, 230) 'ACTIVE COLOR
#End Region

#Region "Functions | Create round"
    Private Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        Dim CreateRoundPath As GraphicsPath

        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function
#End Region

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Top
    End Sub

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
         ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True

        Dock = DockStyle.Top

        BGColor = Color.White
        Font = New Font("Segoe UI", 9)
        SizeMode = TabSizeMode.FillToRight
        ItemSize = New Size(120, 30)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.Clear(Color.FromArgb(219, 219, 219))

        G.SmoothingMode = SmoothingMode.HighQuality

        Dim Brsh As New SolidBrush(Color.FromArgb(215, 215, 215))

        G.FillRectangle(Brsh, New Rectangle(-1, ItemSize.Height - 5, Width, 5))

        For i = 0 To TabCount - 1

            Dim Base As Rectangle = GetTabRect(i)
            Dim Custom As Rectangle = Base

            Custom.Y -= 2
            Custom.Height -= 12

            If i = SelectedIndex Then
                Dim GP As New GraphicsPath
                GP = CreateRound(Custom, 10)
                G.FillPath(New SolidBrush(AC), GP)

            Else
                G.FillRectangle(New SolidBrush(BGColor), Custom)

            End If

            G.DrawString(TabPages(i).Text, Font, New SolidBrush(TC), Custom, New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        Next

        e.Graphics.DrawImage(B.Clone, 0, 0)
        G.Dispose()
        B.Dispose()
        MyBase.OnPaint(e)
    End Sub

End Class