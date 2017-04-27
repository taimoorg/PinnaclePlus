Namespace PinnaclePlus.nuVizz
    Public Class Weight
        Public Property value As Double

    End Class

    Public Class Volume
        Public Property value As Double
    End Class

    Public Class CompanyId
        Public Property companyId As Integer
        Public Property companyName As String
        Public Property address1 As String
        Public Property address2 As String
        Public Property city As String
        Public Property state As String
        Public Property zip As String
    End Class

    Public Class LoadHeader
        Public Property assignedByCompanyCode As String
        Public Property assignedByCompanyName As String
        Public Property loadAvailable As Boolean
        Public Property loadId As Integer
        Public Property loadSeq As Integer
        Public Property loadNbr As String
        Public Property status As String
        Public Property origin As String
        Public Property originName As String
        Public Property originAddr1 As String
        Public Property originAddr2 As String
        Public Property originCity As String
        Public Property originState As String
        Public Property originZip As String
        Public Property originCountry As String
        Public Property originLatitude As Double
        Public Property originLongitude As Double
        Public Property totalStops As Integer
        Public Property reference As String
        Public Property earliestStartDttm As String
        Public Property weight As Weight
        Public Property volume As Volume
        Public Property totalPallets As Integer
        Public Property totalCartons As Integer
        Public Property companyId As CompanyId
        Public Property assignedDriver As String
        Public Property companyCode As String
        Public Property originPhoneNumber As String
    End Class

    Public Class LoadId
        Public Property loadHeader As LoadHeader
    End Class

    Public Class DriverInfo
        Public Property firstName As String
        Public Property lastName As String
        Public Property phoneNumber As String
    End Class

    Public Class nuVizzStop
        Public Property stopId As Integer
        Public Property loadId As LoadId
        Public Property stopNbr As String
        Public Property status As String
        Public Property stopSeq As Integer
        Public Property shipmentNbr As String
        Public Property bol As String
        Public Property signatureRequired As Boolean
        Public Property shipToName As String
        Public Property shipToAddr1 As String
        Public Property shipToAddr2 As String
        Public Property shipToCity As String
        Public Property shipToState As String
        Public Property shipToZip As String
        Public Property latitude As Double
        Public Property longitude As Double
        Public Property billToState As String
        Public Property comments As Object()
        Public Property documents As Object()
        Public Property stopDetails As Object()
        Public Property driverInfo As DriverInfo
    End Class


End Namespace