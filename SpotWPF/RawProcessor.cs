﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpotWPF {
    public class RawProcessor : IMultiLineProcess {
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

        public void xProcessLine(string pLine) {
            if (mHeaders) {
                mStream.WriteLine(pLine);
                if (pLine.Length == 0) {
                    mHeaders = false;
                }
            } else {
                mStream.Write(pLine);
            }
            mLines.Add(pLine);
        }

        public void xEndProcess() {
            mStream.Close();
        }
    }
}
