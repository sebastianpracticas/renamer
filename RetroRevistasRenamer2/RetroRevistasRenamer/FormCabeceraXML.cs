using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetroRevistasRenamer
{
    public partial class FormCabeceraXML : Form
    {
        Almacen almacen;

        public FormCabeceraXML(Almacen almacen)
        {
            this.almacen = almacen;
            InitializeComponent();
            cargarValoresPrevios();
        }

        private void cargarValoresPrevios()
        {
            textBoxName.Text = almacen.getName();
            checkBoxName.Checked = almacen.getName() != null;
            textBoxDescription.Text = almacen.getDescription();
            checkBoxDescription.Checked = almacen.getDescription() != null;
            textBoxCategory.Text = almacen.getCategory();
            checkBoxCategory.Checked = almacen.getCategory() != null;
            textBoxAuthor.Text = almacen.getAuthor();
            checkBoxAuthor.Checked = almacen.getAuthor() != null;
            textBoxEmail.Text = almacen.getEmail();
            checkBoxEmail.Checked = almacen.getEmail() != null;
            textBoxHomepage.Text = almacen.getHomepage();
            checkBoxHomepage.Checked = almacen.getHomepage() != null;
            textBoxUrl.Text = almacen.getUrl();
            checkBoxUrl.Checked = almacen.getUrl() != null;
            textBoxComments.Text = almacen.getComments();
            checkBoxComments.Checked = almacen.getComments() != null;
            textBoxFormato.Text = almacen.getFormato();
            checkBoxFormato.Checked = almacen.getFormato() != null;
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox senderCheckBox = (CheckBox) sender;
            String textBoxName = "textBox" + senderCheckBox.Name.Replace("checkBox", "");
            TextBox textBox = this.Controls[textBoxName] as TextBox;
            textBox.ReadOnly = !senderCheckBox.Checked;
            if (!senderCheckBox.Checked)
            {
                textBox.Text = "";
            }
        }

        private void botonGuardar_Click(object sender, EventArgs e)
        {
            almacen.setName(checkBoxName.Checked ? textBoxName.Text.Trim() : null);
            almacen.setDescription(checkBoxDescription.Checked ? textBoxDescription.Text.Trim() : null);
            almacen.setCategory(checkBoxCategory.Checked ? textBoxCategory.Text.Trim() : null);
            almacen.setAuthor(checkBoxAuthor.Checked ? textBoxAuthor.Text.Trim() : null);
            almacen.setEmail(checkBoxEmail.Checked ? textBoxEmail.Text.Trim() : null);
            almacen.setHomepage(checkBoxHomepage.Checked ? textBoxHomepage.Text.Trim() : null);
            almacen.setUrl(checkBoxUrl.Checked ? textBoxUrl.Text.Trim() : null);
            almacen.setComments(checkBoxComments.Checked ? textBoxComments.Text.Trim() : null);
            almacen.setFormato(checkBoxFormato.Checked ? textBoxFormato.Text.Trim() : null);
            this.Close();
        }

        private void botonCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
