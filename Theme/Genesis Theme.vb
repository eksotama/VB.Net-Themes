Imports System.Drawing.Drawing2D
Imports System.ComponentModel

Class genesisTheme
    Inherits ThemeContainer151

    Sub New()
        TransparencyKey = Color.Fuchsia

        SetColor("BackColor", Color.FromArgb(33, 33, 33))
        SetColor("BorderColor", Color.Black)
        SetColor("InsetBorderColor", Color.FromArgb(72, 72, 72))
        SetColor("TextColor", Color.White)
        SetColor("TopGloss", Color.FromArgb(62, Color.White))
        SetColor("BottomGloss", Color.FromArgb(30, Color.White))

        BackColor = Color.FromArgb(33, 33, 33)
        ForeColor = Color.White
        Sizable = False
        _ShowIcon = False
    End Sub
    Dim C1, C2, C3, C4, C5, C6 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("BackColor")
        C2 = GetColor("BorderColor")
        C3 = GetColor("InsetBorderColor")
        C4 = GetColor("TextColor")
        C5 = GetColor("TopGloss")
        C6 = GetColor("BottomGloss")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(33, 33, 33))
        G.FillRectangle(New SolidBrush(C1), New Rectangle(1, 1, Width - 2, Height - 2))
        DrawBorders(New Pen(New SolidBrush(C3)), 1)
        DrawGradient(C5, C6, New Rectangle(1, 1, Width - 2, 13), 90S)
        DrawBorders(New Pen(New SolidBrush(C2)))
        DrawCorners(TransparencyKey)
        Select Case _ShowIcon
            Case True
                If Not _Icon Is Nothing Then
                    G.DrawIcon(_Icon, New Rectangle(8, 6, 12, 12))
                End If
                DrawText(New SolidBrush(C4), HorizontalAlignment.Left, 24, 0)
            Case False
                DrawText(New SolidBrush(C4), HorizontalAlignment.Left, 8, 0)
        End Select
    End Sub

    Private _ShowIcon As Boolean
    Public Property ShowIcon() As Boolean
        Get
            Return _ShowIcon
        End Get
        Set(ByVal v As Boolean)
            _ShowIcon = v
            Invalidate()
        End Set
    End Property

    Private _Icon As Icon
    Public Property Icon() As Icon
        Get
            Return _Icon
        End Get
        Set(ByVal v As Icon)
            _Icon = v
            Invalidate()
        End Set
    End Property


End Class
Class genesisButton
    Inherits ThemeControl151

    Sub New()
        BackColor = Color.FromArgb(33, 33, 33)

        SetColor("ButtonFillColor", Color.FromArgb(47, 47, 47))
        SetColor("ButtonClickColor", Color.FromArgb(44, 44, 44))
        SetColor("InsetBorderColor", Color.FromArgb(72, 72, 72))
        SetColor("TopGloss", Color.FromArgb(62, Color.White))
        SetColor("BottomGloss", Color.FromArgb(30, Color.White))
        SetColor("TopInset", Color.FromArgb(45, 45, 45))
        SetColor("BottomInset", Color.FromArgb(70, 70, 70))
        SetColor("BorderColor", Color.Black)
        SetColor("TextColor", Color.White)
    End Sub
    Dim C1, C2, C3, C4, C5, C6, C7, C8, C9 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("ButtonFillColor")
        C2 = GetColor("InsetBorderColor")
        C3 = GetColor("TopGloss")
        C4 = GetColor("BottomGloss")
        C5 = GetColor("TopInset")
        C6 = GetColor("BottomInset")
        C7 = GetColor("BorderColor")
        C8 = GetColor("TextColor")
        C9 = GetColor("ButtonClickColor")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Select Case State
            Case 0
                G.FillRectangle(New SolidBrush(C1), New Rectangle(1, 1, Width - 2, Height - 2))
                DrawBorders(New Pen(New SolidBrush(C2)), 2)
                DrawGradient(C3, C4, New Rectangle(1, 1, Width - 2, Height / 2 - 2), 90S)
            Case 1
                G.FillRectangle(New SolidBrush(C1), New Rectangle(1, 1, Width - 2, Height - 2))
                DrawBorders(New Pen(New SolidBrush(C2)), 2)
                DrawGradient(C3, C4, New Rectangle(1, 1, Width - 2, Height / 2 - 2), 90S)
            Case 2
                G.FillRectangle(New SolidBrush(C9), New Rectangle(1, 1, Width - 2, Height - 2))
                DrawBorders(New Pen(New SolidBrush(C2)), 2)
                DrawGradient(C4, C4, New Rectangle(1, 1, Width - 2, Height / 2 - 2), 90S)
        End Select


        DrawBorders(New Pen(New SolidBrush(C7)), 1)
        G.DrawLine(New Pen(New SolidBrush(C5)), 0, 0, Width, 0)
        G.DrawLine(New Pen(New SolidBrush(C5)), 0, 0, 0, Height)
        G.DrawLine(New Pen(New SolidBrush(C5)), Width - 1, 0, Width - 1, Height)
        G.DrawLine(New Pen(New SolidBrush(C6)), 0, Height - 1, Width, Height - 1)
        DrawCorners(BackColor)
        DrawCorners(BackColor, New Rectangle(1, 1, Width - 2, Height - 2))
        DrawText(New SolidBrush(C8), HorizontalAlignment.Center, 0, 0)
    End Sub
End Class
Class genesisTopButton
    Inherits ThemeControl151

    Sub New()
        BackColor = Color.FromArgb(33, 33, 33)
        Size = New Size(20, 20)
        LockHeight = 20
        LockWidth = 20

        SetColor("BackColor", Color.FromArgb(47, 47, 47))
        SetColor("BorderColor", Color.FromArgb(70, 70, 70))
        SetColor("InsetBorderColor2", Color.FromArgb(72, 72, 72))
        SetColor("InsetBorderColor", Color.Black)
        SetColor("TopGloss", Color.FromArgb(62, Color.White))
        SetColor("BottomGloss", Color.FromArgb(30, Color.White))
        SetColor("TopInset", Color.FromArgb(45, 45, 45))
        SetColor("ButtonClickColor", Color.FromArgb(44, 44, 44))
    End Sub
    Dim C1, C2, C3, C4, C5, C6, C7, C8 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("BackColor")
        C2 = GetColor("InsetBorderColor2")
        C3 = GetColor("TopGloss")
        C4 = GetColor("BottomGloss")
        C5 = GetColor("BorderColor")
        C6 = GetColor("InsetBorderColor")
        C7 = GetColor("TopInset")
        C8 = GetColor("ButtonClickColor")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Select Case State
            Case 0
                G.FillRectangle(New SolidBrush(C1), New Rectangle(1, 1, Width - 2, Height - 2))
                DrawBorders(New Pen(New SolidBrush(C2)), 2)
                DrawGradient(C3, C4, New Rectangle(1, 1, Width - 2, Height / 2 - 2), 90S)
            Case 1
                G.FillRectangle(New SolidBrush(C1), New Rectangle(1, 1, Width - 2, Height - 2))
                DrawBorders(New Pen(New SolidBrush(C2)), 2)
                DrawGradient(C3, C4, New Rectangle(1, 1, Width - 2, Height / 2 - 2), 90S)
            Case 2
                G.FillRectangle(New SolidBrush(C8), New Rectangle(1, 1, Width - 2, Height - 2))
                DrawBorders(New Pen(New SolidBrush(C2)), 2)
                DrawGradient(C4, C4, New Rectangle(1, 1, Width - 2, Height / 2 - 2), 90S)
        End Select


        DrawBorders(New Pen(New SolidBrush(C6)), 1)
        DrawBorders(New Pen(New SolidBrush(C5)))
        G.DrawLine(New Pen(New SolidBrush(C7)), 0, Height \ 2, 0, Height)
        G.DrawLine(New Pen(New SolidBrush(C7)), Width - 1, Height \ 2, Width - 1, Height)
        DrawCorners(BackColor, New Rectangle(1, 1, Width - 2, Height - 2))
    End Sub
End Class
Class genesisTextBox
    Inherits ThemeControl151
    Dim WithEvents txtbox As New TextBox

    Private _PassMask As Boolean
    Public Property UsePasswordMask() As Boolean
        Get
            Return _PassMask
        End Get
        Set(ByVal v As Boolean)
            _PassMask = v
            Invalidate()
        End Set
    End Property
    Private _maxchars As Integer
    Public Property MaxCharacters() As Integer
        Get
            Return _maxchars
        End Get
        Set(ByVal v As Integer)
            _maxchars = v
        End Set
    End Property

    Sub New()
        txtbox.TextAlign = HorizontalAlignment.Center
        txtbox.BorderStyle = BorderStyle.None
        txtbox.Location = New Point(5, 6)
        txtbox.Font = New Font("Verdana", 7.25)
        Controls.Add(txtbox)
        BackColor = Color.FromArgb(37, 37, 37)
        Text = ""
        txtbox.Text = ""
        Me.Size = New Size(135, 35)

        SetColor("BackColor", Color.FromArgb(37, 37, 37))
        SetColor("TextColor", Color.White)
        SetColor("TopInset", Color.FromArgb(45, 45, 45))
        SetColor("BottomInset", Color.FromArgb(70, 70, 70))
        SetColor("TextBoxShadow", Color.FromArgb(31, 31, 31))
        SetColor("BorderColor", Color.Black)
    End Sub

    Dim C1, C2, C3, C4, C5, C6 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("BackColor")
        C2 = GetColor("TextColor")
        C3 = GetColor("TopInset")
        C4 = GetColor("BottomInset")
        C5 = GetColor("TextBoxShadow")
        C6 = GetColor("BorderColor")
        txtbox.ForeColor = C2
        txtbox.BackColor = C1
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Select Case UsePasswordMask
            Case True
                txtbox.UseSystemPasswordChar = True
            Case False
                txtbox.UseSystemPasswordChar = False
        End Select
        Size = New Size(Me.Width, 25)
        txtbox.Font = Font
        txtbox.Size = New Size(Me.Width - 10, txtbox.Height - 10)
        txtbox.MaxLength = MaxCharacters
        G.FillRectangle(New SolidBrush(C1), New Rectangle(1, 1, Width - 2, Height - 2))
        DrawBorders(New Pen(New SolidBrush(C6)), 1)
        DrawBorders(New Pen(New SolidBrush(C4)))
        G.DrawLine(New Pen(New SolidBrush(C3)), 0, 0, Width, 0)
        G.DrawLine(New Pen(New SolidBrush(C3)), 0, 0, 0, Height)
        G.DrawLine(New Pen(New SolidBrush(C3)), Width - 1, 0, Width - 1, Height)
        G.DrawLine(New Pen(New SolidBrush(C5)), 2, 2, Width - 3, 2)
        DrawCorners(BackColor)
        DrawCorners(BackColor, New Rectangle(1, 1, Width - 2, Height - 2))
    End Sub
    Sub TextChngTxtBox() Handles txtbox.TextChanged
        Text = txtbox.Text
    End Sub
    Sub TextChng() Handles MyBase.TextChanged
        txtbox.Text = Text
    End Sub
End Class
<DefaultEvent("CheckedChanged")> _
Class genesisCheckBox
    Inherits ThemeControl151

    Sub New()
        BackColor = Color.FromArgb(37, 37, 37)
        LockWidth = 50
        LockHeight = 20
        Checked = False

        SetColor("OnGradientA", Color.FromArgb(63, 83, 100))
        SetColor("OnGradientB", Color.FromArgb(87, 127, 151))
        SetColor("OffGradientA", Color.FromArgb(23, 23, 23))
        SetColor("OffGradientB", Color.FromArgb(33, 33, 33))
        SetColor("SwitchColor", Color.FromArgb(25, 25, 25))
        SetColor("TopGloss", Color.FromArgb(62, Color.White))
        SetColor("BottomGloss", Color.FromArgb(30, Color.White))
        SetColor("SwitchBorder", Color.Black)
        SetColor("SwitchInsetBorder", Color.FromArgb(47, 47, 47))
        SetColor("BorderColor", Color.Black)
        SetColor("TopInset", Color.FromArgb(45, 45, 45))
        SetColor("BottomInset", Color.FromArgb(70, 70, 70))
    End Sub

    Dim C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("OnGradientA")
        C2 = GetColor("OnGradientB")
        C3 = GetColor("OffGradientA")
        C4 = GetColor("OffGradientB")
        C5 = GetColor("SwitchColor")
        C6 = GetColor("TopGloss")
        C7 = GetColor("BottomGloss")
        C8 = GetColor("SwitchBorder")
        C9 = GetColor("SwitchInsetBorder")
        C10 = GetColor("BorderColor")
        C11 = GetColor("TopInset")
        C12 = GetColor("BottomInset")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        Select Case Checked
            Case True
                DrawGradient(C1, C2, ClientRectangle, 90S)
                G.FillRectangle(New SolidBrush(C5), New Rectangle(Width - 19, 1, 17, Height - 4))
                G.DrawRectangle(New Pen(New SolidBrush(C8)), New Rectangle(Width - 20, 0, 20, Height - 1))
                G.DrawRectangle(New Pen(New SolidBrush(C9)), New Rectangle(Width - 19, 1, 16, Height - 4))
                DrawGradient(C6, C7, New Rectangle(Width - 19, 2, 17, Height / 2 - 2), 90S)
                G.DrawString("ON", Font, Brushes.White, 6, 3)
            Case False
                DrawGradient(C3, C4, ClientRectangle, 90S)
                G.FillRectangle(New SolidBrush(C5), New Rectangle(1, 1, 20, Height - 1))
                G.DrawRectangle(New Pen(New SolidBrush(C8)), New Rectangle(0, 0, 20, Height - 1))
                G.DrawRectangle(New Pen(New SolidBrush(C9)), New Rectangle(2, 2, 17, Height - 5))
                DrawGradient(C6, C7, New Rectangle(1, 1, 19, Height / 2 - 2), 90S)
                G.DrawString("OFF", Font, Brushes.White, 22, 3)
        End Select
        DrawCorners(BackColor, New Rectangle(1, 2, Width - 1, Height - 4))
        G.DrawLine(New Pen(New SolidBrush(C11)), 0, 0, Width, 0)
        G.DrawLine(New Pen(New SolidBrush(C11)), 0, 0, 0, Height)
        G.DrawLine(New Pen(New SolidBrush(C11)), Width - 1, 0, Width - 1, Height)
        G.DrawLine(New Pen(New SolidBrush(C12)), 0, Height - 1, Width, Height - 1)
        DrawBorders(New Pen(New SolidBrush(C10)), 1)
        DrawCorners(BackColor)
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

    Protected Overrides Sub OnClick(ByVal e As System.EventArgs)
        _Checked = Not _Checked
        RaiseEvent CheckedChanged(Me)
        MyBase.OnClick(e)
    End Sub

    Event CheckedChanged(ByVal sender As Object)
End Class
Class genesisTabControl
    Inherits TabControl
    Private LightBlack As Color = Color.FromArgb(37, 37, 37)
    Private LighterBlack As Color = Color.FromArgb(44, 44, 44)
    Private DrawGradientBrush, DrawGradientBrush2 As LinearGradientBrush
    Private _ControlBColor As Color
    Public Property TabTextColor() As Color
        Get
            Return _ControlBColor
        End Get
        Set(ByVal v As Color)
            _ControlBColor = v
            Invalidate()
        End Set
    End Property

    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer, True)
        TabTextColor = Color.White
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim r2 As New Rectangle(2, 0, Width - 1, 11)
        Dim r3 As New Rectangle(2, 0, Width - 1, 11)
        e.Graphics.Clear(Color.FromArgb(37, 37, 37))
        Dim ItemBounds As Rectangle
        Dim TextBrush As New SolidBrush(Color.Empty)
        Dim TabBrush As New SolidBrush(Color.DimGray)
        e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(33, 33, 33)), New Rectangle(1, 1, Width - 2, Height - 2))
        DrawGradientBrush2 = New LinearGradientBrush(r3, Color.FromArgb(62, Color.White), Color.FromArgb(30, Color.White), 90S)
        e.Graphics.FillRectangle(DrawGradientBrush2, r2)
        For TabItemIndex As Integer = 0 To Me.TabCount - 1
            ItemBounds = Me.GetTabRect(TabItemIndex)

            If CBool(TabItemIndex And 1) Then
                TabBrush.Color = Color.Transparent
            Else
                TabBrush.Color = Color.Transparent
            End If
            e.Graphics.FillRectangle(TabBrush, ItemBounds)
            Dim BorderPen As Pen
            If TabItemIndex = SelectedIndex Then
                BorderPen = New Pen(Color.Transparent, 1)
            Else
                BorderPen = New Pen(Color.Transparent, 1)
            End If
            e.Graphics.DrawRectangle(BorderPen, New Rectangle(ItemBounds.Location.X + 3, ItemBounds.Location.Y + 1, ItemBounds.Width - 8, ItemBounds.Height - 4))
            BorderPen.Dispose()
            Dim sf As New StringFormat
            sf.LineAlignment = StringAlignment.Center
            sf.Alignment = StringAlignment.Center

            If Me.SelectedIndex = TabItemIndex Then
                TextBrush.Color = TabTextColor
            Else
                TextBrush.Color = Color.DimGray
            End If
            e.Graphics.DrawString( _
            Me.TabPages(TabItemIndex).Text, _
            Me.Font, TextBrush, _
            RectangleF.op_Implicit(Me.GetTabRect(TabItemIndex)), sf)
            Try
                Me.TabPages(TabItemIndex).BackColor = Color.FromArgb(35, 35, 35)
            Catch
            End Try
        Next
        Try
            For Each tabpg As TabPage In Me.TabPages
                tabpg.BorderStyle = BorderStyle.None
            Next
        Catch
        End Try
        e.Graphics.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(255, Color.Black))), 2, 0, Width - 3, Height - 3)
        e.Graphics.DrawRectangle(New Pen(New SolidBrush(LighterBlack)), New Rectangle(3, 24, Width - 5, Height - 28))
        e.Graphics.DrawLine(New Pen(New SolidBrush(Color.FromArgb(255, Color.Black))), 2, 23, Width - 2, 23)
        e.Graphics.DrawLine(New Pen(New SolidBrush(Color.FromArgb(35, 35, 35))), 0, 0, 1, 1)
        e.Graphics.DrawLine(New Pen(New SolidBrush(Color.FromArgb(70, 70, 70))), 2, Height - 2, Width + 1, Height - 2)

    End Sub
End Class
Class genesisComboBox
    Inherits ComboBox
    Sub New()
        MyBase.New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
        ControlStyles.ResizeRedraw Or _
        ControlStyles.UserPaint Or _
        ControlStyles.DoubleBuffer, True)
        DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
        BackColor = Color.FromArgb(45, 45, 45)
        ForeColor = Color.White
        DropDownStyle = ComboBoxStyle.DropDownList
    End Sub
    Private _StartIndex As Integer = 0
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
    Private LightBlack As Color = Color.FromArgb(37, 37, 37)
    Private LighterBlack As Color = Color.FromArgb(60, 60, 60)
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim G As Graphics = e.Graphics
        Dim T As LinearGradientBrush = New LinearGradientBrush(New Rectangle(0, 0, Width, 10), Color.FromArgb(62, Color.White), Color.FromArgb(30, Color.White), 90S)
        Dim DrawCornersBrush As SolidBrush
        DrawCornersBrush = New SolidBrush(Color.FromArgb(37, 37, 37))
        Try
            With G
                G.SmoothingMode = SmoothingMode.HighQuality
                .Clear(Color.FromArgb(37, 37, 37))
                .FillRectangle(T, New Rectangle(0, 0, Width, 9))
                .DrawString(Items(SelectedIndex).ToString, Font, Brushes.White, New Point(3, 3))
                .DrawLine(New Pen(New SolidBrush(Color.FromArgb(37, 37, 37))), 0, 0, 0, 0)
                .DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(0, 0, 0))), New Rectangle(1, 1, Width - 3, Height - 3))

                .DrawLine(New Pen(New SolidBrush(Color.FromArgb(45, 45, 45))), 0, 0, Width, 0)
                .DrawLine(New Pen(New SolidBrush(Color.FromArgb(45, 45, 45))), 0, 0, 0, Height)
                .DrawLine(New Pen(New SolidBrush(Color.FromArgb(45, 45, 45))), Width - 1, 0, Width - 1, Height)
                .DrawLine(New Pen(New SolidBrush(Color.FromArgb(70, 70, 70))), 0, Height - 1, Width, Height - 1)

                DrawTriangle(Color.White, New Point(Width - 15, 8), New Point(Width - 7, 8), New Point(Width - 11, 13), G)
            End With
        Catch
        End Try
    End Sub
    Sub ReplaceItem(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DrawItemEventArgs) Handles Me.DrawItem
        e.DrawBackground()
        Try
            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(37, 37, 37)), e.Bounds)
            End If
            Using b As New SolidBrush(e.ForeColor)
                e.Graphics.DrawString(MyBase.GetItemText(MyBase.Items(e.Index)), e.Font, b, e.Bounds)
            End Using
        Catch
        End Try
        e.DrawFocusRectangle()
    End Sub
    Protected Sub DrawTriangle(ByVal Clr As Color, ByVal FirstPoint As Point, ByVal SecondPoint As Point, ByVal ThirdPoint As Point, ByVal G As Graphics)
        Dim points As New List(Of Point)()
        points.Add(FirstPoint)
        points.Add(SecondPoint)
        points.Add(ThirdPoint)
        G.FillPolygon(New SolidBrush(Clr), points.ToArray)
    End Sub
End Class
Class genesisGroupBox
    Inherits ThemeContainer151

    Sub New()
        ControlMode = True
        Dock = DockStyle.None
        Size = New Size(180, 200)
        BackColor = Color.FromArgb(37, 37, 37)

        SetColor("BackColor", Color.FromArgb(37, 37, 37))
        SetColor("HeaderColor", Color.FromArgb(32, 32, 32))
        SetColor("TextColor", Color.White)
        SetColor("BorderColor", Color.Black)
        SetColor("TopInset", Color.FromArgb(45, 45, 45))
        SetColor("BottomInset", Color.FromArgb(70, 70, 70))
    End Sub

    Dim C1, C2, C3, C4, C5, C6 As Color
    Protected Overrides Sub ColorHook()
        C1 = GetColor("BackColor")
        C2 = GetColor("HeaderColor")
        C3 = GetColor("TextColor")
        C4 = GetColor("BorderColor")
        C5 = GetColor("TopInset")
        C6 = GetColor("BottomInset")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(Color.FromArgb(37, 37, 37))
        G.FillRectangle(New SolidBrush(C1), New Rectangle(1, 1, Width - 2, Height - 2))
        G.FillRectangle(New SolidBrush(C2), New Rectangle(1, 1, Width - 2, 20))
        G.DrawLine(Pens.Black, 0, 21, Width, 21)
        G.DrawLine(New Pen(New SolidBrush(C5)), 0, 22, Width, 22)
        DrawBorders(New Pen(New SolidBrush(C4)), 1)
        DrawBorders(New Pen(New SolidBrush(C6)))
        G.DrawLine(New Pen(New SolidBrush(C5)), 0, 0, Width, 0)
        G.DrawLine(New Pen(New SolidBrush(C5)), 0, 0, 0, Height)
        G.DrawLine(New Pen(New SolidBrush(C5)), Width - 1, 0, Width - 1, Height)
        DrawCorners(BackColor)
        DrawCorners(BackColor, New Rectangle(1, 1, Width - 2, Height - 2))
        DrawText(New SolidBrush(C3), HorizontalAlignment.Center, 0, 0)
    End Sub
End Class