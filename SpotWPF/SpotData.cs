using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class SpotData {
        private static string[] cEroCats = {"d23", "d24", "d25", "d26", "d72", "d73", "d74", "d75", "z03"};
        private long mArticleNumber;
        private protected string mArticleId;
        private string mPoster;
        private string mTitle;
        private string mTag;
        private DateTime mCreated;
        private long mSize;
        private int mCategory;
        private List<string> mSubCategories;
        private bool mEro;

        internal SpotData() {
            mArticleNumber = 0;
            mArticleId = string.Empty;
            mPoster = string.Empty;
            mTitle = string.Empty;
            mTag = string.Empty;
            mCreated = DateTime.MinValue;
            mSize = 0;
            mCategory = 0;
            mSubCategories = new List<string>();
            mEro = false;
        }

        private protected SpotData(SpotData pSpot) {
            mArticleNumber = pSpot.mArticleNumber;
            mArticleId=pSpot.mArticleId;
            mPoster=pSpot.mPoster;
            mTitle = pSpot.mTitle;
            mTag = pSpot.mTag;
            mCreated = pSpot.mCreated;
            mSize = pSpot.mSize;
            mCategory = pSpot.mCategory;
            mSubCategories = new List<string>();
            foreach (var bSub in pSpot.mSubCategories) {
                mSubCategories.Add(bSub);
            }
            mEro = pSpot.mEro;
        }

        internal SpotData(string pArticleId, long pArticleNumber, DateTime pCreated, string pPoster, string pTag, string pTitle, long pSize, int pCategory, string pSubCategories, bool pEro) {
            int lStart;
            int lStep;
//            DateTimeOffset lDate;

            mArticleId = pArticleId;
            mArticleNumber = pArticleNumber;

            mCreated = pCreated;
            mPoster = pPoster;
            mTitle = pTitle;
            mTag = pTag;
            mSize = pSize;
            mCategory = pCategory;
            mSubCategories = new List<string>();
            lStart = 0;
            if (pSubCategories.Length > 3) {
                if (pSubCategories[3] == ' ') {
                    lStep = 4;
                } else {
                    lStep = 3;
                }
            } else {
                lStep = 3;
            }
            try {
                while (lStart < pSubCategories.Length) {
                    if (lStart > pSubCategories.Length - 3) {
                        mSubCategories.Add(pSubCategories.Substring(lStart));
                    } else {
                        mSubCategories.Add(pSubCategories.Substring(lStart, 3));
                    }

                    lStart += lStep;
                }
            } catch (Exception) {
            }
            mEro = pEro;
        }

        public long xArticleNumber {
            get {
                return mArticleNumber;
            }
            set {
                mArticleNumber = value;
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

        internal string xPoster {
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

        internal int xCategory {
            get {
                return mCategory;
            }
            set {
                mCategory = value;
            }
        }

        internal void xAddSubCategory(string pSubCat) {
            mSubCategories.Add(pSubCat);
            if (mCategory == 1) {
                if (cEroCats.Contains(pSubCat)) {
                    mEro = true;
                }
            }
        }

        internal string xSubCategories {
            get {
                StringBuilder lResult;
                bool lFirst;

                lResult = new StringBuilder();
                lFirst = true;
                foreach (string bSubCat in mSubCategories) {
                    if (lFirst) {
                        lFirst = false;
                    } else {
                        lResult.Append(' ');
                    }
                    lResult.Append(bSubCat);
                }
                return lResult.ToString();
            }
        }

        internal bool xEro {
            get {
                return mEro;
            }
        }
    }
}
