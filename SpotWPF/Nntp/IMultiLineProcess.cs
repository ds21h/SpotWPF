using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    public interface IMultiLineProcess {
        internal void xStartProcess();
        internal void xProcessLine(string pLine);
        internal void xEndProcess();
    }
}
