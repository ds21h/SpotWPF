using System;
using NntpBase.Nntp;

namespace SpotWPF {
    internal class XoverCommentsProcessor : IMultiLineProcess {
        private readonly DateTimeOffset mMinDate;
        Data mData = null;

        internal XoverCommentsProcessor() {
            mMinDate = DateTimeOffset.Now.AddDays(-Global.cMaxAge);
        }

        public void xStartProcess() {
            mData = Data.getInstance;
        }

        public void xProcessLine(string pLine) {
            string[] lHeaders;
            string[] lReferences;
            string lDateString;
            DateTimeOffset lDate;
            int lIndex;
            long lArticleNumber;

            lHeaders = pLine.Split('\t');
            if (lHeaders.Length >= 6) {
                if ((lHeaders[5].Length > Global.cMessageIdSuffix.Length) && (lHeaders[5].Substring(lHeaders[5].Length - Global.cMessageIdSuffix.Length) == Global.cMessageIdSuffix)) {
                    lDateString = lHeaders[3];
                    if (lDateString.Substring(lDateString.Length - 1) == ")") {
                        lIndex = lDateString.IndexOf('(');
                        if (lIndex > 0) {
                            lDateString = lDateString.Substring(0, lIndex);
                        }
                    }
                    if (DateTimeOffset.TryParse(lDateString, out lDate)) {
                        lDate = lDate.ToLocalTime();
                        if (lDate.CompareTo(mMinDate) > 0) {
                            lReferences = lHeaders[5].Split(" ");
                            if (lReferences.Length == 1) {
                                if (long.TryParse(lHeaders[0], out lArticleNumber)) {
                                    mData.xStoreComment(lArticleNumber, lHeaders[5], lDate.DateTime);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void xEndProcess() {
        }
    } 
}
