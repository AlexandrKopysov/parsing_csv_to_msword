using System;
using System.Collections.Generic;
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
        public float averageDiametr { get; set; }
        /// <summary>
        /// Относительное среднее значение некруглости по длине
        /// </summary>
        public float averageNonRoundless { get; set; }
        /// <summary>
        /// Максимальное значение диаметра
        /// </summary>
        public float dMax { get; set; }
        /// <summary>
        /// Минимальное значение диаметра
        /// </summary>
        public float dMin { get; set; }
        /// <summary>
        /// Путь к файлу сканирования геометрии
        /// </summary>
        public string fileScanGeometry { get; set; }
        /// <summary>
        /// Путь к файлу сканирования деффектов
        /// </summary>
        public string fileScandeffect { get; set; }
        /// <summary>
        /// Путь к файлу измерений спектров пропускания/отражения
        /// </summary>
        public string fileSpectr { get; set; }
        /// <summary>
        /// Дата формирования паспорта
        /// </summary>
        public DateTime dateTime { get; set; }
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
        /// Массив строк
        /// </summary>
        public Array dataArray { get; set; }

    }
}
