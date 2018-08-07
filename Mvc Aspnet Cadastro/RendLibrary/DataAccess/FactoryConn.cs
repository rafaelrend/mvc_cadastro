using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DataAccess
{
   
            public class FactoryConn{
    
                public static IDbPersist getConn( string tipo, string connectionString, string var_key = "connection_system" ){
    
                          IDbPersist saida = null;
              
                     /*
                          if ( $tipo == self::DB_POSTGRESQL)
                              $saida = new PostGreSQLConnection ($arr_conection, $var_name);
              
              
                          if ( $tipo == self::DB_SQLSERVER)
                              $saida = new MSSQLConnection($arr_conection, $var_name);
                      * 
                      * */
              
                          if ( tipo ==  DB_MYSQL){
                              saida = new ConnMySql();    // MYSQLConnection($arr_conection, $var_name);
                              saida.connect(connectionString, var_key);
                          }
              
                          if ( tipo == DB_ODBC){
                              saida = new ConnODBC();
                              saida.connect(connectionString, var_key);
                          }
                           //   $saida = new ODBCConnection($arr_conection, $var_name);
              
        
                          return saida;
                }
    
    
    
                const string DB_POSTGRESQL = "postgres";
                const string DB_MYSQL = "mysql";
                const string DB_SQLSERVER = "sql server";
                const string DB_ODBC = "odbc";
            }
}
