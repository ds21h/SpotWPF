using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class CommentEntry {
        private string mAuthor;
        private DateTimeOffset mDate;
        private string mCommentRaw;
        private string mComment;

        internal CommentEntry() {
            mAuthor = string.Empty;
            mDate = DateTimeOffset.MinValue;
            mCommentRaw = string.Empty;
            mComment = string.Empty;
        }

        internal string xAuthor {
            get {
                return mAuthor;
            }
            set {
                mAuthor = value.Trim();
            }
        }

        internal DateTimeOffset xDate {
            set {
                mDate = value;
            }
        }

        public string xDateLocalString {
            get {
                return mDate.ToLocalTime().ToString("ddd d-M-yyyy HH:mm");
            }
        }

        internal string xComment {
            get {
                return mComment;
            }
            set { 
                mCommentRaw = value;
                sProcessRaw();
            }
        }

        private void sProcessRaw() {
            //              < IMG ID = 'DownloadButton' TITLE = 'Downloaden' STYLE = 'float:right; cursor:hand; margin-right:20px; width: 64px; height:64px;' SRC = "[SN:PATH]/Images/download.png" >
            int lStart;
            int lLastStart;
            int lEnd;
            StringBuilder lResult;

            lStart = 0;
            lLastStart = 0;
            lResult = new StringBuilder();
            do {
                lStart = mCommentRaw.IndexOf('[', lLastStart);
                if (lStart < 0) {
                    lResult.Append(mCommentRaw.Substring(lLastStart));
                    lLastStart = mCommentRaw.Length;
                } else {
                    lEnd = mCommentRaw.IndexOf(']', lStart);
                    if (lEnd < 0) {
                        lResult.Append(mCommentRaw.Substring(lLastStart));
                        lLastStart = mCommentRaw.Length;
                    } else {
                        lResult.Append(mCommentRaw.Substring(lLastStart, lStart - lLastStart));
                        lResult.Append(sTransLatePart(mCommentRaw.Substring(lStart, lEnd - lStart + 1)));
                        lStart = lEnd + 1;
                        lLastStart = lStart;
                    }
                }
            } while(lLastStart < mCommentRaw.Length);
            mComment = lResult.ToString();
        }

        private string sTransLatePart(string pIn) {
            string lFileName;
            string lResult;

            lResult = pIn;
            if (pIn.Length > 8) {
                if (pIn.StartsWith("[img=")) {
                    lFileName = pIn.Substring(5, pIn.Length - 6) + ".gif";
                    lResult = "<IMG SRC = \"" + Global.cHomeDir + @"\" + Global.cSmileyDir + @"\" + lFileName + "\">";
                }
            }
            return lResult;
        }
    }
}
