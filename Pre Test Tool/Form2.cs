
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pre_Test_Tool
{
    public partial class Form2 : DevExpress.XtraEditors.XtraForm
    {
        public Form2()
        {
            InitializeComponent();
            String path = open();
            if (path.Equals(""))
            {
                return;
            }
            else
            {
                expe.Hide();
                label10.Hide();
                label17.Hide();

                XMLParser xml = new XMLParser(path);
                int count = (xml.rootItem.children.Count - xml.getAllConnections().Count);
                decimal sccop = 0, scoh = 0, sccd = 0;
                //FuzzyLib.MyFuzzy FIM = new FuzzyLib.MyFuzzy();
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

                decimal sco = scoh / count;
                decimal sc = sccop / count;
                decimal scd = sccd / count * count;

                sccoh.Text = sco.ToString("F");
                scop.Text = sc.ToString("F");
                ccd.Text = scd.ToString("F");
                SysTe.Text = IFuzzyMetric.fuzzication((double)sco, (double)sc, (double)scd).ToString("F");
            }
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

        public string open()
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                }
            }

             return filePath;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
