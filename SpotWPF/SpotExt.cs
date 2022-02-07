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
using System.IO.Compression;

namespace SpotWPF {
    internal class SpotExt : SpotData {
        private string mDescrBase;
        private string mDescription;
        private string mWebsite;
        private int mKey;
        private int mImageWidth;
        private int mImageHeight;
        private List<string> mImageSegment;
        private List<string> mNzbSegments;
        private string mNzb;
        private string mImageFile;

        internal event EventHandler eNzbUpdated;
        internal SpotExt(SpotData pSpot) : base(pSpot) {
            mDescrBase = "";
            mImageSegment = new List<string>();
            mNzbSegments = new List<string>();
            mNzb = "";
            mImageFile = "";
        }

        internal string xDescription {
            get {
                return mDescription;
            }
        }

        internal string xNzb {
            get {
                return mNzb;
            }
        }

        internal string xImageFile {
            get {
                return mImageFile;
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
                                        mDescrBase = lReader.Value;
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
                                            mImageSegment.Add(lReader.Value);
                                        } else {
                                            if (lElement[lLevel - 1] == "NZB") {
                                                mNzbSegments.Add(lReader.Value);
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
            mDescription = mDescrBase.Replace("[", "<", StringComparison.Ordinal).Replace("]", ">", StringComparison.Ordinal);
        }

        internal async Task xGetNZB() {
            byte[] lLatinNzb;
            MemoryStream lStreamIn;
            DeflateStream lDeflate;

            lLatinNzb = sGetBinary(mNzbSegments);
            lStreamIn = new MemoryStream(lLatinNzb);
            lDeflate = new DeflateStream(lStreamIn, CompressionMode.Decompress);
            mNzb = new StreamReader(lDeflate, Encoding.Latin1).ReadToEnd();
        }

        internal async Task xGetPrv() {
            byte[] lLatinPrv;
            StreamWriter lWriter;
            BinaryWriter lBWriter;
            string lFileName;

            lFileName = Temp.GetTempFileName();
            lLatinPrv = sGetBinary(mImageSegment);
            lWriter = new StreamWriter(lFileName, false, Encoding.GetEncoding(28591));
            lBWriter = new BinaryWriter(lWriter.BaseStream, Encoding.GetEncoding(28591));
            lBWriter.Write(lLatinPrv);
            lWriter.Close();
            mImageFile = lFileName;
        }

        private byte[] sGetBinary(List<string> pSegments) {
            NntpResponse lResponse;
            NntpClient lClient = new NntpClient(new NntpConnection());
            RawProcessor lProcessor;
            StringBuilder lBuilder;
            List<string> lContent;
            string lBasicBinary;
            byte[] lLatinBinary;

            lProcessor = new RawProcessor();
            lBuilder = new StringBuilder();
            if (mNzbSegments.Count > 0) {
                try {
                    if (lClient.Connect(Global.gServer.xReader, Global.gServer.xPort, Global.gServer.xSSL)) {
                        if (lClient.Authenticate(Global.gServer.xUserId, Global.gServer.xPassWord)) {
                            foreach (string bSegment in pSegments) {
                                lResponse = lClient.Body(new NntpMessageId(bSegment), lProcessor);
                                if (lResponse.Success) {
                                    lContent = lProcessor.xLines;
                                    foreach (string bLine in lContent) {
                                        lBuilder.Append(bLine);
                                    }
                                }
                            }
                            lResponse = lClient.Quit();
                        }
                    }
                } catch (Usenet.Exceptions.NntpException pExc) {
                }
            }

            lBasicBinary = lBuilder.ToString().Replace("=C", "\n").Replace("=B", "\r").Replace("=A", "\0").Replace("=D", "=");
            lLatinBinary = Encoding.GetEncoding(28591).GetBytes(lBasicBinary);

            return lLatinBinary;
        }


        internal async Task xPrintSpot() {
            NntpClient lClient = new NntpClient(new NntpConnection());
            PrintProcessor lProcessor;

            if (lClient.Connect(Global.gServer.xReader, Global.gServer.xPort, Global.gServer.xSSL)) {
                if (lClient.Authenticate(Global.gServer.xUserId, Global.gServer.xPassWord)) {
                    lProcessor = new PrintProcessor(@"E:\Test\Spotz\Article.txt");
                    lClient.Article(new NntpMessageId(xArticleId), lProcessor);
                    lClient.Quit();
                }
            }
        }

    }
}
