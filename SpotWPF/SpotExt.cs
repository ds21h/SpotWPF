using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usenet.Nntp;
using Usenet.Nntp.Models;
using Usenet.Nntp.Responses;
using System.Collections.Immutable;
using System.Xml;
using System.IO;

namespace SpotWPF {
    internal class SpotExt : SpotData {
        private const string cHtmlStart = "<!DOCTYPE html><html><head><meta charset = \"UTF-8\"></head><body>";
        private const string cHtmlEnd = "</body></html>";
        private string mDescription;
        private string mDescrHtml;
        private string mWebsite;
        private int mKey;
        private int mImageWidth;
        private int mImageHeight;
        private string mImageSegment;
        private string mNzbSegment;

        internal SpotExt(SpotData pSpot) : base(pSpot) {
            mDescription = "";
        }

        internal string xDescription {
            get {
                return mDescription;
            }
        }

        internal string xDescHtml {
            get {
                return mDescrHtml;
            }
        }

        internal async Task xInit() {
            sGetSpot();
            sConvertDesc();
        }

        private void sGetSpot() {
            NntpResponse lResponse;
            NntpArticleResponse lArticleResponse;
            NntpClient lClient = new NntpClient(new NntpConnection());

            if (lClient.Connect(Global.gServer.xReader, Global.gServer.xPort, Global.gServer.xSSL)) {
                if (lClient.Authenticate(Global.gServer.xUserId, Global.gServer.xPassWord)) {
                    lArticleResponse = lClient.Head(new NntpMessageId(xArticleId));
                    if (lArticleResponse.Success) {
                        sProcessArticle(lArticleResponse.Article);
                    }
                    lResponse = lClient.Quit();
                }
            }
        }

        private void sProcessArticle(NntpArticle pArticle) {
            ImmutableDictionary<string, ImmutableHashSet<string>> lHeaders;
            ImmutableHashSet<string> lHValues;
            string lXml;

            lHeaders = pArticle.Headers;
            if (lHeaders.TryGetValue("X-XML", out lHValues)) {
                lXml = sCombineXml(lHValues);
                sParseXml(lXml);
            }
        }

        private string sCombineXml(ImmutableHashSet<string> pXmlIn) {
            StringBuilder pResult = new StringBuilder();
            string[] lParts;
            string lPart;
            int lIndex;
            bool lLineOK = false;

            lParts = new string[pXmlIn.Count];
            foreach (var bXml in pXmlIn) {
                lLineOK = false;
                lPart = bXml;
                if (lPart != null) {
                    if (lPart[0] == '(') {
                        if (lPart[2] == ')') {
                            if (int.TryParse(lPart.Substring(1, 1), out lIndex)) {
                                if (lIndex < lParts.Length) {
                                    lParts[lIndex] = lPart.Substring(3);
                                    lLineOK = true;
                                }
                            }
                        } else {
                            if (lPart[3] == ')') {
                                if (int.TryParse(lPart.Substring(1, 2), out lIndex)) {
                                    if (lIndex < lParts.Length) {
                                        lParts[lIndex] = lPart.Substring(4);
                                        lLineOK = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if (!lLineOK) {
                    break;
                }
            }
            if (lLineOK) {
                for (lIndex = 0; lIndex < lParts.Length; lIndex++) {
                    pResult.Append(lParts[lIndex]);
                }
                return pResult.ToString();
            } else {
                return null;
            }
        }

        private void sParseXml(string pXml) {
            XmlReader lReader;
            int lLevel;
            string[] lElement;
            int lTempInt;

            lReader = XmlReader.Create(new StringReader(pXml));
            lElement = new string[5];
            lLevel = -1;
            while (lReader.Read()) {
                switch (lReader.NodeType) {
                    case XmlNodeType.Element: {
                            lLevel++;
                            lElement[lLevel] = lReader.Name;
                            if (lReader.Name == "Image") {
                                while (lReader.MoveToNextAttribute()) {
                                    if (lReader.Name == "Width") {
                                        if (int.TryParse(lReader.Value, out lTempInt)) {
                                            mImageWidth = lTempInt;
                                        }
                                    } else {
                                        if (lReader.Name == "Height") {
                                            if (int.TryParse(lReader.Value, out lTempInt)) {
                                                mImageHeight = lTempInt;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Text: {
                            switch (lElement[lLevel]) {
                                case "Key": {
                                        if (int.TryParse(lReader.Value, out lTempInt)) {
                                            mKey = lTempInt;
                                        }
                                        break;
                                    }
                                case "Poster": {
                                        break;
                                    }
                                case "Title": {
                                        break;
                                    }
                                case "Tag": {
                                        break;
                                    }
                                case "Created": {
                                        break;
                                    }
                                case "Size": {
                                        break;
                                    }
                                case "Description": {
                                        mDescription = lReader.Value;
                                        break;
                                    }
                                case "Website": {
                                        mWebsite = lReader.Value;
                                        break;
                                    }
                                case "Category": {
                                        break;
                                    }
                                case "Sub": {
                                        break;
                                    }
                                case "Segment": {
                                        if (lElement[lLevel - 1] == "Image") {
                                            mImageSegment = lReader.Value;
                                        } else {
                                            if (lElement[lLevel - 1] == "NZB") {
                                                mNzbSegment = lReader.Value;
                                            }
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                    case XmlNodeType.EndElement: {
                            lLevel--;
                            break;
                        }
                }
            }
        }

        private void sConvertDesc() {
//            mDescrHtml = cHtmlStart + mDescription.Replace("[br]", "<br>", StringComparison.OrdinalIgnoreCase).Replace("[b]", "<b>", StringComparison.OrdinalIgnoreCase).Replace("[/b]", "</b>", StringComparison.OrdinalIgnoreCase) + cHtmlEnd;
            mDescrHtml = cHtmlStart + mDescription.Replace("[", "<", StringComparison.Ordinal).Replace("]", ">", StringComparison.Ordinal) + cHtmlEnd;
        }
    }
}
