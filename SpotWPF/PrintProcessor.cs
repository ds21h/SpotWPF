using System.IO;

namespace SpotWPF {
    internal class PrintProcessor : NntpBase.Nntp.IMultiLineProcess {
        private StreamWriter mStream;
        private readonly string mFileName;

        internal PrintProcessor(string pFileName) {
            mFileName = pFileName;
        }

        public void xStartProcess() {
            mStream = new StreamWriter(mFileName, false);
        }

        public bool xProcessLine(string pLine) {
            mStream.WriteLine(pLine);
            return true;
        }

        public void xEndProcess() {
            mStream.Close();
        }

    }
}
