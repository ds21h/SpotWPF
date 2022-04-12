using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class SpotData {
        private long mArticleNumber;
        private protected string mArticleId;
        private string mPoster;
        private string mTitle;
        private string mTag;
        private DateTime mCreated;
        private long mSize;
        private int mCategory;
        private int mSubA;
        private List<int> mSubB;
        private List<int> mSubC;
        private List<int> mSubD;
        private int mSubZ;
        private bool mEro;
        private string mFormat;

        internal SpotData() {
            mArticleNumber = 0;
            mArticleId = string.Empty;
            mPoster = string.Empty;
            mTitle = string.Empty;
            mTag = string.Empty;
            mCreated = DateTime.MinValue;
            mSize = 0;
            mCategory = 0;
            mSubA = 0;
            mSubB = new List<int>();
            mSubC = new List<int>();
            mSubD = new List<int>();
            mSubZ = 0;
            mEro = false;
            mFormat = "";
        }

        private protected SpotData(SpotData pSpot) {
            mArticleNumber = pSpot.mArticleNumber;
            mArticleId = pSpot.mArticleId;
            mPoster = pSpot.mPoster;
            mTitle = pSpot.mTitle;
            mTag = pSpot.mTag;
            mCreated = pSpot.mCreated;
            mSize = pSpot.mSize;
            mCategory = pSpot.mCategory;
            mSubA = pSpot.mSubA;
            mSubB = new List<int>();
            foreach (int bSubCat in pSpot.mSubB) {
                mSubB.Add(bSubCat);
            }
            mSubC = new List<int>();
            foreach (int bSubCat in pSpot.mSubC) {
                mSubC.Add(bSubCat);
            }
            mSubD = new List<int>();
            foreach (int bSubCat in pSpot.mSubD) {
                mSubD.Add(bSubCat);
            }
            mSubZ = pSpot.mSubZ;
            mEro = pSpot.mEro;
            mFormat = pSpot.mFormat;
        }

        internal SpotData(string pArticleId, long pArticleNumber, DateTime pCreated, string pPoster, string pTag, string pTitle, long pSize, int pCategory, string pSubCategories, bool pEro) {
            int lStart;
            string lSubStr;
            int lSubCat;

            mArticleId = pArticleId;
            mArticleNumber = pArticleNumber;
            mCreated = pCreated;
            mPoster = pPoster;
            mTitle = pTitle;
            mTag = pTag;
            mSize = pSize;
            mCategory = pCategory;
            mSubA = 0;
            mSubB = new List<int>();
            mSubC = new List<int>();
            mSubD = new List<int>();
            mSubZ = 0;
            lStart = 0;
            try {
                while (lStart < pSubCategories.Length) {
                    if (lStart > pSubCategories.Length - 3) {
                        lSubStr = pSubCategories.Substring(lStart);
                    } else {
                        lSubStr = pSubCategories.Substring(lStart, 3);
                    }
                    if (int.TryParse(lSubStr.Substring(1), out lSubCat)) {
                        switch (lSubStr[0]) {
                            case 'a': {
                                    mSubA = lSubCat;
                                    break;
                                }
                            case 'b': {
                                    mSubB.Add(lSubCat);
                                    break;
                                }
                            case 'c': {
                                    mSubC.Add(lSubCat);
                                    break;
                                }
                            case 'd': {
                                    mSubD.Add(lSubCat);
                                    break;
                                }
                            case 'z': {
                                    mSubZ = lSubCat;
                                    break;
                                }
                        }
                    }

                    lStart += 4;
                }
            } catch (Exception) {
            }
            mEro = pEro;
            mFormat = "";
        }

        public long xArticleNumber {
            get {
                return mArticleNumber;
            }
            set {
                mArticleNumber = value;
            }
        }

        public string xFormat {
            get {
                if (mFormat == "") {
                    mFormat = SpotCoding.xFormat(mCategory, mSubA);
                }
                return mFormat;
            }
        }

        public string xType {
            get {
                string lResult;

                switch (mCategory) {
                    default: {
                            if (mSubD.Count > 0) {
                                lResult = SpotCoding.xTypeImage(mSubD[0]);
                            } else {
                                lResult = "--";
                            }
                            break;
                        }
                    case 2: {
                            if (mSubD.Count > 0) {
                                lResult = SpotCoding.xTypeSound(mSubD[0]);
                            } else {
                                lResult = "--";
                            }
                            break;
                        }
                    case 3: {
                            if (mSubC.Count > 0) {
                                lResult = SpotCoding.xTypeGames(mSubC[0]);
                            } else {
                                lResult = "--";
                            }
                            break;
                        }
                    case 4: {
                            if (mSubB.Count > 0) {
                                lResult = SpotCoding.xTypeApp(mSubB[0]);
                            } else {
                                lResult = "--";
                            }
                            break;
                        }
                }
                return lResult;
            }
        }

        public bool xNew {
            get {
                return (mArticleNumber > Global.gServer.xHighSeenId);
            }
        }

        internal string xArticleId {
            get {
                return mArticleId;
            }
            set {
                mArticleId = value;
            }
        }

        public string xPoster {
            get {
                return mPoster;
            }
            set {
                mPoster = value;
            }
        }

        public string xTitle {
            get {
                return mTitle;
            }
            set {
                mTitle = value;
            }
        }

        internal string xTag {
            get {
                return mTag;
            }
            set {
                mTag = value;
            }
        }

        internal DateTime xCreated {
            get {
                return mCreated;
            }
            set {
                mCreated = value;
            }
        }

        public string xCreatedLocalString {
            get {
                return mCreated.ToLocalTime().ToString("ddd d-M-yyyy HH:mm");
            }
        }

        internal long xSize {
            get {
                return mSize;
            }
            set {
                mSize = value;
            }
        }

        internal string xSizeStr {
            get {
                float lSize;
                string lResult;

                if (mSize < 1024) {
                    lResult = mSize.ToString() + " B";
                } else {
                    lSize = mSize / 1024f;
                    if (lSize < 1024f) {
                        lResult = lSize.ToString("###0.00") + " KB";
                    } else {
                        lSize = lSize / 1024f;
                        if (lSize < 1024f) {
                            lResult = lSize.ToString("###0.00") + " MB";
                        } else {
                            lSize = lSize / 1024f;
                            lResult = lSize.ToString("###0.00") + " GB";
                        }

                    }
                }
                return lResult;
            }
        }

        internal int xCategory {
            get {
                return mCategory;
            }
            set {
                mCategory = value;
            }
        }

        internal void xAddSubCategory(string pSubCat) {
            string lSubStr;
            int lSubCat;

            lSubStr = pSubCat.ToLower();
            if (int.TryParse(lSubStr.Substring(1), out lSubCat)) {
                if (mCategory == 1) {
                    if (SpotCoding.xIsEroCat(lSubStr)) {
                        mCategory = 9;
                        mEro = true;
                    } else {
                        if (SpotCoding.xIsEbookCat(lSubStr)) {
                            mCategory = 5;
                        } else {
                            if (SpotCoding.xIsSerialsCat(lSubStr)) {
                                mCategory = 6;
                            }
                        }
                    }
                }
                switch (lSubStr[0]) {
                    case 'a': {
                            mSubA = lSubCat;
                            break;
                        }
                    case 'b': {
                            mSubB.Add(lSubCat);
                            break;
                        }
                    case 'c': {
                            mSubC.Add(lSubCat);
                            break;
                        }
                    case 'd': {
                            mSubD.Add(lSubCat);
                            break;
                        }
                    case 'z': {
                            mSubZ = lSubCat;
                            break;
                        }
                }
            }
        }

        internal string xSubCategories {
            get {
                StringBuilder lResult;

                lResult = new StringBuilder();
                lResult.Append('a');
                lResult.Append(mSubA.ToString("00"));
                foreach (int bSubCat in mSubB) {
                    lResult.Append(" b");
                    lResult.Append(bSubCat.ToString("00"));
                }
                foreach (int bSubCat in mSubC) {
                    lResult.Append(" c");
                    lResult.Append(bSubCat.ToString("00"));
                }
                foreach (int bSubCat in mSubD) {
                    lResult.Append(" d");
                    lResult.Append(bSubCat.ToString("00"));
                }
                lResult.Append(" z");
                lResult.Append(mSubZ.ToString("00"));
                return lResult.ToString();
            }
        }

        internal int xSubA {
            get {
                return mSubA;
            }
        }

        internal List<int> xSubB {
            get {
                return mSubB;
            }
        }

        internal List<int> xSubC {
            get {
                return mSubC;
            }
        }

        internal List<int> xSubD {
            get {
                return mSubD;
            }
        }

        internal bool xEro {
            get {
                return mEro;
            }
        }
    }
}
