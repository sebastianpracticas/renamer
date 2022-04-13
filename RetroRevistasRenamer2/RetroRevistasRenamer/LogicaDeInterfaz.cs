using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RetroRevistasRenamer;
using static System.Windows.Forms.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

public class LogicaDeInterfaz
{

    private Form1 form;
    private Almacen almacen;
    private Herramientas herramientas;

    public LogicaDeInterfaz(Form1 form, Almacen almacen)
    {
        this.form = form;
        this.almacen = almacen;
        this.herramientas = new Herramientas(form, almacen);
    }

    public void abrirCarpeta()
    {
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
        {
            if (Directory.Exists(folderBrowserDialog.SelectedPath) && !herramientas.existeCarpetaContenedora(folderBrowserDialog.SelectedPath))
            {
                CarpetaContenedora contenedora = new CarpetaContenedora(folderBrowserDialog.SelectedPath);
                almacen.anyadirAListaCarpetas(contenedora);
                herramientas.crearRevistasEIssues(contenedora); //Rellena la lista de revistas del almacen
                actualizarArbol();
            }
        }
    }
    
    public void actualizarArbol()
    {
        TreeView treeView1 = form.getTreeView1();
        treeView1.BeginUpdate();
        treeView1.Nodes.Clear();    //Limpiar treeview si tiene algo.
        
        foreach (CarpetaContenedora contenedora in almacen.getListaCarpetas())
        {
            TreeNode nodeContenedora = treeView1.Nodes.Add(contenedora.ToString());
            foreach (Magazine revista in contenedora.ListaRevistas)
            {
                if (herramientas.comprobarSiDebeMostrarRevista(revista)) {
                    nodeContenedora.Nodes.Add(revista.MagazineFileName);
                }
            }
        }

        //Selecciona el nodo raíz si existe.
        if (treeView1.Nodes.Count > 0)
        {
            treeView1.SelectedNode = treeView1.Nodes[0];
        }
        
        //Guardar cambios.
        treeView1.EndUpdate();
        treeView1.ExpandAll();
        form.setTreeView1(treeView1);
    }

    public void actualizarListView()
    {
        //Se ejecuta cuando se selecciona una opción en el listado de carpetas.
        //Hay dos opciones: se puede estar seleccionando un directorio externo (sin nodos padres que aparece "listaCarpetas" o puede ser un directorio de revista que contiene issues.

        TreeView tv = form.getTreeView1();
        TreeNode currentNode = null;

        if (tv.SelectedNode != null)
        {
            //Cuando el usuario tiene un nodo seleccionado se asigna a currentNode.
            currentNode = tv.SelectedNode;
        } else
        {
            //Cuando el usuario no tiene un nodo seleccionado aún se le asigna a currentNode el primer nodo si existe. Si el árbol está vacío saca null.
            currentNode = tv.Nodes.Count > 0 ? tv.Nodes[0] : null;
            tv.SelectedNode = currentNode;  //Se marca en el treeview el nodo[0] como seleccionado.
        }

        //Cambiar el texto del label parte superior izquierda
        String textoLabelCarpeta = "";
        if (currentNode != null)
        {
            textoLabelCarpeta = currentNode.Text;
        }
        form.setLabelCarpetaText(textoLabelCarpeta);

        if (currentNode != null)
        {
            String rutaNode = currentNode.FullPath;
            CarpetaContenedora carpeta = null;
            Boolean isMagazine = false;

            if (Regex.Matches(rutaNode, @"^.*[\\|/].*$").Count > 0)
            {
                isMagazine = true;
            }

            String rutaParaComparar = herramientas.obtenerPrimerElemento(rutaNode);
            foreach (CarpetaContenedora carpetaContenedora in almacen.getListaCarpetas())
            {
                if (rutaParaComparar.Equals(carpetaContenedora.ToString()))
                {
                    carpeta = carpetaContenedora;
                    break;
                }
            }

            form.limpiarListView();
            if (carpeta != null) {
                if (isMagazine)
                {
                    Magazine revista = null;
                    //Obtener la revista
                    foreach (Magazine revistaGuardada in carpeta.ListaRevistas)
                    {
                        if (herramientas.obtenerUltimoElemento(rutaNode).Equals(revistaGuardada.MagazineFileName))
                        {
                            revista = revistaGuardada;
                            break;
                        }
                    }

                    if (revista != null && Directory.Exists(revista.rutaCompleta()))
                    {
                        //Adaptar la tabla
                        adaptarFormatoListViewIssues();
                        if (revista != null) {
                            //Listar en listView1 los issues.
                            actualizarIssuesListView(carpeta, revista);
                        }
                    } else
                    {
                        MessageBox.Show("La carpeta seleccionada no existe", "No existe");
                        actualizarArbol();
                    }
                }
                else
                {
                    if (Directory.Exists(carpeta.Ruta))
                    {
                        //Adaptar la tabla
                        adaptarFormatoListViewMagazines();
                        //Listar en listView1 las revistas.
                        actualizarRevistasListView(carpeta);
                        //Actualizar el contador con el nombre de las revistas que están bien.
                        actualizarContador();
                    } else
                    {
                        MessageBox.Show("La carpeta seleccionada no existe", "No existe");
                        actualizarArbol();
                    }
                }
            }
        } else
        {
            //En caso de que esté vacío el árbol de directorios vaciamos el listView.
            ListView listView = form.getListView1();
            listView.Clear();
            form.setListView1(listView);
        }
    }

    public void actualizarIssuesListView(CarpetaContenedora carpeta, Magazine revista)
    {
        ListView listView = form.getListView1();

        foreach(Issue issue in revista.ListaIssues)
        {
            if (herramientas.comprobarSiDebeMostrarIssue(issue)) {
                ListViewItem listViewItem = new ListViewItem(issue.IssueName);
                listViewItem.SubItems.Add(revista.MagazineName);
                listViewItem.SubItems.Add(issue.IssueNumber);
                listViewItem.SubItems.Add(herramientas.procesarPeso(new FileInfo(issue.rutaCompleta()).Length));
                listViewItem.SubItems.Add(revista.MagazineCountry);
                listViewItem.SubItems.Add(revista.MagazineLanguage);
                listViewItem.SubItems.Add(revista.MagazineYear);
                listViewItem.SubItems.Add(issue.IssueStatus);
                listView.Items.Add(listViewItem);
            }
        }

        form.setListView1(listView);
        actualizarContador();
    }

    public void actualizarRevistasListView(CarpetaContenedora carpeta)
    {
        ListView listView1 = form.getListView1();

        foreach (Magazine revista in carpeta.ListaRevistas) {
            ListViewItem listViewItem = new ListViewItem(revista.MagazineFileName);
            listViewItem.SubItems.Add(revista.MagazineName);
            listViewItem.SubItems.Add(herramientas.procesarPeso(revista.calcularPesoIssuesInternos()));
            listViewItem.SubItems.Add(revista.MagazineSize);
            listViewItem.SubItems.Add(revista.MagazineCountry);
            listViewItem.SubItems.Add(revista.MagazineLanguage);
            listViewItem.SubItems.Add(revista.MagazineYear);
            listViewItem.SubItems.Add(revista.MagazineState);

            listView1.Items.Add(listViewItem);
        }
    }

    public void actualizarContador()
    {
        int[] lista = herramientas.contarEstadoItems();
        int contadorOk = lista[0];
        int contadorIncorrecto = lista[1];
        int contadorDesconocido = lista[2];

        String resultado = "";
        //Si todos son 0 saca "".
        //Si incorrecto y desconocido es 0 pero Ok es mayor a 0 saca "Todo OK".
        //En cualquier otro caso saca "OK: " + listaOk.Count + " |Incorrecto: " + listaIncorrecto.Count + " |Desconocido: " + listaDesconocido.Count
        if (contadorIncorrecto == 0 && contadorDesconocido == 0)
        {
            if (contadorOk > 0)
            {
                resultado = "Todo OK";
            }
        } else
        {
            resultado = "OK: " + contadorOk + " |Incorrecto: " + contadorIncorrecto + " |Desconocido: " + contadorDesconocido;
        }
        
        form.setLabelEstadoText(resultado);
    }

    public void eliminarDirectorio()
    {
        //Obtener lista de directorios
        TreeView treeView = form.getTreeView1();

        if (treeView.SelectedNode != null)
        {
            if (MessageBox.Show("Confirme que desea quitar una carpeta contenedora del árbol de directorios (no se borrará de su ordenador)", "Eliminar directorio", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                //Eliminarlo de la lista
                ArrayList listaCarpetas = almacen.getListaCarpetas();
                for (int i=(listaCarpetas.Count-1); i>=0; i--)
                {
                    CarpetaContenedora carpeta = (CarpetaContenedora) listaCarpetas[i];
                    if (herramientas.obtenerPrimerElemento(treeView.SelectedNode.FullPath).Equals(herramientas.obtenerUltimoElemento(carpeta.ToString())))
                    {
                        listaCarpetas.Remove(carpeta);
                        carpeta.destroyMagazines();
                        carpeta = null;
                    }
                }
                almacen.setListaCarpetas(listaCarpetas);

                //Actualizar el árbol para que se vacíe.
                actualizarArbol();
                //Actualizar el listView.
                actualizarListView();
            }
        }
    }

    public void mostrarOcultarRevistas(String tipo)
    {
        switch (tipo)
        {
            case "Ok":
                almacen.setMostrarRevistaOk(!almacen.getMostrarRevistaOk());
                break;
            case "Incorrecta":
                almacen.setMostrarRevistaIncorrecta(!almacen.getMostrarRevistaIncorrecta());
                break;
            case "Desconocida":
                almacen.setMostrarRevistaDesconocida(!almacen.getMostrarRevistaDesconocida());
                break;
        }
        //Actualizar TreeView
        actualizarArbol();
    }

    public void mostrarOcultarFicheros(String tipo)
    {
        switch (tipo)
        {
            case "Ok":
                almacen.setMostrarIssuesOk(!almacen.getMostrarIssuesOk());
                break;
            case "Incorrecto":
                almacen.setMostrarIssuesIncorrecto(!almacen.getMostrarIssuesIncorrecto());
                break;
            case "Desconocido":
                almacen.setMostrarIssuesDesconocido(!almacen.getMostrarIssuesDesconocido());
                break;
        }
        actualizarListView();
    }

    //Adaptar columnas listview
    public void adaptarFormatoListViewIssues()
    {
        ListView listView1 = form.getListView1();
        listView1.Columns.Clear();
        listView1.Columns.Add("Nombre fichero");
        listView1.Columns.Add("Nombre revista");
        listView1.Columns.Add("Número");
        listView1.Columns.Add("Tamaño completa");
        listView1.Columns.Add("Pais");
        listView1.Columns.Add("Idioma");
        listView1.Columns.Add("Años");
        listView1.Columns.Add("Estado");

        almacen.setTipoLista("issues");
    }

    //Adaptar columnas listview
    public void adaptarFormatoListViewMagazines()
    {
        ListView listView1 = form.getListView1();
        listView1.Columns.Clear();
        listView1.Columns.Add("Nombre carpeta");
        listView1.Columns.Add("Nombre revista");
        listView1.Columns.Add("Tamaño actual");
        listView1.Columns.Add("Tamaño completa");
        listView1.Columns.Add("Pais");
        listView1.Columns.Add("Idioma");
        listView1.Columns.Add("Años");
        listView1.Columns.Add("Estado");

        almacen.setTipoLista("magazines");
    }

    public bool generarXml()
    {
        if(MessageBox.Show("Confirme que desea generar un .xml con las revistas e issues existentes", "Generar XML", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            if (almacen.getRutaDtd() == null)
            {
                if (MessageBox.Show("No ha seleccionado un .dtd\nSe dejará la ruta vacía. ¿Desea continuar?", "DTD no seleccionado", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }
            XmlFactory xmlFactory = new XmlFactory(almacen);
            xmlFactory.generarXml();
            return true;
        }

        return false;
    }

    //Obtiene información de un XML y la almacena para usarse desde los demás métodos
    public bool leerXml()
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            String nombreFichero = fileDialog.FileName;
            if (File.Exists(nombreFichero))
            {
                if (almacen.getNombresEsperadosXml() != null)
                {
                    if (MessageBox.Show("Va a reemplazar al .xml anterior ¿Desea continuar?", "Reemplazar .xml", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return false;
                    }
                }
                XmlFactory xmlFactory = new XmlFactory(almacen);
                xmlFactory.setPath(nombreFichero);              //Establece la ruta del xml
                ArrayList lecturaXml = xmlFactory.leerXml();    //Lee el XML y obtiene un arraylist con el resultado.
                if (lecturaXml.Count > 0)
                {
                    almacen.setNombresEsperadosXml(lecturaXml);     //Almacena el arraylist obtenido del XML para su uso en el resto de componentes.
                    form.setXmlFileLabelText(herramientas.obtenerUltimoElemento(nombreFichero));    //Modifica el label inferior.
                    if (MessageBox.Show("Se ha añadido un fichero .xml\n¿Desea renombrar los issues y revistas?", "Fichero .xml añadido", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        renombrarIssuesDesdeXml();
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("El .xml seleccionado no tiene un formato válido o está vacío", "Fichero no válido");
                    return false;
                }
            } else
            {
                MessageBox.Show("El fichero seleccionado no existe o está corrupto", "Error");
                return false;
            }
        }
        return false;
    }

    public void cerrarXml()
    {
        if (MessageBox.Show("Va a cerrar el fichero .xml ¿Desea continuar?", "Cerrar .xml", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            almacen.setNombresEsperadosXml(null);
            form.setXmlFileLabelText("No cargado");
            actualizarListView();    //Actualizar para que los cambios se muestren en el momento en ListView.
            actualizarArbol();          //Actualizar el árbol de directorios para que se calcule el estado de las revistas en el momento.
            MessageBox.Show("Se ha cerrado el fichero .xml seleccionado", "Fichero .xml cerrado");
        }
    }

    public void abrirCabeceraXml()
    {
        FormCabeceraXML formCabeceraXml = new FormCabeceraXML(almacen);
        formCabeceraXml.ShowDialog();
    }

    public void abrirEditorListView()
    {
        if (almacen.getTipoLista() == "magazines")
        {
            Magazine revista = herramientas.obtenerRevistaDesdeNombre(form.getListView1().SelectedItems[0].Text);
            FormEditar formEditar = new FormEditar(almacen, herramientas, form.getTreeView1(), revista);

            if (formEditar.ShowDialog() == DialogResult.OK)
            {
                actualizarListView();
            }

        } else
        {
            Issue issue = herramientas.obtenerIssueDesdeNombre(form.getListView1().SelectedItems[0].Text);
            FormEditarIssue formEditar = new FormEditarIssue(almacen, herramientas, form.getTreeView1(), issue);

            if (formEditar.ShowDialog() == DialogResult.OK)
            {
                actualizarListView();
                actualizarArbol();  //En caso de que se haya actualizado el nombre de una revista actualizar el árbol.
            }

        }
    }

    public void abrirDtd()
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        if (fileDialog.ShowDialog() == DialogResult.OK)
        {
            String fichero = fileDialog.FileName;
            if (herramientas.comprobarExtensionFichero(fichero, "dtd"))
            {
                if (almacen.getRutaDtd() == null)
                {
                    almacen.setRutaDtd(fichero);
                }
                else
                {
                    if (MessageBox.Show("Ya existe un fichero .dtd seleccionado:\n" + almacen.getRutaDtd() + "\n¿Desea reemplazarlo?", "Reemplazar .dtd", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                        almacen.setRutaDtd(fichero);
                    }
                }
            } else
            {
                MessageBox.Show("El .dtd seleccionado no tiene una extensión válida o no existe", "Fichero no válido");
            }
        }
    }

    public void cerrarDtd()
    {
        if (almacen.getRutaDtd() != null)
        {
            if (MessageBox.Show("Va a cerrar el fichero .dtd:\n" + almacen.getRutaDtd() + "\nÉl fichero .dtd es necesario para generar ficheros .xml\n ¿Desea continuar?", "Cerrar .dtd", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                almacen.setRutaDtd(null);
            }
        }
    }

    public void renombrarIssuesDesdeXml()
    {
        ArrayList lecturaXml = almacen.getNombresEsperadosXml();
        if (lecturaXml != null && lecturaXml.Count>0) {
            new CambiarNombres(almacen, herramientas);      //El constructor de CambiarNombres ejecuta todo el proceso de cambio de nombres de issues y revista.
            actualizarListView();    //Actualizar para que los cambios se muestren en el momento en ListView.
            actualizarArbol();          //Actualizar el árbol de directorios para que se calcule el estado de las revistas en el momento.
        } else
        {
            MessageBox.Show("No se pueden renombrar los issues ya que no hay un .xml válido o está vacío", "Error .xml");
        }
    }
}
