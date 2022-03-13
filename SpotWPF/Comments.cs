﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Usenet.Nntp;
using Usenet.Nntp.Models;
using Usenet.Nntp.Responses;

namespace SpotWPF {
    internal class Comments {
        private static Comments mInstance;

        internal static Comments getInstance {
            get {
                if (mInstance == null) {
                    mInstance = new Comments();
                }
                return mInstance;
            }
        }

        private Comments() {
        }

        internal void xRefresh() {
            NntpResponse lResponse;
            NntpGroupResponse lGroupResponse;
            NntpClient lClient = new NntpClient(new NntpConnection());
            long lLow;
            long lHigh;
            NntpArticleRange lArticleRange;
            string lFileName = "";
            bool lSuccess;

            lSuccess = false;
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
                            if ((lHigh - lLow) > 10000) {
                                lFileName = Temp.GetTempFileName();
                            } else {
                                lFileName = "";
                            }
                            lResponse = lClient.Xover(lArticleRange, new XoverCommentsProcessor(lFileName));
                            if (lResponse.Success) {
                                lSuccess = true;
                                Global.gServer.xHighCommentId = lHigh;
                                Data.getInstance.xUpdateServer(Global.gServer);
                            }
                        }
                    }
                    lResponse = lClient.Quit();
                }
            }
            if (lSuccess && !string.IsNullOrEmpty(lFileName)) {
                Data.getInstance.xStoreComments(lFileName, Global.cHomeDir + @"\" + Global.cCommentsFormat);
            }
        }
    }
}
