<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register src="../Shared/ucPaginacao.ascx" tagname="ucPaginacao" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Lista de Contato
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form id="form1" runat="server">
<%
    System.Data.DataTable dtLista =  (System.Data.DataTable)ViewBag.dtLista;
    RendLibrary.Paginator paginator  =  (RendLibrary.Paginator)ViewBag.Paginator;
    
%>


         <table class='breadcrumbtable' border="0" style='width: 99%; '>
        <tr>
            <td>
                
    <h1 >Lista - Contato</h1>
            </td>
            
            <td style="text-align: right">
              
              
            </td>
        </tr>
    </table>


<div class="fieldBox">
    <div class="row">
    <div class="col-xs-12" style="width: 98%; margin-left: 1%">
	
     <div class="form-inline">
	 <h4 class="sistem-subtitle">Filtrar por:</h4>
                 <div class="row col-xs-12">
         <div class="form-group">
              <input type="text"  name="nome" class="form-control" placeholder="Nome do contato" value="<%: RendLibrary.Helper.RequestString("nome") %>"   maxlength="300" />
             
         </div>
              	
     
         <div class="form-group">
             
         <input type="text" placeholder="Empresa" class="form-control"  name="empresa" value="<%: RendLibrary.Helper.RequestString("empresa") %>"   maxlength="400" />
             
         </div>	

              <div class="form-group">
             
                   <input type="text" placeholder="E-mail" class="form-control"  name="email" value="<%: RendLibrary.Helper.RequestString("email") %>"   maxlength="400" />
             
             </div>	   
                     
              <div class="form-group">
             
                  <input type="text" placeholder="Telefone" class="form-control"  name="telefone"  value="<%: RendLibrary.Helper.RequestString("telefone") %>"   maxlength="19" />
             
             </div>
                  <div class="form-group">
         		<input type="button" class="btn btn-primary" class="botao" value="Pesquisar" value="Buscar" onclick="document.forms[0].submit()" />
                   
                 </div>       
                 </div>
         
          

         </div>

    </div>
        </div>


        <div class="row">
        <div class="col-xs-12">
            
            <br />
	<table width="95%" cellspacing="0" cellpadding="0"  align="center" ><tr>

	<td  class="t-bold" style="width: 150px">Lista de registro(s) </td>

	<td  style="text-align: right; ">
	<a href="<%: Url.Action("/Cadastro") %>" title="Novo Cadastro"><span class="glyphicon glyphicon-plus"></span></a>
	
        &nbsp;&nbsp;&nbsp;
	<a href="#" onclick="to_excel('<%: Url.Action("/Excel") %>') "><img width="20" height="20" src="<%: Url.Content("~/Assets") %>/images/excel.png" title="Exportar para excel" /></a>
	
	</td>
	</tr></table>

            </div>
            </div>


        <div class="row">
        <div class="col-xs-12">
    
	<table class="table table-hover table-condensed " style="width: 98%; margin-left: 1%"   	>
    <thead>
	 <tr  class="trheader">
     
			  
				<th  align="center" class="header">ID</th>
				<th  align="center" class="header">Nome</th>
			  
				<th  align="center" class="header">Empresa</th>
				<th  align="center" class="header">Tel. Pessoal</th>
				<th  align="center" class="header">Tel. Comercial</th>
				<th  align="center" class="header">Emails</th>
				<th  align="center" class="header">Data de Cadastro</th>		
             
			   <th></th>
	 </tr>
	 </thead>
        <tbody>

              <% if ( dtLista.Rows.Count <= 0 ){  %> 
                  <tr>
                    <td colspan="8" class="f-tabela-texto">
                      N&atilde;o h&aacute; dados a serem exibidos!
                    </td>
                  </tr>
             <% } %>

            
              <% for ( int z = 0; z < dtLista.Rows.Count; z++ ){ 
                  
                  	if (z >= paginator.fim)
		                  break;
	
	                if (z < paginator.inicio)
		                continue;
                  
                  System.Data.DataRow item = dtLista.Rows[z];
                   %> 
             <tr>
                 
			 <td align="center" class="td"><% RendLibrary.Helper.showStr(item,"id"); %></td> 
			 <td align="left" class="td"><% RendLibrary.Helper.showStr(item,"nome"); %></td> 
			 <td align="left" class="td"><% RendLibrary.Helper.showStr(item,"empresa"); %></td> 
			 <td align="left" class="td"><% RendLibrary.Helper.showStr(item,"telefone_pessoal"); %></td> 
			 <td align="left" class="td"><% RendLibrary.Helper.showStr(item,"telefone_comercial"); %></td> 
			 <td align="left" class="td"><%: RendLibrary.Helper.limita( RendLibrary.Helper.getStrDataRow (item,"emails"), 50) %></td> 
			 <td align="left" class="td"><% RendLibrary.Helper.showStr(item,"data_cadastro","dd/MM/yyyy HH:mm"); %></td> 
                 <td>
                       <a href="<%: Url.Action("/Cadastro" ) + "?id="+item["id"].ToString()+"&acao=LOAD" %>" data-add-class="tipr_black" title="Editar" onclick="load('<% RendLibrary.Helper.showStr(item,"id"); %>');">
                                  <span class="glyphicon glyphicon-edit"></span>
                              </a>

                 </td>
		      </tr>
             <% } %>

		


        </tbody>
          <tfoot>
                	<tr>
                 		<td colspan="8">
                    	<div class="form-inline">
                      	<div class="form-group form-group-sm">
                          <table>

                              
                      	    <uc1:ucPaginacao ID="ucPaginacao1" runat="server" />
                          </table>
                         
                      	</div>
                    	</div>
                  	</td>
                	</tr>
              	</tfoot>

        </table>


            </div>
            </div>
    </div>
    </form>
<script type="text/javascript">
    function to_excel( url ) {

        var f = document.forms[0];
        var oldact = f.action;


        f.action = url;
        f.target = "_blank";
        f.submit();

        f.action = oldact;
        f.target = "_self";

    }



</script>


</asp:Content>
