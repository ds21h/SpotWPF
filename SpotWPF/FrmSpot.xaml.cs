using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using NntpBase.Nntp;
using NntpBase.Nntp.Responses;

// https://stackoverflow.com/questions/470222/hosting-and-interacting-with-a-webpage-inside-a-wpf-app

namespace SpotWPF {
    public partial class FrmSpot : Window {
        private readonly SpotExt mSpot;
        private readonly SpotPage mSpotPage;
        private readonly ScriptingHelper mScriptingHelper;
        private bool mNzbDownload;

        internal FrmSpot(SpotData pSpot) {
            InitializeComponent();
            mNzbDownload = false;
            mSpot = new SpotExt(pSpot);
            mSpotPage = new SpotPage(mSpot);
            brSpot.LoadCompleted += sSpotLoadCompleted;
            mScriptingHelper = new ScriptingHelper();
            mScriptingHelper.eDownLoadPressed += sDownloadPressed;
            brSpot.ObjectForScripting = mScriptingHelper;
        }

        [ComVisible(true)]
        public class ScriptingHelper {

            internal event EventHandler eDownLoadPressed;

            public void DownloadPressed() {
                EventHandler lHandler;

                if (eDownLoadPressed != null) {
                    lHandler = eDownLoadPressed;
                    lHandler.Invoke(this, new EventArgs());
                }
            }
        }

        private async void sDownloadPressed(object sender, EventArgs e) {
            if (!mNzbDownload) {
                mNzbDownload = true;
                await sGetNzb().ConfigureAwait(true);
                mNzbDownload = false;
            }
        }

        private async Task sFillScreen() {
            Task lReadSpot;

            lReadSpot = mSpot.xInit();
            await lReadSpot.ConfigureAwait(true);
            brSpot.NavigateToString(await mSpotPage.xAsyncCreatePage());
        }

        private async Task sGetNzb() {
            Task lGetNzb;
            SaveFileDialog lDialog;
            StreamWriter lStream;

            lGetNzb = mSpot.xGetNZB();
            await lGetNzb.ConfigureAwait(true);
            if (string.IsNullOrEmpty(mSpot.xNzb)) {
                brSpot.InvokeScript("UserMessage", "No NZB found!");
            } else {
                lDialog = new SaveFileDialog();
                lDialog.Filter = "NZB (*.nzb)|*.nzb";
                lDialog.InitialDirectory = KnownFolders.GetPath(KnownFolder.Downloads);
                lDialog.FileName = mSpot.xTitle;
                if ((bool)lDialog.ShowDialog()) {
                    lStream = new StreamWriter(lDialog.FileName, false);
                    lStream.Write(mSpot.xNzb);
                    lStream.Close();
                }
            }
        }

        private async Task sGetPrv() {
            Task lGetPrv;

            lGetPrv = mSpot.xGetPrv();
            await lGetPrv.ConfigureAwait(true);
            if (mSpot.xImageFile != string.Empty) {
                brSpot.InvokeScript("SetImage", mSpot.xImageFile);
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e) {
            await sFillScreen().ConfigureAwait(true);
        }

        private async void sSpotLoadCompleted(object sender, NavigationEventArgs e) {
            if (Global.gSpotBase != Global.gBaseError) {
                await sGetPrv().ConfigureAwait(true);
                await sGetComments().ConfigureAwait(true);
            }
        }

        private async Task sGetComments() {
            List<long> lCommentNumbers;
            NntpClient lClient = new NntpClient(new NntpConnection());
            NntpResponse lResponse;
            NntpGroupResponse lGroupResponse;
            StreamReader lReader;
            string lHtml;
            CommentEntry lComment;
            ArticleCommentProcessor lProcessor;

            if (string.IsNullOrEmpty(Global.gCommentBase)) {
                try {
                    lReader = new StreamReader(Global.cHomeDir + @"\" + Global.cCommentBase);
                    Global.gCommentBase = await lReader.ReadToEndAsync().ConfigureAwait(true);
                } catch (Exception) {
                    Global.gCommentBase = Global.gBaseError;
                }
            }

            lCommentNumbers = await Data.getInstance.xGetCommentsAsync(mSpot.xSpotId).ConfigureAwait(true);
            if (lCommentNumbers.Count > 0) {
                if (await lClient.ConnectAsync(Global.gServer.xReader, Global.gServer.xPort, Global.gServer.xSSL).ConfigureAwait(true)) {
                    if (lClient.Authenticate(Global.gServer.xUserId, Global.gServer.xPassWord)) {
                        lGroupResponse = lClient.Group(Global.cCommentGroup);
                        if (lGroupResponse.Success) {
                            foreach (long bCommentNumber in lCommentNumbers) {
                                lComment = new CommentEntry();
                                lProcessor = new ArticleCommentProcessor(lComment);
                                lResponse = lClient.Article(bCommentNumber, lProcessor);
                                if (lResponse.Success) {
                                    lHtml = Global.gCommentBase.Replace("[SN:DESC]", lComment.xComment).Replace("[SN:FROM]", lComment.xAuthor).Replace("[SN:DATE]", lComment.xDateLocalString);
                                    brSpot.InvokeScript("AddComment", lHtml);
                                }
                            }
                        }
                        lResponse = lClient.Quit();
                    }
                }
            } else {
                brSpot.InvokeScript("AddComment", "<DIV style='margin - bottom: 20px'>Geen Comments gevonden!</ DIV ><HR><BR>");
            }
            brSpot.InvokeScript("HideProgress");
        }
    }
}
