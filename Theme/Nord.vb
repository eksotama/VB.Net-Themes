#Region " Namespaces "

Imports System.Drawing.Drawing2D
Imports System.ComponentModel

#End Region

#Region " Helper Methods "

Public Module HelperMethods

    Public GP As GraphicsPath

    Public Enum MouseMode As Byte
        NormalMode
        Hovered
        Pushed
    End Enum

    Public Sub DrawImageFromBase64(ByVal G As Graphics, ByVal Base64Image As String, ByVal Rect As Rectangle)
        Dim IM As Image = Nothing
        With G
            Using ms As New System.IO.MemoryStream(Convert.FromBase64String(Base64Image))
                IM = Image.FromStream(ms) : ms.Close()
            End Using
            .DrawImage(IM, Rect)
        End With
    End Sub

    Public Sub FillRoundedPath(ByVal G As Graphics, ByVal C As Color, ByVal Rect As Rectangle, ByVal Curve As Integer, _
                                 Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                                 Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True)
        With G
            .FillPath(New SolidBrush(C), RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight))
        End With
    End Sub

    Public Sub FillRoundedPath(ByVal G As Graphics, ByVal B As Brush, ByVal Rect As Rectangle, ByVal Curve As Integer, _
                                 Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                                 Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True)
        With G
            .FillPath(B, RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight))
        End With
    End Sub

    Public Sub DrawRoundedPath(ByVal G As Graphics, ByVal C As Color, ByVal Size As Single, ByVal Rect As Rectangle, ByVal Curve As Integer, _
                                 Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                                 Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True)
        With G
            .DrawPath(New Pen(C, Size), RoundRec(Rect, Curve, TopLeft, TopRight, BottomLeft, BottomRight))
        End With
    End Sub

    Public Function Triangle(ByVal Clr As Color, ByVal P1 As Point, ByVal P2 As Point, ByVal P3 As Point) As Point()
        Return New Point() {P1, P2, P3}
    End Function

    Public Function PenRGBColor(ByVal GR As Graphics, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer, ByVal Size As Single) As Pen
        Return New Pen(Color.FromArgb(R, G, B), Size)
    End Function

    Public Function PenHTMlColor(ByVal C_WithoutHash As String, ByVal Size As Single) As Pen
        Return New Pen(GetHTMLColor(C_WithoutHash), Size)
    End Function

    Public Function SolidBrushRGBColor(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer, Optional ByVal A As Integer = 0) As SolidBrush
        Return New SolidBrush(Color.FromArgb(A, R, G, B))
    End Function

    Public Function SolidBrushHTMlColor(ByVal C_WithoutHash As String) As SolidBrush
        Return New SolidBrush(GetHTMLColor(C_WithoutHash))
    End Function

    Public Function GetHTMLColor(ByVal C_WithoutHash As String) As Color
        Return ColorTranslator.FromHtml("#" & C_WithoutHash)
    End Function

    Public Function ColorToHTML(ByVal C As Color) As String
        Return ColorTranslator.ToHtml(C)
    End Function

    Public Sub CentreString(ByVal G As Graphics, ByVal Text As String, ByVal font As Font, ByVal brush As Brush, ByVal Rect As Rectangle)
        G.DrawString(Text, font, brush, New Rectangle(0, Rect.Y + (Rect.Height / 2) - (G.MeasureString(Text, font).Height / 2) + 0, Rect.Width, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Center})
    End Sub

    Public Sub LeftString(ByVal G As Graphics, ByVal Text As String, ByVal font As Font, ByVal brush As Brush, ByVal Rect As Rectangle)
        G.DrawString(Text, font, brush, New Rectangle(4, Rect.Y + (Rect.Height / 2) - (G.MeasureString(Text, font).Height / 2) + 0, Rect.Width, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Near})
    End Sub

    Public Sub RightString(ByVal G As Graphics, ByVal Text As String, ByVal font As Font, ByVal brush As Brush, ByVal Rect As Rectangle)
        G.DrawString(Text, font, brush, New Rectangle(4, Rect.Y + (Rect.Height / 2) - (G.MeasureString(Text, font).Height / 2), Rect.Width - Rect.Height + 10, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Far})
    End Sub


#Region " Round Border "

    ''' <summary>
    ''' Credits : AeonHack
    ''' </summary>

    Public Function RoundRec(ByVal r As Rectangle, ByVal Curve As Integer, _
                                 Optional ByVal TopLeft As Boolean = True, Optional ByVal TopRight As Boolean = True, _
                                 Optional ByVal BottomLeft As Boolean = True, Optional ByVal BottomRight As Boolean = True) As GraphicsPath
        Dim CreateRoundPath As New GraphicsPath(FillMode.Winding)
        If TopLeft Then
            CreateRoundPath.AddArc(r.X, r.Y, Curve, Curve, 180.0F, 90.0F)
        Else
            CreateRoundPath.AddLine(r.X, r.Y, r.X, r.Y)
        End If
        If TopRight Then
            CreateRoundPath.AddArc(r.Right - Curve, r.Y, Curve, Curve, 270.0F, 90.0F)
        Else
            CreateRoundPath.AddLine(r.Right - r.Width, r.Y, r.Width, r.Y)
        End If
        If BottomRight Then
            CreateRoundPath.AddArc(r.Right - Curve, r.Bottom - Curve, Curve, Curve, 0.0F, 90.0F)
        Else
            CreateRoundPath.AddLine(r.Right, r.Bottom, r.Right, r.Bottom)

        End If
        If BottomLeft Then
            CreateRoundPath.AddArc(r.X, r.Bottom - Curve, Curve, Curve, 90.0F, 90.0F)
        Else
            CreateRoundPath.AddLine(r.X, r.Bottom, r.X, r.Bottom)
        End If
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

#End Region



End Module

#End Region

#Region " Form "

Public Class NordTheme : Inherits ContainerControl

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.ContainerControl Or ControlStyles.ResizeRedraw, True)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 13, FontStyle.Bold)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        With e.Graphics

            Dim Rect As New Rectangle(0, 0, Width, Height)

            .FillRectangle(SolidBrushHTMlColor("bbd2d8"), Rect)
            .FillRectangle(SolidBrushHTMlColor("174b7a"), New Rectangle(0, 0, Width, 58))
            .FillRectangle(SolidBrushHTMlColor("164772"), New Rectangle(0, 58, Width, 10))
            .DrawLine(PenHTMlColor("002e5e", 2), New Point(0, 68), New Point(Width, 68))

        End With
    End Sub

#End Region

#Region " Events "

    Protected NotOverridable Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Dock = DockStyle.Fill
        Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " Button "

Public Class NordButtonGreen : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _NormalColor As Color = GetHTMLColor("75b81b")
    Private _NormalBorderColor As Color = GetHTMLColor("83ae48")
    Private _NormalTextColor As Color = Color.White
    Private _HoverColor As Color = GetHTMLColor("8dd42e")
    Private _HoverBorderColor As Color = GetHTMLColor("83ae48")
    Private _HoverTextColor As Color = Color.White
    Private _PushedColor As Color = GetHTMLColor("548710")
    Private _PushedBorderColor As Color = GetHTMLColor("83ae48")
    Private _PushedTextColor As Color = Color.White

#End Region

#Region " Properties "

    <Category(" Custom Properties "),
    Description("Gets or sets the the button color in normal mouse state.")> Public Property NormalColor As Color
        Get
            Return _NormalColor
        End Get
        Set(value As Color)
            _NormalColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the the button border color in normal mouse state.")> Public Property NormalBorderColor As Color
        Get
            Return _NormalBorderColor
        End Get
        Set(value As Color)
            _NormalBorderColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the the button Text color in normal mouse state.")> Public Property NormalTextColor As Color
        Get
            Return _NormalTextColor
        End Get
        Set(value As Color)
            _NormalTextColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the the button color in hover mouse state.")> Public Property HoverColor As Color
        Get
            Return _HoverColor
        End Get
        Set(value As Color)
            _HoverColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the the button border color in hover mouse state.")> Public Property HoverBorderColor As Color
        Get
            Return _HoverBorderColor
        End Get
        Set(value As Color)
            _HoverBorderColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the the button Text color in hover mouse state.")> Public Property HoverTextColor As Color
        Get
            Return _HoverTextColor
        End Get
        Set(value As Color)
            _HoverTextColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the the button color in mouse down state.")>
    Public Property PushedColor As Color
        Get
            Return _PushedColor
        End Get
        Set(value As Color)
            _PushedColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the the button border color in mouse down state.")> Public Property PushedBorderColor As Color
        Get
            Return _PushedBorderColor
        End Get
        Set(value As Color)
            _PushedBorderColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the the button Text color in mouse down state.")> Public Property PushedTextColor As Color
        Get
            Return _PushedTextColor
        End Get
        Set(value As Color)
            _PushedTextColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 12, FontStyle.Bold)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim Rect As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        With G
            .SmoothingMode = SmoothingMode.AntiAlias
            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            Select Case State
                Case MouseMode.NormalMode
                    Dim R As RectangleF = RoundRec(Rect, 5).GetBounds
                    Dim lgb As New LinearGradientBrush(New Rectangle(0, Height - 5, Width - 1, Height - 1), _
                                                       Color.FromArgb(20, 0, 0, 0), Color.FromArgb(20, 0, 0, 0), 90S)
                    FillRoundedPath(G, New SolidBrush(NormalColor), Rect, 5)
                    .SmoothingMode = SmoothingMode.None
                    FillRoundedPath(G, lgb, New Rectangle(0, Height - 5, Width - 1, Height - 5), 5, False, False, True, True)
                    .SmoothingMode = SmoothingMode.AntiAlias
                    DrawRoundedPath(G, NormalBorderColor, 1, Rect, 5)
                    CentreString(G, Text, New Font("Arial", 11, FontStyle.Bold), New SolidBrush(NormalTextColor), Rect)
                Case MouseMode.Hovered
                    Dim lgb As New LinearGradientBrush(New Rectangle(0, Height - 5, Width - 1, Height - 5), _
                                                       Color.FromArgb(20, 0, 0, 0), Color.FromArgb(20, 0, 0, 0), 90S)
                    FillRoundedPath(G, New SolidBrush(HoverColor), Rect, 5)
                    .SmoothingMode = SmoothingMode.None
                    FillRoundedPath(G, lgb, New Rectangle(0, Height - 5, Width - 1, Height - 5), 5, False, False, True, True)
                    .SmoothingMode = SmoothingMode.AntiAlias
                    DrawRoundedPath(G, HoverBorderColor, 1, Rect, 5)
                    CentreString(G, Text, New Font("Arial", 11, FontStyle.Bold), New SolidBrush(HoverTextColor), Rect)
                Case MouseMode.Pushed
                    Dim lgb As New LinearGradientBrush(New Rectangle(0, Height - 5, Width - 1, Height - 5), _
                                           Color.FromArgb(20, 0, 0, 0), Color.FromArgb(20, 0, 0, 0), 90S)
                    FillRoundedPath(G, New SolidBrush(PushedColor), Rect, 5)
                    .SmoothingMode = SmoothingMode.None
                    FillRoundedPath(G, lgb, New Rectangle(0, Height - 5, Width - 1, Height - 5), 5, False, False, True, True)
                    .SmoothingMode = SmoothingMode.AntiAlias
                    DrawRoundedPath(G, PushedBorderColor, 1, Rect, 5)
                    CentreString(G, Text, New Font("Arial", 11, FontStyle.Bold), New SolidBrush(PushedTextColor), Rect)

            End Select

        End With

    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

Public Class NordButtonClear : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _RoundRadius As Integer = 5
    Private _IsEnabled As Boolean = True

#End Region

#Region " Properties "

    <Category(" Custom Properties "),
    Description("Gets or sets a value indicating whether the control can Rounded in corners.")>
    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(value As Integer)
            _RoundRadius = value

            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets a value indicating whether the control can respond to user interaction.")> Public Property IsEnabled As Boolean
        Get
            Return _IsEnabled
        End Get
        Set(value As Boolean)
            Enabled = value
            _IsEnabled = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 12, FontStyle.Bold)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim Rect As Rectangle = New Rectangle(0, 0, Width - 1, Height - 1)
        With G
            .SmoothingMode = SmoothingMode.AntiAlias
            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            If IsEnabled Then
                Select Case State
                    Case MouseMode.NormalMode
                        DrawRoundedPath(G, GetHTMLColor("164772"), 1, Rect, RoundRadius)
                        CentreString(G, Text, New Font("Arial", 11, FontStyle.Regular), SolidBrushHTMlColor("164772"), Rect)
                    Case MouseMode.Hovered
                        FillRoundedPath(G, SolidBrushHTMlColor("eeeeee"), Rect, RoundRadius)
                        DrawRoundedPath(G, GetHTMLColor("d7d7d7"), 1, Rect, RoundRadius)
                        CentreString(G, Text, New Font("Arial", 9, FontStyle.Bold), SolidBrushHTMlColor("d7d7d7"), Rect)
                    Case MouseMode.Pushed
                        FillRoundedPath(G, SolidBrushHTMlColor("f3f3f3"), Rect, RoundRadius)
                        DrawRoundedPath(G, GetHTMLColor("d7d7d7"), 1, Rect, RoundRadius)
                        CentreString(G, Text, New Font("Arial", 9, FontStyle.Bold), SolidBrushHTMlColor("747474"), Rect)

                End Select
            Else

                DrawRoundedPath(G, GetHTMLColor("dadada"), 1, Rect, RoundRadius)
                CentreString(G, Text, New Font("Arial", 9, FontStyle.Bold), SolidBrushHTMlColor("dadada"), Rect)

            End If
        End With
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " Switch "

<DefaultEvent("CheckedChanged")> Public Class NordSwitchBlue : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Protected State As MouseMode = MouseMode.NormalMode
    Private _UnCheckedColor As Color = Color.Black
    Private _CheckedColor As Color = GetHTMLColor("3075bb")
    Private _CheckedBallColor As Color = Color.White
    Private _UnCheckedBallColor As Color = Color.Black

#End Region

#Region " Properties "

    <Category("Appearance")>
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control color while unchecked")>
    Public Property UnCheckedColor As Color
        Get
            Return _UnCheckedColor
        End Get
        Set(value As Color)
            _UnCheckedColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control color while checked")>
    Public Property CheckedColor As Color
        Get
            Return _CheckedColor
        End Get
        Set(value As Color)
            _CheckedColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control ball color while checked")>
    Public Property CheckedBallColor As Color
        Get
            Return _CheckedBallColor
        End Get
        Set(value As Color)
            _CheckedBallColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control ball color while unchecked")>
    Public Property UnCheckedBallColor As Color
        Get
            Return _UnCheckedBallColor
        End Get
        Set(value As Color)
            _UnCheckedBallColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias

            If Checked Then
                FillRoundedPath(e.Graphics, New SolidBrush(CheckedColor), New Rectangle(0, 0, 40, 16), 16)
                .FillEllipse(New SolidBrush(CheckedBallColor), New Rectangle(Width - 14.5, 2.7, 10, 10))
            Else
                DrawRoundedPath(e.Graphics, UnCheckedColor, 1.8, New Rectangle(0, 0, 40, 16), 16)
                .FillEllipse(New SolidBrush(UnCheckedBallColor), New Rectangle(2.7, 2.7, 10, 10))
            End If

        End With
    End Sub

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        BackColor = Color.Transparent
        ForeColor = GetHTMLColor("222222")
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(42, 18)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnMouseHover(e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

#End Region

End Class

<DefaultEvent("CheckedChanged")> Public Class NordSwitchGreen : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Protected State As MouseMode = MouseMode.NormalMode
    Private _UnCheckedColor As Color = GetHTMLColor("dedede")
    Private _CheckedColor As Color = GetHTMLColor("3acf5f")
    Private _CheckedBallColor As Color = Color.White
    Private _UnCheckedBallColor As Color = Color.White

#End Region

#Region " Properties "

    <Category("Appearance")>
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control color while unchecked")>
    Public Property UnCheckedColor As Color
        Get
            Return _UnCheckedColor
        End Get
        Set(value As Color)
            _UnCheckedColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control color while checked")>
    Public Property CheckedColor As Color
        Get
            Return _CheckedColor
        End Get
        Set(value As Color)
            _CheckedColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control ball color while checked")>
    Public Property CheckedBallColor As Color
        Get
            Return _CheckedBallColor
        End Get
        Set(value As Color)
            _CheckedBallColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control ball color while unchecked")>
    Public Property UnCheckedBallColor As Color
        Get
            Return _UnCheckedBallColor
        End Get
        Set(value As Color)
            _UnCheckedBallColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(30, 19)
        Invalidate()
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnMouseHover(e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias
            If Checked Then
                FillRoundedPath(e.Graphics, New SolidBrush(CheckedColor), New Rectangle(0, 1, 28, 16), 16)
                .FillEllipse(New SolidBrush(CheckedBallColor), New Rectangle(Width - 17, 0, 16, 18))
            Else
                FillRoundedPath(e.Graphics, New SolidBrush(UnCheckedColor), New Rectangle(0, 1, 28, 16), 16)
                .FillEllipse(New SolidBrush(UnCheckedBallColor), New Rectangle(0.5, 0, 16, 18))
            End If

        End With
    End Sub

#End Region

End Class

<DefaultEvent("CheckedChanged")> Public Class NordSwitchPower : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Protected State As MouseMode = MouseMode.NormalMode
    Private _UnCheckedColor As Color = GetHTMLColor("103859")
    Private _CheckedColor As Color = GetHTMLColor("103859")
    Private _CheckedBallColor As Color = GetHTMLColor("f1f1f1")
    Private _UnCheckedBallColor As Color = GetHTMLColor("f1f1f1")
    Private _CheckedPowerColor As Color = GetHTMLColor("73ba10")
    Private _UnCheckedPowerColor As Color = GetHTMLColor("c3c3c3")

#End Region

#Region " Properties "

    <Category("Appearance")>
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control color while unchecked")>
    Public Property UnCheckedColor As Color
        Get
            Return _UnCheckedColor
        End Get
        Set(value As Color)
            _UnCheckedColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control color while checked")>
    Public Property CheckedColor As Color
        Get
            Return _CheckedColor
        End Get
        Set(value As Color)
            _CheckedColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control ball color while checked")>
    Public Property CheckedBallColor As Color
        Get
            Return _CheckedBallColor
        End Get
        Set(value As Color)
            _CheckedBallColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control ball color while unchecked")>
    Public Property UnCheckedBallColor As Color
        Get
            Return _UnCheckedBallColor
        End Get
        Set(value As Color)
            _UnCheckedBallColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control power symbol color while checked")>
    Public Property CheckedPowerColor As Color
        Get
            Return _CheckedPowerColor
        End Get
        Set(value As Color)
            _CheckedPowerColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the switch control power symbol color while unchecked")>
    Public Property UnCheckedPowerColor As Color
        Get
            Return _UnCheckedPowerColor
        End Get
        Set(value As Color)
            _UnCheckedPowerColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        With e.Graphics
            .SmoothingMode = SmoothingMode.AntiAlias
            If Checked Then
                FillRoundedPath(e.Graphics, New SolidBrush(CheckedColor), New Rectangle(0, 8, 55, 25), 20)
                .FillEllipse(New SolidBrush(UnCheckedBallColor), New Rectangle(Width - 39, 0, 35, 40))
                .DrawArc(New Pen(CheckedPowerColor, 2), Width - 31, 10, 19, Height - 23, -62, 300)
                .DrawLine(New Pen(CheckedPowerColor, 2), Width - 22, 8, Width - 22, 17)
            Else
                FillRoundedPath(e.Graphics, New SolidBrush(UnCheckedColor), New Rectangle(2, 8, 55, 25), 20)
                .FillEllipse(New SolidBrush(UnCheckedBallColor), New Rectangle(0, 0, 35, 40))
                .DrawArc(New Pen(UnCheckedPowerColor, 2), CInt(7.5), 10, Width - 41, Height - 23, -62, 300)
                .DrawLine(New Pen(UnCheckedPowerColor, 2), 17, 8, 17, 17)
            End If
        End With

    End Sub

#End Region

#Region " Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Size = New Size(60, 44)
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseHover(e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " TabControl "

#Region " Hrizontal TabControl "

Public Class NordHorizontalTabControl : Inherits TabControl

#Region " Variables "

    Private _TabPageColor As Color = GetHTMLColor("bbd2d8")
    Private _TabColor As Color = GetHTMLColor("174b7a")
    Private _TabLowerColor As Color = GetHTMLColor("164772")
    Private _TabSelectedTextColor As Color = Color.White
    Private _TabUnSlectedTextColor As Color = GetHTMLColor("7188ad")

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        Dock = DockStyle.None
        ItemSize = New Size(80, 55)
        Alignment = TabAlignment.Top
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        With G
            .Clear(TabPageColor)
            Cursor = Cursors.Hand

            .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            .FillRectangle(New SolidBrush(TabColor), New Rectangle(0, 0, Width, 60))

            .FillRectangle(New SolidBrush(TabLowerColor), New Rectangle(0, 52, Width, 8))

            For i = 0 To TabCount - 1

                Dim R As Rectangle = GetTabRect(i)

                If SelectedIndex = i Then
                    .DrawString(TabPages(i).Text, New Font("Helvetica CE", 9, FontStyle.Bold), New SolidBrush(TabSelectedTextColor), R.X + 30, R.Y + 20, New StringFormat() With {.Alignment = StringAlignment.Center})
                Else
                    .DrawString(TabPages(i).Text, New Font("Helvetica CE", 9, FontStyle.Bold), New SolidBrush(TabUnSlectedTextColor), R.X + 30, R.Y + 20, New StringFormat() With {.Alignment = StringAlignment.Center})
                End If

            Next
        End With

    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        For Each Tab As TabPage In MyBase.TabPages
            Tab.BackColor = TabPageColor
        Next
    End Sub

#End Region

#Region " Properties "

    <Category("Custom Properties"),
    Description("Gets or sets the tabpages color of the tabcontrol")>
    Public Property TabPageColor As Color
        Get
            Return _TabPageColor
        End Get
        Set(value As Color)
            _TabPageColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the tabcontrol header color")>
    Public Property TabColor As Color
        Get
            Return _TabColor
        End Get
        Set(value As Color)
            _TabColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the tabcontrol line color below the header")>
    Public Property TabLowerColor As Color
        Get
            Return _TabLowerColor
        End Get
        Set(value As Color)
            _TabLowerColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the tabcontrol Text color while unchecked")>
    Public Property TabSelectedTextColor As Color
        Get
            Return _TabSelectedTextColor
        End Get
        Set(value As Color)
            _TabSelectedTextColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the tabcontrol Text color while unchecked")>
    Public Property TabUnSlectedTextColor As Color
        Get
            Return _TabUnSlectedTextColor
        End Get
        Set(value As Color)
            _TabUnSlectedTextColor = value
            Invalidate()
        End Set
    End Property

#End Region

End Class

#End Region

#Region " Vertical TabControl "

Public Class NordVerticalTabControl : Inherits TabControl

#Region " Variables "

    Private _TabColor As Color = GetHTMLColor("f6f6f6")
    Private _TabPageColor As Color = Color.White
    Private _TabSelectedTextColor As Color = GetHTMLColor("174b7a")
    Private _TabUnSlectedTextColor As Color = Color.DarkGray

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        Dock = DockStyle.None
        ItemSize = New Size(35, 135)
        Alignment = TabAlignment.Left
        Font = New Font("Segoe UI", 8)
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    <Category("Custom Properties"),
    Description("Gets or sets the tabcontrol left side color")>
    Public Property TabColor As Color
        Get
            Return _TabColor
        End Get
        Set(value As Color)
            _TabColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the tabpages color of the tabcontrol")>
    Public Property TabPageColor As Color
        Get
            Return _TabPageColor
        End Get
        Set(value As Color)
            _TabPageColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the tabcontrol Text color while selected")>
    Public Property TabSelectedTextColor As Color
        Get
            Return _TabSelectedTextColor
        End Get
        Set(value As Color)
            _TabSelectedTextColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the tabcontrol Text color while not selected")>
    Public Property TabUnSlectedTextColor As Color
        Get
            Return _TabUnSlectedTextColor
        End Get
        Set(value As Color)
            _TabUnSlectedTextColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics

        With G

            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            .FillRectangle(New SolidBrush(TabColor), New Rectangle(0, 0, ItemSize.Height, Height))
            .FillRectangle(New SolidBrush(TabPageColor), New Rectangle(ItemSize.Height, 0, Width, Height))

            For i = 0 To TabCount - 1

                Dim R As Rectangle = GetTabRect(i)
                If SelectedIndex = i Then

                    CentreString(G, TabPages(i).Text, Font, New SolidBrush(TabSelectedTextColor), New Rectangle(R.X, R.Y + 5, R.Width - 4, R.Height))
                Else
                    CentreString(G, TabPages(i).Text, Font, New SolidBrush(TabUnSlectedTextColor), New Rectangle(R.X, R.Y + 5, R.Width - 4, R.Height))

                End If
                If Not ImageList Is Nothing Then

                    .DrawImage(ImageList.Images(i), New Rectangle(R.X + 9, R.Y + 10, 20, 20))

                End If

            Next

        End With

    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        For Each Tab As TabPage In MyBase.TabPages
            Tab.BackColor = TabPageColor
        Next
    End Sub

#End Region

End Class

#End Region

#End Region

#Region " GroupBox "

Public Class NordGroupBox : Inherits ContainerControl

#Region " Variables "

    Private _HeaderColor As Color = GetHTMLColor("f8f8f9")
    Private _HeaderTextColor As Color = GetHTMLColor("dadada")
    Private _BorderColor As Color = GetHTMLColor("eaeaeb")
    Private _BaseColor As Color = Color.White

#End Region

#Region " Properties "

    <Category(" Custom Properties "),
    Description("Gets or sets the Header color for the control.")>
    Public Property HeaderColor As Color
        Get
            Return _HeaderColor
        End Get
        Set(value As Color)
            _HeaderColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the text color for the control.")>
    Public Property HeaderTextColor As Color
        Get
            Return _HeaderTextColor
        End Get
        Set(value As Color)
            _HeaderTextColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the background border color for the control.")>
    Public Property BorderColor As Color
        Get
            Return _BorderColor
        End Get
        Set(value As Color)
            _BorderColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the background color for the control.")>
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        With e.Graphics
            Dim Rect As New Rectangle(0, 0, Width, Height)

            .FillRectangle(New SolidBrush(BaseColor), Rect)
            .FillRectangle(New SolidBrush(HeaderColor), New Rectangle(0, 0, Width, 50))

            .DrawLine(New Pen(BorderColor, 1), New Point(0, 50), New Point(Width, 50))
            .DrawRectangle(New Pen(BorderColor, 1), New Rectangle(0, 0, Width - 1, Height - 1))

            .DrawString(Text, Font, New SolidBrush(HeaderTextColor), New Rectangle(5, 0, Width, 50), New StringFormat() With {.LineAlignment = StringAlignment.Center})

        End With
    End Sub

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Font = New Font("Segoe UI", 10)
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

End Class

#End Region

#Region " TextBox "

Public Class NordTextbox : Inherits Control

#Region " Variables "

    Private WithEvents T As New TextBox
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean = False
    Private _UseSystemPasswordChar As Boolean = False
    Private _WatermarkText As String = String.Empty
    Private _SideImage As Image
    Private TBC As Color = GetHTMLColor("bbd2d8")
    Private TFC As Color = GetHTMLColor("eaeaeb")
    Private State As MouseMode = MouseMode.NormalMode
    Private _BackColor As Color = TBC
    Private _NormalLineColor As Color = GetHTMLColor("eaeaeb")
    Private _HoverLineColor As Color = GetHTMLColor("fc3955")
    Private _PushedLineColor As Color = GetHTMLColor("fc3955")

#End Region

#Region " Native Methods "

    Private Declare Auto Function SendMessage Lib "user32.dll" (hWnd As IntPtr, msg As Integer, wParam As Integer, <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)> lParam As String) As Int32

#End Region

#Region " Properties "

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    ReadOnly Property BorderStyle As BorderStyle
        Get
            Return BorderStyle.None
        End Get
    End Property

    <Category("Appearance"),
    Description("Gets or sets how text is aligned in a System.Windows.Forms.TextBox control.")>
    Public Overridable Shadows Property TextAlign() As HorizontalAlignment
        Get
            Return _TextAlign
        End Get
        Set(ByVal value As HorizontalAlignment)
            _TextAlign = value
            If T IsNot Nothing Then
                T.TextAlign = value
            End If
        End Set
    End Property

    <Category("Behavior"),
     Description("Gets or sets how text is aligned in a System.Windows.Forms.TextBox control.")>
    Public Overridable Shadows Property MaxLength() As Integer
        Get
            Return _MaxLength
        End Get
        Set(ByVal value As Integer)
            _MaxLength = value
            If T IsNot Nothing Then
                T.MaxLength = value
            End If
        End Set
    End Property

    <Category("Appearance"),
     Description("Gets or sets the background color of the control.")>
    Public Shadows Property BackColor As Color
        Get
            Return _BackColor
        End Get
        Set(value As Color)
            MyBase.BackColor = value
            _BackColor = value
            T.BackColor = value
            Invalidate()
        End Set
    End Property

    <Category("Behavior"),
     Description("Gets or sets a value indicating whether text in the text box is read-only.")>
    Public Overridable Shadows Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If T IsNot Nothing Then
                T.ReadOnly = value
            End If
        End Set
    End Property

    <Category("Behavior"),
     Description("Gets or sets a value indicating whether the text in the System.Windows.Forms.TextBox control should appear as the default password character.")>
    Public Overridable Shadows Property UseSystemPasswordChar() As Boolean
        Get
            Return _UseSystemPasswordChar
        End Get
        Set(ByVal value As Boolean)
            _UseSystemPasswordChar = value
            If T IsNot Nothing Then
                T.UseSystemPasswordChar = value
            End If
        End Set
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property Multiline() As Boolean
        Get
            Return False
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property BackgroundImage() As Image
        Get
            Return Nothing
        End Get
    End Property

    <Category("Appearance"),
    Description("Gets or sets the current text in the System.Windows.Forms.TextBox.")>
    Public Overridable Shadows Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal value As String)
            MyBase.Text = value
            If T IsNot Nothing Then
                T.Text = value
            End If
        End Set
    End Property

    <Category("Custom Properties "),
     Description("Gets or sets the text in the System.Windows.Forms.TextBox while being empty.")>
    Public Property WatermarkText As String
        Get
            Return _WatermarkText
        End Get
        Set(value As String)
            _WatermarkText = value
            SendMessage(T.Handle, &H1501, 0, value)
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties "),
     Description("Gets or sets the image of the control.")>
    <Browsable(True)> Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties "),
     Description("Gets or sets the lower line color of the control in normal mouse state.")>
    Public Property NormalLineColor As Color
        Get
            Return _NormalLineColor
        End Get
        Set(value As Color)
            _NormalLineColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties "),
     Description("Gets or sets the lower line color of the control in mouse hover state.")>
    Public Property HoverLineColor As Color
        Get
            Return _HoverLineColor
        End Get
        Set(value As Color)
            _HoverLineColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties "),
    Description("Gets or sets the lower line color of the control in mouse down state.")>
    Public Property PushedLineColor As Color
        Get
            Return _PushedLineColor
        End Get
        Set(value As Color)
            _PushedLineColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                  ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                  ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = TBC
        Font = New Font("Segoe UI", 11, FontStyle.Regular)
        With T
            .Multiline = False
            .Cursor = Cursors.IBeam
            .BackColor = BackColor
            .ForeColor = TFC
            .BorderStyle = BorderStyle.None
            .Location = New Point(7, 8)
            .Font = Font
            .Size = New Size(Width - 10, 30)
            .UseSystemPasswordChar = _UseSystemPasswordChar
        End With
        Size = New Size(135, 30)
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Private Sub T_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles T.TextChanged
        Text = T.Text
    End Sub

    Private Sub T_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles T.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.A Then e.SuppressKeyPress = True
        If e.Control AndAlso e.KeyCode = Keys.C Then
            T.Copy()
            e.SuppressKeyPress = True
        End If
    End Sub

    Protected NotOverridable Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        If Not Controls.Contains(T) Then Controls.Add(T)
    End Sub

    Protected NotOverridable Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 30
    End Sub

    Private Sub T_MouseHover(ByVal sender As Object, e As EventArgs) Handles T.MouseHover
        State = MouseMode.Hovered
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseHover(e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub
    Private Sub T_MouseLeave(ByVal sender As Object, e As EventArgs) Handles T.MouseLeave
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseUp(ByVal sender As Object, e As MouseEventArgs) Handles T.MouseUp
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseEnter(ByVal sender As Object, e As EventArgs) Handles T.MouseEnter
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

    Private Sub T_MouseDown(ByVal sender As Object, e As EventArgs) Handles T.MouseDown
        State = MouseMode.Pushed
        Invalidate()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        With G

            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            Select Case State

                Case MouseMode.NormalMode
                    .DrawLine(New Pen(NormalLineColor, 1), New Point(0, 29), New Point(Width, 29))
                Case MouseMode.Hovered
                    .DrawLine(New Pen(HoverLineColor, 1), New Point(0, 29), New Point(Width, 29))
                Case MouseMode.Pushed
                    .DrawLine(New Pen(PushedLineColor, 1), New Point(0, 29), New Point(Width, 29))

            End Select

            If Not SideImage Is Nothing Then
                T.Location = New Point(33, 5)
                T.Width = Width - 60
                .InterpolationMode = InterpolationMode.HighQualityBicubic
                .DrawImage(SideImage, New Rectangle(8, 5, 16, 16))
            Else
                T.Location = New Point(7, 5)
                T.Width = Width - 10
            End If

            If Not ContextMenuStrip Is Nothing Then T.ContextMenuStrip = ContextMenuStrip

        End With

    End Sub

#End Region

End Class

#End Region

#Region " CheckBox "

Public Class NordCheckBox : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Protected State As MouseMode = MouseMode.NormalMode
    Private _BorderColor As Color = GetHTMLColor("164772")
    Private _CheckColor As Color = GetHTMLColor("5db5e9")

#End Region

#Region " Properties "

    <Category("Appearance")>
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the Checkbox control color while checked.")>
    Property BorderColor As Color
        Get
            Return _BorderColor
        End Get
        Set(value As Color)
            _BorderColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the Checkbox control check symbol color while checked.")>
    Property CheckColor As Color
        Get
            Return _CheckColor
        End Get
        Set(value As Color)
            _CheckColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        BackColor = Color.Transparent
        ForeColor = GetHTMLColor("222222")
        Font = New Font("Segoe UI", 9, FontStyle.Regular)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        G.SmoothingMode = SmoothingMode.AntiAlias
        Using CheckBorder As New GraphicsPath With {.FillMode = FillMode.Winding}
            CheckBorder.AddArc(0, 0, 10, 8, 180, 90)
            CheckBorder.AddArc(8, 0, 8, 10, -90, 90)
            CheckBorder.AddArc(8, 8, 8, 8, 0, 70)
            CheckBorder.AddArc(0, 8, 10, 8, 90, 90)
            CheckBorder.CloseAllFigures()
            G.DrawPath(New Pen(BorderColor, 1.5), CheckBorder)
            If Checked Then
                G.DrawString("b", New Font("Marlett", 13), New SolidBrush(CheckColor), New Rectangle(-2, 0.5, Width, Height))
            End If
        End Using

        G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(18, 1, Width, Height - 4), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

    End Sub

#End Region

#Region " Events "

    Event CheckedChanged(ByVal sender As Object)

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 18
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseHover(e As EventArgs)
        MyBase.OnMouseHover(e)
        State = MouseMode.Hovered
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseMode.NormalMode
        Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " RadioButton "

<DefaultEvent("CheckedChanged")> Public Class NordRadioButton : Inherits Control

#Region " Variables "

    Private _Checked As Boolean
    Protected _Group As Integer = 1
    Protected State As MouseMode = MouseMode.NormalMode
    Private _CheckBorderColor As Color = GetHTMLColor("164772")
    Private _UnCheckBorderColor As Color = Color.Black
    Private _CheckColor As Color = Color.Black

#End Region

#Region " Events "

    Event CheckedChanged(ByVal sender As Object)

#End Region

#Region " Properties "

    <Category("Appearance")>
    Property Checked As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            RaiseEvent CheckedChanged(Me)
            Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    Property Group As Integer
        Get
            Return _Group
        End Get
        Set(ByVal value As Integer)
            _Group = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the Radiobutton control border color while checked.")>
    Property CheckBorderColor As Color
        Get
            Return _CheckBorderColor
        End Get
        Set(value As Color)
            _CheckBorderColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the Radiobutton control border color while unchecked.")>
    Property UnCheckBorderColor As Color
        Get
            Return _UnCheckBorderColor
        End Get
        Set(value As Color)
            _UnCheckBorderColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties"),
    Description("Gets or sets the Radiobutton control check symbol color while checked.")>
    Property CheckColor As Color
        Get
            Return _CheckColor
        End Get
        Set(value As Color)
            _CheckColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
    ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        BackColor = Color.Transparent
        ForeColor = GetHTMLColor("222222")
        Font = New Font("Segoe UI", 9, FontStyle.Regular)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim R As New Rectangle(1, 1, 18, 18)
        With G
            .SmoothingMode = SmoothingMode.HighQuality
            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            If Checked Then
                .FillEllipse(New SolidBrush(CheckColor), New Rectangle(4, 4, 12, 12))
                .DrawEllipse(New Pen(CheckBorderColor, 2), R)
            Else
                .DrawEllipse(New Pen(UnCheckBorderColor, 2), R)
            End If
            .DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(21, 1.5, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
        End With

    End Sub

#End Region

#Region " Events "

    Private Sub UpdateState()
        If Not IsHandleCreated OrElse Not Checked Then Return
        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is NordRadioButton AndAlso DirectCast(C, NordRadioButton).Group = _Group Then
                DirectCast(C, NordRadioButton).Checked = False
            End If
        Next
    End Sub

    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Checked = Not Checked
        UpdateState()
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnCreateControl()
        UpdateState()
        MyBase.OnCreateControl()
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Invalidate() : MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 21
        Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " ComboBox "

Public Class NordComboBox : Inherits ComboBox

#Region " Variables "

    Private _StartIndex As Integer = 0
    Private _BaseColor As Color = GetHTMLColor("bbd2d8")
    Private _LinesColor As Color = GetHTMLColor("75b81b")
    Private _TextColor As Color = Color.Gray

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        BackColor = GetHTMLColor("291a2a")
        Font = New Font("Segoe UI", 12, FontStyle.Regular)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DoubleBuffered = True
        StartIndex = 0
        DropDownHeight = 100
        DropDownStyle = ComboBoxStyle.DropDownList
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    <Category("Behavior"),
    Description("When overridden in a derived class, gets or sets the zero-based index of the currently selected item.")>
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

    <Category(" Custom Properties "),
    Description("Gets or sets the background color for the control.")>
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the lines color for the control.")>
    Public Property LinesColor As Color
        Get
            Return _LinesColor
        End Get
        Set(value As Color)
            _LinesColor = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the text color for the control.")>
    Public Property TextColor As Color
        Get
            Return _TextColor
        End Get
        Set(value As Color)
            _TextColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        Try
            Dim G As Graphics = e.Graphics
            e.DrawBackground()
            With G
                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                .FillRectangle(New SolidBrush(BaseColor), e.Bounds)

                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    Cursor = Cursors.Hand
                    CentreString(G, Items(e.Index), Font, New SolidBrush(TextColor), New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 3, e.Bounds.Width - 2, e.Bounds.Height - 2))
                Else
                    CentreString(G, Items(e.Index), Font, Brushes.DimGray, New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 2, e.Bounds.Width - 2, e.Bounds.Height - 2))
                End If
            End With
        Catch
        End Try
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim Rect As New Rectangle(0, 0, Width, Height - 1)
        With e.Graphics
            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            .FillRectangle(New SolidBrush(BaseColor), Rect)

            .DrawLine(New Pen(LinesColor, 2), New Point(Width - 21, (Height / 2) - 3), New Point(Width - 7, (Height / 2) - 3))
            .DrawLine(New Pen(LinesColor, 2), New Point(Width - 21, (Height / 2) + 1), New Point(Width - 7, (Height / 2) + 1))
            .DrawLine(New Pen(LinesColor, 2), New Point(Width - 21, (Height / 2) + 5), New Point(Width - 7, (Height / 2) + 5))

            .DrawLine(New Pen(LinesColor, 1), New Point(0, Height - 1), New Point(Width, Height - 1))
            .DrawString(Text, Font, New SolidBrush(TextColor), New Rectangle(5, 1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
        End With
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " Seperator "

Public Class NordSeperator : Inherits Control

#Region " Variables "

    Private _SepStyle As Style = Style.Horizental
    Private _SeperatorColor As Color = GetHTMLColor("eaeaeb")

#End Region

#Region " Enumerators "

    Enum Style
        Horizental
        Vertiacal
    End Enum

#End Region

#Region " Properties "

    <Category("Appearance"),
    Description("Gets or sets the style for the control.")>
    Public Property SepStyle As Style
        Get
            Return _SepStyle
        End Get
        Set(value As Style)
            _SepStyle = value
            Invalidate()
        End Set
    End Property

    <Category(" Custom Properties "),
    Description("Gets or sets the color for the control.")>
    Public Property SeperatorColor As Color
        Get
            Return _SeperatorColor
        End Get
        Set(value As Color)
            _SeperatorColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        With G
            Select Case SepStyle
                Case Style.Horizental
                    .DrawLine(New Pen(SeperatorColor), 0, 1, Width, 1)
                Case Style.Vertiacal
                    .DrawLine(New Pen(SeperatorColor), 1, 0, 1, Height)
            End Select
        End With

    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        If SepStyle = Style.Horizental Then
            Height = 4
        Else
            Width = 4
        End If
    End Sub

#End Region

End Class

#End Region

#Region " Label "

<DefaultEvent("TextChanged")> Public Class NordLabel : Inherits Control

#Region " Draw Cotnrol "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        With e.Graphics
            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            .DrawString(Text, Font, New SolidBrush(ForeColor), ClientRectangle)
        End With
    End Sub

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 12, FontStyle.Regular)
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnResize(e As EventArgs)
        Height = Font.Height
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " LinkLabel "

<DefaultEvent("TextChanged")> Public Class NordLinkLabel : Inherits Control

#Region " Variables "

    Private State As MouseMode = MouseMode.NormalMode
    Private _HoverColor As Color = Color.SteelBlue
    Private _PushedColor As Color = Color.DarkBlue

#End Region

#Region " Properties "

    <Category("Custom Properties "),
    Description("Gets or sets the tect color of the control in mouse hover state.")>
    Public Property HoverColor As Color
        Get
            Return _HoverColor
        End Get
        Set(value As Color)
            _HoverColor = value
            Invalidate()
        End Set
    End Property

    <Category("Custom Properties "),
    Description("Gets or sets the text color of the control in mouse down state.")>
    Public Property PushedColor As Color
        Get
            Return _PushedColor
        End Get
        Set(value As Color)
            _PushedColor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        With e.Graphics
            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            Select Case State
                Case 0
                    .DrawString(Text, Font, New SolidBrush(ForeColor), ClientRectangle)
                Case 1
                    Cursor = Cursors.Hand
                    .DrawString(Text, Font, New SolidBrush(HoverColor), ClientRectangle)
                Case 2
                    .DrawString(Text, Font, New SolidBrush(PushedColor), ClientRectangle)
            End Select

        End With
    End Sub

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        ForeColor = Color.MediumBlue
        Font = New Font("Segoe UI", 12, FontStyle.Underline)
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnResize(e As EventArgs)
        Height = Font.Height + 2
    End Sub
    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseMode.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseMode.NormalMode : Invalidate()
    End Sub

#End Region

End Class

#End Region