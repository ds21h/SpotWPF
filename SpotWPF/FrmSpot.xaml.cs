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
using System.Windows.Shapes;
using System.Net;

namespace SpotWPF {
    /// <summary>
    /// Interaction logic for FrmSpot.xaml
    /// </summary>
    public partial class FrmSpot : Window {
        private SpotExt mSpot;
        internal FrmSpot(SpotData pSpot) {
            InitializeComponent();
            mSpot = new SpotExt(pSpot);
            sFillScreen();
        }
        private async Task sFillScreen() {
            Task lReadSpot = null;

            lReadSpot = mSpot.xInit();
            await lReadSpot;
            txtSpot.Text = mSpot.xDescHtml;
            brSpot.NavigateToString(mSpot.xDescHtml);
        }
    }
}
