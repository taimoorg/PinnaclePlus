Namespace PinnaclePlus.onfleet


    Public Class Vehicle
        Public Property id As String
        Public Property type As String
        Public Property description As String
        Public Property licensePlate As String
        Public Property color As String
    End Class

    Public Class Worker
        Public Property id As String
        Public Property timeCreated As Long
        Public Property timeLastModified As Long
        Public Property organization As String
        Public Property name As String
        Public Property phone As String
        Public Property activeTask As Object
        Public Property tasks As Object()
        Public Property onDuty As Boolean
        Public Property timeLastSeen As Object
        Public Property delayTime As Object
        Public Property teams As String()
        Public Property metadata As Object()
        Public Property vehicle As Vehicle
    End Class

End Namespace