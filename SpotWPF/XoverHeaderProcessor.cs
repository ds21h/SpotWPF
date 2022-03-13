using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class XoverHeaderProcessor : IMultiLineProcess {
        //        private StreamWriter mStream;
        Data mData;
        private DateTimeOffset mMinDate;

        internal XoverHeaderProcessor() {
        }

        public void xStartProcess() {
            //            mStream = new StreamWriter(@"E:\Test\Spotz\TestXover.txt", false);
            mData = Data.getInstance;
            mMinDate = DateTimeOffset.Now.AddDays(-Global.cMaxAge);
        }

        public void xProcessLine(string pLine) {
            SpotData lSpot;
            string[] lHeaders;

            lSpot = new SpotData();
            lHeaders = pLine.Split('\t');
            if (lHeaders.Length >= 8) {
                if (sCheckDate(lHeaders[3])) {
                    if (sProcessArticleNumber(lHeaders[0], lSpot)) {
                        if (sProcessSubject(lHeaders[1], lSpot)) {
                            if (sProcessAuthor(lHeaders[2], lSpot)) {
                                if (sProcessMessageId(lHeaders[4], lSpot)) {
                                    mData.xStoreSpot(lSpot);
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool sCheckDate(string pDate) {
            bool lResult;
            DateTimeOffset lDate;

            lResult = false;
            if (DateTimeOffset.TryParse(pDate, out lDate)) {
                lDate = lDate.ToLocalTime();
                if (lDate.CompareTo(mMinDate) > 0) {
                    lResult = true;
                }
            }
            return lResult;
        }

        private bool sProcessArticleNumber(string pArticleNumber, SpotData pSpot) {
            long lTempLong;
            bool lResult;

            lResult=false;
            if (long.TryParse(pArticleNumber, out lTempLong)) {
                pSpot.xArticleNumber = lTempLong;
                lResult = true;
            }
            return lResult;
        }

        private bool sProcessSubject(string pSubject, SpotData pSpot) {
            int lStart;
            bool lResult;

            lStart = pSubject.IndexOf('|');
            if (lStart == -1) {
                pSpot.xTitle = pSubject.Trim();
            } else {
                pSpot.xTitle = pSubject.Substring(0, lStart).Trim();
                pSpot.xTag = pSubject.Substring(lStart + 1).Trim();
            }
            lResult = true;
            return lResult;
        }

        private bool sProcessAuthor(string pAuthor, SpotData pSpot) {
            int lStart;
            int lEnd;
            int lTempInt;
            long lTempLong;
            bool lResult;

            lResult = false;
            lStart = pAuthor.IndexOf('<');
            if (lStart > 1) {
                pSpot.xPoster = pAuthor.Substring(0, lStart).Trim();
                if (pAuthor.Length > lStart + 1) {
                    lStart = pAuthor.IndexOf('@', lStart + 1);
                    if (lStart > 0) {
                        if (pAuthor.Length > lStart + 1) {
                            lEnd = pAuthor.IndexOf('.', lStart + 1);
                            lStart++;
                            if ((lEnd - lStart) >= 2) {
                                if (int.TryParse(pAuthor.Substring(lStart, 1), out lTempInt)) {
                                    pSpot.xCategory = lTempInt;
                                    lStart += 2;
                                    if ((lEnd - lStart) % 3 == 0) {
                                        while (lStart < lEnd) {
                                            pSpot.xAddSubCategory(pAuthor.Substring(lStart, 3));
                                            lStart += 3;
                                        }
                                        lStart++;
                                        if (pAuthor.Length > lStart) {
                                            lEnd = pAuthor.IndexOf('.', lStart);
                                            if (lEnd > 0) {
                                                if (long.TryParse(pAuthor.Substring(lStart, lEnd - lStart), out lTempLong)) {
                                                    pSpot.xSize = lTempLong;
                                                    if (pAuthor.Length > lEnd + 1) {
                                                        lStart = lEnd + 1;
                                                        lEnd = pAuthor.IndexOf('.', lStart);
                                                        if (pAuthor.Length > lEnd + 1) {
                                                            lStart = lEnd + 1;
                                                            lEnd = pAuthor.IndexOf('.', lStart);
                                                            if (lEnd > 0) {
                                                                if (long.TryParse(pAuthor.Substring(lStart, lEnd - lStart), out lTempLong)) {
                                                                    pSpot.xCreated = DateTimeOffset.FromUnixTimeSeconds(lTempLong).DateTime;
                                                                    lResult = true;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return lResult;
        }

        private bool sProcessMessageId(string pMessageId, SpotData pSpot) {
            string lTempString;
            bool lResult;

            lResult = false;
            if (!string.IsNullOrEmpty(pMessageId)) {
                lTempString = pMessageId.TrimStart('<').TrimEnd('>').Trim();
                if (!string.IsNullOrEmpty(lTempString)) {
                    pSpot.xArticleId = lTempString;
                    lResult = true;
                }
            }
            return lResult;
        }

        public void xEndProcess() {
            //            mStream.Close();
        }
    }
}
