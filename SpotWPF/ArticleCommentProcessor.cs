using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class ArticleCommentProcessor : IMultiLineProcess {
        private bool mHeaders;
        private CommentEntry mComment;
        private StringBuilder mText;

        internal ArticleCommentProcessor(CommentEntry pComment) {
            mComment = pComment;
        }

        public void xStartProcess() {
            mHeaders = true;
            mText = new StringBuilder();
        }

        public void xProcessLine(string pLine) {
            int lIndex;
            DateTimeOffset lDate;

            if (mHeaders) {
                if (pLine.Length == 0) {
                    mHeaders = false;
                } else {
                    if (pLine.Substring(0, 6).ToLower() == "from: ") {
                        lIndex = pLine.IndexOf('<');
                        if (lIndex > 6) {
                            mComment.xAuthor = pLine.Substring(6, lIndex - 6);
                        }
                    } else {
                        if (pLine.Substring(0, 6).ToLower() == "date: ") {
                            if (DateTimeOffset.TryParse(pLine.Substring(6), out lDate)) {
                                mComment.xDate = lDate;
                            }
                        }
                    }
                }
            } else {
                mText.Append(pLine);
                mText.Append("<BR>");
            }
        }

        public void xEndProcess() {
            mComment.xComment = mText.ToString();
        }
    }
}
