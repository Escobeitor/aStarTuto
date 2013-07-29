Public Class Form1

    '   0123
    '   y-------->
    '0 x
    '1 |
    '2 |
    '3 v
    Dim casillas(,) As Integer = {{0, 0, 0, 0, 0, 0, 0},
                                  {0, 0, 0, 1, 0, 0, 0},
                                  {0, 2, 0, 1, 0, 3, 0},
                                  {0, 0, 0, 1, 0, 0, 0},
                                  {0, 0, 0, 0, 0, 0, 0}}


    Const VACIO As Integer = 0
    Const PARED As Integer = 1
    Const ORIGEN As Integer = 2
    Const DESTINO As Integer = 3

    Const costeOrtogonal As Integer = 10
    Const costeDiagonal As Integer = 14

    Dim tamañoCasilla As Integer = 20

    Dim puntoDestino As New Point()
    Dim puntoOrigen As New Point()

    Dim listaAbierta As New List(Of Casilla)
    Dim listaCerrada As New List(Of Casilla)

    Dim casillaOrigen As Casilla()
    Dim casillaDestino As Casilla





    Sub New()

        ' Llamada necesaria para el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

        Try

            puntoOrigen = obtenerPunto(casillas, ORIGEN)
            Dim casillaOrigen = New Casilla(puntoOrigen)
            puntoDestino = obtenerPunto(casillas, DESTINO)
            Dim casillaDestino = New Casilla(puntoDestino)


            Dim algo As Integer = obtenerValor(puntoOrigen)

            listaAbierta.Add(casillaOrigen)

            'busca todos los cuadros adyacentes a puntoOrigen, ignorando paredes, metiendolos en la lista abierta y poniendo puntoOrigen como padre
            buscarCeldasAdyacentes(puntoOrigen, casillas, listaAbierta)

            'Saca el cuadro inicial A desde tu lista abierta y añádelo a una "lista cerrada" de cuadrados que no necesitan, por ahora, ser mirados de nuevo.
            listaAbierta.Remove(casillaOrigen)
            listaCerrada.Add(casillaOrigen)

            'calculo de G en listaAbierta

            For Each casilla As Casilla In listaAbierta
                casilla._G = calculoCosteG(casilla)
            Next

            'calculo de H en listaAbierta
            For Each casilla As Casilla In listaAbierta
                casilla._H = calculoCosteH(casilla, casillaDestino)
            Next




        Catch ex As Exception
            MsgBox(ex.Message)
        End Try




    End Sub

    Public Function obtenerValor(ByVal punto As Point) As Integer

        Return casillas(punto.Y - 1, punto.X - 1)

    End Function

    Public Function obtenerPunto(ByVal casillas(,) As Integer, ByVal tipoCasilla As Integer) As Point

        Dim width As Integer = casillas.GetUpperBound(1)
        Dim height As Integer = casillas.GetUpperBound(0)



        For x As Integer = 0 To width
            For y As Integer = 0 To height

                Select Case casillas(y, x)

                    Case tipoCasilla
                        Return New Point(x + 1, y + 1)
                End Select

            Next
        Next

    End Function

    Private Sub button1_Click(sender As Object, e As EventArgs) Handles button1.Click
        pictureBox1.Invalidate()
        dibujarMapa(casillas)
    End Sub

    Private Sub dibujarMapa(casillas As Integer(,))


    End Sub

    Private Sub pictureBox1_Paint(sender As Object, e As PaintEventArgs) Handles pictureBox1.Paint

        If casillas Is Nothing Then
            Return
        Else

            ' Get bounds of the array.
            Try
                Dim width As Integer = casillas.GetUpperBound(1)
                Dim height As Integer = casillas.GetUpperBound(0)

                For x As Integer = 0 To width
                    For y As Integer = 0 To height

                        Select Case casillas(y, x)
                            Case 0
                                e.Graphics.FillRectangle(New SolidBrush(Color.Black), New Rectangle(x * tamañoCasilla, y * tamañoCasilla, tamañoCasilla, tamañoCasilla))
                            Case 1
                                e.Graphics.FillRectangle(New SolidBrush(Color.White), New Rectangle(x * tamañoCasilla, y * tamañoCasilla, tamañoCasilla, tamañoCasilla))
                            Case 2
                                e.Graphics.FillRectangle(New SolidBrush(Color.Green), New Rectangle(x * tamañoCasilla, y * tamañoCasilla, tamañoCasilla, tamañoCasilla))
                            Case 3
                                e.Graphics.FillRectangle(New SolidBrush(Color.Red), New Rectangle(x * tamañoCasilla, y * tamañoCasilla, tamañoCasilla, tamañoCasilla))

                        End Select

                    Next
                Next
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End If
    End Sub

    Private Sub buscarCeldasAdyacentes(puntoOrigen As Point, casillas As Integer(,), ByRef listaAbierta As List(Of Casilla))

        '(1,2),(2,2),(3,2)
        '(1,3),(2,3),(3,3)
        '(1,4),(2,4),(3,4)

        Dim xModificador As Integer = -1
        Dim yModificador As Integer = -1

        Dim filaLongitud As Integer = 0

        For i = 0 To 8
            filaLongitud += 1

            Dim puntoEvaluar As New Point(puntoOrigen.X + xModificador, puntoOrigen.Y + yModificador)

            If obtenerValor(puntoEvaluar) = VACIO Then
                listaAbierta.Add(New Casilla(puntoEvaluar, puntoOrigen))
            End If

            If filaLongitud = 3 Then
                yModificador += 1
                xModificador = -1
                filaLongitud = 0
            Else
                xModificador += 1
            End If
        Next



    End Sub

    Private Function calculoCosteG(casilla As Casilla) As Integer


        '(-1,-1),(0,-1),(1,-1)
        '(-1,0),(2,3),(1,0)
        '(-1,1),(0,1),(1,1)

        If casilla.posX - casilla.casillaPadre.X <> 0 And casilla.posY - casilla.casillaPadre.Y <> 0 Then
            Return costeDiagonal
        Else
            Return costeOrtogonal
        End If
    End Function

    Private Function calculoCosteH(casilla As Casilla, casillaDestino As Casilla) As Integer

        'metodo Manhattan:  calcular el número total de cuadros movidos horizontalmente y verticalmente para alcanzar el cuadrado destino desde el cuadro actual, sin hacer uso de movimientos diagonales. Luego multiplicamos el total por 10.
        Dim movimientosX As Integer = Math.Abs(Math.Abs(casilla.posX) - Math.Abs(casillaDestino.posX))
        Dim movimientosY As Integer = Math.Abs(Math.Abs(casilla.posY) - Math.Abs(casillaDestino.posY))

        Return movimientosX * costeOrtogonal + movimientosY * costeOrtogonal

    End Function

End Class
