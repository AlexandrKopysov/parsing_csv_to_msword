using csv_to_word_v1.Model;
using System;
using System.Collections.Generic;
using System.Text;
using static csv_to_word_v1.Model.DataModel;
using static csv_to_word_v1.Model.Row;
using Microsoft.VisualBasic.FileIO;
using System.Linq;

namespace csv_to_word_v1.Services
{
    
    public class Csv
    {
        
        private DataModel data;
        public Csv(DataModel data)
        {
            this.data = data;
        }
        public DataModel Import()
        {
            getParametersStatic();
            getParametersRows();
            //getAverageDiametr();
            groupByX_MM();
            return data;
        }

        private DataModel groupByX_MM()
        {
            var groupFi = data.dataArray.GroupBy(x => x.X_MM).ToArray();

            List<SectionModel> sectionArray = new List<SectionModel>();
            foreach (var group in groupFi)
            {
                SectionModel scectionModel = new SectionModel();
                scectionModel.key = Convert.ToInt32(group.Key);
                scectionModel.averagInSection = Math.Round(group.Select(x => x.Diameter).Average(), 4);
                sectionArray.Add(scectionModel);                
            }
            data.sectionArray = sectionArray;
            data.averageDiametr = (float)Math.Round(
                Convert.ToDouble(data.sectionArray.Select(x => x.averagInSection).ToArray().Average()), 4);
            foreach(var group in data.sectionArray)
            {
                //group.averageNonRoundless = 
            }

            return data;
        }

        private DataModel getAverageDiametr()
        {
            var arrayDiamter = data.dataArray.Select(x => x.X_MM).ToArray();
            data.averageDiametr = (float)Math.Round(Convert.ToDouble(arrayDiamter.Average()),4);
            return data;
        }

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
