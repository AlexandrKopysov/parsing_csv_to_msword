using System;
using System.Collections.Generic;
using System.Text;

namespace csv_to_word_v1.Model
{
    /// <summary>
    /// Модель данных файла деффекта
    /// </summary>
    public class SectionDeffectModel
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public int key { get; set; }
        /// <summary>
        /// Среднее значение
        /// </summary>
        public double averageValue { get; set; }
        /// <summary>
        /// Массив деффектов +3сигма
        /// </summary>
        public List<DefectRow> defectPlus3sigmaArray { get; set; }
        /// <summary>
        /// Массив деффектов -3сигма
        /// </summary>
        public List<DefectRow> defectMinus3sigmaArray { get; set; }
        /// <summary>
        /// среднеквадратичное отклонение
        /// </summary>
        public double averageDeviationOnSection { get; set; }
        /// <summary>
        /// Максимальное значение в выборке (Fi или X_MM)
        /// </summary>
        public int maxValueInTheSampleOnSetcion { get; set; }
        /// <summary>
        /// Минимальное значение в выборке (Fi или X_MM)
        /// </summary>
        public int minValueInTheSample { get; set; }
        
    }
}
