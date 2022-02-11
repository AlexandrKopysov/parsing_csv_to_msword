using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WordOffice = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using System.Drawing;
using Syncfusion.OfficeChart;
using csv_to_word_v1.Model;
using System.Drawing.Imaging;
using OfficeOpenXml;
using IronXL;
using System.Runtime.InteropServices;
using System.Linq;

namespace csv_to_word_v1.Services
{
    class Word
    {
        private FileInfo _fileInfo;

        public Word(string fileName)
        {
            if (File.Exists(fileName))
            {
                _fileInfo = new FileInfo(fileName);
            } 
            else
            {
                throw new ArgumentException("File not found");
            }
        }

        internal bool Process(
            Dictionary<string, string> items, 
            Dictionary<string, string> filesItems, 
            //Dictionary<string, Array> charts,
            List<string> pictureLink,
            List<GroupDefectRow> GroupInGropupsDefect,
            List<DefectRow> dataDefectArray,            
            string pasportNumber,
            bool excelLoad,
            List<string> squarePicture 
            )
        {
            WordOffice.Application app = null;
            
            try
            {            
                app = new WordOffice.Application();
                Object file = _fileInfo.FullName;
                Object missing = Type.Missing;

                app.Documents.Open(file);
                foreach (var item in items)
                {                                        
                    
                    WordOffice.Find find = app.Selection.Find;
                    find.Text = item.Key;
                    find.Replacement.Text = item.Value;                    

                    Object wrap = WordOffice.WdFindWrap.wdFindContinue;
                    Object replace = WordOffice.WdReplace.wdReplaceAll;
                    find.Execute(
                        FindText: Type.Missing,
                        MatchCase: false,
                        MatchWholeWord: false,
                        MatchWildcards: false,
                        MatchSoundsLike: missing,
                        MatchAllWordForms: false,
                        Forward: true,
                        Wrap: wrap,
                        Format:false,
                        ReplaceWith:missing, 
                        Replace:replace
                    );
                }

                //string newFileName = Path.Combine("E:\\",
                //    DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + " " + pasportNumber + ".docx");
                string newFileName = Path.Combine(_fileInfo.DirectoryName,
                    DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + " " + pasportNumber + ".docx");
                string newFileNameForXls = Path.Combine(_fileInfo.DirectoryName,
                    DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + " " + pasportNumber+ " Карта дефектов" + ".xlsx");
                app.ActiveDocument.SaveAs2(newFileName);
                app.ActiveDocument.Close();                

                //Удаляем верхний колонтитул                
                app.Documents.Open(newFileName);
                WordOffice.HeaderFooter hdr = app.ActiveDocument.Sections[1].Headers[WordOffice.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                hdr.Range.Delete();
                app.ActiveDocument.SaveAs2(newFileName);
                app.ActiveDocument.Close();

                AddDopInformation(filesItems, newFileName, pictureLink, GroupInGropupsDefect, squarePicture);
                if (excelLoad)
                {
                    createExcell(dataDefectArray, newFileNameForXls);
                };

                MessageBox.Show("Файл успешно сформирован");
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка \n" + ex.Message);
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (app != null)
                {
                    //app.Quit();
                    Marshal.FinalReleaseComObject(app);                                        
                }
            }
        }
        /// <summary>
        /// добавление линейного графика
        /// </summary>
        /// <param name="items">Массив данных по графикам</param>
        /// <param name="newFileName">Новое имя файла</param>
        private void addChartsToFile(Dictionary<string, Array> items, string newFileName)
        {
            using (WordDocument document = new WordDocument(newFileName, FormatType.Docx))
            {
                foreach (var item in items)
                {
                    if (items[item.Key] != null)
                    {
                        TextBodyPart textBodyPart = new TextBodyPart(document);
                        WParagraph paragraph = new WParagraph(document);
                        textBodyPart.BodyItems.Add(paragraph);                        
                        object[][] data = new object[items[item.Key].Length][];
                        for (int i = 0; i < items[item.Key].Length; i++)
                        {                            
                            data[i] = new object[1];
                            data[i][0] = ((double[])items[item.Key])[i];
                        }
                                                
                        //Creates and Appends chart to the paragraph
                        WChart chart = paragraph.AppendChart(data, 400 , 200);
                        chart.ChartTitle = "Средний размер в разрезе";
                        chart.ChartTitleArea.FontName = "Times New Roman";
                        chart.ChartTitleArea.Size = 12;
                        chart.ChartType = OfficeChartType.Line;
                        paragraph.ParagraphFormat.Tabs.AddTab(0, TabJustification.Left, TabLeader.Single);
                        paragraph.ParagraphFormat.AfterSpacing = 0;
                        paragraph.ParagraphFormat.BeforeSpacing = 0;
                        paragraph.ParagraphFormat.FirstLineIndent = 10f;
                        paragraph.ParagraphFormat.LineSpacing = 10f;
                        chart.Legend.Delete();
                        chart.ChartArea.Border.LinePattern = OfficeChartLinePattern.None;
                                                

                        document.Replace(item.Key, textBodyPart, false, true);
                        textBodyPart.Clear();
                    };
                }
                document.Save(newFileName);
            }
        }

        /// <summary>
        /// Добавляем гиперссылки, используя доп пакет Nuget
        /// </summary>
        /// <param name="items"></param>
        private void AddDopInformation(
            Dictionary<string, string> items, 
            string newFileName,
            List<string> pictureLink,
            List<GroupDefectRow> GroupInGropupsDefect,
            List<string> squarePicture)
        {

            using (WordDocument document = new WordDocument(newFileName, FormatType.Docx))
            {
                
                if (squarePicture != null)
                {
                    if (squarePicture.Count != 0)
                    {
                        int index = 1;
                        foreach (var picture in squarePicture)
                        {
                            TextBodyPart textBodyPart = new TextBodyPart(document);
                            WParagraph paragraph = new WParagraph(document);
                            textBodyPart.BodyItems.Add(paragraph);
                            using (Bitmap bmp = new Bitmap(picture))
                            {
                                Bitmap bmp1 = new Bitmap(bmp, new Size(190, 160));
                                paragraph.AppendPicture(bmp1);
                            }
                            document.Replace("<squereSlicePictures_" + index + ">", textBodyPart, false, true);
                            textBodyPart.Clear();
                            index++;
                        }
                    }
                }
                                
                foreach(var item in items)
                {            
                    if (items[item.Key] != null)
                    {
                        TextBodyPart textBodyPart = new TextBodyPart(document);
                        WParagraph paragraph = new WParagraph(document);
                        textBodyPart.BodyItems.Add(paragraph);                        
                        paragraph.AppendText("\t");
                        paragraph.ParagraphFormat.Tabs.AddTab(0, TabJustification.Left, TabLeader.Single);
                        paragraph.AppendHyperlink(item.Value, item.Value, HyperlinkType.FileLink);                        
                        paragraph.ParagraphFormat.AfterSpacing = 0;
                        paragraph.ParagraphFormat.BeforeSpacing = 0;                                                
                        paragraph.ParagraphFormat.FirstLineIndent = 10f;
                        paragraph.ParagraphFormat.LineSpacing = 10f;
                        document.Replace(item.Key, textBodyPart, false, true);
                        textBodyPart.Clear();
                    };                    
                }

                if (pictureLink != null)
                {
                    if (pictureLink.Count != 0)
                    {
                        TextBodyPart textBodyPart = new TextBodyPart(document);
                        WParagraph paragraph = new WParagraph(document);
                        textBodyPart.BodyItems.Add(paragraph);
                        foreach (string link in pictureLink)
                        {
                            using (Bitmap bmp = new Bitmap(link))
                            {
                                Bitmap bmp1 = new Bitmap(bmp, new Size(615, 70));
                                paragraph.AppendPicture(bmp1);
                            }
                        }
                        document.Replace("<mapDeffect>", textBodyPart, false, true);
                        textBodyPart.Clear();
                    }
                }
                if (GroupInGropupsDefect != null)
                {
                    if (GroupInGropupsDefect.Count != 0)
                    {
                        TextBodyPart textBodyPart = new TextBodyPart(document);
                        WParagraph paragraph = new WParagraph(document);
                        textBodyPart.BodyItems.Add(paragraph);
                        int index = 1;
                        foreach (var row in GroupInGropupsDefect)
                        {
                            paragraph.ParagraphFormat.Tabs.AddTab(0, TabJustification.Left, TabLeader.Single);
                            paragraph.AppendText("\n\tДефект №" + index + "\n\t" + "расположение по длине: " + row.min_X_MM + "..." + row.max_X_MM + " мм;\n" +
                                "\tрасположение по углу:" + row.min_Fi + "..." + row.max_Fi + ";\n" +
                                "\tусловная площадь " + row.square + " мм2.\n");
                            index++;
                        }
                        paragraph.ParagraphFormat.AfterSpacing = 0;
                        paragraph.ParagraphFormat.BeforeSpacing = 0;
                        //paragraph.ParagraphFormat.FirstLineIndent = 10f;
                        //paragraph.ParagraphFormat.LineSpacing = 10f;
                        document.Replace("<groupsDeffect>", textBodyPart, false, true);
                        textBodyPart.Clear();
                    }
                }
                
                document.Save(newFileName);
            }
        }
        
        private void createExcell(List<DefectRow> dataDefectArray, string newFileName)
        {            
            Excel.Application application = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Sheets worksheets = null;
            Excel.Worksheet worksheet = null;
            //переменная для хранения диапазона ячеек
            //в нашем случае - это будет одна ячейка
            Excel.Range cell = null;
            try
            {
                application = new Excel.Application
                {
                    Visible = false
                };
                workbooks = application.Workbooks;
                workbook = workbooks.Add();
                worksheets = workbook.Worksheets; //получаем доступ к коллекции рабочих листов
                worksheet = worksheets.Item[1];//получаем доступ к первому листу
                var x_MM_Min = dataDefectArray.Select(x => x.X_MM).Min();
                var x_MM_Max = dataDefectArray.Select(x => x.X_MM).Max();
                var round = x_MM_Max - x_MM_Min;
                for (int i = 0; i <= round; i++)
                {
                    cell = worksheet.Cells[i + 2 , 1];
                    cell.Value = x_MM_Min + i;
                    cell.BorderAround2();
                }
                for (int i = 2; i <= 360; i++)
                {
                    cell = worksheet.Cells[1 ,i];
                    cell.Value = i-2;
                    cell.BorderAround2();
                }
                workbook.SaveAs(newFileName);
                var groupOnX_MM = dataDefectArray.GroupBy(x => x.X_MM).ToArray();
                int index_X_MM = 1;
                foreach (var row in groupOnX_MM)
                {                    
                    index_X_MM++;
                    int keyX_MM = (int)row.Key;
                    foreach (var rowFi in row)
                    {
                        //Console.WriteLine(rowFi);
                        Excel.Range cellSecond = null;                        
                        worksheet.Cells[index_X_MM, rowFi.Fi + 2] = rowFi.Brightness.ToString();
                        cellSecond = worksheet.Cells[index_X_MM, rowFi.Fi + 2];
                        int color = Convert.ToInt32(255 - (255 * rowFi.Brightness));
                        cellSecond.Interior.Color = System.Drawing.Color.FromArgb(255, color, color);
                    }
                    workbook.Save();                    
                }
                workbook.Save();
                workbook.Close();                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);                
            }
            finally
            {
                //освобождаем память, занятую объектами
                if(application != null)
                {
                    //application.Quit();
                    Marshal.FinalReleaseComObject(application);
                }
                Marshal.ReleaseComObject(worksheet);
                Marshal.ReleaseComObject(worksheets);
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(workbooks);
                Marshal.ReleaseComObject(application);
            }
        }
    }
}
