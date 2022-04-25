using NntpBase.Nntp;
using NntpBase.Nntp.Models;
using NntpBase.Nntp.Responses;

namespace SpotWPF {
    internal class Comments {
        private static Comments mInstance;
        private volatile bool mRefreshRunning;

        internal static Comments getInstance {
            get {
                if (mInstance == null) {
                    mInstance = new Comments();
                }
                return mInstance;
            }
        }

        private Comments() {
            mRefreshRunning = false;
        }

        internal void xRefresh() {
            NntpResponse lResponse;
            NntpGroupResponse lGroupResponse;
            NntpClient lClient = new NntpClient(new NntpConnection());
            long lLow;
            long lHigh;
            NntpArticleRange lArticleRange;

            if (!mRefreshRunning) {
                mRefreshRunning = true;
                if (lClient.Connect(Global.gServer.xReader, Global.gServer.xPort, Global.gServer.xSSL)) {
                    if (lClient.Authenticate(Global.gServer.xUserId, Global.gServer.xPassWord)) {
                        lGroupResponse = lClient.Group(Global.cCommentGroup);
                        if (lGroupResponse.Success) {
                            if (Global.gServer.xHighCommentId < lGroupResponse.Group.LowWaterMark) {
                                lLow = lGroupResponse.Group.LowWaterMark;
                            } else {
                                lLow = Global.gServer.xHighCommentId + 1;
                            }
                            if (lLow <= lGroupResponse.Group.HighWaterMark) {
                                lHigh = lGroupResponse.Group.HighWaterMark;
                                lArticleRange = new NntpArticleRange(lLow, lHigh);
                                lResponse = lClient.Xover(lArticleRange, new XoverCommentsProcessor());
                                if (lResponse.Success) {
                                    Global.gServer.xHighCommentId = lHigh;
                                    Data.getInstance.xUpdateServer(Global.gServer);
                                }
                            }
                        }
                        lResponse = lClient.Quit();
                    }
                }
                mRefreshRunning = false;
            }
        }
    }
}
