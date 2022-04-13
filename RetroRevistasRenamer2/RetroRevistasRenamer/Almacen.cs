using RetroRevistasRenamer;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

public class Almacen
{

    private ArrayList listaCarpetas;
    private Boolean mostrarIssuesOk, mostrarIssuesIncorrecto, mostrarIssuesDesconocido, mostrarRevistaOk, mostrarRevistaIncorrecta, mostrarRevistaDesconocida;
    private String tipoLista;
    private ArrayList nombresEsperadosXml;

    //Datos Header XML:
    private String name, description, category, author, email, homepage, url, comments, formato;
    private String rutaDtd, rutaRaiz;

    public Almacen()
	{
        listaCarpetas = new ArrayList();
        mostrarIssuesOk = true;
        mostrarIssuesIncorrecto = true;
        mostrarIssuesDesconocido = true;
        mostrarRevistaOk = true;
        mostrarRevistaIncorrecta = true;
        mostrarRevistaDesconocida = true;
        tipoLista = "";
        
        rutaRaiz = "C:\\retrorevistasRenamer";
        Directory.CreateDirectory(rutaRaiz);
        //Header XML por defecto:
        name = "Retrorevistas - Revistas España Videojuegos";
        description = "Retrorevistas - Revistas España Videojuegos";
        category = "Magazines";
        author = "DevilGaia";
        email = "retropublicaciones@gmail.com";
        homepage = "www.retrorevistas.com";
        url = "https://www.github.com/devilgaia/retrorevistas-dat/";
        comments = "Revistas retro de videojuegos de España";
        //formato = "clrmamepro";
        formato = null;


        //Dtd
        rutaDtd = null;
    }

    public ArrayList getListaCarpetas()
    {
        return listaCarpetas;
    }

    public void setListaCarpetas(ArrayList listaCarpetas)
    {
        this.listaCarpetas = listaCarpetas;
    }

    public Boolean getMostrarIssuesOk()
    {
        return mostrarIssuesOk;
    }

    public void setMostrarIssuesOk(Boolean estado)
    {
        this.mostrarIssuesOk = estado;
    }

    public Boolean getMostrarIssuesIncorrecto()
    {
        return mostrarIssuesIncorrecto;
    }

    public void setMostrarIssuesIncorrecto(Boolean estado)
    {
        this.mostrarIssuesIncorrecto = estado;
    }

    public Boolean getMostrarIssuesDesconocido()
    {
        return mostrarIssuesDesconocido;
    }

    public void setMostrarIssuesDesconocido(Boolean estado)
    {
        this.mostrarIssuesDesconocido = estado;
    }

    public Boolean getMostrarRevistaOk()
    {
        return mostrarRevistaOk;
    }

    public void setMostrarRevistaOk(Boolean estado)
    {
        this.mostrarRevistaOk = estado;
    }

    public Boolean getMostrarRevistaIncorrecta()
    {
        return mostrarRevistaIncorrecta;
    }

    public void setMostrarRevistaIncorrecta(Boolean estado)
    {
        this.mostrarRevistaIncorrecta = estado;
    }

    public Boolean getMostrarRevistaDesconocida()
    {
        return mostrarRevistaDesconocida;
    }

    public void setMostrarRevistaDesconocida(Boolean estado)
    {
        this.mostrarRevistaDesconocida = estado;
    }

    public String getTipoLista()
    {
        return tipoLista;
    }

    public void setTipoLista(String tipoLista)
    {
        this.tipoLista = tipoLista;
    }

    public ArrayList getNombresEsperadosXml()
    {
        return nombresEsperadosXml;
    }

    public void setNombresEsperadosXml(ArrayList nombresEsperadosXml)
    {
        this.nombresEsperadosXml = nombresEsperadosXml;
    }

    public String getName()
    {
        return this.name;
    }

    public void setName(String name)
    {
        this.name = name;
    }

    public String getDescription()
    {
        return this.description;
    }

    public void setDescription(String description)
    {
        this.description = description;
    }

    public String getCategory()
    {
        return this.category;
    }

    public void setCategory(String category)
    {
        this.category = category;
    }

    public String getAuthor()
    {
        return this.author;
    }

    public void setAuthor(String author)
    {
        this.author = author;
    }

    public String getEmail()
    {
        return this.email;
    }

    public void setEmail(String email)
    {
        this.email = email;
    }

    public String getHomepage()
    {
        return this.homepage;
    }

    public void setHomepage(String homepage)
    {
        this.homepage = homepage;
    }

    public String getUrl()
    {
        return this.url;
    }

    public void setUrl(String url)
    {
        this.url = url;
    }

    public String getComments()
    {
        return this.comments;
    }

    public void setComments(String comments)
    {
        this.comments = comments;
    }

    public String getFormato()
    {
        return this.formato;
    }

    public void setFormato(String formato)
    {
        this.formato = formato;
    }

    public String getRutaDtd()
    {
        return this.rutaDtd;
    }

    public void setRutaDtd(String rutaDtd)
    {
        this.rutaDtd = rutaDtd;
    }

    public String getRutaRaiz()
    {
        /* Este código obtiene la ruta raíz del usuarios cuando esta no exista.
        if (this.rutaRaiz == null)
        {
            FormRutaRaiz frr = new FormRutaRaiz(this);
        }

        Directory.CreateDirectory(this.rutaRaiz);
        if (Directory.Exists(this.rutaRaiz))
        {
            return this.rutaRaiz;
        } else
        {
            MessageBox.Show("La ruta raiz seleccionada no existe (" + this.rutaRaiz + ")", "Error ruta raiz");
        }*/

        return this.rutaRaiz;
    }

    public void setRutaRaiz(String rutaRaiz)
    {
        this.rutaRaiz = rutaRaiz;
    }

    //Métodos:
    public void anyadirAListaCarpetas(CarpetaContenedora carpeta)
    {
        this.listaCarpetas.Add(carpeta);
    }

    public void removerListaCarpetas(CarpetaContenedora carpeta)
    {
        this.listaCarpetas.Remove(carpeta);
    }
}
