using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;


namespace SpotWPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class FrmOverview : Window {
        private const string cSearchString = "(CONTAINS(Title, '\"[SN:STRING]\"'))";
        private readonly Data mData;
        private readonly Spots mSpots;
        private readonly Comments mComments;
        private int mFilter;
        private bool mFrmInit;
        private List<FilterEntry> mFilters;

        public FrmOverview() {
            ILoggerFactory lFactory;

            InitializeComponent();
            lFactory = LoggerFactory.Create(builder => builder.AddDebug());
            Usenet.Logger.Factory = lFactory;
            mFrmInit = false;
            mFilter = -1;
            mData = Data.getInstance;
            Global.gServer = mData.xGetServer("I-telligent SSL");
            mSpots = new Spots();
            mComments = Comments.getInstance;
            mFilters = mData.xGetFilters();
            cmbFilter.ItemsSource = mFilters;
            cmbFilter.DisplayMemberPath = "xTitle";
        }

        private void lstSpots_SizeChanged(object sender, SizeChangedEventArgs e) {
            GridView lGridView;
            double lWorkWidth;
            int lColumnCount;
            const int cColumnNumber = 1;

            lGridView = lstSpots.View as GridView;
            if (lGridView != null) {
                lWorkWidth = lstSpots.ActualWidth - SystemParameters.VerticalScrollBarWidth - lstSpots.Margin.Left - lstSpots.Margin.Right;
                for (lColumnCount = 0; lColumnCount < lGridView.Columns.Count; lColumnCount++) {
                    if (lColumnCount != cColumnNumber) {
                        lWorkWidth -= lGridView.Columns[lColumnCount].ActualWidth;
                    }
                }
                lGridView.Columns[cColumnNumber].Width = lWorkWidth;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e) {
            sInitFilter();
            await mSpots.xInitSpotsAsync();
            lstSpots.DataContext = mSpots;
            mFrmInit = true;
        }

        private void sInitFilter() {
            int lFilter;
            FilterEntry lEntry;

            lFilter = cmbFilter.SelectedIndex;
            if (lFilter >= 0 && lFilter != mFilter) {
                lEntry = mFilters[lFilter];
                mData.xSetView(lEntry.xFilterString);
                mFilter = lFilter;
            }
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e) {
            Task lRefreshSpots;
            Task lRefreshComments;
            bool lUpdate;

            btnRefresh.IsEnabled = false;
            lUpdate = (bool)chkUpdate.IsChecked;
            lRefreshSpots = Task.Run(() => mSpots.xRefresh(lUpdate));
            lRefreshComments = Task.Run(() => mComments.xRefresh());
            await lRefreshSpots;
            lstSpots.DataContext = null;
            lstSpots.InvalidateVisual();
            lstSpots.DataContext = mSpots;
            await lRefreshComments;
            btnRefresh.IsEnabled = true;
        }

        void ListViewItem_MouseDoubleClick(object pSender, MouseButtonEventArgs e) {
            FrmSpot lFrmSpot;
            ListViewItem lItem;
            SpotData lSpot;

            lItem = (ListViewItem)pSender;
            lSpot = lItem.Content as SpotData;
            if (lSpot != null) {
                lFrmSpot = new FrmSpot(lSpot);
                lFrmSpot.ShowDialog();
            }
        }

        private async void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (mFrmInit) {
                if (cmbFilter.SelectedIndex >= 0 && cmbFilter.SelectedIndex != mFilter) {
                    lstSpots.DataContext = null;
                    lstSpots.InvalidateVisual();
                    sInitFilter();
                    await mSpots.xInitSpotsAsync();
                    lstSpots.DataContext = mSpots;
                }
            }
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e) {
            string lSearch;

            lSearch = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(lSearch)) {
                mData.xSetView(cSearchString.Replace("[SN:STRING]", txtSearch.Text));
                lstSpots.DataContext = null;
                lstSpots.InvalidateVisual();
                await mSpots.xInitSpotsAsync();
                lstSpots.DataContext = mSpots;
            }
        }

        private void btnRaw_Click(object sender, RoutedEventArgs e) {
            SpotData lSpot;

            if (lstSpots.SelectedIndex >= 0) { 
                lSpot = lstSpots.SelectedItem as SpotData;
                mSpots.xGetSpotRaw(lSpot.xArticleId);
            }
        }
    }
}
