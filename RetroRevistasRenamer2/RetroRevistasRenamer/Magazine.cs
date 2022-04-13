using System;
using System.Collections;

public class Magazine
{

    //VARIABLES
    private String magazineName;
    private String magazineFileName;
    private String magazineDescription;
    private String magazineSize;
    private String magazineCountry;
    private String magazineLanguage;
    private String magazineYear;
    private String magazinePublisher;
    private String ruta;
    private String magazineState;
    private ArrayList listaIssues;

    //CONSTRUCTORES
    public Magazine()
	{
        this.MagazineName = "";
        this.MagazineFileName = "";
        this.MagazineDescription = "";
        this.MagazineSize = "";
        this.MagazineCountry = "";
        this.MagazineLanguage = "";
        this.MagazineYear = "";
        this.MagazinePublisher = "";
        this.Ruta = "";
        this.MagazineState = "";
        this.ListaIssues = new ArrayList();
    }

    public Magazine(String magazineName, String magazineFileName, String magazineDescription, String magazineSize, String magazineCountry, String magazineLanguage, String magazineYear, String magazinePublisher, String ruta, String magazineState)
    {
        this.MagazineName = magazineName;
        this.MagazineFileName = magazineFileName;
        this.MagazineDescription = magazineDescription;
        this.MagazineSize = magazineSize;
        this.MagazineCountry = magazineCountry;
        this.MagazineLanguage = magazineLanguage;
        this.MagazineYear = magazineYear;
        this.MagazinePublisher = magazinePublisher;
        this.Ruta = ruta;
        this.MagazineState = magazineState;
        this.ListaIssues = new ArrayList();
    }

    public Magazine(String magazineName, String magazineFileName, String magazineDescription, String magazineSize, String magazineCountry, String magazineLanguage, String magazineYear, String magazinePublisher, String rutaCompleta, ArrayList listaIssues)
    {
        this.MagazineName = magazineName;
        this.MagazineFileName = magazineFileName;
        this.MagazineDescription = magazineDescription;
        this.MagazineSize = magazineSize;
        this.MagazineCountry = magazineCountry;
        this.MagazineLanguage = magazineLanguage;
        this.MagazineYear = magazineYear;
        this.MagazinePublisher = magazinePublisher;
        this.Ruta = rutaCompleta;
        this.ListaIssues = listaIssues;
    }


    //GETTERS y SETTERS
    public String MagazineName { get => magazineName; set => magazineName = value; }
    public String MagazineFileName { get => magazineFileName; set => magazineFileName = value; }
    public String MagazineDescription { get => magazineDescription; set => magazineDescription = value; }
    public String MagazineSize { get => magazineSize; set => magazineSize = value; }
    public String MagazineCountry { get => magazineCountry; set => magazineCountry = value; }
    public String MagazineLanguage { get => magazineLanguage; set => magazineLanguage = value; }
    public String MagazineYear { get => magazineYear; set => magazineYear = value; }
    public String MagazinePublisher { get => magazinePublisher; set => magazinePublisher = value; }
    public ArrayList ListaIssues { get => listaIssues; set => listaIssues = value; }
    public String Ruta { get => ruta; set => ruta = value; }
    public string MagazineState { get => magazineState; set => magazineState = value; }

    //MÉTODOS
    public void addIssue(Issue issue)
    {
        this.ListaIssues.Add(issue);
    }

    public void removeIssue(Issue issue)
    {
        this.ListaIssues.Remove(issue);
    }

    public String rutaCompleta()
    {
        return this.Ruta + "\\" + this.MagazineFileName;
    }

    public long calcularPesoIssuesInternos()
    {
        long resultado = 0;
        foreach(Issue issue in ListaIssues)
        {
            resultado += issue.IssueSize;
        }
        return resultado;
    }

    public void actualizarRutaIssuesInternos()
    {
        foreach (Issue issue in ListaIssues)
        {
            issue.IssuePath = this.rutaCompleta();
        }
    }

    public void destroyIssues()
    {
        for (int i = (listaIssues.Count-1); i>=0 ; i--)
        {
            Issue issue = (Issue) listaIssues[i];
            listaIssues.Remove(issue);
            issue.desactivarProteccionContraCambios();
            issue = null;
        }
    }
}
