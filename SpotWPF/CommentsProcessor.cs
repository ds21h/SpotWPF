using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpotWPF {
    internal class CommentsProcessor : IMultiLineProcess {
        private StreamWriter mStream;
        private string mFileName;

        internal CommentsProcessor(string pFileName) {
            mFileName = pFileName;
       }

        public void xStartProcess() {
            mStream = new StreamWriter(mFileName, false);
        }

        public void xProcessLine(string pLine) {
            string[] lWords;

            lWords = pLine.Split(" ");
            if (lWords.Length == 2) {
                mStream.WriteLine(pLine);
            }
        }

        public void xEndProcess() {
            mStream.Close();
        }
    }
}
