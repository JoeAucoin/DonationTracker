<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListDrives.ascx.cs" Inherits="GIBS.Modules.DonationTracker.ListDrives" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/smoothness/jquery-ui.css" />
<script type="text/javascript">



    $(function () {
        $("#txtDriveDate").datepicker({
            numberOfMonths: 2,
            showButtonPanel: false,
            showCurrentAtPos: 1
        });

    });

    function UseData() {
        $.Watermark.HideAll();   //Do Stuff   $.Watermark.ShowAll();
    }

</script>
<style type="text/css">
.tokens textarea 
{ 
   
border: none;
    overflow: auto;
    outline: none;
    -webkit-box-shadow: none;
    -moz-box-shadow: none;
    box-shadow: none;


}
</style>



<asp:Panel ID="PanelGrid" runat="server">

<asp:GridView ID="GridView1" runat="server" EnableModelValidation="True" 
    DataKeyNames="DriveID" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"   
    resourcekey="GridView1Resource1" AutoGenerateColumns="False" CssClass="dnnGrid"  
    CellPadding="4" GridLines="None" >
   <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Center" />
    <RowStyle CssClass="dnnGridItem" />
    <AlternatingRowStyle CssClass="dnnGridAltItem" />
    <EditRowStyle CssClass="dnnFormInput" />
    <SelectedRowStyle CssClass="dnnFormError" />
    <FooterStyle CssClass="dnnGridFooter" />
    <Columns>

        <asp:TemplateField HeaderText="" meta:resourcekey="TemplateFieldResource1">
         <ItemTemplate>
           <asp:LinkButton ID="LinkButtonEdit"    
             CommandArgument='<%# Eval("DriveID") %>' 
             CommandName="Edit" runat="server" meta:resourcekey="LinkButtonEditResource1">
             Edit</asp:LinkButton>
         </ItemTemplate>
       </asp:TemplateField>
      
        <asp:BoundField HeaderText="Drive Name" DataField="DriveName" ItemStyle-Width="200px"></asp:BoundField>
        <asp:BoundField HeaderText="Thank You Letter" DataField="Description" Visible="false"></asp:BoundField>
        <asp:TemplateField HeaderText="Thank You Letter">
            <ItemTemplate>
            <asp:Label ID="lblTYLetter" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Description").ToString().PadRight(80).Substring(0,80).TrimEnd() %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Reminder">
            <ItemTemplate>
            <asp:Label ID="lblRLetter" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Reminder").ToString().PadRight(80).Substring(0,80).TrimEnd()  %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
		
        <asp:TemplateField HeaderText="Pledge Letter">
            <ItemTemplate>
            <asp:Label ID="lblPLetter" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PledgeLetter").ToString().PadRight(80).Substring(0,80).TrimEnd()  %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField HeaderText="Date" DataField="DriveDate" DataFormatString="{0:d}" Visible="False"></asp:BoundField>
        <asp:BoundField HeaderText="Created By" DataField="CreatedByUserName" Visible="False"></asp:BoundField>
        <asp:BoundField HeaderText="IsActive" DataField="IsActive"></asp:BoundField>

    </Columns>
</asp:GridView>

<br />
<div>
    <asp:linkbutton CssClass="dnnPrimaryAction" id="cmdAddRecord" resourcekey="cmdAddRecord" 
        runat="server" text="Add New Record" OnClick="cmdAddRecord_Click" 
        meta:resourcekey="cmdAddNewRecord"></asp:linkbutton>

        

</div>

</asp:Panel>

<asp:panel id="PanelDriveDetail" runat="server" Visible="false">
 	
<div class="dnnForm" id="form-edit-drives">


    <fieldset>

<div class="dnnFormItem">	
	<dnn:label id="lblDriveName" runat="server" controlname="txtDriveName" suffix=":" />

	<asp:TextBox ID="txtDriveName" runat="server" meta:resourcekey="DriveName" />

	<asp:RequiredFieldValidator runat="server" id="reqDriveName" controltovalidate="txtDriveName" errormessage="DriveName is Required!" resourcekey="reqDriveName" />

</div>	

<div class="dnnFormItem">	
		<dnn:label id="lblDriveDate" runat="server" controlname="txtDriveDate" suffix=":" />
			<asp:TextBox ID="txtDriveDate" runat="server" MaxLength="10" meta:resourcekey="DriveDate" ClientIDMode="Static"></asp:TextBox>
	    <asp:RequiredFieldValidator runat="server" id="reqDriveDate" resourcekey="reqDriveDate" controltovalidate="txtDriveDate" errormessage="Required!" />
				
</div>	

<div class="dnnFormItem">	
		<dnn:label id="lblIsActive" runat="server" controlname="isActive" suffix=":" />
		<asp:RadioButtonList runat="server" 
			ID="isActive" CssClass="NormalTextBox" RepeatDirection="Horizontal">
			 <asp:ListItem Value="false" Text="No" />
			 <asp:ListItem Value="true" Text="Yes" Selected="True" />
			 </asp:RadioButtonList>				
</div>	

<div class="dnnFormItem">	
		<dnn:label id="lblDriveNotes" runat="server" controlname="txtDriveNotes" suffix=":" />
			<asp:TextBox ID="txtDriveNotes" runat="server" Columns="30" Height="120px" TextMode="MultiLine"></asp:TextBox>				
</div>	



<div class="dnnFormItem">	
		<dnn:label id="lblReminder" runat="server" controlname="txtReminder" suffix=":" />
			<asp:TextBox ID="txtReminder" runat="server" Columns="30" Height="120px" TextMode="MultiLine" />				
</div>	



<div class="dnnFormItem">	
		<dnn:label id="lblPledgeLetter" runat="server" controlname="txtPledgeLetter" suffix=":" />
			<asp:TextBox ID="txtPledgeLetter" runat="server" Columns="30" Height="120px" TextMode="MultiLine" />				
</div>	



<div class="dnnFormItem">	
<dnn:label id="lblTokens" runat="server" controlname="txtTokens" suffix=":" />


<pre style="width:380px; float:left;">[DriveName]
[PrimaryAddress]
[FirstName]
[LastName]
[FirstNames]
[DonationAmount]
[DonationDate]
[PledgeAmount]
[TotalAmountPledged]
[PledgeStartDate]
[Frequency]
[NumberOfPayments]
[InHonorOf]
</pre>

</div>
    </fieldset>

</div>  

    <asp:HiddenField ID="HiddenItemID" runat="server" />

<p>
    <asp:linkbutton CssClass="dnnPrimaryAction" id="cmdEditRecord" resourcekey="cmdEditRecord" 
        runat="server" text="Update" OnClick="cmdEditRecord_Click"></asp:linkbutton>
        &nbsp;
        <asp:linkbutton CssClass="dnnSecondaryAction" id="cmdAddCancel" resourcekey="cmdAddCancel" 
        runat="server" text="Cancel" OnClick="cmdAddCancel_Click" CausesValidation="false"></asp:linkbutton>

</p>

</asp:panel> 



<div style="float:right; padding-top:10px;">
    <asp:linkbutton CssClass="dnnSecondaryAction" id="cmdCancel" resourcekey="cmdCancel" 
            runat="server" text="Cancel" OnClick="cmdCancel_Click" causesvalidation="False"></asp:linkbutton>
</div>
