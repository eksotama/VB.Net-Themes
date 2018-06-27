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
        G.DrawString(Text, font, brush, New Rectangle(Rect.X, Rect.Y, Rect.Width, Rect.Height), New StringFormat With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
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

Public Class ModalTheme : Inherits ContainerControl

#Region " Variables "

    Private Movable As Boolean = False
    Private MousePoint As New Point(0, 0)
    Private MoveHeight = 50
    Private _TitleTextPostion As TitlePostion = TitlePostion.Left
    Private _BorderThickness As Integer = 1
    Private _ShowIcon As Boolean

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.ResizeRedraw, True)
        DoubleBuffered = True
        Font = New Font("Ubuntu", 13, FontStyle.Bold)
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    Public Property BorderThickness As Integer
        Get
            Return _BorderThickness
        End Get
        Set(value As Integer)
            _BorderThickness = value
            Invalidate()
        End Set
    End Property


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
        With e.Graphics

            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

            .FillRectangle(SolidBrushHTMlColor("331f35"), New Rectangle(0, 0, Width, Height))

            .FillRectangle(SolidBrushHTMlColor("241525"), New Rectangle(0, 0, Width, 55))

            .DrawLine(PenHTMlColor("231024", 1), New Point(1, 55), New Point(Width - 1, 55))

            .DrawRectangle(PenHTMlColor("241525", BorderThickness), New Rectangle(0, 0, Width, Height))

            If FindForm.ShowIcon Then

                If Not FindForm.Icon Is Nothing Then

                    Select Case TitleTextPostion
                        Case TitlePostion.Left
                            .DrawString(Text.ToUpper, Font, SolidBrushHTMlColor("f3ebf3"), 27, 15)
                            .DrawIcon(FindForm.Icon, New Rectangle(5, 16, 20, 20))
                        Case TitlePostion.Center
                            CentreString(e.Graphics, Text.ToUpper, Font, SolidBrushHTMlColor("f3ebf3"), New Rectangle(0, 0, Width, 50))
                            .DrawIcon(FindForm.Icon, New Rectangle(5, 16, 20, 20))
                        Case TitlePostion.Right
                            RightString(e.Graphics, Text.ToUpper, Font, SolidBrushHTMlColor("f3ebf3"), New Rectangle(0, 0, Width, 50))
                            .DrawIcon(FindForm.Icon, New Rectangle(Width - 30, 16, 20, 20))
                    End Select

                End If

            Else

                Select Case TitleTextPostion
                    Case TitlePostion.Left
                        .DrawString(Text.ToUpper, Font, SolidBrushHTMlColor("f3ebf3"), 5, 17)
                    Case TitlePostion.Center
                        CentreString(e.Graphics, Text.ToUpper, Font, SolidBrushHTMlColor("f3ebf3"), New Rectangle(0, 2, Width, 50))
                    Case TitlePostion.Right
                        RightString(e.Graphics, Text.ToUpper, Font, SolidBrushHTMlColor("f3ebf3"), New Rectangle(0, 2, Width, 50))
                End Select

            End If



        End With
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
        Dock = DockStyle.Fill
        Invalidate()
    End Sub

#End Region

End Class

#End Region

#Region " Flat Button "

Public Class ModalFlatButton : Inherits Control

#Region " Variables "

    Private State As MouseMode

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Ubuntu", 14, FontStyle.Regular, GraphicsUnit.Pixel)
        UpdateStyles()

    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            G.SmoothingMode = SmoothingMode.AntiAlias
            G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
            Select Case State

                Case MouseMode.NormalMode

                    FillRoundedPath(G, SolidBrushHTMlColor("291a2a"), Rect, 2)

                    DrawRoundedPath(G, GetHTMLColor("231625"), 1, Rect, 2)


                Case MouseMode.Hovered
                    Cursor = Cursors.Hand

                    FillRoundedPath(G, SolidBrushHTMlColor("231625"), Rect, 2)

                    DrawRoundedPath(G, GetHTMLColor("231625"), 1, Rect, 2)


                Case MouseMode.Pushed

                    FillRoundedPath(G, SolidBrushHTMlColor("291a2a"), Rect, 2)
                    DrawRoundedPath(G, GetHTMLColor("231625"), 1, Rect, 2)

            End Select

            CentreString(G, Text, Font, SolidBrushHTMlColor("e5d2e6"), New Rectangle(0, 0, Width - 2, Height - 4))

            e.Graphics.DrawImage(B.Clone, 0, 0)
            G.Dispose() : B.Dispose()
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

#Region " Button "

Public Class ModalButton : Inherits Control

#Region " Variables "

    Private State As MouseMode

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or _
         ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Ubuntu", 14, FontStyle.Regular, GraphicsUnit.Pixel)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            G.SmoothingMode = SmoothingMode.AntiAlias
            G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
            Select Case State

                Case MouseMode.NormalMode

                    Using HB As LinearGradientBrush = New LinearGradientBrush(Rect, Color.FromArgb(200, GetHTMLColor("431448")), Color.FromArgb(200, GetHTMLColor("5b2960")), 270S)
                        FillRoundedPath(G, HB, Rect, 2)
                        Using HB2 As LinearGradientBrush = New LinearGradientBrush(Rect, Color.FromArgb(230, GetHTMLColor("431448")), Color.FromArgb(230, GetHTMLColor("5b2960")), 270S)
                            FillRoundedPath(G, HB2, Rect, 2)
                        End Using
                        DrawRoundedPath(G, GetHTMLColor("311833"), 1, Rect, 2)
                    End Using

                Case MouseMode.Hovered
                    Cursor = Cursors.Hand
                    Using HB As LinearGradientBrush = New LinearGradientBrush(Rect, Color.FromArgb(200, GetHTMLColor("241525")), Color.FromArgb(200, GetHTMLColor("241525")), 270S)
                        FillRoundedPath(G, HB, Rect, 2)
                        Using HB2 As LinearGradientBrush = New LinearGradientBrush(Rect, Color.FromArgb(230, GetHTMLColor("431448")), Color.FromArgb(230, GetHTMLColor("5b2960")), 270S)
                            FillRoundedPath(G, HB2, Rect, 2)
                        End Using
                        DrawRoundedPath(G, GetHTMLColor("311833"), 1, Rect, 2)
                    End Using

                Case MouseMode.Pushed

                    Using HB As LinearGradientBrush = New LinearGradientBrush(Rect, Color.FromArgb(200, GetHTMLColor("431448")), Color.FromArgb(200, GetHTMLColor("5b2960")), 270S)
                        FillRoundedPath(G, HB, Rect, 2)
                        Using HB2 As LinearGradientBrush = New LinearGradientBrush(Rect, Color.FromArgb(230, GetHTMLColor("431448")), Color.FromArgb(230, GetHTMLColor("5b2960")), 270S)
                            FillRoundedPath(G, HB2, Rect, 2)
                        End Using
                        DrawRoundedPath(G, GetHTMLColor("311833"), 1, Rect, 2)
                    End Using

            End Select

            CentreString(G, Text, Font, SolidBrushHTMlColor("e5d2e6"), New Rectangle(0, 0, Width - 1, Height - 4))

            e.Graphics.DrawImage(B.Clone, 0, 0)
            G.Dispose() : B.Dispose()
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

<DefaultEvent("TextChanged")> Public Class ModalTextbox : Inherits Control

#Region " Variables "

    Private WithEvents T As New TextBox
    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Private _MaxLength As Integer = 32767
    Private _ReadOnly As Boolean = False
    Private _UseSystemPasswordChar As Boolean = False
    Private _WatermarkText As String = String.Empty
    Private _SideImage As Image
    Private TBC = GetHTMLColor("291a2a")
    Private TFC = GetHTMLColor("a89ea9")
    Private State As MouseMode = MouseMode.NormalMode

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

#Region " Initilization "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                  ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or _
                  ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Ubuntu", 10, FontStyle.Regular)
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

    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
            Height = 30

            With G

                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

                .FillRectangle(SolidBrushHTMlColor("291a2a"), Rect)

                .DrawLine(PenHTMlColor("231625", 1), New Point(0, Height - 1), New Point(Width - 2, Height - 1))

                If Not SideImage Is Nothing Then
                    If SideImageAlign = SideAligin.Right Then
                        T.Location = New Point(7, 5)
                        T.Width = Width - 60
                        .InterpolationMode = InterpolationMode.HighQualityBicubic
                        .DrawImage(SideImage, New Rectangle(Rect.Width - 24, 6, 16, 16))
                    Else
                        T.Location = New Point(33, 5)
                        T.Width = Width - 60
                        .InterpolationMode = InterpolationMode.HighQualityBicubic
                        .DrawImage(SideImage, New Rectangle(8, 6, 16, 16))
                    End If

                Else
                    T.Location = New Point(7, 5)
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

#Region " ComboBox "

Public Class ModalComboBox : Inherits ComboBox

#Region " Variables "

    Private _StartIndex As Integer = 0

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        BackColor = GetHTMLColor("291a2a")
        Font = New Font("Ubuntu", 12, FontStyle.Regular)
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

#Region " Draw Control "

    Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
        Try
            Dim G As Graphics = e.Graphics
            With G
                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                    Cursor = Cursors.Hand
                    .FillRectangle(New SolidBrush(Color.FromArgb(220, GetHTMLColor("291a2a"))), New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 2, e.Bounds.Width - 2, e.Bounds.Height - 2))
                    CentreString(G, Items(e.Index), New Font("Ubuntu", 10, FontStyle.Bold), SolidBrushHTMlColor("a89ea9"), New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 3, e.Bounds.Width - 2, e.Bounds.Height - 2))
                Else
                    .FillRectangle(SolidBrushHTMlColor("291a2a"), New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 2, e.Bounds.Width - 2, e.Bounds.Height - 2))
                    CentreString(G, Items(e.Index), New Font("Ubuntu", 10, FontStyle.Regular), Brushes.Gainsboro, New Rectangle(e.Bounds.X + 1, e.Bounds.Y + 2, e.Bounds.Width - 2, e.Bounds.Height - 2))
                End If
            End With
        Catch
        End Try
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
        With e.Graphics
            .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            .FillRectangle(SolidBrushHTMlColor("291a2a"), Rect)

            .DrawLine(PenHTMlColor("231625", 2), New Point(Width - 21, (Height / 2) - 3), New Point(Width - 7, (Height / 2) - 3))
            .DrawLine(PenHTMlColor("231625", 2), New Point(Width - 21, (Height / 2)), New Point(Width - 7, (Height / 2)))
            .DrawLine(PenHTMlColor("231625", 2), New Point(Width - 21, (Height / 2) + 5), New Point(Width - 7, (Height / 2) + 5))

            .DrawLine(PenHTMlColor("231625", 1), New Point(1, Height - 1), New Point(Width - 2, Height - 1))
            .DrawString(Text, Font, New SolidBrush(GetHTMLColor("a89ea9")), New Rectangle(5, 1, Width - 1, Height - 1), New StringFormat With {.LineAlignment = StringAlignment.Center, .Alignment = StringAlignment.Near})
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

#Region " CheckBox "

<DefaultEvent("CheckedChanged")> Public Class ModalCheckBox : Inherits Control

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
        Font = New Font("Ubuntu", 11, FontStyle.Regular)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim R As New Rectangle(1, 1, 18, 18)
            G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            G.FillRectangle(SolidBrushHTMlColor("291a2a"), R)
            G.DrawRectangle(PenHTMlColor("231625", 1.5), R)
            If Checked Then
                G.DrawString("b", New Font("Marlett", 16, FontStyle.Regular), SolidBrushHTMlColor("5b2960"), New Rectangle(-2.7, 0, Width - 4, Height))
            End If
            G.DrawString(Text, Font, SolidBrushHTMlColor("a89ea9"), New Rectangle(22, 1.8, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})
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

#Region " Radio Button "

<DefaultEvent("TextChanged")> Public Class ModalRadioButton : Inherits Control

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
        Font = New Font("Ubuntu", 11, FontStyle.Regular)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)

        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            G.SmoothingMode = SmoothingMode.AntiAlias

            G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            G.FillEllipse(SolidBrushHTMlColor("291a2a"), New Rectangle(1, 1, 18, 18))
            G.DrawEllipse(PenHTMlColor("231625", 2.8), New Rectangle(1, 1, 18, 18))

            If Checked Then

                G.DrawString("-", Font, SolidBrushHTMlColor("5b2960"), New Rectangle(5, 0.8, Width - 4, Height))

            Else

                G.DrawString("+", Font, SolidBrushHTMlColor("5b2960"), New Rectangle(4.5, 1, Width - 4, Height))

            End If

            G.DrawString(Text, Font, SolidBrushHTMlColor("a89ea9"), New Rectangle(22, 1.6, Width, Height - 2), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Center})

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
            If C IsNot Me AndAlso TypeOf C Is ModalRadioButton AndAlso DirectCast(C, ModalRadioButton).Group = _Group Then
                DirectCast(C, ModalRadioButton).Checked = False
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
        MyBase.OnCreateControl()
        UpdateState()
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

<DefaultEvent("TextChanged")> Public Class ModalLabel : Inherits Control

#Region " Draw Cotnrol "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        With e.Graphics
            .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            .DrawString(Text, Font, SolidBrushHTMlColor("a89ea9"), ClientRectangle)
        End With
    End Sub

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Ubuntu", 12, FontStyle.Regular)
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

<DefaultEvent("TextChanged")> Public Class ModalLinkLabel : Inherits Control

#Region " Variables "

    Private State As MouseMode = MouseMode.NormalMode
    Private _URL As String = String.Empty
    Private _HoverColor As Color = GetHTMLColor("311833")
    Private _PushedColor As Color = GetHTMLColor("431448")

#End Region

#Region " Properties "

    Public Property URL As String
        Get
            Return _URL
        End Get
        Set(value As String)
            _URL = value
            Invalidate()
        End Set
    End Property

    Public Property HoverColor As Color
        Get
            Return _HoverColor
        End Get
        Set(value As Color)
            _HoverColor = value
            Invalidate()
        End Set
    End Property

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
                    .DrawString(Text, Font, SolidBrushHTMlColor("a89ea9"), ClientRectangle)
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
        Font = New Font("Ubuntu", 12, FontStyle.Underline)
        UpdateStyles()
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnResize(e As EventArgs)
        Height = Font.Height + 2
    End Sub
    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        If URL <> String.Empty Then
            If Not URL.StartsWith("http://www.") Then
                URL = "http://www." & URL
                Process.Start(URL)
            End If
        End If
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

#Region " Seperator "

Public Class ModalSeperator : Inherits Control

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
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            With G
                Select Case SepStyle
                    Case Style.Horizental
                        .DrawLine(PenHTMlColor("231625", 1), 0, 1, Width, 1)
                    Case Style.Vertiacal
                        .DrawLine(PenHTMlColor("231625", 1), 1, 0, 1, Height)
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
        If SepStyle = Style.Horizental Then
            Height = 4
        Else
            Width = 4
        End If
    End Sub

#End Region

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

#Region " Panel "

Public Class ModalPanel : Inherits ContainerControl

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
            With G

                FillRoundedPath(G, SolidBrushHTMlColor("291a2a"), Rect, 2)

                DrawRoundedPath(G, GetHTMLColor("231625"), 1, Rect, 2)

            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

End Class

#End Region

#Region " GroupBox "

Public Class ModalGroupBox : Inherits ContainerControl

#Region " Variables "

    Private _GroupBoxStyle As Style = Style.I

#End Region

#Region " Properties "

    Public Property GroupBoxStyle As Style
        Get
            Return _GroupBoxStyle
        End Get
        Set(value As Style)
            _GroupBoxStyle = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Enumerators "

    Public Enum Style
        I
        O
    End Enum

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Font = New Font("Ubuntu", 11, FontStyle.Regular)
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)
            With G

                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

                If GroupBoxStyle = Style.I Then
                    FillRoundedPath(G, SolidBrushHTMlColor("291a2a"), Rect, 2)
                    DrawRoundedPath(G, GetHTMLColor("231625"), 1, Rect, 2)

                    G.DrawString(Text, Font, SolidBrushHTMlColor("a89ea9"), New Point(5, 6), StringFormat.GenericTypographic)
                    G.DrawLine(PenHTMlColor("231625", 1), New Point(3, 32), New Point(130, 32))
                Else
                    FillRoundedPath(G, SolidBrushHTMlColor("291a2a"), New Rectangle(0, 0, Width - 1, 32), 2)
                    DrawRoundedPath(G, GetHTMLColor("231625"), 1, Rect, 2)

                    G.DrawString(Text, Font, SolidBrushHTMlColor("a89ea9"), New Point(5, 6), StringFormat.GenericTypographic)
                    G.DrawLine(PenHTMlColor("231625", 1), New Point(1, 32), New Point(Width - 1, 32))

                End If



            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

End Class

#End Region

#Region " Progress "

Public Class ModalProgressBar : Inherits Control

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

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G

                Dim CurrentValue As Integer = CInt(Value / Maximum * Width)

                Dim Rect As New Rectangle(0, 0, Width, Height)

                .FillRectangle(SolidBrushHTMlColor("291a2a"), Rect)
                .DrawRectangle(PenHTMlColor("231625", 1), New Rectangle(0, 0, Width - 1, Height - 1))

                If Not CurrentValue = 0 Then
                    .FillRectangle(New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(Rect.X + 1, Rect.Y + 1, CurrentValue - 2, Rect.Height - 2))
                End If

            End With

            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()

        End Using
    End Sub

#End Region

End Class

#End Region

#Region " Slider "

<DefaultEvent("Scroll")> Public Class ModalTrack : Inherits Control

#Region " Variables "

    Protected Variable As Boolean
    Private Track, TrackSide As Rectangle
    Protected _Maximum As Integer = 100
    Private _Minimum As Integer
    Private _Value As Integer
    Private CurrentValue As Integer = CInt(Value / Maximum - 2 * (Width))

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
        ControlStyles.ResizeRedraw Or ControlStyles.SupportsTransparentBackColor Or ControlStyles.UserPaint, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        UpdateStyles()
    End Sub

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
        Height = 19
    End Sub

    Sub RenewCurrentValue()
        CurrentValue = CInt((Value - Minimum) / (Maximum - Minimum) * (Width - 22.5))
    End Sub

    Protected Sub MoveTrack()
        If Height > 0 AndAlso Width > 0 Then Track = New Rectangle(CurrentValue, 0, 21, 18.5)
        TrackSide = New Rectangle(CurrentValue + 5.6, 5, 8, 8)
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            With G

                Cursor = Cursors.Hand
                .FillRectangle(SolidBrushHTMlColor("291a2a"), New Rectangle(0, 5.5, Width, 8))
                .DrawRectangle(PenHTMlColor("231625", 1), New Rectangle(0, 5, Width - 1, 8))
                If Not CurrentValue = 0 Then
                    .FillRectangle(New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(1, 5.5, CurrentValue + 4, 7))
                End If
                .FillRectangle(SolidBrushHTMlColor("291a2a"), Track)
                .DrawRectangle(PenHTMlColor("231625", 1), Track)
                .FillRectangle(New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), TrackSide)
                .DrawRectangle(PenHTMlColor("231625", 1), TrackSide)

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

Class ModalControlButton : Inherits Control

#Region " Variables "

    Private State As MouseMode
    Private _ControlStyle As Style = Style.Close

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

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(0, 0, 18.5, 21)
            With G

                .SmoothingMode = SmoothingMode.AntiAlias

                Select Case State

                    Case MouseMode.NormalMode

                        FillRoundedPath(G, SolidBrushHTMlColor("291a2a"), Rect, 2)
                        DrawRoundedPath(G, GetHTMLColor("231625"), 1, Rect, 2)
                        Select Case ControlStyle

                            Case Style.Close
                                FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(5.4, 5.4, 8, 11), 2)
                                DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(5.4, 5.4, 8, 11), 2)
                            Case Style.Maximize
                                FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(4.5, 6, 9, 9), 2)
                                DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(4.5, 6, 9, 9), 2)
                            Case Style.Minimize
                                FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(3, 7.5, 12, 5), 2)
                                DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(3, 7.5, 12, 5), 2)
                        End Select


                    Case MouseMode.Hovered

                        Cursor = Cursors.Hand
                        FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("291a2a"))), Rect, 2)
                        DrawRoundedPath(G, Color.FromArgb(130, GetHTMLColor("231625")), 1, New Rectangle(0, 0, 18.5, 21), 2)
                        Select Case ControlStyle
                            Case Style.Close
                                FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(5.4, 5.4, 8, 11), 2)
                                DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(5.4, 5.4, 8, 11), 2)
                            Case Style.Maximize
                                FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(4.5, 6, 9, 9), 2)
                                DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(4.5, 6, 9, 9), 2)
                            Case Style.Minimize
                                FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(3, 7.5, 12, 5), 2)
                                DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(3, 7.5, 12, 5), 2)
                        End Select


                    Case MouseMode.Pushed

                        FillRoundedPath(G, SolidBrushHTMlColor("291a2a"), Rect, 2)
                        DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(0, 0, 18.5, 21), 2)
                        Select Case ControlStyle

                            Case Style.Close
                                FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(5.4, 5.4, 8, 11), 2)
                                DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(5.4, 5.4, 8, 11), 2)
                            Case Style.Maximize
                                FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(4.5, 6, 9, 9), 2)
                                DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(4.5, 6, 9, 9), 2)
                            Case Style.Minimize
                                FillRoundedPath(G, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(3, 7.5, 12, 5), 2)
                                DrawRoundedPath(G, GetHTMLColor("231625"), 1, New Rectangle(3, 7.5, 12, 5), 2)
                        End Select

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

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(20, 23)
    End Sub

#End Region

End Class

#End Region

#Region " Horizental TabControl "

Public Class ModalHorizentalTabControl : Inherits TabControl

#Region " Variables "

    Private TabColor As Color = GetHTMLColor("331f35")

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        Alignment = TabAlignment.Top
        SizeMode = TabSizeMode.Fixed
        DrawMode = TabDrawMode.OwnerDrawFixed
        Dock = DockStyle.None
        DoubleBuffered = True
        ItemSize = New Size(110, 35)
        Font = New Font("Ubuntu", 13, FontStyle.Regular, GraphicsUnit.Pixel)
        UpdateStyles()
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            With G
                .Clear(GetHTMLColor("331f35"))
                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                .DrawLine(New Pen(Color.FromArgb(130, GetHTMLColor("5b2960")), 2), New Point(4, ItemSize.Height + 1), New Point(Width - 4, ItemSize.Height + 1))
                For i = 0 To TabCount - 1
                    Dim R As Rectangle = GetTabRect(i)
                    If i = SelectedIndex Then
                        .DrawLine(PenHTMlColor("5b2960", 2), New Point(R.X + 10, ItemSize.Height + 1), New Point(R.X - 10 + R.Width, ItemSize.Height + 1))
                        CentreString(G, TabPages(i).Text, Font, SolidBrushHTMlColor("5b2960"), R)
                        CentreString(G, TabPages(i).Text, Font, New SolidBrush(Color.FromArgb(30, Color.White)), R)
                    Else
                        CentreString(G, TabPages(i).Text, Font, SolidBrushHTMlColor("a89ea9"), R)
                    End If
                Next
            End With
            e.Graphics.DrawImage(B, 0, 0)
            G.Dispose()
            B.Dispose()
        End Using
    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        For Each Tab As TabPage In MyBase.TabPages
            Tab.BackColor = TabColor
        Next
    End Sub

#End Region

End Class

#End Region

#Region " Vertical TabControl "

Public Class ModalVerticalTabControl : Inherits TabControl

#Region " Variables "

    Private TabColor As Color = GetHTMLColor("331f35")
    Private _ShowBorder As Boolean = True

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        SizeMode = TabSizeMode.Fixed
        DrawMode = TabDrawMode.OwnerDrawFixed
        Dock = DockStyle.None
        ItemSize = New Size(30, 120)
        Alignment = TabAlignment.Left
        DoubleBuffered = True
        Font = New Font("Ubuntu", 13, FontStyle.Regular, GraphicsUnit.Pixel)
        UpdateStyles()
    End Sub

#End Region

#Region " Properties "

    Public Property ShowBorder As Boolean
        Get
            Return _ShowBorder
        End Get
        Set(value As Boolean)
            _ShowBorder = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            With G
                .Clear(GetHTMLColor("331f35"))
                For i = 0 To TabCount - 1
                    Dim R As Rectangle = GetTabRect(i)
                    If TabPages(i).Tag IsNot Nothing Then
                        .DrawString(TabPages(i).Text.ToUpper, Font, New SolidBrush(Color.FromArgb(180, GetHTMLColor("5b2960"))), New Point(R.X + 5, R.Y + 9))
                    ElseIf SelectedIndex = i Then
                        .FillRectangle(New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), R)
                        .FillRectangle(New SolidBrush(Color.FromArgb(230, GetHTMLColor("291a2a"))), New Rectangle(R.X, R.Y, 5, R.Height))
                        CentreString(G, TabPages(i).Text, Font, SolidBrushHTMlColor("5b2960"), R)
                        CentreString(G, TabPages(i).Text, Font, New SolidBrush(Color.FromArgb(30, Color.White)), R)
                    Else
                        CentreString(G, TabPages(i).Text, Font, SolidBrushHTMlColor("a89ea9"), R)
                    End If
                Next
                If ShowBorder Then
                    .DrawRectangle(PenHTMlColor("291a2a", 1), New Rectangle(0, 0, Width - 1, Height - 1))
                End If
            End With
            e.Graphics.DrawImage(B.Clone, 0, 0)
            G.Dispose() : B.Dispose()
        End Using
    End Sub

#End Region

#Region " Events "


    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        For Each Tab As TabPage In MyBase.TabPages
            Tab.BackColor = TabColor
        Next
    End Sub

#End Region

End Class

#End Region

#Region " RichTextBox "

<DefaultEvent("TextChanged")> Public Class ModalRichTextbox : Inherits Control

#Region " Variables "

    Private WithEvents T As New RichTextBox
    Private _ReadOnly As Boolean = False
    Private _SideImage As Image
    Private TBC = GetHTMLColor("291a2a")
    Private TFC = GetHTMLColor("a89ea9")
    Private State As MouseMode = MouseMode.NormalMode
    Private _WordWrap As Boolean
    Private _AutoWordSelection As Boolean

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

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property ForeColor As Color
        Get
            Return Color.Transparent
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property BackColor As Color
        Get
            Return Color.Transparent
        End Get
    End Property

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property BackgroungImage As Image()
        Get
            Return Nothing
        End Get
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

    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Overridable Shadows ReadOnly Property Multiline() As Boolean
        Get
            Return True
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

    Public Property WordWrap() As Boolean
        Get
            Return _WordWrap
        End Get
        Set(ByVal value As Boolean)
            _WordWrap = value
            If T IsNot Nothing Then
                T.WordWrap = value
            End If
        End Set
    End Property

    Public Property AutoWordSelection() As Boolean
        Get
            Return _AutoWordSelection
        End Get
        Set(ByVal value As Boolean)
            _AutoWordSelection = value
            If T IsNot Nothing Then
                T.AutoWordSelection = value
            End If
        End Set
    End Property

#End Region

#Region " Initilization "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or _
                  ControlStyles.OptimizedDoubleBuffer Or _
                  ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        WordWrap = True
        AutoWordSelection = False
        Font = New Font("Ubuntu", 10, FontStyle.Regular)
        With T
            .Size = New Size(Width, Height)
            .Multiline = True
            .Cursor = Cursors.IBeam
            .BackColor = TBC
            .ForeColor = TFC
            .BorderStyle = BorderStyle.None
            .Location = New Point(7, 5)
            .Font = Font
        End With
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

    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        MyBase.OnFontChanged(e)
        T.Font = Font
    End Sub

    Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
        MyBase.OnSizeChanged(e)
        T.Size = New Size(Width - 10, Height - 5)
    End Sub

#End Region

#Region " Draw Control "

    Protected NotOverridable Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)
            Dim Rect As New Rectangle(0, 0, Width - 1, Height - 1)

            With G

                .TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

                .FillRectangle(SolidBrushHTMlColor("291a2a"), Rect)
                .DrawRectangle(PenHTMlColor("231625", 1), Rect)

                If ContextMenuStrip IsNot Nothing Then T.ContextMenuStrip = ContextMenuStrip

            End With

            e.Graphics.DrawImage(B.Clone(), 0, 0)
            G.Dispose() : B.Dispose()

        End Using
    End Sub

#End Region

End Class

#End Region

#Region " Switch "

<DefaultEvent("Switch")> Public Class ModalSwitch : Inherits Control

#Region " Variables "

    Private _Switched As Boolean = False

#End Region

#Region " Properties "

    Public Property Switched() As Boolean
        Get
            Return _Switched
        End Get
        Set(ByVal value As Boolean)
            _Switched = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                    ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Cursor = Cursors.Hand
        Font = New Font("Ubuntu", 10, FontStyle.Regular)
        Size = New Size(70, 28)
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G

                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                If Switched Then

                    .FillRectangle(New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(0, 0, 70, 27))

                    .FillRectangle(SolidBrushHTMlColor("291a2a"), New Rectangle(Width - 28.5, 1.5, 25, 23))
                    .DrawRectangle(PenHTMlColor("231625", 1), New Rectangle(Width - 28.5, 1.5, 25, 23))
                    .DrawLine(PenHTMlColor("5b2960", 1), Width - 13, 8, Width - 13, 18)
                    .DrawLine(PenHTMlColor("5b2960", 1), Width - 16, 7, Width - 16, 19)
                    .DrawLine(PenHTMlColor("5b2960", 1), Width - 19, 8, Width - 19, 18)

                    .DrawString("ON", Font, Brushes.Silver, New Point(Width - 62, 5))
                Else

                    .FillRectangle(SolidBrushHTMlColor("291a2a"), New Rectangle(0, 0, 70, 27))

                    .FillRectangle(New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), New Rectangle(3, 1.5, 25, 23))
                    .DrawRectangle(PenHTMlColor("231625", 1), New Rectangle(3, 1.5, 25, 23))

                    .DrawLine(PenHTMlColor("231625", 1), 12, 8, 12, 18)
                    .DrawLine(PenHTMlColor("231625", 1), 15, 7, 15, 19)
                    .DrawLine(PenHTMlColor("231625", 1), 18, 8, 18, 18)

                    .DrawString("OFF", Font, SolidBrushHTMlColor("a89ea9"), New Point(33, 5))

                End If
                .DrawRectangle(PenHTMlColor("231625", 1), New Rectangle(0, 0, 69, 27))
            End With

            e.Graphics.DrawImage(B.Clone(), 0, 0)
            G.Dispose() : B.Dispose()

        End Using

    End Sub

#End Region

#Region " Events "

    Event Switch(ByVal sender As Object)
    Protected Overrides Sub OnClick(ByVal e As EventArgs)
        _Switched = Not _Switched
        RaiseEvent Switch(Me)
        MyBase.OnClick(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnResize(ByVal e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(70, 28)
    End Sub

#End Region

End Class

#End Region

#Region " ContextMenuStrip "

Public Class ModalContextMenuStrip : Inherits ContextMenuStrip

#Region " Variables "

    Private ClickedEventArgs As ToolStripItemClickedEventArgs

#End Region

#Region " Constructors "

    Public Sub New()
        Renderer = New ModalToolStripRender()
        BackColor = GetHTMLColor("291a2a")
    End Sub

#End Region

#Region " Events "

    Event Clicked(ByVal sender As Object)

    Protected Overrides Sub OnItemClicked(e As ToolStripItemClickedEventArgs)
        If Not e.ClickedItem Is Nothing AndAlso Not (TypeOf e.ClickedItem Is ToolStripSeparator) Then
            If e Is ClickedEventArgs Then OnItemClicked(e) Else ClickedEventArgs = e : RaiseEvent Clicked(Me)
        End If
    End Sub

    Protected Overrides Sub OnMouseHover(e As EventArgs)
        MyBase.OnMouseHover(e)
        Cursor = Cursors.Hand
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        Cursor = Cursors.Hand
        Invalidate()
    End Sub

#End Region

    NotInheritable Class ModalToolStripMenuItem : Inherits ToolStripMenuItem

#Region " Constructors "

        Public Sub New()
            AutoSize = False
            Size = New Size(160, 30)
        End Sub

#End Region

#Region " Adding DropDowns "

        Protected Overrides Function CreateDefaultDropDown() As ToolStripDropDown
            If DesignMode Then Return MyBase.CreateDefaultDropDown()
            Dim DP = New ModalContextMenuStrip()
            DP.Items.AddRange(MyBase.CreateDefaultDropDown().Items)
            Return DP
        End Function

#End Region

    End Class

    NotInheritable Class ModalToolStripRender : Inherits ToolStripProfessionalRenderer

#Region " Drawing Text "

        Protected Overrides Sub OnRenderItemText(e As ToolStripItemTextRenderEventArgs)
            With e.Graphics
                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                Dim textRect = New Rectangle(25, e.Item.ContentRectangle.Y, e.Item.ContentRectangle.Width - (24 + 16), e.Item.ContentRectangle.Height - 4)
                Using F As New Font("Ubuntu", 11, FontStyle.Regular), B As Brush = If(e.Item.Enabled, SolidBrushHTMlColor("a89ea9"), New SolidBrush(Color.FromArgb(70, GetHTMLColor("5b2960")))), St As New StringFormat() With {.LineAlignment = StringAlignment.Center}
                    .DrawString(e.Text, F, B, textRect)
                End Using
            End With
        End Sub

#End Region

#Region " Drawing Backgrounds "

        Protected Overrides Sub OnRenderToolStripBackground(e As ToolStripRenderEventArgs)
            MyBase.OnRenderToolStripBackground(e)
            With e.Graphics
                .SmoothingMode = SmoothingMode.AntiAlias
                .InterpolationMode = InterpolationMode.High
                .Clear(GetHTMLColor("291a2a"))
            End With
        End Sub

        Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
            With e.Graphics
                .InterpolationMode = InterpolationMode.High
                .Clear(GetHTMLColor("291a2a"))
                Dim R As New Rectangle(0, e.Item.ContentRectangle.Y - 2, e.Item.ContentRectangle.Width + 4, e.Item.ContentRectangle.Height + 3)

                .FillRectangle(If(e.Item.Selected AndAlso e.Item.Enabled, New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), SolidBrushHTMlColor("291a2a")), R)

            End With
        End Sub

#End Region

#Region " Set Image Margin "

        Protected Overrides Sub OnRenderImageMargin(e As ToolStripRenderEventArgs)
            'MyBase.OnRenderImageMargin(e) 
            'I Make above line comment which makes users to be able to add images to ToolStrips
        End Sub

#End Region

#Region " Drawing Seperators & Borders "

        Protected Overrides Sub OnRenderSeparator(e As ToolStripSeparatorRenderEventArgs)
            With e.Graphics
                .SmoothingMode = SmoothingMode.AntiAlias
                .DrawLine(New Pen(Color.FromArgb(200, GetHTMLColor("231625")), 1), New Point(e.Item.Bounds.Left, e.Item.Bounds.Height / 2), New Point(e.Item.Bounds.Right - 5, e.Item.Bounds.Height / 2))
            End With
        End Sub

        Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
            With e.Graphics
                .InterpolationMode = InterpolationMode.High
                .SmoothingMode = SmoothingMode.AntiAlias
                DrawRoundedPath(e.Graphics, GetHTMLColor("231625"), 1, New Rectangle(e.AffectedBounds.X, e.AffectedBounds.Y, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1), 4)
            End With
        End Sub

#End Region

#Region " Drawing DropDown Arrows "

        Protected Overrides Sub OnRenderArrow(e As ToolStripArrowRenderEventArgs)
            With e.Graphics
                Dim ArrowX, ArrowY As Integer
                ArrowX = e.ArrowRectangle.X + e.ArrowRectangle.Width / 2
                ArrowY = e.ArrowRectangle.Y + e.ArrowRectangle.Height / 2
                Dim ArrowPoints As Point() = New Point() {New Point(ArrowX - 5, ArrowY - 5), New Point(ArrowX, ArrowY), New Point(ArrowX - 5, ArrowY + 5)}
                If e.Item.Enabled Then
                    .FillPolygon(New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), ArrowPoints)
                Else
                    .FillPolygon(New SolidBrush(Color.FromArgb(40, GetHTMLColor("5b2960"))), ArrowPoints)
                End If
            End With
        End Sub

#End Region

    End Class

End Class

#End Region

#Region " Numerical UP & Down "

Public Class ModalNumericUpDown : Inherits Control

#Region " Variables "

    Private X, Y, _Value, _Maximum, _Minimum As Integer

#End Region

#Region " Properties "

    Public Property Value() As Long
        Get
            Return _Value
        End Get
        Set(value As Long)
            If value <= Maximum And value >= Minimum Then _Value = value
            Invalidate()
        End Set

    End Property

    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Integer)
            If value > Minimum Then _Maximum = value
            If value > _Maximum Then value = _Maximum
            Invalidate()
        End Set
    End Property

    Public Property Minimum() As Integer
        Get
            Return _Minimum
        End Get
        Set(ByVal value As Integer)
            If value < Maximum Then _Minimum = value
            If value < _Minimum Then value = _Minimum
            Invalidate()
        End Set
    End Property

#End Region

#Region " Constructors "

    Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        BackColor = Color.Transparent
        Font = New Font("Ubuntu", 10, FontStyle.Regular)
    End Sub

#End Region

#Region " Draw Control "

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)

        Using B As New Bitmap(Width, Height), G As Graphics = Graphics.FromImage(B)

            With G

                .SmoothingMode = SmoothingMode.AntiAlias
                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

                Dim Rect As New Rectangle(1, 1, Width - 2, Height - 2)

                .FillRectangle(SolidBrushHTMlColor("291a2a"), Rect)

                G.FillPath(New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), RoundRec(New Rectangle(Width - 25, 0.6, 23, Height - 2.6), 2))
                .DrawLine(PenHTMlColor("231625", 1), New Point(Width - 25, 1), New Point(Width - 25, Height - 2))
                .DrawRectangle(PenHTMlColor("231625", 1), Rect)

                Using AboveWardTriangle As New GraphicsPath
                    AboveWardTriangle.AddLine(Width - 17, 12, Width - 2, 12)
                    AboveWardTriangle.AddLine(Width - 9, 12, Width - 13, CInt(4.7))
                    AboveWardTriangle.CloseFigure()
                    G.FillPath(SolidBrushHTMlColor("291a2a"), AboveWardTriangle)
                End Using

                Using DownWardTriangle As New GraphicsPath
                    DownWardTriangle.AddLine(Width - 17, 15, Width - 2, 15)
                    DownWardTriangle.AddLine(Width - 9, 15, Width - 13, 22)
                    DownWardTriangle.CloseFigure()
                    G.FillPath(SolidBrushHTMlColor("291a2a"), DownWardTriangle)
                End Using

                CentreString(G, Value, Font, SolidBrushHTMlColor("a89ea9"), New Rectangle(0, 0, Width - 18, Height - 1))

            End With

            e.Graphics.DrawImage(B.Clone, 0, 0)
            G.Dispose() : B.Dispose()

        End Using

    End Sub

#End Region

#Region " Events "

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        Maximum = Integer.MaxValue
        Minimum = 0
    End Sub

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        X = e.Location.X
        Y = e.Location.Y
        Invalidate()
        If e.X < Width - 23 Then Cursor = Cursors.IBeam Else Cursor = Cursors.Hand
    End Sub

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Me.Height = 26
    End Sub

    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseClick(e)
        If X > Width - 17 AndAlso X < Width - 3 Then
            If Y < 13 Then
                If (Value + 1) <= Maximum Then Value += 1
            Else
                If (Value - 1) >= Minimum Then Value -= 1
            End If
        End If
        Invalidate()
    End Sub

#End Region

End Class


#End Region

#Region "ListBox"

Public Class ModalListBox : Inherits Control

#Region "Variables"

    Private WithEvents LB As New ListBox()
    Private _Items As String() = New String() {String.Empty}

#End Region

#Region "Properties"

    Public Property Items() As String()
        Get
            Return _Items
        End Get
        Set(value As String())
            _Items = value
            LB.Items.Clear()
            LB.Items.AddRange(value)
            Invalidate()
        End Set
    End Property

    Public ReadOnly Property SelectedItem() As String
        Get
            Return LB.SelectedItem
        End Get
    End Property

    Public ReadOnly Property SelectedIndex() As Integer
        Get
            If LB.SelectedIndex < 0 Then
                Return 0
            Else
                Return LB.SelectedIndex
            End If
        End Get
    End Property

    Public Sub Clear()
        LB.Items.Clear()
    End Sub

#End Region

#Region "Events"

    Public Sub ClearSelected()
        Dim i As Integer = (LB.SelectedItems.Count - 1)
        While i >= 0
            LB.Items.Remove(LB.SelectedItems(i))
            i += -1
        End While
    End Sub

    Protected Overrides Sub OnCreateControl()
        MyBase.OnCreateControl()
        With LB
            .DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            .ScrollAlwaysVisible = False
            .HorizontalScrollbar = False
            .BorderStyle = BorderStyle.None
            .ItemHeight = 20
            .IntegralHeight = False
        End With
        If Not Controls.Contains(LB) Then Controls.Add(LB)

    End Sub

    Public Sub AddRange(items As Object())
        With LB
            .Items.Remove(String.Empty)
            .Items.AddRange(items)
        End With
    End Sub

    Public Sub AddItem(item As Object)
        With LB
            .Items.Remove(String.Empty)
            .Items.Add(item)
        End With
    End Sub

    Private Sub LB_SelectIndexChanged(sender As Object, e As EventArgs) Handles LB.SelectedIndexChanged
        LB.Invalidate()
    End Sub

#End Region

#Region "Constructors"

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True
        Font = New Font("Ubuntu", 10, FontStyle.Regular)
        BackColor = Color.Transparent
        With LB
            .BackColor = GetHTMLColor("291a2a")
            .ForeColor = GetHTMLColor("a89ea9")
            .Location = New Point(2, 2)
            .Font = Font
            .Items.Clear()
        End With
        Size = New Size(130, 100)
    End Sub

#End Region

#Region "Draw Control"

    Protected Sub OnDrawItem(sender As Object, e As DrawItemEventArgs) Handles LB.DrawItem
        'e.DrawBackground();

        Using B As New Bitmap(Width, Height)
            Using G As Graphics = Graphics.FromImage(B)
                G.SmoothingMode = SmoothingMode.HighQuality
                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                If e.Index < 0 Or Items.Length < 1 Then
                    Return
                End If


                Dim MainRect As New Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + 2, e.Bounds.Height - 1)
                Dim ItemRect As New Rectangle(e.Bounds.X - 1, e.Bounds.Y + 4, e.Bounds.Width, e.Bounds.Height + 2)

                G.FillRectangle(SolidBrushHTMlColor("291a2a"), ItemRect)


                If e.State > 0 Then
                    G.FillRectangle(New SolidBrush(Color.FromArgb(130, GetHTMLColor("5b2960"))), ItemRect)
                    G.DrawRectangle(PenHTMlColor("231625", 1), ItemRect)
                    G.DrawString(Items(e.Index).ToString(), Font, SolidBrushHTMlColor("a89ea9"), 3, e.Bounds.Y + 4)
                Else
                    G.DrawString(Items(e.Index).ToString(), Font, SolidBrushHTMlColor("a89ea9"), 3, e.Bounds.Y + 4)
                End If


                e.Graphics.DrawImage(B, 0, 0)
                G.Dispose()

                B.Dispose()
            End Using
        End Using

    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Using B As New Bitmap(Width, Height)
            Using G As Graphics = Graphics.FromImage(B)

                G.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

                Dim MainRect As New Rectangle(0, 0, Width - 1, Height - 1)

                LB.Size = New Size(Width - 6, Height - 5)
                G.FillRectangle(SolidBrushHTMlColor("291a2a"), MainRect)
                G.DrawRectangle(PenHTMlColor("231625", 1), MainRect)


                e.Graphics.DrawImage(B, 0, 0)
                G.Dispose()

                B.Dispose()
            End Using
        End Using
    End Sub

#End Region

End Class

#End Region