using System;
using System.Data.SqlClient;

namespace MiniStore.Data
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString =
            @"Server=MSI\SQLEXPRESS1;Database=MiniStore;User Id=sa;Password=Password123!;";

        // Thực thi câu lệnh INSERT/UPDATE/DELETE
        public static void ExecuteNonQuery(string query, Action<SqlCommand> parameterize)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    parameterize?.Invoke(cmd);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Thực thi câu lệnh trả về 1 giá trị
        public static object ExecuteScalar(string query, Action<SqlCommand> parameterize)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    parameterize?.Invoke(cmd);
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
