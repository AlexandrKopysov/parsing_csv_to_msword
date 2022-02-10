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

            data.squereSliceMax = square_1.Text;
            data.squereSliceMin = square_2.Text;
            data.squereSliceMediana = square_3.Text;
            data.createExcell = Excell.Checked;

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
            
            //var charts = new Dictionary<string, Array>
            //{
            //    {"<chart_1>", data.sectionArray.Select(x => x.averagInSection).ToArray()}
            //};


            var items = new Dictionary<string, string>
            {
                {"<pasportNumber>", data.pasportNumber},                
                {"<averageDiametr>", data.averageDiametr.ToString()},
                {"<averageNonRoundless>", data.averageNonRoundless.ToString()},
                {"<averageDeviationDiametr>", data.averageDeviationDiametr.ToString()},
                {"<dMax>", data.dMax.ToString()},
                {"<dMin>", data.dMin.ToString()},
                {"<squereSliceMax>",  data.squereSliceMax},
                {"<squereSliceMin>", data.squereSliceMin},
                {"<squereSliceMediana>", data.squereSliceMediana},
                {"<dateTime_Scan>", data.dateTime},
                {"<dateTime_Form>", DateTime.Now.ToString()},
                {"<owner>", data.owner}
            };
            
            word.Process(
                items, 
                filesItems, 
                //charts,
                data.Picture,
                data.GroupInGropupsDefect,
                data.dataDefectArray,
                data.pasportNumber,
                data.createExcell,
                data.squereSlicePictures);
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

        private void scanPasport_Load(object sender, EventArgs e)
        {

        }

        private void Image_Click(object sender, EventArgs e)
        {
            data.squereSlicePictures = new List<string>();
            OpenFileDialog file1 = new OpenFileDialog();
            if (file1.ShowDialog() == DialogResult.OK)
            {
                data.squereSlicePictures.Add(file1.FileName);                
            }
            OpenFileDialog file2 = new OpenFileDialog();
            if (file2.ShowDialog() == DialogResult.OK)
            {
                data.squereSlicePictures.Add(file2.FileName);
            }
            OpenFileDialog file3 = new OpenFileDialog();
            if (file3.ShowDialog() == DialogResult.OK)
            {
                data.squereSlicePictures.Add(file3.FileName);
            }
        }

        private void Excell_CheckedChanged(object sender, EventArgs e)
        {
            data.createExcell = Excell.Checked;
        }
    }
}
