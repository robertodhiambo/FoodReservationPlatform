<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="FoodReservation.User.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- about section -->

  <section class="about_section layout_padding">
    <div class="container  ">

      <div class="row">
        <div class="col-md-6 ">
          <div class="img-box">
            <img src="../TemplateFiles/images/coffee.jpg" alt="">
          </div>
        </div>
        <div class="col-md-6">
          <div class="detail-box">
            <div class="heading_container">
              <h2>
                We Are Robb's Foodie
              </h2>
            </div>
            <p>
              We provide customers with convenient, high-quality, and delicious meals through a seamless online 
                ordering experience. Our goal is to ensure fast and reliable delivery, excellent customer service, 
                and a diverse menu that caters to various dietary preferences. By leveraging cutting-edge technology, 
                we aim to enhance operational efficiency, maintain food safety standards, and create a hassle-free dining 
                experience for our customers. Through innovation and dedication, we strive to build a trusted brand that 
                prioritizes customer satisfaction and culinary excellence.
            </p>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- end about section -->

</asp:Content>
