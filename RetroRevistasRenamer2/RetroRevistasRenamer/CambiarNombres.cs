using RetroRevistasRenamer;
using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

public class CambiarNombres
{
	//VARIABLES
	private Almacen almacen;
	private ArrayList listaIssuesEstados, listaRevistasXml;
	private int contador, contadorErrores, contadorEstados1, contadorEstados2, contadorEstados3, contadorEstados4, contadorEstados5, contadorEstados6;
	private String logCambios;
	private Herramientas herramientas;
	private LogCambios ficheroLog;

	//CONSTRUCTOR
	public CambiarNombres(Almacen almacen, Herramientas herramientas)
	{
		//Inicializar variables:
		this.almacen = almacen;
		this.herramientas = herramientas;
		listaIssuesEstados = new ArrayList();
		listaRevistasXml = almacen.getNombresEsperadosXml();
		contador = 0;
		contadorErrores = 0;
		logCambios = "";
		contadorEstados1 = 0;
		contadorEstados2 = 0;
		contadorEstados3 = 0;
		contadorEstados4 = 0;
		contadorEstados5 = 0;
		contadorEstados6 = 0;

		herramientas.obtenerCarpetaContenedoraRecovery();	//Creamos carpeta recovery por si acaso.
		proteccionEscrituraFicheros(false);

		//Ejecución de tareas:
		if (revisarSistemaDeArchivos()) //Paso 1.
		{
			comprobarIssues();  //Pasos 2 y 3.
			if (confirmacionUsuario())  //Paso 4.
			{
				if (realizarCopiaDeSeguridad())	//Copia de seguridad.
				{	
					corregirErroresIssues();    //Paso 5
					corregirErroresRevistas();  //Paso 6
					corregirIssuesDuplicados();	//Corregir duplicados.
					generarLog();               //Paso 7
					imprimirResultados();       //Paso 8
				}
			} else
			{
				asignarEstados();   //Paso 4A
			}
		} else
        {
			MessageBox.Show("Error 1001: No se han podido corregir los nombres de los issues porque se ha detectado que hay issues que no están en la posición esperada", "Error del sistema de archivos");
        }

		herramientas.eliminarCarpetaContenedoraRecovery();	//Solo se elimina si no tiene nada dentro.
		proteccionEscrituraFicheros(true);
	}

	private void proteccionEscrituraFicheros(bool activar)
    {
		foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
        {
			foreach (Magazine revista in carpeta.ListaRevistas)
            {
				foreach (Issue issue in revista.ListaIssues)
                {
					if (activar)
                    {
						//Encender protección de escritura.
						issue.activarProteccionContraCambios();
                    } else
                    {
						//Apagar protección de escritura.
						issue.desactivarProteccionContraCambios();
                    }
                }
            }
        }
    }

	//Comprueba que las revistas e issues que tenemos registradas en el almacén existen y no se les ha cambiado el nombre:
	private bool revisarSistemaDeArchivos()
	{
		foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
		{
			foreach (Magazine revista in carpeta.ListaRevistas)
			{
				if (!Directory.Exists(revista.rutaCompleta()))
				{
					return false;
				}

				foreach (Issue issue in revista.ListaIssues)
				{
					if (!File.Exists(issue.rutaCompleta()))
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	//Borra el "estado" de issues y revistas.
	//Compara los issues y revistas con los del .xml para ver si coinciden.
	private void comprobarIssues()
	{
		foreach (CarpetaContenedora carpetaAlmacenada in almacen.getListaCarpetas())
		{
			foreach (Magazine revistaAlmacenada in carpetaAlmacenada.ListaRevistas)
			{
				revistaAlmacenada.MagazineState = "";   //Borrando estado revista.
				foreach (Issue issueAlmacenado in revistaAlmacenada.ListaIssues)
				{
					issueAlmacenado.IssueStatus = ""; //Borrando estado issue.
					object[] resultado = compararIssueConXml(revistaAlmacenada, issueAlmacenado);
					listaIssuesEstados.Add(resultado);
				}
			}
		}
		repasarIssuesRestantes();
	}

	//Cada vez que se invoca recibe un issue que tenemos en el sistema de archivos (y la revista a la que pertenece) y lo compara con los del .xml para devolver su estado con un entero del 1 al 6.
	private object[] compararIssueConXml(Magazine revistaAlmacenada, Issue issueAlmacenado)
	{
		//POSIBLES RETORNOS:
		//1.En el lugar correcto (dentro de una revista con el mismo nombre) y con el nombre correcto.
		//2.En el lugar correcto pero con el nombre incorrecto.
		//3.En el lugar incorrecto(otra revista o la misma con otro nombre) y con el nombre correcto.
		//4.En el lugar incorrecto(otra revista o la misma con otro nombre) y con el nombre incorrecto.
		//5.Están en el XML y no en el sistema de archivos(faltan en el ordenador).
		//6.Están en el sistema de archivos pero no en el XML(no deberían estar ahí).

		foreach (Magazine revistaXml in listaRevistasXml)
		{
			for (int i = (revistaXml.ListaIssues.Count - 1); i >= 0; i--)
			{
				Issue issueXml = (Issue)revistaXml.ListaIssues[i];
				//En caso de que el issue coincida comprobamos el resto de posibilidades:
				if (issueAlmacenado.IssueMd5 == issueXml.IssueMd5 && issueAlmacenado.IssueSha1 == issueXml.IssueSha1)
				{
					if (issueXml.IssueVisto)
                    {
						issueAlmacenado.IssueDuplicado = true;	//En caso de que este issue del XML ya hubiera encontrado algún match anterior marco este issue almacenado como duplicado para hacer algo al final de la ejecución.
                    }

					object[] resultado;
					//El issue coincide, buscamos cual opción retornar
					if (revistaAlmacenada.MagazineName == revistaXml.MagazineName)
					{
						if (issueAlmacenado.IssueName == issueXml.IssueName)
						{
							contadorEstados1++;
							resultado = new object[] { 1, issueAlmacenado };   //Nombre de revista e issue coinciden.
						}
						else
						{
							contadorEstados2++;
							string destinoCompleto = issueAlmacenado.IssuePath + "\\" + issueXml.IssueName;
							resultado = new object[] { 2, issueAlmacenado, destinoCompleto };   //Nombre de revista coincide, pero el issue no.			
						}
					} else
					{
						Magazine revistaDestino = herramientas.obtenerRevistaDesdeNombre(revistaXml.MagazineName);
						if (revistaDestino == null)
                        {
							CarpetaContenedora carpeta = herramientas.obtenerCarpetaContenedoraRecovery();
							revistaDestino = new Magazine(revistaXml.MagazineName, revistaXml.MagazineFileName, revistaXml.MagazineDescription, revistaXml.MagazineSize, revistaXml.MagazineCountry, revistaXml.MagazineLanguage, revistaXml.MagazineYear, revistaXml.MagazinePublisher, carpeta.Ruta, "");
							carpeta.anyadirRevistaALista(revistaDestino);
                        }

						if (issueAlmacenado.IssueName == issueXml.IssueName)
						{
							contadorEstados3++;
							resultado = new object[] { 3, issueAlmacenado, revistaAlmacenada, revistaDestino };    //Nombre de revista no coinciden, pero issues sí.	
						}
						else
						{
							contadorEstados4++;
							string nombreDestino = issueXml.IssueName;
							resultado = new object[] { 4, issueAlmacenado, revistaAlmacenada, revistaDestino, nombreDestino };   //Nombre de revista e issues no coinciden.
						}
					}
					issueXml.IssueVisto = true;
					return resultado;
				}
			}
		}
		contadorEstados6++;
		return new object[] { 6, issueAlmacenado, revistaAlmacenada }; //Al comparar el MD5 y SHA1 no se han encontrado coincidencias para este issue o es un duplicado.
	}

	//Repasa las revistas de listaRevistasXml, y todos los issues cuya variable "IssueVisto" no haya sido marcada como "true" antes tienen estado 5.
	private void repasarIssuesRestantes()
	{
		foreach (Magazine revistaXml in listaRevistasXml)
		{
			foreach (Issue issueXml in revistaXml.ListaIssues)
			{
				if (!issueXml.IssueVisto) {
					contadorEstados5++;
					object[] array = new object[] { 5, issueXml };     //El issue aparece en el Xml pero no existe en el sistema de archivos.
					listaIssuesEstados.Add(array);
				}
			}
		}
	}

	private bool confirmacionUsuario()
	{
		String mensaje = "";
		mensaje += contadorEstados1 > 0 ? "Hay " + contadorEstados1 + " issues OK.\n" : "";
		mensaje += contadorEstados2 > 0 ? "Hay " + contadorEstados2 + " issues con el nombre incorrecto.\n" : "";
		mensaje += contadorEstados3 > 0 ? "Hay " + contadorEstados3 + " issues en la revista incorrecta.\n" : "";
		mensaje += contadorEstados4 > 0 ? "Hay " + contadorEstados4 + " issues en la revista incorrecta, que además tienen el nombre incorrecto.\n" : "";
		mensaje += contadorEstados5 > 0 ? "Faltan " + contadorEstados5 + " issues en su ordenador.\n" : "";
		mensaje += contadorEstados6 > 0 ? "Hay " + contadorEstados6 + " issues que desconocemos (no aparecen en el .xml) o están duplicados.\n" : "";
		if (mensaje != "") {
			if (contadorEstados2>0 || contadorEstados3>0 || contadorEstados4>0 || contadorEstados6>0) {
				mensaje += "¿Desea corregir los errores?\n";
			} else
            {
				mensaje += "¿Desea continuar?\n";
            }
			if (MessageBox.Show(mensaje, "Continuar cambios", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				return true;
			}
		} else
		{
			MessageBox.Show("Ningún nombre que comprobar", "Cargar .xml");
		}
		return false;
	}

	//Copia el sistema de archivos actual a otra carpeta de forma temporal.
	private bool realizarCopiaDeSeguridad()
    {
		String rutaBackup = almacen.getRutaRaiz() + "\\backup\\backup " + DateTime.Now.ToString("dd-MM-yyyy HH mm ss");
		Directory.CreateDirectory(rutaBackup);
		foreach (CarpetaContenedora directorio in almacen.getListaCarpetas())
		{
			//Obtener DirectoryInfo de la carpeta contenedora.
			DirectoryInfo dir = new DirectoryInfo(directorio.Ruta);
			Directory.CreateDirectory(rutaBackup + "\\" + dir.Name);
			if (dir.Exists)
			{
				DirectoryInfo[] dirs = dir.GetDirectories();
				
				//Copiar subdirectorios (revistas) y ficheros que tengan dentro (issues).
				foreach (DirectoryInfo subdir in dirs)
				{
					Directory.CreateDirectory(rutaBackup + "\\" + dir.Name + "\\" + subdir.Name);
					FileInfo[] files = subdir.GetFiles();
					foreach (FileInfo file in files)
					{
						file.CopyTo(rutaBackup + "\\" + dir.Name + "\\" + subdir.Name + "\\" + file.Name, false);
					}
				}
				
			} else
            {
				MessageBox.Show("Error al hacer backup del directorio " + directorio.Ruta + "\nOperación de renombrado abortada.", "Error haciendo copia de seguridad");
				return false;
            }
        }
		return true;
    }

	//Se repasa el arrayList solucionando todos los errores.
	private void corregirErroresIssues()
	{
		//Posible contenido de listaIssuesEstados:
		//[1, issue]
		//[2, issue, destinoCompleto(string)]
		//[3, issue, magazineOrigen, magazineDestino]
		//[4, issue, magazineOrigen, magazineDestino, nombreDestino]
		//[5, issueXML]
		//[6, issue]

		foreach (object[] arrayIssue in listaIssuesEstados)
		{
			Issue issue = (Issue)arrayIssue[1];
			switch (arrayIssue[0]) {
				case 1:
					corregirErroresIssuesCaso1(issue);
					break;
				case 2:
					string destinoCompleto = (string)arrayIssue[2];
					corregirErroresIssuesCaso2(issue, destinoCompleto);
					break;
				case 3:
					Magazine revistaOrigenCase3 = (Magazine)arrayIssue[2];
					Magazine revistaDestinoCase3 = (Magazine)arrayIssue[3];
					corregirErroresIssuesCaso3(issue, revistaOrigenCase3, revistaDestinoCase3);
					break;
				case 4:
					Magazine revistaOrigenCase4 = (Magazine)arrayIssue[2];
					Magazine revistaDestinoCase4 = (Magazine)arrayIssue[3];
					string nombreDestino = (string)arrayIssue[4];
					corregirErroresIssuesCaso4(issue, revistaOrigenCase4, revistaDestinoCase4, nombreDestino);
					break;
				case 5:
					corregirErroresIssuesCaso5(issue);
					break;
				case 6:
					Magazine revistaCase6 = (Magazine)arrayIssue[2];
					corregirErroresIssuesCaso6(revistaCase6, issue);
					break;
			}
		}
		listaIssuesEstados.Clear();
	}

	private void corregirErroresIssuesCaso1(Issue issue)
	{
		logCambios += "CORRECTO - " + issue.IssueName + ": Todo OK. Sin Cambios.\n\n";
		issue.IssueStatus = "Ok";
	}

	private void corregirErroresIssuesCaso2(Issue issue, string destinoCompleto)
	{
		if (!File.Exists(destinoCompleto))
		{
			try
			{
				string rutaOrigen = issue.rutaCompleta();
				File.Move(rutaOrigen, destinoCompleto);
				issue.IssueName = herramientas.obtenerUltimoElemento(destinoCompleto);
				logCambios += "CORRECTO - " + issue.IssueName + ": Se ha detectado y corregido un nombre de issue incorrecto:\nRuta completa actual: " + rutaOrigen + "\nRuta completa tras corrección: " + destinoCompleto + "\n\n";
				issue.IssueStatus = "Ok";
				contador++;
			} catch
			{
				logCambios += "ERROR201 - " + issue.IssueName + ": Se ha detectado un nombre incorrecto pero ha ocurrido un error al intentar resolverlo.\n\n";
				issue.IssueStatus = "Incorrecto";
				contadorErrores++;
			}
		} else
		{
			logCambios += "ERROR202 - " + issue.IssueName + ": Se ha detectado un nombre incorrecto, pero ya existe un fichero con el nombre que intentamos asignar en la misma revista.\n\n";
			issue.IssueStatus = "Incorrecto";
			contadorErrores++;
		}
	}

	private bool corregirErroresIssuesCaso3(Issue issue, Magazine revistaOrigen, Magazine revistaDestino)
	{
		//Verificar que dentro del magazine de destino no hay issues con el mismo nombre.
		foreach (Issue issueComprobacion in revistaDestino.ListaIssues)
		{
			if (issue.IssueName == issueComprobacion.IssueName)
			{
				logCambios += "ERROR301 - " + issue.IssueName + ": Se ha detectado un issue en la revista incorrecta, se intentó mover a la revista correcta, sin embargo esta revista ya tiene un issue con el nombre que intentamos asignar:\nRevista de origen: " + revistaOrigen.MagazineName + "\nRevista de destino: " + revistaDestino.MagazineName + "\n\n";
				issue.IssueStatus = "Incorrecto";
				contadorErrores++;
				return false;
			}
		}

		//Verificar que no hay ficheros en la ruta de destino.
		string rutaDestino = revistaDestino.rutaCompleta() + "\\" + issue.IssueName;
		if (File.Exists(rutaDestino))
		{
			logCambios += "ERROR302 - " + issue.IssueName + ": Se ha detectado un issue en la revista incorrecta, se intentó mover a la revista correcta, sin embargo el directorio de destino contiene un fichero con el nombre que intentamos asignar (aunque no tenemos ningún issue con ese nombre registrado. Recomendado recargar la aplicación)\nRevista de origen: " + revistaOrigen.MagazineName + "\nRevista de destino: " + revistaDestino.MagazineName + "\n\n";
			issue.IssueStatus = "Incorrecto";
			contadorErrores++;
			return false;
		}

		try
		{
			Directory.CreateDirectory(revistaDestino.rutaCompleta());
			File.Move(issue.rutaCompleta(), rutaDestino);
			revistaOrigen.removeIssue(issue);
			revistaDestino.addIssue(issue);
			issue.IssuePath = revistaDestino.rutaCompleta();
			logCambios += "CORRECTO - " + issue.IssueName + ": Se ha detectado y corregido un problema en un issue: La revista era incorrecta\nRevista de origen: " + revistaOrigen.MagazineName + "\nRevista de destino: " + revistaDestino.MagazineName + "\n\n";
			issue.IssueStatus = "Ok";
			contador++;
			return true;
		} catch
		{
			logCambios += "ERROR303 - " + issue.IssueName + ": Se ha detectado un issue en la revista incorrecta, Ha ocurrido un error al intentar solucionar el problema.\nRevista de origen: " + revistaOrigen.MagazineName + "\nRevista de destino: " + revistaDestino.MagazineName + "\n\n";
			issue.IssueStatus = "Incorrecto";
			contadorErrores++;
			return false;
		}
	}

	private bool corregirErroresIssuesCaso4(Issue issue, Magazine revistaOrigen, Magazine revistaDestino, string nombreDestino)
	{
		//Verificar que dentro del magazine de destino no hay issues con el mismo nombre.
		foreach (Issue issueComprobacion in revistaDestino.ListaIssues)
		{
			if (issue.IssueName == issueComprobacion.IssueName)
			{
				logCambios += "ERROR401 - " + issue.IssueName + ": Se ha detectado un issue con el nombre incorrecto en la revista incorrecta, se intentó renombrar y mover a la revista correcta, sin embargo esta revista ya tiene un issue con el nombre que intentamos asignar:\nRevista de origen: " + revistaOrigen.MagazineName + "\nRevista de destino: " + revistaDestino.MagazineName + "\nNombre destino: " + nombreDestino + "\n\n";
				issue.IssueStatus = "Incorrecto";
				contadorErrores++;
				return false;
			}
		}

		//Verificar que no hay ficheros en la ruta de destino.
		string rutaDestino = revistaDestino.rutaCompleta() + "\\" + nombreDestino;
		if (File.Exists(rutaDestino))
		{
			logCambios += "ERROR402 - " + issue.IssueName + ": Se ha detectado un issue con el nombre incorrecto en la revista incorrecta, se intentó renombrar y mover a la revista correcta, sin embargo el directorio de destino contiene un fichero con el nombre que intentamos asignar (aunque no tenemos ningún issue con ese nombre registrado. Recomendado recargar la aplicación)\nRevista de origen: " + revistaOrigen.MagazineName + "\nRevista de destino: " + revistaDestino.MagazineName + "\nNombre destino: " + nombreDestino + "\n\n";
			issue.IssueStatus = "Incorrecto";
			contadorErrores++;
			return false;
		}

		try
		{
			Directory.CreateDirectory(revistaDestino.rutaCompleta());
			File.Move(issue.rutaCompleta(), rutaDestino);
			revistaOrigen.removeIssue(issue);
			revistaDestino.addIssue(issue);
			issue.IssuePath = revistaDestino.rutaCompleta();
			logCambios += "CORRECTO - " + issue.IssueName + ": Se ha detectado y corregido un problema en un issue: El nombre del issue y la revista en la que estaba eran incorrectos\nRevista de origen: " + revistaOrigen.MagazineName + "\nRevista de destino: " + revistaDestino.MagazineName + "\nNuevo nombre del Issue: " + nombreDestino + "\n\n";
			issue.IssueName = nombreDestino;
			issue.IssueStatus = "Ok";
			contador++;
			return true;
		}
		catch
		{
			logCambios += "ERROR403 - " + issue.IssueName + ": Se ha detectado un issue con el nombre incorrecto en la revista incorrecta, Ha ocurrido un error al intentar solucionar el problema.\nRevista de origen: " + revistaOrigen.MagazineName + "\nRevista de destino: " + revistaDestino.MagazineName + "\nNombre destino: " + nombreDestino + "\n\n";
			issue.IssueStatus = "Incorrecto";
			contadorErrores++;
			return false;
		}
	}

	private void corregirErroresIssuesCaso5(Issue issue)
	{
		logCambios += "ERROR 500 - Falta el issue " + issue.IssueName + " en el sistema de archivos.\n\n";
		contadorErrores++;
	}

	private void corregirErroresIssuesCaso6(Magazine revista, Issue issue)
	{
		logCambios += "ERROR 600 - " + issue.IssueName + ": Issue Desconocido. No aparece en el .xml seleccionado o es un issue duplicado.\n\n";
		contadorErrores++;
		issue.IssueStatus = "Desconocido";
		eliminarIssue(revista, issue);
	}

	private void corregirIssuesDuplicados()
    {
		bool mensajeMostrarDuplicadosMostrado = false;

		foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
        {
			foreach (Magazine revista in carpeta.ListaRevistas)
            {
				for (int i=(revista.ListaIssues.Count-1); i>=0;i--)
                {
					Issue issueAComprobar = (Issue) revista.ListaIssues[i];
					if (issueAComprobar.IssueDuplicado) { 
						if (!mensajeMostrarDuplicadosMostrado)
                        {
							logCambios += "***COMPROBANDO ISSUES DUPLICADOS***\n\n";
							mensajeMostrarDuplicadosMostrado = true;
                        }

						object[] arrayIssue2 = encontrarIssueDuplicado(issueAComprobar);
						if (arrayIssue2 != null)
                        {
							if (issueAComprobar.IssueStatus == "Ok")
							{
								eliminarIssue((Magazine) arrayIssue2[0], (Issue) arrayIssue2[1]);
							} else
							{
								eliminarIssue(revista, issueAComprobar);
							}
                        } else
                        {
							issueAComprobar.IssueDuplicado = false;
							logCambios += "ERROR 2001 - " + issueAComprobar.IssueName + ": Para un issue marcado como duplicado anteriormente: No se ha encontrado su pareja.\n\n";
                        }

						//Elijo que issue conservar y cual eliminar. El que conservo pasa a tener issueDuplicado = false.
					}
                }
            }
        }
    }

	//Este método encuentra el doble de un issue y lo retorna.
	private object[] encontrarIssueDuplicado(Issue issueAComprobar)
    {
		//Repasa todos los issues del almacen comparando sha y md5 hasta encontrar uno que coincida y además tenga "duplicado=false" y el objeto no sea el mismo que el original que comparo.
		foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
        {
			foreach (Magazine revista in carpeta.ListaRevistas)
            {
				foreach (Issue issue in revista.ListaIssues)
                {
					if (issue.IssueSha1 == issueAComprobar.IssueSha1 && issue.IssueMd5 == issueAComprobar.IssueMd5 && !issue.IssueDuplicado && issue != issueAComprobar)
                    {                        
						return new object[]{ revista, issue };
                    }
                }
            }
        }
		return null;
    }

	private void eliminarIssue(Magazine revista, Issue issue)
    {
		//borro de ListaIssues de su revista, suprimo el objeto en sí, y borro el fichero de su ubicación.
		try {
			File.Delete(issue.rutaCompleta());
			revista.removeIssue(issue);
			logCambios += "CORRECTO - " + issue.IssueName + ": Se ha eliminado el issue del sistema de archivos y de la revista de forma satisfactoria.\nRuta fichero eliminado: " + issue.rutaCompleta() + "\n\n";
			issue = null;
			contador++;
			contadorErrores--;
		} catch
        {
			logCambios += "ERROR 2002 - " + issue.IssueName + ": Intento fallido de eliminar el issue duplicado (excepción al intentar borrarlo).\nLa ruta del fichero es: " + issue.rutaCompleta() + "\n\n";
			contadorErrores++;
        }
    }

	//PASO 4A:
	//Repasa toda la lista de magazines e issues asignando valor "OK" a los casos 1, "Incorrecto" a los casos 2,3 y 4 y "Desconocido" a los casos 6.
	//Solo se ejecuta cuando el usuario decido no reparar los problemas encontrados.
	private void asignarEstados()
	{
		foreach (object[] arrayIssue in listaIssuesEstados)
		{
			Issue issue = (Issue)arrayIssue[1];
			switch (arrayIssue[0])
			{
				case 1:
					issue.IssueStatus = "Ok";
					break;
				case 2:
				case 3:
				case 4:
					issue.IssueStatus = "Incorrecto";
					break;
				case 5:
					break;
				case 6:
					issue.IssueStatus = "Desconocido";
					break;
			}
		}
	}

	//Repasa todas las revistas del xml (volver a buscar el XML en el almacen) buscando las que no existen.
	private void corregirErroresRevistas()
	{
		foreach (CarpetaContenedora carpeta in almacen.getListaCarpetas())
        {
			for (int i=(carpeta.ListaRevistas.Count-1); i>=0; i--)
            {
				Magazine revista = (Magazine) carpeta.ListaRevistas[i];
				//Calcular si está vacía:
				int contenidoRevista = 0;
				contenidoRevista += Directory.GetFiles(revista.rutaCompleta()).Length;
				contenidoRevista += Directory.GetDirectories(revista.rutaCompleta()).Length;

				if (contenidoRevista == 0 && !revistaExisteEnElXml(revista))
                {
					try {
						Directory.Delete(revista.rutaCompleta(), true);
						carpeta.removerRevistaDeLaLista(revista);
						logCambios += "CORRECTO - Se ha eliminado la revista " + revista.MagazineName + " porque no existe en el .xml y está vacía.";
					} catch
                    {
						logCambios += "ERROR 1701 - Por motivos desconocidos no se ha podido eliminar la revista " + revista.MagazineName + " a pesar de que no existe en el .xml y parece estar vacía.";
                    }
                }
            }
        }
	}

	private bool revistaExisteEnElXml(Magazine revista)
    {
		foreach (Magazine revistaXml in listaRevistasXml)
        {
			if (revistaXml.MagazineName == revista.MagazineName)
            {
				return true;
            }
        }
		return false;
    }

	private void corregirErroresRevistasCaso2(Magazine revistaOrigen, Magazine revistaDestino)
	{
		//Caso 2: "try/catch" para asignar nuevo nombre al directorio si el de destino no existe. Actualizar ruta Issues internos. Luego estado revista "OK".Break.subir contador.Log_cambios.
		String rutaDestino = revistaOrigen.Ruta + "\\" + revistaDestino.MagazineFileName;
		if (!Directory.Exists(rutaDestino))
		{
			try
			{
				String nombreRevistaAntiguo = revistaOrigen.MagazineName;
				String nombreDirectorioRevistaAntiguo = revistaOrigen.MagazineFileName;
				String ubicacionAnterior = revistaOrigen.rutaCompleta();
				Directory.Move(revistaOrigen.rutaCompleta(), rutaDestino);
				revistaOrigen.MagazineName = revistaDestino.MagazineName;
				revistaOrigen.MagazineFileName = revistaDestino.MagazineFileName;
				revistaOrigen.actualizarRutaIssuesInternos();
				revistaOrigen.MagazineState = "Ok";
				logCambios += "CORRECTO - " + revistaOrigen.MagazineName + ": Se ha detectado y corregido un nombre incorrecto en una revista.\nNombre anterior: " + nombreRevistaAntiguo + "\nNombre actual: " + revistaOrigen.MagazineName + "\nNombre directorio anterior: " + nombreDirectorioRevistaAntiguo + "\nNombre directorio actual: " + revistaOrigen.MagazineFileName + "\nUbicación anterior: " + ubicacionAnterior + "\nUbicación actual: " + revistaOrigen.rutaCompleta() + "\n\n";
				contador++;

				//Vuelvo a repasar si los issues dentro de esta revista tienen errores.
				if (revistaOrigen.ListaIssues.Count > 0)
                {
					logCambios += "*Volviendo a revisar issues con ADVERTENCIA 700 para revista " + revistaOrigen.MagazineName + "*\n\n";
                }

				foreach (Issue issue in revistaOrigen.ListaIssues)
				{
					issue.IssueStatus = ""; //Borrando estado issue.
					object[] resultado = compararIssueConXml(revistaOrigen, issue);
					listaIssuesEstados.Add(resultado);
				}
				corregirErroresIssues();

			} catch
			{
				logCambios += "ERROR 1201 - " + revistaOrigen.MagazineName + ": Ha ocurrido un error intentando corregir el nombre incorrecto de la revista.\nNombre preexistente: " + revistaOrigen.MagazineName + "\nNombre que se intentó poner: " + revistaDestino.MagazineName + "\nRuta completa actual: " + revistaOrigen.rutaCompleta() + "\nRuta que se intentó asignar: " + rutaDestino + "\n\n";
				revistaOrigen.MagazineState = "Incorrecto";
				contadorErrores++;
			}
		} else
		{
			logCambios += "ERROR 1202 - " + revistaOrigen.MagazineName + ": Se ha intentado corregir el nombre de una revista, pero ya existe otra revista en la ruta que se intentó usar.\nNombre preexistente: " + revistaOrigen.MagazineName + "\nNombre que se intentó poner: " + revistaDestino.MagazineName + "\nRuta completa actual: " + revistaOrigen.rutaCompleta() + "\nRuta que se intentó asignar: " + rutaDestino + "\n\n";
			revistaOrigen.MagazineState = "Incorrecto";
			contadorErrores++;
		}


	}

	private int compararContenidoRevistas(Magazine revista1, Magazine revista2)
	{
		int resultado = 0;
		foreach (Issue issue1 in revista1.ListaIssues)
		{
			foreach (Issue issue2 in revista2.ListaIssues)
			{
				//En caso de que el issue coincida comprobamos el resto de posibilidades:
				if (issue1.IssueMd5 == issue2.IssueMd5 && issue1.IssueSha1 == issue2.IssueSha1)
				{
					resultado++;
				}
			}
		}
		return resultado;
	}

	//Genera un fichero de log con los resultados.
	private void generarLog()
	{
		ficheroLog = new LogCambios(almacen, logCambios);
	}

	//Imprimir messageBox con el contador de cambios satisfactorios y errores.
	private void imprimirResultados()
	{
		int errores = (contadorErrores>0)?contadorErrores:0;
		MessageBox.Show("Se ha completado la operación con " + contador + " cambios satisfactorios y " + errores + " errores.\nPuede comprobarlos en el fichero " + ficheroLog.Path, "Resultado cambios de nombre");
	}
}
