using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroRevistasRenamer
{

    public class CarpetaContenedora
    {

        //DECLARACIÓN DE VARIABLES
        String ruta;
        ArrayList listaRevistas;
        bool forRecovery;

        //CONSTRUCTORES
        public CarpetaContenedora()
        {
            this.Ruta = "";
            this.ListaRevistas = new ArrayList();
            this.ForRecovery = false;
        }

        public CarpetaContenedora(String ruta)
        {
            this.Ruta = ruta;
            this.ListaRevistas = new ArrayList();
            this.ForRecovery = false;
        }


        //GETTERS y SETTERS
        public string Ruta { get => ruta; set => ruta = value; }
        public ArrayList ListaRevistas { get => listaRevistas; set => listaRevistas = value; }
        public bool ForRecovery { get => forRecovery; set => forRecovery = value; }
     

        //MÉTODOS
        public void anyadirRevistaALista(Magazine revista)
        {
            this.ListaRevistas.Add(revista);
        }

        public void removerRevistaDeLaLista(Magazine revista)
        {
            this.ListaRevistas.Remove(revista);
        }


        //TOSTRING
        public override String ToString()
        {
            Herramientas herramientas = new Herramientas();
            return herramientas.obtenerUltimoElemento(this.Ruta);
        }

        public void destroyMagazines()
        {
            for (int i = (listaRevistas.Count-1); i>=0; i--)
            {
                Magazine revista = (Magazine) listaRevistas[i];
                revista.destroyIssues();
                listaRevistas.Remove(revista);
                revista = null;
            }
        }

    }
}
