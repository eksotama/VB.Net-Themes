Imports System.Drawing.Drawing2D
Imports System.ComponentModel

' PLEASE DONM'T REMOVE THE CREDITS...

' DynamUI Free (Ported to VB 2010)
' Creator: Chris2k
'          I didn't design this nor do i know who did...
' Released On: 07.01.2014
'

#Region "Theme funcs"
Module ThemeModule

#Region "V"
    Friend G As Graphics, B As Bitmap
    'Friend NearSF As New StringFormat() With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near}
    Friend CenterString As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
#End Region

    Public Sub DrawCross(ByVal Graphics As Graphics, ByVal Bounds As Rectangle, ByVal Fill As LinearGradientBrush, ByVal Size As Integer, Optional ByVal Angle As Integer = 0)
        Using Path As New GraphicsPath()
            Path.AddRectangle(New Rectangle(((Bounds.X + Bounds.Width + (Size \ 2)) \ 2), Bounds.Y, Size, Bounds.Height))
            Path.AddRectangle(New Rectangle(Bounds.X, ((Bounds.Y + Bounds.Height + (Size \ 2)) \ 2), Bounds.Width, Size))
            Path.AddRectangle(New Rectangle(((Bounds.X + Bounds.Width + (Size \ 2)) \ 2), ((Bounds.Y + Bounds.Height + (Size \ 2)) \ 2), Size, Size))
            If (Angle > 0) AndAlso (Angle < 360) Then
                Graphics.TranslateTransform(((Bounds.X + Bounds.Width + Size) \ 2), ((Bounds.Y + Bounds.Height + Size) \ 2))
                Graphics.RotateTransform(CSng(Angle))
                Graphics.TranslateTransform(-((Bounds.X + Bounds.Width + Size) \ 2), -((Bounds.Y + Bounds.Height + Size) \ 2))
            End If
            Graphics.FillPath(Fill, Path)
        End Using
    End Sub
    Public Sub DrawRoundedRectangle(ByVal Graphics As Graphics, ByVal Bounds As Rectangle, ByVal Radius As Integer, ByVal Outline As Pen, ByVal Fill As LinearGradientBrush)
        Dim Stroke As Integer = Convert.ToInt32(Math.Ceiling(Outline.Width))
        Bounds = Rectangle.Inflate(Bounds, -Stroke, -Stroke)
        Outline.EndCap = Outline.StartCap = LineCap.Round
        Using Path As New GraphicsPath()
            Path.AddLine(Bounds.X + Radius, Bounds.Y, Bounds.X + Bounds.Width - (Radius * 2), Bounds.Y)
            Path.AddArc(Bounds.X + Bounds.Width - (Radius * 2), Bounds.Y, Radius * 2, Radius * 2, 270, 90)
            Path.AddLine(Bounds.X + Bounds.Width, Bounds.Y + Radius, Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height - (Radius * 2))
            Path.AddArc(Bounds.X + Bounds.Width - (Radius * 2), Bounds.Y + Bounds.Height - (Radius * 2), Radius * 2, Radius * 2, 0, 90)
            Path.AddLine(Bounds.X + Bounds.Width - (Radius * 2), Bounds.Y + Bounds.Height, Bounds.X + Radius, Bounds.Y + Bounds.Height)
            Path.AddArc(Bounds.X, Bounds.Y + Bounds.Height - (Radius * 2), Radius * 2, Radius * 2, 90, 90)
            Path.AddLine(Bounds.X, Bounds.Y + Bounds.Height - (Radius * 2), Bounds.X, Bounds.Y + Radius)
            Path.AddArc(Bounds.X, Bounds.Y, Radius * 2, Radius * 2, 180, 90)
            Path.CloseFigure()
            Graphics.FillPath(Fill, Path)
            Graphics.DrawPath(Outline, Path)
        End Using
    End Sub
End Module

Public Class Gradient
    Public Property Color1() As Color
        Get
            Return m_Color1
        End Get
        Set(ByVal value As Color)
            m_Color1 = value
        End Set
    End Property
    Private m_Color1 As Color
    Public Property Color2() As Color
        Get
            Return m_Color2
        End Get
        Set(ByVal value As Color)
            m_Color2 = value
        End Set
    End Property
    Private m_Color2 As Color

    Public Sub New(ByVal Color1 As Color, ByVal Color2 As Color)
        Me.Color1 = Color1
        Me.Color2 = Color2
    End Sub
End Class
#End Region

Class DynamUIThemeContainer
    Inherits ThemeContainer154

    Private _TitleColour As Color = Color.FromArgb(50, 153, 187)
    Public Property TitleColour As Color
        Get
            Return _TitleColour
        End Get
        Set(ByVal value As Color)
            _TitleColour = value
        End Set
    End Property

    <Category("Options")>
    Private _Font As Font
    Overrides Property Font As Font
        Get
            Return _Font
        End Get
        Set(ByVal value As Font)
            _Font = value
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
             ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        Font = New Font("Verdana", 12, FontStyle.Bold)
        Dock = DockStyle.Fill
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Private W, H As Integer
    Private LGB As LinearGradientBrush

    Protected Overrides Sub PaintHook()
        G.Clear(Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        W = Width : H = Height

        Dim Rect As Rectangle = New Rectangle(0, 0, W, H)
        LGB = New LinearGradientBrush(Rect, Color.FromArgb(237, 237, 237), Color.FromArgb(237, 237, 237), 90.0F)

        DrawRoundedRectangle(G, New Rectangle(0, 0, W, H), 5, New Pen(Color.Gray, 1), LGB)
        G.DrawString(Text, Font, New SolidBrush(TitleColour), New Point(20, 6))

        Dim R As Rectangle = New Rectangle(21, 30, W - 45, 1)
        G.FillRectangle(New SolidBrush(Color.FromArgb(128, 128, 128)), R)

        'G.Dispose()
    End Sub
End Class

Class DynamUIControlButton
    Inherits ThemeControl154

    Enum Button As Byte
        None = 0
        Minimize = 1
        MaximizeRestore = 2
        Close = 3
    End Enum

    Private _ControlButton As Button = Button.Close
    Public Property ControlButton() As Button
        Get
            Return _ControlButton
        End Get
        Set(ByVal value As Button)
            _ControlButton = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Width = 18
        Height = 18
        MinimumSize = Size
        MaximumSize = Size
        Margin = New Padding(0)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Parent.BackColor)
        'G.SmoothingMode = SmoothingMode.HighQuality

        Select Case _ControlButton
            Case Button.Minimize
                'DrawMinimize(3, 10)
            Case Button.MaximizeRestore
                If FindForm().WindowState = FormWindowState.Normal Then
                    '0DrawMaximize(3, 5)
                Else
                    'DrawRestore(3, 4)
                End If
            Case Button.Close
                DrawCross(G, New Rectangle(3, 3, (Width - 5), (Height - 5)), New LinearGradientBrush(New Point((Width \ 2), 0), New Point((Width \ 2), Height), Color.LightBlue, Color.DarkBlue), 3, 45)
        End Select

        'G.Dispose()
    End Sub
End Class

<DefaultEvent("CheckedChanged")> _
Class DynamUISwitch
    Inherits ThemeControl154

    Event CheckedChanged(ByVal sender As Object)
    Private _Switched As Boolean
    Public Property Switched() As Boolean
        Get
            Return _Switched
        End Get
        Set(ByVal value As Boolean)
            _Switched = value
            Invalidate()
        End Set
    End Property

    Private _OnColor As Gradient, _OffColor As Gradient
    Private Property OnColor() As Gradient
        Get
            Return _OnColor
        End Get
        Set(ByVal value As Gradient)
            _OnColor = value
            Invalidate()
        End Set
    End Property
    Private Property OffColor() As Gradient
        Get
            Return _OffColor
        End Get
        Set(ByVal value As Gradient)
            _OffColor = value
            Invalidate()
        End Set
    End Property

    <Category("On Color")> _
    Public Property OnColor1() As Color
        Get
            Return OnColor.Color1
        End Get
        Set(ByVal value As Color)
            OnColor.Color1 = value
            Invalidate()
        End Set
    End Property
    <Category("On Color")> _
    Public Property OnColor2() As Color
        Get
            Return OnColor.Color2
        End Get
        Set(ByVal value As Color)
            OnColor.Color2 = value
            Invalidate()
        End Set
    End Property

    <Category("Off Color")> _
    Public Property OffColor1() As Color
        Get
            Return OffColor.Color1
        End Get
        Set(ByVal value As Color)
            OffColor.Color1 = value
            Invalidate()
        End Set
    End Property
    <Category("Off Color")> _
    Public Property OffColor2() As Color
        Get
            Return OffColor.Color2
        End Get
        Set(ByVal value As Color)
            OffColor.Color2 = value
            Invalidate()
        End Set
    End Property

    Private _Radius As Integer
    Public Property Radius() As Integer
        Get
            Return _Radius
        End Get
        Set(ByVal value As Integer)
            _Radius = value
            Invalidate()
        End Set
    End Property

    Public Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Radius = 6
        Cursor = Cursors.Hand
        OnColor = New Gradient(Color.FromArgb(129, 200, 56), Color.FromArgb(167, 219, 96))
        OffColor = New Gradient(Color.FromArgb(221, 35, 35), Color.FromArgb(234, 70, 70))
        Size = New Size(60, 18)
        LockHeight = 18
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Private GB1, GB2 As LinearGradientBrush
    Private R1, R2 As Rectangle

    Protected Overrides Sub PaintHook()
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        R1 = New Rectangle(1, 1, Width - 6, Height - 1) ' Main Rectangle

        If _Switched Then
            R2 = New Rectangle(39, 1, 16, 16)
            DrawRoundedRectangle(G, R1, Radius, New Pen(Color.Gray, 1), New LinearGradientBrush(New Point((Width / 2), 0), New Point((Width / 2), Height), OnColor1, OnColor2))
            G.FillEllipse(New SolidBrush(Color.LightGray), R2)
            G.DrawEllipse(Pens.Gray, R2)
            G.DrawString("On", New Font("Verdana", 7, FontStyle.Bold), New SolidBrush(Color.White), New PointF(8, 3.55))
        Else
            R2 = New Rectangle(0, 1, 16, 16)
            DrawRoundedRectangle(G, R1, Radius, New Pen(Color.LightGray, 1), New LinearGradientBrush(New Point((Width / 2), 0), New Point((Width / 2), Height), OffColor1, OffColor2))
            G.FillEllipse(New SolidBrush(Color.LightGray), R2)
            G.DrawEllipse(Pens.Gray, R2)
            G.DrawString("Off", New Font("Verdana", 7, FontStyle.Bold), New SolidBrush(Color.White), New PointF(22, 3.55))
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        If _Switched Then
            _Switched = False
        Else
            _Switched = True
        End If

        RaiseEvent CheckedChanged(Me)
    End Sub
End Class

<DefaultEvent("CheckedChanged")>
Class DynamUICheckbox
    Inherits ThemeControl154

    Event CheckedChanged(ByVal sender As Object)
    Private _Checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Public Enum CheckedStyleE
        Blop
        Cross
        Tick
    End Enum
    Private _CheckedStyle As CheckedStyleE
    Public Property CheckedStyle() As CheckedStyleE
        Get
            Return _CheckedStyle
        End Get
        Set(ByVal value As CheckedStyleE)
            _CheckedStyle = value
            Invalidate()
        End Set
    End Property

    Private _BoxColor As Gradient, _CheckedColor As Gradient
    Private Property BoxColor() As Gradient
        Get
            Return _BoxColor
        End Get
        Set(ByVal value As Gradient)
            _BoxColor = value
            Invalidate()
        End Set
    End Property
    Private Property CheckedColor() As Gradient
        Get
            Return _CheckedColor
        End Get
        Set(ByVal value As Gradient)
            _CheckedColor = value
            Invalidate()
        End Set
    End Property

    <Category("Background Color")> _
    Public Property BackColor1() As Color
        Get
            Return BoxColor.Color1
        End Get
        Set(ByVal value As Color)
            BoxColor.Color1 = value
            Invalidate()
        End Set
    End Property
    <Category("Background Color")> _
    Public Property BackColor2() As Color
        Get
            Return BoxColor.Color2
        End Get
        Set(ByVal value As Color)
            BoxColor.Color2 = value
            Invalidate()
        End Set
    End Property

    <Category("CheckedColor Color")> _
    Public Property CheckedColor1() As Color
        Get
            Return CheckedColor.Color1
        End Get
        Set(ByVal value As Color)
            CheckedColor.Color1 = value
            Invalidate()
        End Set
    End Property
    <Category("CheckedColor Color")> _
    Public Property CheckedColor2() As Color
        Get
            Return CheckedColor.Color2
        End Get
        Set(ByVal value As Color)
            CheckedColor.Color2 = value
            Invalidate()
        End Set
    End Property

    Public Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(18, 18)
        MinimumSize = Size
        MaximumSize = Size
        CheckedStyle = CheckedStyleE.Blop
        BoxColor = New Gradient(Color.FromArgb(220, 220, 220), Color.FromArgb(180, 180, 180))
        CheckedColor = New Gradient(Color.FromArgb(100, 100, 100), Color.FromArgb(60, 60, 60))
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality

        DrawRoundedRectangle(G, New Rectangle(0, 0, Width, Height), 3, New Pen(Color.Gray, 1), New LinearGradientBrush(New Point((Width \ 2), 0), New Point((Width \ 2), Height), BackColor1, BackColor2))
        If Checked Then
            If CheckedStyle = CheckedStyleE.Blop Then
                DrawRoundedRectangle(G, New Rectangle(2, 2, (Width - 4), (Height - 4)), 3, New Pen(Color.Gray, 1), New LinearGradientBrush(New Point((Width \ 2), 0), New Point((Width \ 2), Height), CheckedColor1, CheckedColor2))
            ElseIf CheckedStyle = CheckedStyleE.Cross Then
                DrawCross(G, New Rectangle(3, 3, (Width - 5), (Height - 5)), New LinearGradientBrush(New Point((Width \ 2), 0), New Point((Width \ 2), Height), CheckedColor1, CheckedColor2), 3, 45)
            ElseIf CheckedStyle = CheckedStyleE.Tick Then
                Dim c As Rectangle = New Rectangle(3, 2, Width - 7, Height - 7)
                Using CheckPen As New Pen(Brushes.DimGray, 2)
                    G.DrawLine(CheckPen, New Point(c.X, c.Y + c.Height / 2), New Point(c.X + c.Width / 2, c.Y + c.Height))
                    G.DrawLine(CheckPen, New Point(c.X + c.Width / 2, c.Y + c.Height), New Point(c.X + c.Width, c.Y))
                End Using
            End If
        End If
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        If _Checked Then
            _Checked = False
        Else
            _Checked = True
        End If
        RaiseEvent CheckedChanged(Me)
    End Sub
End Class

<DefaultEvent("CheckedChanged")> _
Class DynamUIRadioButton
    Inherits ThemeControl154

    Event CheckedChanged(ByVal sender As Object)
    Private _Checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property

    Private _CheckedColour As Color
    <Category("CheckedColor Color")> _
    Public Property CheckedColour As Color
        Get
            Return _CheckedColour
        End Get
        Set(ByVal value As Color)
            _CheckedColour = value
        End Set
    End Property

    Private _CheckboxBorderColour As Color
    <Category("CheckedColor Color")> _
    Public Property CheckboxBorderColour As Color
        Get
            Return _CheckboxBorderColour
        End Get
        Set(ByVal value As Color)
            _CheckboxBorderColour = value
        End Set
    End Property

    Protected Overrides Sub ColorHook()
    End Sub

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.FromArgb(237, 237, 237)
        Size = New Size(160, 18)
        LockHeight = 18
        CheckedColour = Color.DimGray
        CheckboxBorderColour = Color.DimGray
    End Sub

    Private R1 As Rectangle
    Private Mark As Rectangle

    Protected Overrides Sub PaintHook()
        G.Clear(Parent.BackColor)

        R1 = New Rectangle(0, 0, Height - 1, Height - 1)
        Mark = New Rectangle(5, 5, R1.Width - 10, R1.Height - 10)

        If _Checked Then
            G.FillEllipse(New SolidBrush(Color.White), R1)
            G.DrawEllipse(New Pen(_CheckboxBorderColour), R1)
            G.FillEllipse(New SolidBrush(_CheckedColour), Mark)
            G.DrawEllipse(New Pen(_CheckboxBorderColour), Mark)
        Else
            G.FillEllipse(New SolidBrush(Color.White), R1)
            G.DrawEllipse(New Pen(_CheckboxBorderColour), R1)
        End If

        G.DrawString(Text, Font, New SolidBrush(Color.Black), New Rectangle(24, 3, Width, Height), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)

        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is DynamUIRadioButton Then
                DirectCast(C, DynamUIRadioButton).Checked = False
            End If
        Next

        If _checked Then
            _checked = False
        Else
            _checked = True
        End If
        RaiseEvent CheckedChanged(Me)
    End Sub
End Class

Class DynamUIButton
    Inherits ThemeControl154

    Private Property Hovering() As Boolean
        Get
            Return m_Hovering
        End Get
        Set(ByVal value As Boolean)
            m_Hovering = value
        End Set
    End Property

    Private m_Hovering As Boolean

    Protected Overrides Sub OnMouseHover(ByVal e As EventArgs)
        Hovering = True
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        Hovering = True
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        Hovering = False
        Invalidate()
    End Sub

    Private _StandardColor As Gradient, _HoverColor As Gradient
    Private Property StandardColor() As Gradient
        Get
            Return _StandardColor
        End Get
        Set(ByVal value As Gradient)
            _StandardColor = value
            Invalidate()
        End Set
    End Property
    Private Property HoverColor() As Gradient
        Get
            Return _HoverColor
        End Get
        Set(ByVal value As Gradient)
            _HoverColor = value
            Invalidate()
        End Set
    End Property

    <Category("Standard Color")> _
    Public Property StandardColor1() As Color
        Get
            Return StandardColor.Color1
        End Get
        Set(ByVal value As Color)
            StandardColor.Color1 = value
            Invalidate()
        End Set
    End Property
    <Category("Standard Color")> _
    Public Property StandardColor2() As Color
        Get
            Return StandardColor.Color2
        End Get
        Set(ByVal value As Color)
            StandardColor.Color2 = value
            Invalidate()
        End Set
    End Property

    <Category("Hover Color")> _
    Public Property HoverColor1() As Color
        Get
            Return HoverColor.Color1
        End Get
        Set(ByVal value As Color)
            HoverColor.Color1 = value
            Invalidate()
        End Set
    End Property
    <Category("Hover Color")> _
    Public Property HoverColor2() As Color
        Get
            Return HoverColor.Color2
        End Get
        Set(ByVal value As Color)
            HoverColor.Color2 = value
            Invalidate()
        End Set
    End Property

    Private _Radius As Integer = 5
    <Description("The smoothness of the edges of the button.")> _
    Public Property Radius() As Integer
        Get
            Return _Radius
        End Get
        Set(ByVal value As Integer)
            _Radius = value
            Invalidate()
        End Set
    End Property

    Private _StrokeColor As Color, _TextColor As Color
    Public Property StrokeColor() As Color
        Get
            Return _StrokeColor
        End Get
        Set(ByVal value As Color)
            _StrokeColor = value
            Invalidate()
        End Set
    End Property
    Public Property TextColor() As Color
        Get
            Return _TextColor
        End Get
        Set(ByVal value As Color)
            _TextColor = value
            Invalidate()
        End Set
    End Property

    Private _TextAlignHorizontal As StringAlignment, _TextAlignVertical As StringAlignment
    Public Property TextAlignHorizontal() As StringAlignment
        Get
            Return _TextAlignHorizontal
        End Get
        Set(ByVal value As StringAlignment)
            _TextAlignHorizontal = value
            Invalidate()
        End Set
    End Property
    Public Property TextAlignVertical() As StringAlignment
        Get
            Return _TextAlignVertical
        End Get
        Set(ByVal value As StringAlignment)
            _TextAlignVertical = value
            Invalidate()
        End Set
    End Property

    Public Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(140, 30)
        Font = New Font("Verdana", 8, FontStyle.Bold)
        StandardColor = New Gradient(Color.FromArgb(240, 240, 240), Color.FromArgb(220, 220, 220))
        HoverColor = New Gradient(Color.FromArgb(210, 210, 210), Color.FromArgb(230, 230, 230))
        StrokeColor = Color.DimGray
        TextColor = Color.FromArgb(74, 74, 74)
        TextAlignHorizontal = StringAlignment.Center
        TextAlignVertical = StringAlignment.Center
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Parent.BackColor)
        If Not Hovering Then
            DrawRoundedRectangle(G, New Rectangle(0, 0, Width, Height), Radius, New Pen(StrokeColor, 1), New LinearGradientBrush(New Point((Width \ 2), 0), New Point((Width \ 2), Height), StandardColor.Color1, StandardColor.Color2))
        Else
            DrawRoundedRectangle(G, New Rectangle(0, 0, Width, Height), Radius, New Pen(StrokeColor, 1), New LinearGradientBrush(New Point((Width \ 2), 0), New Point((Width \ 2), Height), HoverColor.Color1, HoverColor.Color2))
        End If

        Dim SF As New StringFormat()
        SF.Alignment = TextAlignHorizontal
        SF.LineAlignment = TextAlignVertical

        If Hovering Then
            G.DrawString(Text, Font, New SolidBrush(TextColor), New PointF((Width \ 2), (Height \ 2 + 1)), SF)
        Else
            G.DrawString(Text, Font, New SolidBrush(TextColor), New PointF((Width \ 2), (Height \ 2 + 1)), SF)
        End If
    End Sub
End Class

<DefaultEvent("TextChanged")> _
Class DynamUITextbox
    Inherits ThemeControl154

#Region "Textbox Properties"
    Private _TextAlign As HorizontalAlignment
    Public Property TextAlign() As HorizontalAlignment
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
    Public Property MaxLength() As Integer
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
    Public Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If Base IsNot Nothing Then
                Base.[ReadOnly] = value
            End If
        End Set
    End Property
    Private _UseSystemPasswordChar As Boolean
    Public Property UseSystemPasswordChar() As Boolean
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
    Public Property Multiline() As Boolean
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
    Public Overrides Property Text() As String
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
    Public Overrides Property Font() As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If Base IsNot Nothing Then
                Base.Font = value
                Base.Location = New Point(10, 6)
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
#End Region

    Private Base As TextBox
    Public Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Base = New TextBox()

        Base.Font = New Font("Verdana", 8)
        Base.Text = Text
        Base.MaxLength = _MaxLength
        Base.Multiline = _Multiline
        Base.[ReadOnly] = _ReadOnly
        Base.UseSystemPasswordChar = _UseSystemPasswordChar
        Base.Size = New Size(100, 25)
        Size = New Size(112, 25)
        Base.BorderStyle = BorderStyle.None

        Base.Location = New Point(10, 6)
        Base.Width = Width - 10

        If _Multiline Then
            Base.Height = Height - 11
        Else
            LockHeight = Base.Height + 11
        End If

        AddHandler Base.TextChanged, AddressOf OnBaseTextChanged
        AddHandler Base.KeyDown, AddressOf OnBaseKeyDown
    End Sub

#Region "Colours"
    Private _BaseColour As Color = Color.FromArgb(240, 240, 240)
    Private _BorderColour As Color = Color.FromArgb(183, 183, 183)
    Private _TextColour As Color = Color.FromArgb(128, 128, 128)
#End Region

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Parent.BackColor)
        G.SmoothingMode = SmoothingMode.HighQuality
        G.PixelOffsetMode = PixelOffsetMode.HighQuality

        Base.Size = New Size(Width - 10, Height - 10)

        Base.BackColor = _BaseColour
        Base.ForeColor = _TextColour

        G.FillRectangle(New SolidBrush(_BaseColour), New Rectangle(1, 1, Width - 2, Height - 2))
        DrawBorders(New Pen(New SolidBrush(_BorderColour)), 1)
        'DrawBorders(New Pen(New SolidBrush(i)))
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

Class DynamUICombobox
    Inherits ComboBox

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                 ControlStyles.OptimizedDoubleBuffer Or _
                 ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        SetStyle(DirectCast(139286, ControlStyles), True)
        BackColor = Color.FromArgb(237, 237, 237)
        MyBase.CreateHandle()
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DropDownStyle = ComboBoxStyle.DropDownList
        DoubleBuffered = True
        Size = New Size(180, 18)
        ItemHeight = 15
    End Sub

    Private R As Rectangle
    Private LGB As LinearGradientBrush
    Private Path As GraphicsPath
    Private T, B As Point()
    Private CB As ColorBlend

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Dim G As Graphics = e.Graphics
        Dim _font As New Font("Verdana", 8)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Parent.BackColor)

        R = New Rectangle(0, 0, Width - 1, Height - 1)
        LGB = New LinearGradientBrush(R, Color.FromArgb(250, 250, 250), Color.FromArgb(235, 240, 245), 90.0F)
        Path = RoundRect(R, 6)
        G.FillPath(LGB, Path)

        Dim Pen As New Pen(New LinearGradientBrush(R, Color.FromArgb(220, 220, 220), Color.FromArgb(160, 165, 170), 90.0F))
        G.DrawPath(Pen, Path)

        T = New Point() {New Point(Width - 15, 6), New Point(Width - 19, 11), New Point(Width - 11, 11)}
        G.FillPolygon(Brushes.DimGray, T)

        B = New Point() {New Point(Width - 15, 19), New Point(Width - 19, 14), New Point(Width - 11, 14)}
        G.FillPolygon(Brushes.DimGray, B)

        CB = New ColorBlend(3)
        CB.Colors(0) = Color.Transparent
        CB.Colors(1) = Color.FromArgb(200, 200, 200)
        CB.Colors(2) = Color.Transparent
        CB.Positions = New Single() {0.0, 0.5, 1.0}

        LGB = New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 1), Color.Black, Color.Black)
        LGB.InterpolationColors = CB
        G.DrawLine(New Pen(LGB), New Point(Width - 27, 0), New Point(Width - 27, Height - 1))

        Try
            If Items.Count > 0 Then
                If Not SelectedIndex = -1 Then
                    Dim textX As Integer = 6
                    Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Items(SelectedIndex), _font).Height / 2) + 1
                    G.DrawString(Items(SelectedIndex), _font, Brushes.DimGray, New Point(textX, textY))
                Else
                    Dim textX As Integer = 6
                    Dim textY As Integer = ((Me.Height - 1) / 2) - (G.MeasureString(Items(0), _font).Height / 2) + 1
                    G.DrawString(Items(0), _font, Brushes.DimGray, New Point(textX, textY))
                End If
            End If
        Catch : End Try
    End Sub

    Sub ReplaceItem(ByVal sender As Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()

        Dim G As Graphics = e.Graphics
        G.SmoothingMode = SmoothingMode.HighQuality

        Dim _font As New Font("Verdana", 8)

        Try
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                LGB = New LinearGradientBrush(e.Bounds, Color.FromArgb(180, 200, 215), Color.FromArgb(160, 180, 205), 90.0F)
                G.FillRectangle(LGB, New Rectangle(e.Bounds.X - 1, e.Bounds.Y - 1, e.Bounds.Width + 2, e.Bounds.Height + 2))
                G.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), _font, Brushes.White, New Rectangle(e.Bounds.X + 6, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height))
            Else
                G.FillRectangle(New SolidBrush(Color.FromArgb(235, 240, 245)), e.Bounds)
                G.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), _font, Brushes.Black, New Rectangle(e.Bounds.X + 6, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height))
            End If

        Catch : End Try
    End Sub

    Protected Overrides Sub OnSelectedItemChanged(ByVal e As System.EventArgs)
        MyBase.OnSelectedItemChanged(e)
        Invalidate()
    End Sub

    Public Function RoundRect(ByVal r As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(r.X, r.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(r.Width - ArcRectangleWidth + r.X, r.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(r.Width - ArcRectangleWidth + r.X, r.Height - ArcRectangleWidth + r.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(r.X, r.Height - ArcRectangleWidth + r.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(r.X, r.Height - ArcRectangleWidth + r.Y), New Point(r.X, Curve + r.Y))
        P.CloseAllFigures()
        Return P
    End Function
End Class

Class DynamUIProgressBar
    Inherits ThemeControl154

    Private _Progress As Integer = 0
    Private _Maximum As Integer = 100

    <Category("Control")>
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal V As Integer)
            Select Case V
                Case Is < _Progress
                    _Progress = V
            End Select
            _Maximum = V
            Invalidate()
        End Set
    End Property

    <Category("Control")>
    Public Property Value() As Integer
        Get
            Select Case _Progress
                Case 0
                    Return 0
                    Invalidate()
                Case Else
                    Return _Progress
                    Invalidate()
            End Select
        End Get
        Set(ByVal value As Integer)
            Select Case value
                Case Is > _Maximum
                    value = _Maximum
                    Invalidate()
            End Select
            _Progress = value
            Invalidate()
        End Set
    End Property

    Public Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        MinimumSize = Size
        Size = New Size(280, 18)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Parent.BackColor)
        DrawRoundedRectangle(G, New Rectangle(0, 0, Width, Height), 5, New Pen(Color.Gray, 1), New LinearGradientBrush(New Point((Width \ 2), 0), New Point((Width \ 2), Height), Color.FromArgb(80, 80, 80), Color.FromArgb(100, 100, 100)))
        DrawRoundedRectangle(G, New Rectangle(2, 2, CInt(Math.Truncate((CSng(_Progress) / CSng(_Maximum)) * (Width - 4))), (Height - 4)), 5, New Pen(Color.Gray, 1), New LinearGradientBrush(New Point((Width \ 2), 0), New Point((Width \ 2), Height), Color.FromArgb(180, 180, 180), Color.FromArgb(200, 200, 200)))
        G.DrawString((CInt(Math.Truncate((CSng(_Progress) / CSng(_Maximum)) * 100)).ToString() & "%"), New Font("Verdana", 7, FontStyle.Bold), New SolidBrush(Color.Black), New PointF(CInt(Math.Truncate((CSng(_Progress) / CSng(_Maximum)) * (Width - 4) + 3)), 5))
        G.DrawString((CInt(Math.Truncate((CSng(_Progress) / CSng(_Maximum)) * 100)).ToString() & "%"), New Font("Verdana", 7, FontStyle.Bold), New SolidBrush(Color.White), New PointF(CInt(Math.Truncate((CSng(_Progress) / CSng(_Maximum)) * (Width - 4) + 2)), 4))
    End Sub
End Class

Class DynamUISeperator
    Inherits ThemeControl154

    Enum Style
        Horizontal
        Vertical
    End Enum

    Private _Type As Style = Style.Horizontal
    <Category("Control")>
    Public Property Alignment As Style
        Get
            Return _Type
        End Get
        Set(ByVal value As Style)
            _Type = value
            Invalidate()
        End Set
    End Property

    Private _Thickness As Single = 1
    <Category("Control")>
    Public Property Thickness As Single
        Get
            Return _Thickness
        End Get
        Set(ByVal value As Single)
            _Thickness = value
            Invalidate()
        End Set
    End Property

    Private _SeperatorColour As Color = Color.FromArgb(128, 128, 128)
    <Category("Colours")>
    Public Property SeperatorColour As Color
        Get
            Return _SeperatorColour
        End Get
        Set(ByVal value As Color)
            _SeperatorColour = value
            Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(30, 15)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Parent.BackColor)
        G.SmoothingMode = SmoothingMode.Default

        Select Case _Type
            Case Style.Horizontal
                G.DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(0, Height / 2), New Point(Width, Height / 2))
            Case Style.Vertical
                G.DrawLine(New Pen(_SeperatorColour, _Thickness), New Point(Width / 2, 0), New Point(Width / 2, Height))
        End Select
    End Sub
End Class

Class DynamUIGroupbox
    Inherits ThemeControl154

    Private _TextColour As Color = Color.FromArgb(50, 153, 187)
    <Category("Colours")>
    Public Property TextColour As Color
        Get
            Return _TextColour
        End Get
        Set(ByVal value As Color)
            _TextColour = value
            Invalidate()
        End Set
    End Property

    Private _Text As String
    Overrides Property Text As String
        Get
            Return _Text
        End Get
        Set(ByVal value As String)
            _Text = value
            Invalidate()
        End Set
    End Property

    Public Overrides Property Font As System.Drawing.Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            MyBase.Font = value
        End Set
    End Property

    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, False)
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or _
                 ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Size = New Size(120, 140)
        Font = New Font("Verdana", 11)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Parent.BackColor)
        G.SmoothingMode = SmoothingMode.Default

        G.FillRectangle(New SolidBrush(Color.FromArgb(237, 237, 237)), New Rectangle(0, 1, Width - 1, Height))
        Dim TextPoint As Point = New Point(5, 5)
        G.DrawString(Text, Font, New SolidBrush(_TextColour), TextPoint)

        G.DrawRectangle(New Pen(Brushes.DimGray, 1), New Rectangle(G.MeasureString(Text, Font).Width + 6, 14.5, Width, 1))
    End Sub
End Class