<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListLetters.ascx.cs" Inherits="GIBS.Modules.DonationTracker.ListLetters" %>

<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>




<asp:Panel ID="PanelGrid" runat="server">

<div style="text-align:right; width:100%;"> <asp:CheckBox ID="cbxShowInActive" runat="server" Text=" &nbsp;Show Inactive Templates" 
        AutoPostBack="True" oncheckedchanged="cbxShowInActive_CheckedChanged"  /></div>

<asp:GridView ID="GridView1" runat="server" EnableModelValidation="True" 
    DataKeyNames="LetterID" OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating"   
    resourcekey="GridView1Resource1" AutoGenerateColumns="False" CssClass="dnnGrid"  
    CellPadding="4" GridLines="None" >
   <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Center" />
    <RowStyle CssClass="dnnGridItem" />
    <AlternatingRowStyle CssClass="dnnGridAltItem" />
    <EditRowStyle CssClass="dnnFormInput" />
    <SelectedRowStyle CssClass="dnnFormError" />
    <FooterStyle CssClass="dnnGridFooter" />
    <Columns>

        <asp:TemplateField HeaderText="" meta:resourcekey="TemplateFieldResource1" ItemStyle-VerticalAlign="Top">
         <ItemTemplate>
           <asp:LinkButton ID="LinkButtonEdit"    
             CommandArgument='<%# Eval("LetterID") %>' 
             CommandName="Edit" runat="server" meta:resourcekey="LinkButtonEditResource1">
             Edit</asp:LinkButton>
         </ItemTemplate>
       </asp:TemplateField>
      
        <asp:BoundField HeaderText="Name" DataField="LetterName" ItemStyle-VerticalAlign="Top" />
       


        <asp:TemplateField HeaderText="Letter">
            <ItemTemplate>
            <asp:Label ID="lblPLetter" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Letter").ToString().PadRight(280).Substring(0,280).TrimEnd()  %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField HeaderText="Created By" DataField="CreatedByUserName" Visible="False"></asp:BoundField>
        <asp:BoundField HeaderText="IsActive" DataField="IsActive" ItemStyle-VerticalAlign="Top"></asp:BoundField>

    </Columns>
</asp:GridView>

<br />
<div>
    <asp:linkbutton CssClass="dnnPrimaryAction" id="cmdAddRecord" resourcekey="cmdAddRecord" 
        runat="server" text="Add New Record" OnClick="cmdAddRecord_Click" 
        meta:resourcekey="cmdAddNewRecord"></asp:linkbutton>

        

</div>

</asp:Panel>

<asp:panel id="PanelLetterDetail" runat="server" Visible="false">
 	
<div class="dnnForm" id="form-edit-drives">


    <fieldset>

<div class="dnnFormItem">	
	<dnn:label id="lblLetterName" runat="server" controlname="txtLetterName" suffix=":" />

	<asp:TextBox ID="txtLetterName" runat="server" meta:resourcekey="LetterName" />

	<asp:RequiredFieldValidator runat="server" id="reqLetterName" controltovalidate="txtLetterName" errormessage="Letter Name is Required!" resourcekey="reqLetterName" />

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
		<dnn:label id="lblLetter" runat="server" controlname="txtLetter" suffix=":" />
			<asp:TextBox ID="txtLetter" runat="server" Columns="30" Height="160px" TextMode="MultiLine"></asp:TextBox>				
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
[InHonorOf]</pre>

</div>




    </fieldset>

</div>  

    <asp:HiddenField ID="HiddenLetterID" runat="server" />

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

