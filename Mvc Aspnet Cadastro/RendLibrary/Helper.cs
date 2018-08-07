using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace RendLibrary
{
    /// <summary>
    /// Funções úteis em qualquer projeto.
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Request data from form. Firts get from FORM collection, after request from other collections.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static string RequestString(string prop)
        {
            string req =  System.Web.HttpContext.Current.Request.Form[prop];

            if (req == "" || req == null )
            {
                req = System.Web.HttpContext.Current.Request[prop];
            }


            if (req == null)
                req = "";


            return req;

        }



        /// <summary>
        /// Request value (generic).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static Nullable<T> RequestValue<T>(string prop) where T : struct, IConvertible
        {
            T result = default(T);
            string value = RequestString(prop);

            if (!String.IsNullOrEmpty(value))
            {
                try
                {
                    result = (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    //Could not convert.  Pass back default value...
                    result = default(T);
                }

            }
           
            return result;
        }


        /// <summary>
        /// Load data from form
        /// </summary>
        /// <param name="dr"></param>
        public static void loadDataRowFromForm(System.Data.DataRow dr)
        {
            foreach (DataColumn dc in dr.Table.Columns)
            {
                string value = RequestString(dc.ColumnName);

                if ( ! HttpContext.Current.Request.Form.AllKeys.Contains(dc.ColumnName) &&
                    !HttpContext.Current.Request.QueryString.AllKeys.Contains(dc.ColumnName))
                           continue;

                if (dc.DataType != typeof(string))
                {
                    if (value != String.Empty)
                    {
                        try
                        {
                            dr[dc.ColumnName] = Convert.ChangeType(value, dc.DataType);
                        }
                        catch { }
                    }
                    else
                    {

                        dr[dc.ColumnName] = DBNull.Value;
                    }
                }
                else
                {
                    dr[dc.ColumnName] = value;
                }
            }

        }

        public static string limita(string str, int limit)
        {
            if (str.Length > limit)
                return str.Substring(0, limit) + "...";
            else
                return str;

        }


        public static void showStr(System.Data.DataRow dr, string column, string format = "")
        {
            string str = getStrDataRow(dr, column, format);
            HttpContext.Current.Response.Write(str);
        }

        public static void showRequest(string tp)
        {
            string str = RequestString(tp);
            HttpContext.Current.Response.Write(str);
        }

        public static string getStrDataRow(System.Data.DataRow dr, string column, string format = "" ){

            if (dr == null)
                return "";

            if (dr[column] == DBNull.Value)
                return "";

            object retorno = dr[column];

            if (format != "" && !String.IsNullOrEmpty(retorno.ToString() ) )
            {
                if (dr.Table.Columns[column].DataType == typeof(DateTime))
                {
                    return Convert.ToDateTime(retorno).ToString(format);
                }
                if (dr.Table.Columns[column].DataType == typeof(Decimal) )
                {
                    return Convert.ToDecimal(retorno).ToString(format);
                }
                if (dr.Table.Columns[column].DataType == typeof(Double) || dr.Table.Columns[column].DataType == typeof(Single))
                {
                    return Convert.ToDouble(retorno).ToString(format);
                }
            }

            return retorno.ToString();

        }

        public static bool isInteger(string id)
        {
            try
            {
                int id_i = Convert.ToInt32(id);

                return true;
            }
            catch
            {
                return false;
            }

        }


        public static string getInputsHidden(System.Collections.ArrayList arrNames, System.Collections.ArrayList arrValues){

            string campos = "";

            for (int i = 0; i < arrNames.Count; i++)
            {
                campos += System.Environment.NewLine +
                    "<input type=\"hidden\" name=\"" + arrNames[i].ToString() + "\" value=\"" + arrValues[i].ToString() + "\" />";
            }

            return campos;

        }

        /// <summary>
        /// Remove excesso de arquivos temporários de uma pasta..
        /// </summary>
        /// <param name="path"></param>
        public static void limpaExcessoarquivosTmp(string path)
        {

            string[] files = System.IO.Directory.GetFiles(path);

            for (int i = 0; i < files.Length; i++)
            {

                try
                {
                    string arquivo = path + "\\" + files[i];

                    System.IO.FileInfo fil = new System.IO.FileInfo(arquivo);

                    if (fil.CreationTime < (DateTime.Now.AddDays(-1)))
                    {
                        System.IO.File.Delete(fil.FullName);

                    }

                }
                catch { }
            }

        }

    }
}