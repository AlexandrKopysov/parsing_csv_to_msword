
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
            this.SuspendLayout();
            // 
            // change_button
            // 
            this.change_button.Location = new System.Drawing.Point(324, 93);
            this.change_button.Name = "change_button";
            this.change_button.Size = new System.Drawing.Size(150, 35);
            this.change_button.TabIndex = 1;
            this.change_button.Text = "Изменить";
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
            // scanPasport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 159);
            this.Controls.Add(this.fileSpectr_button);
            this.Controls.Add(this.fileScandeffect_button);
            this.Controls.Add(this.fileScanGeometry_button);
            this.Controls.Add(this.change_button);
            this.Name = "scanPasport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Паспорт сканирования";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button change_button;
        private System.Windows.Forms.Button fileScanGeometry_button;
        private System.Windows.Forms.Button fileScandeffect_button;
        private System.Windows.Forms.Button fileSpectr_button;
    }
}

