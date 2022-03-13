using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using Microsoft.Win32;
using System.IO;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using Usenet.Nntp;
using Usenet.Nntp.Models;
using Usenet.Nntp.Responses;

// https://stackoverflow.com/questions/470222/hosting-and-interacting-with-a-webpage-inside-a-wpf-app

namespace SpotWPF {
    public partial class FrmSpot : Window {
        private const string cFontSize = "18";
        private readonly SpotExt mSpot;
        private readonly ScriptingHelper mScriptingHelper;
        private bool mNzbDownload;

        internal FrmSpot(SpotData pSpot) {
            InitializeComponent();
            mNzbDownload = false;
            mSpot = new SpotExt(pSpot);
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
            string lHtml = null;
            StreamReader lReader;

            lReadSpot = mSpot.xInit();
            try {
                lReader = new StreamReader(Global.cHomeDir + @"\" + Global.cSpotBase);
                lHtml = await lReader.ReadToEndAsync().ConfigureAwait(true);
            } catch (Exception) {
            }
            await lReadSpot.ConfigureAwait(true);
            if (lHtml == null) {
                lHtml = "";
            } else {
                lHtml = lHtml.Replace("[SN:DESC]", mSpot.xDescription).Replace ("[SN:FONTSIZE]", cFontSize).Replace("[SN:PATH]", Global.cHomeDir);
            }
            brSpot.NavigateToString(lHtml);
        }

        private async Task sGetNzb() {
            Task lGetNzb;
            SaveFileDialog lDialog;
            StreamWriter lStream;

            lGetNzb = mSpot.xGetNZB();
            await lGetNzb.ConfigureAwait(true);
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

        private async Task sGetPrv() {
            Task lGetPrv;

            lGetPrv = mSpot.xGetPrv();
            await lGetPrv.ConfigureAwait(true);
            if (mSpot.xImageFile != string.Empty) {
                brSpot.InvokeScript("SetImage", mSpot.xImageFile);
            }
        }

        private async void btnSpot_Click(object sender, RoutedEventArgs e) {
            btnSpot.IsEnabled = false;
            await sGetSpot().ConfigureAwait(true);
            btnSpot.IsEnabled = true;
        }

        private async Task sGetSpot() {
            Task lGetSpot;

            lGetSpot = mSpot.xPrintSpot();
            await lGetSpot.ConfigureAwait(false);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e) {
            await sFillScreen().ConfigureAwait(true);
        }

        private async void sSpotLoadCompleted(object sender, NavigationEventArgs e) {
            await sGetPrv().ConfigureAwait(true);
            await sGetComments().ConfigureAwait(true);
        }

        private async void btnComment_Click(object sender, RoutedEventArgs e) {
            List<long> lCommentNumbers;
            NntpClient lClient = new NntpClient(new NntpConnection());
            NntpResponse lResponse;
            NntpGroupResponse lGroupResponse;
            NntpArticleResponse lArticleResponse;
            StringBuilder lBuilder;
            StreamReader lReader;
            string lHtml;

            if (string.IsNullOrEmpty(Global.gCommentBase)) {
                try {
                    lReader = new StreamReader(Global.cHomeDir + @"\" + Global.cCommentBase);
                    Global.gCommentBase = await lReader.ReadToEndAsync().ConfigureAwait(true);
                } catch (Exception) {
                }
            }

            lCommentNumbers = await Data.getInstance.xGetCommentsAsync(mSpot.xSpotId).ConfigureAwait(true);
            if (lCommentNumbers.Count > 0) {
                if (await lClient.ConnectAsync(Global.gServer.xReader, Global.gServer.xPort, Global.gServer.xSSL).ConfigureAwait(true)) {
                    if (lClient.Authenticate(Global.gServer.xUserId, Global.gServer.xPassWord)) {
                        lGroupResponse = lClient.Group(Global.cCommentGroup);
                        if (lGroupResponse.Success) {
                            foreach (long bCommentNumber in lCommentNumbers) {
                                lArticleResponse = lClient.Article(bCommentNumber);
                                if (lArticleResponse.Success) {
                                    lBuilder = new StringBuilder();
                                    foreach (string bLine in lArticleResponse.Article.Body) {
                                        lBuilder.Append(bLine);
                                        lBuilder.Append("<BR>");
                                    }
                                    lHtml = Global.gCommentBase.Replace("[SN:DESC]", lBuilder.ToString());
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

        private async Task sGetComments() {
            List<long> lCommentNumbers;
            NntpClient lClient = new NntpClient(new NntpConnection());
            NntpResponse lResponse;
            NntpGroupResponse lGroupResponse;
            NntpArticleResponse lArticleResponse;
            StringBuilder lBuilder;
            StreamReader lReader;
            string lHtml;

            if (string.IsNullOrEmpty(Global.gCommentBase)) {
                try {
                    lReader = new StreamReader(Global.cHomeDir + @"\" + Global.cCommentBase);
                    Global.gCommentBase = await lReader.ReadToEndAsync().ConfigureAwait(true);
                } catch (Exception) {
                }
            }

            lCommentNumbers = await Data.getInstance.xGetCommentsAsync(mSpot.xSpotId).ConfigureAwait(true);
            if (lCommentNumbers.Count > 0) {
                if (await lClient.ConnectAsync(Global.gServer.xReader, Global.gServer.xPort, Global.gServer.xSSL).ConfigureAwait(true)) {
                    if (lClient.Authenticate(Global.gServer.xUserId, Global.gServer.xPassWord)) {
                        lGroupResponse = lClient.Group(Global.cCommentGroup);
                        if (lGroupResponse.Success) {
                            foreach (long bCommentNumber in lCommentNumbers) {
                                lArticleResponse = lClient.Article(bCommentNumber);
                                if (lArticleResponse.Success) {
                                    lBuilder = new StringBuilder();
                                    foreach (string bLine in lArticleResponse.Article.Body) {
                                        lBuilder.Append(bLine);
                                        lBuilder.Append("<BR>");
                                    }
                                    lHtml = Global.gCommentBase.Replace("[SN:DESC]", lBuilder.ToString());
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
