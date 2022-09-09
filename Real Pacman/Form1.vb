Imports System.Drawing.Text

Public Class Form1


    '16 * 16 grid
    Public pacFrames(8)
    Public up(2)
    Public l(2)
    Public down(2)
    Public r(2)
    Public pacdeath(12)
    Public num = 0
    Public side As Integer = 3
    Public gside As Integer = 0
    Public speed As Integer = 8
    Public walls As List(Of PictureBox) = New List(Of PictureBox)
    Public cold(1)
    Public blinkyU(1)
    Public blinkyd(1)
    Public blinkyl(1)
    Public blinkyr(1)
    Public score = 0
    Public scared As Boolean = False
    Public time As Integer
    Public dots As List(Of PictureBox) = New List(Of PictureBox)
    Public highscore As Integer = Int(My.Computer.FileSystem.ReadAllText("highscore.txt"))
    Public level = 1
    Public lives(4)
    Public life = 4
    Public scores(7)
    Public pelletseaten = 0
    Public once = 0
    Public eyes(3)
    Public gscore(3)
    Public blinkyg As Ghost = New Ghost("blinky", 0.8)
    Public pinkyg As Ghost = New Ghost("pinky", 0.8)
    Public pinkyArr()() As Bitmap = New Bitmap(3)() {}

    Dim temp(11)

    Public Function rectCent(pic As PictureBox, size As Size)
        Return New Rectangle(pic.Left + ((pic.Width - size.Width) / 2), pic.Top + ((pic.Height - size.Height) / 2), size.Width, size.Height)

    End Function


    Public Sub JoyStick(form As Object)
        Dim priv As New PrivateFontCollection
        priv.AddFontFile("Joystix.ttf")
        For Each item In Me.Controls
            If TypeOf item Is Label Then
                item.font = New Font(priv.Families(0), item.font.size, FontStyle.Regular)
            End If
        Next

    End Sub



    ''' <summary>
    ''' Reads in a Sprite Sheet and returns an array of images
    ''' </summary>
    ''' <param name="loc"></param>
    ''' <param name="width"></param>
    ''' <param name="height"></param>
    ''' <param name="rows"></param>
    ''' <param name="columns"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function readSheet(loc As String, width As Integer, height As Integer, rows As Integer, columns As Integer)
        Dim img As New Bitmap(loc)
        Dim sprite((rows * columns) - 1)
        For i As Integer = 0 To ((rows * columns) - 1)
            sprite(i) = New Bitmap(width, height)
            Dim gr As Graphics = Graphics.FromImage(sprite(i))
            Dim leftPos As Integer = 0
            leftPos = width * (i Mod columns)
            gr.DrawImage(img, 0, 0, New RectangleF(leftPos, Int(i / columns) * height, width, height), GraphicsUnit.Pixel)
        Next
        Return sprite
    End Function

    Public Function hitx(obj As PictureBox) As Boolean
        Dim area As List(Of PictureBox) = New List(Of PictureBox)
        area.AddRange(walls)
        area.AddRange({ppellet1, pppellet2, ppellet3, ppellet4}.ToList)
        area.Add(ghostarea)
        area.Add(area1)
        area.Add(area2)
        area.Add(pctpac)
        Dim bool As Boolean = False
        For Each wall In area
            If CheckHit(wall, obj) Then
                bool = True
            End If
        Next
        Controls.Remove(ghostarea)
        Controls.Remove(area1)
        Controls.Remove(area2)

        Return bool
    End Function




    Public Sub loaddots()
        Static dotx As Integer = 22
        Static doty As Integer = 70
        Do

            Dim dot As PictureBox = New PictureBox
            dot.Size = New Size(4, 4)
            dot.Location = New Point(dotx, doty)
            dot.BackColor = Color.FromArgb(255, &HFF, &HBC, &H97)
            If Not hitx(dot) Then
                Controls.Add(dot)
                dots.Add(dot)
            End If

            If dot.Left = 422 Then
                doty += 16
                dotx = 22
            Else
                dotx += 16
            End If


        Loop Until doty = 534 And dotx = 422
    End Sub

    Private Function CheckHit(mover As Object, stationary As Object) As Boolean
        If mover.Bounds.IntersectsWith(stationary.Bounds) Then
            CheckHit = True
        Else
            CheckHit = False

        End If
    End Function

    Public Sub loadghosts()

        temp = readSheet("pinky.png", 448, 448, 4, 3)
        pinkyArr = {New Bitmap() {temp(8), temp(9)}, New Bitmap() {temp(2), temp(3)}, New Bitmap() {temp(6), temp(7)}, New Bitmap() {temp(4), temp(5)}}




    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ClientSize = New Size(448, 576)
        pctpac.Location = New Point((448 / 2) - 16, 408)
        loadPacSheet()
        loadwalls()
        blinkyload()
        loaddots()
        JoyStick(Me)
        If highscore = 0 Then
            lblhs.Text = "00"
        Else
            lblhs.Text = highscore
        End If
        lblhs.Left = 295 - lblhs.Width
        loaddeath()
        lives = {life1, life2, life3, life4, life5}
        loadscores()
        eyes = readSheet("eyes.png", 448, 448, 2, 2)
        gscore = readSheet("ghost score.png", 576, 224, 2, 2)
        loadghosts()
        pinky.Image = pinkyArr(2)(0)
    End Sub

    Public Sub loadwalls()
        walls = {PictureBox1, PictureBox2, PictureBox3, PictureBox4, PictureBox5, PictureBox6, PictureBox7, PictureBox8, PictureBox9, PictureBox10, PictureBox11, PictureBox12, PictureBox13, PictureBox14, PictureBox15, PictureBox16, PictureBox17, PictureBox18, PictureBox19, PictureBox20, PictureBox21, PictureBox22, PictureBox23, PictureBox24, PictureBox25, PictureBox26, PictureBox27, PictureBox28, PictureBox29, PictureBox30, PictureBox31, PictureBox32, PictureBox33, PictureBox34, PictureBox35, PictureBox36, PictureBox37, PictureBox38, PictureBox39, PictureBox40, PictureBox41, PictureBox42, PictureBox43, PictureBox44, PictureBox45, PictureBox46, PictureBox47, PictureBox48, PictureBox49, PictureBox50, PictureBox51, PictureBox52, PictureBox53, PictureBox54, door}.ToList
    End Sub

    Public Sub loadscores()
        Dim img As New Bitmap("score.png")

        For i As Integer = 0 To 7
            scores(i) = New Bitmap(640, 224)
            Dim gr As Graphics = Graphics.FromImage(scores(i))
            Dim leftPos As Integer = 0
            Select Case i
                Case 2, 5
                    leftPos = 1280
                Case 3, 6
                    leftPos = 0
                Case 1, 4, 7,
                    leftPos = 640
            End Select
            gr.DrawImage(img, 0, 0, New RectangleF(leftPos, Int(i / 3) * 224, 640, 224), GraphicsUnit.Pixel)
        Next
    End Sub

    Public Sub loadPacSheet()
        'Dim img As New Bitmap("Pac.png")

        'For i As Integer = 0 To 8
        '    pacFrames(i) = New Bitmap(416, 416)
        '    Dim gr As Graphics = Graphics.FromImage(pacFrames(i))
        '    Dim leftPos As Integer = 0
        '    Select Case i
        '        Case 2, 5, 8
        '            leftPos = 832
        '        Case 3, 6
        '            leftPos = 0
        '        Case 1, 4, 7
        '            leftPos = 416
        '    End Select
        '    gr.DrawImage(img, 0, 0, New RectangleF(leftPos, Int(i / 3) * 416, 416, 416), GraphicsUnit.Pixel)
        'Next

        pacFrames = readSheet("Pac.png", 416, 416, 3, 3)



        up = {pacFrames(3), pacFrames(4), pacFrames(2)}
        l = {pacFrames(5), pacFrames(6), pacFrames(2)}
        down = {pacFrames(7), pacFrames(8), pacFrames(2)}
        r = {pacFrames(0), pacFrames(1), pacFrames(2)}
        pacFrames = Nothing
    End Sub

    Public Sub blinkyload()
        Dim blinky(11)
        Dim img As New Bitmap("blinky.png")

        For i As Integer = 0 To 11
            blinky(i) = New Bitmap(448, 448)
            Dim gr As Graphics = Graphics.FromImage(blinky(i))
            Dim leftPos As Integer = 0
            Select Case i
                Case 2, 5, 8, 11
                    leftPos = 896
                Case 3, 6, 9
                    leftPos = 0
                Case 1, 4, 7, 10
                    leftPos = 448
            End Select
            gr.DrawImage(img, 0, 0, New RectangleF(leftPos, Int(i / 3) * 448, 448, 448), GraphicsUnit.Pixel)
        Next

        'blinky = readSheet("blinky.png", 448, 448, 3, 3)

        blinkyr = {blinky(2), blinky(3)}
        blinkyl = {blinky(4), blinky(5)}
        blinkyd = {blinky(6), blinky(7)}
        blinkyU = {blinky(8), blinky(9)}
        cold = {blinky(1), blinky(0)}

    End Sub

    Public Sub loaddeath()

        Dim img As New Bitmap("death.png")

        For i As Integer = 0 To 12
            pacdeath(i) = New Bitmap(480, 320)
            Dim gr As Graphics = Graphics.FromImage(pacdeath(i))
            Dim leftPos As Integer = 0
            Select Case i
                Case 2, 5, 8, 11
                    leftPos = 960
                Case 3, 6, 9, 12
                    leftPos = 0
                Case 1, 4, 7, 10
                    leftPos = 480
            End Select
            gr.DrawImage(img, 0, 0, New RectangleF(leftPos, Int(i / 3) * 320, 480, 320), GraphicsUnit.Pixel)
        Next

    End Sub

    Private Sub pacmove_Tick(sender As Object, e As EventArgs) Handles pacmove.Tick
        If gside = 0 And Not hitanyside(pctpac, 2) Then
            pctpac.Top -= speed
            NextFrame()
        End If
        If gside = 1 And Not hitanyside(pctpac, 3) Then
            pctpac.Left += speed
            NextFrame()
        End If
        If gside = 2 And Not hitanyside(pctpac, 0) Then
            pctpac.Top += speed
            NextFrame()
        End If
        If gside = 3 And Not hitanyside(pctpac, 1) Then
            pctpac.Left -= speed
            NextFrame()
        End If

        If pctpac.Left = Me.ClientSize.Width And gside = 1 Then
            pctpac.Left = 0 - pctpac.Width
        End If

        If pctpac.Right = 0 And gside = 3 Then
            pctpac.Left = Me.ClientSize.Width
        End If
        checkppellet()
        checkpdot()
        If pctpac.Bounds.IntersectsWith(cherry.Bounds) And cherry.Visible And once = 0 Then
            cherry.Size = New Size(38, 38)
            cherry.Location = New Point(cherry.Left - 7, cherry.Top - 7)
            cherry.Image = scores(0)

            score += 100
            once = 1
            scre.Text = score
            scre.Left = 117 - scre.Width
            If score > highscore Then
                highscore = score
                lblhs.Text = highscore
                lblhs.Left = 295 - lblhs.Width
            End If
            fruity.Stop()
            fru.start(0, 1)


        End If


        If rectCent(pctpac, New Size(16, 16)).IntersectsWith(rectCent(blinky, New Size(16, 16))) And blinkyg.statis <> 0 Then
            Select Case scared
                Case False

                    gsidetoside.Stop()
                    PacDir.Stop()

                    blinky.Hide()
                    pctpac.Image = l(2)

                    pacmove.Stop()
                    life = life - 1
                    death.Start()
                    BlinkyMove.Stop()


                Case Else
                    blinkyg.statis = 0
                    blinkyFrightened.Stop()
                    pacmove.Stop()
                    updatesc(200)
                    pctpac.Hide()
                    blinky.Image = gscore(0)
                    ghostsc.Start()
                    pctpac.BringToFront()
            End Select
        End If

    End Sub

    Public Sub updatesc(points As Integer)
        score += points

        scre.Text = score
        scre.Left = 117 - scre.Width
        If score > highscore Then
            highscore = score
            lblhs.Text = highscore
            lblhs.Left = 295 - lblhs.Width
        End If
    End Sub



    Public Sub NextFrame()
        num = (num + 1) Mod 3
        Select Case gside
            Case 0
                pctpac.Image = up(num)
            Case 1
                pctpac.Image = r(num)
            Case 2
                pctpac.Image = down(num)
            Case 3
                pctpac.Image = l(num)
        End Select


    End Sub

    Private Sub PacDir_Tick(sender As Object, e As EventArgs) Handles PacDir.Tick
        If GetKeyState(Keys.Up) < 0 Then
            side = 0
        End If
        If GetKeyState(Keys.Right) < 0 Then
            side = 1
        End If
        If GetKeyState(Keys.Down) < 0 Then
            side = 2
        End If
        If GetKeyState(Keys.Left) < 0 Then
            side = 3
        End If
    End Sub

    Private Declare Function GetKeyState Lib "user32" (ByVal nVirtKey As Short) As Short

    Public Function hitside(obj As Object, obj2 As Object, side As Integer)
        Select Case side
            Case 0

                Return obj.bottom = obj2.top And obj.right > obj2.left And obj.left < obj2.right
            Case 1
                Return obj.left = obj2.right And obj.bottom > obj2.top And obj.top < obj2.bottom
            Case 2
                Return obj.top = obj2.bottom And obj.right > obj2.left And obj.left < obj2.right
            Case 3
                Return obj.right = obj2.left And obj.bottom > obj2.top And obj.top < obj2.bottom
            Case Else
                Return False
        End Select
    End Function


    Public Function hitanyside(obj As Object, sde As Integer, Optional remove As Object = Nothing)
        Dim bool As Boolean = False
        If remove Is Nothing Then
            For Each item In walls

                If hitside(obj, item, sde) Then
                    bool = True
                    Exit For
                End If

            Next
        Else

            For Each item In walls
                If item IsNot remove Then
                    If hitside(obj, item, sde) Then
                        bool = True

                        Exit For
                    End If
                End If

            Next
        End If
        Return bool
    End Function

    Private Sub gsidetoside_Tick(sender As Object, e As EventArgs) Handles gsidetoside.Tick
        Select Case side
            Case 0
                If Not hitanyside(pctpac, 2) Then
                    gside = side
                End If

            Case 1
                If Not hitanyside(pctpac, 3) Then
                    gside = side
                End If
            Case 2
                If Not hitanyside(pctpac, 0) Then
                    gside = side
                End If
            Case 3
                If Not hitanyside(pctpac, 1) Then
                    gside = side
                End If


        End Select

    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown

    End Sub

    Public Function ArrayShuffle(arr As Object)
        Dim index As Long
        Dim newIndex As Long
        Dim firstIndex As Long
        Dim itemCount As Long
        Dim tmpValue As Object

        firstIndex = LBound(arr)
        itemCount = UBound(arr) - LBound(arr) + 1

        For index = UBound(arr) To LBound(arr) + 1 Step -1
            ' evaluate a random index from LBound to INDEX
            newIndex = firstIndex + Int(Rnd() * itemCount)
            ' swap the two items
            tmpValue = arr(index)
            arr(index) = arr(newIndex)
            arr(newIndex) = tmpValue
            ' prepare for next iteration
            itemCount = itemCount - 1
        Next

        Return arr
    End Function

    Public Sub cangestate(num, deri)
        Select Case deri
            Case 0
                blinky.Image = blinkyU(num)
            Case 1
                blinky.Image = blinkyr(num)
            Case 2
                blinky.Image = blinkyd(num)
            Case 3
                blinky.Image = blinkyl(num)
        End Select
    End Sub

    Private Sub BlinkyMove_Tick(sender As Object, e As EventArgs) Handles BlinkyMove.Tick
        Static num As Integer = 0
        Static deric As Integer = 3
        Static vec As Vector = New Vector
        vec.x = pctpac.Left - blinky.Left
        vec.y = pctpac.Top - blinky.Top
        Select Case deric

            Case 0
                If Not hitanyside(blinky, 2) Then
                    blinky.Top -= 8
                Else
                    deric = check(0, blinky, vec)
                End If

            Case 1
                If Not hitanyside(blinky, 3) Then
                    blinky.Left += 8
                Else
                    deric = check(1, blinky, vec)
                End If
            Case 2
                If Not hitanyside(blinky, 0) Then
                    blinky.Top += 8
                Else
                    deric = check(2, blinky, vec)
                End If
            Case 3
                If Not hitanyside(blinky, 1) Then
                    blinky.Left -= 8
                Else
                    deric = check(3, blinky, vec)
                End If
        End Select
        If atIntersection(deric, blinky) Then
            deric = towards(pctpac.Location, blinky, deric)
            'Dim map As Dictionary(Of Single, Integer) = New Dictionary(Of Single, Integer)
            'Select Case deric
            '    Case 0
            '        If Not hitanyside(blinky, 2) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - blinky.Left) ^ 2 + (pctpac.Top - (blinky.Top - 8)) ^ 2), 0)

            '            Catch
            '            End Try
            '        End If
            '        If Not hitanyside(blinky, 3) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - (blinky.Left + 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 1)
            '            Catch
            '            End Try
            '        End If
            '        If Not hitanyside(blinky, 1) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - (blinky.Left - 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 3)
            '            Catch
            '            End Try
            '        End If

            '        deric = map.Item(map.Keys.Min)
            '        map.Clear()
            '    Case 1
            '        If Not hitanyside(blinky, 2) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - blinky.Left) ^ 2 + (pctpac.Top - (blinky.Top - 8)) ^ 2), 0)
            '            Catch
            '            End Try
            '        End If
            '        If Not hitanyside(blinky, 3) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - (blinky.Left + 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 1)
            '            Catch
            '            End Try
            '        End If
            '        If Not hitanyside(blinky, 0) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - (blinky.Left)) ^ 2 + (pctpac.Top - (blinky.Top + 8)) ^ 2), 2)
            '            Catch
            '            End Try
            '        End If


            '        deric = map.Item(map.Keys.Min)
            '        map.Clear()
            '    Case 2

            '        If Not hitanyside(blinky, 3) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - (blinky.Left + 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 1)
            '            Catch
            '            End Try
            '        End If
            '        If Not hitanyside(blinky, 0) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - blinky.Left) ^ 2 + (pctpac.Top - (blinky.Top + 8)) ^ 2), 2)
            '            Catch
            '            End Try
            '        End If
            '        If Not hitanyside(blinky, 1) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - (blinky.Left - 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 3)
            '            Catch
            '            End Try
            '        End If

            '        deric = map.Item(map.Keys.Min)
            '        map.Clear()
            '    Case 3
            '        If Not hitanyside(blinky, 2) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - blinky.Left) ^ 2 + (pctpac.Top - (blinky.Top - 8)) ^ 2), 0)
            '            Catch
            '            End Try
            '        End If
            '        If Not hitanyside(blinky, 1) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - (blinky.Left - 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 3)
            '            Catch

            '            End Try

            '        End If
            '        If Not hitanyside(blinky, 0) Then
            '            Try
            '                map.Add(Math.Sqrt((pctpac.Left - (blinky.Left)) ^ 2 + (pctpac.Top - (blinky.Top + 8)) ^ 2), 2)
            '            Catch
            '            End Try
            '        End If

            '        deric = map.Item(map.Keys.Min)
            '        map.Clear()

            'End Select
        End If

        cangestate(num, deric)
        num = (num + 1) Mod 2
    End Sub

    Public Function atIntersection(deric As Integer, pct As PictureBox)


        If (Not hitanyside(pct, (deric + 1) Mod 4, door)) Or (Not hitanyside(pct, (ops(deric) + 1) Mod 4, door)) Then
            '(Not hitanyside(pct, 2) And Not (deric = 0 Or deric = 2)) Or (Not hitanyside(pct, 0) And Not (deric = 0 Or deric = 2)) Or (Not hitanyside(pct, 1) And Not (deric = 3 Or deric = 1)) Or (Not hitanyside(pct, 3) And Not (deric = 1 Or deric = 3)) Then

            Return True
        Else
            Return False
        End If
    End Function



    Public Function check(pos As Integer, pic As PictureBox, vect As Vector)

        Dim map As Dictionary(Of Single, Integer) = New Dictionary(Of Single, Integer)
        Select Case pos
            Case 0
                If Not hitanyside(blinky, 2) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - blinky.Left) ^ 2 + (pctpac.Top - (blinky.Top - 8)) ^ 2), 0)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 3) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - (blinky.Left + 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 1)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 1) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - (blinky.Left - 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 3)
                    Catch
                    End Try
                End If

                pos = map.Item(map.Keys.Min)
                map.Clear()
            Case 1
                If Not hitanyside(blinky, 2) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - blinky.Left) ^ 2 + (pctpac.Top - (blinky.Top - 8)) ^ 2), 0)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 3) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - (blinky.Left + 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 1)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 0) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - (blinky.Left)) ^ 2 + (pctpac.Top - (blinky.Top + 8)) ^ 2), 2)
                    Catch
                    End Try
                End If


                pos = map.Item(map.Keys.Min)
                map.Clear()
            Case 2

                If Not hitanyside(blinky, 3) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - (blinky.Left + 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 1)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 0) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - blinky.Left) ^ 2 + (pctpac.Top - (blinky.Top + 8)) ^ 2), 2)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 1) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - (blinky.Left - 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 3)
                    Catch
                    End Try
                End If

                pos = map.Item(map.Keys.Min)
                map.Clear()
            Case 3
                If Not hitanyside(blinky, 2) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - blinky.Left) ^ 2 + (pctpac.Top - (blinky.Top - 8)) ^ 2), 0)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 1) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - (blinky.Left - 8)) ^ 2 + (pctpac.Top - (blinky.Top)) ^ 2), 3)
                    Catch

                    End Try

                End If
                If Not hitanyside(blinky, 0) Then
                    Try
                        map.Add(Math.Sqrt((pctpac.Left - (blinky.Left)) ^ 2 + (pctpac.Top - (blinky.Top + 8)) ^ 2), 2)
                    Catch
                    End Try
                End If

                Return map.Item(map.Keys.Min)
                map.Clear()

        End Select

    End Function

    Public Function ops(num As Integer)
        Return (num + 2) Mod 4
    End Function

    Public Sub checkppellet()
        Dim ppellets As List(Of PictureBox) = New List(Of PictureBox)
        ppellets = {ppellet1, pppellet2, ppellet3, ppellet4}.ToList

        For Each ppellet In ppellets
            If pctpac.Bounds.IntersectsWith(ppellet.Bounds) And ppellet.Visible Then
                ppellet.Hide()
                scared = True
                BlinkyMove.Stop()
                If blinkyg.statis <> 0 Then
                    blinkyFrightened.Start()
                End If
                pacmove.Interval = 70
                time = TimeOfDay.Second
                score += 50
                scre.Text = score
                scre.Left = 117 - scre.Width
                If score > highscore Then
                    highscore = score
                    lblhs.Text = highscore
                    lblhs.Left = 295 - lblhs.Width
                End If
            End If
        Next
    End Sub
    Public Sub checkpdot()
        Dim hit As Boolean = False
        Dim cent As Rectangle = New Rectangle()
        cent.Size = New Size(26, 26)
        cent.Location = New Point(pctpac.Left + 3, pctpac.Top + 3)
        For Each dot In dots
            If dot.Bounds.IntersectsWith(cent) And dot.Visible And pctpac.Image Is up(2) Then
                dot.Hide()

                pelletseaten += 1
                score += 10
                scre.Text = score
                scre.Left = 117 - scre.Width
                If score > highscore Then
                    highscore = score
                    lblhs.Text = highscore
                    lblhs.Left = 295 - lblhs.Width
                End If
                hit = True
                If scared Then
                    pacmove.Interval = 63
                Else
                    pacmove.Interval = 70
                End If
            End If
        Next

        If Not hit Then
            pacmove.Interval = 62
        End If
        fruit()
        If pelletseaten = 240 And Not (ppellet1.Visible) And Not (pppellet2.Visible) And Not (ppellet3.Visible) And Not (ppellet4.Visible) Then
            BlinkyMove.Stop()
            blinkyFrightened.Stop()
            Gohome.Stop()
            upp.Stop()
            pacmove.Stop()
            pctpac.Image = up(2)
           
            EndGame.Start()

        End If
    End Sub

    Public Function randomDirection(dir As Integer)
        Randomize()

        Dim deric As List(Of Integer) = New List(Of Integer)

        If Not hitanyside(blinky, ops(dir)) Then

            deric.Add(dir)


        End If
        If Not hitanyside(blinky, (dir + 1) Mod 4) Then

            deric.Add((dir - 1) Mod 4)

        End If
        If Not hitanyside(blinky, ((dir - 1) Mod 4)) Then

            deric.Add((dir + 1) Mod 4)


        End If
        Dim derc As Integer = Math.Round(Rnd() * (deric.Count - 1))
        'deric = ArrayShuffle(deric)
        'derc = deric(1)
        'If derc = dir Or derc = ops(dir) Then
        '    derc = randomDirection(dir)
        'End If
        Return deric.Item(derc)
    End Function

    Private Sub blinkyFrightened_Tick(sender As Object, e As EventArgs) Handles blinkyFrightened.Tick

        Static deric As Integer = 0
        Static term As Integer = 0
        Static modu As Integer = 1
        Static ttime As Integer = TimeOfDay.Second

        Select Case deric

            Case 0
                If Not hitanyside(blinky, 2) Then
                    blinky.Top -= 8
                Else
                    deric = randomDirection(0)
                End If

            Case 1
                If Not hitanyside(blinky, 3) Then
                    blinky.Left += 8
                Else
                    deric = randomDirection(1)
                End If
            Case 2
                If Not hitanyside(blinky, 0) Then
                    blinky.Top += 8
                Else
                    deric = randomDirection(2)
                End If
            Case 3
                If Not hitanyside(blinky, 1) Then
                    blinky.Left -= 8
                Else
                    deric = randomDirection(3)
                End If
        End Select



        blinky.Image = cold(term)
        term = (term + 1) Mod modu




        If atIntersection(deric, blinky) Then
            deric = randomDirection(deric)
        End If


        If TimeOfDay.Second - time = 6 Then
            modu = 2
        End If

        If TimeOfDay.Second - time = 8 Then
            BlinkyMove.Start()
            modu = 1
            scared = False
            pacmove.Interval = 55
            blinkyFrightened.Stop()
        End If
    End Sub


    Public Function f(pos As Integer, pic As PictureBox, vect As Vector)
        Dim closer As Integer
        Select Case pos
            Case 0, 2
                If vect.x < 0 Then
                    closer = 1
                Else
                    closer = 3
                End If
            Case Else
                If vect.y < 0 Then
                    closer = 2
                Else
                    closer = 1
                End If
        End Select

        If Not hitanyside(pic, ops(closer)) Then
            Return closer
        ElseIf Not hitanyside(pic, closer) Then
            Return ops(closer)

        End If

    End Function

    Private Sub Start_Tick(sender As Object, e As EventArgs) Handles Start.Tick

        Static ttime As Integer = 0
        If ttime = 2 Then
            blinky.Show()
            pctpac.Show()
            pinky.Show()
            lblplayer.Hide()
            life5.Hide()
        End If
        If ttime = 4 Then
            BlinkyMove.Start()
            pinkymove.Start()
            pacmove.Start()
            PacDir.Start()
            gsidetoside.Start()
            ttime = 0
            lblReady.Hide()
            Start.Stop()
        Else
            ttime += 1
        End If

    End Sub

    Private Sub oneup_Tick(sender As Object, e As EventArgs) Handles oneup.Tick
        lbloneup.Visible = Not lbloneup.Visible
    End Sub

    Private Sub PictureBox18_Click(sender As Object, e As EventArgs) Handles PictureBox18.Click

    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Computer.FileSystem.WriteAllText("highscore.txt", highscore.ToString, False)
    End Sub





    Private Sub death_Tick(sender As Object, e As EventArgs) Handles death.Tick
        Static num = 0

        pctpac.Image = pacdeath(num)
        If pctpac.Image Is pacdeath(12) Then
            If life <> -1 Then
                lives(life).hide()
                pctpac.Image = l(2)
                pctpac.Hide()
                cherry.Hide()
                refreshGame()
            Else
                gsidetoside.Stop()
                PacDir.Stop()

                blinky.Hide()
                pctpac.Image = l(2)
                pctpac.Hide()
                pacmove.Stop()
                lblReady.Hide()
                lblgame.Show()
                lblover.Show()
                BlinkyMove.Stop()
            End If

        End If
        num += 1
        num = num Mod 13
    End Sub

    Public Sub refreshGame()
        lblReady.Show()
        pctpac.Location = New Point(208, 408)
        'pctpac.Image = My.Resources.ResourceManager.GetObject("l0_sprite_3.png")
        'l0_sprite_3

        pctpac.Show()
        blinky.Location = New Point(208, 216)
        blinky.Show()
        refresh.Start()
        death.Stop()
    End Sub

    Public Sub fruit()


        If 73 >= pelletseaten And pelletseaten >= 70 Or 174 >= pelletseaten And pelletseaten >= 170 Then

            fruity.Start()
            cherry.Show()
        End If
    End Sub


    Private Sub refresh_Tick(sender As Object, e As EventArgs) Handles refresh.Tick
        Static num As Integer = 0
        If num = 1 Then
            num = 0
            side = 3
            gside = 0
            pacmove.Start()
            PacDir.Start()
            lblReady.Hide()
            gsidetoside.Start()
            BlinkyMove.Start()
            refresh.Stop()
        Else
            num += 1
        End If
    End Sub

    Private Sub PictureBox56_Click(sender As Object, e As EventArgs) Handles PictureBox56.Click

    End Sub

    Private Sub fruity_Tick(sender As Object, e As EventArgs) Handles fruity.Tick

        Static num As Single = 0
        Static rand As Single = Math.Round((Rnd() * 1) + 9)

        If num = rand Then
            cherry.Hide()
            fruity.Stop()
            Randomize()
            rand = Math.Round((Rnd() * 1) + 9)
            num = 0
        Else
            num += 1
        End If


    End Sub

    Private Sub lblReady_Click(sender As Object, e As EventArgs) Handles lblReady.Click

    End Sub

    Private Sub fru_Tick(sender As Object, e As EventArgs) Handles fru.Tick

        If fru.startTime = fru.endTime Then
            cherry.Hide()
            cherry.Size = New Size(24, 24)
            cherry.Location = New Point(cherry.Left + 7, cherry.Top + 7)
            cherry.Image = My.Resources.ResourceManager.GetObject("cherry")
            once = 0
            fru.Stop()
        Else
            fru.endTime += 1
        End If
    End Sub

    Private Sub Gohome_Tick(sender As Object, e As EventArgs) Handles Gohome.Tick
        Static num As Integer = 0
        Static deric As Integer = 3
        Static vec As Vector = New Vector
        Gohome.Interval = 48
        vec.x = pctpac.Left - blinky.Left
        vec.y = pctpac.Top - blinky.Top
        Select Case deric

            Case 0
                If Not hitanyside(blinky, 2, door) Then
                    blinky.Top -= 8

                End If

            Case 1
                If Not hitanyside(blinky, 3, door) Then
                    blinky.Left += 8

                End If
            Case 2
                If Not hitanyside(blinky, 0, door) Then
                    blinky.Top += 8

                End If
            Case 3
                If Not hitanyside(blinky, 1, door) Then
                    blinky.Left -= 8

                End If
        End Select
        If atIntersection(deric, blinky) Then
            Dim map As Dictionary(Of Single, Integer) = New Dictionary(Of Single, Integer)
            Select Case deric
                Case 0
                    If Not hitanyside(blinky, 2) Then
                        Try
                            map.Add(Math.Sqrt((208 - blinky.Left) ^ 2 + (272 - (blinky.Top - 8)) ^ 2), 0)

                        Catch
                        End Try
                    End If
                    If Not hitanyside(blinky, 3) Then
                        Try
                            map.Add(Math.Sqrt((208 - (blinky.Left + 8)) ^ 2 + (272 - (blinky.Top)) ^ 2), 1)
                        Catch
                        End Try
                    End If
                    If Not hitanyside(blinky, 1) Then
                        Try
                            map.Add(Math.Sqrt((208 - (blinky.Left - 8)) ^ 2 + (272 - (blinky.Top)) ^ 2), 3)
                        Catch
                        End Try
                    End If

                    deric = map.Item(map.Keys.Min)
                    map.Clear()
                Case 1
                    If Not hitanyside(blinky, 2) Then
                        Try
                            map.Add(Math.Sqrt((208 - blinky.Left) ^ 2 + (272 - (blinky.Top - 8)) ^ 2), 0)
                        Catch
                        End Try
                    End If
                    If Not hitanyside(blinky, 3) Then
                        Try
                            map.Add(Math.Sqrt((208 - (blinky.Left + 8)) ^ 2 + (272 - (blinky.Top)) ^ 2), 1)
                        Catch
                        End Try
                    End If
                    If Not hitanyside(blinky, 0, door) Then
                        Try
                            map.Add(Math.Sqrt((208 - (blinky.Left)) ^ 2 + (272 - (blinky.Top + 8)) ^ 2), 2)
                        Catch
                        End Try
                    End If


                    deric = map.Item(map.Keys.Min)
                    map.Clear()
                Case 2

                    If Not hitanyside(blinky, 3) Then
                        Try
                            map.Add(Math.Sqrt((208 - (blinky.Left + 8)) ^ 2 + (272 - (blinky.Top)) ^ 2), 1)
                        Catch
                        End Try
                    End If
                    If Not hitanyside(blinky, 0, door) Then
                        Try
                            map.Add(Math.Sqrt((208 - blinky.Left) ^ 2 + (272 - (blinky.Top + 8)) ^ 2), 2)
                        Catch
                        End Try
                    End If
                    If Not hitanyside(blinky, 1) Then
                        Try
                            map.Add(Math.Sqrt((208 - (blinky.Left - 8)) ^ 2 + (272 - (blinky.Top)) ^ 2), 3)
                        Catch
                        End Try
                    End If

                    deric = map.Item(map.Keys.Min)
                    map.Clear()
                Case 3
                    If Not hitanyside(blinky, 2) Then
                        Try
                            map.Add(Math.Sqrt((208 - blinky.Left) ^ 2 + (272 - (blinky.Top - 8)) ^ 2), 0)
                        Catch
                        End Try
                    End If
                    If Not hitanyside(blinky, 0, door) Then
                        Try
                            map.Add(Math.Sqrt((208 - (blinky.Left)) ^ 2 + (272 - (blinky.Top + 8)) ^ 2), 2)
                        Catch
                        End Try
                    End If
                    If Not hitanyside(blinky, 1) Then
                        Try
                            map.Add(Math.Sqrt((208 - (blinky.Left - 8)) ^ 2 + (272 - (blinky.Top)) ^ 2), 3)
                        Catch

                        End Try

                    End If


                    deric = map.Item(map.Keys.Min)
                    map.Clear()

            End Select


            Select Case deric
                Case 0
                    blinky.Image = eyes(2)





                Case 1
                    blinky.Image = eyes(3)
                Case 3
                    blinky.Image = eyes(1)
                Case 2
                    blinky.Image = eyes(0)
            End Select


        End If

        If blinky.Location = New Point(208, 272) Then
            scared = False
            Gohome.Stop()

            upp.start(0, 2)
        End If
    End Sub

    Public Function towards(target As Point, ghost As PictureBox, deriction As Integer)
        Dim map As Dictionary(Of Single, Integer) = New Dictionary(Of Single, Integer)
        Select Case deriction
            Case 0
                If Not hitanyside(ghost, 2) Then
                    Try
                        map.Add(Math.Sqrt((target.X - ghost.Left) ^ 2 + (target.Y - (ghost.Top - 8)) ^ 2), 0)

                    Catch
                    End Try
                End If
                If Not hitanyside(ghost, 3) Then
                    Try
                        map.Add(Math.Sqrt((target.X - (ghost.Left + 8)) ^ 2 + (target.Y - (ghost.Top)) ^ 2), 1)
                    Catch
                    End Try
                End If
                If Not hitanyside(ghost, 1) Then
                    Try
                        map.Add(Math.Sqrt((target.X - (ghost.Left - 8)) ^ 2 + (target.Y - (ghost.Top)) ^ 2), 3)
                    Catch
                    End Try
                End If

                Return map.Item(map.Keys.Min)
                map.Clear()
            Case 1
                If Not hitanyside(ghost, 2) Then
                    Try
                        map.Add(Math.Sqrt((target.X - ghost.Left) ^ 2 + (target.Y - (ghost.Top - 8)) ^ 2), 0)
                    Catch
                    End Try
                End If
                If Not hitanyside(ghost, 3) Then
                    Try
                        map.Add(Math.Sqrt((target.X - (ghost.Left + 8)) ^ 2 + (target.Y - (ghost.Top)) ^ 2), 1)
                    Catch
                    End Try
                End If
                If Not hitanyside(ghost, 0, door) Then
                    Try
                        map.Add(Math.Sqrt((target.X - (ghost.Left)) ^ 2 + (target.Y - (ghost.Top + 8)) ^ 2), 2)
                    Catch
                    End Try
                End If


                Return map.Item(map.Keys.Min)
                map.Clear()
            Case 2

                If Not hitanyside(blinky, 3) Then
                    Try
                        map.Add(Math.Sqrt((target.X - (blinky.Left + 8)) ^ 2 + (target.Y - (blinky.Top)) ^ 2), 1)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 0, door) Then
                    Try
                        map.Add(Math.Sqrt((target.X - blinky.Left) ^ 2 + (target.Y - (blinky.Top + 8)) ^ 2), 2)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 1) Then
                    Try
                        map.Add(Math.Sqrt((target.X - (blinky.Left - 8)) ^ 2 + (target.Y - (blinky.Top)) ^ 2), 3)
                    Catch
                    End Try
                End If

                Return map.Item(map.Keys.Min)
                map.Clear()
            Case 3
                If Not hitanyside(blinky, 2) Then
                    Try
                        map.Add(Math.Sqrt((target.X - blinky.Left) ^ 2 + (target.Y - (blinky.Top - 8)) ^ 2), 0)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 0, door) Then
                    Try
                        map.Add(Math.Sqrt((target.X - (blinky.Left)) ^ 2 + (target.Y - (blinky.Top + 8)) ^ 2), 2)
                    Catch
                    End Try
                End If
                If Not hitanyside(blinky, 1) Then
                    Try
                        map.Add(Math.Sqrt((target.X - (blinky.Left - 8)) ^ 2 + (target.Y - (blinky.Top)) ^ 2), 3)
                    Catch

                    End Try

                End If


                Return map.Item(map.Keys.Min)
                map.Clear()

        End Select
    End Function

    Private Sub upp_Tick(sender As Object, e As EventArgs) Handles upp.Tick
        Static numb = 0
        blinky.Image = blinkyU(numb)
        numb = (numb + 1) Mod 2
        blinky.Top -= 4
        If blinky.Location = New Point(208, 216) Then
            scared = False
            blinkyg.statis = 1
            upp.Stop()
            BlinkyMove.Start()
            pacmove.Start()
        End If
    End Sub

    Private Sub ghostsc_Tick(sender As Object, e As EventArgs) Handles ghostsc.Tick
        blinky.Image = eyes(0)
        pctpac.Show()
        pacmove.Start()
        Gohome.Start()
        ghostsc.Stop()

    End Sub

    Private Sub pinkymove_Tick(sender As Object, e As EventArgs) Handles pinkymove.Tick
        Static num As Integer = 0
        Static deric As Integer = 3
        Static vec As Vector = New Vector
        Dim Dirx = 0
        Dim diry = 0
        Select Case gside
            Case 0
                Dirx = -128
                diry = -128
            Case 1
                Dirx = 128
                diry = 0
            Case 2
                Dirx = 0
                diry = 128
            Case 3
                Dirx = -128
                diry = 0




        End Select

        pinkyg.target = New Point(pctpac.Left + Dirx, pctpac.Top + diry)
        Select Case deric

            Case 0
                If Not hitanyside(pinky, 2) Then
                    pinky.Top -= 8
                Else
                    deric = towards(pinkyg.target, pinky, 0)
                End If


            Case 1
                If Not hitanyside(pinky, 3) Then
                    pinky.Left += 8
                Else
                    deric = towards(pinkyg.target, pinky, 1)
                End If
            Case 2
                If Not hitanyside(pinky, 0) Then
                    pinky.Top += 8
                Else
                    deric = towards(pinkyg.target, pinky, 2)
                End If
            Case 3
                If Not hitanyside(pinky, 1) Then
                    pinky.Left -= 8
                Else
                    deric = towards(pinkyg.target, pinky, 3)
                End If
        End Select
        If atIntersection(deric, pinky) Then
            deric = towards(pinkyg.target, pinky, deric)

        End If


    End Sub

    Private Sub EndGame_Tick(sender As Object, e As EventArgs) Handles EndGame.Tick
        Static num = 0
        Static tim = 0
        If tim > 1.75 Then
            For Each wall In walls
                If num = 0 Then
                    wall.BackColor = Color.White
                Else
                    wall.BackColor = Color.FromArgb(255, 0, 0, 192)
                End If
            Next
          
        End If
        num += 1
        num = num Mod 2
        tim += 0.25
        If tim = 2 Then
            cherry.Hide()
            blinky.Hide()
        End If

        If tim = 4 Then
            EndGame.Stop()
            pctpac.Hide()
            lblwon.Show()
        End If
    End Sub
End Class


