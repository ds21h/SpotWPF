using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class SpotPage {
        private const string cFontSize = "18";
        private SpotExt mSpot;

        internal SpotPage(SpotExt pSpot) {
            mSpot = pSpot;
        }

        internal async Task<string> xAsyncCreatePage() {
            string lHtml;
            string lInfo;

            if (string.IsNullOrEmpty(Global.gSpotBase)) {
                await sGetHtmlBase();
            }
            lInfo = sMakeInfo();
            lHtml = Global.gSpotBase.Replace("[SN:TITLE]", mSpot.xTitle).Replace("[SN:DESC]", mSpot.xDescription).Replace("[SN:FONTSIZE]", cFontSize).Replace("[SN:PATH]", Global.cHomeDir).Replace("[SN:INFO]", lInfo);

            return lHtml;
        }

        private async Task sGetHtmlBase() {
            StreamReader lReader;

            try {
                lReader = new StreamReader(Global.cHomeDir + @"\" + Global.cSpotBase);
                Global.gSpotBase = await lReader.ReadToEndAsync().ConfigureAwait(false);
            } catch (Exception) {
                Global.gSpotBase = string.Empty;
            }
       }

        private string sMakeInfo() {
            StringBuilder lInfo;
            bool lFirst;

            lInfo = new StringBuilder();
            lInfo.Append("<TABLE BORDER=0>");
            lInfo.Append("<TR><TD><b>Categorie:&nbsp;<b></TD><TD>" + SpotCoding.xCategory(mSpot.xCategory) + "</TD></TR>");
            switch (mSpot.xCategory) {
                default: {
                        lInfo.Append("<TR><TD><b>Formaat:&nbsp;<b></TD><TD>" + mSpot.xFormat + "</TD></TR>");
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubC) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Taal:&nbsp;<b></TD><TD>" + SpotCoding.xLanguage(mSpot.xCategory, bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xLanguage(mSpot.xCategory, bSCat) + "</TD></TR>");
                            }
                        }
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubD) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Genre:&nbsp;<b></TD><TD>" + SpotCoding.xTypeImage(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xTypeImage(bSCat) + "</TD></TR>");
                            }
                        }
                        break;
                    }
                case 2: {
                        lInfo.Append("<TR><TD><b>Formaat:&nbsp;<b></TD><TD>" + mSpot.xFormat + "</TD></TR>");
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubC) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Bitrate:&nbsp;<b></TD><TD>" + SpotCoding.xBitrate(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xBitrate(bSCat) + "</TD></TR>");
                            }
                        }
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubD) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Genre:&nbsp;<b></TD><TD>" + SpotCoding.xTypeSound(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xTypeSound(bSCat) + "</TD></TR>");
                            }
                        }
                        break;
                    }
                case 3: {
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubB) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Formaat:&nbsp;<b></TD><TD>" + SpotCoding.xGameSource(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xGameSource(bSCat) + "</TD></TR>");
                            }
                        }
                        lInfo.Append("<TR><TD><b>Platform:&nbsp;<b></TD><TD>" + SpotCoding.xFormat(mSpot.xCategory, mSpot.xSubA) + "</TD></TR>");
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubC) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Genre:&nbsp;<b></TD><TD>" + SpotCoding.xTypeGames(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xTypeGames(bSCat) + "</TD></TR>");
                            }
                        }
                        break;
                    }
                case 4: {
                        lInfo.Append("<TR><TD><b>Platform:&nbsp;<b></TD><TD>" + SpotCoding.xFormat(mSpot.xCategory, mSpot.xSubA) + "</TD></TR>");
                        lFirst = true;
                        foreach (var bSCat in mSpot.xSubB) {
                            if (lFirst) {
                                lInfo.Append("<TR><TD><b>Genre:&nbsp;<b></TD><TD>" + SpotCoding.xTypeApp(bSCat) + "</TD></TR>");
                                lFirst = false;
                            } else {
                                lInfo.Append("<TR><TD><b>&nbsp;&nbsp;<b></TD><TD>" + SpotCoding.xTypeApp(bSCat) + "</TD></TR>");
                            }
                        }
                        break;
                    }
            }
            lInfo.Append("<TR><TD><b>Omvang:&nbsp;<b></TD><TD>" + mSpot.xSizeStr + "</TD></TR>");
            lInfo.Append("<TR><TD><b>Spotter:&nbsp;<b></TD><TD>" + mSpot.xPoster + "</TD></TR>");
            lInfo.Append("<TR><TD><b>Moment:&nbsp;<b></TD><TD>" + mSpot.xCreatedLocalString + "</TD></TR>");

            lInfo.Append("</TABLE>");
            return lInfo.ToString();
        }
    }
}
