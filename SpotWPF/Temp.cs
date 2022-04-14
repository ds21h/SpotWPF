using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SpotWPF {
    internal static class Temp {
        private static readonly string cTempPath;
        private const string cTempName = "Tmp";
        private const string cTempSuffix = ".tmp";
        private static int mStartSeq;

        static Temp() {
            DirectoryInfo lTempDir;
            FileInfo[] lFiles;

            cTempPath = Global.cHomeDir + @"\" + Global.cTempDir;
            lTempDir = new DirectoryInfo(cTempPath);
            if (!lTempDir.Exists) { 
                lTempDir.Create();
            }
            lFiles = lTempDir.GetFiles();
            foreach (FileInfo bFile in lFiles) { 
                bFile.Delete();
            }
            mStartSeq = 0;
        }

        internal static string GetTempFileName() {
            string lFileName;
            FileInfo lFile;

            do {
                lFileName = cTempPath + @"\" + cTempName + mStartSeq.ToString("000000") + cTempSuffix;
                lFile = new FileInfo(lFileName);
                mStartSeq++;
            } while (lFile.Exists);
            return lFileName;
        }

    }
}
