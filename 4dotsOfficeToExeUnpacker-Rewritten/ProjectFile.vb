Imports System.Drawing
Imports System.Xml
Imports System.Globalization
Imports System.Text

Public Class ProjectFile

    Private EncryptHelper As New EncryptHelper

    Private XmlDoc As New Xml.XmlDocument
    Private StayOnSlide_ As Double
    Private EffectDuration_ As Double
    Private ImageTransition_ As Integer
    Private BackgroundMusic_ As String
    Private BackgroundMusicVolume_ As Integer
    Private LoopBackgroundMusic_ As Boolean
    Private SizeMode_ As Integer
    Private ImageAlign_ As Integer
    Private BackgroundColor_ As Color
    Private RotateEXIF_ As Boolean
    Private LoopSlideshow_ As Boolean
    Private FullScreen_ As Boolean
    Private EncryptImages_ As Boolean
    Private AskForPassword_ As Boolean
    Private Password_ As String
    Private ProhibitCopyScreen_ As Boolean
    Private ExtendSlideshow_ As Boolean
    Private GUID_ As Guid
    Private MaxViewTimes_ As Integer
    Private MaxPrintTimes_ As Integer
    Private MaxViewTime_ As Integer
    Private ExpireAfter_ As Integer
    Private ViewDateFrom_ As Date
    Private ViewDateTo_ As Date
    Private ViewTimeFrom_ As Date
    Private ViewTimeTo_ As Date
    Private ViewUserNames_ As Boolean
    Private ViewDomainNames_ As Boolean
    Private ViewComputerNames_ As Boolean
    Private ViewMachineSignatures_ As Boolean
    Private MessageOnStart_ As String
    Private MessageOnExit_ As String
    Private AboutBoxText_ As String
    Private WindowTitle_ As String
    Private HasSplashScreen_ As Boolean
    Private SplashScreenText_ As String
    Private SplashScreenTextColor_ As Color
    Private SplashScreenBackColor_ As Color
    Private AllowDrawing_ As Boolean
    Private AllowExportImages_ As Boolean
    Private AllowPrinting_ As Boolean
    Private AllowSaveImage_ As Boolean
    Private AllowViewDocumentProperties_ As Boolean
    Private AllowFullscreen_ As Boolean

    Private HasViewTimeTo_ As Boolean
    Private HasViewTimeFrom_ As Boolean
    Private HasViewDateTo_ As Boolean
    Private HasViewDateFrom_ As Boolean
    Private expireAfterFirstViewTimeSpan_ As TimeSpan
    Private expiresAfterFirstView_ As Boolean
    Private maxViewTimeTimeSpan_ As TimeSpan
    Private maxViewTimeSecs_ As Integer

    Public Enum SlideshowSizeModeEnum
        Zoom
        AsIs
        Stretch
    End Enum

    Sub load()
        Dim xmlNode As XmlNode = XmlDoc.SelectSingleNode("//Misc")
        StayOnSlide = Decimal.Parse(xmlNode.Attributes.GetNamedItem("StayOnSlide").Value.ToString().Replace(",", "."), New CultureInfo("en-US"))
        EffectDuration = Decimal.Parse(xmlNode.Attributes.GetNamedItem("EffectDuration").Value.ToString().Replace(",", "."), New CultureInfo("en-US"))
        ImageTransition = Integer.Parse(xmlNode.Attributes.GetNamedItem("ImageTransition").Value.ToString())
        BackgroundMusic = xmlNode.Attributes.GetNamedItem("BackgroundMusic").Value.ToString()
        BackgroundMusicVolume = xmlNode.Attributes.GetNamedItem("BackgroundMusicVolume").Value.ToString()
        LoopBackgroundMusic = (xmlNode.Attributes.GetNamedItem("BackgroundMusicLoop").Value.ToString() = Boolean.TrueString)
        SizeMode = CType([Enum].Parse(GetType(SlideshowSizeModeEnum), xmlNode.Attributes.GetNamedItem("SizeMode").Value.ToString()), SlideshowSizeModeEnum)
        ImageAlign = Integer.Parse(xmlNode.Attributes.GetNamedItem("ImageAlign").Value.ToString())
        BackgroundColor = ColorTranslator.FromWin32(Integer.Parse(xmlNode.Attributes.GetNamedItem("BackgroundColor").Value.ToString()))
        RotateEXIF = (xmlNode.Attributes.GetNamedItem("RotateEXIF").Value.ToString() = Boolean.TrueString)
        LoopSlideshow = (xmlNode.Attributes.GetNamedItem("LoopSlideshow").Value.ToString() = Boolean.TrueString)
        ExtendSlideshow = (xmlNode.Attributes.GetNamedItem("ExtendSlideshow").Value.ToString() = Boolean.TrueString)
        FullScreen = (xmlNode.Attributes.GetNamedItem("ShowFullScreen").Value.ToString() = Boolean.TrueString)
        AskForPassword = (xmlNode.Attributes.GetNamedItem("AskForPassword").Value.ToString() = Boolean.TrueString)
        EncryptImages = (xmlNode.Attributes.GetNamedItem("EncryptImages").Value.ToString() = Boolean.TrueString)
        AllowDrawing = (xmlNode.Attributes.GetNamedItem("AllowDrawing").Value.ToString() = Boolean.TrueString)
        AllowExportImages = (xmlNode.Attributes.GetNamedItem("AllowExportImages").Value.ToString() = Boolean.TrueString)
        AllowPrinting = (xmlNode.Attributes.GetNamedItem("AllowPrinting").Value.ToString() = Boolean.TrueString)
        AllowSaveImage = (xmlNode.Attributes.GetNamedItem("AllowSaveImage").Value.ToString() = Boolean.TrueString)
        AllowViewDocumentProperties = (xmlNode.Attributes.GetNamedItem("AllowViewDocumentProperties").Value.ToString() = Boolean.TrueString)
        AllowFullscreen = (xmlNode.Attributes.GetNamedItem("AllowFullscreen").Value.ToString() = Boolean.TrueString)
        ProhibitCopyScreen = (xmlNode.Attributes.GetNamedItem("ProhibitCopyScreen").Value.ToString() = Boolean.TrueString)
        GUID = New Guid(xmlNode.Attributes.GetNamedItem("GUID").Value.ToString())
        ViewUserNames = If((xmlNode.Attributes.GetNamedItem("ViewUserNames").Value.ToString() <> String.Empty), EncryptHelper.DecryptString(xmlNode.Attributes.GetNamedItem("ViewUserNames").Value.ToString(), "493589549485043859430889230823"), False)
        ViewDomainNames = If((xmlNode.Attributes.GetNamedItem("ViewDomainNames").Value.ToString() <> String.Empty), EncryptHelper.DecryptString(xmlNode.Attributes.GetNamedItem("ViewDomainNames").Value.ToString(), "493589549485043859430889230823"), False)
        ViewComputerNames = If((xmlNode.Attributes.GetNamedItem("ViewComputerNames").Value.ToString() <> String.Empty), EncryptHelper.DecryptString(xmlNode.Attributes.GetNamedItem("ViewComputerNames").Value.ToString(), "493589549485043859430889230823"), False)
        ViewMachineSignatures = If(xmlNode.Attributes.GetNamedItem("ViewMachineSignatures").Value.ToString() <> String.Empty, xmlNode.Attributes.GetNamedItem("ViewMachineSignatures").Value.ToString(), False)
        MessageOnStart = xmlNode.Attributes.GetNamedItem("MessageOnStart").Value.ToString()
        MessageOnExit = xmlNode.Attributes.GetNamedItem("MessageOnExit").Value.ToString()
        AboutBoxText = xmlNode.Attributes.GetNamedItem("AboutBoxText").Value.ToString()
        WindowTitle = xmlNode.Attributes.GetNamedItem("WindowTitle").Value.ToString()
        HasSplashScreen = (xmlNode.Attributes.GetNamedItem("HasSplashScreen").Value.ToString() = Boolean.TrueString)
        SplashScreenText = xmlNode.Attributes.GetNamedItem("SplashScreenText").Value.ToString()
        SplashScreenTextColor = Color.FromArgb(Integer.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.ToString().Substring(0, 3)), Integer.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.ToString().Substring(4, 3)), Integer.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenTextColor").Value.ToString().Substring(8, 3)))
        SplashScreenBackColor = Color.FromArgb(Integer.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.ToString().Substring(0, 3)), Integer.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.ToString().Substring(4, 3)), Integer.Parse(xmlNode.Attributes.GetNamedItem("SplashScreenBackColor").Value.ToString().Substring(8, 3)))
        If xmlNode.Attributes.GetNamedItem("MaxViewTimes").Value.ToString() <> String.Empty Then
            Dim text As String = EncryptHelper.DecryptString(xmlNode.Attributes.GetNamedItem("MaxViewTimes").Value.ToString(), "3434234-92323454534095")
            Dim array As String() = text.Split(New String() {"|||"}, StringSplitOptions.None)
            MaxViewTimes = Integer.Parse(array(1))
        End If
        If xmlNode.Attributes.GetNamedItem("MaxPrintTimes").Value.ToString() <> String.Empty Then
            Dim text As String = EncryptHelper.DecryptString(xmlNode.Attributes.GetNamedItem("MaxPrintTimes").Value.ToString(), "3434234-92323454534095")
            Dim array As String() = text.Split(New String() {"|||"}, StringSplitOptions.None)
            MaxPrintTimes = Integer.Parse(array(1))
        End If
        If xmlNode.Attributes.GetNamedItem("MaxViewTime").Value.ToString() <> String.Empty Then
            Dim text As String = EncryptHelper.DecryptString(xmlNode.Attributes.GetNamedItem("MaxViewTime").Value.ToString(), "3434234-92323454534095")
            Dim array As String() = text.Split(New String() {"|||"}, StringSplitOptions.None)
            Dim num3 As Integer = Integer.Parse(array(1))
            maxViewTimeTimeSpan_ = New TimeSpan(0, 0, num3)
            maxViewTimeSecs_ = num3
        End If
        If xmlNode.Attributes.GetNamedItem("ExpireAfter").Value.ToString() <> String.Empty Then
            Dim text As String = EncryptHelper.DecryptString(xmlNode.Attributes.GetNamedItem("ExpireAfter").Value.ToString(), "3434234-92323454534095")
            Dim array As String() = text.Split(New String() {"|||"}, StringSplitOptions.None)
            Dim days As Integer = Integer.Parse(array(1))
            Dim num3 As Integer = Integer.Parse(array(2))
            expireAfterFirstViewTimeSpan_ = New TimeSpan(days, 0, 0, num3)
            expiresAfterFirstView_ = True
        End If
        If xmlNode.Attributes.GetNamedItem("ViewDateFrom").Value.ToString() <> String.Empty Then
            HasViewDateFrom_ = True
            ViewDateFrom = DateTime.FromFileTimeUtc(Long.Parse(xmlNode.Attributes.GetNamedItem("ViewDateFrom").Value.ToString()))
        End If
        If xmlNode.Attributes.GetNamedItem("ViewDateTo").Value.ToString() <> String.Empty Then
            HasViewDateTo_ = True
            ViewDateTo = DateTime.FromFileTimeUtc(Long.Parse(xmlNode.Attributes.GetNamedItem("ViewDateTo").Value.ToString()))
        End If
        If xmlNode.Attributes.GetNamedItem("ViewTimeFrom").Value.ToString() <> String.Empty Then
            HasViewTimeFrom_ = True
            ViewTimeFrom = DateTime.FromFileTimeUtc(Long.Parse(xmlNode.Attributes.GetNamedItem("ViewTimeFrom").Value.ToString()))
        End If
        If xmlNode.Attributes.GetNamedItem("ViewTimeTo").Value.ToString() <> String.Empty Then
            HasViewTimeTo_ = True
            ViewTimeTo = DateTime.FromFileTimeUtc(Long.Parse(xmlNode.Attributes.GetNamedItem("ViewTimeTo").Value.ToString()))
        End If
        If xmlNode.Attributes.GetNamedItem("AskForPasswordValue").Value.ToString() <> String.Empty Then
            Password_ = EncryptHelper.DecryptString(xmlNode.Attributes.GetNamedItem("AskForPasswordValue").Value.ToString(), "493589549485043859430889230823")
        Else
            Password_ = ""
        End If
    End Sub

    Sub New(ByVal RawXML As String)
        XmlDoc.LoadXml(RawXML)
        load()
    End Sub

    Public Property StayOnSlide() As Double
        Get
            Return StayOnSlide_
        End Get
        Set(ByVal value As Double)
            StayOnSlide_ = value
        End Set
    End Property
    Public Property EffectDuration() As Double
        Get
            Return EffectDuration_
        End Get
        Set(ByVal value As Double)
            EffectDuration_ = value
        End Set
    End Property
    Public Property ImageTransition() As Integer
        Get
            Return ImageTransition_
        End Get
        Set(ByVal value As Integer)
            ImageTransition_ = value
        End Set
    End Property
    Public Property BackgroundMusic() As String
        Get
            Return BackgroundMusic_
        End Get
        Set(ByVal value As String)
            BackgroundMusic_ = value
        End Set
    End Property
    Public Property BackgroundMusicVolume() As String
        Get
            Return BackgroundMusicVolume_ & "%"
        End Get
        Set(ByVal value As String)
            BackgroundMusicVolume_ = Integer.Parse(value.Replace("%", Nothing))
        End Set
    End Property
    Public Property LoopBackgroundMusic() As Boolean
        Get
            Return LoopBackgroundMusic_
        End Get
        Set(ByVal value As Boolean)
            LoopBackgroundMusic_ = value
        End Set
    End Property
    Public Property SizeMode() As Integer
        Get
            Return SizeMode_
        End Get
        Set(ByVal value As Integer)
            SizeMode_ = value
        End Set
    End Property
    Public Property ImageAlign() As Integer
        Get
            Return ImageAlign_
        End Get
        Set(ByVal value As Integer)
            ImageAlign_ = value
        End Set
    End Property
    Public Property BackgroundColor() As Color
        Get
            Return BackgroundColor_
        End Get
        Set(ByVal value As Color)
            BackgroundColor_ = value
        End Set
    End Property
    Public Property RotateEXIF() As Boolean
        Get
            Return RotateEXIF_
        End Get
        Set(ByVal value As Boolean)
            RotateEXIF_ = value
        End Set
    End Property
    Public Property LoopSlideshow() As Boolean
        Get
            Return LoopSlideshow_
        End Get
        Set(ByVal value As Boolean)
            LoopSlideshow_ = value
        End Set
    End Property
    Public Property FullScreen() As Boolean
        Get
            Return FullScreen_
        End Get
        Set(ByVal value As Boolean)
            FullScreen_ = value
        End Set
    End Property
    Public Property EncryptImages() As Boolean
        Get
            Return EncryptImages_
        End Get
        Set(ByVal value As Boolean)
            EncryptImages_ = value
        End Set
    End Property
    Public Property AskForPassword() As Boolean
        Get
            Return AskForPassword_
        End Get
        Set(ByVal value As Boolean)
            AskForPassword_ = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return password_
        End Get
        Set(ByVal value As String)
            password_ = value
        End Set
    End Property
    Public Property ProhibitCopyScreen() As Boolean
        Get
            Return ProhibitCopyScreen_
        End Get
        Set(ByVal value As Boolean)
            ProhibitCopyScreen_ = value
        End Set
    End Property
    Public Property ExtendSlideshow() As Boolean
        Get
            Return ExtendSlideshow_
        End Get
        Set(ByVal value As Boolean)
            ExtendSlideshow_ = value
        End Set
    End Property
    Public Property GUID() As Guid
        Get
            Return GUID_
        End Get
        Set(ByVal value As Guid)
            GUID_ = value
        End Set
    End Property
    Public Property MaxViewTimes() As Integer
        Get
            Return MaxViewTimes_
        End Get
        Set(ByVal value As Integer)
            MaxViewTimes_ = value
        End Set
    End Property
    Public Property MaxPrintTimes() As Integer
        Get
            Return MaxPrintTimes_
        End Get
        Set(ByVal value As Integer)
            MaxPrintTimes_ = value
        End Set
    End Property
    Public Property MaxViewTime() As Integer
        Get
            Return MaxViewTime_
        End Get
        Set(ByVal value As Integer)
            MaxViewTime_ = value
        End Set
    End Property
    Public Property ExpireAfter() As Integer
        Get
            Return ExpireAfter_
        End Get
        Set(ByVal value As Integer)
            ExpireAfter_ = value
        End Set
    End Property
    Public Property ViewDateFrom() As Date
        Get
            Return ViewDateFrom_
        End Get
        Set(ByVal value As Date)
            ViewDateFrom_ = value
        End Set
    End Property
    Public Property ViewDateTo() As Date
        Get
            Return ViewDateTo_
        End Get
        Set(ByVal value As Date)
            ViewDateTo_ = value
        End Set
    End Property
    Public Property ViewTimeFrom() As Date
        Get
            Return ViewTimeFrom_
        End Get
        Set(ByVal value As Date)
            ViewTimeFrom_ = value
        End Set
    End Property
    Public Property ViewTimeTo() As Date
        Get
            Return ViewTimeTo_
        End Get
        Set(ByVal value As Date)
            ViewTimeTo_ = value
        End Set
    End Property
    Public Property ViewUserNames() As Boolean
        Get
            Return ViewUserNames_
        End Get
        Set(ByVal value As Boolean)
            ViewUserNames_ = value
        End Set
    End Property
    Public Property ViewDomainNames() As Boolean
        Get
            Return ViewDomainNames_
        End Get
        Set(ByVal value As Boolean)
            ViewDomainNames_ = value
        End Set
    End Property
    Public Property ViewComputerNames() As Boolean
        Get
            Return ViewComputerNames_
        End Get
        Set(ByVal value As Boolean)
            ViewComputerNames_ = value
        End Set
    End Property
    Public Property ViewMachineSignatures() As Boolean
        Get
            Return ViewMachineSignatures_
        End Get
        Set(ByVal value As Boolean)
            ViewMachineSignatures_ = value
        End Set
    End Property
    Public Property MessageOnStart() As String
        Get
            Return MessageOnStart_
        End Get
        Set(ByVal value As String)
            MessageOnStart_ = value
        End Set
    End Property
    Public Property MessageOnExit() As String
        Get
            Return MessageOnExit_
        End Get
        Set(ByVal value As String)
            MessageOnExit_ = value
        End Set
    End Property
    Public Property AboutBoxText() As String
        Get
            Return AboutBoxText_
        End Get
        Set(ByVal value As String)
            AboutBoxText_ = value
        End Set
    End Property
    Public Property WindowTitle() As String
        Get
            Return WindowTitle_
        End Get
        Set(ByVal value As String)
            WindowTitle_ = value
        End Set
    End Property
    Public Property HasSplashScreen() As Boolean
        Get
            Return HasSplashScreen_
        End Get
        Set(ByVal value As Boolean)
            HasSplashScreen_ = value
        End Set
    End Property
    Public Property SplashScreenText() As String
        Get
            Return SplashScreenText_
        End Get
        Set(ByVal value As String)
            SplashScreenText_ = value
        End Set
    End Property
    Public Property SplashScreenTextColor() As Color
        Get
            Return SplashScreenTextColor_
        End Get
        Set(ByVal value As Color)
            SplashScreenTextColor_ = value
        End Set
    End Property
    Public Property SplashScreenBackColor() As Color
        Get
            Return SplashScreenBackColor_
        End Get
        Set(ByVal value As Color)
            SplashScreenBackColor_ = value
        End Set
    End Property
    Public Property AllowDrawing() As Boolean
        Get
            Return AllowDrawing_
        End Get
        Set(ByVal value As Boolean)
            AllowDrawing_ = value
        End Set
    End Property
    Public Property AllowExportImages() As Boolean
        Get
            Return AllowExportImages_
        End Get
        Set(ByVal value As Boolean)
            AllowExportImages_ = value
        End Set
    End Property
    Public Property AllowPrinting() As Boolean
        Get
            Return AllowPrinting_
        End Get
        Set(ByVal value As Boolean)
            AllowPrinting_ = value
        End Set
    End Property
    Public Property AllowSaveImage() As Boolean
        Get
            Return AllowSaveImage_
        End Get
        Set(ByVal value As Boolean)
            AllowSaveImage_ = value
        End Set
    End Property
    Public Property AllowViewDocumentProperties() As Boolean
        Get
            Return AllowViewDocumentProperties_
        End Get
        Set(ByVal value As Boolean)
            AllowViewDocumentProperties_ = value
        End Set
    End Property
    Public Property AllowFullscreen() As Boolean
        Get
            Return AllowFullscreen_
        End Get
        Set(ByVal value As Boolean)
            AllowFullscreen_ = value
        End Set
    End Property

    Public Property HasViewTimeTo() As Boolean
        Get
            Return HasViewTimeTo_
        End Get
        Set(ByVal value As Boolean)
            HasViewTimeTo_ = value
        End Set
    End Property
    Public Property HasViewTimeFrom() As Boolean
        Get
            Return HasViewTimeFrom_
        End Get
        Set(ByVal value As Boolean)
            HasViewTimeFrom_ = value
        End Set
    End Property
    Public Property HasViewDateTo() As Boolean
        Get
            Return HasViewDateTo_
        End Get
        Set(ByVal value As Boolean)
            HasViewDateTo_ = value
        End Set
    End Property
    Public Property HasViewDateFrom() As Boolean
        Get
            Return HasViewDateFrom_
        End Get
        Set(ByVal value As Boolean)
            HasViewDateFrom_ = value
        End Set
    End Property
    Public Property ExpireAfterFirstViewTimeSpan() As TimeSpan
        Get
            Return expireAfterFirstViewTimeSpan_
        End Get
        Set(ByVal value As TimeSpan)
            expireAfterFirstViewTimeSpan_ = value
        End Set
    End Property
    Public Property ExpiresAfterFirstView() As Boolean
        Get
            Return expiresAfterFirstView_
        End Get
        Set(ByVal value As Boolean)
            expiresAfterFirstView_ = value
        End Set
    End Property
    Public Property MaxViewTimeTimeSpan() As TimeSpan
        Get
            Return maxViewTimeTimeSpan_
        End Get
        Set(ByVal value As TimeSpan)
            maxViewTimeTimeSpan_ = value
        End Set
    End Property
    Public Property MaxViewTimeSecs() As Integer
        Get
            Return maxViewTimeSecs_
        End Get
        Set(ByVal value As Integer)
            maxViewTimeSecs_ = value
        End Set
    End Property
End Class
