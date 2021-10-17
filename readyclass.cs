using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;

using System.Net;

public class readyclass
{

    public Int32 autoid(string tablename, string fieldname)
    {
        SqlConnection con = Database.getConnection();
        SqlCommand cmd = new SqlCommand("select " + fieldname + " from " + tablename + " order by 1 desc", con);
        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        int id;

        if (dr.Read() == true)
        {
            id = Convert.ToInt32(dr[0].ToString());
            id += 1;
        }
        else
        {
            id = 1;
        }
        return id;
    }

    public Int32 getid(string tablename, string fieldname)
    {
        SqlConnection con = Database.getConnection();
        SqlCommand cmd = new SqlCommand("select top 1 " + fieldname + " from " + tablename + " Order by 1 Desc", con);
        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        int id;

        if (dr.Read())
        {
            id = Convert.ToInt32(dr[0].ToString().Substring(Math.Max(0, dr[0].ToString().Length - 4)));
            id += 1;
        }
        else
        {
            id = 1;
        }
        return id;
    }

    public bool BindGrid(GridView grd, string sql)
    {

        DataTable dt;
        grd.DataSource = null;

        dt = Database.executeselect(sql);

        if (dt.Rows.Count > 0)
        {
            grd.DataSource = dt;
            grd.DataBind();
            return true;
        }
        else
        {
            grd.DataBind();
            return false;
        }
    }

    public bool checkduplicate(string str)
    {

        SqlConnection con = Database.getConnection();
        SqlCommand cmd = new SqlCommand(str, con);
        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (dr.Read())
        {
            dr.Close();
            cmd.Dispose();
            con.Close();
            return true;
        }
        else
        {
            dr.Close();
            cmd.Dispose();
            con.Close();
            return false;
        }
    }

    public static void errormessage(Label l, string str)
    {
        l.Text = str;
        l.Visible = true;
    }

    public void filllist(DropDownList d, string str)
    {
        d.Items.Clear();
        d.Items.Insert(0, "Select");
        SqlConnection con = Database.getConnection();
        SqlCommand cmd = new SqlCommand(str, con);

        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        int i = 1;
        while (dr.Read())
        {
            d.Items.Insert(i, dr[0].ToString().Trim());
            i++;
        }
        dr.Close();
        cmd.Dispose();
        con.Close();
    }


    public void makereadonly(ControlCollection cntrl)
    {
        foreach (Control item in cntrl)
        {
            if (item is TextBox)
                ((TextBox)item).ReadOnly = true;
            else if (item is DropDownList)
                ((DropDownList)item).Enabled = false;
            makereadonly(item.Controls);
        }
    }

    public void makeeditable(ControlCollection cntrl)
    {
        foreach (Control item in cntrl)
        {
            if (item is TextBox)
                ((TextBox)item).ReadOnly = false;
            else if (item is DropDownList)
                ((DropDownList)item).Enabled = true;
            makeeditable(item.Controls);
        }
    }

    public void cleartext(ControlCollection cntrl)
    {
        foreach (Control item in cntrl)
        {
            if (item is TextBox)
                ((TextBox)item).Text = "";
            else if (item is DropDownList)
                ((DropDownList)item).ClearSelection();
            cleartext(item.Controls);
        }
    }


    public void fill(GridView g, string sql, Label l)
    {
        bool flag = BindGrid(g, sql);
        if (flag == false)
        {
            errormessage(l, "Records not found");
        }
        else
        {
            l.Text = "";
        }
    }

    public void fillwithouterrormsg(GridView g, string sql)
    {
        bool flag = BindGrid(g, sql);
    }

    public string generateOTP()
    {
        Random rand = new Random((int)DateTime.Now.Ticks);
        int numIterations = 0;
        numIterations = rand.Next(1, 10000);
        string str = "LK" + numIterations;

        return str;
    }

    public void Show(string message, string url)
    {
        var page = HttpContext.Current.CurrentHandler as Page;

        string script = "alert('";
        script += message;
        script += "');";
        script += "window.location = '";
        script += url;
        script += "'; ";

        ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), script, true);
    }


    public void sendSMS(string msg, string mobile)
    {
        try
        {
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("http://s.exonics.co.in/api/sendmsg.php?user=chetanpuc&pass=chetan12345678&sender=CHETAN&phone= " + mobile + "&text=" + msg + "&priority=ndnd&stype=normal");
            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
            string responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            myResp.Close();
        }
        catch { }
    }
}