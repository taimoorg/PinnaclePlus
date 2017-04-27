Namespace PinnaclePlus.onfleet

    Public Class Metadata
        Public Property name As String
        Public Property type As String
        Public Property value As String
        Public Property visibility As String()
        Sub New()
            visibility = {"api"}
        End Sub
    End Class
    Public Class ofEvent
        Public Property name As String
        Public Property time As Object
        Public Property location As List(Of Double)
    End Class

    Public Class CompletionDetails
        Public Property events As List(Of ofEvent)
        Public Property failureReason As String
        Public Property time As Nullable(Of Double)
        Public Property success As Boolean
        Public Property photoUploadId As String
        Public Property notes As String
        Public Property signatureUploadId As Object
        Public Property distance As Double
    End Class

    

    Public Class Recipient
        Public Property id As String
        Public Property organization As String
        Public Property timeCreated As Long
        Public Property timeLastModified As Long
        Public Property name As String
        Public Property phone As String
        Public Property notes As String
        Public Property skipSMSNotifications As Boolean
        Public Property metadata As List(Of Metadata)
    End Class

    Public Class Address
        Public Property apartment As String
        Public Property state As String
        Public Property postalCode As String
        Public Property country As String
        Public Property city As String
        Public Property street As String
        Public Property number As String
    End Class

    Public Class Destination
        Public Property id As String
        Public Property timeCreated As Long
        Public Property timeLastModified As Long
        Public Property location As List(Of Double)
        Public Property address As Address
        Public Property notes As String
        Public Property metadata As List(Of Metadata)
    End Class

    Public Class OnfleetTask
        Public Property id As String
        Public Property timeCreated As Long
        Public Property timeLastModified As Long
        Public Property organization As String
        Public Property shortId As String
        Public Property trackingURL As String
        Public Property worker As String
        Public Property merchant As String
        Public Property executor As String
        Public Property creator As String
        Public Property dependencies As List(Of String)
        Public Property state As Integer
        Public Property completeAfter As Long
        Public Property completeBefore As Long
        Public Property pickupTask As Boolean
        Public Property notes As String
        Public Property completionDetails As CompletionDetails
        Public Property feedback As Object
        Public Property metadata As List(Of Metadata)
        Public Property override As Object
        Public Property quantity As Integer
        Public Property serviceTime As Integer
        Public Property delayTime As Object
        Public Property estimatedCompletionTime As Object
        Public Property recipients As List(Of Recipient)
        Public Property destination As Object
        Public Property container As container
    End Class
    Public Class Feedback
        Public Property recipient As String
        Public Property rating As Integer
        Public Property comments As String
    End Class
    Public Class container
        Public Property type As String
        Public Property team As Object
        Public Property worker As Object
    End Class




    Public Class Message
        Public Property err As Integer
        Public Property message As String
        Public Property request As String
        Public Property cause As String
    End Class

    Public Class ErrorResponse
        Public Property code As String
        Public Property message As Message
    End Class
End Namespace