Imports System.Drawing.Drawing2D
Imports System.ComponentModel

'--------------------- [ THEME ] ---------------------
'Name: Heaven Theme
'Creator: Ashlanfox
'Contact: Ashlanfox (Skype)
'Created: 04.01.2015
'Changed: 06.01.2015
'Thanks to: UnReLaTeDScript (Resize & Move events), Mephobia (TabControl)!
'Inspired of: WebappPro (by Ben Garratt).
'-------------------- [ /THEME ] ---------------------

'
Class HeavenTheme
    Inherits ContainerControl

#Region "Properties | Control"
    Private _SecondText As String
    Public Property SecondText() As String
        Get
            Return _SecondText
        End Get
        Set(ByVal v As String)
            _SecondText = v
            Invalidate()
        End Set
    End Property
#End Region

#Region "Functions | Create control"
    Private CreateRoundPath As GraphicsPath
    Function CreateTopRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddRectangle(New Rectangle(r.X, r.Y + (slope / 2), r.Right, r.Y + 40))
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

    Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)

        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)

        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function
#End Region

#Region "Subs | Events Move & Resize"
    Private _TransparencyKey As Color = Color.Empty
    Private _MoveHeight As Integer = 38

    Private ParentIsForm As Boolean
    Protected Overrides Sub OnTextChanged(e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Refresh()
    End Sub

    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        Dock = DockStyle.Fill
        ParentIsForm = TypeOf Parent Is Form
        If ParentIsForm Then
            If Not _TransparencyKey = Color.Empty Then ParentForm.TransparencyKey = _TransparencyKey
            ParentForm.FormBorderStyle = FormBorderStyle.None
        End If
        MyBase.OnHandleCreated(e)
    End Sub

    Private _Resizable As Boolean = True
    Property Resizable() As Boolean
        Get
            Return _Resizable
        End Get
        Set(ByVal value As Boolean)
            _Resizable = value
        End Set
    End Property

    Private Flag As IntPtr
    Protected Overrides Sub OnMouseDown(ByVal e As MouseEventArgs)
        If Not e.Button = MouseButtons.Left Then Return
        If ParentIsForm Then If ParentForm.WindowState = FormWindowState.Maximized Then Return

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
        PTC = PointToClient(MousePosition)
        F1 = PTC.X < 7
        F2 = PTC.X > Width - 7
        F3 = PTC.Y < 7
        F4 = PTC.Y > Height - 7

        If F1 And F3 Then Return New Pointer(Cursors.SizeNWSE, 13)
        If F1 And F4 Then Return New Pointer(Cursors.SizeNESW, 16)
        If F2 And F3 Then Return New Pointer(Cursors.SizeNESW, 14)
        If F2 And F4 Then Return New Pointer(Cursors.SizeNWSE, 17)
        If F1 Then Return New Pointer(Cursors.SizeWE, 10)
        If F2 Then Return New Pointer(Cursors.SizeWE, 11)
        If F3 Then Return New Pointer(Cursors.SizeNS, 12)
        If F4 Then Return New Pointer(Cursors.SizeNS, 15)
        Return New Pointer(Cursors.Default, 0)
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
        Header = New Rectangle(7, 7, Width - 14, _MoveHeight - 7)
        Invalidate()
        MyBase.OnSizeChanged(e)
    End Sub

#End Region

    Sub New()
        DoubleBuffered = True
        _TransparencyKey = Color.Fuchsia

        Font = New Font("Arial", 9, FontStyle.Regular)
        SecondText = "Pro"
        BackColor = Color.White
        ForeColor = Color.FromArgb(225, 229, 208)
        SetStyle(DirectCast(139270, ControlStyles), True)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.Clear(Color.Fuchsia)

        G.SmoothingMode = SmoothingMode.Default
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        G.InterpolationMode = InterpolationMode.HighQualityBicubic

        Dim high As Integer = 30
        Dim slope As Integer = 10
        Dim FT As New Font("Arial", 14, FontStyle.Bold)

        G.FillPath(New SolidBrush(Color.FromArgb(242, 242, 242)), CreateRound(New Rectangle(0, 0, Width, Height), slope + 3))
        G.DrawPath(New Pen(Color.FromArgb(157, 157, 157)), CreateRound(New Rectangle(0, 0, Width - 1, Height - 1), slope + 1))
        G.FillPath(New SolidBrush(Color.FromArgb(203, 203, 203)), CreateTopRound(New Rectangle(0, 0, Width, high), slope))
        G.FillRectangle(New SolidBrush(Color.FromArgb(55, 55, 55)), New Rectangle(0, high, Width, high * 2))

        Dim TextH As Integer = e.Graphics.MeasureString(Text, FT).Height
        Dim TextW As Integer = e.Graphics.MeasureString(Text, FT).Width

        G.DrawString(Text, New Font(Font.FontFamily, FT.Size, FontStyle.Bold), New SolidBrush(ForeColor), New Point(15, (high * 2) - (TextH / 2) + 1))
        G.DrawString(SecondText, New Font(Font.FontFamily, FT.Size, FontStyle.Regular), New SolidBrush(ForeColor), New Point(12 + TextW, (high * 2) - (TextH / 2) + 1))

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.High
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class HeavenTopControl
    Inherits Control

#Region "Properties | Settings"
    Private State As MouseState

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum

    Enum BType As Byte
        Close = 0
        Hide = 1
        Max = 2
    End Enum

    Private _ButtonType As BType = BType.Close
    Public Property ButtonType() As BType
        Get
            Return _ButtonType
        End Get
        Set(ByVal v As BType)
            _ButtonType = v
            Invalidate()
        End Set
    End Property
#End Region

#Region "Subs | Events"
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
    Protected Overrides Sub OnResize(e As System.EventArgs)
        MyBase.OnResize(e)
        Size = New Size(11, 11)
    End Sub
#End Region

#Region "Function | Create round"
    Private CreateRoundPath As GraphicsPath
    Function CreateRound(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)

        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Bottom - slope, slope, slope, 0.0F, 90.0F)
        CreateRoundPath.AddArc(r.X, r.Bottom - slope, slope, slope, 90.0F, 90.0F)

        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function
#End Region

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint Or _
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.SupportsTransparentBackColor, True)
        DoubleBuffered = True

        BackColor = Color.Transparent
        Size = New Size(11, 11)

        Cursor = Cursors.Hand
        Font = New Font("Marlett", 8)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim ColorH As New Color

        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        G.InterpolationMode = InterpolationMode.HighQualityBicubic

        G.Clear(BackColor)

        Select Case State
            Case MouseState.None
                G.FillEllipse(New SolidBrush(Color.White), New Rectangle(0, 0, Width - 1, Height - 1))
            Case MouseState.Down
                Select Case ButtonType
                    Case BType.Close
                        G.FillEllipse(New SolidBrush(Color.FromArgb(230, 59, 70)), New Rectangle(0, 0, Width - 1, Height - 1))
                        G.FillEllipse(New SolidBrush(Color.FromArgb(200, 59, 70)), New Rectangle(2, 2, Width - 5, Height - 5))
                    Case BType.Hide
                        G.FillEllipse(New SolidBrush(Color.FromArgb(125, 220, 128)), New Rectangle(0, 0, Width - 1, Height - 1))
                        G.FillEllipse(New SolidBrush(Color.FromArgb(95, 220, 128)), New Rectangle(2, 2, Width - 5, Height - 5))
                    Case BType.Max
                        G.FillEllipse(New SolidBrush(Color.FromArgb(250, 194, 90)), New Rectangle(0, 0, Width - 1, Height - 1))
                        G.FillEllipse(New SolidBrush(Color.FromArgb(220, 194, 90)), New Rectangle(2, 2, Width - 5, Height - 5))
                End Select
            Case MouseState.Over
                Select Case ButtonType
                    Case BType.Close
                        G.FillEllipse(New SolidBrush(Color.FromArgb(230, 59, 70)), New Rectangle(0, 0, Width - 1, Height - 1))
                    Case BType.Hide
                        G.FillEllipse(New SolidBrush(Color.FromArgb(125, 220, 128)), New Rectangle(0, 0, Width - 1, Height - 1))
                    Case BType.Max
                        G.FillEllipse(New SolidBrush(Color.FromArgb(250, 194, 90)), New Rectangle(0, 0, Width - 1, Height - 1))
                End Select
        End Select

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.High
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Public Class HeavenTabControl
    Inherits TabControl

#Region "Properties | Settings"
    Private _TabRectLines As Boolean = True
    <Category("Settings")> Public Property TabRectLines As Boolean
        Get
            Return _TabRectLines
        End Get
        Set(ByVal v As Boolean)
            _TabRectLines = v
            Invalidate()
        End Set
    End Property

    Private _TabRectColors As Boolean = True
    <Category("Settings")> Public Property TabRectColors() As Boolean
        Get
            Return _TabRectColors
        End Get
        Set(ByVal v As Boolean)
            _TabRectColors = v
            Invalidate()
        End Set
    End Property

    Private _SelectedTabColor As Color = Color.FromArgb(222, 98, 98)
    <Category("Settings")> Public Property SelectedTabColor() As Color
        Get
            Return _SelectedTabColor
        End Get
        Set(ByVal v As Color)
            _SelectedTabColor = v
            Invalidate()
        End Set
    End Property
#End Region

    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
    End Sub

    Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.ResizeRedraw Or ControlStyles.UserPaint Or ControlStyles.DoubleBuffer, True)
        DoubleBuffered = True
        SizeMode = TabSizeMode.Fixed
        ItemSize = New Size(50, 200)
    End Sub
    Protected Overrides Sub CreateHandle()
        MyBase.CreateHandle()
        Alignment = TabAlignment.Left
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        Dim FT As New Font("Arial", 10, FontStyle.Bold)
        Dim colorBG As Color = Color.FromArgb(242, 242, 242)

        G.Clear(colorBG)

        Dim heightTB As Integer = Height - 8
        Dim widthTB As Integer = Width - (ItemSize.Height + 8)

        G.FillRectangle(New SolidBrush(SelectedTabColor), New Rectangle(ItemSize.Height + 4, 0, widthTB, 4))

        For i = 0 To TabCount - 1

            If TabRectLines Then
                G.DrawLine(New Pen(Color.FromArgb(225, 229, 208)), New Point(GetTabRect(i).X - 2, GetTabRect(i).Bottom - 3), New Point(GetTabRect(i).Right + 1, GetTabRect(i).Bottom - 3))
                G.DrawLine(New Pen(Color.FromArgb(225, 229, 208)), New Point((ItemSize.Height / 4) - 1, GetTabRect(i).Y - 2), New Point((ItemSize.Height / 4) - 1, GetTabRect(i).Bottom - 3))
            End If

            G.DrawString(TabPages(i).Text.ToUpper, FT, New SolidBrush(Color.FromArgb(111, 109, 110)), New Point(GetTabRect(i).X + (ItemSize.Height / 4) + 13, GetTabRect(i).Y + (GetTabRect(i).Height / 2) - (e.Graphics.MeasureString(TabPages(i).Text, FT).Height / 2) - 3))

            If i = SelectedIndex Then

                G.FillRectangle(New SolidBrush(SelectedTabColor), New Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y - 2, (ItemSize.Height / 4), ItemSize.Width))

                Dim bmp As New Bitmap(Width, Height)
                Dim graph As Graphics = Graphics.FromImage(bmp)

                graph.FillRectangle(New SolidBrush(colorBG), New Rectangle(0, 0, widthTB, heightTB))

                graph.FillRectangle(New SolidBrush(Color.FromArgb(247, 246, 242)), New Rectangle(0, 0, widthTB, 75))
                graph.DrawLine(New Pen(Color.FromArgb(206, 204, 192)), New Point(0, 75), New Point(widthTB, 75))

                graph.FillPolygon(New SolidBrush(Color.FromArgb(247, 246, 242)), {New Point(45, 75), New Point(51, 85), New Point(57, 75)})

                graph.SmoothingMode = SmoothingMode.HighQuality : graph.DrawLine(New Pen(Color.FromArgb(206, 204, 192)), New Point(45, 75), New Point(51, 85))
                graph.DrawLine(New Pen(Color.FromArgb(206, 204, 192)), New Point(51, 85), New Point(57, 75)) : graph.SmoothingMode = SmoothingMode.Default

                graph.DrawLine(New Pen(Color.FromArgb(225, 229, 208)), New Point(0, 0), New Point(0, heightTB))
                graph.DrawLine(New Pen(Color.FromArgb(100, 225, 229, 208)), New Point(1, 0), New Point(1, heightTB))
                graph.DrawLine(New Pen(Color.FromArgb(50, 225, 229, 208)), New Point(2, 0), New Point(2, heightTB))

                graph.DrawString(TabPages(i).Text.ToUpper, FT, New SolidBrush(Color.FromArgb(212, 210, 195)), New Point(widthTB - e.Graphics.MeasureString(TabPages(i).Text, FT).Width - 30, 35 - (e.Graphics.MeasureString(TabPages(i).Text, FT).Height / 2)))


                Dim pX As Integer = widthTB - e.Graphics.MeasureString(TabPages(i).Text, FT).Width - 30 - 15
                Dim pY As Integer = 38 - (e.Graphics.MeasureString(TabPages(i).Text, FT).Height / 2)

                Dim intY As Integer = (e.Graphics.MeasureString(TabPages(i).Text, FT).Height / 4)

                graph.DrawLine(New Pen(Color.FromArgb(212, 210, 195)), pX, pY, pX + 10, pY)
                graph.DrawLine(New Pen(Color.FromArgb(212, 210, 195)), pX, pY + intY, pX + 10, pY + intY)
                graph.DrawLine(New Pen(Color.FromArgb(212, 210, 195)), pX, pY + (intY * 2), pX + 10, pY + (intY * 2))


                SelectedTab.BackgroundImage = bmp
                SelectedTab.BackgroundImageLayout = ImageLayout.Center
            Else

                If TabRectColors Then G.FillRectangle(New SolidBrush(TabPages(i).BackColor), New Rectangle(GetTabRect(i).X - 2, GetTabRect(i).Y - 2, 3, ItemSize.Width))
            End If

            Try
                If ImageList.Images(TabPages(i).ImageIndex) IsNot Nothing Then
                    Dim imgW As Integer = ImageList.Images(TabPages(i).ImageIndex).Width
                    Dim imgH As Integer = ImageList.Images(TabPages(i).ImageIndex).Width

                    Dim pX As Integer = GetTabRect(i).X + (((ItemSize.Height / 4) - imgW) / 2)
                    Dim pY As Integer = GetTabRect(i).Y + ((ItemSize.Width - imgH) / 2)

                    G.DrawImage(ImageList.Images(TabPages(i).ImageIndex), New Point(pX - 2, pY - 2))
                End If
            Catch : End Try

        Next

        e.Graphics.DrawImage(B.Clone, 0, 0)
        G.Dispose() : B.Dispose()
    End Sub
End Class