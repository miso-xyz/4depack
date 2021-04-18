Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class EncryptHelper

#Region "Encrypt"

    Public Sub EncryptFile(ByVal path As String, ByVal encoding As Encoding, ByVal password As String)
        Dim value As String = EncryptString(IO.File.ReadAllText(path), password)
        Using streamWriter As StreamWriter = New StreamWriter(path, False, encoding)
            streamWriter.Write(value)
        End Using
    End Sub
    Public Function EncryptString(ByVal data As String, ByVal password As String) As String
        Dim utf8Encoding As UTF8Encoding = New UTF8Encoding()
        Dim md5CryptoServiceProvider As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
        Dim key As Byte() = md5CryptoServiceProvider.ComputeHash(utf8Encoding.GetBytes(password))
        Dim tripleDESCryptoServiceProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
        tripleDESCryptoServiceProvider.Key = key
        tripleDESCryptoServiceProvider.Mode = CipherMode.ECB
        tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7
        Dim bytes As Byte() = utf8Encoding.GetBytes(data)
        Dim inArray As Byte()
        Try
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor()
            inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length)
        Finally
            tripleDESCryptoServiceProvider.Clear()
            md5CryptoServiceProvider.Clear()
        End Try
        Return Convert.ToBase64String(inArray)
    End Function

    Public Sub EncryptBytesFile(ByVal path As String, ByVal password As String)
        Dim bMessage As Byte() = File.ReadAllBytes(path)
        File.WriteAllBytes(path, EncryptBytes(bMessage, password))
    End Sub
    Public Function EncryptBytes(ByVal data As Byte(), ByVal password As String) As Byte()
        Dim utf8Encoding As UTF8Encoding = New UTF8Encoding()
        Dim md5CryptoServiceProvider As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
        Dim key As Byte() = md5CryptoServiceProvider.ComputeHash(utf8Encoding.GetBytes(password))
        Dim tripleDESCryptoServiceProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
        tripleDESCryptoServiceProvider.Key = key
        tripleDESCryptoServiceProvider.Mode = CipherMode.ECB
        tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7
        Dim result As Byte()
        Try
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor()
            result = cryptoTransform.TransformFinalBlock(data, 0, data.Length)
        Finally
            tripleDESCryptoServiceProvider.Clear()
            md5CryptoServiceProvider.Clear()
        End Try
        Return result
    End Function

#End Region

    Public Function DecryptFile(ByVal path As String, ByVal password As String) As String
        Using streamReader As StreamReader = New StreamReader(path, True)
            Return DecryptString(streamReader.ReadToEnd(), password)
        End Using
    End Function

    Public Function GetEncryptionAlgorithm(ByVal value As Integer) As String
        Select Case value
            Case 0
                Return ""
        End Select
    End Function

    Public Function DecryptBytes(ByVal data As Byte(), ByVal password As String) As Byte()
        Dim utf8Encoding As New UTF8Encoding()
        Dim md5CryptoServiceProvider As New MD5CryptoServiceProvider()
        Dim key As Byte() = md5CryptoServiceProvider.ComputeHash(utf8Encoding.GetBytes(password))
        Dim tripleDESCryptoServiceProvider As New TripleDESCryptoServiceProvider()
        tripleDESCryptoServiceProvider.Key = key
        tripleDESCryptoServiceProvider.Mode = CipherMode.ECB
        tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7
        Dim result As Byte()
        Try
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor()
            result = cryptoTransform.TransformFinalBlock(data, 0, data.Length)
        Finally
            tripleDESCryptoServiceProvider.Clear()
            md5CryptoServiceProvider.Clear()
        End Try
        Return result
    End Function

    Public Function DecryptString(ByVal data As String, ByVal password As String) As String
        Dim utf8Encoding As New UTF8Encoding()
        Dim md5CryptoServiceProvider As New MD5CryptoServiceProvider()
        Dim key As Byte() = md5CryptoServiceProvider.ComputeHash(utf8Encoding.GetBytes(password))
        Dim tripleDESCryptoServiceProvider As New TripleDESCryptoServiceProvider()
        tripleDESCryptoServiceProvider.Key = key
        tripleDESCryptoServiceProvider.Mode = CipherMode.ECB
        tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7
        Dim array As Byte() = Convert.FromBase64String(data)
        Dim bytes As Byte()
        Try
            Dim cryptoTransform As ICryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor()
            bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length)
        Finally
            tripleDESCryptoServiceProvider.Clear()
            md5CryptoServiceProvider.Clear()
        End Try
        Return utf8Encoding.GetString(bytes)
    End Function
End Class
