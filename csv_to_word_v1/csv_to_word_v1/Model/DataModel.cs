using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace csv_to_word_v1.Model
{
    /// <summary>
    /// Модель данных , взятых из csv файла
    /// </summary>
    public class DataModel
    {
        /// <summary>
        /// Паспорт сканирования
        /// </summary>
        public string pasportNumber { get; set; }
        /// <summary>
        /// Средневзвешенный диаметр по длине
        /// </summary>
        public double averageDiametr { get; set; }
        /// <summary>
        /// Среднеквадратичное отклонение диаметра по длине
        /// </summary>
        public double averageDeviationDiametr { get; set; }
        /// <summary>
        /// Относительное среднее значение некруглости по длине
        /// </summary>
        public double averageNonRoundless { get; set; }
        ///
        ///Среднеквадратичное отклонение по длине для деффекта
        ///
        public double averageDeviationOnDefect;
        /// <summary>
        /// Максимальное значение диаметра
        /// </summary>
        public double dMax { get; set; }
        /// <summary>
        /// Минимальное значение диаметра
        /// </summary>
        public double dMin { get; set; }
        /// <summary>
        /// Путь к файлу сканирования геометрии
        /// </summary>
        public string fileScanGeometry { get; set; }
        /// <summary>
        /// Путь к файлу сканирования деффектов
        /// </summary>
        public string fileScanDefect { get; set; }
        /// <summary>
        /// Путь к файлу измерений спектров пропускания/отражения
        /// </summary>
        public string fileSpectr { get; set; }
        /// <summary>
        /// Дата формирования паспорта
        /// </summary>
        public string dateTime { get; set; }
        /// <summary>
        /// Оператор
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// График средних значений по срезам от длины
        /// </summary>
        public string chart1 { get; set; }
        /// <summary>
        /// График средних значений по некруглости от длины
        /// </summary>
        public string chart2 { get; set; }

        /// <summary>
        /// Путь до файла шаблона
        /// </summary>
        public string fileTemplate { get; set; }
        /// <summary>
        /// Путь до csv файла
        /// </summary>
        public string fileCsv { get; set; }
        /// <summary>
        /// Массив строк основного файла
        /// </summary>
        public List<Row> dataArray { get; set; }
        /// <summary>
        /// Массив строк файла дефекта
        /// </summary>
        public List<DefectRow> dataDefectArray { get; set; }
        /// <summary>
        /// +3сигма для файла деффекта
        /// </summary>
        public double defectPlus3sigma { get; set; }
        /// <summary>
        /// -3сигма для файла деффекта
        /// </summary>
        public double defectMinus3sigma { get; set; }       
        /// <summary>
        /// Массив строк
        /// </summary>
        public List<SectionModel> sectionArray { get; set; }
        /// <summary>
        /// Сгруппированный массив деффектов
        /// </summary>
        public List<GroupDefectRow> GroupInGropupsDefect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Picture { get; set; }
    }
}
