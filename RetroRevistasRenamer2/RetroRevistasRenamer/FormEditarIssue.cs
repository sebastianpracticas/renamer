using System;
using System.IO;
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
    public partial class FormEditarIssue : Form
    {
        Almacen almacen;
        Herramientas herramientas;
        TreeView treeView1;
        Issue issue;

        public FormEditarIssue(Almacen almacen, Herramientas herramientas, TreeView treeView1, Issue issue)
        {
            InitializeComponent();
            this.almacen = almacen;
            this.herramientas = herramientas;
            this.treeView1 = treeView1;
            this.issue = issue;
            cargarDatosPrevios();
        }

        private void cargarDatosPrevios()
        {
            textBoxName.Text = this.issue.IssueName;
            textBoxNumber.Text = this.issue.IssueNumber;
        }

        private void botonCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void botonRestablecer_Click(object sender, EventArgs e)
        {
            cargarDatosPrevios();
        }

        private void botonGuardar_Click(object sender, EventArgs e)
        {
            bool continuarGuardado = true;
            if (textBoxName.Text.Trim() != issue.IssueName)
            {
                try
                {
                    //Desactivar la protección contra escritura del fichero.
                    issue.desactivarProteccionContraCambios();

                    //Mover el fichero.
                    String rutaCompleta = issue.rutaCompleta();
                    String nuevaRuta = herramientas.quitarUltimoElemento(rutaCompleta) + "\\" + textBoxName.Text.Trim();
                    File.Move(rutaCompleta, nuevaRuta);
                    issue.IssueName = textBoxName.Text.Trim();
                }
                catch
                {
                    MessageBox.Show("Algo salió mal. No se ha podido renombrar el fichero o directorio.\nNo se ha guardado ningún cambio", "Error");
                    continuarGuardado = false;
                }
                finally
                {
                    issue.activarProteccionContraCambios();
                }
            }

            if (continuarGuardado)
            {
                issue.IssueNumber = textBoxNumber.Text.Trim();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
