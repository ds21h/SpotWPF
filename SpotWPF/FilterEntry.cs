namespace SpotWPF {
    internal class FilterEntry {
        private int mSeq;
        private string mTitle;
        private string mFilterString;

        internal FilterEntry(int pSeq, string pTitle, string pFilterString) {
            mSeq = pSeq;
            mTitle = pTitle;
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
    }
}
