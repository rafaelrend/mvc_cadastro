using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class BaseModel
    {
        private System.Data.DataTable dtList;

        public System.Data.DataTable DtList
        {
            get { return dtList; }
            set { dtList = value; }
        }
        private System.Data.DataRow registro;

        public System.Data.DataRow Registro
        {
            get { return registro; }
            set { registro = value; }
        }
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }


    }
}