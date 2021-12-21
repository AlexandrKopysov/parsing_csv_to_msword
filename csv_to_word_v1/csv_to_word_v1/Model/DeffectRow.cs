using System;
using System.Collections.Generic;
using System.Text;

namespace csv_to_word_v1.Model
{
    /// <summary>
    /// Модель строки файла деффектов
    /// </summary>
    public class DeffectRow
    {        
        /// <summary>
        /// Отрезок, на котором проводят измерения
        /// </summary>
        public int X_MM { get; set; }
        /// <summary>
        /// Градус измерения 0-360
        /// </summary>
        public float Fi { get; set; }
        /// <summary>
        /// Непонятный параметр
        /// </summary>
        public float Brightness { get; set; }
    }
}
