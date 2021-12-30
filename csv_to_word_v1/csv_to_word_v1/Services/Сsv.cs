﻿using csv_to_word_v1.Model;
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
        private List<SectionDeffectModel> sectionDeffectArray = new List<SectionDeffectModel>();
        
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
            getParametersDeffectRows();

            groupByX_MM();
            //sectionDeffectArray = groupDeffectsOnFi();
            sectionDeffectArray = groupDeffectsOnX_MM();

            
            CreateGridImage(360, sectionDeffectArray.Count(), 0,0,10);            
            return data;
        }        

        public void CreateGridImage(
            int maxXCells,
            int maxYCells,
            int cellXPosition,
            int cellYPosition,
            int boxSize)
        {

            using (var bmp = new System.Drawing.Bitmap(maxXCells * boxSize + 1, maxYCells * boxSize + 1))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.White);
                    //Для линий
                    Pen pen = new Pen(Color.Black);
                    pen.Width = 1;

                    //Группируем карту деффектов по X_MM
                    var groupX_MM = data.dataDefectArray.GroupBy(x => x.X_MM).ToArray();
                    for (var index = 0; index < maxYCells; index++)
                    {
                        foreach(var cell in groupX_MM[index])
                        {

                            int red = 0;
                            int green = 0;

                            if (cell.Brightness <= 0.5)
                            {
                                red = Convert.ToInt32(255*(cell.Brightness*2));
                                green = 255;
                            } else
                            {
                                red = 255;
                                green = Convert.ToInt32(255 * ((1 - cell.Brightness)*2));
                            }

                            Brush color = new SolidBrush(Color.FromArgb(255, red, green, 0));
                            Rectangle rect = new Rectangle((boxSize * (int)cell.Fi), (boxSize * index), boxSize, boxSize);
                            g.FillRectangle(color, rect);
                        }
                    }

                    //Для прямоугольников
                    //Brush red = new SolidBrush(Color.Red);
                    //Pen redPen = new Pen(red, 10);
                    ////Для прямоугольников
                    //Brush blue = new SolidBrush(Color.Blue);
                    //Pen bluePen = new Pen(red, 10);

                    //for (var index = 0; index < maxYCells; index++)
                    //{
                    //    if (sectionDeffectArray[index].defectMinus3sigmaArray.Count() != 0)
                    //    {
                    //        foreach(var row in sectionDeffectArray[index].defectMinus3sigmaArray)
                    //        {
                    //            Rectangle rect = new Rectangle((boxSize * (int)row.Fi), (boxSize * index), boxSize, boxSize);
                    //            g.FillRectangle(blue, rect);
                    //        }
                    //    }                        
                    //    if (sectionDeffectArray[index].defectPlus3sigmaArray.Count() != 0)
                    //    {
                    //        foreach (var row in sectionDeffectArray[index].defectPlus3sigmaArray)
                    //        {
                    //            Rectangle rect = new Rectangle((boxSize * (int)row.Fi), (boxSize * index), boxSize, boxSize);
                    //            g.FillRectangle(red, rect);
                    //        }
                    //    }
                    //}

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
                    img.Save("D:\\file.png");
                }                
            }
        }
        /// <summary>
        /// Группировка параметров деффекта по Fi
        /// </summary>
        private List<SectionDeffectModel> groupDeffectsOnFi()
        {
            var groupFi = data.dataDefectArray.GroupBy(x => x.Fi).ToArray();
            maxValueInTheSample = groupFi.First().Count();
            double mainAverageValue = Math.Round(data.dataDefectArray.Select(x => x.Brightness).ToArray().Average(), 3);
            List<SectionDeffectModel> sectionDeffectArray = new List<SectionDeffectModel>();
            foreach (var group in groupFi)
            {
                SectionDeffectModel sectionDeffectModel = new SectionDeffectModel();
                sectionDeffectModel.key = Convert.ToInt32(group.Key);
                sectionDeffectModel.averageValue = Math.Round(group.Select(x => x.Brightness).Average(), 3);
                sectionDeffectModel.averageDeviationOnSection = Math.Round(Math.Pow((sectionDeffectModel.averageValue - mainAverageValue), 2), 3);
                maxValueInTheSample = group.Count() > maxValueInTheSample ? group.Count() : maxValueInTheSample;
                sectionDeffectArray.Add(sectionDeffectModel);
            }
            //Показывает 0!!!
            data.averageDeviationOnDefect = Math.Round(
                Math.Sqrt(sectionDeffectArray.Select(
                    x => x.averageDeviationOnSection).Sum() / (sectionDeffectArray.Count() - 1)), 3);

            return sectionDeffectArray;
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
            for (var index = 0; index < groupX_MM.Count(); index++)
            {
                sectionDeffectArray[index].defectMinus3sigmaArray = groupX_MM[index].Where(row => row.Brightness <= data.defectMinus3sigma).ToList();
                sectionDeffectArray[index].defectPlus3sigmaArray = groupX_MM[index].Where(row => row.Brightness >= data.defectPlus3sigma).ToList();
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
