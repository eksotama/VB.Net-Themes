'' <summary>
'' Acacia Theme
'' Author : THE LORD
'' Release Date : Tuesday, January 10, 2017
'' </summary>

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

#Region " Skin "

Public Class AcaciaSkin : Inherits ContainerControl

#Region " Variables "

    Private Movable As Boolean = False
    Private MousePoint As New Point(0, 0)
    Private MoveHeight = 50
    Private _TitleTextPostion As TitlePostion = TitlePostion.Left

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.ResizeRedraw, True)
        DoubleBuffered = True
        Font = New Font("Arial", 12, FontStyle.Bold)
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

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            G.Clear(Color.Fuchsia)

            G.FillRectangle(SolidBrushHTMlColor("24273e"), New Rectangle(0, 0, Width, Height))

            G.FillRectangle(SolidBrushHTMlColor("1e2137"), New Rectangle(0, 0, Width, 55))

            G.DrawLine(PenHTMlColor("1d1f38", 1), New Point(0, 55), New Point(Width, 55))

            G.DrawRectangle(PenHTMlColor("1d1f38", 1), New Rectangle(0, 0, Width - 1, Height - 1))

            If FindForm.ShowIcon Then
                If Not FindForm.Icon Is Nothing Then

                    Select Case TitleTextPostion
                        Case TitlePostion.Left
                            G.DrawString(Text, Font, SolidBrushHTMlColor("e4ecf2"), 27, 18)
                            G.DrawIcon(FindForm.Icon, New Rectangle(5, 16, 20, 20))
                        Case TitlePostion.Center
                            CentreString(G, Text, Font, SolidBrushHTMlColor("e4ecf2"), New Rectangle(0, 0, Width, 50))
                            G.DrawIcon(FindForm.Icon, New Rectangle(5, 16, 20, 20))
                        Case TitlePostion.Right
                            RightString(G, Text, Font, SolidBrushHTMlColor("e4ecf2"), New Rectangle(0, 0, Width, 50))
                            G.DrawIcon(FindForm.Icon, New Rectangle(Width - 30, 16, 20, 20))
                    End Select
                End If

            Else
                Select Case TitleTextPostion
                    Case TitlePostion.Left
                        G.DrawString(Text, Font, SolidBrushHTMlColor("e4ecf2"), 5, 18)
                    Case TitlePostion.Center
                        CentreString(G, Text, Font, SolidBrushHTMlColor("e4ecf2"), New Rectangle(0, 0, Width, 50))
                    Case TitlePostion.Right
                        RightString(G, Text, Font, SolidBrushHTMlColor("e4ecf2"), New Rectangle(0, 0, Width, 50))
                End Select
            End If

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose() : B.Dispose()
        End Using
    End Sub

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
        ParentForm.Dock = DockStyle.None
        Dock = DockStyle.Fill
        Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " Button "

Public Class AcaciaButton : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _SideImage As Image
    Private _SideImageAlign As SideAligin = SideAligin.Left

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
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

            G.SmoothingMode = SmoothingMode.AntiAlias
            G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            Dim Rect As New Rectangle(2, 2, Width - 5, Height - 5)
            Select Case State

                Case MouseMode.NormalMode

                    Using HB As PathGradientBrush = New PathGradientBrush(RoundRec(New Rectangle(0, 0, Width, Height), 2))
                        FillRoundedPath(G, SolidBrushHTMlColor("fc3955"), Rect, 2)
                        HB.WrapMode = WrapMode.Clamp
                        Dim CB As New ColorBlend(4)
                        CB.Colors = New Color() {Color.FromArgb(220, GetHTMLColor("fc3955")), Color.FromArgb(220, GetHTMLColor("fc3955")), Color.FromArgb(220, GetHTMLColor("fc3955")), Color.FromArgb(220, GetHTMLColor("fc3955"))}
                        CB.Positions = New Single() {0.0F, 0.2F, 0.8F, 1.0F}
                        HB.InterpolationColors = CB
                        FillRoundedPath(G, HB, New Rectangle(0, 0, Width - 1, Height - 1), 2)
                    End Using

                Case MouseMode.Hovered
                    Cursor = Cursors.Hand
                    Using HB As PathGradientBrush = New PathGradientBrush(RoundRec(New Rectangle(0, 0, Width, Height), 2))
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(150, GetHTMLColor("fc3955"))), Rect, 2)
                        HB.WrapMode = WrapMode.Clamp
                        Dim CB As New ColorBlend(4)
                        CB.Colors = New Color() {Color.FromArgb(150, GetHTMLColor("fc3955")), Color.FromArgb(150, GetHTMLColor("fc3955")), Color.FromArgb(150, GetHTMLColor("fc3955")), Color.FromArgb(150, GetHTMLColor("fc3955"))}
                        CB.Positions = New Single() {0.0F, 0.2F, 0.8F, 1.0F}
                        HB.InterpolationColors = CB
                        FillRoundedPath(G, HB, New Rectangle(0, 0, Width - 1, Height - 1), 2)
                    End Using

                Case MouseMode.Pushed

                    Using HB As PathGradientBrush = New PathGradientBrush(RoundRec(New Rectangle(0, 0, Width, Height), 2))
                        FillRoundedPath(G, SolidBrushHTMlColor("fc3955"), Rect, 2)
                        HB.WrapMode = WrapMode.Clamp
                        Dim CB As New ColorBlend(4)
                        CB.Colors = New Color() {Color.FromArgb(220, GetHTMLColor("fc3955")), Color.FromArgb(220, GetHTMLColor("fc3955")), Color.FromArgb(220, GetHTMLColor("fc3955")), Color.FromArgb(220, GetHTMLColor("fc3955"))}
                        CB.Positions = New Single() {0.0F, 0.2F, 0.8F, 1.0F}
                        HB.InterpolationColors = CB
                        FillRoundedPath(G, HB, New Rectangle(0, 0, Width - 1, Height - 1), 2)
                    End Using

            End Select
            If Not SideImage Is Nothing Then
                If SideImageAlign = SideAligin.Right Then
                    G.DrawImage(SideImage, New Rectangle(Rect.Width - 24, Rect.Y + 7, 16, 16))
                Else
                    G.DrawImage(SideImage, New Rectangle(8, Rect.Y + 7, 16, 16))
                End If
            End If
            CentreString(G, Text, Font, SolidBrushHTMlColor("e4ecf2"), Rect)

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

#Region " TextBox "

Public Class AcaciaTextbox : Inherits Control

#Region " Variables "

    Protected WithEvents T As New TextBox
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean = False
    Private _UseSystemPasswordChar As Boolean = False
    Private _WatermarkText As String = String.Empty
    Private _SideImage As Image
    Protected TBC As Color = GetHTMLColor("24273e")
    Protected TFC As Color = GetHTMLColor("585c73")
    Protected State As MouseMode = MouseMode.NormalMode
    Private _BackColor As Color = TBC
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
        BackColor = TBC
        Font = New Font("Arial", 11, FontStyle.Regular)
        With T
            .Multiline = False
            .Cursor = Cursors.IBeam
            .BackColor = BackColor
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

            With G

                .SmoothingMode = SmoothingMode.HighQuality
                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

                Select Case State

                    Case MouseMode.NormalMode
                        .DrawLine(PenHTMlColor("585c73", 1), New Point(0, 29), New Point(Width, 29))
                    Case MouseMode.Hovered
                        .DrawLine(New Pen(GetHTMLColor("fc3955"), 1), New Point(0, 29), New Point(Width, 29))
                    Case MouseMode.Pushed
                        .DrawLine(New Pen(Color.FromArgb(150, GetHTMLColor("fc3955")), 1), New Point(0, 29), New Point(Width, 29))

                End Select

                If Not SideImage Is Nothing Then
                    T.Location = New Point(33, 4.5)
                    T.Width = Width - 60
                    .InterpolationMode = InterpolationMode.HighQualityBicubic
                    .DrawImage(SideImage, New Rectangle(8, 4, 16, 16))
                Else
                    T.Location = New Point(7, 4.5)
                    T.Width = Width - 10
                End If

                If Not ContextMenuStrip Is Nothing Then T.ContextMenuStrip = ContextMenuStrip

            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

End Class

#End Region

#Region " Control Button "

Class AcaciaControlButton : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _ControlStyle As Style = Style.Close

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G

                .SmoothingMode = SmoothingMode.HighQuality

                Select Case State

                    Case MouseMode.NormalMode

                        .DrawEllipse(New Pen(Color.FromArgb(150, GetHTMLColor("fc3955")), 2), New Rectangle(1, 1, 15, 15))
                        .FillEllipse(New SolidBrush(Color.FromArgb(150, GetHTMLColor("fc3955"))), New Rectangle(5, 5, 7, 7))

                    Case MouseMode.Hovered

                        Cursor = Cursors.Hand

                        .DrawEllipse(PenHTMlColor("fc3955", 2), New Rectangle(1, 1, 15, 15))
                        .FillEllipse(SolidBrushHTMlColor("fc3955"), New Rectangle(5, 5, 7, 7))

                    Case MouseMode.Pushed

                        .DrawEllipse(PenHTMlColor("24273e", 2), New Rectangle(1, 1, 15, 15))
                        .FillEllipse(SolidBrushHTMlColor("24273e"), New Rectangle(5, 5, 7, 7))

                End Select


            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub

#End Region

#Region " Properties "

    Public Property ControlStyle As Style
        Get
            Return _ControlStyle
        End Get
        Set(value As Style)
            _ControlStyle = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Enumerators "

    Enum Style
        Close
        Minimize
        Maximize
    End Enum

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
       ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        UpdateStyles()
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Size = New Size(18, 18)
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        If ControlStyle = Style.Close Then
            Environment.Exit(0)
            Application.Exit()
        ElseIf ControlStyle = Style.Minimize Then
            If FindForm.WindowState = FormWindowState.Normal Then
                FindForm.WindowState = FormWindowState.Minimized
            End If
        ElseIf ControlStyle = Style.Maximize Then
            If FindForm.WindowState = FormWindowState.Normal Then
                FindForm.WindowState = FormWindowState.Maximized
            ElseIf FindForm.WindowState = FormWindowState.Maximized Then
                FindForm.WindowState = FormWindowState.Normal
            End If
        End If
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

#Region " CheckBox "

<DefaultEvent("CheckedChanged")> Public Class AcaciaCheckBox : Inherits Control

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
        Font = New Font("Arial", 11, FontStyle.Regular)
        UpdateStyles()
    End Sub

#End Region

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            G.SmoothingMode = SmoothingMode.AntiAlias

            G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            If Checked Then

                DrawRoundedPath(G, GetHTMLColor("fc3955"), 2.5, New Rectangle(1, 1, 17, 17), 1)

                FillRoundedPath(G, SolidBrushHTMlColor("fc3955"), New Rectangle(5, 5, 9, 9), 1)

                G.DrawString(Text, Font, SolidBrushHTMlColor("e4ecf2"), New Rectangle(22, 1.6, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

            Else

                Select Case State

                    Case MouseMode.NormalMode

                        DrawRoundedPath(G, Color.FromArgb(150, GetHTMLColor("fc3955")), 2.5, New Rectangle(1, 1, 17, 17), 1)

                        G.DrawString(Text, Font, New SolidBrush(Color.Silver), New Rectangle(22, 1.6, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

                    Case MouseMode.Hovered

                        DrawRoundedPath(G, GetHTMLColor("fc3955"), 2.5, New Rectangle(1, 1, 17, 17), 1)

                        FillRoundedPath(G, SolidBrushHTMlColor("fc3955"), New Rectangle(5, 5, 9, 9), 1)

                        G.DrawString(Text, Font, SolidBrushHTMlColor("e4ecf2"), New Rectangle(22, 1.6, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

                End Select

            End If

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub

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

#Region " Radio Button "

<DefaultEvent("TextChanged")> Public Class AcaciaRadioButton : Inherits Control

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
        Font = New Font("Arial", 11, FontStyle.Regular)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            G.SmoothingMode = SmoothingMode.AntiAlias

            G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            If Checked Then

                G.DrawEllipse(PenHTMlColor("fc3955", 2.8), New Rectangle(1, 1, 18, 18))

                G.FillEllipse(SolidBrushHTMlColor("fc3955"), New Rectangle(5, 5, 10, 10))

                G.DrawString(Text, Font, SolidBrushHTMlColor("e4ecf2"), New Rectangle(22, 1.6, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

            Else

                Select Case State

                    Case MouseMode.NormalMode

                        G.DrawEllipse(New Pen(Color.FromArgb(150, GetHTMLColor("fc3955")), 2.8), New Rectangle(1, 1, 18, 18))

                        G.DrawString(Text, Font, New SolidBrush(Color.Silver), New Rectangle(22, 1.6, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

                    Case MouseMode.Hovered

                        G.DrawEllipse(PenHTMlColor("fc3955", 2.8), New Rectangle(1, 1, 18, 18))

                        G.FillEllipse(SolidBrushHTMlColor("fc3955"), New Rectangle(5, 5, 10, 10))

                        G.DrawString(Text, Font, SolidBrushHTMlColor("e4ecf2"), New Rectangle(22, 1.6, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

                End Select

            End If

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
            If C IsNot Me AndAlso TypeOf C Is AcaciaRadioButton AndAlso DirectCast(C, AcaciaRadioButton).Group = _Group Then
                DirectCast(C, AcaciaRadioButton).Checked = False
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

<DefaultEvent("TextChanged")> Public Class AcaciaLabel : Inherits Control

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        With e.Graphics
            .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
            .DrawString(Text, Font, SolidBrushHTMlColor("e4ecf2"), ClientRectangle)
        End With
    End Sub

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Arial", 10, FontStyle.Bold)
        UpdateStyles()
    End Sub

#End Region

    Protected Overrides Sub OnResize(e As EventArgs)
        Height = Font.Height
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
        Invalidate()
    End Sub

End Class

#End Region

#Region " Seperator "

Public Class AcaciaSeperator : Inherits Control

#Region " Variables "

    Private _SepStyle As Style = Style.Horizental

#End Region

#Region " Enumerators "

    Enum Style
        Horizental
        Vertiacal
    End Enum

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        ForeColor = Color.FromArgb(150, GetHTMLColor("fc3955"))
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G
                .SmoothingMode = SmoothingMode.HighQuality
                Dim BL1, BL2 As New ColorBlend
                BL1.Positions = New Single() {0.0F, 0.15F, 0.85F, 1.0F}
                BL1.Colors = New Color() {Color.Transparent, ForeColor, ForeColor, Color.Transparent}
                Select Case SepStyle
                    Case Style.Horizental
                        Using lb1 As New LinearGradientBrush(ClientRectangle, Color.Empty, Color.Empty, 0.0F)
                            lb1.InterpolationColors = BL1
                            .DrawLine(New Pen(lb1), 0, 1, Width, 1)
                        End Using
                    Case Style.Vertiacal
                        Using lb1 As New LinearGradientBrush(ClientRectangle, Color.Empty, Color.Empty, 0.0F)
                            lb1.InterpolationColors = BL1
                            .DrawLine(New Pen(lb1), 1, 0, 1, Height)
                        End Using
                End Select
            End With
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub

#End Region

    Protected Overrides Sub OnResize(e As EventArgs)
        If SepStyle = Style.Horizental Then
            Height = 4
        Else
            Width = 4
        End If
    End Sub

#Region " Properties "

    Public Property SepStyle As Style
        Get
            Return _SepStyle
        End Get
        Set(value As Style)
            _SepStyle = value
            If value = Style.Horizental Then
                Height = 4
            Else
                Width = 4
            End If
        End Set
    End Property

#End Region


End Class

#End Region

#Region " Combo Box "

Public Class AcaciaComboBox : Inherits ComboBox

#Region " Variables "

    Private _StartIndex As Integer = 0

#End Region

#Region " Constructors "

    Sub New()

        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or _
                  ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Font = New Font("Arial", 12)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        DoubleBuffered = True
        StartIndex = 0
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
                    .FillRectangle(New SolidBrush(Color.FromArgb(120, GetHTMLColor("fc3955"))), e.Bounds)
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, SolidBrushHTMlColor("585c73"), 1, e.Bounds.Y + 4)
                Else
                    .FillRectangle(Brushes.WhiteSmoke, e.Bounds)
                    .DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), Font, SolidBrushHTMlColor("585c73"), 1, e.Bounds.Y + 4)
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

                DrawRoundedPath(G, GetHTMLColor("585c73"), 1.7, Rect, 1)
                .SmoothingMode = SmoothingMode.AntiAlias
                DrawTriangle(G, GetHTMLColor("fc3955"), 1.5, _
                          New Point(Width - 20, 12), New Point(Width - 16, 16), _
                          New Point(Width - 16, 16), New Point(Width - 12, 12), _
                          New Point(Width - 16, 17), New Point(Width - 16, 16) _
                          )
                .SmoothingMode = SmoothingMode.None
                .DrawString(Text, Font, New SolidBrush(GetHTMLColor("585c73")), New Rectangle(7, 1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
            End With
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub

#Region " Events "

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " ProgressBar "

Public Class AcaciaProgressBar : Inherits Control

#Region " Variables "

    Private _Maximum As Integer = 100
    Private _Value As Integer = 0


#End Region

#Region " Constructors "

    Sub New()

        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or _
                       ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        UpdateStyles()

    End Sub

#End Region

#Region " Properties "

    Public Property Value() As Integer
        Get
            If _Value < 0 Then
                Return 0
            Else
                Return _Value
            End If
        End Get
        Set(ByVal Value As Integer)
            If Value > Maximum Then
                Value = Maximum
            End If
            _Value = Value
            Invalidate()
        End Set
    End Property

    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal Value As Integer)
            Select Case Value
                Case Is < _Value
                    _Value = Value
            End Select
            _Maximum = Value
            Invalidate()
        End Set
    End Property

#End Region

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G

                .SmoothingMode = SmoothingMode.HighQuality

                Dim CurrentValue As Integer = CInt(Value / Maximum * Width)

                Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)

                FillRoundedPath(G, SolidBrushHTMlColor("1e2137"), Rect, 1)

                If Not CurrentValue = 0 Then
                    FillRoundedPath(G, GetHTMLColor("fc3955"), New Rectangle(Rect.X, Rect.Y, CurrentValue, Rect.Height), 1)
                End If

            End With

            e.Graphics.DrawImage(B.Clone, 0, 0)
            G.Dispose() : B.Dispose()

        End Using
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Height = 20
    End Sub

End Class

#End Region

#Region " TrackBar "

<DefaultEvent("Scroll")> Public Class AcaciaTrackBar : Inherits Control

#Region " Variables "

    Protected Variable As Boolean
    Private Track, TrackSide As Rectangle
    Protected _Maximum As Integer = 100
    Private _Minimum As Integer
    Private _Value As Integer
    Private CurrentValue As Integer = CInt(Value / Maximum - 2 * (Width))

#End Region

#Region " Events "

    Event Scroll(ByVal sender As Object)

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Subtract OrElse e.KeyCode = Keys.Down OrElse e.KeyCode = Keys.Left Then
            If Value = 0 Then Exit Sub
            Value -= 1
        ElseIf e.KeyCode = Keys.Add OrElse e.KeyCode = Keys.Up OrElse e.KeyCode = Keys.Right Then
            If Value = Maximum Then Exit Sub
            Value += 1
        End If
        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If e.Button = MouseButtons.Left AndAlso Height > 0 Then
            RenewCurrentValue()
            If Width > 0 AndAlso Height > 0 Then
                Try
                    Track = New Rectangle(CurrentValue + 0.8, 0, 25, 24)
                Catch
                End Try

            End If
            Variable = New Rectangle(CurrentValue, 0, 24, Height).Contains(e.Location)
        End If
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
        If Variable AndAlso e.X > -1 AndAlso e.X < Width + 1 Then Value = Minimum + CInt((Maximum - Minimum) * (e.X / Width))
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        Variable = False : MyBase.OnMouseUp(e)
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        If Width > 0 AndAlso Height > 0 Then
            RenewCurrentValue()
            MoveTrack()
        End If
        Invalidate()
        MyBase.OnResize(e)
    End Sub

    Sub RenewCurrentValue()
        CurrentValue = CInt((Value - Minimum) / (Maximum - Minimum) * (Width - 23.5))
    End Sub

    Protected Sub MoveTrack()
        If Height > 0 AndAlso Width > 0 Then Track = New Rectangle(CurrentValue + 1, 0, 21, 20)
        TrackSide = New Rectangle(CurrentValue + 8.5, 7, 6, 6)
    End Sub

#End Region

#Region " Properties "

    Property Maximum As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            _Maximum = value
            RenewCurrentValue()
            MoveTrack()
            Invalidate()
        End Set

    End Property

    Property Minimum As Integer
        Get
            Return _Minimum
        End Get
        Set(ByVal value As Integer)
            If Not value < 0 Then
                _Minimum = value
                RenewCurrentValue()
                MoveTrack()
                Invalidate()
            End If
        End Set
    End Property

    Property Value As Integer
        Get
            Return _Value
        End Get
        Set(ByVal value As Integer)
            If value <> _Value Then
                _Value = value
                RenewCurrentValue()
                MoveTrack()
                Invalidate()
                RaiseEvent Scroll(Me)
            End If
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

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G
                Cursor = Cursors.Hand
                .SmoothingMode = SmoothingMode.HighQuality
                .PixelOffsetMode = PixelOffsetMode.HighQuality

                FillRoundedPath(G, SolidBrushHTMlColor("1e2137"), New Rectangle(0, 5.5, Width, 8), 8)

                If Not CurrentValue = 0 Then
                    FillRoundedPath(G, GetHTMLColor("fc3955"), New Rectangle(0, 5.5, CurrentValue + 4, 8), 6)
                End If
                .PixelOffsetMode = PixelOffsetMode.Half
                .FillEllipse(SolidBrushHTMlColor("fc3955"), Track)
                .FillEllipse(SolidBrushHTMlColor("1e2137"), TrackSide)

            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region


End Class

#End Region

#Region " Panel "

Public Class AcaciaPanel : Inherits ContainerControl

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)

            With G
                .FillRectangle(SolidBrushHTMlColor("24273e"), Rect)
                .DrawRectangle(PenHTMlColor("1d1f38", 1), Rect)

            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using

    End Sub

#End Region

End Class

#End Region