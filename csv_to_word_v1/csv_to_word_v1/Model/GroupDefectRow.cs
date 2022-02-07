using System;
using System.Collections.Generic;
using System.Text;

namespace csv_to_word_v1.Model
{
    /// <summary>
    /// Клас сгруппированных значений
    /// </summary>
    public class GroupDefectRow
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public int key { get; set; }
        /// <summary>
        /// Массив для группировки значений строк
        /// </summary>
        public List<DefectRow> DefectRowInGroup  { get; set;}
        public float max_Fi { get; set; }
        public float min_Fi { get; set; }
        public int max_X_MM { get; set; }
        public int min_X_MM { get; set; }
        public double square { get; set; }
    }
}
