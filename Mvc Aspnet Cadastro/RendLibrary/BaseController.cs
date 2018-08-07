using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using System.Collections;

namespace RendLibrary
{
    public class BaseController : Controller
    {

        /// <summary>
        /// Obtém a conexão principal com o banco de dados.
        /// </summary>
        /// <returns></returns>
        public IDbPersist getMainConn()
        {

            if (ViewData["oConn"] != null)
                return (DataAccess.IDbPersist)ViewData["oConn"];


            DataAccess.IDbPersist oConn = DataAccess.FactoryConn.getConn(System.Configuration.ConfigurationManager.AppSettings["tipo_sgbd"],
                                    System.Configuration.ConfigurationManager.ConnectionStrings["bdConnectionString"].ConnectionString,
                                    "connmysql");

            ViewData["oConn"] = oConn;

            return oConn;

        }



        /// <summary>
        /// Guarda o status atual da tela para poder voltar a mesma, nas mesmas características, caso necessário.
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="pref"></param>
        public void guardaSessao(string pref)
        {

            string url = Request.ServerVariables["URL"].ToString();

            string queryString = string.Empty;
            string sep = "?";

          
            ArrayList arrForm = new ArrayList();
            ArrayList valoresForm = new ArrayList();
            ArrayList arrGET = new ArrayList();
            ArrayList valoresGET = new ArrayList();

            try
            {
                if (Request != null && Request.Form != null)
                {
                    for (int i = 0; i < Request.Form.AllKeys.Length; i++)
                    {

                        if (Request.Form.AllKeys[i] == null)
                            break;


                        if (queryString.IndexOf("&" + Request.Form.AllKeys[i] + "=") > -1)
                            continue;


                        if (queryString.IndexOf("?" + Request.Form.AllKeys[i] + "=") > -1)
                            continue;


                        if (Request.Form.AllKeys[i].IndexOf("EVENTTARGET") > -1)
                            continue;


                        if (Request.Form.AllKeys[i].IndexOf("EVENTARGUMENT") > -1)
                            continue;


                        if (Request.Form.AllKeys[i].IndexOf("__VIEWSTATE") > -1)
                            continue;

                        if (Request.Form.AllKeys[i].IndexOf("__VIEWSTATEGENERATOR") > -1)
                            continue;


                        if (Request.Form.AllKeys[i].IndexOf("ClientState") > -1)
                            continue;



                        if (Request.Form.AllKeys[i].IndexOf("VIEWSTATE") > -1)
                            continue;


                        if (queryString != String.Empty)
                            sep = "&";


                        queryString += sep + Request.Form.AllKeys[i] + "=" + Request.Form[Request.Form.AllKeys[i]];
                        arrForm.Add(Request.Form.AllKeys[i]);
                        valoresForm.Add(Request.Form[Request.Form.AllKeys[i]]);

                    }
                }
                if (Request != null && Request.QueryString != null)
                {

                    for (int i = 0; i < Request.QueryString.AllKeys.Length; i++)
                    {
                        if (Request.QueryString.AllKeys[i].IndexOf("ClientState") > -1)
                            continue;

                        if (Request.QueryString.AllKeys[i] == null)
                            continue;

                        if (queryString.IndexOf("&" + Request.QueryString.AllKeys[i] + "=") > -1)
                            continue;

                        if (queryString.IndexOf("?" + Request.QueryString.AllKeys[i] + "=") > -1)
                            continue;



                        if (queryString != String.Empty)
                            sep = "&";

                        if (Request.QueryString[Request.QueryString.AllKeys[i]].ToString() == String.Empty)
                        {
                            continue;
                        }

                        if (!arrForm.Contains(Request.QueryString.AllKeys[i]))
                        {
                            queryString += sep + Request.QueryString.AllKeys[i] + "=" + Request.QueryString[Request.QueryString.AllKeys[i]];
                            arrGET.Add(Request.QueryString.AllKeys[i]);
                            valoresGET.Add(Request.QueryString[Request.QueryString.AllKeys[i]]);
                        }
                    }
                }
            }
            catch { }


            queryString = string.Empty;
            sep = "?";

            for (int i = 0; i < arrForm.Count; i++)
            {


                if (queryString != String.Empty)
                    sep = "&";

                if (arrForm[i].ToString() != String.Empty && valoresForm[i] != null &&
                     valoresForm[i].ToString() != String.Empty)
                {
                    queryString += sep + arrForm[i].ToString() + "=" + valoresForm[i].ToString();
                }
            }

            for (int i = 0; i < arrGET.Count; i++)
            {


                if (queryString != String.Empty)
                    sep = "&";

                if (arrGET[i].ToString() != String.Empty && valoresGET[i] != null &&
                     valoresGET[i].ToString() != String.Empty)
                {
                    queryString += sep + arrGET[i].ToString() + "=" + valoresGET[i].ToString();

                    //A preferência é sempre da variável POST
                    if (!arrForm.Contains(arrGET[i].ToString()))
                    {
                        arrForm.Add(arrGET[i].ToString());
                        valoresForm.Add(valoresGET[i].ToString());
                    }
                }
            }

            Session["_url_lista" + pref] = url + queryString;
            Session["_url_arrays_def" + pref] = arrForm;
            Session["_url_arrays_val" + pref] = valoresForm;
            Session["_url_lista_limpa" + pref] = Request.ServerVariables["URL"].ToString();
       
        }

    }
}