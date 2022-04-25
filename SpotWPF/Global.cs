using System.IO;

namespace SpotWPF {
    internal static class Global {
        internal const string cTestHomeDir = @"E:\Test\Spotz";
        internal static readonly string cHomeDir;
        internal const string cTempDir = "Temp";
        internal const string cSmileyDir = @"Images\Smileys";
        internal const string cSpotBase = "SpotView.htm";
        internal const string cCommentsFormat = "Comments-format.xml";
        internal const string cSpotGroup = "free.pt";
        internal const string cCommentGroup = "free.usenet";
        internal const string cMessageIdSuffix = "@spot.net>";
        internal const int cMaxAge = 1500;
        internal static Server gServer = null;
        internal const string cCommentBase = "CommentEntry.htm";
        internal static string gCommentBase = "";
        internal static string gSpotBase = "";
        internal static string gBaseError = "<!DOCTYPE HTML><HTML><BODY><br><b>HTML base file missing!!!!</b><BR><BR></BODY></HTML>";

        static Global() {
            string lHomeDir;

            lHomeDir = Directory.GetCurrentDirectory();
            if (lHomeDir.StartsWith(@"D:\Source")) {
                cHomeDir = cTestHomeDir; 
            } else {
                cHomeDir = lHomeDir;
            }
        }
    }
}
