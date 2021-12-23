using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class SpotData {
        private long mArticleNumber;
        private string mArticleId;
        private string mPoster;
        private string mTitle;
        private string mTag;
        private DateTime mCreated;
        private long mSize;
        private int mCategory;
        private List<string> mSubCategories;

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
        }

        internal SpotData(string pArticleId, long pArticleNumber, DateTime pCreated, string pPoster, string pTag, string pTitle, long pSize, int pCategory, string pSubCategories) {
            int lStart;

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
            try {
                while (lStart < pSubCategories.Length) {
                    if (lStart > pSubCategories.Length - 3) {
                        mSubCategories.Add(pSubCategories.Substring(lStart));
                    } else {
                        mSubCategories.Add(pSubCategories.Substring(lStart, 3));
                    }

                    lStart += 3;
                }
            } catch (Exception) {
            }
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
        }

        internal string xSubCategories {
            get {
                StringBuilder lResult;

                lResult = new StringBuilder();
                foreach (string bSubCat in mSubCategories) {
                    lResult.Append(bSubCat);
                }
                return lResult.ToString();
            }
        }
    }
}
