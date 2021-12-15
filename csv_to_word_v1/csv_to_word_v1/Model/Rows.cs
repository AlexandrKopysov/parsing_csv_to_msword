using System;
using System.Collections.Generic;
using System.Text;

namespace csv_to_word_v1.Model
{
    /// <summary>
    /// Модель строки
    /// </summary>
    public class Row
    {
        public int X_MM { get; set; }
        public float Fi { get; set; }
        public float Diameter { get; set; }
        public float UpPointY_PX { get; set; }
        public float DownPointY_PX { get; set; }
    }
}
