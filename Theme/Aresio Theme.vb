Imports System.Drawing.Drawing2D

Module DesignFunctions
    Function ToBrush(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Brush
        Return ToBrush(Color.FromArgb(A, R, G, B))
    End Function
    Function ToBrush(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Brush
        Return ToBrush(Color.FromArgb(R, G, B))
    End Function
    Function ToBrush(ByVal A As Integer, ByVal C As Color) As Brush
        Return ToBrush(Color.FromArgb(A, C))
    End Function
    Function ToBrush(ByVal Pen As Pen) As Brush
        Return ToBrush(Pen.Color)
    End Function
    Function ToBrush(ByVal Color As Color) As Brush
        Return New SolidBrush(Color)
    End Function
    Function ToPen(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Pen
        Return ToPen(Color.FromArgb(A, R, G, B))
    End Function
    Function ToPen(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Pen
        Return ToPen(Color.FromArgb(R, G, B))
    End Function
    Function ToPen(ByVal A As Integer, ByVal C As Color) As Pen
        Return ToPen(Color.FromArgb(A, C))
    End Function
    Function ToPen(ByVal Color As Color) As Pen
        Return ToPen(New SolidBrush(Color))
    End Function
    Function ToPen(ByVal Brush As SolidBrush) As Pen
        Return New Pen(Brush)
    End Function

    Class CornerStyle
        Public TopLeft As Boolean
        Public TopRight As Boolean
        Public BottomLeft As Boolean
        Public BottomRight As Boolean
    End Class

    Public Function AdvRect(ByVal Rectangle As Rectangle, ByVal CornerStyle As CornerStyle, ByVal Curve As Integer) As GraphicsPath
        AdvRect = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2

        If CornerStyle.TopLeft Then
            AdvRect.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        Else
            AdvRect.AddLine(Rectangle.X, Rectangle.Y, Rectangle.X + ArcRectangleWidth, Rectangle.Y)
        End If

        If CornerStyle.TopRight Then
            AdvRect.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        Else
            AdvRect.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y, Rectangle.X + Rectangle.Width, Rectangle.Y + ArcRectangleWidth)
        End If

        If CornerStyle.BottomRight Then
            AdvRect.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        Else
            AdvRect.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height, Rectangle.X + Rectangle.Width - ArcRectangleWidth, Rectangle.Y + Rectangle.Height)
        End If

        If CornerStyle.BottomLeft Then
            AdvRect.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        Else
            AdvRect.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height, Rectangle.X, Rectangle.Y + Rectangle.Height - ArcRectangleWidth)
        End If

        AdvRect.CloseAllFigures()

        Return AdvRect
    End Function

    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        RoundRect = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        RoundRect.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        RoundRect.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        RoundRect.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        RoundRect.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        RoundRect.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, ArcRectangleWidth + Rectangle.Y))
        RoundRect.CloseAllFigures()
        Return RoundRect
    End Function

    Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
        Return RoundRect(New Rectangle(X, Y, Width, Height), Curve)
    End Function

    Class PillStyle
        Public Left As Boolean
        Public Right As Boolean
    End Class

    Public Function Pill(ByVal Rectangle As Rectangle, ByVal PillStyle As PillStyle) As GraphicsPath
        Pill = New GraphicsPath()

        If PillStyle.Left Then
            Pill.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Height, Rectangle.Height), -270, 180)
        Else
            Pill.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height, Rectangle.X, Rectangle.Y)
        End If

        If PillStyle.Right Then
            Pill.AddArc(New Rectangle(Rectangle.X + Rectangle.Width - Rectangle.Height, Rectangle.Y, Rectangle.Height, Rectangle.Height), -90, 180)
        Else
            Pill.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height)
        End If

        Pill.CloseAllFigures()

        Return Pill
    End Function

    Public Function Pill(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal PillStyle As PillStyle)
        Return Pill(New Rectangle(X, Y, Width, Height), PillStyle)
    End Function

End Module

Class AresioButton
    Inherits Control

    Enum MouseState
        None
        Over
        Down
    End Enum

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer, True)
    End Sub

    Private State As MouseState = 0

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics

        G.SmoothingMode = SmoothingMode.HighQuality

        'Background
        G.FillPath(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(250, 200, 70), Color.FromArgb(250, 160, 40)), _
                   RoundRect(0, 0, Width - 1, Height - 1, 4))

        G.DrawPath(ToPen(50, Color.White), RoundRect(0, 1, Width - 1, Height - 2, 4))
        G.DrawPath(ToPen(150, 100, 70), RoundRect(0, 0, Width - 1, Height - 1, 4))
        Select Case Enabled
            Case True
                Select Case State
                    Case MouseState.Over
                        G.FillPath(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(50, Color.White), Color.Transparent), _
           RoundRect(0, 0, Width - 1, Height - 1, 4))
                    Case MouseState.Down
                        G.FillPath(New LinearGradientBrush(New Point(0, 0), New Point(0, Height), Color.FromArgb(50, Color.Black), Color.Transparent), _
                   RoundRect(0, 0, Width - 1, Height - 1, 4))
                End Select

                G.DrawString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular), ToBrush(100, Color.White), New Point(CInt((Width / 2) - (G.MeasureString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular)).Width / 2)), CInt((Height / 2) - (G.MeasureString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular)).Height / 2)) + 1))
                G.DrawString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular), ToBrush(200, Color.Black), New Point(CInt((Width / 2) - (G.MeasureString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular)).Width / 2)), CInt((Height / 2) - (G.MeasureString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular)).Height / 2))))
            Case False
                G.DrawString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular), Brushes.White, New Point(CInt((Width / 2) - (G.MeasureString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular)).Width / 2)) + 1, CInt((Height / 2) - (G.MeasureString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular)).Height / 2)) + 1))
                G.DrawString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular), Brushes.Gray, New Point(CInt((Width / 2) - (G.MeasureString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular)).Width / 2)), CInt((Height / 2) - (G.MeasureString(Text, New Font(Font.FontFamily, Font.Size, FontStyle.Regular)).Height / 2))))
        End Select
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e) : State = MouseState.Over : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e) : State = MouseState.None : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e) : State = MouseState.Down : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : State = MouseState.Over : Invalidate()
    End Sub
End Class


Class AresioTrackBar
    Inherits Control

#Region "Properties"
    Dim _Maximum As Integer = 10
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If value > 0 Then _Maximum = value
            If value < _Value Then _Value = value
            Invalidate()
        End Set
    End Property

    Event ValueChanged()
    Dim _Value As Integer = 0
    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)

            Select Case value
                Case Is = _Value
                    Exit Property
                Case Is < 0
                    _Value = 0
                Case Is > _Maximum
                    _Value = _Maximum
                Case Else
                    _Value = value
            End Select

            Invalidate()
            RaiseEvent ValueChanged()
        End Set
    End Property
#End Region

    Sub New()
        Me.SetStyle(ControlStyles.DoubleBuffer Or _
                    ControlStyles.AllPaintingInWmPaint Or _
                    ControlStyles.ResizeRedraw Or _
                    ControlStyles.UserPaint Or _
                    ControlStyles.Selectable Or _
                    ControlStyles.SupportsTransparentBackColor, True)
    End Sub

    Dim CaptureM As Boolean = False
    Dim Bar As Rectangle = New Rectangle(0, 10, Width - 1, Height - 21)
    Dim Track As Size = New Size(20, 20)

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics
        Bar = New Rectangle(10, 10, Width - 21, Height - 21)
        G.Clear(Parent.FindForm.BackColor)
        G.SmoothingMode = SmoothingMode.AntiAlias

        'Background
        Dim BackLinear As LinearGradientBrush = New LinearGradientBrush(New Point(0, CInt((Height / 2) - 4)), New Point(0, CInt((Height / 2) + 4)), Color.FromArgb(50, Color.Black), Color.Transparent)
        G.FillPath(BackLinear, RoundRect(0, CInt((Height / 2) - 4), Width - 1, 8, 3))
        G.DrawPath(ToPen(50, Color.Black), RoundRect(0, CInt((Height / 2) - 4), Width - 1, 8, 3))
        BackLinear.Dispose()


        'Fill
        G.FillPath(New LinearGradientBrush(New Point(1, CInt((Height / 2) - 4)), New Point(1, CInt((Height / 2) + 4)), Color.FromArgb(250, 200, 70), Color.FromArgb(250, 160, 40)), RoundRect(1, CInt((Height / 2) - 4), CInt(Bar.Width * (Value / Maximum)) + CInt(Track.Width / 2), 8, 3))
        G.DrawPath(ToPen(100, Color.White), RoundRect(2, CInt((Height / 2) - 2), CInt(Bar.Width * (Value / Maximum)) + CInt(Track.Width / 2), 4, 3))
        G.SetClip(RoundRect(1, CInt((Height / 2) - 4), CInt(Bar.Width * (Value / Maximum)) + CInt(Track.Width / 2), 8, 3))
        For i = 0 To CInt(Bar.Width * (Value / Maximum)) + CInt(Track.Width / 2) Step 10
            G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(20, Color.Black)), 4), New Point(i, CInt((Height / 2) - 10)), New Point(i - 10, CInt((Height / 2) + 10)))
        Next
        G.SetClip(New Rectangle(0, 0, Width, Height))

        'Button
        G.FillEllipse(Brushes.White, Bar.X + CInt(Bar.Width * (Value / Maximum)) - CInt(Track.Width / 2), Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2), Track.Width, Track.Height)
        G.DrawEllipse(ToPen(50, Color.Black), Bar.X + CInt(Bar.Width * (Value / Maximum)) - CInt(Track.Width / 2), Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2), Track.Width, Track.Height)
        G.FillEllipse(New LinearGradientBrush(New Point(0, Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2)), New Point(0, Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2) + Track.Height), Color.FromArgb(200, Color.Black), Color.FromArgb(100, Color.Black)), New Rectangle(Bar.X + CInt(Bar.Width * (Value / Maximum)) - CInt(Track.Width / 2) + 6, Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2) + 6, Track.Width - 12, Track.Height - 12))

    End Sub

    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        Me.BackColor = Color.Transparent

        MyBase.OnHandleCreated(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e)
        Dim mp = New Rectangle(New Point(e.Location.X, e.Location.Y), New Size(1, 1))
        Dim Bar As Rectangle = New Rectangle(10, 10, Width - 21, Height - 21)
        If New Rectangle(New Point(Bar.X + CInt(Bar.Width * (Value / Maximum)) - CInt(Track.Width / 2), 0), New Size(Track.Width, Height)).IntersectsWith(mp) Then
            CaptureM = True
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        CaptureM = False
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        If CaptureM Then
            Dim mp As Point = New Point(e.X, e.Y)
            Dim Bar As Rectangle = New Rectangle(10, 10, Width - 21, Height - 21)
            Value = CInt(Maximum * ((mp.X - Bar.X) / Bar.Width))
        End If
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e) : CaptureM = False
    End Sub

End Class

Class AresioSwitch
    Inherits Control

    Private ToggleLocation As Integer = 0
    Private WithEvents ToggleAnimation As Timer = New Timer With {.Interval = 1}

    Event ToggledChanged()
    Private _toggled As Boolean
    Public Property Toggled() As Boolean
        Get
            Return _toggled
        End Get
        Set(ByVal value As Boolean)
            _toggled = value
            Invalidate()

            RaiseEvent ToggledChanged()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                ControlStyles.ResizeRedraw Or _
                ControlStyles.UserPaint Or _
                ControlStyles.DoubleBuffer, True)
    End Sub

    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        ToggleAnimation.Start()
    End Sub

    Private Sub Animation() Handles ToggleAnimation.Tick
        If _toggled Then
            If ToggleLocation < 100 Then
                ToggleLocation += 10
            End If
        Else
            If ToggleLocation > 0 Then
                ToggleLocation -= 10
            End If
        End If

        Invalidate()
    End Sub

    Dim Bar As Rectangle = New Rectangle(0, 10, Width - 1, Height - 21)
    Dim Track As Size = New Size(20, 20)

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics
        Bar = New Rectangle(10, 10, Width - 21, Height - 21)
        G.Clear(Parent.FindForm.BackColor)
        G.SmoothingMode = SmoothingMode.AntiAlias

        'Background
        Dim BackLinear As LinearGradientBrush = New LinearGradientBrush(New Point(0, CInt((Height / 2) - (Track.Height / 2))), New Point(0, CInt((Height / 2) + (Track.Height / 2))), Color.FromArgb(50, Color.Black), Color.Transparent)
        'G.FillPath(BackLinear, RoundRect(0, CInt((Height / 2) - 4), Width - 1, 8, 3))
        G.FillPath(BackLinear, Pill(0, CInt(Height / 2 - Track.Height / 2), Width - 1, Track.Height - 2, New PillStyle With {.Left = True, .Right = True}))
        G.DrawPath(ToPen(50, Color.Black), Pill(0, CInt(Height / 2 - Track.Height / 2), Width - 1, Track.Height - 2, New PillStyle With {.Left = True, .Right = True}))

        BackLinear.Dispose()

        'Fill
        If ToggleLocation > 0 Then
            G.FillPath(New LinearGradientBrush(New Point(0, CInt((Height / 2) - Track.Height / 2)), New Point(1, CInt((Height / 2) + Track.Height / 2)), Color.FromArgb(250, 200, 70), Color.FromArgb(250, 160, 40)), Pill(1, CInt((Height / 2) - Track.Height / 2), CInt(Bar.Width * (ToggleLocation / 100)) + CInt(Track.Width / 2), Track.Height - 3, New PillStyle With {.Left = True, .Right = True}))
            G.DrawPath(ToPen(100, Color.White), Pill(1, CInt((Height / 2) - Track.Height / 2 + 1), CInt(Bar.Width * (ToggleLocation / 100)) + CInt(Track.Width / 2), Track.Height - 5, New PillStyle With {.Left = True, .Right = True}))
        End If

        If Toggled Then
            G.DrawString("ON", New Font("Arial", 6, FontStyle.Bold), ToBrush(150, Color.Black), New Rectangle(0, -1, Width - Track.Width + Track.Width / 3, Height), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        Else
            G.DrawString("OFF", New Font("Arial", 6, FontStyle.Bold), ToBrush(150, Color.Black), New Rectangle(Track.Width - Track.Width / 3, -1, Width - Track.Width + Track.Width / 3, Height), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End If

        'Button
        G.FillEllipse(Brushes.White, Bar.X + CInt(Bar.Width * (ToggleLocation / 100)) - CInt(Track.Width / 2), Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2), Track.Width, Track.Height)
        G.DrawEllipse(ToPen(50, Color.Black), Bar.X + CInt(Bar.Width * (ToggleLocation / 100) - CInt(Track.Width / 2)), Bar.Y + CInt((Bar.Height / 2)) - CInt(Track.Height / 2), Track.Width, Track.Height)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        Toggled = Not Toggled
    End Sub
End Class


Class AresioTabControl
    Inherits TabControl

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer, True)
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        SizeMode = TabSizeMode.Normal
        ItemSize = New Size(77, 31)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim G As Graphics = e.Graphics
        Dim ItemBounds As Rectangle
        Dim TextBrush As New SolidBrush(Color.Empty)
        Dim SOFF As Integer = 0
        G.Clear(Color.FromArgb(236, 237, 239))

        For TabItemIndex As Integer = 0 To Me.TabCount - 1
            ItemBounds = Me.GetTabRect(TabItemIndex)

            If Not TabItemIndex = SelectedIndex Then
                SOFF = 2

                G.FillPath(ToBrush(10, Color.Black), RoundRect(New Rectangle(New Point(ItemBounds.X, ItemBounds.Y + SOFF), New Size(ItemBounds.Width, ItemBounds.Height)), 2))
                G.DrawPath(ToPen(90, Color.Black), RoundRect(New Rectangle(New Point(ItemBounds.X, ItemBounds.Y + SOFF), New Size(ItemBounds.Width, ItemBounds.Height)), 2))

                Dim sf As New StringFormat
                sf.LineAlignment = StringAlignment.Center
                sf.Alignment = StringAlignment.Center
                TextBrush.Color = Color.FromArgb(80, 80, 80)
                Try
                    G.DrawString(TabPages(TabItemIndex).Text, New Font(Font.Name, Font.Size - 1), TextBrush, New Rectangle(GetTabRect(TabItemIndex).Location, GetTabRect(TabItemIndex).Size), sf)
                    TabPages(TabItemIndex).BackColor = Color.FromArgb(236, 237, 239)
                Catch : End Try

            End If
        Next

        G.FillPath(ToBrush(236, 237, 239), RoundRect(0, ItemSize.Height - 1, Width - 1, Height - ItemSize.Height - 1, 2))
        G.DrawPath(ToPen(150, 151, 153), RoundRect(0, ItemSize.Height - 1, Width - 1, Height - ItemSize.Height - 1, 2))

        For TabItemIndex As Integer = 0 To Me.TabCount - 1
            ItemBounds = Me.GetTabRect(TabItemIndex)

            If TabItemIndex = SelectedIndex Then

                G.FillPath(ToBrush(236, 237, 239), RoundRect(New Rectangle(New Point(ItemBounds.X - 2, ItemBounds.Y), New Size(ItemBounds.Width + 3, ItemBounds.Height - 2)), 2))
                G.DrawPath(ToPen(150, 151, 153), RoundRect(New Rectangle(New Point(ItemBounds.X - 2, ItemBounds.Y), New Size(ItemBounds.Width + 2, ItemBounds.Height - 2)), 2))

                G.FillRectangle(ToBrush(236, 237, 239), New Rectangle(New Point(ItemBounds.X - 1, ItemBounds.Y + 1), New Size(ItemBounds.Width + 1, ItemBounds.Height)))
                SOFF = 0

                Dim sf As New StringFormat
                sf.LineAlignment = StringAlignment.Center
                sf.Alignment = StringAlignment.Center
                TextBrush.Color = Color.FromArgb(80, 80, 80)
                Try
                    G.DrawString(TabPages(TabItemIndex).Text, Font, TextBrush, New Rectangle(GetTabRect(TabItemIndex).Location + New Point(0, SOFF), GetTabRect(TabItemIndex).Size), sf)
                    TabPages(TabItemIndex).BackColor = Color.FromArgb(236, 237, 239)
                Catch : End Try

            End If
        Next
    End Sub

End Class

Class AresioProgressBar
    Inherits Control

#Region " Properties "

    Private _minimum As Integer
    Public Property Minimum() As Integer
        Get
            Return _minimum
        End Get
        Set(ByVal value As Integer)
            _minimum = value

            If value > _maximum Then _maximum = value
            If value > _value Then _value = value

            Invalidate()
        End Set
    End Property

    Private _maximum As Integer
    Public Property Maximum() As Integer
        Get
            Return _maximum
        End Get
        Set(ByVal value As Integer)
            _maximum = value

            If value < _minimum Then _minimum = value
            If value < _value Then _value = value

            Invalidate()
        End Set
    End Property

    Event ValueChanged()
    Private _value As Integer
    Public Property Value() As Integer
        Get
            Return _value
        End Get
        Set(ByVal value As Integer)
            If value < _minimum Then
                _value = _minimum
            ElseIf value > _maximum Then
                _value = _maximum
            Else
                _value = value
            End If

            Invalidate()
            RaiseEvent ValueChanged()
        End Set
    End Property

#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                ControlStyles.ResizeRedraw Or _
                ControlStyles.UserPaint Or _
                ControlStyles.DoubleBuffer, True)
        _maximum = 100
        _minimum = 0
        _value = 0
    End Sub


    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics
        G.Clear(Parent.FindForm.BackColor)
        G.SmoothingMode = SmoothingMode.AntiAlias

        'Background
        Dim BackLinear As LinearGradientBrush = New LinearGradientBrush(New Point(0, CInt((Height / 2) - 4)), New Point(0, CInt((Height / 2) + 4)), Color.FromArgb(50, Color.Black), Color.Transparent)
        G.FillPath(BackLinear, RoundRect(0, CInt((Height / 2) - 4), Width - 1, 8, 3))
        G.DrawPath(ToPen(50, Color.Black), RoundRect(0, CInt((Height / 2) - 4), Width - 1, 8, 3))
        BackLinear.Dispose()

        'Fill
        If _value > 0 Then
            G.FillPath(New LinearGradientBrush(New Point(1, CInt((Height / 2) - 4)), New Point(1, CInt((Height / 2) + 4)), Color.FromArgb(250, 200, 70), Color.FromArgb(250, 160, 40)), RoundRect(1, CInt((Height / 2) - 4), CInt((Width - 2) * (Value / Maximum)), 8, 3))
            G.DrawPath(ToPen(100, Color.White), RoundRect(2, CInt((Height / 2) - 2), CInt((Width - 4) * (Value / Maximum)), 4, 3))
        End If
    End Sub
End Class

Class AresioRadioButton
    Inherits Control

    Event CheckedChanged()
    Private _checked As Boolean
    Public Property Checked() As Boolean
        Get
            Return _checked
        End Get
        Set(ByVal value As Boolean)
            _checked = value

            Invalidate()
            RaiseEvent CheckedChanged()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                ControlStyles.ResizeRedraw Or _
                ControlStyles.UserPaint Or _
                ControlStyles.DoubleBuffer, True)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics

        G.SmoothingMode = SmoothingMode.AntiAlias

        Dim MLG As LinearGradientBrush = New LinearGradientBrush(New Point(Height / 2, 0), New Point(Height / 2, Height), Color.FromArgb(50, Color.Black), Color.Transparent)

        G.FillEllipse(MLG, New Rectangle(0, 0, Height - 1, Height - 1))
        G.DrawEllipse(ToPen(50, Color.Black), New Rectangle(0, 0, Height - 1, Height - 1))

        G.DrawString(Text, Font, Brushes.Black, New Rectangle(Height + 5, 0, Width - Height + 4, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})

        If _checked Then
            Dim MLG2 As LinearGradientBrush = New LinearGradientBrush(New Point(Height / 2, 3), New Point(Height / 2, Height - 6), Color.FromArgb(200, Color.White), Color.FromArgb(10, Color.White))
            G.FillEllipse(MLG2, New Rectangle(3, 3, Height - 7, Height - 7))
            G.DrawEllipse(ToPen(50, Color.Black), New Rectangle(3, 3, Height - 7, Height - 7))
        End If
    End Sub

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        MyBase.OnClick(e)

        If Not Checked Then Checked = True

        For Each ctl As Control In Parent.Controls
            If TypeOf ctl Is AresioRadioButton Then
                If ctl.Handle = Me.Handle Then Continue For
                If ctl.Enabled Then DirectCast(ctl, AresioRadioButton).Checked = False
            End If
        Next
    End Sub
End Class