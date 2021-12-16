using System;
using System.Collections.Generic;
using System.Text;

namespace csv_to_word_v1.Model
{
    /// <summary>
    /// Модель данных в разрезах
    /// </summary>
    public class SectionModel
    {
        /// <summary>
        /// Ключ - Fi
        /// </summary>
        public int key { get; set; }
        /// <summary>
        /// Cредний размер в срезе
        /// </summary>
        public double averagInSection { get; set; }
        /// <summary>
        /// Относительное среднее значение некруглости по срезу:
        /// </summary>
        public double averageNonRoundless { get; set; }
    }
}
