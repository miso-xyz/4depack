Imports System.Reflection
Imports System.IO
Public Class ResourceFiles
    Dim data As Byte()
    Private filename_ As String

    Dim EncryptHelper As New EncryptHelper

    Public Property Filename As String
        Get
            Return filename_
        End Get
        Set(ByVal value As String)
            filename_ = value
        End Set
    End Property

    Public Property RawData As Byte()
        Get
            Return data
        End Get
        Set(ByVal value As Byte())
            data = value
        End Set
    End Property

    Sub New(ByVal rawBytes As Byte(), ByVal resourceName As String)
        data = rawBytes
        filename_ = resourceName
    End Sub

    Public Function Extract(ByVal isEncrypted As Boolean, Optional ByVal password As String = "433424234234-93435849839453") As Byte()
        If isEncrypted Then
            Try
                Return EncryptHelper.DecryptBytes(data, password)
            Catch ex As Exception
                Throw New Exception("Failed to decrypt resource!")
            End Try
        Else
            Return data
        End If
    End Function
End Class
