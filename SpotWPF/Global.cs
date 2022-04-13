using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal static class Global {
        internal const string cHomeDir = @"E:\Test\Spotz";
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
    }
}
