Imports System.Data.SqlTypes
Imports System.Security.Cryptography

Public Class Form1
    Const KOKYU_FLAG_UP = 0
    Const KOKYU_FLAG_DOWN = 1
    Private Class clMove
        Public Life As Integer = 100
        Public cx As Integer = 0
        Public cy As Integer = 0
        Public walk_vx = 0
        Public jump As Integer = 0
        Public jump_vx As Integer = 0
        Public jump_vy As Double = 0
        Public jump_time = 0
        Public jump_height As Integer = 12
        Public gravity As Double = 0.4
        Public sonic_x As Integer = 1000
        Public sonic_r As Integer = 0
        Public state As Integer = 0
        Public next_state As Integer = 0
        Public state_time As Integer = 0
        Public tech_flag As Integer = 0
        Public tech_time As Integer = 0
        Public hitbox As Rectangle = New Rectangle(0, 0, 0, 0)
        Public recieve As Rectangle = New Rectangle(0, 0, 0, 0)
        Public hitcheck As Integer = 0
        Public damage As Integer = 0
        Public firecheck As Integer = 0
        Public firedamage As Integer = 25
        Public damagebuff As Integer = 0
        Public guardbuff As Integer = 0
        Public hitmark As Point = New Point(0, 0)
        Public kokyu As Integer = 0
        Public kokyu_ud As Integer = KOKYU_FLAG_DOWN
    End Class
    Dim Time As Integer = 99
    Dim frame As Integer = 0
    Dim c1 As clMove = New clMove()
    Dim c2 As clMove = New clMove()
    Dim ryu_state = 1
    Dim hadou_x = 1000
    Dim img_0 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風立ち.png")
    Dim img_1 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風パンチ.png")
    Dim img_2 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風ムエタイキック.png")
    Dim img_3 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風構え.png")
    Dim img_4 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風ソニックブーム.png")
    Dim img_5 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風ガード.png")
    Dim img_sonic As Image = Image.FromFile("..\..\アニメ素材\ソニックブーム.png")
    Dim img_sonic2 As Image = Image.FromFile("..\..\アニメ素材\ソニックブーム２.png")
    Dim img_r0_1 As Image = Image.FromFile("..\..\アニメ素材\リュウ立ち\1.png")
    Dim img_r0_2 As Image = Image.FromFile("..\..\アニメ素材\リュウ立ち\2.png")
    Dim img_r0_3 As Image = Image.FromFile("..\..\アニメ素材\リュウ立ち\3.png")
    Dim img_r1 As Image = Image.FromFile("..\..\アニメ素材\リュウ立ち.png")
    Dim img_r2 As Image = Image.FromFile("..\..\アニメ素材\リュウ構え.png")
    Dim img_r3 As Image = Image.FromFile("..\..\アニメ素材\リュウ波動拳.png")
    Dim img_r4 As Image = Image.FromFile("..\..\アニメ素材\リュウジャンプ.png")
    Dim img_r5 As Image = Image.FromFile("..\..\アニメ素材\リュウガード.png")
    Dim img_r6 As Image = Image.FromFile("..\..\アニメ素材\リュウパンチ.png")
    Dim img_r7 As Image = Image.FromFile("..\..\アニメ素材\リュウキック.png")
    Dim img_r8 As Image = Image.FromFile("..\..\アニメ素材\リュウ飛び蹴り.png")
    Dim img_hadou As Image = Image.FromFile("..\..\アニメ素材\波動拳.png")
    Dim img_hit As Image = Image.FromFile("..\..\アニメ素材\ヒットマーク.png")
    Dim img_back As Image = Image.FromFile("..\..\アニメ素材\背景.png")
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        frame = frame + 1
        If frame Mod 100 = 0 Then
            Time = Time - 1
        End If
        If c1.damagebuff > 0 Then
            c1.damagebuff -= 1
        End If
        If c2.damagebuff > 0 Then
            c2.damagebuff -= 1
        End If
        If c1.guardbuff > 0 Then
            c1.guardbuff -= 1
        End If
        If c2.guardbuff > 0 Then
            c2.guardbuff -= 1
        End If
        PictureBox1.BackColor = Color.Black
        Dim canvas As New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim g As Graphics = Graphics.FromImage(canvas)
        g.DrawImage(img_back, 0, 0, canvas.Width, canvas.Height)
        ProgressFrame(c1)
        ProgressFrame(c2)
        Dim img As Image
        Select Case c1.state
            Case 0
                img = img_0
                c1.hitbox = New Rectangle(0, 0, 0, 0)
            Case 1
                img = img_1
                SetNextFrame(c1, 0, 9)
                c1.hitbox = New Rectangle(100, 0, 100, 100)
                c1.damage = 9
            Case 2
                img = img_2
                SetNextFrame(c1, 0, 14)
                c1.hitbox = New Rectangle(100, 100, 100, 100)
                c1.damage = 14
            Case 3
                img = img_3
                SetNextFrame(c1, 4, 10)
            Case 4
                img = img_4
                If c1.tech_flag = 0 Then
                    c1.sonic_x = c1.cx
                    c1.firecheck = 0
                End If
                SetNextFrame(c1, 0, 36)
            Case 5
                img = img_5
            Case Else
                img = img_0
                c1.tech_flag = 0
        End Select
        DrawTime(g)
        JumpCalc(c1)
        JumpCalc(c2)
        Dim shadow As Rectangle = New Rectangle(40 + c1.cx, 380, 100, 30)
        g.FillEllipse(Brushes.Black, shadow)
        g.DrawImage(img, 20 + c1.cx, 220 - c1.cy, 200, 200)
        If c1.sonic_x <= canvas.Width And c1.firecheck = 0 Then
            If c1.sonic_x Mod 50 = 0 Then
                c1.sonic_r = c1.sonic_r + 1
            End If
            If c1.sonic_r Mod 2 = 0 Then
                g.DrawImage(img_sonic, 20 + c1.sonic_x, 220, 200, 200)
            Else
                g.DrawImage(img_sonic2, 20 + c1.sonic_x, 220, 200, 200)
            End If
            c1.sonic_x = c1.sonic_x + 5
        End If
        Select Case c2.state
            Case 0
                img = img_r0_1
                c2.hitbox = New Rectangle(0, 0, 0, 0)
            Case 1
                img = img_r2
                SetNextFrame(c2, 2, 12)
            Case 2
                img = img_r3
                If c2.tech_flag = 0 Then
                    hadou_x = c2.cx
                    c2.firecheck = 0
                End If
                SetNextFrame(c2, 0, 48)
            Case 5
                img = img_r5
            Case 6
                img = img_r6
                SetNextFrame(c2, 0, 9)
                c2.hitbox = New Rectangle(100, 0, 100, 100)
                c2.damage = 9
            Case 7
                img = img_r7
                SetNextFrame(c2, 0, 12)
                c2.hitbox = New Rectangle(100, 70, 100, 100)
                c2.damage = 14
            Case 8
                img = img_r8
                c2.hitbox = New Rectangle(100, 70, 100, 100)
                c2.damage = 14
            Case Else
                img = img_r0_1
                c2.tech_flag = 0
        End Select
        shadow = New Rectangle(460 + c2.cx, 380, 100, 30)
        g.FillEllipse(Brushes.Black, shadow)
        If c2.cy > 0 Then
            Dim center_x As Integer = 400 + c2.cx + 100
            Dim center_y As Integer = 220 - c2.cy + 100
            Dim r As Double = 100 * Math.Sqrt(2)
            If c2.state = 8 Then
                g.DrawImage(img_r8, 400 + c2.cx, 220 - c2.cy, 200, 200)
            ElseIf c2.jump_vx < 0 Then
                Dim angle1 As Double = (135 + c2.jump_time / 19 * 360) * 3.14 / 180
                Dim angle2 As Double = (45 + c2.jump_time / 19 * 360) * 3.14 / 180
                Dim angle3 As Double = (225 + c2.jump_time / 19 * 360) * 3.14 / 180
                Dim dp() As Point = New Point() {New Point(center_x + r * Math.Sin(angle1), center_y + r * Math.Cos(angle1)),
                New Point(center_x + r * Math.Sin(angle2), center_y + r * Math.Cos(angle2)),
                New Point(center_x + r * Math.Sin(angle3), center_y + r * Math.Cos(angle3))}
                g.DrawImage(img_r4, dp)
            ElseIf c2.jump_vx > 0 Then
                Dim angle1 As Double = (135 - c2.jump_time / 19 * 360) * 3.14 / 180
                Dim angle2 As Double = (45 - c2.jump_time / 19 * 360) * 3.14 / 180
                Dim angle3 As Double = (225 - c2.jump_time / 19 * 360) * 3.14 / 180
                Dim dp() As Point = New Point() {New Point(center_x + r * Math.Sin(angle1), center_y + r * Math.Cos(angle1)),
                New Point(center_x + r * Math.Sin(angle2), center_y + r * Math.Cos(angle2)),
                New Point(center_x + r * Math.Sin(angle3), center_y + r * Math.Cos(angle3))}
                g.DrawImage(img_r4, dp)
            Else
                g.DrawImage(img_r4, 400 + c2.cx, 220 - c2.cy, 200, 200)
            End If
        ElseIf c2.state = 0 Then
            g.DrawImage(img, 400 + c2.cx, 220 - c2.cy, 200, 200)
            If c2.kokyu_ud = KOKYU_FLAG_DOWN Then
                c2.kokyu = c2.kokyu + 1
                If c2.kokyu = 20 Then
                    c2.kokyu_ud = KOKYU_FLAG_UP
                End If
            Else
                c2.kokyu = c2.kokyu - 1
                If c2.kokyu = -60 Then
                    c2.kokyu_ud = KOKYU_FLAG_DOWN
                End If
            End If
                Dim k As Integer = c2.kokyu / 10
            g.DrawImage(img_r0_2, 400 + c2.cx + 138, 220 - c2.cy + 48 + k, 38, 55)
            g.DrawImage(img_r0_3, 400 + c2.cx + 40, 220 - c2.cy + 44 + k, 44, 48)
        Else
            g.DrawImage(img, 400 + c2.cx, 220 - c2.cy, 200, 200)
        End If

        If hadou_x <= canvas.Width And c2.firecheck = 0 Then
            g.DrawImage(img_hadou, 360 + hadou_x, 220, 200, 200)
            hadou_x = hadou_x - 5
        End If
        Dim h1 As Rectangle = New Rectangle(20 + c1.cx + c1.hitbox.X, 220 - c1.cy + c1.hitbox.Y, c1.hitbox.Width, c1.hitbox.Height)
        Dim h2 As Rectangle = New Rectangle(450 + c2.cx - c2.hitbox.X, 220 - c2.cy + c2.hitbox.Y, c2.hitbox.Width, c2.hitbox.Height)
        If CheckBox1.Checked = True Then
            g.DrawRectangle(Pens.Red, h1)
            g.DrawRectangle(Pens.Red, h2)
        End If
        Dim h3 As Rectangle = New Rectangle(70 + c1.sonic_x, 280, 80, 80)
        Dim h4 As Rectangle = New Rectangle(430 + hadou_x, 260, 80, 80)
        If CheckBox1.Checked = True Then
            If c1.firecheck = 0 Then
                g.DrawRectangle(Pens.Red, h3)
            End If
            If c2.firecheck = 0 Then
                g.DrawRectangle(Pens.Red, h4)
            End If
        End If
        Dim r1 As Rectangle = New Rectangle(30 + c1.cx, 220 - c1.cy, 100, 160)
        Dim r2 As Rectangle = New Rectangle(460 + c2.cx, 220 - c2.cy, 100, 160)
        If CheckBox1.Checked = True Then
            g.DrawRectangle(Pens.Blue, r1)
            g.DrawRectangle(Pens.Blue, r2)
        End If
        If h3.IntersectsWith(h4) And c1.firecheck = 0 And c2.firecheck = 0 Then
            c1.firecheck = 1
            c2.firecheck = 1
        End If
        HitJudge(h1, r2, c1, c2)
        HitJudgeFire(h3, r2, c1, c2)
        HitJudge(h2, r1, c2, c1)
        HitJudgeFire(h4, r1, c2, c1)
        TextBox1.Text = c1.Life
        TextBox2.Text = c2.Life
        If c2.damagebuff > 0 Or c2.guardbuff > 0 Then
            g.DrawImage(img_hit, c1.hitmark.X, c1.hitmark.Y, 200, 200)
        End If
        If c1.damagebuff > 0 Or c1.guardbuff > 0 Then
            g.DrawImage(img_hit, c2.hitmark.X, c2.hitmark.Y, 200, 200)
        End If
        g.Dispose()
        PictureBox1.Image = canvas
    End Sub
    Private Sub HitJudge(ByVal h As Rectangle, ByVal r As Rectangle, ByRef c As clMove, ByRef cc As clMove)
        If h.IntersectsWith(r) And c.hitcheck = 0 Then
            If cc.state <> 5 Then
                cc.Life = cc.Life - c.damage
                cc.damagebuff += c.damage
            Else
                cc.guardbuff += c.damage
            End If
            c.hitcheck = 1
            HitStart(c, h, r)
        End If
    End Sub
    Private Sub HitJudgeFire(ByVal h As Rectangle, ByVal r As Rectangle, ByRef c As clMove, ByRef cc As clMove)
        If h.IntersectsWith(r) And c.firecheck = 0 Then
            If cc.state <> 5 Then
                cc.Life = cc.Life - c.firedamage
                cc.damagebuff += c.firedamage
            Else
                cc.Life = cc.Life - c.firedamage / 4
                cc.damagebuff += c.firedamage / 4
            End If
            c.firecheck = 1
            HitStart(c, h, r)
        End If
    End Sub
    Private Sub HitStart(ByRef c As clMove, ByVal h1 As Rectangle, ByVal r1 As Rectangle)
        Dim h As Rectangle = h1
        h.Intersect(r1)
        c.hitmark = New Point(h.X + h.Width / 2, h.Y + h.Height / 2)
    End Sub
    Private Sub JumpCalc(ByRef c As clMove)
        If c.jump = 1 Then
            c.jump_time = c.jump_time + 1
            Dim t As Double = c.jump_time / 6
            c.jump_vy = c.jump_height - c.gravity * t * t
            c.cx = c.cx + c.jump_vx
            c.cy = c.cy + c.jump_vy
            If c.cy < 0 Then
                c.jump = 0
                c.jump_time = 0
                c.jump_vx = 0
                c.jump_vy = 0
                c.state = 0
                c.hitcheck = 0
                c.cy = 0
            End If
        Else
            c.cx = c.cx + c.walk_vx
        End If
    End Sub
    Private Sub DrawTime(ByRef g As Graphics)
        Dim f As Font = New Font("MSゴシック", 20)
        Dim b As Brush = Brushes.White
        Dim p As Point = New Point(PictureBox1.Width / 2 - 30, 10)
        g.DrawString(Time.ToString, f, b, p)
        f = New Font("MSゴシック", 16)
        b = Brushes.White
        p = New Point(10, 45)
        Dim name As String = "GUILE"
        g.DrawString(name, f, b, p)
        name = "RYU"
        p = New Point(PictureBox1.Width - 15 - f.Size * name.Length, 45)
        g.DrawString(name, f, b, p)
        Dim r1 As Rectangle = New Rectangle(10, 10, 200, 30)
        g.FillRectangle(Brushes.Red, r1)
        Dim r2 As Rectangle = New Rectangle(210 - c1.Life * 2, 10, c1.Life * 2, 30)
        g.FillRectangle(Brushes.Yellow, r2)
        Dim r5 As Rectangle = New Rectangle(210 - c1.Life * 2 - c1.damagebuff * 2, 10, c1.damagebuff * 2, 30)
        g.FillRectangle(Brushes.White, r5)
        Dim a As Integer = PictureBox1.Width
        Dim r3 As Rectangle = New Rectangle(a - 210, 10, 200, 30)
        g.FillRectangle(Brushes.Red, r3)
        Dim r4 As Rectangle = New Rectangle(a - 210, 10, c2.Life * 2, 30)
        g.FillRectangle(Brushes.Yellow, r4)
        Dim r6 As Rectangle = New Rectangle(a - 210 + c2.Life * 2, 10, c2.damagebuff, 30)
        g.FillRectangle(Brushes.White, r6)
    End Sub
    Private Sub SetNextFrame(ByRef c As clMove, ByVal ns As Integer, ByVal st As Integer)
        If c.tech_flag = 0 Then
            c.next_state = ns
            c.state_time = st
            c.tech_flag = 1
            c.hitcheck = 0
            c.damage = 0
        End If
    End Sub
    Private Sub ProgressFrame(ByRef c As clMove)
        If c.tech_flag = 0 Then
            c.tech_time = 0
        Else
            c.tech_time = c.tech_time + 1
            If c.state_time = c.tech_time Then
                c.state = c.next_state
                c.tech_flag = 0
            End If
        End If
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If c1.jump = 0 Then
            c1.jump = 1
            c1.jump_time = 0
            c1.jump_vy = 0
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If c1.jump = 0 Then
            c1.jump = 1
            c1.jump_time = 0
            c1.jump_vy = 0
            c1.jump_vx = -5
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If c1.jump = 0 Then
            c1.jump = 1
            c1.jump_time = 0
            c1.jump_vy = 0
            c1.jump_vx = 5
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        c1.state = 5
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        c1.state = 1
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        c1.state = 1
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        c1.state = 3
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        c1.state = 2
    End Sub
    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        c1.state = 2
    End Sub
    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        c1.state = 2
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim t As String = TextBox1.Text.ToString
        If t = "" Then
            c1.Life = 0
        Else
            c1.Life = t
        End If
        If c1.Life < 0 Then
            c1.Life = 0
        End If
        If c1.Life > 100 Then
            c1.Life = 100
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim t As String = TextBox2.Text.ToString
        If t = "" Then
            c2.Life = 0
        Else
            c2.Life = t
        End If
        If c2.Life < 0 Then
            c2.Life = 0
        End If
        If c2.Life > 100 Then
            c2.Life = 100
        End If
    End Sub

    Private Sub Button6_MouseDown(sender As Object, e As MouseEventArgs) Handles Button6.MouseDown
        c1.walk_vx = 5
    End Sub
    Private Sub Button4_MouseDown(sender As Object, e As MouseEventArgs) Handles Button4.MouseDown
        c1.walk_vx = -5
    End Sub
    Private Sub Button6_MouseUp(sender As Object, e As MouseEventArgs) Handles Button6.MouseUp
        c1.walk_vx = 0
    End Sub
    Private Sub Button4_MouseUp(sender As Object, e As MouseEventArgs) Handles Button4.MouseUp
        c1.walk_vx = 0
    End Sub

    Private Sub Button21_MouseDown(sender As Object, e As EventArgs) Handles Button21.MouseDown
        c2.walk_vx = 5
    End Sub

    Private Sub Button19_MouseDown(sender As Object, e As EventArgs) Handles Button19.MouseDown
        c2.walk_vx = -5
    End Sub
    Private Sub Button21_MouseUp(sender As Object, e As MouseEventArgs) Handles Button21.MouseUp
        c2.walk_vx = 0
    End Sub
    Private Sub Button19_MouseUp(sender As Object, e As MouseEventArgs) Handles Button19.MouseUp
        c2.walk_vx = 0
    End Sub

    Private Sub Button27_Click(sender As Object, e As EventArgs) Handles Button27.Click
        c2.state = 1
    End Sub

    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click
        If c2.jump = 0 Then
            c2.jump = 1
            c2.jump_time = 0
            c2.jump_vy = 0
            c2.jump_vx = -5
        End If
    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        If c2.jump = 0 Then
            c2.jump = 1
            c2.jump_time = 0
            c2.jump_vy = 0
        End If

    End Sub

    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        If c2.jump = 0 Then
            c2.jump = 1
            c2.jump_time = 0
            c2.jump_vy = 0
            c2.jump_vx = 5
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
    End Sub

    Private Sub Button30_Click(sender As Object, e As EventArgs) Handles Button30.Click
        If c2.state = 8 Then
            c2.state = 0
        Else
            c2.state = 8
        End If
    End Sub

    Private Sub Button25_Click(sender As Object, e As EventArgs) Handles Button25.Click
        c2.state = 6
    End Sub

    Private Sub Button28_Click(sender As Object, e As EventArgs) Handles Button28.Click
        c2.state = 7
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
    End Sub
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Q
                Button7.PerformClick()
            Case Keys.W
                Button8.PerformClick()
            Case Keys.E
                Button9.PerformClick()
            Case Keys.S
                Button5.PerformClick()
            Case Keys.F
                Button10.PerformClick()
            Case Keys.G
                Button11.PerformClick()
            Case Keys.H
                Button12.PerformClick()
            Case Keys.V
                Button13.PerformClick()
            Case Keys.B
                Button14.PerformClick()
            Case Keys.N
                Button15.PerformClick()
            Case Keys.NumPad5
                Button20.PerformClick()
            Case Keys.NumPad7
                Button22.PerformClick()
            Case Keys.NumPad8
                Button23.PerformClick()
            Case Keys.NumPad9
                Button24.PerformClick()
            Case Keys.K
                Button25.PerformClick()
            Case Keys.L
                Button26.PerformClick()
            Case Keys.Oemplus
                Button27.PerformClick()
            Case Keys.Oemcomma
                Button28.PerformClick()
            Case Keys.OemPeriod
                Button29.PerformClick()
            Case Keys.OemQuestion
                Button30.PerformClick()
        End Select
    End Sub
    Private Sub Form1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MyBase.KeyPress
        Select Case e.KeyChar
            Case "A", "a"
                c1.walk_vx = -5
            Case "S", "s"
                c1.walk_vx = 0
            Case "D", "d"
                c1.walk_vx = 5
            Case "4"
                c2.walk_vx = -5
            Case "5"
                c2.walk_vx = 0
            Case "6"
                c2.walk_vx = 5
        End Select
    End Sub

    Private Sub Form1_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        Select Case e.KeyCode
            Case Keys.A
                c1.walk_vx = 0
            Case Keys.S
                c1.walk_vx = 0
            Case Keys.D
                c1.walk_vx = 0
            Case Keys.NumPad4
                c2.walk_vx = 0
            Case Keys.NumPad5
                c2.walk_vx = 0
            Case Keys.NumPad6
                c2.walk_vx = 0
        End Select
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        c2.state = 5
    End Sub

    Private Sub Button26_Click(sender As Object, e As EventArgs) Handles Button26.Click
        c2.state = 6
    End Sub

    Private Sub Button29_Click(sender As Object, e As EventArgs) Handles Button29.Click
        c2.state = 7
    End Sub
End Class
