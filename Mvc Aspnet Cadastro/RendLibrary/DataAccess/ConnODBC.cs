using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data;

namespace DataAccess
{
    /// <summary>
    /// Conect ODBC
    /// Autor: Rafael Rend
    /// </summary>
    public class ConnODBC : IDbPersist
    {
        #region Private Properties
        private string key_conn = "_myconn_mysql";
        private string Key = "";

        #endregion



        #region IDbPersist Members

        /// <summary>
        /// Fetch data from a query, returning DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet fetchAll(string sql)
        {
            OdbcConnection conn = (OdbcConnection)getConn();

            OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);

            return ds;
        }

        /// <summary>
        /// Fetch data from a query, returning DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable fetchDados(string sql)
        {
            OdbcConnection conn = (OdbcConnection)getConn();

            OdbcDataAdapter da = new OdbcDataAdapter(sql, conn);
            DataTable ds = new DataTable();
            try
            {
                da.Fill(ds);

                return ds;
            }
            catch (Exception exp)
            {
                throw (new Exception("ERRO: " + exp.Message + System.Environment.NewLine + sql));

            }
        }

        public void executeCommand(string sql)
        {
            OdbcConnection conn = (OdbcConnection)getConn();

            OdbcCommand cmd = new OdbcCommand(sql, conn);
            cmd.CommandTimeout = 240;
            cmd.ExecuteNonQuery();
        }

        public object executeScalar(string sql)
        {
            OdbcConnection conn = (OdbcConnection)getConn();

            OdbcCommand cmd = new OdbcCommand(sql, conn);
            cmd.CommandTimeout = 240;
            object cod = cmd.ExecuteScalar();

            return cod;
        }

        /// <summary>
        /// Insert New Data
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="autoincrement"></param>
        /// <param name="pkID"></param>
        public void Insert(DataRow dr, bool autoincrement, string pkID)
        {
            OdbcConnection conn = (OdbcConnection)getConn();
            string tabb = dr.Table.TableName;

            string sql = " insert into " + tabb + " ( ";
            string where = "";
            string where2 = "";

            OdbcCommand cmd = new OdbcCommand();


            int conta = 0;
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                DataColumn coluna = dr.Table.Columns[i];

                if (pkID == coluna.ColumnName && autoincrement)
                {
                    continue;
                }


                if (conta > 0)
                {
                    sql += ", ";
                    where += ", ";
                    where2 += ", ";
                }
                conta++;

                sql += " " + coluna.ColumnName + " ";
            
                where += " ? ";
                where2 += "'" + dr[coluna].ToString() + "'";


                OdbcParameter par = new OdbcParameter(coluna.ColumnName, getTipo(coluna.DataType));

                if (dr[coluna] == null)
                    par.Value = DBNull.Value;
                else
                    par.Value = dr[coluna];

                cmd.Parameters.Add(par);

            }

            string sqlcompleto = sql + ") VALUES ( " + where2 + ") ";
            sql += ") VALUES ( " + where + ") ";


            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.Connection = conn;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                string log = String.Empty;
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    DataColumn coluna = dr.Table.Columns[i];
                    log += System.Environment.NewLine + " " + coluna.ColumnName + " = " + dr[coluna].ToString() + " ( " + dr[coluna].ToString().Length.ToString() + ") ";

                }

                throw new Exception("Erro na querie: " + System.Environment.NewLine + sqlcompleto + System.Environment.NewLine + System.Environment.NewLine +
                    System.Environment.NewLine +
                    log + System.Environment.NewLine + System.Environment.NewLine +
                    exp.Message);
            }
        }

        /// <summary>
        /// Update Data
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <param name="pkID">Primary Key ID</param>
        public void Update(DataRow dr, string pkID)
        {
            OdbcConnection conn = (OdbcConnection)getConn();

            string tab_name = dr.Table.TableName;

            string sql = " update " + tab_name + " set ";
            string where = "";

            OdbcCommand cmd = new OdbcCommand();

            int conta = 0;
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                DataColumn coluna = dr.Table.Columns[i];


                if (pkID == coluna.ColumnName)
                {
                    continue;
                }

                if (conta > 0)
                    sql += ", ";

                conta++;
                string col_nome = coluna.ColumnName;

                sql += col_nome + " = ? ";

                OdbcParameter par = new OdbcParameter(coluna.ColumnName, getTipo(coluna.DataType));

                if (dr[coluna] == null)
                    par.Value = DBNull.Value;
                else
                    par.Value = dr[coluna];

                cmd.Parameters.Add(par);

            }

            DataColumn[] pks = dr.Table.PrimaryKey;

            if (pks.Length <= 0)
            {
                pks = new DataColumn[] { dr.Table.Columns[pkID] };
            }

            for (int i = 0; i < pks.Length; i++)
            {
                DataColumn coluna = pks[i];

                if (where == "")
                    where += " where " + coluna.ColumnName + " = ? ";
                else
                    where += " and " + coluna.ColumnName + " = ? ";

                OdbcParameter par = new OdbcParameter(coluna.ColumnName, getTipo(coluna.DataType));

                if (dr[coluna] == null)
                    par.Value = DBNull.Value;
                else
                    par.Value = dr[coluna];

                cmd.Parameters.Add(par);

            }
            sql += where;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.Connection = conn;

            cmd.ExecuteNonQuery();

        }

        /// <summary>
        /// Disconnet 
        /// </summary>
        public void disconnect()
        {

            OdbcConnection conn = (OdbcConnection)this.getConn();
            conn.Close();
            conn.Dispose();

            conn = null;
            myconn = null;



        }

        public void connect(string connectionString, string Key)
        {
            OdbcConnection conn = (OdbcConnection)this.getConn();
            myconn.Open();

            this.setConn(Key, myconn);
            this.Key = Key;
            System.Web.HttpContext.Current.Items["_myconn_odbc" + Key + "connectionstring"] = connectionString;
        }

        public object getConn()
        {
            if (myconn == null)
            {
                string connectionString = System.Web.HttpContext.Current.Items[this.key_conn + "connectionstring"].ToString();

                this.connect(connectionString, this.Key);
            }

            return myconn;
        }

        public void setConn(string key, object conn)
        {
            this.key_conn = "_myconn_odbc" + key;
            System.Web.HttpContext.Current.Items[this.key_conn] = conn;
        }

        #endregion

        #region Private methods

        private OdbcConnection myconn
        {
            get
            {
                System.Web.HttpContext Gcontext = System.Web.HttpContext.Current;
                if (Gcontext.Items[this.key_conn] == null)
                {
                    return null;
                }
                else
                {
                    return (OdbcConnection)Gcontext.Items[this.key_conn];
                }


            }
            set
            {
                System.Web.HttpContext Gcontext = System.Web.HttpContext.Current;
                Gcontext.Items[this.key_conn] = value;
            }
        }
        private OdbcType  getTipo(Type tipo)
        {

            if (tipo == typeof(Int16) || tipo == typeof(Int32))
                return OdbcType.Int;

            if (tipo == typeof(Int64))
                return OdbcType.BigInt;

            if (tipo == typeof(DateTime))
                return OdbcType.DateTime;

            if (tipo == typeof(TimeSpan))
                return OdbcType.Timestamp;

            if (tipo == typeof(Boolean))
                return OdbcType.Bit;

            if (tipo == typeof(Char))
                return OdbcType.Char;

            if (tipo == typeof(String))
                return OdbcType.VarChar;

            if (tipo == typeof(Byte))
                return OdbcType.TinyInt;


            if (tipo == typeof(Decimal))
                return OdbcType.Double;


            if (tipo == typeof(Double))
                return OdbcType.Double;

            return OdbcType.VarChar;

        }


        #endregion
    }
}
