Imports System.Drawing.Text
Imports System.Drawing.Drawing2D
Imports System.ComponentModel
Imports System.Drawing.Design

''' <summary>
''' Facebook GDI Theme
''' Creator: Xertz (HF)
''' Version: 1.1
''' Date Created: 15/12/2013
''' Date Changed: 15/12/2013
''' UID: 1602992
''' For any bugs / errors, PM me.
''' </summary>
''' <remarks></remarks>

Module DrawHelpers

#Region "Functions"

    Dim Height As Integer
    Dim Width As Integer

    Public Function RoundRec(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        Return P
    End Function

    Public Function RoundRect(x!, y!, w!, h!, Optional r! = 0.3, Optional TL As Boolean = True, Optional TR As Boolean = True, Optional BR As Boolean = True, Optional BL As Boolean = True) As GraphicsPath
        Dim d! = Math.Min(w, h) * r, xw = x + w, yh = y + h
        RoundRect = New GraphicsPath

        With RoundRect
            If TL Then .AddArc(x, y, d, d, 180, 90) Else .AddLine(x, y, x, y)
            If TR Then .AddArc(xw - d, y, d, d, 270, 90) Else .AddLine(xw, y, xw, y)
            If BR Then .AddArc(xw - d, yh - d, d, d, 0, 90) Else .AddLine(xw, yh, xw, yh)
            If BL Then .AddArc(x, yh - d, d, d, 90, 90) Else .AddLine(x, yh, x, yh)

            .CloseFigure()
        End With
    End Function

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum

#End Region

End Module

Public Class FacebookThemeContainer
    Inherits ContainerControl

#Region "Declarations"
    Private _MainColour As Color = Color.FromArgb(252, 252, 252)
    Private _HeaderColour As Color = Color.FromArgb(67, 96, 156)
    Private _BorderColour As Color = Color.DarkGray
    Private _MainBrushColour As New SolidBrush(_MainColour)
    Private _HeaderBrushColour As New SolidBrush(_HeaderColour)
    Private F As New Font("Tahoma", 13, FontStyle.Bold)
    Private Cap As Boolean = False
    Private MoveHeight As Integer = 45
    Private MouseP As Point = New Point(0, 0)
#End Region

#Region "Mouse States"
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Cap = True : MouseP = e.Location
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : Cap = False
    End Sub
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Cap Then
            Parent.Location = MousePosition - MouseP
        End If
    End Sub
#End Region

#Region "Colour Properties"
    <Category("Colours")>
    Public Property BaseColour As Color
        Get
            Return _MainColour
        End Get
        Set(value As Color)
            _MainColour = value
        End Set
    End Property

    <Category("Colours")>
    Public Property HeaderColour As Color
        Get
            Return _HeaderColour
        End Get
        Set(value As Color)
            _HeaderColour = value
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

#Region "Draw Control"
    Sub New()

        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)

        Me.DoubleBuffered = True
        Me.BackColor = _MainColour
        Me.Dock = DockStyle.Fill
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        ParentForm.FormBorderStyle = FormBorderStyle.None
        ParentForm.AllowTransparency = False
        ParentForm.TransparencyKey = Color.Fuchsia
        ParentForm.FindForm.StartPosition = FormStartPosition.CenterScreen
        Dock = DockStyle.Fill
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim G As Graphics
        G = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality
        G.FillRectangle(_HeaderBrushColour, New Rectangle(-1, -1, Me.Width + 1, 45))
        G.DrawLine(New Pen(New SolidBrush(_BorderColour)), New Point(-1, 45), New Point(Me.Width - 1, 45))
        G.DrawRectangle(New Pen(New SolidBrush(_BorderColour)), New Rectangle(0, 0, Width - 1, Height - 1))
        Dim I As Bitmap = Me.ParentForm.Icon.ToBitmap
        Dim IM As Image = I
        Dim FormText As String = Me.ParentForm.Text
        G.TextRenderingHint = TextRenderingHint.AntiAlias
        G.DrawString(FormText, F, New SolidBrush(Color.FromArgb(220, 220, 220)), New Point(43, 11))
        G.DrawImage(IM, New Rectangle(8, 6, 32, 32))
        MyBase.OnPaint(e)
        I.Dispose()
        IM.Dispose()
    End Sub
#End Region

End Class

Public Class FacebookButton
    Inherits Control

#Region "Declarations"
    Private State As MouseState = MouseState.None
    Private _MainColour As Color = Color.FromArgb(70, 98, 158)
    Private _TextColour As Color = Color.FromArgb(255, 255, 255)
    Private _HoverColour As Color = Color.FromArgb(55, 83, 158)
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

#Region "Colour Properties"

    <Category("Colours"), Description("Background Colour Selection")> _
    Public Property BackgroundColour As Color
        Get
            Return _MainColour
        End Get
        Set(value As Color)
            _MainColour = value
        End Set
    End Property

    <Category("Colours"), Description("Text Colour Selection")> _
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property

#End Region

#Region "Draw Control"

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(135, 32)
        BackColor = Color.Transparent
        Font = New Font("Klavika", 9)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim GP, GP1 As New GraphicsPath
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            Select Case State
                Case MouseState.None
                    GP = DrawHelpers.RoundRec(Base, 2)
                    .FillPath(New SolidBrush(_MainColour), GP)
                    .DrawString(Text, Font, New SolidBrush(_TextColour), Base, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                Case MouseState.Over
                    GP = DrawHelpers.RoundRec(Base, 2)
                    .FillPath(New SolidBrush(_HoverColour), GP)
                    .DrawString(Text, Font, New SolidBrush(_TextColour), Base, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                Case MouseState.Down
                    GP = DrawHelpers.RoundRec(Base, 2)
                    .FillPath(New SolidBrush(_HoverColour), GP)
                    .DrawString(Text, Font, New SolidBrush(_TextColour), Base, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                    GP1 = DrawHelpers.RoundRec(New Rectangle(0, 0, Width, Height), 3)
                    .DrawPath(New Pen(New SolidBrush(Color.LightYellow), 2), GP1)
            End Select
        End With
        MyBase.OnPaint(e)
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

#End Region

End Class

<DefaultEvent("TextChanged")>
Public Class FacebookTextBox
    Inherits Control

#Region "Declarations"
    Private State As MouseState = MouseState.None
    Private WithEvents TB As Windows.Forms.TextBox
    Private _BaseColour As Color = Color.FromArgb(255, 255, 255)
    Private _TextColour As Color = Color.FromArgb(50, 50, 50)
    Private _BorderColour As Color = Color.FromArgb(180, 187, 205)
#End Region

#Region "TextBox Properties"

    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
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
    Private _MaxLength As Integer = 32767
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
    Private _ReadOnly As Boolean
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
    Private _UseSystemPasswordChar As Boolean
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
    Private _Multiline As Boolean
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
        TB.Font = New Font("Segoe UI", 10)
        TB.Text = Text
        TB.BackColor = _BaseColour
        TB.ForeColor = _TextColour
        TB.MaxLength = _MaxLength
        TB.Multiline = _Multiline
        TB.ReadOnly = _ReadOnly
        TB.UseSystemPasswordChar = _UseSystemPasswordChar
        TB.BorderStyle = BorderStyle.None
        TB.Location = New Point(5, 5)
        TB.Width = Width - 10
        If _Multiline Then
            TB.Height = Height - 11
        Else
            Height = TB.Height + 11
        End If
        AddHandler TB.TextChanged, AddressOf OnBaseTextChanged
        AddHandler TB.KeyDown, AddressOf OnBaseKeyDown
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            TB.BackColor = _BaseColour
            TB.ForeColor = _TextColour
            .FillRectangle(New SolidBrush(_BaseColour), Base)
            .DrawRectangle(New Pen(New SolidBrush(_BorderColour)), New Rectangle(0, 0, Width, Height))
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class

Public Class FacebookGroupBox
    Inherits ContainerControl

#Region "Declarations"
    Private _MainColour As Color = Color.FromArgb(237, 239, 244)
    Private _HeaderColour As Color = Color.FromArgb(109, 132, 180)
    Private _TextColour As Color = Color.FromArgb(255, 255, 255)
    Private _CircleColour As Color = Color.FromArgb(93, 170, 64)
    Private _BorderColour As Color = Color.FromArgb(14, 44, 109)
    Private _DrawCircle As Boolean = True
#End Region

#Region "Colour & Other Properties"
    <Category("Colours")>
    Public Property MainColour As Color
        Get
            Return _MainColour
        End Get
        Set(value As Color)
            _MainColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property HeaderColour As Color
        Get
            Return _HeaderColour
        End Get
        Set(value As Color)
            _HeaderColour = value
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
    Public Property CircleColour As Color
        Get
            Return _CircleColour
        End Get
        Set(value As Color)
            _CircleColour = value
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
    <Category("Misc")>
    Public Property DrawCircle As Boolean
        Get
            Return _DrawCircle
        End Get
        Set(value As Boolean)
            _DrawCircle = value
        End Set
    End Property
#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
               ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
               ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Size = New Size(160, 110)
        Font = New Font("Segoe UI", 10, FontStyle.Bold)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        Dim Circle As New Rectangle(8, 8, 10, 10)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            .FillRectangle(New SolidBrush(_MainColour), Base)
            .FillRectangle(New SolidBrush(_HeaderColour), New Rectangle(0, 0, Width - 1, 26))
            .DrawRectangle(New Pen(New SolidBrush(_BorderColour)), New Rectangle(0, 0, Width, Height))
            If _DrawCircle Then
                .FillEllipse(New SolidBrush(_CircleColour), Circle)
                .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(23, 4, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
            Else
                .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(5, 4, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
            End If
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class

Public Class FacebookCloseButton
    Inherits Control

#Region "Declarations"
    Private State As MouseState = MouseState.None
    Private x As Integer
    Private _BackColour As Color = Color.FromArgb(67, 96, 156)
#End Region

#Region "Mouse States"

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Down : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.None : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Over : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        x = e.X : Invalidate()
    End Sub

    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        Environment.Exit(0)
    End Sub

#End Region

#Region "Colour Properties"
    <Category("Colors")> _
    Public Property BaseColour As Color
        Get
            Return _BackColour
        End Get
        Set(value As Color)
            _BackColour = value
        End Set
    End Property
#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.White
        Size = New Size(20, 20)
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Font = New Font("Marlett", 20)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
            .SmoothingMode = 2
            .PixelOffsetMode = 2
            .TextRenderingHint = 5
            .Clear(BackColor)
            .FillRectangle(New SolidBrush(_BackColour), Base)
            Select Case State
                Case MouseState.None
                    .DrawString("r", Font, New SolidBrush(Color.FromArgb(211, 218, 233)), New Rectangle(0, 0, Width, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                Case MouseState.Over
                    .DrawString("r", Font, New SolidBrush(Color.FromArgb(151, 158, 172)), New Rectangle(0, 0, Width, Height), New StringFormat With {.LineAlignment = StringAlignment.Near, .Alignment = StringAlignment.Center})
            End Select
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class

Public Class FacebookTabControlVertical
    Inherits TabControl

#Region "Declarations"
    Private _PressedTabColour As Color = Color.FromArgb(200, 215, 237)
    Private _HoverColour As Color = Color.FromArgb(109, 132, 180)
    Private _NormalColour As Color = Color.FromArgb(237, 239, 244)
    Private _BorderColour As Color = Color.FromArgb(139, 162, 210)
    Private _TextColour As Color = Color.FromArgb(58, 66, 73)
    Private HoverIndex As Integer = -1
#End Region

#Region "Colour & Other Properties"
    <Category("Colours")>
    Public Property NormalColour As Color
        Get
            Return _NormalColour
        End Get
        Set(value As Color)
            _NormalColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property HoverColour As Color
        Get
            Return _HoverColour
        End Get
        Set(value As Color)
            _HoverColour = value
        End Set
    End Property
    <Category("Colours")>
    Public Property PressedTabColour As Color
        Get
            Return _PressedTabColour
        End Get
        Set(value As Color)
            _PressedTabColour = value
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
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(value As Color)
            _TextColour = value
        End Set
    End Property
#End Region

#Region "Draw Control"
    Sub New()
        DoubleBuffered = True
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint, True)
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(44, 95)
        Font = New Font("Segoe UI", 9, FontStyle.Regular)
        DrawMode = TabDrawMode.OwnerDrawFixed
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
    End Sub

    Protected Overrides Sub OnControlAdded(ByVal e As ControlEventArgs)
        If TypeOf e.Control Is TabPage Then
            For Each i As TabPage In Me.Controls
                i = New TabPage
            Next
            e.Control.BackColor = Color.FromArgb(255, 255, 255)
        End If
        MyBase.OnControlAdded(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        For I As Integer = 0 To TabPages.Count - 1
            If GetTabRect(I).Contains(e.Location) Then
                HoverIndex = I
                Exit For
            End If
        Next
        Invalidate()
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        HoverIndex = -1
        Invalidate()
        MyBase.OnMouseLeave(e)

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        G.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
        G.SmoothingMode = SmoothingMode.HighQuality
        G.PixelOffsetMode = PixelOffsetMode.HighQuality
        G.Clear(BackColor)
        Try : SelectedTab.BackColor = _NormalColour : Catch : End Try
        With G
            .FillRectangle(New SolidBrush(_NormalColour), New Rectangle(-2, 0, ItemSize.Height + 4, Height + 22))
            For i As Integer = 0 To TabCount - 1
                If i = SelectedIndex Then
                    Dim x2 As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), New Size(GetTabRect(i).Width + 3, GetTabRect(i).Height - 1))
                    .FillRectangle(New SolidBrush(_NormalColour), x2)
                    Dim tabRect As New Rectangle(GetTabRect(i).Location.X - 3, GetTabRect(i).Location.Y + 2, GetTabRect(i).Size.Width + 10, GetTabRect(i).Size.Height - 11)
                    .FillRectangle(New SolidBrush(_PressedTabColour), New Rectangle(tabRect.X + 1, tabRect.Y + 1, tabRect.Width - 1, tabRect.Height - 2))
                    .DrawRectangle(New Pen(_BorderColour), tabRect)
                    .SmoothingMode = SmoothingMode.AntiAlias
                Else
                    Dim x2 As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y - 2), New Size(GetTabRect(i).Width - 1, GetTabRect(i).Height - 11))
                    .FillRectangle(New SolidBrush(_NormalColour), x2)
                    If HoverIndex = i Then
                        Dim x21 As Rectangle = New Rectangle(New Point(GetTabRect(i).Location.X - 2, GetTabRect(i).Location.Y + 2), New Size(GetTabRect(i).Width, GetTabRect(i).Height - 11))
                        .FillRectangle(New SolidBrush(Color.FromArgb(199, 201, 207)), x21)
                    End If
                End If
                Dim tabRect1 As New Rectangle(GetTabRect(i).Location.X + 3, GetTabRect(i).Location.Y + 3, GetTabRect(i).Size.Width - 20, GetTabRect(i).Size.Height - 11)
                .DrawString(TabPages(i).Text, Font, New SolidBrush(_TextColour), tabRect1, New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Center})
                .FillRectangle(New SolidBrush(_NormalColour), New Rectangle(97, 0, Width - 97, Height))
                .DrawLine(New Pen((_BorderColour), 1), New Point(96, 0), New Point(96, Height))
            Next
        End With
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class

Public Class FacebookChatRightBubble
    Inherits Control
#Region "Declarations"
    Private _TextColour As Color = Color.FromArgb(65, 73, 80)
    Private _BorderColour As Color = Color.FromArgb(163, 182, 208)
    Private _BaseColour As Color = Color.FromArgb(214, 231, 254)
    Private _ShowArrow As Boolean = True
    Private _ArrowFixed As Boolean = True
#End Region

#Region "Colour & Other Properties"
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
    <Category("Misc")>
    Public Property ShowArrow As Boolean
        Get
            Return _ShowArrow
        End Get
        Set(value As Boolean)
            _ShowArrow = value
        End Set
    End Property
    <Category("Misc")>
    Public Property ArrowFixed As Boolean
        Get
            Return _ArrowFixed
        End Get
        Set(value As Boolean)
            _ArrowFixed = value
        End Set
    End Property
    '--Need to sort out
#Region ""
    'Private _Multiline As Boolean
    '<Category("Options")>
    'Property Multiline() As Boolean
    '    Get
    '        Return _Multiline
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _Multiline = value
    '        If TB IsNot Nothing Then
    '            TB.Multiline = value

    '            If value Then
    '                TB.Height = Height - 11
    '            Else
    '                Height = TB.Height + 11
    '            End If

    '        End If
    '    End Set
    'End Property
    '<Category("Options")>
    'Overrides Property Text As String
    '    Get
    '        Return MyBase.Text
    '    End Get
    '    Set(ByVal value As String)
    '        MyBase.Text = value
    '        If TB IsNot Nothing Then
    '            TB.Text = value
    '        End If
    '    End Set
    'End Property
    '<Category("Options")>
    'Overrides Property Font As Font
    '    Get
    '        Return MyBase.Font
    '    End Get
    '    Set(ByVal value As Font)
    '        MyBase.Font = value
    '        If TB IsNot Nothing Then
    '            TB.Font = value
    '            TB.Location = New Point(3, 5)
    '            TB.Width = Width - 6

    '            If Not _Multiline Then
    '                Height = TB.Height + 11
    '            End If
    '        End If
    '    End Set
    'End Property
#End Region

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
               ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
               ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(135, 32)
        BackColor = Color.Transparent
        Font = New Font("Klavika", 9)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim GP, GP1 As New GraphicsPath
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            If _ShowArrow Then
                Dim Base As New Rectangle(0, 0, Width - 8, Height - 1)
                Dim BorderBase As New Rectangle(0, 0, Width - 7, Height)
                GP = DrawHelpers.RoundRec(Base, 2)
                GP1 = DrawHelpers.RoundRec(BorderBase, 4)
                .FillPath(New SolidBrush(_BaseColour), GP)
                .DrawPath(New Pen(New SolidBrush(_BorderColour)), GP1)
                .DrawString(Text, Font, New SolidBrush(_TextColour), (New Rectangle(6, 4, Width - 15, Height)))
                If _ArrowFixed Then
                    Dim p() As Point = {New Point(Width - 8, 11), New Point(Width, 17), New Point(Width - 8, 22)}
                    .FillPolygon(New SolidBrush(_BaseColour), p)
                    .DrawPolygon(New Pen(New SolidBrush(_BaseColour)), p)
                    .DrawLine(New Pen(New SolidBrush(_BorderColour)), New Point(Width - 7, 11), New Point(Width, 17))
                    .DrawLine(New Pen(New SolidBrush(_BorderColour)), New Point(Width, 17), New Point(Width - 7, 22))
                Else
                    Dim p() As Point = {New Point(Width - 8, Height - 19), New Point(Width, Height - 25), New Point(Width - 8, Height - 30)}
                    .FillPolygon(New SolidBrush(_BaseColour), p)
                    .DrawPolygon(New Pen(New SolidBrush(_BaseColour)), p)
                    .DrawLine(New Pen(New SolidBrush(_BorderColour)), New Point(Width - 7, Height - 19), New Point(Width, Height - 25))
                    .DrawLine(New Pen(New SolidBrush(_BorderColour)), New Point(Width, Height - 25), New Point(Width - 7, Height - 30))
                End If
            Else
                Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
                Dim BorderBase As New Rectangle(0, 0, Width, Height)
                GP = DrawHelpers.RoundRec(Base, 2)
                GP1 = DrawHelpers.RoundRec(BorderBase, 4)
                .FillPath(New SolidBrush(_BaseColour), GP)
                .DrawPath(New Pen(New SolidBrush(_BorderColour)), GP1)
                .DrawString(Text, Font, New SolidBrush(_TextColour), (New Rectangle(6, 4, Width - 10, Height)))
            End If
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class

Public Class FacebookChatLeftBubble
    Inherits Control

#Region "Declarations"
    Private _TextColour As Color = Color.FromArgb(65, 73, 80)
    Private _BorderColour As Color = Color.FromArgb(198, 198, 198)
    Private _BaseColour As Color = Color.FromArgb(250, 250, 250)
    Private _ShowArrow As Boolean = True
    Private _ArrowFixed As Boolean = True
#End Region

#Region "Colour & Other Properties"
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
    <Category("Misc")>
    Public Property ShowArrow As Boolean
        Get
            Return _ShowArrow
        End Get
        Set(value As Boolean)
            _ShowArrow = value
        End Set
    End Property
    <Category("Misc")>
    Public Property ArrowFixed As Boolean
        Get
            Return _ArrowFixed
        End Get
        Set(value As Boolean)
            _ArrowFixed = value
        End Set
    End Property
    '--Need to sort out
#Region ""
    'Private _Multiline As Boolean
    '<Category("Options")>
    'Property Multiline() As Boolean
    '    Get
    '        Return _Multiline
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _Multiline = value
    '        If TB IsNot Nothing Then
    '            TB.Multiline = value

    '            If value Then
    '                TB.Height = Height - 11
    '            Else
    '                Height = TB.Height + 11
    '            End If

    '        End If
    '    End Set
    'End Property
    '<Category("Options")>
    'Overrides Property Text As String
    '    Get
    '        Return MyBase.Text
    '    End Get
    '    Set(ByVal value As String)
    '        MyBase.Text = value
    '        If TB IsNot Nothing Then
    '            TB.Text = value
    '        End If
    '    End Set
    'End Property
    '<Category("Options")>
    'Overrides Property Font As Font
    '    Get
    '        Return MyBase.Font
    '    End Get
    '    Set(ByVal value As Font)
    '        MyBase.Font = value
    '        If TB IsNot Nothing Then
    '            TB.Font = value
    '            TB.Location = New Point(3, 5)
    '            TB.Width = Width - 6

    '            If Not _Multiline Then
    '                Height = TB.Height + 11
    '            End If
    '        End If
    '    End Set
    'End Property
#End Region

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
               ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
               ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(135, 32)
        BackColor = Color.Transparent
        Font = New Font("Klavika", 9)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim GP, GP1 As New GraphicsPath
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            If _ShowArrow Then
                Dim Base As New Rectangle(7, 0, Width - 7, Height - 1)
                Dim BorderBase As New Rectangle(8, 0, Width - 8, Height)
                GP = DrawHelpers.RoundRec(Base, 2)
                GP1 = DrawHelpers.RoundRec(BorderBase, 4)
                .FillPath(New SolidBrush(_BaseColour), GP)
                .DrawPath(New Pen(New SolidBrush(_BorderColour)), GP1)
                .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(15, 4, Width - 17, Height - 5))
                If _ArrowFixed Then
                    Dim p() As Point = {New Point(9, 11), New Point(0, 17), New Point(9, 22)}
                    .FillPolygon(New SolidBrush(_BaseColour), p)
                    .DrawPolygon(New Pen(New SolidBrush(_BaseColour)), p)
                    .DrawLine(New Pen(New SolidBrush(_BorderColour)), New Point(8, 11), New Point(0, 17))
                    .DrawLine(New Pen(New SolidBrush(_BorderColour)), New Point(0, 17), New Point(8, 22))
                Else
                    Dim p() As Point = {New Point(9, Height - 19), New Point(0, Height - 25), New Point(9, Height - 30)}
                    .FillPolygon(New SolidBrush(_BaseColour), p)
                    .DrawPolygon(New Pen(New SolidBrush(_BaseColour)), p)
                    .DrawLine(New Pen(New SolidBrush(_BorderColour)), New Point(8, Height - 19), New Point(0, Height - 25))
                    .DrawLine(New Pen(New SolidBrush(_BorderColour)), New Point(0, Height - 25), New Point(8, Height - 30))
                End If
            Else
                Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
                Dim BorderBase As New Rectangle(0, 0, Width, Height)
                GP = DrawHelpers.RoundRec(Base, 2)
                GP1 = DrawHelpers.RoundRec(BorderBase, 4)
                .FillPath(New SolidBrush(_BaseColour), GP)
                .DrawPath(New Pen(New SolidBrush(_BorderColour)), GP1)
                .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(6, 4, Width - 17, Height - 5))
            End If
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class

<DefaultEvent("CheckedChanged")>
Public Class FacebookRadioButton
    Inherits Control

#Region "Declarations"
    Private _Checked As Boolean
    Private State As MouseState = MouseState.None
    Private _HighColour As Color = Color.FromArgb(125, 200, 255)
    Private _SecondBorderColour As Color = Color.FromArgb(114, 207, 249)
    Private _CheckedColour As Color = Color.FromArgb(103, 215, 243)
    Private _BorderColour As Color = Color.FromArgb(207, 211, 220)
    Private _BackColour As Color = Color.FromArgb(237, 239, 244)
    Private _TextColour As Color = Color.FromArgb(65, 73, 80)
#End Region

#Region "Colour & Other Properties"

    <Category("Colours")>
    Public Property HighlightColour As Color
        Get
            Return _HighColour
        End Get
        Set(value As Color)
            _HighColour = value
        End Set
    End Property

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
    Public Property SecondCircleColour As Color
        Get
            Return _SecondBorderColour
        End Get
        Set(value As Color)
            _SecondBorderColour = value
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

    Event CheckedChanged(ByVal sender As Object)
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(value As Boolean)
            _Checked = value
            InvalidateControls()
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnClick(e As EventArgs)
        If Not _Checked Then Checked = True
        MyBase.OnClick(e)
    End Sub
    Private Sub InvalidateControls()
        If Not IsHandleCreated OrElse Not _Checked Then Return
        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is FacebookRadioButton Then
                DirectCast(C, FacebookRadioButton).Checked = False
                Invalidate()
            End If
        Next
    End Sub
    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        InvalidateControls()
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
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        Size = New Size(100, 22)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(1, 0, Height - 2, Height - 2)
        Dim Circle As New Rectangle(7, 6, Height - 14, Height - 14)
        Dim SecondBorder As New Rectangle(4, 3, 14, 14)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            .FillEllipse(New SolidBrush(_BackColour), Base)
            .DrawEllipse(New Pen(_BorderColour), Base)
            Select Case State
                Case MouseState.Over
                    .DrawEllipse(New Pen(_HighColour), Base)
                Case MouseState.Down
                    .DrawEllipse(New Pen(_HighColour), Base)
            End Select
            If Checked Then
                .FillEllipse(New SolidBrush(_CheckedColour), Circle)
                .DrawEllipse(New Pen(_HighColour), Base)
                .DrawEllipse(New Pen(_SecondBorderColour), SecondBorder)
            End If
            .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(24, 4, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class

<DefaultEvent("CheckedChanged")>
Public Class FacebookCheckBox
    Inherits Control

#Region "Declarations"
    Private _Checked As Boolean
    Private State As MouseState = MouseState.None
    Private _HighColour As Color = Color.FromArgb(125, 200, 255)
    Private _CheckedColour As Color = Color.FromArgb(103, 215, 243)
    Private _BorderColour As Color = Color.FromArgb(207, 211, 220)
    Private _BackColour As Color = Color.FromArgb(237, 239, 244)
    Private _TextColour As Color = Color.FromArgb(65, 73, 80)
#End Region

#Region "Colour & Other Properties"

    <Category("Colours")>
    Public Property HighlightColour As Color
        Get
            Return _HighColour
        End Get
        Set(value As Color)
            _HighColour = value
        End Set
    End Property

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

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
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
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
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
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        Size = New Size(100, 22)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, 22, 22)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            .FillRectangle(New SolidBrush(_BackColour), Base)
            .DrawRectangle(New Pen(_BorderColour), New Rectangle(1, 1, 20, 20))
            Select Case State
                Case MouseState.Over
                    .DrawRectangle(New Pen(_HighColour), New Rectangle(1, 1, 20, 20))
                Case MouseState.Down
                    .DrawRectangle(New Pen(_HighColour), New Rectangle(1, 1, 20, 20))
            End Select
            If Checked Then
                .FillRectangle(New SolidBrush(_CheckedColour), New Rectangle(3, 3, 16, 16))
                .DrawRectangle(New Pen(_HighColour), New Rectangle(1, 1, 20, 20))
            End If
            .DrawString(Text, Font, New SolidBrush(_TextColour), New Rectangle(24, 4, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class

Public Class FacebookProgressBar
    Inherits Control

#Region "Declarations"
    Private _ProgressColour As Color = Color.LightBlue
    Private _GlowColour As Color = Color.FromArgb(73, 185, 213)
    Private _BorderColour As Color = Color.FromArgb(187, 191, 200)
    Private _BaseColour As Color = Color.FromArgb(237, 237, 237)
    Private _FontColour As Color = Color.FromArgb(50, 50, 50)
    Private _Value As Integer = 0
    Private _Maximum As Integer = 100
#End Region

#Region "Properties"

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
    Public Property GlowColour As Color
        Get
            Return _GlowColour
        End Get
        Set(value As Color)
            _GlowColour = value
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

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 25
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Height = 25
    End Sub

    Public Sub Increment(ByVal Amount As Integer)
        Value += Amount
    End Sub

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(60, 70, 73)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B = New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            Dim ProgVal As Integer = CInt(_Value / _Maximum * (Width - 40))
            Select Case Value
                Case 0
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .DrawLine(New Pen(_BorderColour), New Point(Width - 40, 0), New Point(Width - 40, Height))
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    .DrawRectangle(New Pen(_BorderColour), Base)
                    .DrawString(String.Format("{0}%", _Value), Font, New SolidBrush(_FontColour), New Point(Width - 37, 4))
                Case _Maximum
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    .DrawRectangle(New Pen(_GlowColour), Base)
                    .DrawLine(New Pen(_GlowColour), New Point(Width - 40, 0), New Point(Width - 40, Height))
                    .DrawString(String.Format("{0}%", _Value), Font, New SolidBrush(_FontColour), New Point(Width - 37, 4))
                Case Else
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    .DrawRectangle(New Pen(_BorderColour), Base)
                    .DrawLine(New Pen(_BorderColour), New Point(Width - 40, 0), New Point(Width - 40, Height))
                    .DrawString(String.Format("{0}%", _Value), Font, New SolidBrush(_FontColour), New Point(Width - 37, 4))
            End Select
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

#End Region

End Class

Public Class FacebookComboBox
    Inherits ComboBox

#Region "Declarations"
    Private _StartIndex As Integer = 0
    Private _BorderColour As Color = Color.FromArgb(73, 185, 213)
    Private _BaseColour As Color = Color.White
    Private _FontColour As Color = Color.FromArgb(50, 50, 50)
#End Region

#Region "Properties & Events"

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
    Public Property BaseColour As Color
        Get
            Return _BaseColour
        End Get
        Set(value As Color)
            _BaseColour = value
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

    Public Property StartIndex As Integer
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
        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit
        Dim Rect As New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 1, e.Bounds.Height + 1)
        Try
            With e.Graphics
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    .FillRectangle(Brushes.LightSteelBlue, Rect)
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, New SolidBrush(_FontColour), 1, e.Bounds.Top + 2)
                Else
                    .FillRectangle(New SolidBrush(Color.White), Rect)
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, New SolidBrush(_FontColour), 1, e.Bounds.Top + 2)
                End If
            End With
        Catch
        End Try
        e.DrawFocusRectangle()
        Me.Invalidate()

    End Sub

    Protected Overrides Sub OnTextChanged(e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.Invalidate()
        MyBase.OnMouseClick(e)
    End Sub

    Protected Overrides Sub OnMouseUp(e As System.Windows.Forms.MouseEventArgs)
        MyBase.Invalidate()
        MyBase.OnMouseUp(e)
    End Sub

#End Region

#Region "Draw Control"

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
               ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
               ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DropDownStyle = ComboBoxStyle.DropDownList
        Me.Width = 163
        Font = New Font("Segoe UI", 10)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        With G
            .TextRenderingHint = TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            .Clear(BackColor)
            Try
                Dim Square As New Rectangle(Width - 22, 0, Width, Height)
                .FillRectangle(New SolidBrush(Color.FromArgb(237, 237, 237)), Square)
                .FillRectangle(New SolidBrush(_BaseColour), New Rectangle(0, 0, Width - 22, Height))
                .DrawLine(New Pen(_BorderColour), New Point(Width - 23, 0), New Point(Width - 23, Height))
                Try
                    .DrawString(Text, Font, New SolidBrush(_FontColour), New Rectangle(3, 0, Width - 20, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
                Catch : End Try
                .DrawLine(New Pen(_BorderColour), 0, 0, 0, 0)
                .DrawRectangle(New Pen(_BorderColour), New Rectangle(0, 0, Width, Height))
                Dim P() As Point = {New Point(Width - 18, 9), New Point(Width - 12, 18), New Point(Width - 6, 9)}
                .FillPolygon(New SolidBrush(_BorderColour), P)
                .DrawPolygon(New Pen(Color.Gainsboro), P)
            Catch
            End Try
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub

#End Region


End Class

Public Class Facebook2ndProgressBar
    Inherits Control

#Region "Declarations"
    Private _ProgressColour As Color = Color.FromArgb(109, 131, 179)
    Private _GlowColour As Color = Color.FromArgb(73, 185, 213)
    Private _BorderColour As Color = Color.FromArgb(64, 89, 134)
    Private _BaseColour As Color = Color.FromArgb(237, 237, 237)
    Private _Value As Integer = 0
    Private _Maximum As Integer = 100
#End Region

#Region "Properties"

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
    Public Property GlowColour As Color
        Get
            Return _GlowColour
        End Get
        Set(value As Color)
            _GlowColour = value
        End Set
    End Property
#End Region

#Region "Events"

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 10
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Height = 10
    End Sub

    Public Sub Increment(ByVal Amount As Integer)
        Value += Amount
    End Sub

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(60, 70, 73)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B = New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width, Height)
        With G
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
                    .DrawRectangle(New Pen(_BorderColour, 2), Base)
                   Case Else
                    .FillRectangle(New SolidBrush(_BaseColour), Base)
                    .FillRectangle(New SolidBrush(_ProgressColour), New Rectangle(0, 0, ProgVal - 1, Height))
                    .DrawRectangle(New Pen(_BorderColour, 2), Base)
                     End Select
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region

End Class

Public Class FacebookSeperator
    Inherits Control

#Region "Declarations"
    Private _SeperatorColour As Color = Color.FromArgb(14, 44, 109)
    Private _Alignment As Style = Style.Horizontal
#End Region

#Region "Properties"

    Enum Style
        Horizontal
        Verticle
    End Enum
    <Category("Control")>
    Public Property Alignment As Style
        Get
            Return _Alignment
        End Get
        Set(value As Style)
            _Alignment = value
        End Set
    End Property

    <Category("Colours")>
    Public Property SeperatorColour As Color
        Get
            Return _SeperatorColour
        End Get
        Set(value As Color)
            _SeperatorColour = value
        End Set
    End Property

#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Size = New Size(30, 30)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G = Graphics.FromImage(B)
        Dim Base As New Rectangle(0, 0, Width - 1, Height - 1)
        With G
            .SmoothingMode = SmoothingMode.HighQuality
            .PixelOffsetMode = PixelOffsetMode.HighQuality
            Select Case _Alignment
                Case Style.Horizontal
                    .DrawLine(New Pen(_SeperatorColour, 0.5), New Point(0, Height / 2), New Point(Width, Height / 2))
                Case Style.Verticle
                    .DrawLine(New Pen(_SeperatorColour, 0.5), New Point(Width / 2, 0), New Point(Width / 2, Height))
            End Select
        End With
        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = 7
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
#End Region



End Class