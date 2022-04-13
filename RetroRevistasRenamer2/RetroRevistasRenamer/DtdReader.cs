using System;
using System.IO;
using System.Windows.Forms;

public class DtdReader
{

	private String rutaFichero;
	private String[] dtdLines;

	public DtdReader()
	{
		setRutaFichero("");  //Valor por defecto.
	}

	public DtdReader(String rutaFichero)
    {
		if (File.Exists(rutaFichero))
		{
			setRutaFichero(rutaFichero);
		} else
        {
			MessageBox.Show("El .dtd buscado no existe. Se dejará la ruta vacía.\n" + rutaFichero, ".dtd no existe");
			setRutaFichero("");  //Valor por defecto.
		}
	}

	public String getRutaFichero()
    {
		return rutaFichero;
    }

	public void setRutaFichero(String rutaFichero)
    {
		this.rutaFichero = rutaFichero;
		procesarDtd();

	}

	public String[] getDtdLines()
    {
		return dtdLines;
    }

	public void setDtdLines(String[] dtdLines)
    {
		this.dtdLines = dtdLines;
    }

	public void procesarDtd()
    {
		try
		{
			dtdLines = File.ReadAllLines(rutaFichero);
		} catch
		{
			dtdLines = new string[0];
		}
	}

}
