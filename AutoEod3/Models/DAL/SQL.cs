using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoEod3.Models.DAL
{
    /// <summary>
    /// Exec SQL script
    /// </summary>
    class SQL: IDisposable
    {
        private SqlConnection conn;
        private string index;
        private List<Header> columnsHeader = new List<Header>();
        private List<List<string>> data = new List<List<string>>();
        
        public static bool cancel = false;
        private SqlCommand sqlCommand;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="index"></param>
        public SQL(string connectionString, string index)
        {
            this.conn = new SqlConnection(connectionString);
            this.index = index;
        }

        /// <summary>
        /// Get list headers
        /// </summary>
        public List<Header> ColumnsHeader
        {
            get
            {
                return this.columnsHeader;
            }
            set
            {
                this.columnsHeader = value;
            }
        }

        /// <summary>
        /// Get data
        /// </summary>
        public List<List<string>> Data
        {
            get
            {
                return this.data;
            }
        }

        /// <summary>
        /// Finalize
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.conn.State != System.Data.ConnectionState.Closed)
                {
                    conn.Close();                    
                }
                conn.Dispose();
            }
            catch { }
        }

        /// <summary>
        /// RunQuery
        /// </summary>
        /// <param name="queryText"></param>
        public void RunQuery(string queryText)
        {
            sqlCommand = new SqlCommand(queryText, this.conn);
            sqlCommand.CommandTimeout = 30 * 24 * 60 * 60; 
            this.conn.Open();
            
            Task.Factory.StartNew(() =>
            {
                while (conn.State != System.Data.ConnectionState.Broken || conn.State != System.Data.ConnectionState.Closed)
                {                    
                    if (cancel)
                    {                       
                        sqlCommand.Cancel();
                        break;
                    }
                    Thread.Sleep(100);
                }
            });

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            
            // Add headers
            if (this.columnsHeader == null || this.columnsHeader.Count == 0)
            {
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    this.columnsHeader.Add(
                        new Header(sqlDataReader.GetName(i), sqlDataReader.GetFieldType(i) == typeof(string) ? true : false));
                }
            }

            // Add data from a table                        
            
            while (sqlDataReader.Read())
            {
                List<string> row = new List<string>();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    row.Add(sqlDataReader[i].ToString());
                }
                this.data.Add(row);
            }                 
        }              

           

    }
}
