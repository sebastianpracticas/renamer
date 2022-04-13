using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RetroRevistasRenamer;

public class Herramientas
{
    private Form1 form; //Solo obtiene información de "Form1" mediante getters, nunca usa setters.
    private Almacen almacen;
    private Hash hash;

    public Herramientas()
    {
        this.form = null;
        this.almacen = null;
    }

	public Herramientas(Form1 form, Almacen almacen)
	{
        this.form = form;
        this.almacen = almacen;
	}

    public String obtenerUltimoElemento(String nombreCarpeta)
    {
        Char[] separadores = new Char[] { '\\', '/' };

        foreach (Char separador in separadores)
        {
            try
            {
                String[] arrayNombreCarpeta = nombreCarpeta.Split(separador);
                return arrayNombreCarpeta[arrayNombreCarpeta.Length - 1].Trim();
            }
            catch
            {
                continue;
            }
        }

        return "Error";
    }

    public String obtenerPrimerElemento(String nombre)
    {
        Char[] separadores = new Char[] { '\\', '/' };

        foreach (Char separador in separadores)
        {
            try
            {
                return nombre.Split(separador)[0].Trim();
            }
            catch
            {
                continue;
            }
        }

        return "Error";
    }

    public String quitarUltimoElemento(String nombre)
    {
        Char[] separadores = new Char[] { '\\', '/' };

        foreach (Char separador in separadores)
        {
            try
            {
                String[] arrayNombre = nombre.Split(separador);
                String resultado = "";
                for (int i = 0; i < (arrayNombre.Length - 1); i++)
                {
                    if (i != 0)
                    {
                        resultado += "\\";
                    }
                    resultado += arrayNombre[i];
                }
                return resultado;
            } catch
            {
                continue;
            }
        }

        return "Error";
    }

    public String quitarPrimerElemento(String nombre)
    {
        Char[] separadores = new Char[] { '\\', '/' };

        foreach (Char separador in separadores)
        {
            try
            {
                String[] arrayNombre = nombre.Split(separador);
                String resultado = "";
                for (int i = 1; i < arrayNombre.Length; i++)
                {
                    if (i != 1)
                    {
                        resultado += "\\";
                    }
                    resultado += arrayNombre[i];
                }
                return resultado;
            }
            catch
            {
                continue;
            }
        }

        return "Error";
    }

    //Este método elimina el "-Ok", "-Incorrecto" o "-Desconocido" del final de la ruta para poder usarla.
    public String limpiarNombreNode(String nombre)
    {
        String resultado = "";
        String[] arrayNombre = nombre.Split('-');
        for (int i = 0; i < arrayNombre.Length; i++)
        {
            String nombreParaComparar = arrayNombre[i].Trim();
            if (nombreParaComparar != "Ok" && nombreParaComparar != "Incorrecto" && nombreParaComparar != "Desconocido")
            {
                if (i != 0)
                {
                    resultado += "-";
                }
                resultado += arrayNombre[i];
            }
        }

        return resultado.Trim();
    }

    public String obtenerRutaCompletaRevista(String rutaInterna)
    {
        ArrayList listaCarpetasRaiz = almacen.getListaCarpetas();
        ArrayList subListaCarpetasRaiz = new ArrayList();

        for (int i=0; i<listaCarpetasRaiz.Count; i++)
        {
            subListaCarpetasRaiz.Add(obtenerUltimoElemento((String)listaCarpetasRaiz[i]));
        }

        String subRutaInterna = obtenerPrimerElemento(rutaInterna);
        int indexRaiz = subListaCarpetasRaiz.IndexOf(subRutaInterna);
        if (indexRaiz != -1)
        {
            String resultado = listaCarpetasRaiz[indexRaiz] + "\\" + quitarPrimerElemento(rutaInterna);
            return resultado;
        } else
        {
            //return listaCarpetasRaiz[0] + "\\" + rutaInterna;
            return null;
        }
    }

    public String obtenerRutaIssue(String rutaInterna)
    {
        String nombreRevista = obtenerPrimerElemento(rutaInterna);
        String nombreIssue = obtenerUltimoElemento(rutaInterna);
        ArrayList listaCarpetasRaiz = almacen.getListaCarpetas();
        String ruta = "";

        for (int i=0; i<listaCarpetasRaiz.Count; i++)
        {
            String carpetaRaiz = (String) listaCarpetasRaiz[i];
            String[] directorios = Directory.GetDirectories(carpetaRaiz);
            for (int j=0; j<directorios.Length; j++)
            {
                if (nombreRevista == obtenerUltimoElemento(directorios[j]))
                {
                    ruta = carpetaRaiz;
                }
            }
        }

        if (ruta == "")
        {
            return null;
        } else
        {
            return ruta + "\\" + rutaInterna;
        }
    }

    public Boolean comprobarSiDebeMostrarRevista(Magazine revista)
    {
        Boolean resultado = true;
        foreach (Issue issue in revista.ListaIssues)
        {
            if ((issue.IssueStatus == "Ok" && !almacen.getMostrarRevistaOk()) || (issue.IssueStatus == "Incorrecto" && !almacen.getMostrarRevistaIncorrecta()) || (issue.IssueStatus == "Desconocido" && !almacen.getMostrarRevistaDesconocida()))
            {
                resultado = false;
                break;
            }
        }
        return resultado;
    }

    public Boolean comprobarSiDebeMostrarIssue(Issue issue)
    {
        Boolean resultado = true;

        if ((issue.IssueStatus == "Ok" && !almacen.getMostrarIssuesOk()) || (issue.IssueStatus == "Incorrecto" && !almacen.getMostrarIssuesIncorrecto()) || (issue.IssueStatus == "Desconocido" && !almacen.getMostrarIssuesDesconocido()))
        {
            resultado = false;
        }

        return resultado;
    }

    public int[] contarEstadoItems()
    {
        int contadorOk = 0;
        int contadorIncorrecto = 0;
        int contadorDesconocido = 0;

        foreach (ListViewItem item in form.getListView1().Items)
        {
            switch (item.SubItems[7].Text)
            {
                case "Ok":
                    contadorOk++;
                    break;
                case "Incorrecto":
                    contadorIncorrecto++;
                    break;
                case "Desconocido":
                    contadorDesconocido++;
                    break;
            }
        }
        return new int[] { contadorOk, contadorIncorrecto, contadorDesconocido };
    }

    public String[] comprobarEstadoIssuesInternos(Magazine revista)
    {
        //Comprobar dentro de la revista el estado de sus issues.
        //Comprobar dentro de la revista el estado de sus issues.
        //Retorna un array que contiene un conteo de los tipos de issue que tiene.
        //La longitud del array va entre 0 y 3. Donde 0 significa que no tiene issues, y 3 significa que tiene issues con nombre Ok, Incorrecto y Desconocido.
        ArrayList resultado = new ArrayList();

        foreach (Issue issue in revista.ListaIssues)
        {
            if (resultado.IndexOf(issue.IssueStatus) == -1)
            {
                resultado.Add(issue.IssueStatus);
            }
        }

        return (String[])resultado.ToArray(typeof(String));
    }

    public int compararArrayString(String[] array1, String[] array2)
    {
        int contador = 0;

        for (int i=0; i<array1.Length; i++)
        {
            for (int j=0; j<array2.Length; j++)
            {
                if (array1[i].Equals(array2[j]))
                {
                    contador++;
                    break;
                }
            }
        }

        return contador;
    }

    public void crearRevistasEIssues(CarpetaContenedora contenedora)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(contenedora.Ruta);
        foreach (DirectoryInfo dirHijo in directoryInfo.GetDirectories())
        {
            Magazine revista = new Magazine();
            revista.MagazineFileName = dirHijo.Name;
            revista.Ruta = quitarUltimoElemento(dirHijo.FullName);

            //Rellenar la lista de issues dentro de la revista.
            foreach (FileInfo issueInfo in dirHijo.GetFiles())
            {
                byte[] bytes = File.ReadAllBytes(issueInfo.FullName);
                Issue issue = new Issue();
                issue.IssueName = issueInfo.Name;
                issue.IssuePath = dirHijo.FullName;
                issue.IssueSize = issueInfo.Length;
                issue.IssueMd5 = convertirMD5(bytes);
                issue.IssueSha1 = convertirSha1(bytes);
                revista.addIssue(issue);
                issue.activarProteccionContraCambios();
            }

            contenedora.anyadirRevistaALista(revista);
        }
    }

    public String procesarPeso(long peso)
    {
        String[] tipos = new String[] {"Bytes", "KB", "MB", "GB", "TB"};
        int contador = 0;
        decimal pesoDecimal = Convert.ToDecimal(peso);
        while (pesoDecimal >= 1024)
        {
            contador++;
            pesoDecimal /= 1024;
        }
        return pesoDecimal.ToString("0.##") + " " + tipos[contador];
    }

    public long procesarPesoInverso(String cadena)
    {
        String[] tipos = new String[] { "Bytes", "KB", "MB", "GB", "TB" };
        String[] splitCadena = cadena.Trim().Split(' ');
        decimal resultado = 0;
        int index = -1;
        //Se verifica que splitCadena tenga longitud de 2 y que su segunda parte sea "Bytes", "KB", "MB", "GB" o "TB".
        if (splitCadena.Length == 2 && (index = Array.IndexOf(tipos, splitCadena[1])) != -1)
        {
            decimal.TryParse(splitCadena[0], out resultado);
            for (int i=1; i<=index; i++)
            {
                resultado *= 1024;
            }
        }

        return Decimal.ToInt64(resultado);  //Convierte decimal a long
    }

    public bool comprobarExtensionFichero(String nombreFichero, String extensionEsperada)
    {
        if (File.Exists(nombreFichero))
        {
            String[] arrayNombreFichero = nombreFichero.Split('.');
            if (arrayNombreFichero[arrayNombreFichero.Length-1] == extensionEsperada)
            {
                return true;
            }
        }

        return false;
    }

    public bool existeCarpetaContenedora(String rutaBuscada)
    {
        foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
        {
            if (carpeta.Ruta.Equals(rutaBuscada))
            {
                return true;
            }
        }
        return false;
    }

    private void iniciarHash()
    {
        if (this.hash == null) {
            this.hash = new Hash();
        }
    }

    public String convertirMD5(byte[] bytesFichero)
    {
        String md5Val = "";

        if (bytesFichero.Length > 0)
        {
            iniciarHash();
            md5Val = hash.md5(bytesFichero);
        }
        while (md5Val.Length < 32)
        {
            md5Val = "0" + md5Val;
        }
        return md5Val;
    }

    public String convertirSha1(byte[] bytesFichero)
    {
        String sha1Val = "";

        if (bytesFichero.Length > 0)
        {
            iniciarHash();
            sha1Val = hash.sha1(bytesFichero);
        }
        while (sha1Val.Length < 40)
        {
            sha1Val = "0" + sha1Val;
        }
        return sha1Val;
    }

    public Magazine obtenerRevistaDesdeNombre(String nombre)
    {
        foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
        {
            foreach (Magazine revista in carpeta.ListaRevistas)
            {
                if (revista.MagazineName == nombre || revista.MagazineFileName == nombre)
                {
                    return revista;
                }
            }
        }
        return null;
    }

    public Issue obtenerIssueDesdeNombre(String nombre)
    {
        foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
        {
            foreach (Magazine revista in carpeta.ListaRevistas)
            {
                foreach (Issue issue in revista.ListaIssues)
                {
                    if (issue.IssueName == nombre)
                    {
                        return issue;
                    }
                }
            }
        }
        return new Issue();
    }

    public CarpetaContenedora obtenerCarpetaContenedoraRecovery()
    {
        foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
        {
            if (carpeta.ForRecovery)
            {
                return carpeta;
            }
        }
        return crearCarpetaContenedoraRecovery();
    }

    public CarpetaContenedora crearCarpetaContenedoraRecovery()
    {
        try
        {
            String ruta;
            if (almacen.getListaCarpetas().Count > 0)
            {
                ruta = quitarUltimoElemento(((CarpetaContenedora) almacen.getListaCarpetas()[0]).Ruta);
            } else
            {
                ruta = almacen.getRutaRaiz();
            }
            
            ruta += "\\Directorio Auxiliar";

            Directory.CreateDirectory(ruta);
            CarpetaContenedora carpetaNueva = new CarpetaContenedora(ruta);
            carpetaNueva.ForRecovery = true;
            almacen.anyadirAListaCarpetas(carpetaNueva);
            crearRevistasEIssues(carpetaNueva);
            return carpetaNueva;
        } catch
        {
            return null;
        }
    }

    //Elimina la carpeta auxiliar cuando no tiene nada (está vacía).
    public void eliminarCarpetaContenedoraRecovery()
    {
        CarpetaContenedora carpetaAEliminar = null;
        foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
        {
            if (carpeta.ForRecovery)
            {
                if (Directory.GetDirectories(carpeta.Ruta).Length == 0 && Directory.GetFiles(carpeta.Ruta).Length == 0)
                {
                    carpetaAEliminar = carpeta;
                }
            }
        }
        
        if (carpetaAEliminar != null)
        {
            try {
                almacen.removerListaCarpetas(carpetaAEliminar);
                Directory.Delete(carpetaAEliminar.Ruta);
            } catch
            {
                //Nada
            }
        }
    }
}
