using System;
using System.IO;

public class Issue
{

    //DECLARACIONES
    private String issueName;
    private string issuePath;
    private long issueSize;
    private String issueMd5;
    private String issueSha1;
    private String issueNumber;
    private String issueStatus;
    private bool issueVisto, issueDuplicado; //Variable con usos muy concretos cuando se repasa una lista de issues y quieres no tener en cuenta los que ya se revisaron en iteraciones previas.
    private StreamReader issueSr;

    //CONSTRUCTORES
    public Issue()
	{
        IssueName = "";
        IssuePath = "";
        IssueSize = 0;
        IssueMd5 = "";
        IssueSha1 = "";
        IssueNumber = "";
        IssueStatus = "";
        IssueVisto = false;
        IssueDuplicado = false;
        IssueSr = null;
    }

    public Issue(String issueName, string issuePath, long issueSize, String issueMd5, String issueSha1, String issueNumber, String issueStatus)
    {
        this.IssueName = issueName;
        this.IssuePath = issuePath;
        this.IssueSize = issueSize;
        this.IssueMd5 = issueMd5;
        this.IssueSha1 = issueSha1;
        this.IssueNumber = issueNumber;
        this.IssueStatus = issueStatus;
        this.IssueVisto = false;
        this.IssueDuplicado = false;
        this.IssueSr = null;
    }


    //GETTERS y SETTERS
    public String IssueName { get => issueName; set => issueName = value; }
    public long IssueSize { get => issueSize; set => issueSize = value; }
    public String IssueMd5 { get => issueMd5; set => issueMd5 = value; }
    public String IssueSha1 { get => issueSha1; set => issueSha1 = value; }
    public String IssueNumber { get => issueNumber; set => issueNumber = value; }
    public String IssueStatus { get => issueStatus; set => issueStatus = value; }
    public String IssuePath { get => issuePath; set => issuePath = value; }
    public bool IssueVisto { get => issueVisto; set => issueVisto = value; }
    public bool IssueDuplicado { get => issueDuplicado; set => issueDuplicado = value; }
    private StreamReader IssueSr { get => issueSr; set => issueSr = value; }

    //METODOS
    public String rutaCompleta()
    {
        return this.IssuePath + "\\" + this.IssueName;
    }

    public void activarProteccionContraCambios()
    {
        if (this.IssueSr != null)
        {
            this.IssueSr.Close();
        }
        this.IssueSr = new StreamReader(this.rutaCompleta());
    }

    public void desactivarProteccionContraCambios()
    {
        this.IssueSr.Close();
        this.IssueSr = null;
    }
}
