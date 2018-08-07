using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess
{
    /// <summary>
    /// Class of Database static methods.
    /// Autor: Rafael Rend
    /// </summary>
    public class ConnAccess
    {

        /// <summary>
        /// Fast querie. Get a sql querie informing table + where
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
       public static DataTable fastQuerie(IDbPersist oConn,  string table, string where = "", string order = ""){
	           
			   string sql = " select * from " + table;

			   if ( where != "" )
			      sql += " where " + where;				   
				   
			   if ( order != "" )
			       sql += " order by " + order;
				   

	            return  oConn.fetchDados( sql );
			     
	   }

        /// <summary>
        /// Get One Row from DataBase
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="table"></param>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static DataRow fastOne(IDbPersist oConn,  string table, string where = "", string order = ""){
	   
	         DataTable ar =  ConnAccess.fastQuerie( oConn,  table, where, order );
                 
                // connAccess.fastQuerie( $oConn,  $table, $where, $order );
		
		
			 if ( ar.Rows.Count > 0 ){
			 
			       DataRow ret = ar.Rows[0];
			       return ret;
			 } 
			 
			 return null;
	   }

        /// <summary>
        /// Perfoms a querie
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable fetchData(IDbPersist oConn, string sql)
        {

            return oConn.fetchDados(sql);
        }

        /// <summary>
        /// Executing a scalar function
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object executeScalar(IDbPersist oConn, string sql)
        {
            return oConn.executeScalar(sql);
        }

        /// <summary>
        /// Execute a command
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="sql"></param>
        public static void executeCommand(IDbPersist oConn, string sql)
        {
            oConn.executeCommand (sql);
        }

        /// <summary>
        /// Insert new record
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="dr"></param>
        /// <param name="pk"></param>
        /// <param name="auto_increment"></param>
        public static void Insert(IDbPersist oConn, DataRow dr, string pk, bool auto_increment = true)
        {
            oConn.Insert(dr, auto_increment, pk);
        }

        /// <summary>
        /// Update a record
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="dr"></param>
        /// <param name="pk"></param>
        public static void Update(IDbPersist oConn, DataRow dr, string pk)
        {
            oConn.Update(dr,  pk);
        }


        /// <summary>
        /// Copia os dados de um dataRow para outro.
        /// </summary>
        /// <param name="dr_fonte"></param>
        /// <param name="dr_destino"></param>
        public static void copiaDataRows(DataRow dr_fonte, DataRow dr_destino)
        {
            for (int i = 0; i < dr_fonte.Table.Columns.Count; i++)
            {
                if (dr_destino.Table.Columns.Contains(dr_fonte.Table.Columns[i].ColumnName)
                    &&
                    dr_destino.Table.Columns[dr_fonte.Table.Columns[i].ColumnName].DataType ==
                    dr_fonte.Table.Columns[dr_fonte.Table.Columns[i].ColumnName].DataType
                    )
                {

                    dr_destino[dr_fonte.Table.Columns[i].ColumnName] = dr_fonte[dr_fonte.Table.Columns[i].ColumnName];
                }
            }

        }


        /// <summary>
        /// Converte DbNULL em null
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static object DBNullToNull(object valor)
        {
            if (valor == DBNull.Value)
                return null;

            return valor;
        }

        /// <summary>
        /// Converte NULL em DBNULL
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static object NullToDBNull(object valor)
        {
            if (valor == null)
                return DBNull.Value;

            return valor;
        }


        /// <summary>
        /// Cria um DataRow de dados vazio
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tabela"></param>
        /// <returns></returns>
        public static DataRow getNewRow(IDbPersist conn, string tabela)
        {
            string sql = " select * from " + tabela + " where 1 = 0 ";

            DataTable ds = new DataTable();
            try
            {

                ds = ConnAccess.fetchData(conn, sql);
                DataRow drr = ds.NewRow();

                drr.Table.TableName = tabela;
                return drr;

            }
            catch (Exception exp)
            {
                throw exp;
            }

            return null;
        }



        public static DataRow getRow(IDbPersist conn, string tabela, string pk, string id)
        {
            string sql = " select * from " + tabela + " where " + pk + "  = " + id;


            DataTable ds = new DataTable();
            ds = ConnAccess.fetchData(conn, sql);

            ds.TableName = tabela;

            if (ds.Rows.Count > 0)
                return ds.Rows[0];

            return null;

        }

        /// <summary>
        /// Obtém uma linha DataRow.
        /// </summary>
        /// <param name="conn">Conexão Usada</param>
        /// <param name="tabela"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static DataRow getRow(IDbPersist conn, string tabela, string comp)
        {
            string sql = " select * from " + tabela + " where  1 = 1 " + comp;

            try
            {
                DataTable ds = new DataTable();
                ds = ConnAccess.fetchData(conn, sql);

                ds.TableName = tabela;

                if (ds.Rows.Count > 0)
                    return ds.Rows[0];
            }
            catch (Exception exp)
            {
                throw new Exception(sql + " - " + exp.Message);

            }
            return null;
        }


        public static int getMax(IDbPersist oConn,  string coluna, string tabela, string where)
        {
            string sql = " select max(" + coluna + ") from " + tabela + where;


            object cod = oConn.executeScalar(sql);

            if (cod != null && cod.ToString() != String.Empty)
                return Convert.ToInt32(cod.ToString());

            return 0;


        }


        #region Transaction
        public static void BeginTransaction()
        {

        }


        #endregion

    }
}
