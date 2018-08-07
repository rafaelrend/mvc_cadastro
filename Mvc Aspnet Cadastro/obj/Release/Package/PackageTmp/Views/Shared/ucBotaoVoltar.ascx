<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div  class="interMenu" id="dvVoltar">
<form method="post" name="frmVoltar" action="<%:  Url.Action("/Lista" ) %>">

    <a href="#" onclick="document.frmVoltar.submit();" class="btn btn-midiaclipazul btn-xs" > <span class="glyphicon glyphicon-arrow-left"></span> Voltar </a>
    <%
       string controller =  HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
       if (Session["_url_lista" + controller] != null)
       {


                   if (Session["_url_arrays_def" + controller] != null)
                   {
                       ArrayList arrNames = (ArrayList)Session["_url_arrays_def" + controller];
                       ArrayList arrValues = (ArrayList)Session["_url_arrays_val" + controller];


                       Response.Write( RendLibrary.Helper.getInputsHidden(arrNames, arrValues) );
                   }   
       } 
                %>

</form>
    </div>