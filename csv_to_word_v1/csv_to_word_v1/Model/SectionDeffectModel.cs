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
        /// Ключ - Fi
        /// </summary>
        public int key { get; set; }
        /// <summary>
        /// Среднее значение по градусу
        /// </summary>
        public double averageValueOnDegree { get; set; }
        /// <summary>
        /// среднеквадратичное отклонение по градусу
        /// </summary>
        public double averageDeviationOnDegree { get; set; }
    }
}
