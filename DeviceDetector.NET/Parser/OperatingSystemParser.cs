using System.Collections.Generic;
using System.Linq;
using DeviceDetectorNET.Class;
using DeviceDetectorNET.Results;

namespace DeviceDetectorNET.Parser
{
    public class OperatingSystemParser : ParserAbstract<List<Os>, OsMatchResult>
    {

        private const string Unknown = "Unknown";
        private const string UnknownShort = "UNK";
        /// <summary>
        /// Known operating systems mapped to their internal short codes
        /// </summary>
        protected static readonly Dictionary<string,string> OperatingSystems = new Dictionary<string, string>()
        {
            {"AIX", "AIX"},
            {"AND", "Android"},
            {"AMG", "AmigaOS"},
            {"ATV", "Apple TV"},
            {"ARL", "Arch Linux"},
            {"BTR", "BackTrack"},
            {"SBA", "Bada"},
            {"BEO", "BeOS"},
            {"BLB", "BlackBerry OS"},
            {"QNX", "BlackBerry Tablet OS"},
            {"BMP", "Brew"},
            {"CAI", "Caixa M�gica"},
            {"CES", "CentOS"},
            {"COS", "Chrome OS"},
            {"CYN", "CyanogenMod"},
            {"DEB", "Debian"},
            {"DEE", "Deepin"},
            {"DFB", "DragonFly"},
            {"DVK", "DVKBuntu"},
            {"FED", "Fedora"},
            {"FEN", "Fenix"},
            {"FOS", "Firefox OS"},
            {"FIR", "Fire OS"},
            {"FRE", "Freebox"},
            {"BSD", "FreeBSD"},
            {"FYD", "FydeOS"},
            {"GNT", "Gentoo"},
            {"GRI", "GridOS"},
            {"GTV", "Google TV"},
            {"HPX", "HP-UX"},
            {"HAI", "Haiku OS"},
            {"IPA", "iPadOS"},
            {"HAS", "HasCodingOS"},
            {"IRI", "IRIX"},
            {"INF", "Inferno"},
            {"KOS", "KaiOS"},
            {"KNO", "Knoppix"},
            {"KBT", "Kubuntu"},
            {"LIN", "GNU/Linux"},
            {"LBT", "Lubuntu"},
            {"LOS", "Lumin OS"},
            {"VLN", "VectorLinux"},
            {"MAC", "Mac"},
            {"MAE", "Maemo"},
            {"MAG", "Mageia"},
            {"MDR", "Mandriva"},
            {"SMG", "MeeGo"},
            {"MCD", "MocorDroid"},
            {"MIN", "Mint"},
            {"MLD", "MildWild"},
            {"MOR", "MorphOS"},
            {"NBS", "NetBSD"},
            {"MTK", "MTK / Nucleus"},
            {"MRE", "MRE"},
            {"WII", "Nintendo"},
            {"NDS", "Nintendo Mobile"},
            {"OS2", "OS/2"},
            {"T64", "OSF1"},
            {"OBS", "OpenBSD"},
            {"ORD", "Ordissimo"},
            {"PCL", "PCLinuxOS"},
            {"PSP", "PlayStation Portable"},
            {"PS3", "PlayStation"},
            {"RHT", "Red Hat"},
            {"ROS", "RISC OS"},
            {"RSO", "Rosa"},
            {"REM", "Remix OS"},
            {"RZD", "RazoDroiD"},
            {"SAB", "Sabayon"},
            {"SSE", "SUSE"},
            {"SAF", "Sailfish OS"},
            {"SEE", "SeewoOS"},
            {"SLW", "Slackware"},
            {"SOS", "Solaris"},
            {"SYL", "Syllable"},
            {"SYM", "Symbian"},
            {"SYS", "Symbian OS"},
            {"S40", "Symbian OS Series 40"},
            {"S60", "Symbian OS Series 60"},
            {"SY3", "Symbian^3"},
            {"TDX", "ThreadX"},
            {"TIZ", "Tizen"},
            {"TOS", "TmaxOS"},
            {"UBT", "Ubuntu"},
            {"WAS", "watchOS"},
            {"WTV", "WebTV"},
            {"WHS", "Whale OS"},
            {"WIN", "Windows"},
            {"WCE", "Windows CE"},
            {"WIO", "Windows IoT"},
            {"WMO", "Windows Mobile"},
            {"WPH", "Windows Phone"},
            {"WRT", "Windows RT"},
            {"XBX", "Xbox"},
            {"XBT", "Xubuntu"},
            {"YNS", "YunOs"},
            {"IOS", "iOS"},
            {"POS", "palmOS"},
            {"WOS", "webOS"},
        };

        /// <summary>
        /// Operating system families mapped to the short codes of the associated operating systems
        /// </summary>
        protected static readonly Dictionary<string, string[]> OsFamilies = new Dictionary<string, string[]>
        {
            {"Android"              , new [] {"AND", "CYN", "FIR", "REM", "RZD", "MLD", "MCD", "YNS", "GRI"}},
            {"AmigaOS"              , new [] {"AMG", "MOR"}},
            {"Apple TV"             , new [] {"ATV"}},
            {"BlackBerry"           , new [] {"BLB", "QNX"}},
            {"Brew"                 , new [] {"BMP"}},
            {"BeOS"                 , new [] {"BEO", "HAI"}},
            {"Chrome OS"            , new [] {"COS", "FYD", "SEE"}},
            {"Firefox OS"           , new [] {"FOS", "KOS"}},
            {"Gaming Console"       , new [] {"WII", "PS3"}},
            {"Google TV"            , new [] {"GTV"}},
            {"IBM"                  , new [] {"OS2"}},
            {"iOS"                  , new [] {"IOS", "WAS", "IPA"}},
            {"RISC OS"              , new [] {"ROS"}},
            {"GNU/Linux"            , new [] {"LIN", "ARL", "DEB", "KNO", "MIN", "UBT", "KBT", "XBT", "LBT", "FED",
                                                "RHT", "VLN", "MDR", "GNT", "SAB", "SLW", "SSE", "CES", "BTR", "SAF",
                                                "ORD", "TOS", "RSO", "DEE", "FRE", "MAG", "FEN", "CAI", "PCL", "HAS",
                                                "LOS", "DVK"}},
            {"Mac"                  , new [] {"MAC"}},
            {"Mobile Gaming Console", new [] {"PSP", "NDS", "XBX"}},
            {"Real-time OS"         , new [] {"MTK", "TDX", "MRE"}},
            {"Other Mobile"         , new [] {"WOS", "POS", "SBA", "TIZ", "SMG", "MAE"}},
            {"Symbian"              , new [] {"SYM", "SYS", "SY3", "S60", "S40"}},
            {"Unix"                 , new [] {"SOS", "AIX", "HPX", "BSD", "NBS", "OBS", "DFB", "SYL", "IRI", "T64", "INF"}},
            {"WebTV"                , new [] {"WTV"}},
            {"Windows"              , new [] {"WIN"}},
            {"Windows Mobile"       , new [] {"WPH", "WMO", "WCE", "WRT", "WIO"}},
            {"Other Smart TV"       , new [] {"WHS"}}
        };

        /// <summary>
        /// Returns all available operating systems
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAvailableOperatingSystems()
        {
            return OperatingSystems;
        }

        /// <summary>
        /// Returns all available operating system families
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string[]> GetAvailableOperatingSystemFamilies()
        {
            return OsFamilies;
        }

        public OperatingSystemParser()
        {
            FixtureFile = "regexes/oss.yml";
            ParserName = "os";
            regexList = GetRegexes();
        }

        public override ParseResult<OsMatchResult> Parse()
        {
            var result = new ParseResult<OsMatchResult>();
            Os localOs = null;

            string[] localMatches = null;

            foreach (var os in regexList)
            {
                var matches = MatchUserAgent(os.Regex);
                if (matches.Length > 0)
                {
                    if (os.Versions.Any())
                    {
                        foreach (var version in os.Versions)
                        {
                            if (IsMatchUserAgent(version.Regex))
                            {
                                os.Version = version.Version;
                                break;
                            }
                        }
                    }
                    localOs = os;
                    localMatches = matches;
                    break;
                }
            }

            if (localMatches != null)
            {
                var name = BuildByMatch(localOs.Name, localMatches);
                var @short = UnknownShort;
                foreach (var operatingSystem in OperatingSystems)
                {
                    if (operatingSystem.Value.ToLower().Equals(name.ToLower()))
                    {
                        name = operatingSystem.Value;
                        @short = operatingSystem.Key;
                    }
                }
                var os = new OsMatchResult
                {
                    Name = name,
                    ShortName = @short,
                    Version = BuildVersion(localOs.Version, localMatches),
                    Platform = ParsePlatform()
                };

                if (OperatingSystems.ContainsKey(name))
                {
                    os.ShortName = OperatingSystems.Keys.FirstOrDefault(o=>o.Equals(name));
                }
                if (OperatingSystems.ContainsValue(name))
                {
                    os.Name = OperatingSystems.Values.FirstOrDefault(o => o.Equals(name));
                }

                result.Add(os);

            }
            return result;
        }

        protected string ParsePlatform()
        {
            if (IsMatchUserAgent("arm")) {
                return PlatformType.ARM;
            }
            if(IsMatchUserAgent("WOW64|x64|win64|amd64|x86_64")) {
                return PlatformType.X64;
            }
            return IsMatchUserAgent("i[0-9]86|i86pc") ? PlatformType.X86 : PlatformType.NONE;
        }

        /// <summary>
        /// Returns the operating system family for the given operating system
        /// </summary>
        /// <param name="osLabel"></param>
        /// <param name="name"></param>
        /// <returns>bool|string If false, <see cref="Unknown"/></returns>
        public static bool GetOsFamily(string osLabel, out string name) //TryGetBrowserFamily
        {
            foreach (var family in OsFamilies)
            {
                if (!family.Value.Contains(osLabel)) continue;
                name = family.Key;
                return true;
            }
            name = Unknown;
            return false;
        }

        /// <summary>
        /// Returns the full name for the given short name
        /// </summary>
        /// <param name="os"></param>
        /// <param name="ver"></param>
        /// <returns></returns>
        public static string GetNameFromId(string os, string ver = "")
        {
            if (!OperatingSystems.ContainsKey(os)) return string.Empty;
            var osFullName = OperatingSystems[os];
            return (osFullName + " " + ver).Trim();
        }
    }
}