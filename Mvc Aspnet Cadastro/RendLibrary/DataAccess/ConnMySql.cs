using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web;

namespace DataAccess
{
    /// <summary>
    /// Conect MYSQL 
    /// Autor: Rafael Rend
    /// </summary>
    public class ConnMySql : IDbPersist
    {

        #region Private Properties
        private string key_conn = "_myconn_mysql";
        private string Key = "";
        private string parameterconf = "@";

        #endregion

        #region IDbPersist Members

        /// <summary>
        /// Fetch data from a query, returning DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet fetchAll(string sql)
        {
            MySqlConnection conn = (MySqlConnection)getConn();
            confereStatus(conn);

            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
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
            MySqlConnection conn = (MySqlConnection)getConn();

            confereStatus(conn);

            MySqlDataAdapter da = new MySqlDataAdapter(sql, conn);
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
            MySqlConnection conn = (MySqlConnection)getConn();

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.CommandTimeout = 240;
            cmd.ExecuteNonQuery();
        }

        public object executeScalar(string sql)
        {
            MySqlConnection conn = (MySqlConnection)getConn();

            
            MySqlCommand cmd = new MySqlCommand(sql, conn);
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
            MySqlConnection conn = (MySqlConnection)getConn();
            string tabb = dr.Table.TableName;

            string sql = " insert into " + tabb + " ( ";
            string where = "";
            string where2 = "";

            MySqlCommand cmd = new MySqlCommand();


            int conta = 0;
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                DataColumn coluna = dr.Table.Columns[i];

                if (coluna.DataType == typeof(DateTime))
                {
                    dr[coluna] = datePrecision(dr[coluna], coluna.DataType);
                }


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

                where += "  " + parameterconf + coluna.ColumnName;
                where2 += "'" + dr[coluna].ToString() + "'";
                
                MySqlParameter par = new MySqlParameter(coluna.ColumnName, getTipo(coluna.DataType));

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

                throw new Exception("PAU na querie: " + System.Environment.NewLine + sqlcompleto + System.Environment.NewLine + System.Environment.NewLine +
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
            MySqlConnection conn = (MySqlConnection)getConn();

            string tab_name = dr.Table.TableName;

            string sql = " update " + tab_name + " set ";
            string where = "";

            MySqlCommand cmd = new MySqlCommand();

            int conta = 0;
            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                DataColumn coluna = dr.Table.Columns[i];

                if (coluna.DataType == typeof(DateTime))
                {
                    dr[coluna] = datePrecision(dr[coluna], coluna.DataType);
                }

                if (pkID == coluna.ColumnName)
                {
                    continue;
                }

                if (conta > 0)
                    sql += ", ";

                conta++;
                string col_nome = coluna.ColumnName;

                sql += col_nome + " =  " + parameterconf + coluna.ColumnName;

                MySqlParameter par = new MySqlParameter(coluna.ColumnName, getTipo(coluna.DataType));

                if (dr[coluna] == null)
                    par.Value = DBNull.Value;
                else
                    par.Value = dr[coluna];

                cmd.Parameters.Add(par);

            }

            DataColumn[] pks = dr.Table.PrimaryKey;

            if (pks.Length <= 0)
            {
                pks = new DataColumn[]{ dr.Table.Columns[ pkID ] };
            }

            for (int i = 0; i < pks.Length; i++)
            {
                DataColumn coluna = pks[i];

                if (where == "")
                    where += " where " + coluna.ColumnName + " =  " + parameterconf + coluna.ColumnName;
                else
                    where += " and " + coluna.ColumnName + " =  " + parameterconf + coluna.ColumnName;

                MySqlParameter par = new MySqlParameter(coluna.ColumnName, getTipo(coluna.DataType));

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

            MySqlConnection conn = (MySqlConnection)this.getConn();
            conn.Close();
            conn.Dispose();

            conn = null;
            myconn = null;

            this.KillSleepingConnections(100);


        }

        public void connect(string connectionString, string Key)
        {
            MySqlConnection myconn = new MySqlConnection(connectionString);
            myconn.Open();

            this.setConn(Key, myconn);
            this.Key = Key;
            System.Web.HttpContext.Current.Items["_myconn_mysql" + Key + "connectionstring"] = connectionString;
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
            this.key_conn = "_myconn_mysql" + key;
            System.Web.HttpContext.Current.Items[this.key_conn] = conn;
        }

        #endregion

        #region Private methods

        private MySqlConnection myconn
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
                    return (MySqlConnection)Gcontext.Items[this.key_conn];
                }


            }
            set
            {
                System.Web.HttpContext Gcontext = System.Web.HttpContext.Current;
                Gcontext.Items[this.key_conn] = value;
            }
        }

        /// <summary>
        /// Alguns servidores não derrubam a conexão do Mysql após o close e Dispose. Para isso serve essa função.
        /// </summary>
        /// <param name="iMinSecondsToExpire"></param>
        /// <returns></returns>
        public int KillSleepingConnections(int iMinSecondsToExpire)
        {
            string strSQL = "show processlist";
            System.Collections.ArrayList m_ProcessesToKill = new System.Collections.ArrayList();

            string connectionString = System.Web.HttpContext.Current.Items[this.key_conn + "connectionstring"].ToString();

            MySqlConnection myConn = new MySqlConnection(connectionString);
            MySqlCommand myCmd = new MySqlCommand(strSQL, myConn);
            MySqlDataReader MyReader = null;

            try
            {
                myConn.Open();

                // Get a list of processes to kill.
                MyReader = myCmd.ExecuteReader();
                while (MyReader.Read())
                {
                    // Find all processes sleeping with a timeout value higher than our threshold.
                    int iPID = Convert.ToInt32(MyReader["Id"].ToString());
                    string strState = MyReader["Command"].ToString();
                    int iTime = Convert.ToInt32(MyReader["Time"].ToString());

                    if (strState == "Sleep" && iTime >= iMinSecondsToExpire && iPID > 0)
                    {
                        // This connection is sitting around doing nothing. Kill it.
                        m_ProcessesToKill.Add(iPID);
                    }
                }

                MyReader.Close();

                foreach (int aPID in m_ProcessesToKill)
                {
                    strSQL = "kill " + aPID;
                    myCmd.CommandText = strSQL;
                    myCmd.ExecuteNonQuery();
                }
            }
            catch (Exception excep)
            {
            }
            finally
            {
                if (MyReader != null && !MyReader.IsClosed)
                {
                    MyReader.Close();
                    MyReader.Dispose();
                }

                if (myConn != null && myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                    myConn.Dispose();
                }
            }

            return m_ProcessesToKill.Count;
        }


        private void confereStatus(MySqlConnection conn)
        {
            try
            {
                if (conn.State == ConnectionState.Broken ||
                     conn.State == ConnectionState.Closed)
                {

                    //conn.

                    try
                    {
                        conn.Close();
                    }
                    catch { }

                    conn.Open();

                }
            }
            catch { }


        }

        private MySql.Data.MySqlClient.MySqlDbType getTipo(Type tipo)
        {

            if (tipo == typeof(Int16) || tipo == typeof(Int32))
                return MySqlDbType.Int32;

            if (tipo == typeof(Int64))
                return MySqlDbType.Int64;

            if (tipo == typeof(DateTime))
                return MySqlDbType.DateTime;

            if (tipo == typeof(TimeSpan))
                return MySqlDbType.Timestamp;

            if (tipo == typeof(Boolean))
                return MySqlDbType.Bit;

            if (tipo == typeof(Char))
                return MySqlDbType.VarString;

            if (tipo == typeof(String))
                return MySqlDbType.VarChar;

            if (tipo == typeof(Byte))
                return MySqlDbType.Int16;


            if (tipo == typeof(Decimal))
                return MySqlDbType.Decimal;


            if (tipo == typeof(Double))
                return MySqlDbType.Float;

            return MySqlDbType.VarChar;

        }


        private object datePrecision(object valor, Type tipo)
        {
            if (tipo is DateTime && valor != null && valor != DBNull.Value)
            {
                DateTime dtNovo = Convert.ToDateTime(valor);

                return new DateTime(dtNovo.Year, dtNovo.Month, dtNovo.Day, dtNovo.Hour, dtNovo.Minute, dtNovo.Second);

            }

            return valor;
        }

        #endregion
    }
}
