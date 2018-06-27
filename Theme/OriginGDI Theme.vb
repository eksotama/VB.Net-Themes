Imports System.Drawing.Drawing2D

'--------------------- [ THEME ] ---------------------
'Name: Origin theme
'Creator: Ashlanfox
'Contact: Ashlanfox (Skype)
'Created: 11.08.2014
'Changed: 13.08.2014
'Thanks to: Aeonhack (theme base), iSynthesis (topbutton example), ᒪeumonic (checkbox example) and Aeonhack (textbox example)!
'Inspirated of: Origin client
'-------------------- [ /THEME ] ---------------------

Class OriginForm
    Inherits ThemeContainer154

#Region "Functions | Create form"
    Private CreateRoundPath As GraphicsPath
    Function CreateTop(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)
        CreateRoundPath.AddRectangle(New Rectangle(r.X, r.Y + (slope / 2), r.Right, r.Y + 23))
        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

    Function CreateBottom(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddLine(New Point(r.Right, r.Height - 50), New Point(r.Right, r.Height))
        CreateRoundPath.AddLine(New Point(r.Right, r.Height), New Point(r.X + 4, r.Height))
        CreateRoundPath.AddLine(New Point(r.X + 4, r.Height), New Point(r.X, r.Height - 4))
        CreateRoundPath.AddLine(New Point(r.X, r.Height - 4), New Point(r.X, r.Height - 50))

        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function

    Function CreateForm(ByVal r As Rectangle, ByVal slope As Integer) As GraphicsPath
        CreateRoundPath = New GraphicsPath(FillMode.Winding)
        CreateRoundPath.AddArc(r.X, r.Y, slope, slope, 180.0F, 90.0F)
        CreateRoundPath.AddArc(r.Right - slope, r.Y, slope, slope, 270.0F, 90.0F)

        CreateRoundPath.AddLine(New Point(r.Right, r.Y + (slope / 2)), New Point(r.Right, r.Height))
        CreateRoundPath.AddLine(New Point(r.Right, r.Height), New Point(r.X + 4, r.Height))
        CreateRoundPath.AddLine(New Point(r.X + 4, r.Height), New Point(r.X, r.Height - 4))
        CreateRoundPath.AddLine(New Point(r.X, r.Height - 4), New Point(r.X, r.Y + (slope / 2)))

        CreateRoundPath.CloseFigure()
        Return CreateRoundPath
    End Function
#End Region

    Sub New()
        DoubleBuffered = True
    End Sub

    Protected Overrides Sub ColorHook()
        BackColor = Color.White
        Font = New Font("Verdana", 9)
    End Sub

    Protected Overrides Sub PaintHook()
        TransparencyKey = Color.Fuchsia
        G.Clear(Color.Fuchsia)

        G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        G.InterpolationMode = InterpolationMode.HighQualityBicubic

        Dim BH As Integer = 30

        Dim LBR1 As New LinearGradientBrush(New Rectangle(0, 0, Width, BH), Color.FromArgb(44, 44, 44), Color.FromArgb(27, 27, 27), 90S)
        Dim LBR2 As New LinearGradientBrush(New Rectangle(0, BH, Width, Height), Color.FromArgb(246, 246, 246), Color.FromArgb(230, 230, 230), 270S)
        Dim LBR3 As New LinearGradientBrush(New Point(30, 1), New Point(Width - 31, 1), Color.FromArgb(35, 35, 35), Color.FromArgb(90, 90, 90))
        LBR3.SetBlendTriangularShape(0.5F, 1.0F)
        Dim LBR4 As New LinearGradientBrush(New Rectangle(0, 0, Width, Height), Color.FromArgb(27, 27, 27), Color.FromArgb(206, 206, 206), 90S)

        Dim GP1 As GraphicsPath = CreateTop(New Rectangle(0, 0, Width - 2, Height - 2), 14)
        Dim GP2 As GraphicsPath = CreateForm(New Rectangle(0, 0, Width - 2, Height - 2), 14)
        Dim GP3 As GraphicsPath = CreateBottom(New Rectangle(0, 0, Width - 2, Height - 2), 14)


        G.FillPath(LBR2, GP2)
        G.FillPath(New SolidBrush(Color.FromArgb(206, 206, 206)), GP3)
        G.FillPath(LBR1, GP1)

        G.DrawLine(New Pen(New SolidBrush(Color.FromArgb(206, 206, 206))), New Point(0, Height - 54), New Point(Width - 3, Height - 54))
        G.DrawPath(New Pen(LBR4), GP2)

        G.SmoothingMode = SmoothingMode.HighQuality : G.DrawLine(New Pen(LBR3), New Point(50, 1), New Point(Width - 51, 1))

        DrawText(Brushes.White, HorizontalAlignment.Center, 0, 4)
        G.DrawIcon(FindForm.Icon, New Rectangle(5, 5, 19, 19))
    End Sub

End Class

Class OriginTopButton
    Inherits Control


#Region "Properties | Settings"

    Private State As MouseState
    Dim MOUSEPOSx As Integer

    Enum MouseState As Byte
        None = 0
        Over = 1
        Down = 2
        Block = 3
    End Enum

    Enum BType As Byte
        Close = 0
        Hide = 1
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
    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        Select Case ButtonType
            Case BType.Close
                Environment.Exit(0)
            Case BType.Hide
                FindForm.WindowState = FormWindowState.Minimized
        End Select

    End Sub
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        Size = New Size(23, 18)
    End Sub
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
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyBase.OnMouseMove(e)
        MOUSEPOSx = e.X
        Invalidate()
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
                 ControlStyles.ResizeRedraw Or ControlStyles.OptimizedDoubleBuffer, True)
        DoubleBuffered = True

        BackColor = Color.White
        Size = New Size(23, 18)
        Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Font = New Font("Marlett", 8)
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.PixelOffsetMode = PixelOffsetMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        G.Clear(BackColor)

        Dim r As Rectangle = New Rectangle(0, 0, Width, Height)
        Dim GP As GraphicsPath = CreateRound(New Rectangle(5, 0, Width - 5, Height), 7)

        G.FillRectangle(New LinearGradientBrush(r, Color.FromArgb(30, 30, 30), Color.FromArgb(40, 40, 40), 270S), r)

        Select Case State
            Case MouseState.Over
                G.FillPath(New SolidBrush(Color.FromArgb(245, Color.FromArgb(72, 72, 72))), GP)
            Case MouseState.Down
                G.FillPath(New SolidBrush(Color.FromArgb(245, Color.FromArgb(85, 85, 85))), GP)
        End Select

        Select Case ButtonType
            Case BType.Close
                G.DrawString("r", New Font("Marlett", 8), New SolidBrush(Color.FromArgb(170, 170, 170)), New Rectangle(3, 1, Width, Height), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            Case BType.Hide
                G.DrawString("0", New Font("Marlett", 13), New SolidBrush(Color.FromArgb(170, 170, 170)), New Rectangle(4, -2, Width, Height), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
        End Select

        G.FillRectangle(New SolidBrush(Color.FromArgb(88, 88, 88)), New Rectangle(0, 0, 1, 1))
        G.FillRectangle(New SolidBrush(Color.FromArgb(88, 88, 88)), New Rectangle(0, 3, 1, 1))
        G.FillRectangle(New SolidBrush(Color.FromArgb(88, 88, 88)), New Rectangle(0, 6, 1, 1))
        G.FillRectangle(New SolidBrush(Color.FromArgb(88, 88, 88)), New Rectangle(0, 9, 1, 1))
        G.FillRectangle(New SolidBrush(Color.FromArgb(88, 88, 88)), New Rectangle(0, 12, 1, 1))
        G.FillRectangle(New SolidBrush(Color.FromArgb(88, 88, 88)), New Rectangle(0, 15, 1, 1))

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.High
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class OriginTextBox
    Inherits Control

#Region "Properties | Settings"
    Private State As TBState = TBState.LostFocus
    Enum TBState As Byte
        GotFocus = 0
        LostFocus = 1
    End Enum

    Private _TextAlign As HorizontalAlignment = HorizontalAlignment.Left
    Property TextAlign() As HorizontalAlignment
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
    Property MaxLength() As Integer
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

    Private _ReadOnly As Boolean = False
    Property [ReadOnly]() As Boolean
        Get
            Return _ReadOnly
        End Get
        Set(ByVal value As Boolean)
            _ReadOnly = value
            If Base IsNot Nothing Then
                Base.ReadOnly = value
            End If
        End Set
    End Property

    Private _UseSystemPasswordChar As Boolean = False
    Property UseSystemPasswordChar() As Boolean
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

    Private _Multiline As Boolean = False
    Property Multiline() As Boolean
        Get
            Return _Multiline
        End Get
        Set(ByVal value As Boolean)
            _Multiline = value
            If Base IsNot Nothing Then
                Base.Multiline = value

                If value Then
                    Base.Height = Height - 11
                Else
                    Height = Base.Height + 11
                End If
            End If
        End Set
    End Property

    Overrides Property Text As String
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

    Overrides Property Font As Font
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As Font)
            MyBase.Font = value
            If Base IsNot Nothing Then
                Base.Font = value
                Base.Location = New Point(5, 5)
                Base.Width = Width - 8

                If Not _Multiline Then
                    Height = Base.Height + 11
                End If
            End If
        End Set
    End Property

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        If Not Controls.Contains(Base) Then
            Controls.Add(Base)
        End If

        MyBase.OnHandleCreated(e)
    End Sub

    Private WithEvents Base As TextBox
#End Region

#Region "Subs | Events"

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

        Base.Width = Width - 20
        Base.Height = Height - 11

        MyBase.OnResize(e)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        Base.Focus()
        MyBase.OnMouseDown(e)
    End Sub

    Protected Overrides Sub OnEnter(e As EventArgs)
        Base.Focus()
        Invalidate()
        MyBase.OnEnter(e)
    End Sub

    Private Sub TBGotFocus() Handles Base.GotFocus
        State = TBState.GotFocus
        Refresh()
    End Sub

    Private Sub TBLostFocus() Handles Base.LostFocus
        State = TBState.LostFocus
        Refresh()
    End Sub
#End Region


    Sub New()
        SetStyle(DirectCast(139286, ControlStyles), True)
        SetStyle(ControlStyles.Selectable, True)

        Cursor = Cursors.IBeam

        Base = New TextBox
        Base.Font = New Font("Verdana", 9)
        Base.Text = ""
        Base.Size = New Size(75, 35)
        Base.Width += 10
        Base.MaxLength = _MaxLength
        Base.Multiline = _Multiline
        Base.ReadOnly = _ReadOnly
        Base.UseSystemPasswordChar = _UseSystemPasswordChar

        Base.ForeColor = Color.FromArgb(85, 85, 85)
        Base.BackColor = Color.White

        Base.BorderStyle = BorderStyle.None

        Base.Location = New Point(5, 8)
        Base.Width = Width - 14

        If _Multiline Then
            Base.Height = Height - 11
        Else
            Height = Base.Height + 11
        End If

        AddHandler Base.TextChanged, AddressOf OnBaseTextChanged
        AddHandler Base.KeyDown, AddressOf OnBaseKeyDown
    End Sub


    Protected Overrides Sub OnPaint(e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)

        G.SmoothingMode = SmoothingMode.HighQuality
        G.PixelOffsetMode = PixelOffsetMode.HighQuality
        G.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

        G.Clear(BackColor)

        Height = Base.Height + 12
        Base.Location = New Point(10, (Height / 2) - 8)

        Dim GP1 As New Rectangle(0, 0, Width, Height)

        Select Case State
            Case 0
                G.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(238, 189, 103)), 6), GP1)
                G.DrawRectangle(New Pen(Brushes.White, 2), GP1)
            Case 1
                G.FillRectangle(Brushes.White, GP1)
                G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 1), Color.FromArgb(158, 158, 158), Color.FromArgb(193, 193, 193))), New Point(1, 1), New Point(1, Height - 1))
                G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(Width - 1, 0), Color.FromArgb(158, 158, 158), Color.FromArgb(193, 193, 193))), New Point(1, 1), New Point(Width - 1, 1))
                G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 1), Color.FromArgb(205, 205, 205), Color.FromArgb(225, 225, 225))), New Point(2, 1), New Point(2, Height - 1))
                G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(Width - 1, 0), Color.FromArgb(205, 205, 205), Color.FromArgb(219, 219, 219))), New Point(1, 2), New Point(Width - 1, 2))

                G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(Width - 1, 0), Color.FromArgb(235, 235, 235), Color.FromArgb(249, 249, 249))), New Point(1, Height - 1), New Point(Width - 1, Height - 1))
                G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, Height - 1), Color.FromArgb(235, 235, 235), Color.FromArgb(249, 249, 249))), New Point(Width - 1, 1), New Point(Width - 1, Height - 1))
                G.DrawRectangle(New Pen(Brushes.White, 2), GP1)
        End Select

        MyBase.OnPaint(e)
        G.Dispose()
        e.Graphics.InterpolationMode = InterpolationMode.High
        e.Graphics.DrawImageUnscaled(B, 0, 0)
        B.Dispose()
    End Sub
End Class

Class OriginButton
    Inherits ThemeControl154

#Region "Variables | Colors"
    Private TC As Color = Color.White
#End Region

    Sub New()
        Font = New Font("Verdana", 9, FontStyle.Bold)
    End Sub

    Protected Overrides Sub ColorHook()
    End Sub

    Protected Overrides Sub PaintHook()
        G.SmoothingMode = SmoothingMode.HighQuality
        G.Clear(Color.FromArgb(235, 235, 235))

        If Enabled = False Then State = MouseState.Block

        Select Case State
            Case MouseState.None

                Dim LB1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(255, 191, 0), Color.FromArgb(254, 107, 0), 90.0F)
                Dim LB2 As New LinearGradientBrush(New Point(-10, 0), New Point(Width + 10, 0), Color.FromArgb(255, 191, 0), Color.FromArgb(255, 255, 0))
                LB2.SetSigmaBellShape(0.5F)
                Dim LB3 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(253, 68, 0), Color.FromArgb(255, 165, 0), 270.0F)

                Dim GP As GraphicsPath = CreateRound(3, 4, Width - 7, Height - 7, 8)

                G.DrawPath(New Pen(Color.FromArgb(3, Color.Black), 3), CreateRound(2, 4, Width - 5, Height - 5, 8))
                G.DrawPath(New Pen(Color.FromArgb(9, Color.Black), 2), CreateRound(2, 3, Width - 5, Height - 5, 8))
                G.DrawPath(New Pen(Color.FromArgb(40, Color.Black), 1), CreateRound(2, 4, Width - 5, Height - 6, 8))

                G.FillPath(LB1, GP)
                G.DrawPath(New Pen(LB3), GP)

                G.DrawLine(New Pen(LB2, 1), New Point(5, 5), New Point(Width - 5, 5))
            Case MouseState.Over

                Cursor = Cursors.Hand
                Dim LB1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(255, 238, 0), Color.FromArgb(255, 113, 0), 90.0F)
                Dim LB2 As New LinearGradientBrush(New Point(-10, 0), New Point(Width + 10, 0), Color.FromArgb(255, 191, 0), Color.FromArgb(255, 255, 0))
                LB2.SetSigmaBellShape(0.5F)
                Dim LB3 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(253, 68, 0), Color.FromArgb(255, 165, 0), 270.0F)

                Dim GP As GraphicsPath = CreateRound(3, 4, Width - 7, Height - 7, 8)

                G.DrawPath(New Pen(Color.FromArgb(3, Color.Black), 3), CreateRound(2, 4, Width - 5, Height - 5, 8))
                G.DrawPath(New Pen(Color.FromArgb(9, Color.Black), 2), CreateRound(2, 3, Width - 5, Height - 5, 8))
                G.DrawPath(New Pen(Color.FromArgb(40, Color.Black), 1), CreateRound(2, 4, Width - 5, Height - 6, 8))

                G.FillPath(LB1, GP)
                G.DrawPath(New Pen(LB3), GP)

                G.DrawLine(New Pen(LB2, 1), New Point(5, 5), New Point(Width - 5, 5))
            Case MouseState.Down

                Dim LB1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(222, 103, 0), Color.FromArgb(242, 194, 0), 90.0F)
                Dim LB3 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(234, 128, 0), Color.FromArgb(208, 68, 0), 270.0F)

                Dim GP As GraphicsPath = CreateRound(3, 4, Width - 7, Height - 7, 8)

                G.FillPath(LB1, GP)
                G.DrawPath(New Pen(LB3), GP)

                G.DrawPath(New Pen(Color.FromArgb(18, Color.Black), 2), CreateRound(4, 5, Width - 8, Height - 8, 8))
            Case MouseState.Block

                Dim LB1 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(255, 191, 0), Color.FromArgb(254, 107, 0), 90.0F)
                Dim LB2 As New LinearGradientBrush(New Point(-10, 0), New Point(Width + 10, 0), Color.FromArgb(255, 191, 0), Color.FromArgb(255, 255, 0))
                LB2.SetSigmaBellShape(0.5F)
                Dim LB3 As New LinearGradientBrush(New Rectangle(0, 0, Width - 1, Height - 1), Color.FromArgb(253, 68, 0), Color.FromArgb(255, 165, 0), 270.0F)

                Dim GP As GraphicsPath = CreateRound(3, 4, Width - 7, Height - 7, 8)

                G.DrawPath(New Pen(Color.FromArgb(3, Color.Black), 3), CreateRound(2, 4, Width - 5, Height - 5, 8))
                G.DrawPath(New Pen(Color.FromArgb(9, Color.Black), 2), CreateRound(2, 3, Width - 5, Height - 5, 8))
                G.DrawPath(New Pen(Color.FromArgb(40, Color.Black), 1), CreateRound(2, 4, Width - 5, Height - 6, 8))

                G.FillPath(LB1, GP)
                G.DrawPath(New Pen(LB3), GP)

                G.DrawLine(New Pen(LB2, 1), New Point(5, 5), New Point(Width - 5, 5))

                G.FillRectangle(New SolidBrush(Color.FromArgb(120, Color.FromArgb(235, 235, 235))), New Rectangle(0, 0, Width, Height))
        End Select

        DrawText(New SolidBrush(TC), HorizontalAlignment.Center, 0, 1)
    End Sub
End Class

Class OriginCheckBox
    Inherits Control

#Region "Subs | Events"
    Private State As MouseState = MouseState.None
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
    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        MyBase.OnTextChanged(e)
        Width = CreateGraphics().MeasureString(Text, Font).Width + (2 * 3) + Height
        Invalidate()
    End Sub
    Private _MOUSEPOS As New Point
    Protected Overrides Sub OnMouseMove(e As System.Windows.Forms.MouseEventArgs)
        _MOUSEPOS.X = e.X
        _MOUSEPOS.Y = e.Y
        If HandsCursorOnSquare = HOS.Enable Then Refresh()
        MyBase.OnMouseMove(e)
    End Sub
    Private _Checked As Boolean
    Property Checked() As Boolean
        Get
            Return _Checked
        End Get
        Set(ByVal value As Boolean)
            _Checked = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        MyBase.OnResize(e)
        Height = 23
    End Sub
    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub
    Event CheckedChanged(ByVal sender As Object)
#End Region

#Region "Properties | Hands cursor on square"
    Enum HOS As Byte
        Enable = 0
        Disable = 1
    End Enum

    Private _HandsCursorOnSquare As HOS = HOS.Disable
    Public Property HandsCursorOnSquare() As HOS
        Get
            Return _HandsCursorOnSquare
        End Get
        Set(ByVal v As HOS)
            _HandsCursorOnSquare = v
            Invalidate()
        End Set
    End Property
#End Region

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)

        BackColor = Color.Transparent
        ForeColor = Color.White
        Size = New Size(147, 22)
        DoubleBuffered = True
        Font = New Font("Verdana", 8)
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim B As New Bitmap(Width, Height)
        Dim G As Graphics = Graphics.FromImage(B)
        G.SmoothingMode = SmoothingMode.HighQuality

        G.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
        Dim GP1 As New Rectangle(0, 0, Height - 1, Height - 1)

        G.Clear(Color.Transparent)

        If HandsCursorOnSquare = HOS.Enable Then
            If _MOUSEPOS.X < Height - 1 Then
                Cursor = Cursors.Hand
            Else
                Cursor = Cursors.Default
            End If
        End If

        G.FillRectangle(Brushes.White, GP1)
        G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, GP1.Height - 1), Color.FromArgb(158, 158, 158), Color.FromArgb(193, 193, 193))), New Point(1, 1), New Point(1, GP1.Height - 1))
        G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(GP1.Width - 1, 0), Color.FromArgb(158, 158, 158), Color.FromArgb(193, 193, 193))), New Point(1, 1), New Point(GP1.Width - 1, 1))
        G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, GP1.Height - 1), Color.FromArgb(205, 205, 205), Color.FromArgb(225, 225, 225))), New Point(2, 1), New Point(2, GP1.Height - 1))
        G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(GP1.Width - 1, 0), Color.FromArgb(205, 205, 205), Color.FromArgb(219, 219, 219))), New Point(1, 2), New Point(GP1.Width - 1, 2))

        G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(GP1.Width - 1, 0), Color.FromArgb(235, 235, 235), Color.FromArgb(249, 249, 249))), New Point(1, GP1.Height - 1), New Point(GP1.Width - 1, GP1.Height - 1))
        G.DrawLine(New Pen(New LinearGradientBrush(New Point(0, 0), New Point(0, GP1.Height - 1), Color.FromArgb(235, 235, 235), Color.FromArgb(249, 249, 249))), New Point(GP1.Width - 1, 1), New Point(GP1.Width - 1, GP1.Height - 1))
        G.DrawRectangle(New Pen(Brushes.White, 1), GP1)

        If Checked Then
            Dim chkPoly As Rectangle = New Rectangle(GP1.X + GP1.Width / 4, GP1.Y + GP1.Height / 4, GP1.Width \ 2, GP1.Height \ 2)

            chkPoly.X -= 2
            chkPoly.Y -= 2
            chkPoly.Width += 2
            chkPoly.Height += 3

            Dim Poly() As Point = {New Point(chkPoly.X + 1, chkPoly.Y + chkPoly.Height \ 2), New Point(chkPoly.X + chkPoly.Width \ 2, chkPoly.Y + chkPoly.Height - 1), New Point(chkPoly.X + chkPoly.Width, chkPoly.Y)}
            For i = 0 To Poly.Length - 2 : G.DrawLine(New Pen(Color.FromArgb(255, 166, 32), 3), Poly(i), Poly(i + 1)) : Next
        End If


        G.DrawString(Text, Font, New SolidBrush(Color.FromArgb(68, 68, 68)), New Point(27, 4), New StringFormat With {.Alignment = StringAlignment.Near, .LineAlignment = StringAlignment.Near})

        e.Graphics.DrawImage(B.Clone(), 0, 0)
        G.Dispose() : B.Dispose()

    End Sub

End Class