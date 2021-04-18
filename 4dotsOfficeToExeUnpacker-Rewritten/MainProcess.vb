Imports System.Security.Cryptography
Imports System.Reflection
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Drawing
Imports dnlib.DotNet
Imports dnlib.DotNet.Emit

Public Class MainProcess
    Private app As ModuleDefMD
    Private appPath As String
    Private ProjectXML As ProjectFile
    Private ResourceFiles As ResourceFiles()

    Public ReadOnly Property PackerName() As String
        Get
            '
            ' Following code was scrapped due to the checksum being the same in multiple packers
            '
            'For x = 0 To app.Types.Count - 1
            'If app.Types(x).Name = "frmSplash" Then
            'For x_ = 0 To app.Types(x).Methods.Count - 1
            'For Each inst In app.Types(x).Methods(x_).Body.Instructions
            'If inst.OpCode.ToString = "ldstr" Then
            'Select Case inst.Operand.ToString
            'Case "PPT to EXE"
            'Return "Powerpoint To EXE"
            'End Select
            'End If
            'Next
            'Next
            'End If
            'Next
            Dim result As String = "Unknown Packer"
            For x = 0 To app.Types.Count - 1
                Select Case app.Types(x).Namespace.ToString
                    Case "ConvertExcelToEXE4dots"
                        result = "Excel To EXE Converter (Can be based on it)"
                    Case "ConvertWordToEXE4dots"
                        result = "Word To EXE Converter"
                    Case "ConvertPowerpointToEXE4dots"
                        result = "Powerpoint To EXE Converter"
                    Case "PDFToEXEConverter"
                        result = "PDF To EXE Converter"
                    Case "ZIPSelfExtractor"
                        result = "ZIP Self Extractor Maker"
                    Case "LockedDocument"
                        result = "EXE (Document) Locker"
                End Select
                'If app.Types(x).Name.Contains("EXESlideshowProject") Then
                'Return "EXE Slideshow Maker"
                'End If
            Next
            Return result
        End Get
    End Property

    Function GetResourceFiles() As ResourceFiles()
        Return ResourceFiles
    End Function

    Function GetProjectFile() As ProjectFile
        For x = 0 To ResourceFiles.Count - 1
            If ResourceFiles(x).Filename.Contains("project.xml") Then
                Return New ProjectFile(Encoding.Default.GetString(ResourceFiles(x).RawData))
            End If
        Next
    End Function
    Dim FileNameTarget As New List(Of String) From {"LockedDocument", "project", "4dotsAudioBackgroundMusic", "zipexe", ".jpg"}
    Sub New(ByVal path As String)
        Dim asm As Assembly = Assembly.LoadFile(path)
        app = ModuleDefMD.Load(path)
        Dim temp_rf As New List(Of ResourceFiles)

        For x = 0 To asm.GetManifestResourceNames.Count - 1
            For x_ = 0 To FileNameTarget.Count - 1
                If asm.GetManifestResourceNames(x).Contains(FileNameTarget(x_)) Then
                    Using binaryReader As BinaryReader = New BinaryReader(asm.GetManifestResourceStream(asm.GetManifestResourceNames(x)))
                        Using memoryStream As MemoryStream = New MemoryStream()
                            While True
                                Dim num As Long = 32768L
                                Dim buffer As Byte() = New Byte(num - 1) {}
                                Dim num2 As Integer = binaryReader.Read(buffer, 0, CInt(num))
                                If num2 <= 0 Then
                                    Exit While
                                End If
                                If asm.GetManifestResourceNames(x).Contains(FileNameTarget(2)) Then
                                    Using ms_ As New MemoryStream
                                        asm.GetManifestResourceStream(asm.GetManifestResourceNames(x)).CopyTo(ms_)
                                        temp_rf.Add(New ResourceFiles(ms_.ToArray, asm.GetManifestResourceNames(x)))
                                        Continue For
                                    End Using
                                Else
                                    memoryStream.Write(buffer, 0, num2)
                                End If
                                temp_rf.Add(New ResourceFiles(memoryStream.ToArray, asm.GetManifestResourceNames(x)))
                            End While
                        End Using
                    End Using
                End If
            Next
        Next
        ResourceFiles = temp_rf.ToArray
        appPath = path
        GetProjectFile()
    End Sub

    Private Function MoveWithinArray(ByVal array As Array, ByVal source As Integer, ByVal dest As Integer) As Array
        Dim temp As Object = array.GetValue(source)
        System.Array.Copy(array, dest, array, dest + 1, source - dest)
        array.SetValue(temp, dest)
        Return array
    End Function
End Class