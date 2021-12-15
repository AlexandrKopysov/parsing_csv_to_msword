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
            
            Csv csv = new Csv(data);
            data = csv.Import();

            var items = new Dictionary<string, string>
            {
                {"<pasportNumber>", data.pasportNumber},
                {"<averageDiametr>", data.averageDiametr.ToString()},
                {"<averageNonRoundless>", data.averageNonRoundless.ToString()},
                {"<dMax>", data.dMax.ToString()},
                {"<dMin>", data.dMin.ToString()},
                {"<fileScanGeometry>", data.fileScanGeometry},
                {"<fileScandeffect>", data.fileScandeffect},
                {"<fileSpectr>", data.fileSpectr},
                {"<dateTime>", data.dateTime},
                {"<owner>", data.owner}
            };

            word.Process(items);


        }

        private void fileScanGeometry_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                data.fileScanGeometry = ofd.FileName;                
                fileScanGeometry_textBox.Text = data.fileScanGeometry;
            }
        }

        private void fileScandeffect_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                data.fileScandeffect = ofd.FileName;
                fileScandeffect_textBox.Text = data.fileScandeffect;
            }
        }

        private void fileSpectr_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                data.fileSpectr = ofd.FileName;
                fileSpectr_textBox.Text = data.fileSpectr;
            }
        }
    }
}
