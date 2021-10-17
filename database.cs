using System.Data;
using System.Data.SqlClient;
using System;
using System.Configuration;

public class Database
{
    public static SqlConnection getConnection()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();

        SqlConnection con = new SqlConnection(connectionString);
        con.Open();

        return con;
    }

    public static DataTable executeselect(string query)
    {
        SqlConnection con = getConnection();

        SqlCommand command = new SqlCommand(query, con);

        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = command;
        DataSet ds = new DataSet();
        adapter.Fill(ds);

        con.Close();

        return ds.Tables[0];
    }

    public static void executeQuery(string query)
    {
        SqlConnection con = getConnection();
        SqlCommand cmd = new SqlCommand(query, con);

        cmd.ExecuteNonQuery();

        con.Close();
    }

    public static DataSet getdataset(string query)
    {
        SqlConnection conn = getConnection();

        SqlCommand command = new SqlCommand(query, conn);

        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = command;
        DataSet ds = new DataSet();
        adapter.Fill(ds);

        conn.Close();

        return ds;
    }

    public static SqlDataReader getDataReader(string query)
    {
        SqlConnection conn = getConnection();

        SqlCommand command = new SqlCommand(query, conn);

        SqlDataReader dr = command.ExecuteReader();

        return dr;
    }
}