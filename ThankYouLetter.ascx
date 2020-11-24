<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ThankYouLetter.ascx.cs" Inherits="GIBS.Modules.DonationTracker.ThankYouLetter" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<div style=" text-align:center;">
<asp:Label ID="lblDebug" runat="server" Text=""></asp:Label>
    <br /><asp:Label ID="LblEmail" runat="server" Text=""></asp:Label>
</div>
<div style=" text-align:right;">
    <asp:HyperLink ID="HyperLinkNewSearch" runat="server" Visible="false" CssClass="btn btn-primary">New Search</asp:HyperLink>
    <asp:HyperLink ID="HyperLinkReturnToDonor" runat="server" CssClass="btn btn-primary">Return To Donor</asp:HyperLink> 
</div>

<asp:DropDownList ID="ddlLetterTemplates" runat="server" AutoPostBack="True" 
    onselectedindexchanged="ddlLetterTemplates_SelectedIndexChanged">
</asp:DropDownList>

<asp:TextBox ID="txtLetter" runat="server" TextMode="MultiLine" Height="400px" Width="100%" />
<asp:Button ID="btnSave" runat="server" Text="Generate Thank You Letter" onclick="btnSave_Click" CssClass="btn btn-primary" />
<asp:Button ID="btnEmail" runat="server" Text="E-Mail Thank You" onclick="btnEmail_Click" CssClass="btn btn-primary" />


<asp:HiddenField ID="hidLetterID" runat="server" />
<asp:HiddenField ID="hidPledgeID" runat="server" Value="0" />
<asp:HiddenField ID="hidDonationID" runat="server" Value="0" />
<rsweb:ReportViewer ID="ReportViewer1" OnPreRender="ReportViewer1_PreRender" Visible="false" AsyncRendering="False" Width="100%" Height="1392px" ShowToolBar="true" ShowExportControls="true" ShowFindControls="false" ShowPrintButton="true" ShowParameterPrompts="true" runat="server">
</rsweb:ReportViewer>