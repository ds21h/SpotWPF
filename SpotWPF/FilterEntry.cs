namespace SpotWPF {
    internal class FilterEntry {
        private int mSeq;
        private string mTitle;
        private bool mSpecial;
        private string mFilterString;

        internal FilterEntry(int pSeq, string pTitle, bool pSpecial, string pFilterString) {
            mSeq = pSeq;
            mTitle = pTitle;
            mSpecial = pSpecial;
            mFilterString = pFilterString;
        }

        public int xSeq {
            get { 
                return mSeq; 
            }
        }

        public string xTitle {
            get {
                return mTitle;
            }
        }

        public string xFilterString {
            get { 
                return mFilterString;
            }
        }

        internal bool xSpecial {
            get {
                return mSpecial;
            }
        }
    }
}
