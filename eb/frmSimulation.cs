using eb.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eb
{
    public partial class frmSimulation : Form
    {
        private string code;

        enum LONG_SIMUL_COL
        {
            DATE,
            TIME,
            MSMD,
            PRICE,
            RATE,
            CPOWER,
            DIFFERENCE_RATE,
            DIFFERENCE_AMOUNT,
            PROFIT,
            TOTAL_PROFIT
        }

        public frmSimulation()
        {
            InitializeComponent();
        }

        public frmSimulation(string code)
        {
            InitializeComponent();
            this.code = code;
        }

        private void btnSimulation_Click(object sender, EventArgs e)
        {
            


        }
    }
}
