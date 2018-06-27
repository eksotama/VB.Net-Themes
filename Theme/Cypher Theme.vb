Imports System.Drawing.Drawing2D

'
'
'Created By Marvinn (HF)
'Need Any help feel free to mail/pm me (marvinzwolsman@hotmail.com)
'Please give propper credits!
'11/5/2011 22:30 (Dutch/Netherlands)
'
'
Public Class CypherxButton
    Inherits Control

    Sub New()
        Font = New Font("Arial", 8)
        ForeColor = Color.White
    End Sub

    Private Enum State
        MouseDown
        MouseEnter
        MouseLeft
    End Enum

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        MouseState = State.MouseLeft
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        MouseState = State.MouseEnter
        Invalidate()
    End Sub

    Protected Overrides Sub OnMouseClick(ByVal e As System.Windows.Forms.MouseEventArgs)
        MouseState = State.MouseLeft
        Invalidate()
        MyBase.OnMouseClick(e)
    End Sub
    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        Invalidate()
    End Sub

    Dim MouseState As State = State.MouseLeft
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                Dim OuterR As New Rectangle(0, 0, Width - 1, Height - 1)
                Dim InnerR As New Rectangle(1, 1, Width - 3, Height - 3)
                Dim UpHalf As New Rectangle(2, 2, Width - 3, (Height - 1) / 2)
                Dim DownHalf As New Rectangle(2, (Height - 1) / 2, Width - 3, (Height - 1) / 2)

                Select Case MouseState
                    Case State.MouseLeft
                        Draw.Gradient(g, Color.FromArgb(88, 79, 72), Color.FromArgb(76, 69, 61), UpHalf)
                        Draw.Gradient(g, Color.FromArgb(56, 46, 36), Color.FromArgb(66, 56, 46), DownHalf)
                        g.DrawRectangle(Pens.Black, OuterR)
                        g.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(75, 66, 60))), InnerR)
                        ForeColor = Color.White
                    Case State.MouseEnter
                        Draw.Gradient(g, Color.FromArgb(234, 236, 241), Color.FromArgb(215, 219, 225), UpHalf)
                        Draw.Gradient(g, Color.FromArgb(189, 193, 198), Color.FromArgb(195, 198, 201), DownHalf)
                        ForeColor = Color.FromArgb(23, 32, 37)
                End Select
                Dim S As SizeF = g.MeasureString(Text, Font)
                g.DrawString(Text, Font, New SolidBrush(ForeColor), CInt(Width / 2 - S.Width / 2), CInt(Height / 2 - S.Height / 2))

                e.Graphics.DrawImage(b.Clone, 0, 0)
            End Using
        End Using
        MyBase.OnPaint(e)
    End Sub

    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
End Class

Public Class CypherxTheme
    Inherits Control
    Dim Bgimagee As Bitmap
    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        Dock = DockStyle.Fill
        Bgimagee = CreateBg()
        If TypeOf Parent Is Form Then
            With DirectCast(Parent, Form)
                .FormBorderStyle = 0
                .BackColor = Color.FromArgb(25, 18, 12)
                .ForeColor = Color.White
                .Font = New Font("Arial", 8)
                _Icon = .Icon
                .Text = Text
                DoubleBuffered = True
                .BackgroundImage = Bgimagee
                BackgroundImage = Bgimagee
            End With
        End If
        MyBase.OnHandleCreated(e)
    End Sub

    Dim Balk As New Rectangle(4, 4, Width - 8, 27)


    Function CreateBg() As Bitmap
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                Dim P1 As Color = Color.FromArgb(29, 25, 22)
                Dim P2 As Color = Color.FromArgb(35, 31, 28)

                For y As Integer = 0 To Height Step 4
                    For x As Integer = 3 To Width Step 4
                        g.FillRectangle(New SolidBrush(P1), New Rectangle(x, y, 1, 1))
                        g.FillRectangle(New SolidBrush(P2), New Rectangle(x, y + 1, 1, 1))
                        Try
                            g.FillRectangle(New SolidBrush(P1), New Rectangle(x + 2, y + 2, 1, 1))
                            g.FillRectangle(New SolidBrush(P2), New Rectangle(x + 2, y + 3, 1, 1))
                        Catch
                        End Try
                    Next
                Next
                Return b.Clone
            End Using
        End Using
    End Function


    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                g.FillRectangle(New SolidBrush(Color.FromArgb(25, 18, 12)), New Rectangle(0, 0, Width, Height))

                Dim P1 As Color = Color.FromArgb(29, 25, 22)
                Dim P2 As Color = Color.FromArgb(35, 31, 28)
                If Not Bgimagee.Equals(Nothing) Then
                    g.DrawImage(Bgimagee, 0, 0)
                End If

                g.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(15, 10, 5))), New Rectangle(0, 0, Width - 2, Height - 1))
                g.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(55, 45, 35))), New Rectangle(1, 1, Width - 3, Height - 3))
                g.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(75, 70, 65)), 2), New Rectangle(3, 3, Width - 6, Height - 6))



                Dim BovenHelftBalk As New Rectangle(4, 4, Width - 8, CInt(27 / 2))
                Dim OnderHelftBalk As New Rectangle(4, CInt(27 / 2) + 2, Width - 8, CInt(27 / 2))

                g.FillRectangle(New SolidBrush(Color.FromArgb(200, Color.FromArgb(75, 70, 65))), BovenHelftBalk)
                g.FillRectangle(New SolidBrush(Color.FromArgb(230, P2)), OnderHelftBalk)

                g.DrawImage(ResizeIcon, New Rectangle(10, 10, 16, 16))

                Dim S = g.MeasureString(Text, Font)
                g.DrawString(Text, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(ForeColor), 36, 10)

                Dim MinimizeRec As New Rectangle(Width - 32, 16, 9, 5)

                If Minibox Then
                    Select Case EnteredMinimize
                        Case True
                            g.FillRectangle(Brushes.White, MinimizeRec)
                            g.DrawRectangle(New Pen(Color.FromArgb(255, Color.Black), 1), MinimizeRec)
                        Case False
                            g.FillRectangle(New SolidBrush(Color.FromArgb(100, Color.White)), MinimizeRec)
                            g.DrawRectangle(New Pen(Color.FromArgb(150, Color.Black), 1), MinimizeRec)
                    End Select
                End If

                Select Case EntredClose
                    Case True
                        g.DrawString("x", New Font("Arial", 13, FontStyle.Bold), Brushes.White, Width - 20, 5)
                    Case False
                        g.DrawString("x", New Font("Arial", 13, FontStyle.Bold), New SolidBrush(Color.FromArgb(100, Color.White)), Width - 20, 5)
                End Select


                e.Graphics.DrawImage(b, 0, 0)
            End Using
        End Using
        MyBase.OnPaint(e)
    End Sub

    Dim EnteredMinimize As Boolean = False
    Dim EntredClose As Boolean = False

    Function ResizeIcon() As Bitmap
        Dim TempIcon As Icon = Icon
        Dim TempBitmap As Bitmap = New Bitmap(32, 32)
        Dim BitmapGraphic As Graphics = Graphics.FromImage(TempBitmap)
        Dim XPos, YPos As Integer
        XPos = (TempBitmap.Width - TempIcon.Width) \ 2
        YPos = (TempBitmap.Height - TempIcon.Height) \ 2

        BitmapGraphic.DrawIcon(TempIcon, New Rectangle(XPos, YPos, TempIcon.Width, TempIcon.Height))
        Return TempBitmap
    End Function

    Dim _Icon As Icon
    Public Property Icon As Icon
        Get
            Return _Icon
        End Get
        Set(ByVal value As Icon)
            _Icon = value
            If TypeOf Parent Is Form Then
                With DirectCast(Parent, Form)
                    .Icon = value
                    _Icon = value
                    Invalidate()
                End With
            End If
        End Set
    End Property

    Dim FadingOut As Boolean = True
    ' <summary>
    ' This boolean indicates the use of the Fade Out Effect on close
    ' </summary>
    Public Property UseFadeOut As Boolean
        Get
            Return FadingOut
        End Get
        Set(ByVal value As Boolean)
            FadingOut = value
        End Set
    End Property

    Dim Minibox As Boolean = True
    Public Property MinimizeBox As Boolean
        Get
            Return Minibox
        End Get
        Set(ByVal value As Boolean)
            Minibox = value
            Invalidate()
        End Set
    End Property

#Region " Global Variables "
    Dim Point As New Point()
    Dim X, Y As Integer
#End Region

#Region " GUI "

    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim Last As Boolean = EnteredMinimize

        Dim MinimizeRec As New Rectangle(Width - 32, 16, 9, 5)
        If MinimizeRec.Contains(e.Location) Then
            EnteredMinimize = True
        Else
            EnteredMinimize = False
        End If

        If Not Last = EnteredMinimize Then
            Invalidate()
        End If

        Last = EntredClose

        Dim CloseRec As New Rectangle(Width - 20, 5, 16, 16)
        If CloseRec.Contains(e.Location) Then
            EntredClose = True
        Else
            EntredClose = False
        End If

        If Not Last = EntredClose Then
            Invalidate()
        End If

        If TypeOf Parent Is Form Then
            With DirectCast(Parent, Form)
                If e.Button = MouseButtons.Left And e.Location.X < Width And e.Location.Y < Balk.Height Then
                    Point = Control.MousePosition
                    Point.X = Point.X - (X)
                    Point.Y = Point.Y - (Y)
                    .Location = Point
                End If
            End With
        End If

        MyBase.OnMouseMove(e)
    End Sub


    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        If TypeOf Parent Is Form Then
            With DirectCast(Parent, Form)
                X = Control.MousePosition.X - .Location.X
                Y = Control.MousePosition.Y - .Location.Y
            End With
        End If

        Dim MinimizeRec As New Rectangle(Width - 32, 16, 9, 5)
        If MinimizeRec.Contains(e.Location) Then
            If TypeOf Parent Is Form Then
                With DirectCast(Parent, Form)
                    .WindowState = FormWindowState.Minimized
                End With
            End If
        End If

        Dim CloseRec As New Rectangle(Width - 20, 5, 16, 16)
        If CloseRec.Contains(e.Location) Then
            If TypeOf Parent Is Form Then
                With DirectCast(Parent, Form)
                    If FadingOut Then FadeOut()
                    .Close()
                   
                End With
            End If

        End If

        MyBase.OnMouseDown(e)
    End Sub
#End Region

    Function FadeOut()
        If TypeOf Parent Is Form Then
            With DirectCast(Parent, Form)
                For i As Double = 1 To 0.0 Step -0.1
                    .Opacity = i
                    Threading.Thread.Sleep(50)
                Next
            End With
        End If
        Return True
    End Function

End Class

Public Class CypherxLabel
    Inherits Label
    Sub New()
        Font = New Font("Arial", 8)
        ForeColor = Color.White
        BackColor = Color.Transparent
    End Sub
End Class

Public Class CyperxProgressbar
    Inherits Control
    Sub New()
        Font = New Font("Arial", 8)
        ForeColor = Color.White
    End Sub

    Dim _UseColor As Boolean = False
    Public Property Colorize As Boolean
        Get
            Return _UseColor
        End Get
        Set(ByVal value As Boolean)
            _UseColor = value
            Invalidate()
        End Set
    End Property
    Dim Perc As Double = 0
    Public ReadOnly Property Percentage As Double
        Get
            Return Perc
        End Get
    End Property
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                Dim WholeR As New Rectangle(0, 0, Width - 1, Height - 1)
                Draw.Gradient(g, _Lightcolor, _DarkColor, WholeR)
                g.DrawRectangle(Pens.Black, WholeR)
                Dim OneProcent As Double = Maximum / 100
                Dim ProgressProcent As Integer = _Progress / OneProcent
                Console.WriteLine(ProgressProcent)

                Dim ProgressRec As New Rectangle(2, 2, CInt((Width - 4) * (ProgressProcent * 0.01)), Height - 4)
                Perc = _Progress / (Maximum / 100)
                Select Case _UseColor
                    Case False
                        g.FillRectangle(New SolidBrush(Color.FromArgb(100, Color.Black)), ProgressRec)
                    Case True
                        Dim Drawcolor As Color = Color.FromArgb(150, 255 - 2 * ProgressProcent, (1.7 * ProgressProcent), 0)
                        g.FillRectangle(New SolidBrush(Color.FromArgb(50, Drawcolor)), ProgressRec)
                End Select

                If Showt Then g.DrawString(Text, Font, New SolidBrush(ForeColor), New Point(3, 4))
                e.Graphics.DrawImage(b, 0, 0)
            End Using
        End Using
    End Sub


    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub

#Region " Properties "

    Dim Showt As Boolean = True
    Public Property ShowText As Boolean
        Get
            Return Showt
        End Get
        Set(ByVal value As Boolean)
            Showt = value
            Invalidate()
        End Set
    End Property
    Private _Maximum As Double = 100
    Public Property Maximum() As Double
        Get
            Return _Maximum
        End Get
        Set(ByVal value As Double)
            _Maximum = value
            value = _Current / value * 100
            Invalidate()
        End Set
    End Property

    Private _Current As Double
    Public Property Current() As Double
        Get
            Return _Current
        End Get
        Set(ByVal value As Double)
            _Current = value
            value = value / _Maximum * 100
            Invalidate()
        End Set
    End Property

    Private _Progress As Integer
    Public Property Value() As Double
        Get
            Return _Progress
        End Get
        Set(ByVal value As Double)
            If value < 0 Then value = 0 Else If value > Maximum Then value = Maximum
            _Progress = CInt(value)
            _Current = value * 0.01 * _Maximum
            Invalidate()
        End Set
    End Property

    Public Property DarkColor As Color

        Get
            Return _DarkColor

        End Get
        Set(ByVal value As Color)
            _DarkColor = value
            Invalidate()
        End Set
    End Property

    Public Property Lightcolor() As Color
        Get
            Return _Lightcolor
        End Get
        Set(ByVal value As Color)
            _Lightcolor = value
            Invalidate()
        End Set
    End Property

#End Region

#Region " Colors "
    Dim _Lightcolor As Color = Color.FromArgb(65, 55, 45)
    Dim _DarkColor = Color.FromArgb(75, 70, 65)
#End Region


End Class

Public Class CyperxTextbox

    Inherits Control

    Dim Stroke As Color = Color.FromArgb(80, 71, 62)
    Dim Bg As Color = Color.FromArgb(67, 60, 53)
    Dim tbox As TextBox
    Sub New()
        Me.Text = ""
        tbox = Nothing
        tbox = New TextBox
        tbox.Text = Text
        tbox.BorderStyle = BorderStyle.None
        tbox.BackColor = Color.FromArgb(25, 18, 12)
        tbox.Location = New Point(3, 4)
        tbox.Width = Width - 7
        tbox.Font = Font
        tbox.UseSystemPasswordChar = Pwbox
        tbox.ForeColor = Color.White
        Me.Controls.Add(tbox)
        AddHandler tbox.TextChanged, Sub() TextChange()
    End Sub
    Dim Pwbox As Boolean = False

    Dim DrawRounded As Boolean = True
    Public Property Rounded As Boolean
        Get
            Return DrawRounded
        End Get
        Set(ByVal value As Boolean)
            DrawRounded = value
        End Set
    End Property

    Public Property UseSystemPasswordChar As Boolean
        Get
            Return Pwbox
        End Get
        Set(ByVal value As Boolean)
            Pwbox = value
            tbox = Nothing
            tbox = New TextBox
            tbox.Text = Text
            tbox.BorderStyle = BorderStyle.None
            tbox.BackColor = Color.FromArgb(25, 18, 12)
            tbox.Location = New Point(3, 4)
            tbox.Width = Width - 7
            tbox.Font = Font
            tbox.UseSystemPasswordChar = Pwbox
            tbox.ForeColor = Color.White
            Me.Controls.Add(tbox)
            AddHandler tbox.TextChanged, Sub() TextChange()
        End Set
    End Property

    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        tbox = New TextBox
        tbox.Text = Text
        tbox.BorderStyle = BorderStyle.None
        tbox.BackColor = Color.FromArgb(25, 18, 12)
        tbox.Location = New Point(3, 4)
        tbox.Width = Width - 7
        tbox.Font = Font
        tbox.UseSystemPasswordChar = Pwbox
        tbox.ForeColor = Color.White
        Me.Controls.Add(tbox)
        AddHandler tbox.TextChanged, Sub() TextChange()
        MyBase.OnHandleCreated(e)
    End Sub

    Private Sub TextChange()
        Me.Text = tbox.Text
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                g.DrawRectangle(New Pen(Color.FromArgb(25, 18, 12)), New Rectangle(0, 0, Width, Height))
                If DrawRounded Then
                    Dim Outline As GraphicsPath = Draw.RoundedRectangle(0, 0, Width - 1, Height - 1, 10, 1)
                    g.DrawPath(New Pen(Stroke), Outline)
                Else
                    Dim rec As New Rectangle(0, 0, Width - 1, Height - 1)
                    g.FillRectangle(New SolidBrush(Color.FromArgb(25, 18, 12)), rec)
                    g.DrawRectangle(New Pen(Stroke), rec)
                End If
                e.Graphics.DrawImage(b, 0, 0)
            End Using
        End Using
        MyBase.OnPaint(e)
    End Sub
End Class

Public Class CypherxGroupBox
    Inherits Panel
    Dim Stroke As Color = Color.FromArgb(80, 71, 62)
    Sub New()
        BackColor = Color.Transparent
    End Sub
    Dim _t As String = ""
    Public Property Header As String
        Get
            Return _t
        End Get
        Set(ByVal value As String)
            _t = value
            Invalidate()
        End Set
    End Property
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                Dim M As SizeF = g.MeasureString(_t, font)
                Dim Outline As GraphicsPath = Draw.RoundedRectangle(0, M.Height / 2, Width - 1, Height - 1, 10, 1)
                g.Clear(BackColor)
                g.FillRectangle(New SolidBrush(Color.FromArgb(25, 18, 12)), New Rectangle(0, M.Height / 2, Width - 1, Height - 1))
                g.DrawPath(New Pen(Stroke), Outline)

                g.FillRectangle(New SolidBrush(Color.FromArgb(25, 18, 12)), New Rectangle(10, (M.Height / 2) - 2, M.Width + 10, M.Height))
                g.DrawString(_t, Font, New SolidBrush(Stroke), 12, 2)
                e.Graphics.DrawImage(b, 0, 0)
            End Using
        End Using
        MyBase.OnPaint(e)
    End Sub
End Class

Public Class CyperxSeperator
    Inherits Control
    Sub New()
        If TypeOf Parent Is CypherxTheme Then
            With DirectCast(Parent, CypherxTheme)
                BackgroundImage = .BackgroundImage
            End With
        End If
    End Sub
    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                g.Clear(BackColor)

                Dim P1 As Color = Color.FromArgb(29, 25, 22)
                Dim P2 As Color = Color.FromArgb(80, 71, 62)
                g.FillRectangle(New SolidBrush(Color.FromArgb(25, 18, 12)), New Rectangle(0, 0, Width, Height))
                If D Then Draw.BackGround(Width, Height, g)

                Dim GRec As New Rectangle(0, Height / 2, Width / 5, 2)
                Using GBrush As LinearGradientBrush = New LinearGradientBrush(grec, Color.Transparent, P2, LinearGradientMode.Horizontal)
                    g.FillRectangle(GBrush, GRec)
                End Using
                g.DrawLine(New Pen(P2, 2), New Point(GRec.Width, GRec.Y + 1), New Point(Width - GRec.Width + 1, GRec.Y + 1))

                GRec = New Rectangle(Width - (Width / 5), Height / 2, Width / 5, 2)
                Using GBrush As LinearGradientBrush = New LinearGradientBrush(GRec, P2, Color.Transparent, LinearGradientMode.Horizontal)
                    g.FillRectangle(GBrush, GRec)
                End Using
                e.Graphics.DrawImage(b, 0, 0)
            End Using
        End Using
        MyBase.OnPaint(e)
    End Sub

    Dim D As Boolean = True
    Public Property DrawPatern As Boolean
        Get
            Return D
        End Get
        Set(ByVal value As Boolean)
            D = value
            Invalidate()
        End Set
    End Property
End Class

Public Class CyperxComboBox
    Inherits Control

    Sub New()
        Font = New Font("Arial", 8)
        ForeColor = Color.White
        MinimumSize = New Size(130, 23)

    End Sub

    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        If _Items.Length < 0 Then _Items = New String() {Text}
        MyBase.OnHandleCreated(e)
    End Sub
    Private Enum State
        MouseDown = 0
        MouseEnter = 1
        MouseLeft = 2
        Wait = 3
    End Enum

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        If Not MouseState = State.Wait Then
            MouseState = State.MouseLeft
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)
        If Not MouseState = State.Wait Then
            MouseState = State.MouseEnter
            Invalidate()
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal e As System.Windows.Forms.MouseEventArgs)
        If Not MouseState = State.Wait Then
            MouseState = State.MouseEnter
            Invalidate()
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)


        MouseState = State.Wait
        Dim ShowPopup As New Threading.Thread(AddressOf ShowAndWait)
        ShowPopup.Start()

        Invalidate()

        MyBase.OnMouseDown(e)
    End Sub

    Sub ShowAndWait()
        Dim pop As New Popup(_Items)
        pop.Location = New Point(Location.X, Location.Y + Height + 2)

        Invoke(New AddX(AddressOf AddControl), pop)


        pop.WaitForInput()

        MouseState = State.MouseLeft
        If Not pop.SelectedItem = "" Then
            Invoke(New UpdateTextD(AddressOf UpdateText), pop.SelectedItem)
        Else
            Invoke(New UpdateTextD(AddressOf UpdateText), Text)
        End If


    End Sub

    Delegate Sub UpdateTextD(ByVal text As String)
    Sub UpdateText(ByVal text As String)
        Me.Text = text
        Invalidate()
    End Sub

    Delegate Sub AddX(ByVal control As Control)
    Sub AddControl(ByVal control As Control)
        Parent.Controls.Add(control)
    End Sub
    Dim _Items() As String
    Public Property Items As String()
        Get
            Return _Items
        End Get
        Set(ByVal value As String())
            _Items = value
            Text = value(0)
            Invalidate()
        End Set
    End Property
    Dim MouseState As State = State.MouseLeft
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)
                Dim OuterR As New Rectangle(0, 0, Width - 1, Height - 1)
                Dim InnerR As New Rectangle(1, 1, Width - 3, Height - 3)
                Dim UpHalf As New Rectangle(2, 2, Width - 3, (Height - 1) / 2)
                Dim DownHalf As New Rectangle(2, (Height - 1) / 2, Width - 3, (Height - 1) / 2)

                Select Case MouseState
                    Case State.MouseLeft
                        Draw.Gradient(g, Color.FromArgb(88, 79, 72), Color.FromArgb(76, 69, 61), UpHalf)
                        Draw.Gradient(g, Color.FromArgb(56, 46, 36), Color.FromArgb(66, 56, 46), DownHalf)
                        g.DrawRectangle(Pens.Black, OuterR)
                        g.DrawRectangle(New Pen(New SolidBrush(Color.FromArgb(75, 66, 60))), InnerR)
                        ForeColor = Color.White
                    Case State.MouseEnter
                        ForeColor = Color.FromArgb(23, 32, 37)
                        Draw.Gradient(g, Color.FromArgb(234, 236, 241), Color.FromArgb(215, 219, 225), UpHalf)
                        Draw.Gradient(g, Color.FromArgb(189, 193, 198), Color.FromArgb(195, 198, 201), DownHalf)
                    Case State.Wait
                        ForeColor = Color.FromArgb(23, 32, 37)
                        Draw.Gradient(g, Color.FromArgb(234, 236, 241), Color.FromArgb(215, 219, 225), UpHalf)
                        Draw.Gradient(g, Color.FromArgb(189, 193, 198), Color.FromArgb(195, 198, 201), DownHalf)
                End Select


                Dim UpRec As New Rectangle(Width - 18, 2, 1, 5)
                Draw.Gradient(g, Color.Transparent, Color.FromArgb(25, 18, 12), UpRec)
                g.DrawLine(New Pen(Color.FromArgb(28, 18, 12), 1), New Point(Width - 18, 7), New Point(Width - 18, Height - 7))
                Draw.Gradient(g, Color.FromArgb(28, 18, 12), Color.Transparent, New Rectangle(Width - 18, Height - 7, 1, 5))

                g.DrawLine(New Pen(Brushes.White, 2), New Point(Width - 15, 9), New Point(Width - 10, 14))
                g.DrawLine(New Pen(Brushes.White, 2), New Point(Width - 5, 9), New Point(Width - 10, 14))

                Dim S As SizeF = g.MeasureString(Text, Font)
                g.DrawString(Text, Font, New SolidBrush(ForeColor), 5, CInt(Height / 2 - S.Height / 2))

                e.Graphics.DrawImage(b.Clone, 0, 0)
            End Using
        End Using
        MyBase.OnPaint(e)
    End Sub

    Protected Overrides Sub OnPaintBackground(ByVal pevent As System.Windows.Forms.PaintEventArgs)
    End Sub
End Class

Public Class Popup
    Inherits Control
    Dim _items() As String
    Dim ListOfRec As New List(Of Rectangle)

    Sub New(ByVal items As String())
        DoubleBuffered = True
        _items = items
        FixWidth()
        FixList()

    End Sub
    Protected Overrides Sub OnHandleCreated(ByVal e As System.EventArgs)
        Console.WriteLine(TopLevelControl.TopLevelControl)
        BringToFront()
        MyBase.OnHandleCreated(e)
    End Sub
    Dim MyMousedown As Boolean = False
    Protected Overrides Sub OnMouseDown(ByVal e As System.Windows.Forms.MouseEventArgs)
        MyMousedown = True
        Invalidate()
        _item = Temp_item
        Console.WriteLine("Item: " & SelectedItem)
        Input = True
        Me.Hide()
        MyBase.OnMouseDown(e)
    End Sub

    Sub FixWidth()
        Dim G As Graphics = Graphics.FromImage(New Bitmap(1, 1))
        Dim LongestWidth As Integer = 0
        For Each Str As String In _items
            If G.MeasureString(Str, Font).Width > LongestWidth Then
                LongestWidth = G.MeasureString(Str, Font).Width
            End If
        Next

        If LongestWidth < 85 Then
            Width = 95
        Else
            Me.Width = LongestWidth + 9
        End If

    End Sub
    Sub FixList()
        Dim MyHeight = 23 * _items.Length - 1
        Dim AantalRecs As Integer = MyHeight / 23
        ListOfRec.Add(New Rectangle(2, 3, Width - 5, 23))
        For i As Integer = 1 To AantalRecs
            Dim rec As New Rectangle(2, 23 * i, Width - 5, 23)
            ListOfRec.Add(rec)
        Next

        Me.Height = MyHeight + 5
        Invalidate()
    End Sub
    Dim SelectedReg As New Rectangle(0, 0, 0, 0)
    Protected Overrides Sub OnMouseMove(ByVal e As System.Windows.Forms.MouseEventArgs)
        Dim Oldrec As Rectangle = SelectedReg
        For Each rec As Rectangle In ListOfRec
            If rec.Contains(e.Location) Then
                SelectedReg = rec
            End If
        Next
        If Not Oldrec = SelectedReg Then
            Invalidate()
        End If
        MyBase.OnMouseMove(e)
    End Sub

    Dim Input As Boolean = False
    Public Sub WaitForInput()
        Do While Input = False
            Threading.Thread.Sleep(1)
        Loop
    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)
        SelectedReg = New Rectangle(0, 0, 0, 0)
        Me.Visible = False
        Input = True
        MyBase.OnMouseLeave(e)
    End Sub

    Dim _item As String = ""
    Public Property SelectedItem As String
        Get
            Return _item
        End Get
        Set(ByVal value As String)
            _item = value
        End Set
    End Property

    Dim Temp_item As String = ""
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Using b As New Bitmap(Width, Height)
            Using g As Graphics = Graphics.FromImage(b)

                Dim Fullrec As New Rectangle(0, 0, Width - 1, Height - 1)
                g.FillRectangle(New SolidBrush(Color.FromArgb(20, 13, 6)), Fullrec)
                g.DrawRectangle(New Pen(Color.FromArgb(16, 11, 5)), Fullrec)
                g.FillRectangle(New SolidBrush(Color.FromArgb(177, 177, 179)), SelectedReg)
                g.DrawRectangle(New Pen(Color.FromArgb(51, 51, 50)), SelectedReg)

                If SelectedReg.Contains(New Point(5, 5)) Then
                    g.DrawString(_items(0), Font, New SolidBrush(Color.FromArgb(20, 13, 6)), 5, 6)
                    Console.WriteLine("Selected item " & _items(0))
                    Temp_item = _items(0)
                Else
                    g.DrawString(_items(0), Font, Brushes.White, 5, 6)
                End If


                For I As Integer = 1 To _items.Length - 1
                    If SelectedReg.Contains(New Point(5, I * 23 + 5)) Then
                        g.DrawString(_items(I), Font, New SolidBrush(Color.FromArgb(20, 13, 6)), 5, I * 23 + 5)
                        Console.WriteLine("Selected item: " & _items(I))
                        Temp_item = _items(I)
                    Else
                        g.DrawString(_items(I), Font, Brushes.White, 5, I * 23 + 5)
                    End If
                Next
                e.Graphics.DrawImage(b, 0, 0)
            End Using
        End Using
        MyBase.OnPaint(e)
    End Sub

End Class


'
'
'Created By Aeonhack
'
'
Public Class Draw
    Shared Sub Gradient(ByVal g As Graphics, ByVal c1 As Color, ByVal c2 As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        Dim R As New Rectangle(x, y, width, height)
        Using T As New LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical)
            g.FillRectangle(T, R)
        End Using
    End Sub
    Shared Sub Gradient(ByVal g As Graphics, ByVal c1 As Color, ByVal c2 As Color, ByVal r As Rectangle)
        Using T As New LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical)
            g.FillRectangle(T, R)
        End Using
    End Sub
    Shared Sub Blend(ByVal g As Graphics, ByVal c1 As Color, ByVal c2 As Color, ByVal c3 As Color, ByVal c As Single, ByVal d As Integer, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        Dim v As New ColorBlend(3)
        V.Colors = New Color() {c1, c2, c3}
        V.Positions = New Single() {0, c, 1}
        Dim R As New Rectangle(x, y, width, height)
        Using T As New LinearGradientBrush(R, c1, c1, CType(d, LinearGradientMode))
            T.InterpolationColors = v : g.FillRectangle(T, R)
        End Using
    End Sub
    Shared Function RoundedRectangle(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer, ByVal cornerwidth As Integer, ByVal PenWidth As Integer) As GraphicsPath
        Dim p As New GraphicsPath
        p.StartFigure()
        p.AddArc(New Rectangle(x, y, cornerwidth, cornerwidth), 180, 90)
        p.AddLine(cornerwidth, y, width - cornerwidth - PenWidth, y)

        p.AddArc(New Rectangle(width - cornerwidth - PenWidth, y, cornerwidth, cornerwidth), -90, 90)
        p.AddLine(width - PenWidth, cornerwidth, width - PenWidth, height - cornerwidth - PenWidth)

        p.AddArc(New Rectangle(width - cornerwidth - PenWidth, height - cornerwidth - PenWidth, cornerwidth, cornerwidth), 0, 90)
        p.AddLine(width - cornerwidth - PenWidth, height - PenWidth, cornerwidth, height - PenWidth)

        p.AddArc(New Rectangle(x, height - cornerwidth - PenWidth, cornerwidth, cornerwidth), 90, 90)
        p.CloseFigure()

        Return p
    End Function

    Shared Sub BackGround(ByVal width As Integer, ByVal height As Integer, ByVal G As Graphics)

        Dim P1 As Color = Color.FromArgb(29, 25, 22)
        Dim P2 As Color = Color.FromArgb(35, 31, 28)

        For y As Integer = 0 To height Step 4
            For x As Integer = 0 To width Step 4
                G.FillRectangle(New SolidBrush(P1), New Rectangle(x, y, 1, 1))
                G.FillRectangle(New SolidBrush(P2), New Rectangle(x, y + 1, 1, 1))
                Try
                    G.FillRectangle(New SolidBrush(P1), New Rectangle(x + 2, y + 2, 1, 1))
                    G.FillRectangle(New SolidBrush(P2), New Rectangle(x + 2, y + 3, 1, 1))
                Catch
                End Try
            Next
        Next
    End Sub
End Class