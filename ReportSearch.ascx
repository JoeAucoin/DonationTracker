<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportSearch.ascx.cs" Inherits="GIBS.Modules.DonationTracker.ReportSearch" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>


<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/redmond/jquery-ui.css" />

<script type="text/javascript">

    $(function () {
        $("#txtStartDate").datepicker({
            numberOfMonths: 2,
            showButtonPanel: false,
            showCurrentAtPos: 0
        });
        $("#txtEndDate").datepicker({
            numberOfMonths: 2,
            showButtonPanel: false,
            showCurrentAtPos: 0
        });
    });

 </script>


<div style=" float:right">
<asp:Button ID="btnFilter" runat="server" Text="Update" 
                resourcekey="btnFilter" onclick="btnFilter_Click" CssClass="dnnPrimaryAction" />
</div>


<div class="dnnForm" id="form-demo">
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblStartDate" runat="server" CssClass="dnnFormLabel" AssociatedControlID="txtStartDate" Text="Start Date" />
            <asp:TextBox ID="txtStartDate" runat="server" ClientIDMode="Static" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblEndDate" runat="server" CssClass="dnnFormLabel" AssociatedControlID="txtEndDate" Text="Start Date" />
            <asp:TextBox ID="txtEndDate" runat="server" ClientIDMode="Static" />
        </div>	
		<div class="dnnFormItem">
            <dnn:Label ID="lblDonationAmount" runat="server" CssClass="dnnFormLabel" AssociatedControlID="txtDonationAmount" />
            <asp:TextBox ID="txtDonationAmount" runat="server" />
        </div>        
        	
    </fieldset>
</div>	



<h4><asp:Label ID="lblDetails" runat="server" Text=""></asp:Label></h4>
<br clear="all" />

 <asp:GridView ID="GridViewDetailReport" runat="server" EnableModelValidation="True"   
    DataKeyNames="DonationID" OnRowDataBound="GridViewDetailReport_RowDataBound"   
    AutoGenerateColumns="False" 
    GridLines="None" 
    CssClass="dnnGrid" 
    resourcekey="GridViewDetailReport" ShowFooter="true" HorizontalAlign="Center">
    <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Center" />
    <RowStyle CssClass="dnnGridItem" />
    <AlternatingRowStyle CssClass="dnnGridAltItem" />
    <EditRowStyle CssClass="dnnFormInput" />
    <SelectedRowStyle CssClass="dnnFormError" />
    <FooterStyle CssClass="dnnGridFooter" />
    <Columns>


       <asp:BoundField HeaderText="DriveName" DataField="DriveName"></asp:BoundField>
        <asp:BoundField HeaderText="Date" DataField="DonationDate" DataFormatString="{0:d}" HtmlEncode="false" FooterText="Total" FooterStyle-HorizontalAlign="Right"></asp:BoundField>
	<asp:TemplateField HeaderText="Donation" Visible="true" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
         <ItemTemplate>
         <asp:Label ID="lblAmount" runat="server" Text='<%# String.Format("{0:C}", Eval("DonationAmount")) %>'/>
         </ItemTemplate>
         <FooterTemplate>
         <asp:Label ID="lblTotal" runat="server" Text="Total"></asp:Label>
         </FooterTemplate>
     </asp:TemplateField>
        <asp:BoundField HeaderText="Company" DataField="Company"></asp:BoundField>
        <asp:BoundField HeaderText="Prefix" DataField="Prefix"></asp:BoundField>
        <asp:BoundField HeaderText="First Name" DataField="FirstName"></asp:BoundField>
        <asp:BoundField HeaderText="Middle" DataField="MiddleName"></asp:BoundField>
        <asp:BoundField HeaderText="Last Name" DataField="LastName"></asp:BoundField>
        <asp:BoundField HeaderText="Suffix" DataField="Suffix"></asp:BoundField>
        
        <asp:BoundField HeaderText="Street" DataField="Street"></asp:BoundField>
        <asp:BoundField HeaderText="City" DataField="City"></asp:BoundField>
        <asp:BoundField HeaderText="State" DataField="State"></asp:BoundField>
        <asp:BoundField HeaderText="PostalCode" DataField="PostalCode" HtmlEncode="false" DataFormatString="{0:#####-####}" ></asp:BoundField>
        
	<asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkFollowup" runat="server" 
                            AutoPostBack="true" OnCheckedChanged="GridViewDetailReport_SelectedIndexChanged"
                            Checked='<%# Convert.ToBoolean(Eval("Followup")) %>'/>
                    </ItemTemplate>                    
                </asp:TemplateField>
		
     

    </Columns>
</asp:GridView>

<br clear="all" />

<p><asp:linkbutton  CssClass="dnnPrimaryAction" id="cmdExport" resourcekey="cmdExport" 
        runat="server" text="Export" OnClick="cmdExport_Click"></asp:linkbutton>   &nbsp;     
        

        </p>