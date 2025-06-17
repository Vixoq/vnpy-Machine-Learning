using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace easyMoney.Reports
{
    public partial class ReportViewForm : Form
    {
        public ReportViewForm()
        {
            InitializeComponent();
        }

        private void ReportViewForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
