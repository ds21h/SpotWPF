using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {

    internal class SpotEntry {
        public string xArticleNumber { get; set; }
        public string xTitle { get; set; }

        internal SpotEntry() {
            xArticleNumber = "";
            xTitle = "";
        }
    }
}
