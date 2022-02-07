using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotWPF {
    internal class Server {
        private int mId;
        private string mName;
        private string mReader;
        private int mPort;
        private bool mSSL;
        private string mUserId;
        private string mPassWord;
        private long mHighSpotId;
        private long mHighCommentId;

        internal Server(int pId, string pName, string pReader, int pPort, bool pSSL, string pUserId, string pPassWord, long pHighSpotId) {
            mId = pId;
            mName = pName;
            mReader = pReader;
            mPort = pPort;
            mSSL = pSSL;
            mUserId = pUserId;
            mPassWord = pPassWord;
            mHighSpotId = pHighSpotId;
            mHighCommentId = 0;
        }

        internal int xId {
            get {
                return mId;
            }
        }

        internal string xName {
            get {
                return mName;
            }
        }

        internal string xReader {
            get {
                return mReader;
            }
        }

        internal int xPort {
            get { 
                return mPort; 
            }
        }

        internal bool xSSL {
            get {
                return mSSL;
            }
        }

        internal string xUserId {
            get {
                return mUserId;
            }
        }

        internal string xPassWord {
            get {
                return mPassWord;
            }
        }

        internal long xHighSpotId {
            get {
                return mHighSpotId;
            }
            set {
                mHighSpotId = value;
            }
        }

        internal long xHighCommentId {
            get {
                return mHighCommentId;
            }
            set {
                mHighCommentId = value;
            }
        }
    }
}
