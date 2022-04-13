using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;

namespace RetroRevistasRenamer
{
    public class XmlFactory
    {
        Almacen almacen;
        Herramientas herramientas;
	    DtdReader dtd;
        String path;
        TextWriter writer;
        XmlTextReader reader;
        DateTime dateTime;

        public XmlFactory(Almacen almacen)
        {
            this.almacen = almacen;
            herramientas = new Herramientas();
            crearDtd();
        }

        private void crearDtd()
        {
            if (almacen.getRutaDtd() == null)
            {
                dtd = new DtdReader();
            } else
            {
                dtd = new DtdReader(almacen.getRutaDtd());
            }
        }

        public String getPath()
        {
            return path;
        }

        public void setPath(String path)
        {
            this.path = path;
        }

        public void generarXml()
        {   
            dateTime = DateTime.Now;
            path = almacen.getRutaRaiz() + "\\xml";
            try {
                Directory.CreateDirectory(path);
                path += "\\retrorevistas-test "+dateTime.ToString("dd-MM-yyyy HH mm ss")+".xml";

                if (!File.Exists(path))
                {
                    writer = new StreamWriter(path, true);
                    generarCabecera();
                    generarContenido();
                    cerrarXml();
                    MessageBox.Show("Fichero XML creado", "Generador de XML", MessageBoxButtons.OK);
                }
            } catch
            {
                MessageBox.Show("Error al crear el .xml", "Error .xml");
            }
        }
        
        private void generarCabecera()
        {
            String name = formatoStringXml(almacen.getName());
            String description = formatoStringXml(almacen.getDescription());
            String category = formatoStringXml(almacen.getCategory());
            String version = formatoStringXml(dateTime.ToString("yyyyMMdd"));
            String date = formatoStringXml(dateTime.ToString("yyyy-MM-dd"));
            String author = formatoStringXml(almacen.getAuthor());
            String email = formatoStringXml(almacen.getEmail());
            String homepage = formatoStringXml(almacen.getHomepage());
            String url = formatoStringXml(almacen.getUrl());
            String comments = formatoStringXml(almacen.getComments());
            String formato = formatoStringXml(almacen.getFormato());

            writer.WriteLine("<?xml version='1.0' encoding='UTF-8'?>");
            writer.WriteLine("<!DOCTYPE datafile PUBLIC '-//Logiqx//DTD ROM Management Datafile//EN' '"+dtd.getRutaFichero()+"'>");
            writer.WriteLine("<datafile>");
            writer.WriteLine("    <header>");
            if (name != null)
            {
                writer.WriteLine("        <name>");
                writer.WriteLine("            " + name);
                writer.WriteLine("        </name>");
            }
            if (description != null)
            {
                writer.WriteLine("        <description>");
                writer.WriteLine("            " + description);
                writer.WriteLine("        </description>");
            }
            if (category != null)
            {
                writer.WriteLine("        <category>");
                writer.WriteLine("            " + category);
                writer.WriteLine("        </category>");
            }
            if (version != null)
            {
                writer.WriteLine("        <version>");
                writer.WriteLine("            " + version);
                writer.WriteLine("        </version>");
            }
            if (date != null)
            {
                writer.WriteLine("        <date>");
                writer.WriteLine("            " + date);
                writer.WriteLine("        </date>");
            }
            if (author != null)
            {
                writer.WriteLine("        <author>");
                writer.WriteLine("            " + author);
                writer.WriteLine("        </author>");
            }
            if (email != null)
            {
                writer.WriteLine("        <email>");
                writer.WriteLine("            " + email);
                writer.WriteLine("        </email>");
            }
            if (homepage != null)
            {
                writer.WriteLine("        <homepage>");
                writer.WriteLine("            " + homepage);
                writer.WriteLine("        </homepage>");
            }
            if (url != null)
            {
                writer.WriteLine("        <url>");
                writer.WriteLine("            " + url);
                writer.WriteLine("        </url>");
            }
            if (comments != null)
            {
                writer.WriteLine("        <comment>");
                writer.WriteLine("            " + comments);
                writer.WriteLine("        </comment>");
            }
            if (formato != null)
            {
                writer.WriteLine("        <" + formato + " header=''></"+formato+">");
            }
            writer.WriteLine("    </header>");
        }
        
        private void generarContenido() {
            foreach(CarpetaContenedora carpeta in almacen.getListaCarpetas())
            {
                foreach (Magazine revista in carpeta.ListaRevistas)
                {
                    writer.WriteLine("    <magazine name='" + formatoStringXml(revista.MagazineName) + "' fileName='" + formatoStringXml(revista.MagazineFileName) + "'>");
                    writer.WriteLine("        <description>");
                    writer.WriteLine("            " + formatoStringXml(revista.MagazineDescription));
                    writer.WriteLine("        </description>");
                    writer.WriteLine("        <size>");
                    writer.WriteLine("            " + formatoStringXml(herramientas.procesarPeso(revista.calcularPesoIssuesInternos())));
                    writer.WriteLine("        </size>");
                    writer.WriteLine("        <country>");
                    writer.WriteLine("            " + formatoStringXml(revista.MagazineCountry));
                    writer.WriteLine("        </country>");
                    writer.WriteLine("        <language>");
                    writer.WriteLine("            " + formatoStringXml(revista.MagazineLanguage));
                    writer.WriteLine("        </language>");
                    writer.WriteLine("        <year>");
                    writer.WriteLine("            " + formatoStringXml(revista.MagazineYear));
                    writer.WriteLine("        </year>");
                    writer.WriteLine("        <publisher>");
                    writer.WriteLine("            " + formatoStringXml(revista.MagazinePublisher));
                    writer.WriteLine("        </publisher>");
                    foreach(Issue issue in revista.ListaIssues)
                    {
                        writer.WriteLine("        <issue name='" + formatoStringXml(issue.IssueName) + "' size='" + formatoStringXml(herramientas.procesarPeso(issue.IssueSize)) + "' md5='" + formatoStringXml(issue.IssueMd5) + "' sha1='" + formatoStringXml(issue.IssueSha1) + "' number='" + formatoStringXml(issue.IssueNumber) + "' />");
                    }
                    writer.WriteLine("    </magazine>");
                }
            }
            writer.WriteLine("</datafile>");
        }
        
        public ArrayList leerXml()
        {
            //Leer el XML para generar un arraylist con arays que contengan "nombre revista" y arraylists de issues.
            //Dentro de los arraylist de issues almacenar String[] (arrays) con toda la informacion de cada issues
            ArrayList resultado = new ArrayList();

            if (File.Exists(path))
            {
                try
                {
                    reader = new XmlTextReader(path);
                    Magazine revista = new Magazine();
                    while (reader.Read())
                    {
                        XmlNodeType node = reader.NodeType;
                        switch (node)
                        {
                            case XmlNodeType.Element:
                                String elementText = reader.Name;
                                if (elementText == "magazine")
                                {
                                    revista.MagazineName = leerFormatoStringXml(reader.GetAttribute(0));//Nombre de la revista.
                                    revista.MagazineFileName = leerFormatoStringXml(reader.GetAttribute(1));//Nombre que debería tener la carpeta que contiene la revista.
                                }
                                else if (elementText == "issue")
                                {
                                    Issue issue = new Issue();
                                    issue.IssueName = leerFormatoStringXml(reader.GetAttribute(0));
                                    issue.IssueSize = herramientas.procesarPesoInverso(leerFormatoStringXml(reader.GetAttribute(1)));
                                    issue.IssueMd5 = leerFormatoStringXml(reader.GetAttribute(2));
                                    issue.IssueSha1 = leerFormatoStringXml(reader.GetAttribute(3));
                                    issue.IssueNumber = leerFormatoStringXml(reader.GetAttribute(4));
                                    revista.addIssue(issue);
                                }
                                break;
                            case XmlNodeType.EndElement:
                                if (reader.Name == "magazine")
                                {
                                    resultado.Add(revista);
                                    revista = new Magazine();
                                }
                                break;
                        }
                    }
                } catch
                {
                    //Retorna resultado como arraylist vacío.
                }
            }
            cerrarXml();
            return resultado;
        }
        
        private void cerrarXml()
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (writer != null)
            {
                writer.Close();
            }
        }

        public String formatoStringXml(String input)
        {
            if (input != null)
            {
                String[] chars = new String[] {"<", ">", "&", "\"", "'"};
                String[] codeChars = new String[] {@"&lt;", @"&gt;", @"&amp;", @"&quot;", @"&apos;"};

                for (int i=0; i<chars.Length; i++)
                {
                    input = input.Replace(chars[i], codeChars[i]);
                }
                input = input.Trim();
            }

            return input;
        }

        public String leerFormatoStringXml(String input)
        {
            if (input != null)
            {
                String[] chars = new String[] {"<", ">", "&", "\"", "'"};
                String[] codeChars = new String[] {@"&lt;", @"&gt;", @"&amp;", @"&quot;", @"&apos;"};

                for (int i=0; i<chars.Length; i++)
                {
                    input = input.Replace(codeChars[i], chars[i]);
                }
                input = input.Trim();
            }

            return input;
        }
    }
}