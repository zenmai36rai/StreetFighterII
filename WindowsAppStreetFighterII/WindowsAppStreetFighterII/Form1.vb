Imports System.Data.SqlTypes

Public Class Form1
    Dim Time As Integer = 99
    Dim Life As Integer = 100
    Dim Life2 As Integer = 100
    Dim frame As Integer = 0
    Dim cx As Integer = 0
    Dim cy As Integer = 0
    Dim walk_vx = 0
    Dim jump As Integer = 0
    Dim jump_vx As Integer = 0
    Dim jump_vy As Double = 0
    Dim gravity As Double = 0.5
    Dim jump_time = 0
    Dim sonic_x As Integer = 1000
    Dim state As Integer = 0
    Dim next_state As Integer = 0
    Dim state_time As Integer = 0
    Dim tech_flag As Integer = 0
    Dim tech_time As Integer = 0
    Dim img_0 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風立ち.png")
    Dim img_1 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風パンチ.png")
    Dim img_2 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風ムエタイキック.png")
    Dim img_3 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風構え.png")
    Dim img_4 As Image = Image.FromFile("..\..\アニメ素材\ガイル絵本風ソニックブーム.png")
    Dim img_sonic As Image = Image.FromFile("..\..\アニメ素材\ソニックブーム.png")
    Dim img_r1 As Image = Image.FromFile("..\..\アニメ素材\リュウ立ち.png")
    Dim img_back As Image = Image.FromFile("..\..\アニメ素材\背景.png")
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        frame = frame + 1
        If frame Mod 100 = 0 Then
            Time = Time - 1
        End If
        PictureBox1.BackColor = Color.Black
        Dim canvas As New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim g As Graphics = Graphics.FromImage(canvas)
        g.DrawImage(img_back, 0, 0, canvas.Width, canvas.Height)
        ProgressFrame()
        Dim img As Image
        If (state = 1) Then
            img = img_1
            SetNextFrame(0, 12)
        ElseIf (state = 2) Then
            img = img_2
            SetNextFrame(0, 12)
        ElseIf (state = 3) Then
            img = img_3
            SetNextFrame(4, 6)
        ElseIf (state = 4) Then
            img = img_4
            If tech_flag = 0 Then
                sonic_x = 0
            End If
            SetNextFrame(0, 36)
        Else
            img = img_0
            tech_flag = 0
        End If
        If jump = 1 Then
            jump_time = jump_time + 1
            Dim j As Integer = -20
            Dim t As Double = jump_time / 4
            jump_vy = 15 - gravity * t * t
            cx = cx + jump_vx
            cy = cy + jump_vy
            If cy < 0 Then
                jump = 0
                jump_time = 0
                jump_vx = 0
                jump_vy = 0
                cy = 0
            End If
        Else
            cx = cx + walk_vx
        End If
        DrawTime(g)
        g.DrawImage(img, 20 + cx, 220 - cy, 200, 200)
        If sonic_x <= canvas.Width Then
            g.DrawImage(img_sonic, 20 + cx + sonic_x, 220, 200, 200)
            sonic_x = sonic_x + 5
        End If
        g.DrawImage(img_r1, 400, 220, 200, 200)
        g.Dispose()
        PictureBox1.Image = canvas
    End Sub
    Private Sub DrawTime(ByRef g As Graphics)
        Dim f As Font = New Font("MSゴシック", 20)
        Dim b As Brush = Brushes.White
        Dim p As Point = New Point(PictureBox1.Width / 2 - 30, 10)
        g.DrawString(Time.ToString, f, b, p)
        Dim r1 As Rectangle = New Rectangle(10, 10, 200, 30)
        g.FillRectangle(Brushes.Red, r1)
        Dim r2 As Rectangle = New Rectangle(210 - Life * 2, 10, Life * 2, 30)
        g.FillRectangle(Brushes.Yellow, r2)
        Dim a As Integer = PictureBox1.Width
        Dim r3 As Rectangle = New Rectangle(a - 210, 10, 200, 30)
        g.FillRectangle(Brushes.Red, r3)
        Dim r4 As Rectangle = New Rectangle(a - 210, 10, Life2 * 2, 30)
        g.FillRectangle(Brushes.Yellow, r4)
    End Sub
    Private Sub SetNextFrame(ByVal ns As Integer, ByVal st As Integer)
        If tech_flag = 0 Then
            next_state = ns
            state_time = st
            tech_flag = 1
        End If
    End Sub
    Private Sub ProgressFrame()
        If tech_flag = 0 Then
            tech_time = 0
        Else
            tech_time = tech_time + 1
            If state_time = tech_time Then
                state = next_state
                tech_flag = 0
            End If
        End If
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If jump = 0 Then
            jump = 1
            jump_time = 0
            jump_vy = 0
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If jump = 0 Then
            jump = 1
            jump_time = 0
            jump_vy = 0
            jump_vx = -5
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If jump = 0 Then
            jump = 1
            jump_time = 0
            jump_vy = 0
            jump_vx = 5
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        state = 0
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        state = 1
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        state = 1
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        state = 3
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        state = 2
    End Sub
    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        state = 2
    End Sub
    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        state = 2
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim t As String = TextBox1.Text.ToString
        If t = "" Then
            Life = 0
        Else
            Life = t
        End If
        If Life < 0 Then
            Life = 0
        End If
        If Life > 100 Then
            Life = 100
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim t As String = TextBox2.Text.ToString
        If t = "" Then
            Life2 = 0
        Else
            Life2 = t
        End If
        If Life2 < 0 Then
            Life2 = 0
        End If
        If Life2 > 100 Then
            Life2 = 100
        End If
    End Sub

    Private Sub Button6_MouseDown(sender As Object, e As MouseEventArgs) Handles Button6.MouseDown
        walk_vx = 5
    End Sub
    Private Sub Button4_MouseDown(sender As Object, e As MouseEventArgs) Handles Button4.MouseDown
        walk_vx = -5
    End Sub
    Private Sub Button6_MouseUp(sender As Object, e As MouseEventArgs) Handles Button6.MouseUp
        walk_vx = 0
    End Sub
    Private Sub Button4_MouseUp(sender As Object, e As MouseEventArgs) Handles Button4.MouseUp
        walk_vx = 0
    End Sub
End Class
