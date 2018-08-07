using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Interface para acesso a dados.
    /// Objetivo: Não escrever SQL e ser mais rápido que um OORM. Qualquer nova implementação para outra tecnologia de banco de dados deve extender essa classe.
    /// </summary>
    public interface IDbPersist
    {

        DataSet fetchAll(string sql);
        DataTable fetchDados(string sql);



        void executeCommand(string sql);
        object executeScalar(string sql);

        void Insert(DataRow dr, bool autoincrement, string pkID);
        void Update(DataRow dr, string pkID);

        void disconnect();
        void connect(string connectionString, string Key);

        object getConn();
        void setConn(string key, object conn);


    }
}
