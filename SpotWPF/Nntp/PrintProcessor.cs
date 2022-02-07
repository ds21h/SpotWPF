using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpotWPF {
    internal class PrintProcessor : IMultiLineProcess {
        private StreamWriter mStream;
        private readonly string mFileName;

        internal PrintProcessor(string pFileName) { 
            mFileName = pFileName;
        }

        public void xStartProcess() {
            mStream = new StreamWriter(mFileName, false);
        }

        public void xProcessLine(string pLine) {
            mStream.WriteLine(pLine);
        }

        public void xEndProcess() {
            mStream.Close();
        }

    }
}
