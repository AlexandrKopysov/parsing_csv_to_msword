
namespace csv_to_word_v1
{
    partial class scanPasport
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.change_button = new System.Windows.Forms.Button();
            this.fileScanGeometry_button = new System.Windows.Forms.Button();
            this.fileScandeffect_button = new System.Windows.Forms.Button();
            this.fileSpectr_button = new System.Windows.Forms.Button();
            this.Image = new System.Windows.Forms.Button();
            this.Excell = new System.Windows.Forms.CheckBox();
            this.square_1 = new System.Windows.Forms.TextBox();
            this.square_2 = new System.Windows.Forms.TextBox();
            this.square_3 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // change_button
            // 
            this.change_button.Location = new System.Drawing.Point(324, 149);
            this.change_button.Name = "change_button";
            this.change_button.Size = new System.Drawing.Size(150, 35);
            this.change_button.TabIndex = 1;
            this.change_button.Text = "Старт";
            this.change_button.UseVisualStyleBackColor = true;
            this.change_button.Click += new System.EventHandler(this.change_button_Click);
            // 
            // fileScanGeometry_button
            // 
            this.fileScanGeometry_button.Location = new System.Drawing.Point(12, 12);
            this.fileScanGeometry_button.Name = "fileScanGeometry_button";
            this.fileScanGeometry_button.Size = new System.Drawing.Size(150, 35);
            this.fileScanGeometry_button.TabIndex = 2;
            this.fileScanGeometry_button.Text = "Файл геометрии";
            this.fileScanGeometry_button.UseVisualStyleBackColor = true;
            this.fileScanGeometry_button.Click += new System.EventHandler(this.fileScanGeometry_button_Click);
            // 
            // fileScandeffect_button
            // 
            this.fileScandeffect_button.Location = new System.Drawing.Point(168, 12);
            this.fileScandeffect_button.Name = "fileScandeffect_button";
            this.fileScandeffect_button.Size = new System.Drawing.Size(150, 35);
            this.fileScandeffect_button.TabIndex = 3;
            this.fileScandeffect_button.Text = "Файл дефектов";
            this.fileScandeffect_button.UseVisualStyleBackColor = true;
            this.fileScandeffect_button.Click += new System.EventHandler(this.fileScandeffect_button_Click);
            // 
            // fileSpectr_button
            // 
            this.fileSpectr_button.Location = new System.Drawing.Point(324, 12);
            this.fileSpectr_button.Name = "fileSpectr_button";
            this.fileSpectr_button.Size = new System.Drawing.Size(150, 35);
            this.fileSpectr_button.TabIndex = 4;
            this.fileSpectr_button.Text = "Файл спектров";
            this.fileSpectr_button.UseVisualStyleBackColor = true;
            this.fileSpectr_button.Click += new System.EventHandler(this.fileSpectr_button_Click);
            // 
            // Image
            // 
            this.Image.Location = new System.Drawing.Point(12, 62);
            this.Image.Name = "Image";
            this.Image.Size = new System.Drawing.Size(150, 35);
            this.Image.TabIndex = 5;
            this.Image.Text = "Выбрать изображения";
            this.Image.UseVisualStyleBackColor = true;
            this.Image.Click += new System.EventHandler(this.Image_Click);
            // 
            // Excell
            // 
            this.Excell.AutoSize = true;
            this.Excell.Checked = true;
            this.Excell.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Excell.Location = new System.Drawing.Point(324, 71);
            this.Excell.Name = "Excell";
            this.Excell.Size = new System.Drawing.Size(144, 19);
            this.Excell.TabIndex = 6;
            this.Excell.Text = "Формирование Excell";
            this.Excell.UseVisualStyleBackColor = true;
            this.Excell.CheckedChanged += new System.EventHandler(this.Excell_CheckedChanged);
            // 
            // square_1
            // 
            this.square_1.Location = new System.Drawing.Point(12, 103);
            this.square_1.Name = "square_1";
            this.square_1.Size = new System.Drawing.Size(75, 23);
            this.square_1.TabIndex = 7;
            this.square_1.Text = "0";
            // 
            // square_2
            // 
            this.square_2.Location = new System.Drawing.Point(12, 132);
            this.square_2.Name = "square_2";
            this.square_2.Size = new System.Drawing.Size(75, 23);
            this.square_2.TabIndex = 8;
            this.square_2.Text = "0";
            // 
            // square_3
            // 
            this.square_3.Location = new System.Drawing.Point(12, 161);
            this.square_3.Name = "square_3";
            this.square_3.Size = new System.Drawing.Size(75, 23);
            this.square_3.TabIndex = 9;
            this.square_3.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "Макс. Площадь среза";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Мин. Площадь среза";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(96, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "Медианная площадь среза";
            // 
            // scanPasport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 204);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.square_3);
            this.Controls.Add(this.square_2);
            this.Controls.Add(this.square_1);
            this.Controls.Add(this.Excell);
            this.Controls.Add(this.Image);
            this.Controls.Add(this.fileSpectr_button);
            this.Controls.Add(this.fileScandeffect_button);
            this.Controls.Add(this.fileScanGeometry_button);
            this.Controls.Add(this.change_button);
            this.Name = "scanPasport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Паспорт сканирования";
            this.Load += new System.EventHandler(this.scanPasport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button change_button;
        private System.Windows.Forms.Button fileScanGeometry_button;
        private System.Windows.Forms.Button fileScandeffect_button;
        private System.Windows.Forms.Button fileSpectr_button;
        private System.Windows.Forms.Button Image;
        private System.Windows.Forms.CheckBox Excell;
        private System.Windows.Forms.TextBox square_1;
        private System.Windows.Forms.TextBox square_2;
        private System.Windows.Forms.TextBox square_3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

