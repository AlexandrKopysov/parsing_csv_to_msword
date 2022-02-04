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
    }
}
