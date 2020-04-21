
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;

namespace Pre_Test_Tool
{
    public partial class Form2 : DevExpress.XtraEditors.XtraForm
    {
        public Form2()
        {
            InitializeComponent();
            XMLParser xml = new XMLParser(@"K:\Download\test.xml");
            int count = (xml.rootItem.children.Count - xml.getAllConnections().Count);
            decimal sccop =  0 , scoh = 0 , sccd = 0;
            FuzzyLib.MyFuzzy FIM = new FuzzyLib.MyFuzzy();
            foreach (XMLItem item in xml.rootItem.children)
            {
                if (item._getProperty("ItemKind") != "DiagramConnector")
                {
                    scoh += (decimal)xml.Cohesion(item);
                    sccop += (decimal)xml.Coupling(item);
                    sccd += (decimal)xml.CDep(item);

                    String str = xml.CDep(item).ToString("F");
                    if (xml.CDep(item) == 0.50M) str += "*";
                    component.Items.Add(item.getName());
                    ccoh.Items.Add(xml.Cohesion(item).ToString("F"));
                    cop.Items.Add(xml.Coupling(item).ToString("F"));
                    cdp.Items.Add(str);
                    double x = IFuzzyMetric.fuzzication(xml.Cohesion(item), (double)xml.Coupling(item), (double)xml.CDep(item));
                    te.Items.Add(x.ToString());
                }
            }

            sccoh.Text = (scoh / count).ToString("F");
            scop.Text = (sccop / count).ToString("F");
            ccd.Text = (sccd / (count * count)).ToString("F");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
