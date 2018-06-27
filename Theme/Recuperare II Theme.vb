Imports System, System.IO, System.Collections.Generic, System.Runtime.InteropServices, System.ComponentModel
Imports System.Drawing, System.Drawing.Drawing2D, System.Drawing.Imaging, System.Windows.Forms

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

'--------------------- [ Theme ] --------------------
'Creator: Recuperare
'Contact: cschaefer2183 (Skype)
'Created: 01.02.2012
'Changed: 01.02.2012
'-------------------- [ /Theme ] ---------------------

'PLEASE LEAVE CREDITS IN SOURCE, DO NOT REDISTRIBUTE!

#Region " GLOBAL FUNCTIONS "
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
End Module

Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum
#End Region


Public Class RecuperareIIButton : Inherits Control

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

#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(27, 94, 137)
        DoubleBuffered = True
        Size = New Size(75, 23)
        Font = New Font("Verdana", 6.75F, FontStyle.Bold)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim ClientRectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        MyBase.OnPaint(e)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(BackColor)

        Select Case State
            Case MouseState.None 'Mouse None
                Dim bodyGrad As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 2), Color.FromArgb(245, 245, 245), Color.FromArgb(230, 230, 230), 90S)
                G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
                Dim bodyInBorder As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 4), Color.FromArgb(252, 252, 252), Color.FromArgb(249, 249, 249), 90S)
                G.DrawRectangle(New Pen(bodyInBorder), New Rectangle(1, 1, Width - 3, Height - 4))
                G.DrawRectangle(New Pen(Color.FromArgb(189, 189, 189)), New Rectangle(0, 0, Width - 1, Height - 2))
                G.DrawLine(New Pen(Color.FromArgb(200, 168, 168, 168)), New Point(1, Height - 1), New Point(Width - 2, Height - 1))
                ForeColor = Color.FromArgb(27, 94, 137)
                G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(200, Color.White)), New Rectangle(-1, 0, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            Case MouseState.Over 'Mouse Hover
                Dim bodyGrad As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 2), Color.FromArgb(70, 153, 205), Color.FromArgb(53, 124, 170), 90S)
                G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
                Dim bodyInBorder As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 4), Color.FromArgb(88, 168, 221), Color.FromArgb(76, 149, 194), 90S)
                G.DrawRectangle(New Pen(bodyInBorder), New Rectangle(1, 1, Width - 3, Height - 4))
                G.DrawRectangle(New Pen(Color.FromArgb(38, 93, 131)), New Rectangle(0, 0, Width - 1, Height - 2))
                G.DrawLine(New Pen(Color.FromArgb(200, 25, 73, 109)), New Point(1, Height - 1), New Point(Width - 2, Height - 1))
                ForeColor = Color.White
                G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(200, Color.Black)), New Rectangle(-1, -2, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
            Case MouseState.Down 'Mouse Down
                Dim bodyGrad As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 2), Color.FromArgb(70, 153, 205), Color.FromArgb(53, 124, 170), 270S)
                G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
                Dim bodyInBorder As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 4), Color.FromArgb(88, 168, 221), Color.FromArgb(76, 149, 194), 270S)
                G.DrawRectangle(New Pen(bodyInBorder), New Rectangle(1, 1, Width - 3, Height - 4))
                G.DrawRectangle(New Pen(Color.FromArgb(38, 93, 131)), New Rectangle(0, 0, Width - 1, Height - 2))
                G.DrawLine(New Pen(Color.FromArgb(200, 25, 73, 109)), New Point(1, Height - 1), New Point(Width - 2, Height - 1))
                ForeColor = Color.White
                G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(200, Color.Black)), New Rectangle(-1, -2, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
        End Select
        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(-1, -1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class RecuperareIIComboBox : Inherits ComboBox
#Region " Control Help - Properties & Flicker Control "
    Private _StartIndex As Integer = 0
    Public Property StartIndex() As Integer
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
                e.Graphics.FillRectangle(New SolidBrush(_highlightColor), e.Bounds)
                Dim gloss As New LinearGradientBrush(e.Bounds, Color.FromArgb(15, Color.White), Color.FromArgb(0, Color.White), 90S)
                e.Graphics.FillRectangle(gloss, New Rectangle(New Point(e.Bounds.X, e.Bounds.Y), New Size(e.Bounds.Width, e.Bounds.Height)))
                e.Graphics.DrawRectangle(New Pen(Color.FromArgb(50, Color.Black)) With {.DashStyle = DashStyle.Dot}, New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1))
            Else
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(255, 255, 255, 255)), e.Bounds)
            End If
            Using b As New SolidBrush(Color.Black)
                e.Graphics.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, b, New Rectangle(e.Bounds.X + 2, e.Bounds.Y, e.Bounds.Width - 4, e.Bounds.Height))
            End Using
        Catch
        End Try
        e.DrawFocusRectangle()
    End Sub
    Protected Sub DrawTriangle(ByVal Clr As Color, ByVal FirstPoint As Point, ByVal SecondPoint As Point, ByVal ThirdPoint As Point, ByVal G As Graphics)
        Dim points As New List(Of Point)()
        points.Add(FirstPoint)
        points.Add(SecondPoint)
        points.Add(ThirdPoint)
        G.FillPolygon(New SolidBrush(Clr), points.ToArray)
    End Sub
    Private _highlightColor As Color = Color.FromArgb(121, 176, 214)
    Public Property ItemHighlightColor() As Color
        Get
            Return _highlightColor
        End Get
        Set(ByVal v As Color)
            _highlightColor = v
            Invalidate()
        End Set
    End Property
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer Or _
        ControlStyles.SupportsTransparentBackColor, True)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(27, 94, 137)
        Font = New Font("Verdana", 6.75F, FontStyle.Bold)
        DropDownStyle = ComboBoxStyle.DropDownList
        DoubleBuffered = True
        Size = New Size(Width, 21)
        ItemHeight = 16
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality


        G.Clear(BackColor)
        Dim bodyGradNone As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 2), Color.FromArgb(245, 245, 245), Color.FromArgb(230, 230, 230), 90S)
        G.FillRectangle(bodyGradNone, bodyGradNone.Rectangle)
        Dim bodyInBorderNone As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 3), Color.FromArgb(252, 252, 252), Color.FromArgb(249, 249, 249), 90S)
        G.DrawRectangle(New Pen(bodyInBorderNone), New Rectangle(1, 1, Width - 3, Height - 4))
        G.DrawRectangle(New Pen(Color.FromArgb(189, 189, 189)), New Rectangle(0, 0, Width - 1, Height - 2))
        G.DrawLine(New Pen(Color.FromArgb(200, 168, 168, 168)), New Point(1, Height - 1), New Point(Width - 2, Height - 1))
        DrawTriangle(Color.FromArgb(121, 176, 214), New Point(Width - 14, 8), New Point(Width - 7, 8), New Point(Width - 11, 12), G)
        G.DrawLine(New Pen(Color.FromArgb(27, 94, 137)), New Point(Width - 14, 8), New Point(Width - 8, 8))

        'Draw Separator line
        G.DrawLine(New Pen(Color.White), New Point(Width - 22, 1), New Point(Width - 22, Height - 3))
        G.DrawLine(New Pen(Color.FromArgb(189, 189, 189)), New Point(Width - 21, 1), New Point(Width - 21, Height - 3))
        G.DrawLine(New Pen(Color.White), New Point(Width - 20, 1), New Point(Width - 20, Height - 3))
        Try
            G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(5, -1, Width - 20, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        Catch
        End Try

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class RecuperareIITextBox : Inherits Control
    Dim WithEvents txtbox As New TextBox

#Region " Control Help - Properties & Flicker Control "
    Private _passmask As Boolean = False
    Public Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _passmask
        End Get
        Set(ByVal v As Boolean)
            txtbox.UseSystemPasswordChar = UseSystemPasswordChar
            _passmask = v
            Invalidate()
        End Set
    End Property
    Private _maxchars As Integer = 32767
    Public Shadows Property MaxLength() As Integer
        Get
            Return _maxchars
        End Get
        Set(ByVal v As Integer)
            _maxchars = v
            txtbox.MaxLength = MaxLength
            Invalidate()
        End Set
    End Property
    Private _align As HorizontalAlignment
    Public Shadows Property TextAlignment() As HorizontalAlignment
        Get
            Return _align
        End Get
        Set(ByVal v As HorizontalAlignment)
            _align = v
            Invalidate()
        End Set
    End Property
    Private _multiline As Boolean = False
    Public Shadows Property MultiLine() As Boolean
        Get
            Return _multiline
        End Get
        Set(ByVal value As Boolean)
            _multiline = value
            Invalidate()
        End Set
    End Property


    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnBackColorChanged(e)
        txtbox.BackColor = BackColor
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
    Sub TextChngTxtBox() Handles txtbox.TextChanged
        Text = txtbox.Text
    End Sub
    Sub TextChng() Handles MyBase.TextChanged
        txtbox.Text = Text
    End Sub
    Sub NewTextBox()
        With txtbox
            .Multiline = False
            .BackColor = Color.FromArgb(43, 43, 43)
            .ForeColor = ForeColor
            .Text = String.Empty
            .TextAlign = HorizontalAlignment.Center
            .BorderStyle = BorderStyle.None
            .Location = New Point(5, 4)
            .Font = New Font("Trebuchet MS", 8.25F, FontStyle.Bold)
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
        BackColor = Color.FromArgb(233, 233, 233)
        ForeColor = Color.FromArgb(27, 94, 137)
        Font = New Font("Verdana", 6.75F, FontStyle.Bold)
        Size = New Size(135, 35)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality

        Height = txtbox.Height + 10
        With txtbox
            .Width = Width - 10
            .TextAlign = TextAlignment
            .UseSystemPasswordChar = UseSystemPasswordChar
        End With

        G.Clear(Color.Transparent)

        Dim innerBorderBrush As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 3), Color.FromArgb(220, 220, 220), Color.FromArgb(228, 228, 228), 90S)
        Dim innerBorderPen As New Pen(innerBorderBrush)
        G.DrawRectangle(innerBorderPen, New Rectangle(1, 1, Width - 3, Height - 3))
        G.DrawLine(New Pen(Color.FromArgb(191, 191, 191)), New Point(1, 1), New Point(Width - 3, 1))

        G.DrawRectangle(New Pen(Color.FromArgb(254, 254, 254)), New Rectangle(0, 0, Width - 1, Height - 1))
        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> Public Class RecuperareIICheckBox : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private Mouse As New Point(0, 0)
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        Mouse = e.Location : Invalidate()
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
        Height = 14
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        If Mouse.X < Height - 1 Then
            _Checked = Not _Checked
            RaiseEvent CheckedChanged(Me)
        End If
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        Font = New Font("Verdana", 6.75F, FontStyle.Bold)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(27, 94, 137)
        Size = New Size(145, 16)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim checkBoxRectangle As New Rectangle(0, 0, Height - 1, Height - 1)
        G.SmoothingMode = SmoothingMode.HighQuality

        G.Clear(Parent.FindForm.BackColor)

        Dim bodyGrad As New LinearGradientBrush(checkBoxRectangle, Color.FromArgb(245, 245, 245), Color.FromArgb(231, 231, 231), 90S)
        G.FillRectangle(bodyGrad, bodyGrad.Rectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(189, 189, 189)), New Rectangle(0, 0, Height - 1, Height - 2))
        G.DrawRectangle(New Pen(Color.FromArgb(252, 252, 252)), New Rectangle(1, 1, Height - 3, Height - 4))
        G.DrawLine(New Pen(Color.FromArgb(168, 168, 168)), New Point(1, Height - 1), New Point(Height - 2, Height - 1))

        If Checked Then
            Dim chkPoly As Rectangle = New Rectangle(checkBoxRectangle.X + checkBoxRectangle.Width / 4, checkBoxRectangle.Y + checkBoxRectangle.Height / 4, checkBoxRectangle.Width \ 2, checkBoxRectangle.Height \ 2)
            Dim Poly() As Point = {New Point(chkPoly.X, chkPoly.Y + chkPoly.Height \ 2), _
                           New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height), _
                           New Point(chkPoly.X + chkPoly.Width, chkPoly.Y)}
            G.SmoothingMode = SmoothingMode.HighQuality
            Dim P1 As New Pen(Color.FromArgb(27, 94, 137), 2)
            Dim chkGrad As New LinearGradientBrush(chkPoly, Color.FromArgb(200, 200, 200), Color.FromArgb(255, 255, 255), 0S)
            For i = 0 To Poly.Length - 2
                'G.DrawLine(P1, Poly(i), Poly(i + 1))
            Next
            G.DrawString("a", New Font("Marlett", 10.75), New SolidBrush(Color.FromArgb(220, ForeColor)), New Rectangle(-2, -1, Width - 1, Height - 1), New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
        End If

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(16, 0), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})


        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class

<DefaultEvent("CheckedChanged")> Public Class RecuperareIIRadioButton : Inherits Control

#Region " Control Help - MouseState & Flicker Control"
    Private R1 As Rectangle
    Private G1 As LinearGradientBrush
    Private Mouse As New Point(0, 0)
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 14
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        Mouse = e.Location : Invalidate()
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
        If Mouse.X < Height - 1 Then
            If Not _Checked Then Checked = True
            RaiseEvent CheckedChanged(Me)
        End If
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        InvalidateControls()
    End Sub
    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is RecuperareIIRadioButton Then
                DirectCast(C, RecuperareIIRadioButton).Checked = False
            End If
        Next
    End Sub
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(27, 94, 137)
        Font = New Font("Verdana", 6.75F, FontStyle.Bold)
        Size = New Size(152, 14)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.FindForm.BackColor)

        G.DrawEllipse(New Pen(Color.FromArgb(168, 168, 168)), New Rectangle(0, 0, Height - 2, Height - 1))
        Dim bgGrad As New LinearGradientBrush(New Rectangle(0, 0, Height - 2, Height - 2), Color.FromArgb(245, 245, 245), Color.FromArgb(231, 231, 231), 90S)
        G.FillEllipse(bgGrad, New Rectangle(0, 0, Height - 2, Height - 2))
        G.DrawEllipse(New Pen(Color.FromArgb(252, 252, 252)), New Rectangle(1, 1, Height - 4, Height - 4))

        If Checked Then
            G.FillEllipse(New SolidBrush(Color.FromArgb(27, 94, 137)), New Rectangle(3, 3, Height - 8, Height - 8))
            G.FillEllipse(New SolidBrush(Color.FromArgb(150, 118, 177, 211)), New Rectangle(4, 4, Height - 10, Height - 10))
        End If

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(16, 1), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class

Public Class RecuperareIIStatusBar : Inherits Control

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Dock = DockStyle.Bottom
    End Sub
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e) : Invalidate()
    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e) : Invalidate()
    End Sub

    Sub New()
        MyBase.New()
        Font = New Font("Verdana", 6.75F, FontStyle.Bold)
        ForeColor = Color.FromArgb(27, 94, 137)
        Size = New Size(Width, 20)
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        MyBase.OnPaint(e)

        Dim bodyGradNone As New LinearGradientBrush(New Rectangle(0, 1, Width, Height - 1), Color.FromArgb(245, 245, 245), Color.FromArgb(230, 230, 230), 90S)
        G.FillRectangle(bodyGradNone, bodyGradNone.Rectangle)
        Dim bodyInBorderNone As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 3), Color.FromArgb(200, 252, 252, 252), Color.FromArgb(200, 249, 249, 249), 90S)
        G.DrawRectangle(New Pen(bodyInBorderNone), New Rectangle(1, 1, Width - 3, Height - 3))
        G.DrawRectangle(New Pen(Color.FromArgb(189, 189, 189)), New Rectangle(0, 0, Width - 1, Height - 1))

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(4, 4), StringFormat.GenericDefault)

        e.Graphics.DrawImage(B, 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class

Public Class RecuperareIINumericUpDown : Inherits Control

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
        Me.Height = 30
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseClick(e)
        If X > Me.Width - 21 AndAlso X < Me.Width - 3 Then
            If Y < 15 Then
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
        BackColor = Color.FromArgb(233, 233, 233)
        ForeColor = Color.FromArgb(27, 94, 137)
        DoubleBuffered = True
        Font = New Font("Verdana", 6.75F, FontStyle.Bold)

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.Clear(BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim innerBorderBrush As New LinearGradientBrush(New Rectangle(1, 1, Width - 3, Height - 3), Color.FromArgb(220, 220, 220), Color.FromArgb(228, 228, 228), 90S)
        Dim innerBorderPen As New Pen(innerBorderBrush)
        G.DrawRectangle(innerBorderPen, New Rectangle(1, 1, Width - 3, Height - 3))
        G.DrawLine(New Pen(Color.FromArgb(191, 191, 191)), New Point(1, 1), New Point(Width - 3, 1))

        G.DrawRectangle(New Pen(Color.FromArgb(254, 254, 254)), New Rectangle(0, 0, Width - 1, Height - 1))


        Dim buttonGradient As New LinearGradientBrush(New Rectangle(Width - 23, 4, 19, 21), Color.FromArgb(245, 245, 245), Color.FromArgb(232, 232, 232), 90S)
        G.FillRectangle(buttonGradient, buttonGradient.Rectangle)
        G.DrawRectangle(New Pen(Color.FromArgb(252, 252, 252)), New Rectangle(Width - 22, 5, 17, 19))
        G.DrawRectangle(New Pen(Color.FromArgb(190, 190, 190)), New Rectangle(Width - 23, 4, 19, 21))
        G.DrawLine(New Pen(Color.FromArgb(200, 167, 167, 167)), New Point(Width - 22, Height - 4), New Point(Width - 5, Height - 4))
        G.DrawLine(New Pen(Color.FromArgb(188, 188, 188)), New Point(Width - 22, Height - 16), New Point(Width - 5, Height - 16))
        G.DrawLine(New Pen(Color.FromArgb(252, 252, 252)), New Point(Width - 22, Height - 15), New Point(Width - 5, Height - 15))


        'Top Triangle
        DrawTriangle(Color.FromArgb(27, 94, 137), New Point(Width - 17, 18), New Point(Width - 9, 18), New Point(Width - 13, 21), G)

        'Bottom Triangle
        DrawTriangle(Color.FromArgb(27, 94, 137), New Point(Width - 17, 10), New Point(Width - 9, 10), New Point(Width - 13, 7), G)

        G.DrawString(Value, Font, New SolidBrush(ForeColor), New Rectangle(5, 0, Width - 1, Height - 1), New StringFormat() With {.LineAlignment = StringAlignment.Center})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()
    End Sub

End Class

Public Class RecuperareIILabel : Inherits Label
    Sub New()
        MyBase.New()
        Font = New Font("Verdana", 6.75F, FontStyle.Bold)
        ForeColor = Color.FromArgb(27, 94, 137)
    End Sub
End Class