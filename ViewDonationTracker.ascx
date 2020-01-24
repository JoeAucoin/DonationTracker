
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewDonationTracker.ascx.cs"
    Inherits="GIBS.Modules.DonationTracker.ViewDonationTracker" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>

<style type="text/css">
.gridViewPager td
{
	padding-left: 4px;
	padding-right: 4px;
	padding-top: 1px;
	padding-bottom: 2px;
}
.CommandButtonLetter 
{ font-size:1.3em; }
</style>

<div class="dnnForm" id="form-search">
    <div style="float: right;">
       
        <br />
        <asp:Label ID="lblDebug" runat="server" Text="" /></div>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCriteria" runat="server" ControlName="txtSearch" Suffix=":" />
            <asp:TextBox ID="txtSearch" runat="server" Text="" />&nbsp; 
           
<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/DesktopModules/GIBS/DonationTracker/images/search.png" AlternateText="Serch" ImageAlign="Bottom" Width="30px" ToolTip="Search"
                OnClick="btnSearch_Click"></asp:ImageButton>
            
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSearchField" runat="server" ControlName="ddlSearchType" Suffix=":" />
            <asp:DropDownList ID="ddlSearchType" runat="server" >
            <asp:ListItem Text="First Name" Value="FirstName" />
            <asp:ListItem Text="Last Name" Value="LastName" Selected="True" />
            <asp:ListItem Text="Company" Value="Company" />
            <asp:ListItem Text="City" Value="City" />
             <asp:ListItem Text="Prospect Manager" Value="ProspectManager" />
            
            </asp:DropDownList>
        </div>



        <div class="dnnFormItem">
            <dnn:Label ID="lblOrderBy" runat="server" ControlName="ddlOrderBy" Suffix=":" />
            <asp:DropDownList ID="ddlOrderBy" runat="server" >
            <asp:ListItem Text="First Name" Value="FirstName" />
            <asp:ListItem Text="Last Name" Value="LastName" Selected="True" />
            <asp:ListItem Text="Company" Value="Company" />
            <asp:ListItem Text="City" Value="City" />
           
            </asp:DropDownList>
        </div>
    </fieldset>
         <div style="float: right;"> <asp:Button ID="btnAddNewUser" resourcekey="btnAddNewUser" runat="server" CssClass="btn btn-primary"
            Text="Button" OnClick="btnAddNewUser_Click" />
         </div>
</div>
<asp:Panel ID="plLetterSearch" runat="server" HorizontalAlign="Center">
    <asp:Repeater ID="rptLetterSearch" runat="server">
        <ItemTemplate>
            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="CommandButtonLetter" NavigateUrl='<%# FilterURL(Container.DataItem.ToString(),"1") %>'
                Text='<%# Container.DataItem %>'>
            </asp:HyperLink>&nbsp;&nbsp;
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>



<asp:DataGrid ID="grdUsers" AutoGenerateColumns="false" Width="100%" CellPadding="2"
    GridLines="None" CssClass="table table-striped table-bordered table-list" runat="server" 
    OnPageIndexChanged="dnnGrid_PageIndexChanged">
    <PagerStyle CssClass="NormalBold" />
    <HeaderStyle HorizontalAlign="Center" />
    <ItemStyle CssClass="dnnGridItem" />
    <AlternatingItemStyle CssClass="dnnGridAltItem" />

    <Columns>
        <dnn:ImageCommandColumn CommandName="Edit" ImageURL="~/images/edit.gif" EditMode="URL"
            HeaderText="Edit" KeyField="UserID" />
        <dnn:ImageCommandColumn CommandName="Donate" ImageURL="~/DesktopModules/GIBS/DonationTracker/images/icon_money.png"
            EditMode="URL" KeyField="UserID" Visible="true" ShowImage="True" Text="Donate"
            HeaderText="Give" />
        <dnn:ImageCommandColumn CommandName="Pledge" ImageURL="~/DesktopModules/GIBS/DonationTracker/images/icon_pledge.png"
            EditMode="URL" KeyField="UserID" Visible="true" ShowImage="True" Text="Pledge" 
            HeaderText="Pledge" />

        <dnn:TextColumn DataField="UserName" HeaderText="Username" Visible="false" />
        <dnn:TextColumn DataField="FirstName" HeaderText="First Name" />
        <dnn:TextColumn DataField="LastName" HeaderText="Last Name" />
        <dnn:TextColumn DataField="Company" HeaderText="Company" Visible="true" />
        <dnn:TextColumn DataField="DisplayName" HeaderText="DisplayName" Visible="false" />
        <dnn:TextColumn DataField="Street" HeaderText="Street" Visible="false" />
        <asp:TemplateColumn HeaderText="Address">
            <ItemTemplate>
                <asp:Label ID="FullAddress" Text='<%# Eval("Street") + ", " + Eval("City") + ", " + GetStateLookup(Eval("State")) + " " + Eval("PostalCode") %>'
                    runat="server" />
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
</asp:DataGrid>
<div style="text-align: center; margin: 0 auto;">
    <dnn:PagingControl ID="ctlPagingControl" runat="server" Visible="false" CssClass="dnnGridHeader"
        BorderWidth="2px" BorderColor="Black"></dnn:PagingControl>
</div>
<div style="margin-top: 15px;">
    <asp:Button ID="btnReports" resourcekey="btnReports" runat="server" CssClass="btn btn-default" Visible="false"
        Text="Reports" OnClick="btnReports_Click" />
    <asp:Button ID="btnReportSearch" resourcekey="btnReportSearch" runat="server" CssClass="btn btn-default" Visible="false"
        Text="Donor Search" OnClick="btnReportSearch_Click" />
    <asp:Button ID="btnSources" resourcekey="btnSources" runat="server" CssClass="btn btn-default"
        Text="Sources" OnClick="btnSources_Click" />
    <asp:Button ID="btnLetters" resourcekey="btnLetters" runat="server" CssClass="btn btn-default"
        Text="Letters" OnClick="btnLetters_Click" />
</div>
