using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pre_Test_Tool
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //XMLParser xml = new  XMLParser(@"K:\Download/test.xml");

            //MessageBox.Show(xml.show());

            Form2 result = new Form2();
            result.ShowDialog();
        }
    }
}
