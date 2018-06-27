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
 
    Public Sub DrawTriangle(ByVal G As Graphics, ByVal C As Color, ByVal Size As Integer, ByVal P1_0 As Point, ByVal P1_1 As Point, ByVal P2_0 As Point, ByVal P2_1 As Point, ByVal P3_0 As Point, ByVal P3_1 As Point)
        With G
            .DrawLine(New Pen(C, Size), P1_0, P1_1)
            .DrawLine(New Pen(C, Size), P2_0, P2_1)
            .DrawLine(New Pen(C, Size), P3_0, P3_1)
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
        G.DrawString(Text, font, brush, New Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
    End Sub
 
    Public Sub LeftString(ByVal G As Graphics, ByVal Text As String, ByVal font As Font, ByVal brush As Brush, ByVal Rect As Rectangle)
        G.DrawString(Text, font, brush, New Rectangle(4, Rect.Y + (Rect.Height / 2) - (G.MeasureString(Text, font).Height / 2) + 0, Rect.Width, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Near})
    End Sub
 
    Public Sub RightString(ByVal G As Graphics, ByVal Text As String, ByVal font As Font, ByVal brush As Brush, ByVal Rect As Rectangle)
        G.DrawString(Text, font, brush, New Rectangle(4, Rect.Y + (Rect.Height / 2) - (G.MeasureString(Text, font).Height / 2), Rect.Width - Rect.Height + 10, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Far})
    End Sub
 
 
#Region " Reond Border "
 
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
 
#Region " Skin "
 
Public Class PaleSkin : Inherits ContainerControl
 
#Region " Variables "
 
    Private Movable As Boolean = False
    Private MousePoint As New Point(0, 0)
    Private MoveHeight = 50
    Private _TitleTextPostion As TitlePostion = TitlePostion.Left
 
#End Region
 
#Region " Constructors "
 
    Sub New()
        SetStyle(ControlStyles.Opaque Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 14, FontStyle.Bold)
        UpdateStyles()
    End Sub
 
#End Region
 
#Region " Properties "
 
    Private _ShowIcon As Boolean
    Property ShowIcon As Boolean
        Get
            Return _ShowIcon
        End Get
        Set(ByVal value As Boolean)
            If value = _ShowIcon Then Return
            FindForm.ShowIcon = value
            _ShowIcon = value
            Invalidate()
        End Set
    End Property
 
    Public Overridable Property TitleTextPostion As TitlePostion
        Get
            Return _TitleTextPostion
        End Get
        Set(value As TitlePostion)
            _TitleTextPostion = value
            Invalidate()
        End Set
    End Property
 
    Enum TitlePostion
        Left
        Center
        Right
    End Enum
 
#End Region
 
#Region " Draw Control "
 
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
 
            With G
 
                .Clear(Color.Fuchsia)
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Dim R As New Rectangle(-1, -1, Width, Height)
 
                Using LGB As New LinearGradientBrush(R, GetHTMLColor("f7f7f7"), Color.White, 90.0F)
                    FillRoundedPath(G, LGB, R, 12)
                    DrawRoundedPath(G, GetHTMLColor("f7f7f7"), 2, R, 12)
                    .DrawLine(PenHTMlColor("dadada", 1.5), New Point(12, 55), New Point(Width - 20, 55))
                End Using
 
                If FindForm.ShowIcon Then
                    If Not FindForm.Icon Is Nothing Then
                        Select Case TitleTextPostion
                            Case TitlePostion.Left
                                .DrawString(Text, Font, SolidBrushHTMlColor("2e8fc7"), 27, 14)
                                .DrawIcon(FindForm.Icon, New Rectangle(6, 17, 20, 20))
                            Case TitlePostion.Center
                                CentreString(G, Text, Font, SolidBrushHTMlColor("2e8fc7"), New Rectangle(0, 0, Width, 50))
                                .DrawIcon(FindForm.Icon, New Rectangle(5, 17, 20, 20))
                            Case TitlePostion.Right
                                RightString(G, Text, Font, SolidBrushHTMlColor("2e8fc7"), New Rectangle(0, 0, Width, 50))
                                .DrawIcon(FindForm.Icon, New Rectangle(Width - 30, 17, 20, 20))
                        End Select
                    End If
                Else
                    Select Case TitleTextPostion
                        Case TitlePostion.Left
                            .DrawString(Text, Font, SolidBrushHTMlColor("2e8fc7"), 5, 14)
                        Case TitlePostion.Center
                            CentreString(G, Text, Font, SolidBrushHTMlColor("2e8fc7"), New Rectangle(0, 0, Width, 50))
                        Case TitlePostion.Right
                            RightString(G, Text, Font, SolidBrushHTMlColor("2e8fc7"), New Rectangle(0, 0, Width, 50))
                    End Select
                End If
 
            End With
 
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
 
        End Using
    End Sub
 
#End Region
 
#Region " Events "
 
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button = Windows.Forms.MouseButtons.Left And New Rectangle(0, 0, Width, MoveHeight).Contains(e.Location) Then
            Movable = True
            MousePoint = e.Location
        End If
    End Sub
 
    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        Movable = False
    End Sub
 
    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If Movable Then Parent.Location = MousePosition - MousePoint
    End Sub
 
    Protected NotOverridable Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        ParentForm.FormBorderStyle = FormBorderStyle.None
        ParentForm.AllowTransparency = True
        ParentForm.TransparencyKey = Color.Fuchsia
        Dock = DockStyle.Fill
    End Sub
 
#End Region
 
End Class
 
#End Region
 
#Region " Light Button "
 
Public Class PaleButton : Inherits Control
 
#Region " Variables "
 
    Private State As MouseMode
    Private _SideImage As Image
    Private _SideImageAlign As SideAligin = SideAligin.Left
    Private _RoundRadius As Integer = 10
 
#End Region
 
#Region " Constructors "
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
        ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Myriad Pro", 12, FontStyle.Bold)
        UpdateStyles()
    End Sub
 
#End Region
 
#Region " Properties "
 
    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property
 
    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property
 
    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property
 
#End Region
 
#Region " Enumerators "
 
    Enum SideAligin
        Left
        Right
    End Enum
 
#End Region
 
#Region " Draw Control "
 
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
 
            With G
                Dim Rect As New Rectangle(1, 1, Width - 2, Height - 2)
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Select Case State
                    Case MouseMode.NormalMode
                        Using LGB As New LinearGradientBrush(Rect, Color.FromArgb(180, 250, 250, 250), Color.FromArgb(200, 250, 250, 250), 90S)
                            FillRoundedPath(G, LGB, Rect, RoundRadius)
                        End Using
                        DrawRoundedPath(G, GetHTMLColor("e0e0e0"), 1, Rect, RoundRadius)
                        CentreString(G, Text, Font, SolidBrushHTMlColor("6a6a6a"), Rect)
                    Case MouseMode.Hovered
                        Using LGB As New LinearGradientBrush(Rect, Color.FromArgb(20, GetHTMLColor("6ebeec")), Color.FromArgb(30, GetHTMLColor("6ebeec")), 120S)
                            .DrawPath(New Pen(LGB, 2), RoundRec(Rect, RoundRadius))
                        End Using
                        CentreString(G, Text, Font, SolidBrushHTMlColor("6a6a6a"), Rect)
                    Case MouseMode.Pushed
                        FillRoundedPath(G, SolidBrushHTMlColor("f3f3f3"), Rect, RoundRadius)
                        DrawRoundedPath(G, GetHTMLColor("e0e0e0"), 1, Rect, RoundRadius)
                        CentreString(G, Text, Font, SolidBrushHTMlColor("6a6a6a"), Rect)
 
                End Select
            End With
 
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
 
        End Using
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
 
#Region " Blue Button "
 
Public Class PaleBlueButton : Inherits Control
 
#Region " Variables "
 
    Private State As MouseMode
    Private _SideImage As Image
    Private _SideImageAlign As SideAligin = SideAligin.Left
    Private _RoundRadius As Integer = 10
 
#End Region
 
#Region " Constructors "
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Myriad Pro", 12, FontStyle.Bold)
        UpdateStyles()
    End Sub
 
#End Region
 
#Region " Properties "
 
    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property
 
    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(value As SideAligin)
            _SideImageAlign = value
            Invalidate()
        End Set
    End Property
 
    Public Property RoundRadius As Integer
        Get
            Return _RoundRadius
        End Get
        Set(value As Integer)
            _RoundRadius = value
            Invalidate()
        End Set
    End Property
 
#End Region
 
#Region " Enumerators "
 
    Enum SideAligin
        Left
        Right
    End Enum
 
#End Region
 
#Region " Draw Control "
 
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
 
            With G
                Dim Rect As New Rectangle(2, 2, Width - 4, Height - 4)
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Select Case State
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, SolidBrushHTMlColor("5db6ea"), Rect, RoundRadius)
                        Using lgb As New LinearGradientBrush(Rect, Color.FromArgb(30, GetHTMLColor("5db6ea")), Color.FromArgb(30, 250, 250, 250), 90S)
                            FillRoundedPath(G, lgb, Rect, RoundRadius)
                        End Using
                        DrawRoundedPath(G, GetHTMLColor("4ca6db"), 1, Rect, RoundRadius)
                        CentreString(G, Text, Font, Brushes.White, Rect)
                    Case MouseMode.Hovered
                        FillRoundedPath(G, SolidBrushHTMlColor("5db6ea"), Rect, RoundRadius)
                        CentreString(G, Text, Font, SolidBrushHTMlColor("4ca6db"), Rect)
                    Case MouseMode.Pushed
                        FillRoundedPath(G, Brushes.White, Rect, RoundRadius)
                        DrawRoundedPath(G, GetHTMLColor("4ca6db"), 1, Rect, RoundRadius)
                        CentreString(G, Text, Font, Brushes.White, Rect)
 
                End Select
            End With
 
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
 
        End Using
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
 
#Region " Seperator "
 
Public Class PaleSeperator : Inherits Control
 
#Region " Variables "
 
    Public Property _SepStyle As Style = Style.Horizental
 
#End Region
 
#Region " Enumerators "
 
    Enum Style
        Horizental
        Vertiacal
    End Enum
 
#End Region
 
#Region " Properties "
 
    Public Property SepStyle As Style
        Get
            Return _SepStyle
        End Get
        Set(value As Style)
            _SepStyle = value
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
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            With G
                .SmoothingMode = SmoothingMode.HighQuality
                Select Case SepStyle
                    Case Style.Horizental
                        .DrawLine(New Pen(SolidBrushHTMlColor("d4d4d4")), 0, 1, Width, 1)
                    Case Style.Vertiacal
                        .DrawLine(New Pen(SolidBrushHTMlColor("d4d4d4")), 1, 0, 1, Height)
                End Select
            End With
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
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
 
#Region " TextBox "
 
Public Class PaleTextbox : Inherits Control
 
#Region " Variables "
 
    Protected WithEvents T As New TextBox
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean = False
    Private _UseSystemPasswordChar As Boolean = False
    Private _WatermarkText As String = String.Empty
    Private _SideImage As Image
    Protected TBC As Color = GetHTMLColor("ffffff")
    Protected TFC As Color = GetHTMLColor("a5a5a5")
    Protected State As MouseMode = MouseMode.NormalMode
    Private _BackColor As Color = Color.Transparent
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
 
    <Browsable(True)>
    Public Property SideImage As Image
        Get
            Return _SideImage
        End Get
        Set(value As Image)
            _SideImage = value
            Invalidate()
        End Set
    End Property
 
    Enum SideAligin
        Left
        Right
    End Enum
    Private _SideImageAlign As SideAligin = SideAligin.Left
    <Browsable(True)>
    Public Property SideImageAlign As SideAligin
        Get
            Return _SideImageAlign
        End Get
        Set(value As SideAligin)
            _SideImageAlign = value
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
        Font = New Font("Myriad Pro", 11, FontStyle.Bold)
        With T
            .Multiline = False
            .Cursor = Cursors.IBeam
            .BackColor = TBC
            .ForeColor = TFC
            .BorderStyle = BorderStyle.None
            .Location = New Point(7, 7)
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
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
            Height = 30
 
            With G
 
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
 
                Select Case State
 
                    Case MouseMode.NormalMode
                        FillRoundedPath(G, Brushes.White, Rect, 5)
                        DrawRoundedPath(G, GetHTMLColor("dfdfdf"), 1, Rect, 5)
                    Case MouseMode.Hovered
                        FillRoundedPath(G, Brushes.White, Rect, 5)
                        Using lgb As New LinearGradientBrush(Rect, Color.FromArgb(50, GetHTMLColor("6ebeec")), Color.FromArgb(50, GetHTMLColor("6ebeec")), 120S)
                            .DrawPath(New Pen(lgb, 2), RoundRec(Rect, 8))
                        End Using
                    Case MouseMode.Pushed
                        FillRoundedPath(G, Brushes.White, Rect, 5)
                        DrawRoundedPath(G, GetHTMLColor("dfdfdf"), 1, Rect, 5)
                End Select
 
                If Not SideImage Is Nothing Then
                    If SideImageAlign = SideAligin.Right Then
                        T.Location = New Point(7, 4.5)
                        T.Width = Width - 60
                        .InterpolationMode = InterpolationMode.HighQualityBicubic
                        .DrawImage(SideImage, New Rectangle(Rect.Width - 24, 6, 16, 16))
                    Else
                        T.Location = New Point(33, 4.5)
                        T.Width = Width - 60
                        .InterpolationMode = InterpolationMode.HighQualityBicubic
                        .DrawImage(SideImage, New Rectangle(8, 6, 16, 16))
                    End If
 
                Else
                    T.Location = New Point(7, 4.5)
                    T.Width = Width - 10
                End If
 
                If Not ContextMenuStrip Is Nothing Then T.ContextMenuStrip = ContextMenuStrip
 
            End With
 
            e.Graphics.DrawImage(B.Clone(), 0, 0)
            G.Dispose() : B.Dispose()
 
        End Using
    End Sub
 
#End Region
 
End Class
 
#End Region
 
#Region " CheckBox "
 
Public Class PaleCheckBox : Inherits Control
 
#Region " Variables "
 
    Private _Checked As Boolean
    Protected State As MouseMode = MouseMode.NormalMode
 
#End Region
 
#Region " Events "
 
    Event CheckedChanged(ByVal sender As Object)
 
#End Region
 
#Region " Properties "
 
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
 
#End Region
 
#Region " Constructors "
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or _
     ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        Cursor = Cursors.Hand
        BackColor = Color.Transparent
        Font = New Font("Myriad Pro", 9, FontStyle.Regular)
        UpdateStyles()
    End Sub
 
#End Region
 
#Region " Draw Control "
 
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            G.SmoothingMode = SmoothingMode.AntiAlias
            Using CheckBorder As New GraphicsPath With {.FillMode = FillMode.Winding}
                CheckBorder.AddArc(0, 0, 10, 8, 180, 90)
                CheckBorder.AddArc(8, 0, 8, 10, -90, 90)
                CheckBorder.AddArc(8, 8, 8, 8, 0, 70)
                CheckBorder.AddArc(0, 8, 10, 8, 90, 90)
                CheckBorder.CloseAllFigures()
                G.FillPath(Brushes.White, CheckBorder)
                G.DrawPath(PenHTMlColor("d9d9d9", 1.5), CheckBorder)
                If Checked Then
                    FillRoundedPath(G, SolidBrushHTMlColor("5db5e9"), New Rectangle(3.5, 3.5, 8.5, 8.5), 2)
                    DrawRoundedPath(G, GetHTMLColor("3db3e5"), 1, New Rectangle(3.5, 3.5, 8.5, 8.5), 2)
                End If
            End Using
 
            G.DrawString(Text, Font, SolidBrushHTMlColor("a5a5a5"), New Rectangle(18, 1.4, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub
 
#End Region
 
#Region " Events "
 
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
        Height = 20
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
 
#Region " Label "
 
<DefaultEvent("TextChanged")> Public Class PaleLabel : Inherits Control
 
#Region " Variables "
 
    Private _ColorStyle As Style = Style.Style1
 
#End Region
 
#Region " Properties "
 
    Public Property ColorStyle As Style
        Get
            Return _ColorStyle
        End Get
        Set(value As Style)
            _ColorStyle = value
            Invalidate()
        End Set
    End Property
 
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property ForeColor As Color
        Get
            Return Color.Transparent
        End Get
    End Property
 
#End Region
 
#Region " Enumerators "
 
    Public Enum Style As Byte
        Style1
        Style2
        Style3
    End Enum
 
#End Region
 
#Region " Events "
 
    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub
 
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = Font.Height
    End Sub
 
#End Region
 
#Region " Constructors "
 
    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        Font = New Font("Myriad Pro", 9, FontStyle.Regular)
        UpdateStyles()
    End Sub
 
#End Region
 
#Region " Draw Control "
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            With G
                .SmoothingMode = SmoothingMode.AntiAlias
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                Select Case ColorStyle
                    Case Style.Style1
                        .DrawString(Text, Font, SolidBrushHTMlColor("898989"), ClientRectangle)
                    Case Style.Style2
                        .DrawString(Text, Font, SolidBrushHTMlColor("606060"), ClientRectangle)
                    Case Style.Style3
                        .DrawString(Text, Font, SolidBrushHTMlColor("2e8fc7"), ClientRectangle)
                End Select
            End With
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub
 
#End Region
 
End Class
 
#End Region
 
#Region " Close "
 
Public Class PaleClose : Inherits Control
 
#Region " Variables "
 
    Private IMG As String = _
        "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/GMzLdsAADoTaVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjUtYzAxNCA3OS4xNTE0ODEsIDIwMTMvMDMvMTMtMTI6MDk6MTUgICAgICAgICI+CiAgIDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+CiAgICAgIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICAgICAgICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgICAgICAgICAgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iCiAgICAgICAgICAgIHhtbG5zOnN0RXZ0PSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VFdmVudCMiCiAgICAgICAgICAgIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIKICAgICAgICAgICAgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIgogICAgICAgICAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZXhpZj0iaHR0cDovL25zLmFkb2JlLmNvbS9leGlmLzEuMC8iPgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFBob3Rvc2hvcCBDQyAoV2luZG93cyk8L3htcDpDcmVhdG9yVG9vbD4KICAgICAgICAgPHhtcDpDcmVhdGVEYXRlPjIwMTYtMTItMjlUMDY6NTE6MTktMDg6MDA8L3htcDpDcmVhdGVEYXRlPgogICAgICAgICA8eG1wOk1ldGFkYXRhRGF0ZT4yMDE2LTEyLTI5VDA2OjUxOjE5LTA4OjAwPC94bXA6TWV0YWRhdGFEYXRlPgogICAgICAgICA8eG1wOk1vZGlmeURhdGU+MjAxNi0xMi0yOVQwNjo1MToxOS0wODowMDwveG1wOk1vZGlmeURhdGU+CiAgICAgICAgIDx4bXBNTTpJbnN0YW5jZUlEPnhtcC5paWQ6Nzk1OWQ2MDQtZTAyMC1iZDQ0LTkwYzItOTgyMzJlYTY4ZTg2PC94bXBNTTpJbnN0YW5jZUlEPgogICAgICAgICA8eG1wTU06RG9jdW1lbnRJRD54bXAuZGlkOmRmZjgyZmQzLTlhZmEtM2M0NC1hZjlhLTRjZGZmNzhiOWRmNzwveG1wTU06RG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD54bXAuZGlkOmRmZjgyZmQzLTlhZmEtM2M0NC1hZjlhLTRjZGZmNzhiOWRmNzwveG1wTU06T3JpZ2luYWxEb2N1bWVudElEPgogICAgICAgICA8eG1wTU06SGlzdG9yeT4KICAgICAgICAgICAgPHJkZjpTZXE+CiAgICAgICAgICAgICAgIDxyZGY6bGkgcmRmOnBhcnNlVHlwZT0iUmVzb3VyY2UiPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6YWN0aW9uPmNyZWF0ZWQ8L3N0RXZ0OmFjdGlvbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0Omluc3RhbmNlSUQ+eG1wLmlpZDpkZmY4MmZkMy05YWZhLTNjNDQtYWY5YS00Y2RmZjc4YjlkZjc8L3N0RXZ0Omluc3RhbmNlSUQ+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDp3aGVuPjIwMTYtMTItMjlUMDY6NTE6MTktMDg6MDA8L3N0RXZ0OndoZW4+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDpzb2Z0d2FyZUFnZW50PkFkb2JlIFBob3Rvc2hvcCBDQyAoV2luZG93cyk8L3N0RXZ0OnNvZnR3YXJlQWdlbnQ+CiAgICAgICAgICAgICAgIDwvcmRmOmxpPgogICAgICAgICAgICAgICA8cmRmOmxpIHJkZjpwYXJzZVR5cGU9IlJlc291cmNlIj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmFjdGlvbj5zYXZlZDwvc3RFdnQ6YWN0aW9uPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6aW5zdGFuY2VJRD54bXAuaWlkOjc5NTlkNjA0LWUwMjAtYmQ0NC05MGMyLTk4MjMyZWE2OGU4Njwvc3RFdnQ6aW5zdGFuY2VJRD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OndoZW4+MjAxNi0xMi0yOVQwNjo1MToxOS0wODowMDwvc3RFdnQ6d2hlbj4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OnNvZnR3YXJlQWdlbnQ+QWRvYmUgUGhvdG9zaG9wIENDIChXaW5kb3dzKTwvc3RFdnQ6c29mdHdhcmVBZ2VudD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmNoYW5nZWQ+Lzwvc3RFdnQ6Y2hhbmdlZD4KICAgICAgICAgICAgICAgPC9yZGY6bGk+CiAgICAgICAgICAgIDwvcmRmOlNlcT4KICAgICAgICAgPC94bXBNTTpIaXN0b3J5PgogICAgICAgICA8ZGM6Zm9ybWF0PmltYWdlL3BuZzwvZGM6Zm9ybWF0PgogICAgICAgICA8cGhvdG9zaG9wOkNvbG9yTW9kZT4zPC9waG90b3Nob3A6Q29sb3JNb2RlPgogICAgICAgICA8cGhvdG9zaG9wOklDQ1Byb2ZpbGU+c1JHQiBJRUM2MTk2Ni0yLjE8L3Bob3Rvc2hvcDpJQ0NQcm9maWxlPgogICAgICAgICA8dGlmZjpPcmllbnRhdGlvbj4xPC90aWZmOk9yaWVudGF0aW9uPgogICAgICAgICA8dGlmZjpYUmVzb2x1dGlvbj43MjAwMDAvMTAwMDA8L3RpZmY6WFJlc29sdXRpb24+CiAgICAgICAgIDx0aWZmOllSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpZUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6UmVzb2x1dGlvblVuaXQ+MjwvdGlmZjpSZXNvbHV0aW9uVW5pdD4KICAgICAgICAgPGV4aWY6Q29sb3JTcGFjZT4xPC9leGlmOkNvbG9yU3BhY2U+CiAgICAgICAgIDxleGlmOlBpeGVsWERpbWVuc2lvbj4xNTwvZXhpZjpQaXhlbFhEaW1lbnNpb24+CiAgICAgICAgIDxleGlmOlBpeGVsWURpbWVuc2lvbj4xMzwvZXhpZjpQaXhlbFlEaW1lbnNpb24+CiAgICAgIDwvcmRmOkRlc2NyaXB0aW9uPgogICA8L3JkZjpSREY+CjwveDp4bXBtZXRhPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgIAo8P3hwYWNrZXQgZW5kPSJ3Ij8+t8wKZgAAACBjSFJNAAB6JQAAgIMAAPn/AACA6QAAdTAAAOpgAAA6mAAAF2+SX8VGAAAA5UlEQVR42oTSIUvDQRjH8c9ORJNYhL0BQbAYVnwNZuM/GUWYTUXYwtjEoGA2GtRg3CswuJcgglFkYFLEtFn+G+d5cE+65/n+vnfhnkY1HA9w5G/9YBOvWMEzmklmENDHewKWcVWfuxnxDf2ATxz7Xzs4xUGGneCrUQ3HEDBCS7lG2MY01IMJDjEtiFO0Z7kQgUfcF+QbPM2akMDbgnwXN7G8gE5B7sZOLO9hqyC3UKXyKnqZ8CQzO6sXZy53sJaEvrGbuaBZ/7OADexnXujhAdcZ1sZ6wCUWE/iCi2ibPhK+hPPfAQAi6StkWJPjCAAAAABJRU5ErkJggg=="
 
#End Region
 
#Region " Draw Control "
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
 
            With G
 
                .SmoothingMode = SmoothingMode.HighQuality
                .InterpolationMode = InterpolationMode.High
 
                DrawImageFromBase64(G, IMG, ClientRectangle)
            End With
 
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
 
    End Sub
 
#End Region
 
#Region " Constructors "
 
    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Cursor = Cursors.Hand
        UpdateStyles()
    End Sub
 
#End Region
 
#Region " Events "
 
    Protected Overrides Sub OnResize(e As EventArgs)
        Size = New Size(15, 13)
    End Sub
 
    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        Environment.Exit(0)
        Application.Exit()
    End Sub
 
#End Region
 
End Class
 
#End Region
 
#Region " RadioButton "
 
<DefaultEvent("CheckedChanged")> Public Class PaleRadioButton : Inherits Control
 
#Region " Variables "
 
    Private _Checked As Boolean
    Protected _Group As Integer = 1
    Protected State As MouseMode = MouseMode.NormalMode
 
#End Region
 
#Region " Events "
 
    Event CheckedChanged(ByVal sender As Object)
 
#End Region
 
#Region " Properties "
 
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
 
    Property Group As Integer
        Get
            Return _Group
        End Get
        Set(ByVal value As Integer)
            _Group = value
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
        Font = New Font("Myriad Pro", 9, FontStyle.Regular)
        UpdateStyles()
    End Sub
 
#End Region
 
#Region " Draw Control "
 
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim R As New Rectangle(1, 1, 18, 18)
            With G
                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                If Checked Then
                    .FillEllipse(SolidBrushHTMlColor("5db5e9"), New Rectangle(4, 4, 12, 12))
                    .DrawEllipse(PenHTMlColor("4ca6db", 2), R)
                Else
                    .FillEllipse(Brushes.White, New Rectangle(1, 1, 18, 18))
                    .DrawEllipse(PenHTMlColor("d9d9d9", 2), R)
                End If
                .DrawString(Text, Font, SolidBrushHTMlColor("a5a5a5"), New Rectangle(21, 1.5, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
            End With
 
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
 
        End Using
    End Sub
 
#End Region
 
#Region " Events "
 
    Private Sub UpdateState()
        If Not IsHandleCreated OrElse Not Checked Then Return
        For Each C As Control In Parent.Controls
            If C IsNot Me AndAlso TypeOf C Is PaleRadioButton AndAlso DirectCast(C, PaleRadioButton).Group = _Group Then
                DirectCast(C, PaleRadioButton).Checked = False
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
 
Public Class PaleComboBox : Inherits ComboBox
 
#Region " Variables "
 
    Private _StartIndex As Integer = 0
 
#End Region
 
#Region " Constructors "
 
    Sub New()
 
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or _
                  ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Font = New Font("Myriad Pro", 11)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DoubleBuffered = True
        StartIndex = 0
        DropDownHeight = 100
        DropDownStyle = ComboBoxStyle.DropDownList
        UpdateStyles()
 
    End Sub
 
#End Region
 
#Region " Properties "
 
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
 
#End Region
 
    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        Try
            Dim G As Graphics = e.Graphics
            With G
                .SmoothingMode = SmoothingMode.AntiAlias
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    .FillRectangle(SolidBrushHTMlColor("5db5e9"), e.Bounds)
                    .DrawRectangle(PenHTMlColor("5db5e9", 1), e.Bounds)
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, Brushes.White, 1, e.Bounds.Y + 3)
                Else
                    .FillRectangle(Brushes.White, e.Bounds)
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, SolidBrushHTMlColor("a5a5a5"), 1, e.Bounds.Y + 3)
                End If
            End With
        Catch
        End Try
        Invalidate()
    End Sub
 
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(1, 1, Width - 2.5, Height - 2.5)
            With G
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
                .SmoothingMode = SmoothingMode.AntiAlias
                FillRoundedPath(G, Brushes.White, Rect, 5)
                DrawRoundedPath(G, GetHTMLColor("d9d9d9"), 1.5, Rect, 5)
                FillRoundedPath(G, SolidBrushHTMlColor("5db5e9"), New Rectangle(Width - 30, 1.4, 29, Height - 2.5), 5)
                DrawRoundedPath(G, GetHTMLColor("4ca6db"), 1.5, New Rectangle(Width - 30, 1.4, 29, Height - 2.5), 5)
                DrawTriangle(G, Color.White, 1.5, _
                          New Point(Width - 20, 12), New Point(Width - 16, 16), _
                          New Point(Width - 16, 16), New Point(Width - 12, 12), _
                          New Point(Width - 16, 17), New Point(Width - 16, 16) _
                          )
                .DrawString(Text, Font, New SolidBrush(GetHTMLColor("a5a5a5")), New Rectangle(7, 1.5, Width - 1, Height), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
            End With
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub
 
#Region " Events "
 
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 27
        Invalidate()
    End Sub
 
#End Region
 
End Class
 
#End Region