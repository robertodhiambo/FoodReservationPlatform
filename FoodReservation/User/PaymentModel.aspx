<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="PaymentModel.aspx.cs" Inherits="FoodReservation.User.PaymentModel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>M-Pesa Payment</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f2f2f2;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }

        .payment-container {
            background-color: white;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 6px 18px rgba(0, 0, 0, 0.1);
            width: 100%;
            max-width: 400px;
            margin-top: 40px;
        }

        h2 {
            text-align: center;
            margin-bottom: 20px;
            color: #2c3e50;
        }

        label {
            display: block;
            margin-bottom: 6px;
            font-weight: 600;
        }

        .form-control {
            width: 100%;
            padding: 10px;
            margin-bottom: 15px;
            border-radius: 6px;
            border: 1px solid #ccc;
            font-size: 14px;
            transition: border-color 0.3s;
        }

        .form-control:focus {
            border-color: #4CAF50;
            outline: none;
        }

        .btn {
            width: 100%;
            padding: 10px;
            background-color: #4CAF50;
            color: white;
            font-weight: bold;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .btn:hover {
            background-color: #45a049;
        }

        .center-wrapper {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 80vh; /* enough height to visibly center the form */
}


        .message {
            text-align: center;
            margin-bottom: 15px;
            font-weight: 600;
            color: #3366cc;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="center-wrapper">
        <div class="payment-container">
            <h2>M-Pesa Payment</h2>

            <asp:Label ID="lblMessage" runat="server" CssClass="message" />

            <div>
                <label for="txtAmount">Amount:</label>
                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>

            <div>
                <label for="txtPhone">Phone Number:</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
            </div>

            <div>
                <asp:Button ID="BtnPay" runat="server" Text="Pay Now" CssClass="btn" OnClick="BtnPay_Click" />
            </div>
        </div>
    </div>
</asp:Content>

