using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RetroRevistasRenamer;

namespace RetroRevistasRenamer
{
    public partial class Form1 : Form
    {

        Almacen almacen;    //Solo usa getters (nunca setters) desde esta clase.
        LogicaDeInterfaz logicaInterfaz;

        public Form1()
        {
            almacen = new Almacen();
            logicaInterfaz = new LogicaDeInterfaz(this, almacen);
            InitializeComponent();
            logicaInterfaz.actualizarArbol();
            activarBotones();
        }

        public ListView getListView1() {
            return listView1;
        }

        public void setListView1(ListView listView1)
        {
            this.listView1 = listView1;
        }

        public TreeView getTreeView1()
        {
            return treeView1;
        }

        public void setTreeView1(TreeView treeView1)
        {
            this.treeView1 = treeView1;
        }

        public void setLabelEstadoText(String text)
        {
            labelEstado.Text = text;
        }

        public void setLabelCarpetaText(String text)
        {
            labelCarpeta.Text = text;
        }

        public void setXmlFileLabelText(String text)
        {
            xmlFileLabel.Text = " | XML: " + text;
        }

        public void limpiarListView()
        {
            listView1.Items.Clear();
        }

        private void activarBotones()
        {
            mostrarOcultarFicherosOk.Checked = true;
            mostrarOcultarFicherosIncorrecto.Checked = true;
            mostrarOcultarFicherosDesconocido.Checked = true;
            mostrarOcultarRevistaOk.Checked = true;
            mostrarOcultarRevistaIncorrecta.Checked = true;
            mostrarOcultarRevistaDesconocida.Checked = true;
        }

        private void arreglarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            logicaInterfaz.actualizarListView();
        }        

        private void colorearBoton(ToolStripButton boton, Boolean activo)
        {
            boton.Checked = activo;
        }

        private void mostrarOcultarRevistaIncorrecta_Click(object sender, EventArgs e)
        {
            logicaInterfaz.mostrarOcultarRevistas("Incorrecta");
            colorearBoton(mostrarOcultarRevistaIncorrecta, almacen.getMostrarRevistaIncorrecta());
        }

        private void mostrarOcultarRevistaOk_Click(object sender, EventArgs e)
        {
            logicaInterfaz.mostrarOcultarRevistas("Ok");
            colorearBoton(mostrarOcultarRevistaOk, almacen.getMostrarRevistaOk());
        }

        private void mostrarOcultarRevistaDesconocida_Click(object sender, EventArgs e)
        {
            logicaInterfaz.mostrarOcultarRevistas("Desconocida");
            colorearBoton(mostrarOcultarRevistaDesconocida, almacen.getMostrarRevistaDesconocida());
        }

        private void mostrarOcultarFicherosOk_Click(object sender, EventArgs e)
        {
            logicaInterfaz.mostrarOcultarFicheros("Ok");
            colorearBoton(mostrarOcultarFicherosOk, almacen.getMostrarIssuesOk());
        }

        private void mostrarOcultarFicherosIncorrecto_Click(object sender, EventArgs e)
        {
            logicaInterfaz.mostrarOcultarFicheros("Incorrecto");
            colorearBoton(mostrarOcultarFicherosIncorrecto, almacen.getMostrarIssuesIncorrecto());
        }

        private void mostrarOcultarFicherosDesconocido_Click(object sender, EventArgs e)
        {
            logicaInterfaz.mostrarOcultarFicheros("Desconocido");
            colorearBoton(mostrarOcultarFicherosDesconocido, almacen.getMostrarIssuesDesconocido());
        }

        private void xmlFile_Click(object sender, EventArgs e)
        {

        }

        private void botonAbrirCarpeta_Click(object sender, EventArgs e)
        {
            logicaInterfaz.abrirCarpeta();
        }

        private void botonCerrarCarpeta_Click(object sender, EventArgs e)
        {
            logicaInterfaz.eliminarDirectorio();
        }

        private void botonGenerarXML_Click(object sender, EventArgs e)
        {
            logicaInterfaz.generarXml();
        }

        private void botonSubirXML_Click(object sender, EventArgs e)
        {
            logicaInterfaz.leerXml();
        }

        private void botonCabeceraXML_Click(object sender, EventArgs e)
        {
            logicaInterfaz.abrirCabeceraXml();
        }

        private void cerrarArchivoXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logicaInterfaz.cerrarXml();
        }

        private void listView1DoubleClic(object sender, EventArgs e)
        {
            logicaInterfaz.abrirEditorListView();
        }

        private void agregarArchivoDtdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logicaInterfaz.abrirDtd();
        }

        private void cerrarArchivoDtdToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            logicaInterfaz.cerrarDtd();
        }
    }
}
