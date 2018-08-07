using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using Models;
using RendLibrary;

namespace Mvc_Aspnet_Cadastro.Controllers
{
    public class ContatoController : RendLibrary.BaseController
    {
        //
        // GET: /Contato/
        #region Actions
        public ActionResult Lista()
        {

            DataAccess.IDbPersist oConn = base.getMainConn();

            RendLibrary.Paginator paginator = new RendLibrary.Paginator();
            //Setando a paginação.. quantidade mínima de registros por página.
            if (System.Configuration.ConfigurationManager.AppSettings["minimo_pagina"].ToString() != "")
            {
                paginator.selQtdeRegistro = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["minimo_pagina"].ToString());
            }
            else
            {
                paginator.selQtdeRegistro = 10;
            }
            if (Helper.RequestString("selPagina") != String.Empty && Helper.isInteger( Helper.RequestString("selPagina") ) )
            {
                paginator.selPagina = Helper.RequestValue<int>("selPagina").Value;
            }

            if (Helper.RequestString("selQtdeRegistro") != String.Empty && Helper.isInteger(Helper.RequestString("selQtdeRegistro")))
            {
                paginator.selQtdeRegistro = Helper.RequestValue<int>("selQtdeRegistro").Value;
            }

            //paginator.selQtdeRegistro = 2;
            DataTable dtLista = getContatoDataList(oConn);

            paginator.totalRegistro = dtLista.Rows.Count;
        
            //Fazendo o cálculo da paginação...
            Paginator.calculoPaginacao(ref paginator);

            ViewData["Paginator"] = paginator;
            ViewData["dtLista"] = dtLista;

            
            string controller =  Request.RequestContext.RouteData.Values["controller"].ToString();
            base.guardaSessao(controller);

            return View();
        }

    
        public ActionResult Cadastro()
        {
           
            DataAccess.IDbPersist oConn = base.getMainConn();

            DataRow registro = ConnAccess.getNewRow(oConn, "contato");
            DataRow dr_email = ConnAccess.getNewRow(oConn, "contato_email");

            DataTable dtEmails = dr_email.Table;

            string id = RendLibrary.Helper.RequestString("id");
            string acao = RendLibrary.Helper.RequestString("acao");
            string ispostback = RendLibrary.Helper.RequestString("ispostback");



            if (ispostback == "1")
            {
                if (id.Trim() != String.Empty && RendLibrary.Helper.isInteger(id))
                {
                    registro = ConnAccess.getRow(oConn, "contato", "id", id);
                }

                RendLibrary.Helper.loadDataRowFromForm(registro);
                
                if (registro["data_cadastro"] == DBNull.Value)
                    registro["data_cadastro"] = DateTime.Now;


            }

            if (acao == "SAVE")
            {

                if (registro["id"] == DBNull.Value)
                {
                    registro["data_cadastro"] = DateTime.Now;

                    ConnAccess.Insert(oConn, registro, "id", true);
                    registro["id"] = ConnAccess.getMax(oConn, "id", "contato", "");
                }
                else
                {
                    ConnAccess.Update(oConn, registro, "id");
                }

                if (registro["id"] != DBNull.Value)
                {
                    this.salvaEmails(Convert.ToInt32(registro["id"]), oConn);

                }

                TempData["st_Mensagem"] = "Contato salvo com sucesso!";
                //TempData["st_Mensagem"] = "Contato salvo com sucesso!";

                Response.Redirect("Cadastro?id=" + registro["id"].ToString() + "&acao=LOAD", true);
                Response.End();
            }


            if (acao == "DEL" && id.Trim() != String.Empty && RendLibrary.Helper.isInteger(id))
            {
                ConnAccess.executeCommand(oConn, " delete from contato where id = " + id.ToString());
                TempData["st_Mensagem"] = "Contato removido com sucesso!";

                Response.Redirect("Cadastro", true);
                Response.End();
            }


            if (id.Trim() != String.Empty && RendLibrary.Helper.isInteger(id))
            {
                registro = ConnAccess.getRow(oConn, "contato", "id", id);
                dtEmails = ConnAccess.fetchData(oConn, " select * from contato_email where id_contato = " +
                                                id + " order by ordem_cadastro ");
            }
            
            if ( dtEmails.Rows.Count <= 0 )
            {
                dtEmails.Rows.Add(dr_email.ItemArray);
            }

            ViewData["registro"] = registro;
            ViewData["DtEmails"] = dtEmails;
            
            return View();
        }

        /// <summary>
        /// Exportando dados para o excel..
        /// </summary>
        /// <returns></returns>
        public ActionResult Excel()
        {

            DataAccess.IDbPersist oConn = base.getMainConn();
            DataTable dt = getContatoDataList(oConn);

            dt.Columns["id"].ColumnName = "ID";
            dt.Columns["nome"].ColumnName = "Nome";
            dt.Columns["empresa"].ColumnName = "Empresa";
            dt.Columns["telefone_comercial"].ColumnName = "Telefone Comercial";
            dt.Columns["telefone_pessoal"].ColumnName = "Telefone Pessoal";
            dt.Columns["emails"].ColumnName = "Emails";
            dt.Columns["data_cadastro"].ColumnName = "Data Cadastro";

            dt.TableName = "Contato";

            string path = Server.MapPath("~/tmp");
            Helper.limpaExcessoarquivosTmp(path);

            string nm_arquivo = Session.SessionID.Substring(0, 5)+"contato.xls";

            DataSet ds = new DataSet();
            ds.Tables.Add( dt);

            ExcelLibrary.DataSetHelper.CreateWorkbook(path + "\\" + nm_arquivo, ds);

            oConn.disconnect();

            Response.Redirect("~/tmp/" + nm_arquivo);

            return View();
        }


        #endregion


        #region Metodos para contato
        /// <summary>
        /// Obtém a consulta da tela de listagem (isso também pode ser colocado em outra classe ou em um model, ao depender do tamanho do projeto).
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        private static DataTable getContatoDataList(DataAccess.IDbPersist oConn)
        {
            string filtro = " where 1 = 1 ";
            string coluna = "";
            string coluna_filtro = "";

            if (Helper.RequestString("nome") != String.Empty)
            {
                string strfilt = Helper.RequestString("nome").Replace("'", "''");
                filtro += " and ( match (p.nome) against ('" + strfilt + "' IN BOOLEAN MODE) or upper(p.nome) like upper('%" + strfilt + "%') ) ";
                coluna += ", match (p.nome) against ('" + strfilt + "' ) as reelevance ";
                coluna_filtro = " reelevance desc, ";
            }
            if (Helper.RequestString("empresa") != String.Empty)
            {
                string strfilt = Helper.RequestString("empresa").Replace("'", "''");
                filtro += " and upper(p.empresa) like upper('%" + strfilt + "%') ";
            }
            if (Helper.RequestString("telefone") != String.Empty)
            {
                string strfilt = Helper.RequestString("telefone").Replace("'", "''");
                filtro += " and ( upper(p.telefone_comercial) like upper('%" + strfilt + "%') or upper(p.telefone_pessoal) like upper('%" + strfilt + "%') ) ";
            }
            if (Helper.RequestString("email") != String.Empty)
            {
                string strfilt = Helper.RequestString("email").Replace("'", "''");
                filtro += " and exists( select ce.id from contato_email ce where ce.id_contato = p.id and ce.email like upper('%" + strfilt + "%') ) ";
            }

            string sql = " select p.* " + coluna + " from contato p " + filtro + " order by " + coluna_filtro + " p.nome ";

            DataTable dtLista = ConnAccess.fetchData(oConn, sql);
            return dtLista;
        }

        /// <summary>
        /// Método para salvar os emails que vem da tela.
        /// </summary>
        /// <param name="id_contato"></param>
        /// <param name="oConn"></param>
        private void salvaEmails(int id_contato, IDbPersist oConn)
        {
            DataRow dr_email = ConnAccess.getNewRow(oConn, "contato_email");

            int qtde_emails = RendLibrary.Helper.RequestValue<int>("qtde_emails").Value;

            string str_ids = "0";
            int ordem = 0;
            string emails = "";

            for (int i = 0; i <= qtde_emails; i++)
            {
                string email = RendLibrary.Helper.RequestString("email"+i.ToString());

                if (email.Trim() != String.Empty)
                {
                    ordem++;

                    if (emails != String.Empty)
                        emails += ", ";

                    emails += email.Trim();
                    dr_email["email"] = email;
                    dr_email["id_contato"] = id_contato;
                    dr_email["ordem_cadastro"] = ordem;

                    string sql_item = " select id from contato_email where email='" + email + "' and id_contato=" + id_contato.ToString();
                    object id_tmp = ConnAccess.executeScalar(oConn, sql_item);

                    if (id_tmp != null && id_tmp != DBNull.Value)
                    {
                        dr_email["id"] = id_tmp;
                        ConnAccess.Update(oConn, dr_email, "id");
                    }
                    else
                    {

                        ConnAccess.Insert(oConn, dr_email, "id", true);
                        dr_email["id"] = ConnAccess.getMax(oConn, "id", dr_email.Table.TableName, " where id_contato = " + id_contato.ToString() );
                    }

                    str_ids += "," + dr_email["id"].ToString();
                }
            }
            ConnAccess.executeCommand(oConn, " delete from contato_email where id_contato = " + id_contato.ToString() + " and id not in ( " +
                          str_ids + " ) ");


            ConnAccess.executeCommand(oConn, " update contato set emails = '" + emails + "' where id  = " + id_contato.ToString());

        }

        #endregion
    }
}
