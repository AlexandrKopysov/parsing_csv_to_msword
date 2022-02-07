using csv_to_word_v1.Model;
using csv_to_word_v1.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace csv_to_word_v1
{
    public partial class scanPasport : Form

    {
        private DataModel data = new DataModel();

        public scanPasport()
        {
            InitializeComponent();
        }

        private void change_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog templateFile = new OpenFileDialog();
            if (templateFile.ShowDialog() == DialogResult.OK)
            {
                data.fileTemplate = templateFile.FileName;                
            }
            OpenFileDialog csvFile = new OpenFileDialog();
            if (csvFile.ShowDialog() == DialogResult.OK)
            {
                data.fileCsv = csvFile.FileName;
            }
            var word = new Word(data.fileTemplate);
            //var excel = new ExcellClass(data.dataDefectArray);
            Сsv csv = new Сsv(data);
            data = csv.Import();

            var filesItems = new Dictionary<string, string>
            {
                {"<fileScanGeometry>", data.fileScanGeometry},
                {"<fileScandeffect>", data.fileScanDefect},
                {"<fileSpectr>", data.fileSpectr},
            };

            var charts = new Dictionary<string, Array>
            {
                {"<chart_1>", data.sectionArray.Select(x => x.averagInSection).ToArray()}                
            };

            var items = new Dictionary<string, string>
            {
                {"<pasportNumber>", data.pasportNumber},
                {"<averageDiametr>", data.averageDiametr.ToString()},
                {"<averageNonRoundless>", data.averageNonRoundless.ToString()},
                {"<averageDeviationDiametr>", data.averageDeviationDiametr.ToString()},
                {"<dMax>", data.dMax.ToString()},
                {"<dMin>", data.dMin.ToString()},                
                {"<dateTime>", data.dateTime},
                {"<owner>", data.owner}
            };



            word.Process(items, filesItems, charts,data.Picture,data.GroupInGropupsDefect,data.dataDefectArray);            


        }

        private void fileScanGeometry_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                data.fileScanGeometry = ofd.FileName;
            }
        }

        private void fileScandeffect_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                data.fileScanDefect = ofd.FileName;                
            }
        }

        private void fileSpectr_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                data.fileSpectr = ofd.FileName;                
            }
        }
    }
}
