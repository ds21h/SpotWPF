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
        private Data mData;
        private Spots mSpots;
        private Comments mComments;

        private delegate void dUpdateGrid();
        private dUpdateGrid hUpdateGrid;

        public FrmOverview() {
            ILoggerFactory lFactory;

            InitializeComponent();
            lFactory = LoggerFactory.Create(builder => builder.AddDebug());
            Usenet.Logger.Factory = lFactory;
            mData = Data.getInstance;
            Global.gServer = mData.xGetServer("I-telligent SSL");
            mSpots = new Spots();
            lstSpots.DataContext = mSpots;
            hUpdateGrid = new dUpdateGrid(sUpdateGrid);
            mSpots.eDbUpdated += hDbUpdated;
            mComments = Comments.getInstance;
        }

        private void sUpdateGrid() {
            lstSpots.InvalidateVisual();
            btnRefresh.IsEnabled = true;
        }

        private void hDbUpdated(Object pSender, EventArgs pArgs) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, hUpdateGrid);
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
                        lWorkWidth = lWorkWidth - lGridView.Columns[lColumnCount].ActualWidth;
                    }
                }
                lGridView.Columns[cColumnNumber].Width = lWorkWidth;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e) {
            Thread lThread;

            mComments.xRefresh();

            //btnRefresh.IsEnabled = false;
            //lThread = new Thread(new ThreadStart(mSpots.xRefresh));
            //lThread.Start();
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
    }
}
