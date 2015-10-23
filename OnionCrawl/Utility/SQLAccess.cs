using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionCrawl.Utility
{
    public class SQLAccess
    {
        //public static string connString = @"Data Source=NORMANDY\SR71;Initial Catalog=torcrawl;User ID=caleb;Password=caleb;Integrated Security=False;";
        public static string connString = @"Data Source=localhost;Initial Catalog=torcrawl;User ID=caleb;Password=caleb;Integrated Security=False;";
        public string Procedure { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
        public bool Success { get; set; }
        public Exception Ex { get; set; }
        public DataTable Response { get; set; }
        public bool HasData { get { return Response.Rows.Count > 0; } }


        public SQLAccess()
        {
            Parameters = new Dictionary<string, object>();
            Procedure = string.Empty;
        }

        public class ErrorLog
        {
            public string PublicMessage { get; set; }
            internal string ErrorHeader { get; set; }
            internal string PrivateMessage { get; set; }
            internal DateTime TimeStamp { get; set; }
            internal Guid Guid { get; set; }
            public Guid ItemGuid { get; set; }
        }


        internal void ExecuteProcedure()
        {
            Response = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(Procedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        foreach (var param in Parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            da.Fill(Response);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Ex = ex;
            }
            this.Clear();
        }

        internal void ExecuteQuery(string query)
        {
            Response = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            da.Fill(Response);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Ex = ex;
            }
            this.Clear();
        }



        internal void ExecuteQuery(string query, string connectionString)
        {
            Response = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.CommandTimeout = 6000;
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            da.Fill(Response);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Ex = ex;
            }
        }

        internal void ExecuteBulkCopy(DataTable table, string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlBulkCopy blk = new SqlBulkCopy(conn))
                    {
                        blk.DestinationTableName = tableName;
                        foreach (DataColumn col in table.Columns)
                            blk.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        blk.WriteToServer(table);

                        blk.Close();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                this.Ex = ex;
            }
        }



        internal void Clear()
        {
            Procedure = string.Empty;
            if (Parameters == null)
                Parameters = new Dictionary<string, object>();
            else
                Parameters.Clear();
        }
    }
}
