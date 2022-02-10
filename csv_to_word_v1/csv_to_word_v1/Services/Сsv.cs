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
    public interface IList<T> : ICollection<T> { };

    public class Сsv
    {
        private List<SectionDeffectModel> sectionDefectArray = new List<SectionDeffectModel>();
        private List<DefectRow> listDefectRowArray = new List<DefectRow>();        

        /// <summary>
        /// Граница выборки
        /// </summary>
        private int maxValueInTheSample;

        private DataModel data;
        public Сsv(DataModel data)
        {
            this.data = data;
        }
        public DataModel Import()
        {
            getParametersStatic();            
            getParametersRows();
            groupByX_MM();

            if (data.fileScanDefect != null)
            {
                getParametersDeffectRows();                
                sectionDefectArray = groupDeffectsOnX_MM();
                data.GroupInGropupsDefect = getGroupInGropupsDefect();
                listDefectRowArray = groupDefectsForGroup(sectionDefectArray);
                data.Picture = GroupImg(sectionDefectArray.Count());
            }            

            return data;
        }
        /// <summary>
        /// Создание групп деффектов, находящихся поблизости друг от друга
        /// </summary>
        private List<DefectRow> groupDefectsForGroup(List<SectionDeffectModel> sectionDefectArray)
        {
            List<DefectRow> newArrayDefectRow = new List<DefectRow>();
            
            foreach (var defect in sectionDefectArray)
            {
                Console.WriteLine(defect);
            }

            return newArrayDefectRow;
        }

        public List<string> GroupImg(int maxYCells)
        {
            int pointStart = 0;
            int pointFinish = 0;
            int maxYCount = 15;
            int fileIndex = 0;
            List<string> links = new List<string>();
            for (int indexForward = 0; indexForward < maxYCells; indexForward++)
            {
                int balance = indexForward % maxYCount;
                if (indexForward!=0 && balance == 0)
                {
                    fileIndex++;
                    pointFinish = indexForward-1;
                    links.Add(CreateGridImage(fileIndex, 360, sectionDefectArray.Count(), pointStart, pointFinish, maxYCount, 10));
                    pointStart = indexForward;                    
                }
            }
            fileIndex++;
            links.Add(CreateGridImage(fileIndex, 360, sectionDefectArray.Count(), pointStart, maxYCells-1, maxYCount, 10));
            return links;
        }


        public string CreateGridImage(
            int fileIndex,
            int maxXCells,
            int maxYCells,
            int pointStart,
            int pointFinish,
            int maxYCount,
            int boxSize)
        {
 
            using (var bmp = new System.Drawing.Bitmap(maxXCells * boxSize + 1, maxYCount * boxSize + 1))
            {
                
                
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    //Для линий
                    Pen pen = new Pen(Color.Black);
                    pen.Width = 1;

                    //Группируем карту деффектов по X_MM
                    var indexInBox = 0;
                    var groupX_MM = data.dataDefectArray.GroupBy(x => x.X_MM).ToArray();
                    for (var index = pointStart; index <= pointFinish; index++)
                    {
                       
                        foreach(var cell in groupX_MM[index])
                        {

                            int red = 255;
                            int green = 0;
                            int blue = 0;
                            green = Convert.ToInt32(255 - (255 * cell.Brightness));
                            blue = Convert.ToInt32(255 - (255 * cell.Brightness));
                            Brush color = new SolidBrush(Color.FromArgb(255, red, green, blue));
                            Rectangle rect = new Rectangle((boxSize * (int)cell.Fi), (boxSize * indexInBox), boxSize, boxSize);
                            g.FillRectangle(color, rect);
                        }
                        indexInBox++;
                    }                    

                    //Draw horizontal lines
                    //for (int i = 0; i <= maxXCells; i++)
                    //{
                    //    g.DrawLine(pen, (i * boxSize), 0, i * boxSize, boxSize * maxYCells);
                    //}

                    ////Draw vertical lines            
                    //for (int i = 0; i <= maxYCells; i++)
                    //{
                    //    g.DrawLine(pen, 0, (i * boxSize), boxSize * maxXCells, i * boxSize);
                    //}
                }
                using (var memStream = new MemoryStream())
                {
                    bmp.Save(memStream, ImageFormat.Jpeg);
                    var img = Image.FromStream(memStream);
                    img.Save("C:\\Temp\\file_"+ fileIndex .ToString() + ".png");
                    return "C:\\Temp\\file_" + fileIndex.ToString() + ".png";
                }                
            }            
        }

        /// <summary>
        /// Группировка деффектов по X_MM
        /// </summary>
        /// <param name="bundDefect"></param>
        /// <returns></returns>
        private List<SectionDeffectModel> getGroupDefectOn_X_MM(List<IGrouping<int,DefectRow>> bundDefect)
        {
            List<SectionDeffectModel> groupDefect = new List<SectionDeffectModel>();

            foreach (var defectGroup in bundDefect)
            {             
                int oldIndex = 0;
                int newIndex = 0;
                List<GroupDefectRow> group = new List<GroupDefectRow>();
                foreach (var defectRow in defectGroup)
                {
                    newIndex = (int)defectRow.Fi;
                    if (oldIndex != newIndex - 1)
                    {
                        group.Add(new GroupDefectRow()
                        {
                            key = (int)defectRow.X_MM,
                            DefectRowInGroup = new List<DefectRow>()
                        });

                    }
                    oldIndex = newIndex;
                    DefectRow defRow = new DefectRow();
                    defRow.Brightness = defectRow.Brightness;
                    defRow.Fi = defectRow.Fi;
                    defRow.X_MM = defectRow.X_MM;

                    group[group.Count - 1].DefectRowInGroup.Add(defRow);
                }
                if (group[group.Count - 1].DefectRowInGroup.Count != 1)
                {
                    groupDefect.Add(new SectionDeffectModel()
                    {
                        key = (int)defectGroup.Key,
                        groupDefectArray = group
                    });
                }
            }            
            return groupDefect;
        }
        /// <summary>
        /// Группировка деффектов по Fi
        /// </summary>
        /// <param name="bundDefect"></param>
        /// <returns></returns>
        private List<SectionDeffectModel> getGroupDefectOn_Fi(List<IGrouping<float, DefectRow>> bundDefect)
        {
            List<SectionDeffectModel> groupDefect = new List<SectionDeffectModel>();

            foreach (var defectGroup in bundDefect)
            {
                int oldIndex = 0;
                int newIndex = 0;
                List<GroupDefectRow> groups = new List<GroupDefectRow>();
                foreach (var defectRow in defectGroup)
                {
                    newIndex = (int)defectRow.X_MM;
                    if (oldIndex != newIndex - 1)
                    {
                        groups.Add(new GroupDefectRow()
                        {
                            key = (int)defectRow.Fi,
                            DefectRowInGroup = new List<DefectRow>()
                        });

                    }
                    oldIndex = newIndex;
                    DefectRow defRow = new DefectRow();
                    defRow.Brightness = defectRow.Brightness;
                    defRow.Fi = defectRow.Fi;
                    defRow.X_MM = defectRow.X_MM;

                    groups[groups.Count - 1].DefectRowInGroup.Add(defRow);
                }
                groups.RemoveAll(x => x.DefectRowInGroup.Count < 2);
                foreach (var group in groups)
                {
                    
                    if (group.DefectRowInGroup.Count != 1)
                    {
                        
                        groupDefect.Add(new SectionDeffectModel()
                        {
                            key = (int)defectGroup.Key,
                            groupDefectArray = groups
                        });
                    }
                }
                
            }
            return groupDefect;
        }
        /// <summary>
        /// Костыльный медод по группировке
        /// </summary>
        /// <param name="bundDefect"></param>
        /// <returns></returns>
        private List<GroupDefectRow> getGroupInGrous(List<SectionDeffectModel> bundDefect_XMM , List<SectionDeffectModel> bundDefect_Fi)
        {
            List<SectionDeffectModel> groupDefect = new List<SectionDeffectModel>();
            int groupIndex = 0;
            List<GroupDefectRow> groupDefectRow = new List<GroupDefectRow>();
            foreach (var defectGroup_X_MM in bundDefect_XMM)
            {
                var rows_X_MM = defectGroup_X_MM.groupDefectArray.Select(x => x.DefectRowInGroup).ToArray();
                var key_X_MM = defectGroup_X_MM.key;
                List<DefectRow> newRows = new List<DefectRow>();
                
                foreach (var defectGroup_Fi in bundDefect_Fi)
                {
                    Console.WriteLine(defectGroup_X_MM);

                    var rows_Fi = defectGroup_Fi.groupDefectArray.Select(x => x.DefectRowInGroup).ToArray();
                    var key_Fi = defectGroup_Fi.key;
                    foreach (var row_X_MM in rows_X_MM)
                    {
                        var fi_In_X_MM = row_X_MM.Select(x => x.Fi).ToArray();
                        foreach (var row_Fi in rows_Fi)
                        {
                            var X_MM_In_Fi = row_Fi.Select(x => x.X_MM).Where(i => i == key_X_MM).ToArray();
                            var find_Duble = newRows.Where(x => x.Fi == key_Fi && x.X_MM == key_X_MM).ToArray().Count();
                            if (X_MM_In_Fi.Length > 0 && find_Duble == 0)
                            {
                                newRows.AddRange(row_Fi);
                            }
                        }
                    }
                }
                if (newRows.Count > 0)
                {
                    if (groupDefectRow.Count > 0)
                    {
                        var select_X_MM = groupDefectRow.Where(x => x.DefectRowInGroup.Select(y => y.X_MM).Where(y => y == key_X_MM).ToArray().Count() != 0).Count();
                        if (select_X_MM == 0)
                        {
                            groupIndex++;                            
                            groupDefectRow.Add(new GroupDefectRow
                            {
                                key = groupIndex,
                                DefectRowInGroup = newRows,
                                max_Fi = newRows.Select(x => x.Fi).Max(),
                                min_Fi = newRows.Select(x => x.Fi).Min(),
                                max_X_MM = newRows.Select(x => x.X_MM).Max(),
                                min_X_MM = newRows.Select(x => x.X_MM).Min(),
                                square = (double)newRows.Count() / 10
                            });
                        }
                    } else
                    {
                        groupIndex++;                        
                        groupDefectRow.Add(new GroupDefectRow
                        {
                            key = groupIndex,
                            DefectRowInGroup = newRows,
                            max_Fi = newRows.Select(x => x.Fi).Max(),
                            min_Fi = newRows.Select(x => x.Fi).Min(),
                            max_X_MM = newRows.Select(x => x.X_MM).Max(),
                            min_X_MM = newRows.Select(x => x.X_MM).Min(),
                            square = (double)newRows.Count() / 10
                    });
                    }                    
                }
            }                
            return groupDefectRow;
        }
        
        
        private List<GroupDefectRow> getGroupInGropupsDefect()
        {
            //Новые границы деффекта (Если > 0.5 , до деффект)
            var boundDefect = 0.5;
            var groupX_MM = data.dataDefectArray.GroupBy(x => x.X_MM).ToArray();
            var bundDefect_X_MM = data.dataDefectArray.Where(x => x.Brightness >= boundDefect).OrderBy(x => x.X_MM).GroupBy(x => x.X_MM).ToList();
            var bundDefect_Fi = data.dataDefectArray.Where(x => x.Brightness >= boundDefect).OrderBy(x => x.Fi).GroupBy(x => x.Fi).ToList();
            List<SectionDeffectModel> groupDefect_X_MM = getGroupDefectOn_X_MM(bundDefect_X_MM);
            List<SectionDeffectModel> groupDefect_Fi = getGroupDefectOn_Fi(bundDefect_Fi);
            List<GroupDefectRow> groupInGropups = getGroupInGrous(groupDefect_X_MM, groupDefect_Fi);
            return groupInGropups;
        }
        /// <summary>
        /// Группировка параметров деффекта по X_MM
        /// </summary>
        private List<SectionDeffectModel> groupDeffectsOnX_MM()
        {                 
            var groupX_MM = data.dataDefectArray.GroupBy(x => x.X_MM).ToArray();            
            maxValueInTheSample = groupX_MM.First().Count();
            double mainAverageValue = Math.Round(data.dataDefectArray.Select(x => x.Brightness).ToArray().Average(), 3);          
            List<SectionDeffectModel> sectionDeffectArray = new List<SectionDeffectModel>();            
            foreach (var group in groupX_MM)
            {
                SectionDeffectModel sectionDeffectModel = new SectionDeffectModel();
                sectionDeffectModel.key = Convert.ToInt32(group.Key);
                sectionDeffectModel.averageValue = Math.Round(group.Select(x => x.Brightness).Average(), 3);
                sectionDeffectModel.averageDeviationOnSection = Math.Round(Math.Pow((sectionDeffectModel.averageValue - mainAverageValue),2), 3);
                maxValueInTheSample = group.Count() > maxValueInTheSample ? group.Count() : maxValueInTheSample;
                sectionDeffectArray.Add(sectionDeffectModel);
            }

            data.averageDeviationOnDefect = Math.Round(
                Math.Sqrt(sectionDeffectArray.Select(
                    x => x.averageDeviationOnSection).Sum() / (sectionDeffectArray.Count() - 1)),3);
            
            //+-3Сигма
            data.defectMinus3sigma = Math.Round(mainAverageValue - data.averageDeviationOnDefect * 3 , 3);
            data.defectPlus3sigma = Math.Round(mainAverageValue + data.averageDeviationOnDefect * 3, 3);         

            //Массивы отклонений +-3сигма
            for (var index_1 = 0; index_1 < groupX_MM.Count(); index_1++)
            {
                sectionDeffectArray[index_1].defectMinus3sigmaArray = groupX_MM[index_1].Where(row => row.Brightness <= data.defectMinus3sigma).ToList();
                sectionDeffectArray[index_1].defectPlus3sigmaArray = groupX_MM[index_1].Where(row => row.Brightness >= data.defectPlus3sigma).ToList();
            }

            
            

            return sectionDeffectArray;
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
            TextFieldParser tfp = new TextFieldParser(data.fileScanDefect);
            List<DefectRow> rows = new List<DefectRow>();
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
                        DefectRow row = new DefectRow();
                        row.X_MM = int.Parse(fields[0]);
                        row.Fi = float.Parse(fields[1]);
                        row.Brightness = float.Parse(fields[2]);                        
                        rows.Add(row);
                    }
                    rowIndex++;
                }
            }
            data.dataDefectArray = rows;            
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
