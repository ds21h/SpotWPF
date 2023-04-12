using System.Collections.Generic;
using System.IO;

namespace SpotWPF {
    public class RawProcessor : NntpBase.Nntp.IMultiLineProcess {
        private StreamWriter mStream;
        private List<string> mLines;
        private bool mHeaders;

        internal List<string> xLines {
            get {
                return mLines;
            }
        }

        internal RawProcessor() {
            mLines = new List<string>();
        }

        public void xStartProcess() {
            mStream = new StreamWriter(@"E:\Test\Spotz\TestRaw.txt", false);
            mHeaders = true;
            mLines.Clear();
        }

        public bool xProcessLine(string pLine) {
            if (mHeaders) {
                mStream.WriteLine(pLine);
                if (pLine.Length == 0) {
                    mHeaders = false;
                }
            } else {
                mStream.Write(pLine);
            }
            mLines.Add(pLine);
            return true;    
        }

        public void xEndProcess() {
            mStream.Close();
        }
    }
}
