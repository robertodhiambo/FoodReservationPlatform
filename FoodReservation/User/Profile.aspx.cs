﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace FoodReservation.User
{
    public partial class Profile : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        protected void Page_Load ( object sender , EventArgs e )
        {
            if (!IsPostBack)
            { 
                if ( Session ["userId"] == null )
                {
                    Response.Redirect ( "Login.aspx" );
                }
                else
                {
                    getUserDetails ( );
                    getPurchaseHistory( );
                }
            }
        }

        void getUserDetails ( )
        {
            con = new SqlConnection ( Connection.GetConnectionString ( ) );
            cmd = new SqlCommand ( "User_Crud" , con );
            cmd.Parameters.AddWithValue ( "@Action" , "SELECT4PROFILE" );
            cmd.Parameters.AddWithValue ( "@UserId" , Session ["userId"] );
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter ( cmd );
            dt = new DataTable ( );
            sda.Fill ( dt );
            rUserProfile.DataSource = dt;
            rUserProfile.DataBind ();

            if ( dt.Rows.Count == 1 )
            {
                Session [ "name"] = dt.Rows [ 0 ] [ "Name" ].ToString ( );
                Session [ "email" ] = dt.Rows [ 0 ] [ "Email" ].ToString ( );
                Session [ "imageUrl" ] =  dt.Rows [ 0 ] [ "ImageUrl" ].ToString ( );
                Session [ "createdDate" ]  = dt.Rows [ 0 ] [ "CreatedDate" ].ToString ( );
            }
            
        }

        void getPurchaseHistory ( )
        {
            int sr = 1;
            con = new SqlConnection ( Connection.GetConnectionString ( ) );
            cmd = new SqlCommand ( "Invoice" , con );
            cmd.Parameters.AddWithValue ( "@Action" , "ODRHISTORY" );
            cmd.Parameters.AddWithValue ( "@UserId" , Session [ "userId" ] );
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter ( cmd );
            dt = new DataTable ( );
            sda.Fill ( dt );
            dt.Columns.Add ( "SrNo", typeof(Int32) );
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    dataRow [ "SrNo" ] = sr;
                    sr++;
                }
            }

            if ( dt.Rows.Count == 0 )
            {
                rPurchaseHistory.FooterTemplate = null;
                rPurchaseHistory.FooterTemplate = new CustomTemplate ( ListItemType.Footer );
            }
            rPurchaseHistory.DataSource = dt;
            rPurchaseHistory.DataBind ( );
        }

        //Custom template class to add controls to the reapeater's header, item and footer section
        private sealed class CustomTemplate : ITemplate
        {
            private ListItemType ListItemType { get; set; }

            public CustomTemplate ( ListItemType type )
            {
                ListItemType = type;
            }

            public void InstantiateIn ( Control container )
            {
                if ( ListItemType == ListItemType.Footer )
                {
                    var footer = new LiteralControl ( "<tr><td><b>Hungry? Why not order meal for you.</b><a href='Menu.aspx' class='badge badge-info ml-2'>Click to Order </a></td></tr></tbody></table> " );
                    container.Controls.Add ( footer );
                }
            }
        }

        protected void rPurchaseHistory_ItemDataBound ( object sender , RepeaterItemEventArgs e )
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

           
            double grandTotal = 0;
            HiddenField paymentId = e.Item.FindControl( "hdnPaymentId" ) as HiddenField;
            Repeater repOrders = e.Item.FindControl ( "rOrders" ) as Repeater;

            con = new SqlConnection ( Connection.GetConnectionString ( ) );
            cmd = new SqlCommand ( "Invoice" , con );
            cmd.Parameters.AddWithValue ( "@Action" , "INVOICBYID" );
            cmd.Parameters.AddWithValue ( "@PaymentId" , Convert.ToInt32(paymentId.Value) );
            cmd.Parameters.AddWithValue ( "@UserId" , Session [ "userId" ] );
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter ( cmd );
            dt = new DataTable ( );
            sda.Fill ( dt );
            if ( dt.Rows.Count > 0 )
            {
                foreach ( DataRow dataRow in dt.Rows )
                {
                   grandTotal += Convert.ToDouble(dataRow [ "TotalPrice" ]);
                }
            }
            DataRow dr = dt.NewRow ( );
            dr [ "TotalPrice"] = grandTotal; 
            dt.Rows.Add ( dr );
            repOrders.DataSource = dt;
            repOrders.DataBind ( );
            }
        }
    }
}