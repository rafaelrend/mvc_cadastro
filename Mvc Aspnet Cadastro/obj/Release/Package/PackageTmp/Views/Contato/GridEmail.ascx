<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%
    
    
    System.Data.DataTable dtEmails = (System.Data.DataTable)ViewContext.Controller.ViewBag.DtEmails;

    if (dtEmails == null)
        Response.Write("Null ViewBag");

    //Response.Write(dtEmails.Rows.Count.ToString() + " registros ");
    
 %>

<label>Emails</label>
<% for (int i = 0; i < dtEmails.Rows.Count; i++)
   {
       System.Data.DataRow dr_emails = dtEmails.Rows[i];
        %>
       <div id="div_email<%Response.Write(i); %>" >
                   <input type="text" name="email<%Response.Write(i); %>" id="email<%Response.Write(i); %>" 
                       value="<% RendLibrary.Helper.showStr(dr_emails, "email"); %>"
                       style="width: 300px" />

         <% if ( i == 0 ){ %>
                                                    <a href="#!"  onclick="obj_contato.add_email()">
                                                        <span class="glyphicon glyphicon-plus" ></span>
                                                    </a>
          <% } else { %> 

           
                                                    <a href="#!"  onclick="obj_contato.remove_email('<%Response.Write(i); %>')">
                                                        <span class="glyphicon glyphicon-remove-circle" ></span>
                                                    </a>
           

          <% } %>
           </div>
 <%  }
   %>
<input type="hidden" name="qtde_emails" value="<% Response.Write(dtEmails.Rows.Count.ToString()); %>" id="qtde_emails" />