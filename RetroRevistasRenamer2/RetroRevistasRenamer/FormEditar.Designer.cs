namespace RetroRevistasRenamer
{
    partial class FormEditar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.botonRestablecer = new System.Windows.Forms.Button();
            this.botonGuardar = new System.Windows.Forms.Button();
            this.botonCancelar = new System.Windows.Forms.Button();
            this.textBoxCountry = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLanguage = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPublisher = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(93, 43);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(351, 20);
            this.textBoxName.TabIndex = 1;
            // 
            // botonRestablecer
            // 
            this.botonRestablecer.Location = new System.Drawing.Point(12, 305);
            this.botonRestablecer.Name = "botonRestablecer";
            this.botonRestablecer.Size = new System.Drawing.Size(140, 23);
            this.botonRestablecer.TabIndex = 2;
            this.botonRestablecer.Text = "Restablecer";
            this.botonRestablecer.UseVisualStyleBackColor = true;
            this.botonRestablecer.Click += new System.EventHandler(this.botonRestablecer_Click);
            // 
            // botonGuardar
            // 
            this.botonGuardar.Location = new System.Drawing.Point(158, 305);
            this.botonGuardar.Name = "botonGuardar";
            this.botonGuardar.Size = new System.Drawing.Size(140, 23);
            this.botonGuardar.TabIndex = 3;
            this.botonGuardar.Text = "Guardar";
            this.botonGuardar.UseVisualStyleBackColor = true;
            this.botonGuardar.Click += new System.EventHandler(this.botonGuardar_Click);
            // 
            // botonCancelar
            // 
            this.botonCancelar.Location = new System.Drawing.Point(304, 305);
            this.botonCancelar.Name = "botonCancelar";
            this.botonCancelar.Size = new System.Drawing.Size(140, 23);
            this.botonCancelar.TabIndex = 4;
            this.botonCancelar.Text = "Cancelar";
            this.botonCancelar.UseVisualStyleBackColor = true;
            this.botonCancelar.Click += new System.EventHandler(this.botonCancelar_Click);
            // 
            // textBoxCountry
            // 
            this.textBoxCountry.Location = new System.Drawing.Point(93, 69);
            this.textBoxCountry.Name = "textBoxCountry";
            this.textBoxCountry.Size = new System.Drawing.Size(352, 20);
            this.textBoxCountry.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "País";
            // 
            // textBoxLanguage
            // 
            this.textBoxLanguage.Location = new System.Drawing.Point(93, 95);
            this.textBoxLanguage.Name = "textBoxLanguage";
            this.textBoxLanguage.Size = new System.Drawing.Size(351, 20);
            this.textBoxLanguage.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Idioma";
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(93, 121);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(351, 20);
            this.textBoxYear.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Año";
            // 
            // textBoxPublisher
            // 
            this.textBoxPublisher.Location = new System.Drawing.Point(93, 147);
            this.textBoxPublisher.Name = "textBoxPublisher";
            this.textBoxPublisher.Size = new System.Drawing.Size(351, 20);
            this.textBoxPublisher.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Publicador";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Location = new System.Drawing.Point(93, 173);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(351, 123);
            this.textBoxDescription.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Descripción";
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(93, 17);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(351, 20);
            this.textBoxFileName.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Nombre carpeta";
            // 
            // FormEditar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 340);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxPublisher);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxYear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxLanguage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxCountry);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.botonCancelar);
            this.Controls.Add(this.botonGuardar);
            this.Controls.Add(this.botonRestablecer);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.Name = "FormEditar";
            this.Text = "Editar";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button botonRestablecer;
        private System.Windows.Forms.Button botonGuardar;
        private System.Windows.Forms.Button botonCancelar;
        private System.Windows.Forms.TextBox textBoxCountry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLanguage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPublisher;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label label7;
    }
}