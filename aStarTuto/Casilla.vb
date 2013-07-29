Public Class Casilla

    Public posX As Integer
    Public posY As Integer
    Public casillaPadre As Point

    'F = G + H
    Public F As Integer
    'G = el coste de movimiento para ir desde el punto inicial A a un cierto cuadro de la rejilla, siguiendo el camino generado para llegar allí.
    'Public Property G As Integer
    'H = el coste de movimiento estimado para ir desde ese cuadro de la rejilla hasta el destino final, el punto B. Esto es a menudo nombrado como la heurística, la cual puede ser un poco confusa. La razón por la cual es llamada así, se debe a que es una suposición. Realmente no sabemos la distancia actual hasta que encontramos el camino, ya que toda clase de cosas pueden estar en nuestro camino (muros, agua, etc.)
    'Public Property H As Integer

    Private G As Integer
    Public Property _G() As Integer
        Get
            Return G
        End Get
        Set(ByVal value As Integer)
            G = value
            F = G + H
        End Set
    End Property

    Private H As Integer
    Public Property _H() As Integer
        Get
            Return H
        End Get
        Set(ByVal value As Integer)
            H = value
            F = G + H
        End Set
    End Property





    'casilla padre
    Sub New(pos As Point)
        Me.posX = pos.X
        Me.posY = pos.Y
        Me.casillaPadre = New Point(Me.posX, Me.posY)
    End Sub
    'casilla hijo
    Sub New(pos As Point, casillaPadre As Point)
        Me.posX = pos.X
        Me.posY = pos.Y
        Me.casillaPadre = casillaPadre
    End Sub

    Sub New()

    End Sub

End Class
