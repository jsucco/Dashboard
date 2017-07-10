Imports Microsoft.VisualBasic
Imports System.Security.Cryptography
Imports System.Web.Configuration
Imports System.Text


Public Class cypher

    Private QUERYSTRING_VALIDATION_NAME As String = "qtpval"
    Private QUERYSTRING_VALIDATION_NAME_WITH_SEP As String = "&" + QUERYSTRING_VALIDATION_NAME + "="
    Dim des As New TripleDESCryptoServiceProvider
    Dim mds As New MD5CryptoServiceProvider

    Function MD5Hash(value As String) As Byte()

        Return mds.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value))
    End Function

    Function MD5Encrypt(stringInput As String, key As String) As String
        des.Key = MD5Hash(key)
        des.Mode = CipherMode.ECB

        Dim buffer As Byte() = ASCIIEncoding.ASCII.GetBytes(stringInput)

        Return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length))
    End Function

    Function MD5Decrypt(EncryptedString As String, key As String) As String
        des.Key = MD5Hash(key)
        des.Mode = CipherMode.ECB

        Dim buffer As Byte() = Convert.FromBase64String(EncryptedString)

        Return ASCIIEncoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length))
    End Function


    Private Function ComputeHash(ByVal data As String) As String
        'Get Bytes from Plain Text 
        'Dim OSession As HttpSessionState = HttpContext.Current.Session
        'data += OSession.SessionID
        'OSession("AZ") = 5

        Dim plaintextBytes As Byte() = Encoding.UTF8.GetBytes(data)

        Dim hashAlg As HMACSHA1 = New HMACSHA1(conversions.HexToByteArray(WebConfigurationManager.AppSettings("Key64")))

        Dim hash As Byte() = hashAlg.ComputeHash(plaintextBytes)

        Return conversions.ByteArrayToHex(hash)

    End Function

    Public Function HashQueryString(ByVal queryString As String) As String
        Return ComputeHash(queryString)
        'Return queryString + QUERYSTRING_VALIDATION_NAME_WITH_SEP + ComputeHash(queryString)
    End Function

    Public Sub ValidateQueryString()

        Dim request As HttpRequest = HttpContext.Current.Request

        If request.QueryString.Count = 0 Then
            Return
        End If

        Dim queryString As String = request.Url.Query.TrimStart(New Char() {"?"})

        Dim submittedHash = request.QueryString(QUERYSTRING_VALIDATION_NAME)

        If submittedHash = Nothing Then
            Throw New ApplicationException("QueryString validation hash was not sent!")
        End If

        Dim hashPos As Integer = queryString.IndexOf(QUERYSTRING_VALIDATION_NAME_WITH_SEP)
        queryString = queryString.Substring(0, hashPos)

        If ComputeHash(queryString) <> submittedHash Then
            Throw New ApplicationException("Querystring hash values don't match")
        End If
    End Sub

End Class


