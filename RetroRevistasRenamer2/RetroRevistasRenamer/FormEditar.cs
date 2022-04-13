using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RetroRevistasRenamer
{
    public partial class FormEditar : Form
    {
        Almacen almacen;
        Herramientas herramientas;
        TreeView treeView1;
        Magazine revista;

        public FormEditar(Almacen almacen, Herramientas herramientas, TreeView treeView1, Magazine revista)
        {
            InitializeComponent();
            this.almacen = almacen;
            this.herramientas = herramientas;
            this.treeView1 = treeView1;
            this.revista = revista;

            if (!Directory.Exists(this.revista.rutaCompleta()))
            {
                MessageBox.Show("Error 404. Revista no encontrada. Puede que haya sido borrada o movida.", "Error 404");
                this.Close();
            }

            cargarDatosPrevios();
        }

        private void cargarDatosPrevios()
        {
            textBoxFileName.Text = this.revista.MagazineFileName;
            textBoxName.Text = this.revista.MagazineName;
            textBoxCountry.Text = this.revista.MagazineCountry;
            textBoxLanguage.Text = this.revista.MagazineLanguage;
            textBoxYear.Text = this.revista.MagazineYear;
            textBoxPublisher.Text = this.revista.MagazinePublisher;
            textBoxDescription.Text = this.revista.MagazineDescription;
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
            if (textBoxFileName.Text.Trim() != revista.MagazineFileName)
            {
                try
                {
                    foreach (Issue issue in revista.ListaIssues)
                    {
                        //Eliminar la protección contra escritura de todos los issues de la revista antes de renombrarla.
                        issue.desactivarProteccionContraCambios();
                    }
                    String rutaCompleta = revista.rutaCompleta();
                    String nuevaRuta = herramientas.quitarUltimoElemento(rutaCompleta) + "\\" + textBoxFileName.Text.Trim();
                    Directory.Move(rutaCompleta, nuevaRuta);
                    revista.MagazineFileName = textBoxFileName.Text.Trim();
                    revista.actualizarRutaIssuesInternos();
                }
                catch
                {
                    MessageBox.Show("Algo salió mal. No se ha podido renombrar el directorio.\nNo se ha guardado ningún cambio", "Error");
                    continuarGuardado = false;
                }
                finally
                {
                    foreach (Issue issue in revista.ListaIssues)
                    {
                        //Reactivar protección contra escritura de los issues.
                        issue.activarProteccionContraCambios();
                    }
                }
            }

            if (continuarGuardado)
            {
                revista.MagazineName = textBoxName.Text.Trim();
                revista.MagazineCountry = textBoxCountry.Text.Trim();
                revista.MagazineLanguage = textBoxLanguage.Text.Trim();
                revista.MagazineYear = textBoxYear.Text.Trim();
                revista.MagazinePublisher = textBoxPublisher.Text.Trim();
                revista.MagazineDescription = textBoxDescription.Text.Trim();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
