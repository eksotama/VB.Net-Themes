Imports System, System.Collections
Imports System.Drawing, System.Drawing.Drawing2D
Imports System.ComponentModel, System.Windows.Forms
Imports System.Runtime.InteropServices

'''''''''''''''''''''''''''''
'```````````````````````````'
'```  Creator: ATROCiTY~ ```'
'```    Version: 1.0.0   ```'
'```  Theme Base: 1.5.2  ```'
'```   Thanks AeonHack   ```'
'```````````````````````````'
'!~!~!--DO_NOT_REMOVE--!~!~!'
''''''''''''''''''''''''''''' 


Class aTheme
    Inherits ThemeContainer153


#Region "Close Button Border property"
    <Description("Choose weather or not to draw the border around the close button; Fixed Position!"), Browsable(True)>
    Private _drawCButtonBorder As Boolean = True
    Public Property drawCButtonBorder() As Boolean
        Get
            Return _drawCButtonBorder
        End Get
        Set(ByVal value As Boolean)
            _drawCButtonBorder = value
            Invalidate()
        End Set
    End Property

#End Region


    Sub New()
        Header = 30
        BackColor = Color.FromArgb(41, 41, 41)
        TransparencyKey = Color.Fuchsia

        SetColor("45", 45, 45, 45)
    End Sub


    Protected Overrides Sub ColorHook()

        BackColor = Color.FromArgb(41, 41, 41)

    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)


        DrawGradient(Color.FromArgb(60, 60, 60), Color.FromArgb(41, 41, 41), 0, 0, Width, 31)
        Dim DarkDown As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.Black))
        Dim DarkUp As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.Black))
        G.FillRectangle(DarkDown, New Rectangle(0, 0, ClientRectangle.Width, Header))
        G.FillRectangle(DarkUp, New Rectangle(0, 0, ClientRectangle.Width, Header))

        'New Pen(Color.FromArgb(58, 58, 58)
        G.DrawLine(New Pen(Color.FromArgb(58, 58, 58)), 0, 31, Width, 31)
        G.DrawLine(New Pen(Color.FromArgb(25, 25, 25)), 0, 32, Width, 32)
        G.DrawLine(New Pen(Color.FromArgb(58, 58, 58)), 0, 33, Width, 33)



        G.DrawLine(New Pen(Color.FromArgb(58, 58, 58)), 34, 30, 34, 0)
        G.DrawLine(New Pen(Color.FromArgb(25, 25, 25)), 33, 31, 33, 0)
        G.DrawLine(New Pen(Color.FromArgb(58, 58, 58)), 32, 30, 32, 0)

        G.FillRectangle(New SolidBrush(BackColor), 0, 0, 32, 30)

        If _drawCButtonBorder Then
            G.DrawLine(New Pen(Color.FromArgb(58, 58, 58)), Me.Width - 36, 30, Me.Width - 36, 0)
            G.DrawLine(New Pen(Color.FromArgb(25, 25, 25)), Me.Width - 35, 31, Me.Width - 35, 0)
            G.DrawLine(New Pen(Color.FromArgb(58, 58, 58)), Me.Width - 34, 30, Me.Width - 34, 0)
        End If


        DrawText(New SolidBrush(Color.FromArgb(0, 168, 198)), HorizontalAlignment.Left, 36, 0)

        
        DrawBorders(New Pen(Color.FromArgb(65, 65, 65)), 0)
        DrawBorders(New Pen(Color.FromArgb(70, 70, 70), 1))

        G.DrawImage(Form1.Icon.ToBitmap, New Point(1, 1))

        DrawCorners(TransparencyKey)
    End Sub
End Class

Class aCButton
    Inherits ThemeControl153

    Sub New()
        Me.Size = New Size(19, 18)
        'Me.Anchor = AnchorStyles.Top & AnchorStyles.Right

        'BEGIN BUTTON-STATE GRADIENTS
        SetColor("DownGradient2", 72, 69, 75)
        SetColor("DownGradient1", 24, 23, 26)

        SetColor("NoneGradient2", 24, 23, 26)
        SetColor("NoneGradient1", 72, 69, 75)
        'END BUTTON-STATE GRADIENTS


        SetColor("Text", Color.White)
        SetColor("Border1", 12, Color.White)
        SetColor("Border2", Color.SlateGray)
    End Sub

    Private C1, C2, C3, C4 As Color
    Private B1 As SolidBrush
    Private P1, P2 As Pen

    Protected Overrides Sub ColorHook()
        C1 = GetColor("DownGradient1")
        C2 = GetColor("DownGradient2")
        C3 = GetColor("NoneGradient1")
        C4 = GetColor("NoneGradient2")

        B1 = New SolidBrush(GetColor("Text"))

        P1 = New Pen(GetColor("Border1"))
        P2 = New Pen(GetColor("Border2"))
    End Sub

    Protected Overrides Sub PaintHook()
        Select Case State
            Case MouseState.None
                DrawGradient(C3, C4, ClientRectangle, 90.0F)
            Case MouseState.Over
                DrawGradient(C3, C4, ClientRectangle, 90.0F)
            Case MouseState.Down
                DrawGradient(C1, C2, ClientRectangle, 90.0F)
        End Select

        DrawText(New SolidBrush(Color.FromArgb(0, 168, 198)), "X", HorizontalAlignment.Center, 0, 0)

        DrawBorders(P2)
        'DrawBorders(New Pen(Color.FromArgb(0, 168, 198)))

        'DrawCorners(BackColor)
    End Sub

    Sub IveBeenClicked_ItTicklesLOL() Handles Me.Click
        FindForm().Close()
    End Sub
End Class

Class aButton
    Inherits ThemeControl153

    Sub New()

        Me.Size = New Size(102, 23)

        'BEGIN BUTTON-STATE GRADIENTS
        SetColor("DownGradient2", 72, 69, 75)
        SetColor("DownGradient1", 24, 23, 26)

        SetColor("NoneGradient2", 24, 23, 26)
        SetColor("NoneGradient1", 72, 69, 75)
        'END BUTTON-STATE GRADIENTS


        SetColor("Text", 0, 133, 157)
        SetColor("Border1", 65, 65, 65)
        SetColor("Border2", 75, 75, 75)
    End Sub

    Private C1, C2, C3, C4 As Color
    Private B1 As SolidBrush
    Private P1, P2 As Pen

    Protected Overrides Sub ColorHook()
        C1 = GetColor("DownGradient1")
        C2 = GetColor("DownGradient2")
        C3 = GetColor("NoneGradient1")
        C4 = GetColor("NoneGradient2")

        B1 = New SolidBrush(GetColor("Text"))

        P1 = New Pen(GetColor("Border1"))
        P2 = New Pen(GetColor("Border2"))
    End Sub

    Protected Overrides Sub PaintHook()
        Select Case State
            Case MouseState.None
                DrawGradient(C3, C4, ClientRectangle, 90.0F)
            Case MouseState.Over
                'DrawGradient(C5, C6, ClientRectangle, 90.0F)
            Case MouseState.Down
                DrawGradient(C1, C2, ClientRectangle, 90.0F)
        End Select

        DrawText(B1, HorizontalAlignment.Center, 0, 0)

        DrawBorders(P1, 1)
        DrawBorders(P2)

        DrawCorners(BackColor)
    End Sub
End Class

Class aTextBox
    Inherits TextBox
    Sub New()
        Me.BackColor = Color.FromArgb(41, 41, 41)
        Me.BorderStyle = BorderStyle.FixedSingle
        Me.ForeColor = Color.FromArgb(0, 133, 157)

        Me.Text = Me.Name
    End Sub
End Class

Class aProgressBar
    Inherits ThemeControl153
#Region "Properties"
    <Description("The maximum ammount of steps the progressbar can go before being full"), Category("Atrocity"), Browsable(True)>
    Private _Maximum As Integer = 100
    Public Property Maximum() As Integer
        Get
            Return _Maximum
        End Get
        Set(ByVal v As Integer)

            If v < 1 Then v = 1
            If v < _Value Then _Value = v

            _Maximum = v
            Invalidate()
        End Set
    End Property

    <Description("The current ammount of steps the progressbar has taken."), Category("Atrocity"), Browsable(True)>
    Private _Value As Integer
    Public Property Value() As Integer
        Get
            Return _Value
        End Get
        Set(ByVal v As Integer)

            If v > _Maximum Then v = _Maximum

            _Value = v
            Invalidate()
        End Set
    End Property
#End Region
    Sub New()
        SetColor("bg", 45, 45, 45)
        SetColor("Grad1", 24, 23, 26)
        SetColor("Grad2", 72, 69, 75)

        SetColor("border1", 65, 65, 65)
        SetColor("border2", 70, 70, 70)

        SetColor("text", 0, 168, 198)
    End Sub

    Dim GRAD1, GRAD2, BG1 As Color
    Dim P1, P2 As Pen

    Protected Overrides Sub ColorHook()
        GRAD1 = GetColor("Grad1")
        GRAD2 = GetColor("Grad2")
        BG1 = GetColor("bg")

        P1 = New Pen(GetColor("border1"))
        P2 = New Pen(GetColor("border2"))
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BG1)

        'DrawGradient(GRAD1, GRAD2, 0, 0, CInt(_Value / _Maximum * Width) - 1, Height - 1, -90S)
        G.FillRectangle(New SolidBrush(GRAD1), 0, 0, CInt(_Value / _Maximum * Width) - 1, Height - 1)

        Dim DarkDown As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.Black))
        Dim DarkUp As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.Black))
        G.FillRectangle(DarkDown, New Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height))
        G.FillRectangle(DarkUp, New Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height))




        G.DrawString(CStr(_Value) & "%", New Font("Courier New", 8S), New SolidBrush(GetColor("text")), New Point(Width \ 2 - 9, Height \ 2 - 7))

        DrawBorders(P2, 0)
        DrawBorders(New Pen(Color.Black), 1)
        DrawBorders(P2, 2)

        DrawCorners(BG1)
    End Sub
End Class

Class aLabel
    Inherits Label
    Sub New()
        Me.BackColor = Color.FromArgb(41, 41, 41)
        Me.ForeColor = Color.FromArgb(0, 133, 157)
    End Sub
End Class

Class cBox
    Inherits ThemeControl153

#Region " Properties "
    Private _CheckedState As Boolean
    Public Property CheckedState() As Boolean
        Get
            Return _CheckedState
        End Get
        Set(ByVal v As Boolean)
            _CheckedState = v
            Invalidate()
        End Set
    End Property
#End Region
    Sub New()
        Size = New Size(90, 15)
        MinimumSize = New Size(16, 16)
        MaximumSize = New Size(600, 16)
        CheckedState = False

        SetColor("bg", 41, 41, 41)
        SetColor("Text", 0, 133, 157)

        SetColor("g1", 132, 192, 240)
        SetColor("g2", 78, 123, 168)
        SetColor("g3", 98, 159, 220)
        SetColor("g4", 62, 102, 147)

        SetColor("g5", 80, 80, 80)
    End Sub

    Dim g1, g2, g3, g4, g5 As Color

    Protected Overrides Sub ColorHook()
        g1 = GetColor("g1")
        g2 = GetColor("g2")
        g3 = GetColor("g3")
        g4 = GetColor("g4")
        g5 = GetColor("g5")
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(GetColor("bg"))


        Select Case CheckedState
            Case True
                DrawGradient(g1, g2, 3, 3, 9, 9, 90S)
                DrawGradient(g3, g4, 4, 4, 7, 7, 90S)
            Case False
                DrawGradient(g5, g5, 0, 0, 15, 15, 90S)
        End Select


        G.DrawRectangle(New Pen(GetColor("Text")), 0, 0, 14, 14)
        G.DrawRectangle(Pens.Black, 1, 1, 12, 12)
        DrawText(New SolidBrush(GetColor("Text")), HorizontalAlignment.Left, 17, 0)
    End Sub

    Sub changeCheck() Handles Me.Click
        Select Case CheckedState
            Case True
                CheckedState = False
            Case False
                CheckedState = True
        End Select
    End Sub
End Class

Public Class TabControl
    Inherits System.Windows.Forms.TabControl
    'It may not be mine, but I tweaked the bugger out of the tabpage design.
#Region "This is not mine; it has been used under the MIT license. Expand to read"
    'Copyright (c) 2005 Mick Doherty : http://www.dotnetrix.co.uk
    'Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

    'The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

    'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#End Region

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SetStyle(ControlStyles.AllPaintingInWmPaint Or _
                ControlStyles.DoubleBuffer Or _
                ControlStyles.ResizeRedraw Or _
                ControlStyles.UserPaint, True)
    End Sub

    'UserControl1 overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

#Region " InterOP "

    <StructLayout(LayoutKind.Sequential)> _
    Private Structure NMHDR
        Public HWND As Int32
        Public idFrom As Int32
        Public code As Int32
        Public Overloads Function ToString() As String
            Return String.Format("Hwnd: {0}, ControlID: {1}, Code: {2}", HWND, idFrom, code)
        End Function
    End Structure

    Private Const TCN_FIRST As Int32 = &HFFFFFFFFFFFFFDDA&
    Private Const TCN_SELCHANGING As Int32 = (TCN_FIRST - 2)

    Private Const WM_USER As Int32 = &H400&
    Private Const WM_NOTIFY As Int32 = &H4E&
    Private Const WM_REFLECT As Int32 = WM_USER + &H1C00&

#End Region

#Region " BackColor Manipulation "

    'As well as exposing the property to the Designer we want it to behave just like any other 
    'controls BackColor property so we need some clever manipulation.
    Private m_Backcolor As Color = Color.Empty
    <Browsable(True), _
    Description("The background color used to display text and graphics in a control.")> _
    Public Overrides Property BackColor() As Color
        Get
            If m_Backcolor.Equals(Color.Empty) Then
                If Parent Is Nothing Then
                    Return Control.DefaultBackColor
                Else
                    Return Parent.BackColor
                End If
            End If
            Return m_Backcolor
        End Get
        Set(ByVal Value As Color)
            If m_Backcolor.Equals(Value) Then Return
            m_Backcolor = Value
            Invalidate()
            'Let the Tabpages know that the backcolor has changed.
            MyBase.OnBackColorChanged(EventArgs.Empty)
        End Set
    End Property
    Public Function ShouldSerializeBackColor() As Boolean
        Return Not m_Backcolor.Equals(Color.Empty)
    End Function
    Public Overrides Sub ResetBackColor()
        m_Backcolor = Color.Empty
        Invalidate()
    End Sub

#End Region

    Protected Overrides Sub OnParentBackColorChanged(ByVal e As System.EventArgs)
        MyBase.OnParentBackColorChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnSelectedIndexChanged(ByVal e As System.EventArgs)
        MyBase.OnSelectedIndexChanged(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        e.Graphics.Clear(BackColor)
        Dim r As Rectangle = Me.ClientRectangle
        If TabCount <= 0 Then Return
        'Draw a custom background for Transparent TabPages
        r = SelectedTab.Bounds
        Dim sf As New StringFormat
        sf.Alignment = StringAlignment.Center
        sf.LineAlignment = StringAlignment.Center
        Dim DrawFont As New Font(Font.FontFamily, 24, FontStyle.Regular, GraphicsUnit.Pixel)
        ControlPaint.DrawStringDisabled(e.Graphics, "", DrawFont, BackColor, RectangleF.op_Implicit(r), sf)
        DrawFont.Dispose()
        'Draw a border around TabPage
        r.Inflate(3, 3)
        Dim tp As TabPage = TabPages(SelectedIndex)
        Dim PaintBrush As New SolidBrush(tp.BackColor)
        e.Graphics.FillRectangle(PaintBrush, r)
        ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, ButtonBorderStyle.Outset)
        'Draw the Tabs
        For index As Integer = 0 To TabCount - 1
            tp = TabPages(index)
            tp.Invalidate()
            tp.ForeColor = Color.FromArgb(0, 168, 198)
            tp.BackColor = Color.FromArgb(41, 41, 41)
            r = GetTabRect(index)
            Dim bs As ButtonBorderStyle = ButtonBorderStyle.Outset
            If index = SelectedIndex Then bs = ButtonBorderStyle.Inset
            PaintBrush.Color = tp.BackColor
            e.Graphics.FillRectangle(PaintBrush, r)
            ControlPaint.DrawBorder(e.Graphics, r, PaintBrush.Color, bs)
            PaintBrush.Color = tp.ForeColor

            'Set up rotation for left and right aligned tabs
            If Alignment = TabAlignment.Left Or Alignment = TabAlignment.Right Then
                Dim RotateAngle As Single = 90
                If Alignment = TabAlignment.Left Then RotateAngle = 270
                Dim cp As New PointF(r.Left + (r.Width \ 2), r.Top + (r.Height \ 2))
                e.Graphics.TranslateTransform(cp.X, cp.Y)
                e.Graphics.RotateTransform(RotateAngle)
                r = New Rectangle(-(r.Height \ 2), -(r.Width \ 2), r.Height, r.Width)
            End If
            'Draw the Tab Text
            If tp.Enabled Then
                e.Graphics.DrawString(tp.Text, Font, PaintBrush, RectangleF.op_Implicit(r), sf)
            Else
                ControlPaint.DrawStringDisabled(e.Graphics, tp.Text, Font, tp.BackColor, RectangleF.op_Implicit(r), sf)
            End If

            e.Graphics.ResetTransform()

        Next
        PaintBrush.Dispose()
    End Sub

    <Description("Occurs as a tab is being changed.")> _
    Public Event SelectedIndexChanging As SelectedTabPageChangeEventHandler

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = (WM_REFLECT + WM_NOTIFY) Then
            Dim hdr As NMHDR = DirectCast(Marshal.PtrToStructure(m.LParam, GetType(NMHDR)), NMHDR)
            If hdr.code = TCN_SELCHANGING Then
                Dim tp As TabPage = TestTab(Me.PointToClient(Cursor.Position))
                If Not tp Is Nothing Then
                    Dim e As New TabPageChangeEventArgs(Me.SelectedTab, tp)
                    RaiseEvent SelectedIndexChanging(Me, e)
                    If e.Cancel OrElse tp.Enabled = False Then
                        m.Result = New IntPtr(1)
                        Return
                    End If
                End If
            End If
        End If
        MyBase.WndProc(m)
    End Sub

    Private Function TestTab(ByVal pt As Point) As TabPage
        For index As Integer = 0 To TabCount - 1
            If GetTabRect(index).Contains(pt.X, pt.Y) Then
                Return TabPages(index)
            End If
        Next
        Return Nothing
    End Function
#Region " EventArgs Class's "

    Public Class TabPageChangeEventArgs
        Inherits EventArgs

        Private _Selected As TabPage
        Private _PreSelected As TabPage
        Public Cancel As Boolean = False

        Public ReadOnly Property CurrentTab() As TabPage
            Get
                Return _Selected
            End Get
        End Property

        Public ReadOnly Property NextTab() As TabPage
            Get
                Return _PreSelected
            End Get
        End Property

        Public Sub New(ByVal CurrentTab As TabPage, ByVal NextTab As TabPage)
            _Selected = CurrentTab
            _PreSelected = NextTab
        End Sub

    End Class

    Public Delegate Sub SelectedTabPageChangeEventHandler(ByVal sender As Object, ByVal e As TabPageChangeEventArgs)

#End Region
End Class

Class aGroupBox
    Inherits ThemeContainer153

    Sub New()
        ControlMode = True
        BackColor = Color.FromArgb(41, 41, 41)

        SetColor("Grad1", 60, 60, 60)
        SetColor("Grad2", 20, Color.White)
        SetColor("Grad3", 80, Color.White)
        SetColor("Transp", Color.Transparent)
        SetColor("Text", Color.FromArgb(0, 133, 157))
        SetColor("BG", Color.FromArgb(41, 41, 41))
        SetColor("Border1", Color.FromArgb(25, 25, 25))
        SetColor("Border2", Color.FromArgb(58, 58, 58))


        Me.Size = New Size(200, 100)

    End Sub

    Dim C1, C2, C3, C4 As Color
    Dim P1, P2 As Pen
    Dim B1 As Brush

    Protected Overrides Sub ColorHook()
        C1 = GetColor("Grad1")
        C2 = GetColor("Grad2")
        C3 = GetColor("Grad3")
        C4 = GetColor("Transp")

        B1 = New SolidBrush(GetColor("Text"))

        P1 = New Pen(GetColor("Border1"))
        P2 = New Pen(GetColor("Border2"))
    End Sub

    Protected Overrides Sub PaintHook()
        G.Clear(BackColor)
        DrawGradient(C1, BackColor, 0, 0, Width, 20)
        G.DrawLine(P2, 0, 20, Width + 2, 20)
        Dim DarkDown As New HatchBrush(HatchStyle.DarkDownwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.Black))
        Dim DarkUp As New HatchBrush(HatchStyle.DarkUpwardDiagonal, Color.Transparent, Color.FromArgb(50, Color.Black))
        G.FillRectangle(DarkDown, New Rectangle(0, 0, ClientRectangle.Width, 20))
        G.FillRectangle(DarkUp, New Rectangle(0, 0, ClientRectangle.Width, 20))
        'DrawGradient(C3, C4, 0, -75, Width, 100)
        DrawText(B1, HorizontalAlignment.Center, 0, 0)
        DrawBorders(P2, 0)
        DrawBorders(P1, 1)
        DrawBorders(P2, 2)
    End Sub
End Class