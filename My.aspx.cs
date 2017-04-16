﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class My : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Session["userName"] == null) || (Session["userName"].ToString() == ""))
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            string strName = Session["userName"].ToString();
            string connString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SqlConnStr"].ConnectionString;
            SqlConnection conn = new SqlConnection(connString);
            string strSelect = "SELECT * FROM Users WHERE userName=@username";
            SqlCommand command = new SqlCommand(strSelect, conn);
            command.Parameters.AddWithValue("@username", strName);
            conn.Open();
            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {
                if (dr["isAdmin"].ToString() == "False")
                {
                    DataBinding();
                }
                else
                {
                    Response.Redirect("Admin.aspx");
                }
            }
        }
    }

    private new void DataBinding()
    {
        string connString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["SqlConnStr"].ConnectionString;
        SqlConnection conn = new SqlConnection(connString);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter sda = new SqlDataAdapter();
        DataSet ds = new DataSet();
        conn.Open();
        cmd.Connection = conn;
        cmd.CommandText = "select * from Orders where userID=@userID order by orderID desc";
        cmd.Parameters.AddWithValue("@userID", Session["userID"].ToString());
        sda.SelectCommand = cmd;

        sda.Fill(ds);
        GridView1.DataSource = ds.Tables[0];
        GridView1.DataBind();
        conn.Close();
    }

    protected void ButtonQuit_Click(object sender, EventArgs e)
    {
        Session["userName"] = "";
        Response.Redirect("Default.aspx");
    }
}
