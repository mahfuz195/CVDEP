using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenStreetMap_CV_Toolkit
{
    public partial class FromSettings : Form
    {
        public FromSettings()
        {
            InitializeComponent();
        }

        private void bt_cancle_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
