Imports System.Drawing.Drawing2D
'|===========================================================|
'|===|  Cry Engine Theme
'| Creator: LordPankake
'| HF Account: http://www.hackforums.net/member.php?action=profile&uid=1828119
'| Created: 8/19/2014, Last edited: 8/19/2014
'|===========================================================|
#Region "Base Classes"
Public Class ThemedControl : Inherits Control
    Public D As New DrawUtils
    Public State As MouseState = MouseState.None
    Public Pal As Palette
    Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
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
    Sub New()
        MyBase.New()
        MinimumSize = New Size(20, 20)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
        Pal = New Palette
        Pal.ColHighest = Color.FromArgb(105, 105, 105)
        Pal.ColHigh = Color.FromArgb(65, 67, 69)
        Pal.ColMed = Color.FromArgb(71, 71, 71)
        Pal.ColDim = Color.FromArgb(52, 52, 52)
        Pal.ColDark = Color.FromArgb(39, 39, 39)
        BackColor = Pal.ColDim
    End Sub
End Class
Public Class ThemedListControl : Inherits listbox
    Public D As New DrawUtils
    Public State As MouseState = MouseState.None
    Public Pal As Palette
    Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
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
    Sub New()
        MyBase.New()
        MinimumSize = New Size(20, 20)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Segoe UI", 10.0F)
        DoubleBuffered = True
        Pal = New Palette
        Pal.ColHighest = Color.FromArgb(105, 105, 105)
        Pal.ColHigh = Color.FromArgb(65, 67, 69)
        Pal.ColMed = Color.FromArgb(71, 71, 71)
        Pal.ColDim = Color.FromArgb(52, 52, 52)
        Pal.ColDark = Color.FromArgb(39, 39, 39)
        BackColor = Pal.ColDim
    End Sub
End Class
Public Class ThemedContainer : Inherits ContainerControl
    Public D As New DrawUtils
    Protected DoSize As Boolean = True
    Protected DoDrag As Boolean = True
    Public State As MouseState = MouseState.None
    Protected TopCap As Boolean = False
    Protected SizeCap As Boolean = False
    Public Pal As Palette
    Protected MouseP As Point = New Point(0, 0)
    Protected TopGrip As Integer
    Protected Overrides Sub OnPaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If DoDrag And New Rectangle(0, 0, Width, TopGrip).Contains(e.Location) Then
                TopCap = True : MouseP = e.Location
            ElseIf DoSize And New Rectangle(Width - 15, Height - 15, 15, 15).Contains(e.Location) Then
                SizeCap = True : MouseP = e.Location
            End If
        End If
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over
        TopCap = False
        If DoSize Then
            SizeCap = False
        End If

    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)

        If DoDrag And TopCap Then
            Parent.Location = MousePosition - MouseP
        End If
        If DoSize And SizeCap Then
            MouseP = e.Location
            Parent.Size = New Size(MouseP)
            Invalidate()
        End If

    End Sub
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
    Sub New()
        MyBase.New()
        MinimumSize = New Size(20, 20)
        ForeColor = Color.FromArgb(146, 149, 152)
        Font = New Font("Trebuchet MS", 10.0F)
        DoubleBuffered = True
        Pal = New Palette
        Pal.ColHighest = Color.FromArgb(105, 105, 105)
        Pal.ColHigh = Color.FromArgb(65, 67, 69)
        Pal.ColMed = Color.FromArgb(71, 71, 71)
        Pal.ColDim = Color.FromArgb(49, 49, 49)
        Pal.ColDark = Color.FromArgb(39, 39, 39)
        BackColor = Pal.ColDim
    End Sub
End Class
#End Region
#Region "Theme"
Public Class CryForm : Inherits ThemedContainer
    Sub New()
        MyBase.New()
        MinimumSize = New Size(305, 150)
        BackColor = Pal.ColMed
        Dock = DockStyle.Fill
        TopGrip = 75
        DoDrag = False
        DoSize = False
        Font = New Font("Segoe UI Light", 20.0F)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        Try
            Me.ParentForm.MinimumSize = MinimumSize
            If Me.ParentForm.FormBorderStyle <> FormBorderStyle.FixedToolWindow Then
                Me.ParentForm.FormBorderStyle = FormBorderStyle.FixedToolWindow
            End If
        Catch ex As Exception : End Try
        G.Clear(BackColor)
        Dim TopLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, TopGrip), Pal.ColDim, Pal.ColDark)
        G.FillRectangle(TopLGB, New Rectangle(0, 0, Width, TopGrip))
        G.SmoothingMode = SmoothingMode.AntiAlias
        D.DrawText(G, New Rectangle(12, 0, Width, TopGrip), Text, Font, HorizontalAlignment.Left, Color.WhiteSmoke)
    End Sub
End Class
Public Class CryDialogButton : Inherits ThemedControl
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI Semilight", 13.0F)
        ForeColor = Pal.ColHighest
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim BorderCol As New Color
        Select Case State
            Case MouseState.None
                BorderCol = Color.FromArgb(25, 100, 130)
            Case MouseState.Over
                BorderCol = Color.FromArgb(30, 120, 160)
            Case MouseState.Down
                BorderCol = Color.FromArgb(10, 75, 100)
        End Select
        G.DrawPath(New Pen(BorderCol), D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 5))
        D.DrawText(G, New Rectangle(6, 0, Width - 13, Height - 2), Text, Font, HorizontalAlignment.Left, ForeColor)
    End Sub
End Class
Public Class CryButton : Inherits ThemedControl
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI Semilight", 13.0F)
        ForeColor = Pal.ColHighest
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        Dim TopCol, Botcol As New Color
        Select Case State
            Case MouseState.None
                TopCol = Color.FromArgb(Pal.ColDark.R + 30, Pal.ColDark.R + 30, Pal.ColDark.R + 30)
                Botcol = Pal.ColDark
            Case MouseState.Over
                TopCol = Color.FromArgb(Pal.ColDim.R + 30, Pal.ColDim.R + 30, Pal.ColDim.R + 30)
                Botcol = Pal.ColDim
            Case MouseState.Down
                TopCol = Pal.ColDark
                Botcol = Color.FromArgb(Pal.ColDark.R - 30, Pal.ColDark.R - 30, Pal.ColDark.R - 30)
        End Select
        Dim InnerLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), TopCol, Botcol)
        G.DrawRectangle(New Pen(Pal.ColHighest), New Rectangle(0, 0, Width - 2, Height - 2))
        G.DrawRectangle(New Pen(Color.FromArgb(11, 11, 11)), New Rectangle(1, 1, Width - 2, Height - 2))
        G.FillRectangle(InnerLGB, New Rectangle(0, 0, Width - 1, Height - 1))
        D.DrawText(G, New Rectangle(0, 0, Width - 8, Height - 1), Text, Font, HorizontalAlignment.Center, ForeColor)
    End Sub
End Class
Public Class CryCheckbox : Inherits ThemedControl
    Public Property Checked As Boolean
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        Checked = Not Checked
        ForeColor = Color.WhiteSmoke
    End Sub
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI Semilight", 11.0F)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.Parent.BackColor)
        Height = 21
        G.SmoothingMode = SmoothingMode.HighQuality

        If Checked Then
            G.FillPath(New SolidBrush(Color.FromArgb(21, 21, 21)), D.RoundRect(New Rectangle(0, 0, Height - 1, Height - 1), 1))
            ForeColor = Color.WhiteSmoke
            Dim ColBlend As New ColorBlend
            Dim ChkLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.Black, Color.Black)
            ColBlend.Colors = New Color() {Pal.ColHighest, Pal.ColDark, Pal.ColMed}
            ColBlend.Positions = New Single() {0.0, 0.4, 1.0}
            ChkLGB.InterpolationColors = ColBlend
            G.FillPath(ChkLGB, D.RoundRect(New Rectangle(1, 1, Height - 3, Height - 3), 1))
        Else
            G.FillPath(New SolidBrush(Pal.ColDark), D.RoundRect(New Rectangle(0, 0, Height - 1, Height - 1), 1))
            ForeColor = Pal.ColHighest
        End If
        D.DrawText(G, New Rectangle(Height + 1, 0, Width, Height), Text, Font, HorizontalAlignment.Left, ForeColor)
    End Sub
End Class
Public Class CryRadiobutton : Inherits ThemedControl
    Public Property Checked As Boolean
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        For Each Cont As Control In Parent.Controls
            If TypeOf Cont Is CryRadiobutton Then
                DirectCast(Cont, CryRadiobutton).Checked = False
                Cont.Invalidate()
            End If
        Next
        Checked = True
    End Sub
    Sub New()
        MyBase.New()

        Font = New Font("Segoe UI Semilight", 11.0F)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.Parent.BackColor)
        Height = 21
        G.SmoothingMode = SmoothingMode.HighQuality

        If Checked Then
            G.FillEllipse(New SolidBrush(Color.FromArgb(21, 21, 21)), New Rectangle(0, 0, Height - 1, Height - 1))
            ForeColor = Color.WhiteSmoke
            Dim ColBlend As New ColorBlend
            Dim ChkLGB As New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.Black, Color.Black)
            ColBlend.Colors = New Color() {Pal.ColHighest, Pal.ColDark, Pal.ColMed}
            ColBlend.Positions = New Single() {0.0, 0.4, 1.0}
            ChkLGB.InterpolationColors = ColBlend
            G.FillEllipse(ChkLGB, New Rectangle(1, 1, Height - 3, Height - 3))
        Else
            G.FillEllipse(New SolidBrush(Pal.ColDark), New Rectangle(0, 0, Height - 1, Height - 1))
            ForeColor = Pal.ColHighest
        End If
        D.DrawText(G, New Rectangle(Height + 1, 0, Width, Height), Text, Font, HorizontalAlignment.Left, ForeColor)
    End Sub
End Class
Public Class CryListbox : Inherits ThemedListControl
    Sub New()
        MyBase.New()
        Font = New Font("Segoe UI Semilight", 12.0F)
        ForeColor = Pal.ColHighest
        DrawMode = DrawMode.OwnerDrawVariable
        BorderStyle = BorderStyle.None
        SetStyle(ControlStyles.UserPaint, True)
    End Sub

    Protected Sub OnItemPaint(ByVal G As Graphics, ByVal i As Integer)
        Dim gr As New Rectangle(0, i * 21, Width - 1, 21)

        If i = SelectedIndex Then
            G.FillRectangle(New SolidBrush(Color.FromArgb(50, Pal.ColHighest)), gr)
        End If

        G.DrawString(Items(i).ToString(), Font, New SolidBrush(ForeColor), gr)
        G.DrawRectangle(New Pen(Color.FromArgb(50, Pal.ColHighest)), gr)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim G As Graphics = e.Graphics
        MyBase.OnPaint(e)
        G.Clear(Me.Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        BorderStyle = BorderStyle.None
        G.FillPath(New SolidBrush(Pal.ColDim), D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))

        Dim x As Integer = 0
        For Each i As Object In Items
            OnItemPaint(G, x)
            x += 1
        Next

        G.DrawPath(New Pen(ForeColor), D.RoundRect(New Rectangle(0, 0, Width - 1, Height - 1), 2))
    End Sub
End Class
#End Region
#Region "Theme Utility Stuff"
Public Class Palette
    Public ColHighest As Color
    Public ColHigh As Color
    Public ColMed As Color
    Public ColDim As Color
    Public ColDark As Color
End Class
Public Enum MouseState As Byte
    None = 0
    Over = 1
    Down = 2
    Block = 3
End Enum
Public Class DrawUtils
    Public Sub DrawText(ByVal G As Graphics, ByVal ContRect As Rectangle, ByVal Text As String, ByVal TFont As Font, ByVal TAlign As HorizontalAlignment, ByVal TColor As Color)
        If String.IsNullOrEmpty(Text) Then Return
        Dim TextSize As Size = G.MeasureString(Text, TFont).ToSize
        Dim CenteredY As Integer = ContRect.Height \ 2 - TextSize.Height \ 2
        Select Case TAlign
            Case HorizontalAlignment.Left
                G.DrawString(Text, TFont, New SolidBrush(TColor), ContRect.X, CenteredY)
            Case HorizontalAlignment.Right
                G.DrawString(Text, TFont, New SolidBrush(TColor), ContRect.X + ContRect.Width - TextSize.Width - 5, CenteredY)
            Case HorizontalAlignment.Center
                G.DrawString(Text, TFont, New SolidBrush(TColor), ContRect.X + 4 + ContRect.Width \ 2 - TextSize.Width \ 2, CenteredY)
        End Select
    End Sub
    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim Path As New GraphicsPath
        Dim ArcRectangleWidth As Integer = Curve * 2
        With Path
            .AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
            .AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
            .AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
            .AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
            .AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        End With
        Return Path
    End Function
End Class
#End Region