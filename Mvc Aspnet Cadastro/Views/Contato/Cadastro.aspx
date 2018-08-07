<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Layout.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Register src="../Shared/ucBotoes.ascx" tagname="ucBotoes" tagprefix="uc1" %>
<%@ Register src="../Shared/ucBotaoVoltar.ascx" tagname="ucBotaoVoltar" tagprefix="uc1" %>
<%@ Register src="GridEmail.ascx" tagname="GridEmail" tagprefix="uc2" %>
<%@ Register src="../Shared/ucMensagem.ascx" tagname="ucMensagem" tagprefix="uc3" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Cadastro de Contato
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    
<% 
    
    
    System.Data.DataRow registro = null ;

    if ( ViewBag.registro != null ){
                registro = (System.Data.DataRow)ViewBag.registro; 
     }
   
    
 %>

     <table class='breadcrumbtable' border="0" style='width: 99%; '>
         <tbody>

        <tr>
            <td>
                
    <h1 >Cadastro - Contato</h1>
            </td>
            
            <td style="text-align: right">
              
              
            </td>
        </tr>
         </tbody>
    </table>


      <form id="form1" method="post" action="<% Response.Write(  Url.Action("/Cadastro" ) ); %>">
                <input type="hidden" name="mn" value="" />
	           <input type="hidden" name="acao" value="LOAD" />
	           <input type="hidden" name="ispostback" value="1" />
               <input type="hidden" name="id" value="<% RendLibrary.Helper.showStr(registro, "id"); %>" />


          
               <div class="fieldBox">


                   
    <uc3:ucMensagem ID="ucDvMensagem" runat="server" />
 
                <table cellpadding="0" cellspacing="0"  class="tbcadastro" style="max-width: 500px">
    
	              <% if (registro != null &&  registro["id"] != DBNull.Value ){ %>
                  <thead>
      
	                   <tr   >
                        <td class="textoBold"><label>ID:</label> &nbsp;
                         <span class='mostrapk'><% RendLibrary.Helper.showStr(registro, "id"); %>  </span>
		                   </td>
		                </tr>
		
	
                  </thead>
                <% } %>   
                    <tr>
                        <td colspan="2">
                            <label>Nome</label><span class="campoObrigatorio"> * </span><br />
                            <input type="text" name="nome" value="<% RendLibrary.Helper.showStr(registro, "nome"); %>" maxlength="300" style="width: 100%">
                        </td>

                    </tr>
                     <tr>
                        <td colspan="2">
                            <label>Empresa</label><span class="campoObrigatorio" style="display:none"> * </span><br />
                            <input type="text" name="empresa" value="<% RendLibrary.Helper.showStr(registro, "empresa"); %>" maxlength="300" style="width: 100%">
                        </td>

                    </tr>
                  

                    <tr>
                        <td colspan="2">
                            <div id="div_emails" >
                                <% // Html.Partial("GridEmail", null, new ViewDataDictionary(ViewBag) ); %>
                                
                               <uc2:GridEmail ID="GridEmail1" runat="server" />
                            </div>
                         </td>


                    </tr>

                     <tr>
                        <td>
                            <label>Tel. Pessoal</label><span class="campoObrigatorio" style="display:none"> * </span><br />
                            <input type="text" name="telefone_pessoal" value="<% RendLibrary.Helper.showStr(registro, "telefone_pessoal"); %>" maxlength="16"
                                 onkeypress="return MascaraTelefone(this, event)"
                                 style="width: 110px">
                        </td>
                         <td>
                            <label>Tel. Comercial</label><span class="campoObrigatorio" style="display:none"> * </span><br />
                            <input type="text" name="telefone_comercial" value="<% RendLibrary.Helper.showStr(registro, "telefone_comercial"); %>" maxlength="16" 
                                 onkeypress="return MascaraTelefone(this, event)"
                                style="width: 110px">
                        </td>
                    </tr>
                      <% if (registro != null && registro["data_cadastro"] != DBNull.Value)
                       { %>
                     <tr>
                        <td colspan="2">
                            <label>Data de Cadastro: </label>
                            <% RendLibrary.Helper.showStr(registro, "data_cadastro","dd/MM/yyyy hh:mm"); %>
                        </td>

                    </tr>
                    <% } %>
                    <tr>
                        <td colspan="2">
                            
                          <div class="campoObrigatorio"  ><i> * Campo(s) Obrigatório(s)</i></div>  
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                <uc1:ucBotoes ID="ucBotoes1" runat="server" />

                        </td>

                    </tr>

                 </table>
                  </div>
         

     </form>


    <uc1:ucBotaoVoltar ID="ucBotaoVoltar1" runat="server" ></uc1:ucBotaoVoltar>

<script type="text/javascript" src="<%: Url.Content("~/Assets") %>/javascript/actions/contato.js?y=00025" ></script>
<script type="text/javascript" >
    function salvar() {
        var f = document.forms[0];

      
        if (!obj_contato.form_valido(f) )
            return false;

        f.acao.value = "SAVE";
        f.submit();

    }

    function novo() {
        var f = document.forms[0];
        document.location.href = f.action;
    }


    function excluir() {

        var f = document.forms[0];
        if (f.id.value == "") {
            alert("Não há registro para excluir!");
            return false;
        }


        f.acao.value = "DEL";
        f.submit();

    }
</script>
</asp:Content>
