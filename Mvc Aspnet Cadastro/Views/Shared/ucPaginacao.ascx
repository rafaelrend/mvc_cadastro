<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    var oPag = (RendLibrary.Paginator)ViewContext.Controller.ViewBag.paginator;


    if (oPag.selQtdeRegistro >= oPag.totalRegistro)
    {
      Response.Write(   Html.Hidden(oPag.prefixo + "selQtdeRegistro", oPag.selQtdeRegistro).ToString() );
    }
    else
    {

        Double dbl_pageCount = Convert.ToDouble(oPag.totalRegistro) / Convert.ToDouble(oPag.selQtdeRegistro);
        int pageCount = (int)dbl_pageCount;

        if (pageCount < 1)
            pageCount = 1;

        if (dbl_pageCount > pageCount)
        {
            pageCount++;
        }

      %>
          <tr>
              <td >
	
	       <% if ( oPag.mostrarPaginas ) { 
               
               
                      var list = RendLibrary.Paginator.getlistSelQtdeRegistro(oPag.selQtdeRegistro.ToString());
               %>
                  Mostrar at&eacute;  

                 <select  
                    style="width: auto"  name ="<% Response.Write(oPag.prefixo); %>selQtdeRegistro" id ="<% Response.Write(oPag.prefixo); %>selQtdeRegistro" 
                    onchange="obj_paginator.submitPost( this )">

                  <% 
                  for (int i = 0; i < list.Count; i++)
			            {
			   
                         string selected = "";
                         
                            if ( i.ToString() == list[i].Value )
                                  selected = " selected ";
                         
                            Response.Write("<option "+selected+" value=\""+list[i].Value+"\">"+list[i].Text+"</option>");
			            }
                     %>
                     </select>
                  por p&aacute;gina 
           <% } else { 
             
                   Html.Hidden(oPag.prefixo+"selQtdeRegistro",  oPag.selQtdeRegistro); 
              }  %>        
             </td>
	
	
	 <% if ( oPag.mostrarSetas  &&  oPag.pageCount > 1 ){
         
         if ( oPag.selPagina > 1 ){
         
		%>
		       <td class="f-tabela-paginacao" >
			   
                         <a style="color: #4B636B" href="javascript: obj_paginator.botaoPagina(-1, '<% Response.Write(oPag.prefixo); %>');">
                        <span class="glyphicon glyphicon-circle-arrow-left" style="font-size: 22px" ></span>
                        </a> 
                        
		     </td>
         <% } %>
		 <%  if ( oPag.selPagina < oPag.pageCount ) { %>
                   
                    <td class="f-tabela-paginacao" >
                        <a style="color: #4B636B" href="javascript: obj_paginator.botaoPagina(1, '<% Response.Write(oPag.prefixo); %>');">
                        <span class="glyphicon glyphicon-circle-arrow-right" style="font-size: 22px" ></span>
                        </a> 
                    </td>
         <% }
         } %>
	
	
              <td class="f-tabela-paginacao" >Mostrar p&aacute;gina 
                <select  
                    style="width: auto"  name ="<% Response.Write(oPag.prefixo); %>selPagina" id ="<% Response.Write(oPag.prefixo); %>selPagina" 
                    onchange="obj_paginator.mostrarPagina( this )">

                 <%
                     int z = 1, fim = 0;
                     
                     fim = oPag.pageCount;
                     
                     if ( fim == 0 ){ fim = 1; }
                  
                     while ( z <= fim ) {
                         
                         string selected = "";
                         
                         if ( z == oPag.selPagina )
                             selected = " selected ";
                         
                         Response.Write("<option "+selected+" value=\""+z.ToString()+"\">"+z.ToString()+"</option>");

                         z++;
                     }   
                      %> 
				</select> 
                de <% Response.Write(fim); %>  </td>
              <td class="f-tabela-paginacao" >Total de Registros: <% Response.Write(oPag.totalRegistro); %> </td>
            </tr>
<% } %>

