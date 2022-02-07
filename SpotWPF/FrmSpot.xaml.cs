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

// https://stackoverflow.com/questions/470222/hosting-and-interacting-with-a-webpage-inside-a-wpf-app

namespace SpotWPF {
    public partial class FrmSpot : Window {
        private const string cSpotBase = @"E:\Test\Spotz\SpotView.htm";
        private const string cResourcePath = @"E:\Test\Spotz";
        private const string cFontSize = "18";
        private SpotExt mSpot;
        private ScriptingHelper mScriptingHelper;
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

            public void ShowMessage(string message) {
                EventHandler lHandler;

                if (eDownLoadPressed != null) {
                    lHandler = eDownLoadPressed;
                    lHandler.Invoke(this, new EventArgs());
                }

                MessageBox.Show(message);
            }
        }

        private void sDownloadPressed(object sender, EventArgs e) {
            if (!mNzbDownload) {
                mNzbDownload=true;
                sGetNzb();
            }
        }

        private async Task sFillScreen() {
            Task lReadSpot = null;
            string lHtml = null;
            StreamReader lReader;

            lReadSpot = mSpot.xInit();
            try {
                lReader = new StreamReader(cSpotBase);
                lHtml = await lReader.ReadToEndAsync();
            } catch (Exception pExc) {
            }
            await lReadSpot;
            if (lHtml == null) {
                lHtml = "";
            } else {
                lHtml = lHtml.Replace("[SN:DESC]", mSpot.xDescription).Replace ("[SN:FONTSIZE]", cFontSize).Replace("[SN:PATH]", cResourcePath);
            }
            brSpot.NavigateToString(lHtml);
        }

        private async Task sGetNzb() {
            Task lGetNzb = null;
            SaveFileDialog lDialog;
            StreamWriter lStream;

            lGetNzb = mSpot.xGetNZB();
            await lGetNzb;
            lDialog = new SaveFileDialog();
            lDialog.Filter = "NZB (*.nzb)|*.nzb";
            lDialog.InitialDirectory = KnownFolders.GetPath(KnownFolder.Downloads);
            lDialog.FileName = mSpot.xTitle;
            if ((bool)lDialog.ShowDialog()) {
                lStream = new StreamWriter(lDialog.FileName, false);
                lStream.Write(mSpot.xNzb);
                lStream.Close();
            }
            mNzbDownload = false;
        }

        private async Task sGetPrv() {
            Task lGetPrv = null;

            lGetPrv = mSpot.xGetPrv();
            await lGetPrv;
            if (mSpot.xImageFile != string.Empty) {
                brSpot.InvokeScript("SetImage", mSpot.xImageFile);
            }
        }

        private void btnSpot_Click(object sender, RoutedEventArgs e) {
            btnSpot.IsEnabled = false;
            sGetSpot();
        }

        private async Task sGetSpot() {
            Task lGetSpot = null;

            lGetSpot = mSpot.xPrintSpot();
            await lGetSpot;
            btnSpot.IsEnabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            sFillScreen();

        }
        private void sSpotLoadCompleted(object sender, NavigationEventArgs e) {
            sGetPrv();
        }

        private void btnComment_Click(object sender, RoutedEventArgs e) {
            brSpot.InvokeScript("AddComment", "<BR>Button Tekst uit AddComment<BR>");
        }
    }
}
