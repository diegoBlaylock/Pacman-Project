Public Class timersec : Inherits Timer

    Public endTime As Integer
    Public startTime As Integer

    Public Overloads Sub start(eTime As Integer, sTime As Integer)
        Me.startTime = sTime
        Me.endTime = endTime
        Me.Enabled = True

    End Sub

End Class
