using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWPF.NntpBase.Exceptions {
    public class MultiLineCanceledException : Exception {
        public MultiLineCanceledException() {
        }

        public MultiLineCanceledException(string message) : base(message) {
        }

        public MultiLineCanceledException(string message, Exception innerException) : base(message, innerException) {
        }
    }
}
