using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FourDepack.ProjectFileTypes
{
    public class Command
    {
        public bool HideWindow = false;
        public bool WaitForExit = false;
        public string Filename = "";
        public string Arguments = "";
    }

    public class XMLLockedDocumentProjectProperties
    {
        public string WindowTitle = "";
        public string UnlockExecuteFilepath = "";
        public string ViewComputerNames = "";
        public DateTime ViewDateFrom = DateTime.Now;
        public DateTime ViewDateTo = DateTime.Now;
        public string ViewDomainNames = "";
        public string ViewMachineSignatures = "";
        public DateTime ViewTimeFrom = DateTime.Now;
        public DateTime ViewTimeTo = DateTime.Now;
        public string ViewUserNames = "";
        public Color SplashTextColor = Color.DarkBlue;
        public string SplashText = "";
        public Color SplashBackColor = Color.LightGray;
        public string sPassword = "";
        public string sName = "";
        public bool RunOnlyAndDelete = false;
        public string MessageOnStart = "";
        public string MessageOnExit = "";
        public TimeSpan MaxViewTimeTimeSpan = default(TimeSpan);
        public int MaxViewTimeSecs = -1;
        public int MaxViewTimes = -1;
        public int MaxPrintTimes = -1;
        public DateTime LastModDate = DateTime.Now;
        public List<string> LangCodes = new List<string>();
        public List<string> LangDesc = new List<string>();
        public bool KeepLastModDate = false;
        public bool KeepCreationDate = false;
        public bool HasViewTimeTo = false;
        public bool HasViewTimeFrom = false;
        public bool HasViewDateTo = false;
        public bool HasViewDateFrom = false;
        public bool HasSplash = false;
        public string GUID = "";
        public bool ExpiresAfterFirstView = false;
        public DateTime ExpireDateAfterFirstView = DateTime.Now;
        public TimeSpan ExpireAfterFirstViewTimeSpan = default(TimeSpan);
        public bool EncryptFiles = false;
        public bool DidRunUnlockExecute = false;
        public DateTime CreationDate = DateTime.Now;
        public bool CheckCRC = false;
        public string AboutBoxText = "";
    }

    public class ZSPProjectPropeties
    {
        public bool Encrypted = false;
        public bool AskForPassword = false;
        public string Password = "";
        public string EncryptionPassword = "";
        public int EncryptionAlgorithm = 0;
        public string UnzipDirectory = "";
        public bool BrowseDirectory = false;
        public bool UnzipAutomatically = false;
        public bool AutoSelectLanguage = false;
        public List<Command> CommandsBefore = new List<Command>();
        public List<Command> CommandsAfter = new List<Command>();
        public List<Shortcut> Shortcuts = new List<Shortcut>();
        public string WindowTitle = "";
        public string MessageBefore = "";
        public string MessageAfter = "";
        public string AboutText = "";
        public string BrandingText = "";
        public string UnzipButtonText = "";
        public string HeaderImage = "";
        public string Icon = "";
        public int PromptType = 0;
    }

    public class XMLProjectProperties
    {
        public string AboutBoxText = "";
        public bool AllowDrawing = false;
        public bool AllowExportImages = false;
        public bool AllowFullscreen = false;
        public bool AllowPrinting = false;
        public bool AllowSaveImage = false;
        public bool AllowViewDocumentProperties = false;
        public bool AskForPassword = false;
        public string Password = "";
        public Color BackgroundColor = Color.Black;
        public string BackgroundMusic = "";
        public bool BackgroundMusicLoop = false;
        public string BackgroundMusicVolume = "";
        public Dictionary<string, string> dictMetadata = new Dictionary<string, string>();
        public decimal EffectDuration = -1m;
        public bool EncryptImages = false;
        public TimeSpan ExpireAfterFirstViewTimeSpan = default(TimeSpan);
        public DateTime ExpireDateAfterFirstView = DateTime.Now;
        public bool ExpiresAfterFirstView = false;
        public bool ExtendSlideshow = false;
        public string GUID = "";
        public bool HasSplash = false;
        public bool HasViewDateFrom = false;
        public bool HasViewDateTo = false;
        public bool HasViewTimeFrom = false;
        public bool HasViewTimeTo = false;
        public int ImageAlign = -1;
        public int ImageTransition = -1;
        public bool LoopSlideshow = false;
        public int MaxPrintTimes = -1;
        public int MaxViewTimes = -1;
        public int MaxViewTimeSecs = -1;
        public TimeSpan MaxViewTimeTimeSpan = default(TimeSpan);
        public string MessageOnExit = "";
        public string MessageOnStart = "";
        public bool ProhibitCopyScreen = true;
        public bool RotateEXIF = false;
        public bool ShowFullScreen = true;
        public SlideshowSizeModeEnum SizeMode = SlideshowSizeModeEnum.Zoom;
        public List<SlideException> SlideExceptions = new List<SlideException>();
        public Color SplashBackColor = Color.LightGray;
        public string SplashText = "";
        public Color SplashTextColor = Color.DarkBlue;
        public decimal StayDuration = -1m;
        public string ViewComputerNames = "";
        public DateTime ViewDateFrom = DateTime.Now;
        public DateTime ViewDateTo = DateTime.Now;
        public string ViewDomainNames = "";
        public string ViewMachineSignatures = "";
        public DateTime ViewTimeFrom = DateTime.Now;
        public DateTime ViewTimeTo = DateTime.Now;
        public string ViewUserNames = "";
        public string WindowTitle = "";
    }

    public class SlideException
    {
        public int SlideNumber = -1;
        public string AudioFilepath = "";
        public decimal AudioDelay = -1;
        public int ImageTransition = -1;
        public decimal StayDuration = -1;
    }

    public enum ImageTransitionEffectEnum
    {
        Crossfade,
        FadeInFadeOut,
        ZoomInZoomOut,
        SlideRandom,
        Curtain,
        HorizontalCurtain,
        InvertedCurtain,
        InvertedHorizontalCurtain,
        SlideLeft,
        SlideRight,
        SlideTop,
        SlideBottom,
        SlideTopLeft,
        SlideBottomRight,
        SlideTopRight,
        SlideBottomLeft,
        ZoomTopLeft,
        ZoomTopRight,
        ZoomTopCenter,
        ZoomBottomLeft,
        ZoomBottomRight,
        ZoomBottomCenter,
        ZoomMiddleLeft,
        ZoomMiddleRight,
        ZoomMiddleCenter,
        ZoomRandom,
        None,
        NoneArrows
    }

    public enum SlideshowSizeModeEnum
    {
        Zoom,
        AsIs,
        Stretch
    }

    public class Shortcut
    {
        public bool FileShortcut = true;
        public string Filename = "";
        public string TargetPath = "";
        public string Description = "";
        public string IconLocation = "";
        public string WorkingDirectory = "";
        public string HotKey = "";
        public string Arguments = "";
        public string URL = "";
    }
}
