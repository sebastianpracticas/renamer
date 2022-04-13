using System;
using System.IO;

public class LogCambios
{
	private Almacen almacen;
	private String contenidoLog;
	private String path;

	public LogCambios(Almacen almacen, String contenidoLog)
	{
		this.almacen = almacen;
		this.ContenidoLog = contenidoLog;
		this.Path = "";
		crearLog();
	}

    public string ContenidoLog { get => contenidoLog; set => contenidoLog = value; }
    public string Path { get => path; set => path = value; }

    public void crearLog()
    {
		
		this.Path = almacen.getRutaRaiz() + "\\log";
		Directory.CreateDirectory(path);
		this.Path += "\\log cambios " + DateTime.Now.ToString("dd-MM-yyyy HH mm ss") + ".log";
		if (!File.Exists(this.Path))
		{
			StreamWriter writer = new StreamWriter(this.Path, true);
			writer.WriteLine(this.ContenidoLog);
			writer.Close();
		}
		
	}
}
