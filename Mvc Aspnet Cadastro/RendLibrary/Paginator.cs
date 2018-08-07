using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RendLibrary
{
    public class Paginator
    {

        public int selQtdeRegistro = 10;
        public int selPagina = 1;
        public int totalRegistro = 1;
        public int inicio = 0;
        public int fim = 0;
        /// <summary>
        /// Prefiro para ser adicionado em cada nome de input da própria paginação.
        /// </summary>
        public string prefixo = "";
        public string Url = "";
        public int pageCount = 0;

        public bool mostrarPaginas = true;
        public bool mostrarSetas = true;


        /// <summary>
        /// Realiza o calculo para a paginação. 
        /// </summary>
        /// <param name="obj">Objeto paginator</param>
        /// 
        public static void calculoPaginacao(ref Paginator obj )
            {

	         int selQtdeRegistro = obj.selQtdeRegistro;
             int selPagina = obj.selPagina;
             int totalRegistro = obj.totalRegistro;
             int inicio = 0 ;
             int fim = 0;
            
            
        
	            if ( ! Helper.isInteger(selQtdeRegistro.ToString()))
	                  selQtdeRegistro = 0;
	
	    
	            if ( ! Helper.isInteger(totalRegistro.ToString()))
	                  totalRegistro = 0;

            
	            if ( ! Helper.isInteger(selPagina.ToString()))
	                  selPagina = 1;
	          
	
	            Double dbl_pageCount = Convert.ToDouble(totalRegistro) /  Convert.ToDouble(selQtdeRegistro);
	            int pageCount = (int) dbl_pageCount;

	            if (pageCount < 1)
		            pageCount = 1; 
	
	            if (dbl_pageCount >  pageCount )
	            {    
                    pageCount++;
                }
	
	
	            //echo  $selPagina . "-- ".$pageCount;
	
	            if ( Convert.ToInt32(selPagina) > pageCount){
		            selPagina = pageCount;
                }
	
                 inicio = Convert.ToInt32(selQtdeRegistro)  * (Convert.ToInt32(selPagina) -1);
                 fim = inicio + Convert.ToInt32(selQtdeRegistro);
    
 
                 if (fim > Convert.ToInt32( totalRegistro) ){
                     fim = Convert.ToInt32(totalRegistro);
                 }

                 obj.fim = fim;
                 obj.inicio = inicio;
                 obj.pageCount = pageCount;
                 obj.selPagina = selPagina;

             }

        private static List<System.Web.Mvc.SelectListItem> listSelQtdeRegistro;
        public static List<System.Web.Mvc.SelectListItem> getlistSelQtdeRegistro(string selected)
        {

            if (listSelQtdeRegistro == null)
            {
                listSelQtdeRegistro = new List<System.Web.Mvc.SelectListItem>()
                                                {
                                                    new System.Web.Mvc.SelectListItem() {Text="10 Registros", Value="10", Selected = selected== "10" },
                                                    new System.Web.Mvc.SelectListItem() { Text="15 Registros", Value="15", Selected = selected== "15"},
                                                    new System.Web.Mvc.SelectListItem() { Text="20 Registros", Value="20" , Selected = selected== "20"},
                                                };
            }

            return listSelQtdeRegistro;

        }
            
         

        }



}