<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% 
    string msg = "";
    string classe = "alert alert-success";

    if (RendLibrary.Helper.RequestString("ispostback") != "1")
    {
        try
        {
            msg = String.Copy(TempData["st_Mensagem"].ToString());
        }
        catch { }
        //if (Session["st_Mensagem"] != null && Session["st_Mensagem"].ToString() != String.Empty)
        //{

        //    msg = String.Copy(Session["st_Mensagem"].ToString());

        //}
        //else if (TempData["st_Mensagem"] != null && TempData["st_Mensagem"].ToString() != String.Empty)
        //{

        //    msg = TempData["st_Mensagem"].ToString();
        //}

        if (msg != String.Empty)
        {
             Response.Write("<div class=\"" + classe + "\">" + msg + "</div>");
            // Session["st_Mensagem"] = "";
        }
        else
        {
            // Response.Write("session veio vazia!");
        }
    }
%>