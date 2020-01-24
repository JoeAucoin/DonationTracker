<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="GIBS.Modules.DonationTracker.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>

<div class="dnnForm" id="form-settings">

    <fieldset>

<dnn:sectionhead id="sectGeneralSettings" cssclass="Head" runat="server" text="General Settings" section="GeneralSection"
	includerule="False" isexpanded="True"></dnn:sectionhead>

<div id="GeneralSection" runat="server">   
            
                    
                    		
	<div class="dnnFormItem">					
	<dnn:label id="lblNumPerPage" runat="server" controlname="ddlNumPerPage" suffix=":"></dnn:label>
	<asp:DropDownList ID="ddlNumPerPage" runat="server">
		<asp:ListItem Text="2" Value="2"></asp:ListItem>
		<asp:ListItem Text="5" Value="5"></asp:ListItem>
		<asp:ListItem Text="10" Value="10"></asp:ListItem>
		<asp:ListItem Text="20" Value="20"></asp:ListItem>
			<asp:ListItem Text="30" Value="30"></asp:ListItem>
		<asp:ListItem Text="40" Value="40"></asp:ListItem>
		<asp:ListItem Text="50" Value="50"></asp:ListItem>
		<asp:ListItem Text="100" Value="100"></asp:ListItem>

		</asp:DropDownList>			
				
	</div>	

	<div class="dnnFormItem">
    
        <dnn:label id="lblAddUserRole" runat="server" controlname="ddlRoles" suffix=":"></dnn:label>
        <asp:DropDownList ID="ddlRoles" runat="server" datavaluefield="RoleName" datatextfield="RoleName">
        </asp:DropDownList>
	</div>

	<div class="dnnFormItem">
        <dnn:label id="lblEmailFrom" runat="server" controlname="txtEmailFrom" suffix=":"></dnn:label>
        <asp:TextBox ID="txtEmailFrom" width="320" cssclass="NormalTextBox" runat="server"></asp:TextBox>
	</div>

	<div class="dnnFormItem">
        <dnn:label id="lblEmailBCC" runat="server" controlname="txtEmailBCC" suffix=":"></dnn:label>
        <asp:TextBox ID="txtEmailBCC" width="320" cssclass="NormalTextBox" runat="server"></asp:TextBox>
	</div>

	<div class="dnnFormItem">
        <dnn:label id="lblEmailSubject" runat="server" controlname="txtEmailSubject" suffix=":"></dnn:label>
        <asp:TextBox ID="txtEmailSubject" width="320" cssclass="NormalTextBox" runat="server"></asp:TextBox>
	</div>

	<div class="dnnFormItem">
        <dnn:label id="lblEmailMessage" runat="server" controlname="txtEmailMessage" suffix=":"></dnn:label>
        <asp:TextBox ID="txtEmailMessage" runat="server" Columns="30" Height="120px" TextMode="MultiLine"></asp:TextBox>
	</div>

	<div class="dnnFormItem">
    
        <dnn:label id="lblReportsUserRole" runat="server" controlname="ddlReportsRoles" suffix=":"></dnn:label>
        <asp:DropDownList ID="ddlReportsRoles" runat="server" datavaluefield="RoleName" datatextfield="RoleName">
        </asp:DropDownList>
	</div>

	<div class="dnnFormItem">
    
        <dnn:label id="lblMergeUserRole" runat="server" controlname="ddlMergeRoles" suffix=":"></dnn:label>
        <asp:DropDownList ID="ddlMergeRoles" runat="server" datavaluefield="RoleName" datatextfield="RoleName">
        </asp:DropDownList>
	</div>	

    <div class="dnnFormItem">					
        <dnn:label id="lblShowSendPassword" runat="server" controlname="cbxShowSendPassword" suffix=":"></dnn:label>
        <asp:CheckBox ID="cbxShowSendPassword" runat="server" />
    </div>

    <div class="dnnFormItem">					
        <dnn:label id="lblEmailNewUserCredentials" runat="server" controlname="cbxEmailNewUserCredentials" suffix=":"></dnn:label>
        <asp:CheckBox ID="cbxEmailNewUserCredentials" runat="server" />
    </div>

	<div class="dnnFormItem">					
        <dnn:label id="lblShowDonationHistory" runat="server" controlname="cbxShowDonationHistory" suffix=":"></dnn:label>
        <asp:CheckBox ID="cbxShowDonationHistory" runat="server" />
    </div>

	<div class="dnnFormItem">					
        <dnn:label id="lblEnableAddNewDonor" runat="server" controlname="cbxEnableAddNewDonor" suffix=":"></dnn:label>
        <asp:CheckBox ID="cbxEnableAddNewDonor" runat="server" />
    </div>		



 </div>
        
<dnn:sectionhead id="SectionheadProfileItems" cssclass="Head" runat="server" text="Profile Items Settings" section="ProfileItemsSection"
	includerule="False" isexpanded="False"></dnn:sectionhead>

<div id="ProfileItemsSection" runat="server">
        <div class="dnnFormItem">
            <dnn:Label runat="server" ID="lblProfileCheckDoNotMail" ControlName="txtProfileCheckDoNotMail" ResourceKey="lblProfileCheckDoNotMail" Suffix=":" />
            <asp:Textbox ID="txtProfileCheckDoNotMail" runat="server" ReadOnly="true" />
        </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckAnonymous" ControlName="txtProfileCheckAnonymous" ResourceKey="lblProfileCheckAnonymous" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckAnonymous" runat="server" ReadOnly="true" />
    </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckCompany" ControlName="txtProfileCheckCompany" ResourceKey="lblProfileCheckCompany" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckCompany" runat="server" ReadOnly="true" />
    </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckStreet2" ControlName="txtProfileCheckStreet2" ResourceKey="lblProfileCheckStreet2" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckStreet2" runat="server" ReadOnly="true" />
    </div>
	
    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckWorkPhone" ControlName="txtProfileCheckWorkPhone" ResourceKey="lblProfileCheckWorkPhone" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckWorkPhone" runat="server" ReadOnly="true" />
    </div>	
	
    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckAdditionalName" ControlName="txtProfileCheckAdditionalName" ResourceKey="lblProfileCheckAdditionalName" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckAdditionalName" runat="server" ReadOnly="true" />
    </div>


    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckAdditionalFirstName" ControlName="txtProfileCheckAdditionalFirstName" ResourceKey="lblProfileCheckAdditionalFirstName" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckAdditionalFirstName" runat="server" ReadOnly="true" />
    </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckAdditionalMI" ControlName="txtProfileCheckAdditionalMI" ResourceKey="lblProfileCheckAdditionalMI" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckAdditionalMI" runat="server" ReadOnly="true" />
    </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckCapacityRating" ControlName="txtProfileCheckCapacityRating" ResourceKey="lblProfileCheckCapacityRating" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckCapacityRating" runat="server" ReadOnly="true" />
    </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckInclinationRating" ControlName="txtProfileCheckInclinationRating" ResourceKey="lblProfileCheckInclinationRating" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckInclinationRating" runat="server" ReadOnly="true" />
    </div>		

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckStage" ControlName="txtProfileCheckStage" ResourceKey="lblProfileCheckStage" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckStage" runat="server" ReadOnly="true" />
    </div>		

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckProspectResearch" ControlName="txtProfileCheckProspectResearch" ResourceKey="lblProfileCheckProspectResearch" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckProspectResearch" runat="server" ReadOnly="true" />
    </div>		

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckProspectManager" ControlName="txtProfileCheckProspectManager" ResourceKey="lblProfileCheckProspectManager" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckProspectManager" runat="server" ReadOnly="true" />
    </div>		

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckAge" ControlName="txtProfileCheckAge" ResourceKey="lblProfileCheckAge" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckAge" runat="server" ReadOnly="true" />
    </div>			

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckComments" ControlName="txtProfileCheckComments" ResourceKey="lblProfileCheckComments" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckComments" runat="server" ReadOnly="true" />
    </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckAltStreet" ControlName="txtProfileCheckAltStreet" ResourceKey="lblProfileCheckAltStreet" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckAltStreet" runat="server" ReadOnly="true" />
    </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckAltCity" ControlName="txtProfileCheckAltCity" ResourceKey="lblProfileCheckAltCity" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckAltCity" runat="server" ReadOnly="true" />
    </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckAltState" ControlName="txtProfileCheckAltState" ResourceKey="lblProfileCheckAltState" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckAltState" runat="server" ReadOnly="true" />
    </div>

    <div class="dnnFormItem">
        <dnn:Label runat="server" ID="lblProfileCheckAltPostalCode" ControlName="txtProfileCheckAltPostalCode" ResourceKey="lblProfileCheckAltPostalCode" Suffix=":" />
        <asp:Textbox ID="txtProfileCheckAltPostalCode" runat="server" ReadOnly="true" />
    </div>


</div> 

        
        
<dnn:sectionhead id="Sectionhead1" cssclass="Head" runat="server" text="Report Server Settings" section="ReportServerSection"
	includerule="False" isexpanded="True"></dnn:sectionhead>

<div id="ReportServerSection" runat="server">   

     <div class="dnnFormItem">					
    <dnn:label id="lblurlReportServer" runat="server" suffix=":" controlname="txturlReportServer" ResourceKey="lblurlReportServer" />
         <asp:TextBox ID="txturlReportServer" runat="server" />		
    </div>
 
      <div class="dnnFormItem">					
    <dnn:label id="lblReportPath" runat="server" suffix=":" controlname="txtReportPath" ResourceKey="lblReportPath" />
         <asp:TextBox ID="txtReportPath" runat="server" />		
    </div>
 
      <div class="dnnFormItem">					
    <dnn:label id="lblRSCredentialsUserName" runat="server" suffix=":" controlname="txtRSCredentialsUserName" ResourceKey="lblRSCredentialsUserName" />
         <asp:TextBox ID="txtRSCredentialsUserName" runat="server" />		
    </div>



     <div class="dnnFormItem">					
    <dnn:label id="lblRSCredentialsPassword" runat="server" suffix=":" controlname="txtRSCredentialsPassword" ResourceKey="lblRSCredentialsPassword" />
         <asp:TextBox ID="txtRSCredentialsPassword" runat="server" />		
    </div>



     <div class="dnnFormItem">					
    <dnn:label id="lblRSCredentialsDomain" runat="server" suffix=":" controlname="txtRSCredentialsDomain" ResourceKey="lblRSCredentialsDomain" />
         <asp:TextBox ID="txtRSCredentialsDomain" runat="server" />		
    </div>

</div>        			


    </fieldset>

</div>