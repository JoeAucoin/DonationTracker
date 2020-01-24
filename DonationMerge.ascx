<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DonationMerge.ascx.cs" Inherits="GIBS.Modules.DonationTracker.DonationMerge" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>



<dnn:DnnCssInclude ID="DnnCssInclude2" runat="server" FilePath="~/DesktopModules/GIBS/FBClients/Style.css" />
<dnn:DnnCssInclude ID="DnnCssInclude3" runat="server" FilePath="https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/smoothness/jquery-ui.css" />
<script type="text/javascript" language="javascript" >
    // BIND DNN Tabs
    //jQuery(function ($) { $('#form-demo').dnnTabs(); });
</script>
<div class="dnnForm" id="form-demo">





    <ul class="dnnAdminTabNav">
        <li><a href="#Client">Search For Merge Donors</a></li>
    </ul>
    <div id="Client" class="dnnClear">

        <div style="font-size:1.4em; line-height:1.2em;"><asp:Label ID="lblMasterClient" runat="server" Text="lblMasterClient" /></div>

        <asp:Label runat="server" ID="lblFormMessage" CssClass="dnnFormMessage dnnFormInfo" ResourceKey="lblFormMessage" Visible="False"/>
           
     <div style="text-align:center; font-size:1.2em;">Search for record to MERGE with the one above...</div>
           
            <div class="dnnFormItem dnnFormHelp dnnClear">
                <p class="dnnFormRequired"><asp:Label ID="lblRequiredFields" runat="server" ResourceKey="Required Indicator" Visible="False" /></p>
            </div>
            <div style="position:relative;float:right;padding-right:30px;">
                <asp:Button ID="btnSearch1" runat="server" ResourceKey="btnSearch" onclick="btnSearch_Click" CssClass="dnnPrimaryAction" Visible="false" />
                
                <br />
                <asp:Label ID="lblSearchCriteria" runat="server" Text="" Visible="false" />
            </div>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ID="lblCriteria" runat="server" ControlName="txtSearch" Suffix=":" />
            <asp:TextBox ID="txtSearch" runat="server" />
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/icon_search_16px.gif"
                OnClick="btnSearch_Click"></asp:ImageButton>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblSearchField" runat="server" ControlName="ddlSearchType" Suffix=":" />
            <asp:DropDownList ID="ddlSearchType" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lblOrderBy" runat="server" ControlName="ddlOrderBy" Suffix=":" />
            <asp:DropDownList ID="ddlOrderBy" runat="server" />
        </div>
    </fieldset>

    <div style="text-align:center; font-size:1.4em; padding-bottom:10px; color:Blue;">    <asp:Label ID="lblMsg" runat="server" Text="" />    </div>
  
   <div style="width:100%;">
            <asp:GridView ID="GridViewSearchChild" runat="server" CssClass="dnnGrid" HorizontalAlign="Center" 
                    DataKeyNames="UserID"  OnRowEditing="GridViewSearch_RowEditing" 
                    PageSize="100" AllowPaging="True" AutoGenerateColumns="False" 
                    resourcekey="GridViewSearch" EmptyDataText="No Records Found" >
                        <PagerStyle CssClass="gridViewPager" />
    <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Center" />
    
    <RowStyle CssClass="dnnGridItem" />
    <AlternatingRowStyle CssClass="dnnGridAltItem" />

    <FooterStyle CssClass="dnnGridFooter" />  
                    <PagerSettings Mode="NumericFirstLast" /> 
                    <Columns>
                    <asp:BoundField HeaderText="UserID" DataField="UserID" Visible="true"></asp:BoundField>
                    <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                        <ItemTemplate >
                             <a href='<%#DotNetNuke.Common.Globals.NavigateURL("Edit","UserId=" + Eval("UserID"),"mid=" + ModuleId) %>' target='_blank'  >
                                VIEW
                            </a>

                            
                            </ItemTemplate>
                    </asp:TemplateField>

                     
           
                        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                            
                                <asp:Button GroupName="ChildMerge" Text="Merge"  ID="btnChildMerge"  CausesValidation="False"     
                                    CommandArgument='<%# Eval("UserID") %>'  
                                    OnClick="GridViewSearchChild_SelectedIndexChanged" runat="server">
                                </asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>

                  


                        
                        <asp:BoundField HeaderText="Last Name" DataField="LastName" ItemStyle-HorizontalAlign="Left"></asp:BoundField>
                        <asp:BoundField HeaderText="First Name" DataField="FirstName"></asp:BoundField>
                        <asp:BoundField HeaderText="Company" DataField="Company"></asp:BoundField>
              
                        <asp:BoundField HeaderText="Address" DataField="Street"></asp:BoundField>
                        <asp:BoundField HeaderText="City" DataField="City" ></asp:BoundField>

                </Columns>
            </asp:GridView>	
            
        </div>
   
</div>
<asp:HiddenField ID="hidClientIDMaster" runat="server" />
</div>