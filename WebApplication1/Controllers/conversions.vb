Imports Microsoft.VisualBasic

Public Class conversions

    Public Shared Function HexToByteArray(ByVal hex As String) As Byte()

        Dim hexstring As String = hex.Replace("-", "")

        Return Encoding.UTF8.GetBytes(hexstring)

    End Function

    Public Shared Function ByteArrayToHex(ByVal data As Byte()) As Object

        Dim str As String = BitConverter.ToString(data).Replace("-", "")

        Return str

    End Function

End Class

