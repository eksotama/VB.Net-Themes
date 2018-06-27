Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Text
Imports System.ComponentModel

'Glossyus Theme
'Creator: Kseg HF
'This theme is open source.

Enum MouseState As Byte
    Normal = 0
    Hovered = 1
    Pushed = 2
    Disabled = 3
End Enum

Module Draw
    Public Sub drawcorners(ByVal B As Bitmap, ByVal c As Color)
        'render
        B.SetPixel(0, 0, c)
        B.SetPixel(1, 0, c)
        B.SetPixel(0, 1, c)

        B.SetPixel(B.Width - 1, 0, c)
        B.SetPixel(B.Width - 1, 1, c)
        B.SetPixel(B.Width - 2, 0, c)

        B.SetPixel(B.Width - 1, B.Height - 1, c)
        B.SetPixel(B.Width - 1, B.Height - 2, c)
        B.SetPixel(B.Width - 2, B.Height - 1, c)

        B.SetPixel(0, B.Height - 1, c)
        B.SetPixel(0, B.Height - 2, c)
        B.SetPixel(1, B.Height - 1, c)
    End Sub

    Public Function GetBrush(ByVal c As Color) As SolidBrush
        Return New SolidBrush(c)
    End Function

    Public Function GetPen(ByVal c As Color) As Pen
        Return New Pen(New SolidBrush(c))
    End Function

    Function NoiseBrush(Colors As Color()) As TextureBrush
        Dim B As New Bitmap(128, 128)
        Dim R As New Random(128)

        For X As Integer = 0 To B.Width - 1
            For Y As Integer = 0 To B.Height - 1
                B.SetPixel(X, Y, Colors(R.Next(Colors.Length)))
            Next
        Next
        Dim T As New TextureBrush(B)
        B.Dispose()

        Return T
    End Function

    Public Sub InnerGlow(ByVal G As Graphics, ByVal Rectangle As Rectangle, ByVal Colors As Color())
        Dim SubtractTwo As Integer = 1
        Dim AddOne As Integer = 0
        For Each c In Colors
            G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(c.R, c.B, c.G))), Rectangle.X + AddOne, Rectangle.Y + AddOne, Rectangle.Width - SubtractTwo, Rectangle.Height - SubtractTwo)
            SubtractTwo += 2
            AddOne += 1
        Next
    End Sub

    Public Sub InnerGlowRounded(ByVal G As Graphics, ByVal Rectangle As Rectangle, ByVal Degree As Integer, ByVal Colors As Color())
        Dim SubtractTwo As Integer = 1
        Dim AddOne As Integer = 0
        For Each c In Colors
            G.DrawPath(New Pen(New SolidBrush(Color.FromArgb(c.R, c.B, c.G))), Draw.RoundRect(Rectangle.X + AddOne, Rectangle.Y + AddOne, Rectangle.Width - SubtractTwo, Rectangle.Height - SubtractTwo, Degree))
            SubtractTwo += 2
            AddOne += 1
        Next
    End Sub

    Function ToBrush(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Brush
        Return New SolidBrush(Color.FromArgb(A, R, G, B))
    End Function
    Function ToBrush(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Brush
        Return New SolidBrush(Color.FromArgb(R, G, B))
    End Function
    Function ToBrush(ByVal A As Integer, ByVal C As Color) As Brush
        Return New SolidBrush(Color.FromArgb(A, C))
    End Function
    Function ToBrush(ByVal Pen As Pen) As Brush
        Return New SolidBrush(Pen.Color)
    End Function
    Function ToBrush(ByVal Color As Color) As Brush
        Return New SolidBrush(Color)
    End Function
    Function ToPen(ByVal A As Integer, ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Pen
        Return New Pen(New SolidBrush(Color.FromArgb(A, R, G, B)))
    End Function
    Function ToPen(ByVal R As Integer, ByVal G As Integer, ByVal B As Integer) As Pen
        Return New Pen(New SolidBrush(Color.FromArgb(R, G, B)))
    End Function
    Function ToPen(ByVal A As Integer, ByVal C As Color) As Pen
        Return New Pen(New SolidBrush(Color.FromArgb(A, C)))
    End Function
    Function ToPen(ByVal Brush As SolidBrush) As Pen
        Return New Pen(Brush)
    End Function
    Function ToPen(ByVal Color As Color) As Pen
        Return New Pen(New SolidBrush(Color))
    End Function

    Public Function RoundRect(ByVal Rectangle As Rectangle, ByVal Curve As Integer) As GraphicsPath
        Dim P As GraphicsPath = New GraphicsPath()
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.FillMode = FillMode.Winding
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        P.CloseAllFigures()
        Return P
    End Function
    Public Function RoundRect(ByVal X As Integer, ByVal Y As Integer, ByVal Width As Integer, ByVal Height As Integer, ByVal Curve As Integer) As GraphicsPath
        Dim Rectangle As Rectangle = New Rectangle(X, Y, Width, Height)
        Dim P As GraphicsPath = New GraphicsPath()
        P.FillMode = FillMode.Winding
        Dim ArcRectangleWidth As Integer = Curve * 2
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -180, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), -90, 90)
        P.AddArc(New Rectangle(Rectangle.Width - ArcRectangleWidth + Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 0, 90)
        P.AddArc(New Rectangle(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y, ArcRectangleWidth, ArcRectangleWidth), 90, 90)
        P.AddLine(New Point(Rectangle.X, Rectangle.Height - ArcRectangleWidth + Rectangle.Y), New Point(Rectangle.X, Curve + Rectangle.Y))
        P.CloseAllFigures()
        Return P
    End Function

    Public Sub DrawDropShadow(G As Graphics, Color As Color, pth As GraphicsPath, size As Integer, Optional Angle As Single = 120, Optional Distance As Single = 0, Optional Opacity As Single = 1, Optional Spread As Single = 2)
        'Credit to BlackCap
        ' Trim the angle
        If Angle > 360 Then Angle = 360 * (Angle / 360 - Math.Floor(Angle / 360))
        If Angle < 0 Then Angle = 360 * (Angle / 360 - Math.Floor(Angle / 360))

        If Distance < 0 Then Throw New Exception("The distance have to be => 0") ' We don't want negative distances
        If Opacity < 0 Or Opacity > 1 Then Throw New Exception("The opacity have to be between 1 and 0")

        Dim pthCopy As GraphicsPath = pth.Clone    ' Clone the path so we don't fuck up the original one
        size *= 2 ' Double the size to get actual pixel size

        ' Calculate and apply the distance transform
        Dim xi As Double = Math.Cos((Math.PI / 180) * (Angle - 90)) * Distance
        Dim yi As Double = Math.Sin((Math.PI / 180) * (Angle - 90)) * Distance
        If Distance > 0 Then pthCopy.Transform(New Matrix(1, 0, 0, 1, xi, yi))

        ' We don't want to draw inside the path, so we have to create a clip
        Dim OldClip As Region = G.Clip
        Dim NewClip As GraphicsPath = pth.Clone
        Dim b = NewClip.GetBounds
        NewClip.Transform(New Matrix((b.Width - 1) / b.Width, 0, 0, (b.Height - 1) / b.Height, 0.5, 0.5)) ' Shrink it a pixle to make sure the visible shadow don't get clipped
        NewClip.AddRectangle(New RectangleF((b.X - size) * 2, (b.Y - size) * 2, (b.Width + size) * 2, (b.Height + size) * 2)) ' Invert it
        G.SetClip(NewClip)

        ' Draw the shadow
        For i = 0 To size
            b = pthCopy.GetBounds

            Using m As New Matrix((b.Width + 1) / b.Width, 0, 0, (b.Height + 1) / b.Height, -0.5, -0.5)
                Using br As New SolidBrush(Color.FromArgb((((255 - Math.Pow(i / size, 1 + Spread) * 255) * Opacity) / size), Color))
                    pthCopy.Transform(m)
                    G.FillPath(br, pthCopy)
                End Using
            End Using
        Next

        ' Clean up
        pthCopy.Dispose()
        NewClip.Dispose()
        If OldClip IsNot Nothing Then G.Clip = OldClip Else G.ResetClip()
    End Sub
End Module

Public Class RadialProgressBar
    Inherits Control

#Region "Var & Propeties"
    Private _Value As Integer = 100
    Private _Thickness As Integer = 15
    Private _Angle As Integer = -90
    Private _Symbol As String = "%"
    Private _ValueText As Boolean = True
    Private _ShadowColor As Color = Color.Gray
    Private _GroundColor As Color = Color.FromArgb(64, 0, 0, 0)
    Private _BarColor1 As Color = Color.DarkOrange
    Private _BarColor2 As Color = Color.Yellow
    Private _BarColor3 As Color = Color.GreenYellow
    Private _BarThickness As Integer = 20
    Private _MaxValue As Integer = 100
    Private _AvoidAngle As Integer = 90
    Private _3DEffect As Boolean = True
    Private _BarColorMode As LinearGradientMode = LinearGradientMode.ForwardDiagonal
    Private _EndBarForm As LineCap = LineCap.Round

    Public Property EndBarForm As LineCap
        Get
            Return _EndBarForm
        End Get
        Set(value As LineCap)
            _EndBarForm = value : Invalidate()
        End Set
    End Property

    Public Property BarColorMode As LinearGradientMode
        Get
            Return _BarColorMode
        End Get
        Set(value As LinearGradientMode)
            _BarColorMode = value : Invalidate()
        End Set
    End Property

    Public Property EffectEnabled As Boolean
        Get
            Return _3DEffect
        End Get
        Set(value As Boolean)
            _3DEffect = value : Invalidate()
        End Set
    End Property

    Public Property GroundColor As Color
        Get
            Return _GroundColor
        End Get
        Set(value As Color)
            _GroundColor = value : Invalidate()
        End Set
    End Property

    Public Property BarColor1 As Color
        Get
            Return _BarColor1
        End Get
        Set(value As Color)
            _BarColor1 = value : Invalidate()
        End Set
    End Property
    Public Property BarColor2 As Color
        Get
            Return _BarColor2
        End Get
        Set(value As Color)
            _BarColor2 = value : Invalidate()
        End Set
    End Property
    Public Property BarColor3 As Color
        Get
            Return _BarColor3
        End Get
        Set(value As Color)
            _BarColor3 = value : Invalidate()
        End Set
    End Property

    Public Property ShadowColor As Color
        Get
            Return _ShadowColor
        End Get
        Set(value As Color)
            _ShadowColor = value : Invalidate()
        End Set
    End Property

    Event ValueChanged()
    Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal v As Integer)
            If v > MaxValue Or v < 0 Then
                _Value = _Value
            Else
                _Value = v : Invalidate()
            End If
            RaiseEvent ValueChanged()
        End Set
    End Property

    Public Property MaxValue As Integer
        Get
            Return _MaxValue
        End Get
        Set(ByVal value As Integer)
            _MaxValue = value : Invalidate()
        End Set
    End Property

    Public Property TextVisible As Boolean
        Get
            Return _ValueText
        End Get
        Set(ByVal value As Boolean)
            _ValueText = value : Invalidate()
        End Set
    End Property

    Public Property Angle() As Integer
        Get
            Return _Angle
        End Get
        Set(ByVal v As Integer)
            _Angle = v : Invalidate()
        End Set
    End Property

    Public Property AvoidAngle() As Integer
        Get
            Return _AvoidAngle
        End Get
        Set(ByVal v As Integer)
            If v < 0 Or v >= 360 Then
                _AvoidAngle = _AvoidAngle
            Else
                _AvoidAngle = v : Invalidate()
            End If
        End Set
    End Property

    Public Property Symbol() As String
        Get
            Return _Symbol
        End Get
        Set(ByVal v As String)
            _Symbol = v : Invalidate()
        End Set
    End Property

    Public Property Thickness() As Integer
        Get
            Return _Thickness
        End Get
        Set(ByVal v As Integer)
            _Thickness = v : Invalidate()
        End Set
    End Property

    Public Property BarThickness() As Integer
        Get
            Return _BarThickness
        End Get
        Set(ByVal v As Integer)
            _BarThickness = v : Invalidate()
        End Set
    End Property
#End Region

    Sub New()
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.Gray
        Font = New Font("Arial", 13)
        Size = New Size(125, 125)
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        If Me.Width < 24 Then Me.Width = 24 : If Me.Height < 24 Then Me.Height = 24
        Dim i As Integer = (360 - (AvoidAngle)) / MaxValue * _Value
        Using B1 As New Bitmap(Width, Height)

            Using G As Graphics = Graphics.FromImage(B1)
                G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                G.SmoothingMode = SmoothingMode.AntiAlias
                DoubleBuffered = True

                Dim TBrush1 As TextureBrush = NoiseBrush(New Color() {Color.FromArgb(10, BackColor), Color.FromArgb(10, Color.Gray), Color.FromArgb(10, Color.DarkGray)})
                G.FillRectangle(TBrush1, ClientRectangle)

                Dim GP2 As GraphicsPath = New GraphicsPath()
                GP2.AddEllipse(ClientRectangle)
                Dim PGB2 As PathGradientBrush = New PathGradientBrush(GP2)
                PGB2.CenterPoint = New PointF(Width / 2, Height / 2)
                PGB2.CenterColor = Color.FromArgb(128, ShadowColor)
                PGB2.SurroundColors = New Color() {Color.Transparent}
                PGB2.SetBlendTriangularShape(0.3, 1)
                PGB2.FocusScales = New PointF(0.4, 0.4)
                G.FillPath(PGB2, GP2)
                PGB2.Dispose() : GP2.Dispose()

                Using LGB As New LinearGradientBrush(ClientRectangle, GroundColor, GroundColor, LinearGradientMode.Vertical)
                    Using P1 As New Pen(LGB, Thickness)
                        G.DrawArc(P1, CInt(Thickness / 2) + 9, CInt(Thickness / 2) + 9, Width - Thickness - 18, Height - Thickness - 18, -90, 360)
                    End Using
                End Using

                Using LGB1 As New LinearGradientBrush(ClientRectangle, Color.Black, Color.Black, BarColorMode)
                    Using P1 As New Pen(LGB1, BarThickness)
                        Dim gp1 As New GraphicsPath

                        Dim CB As New ColorBlend 'You can Add more Colors if you want.
                        CB.Colors = New Color() {BarColor1, BarColor2, BarColor3}
                        CB.Positions = New Single() {0.0F, 0.5F, 1.0F}

                        LGB1.InterpolationColors = CB
                        P1.Brush = LGB1
                        P1.EndCap = EndBarForm
                        G.DrawArc(P1, CInt(Thickness / 2) + 9, CInt(Thickness / 2) + 9, Width - Thickness - 18, Height - Thickness - 18, CInt(_Angle + (AvoidAngle / 2)), i)

                        '3D effect ^^ (has some problems...)
                        If _3DEffect Then
                            Dim LGB2 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(48, Color.White), Color.FromArgb(48, Color.Black), 90)
                            Dim P2 As New Pen(LGB2, BarThickness / 2)
                            P2.EndCap = EndBarForm
                            G.DrawArc(P2, CInt(Thickness / 2) + 4, CInt(Thickness / 2) + 4, Width - Thickness - 8, Height - Thickness - 8, CInt(_Angle + (AvoidAngle / 2)), i)
                        End If
                    End Using
                End Using

                If TextVisible Then
                    G.DrawString(_Value & _Symbol, Font, New SolidBrush(ForeColor), New Point(Me.Width / 2 - G.MeasureString(_Value & _Symbol, Font).Width / 2 + 1, Me.Height / 2 - G.MeasureString(_Value & "%", Font).Height / 2 + 1))
                End If
            End Using
            e.Graphics.DrawImage(B1, 0, 0)
        End Using
    End Sub
End Class

Public Class ProgressBar
    Inherits Control

    Private _Value As Integer = 50
    Private _Thickness As Integer = 20
    Private _Symbol As String = "%"
    Private _ValueText As Boolean = True
    Private _GlowColor As Color = Color.Gold
    Private _GroundColor As Color = Color.Silver
    Private _BarColor1 As Color = Color.DarkOrange
    Private _BarColor2 As Color = Color.Yellow
    Private _BarColor3 As Color = Color.GreenYellow
    Private _MaxValue As Integer = 100
    Private _GlowEnabled As Boolean = True
    Private _LinesEnabled As Boolean = True
    Private _Angle As Integer = 0
    Private _LinesColor As Color = Color.Black
    Sub New()
        Me.SetStyle(ControlStyles.DoubleBuffer Or _
            ControlStyles.AllPaintingInWmPaint Or _
            ControlStyles.ResizeRedraw Or _
            ControlStyles.UserPaint Or _
            ControlStyles.Selectable Or _
            ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.Gray
        Font = New Font("Arial", 10)
        Size = New Size(125, 25)
        Invalidate()
    End Sub

    Public Property Angle As Integer
        Get
            Return _Angle
        End Get
        Set(value As Integer)
            _Angle = value : Invalidate()
        End Set
    End Property

    Public Property GlowEnabled As Boolean
        Get
            Return _GlowEnabled
        End Get
        Set(value As Boolean)
            _GlowEnabled = value : Invalidate()
        End Set
    End Property

    Public Property LinesEnabled As Boolean
        Get
            Return _LinesEnabled
        End Get
        Set(value As Boolean)
            _LinesEnabled = value : Invalidate()
        End Set
    End Property

    Public Property GroundColor As Color
        Get
            Return _GroundColor
        End Get
        Set(value As Color)
            _GroundColor = value : Invalidate()
        End Set
    End Property

    Public Property BarColor1 As Color
        Get
            Return _BarColor1
        End Get
        Set(value As Color)
            _BarColor1 = value : Invalidate()
        End Set
    End Property
    Public Property BarColor2 As Color
        Get
            Return _BarColor2
        End Get
        Set(value As Color)
            _BarColor2 = value : Invalidate()
        End Set
    End Property
    Public Property BarColor3 As Color
        Get
            Return _BarColor3
        End Get
        Set(value As Color)
            _BarColor3 = value : Invalidate()
        End Set
    End Property

    Public Property GlowColor As Color
        Get
            Return _GlowColor
        End Get
        Set(value As Color)
            _GlowColor = value : Invalidate()
        End Set
    End Property

    Event ValueChanged()
    Public Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal v As Integer)
            If v > MaxValue Or v < 0 Then
                _Value = _Value
            Else
                _Value = v : Invalidate()
            End If
            RaiseEvent ValueChanged()
        End Set
    End Property

    Public Property MaxValue As Integer
        Get
            Return _MaxValue
        End Get
        Set(ByVal value As Integer)
            _MaxValue = value : Invalidate()
        End Set
    End Property

    Public Property TextVisible As Boolean
        Get
            Return _ValueText
        End Get
        Set(ByVal value As Boolean)
            _ValueText = value : Invalidate()
        End Set
    End Property

    Public Property Symbol() As String
        Get
            Return _Symbol
        End Get
        Set(ByVal v As String)
            _Symbol = v : Invalidate()
        End Set
    End Property

    Public Property Thickness() As Integer
        Get
            Return _Thickness
        End Get
        Set(ByVal v As Integer)
            _Thickness = v : Invalidate()
        End Set
    End Property

    Public Property LinesColor As Color
        Get
            Return _LinesColor
        End Get
        Set(value As Color)
            _LinesColor = value : Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)

        Size = New Size(Width, Thickness + 10) : Me.Update()

        Dim Track As Size = New Size(20, 20)

        Dim TS As Integer

        Using B1 As New Bitmap(Width, Height)

            Using G As Graphics = Graphics.FromImage(B1)
                If TextVisible Then
                    TS = G.MeasureString(_Value & _Symbol, Font).Width - 10
                Else
                    TS = -10
                End If

                G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                G.Clear(BackColor)
                G.SmoothingMode = SmoothingMode.AntiAlias

                Dim TBrush1 As TextureBrush = NoiseBrush(New Color() {Color.FromArgb(10, BackColor), Color.FromArgb(10, Color.Gray), Color.FromArgb(10, Color.DarkGray)})
                G.FillRectangle(TBrush1, ClientRectangle)

                Dim Base As GraphicsPath = RoundRect(5, CInt((Height / 2) - CInt(Thickness / 2)), Width - 20 - TS, Thickness, CInt(Thickness / 2) - CInt(Thickness * 0.2)) 'Rounded 0.2 elipsis
                Dim BackLine As LinearGradientBrush = New LinearGradientBrush(New Point(5, CInt((Height / 2) - CInt(Thickness / 2))), New Point(5, CInt((Height / 2) + CInt(Thickness / 2))), Color.FromArgb(25, Color.Black), Color.FromArgb(25, GroundColor))
                G.FillPath(BackLine, Base)
                BackLine.Dispose()

                If Value <> 0 Then
                    Dim Bar As GraphicsPath = RoundRect(5, CInt((Height / 2) - CInt(Thickness / 2)), CInt((Width - 20 - TS) * (Value / MaxValue)), Thickness, CInt(Thickness / 2) - CInt(Thickness * 0.2))
                    If GlowEnabled Then
                        If Thickness >= 10 Then
                            DrawDropShadow(G, GlowColor, Bar, 3, 0, , 0.4, 3)
                            DrawDropShadow(G, GlowColor, Bar, 3, 90, , 0.4, 3)
                        End If
                    End If
                    Dim BarLGB1 As LinearGradientBrush = New LinearGradientBrush(New Rectangle(New Point(4, CInt((Height / 2) - CInt(Thickness / 2))), New Point(Width - 8 - TS, CInt((Height / 2 - TS) + CInt(Thickness / 2)))), Color.Transparent, Color.FromArgb(40, Color.Black), Angle)
                    Dim CB As New ColorBlend
                    CB.Colors = New Color() {BarColor1, BarColor2, BarColor3}
                    CB.Positions = New Single() {0.0F, 0.5F, 1.0F}
                    BarLGB1.InterpolationColors = CB
                    G.FillPath(BarLGB1, Bar)

                    Dim BarLGB2 As LinearGradientBrush = New LinearGradientBrush(New Point(1, CInt((Height / 2) - CInt(Thickness / 2))), New Point(1, CInt((Height / 2) + CInt(Thickness / 2))), Color.Transparent, Color.FromArgb(40, Color.Black))
                    G.FillPath(BarLGB2, Bar)

                    If LinesEnabled Then
                        Dim LinesLGB1 As New LinearGradientBrush(ClientRectangle, Color.Black, Color.Black, 90)
                        Dim CB2 As New ColorBlend
                        CB2.Colors = New Color() {Color.Transparent, Color.FromArgb(32, LinesColor), Color.Transparent}
                        CB2.Positions = New Single() {0, 1 / 2, 1}
                        LinesLGB1.InterpolationColors = CB2
                        For i = 5 To CInt((Width - TS) * (Value / MaxValue)) Step 45 'var
                            G.DrawLine(New Pen(LinesLGB1, 20), New Point(i, CInt((Height / 2) - Thickness * 2)), New Point(i - Thickness * 2, CInt((Height / 2) + Thickness * 2))) 'Bar on 20 and on size
                        Next
                    End If


                End If

                G.DrawPath(ToPen(50, Color.Black), Base)

                If TextVisible Then
                    G.DrawString(_Value & _Symbol, Font, New SolidBrush(ForeColor), New Point(Me.Width - 5 - G.MeasureString(_Value & Symbol, Font).Width, Me.Height / 2 - G.MeasureString(_Value & Symbol, Font).Height / 2 + 2))
                End If
            End Using
            e.Graphics.DrawImage(B1, 0, 0)
        End Using
    End Sub
End Class

Public Class Button
    Inherits Control

#Region "Var & Properties"
    Private _GroundColor1 As Color = Color.GreenYellow
    Private _GroundColor2 As Color = Color.Gold
    Private _GroundColor3 As Color = Color.DarkOrange
    Private _FrontColor As Color = Color.FromArgb(249, 247, 252)
    Private _GlowColor As Color = Color.Gold
    Private _SolidText As Boolean = True
    Private _DisabledColor As Color = Color.Silver

    Public Property DisabledColor As Color
        Get
            Return _DisabledColor
        End Get
        Set(value As Color)
            _DisabledColor = value : Invalidate()
        End Set
    End Property

    Public Property GlowColor As Color
        Get
            Return _GlowColor
        End Get
        Set(value As Color)
            _GlowColor = value : Invalidate()
        End Set
    End Property

    Public Property FrontColor As Color
        Get
            Return _FrontColor
        End Get
        Set(value As Color)
            _FrontColor = value : Invalidate()
        End Set
    End Property

    Public Property SolidText As Boolean
        Get
            Return _SolidText
        End Get
        Set(value As Boolean)
            _SolidText = value : Invalidate()
        End Set
    End Property

    Public Property GroundColor1 As Color
        Get
            Return _GroundColor1
        End Get
        Set(value As Color)
            _GroundColor1 = value : Invalidate()
        End Set
    End Property

    Public Property GroundColor2 As Color
        Get
            Return _GroundColor2
        End Get
        Set(value As Color)
            _GroundColor2 = value : Invalidate()
        End Set
    End Property

    Public Property GroundColor3 As Color
        Get
            Return _GroundColor3
        End Get
        Set(value As Color)
            _GroundColor3 = value : Invalidate()
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
        Size = New Size(100, 40)
        Font = New Font("Arial", 10)
        ForeColor = Color.Gray
        Invalidate()
    End Sub

    Private state As MouseState = 0

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)

        Using B1 As New Bitmap(Width, Height)
            Using G As Graphics = Graphics.FromImage(B1)
                G.SmoothingMode = SmoothingMode.AntiAlias
                G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                DoubleBuffered = True

                Dim TBrush1 As TextureBrush = NoiseBrush(New Color() {Color.FromArgb(10, BackColor), Color.FromArgb(10, Color.Gray), Color.FromArgb(10, Color.DarkGray)})
                G.FillRectangle(TBrush1, ClientRectangle)

                Dim BodyRec1 As Rectangle = New Rectangle(3, 3, Width - 6, Height - 6)
                Dim BodyRec2 As Rectangle = New Rectangle(5, 5, Width - 10, Height - 10)

                Dim BodyGP1 As GraphicsPath = RoundRect(BodyRec1, CInt(BodyRec1.Height / 2) - CInt(BodyRec1.Height * 0.14))
                Dim BodyGP2 As GraphicsPath = RoundRect(BodyRec2, CInt(BodyRec2.Height / 2) - CInt(BodyRec2.Height * 0.14))

                Dim BodyLGB1 As New LinearGradientBrush(ClientRectangle, Color.Black, Color.Black, 45)
                Dim LineLGB1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(64, Color.Black), Color.FromArgb(64, Color.White), 90)
                Dim FrontLGB1 As New LinearGradientBrush(ClientRectangle, Color.Black, Color.Black, 45)



                Dim CB2 As New ColorBlend
                CB2.Colors = New Color() {Color.DimGray, Color.DarkGray, Color.Silver, Color.Gray, Color.DarkGray, Color.Gray, Color.Gainsboro, Color.DarkGray}
                CB2.Positions = New Single() {0.0F, 0.2F, 0.4F, 0.5F, 0.6F, 0.8F, 0.9F, 1.0F}
                FrontLGB1.InterpolationColors = CB2
                Dim CB1 As New ColorBlend

                Select Case state
                    Case MouseState.Normal
                        CB1.Colors = New Color() {GroundColor1, GroundColor2, GroundColor3}
                        CB1.Positions = New Single() {0.0F, 0.5F, 1.0F}
                        BodyLGB1.InterpolationColors = CB1
                        G.FillPath(BodyLGB1, BodyGP1)
                    Case MouseState.Hovered
                        DrawDropShadow(G, GlowColor, BodyGP1, 3, , , 1, 3)
                        CB1.Colors = New Color() {GroundColor1, GroundColor2, GroundColor3}
                        CB1.Positions = New Single() {0.0F, 0.5F, 1.0F}
                        BodyLGB1.InterpolationColors = CB1
                        G.FillPath(BodyLGB1, BodyGP1)
                    Case MouseState.Pushed
                        CB1.Colors = New Color() {Color.FromArgb(200, GroundColor3), Color.FromArgb(100, GroundColor2), Color.FromArgb(200, GroundColor1)}
                        CB1.Positions = New Single() {0.0F, 0.5F, 1.0F}
                        BodyLGB1.InterpolationColors = CB1
                        G.FillPath(BodyLGB1, BodyGP1)
                    Case MouseState.Disabled
                        CB1.Colors = New Color() {DisabledColor, DisabledColor}
                        CB1.Positions = New Single() {0.0F, 1.0F}
                        BodyLGB1.InterpolationColors = CB1
                        G.FillPath(BodyLGB1, BodyGP1)
                End Select

                G.FillPath(New SolidBrush(FrontColor), BodyGP2)
                G.FillPath(TBrush1, BodyGP2)

                G.DrawPath(New Pen(LineLGB1, 1), BodyGP1)

                G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(64, Color.White)), New Point(CInt(Width / 2) - CInt(G.MeasureString(Text, Font).Width / 2) - 1, CInt(Height / 2) - CInt(G.MeasureString(Text, Font).Height / 2)))

                If SolidText Then : G.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(CInt(Width / 2) - CInt(G.MeasureString(Text, Font).Width / 2), CInt(Height / 2) - CInt(G.MeasureString(Text, Font).Height / 2 - 1)))
                Else : G.DrawString(Text, Font, BodyLGB1, New Point(CInt(Width / 2) - CInt(G.MeasureString(Text, Font).Width / 2), CInt(Height / 2) - CInt(G.MeasureString(Text, Font).Height / 2 - 1)))
                End If
            End Using
            e.Graphics.DrawImage(B1, 0, 0)
        End Using
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MyBase.OnMouseEnter(e) : state = MouseState.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MyBase.OnMouseLeave(e) : state = MouseState.Normal : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseDown(e) : state = MouseState.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e) : state = MouseState.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        If Me.Enabled Then
            state = MouseState.Normal
        Else
            state = MouseState.Disabled
        End If
        MyBase.OnEnabledChanged(e) : Invalidate()
    End Sub
End Class

Class PlainTabControl
    Inherits TabControl

    Private _BaseColor As Color = Color.Transparent
    Public Property BaseColor As Color
        Get
            Return _BaseColor
        End Get
        Set(value As Color)
            _BaseColor = value : Invalidate()
        End Set
    End Property

    Private _TabPagesColor As Color = Color.WhiteSmoke
    Public Property TabPagesColor As Color
        Get
            Return _TabPagesColor
        End Get
        Set(value As Color)
            _TabPagesColor = value : Invalidate()
        End Set
    End Property

    Private _TabColor1 As Color = Color.GreenYellow
    Public Property TabColor1 As Color
        Get
            Return _TabColor1
        End Get
        Set(value As Color)
            _TabColor1 = value : Invalidate()
        End Set
    End Property

    Private _TabColor2 As Color = Color.Gold
    Public Property TabColor2 As Color
        Get
            Return _TabColor2
        End Get
        Set(value As Color)
            _TabColor2 = value : Invalidate()
        End Set
    End Property

    Private _TabColor3 As Color = Color.DarkOrange
    Public Property TabColor3 As Color
        Get
            Return _TabColor3
        End Get
        Set(value As Color)
            _TabColor3 = value : Invalidate()
        End Set
    End Property

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer Or _
        ControlStyles.SupportsTransparentBackColor, True)
        Size = New Size(250, 150)
        Invalidate()
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        SizeMode = TabSizeMode.Normal
        ItemSize = New Size(1, 1)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B1 As New Bitmap(Width, Height)
            Using G As Graphics = Graphics.FromImage(B1)
                G.SmoothingMode = SmoothingMode.AntiAlias
                G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                DoubleBuffered = True

                G.Clear(BaseColor)
                Dim TBrush1 As TextureBrush = NoiseBrush(New Color() {Color.FromArgb(10, BaseColor), Color.FromArgb(10, Color.Gray), Color.FromArgb(10, Color.DarkGray)})
                G.FillRectangle(TBrush1, ClientRectangle)

                Dim BorderLGB1 As New LinearGradientBrush(ClientRectangle, Color.FromArgb(64, Color.Black), Color.FromArgb(64, Color.White), 90)
                Dim BodyLGB1 As New LinearGradientBrush(ClientRectangle, Color.Black, Color.Black, 45)

                Dim CB1 As New ColorBlend
                CB1.Colors = New Color() {TabColor1, TabColor2, TabColor3}
                CB1.Positions = New Single() {0.0F, 0.5F, 1.0F}
                BodyLGB1.InterpolationColors = CB1



                G.FillPath(BodyLGB1, RoundRect(0, 0, Width - 1, Height - 1, 4))
                G.DrawPath(New Pen(BorderLGB1, 1), RoundRect(0, 0, Width - 1, Height - 1, 4))
                For TabItemIndex As Integer = 0 To Me.TabCount - 1
                    Try : TabPages(TabItemIndex).BackColor = TabPagesColor : Catch : End Try
                Next
            End Using
            e.Graphics.DrawImage(B1, 0, 0)
        End Using
    End Sub

End Class

Class TabControlController
    Inherits Control

    Enum OrientationMode
        Vertical = 0
        Horizontal = 1
    End Enum
    Private _Orientation As OrientationMode = OrientationMode.Horizontal
    Public Property Orientation As OrientationMode
        Get
            Return _Orientation
        End Get
        Set(value As OrientationMode)
            _Orientation = value : Invalidate()
        End Set
    End Property

    Private _NoShape As Boolean = False
    Public Property NoShape As Boolean
        Get
            Return _NoShape
        End Get
        Set(value As Boolean)
            _NoShape = value : Invalidate()
        End Set
    End Property

    Private _BallSize As Integer = 10
    Public Property BallSize As Integer
        Get
            Return _BallSize
        End Get
        Set(value As Integer)
            _BallSize = value : Invalidate()
        End Set
    End Property

    Private WithEvents _owner As TabControl
    Public Property Owner() As TabControl
        Get
            Return _owner
        End Get
        Set(ByVal value As TabControl)
            _owner = value : Invalidate()
        End Set
    End Property

    Private _BallGroudColor As Color = Color.Black
    Public Property BallGroudColor As Color
        Get
            Return _BallGroudColor
        End Get
        Set(value As Color)
            _BallGroudColor = value : Invalidate()
        End Set
    End Property

    Private _BaseColor1 As Color = Color.GreenYellow
    Public Property BaseColor1 As Color
        Get
            Return _BaseColor1
        End Get
        Set(value As Color)
            _BaseColor1 = value : Invalidate()
        End Set
    End Property

    Private _BaseColor2 As Color = Color.Gold
    Public Property BaseColor2 As Color
        Get
            Return _BaseColor2
        End Get
        Set(value As Color)
            _BaseColor2 = value : Invalidate()
        End Set
    End Property

    Private _BaseColor3 As Color = Color.DarkOrange
    Public Property BaseColor3 As Color
        Get
            Return _BaseColor3
        End Get
        Set(value As Color)
            _BaseColor3 = value : Invalidate()
        End Set
    End Property

    Sub Hook() Handles _owner.SelectedIndexChanged, _owner.TabIndexChanged
        Invalidate()
    End Sub

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer Or _
        ControlStyles.SupportsTransparentBackColor, True)
        Size = New Size(216, 24)
        Invalidate()
    End Sub

    Private Offset As Integer = BallSize

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)

        Using B1 As New Bitmap(Width, Height)
            Using G As Graphics = Graphics.FromImage(B1)
                G.SmoothingMode = SmoothingMode.AntiAlias
                G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                DoubleBuffered = True

                G.Clear(BackColor)
                Dim TBrush1 As TextureBrush = NoiseBrush(New Color() {Color.FromArgb(10, BackColor), Color.FromArgb(10, Color.Gray), Color.FromArgb(10, Color.DarkGray)})
                G.FillRectangle(TBrush1, ClientRectangle)

                Dim BaseLGB1 As New LinearGradientBrush(ClientRectangle, Color.Black, Color.Black, 45)
                Dim CB1 As New ColorBlend
                CB1.Colors = New Color() {BaseColor1, BaseColor2, BaseColor3}
                CB1.Positions = New Single() {0.0F, 0.5F, 1.0F}
                BaseLGB1.InterpolationColors = CB1

                If Orientation = OrientationMode.Horizontal Then
                    Dim Roundness As Integer = Height / 3

                    Dim BaseGP As GraphicsPath = RoundRect(1, 1, Width - 2, Height - 2, Roundness)
                    Dim BodyGP As GraphicsPath = RoundRect(3, 3, Width - 6, Height - 6, Roundness)

                    If NoShape = False Then
                        G.FillPath(BaseLGB1, BaseGP)
                        G.DrawPath(New Pen(Color.FromArgb(70, Color.Black)), BaseGP)
                    End If
                    G.FillPath(ToBrush(BackColor), BodyGP)
                    G.FillPath(TBrush1, BodyGP)

                    Try
                        If Not Owner Is Nothing Then
                            Dim TotalOffset As Integer = (10 + Offset) * Owner.TabPages.Count - Offset
                            Dim FarLeft As Integer = Width / 2 - TotalOffset / 2

                            For i = 0 To Owner.TabPages.Count - 1
                                Dim CurrentRectangle As Rectangle = New Rectangle(FarLeft + i * (10 + Offset), Height / 2 - 5, BallSize, BallSize)
                                G.FillEllipse(New LinearGradientBrush(CurrentRectangle.Location, New Point(CurrentRectangle.X, CurrentRectangle.Y + 10), _
                                                                      Color.FromArgb(200, BallGroudColor), Color.FromArgb(150, BallGroudColor)), CurrentRectangle)
                                G.DrawEllipse(ToPen(70, 128, 128, 128), CurrentRectangle)
                            Next
                            If Not Owner.TabPages.Count <= 0 Then
                                Dim SelectedRectangle As Rectangle = New Rectangle(FarLeft + Owner.SelectedIndex * (10 + Offset), Height / 2 - 5, BallSize, BallSize)
                                SelectedRectangle.Inflate(2, 2)
                                G.FillEllipse(BaseLGB1, SelectedRectangle)
                                G.FillEllipse(New SolidBrush(Color.FromArgb(32, Color.Black)), SelectedRectangle)
                                SelectedRectangle.Inflate(-3, -3)
                                G.DrawEllipse(New Pen(BackColor, 3), SelectedRectangle)
                                G.DrawEllipse(ToPen(30, Color.Black), SelectedRectangle)
                            End If
                        End If
                    Catch : End Try
                End If

                If Orientation = OrientationMode.Vertical Then
                    Dim Roundness As Integer = Width / 3

                    Dim BaseGP As GraphicsPath = RoundRect(1, 1, Width - 2, Height - 2, Roundness)
                    Dim BodyGP As GraphicsPath = RoundRect(3, 3, Width - 6, Height - 6, Roundness)

                    If NoShape = False Then
                        G.FillPath(BaseLGB1, BaseGP)
                        G.DrawPath(New Pen(Color.FromArgb(70, Color.Black)), BaseGP)
                    End If
                    G.FillPath(ToBrush(BackColor), BodyGP)
                    G.FillPath(TBrush1, BodyGP)

                    Try
                        If Not Owner Is Nothing Then
                            Dim TotalOffset As Integer = (BallSize + Offset) * Owner.TabPages.Count - Offset
                            Dim FarLeft As Integer = Height / 2 - TotalOffset / 2

                            For i = 0 To Owner.TabPages.Count - 1
                                Dim CurrentRectangle As Rectangle = New Rectangle(Width / 2 - BallSize / 2, FarLeft + i * (BallSize + Offset), BallSize, BallSize)
                                G.FillEllipse(New LinearGradientBrush(CurrentRectangle.Location, New Point(CurrentRectangle.X, CurrentRectangle.Y + BallSize), _
                                                                      Color.FromArgb(200, BallGroudColor), Color.FromArgb(150, BallGroudColor)), CurrentRectangle)
                                G.DrawEllipse(ToPen(70, 128, 128, 128), CurrentRectangle)
                            Next
                            If Not Owner.TabPages.Count <= 0 Then
                                Dim SelectedRectangle As Rectangle = New Rectangle(Width / 2 - BallSize / 2, FarLeft + Owner.SelectedIndex * (10 + Offset), BallSize, BallSize)
                                SelectedRectangle.Inflate(2, 2)
                                G.FillEllipse(BaseLGB1, SelectedRectangle)
                                G.FillEllipse(New SolidBrush(Color.FromArgb(32, Color.Black)), SelectedRectangle)
                                SelectedRectangle.Inflate(-3, -3)
                                G.DrawEllipse(New Pen(BackColor, 3), SelectedRectangle)
                                G.DrawEllipse(ToPen(30, Color.Black), SelectedRectangle)
                            End If
                        End If
                    Catch : End Try
                End If
            End Using
            e.Graphics.DrawImage(B1, 0, 0)
        End Using
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseUp(e)
        Dim TotalOffset As Integer = (10 + Offset) * Owner.TabPages.Count - Offset
        If Orientation = OrientationMode.Horizontal Then
            Dim FarLeft As Integer = Width / 2 - TotalOffset / 2

            For i = 0 To Owner.TabPages.Count - 1
                Dim CurrentRectangle As Rectangle = New Rectangle(FarLeft + i * (10 + Offset), Height / 2 - 5, BallSize, BallSize)

                If CurrentRectangle.Contains(e.Location) Then
                    _owner.SelectedIndex = i
                    Exit For
                End If
            Next
        Else
            Dim FarLeft As Integer = Height / 2 - TotalOffset / 2

            For i = 0 To Owner.TabPages.Count - 1
                Dim CurrentRectangle As Rectangle = New Rectangle(Width / 2 - 5, FarLeft + i * (10 + Offset), BallSize, BallSize)

                If CurrentRectangle.Contains(e.Location) Then
                    _owner.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
    End Sub

End Class

Public Class FormSkin
    Inherits ContainerControl

    Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(ByVal v As String)
            MyBase.Text = v
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        ParentForm.FormBorderStyle = FormBorderStyle.None
        MyBase.OnHandleCreated(e)
    End Sub

    Private _Header As Integer = 38
    Public Property HeaderSize As Integer
        Get
            Return _Header
        End Get
        Set(value As Integer)
            _Header = value : Invalidate()
        End Set
    End Property

    Private _TextOffset As Integer = 0
    Public Property TextOffset As Integer
        Get
            Return _TextOffset
        End Get
        Set(value As Integer)
            _TextOffset = value : Invalidate()
        End Set
    End Property

    Private _Color1 As Color = Color.DarkOrange
    Public Property Color1 As Color
        Get
            Return _Color1
        End Get
        Set(value As Color)
            _Color1 = value : Invalidate()
        End Set
    End Property

    Private _Color2 As Color = Color.Gold
    Public Property Color2 As Color
        Get
            Return _Color2
        End Get
        Set(value As Color)
            _Color2 = value : Invalidate()
        End Set
    End Property

    Private _Color3 As Color = Color.GreenYellow
    Public Property Color3 As Color
        Get
            Return _Color3
        End Get
        Set(value As Color)
            _Color3 = value : Invalidate()
        End Set
    End Property

#Region " Sizing and Movement "

    Private _Resizable As Boolean = True
    Property Resizable() As Boolean
        Get
            Return _Resizable
        End Get
        Set(ByVal value As Boolean)
            _Resizable = value
        End Set
    End Property

    Private _MoveHeight As Integer = _Header
    Property MoveHeight() As Integer
        Get
            Return _MoveHeight
        End Get
        Set(ByVal v As Integer)
            _MoveHeight = v
            Header = New Rectangle(7, 7, Width - 14, _MoveHeight - 2)
        End Set
    End Property

    Private Flag As IntPtr
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If Not e.Button = MouseButtons.Left Then Return

        If Header.Contains(e.Location) Then
            Flag = New IntPtr(2)
        ElseIf Current.Position = 0 Or Not _Resizable Then
            Return
        Else
            Flag = New IntPtr(Current.Position)
        End If

        Capture = False
        DefWndProc(Message.Create(Parent.Handle, 161, Flag, Nothing))

        MyBase.OnMouseDown(e)
    End Sub


    Private Structure Pointer
        ReadOnly Cursor As Cursor, Position As Byte
        Sub New(ByVal c As Cursor, ByVal p As Byte)
            Cursor = c
            Position = p
        End Sub
    End Structure

    Private F1, F2, F3, F4 As Boolean, PTC As Point
    Private Function GetPointer() As Pointer
        If Not FindForm.WindowState = FormWindowState.Maximized Then
            PTC = PointToClient(MousePosition)
            F1 = PTC.X < 4
            F2 = PTC.X > Width - 4
            F3 = PTC.Y < 4
            F4 = PTC.Y > Height - 4

            If F1 And F3 Then Return New Pointer(Cursors.SizeNWSE, 13)
            If F1 And F4 Then Return New Pointer(Cursors.SizeNESW, 16)
            If F2 And F3 Then Return New Pointer(Cursors.SizeNESW, 14)
            If F2 And F4 Then Return New Pointer(Cursors.SizeNWSE, 17)
            If F1 Then Return New Pointer(Cursors.SizeWE, 10)
            If F2 Then Return New Pointer(Cursors.SizeWE, 11)
            If F3 Then Return New Pointer(Cursors.SizeNS, 12)
            If F4 Then Return New Pointer(Cursors.SizeNS, 15)
            Return New Pointer(Cursors.Default, 0)
        End If
        Return Nothing
    End Function

    Private Current, Pending As Pointer
    Private Sub SetCurrent()
        Pending = GetPointer()
        If Current.Position = Pending.Position Then Return
        Current = GetPointer()
        Cursor = Current.Cursor
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If _Resizable Then SetCurrent()
        MyBase.OnMouseMove(e)
    End Sub

    Protected Header As Rectangle
    Protected Overrides Sub OnSizeChanged(ByVal e As EventArgs)
        If Width = 0 OrElse Height = 0 Then Return
        Header = New Rectangle(7, 7, Width - 14, _MoveHeight - 2)
        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub
    Protected Overrides Sub OnCreateControl()
        FindForm.TransparencyKey = Color.Fuchsia
        MyBase.OnCreateControl()
    End Sub
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer Or _
        ControlStyles.SupportsTransparentBackColor, True)
        Dock = DockStyle.Fill
        ForeColor = Color.Gray
        Font = New Font("Arial", 10, FontStyle.Bold)
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Using B1 As New Bitmap(Width, Height)
            Using G As Graphics = Graphics.FromImage(B1)
                G.SmoothingMode = SmoothingMode.AntiAlias
                G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                DoubleBuffered = True

                G.Clear(BackColor)

                Dim BaseR As New Rectangle(0, 0, Width - 1, Height - 1)
                Dim BaseGP1 As GraphicsPath = RoundRect(BaseR, 3)
                Dim TBrush1 As TextureBrush = NoiseBrush(New Color() {Color.FromArgb(10, BackColor), Color.FromArgb(10, Color.Gray), Color.FromArgb(10, Color.DarkGray)})

                G.DrawPath(New Pen(Color.FromArgb(64, Color.Black)), BaseGP1)
                G.FillPath(TBrush1, BaseGP1)


                Dim BaseLGB1 As New LinearGradientBrush(ClientRectangle, Color.Black, Color.Black, 0)
                Dim CB1 As New ColorBlend
                CB1.Colors = New Color() {Color1, Color2, Color3}
                CB1.Positions = New Single() {0.0F, 0.5F, 1.0F}
                BaseLGB1.InterpolationColors = CB1

                G.DrawLine(New Pen(BaseLGB1, 4), 0, _Header - 2, Width, _Header - 2)
                G.DrawPath(New Pen(BaseLGB1, 3), BaseGP1)
                G.DrawPath(New Pen(New LinearGradientBrush(ClientRectangle, BackColor, Color.FromArgb(128, BackColor), 90), 4), BaseGP1)

                G.DrawString(ParentForm.Text, Font, ToBrush(ForeColor), New Point(CInt(Width / 2) - CInt(G.MeasureString(ParentForm.Text, Font).Width / 2) - TextOffset, CInt(HeaderSize / 2) - CInt(G.MeasureString(ParentForm.Text, Font).Height / 2)))


                drawcorners(B1, FindForm.TransparencyKey)
            End Using
            e.Graphics.DrawImage(B1, 0, 0)
        End Using
    End Sub
End Class

Public Class TopButton
    Inherits Control

    Enum Button
        ExitButton = 0
        MinimizeButton = 1
        MaximizeButton = 2
        HelpButton = 3
    End Enum

    Private _Color As Color = Color.GreenYellow
    Private _ButtonMode As Button = Button.ExitButton

    Public Property Color As Color
        Get
            Return _Color
        End Get
        Set(value As Color)
            _Color = value : Invalidate()
        End Set
    End Property

    Public Property ButtonMode As Button
        Get
            Return _ButtonMode
        End Get
        Set(value As Button)
            _ButtonMode = value : Invalidate()
        End Set
    End Property

    Private State As MouseState = MouseState.Normal

    Sub New()
        Size = New Size(21, 21)
        State = MouseState.Normal
        BackColor = Drawing.Color.WhiteSmoke
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Using B1 As New Bitmap(Width, Height)
            Using G As Graphics = Graphics.FromImage(B1)
                G.SmoothingMode = SmoothingMode.AntiAlias
                G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                DoubleBuffered = True

                G.Clear(BackColor)
                Dim R1 As Rectangle = New Rectangle(3, 3, 15, 15)
                Dim TBrush1 As TextureBrush = NoiseBrush(New Color() {Color.FromArgb(10, BackColor), Color.FromArgb(10, Color.Gray), Color.FromArgb(10, Color.DarkGray)})
                G.FillRectangle(TBrush1, ClientRectangle)

                Select Case State
                    Case MouseState.Normal
                        G.FillEllipse(New SolidBrush(Color), R1)
                        R1.Inflate(-2, -2)

                        G.FillEllipse(New SolidBrush(BackColor), R1)
                        G.FillEllipse(TBrush1, R1)
                        R1.Inflate(-2, -2)

                        G.FillEllipse(New SolidBrush(Color), R1)
                    Case MouseState.Hovered
                        G.FillEllipse(New SolidBrush(Color), R1)
                        R1.Inflate(-2, -2)

                        G.FillEllipse(New SolidBrush(BackColor), R1)
                        G.FillEllipse(TBrush1, R1)

                        If ButtonMode = Button.ExitButton Then
                            G.DrawString("X", New Font("Arial", 9, FontStyle.Bold), ToBrush(Color), 5, 5)
                        ElseIf ButtonMode = Button.MinimizeButton Then
                            G.DrawString("6", New Font("Marlett", 9, FontStyle.Bold), ToBrush(Color), 3, 6)
                        ElseIf ButtonMode = Button.MaximizeButton Then
                            If FindForm.WindowState = FormWindowState.Normal Then
                                G.DrawString("1", New Font("Marlett", 7, FontStyle.Bold), ToBrush(Color), 5, 7)
                            ElseIf FindForm.WindowState = FormWindowState.Maximized Then
                                G.DrawString("2", New Font("Marlett", 6, FontStyle.Bold), ToBrush(Color), 5, 7)
                            End If
                        ElseIf ButtonMode = Button.HelpButton Then
                            G.DrawString("?", New Font("Arial", 9, FontStyle.Bold), ToBrush(Color), 5, 5)
                        End If
                    Case MouseState.Pushed

                    Case MouseState.Disabled

                End Select

            End Using
            e.Graphics.DrawImage(B1, 0, 0)
        End Using
    End Sub

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e) : State = MouseState.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e) : State = MouseState.Pushed : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        If ButtonMode = Button.ExitButton Then
            FindForm.Close()
        ElseIf ButtonMode = Button.MinimizeButton Then
            FindForm.WindowState = FormWindowState.Minimized
        ElseIf ButtonMode = Button.MaximizeButton Then
            If FindForm.WindowState = FormWindowState.Maximized Then
                FindForm.WindowState = FormWindowState.Normal
            ElseIf FindForm.WindowState = FormWindowState.Normal Then
                FindForm.WindowState = FormWindowState.Maximized
            End If
        End If
        MyBase.OnMouseUp(e) : State = MouseState.Hovered : Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e) : State = MouseState.Normal : Invalidate()
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        If Enabled Then
            : State = MouseState.Normal : Invalidate()
        Else
            : State = MouseState.Disabled : Invalidate()
        End If
        MyBase.OnEnabledChanged(e)
    End Sub
End Class

<DefaultEvent("CheckedChanged")> _
Public Class CheckBox
    Inherits Control

    Private _Checked As Boolean
    Private State As MouseState = MouseState.Normal
    Private _CheckedColor As Color = Color.FromArgb(173, 173, 174)
    Private _SolidCheckedColor As Boolean = False
    Private _BaseColor As Color = Color.WhiteSmoke
    Private _Color1 As Color = Color.DarkOrange
    Private _Color2 As Color = Color.Gold
    Private _Color3 As Color = Color.LawnGreen
    Private _BackColor As Color = Color.WhiteSmoke
    Private _ForeColor As Color = Color.Gray

#Region "Properties"

    <Category("Colors")> _
    Public Property BaseColor() As Color
        Get
            Return _BaseColor
        End Get
        Set(ByVal value As Color)
            _BaseColor = value : Invalidate()
        End Set
    End Property

    <Category("Colors")> _
    Public Property Color1() As Color
        Get
            Return _Color1
        End Get
        Set(ByVal value As Color)
            _Color1 = value : Invalidate()
        End Set
    End Property

    <Category("Colors")> _
    Public Property Color2() As Color
        Get
            Return _Color2
        End Get
        Set(ByVal value As Color)
            _Color2 = value : Invalidate()
        End Set
    End Property

    <Category("Colors")> _
    Public Property Color3() As Color
        Get
            Return _Color3
        End Get
        Set(ByVal value As Color)
            _Color3 = value : Invalidate()
        End Set
    End Property

    <Category("Colors")> _
    Public Property CheckedColor() As Color
        Get
            Return _CheckedColor
        End Get
        Set(ByVal value As Color)
            _CheckedColor = value : Invalidate()
        End Set
    End Property

    <Category("Colors")> _
    Public Property SolidCheckedColor() As Boolean
        Get
            Return _SolidCheckedColor
        End Get
        Set(ByVal value As Boolean)
            _SolidCheckedColor = value : Invalidate()
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

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Height = 25
    End Sub
#End Region

#Region "Draw Control"
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                   ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer _
                   Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        Size = New Size(100, 25)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B1 As New Bitmap(Width, Height)
            Using G As Graphics = Graphics.FromImage(B1)
                G.SmoothingMode = SmoothingMode.AntiAlias
                G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                G.PixelOffsetMode = PixelOffsetMode.HighQuality
                DoubleBuffered = True

                G.Clear(BackColor)
                Dim TBrush1 As TextureBrush = NoiseBrush(New Color() {Color.FromArgb(10, BackColor), Color.FromArgb(10, Color.Gray), Color.FromArgb(10, Color.DarkGray)})
                G.FillRectangle(TBrush1, ClientRectangle)



                Dim CBase As New Rectangle(4, 4, 17, 17)

                Select Case State
                    Case MouseState.Hovered
                        CBase.Inflate(2, 2)
                    Case MouseState.Pushed
                        CBase.Inflate(-1, -1)
                End Select
                Dim GP1 As GraphicsPath = RoundRect(CBase, 3)
                CBase.Inflate(-3, -3)
                Dim GP2 As GraphicsPath = RoundRect(CBase, 3)

                Dim LGB1 As New LinearGradientBrush(New Point(1, 1), New Point(23, 23), Color.Black, Color.Black)
                Dim CB1 As New ColorBlend
                CB1.Colors = New Color() {Color1, Color2, Color3}
                CB1.Positions = New Single() {0.0F, 0.5F, 1.0F}
                LGB1.InterpolationColors = CB1

                G.FillPath(LGB1, GP1)
                G.FillPath(ToBrush(BaseColor), GP2)
                If Checked Then
                    Dim P() As Point = {New Point(6, 14), New Point(8, 11), New Point(11, 15), New Point(17, 6), New Point(19, 9), New Point(11, 19)}
                    If SolidCheckedColor Then
                        G.FillPolygon(New SolidBrush(_CheckedColor), P)
                    Else
                        G.FillPolygon(LGB1, P)
                    End If
                End If
                G.DrawString(Text, Font, New SolidBrush(ForeColor), New Rectangle(24, 1, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
                G.Dispose()
                MyBase.OnPaint(e)
            End Using
            e.Graphics.DrawImage(B1, 0, 0) : B1.Dispose()
        End Using
    End Sub
#End Region

#Region "Mouse States"

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        State = MouseState.Pushed : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        State = MouseState.Hovered : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseEnter(ByVal e As EventArgs)
        MyBase.OnMouseEnter(e)
        State = MouseState.Hovered : Invalidate()
    End Sub
    Protected Overrides Sub OnMouseLeave(ByVal e As EventArgs)
        MyBase.OnMouseLeave(e)
        State = MouseState.Normal : Invalidate()
    End Sub

#End Region

End Class