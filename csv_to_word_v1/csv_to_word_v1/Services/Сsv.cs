using csv_to_word_v1.Model;
using System;
using System.Collections.Generic;
using System.Text;
using static csv_to_word_v1.Model.DataModel;
using static csv_to_word_v1.Model.Row;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace csv_to_word_v1.Services
{
    
    public class Сsv
    {
        
        private DataModel data;
        public Сsv(DataModel data)
        {
            this.data = data;
        }
        public DataModel Import()
        {
            getParametersStatic();            

            getParametersRows();
            getParametersDeffectRows();

            groupByX_MM();
            groupDeffectsOnFi();
            
            CreateGridImage(360,30,0,0,10);            
            return data;
        }        

        public void CreateGridImage(
            int maxXCells,
            int maxYCells,
            int cellXPosition,
            int cellYPosition,
            int boxSize)
        {

            //без группировки по Fi
            double averageDeviation = Math.Round(data.dataDeffectArray.Select(x => x.Brightness).Average(), 3);

            using (var bmp = new System.Drawing.Bitmap(maxXCells * boxSize + 1, maxYCells * boxSize + 1))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Yellow);
                    Pen pen = new Pen(Color.Black);
                    pen.Width = 1;

                    //Draw red rectangle to go behind cross
                    Rectangle rect = new Rectangle(boxSize * (cellXPosition - 1), boxSize * (cellYPosition - 1), boxSize, boxSize);
                    g.FillRectangle(new SolidBrush(Color.Red), rect);

                    //Draw cross
                    g.DrawLine(pen, boxSize * (cellXPosition - 1), boxSize * (cellYPosition - 1), boxSize * cellXPosition, boxSize * cellYPosition);
                    g.DrawLine(pen, boxSize * (cellXPosition - 1), boxSize * cellYPosition, boxSize * cellXPosition, boxSize * (cellYPosition - 1));

                    //Draw horizontal lines
                    for (int i = 0; i <= maxXCells; i++)
                    {
                        g.DrawLine(pen, (i * boxSize), 0, i * boxSize, boxSize * maxYCells);
                    }

                    //Draw vertical lines            
                    for (int i = 0; i <= maxYCells; i++)
                    {
                        g.DrawLine(pen, 0, (i * boxSize), boxSize * maxXCells, i * boxSize);
                    }
                }
                using (var memStream = new MemoryStream())
                {
                    bmp.Save(memStream, ImageFormat.Jpeg);
                    var img = Image.FromStream(memStream);
                    img.Save("D:\\file.jpg");
                }                
            }
        }
        //Группируем параметры деффектов в модель
        private void groupDeffectsOnFi()
        {
            var groupFi = data.dataDeffectArray.GroupBy(x => x.Fi).ToArray();
            List<SectionDeffectModel> sectionDeffectArray = new List<SectionDeffectModel>();
            foreach (var group in groupFi)
            {
                SectionDeffectModel sectionDeffectModel = new SectionDeffectModel();
                sectionDeffectModel.key = Convert.ToInt32(group.Key);
                sectionDeffectModel.averageDeviationOnDegree = Math.Round(group.Select(x => x.Brightness).Average(), 2);                
                sectionDeffectArray.Add(sectionDeffectModel);                
            }
        }

        private DataModel groupByX_MM()
        {
            var groupX_MM = data.dataArray.GroupBy(x => x.X_MM).ToArray();
            //Костыль, взять значения из первого жлемента массива
            double dMax = -10000;
            double dMin = 10000;
            List<SectionModel> sectionArray = new List<SectionModel>();
            foreach (var group in groupX_MM)
            {
                SectionModel scectionModel = new SectionModel();
                scectionModel.key = Convert.ToInt32(group.Key);
                scectionModel.averagInSection = Math.Round(group.Select(x => x.Diameter).Average(), 2);
                scectionModel.dMax = Math.Round(group.Select(x => x.Diameter).Max(), 2);
                scectionModel.dMin = Math.Round(group.Select(x => x.Diameter).Min(), 2);

                sectionArray.Add(scectionModel);
                dMax = dMax < scectionModel.dMax ? scectionModel.dMax : dMax;
                dMin = dMin > scectionModel.dMin ? scectionModel.dMin : dMin;
            }
            data.sectionArray = sectionArray;
            data.averageDiametr = Math.Round(
                Convert.ToDouble(data.sectionArray.Select(x => x.averagInSection).ToArray().Average()), 2);
            
            foreach (var group in data.sectionArray)
            {                
                group.averageNonRoundless = Math.Round(((group.dMax - group.dMin) / data.averageDiametr) * 100, 2);
                group.averageDeviationDiametr = Math.Round(Math.Pow((group.averagInSection - data.averageDiametr),2),2);
            }
            
            data.averageNonRoundless = Math.Round(data.sectionArray.Select(x => x.averageNonRoundless).ToArray().Average(), 2);
            //double k = data.sectionArray.Select(x => x.averageDeviationDiametr).Sum()/ data.sectionArray.Count();
            data.averageDeviationDiametr = Math.Round(
                Math.Sqrt(data.sectionArray
                .Select(x => x.averageDeviationDiametr).Sum() / (data.sectionArray.Count()-1)), 2);

            data.dMax = dMax;
            data.dMin = dMin;            

            return data;
        }

        private DataModel getAverageDiametr()
        {
            var arrayDiamter = data.dataArray.Select(x => x.X_MM).ToArray();
            data.averageDiametr = (float)Math.Round(Convert.ToDouble(arrayDiamter.Average()),4);
            return data;
        }
        /// <summary>
        /// Строки основного csv файла группируем в модель
        /// </summary>
        /// <returns></returns>
        private DataModel getParametersRows()
        {
            TextFieldParser tfp = new TextFieldParser(data.fileCsv);
            List<Row> rows = new List<Row>();
            using (tfp)
            {
                tfp.TextFieldType = FieldType.Delimited;
                tfp.SetDelimiters(";");                
                int rowIndexStart = 4;
                int rowIndex = 0;
                while (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();
                    if (rowIndex >= rowIndexStart)
                    {
                        Row row = new Row();
                        row.X_MM = int.Parse(fields[0]);
                        row.Fi = float.Parse(fields[1]);
                        row.Diameter = float.Parse(fields[2]);
                        row.UpPointY_PX = float.Parse(fields[3]);
                        row.DownPointY_PX = float.Parse(fields[4]);
                        rows.Add(row);
                    }
                    rowIndex++;
                }
            }
            data.dataArray = rows;
            return data;
        }
        /// <summary>
        /// Параметры файла деффекта группируем в модель
        /// </summary>
        /// <returns></returns>
        private DataModel getParametersDeffectRows()
        {
            TextFieldParser tfp = new TextFieldParser(data.fileScanDeffect);
            List<DeffectRow> rows = new List<DeffectRow>();
            using (tfp)
            {
                tfp.TextFieldType = FieldType.Delimited;
                tfp.SetDelimiters(";");
                int rowIndexStart = 1;
                int rowIndex = 0;
                while (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();
                    if (rowIndex >= rowIndexStart)
                    {
                        DeffectRow row = new DeffectRow();
                        row.X_MM = int.Parse(fields[0]);
                        row.Fi = float.Parse(fields[1]);
                        row.Brightness = float.Parse(fields[2]);                        
                        rows.Add(row);
                    }
                    rowIndex++;
                }
            }
            data.dataDeffectArray = rows;            
            return data;
        }
        /// <summary>
        /// Заголовки основного csv файла
        /// </summary>
        /// <returns></returns>
        private DataModel getParametersStatic()
        {
            TextFieldParser tfp = new TextFieldParser(data.fileCsv);
            using (tfp)
            {
                tfp.TextFieldType = FieldType.Delimited;
                tfp.SetDelimiters(";");
                while (!tfp.EndOfData)
                {
                    string fibernumber = "FiberNumber=";
                    string operatorName = "OperatorName=";
                    string DateTimeMeasurement = "DateTimeMeasurement=";
                    string[] fields = tfp.ReadFields();
                    if (fields[0].Contains(fibernumber))
                    {
                        int startIndex = fields[0].IndexOf(fibernumber) + fibernumber.Length + 1;
                        int lenght = fields[0].Length - (fields[0].IndexOf(fibernumber) + fibernumber.Length) - 2;
                        data.pasportNumber = fields[0].Substring(startIndex,lenght);
                    }
                    if (fields[0].Contains(operatorName))
                    {
                        int startIndex = fields[0].IndexOf(operatorName) + operatorName.Length + 1;
                        int lenght = fields[0].Length - (fields[0].IndexOf(operatorName) + operatorName.Length) - 2;
                        data.owner = fields[0].Substring(startIndex, lenght);
                    }
                    if (fields[0].Contains(DateTimeMeasurement))
                    {
                        int startIndex = fields[0].IndexOf(DateTimeMeasurement) + DateTimeMeasurement.Length;
                        int lenght = fields[0].Length - (fields[0].IndexOf(DateTimeMeasurement) + DateTimeMeasurement.Length)-5;
                        data.dateTime = fields[0].Substring(startIndex, lenght);
                        break;
                    }
                }
            }
            return data;
        }
    }
}
