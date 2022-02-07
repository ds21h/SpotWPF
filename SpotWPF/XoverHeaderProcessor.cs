using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class XoverHeaderProcessor : IMultiLineProcess {
        //        private StreamWriter mStream;
        Data mData;

        internal XoverHeaderProcessor() {
        }

        public void xStartProcess() {
            //            mStream = new StreamWriter(@"E:\Test\Spotz\TestXover.txt", false);
            mData = Data.getInstance;
        }

        public void xProcessLine(string pLine) {
            int lStart;
            int lEnd;
            int lCount;
            SpotData lSpot;
            bool lSpotOK;
            string lHeader;

            lStart = 0;
            lCount = 0;
            lSpot = new SpotData();
            lSpotOK = false;
            do {
                if (lStart >= pLine.Length) {
                    break;
                }
                lEnd = pLine.IndexOf('\t', lStart);
                if (lEnd == -1) {
                    lHeader = pLine.Substring(lStart);
                } else {
                    lHeader = pLine.Substring(lStart, lEnd - lStart);
                }
                if (sProcessHeader(lHeader, lCount, lSpot)) {
                    lSpotOK = true;
                } else {
                    lSpotOK = false;
                    break;
                }
                lStart = lEnd + 1;
                lCount++;
            } while (lEnd >= 0);
            if (lSpotOK) {
                mData.xStoreSpot(lSpot);
            }
        }

        private bool sProcessHeader(string pHeader, int pSeq, SpotData pSpot) {
            long lTempLong;
            int lTempInt;
            string lTempString;
            int lStart;
            int lEnd;
            bool lResult;

            lResult = false;
            switch (pSeq) {
                case 0: {
                        if (long.TryParse(pHeader, out lTempLong)) {
                            pSpot.xArticleNumber = lTempLong;
                            lResult = true;
                        }
                        break;
                    }
                case 1: {
                        lStart = pHeader.IndexOf('|');
                        if (lStart == -1) {
                            pSpot.xTitle = pHeader.Trim();
                        } else {
                            pSpot.xTitle = pHeader.Substring(0, lStart).Trim();
                            pSpot.xTag = pHeader.Substring(lStart + 1).Trim();
                        }
                        lResult = true;
                        break;
                    }
                case 2: {
                        lStart = pHeader.IndexOf('<');
                        if (lStart > 1) {
                            pSpot.xPoster = pHeader.Substring(0, lStart).Trim();
                            if (pHeader.Length > lStart + 1) {
                                lStart = pHeader.IndexOf('@', lStart + 1);
                                if (lStart > 0) {
                                    if (pHeader.Length > lStart + 1) {
                                        lEnd = pHeader.IndexOf('.', lStart + 1);
                                        lStart++;
                                        if ((lEnd - lStart) >= 2) {
                                            if (int.TryParse(pHeader.Substring(lStart, 1), out lTempInt)) {
                                                pSpot.xCategory = lTempInt;
                                                lStart += 2;
                                                if ((lEnd - lStart) % 3 == 0) {
                                                    while (lStart < lEnd) {
                                                        pSpot.xAddSubCategory(pHeader.Substring(lStart, 3));
                                                        lStart += 3;
                                                    }
                                                    lStart++;
                                                    if (pHeader.Length > lStart) {
                                                        lEnd = pHeader.IndexOf('.', lStart);
                                                        if (lEnd > 0) {
                                                            if (long.TryParse(pHeader.Substring(lStart, lEnd - lStart), out lTempLong)) {
                                                                pSpot.xSize = lTempLong;
                                                                if (pHeader.Length > lEnd + 1) {
                                                                    lStart = lEnd + 1;
                                                                    lEnd = pHeader.IndexOf('.', lStart);
                                                                    if (pHeader.Length > lEnd + 1) {
                                                                        lStart = lEnd + 1;
                                                                        lEnd = pHeader.IndexOf('.', lStart);
                                                                        if (lEnd > 0) {
                                                                            if (long.TryParse(pHeader.Substring(lStart, lEnd - lStart), out lTempLong)) {
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
                        break;
                    }
                case 4: {
                        if (!string.IsNullOrEmpty(pHeader)) {
                            lTempString = pHeader.TrimStart('<').TrimEnd('>').Trim();
                            if (!string.IsNullOrEmpty(lTempString)) {
                                pSpot.xArticleId = lTempString;
                                lResult = true;
                            }
                        }
                        break;
                    }
                default: {
                        lResult = true;
                        break;
                    }
            }
            return lResult;
        }

        public void xEndProcess() {
            //            mStream.Close();
        }
    }
}
