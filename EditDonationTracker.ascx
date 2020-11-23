<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditDonationTracker.ascx.cs" Inherits="GIBS.Modules.DonationTracker.EditDonationTracker" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Audit" Src="~/controls/ModuleAuditControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/smoothness/jquery-ui.css" />

 

<script type="text/javascript">

//    $(document).ready(function () {

//        $("#DivDonationFromName").hide();

//        $('#<%= ddlTypeOfDonation.ClientID%>').on('change', function () {
//            if (this.value == 'In Memory Of') {
//                $("#DivDonationFromName").show();
//            }
//            else if (this.value == 'In Honor Of') {
//                $("#DivDonationFromName").show();
//            }

//            else {
//                $("#DivDonationFromName").hide();
//            }


//        });
//    });


    jQuery(function ($) {
        $('#tabs-donors').dnnTabs();
    });


    jQuery(function ($) {
        $("#<%= txtTelephone.ClientID %>").mask("(999) 999-9999? x99999");
        $("#<%= txtWorkPhone.ClientID %>").mask("(999) 999-9999? x99999");
        $("#<%= txtCellPhone.ClientID %>").mask("(999) 999-9999");
        $("#<%= txtFax.ClientID %>").mask("(999) 999-9999");
        $("#<%= txtZip.ClientID %>").mask("99999?-9999");

        $("#<%= txtFirstName.ClientID %>").Watermark("First Name");
        $("#<%= txtMiddleName.ClientID %>").Watermark("MI");
        $("#<%= txtLastName.ClientID %>").Watermark("Last Name");

    });


    $(function () {
        $("#txtDonationDate").datepicker({
            numberOfMonths: 2,
            showButtonPanel: false,
            showCurrentAtPos: 1
        });

    });

    $(function () {
        $("#txtPledgeDate").datepicker({
        dateFormat: "yy-mm-dd",
            numberOfMonths: 1,
            showButtonPanel: false,
            showCurrentAtPos: 0
        });
        $("#txtPledgeDateEnd").datepicker({
            dateFormat: "yy-mm-dd",
            numberOfMonths: 0,
            showButtonPanel: false,
            showCurrentAtPos: 0
        });

    });

    function UseData() {
        $.Watermark.HideAll();   //Do Stuff   $.Watermark.ShowAll();
    }

</script>

<script type="text/javascript">
    function SelectAll(id) {
        document.getElementById(id).focus();
        document.getElementById(id).select();
    }
</script>

<asp:Label ID="lblDebug" runat="server" Visible="false" />
<asp:HiddenField ID="hidDonationUserId" runat="server" />

<h2><asp:Label ID="LabelDonorName" runat="server" Text=""></asp:Label></h2>


<div style=" text-align:center; padding-bottom:6px;">
    <asp:Label ID="lblStatus" runat="server" Text="" CssClass="NormalRed"></asp:Label><br /></div>

<div class="dnnForm" id="tabs-donors">
    <ul class="dnnAdminTabNav">
        <li><a href="#DonorRecord">Donor Record <asp:Image
        ID="ImageDoNotMail" ImageUrl="~/DesktopModules/GIBS/DonationTracker/images/nomail.png" runat="server" 
        ToolTip="Do Not Mail" AlternateText="Do Not Mail" Visible="false" Height="20px" /></a></li>
        <li><a href="#AltAddress">Alt. Address</a></li>
         <li><a href="#Notes">Strategy/Notes</a></li>
        <li><a href="#History">History</a></li>
        <li><a href="#AddDonation">Add Donation</a></li>
         <li><a href="#AddPledge">Pledge</a></li>
        <li><a href="#MailingLabels">Mailing Labels</a></li>
    </ul>
    <div id="DonorRecord" class="dnnClear">
         <div style="float:right;">
            <asp:LinkButton ID="cmdUpdate" resourcekey="cmdUpdateDonorRecord" ValidationGroup="UserForm"
                OnClientClick="UseData();" runat="server" OnClick="cmdUpdate_Click"
                CssClass="dnnPrimaryAction"></asp:LinkButton><br />
                 <asp:LinkButton ID="LinkButtonMerge" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to MERGE another client to this record?');"     
             CommandArgument='' Text="Merge" CssClass="dnnSecondaryAction"
             CommandName="Merge" runat="server" onclick="LinkButtonMerge_Click" Visible="true">
             </asp:LinkButton>
                <br />
            <asp:LinkButton ID="cmdSendCredentials" Text="Send Password" runat="server" CssClass="dnnSecondaryAction" 
                OnClick="cmdSendCredentials_Click"></asp:LinkButton>
            <asp:Label ID="lblSendCredentials" runat="server" Text="" CssClass="NormalBold NormalRed" />
        </div>
        <div class="dnnForm" id="form-edit-donor">
            <fieldset>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblAnonymous" runat="server" ControlName="cbxAnonymous" Suffix=":" />
					 <asp:CheckBox ID="cbxAnonymous" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDoNotMail" runat="server" ControlName="cbxDoNotMail" Suffix=":" />
					 <asp:CheckBox ID="cbxDoNotMail" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDonorType" runat="server" ControlName="txtPrefix" Suffix=":" />
                    <asp:DropDownList ID="ddlDonorType" runat="server">
                    </asp:DropDownList>
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblPrefix" runat="server" ControlName="ddlPrefix" Suffix=":" />
                    <asp:DropDownList ID="ddlPrefix" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblFirstName" runat="server" ControlName="txtFirstName" Suffix=":" />
                    <asp:TextBox ID="txtFirstName" runat="server" ValidationGroup="UserForm" CssClass="dnnFormRequired"
                        Width="20%" /><asp:RequiredFieldValidator runat="server" ID="reqFirstName" CssClass="dnnFormMessage dnnFormError"
                        resourcekey="reqFirstName" ControlToValidate="txtFirstName" ErrorMessage="First Name Required!" Display="Dynamic"
                        ValidationGroup="UserForm" />
                    <asp:TextBox ID="txtMiddleName" runat="server" Width="5%" />
                    <asp:TextBox ID="txtLastName" runat="server" ValidationGroup="UserForm" CssClass="dnnFormRequired"
                        Width="20%" />
                    
                    <asp:RequiredFieldValidator runat="server" ID="reqLastName" resourcekey="reqLastName"
                        CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtLastName" ErrorMessage="Lanst Name Required!" Display="Dynamic"
                        ValidationGroup="UserForm" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblSuffix" runat="server" ControlName="ddlSuffix" Suffix=":"></dnn:Label>
                    <asp:DropDownList ID="ddlSuffix" runat="server">
                    </asp:DropDownList>
                </div>
		        <div class="dnnFormItem">
                    <dnn:Label ID="lblAdditionalName" runat="server" ControlName="txtAdditionalName" Suffix=":" />
                    <asp:TextBox ID="txtAdditionalFirstName" runat="server" Width="20%" />
					<asp:TextBox ID="txtAdditionalMI" runat="server" Width="5%" />
                    <asp:TextBox ID="txtAdditionalName" runat="server" Width="20%" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblCompany" runat="server" ControlName="txtCompany" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtCompany" runat="server"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblEmail" runat="server" ControlName="txtEmail" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblStreet" runat="server" ControlName="txtStreet" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtStreet" runat="server"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblStreet2" runat="server" ControlName="txtStreet2" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtStreet2" runat="server"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblCityStateZip" runat="server" ControlName="txtCity" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtCity" runat="server" Width="20%"></asp:TextBox>&nbsp;<asp:DropDownList
                        ID="ddlState" runat="server" Width="15%">
                    </asp:DropDownList>
                    &nbsp;<asp:TextBox ID="txtZip" runat="server" Width="8%"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblTelephone" runat="server" ControlName="txtTelephone" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtTelephone" runat="server"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblWorkPhone" runat="server" ControlName="txtWorkPhone" Suffix=":" />
                    <asp:TextBox ID="txtWorkPhone" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblCellPhone" runat="server" ControlName="txtCellPhone" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtCellPhone" runat="server"></asp:TextBox>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblFax" runat="server" ControlName="txtFax" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtFax" runat="server"></asp:TextBox>
                </div>
            </fieldset>
        </div>

    </div>

    <div id="AltAddress" class="dnnClear">

        <div style="float:right;">
            <asp:LinkButton ID="cmdUpdateAltAddress" resourcekey="cmdUpdateDonorRecord" ValidationGroup="UserForm"
                OnClientClick="UseData();" runat="server" OnClick="cmdUpdate_Click"
                CssClass="dnnPrimaryAction"></asp:LinkButton>
        </div>

        <div class="dnnForm" id="form-alt-address">
            <fieldset>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblAltStreet" runat="server" ControlName="txtAltStreet" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtAltStreet" runat="server"></asp:TextBox>
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblAltCityStateZip" runat="server" ControlName="txtAltCity" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtAltCity" runat="server" Width="20%"></asp:TextBox>&nbsp;<asp:TextBox
                        ID="txtAltState" runat="server" Width="5%" />
                    
                    &nbsp;<asp:TextBox ID="txtAltZip" runat="server" Width="20%"></asp:TextBox>
                </div>


            </fieldset>
        </div>    
    
    </div>

    <div id="Notes" class="dnnClear">
    
      <div style="float:right;">
            <asp:LinkButton ID="cmdUpdateNotesSection" resourcekey="cmdUpdateDonorRecord" ValidationGroup="UserForm"
                OnClientClick="UseData();" runat="server" OnClick="cmdUpdate_Click"
                CssClass="dnnPrimaryAction"></asp:LinkButton>
        </div>

        <div class="dnnForm" id="form-wealthManagement">
            <fieldset>



                <div class="dnnFormItem">
                    <dnn:Label ID="lblComments" runat="server" ControlName="txtComments" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>


                <div class="dnnFormItem">
                    <dnn:Label ID="lblCapacityRating" runat="server" ControlName="txtCapacityRating" Suffix=":"></dnn:Label>
                    <asp:TextBox ID="txtCapacityRating" runat="server" />
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblInclinationRating" runat="server" ControlName="txtInclinationRating" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtInclinationRating" runat="server" />
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblStage" runat="server" ControlName="txtStage" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtStage" runat="server" />
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblProspectResearch" runat="server" ControlName="txtProspectResearch" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtProspectResearch" runat="server" />
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblProspectManager" runat="server" ControlName="txtProspectManager" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtProspectManager" runat="server" />
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblAge" runat="server" ControlName="txtAge" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtAge" runat="server" />
                </div>				
            </fieldset>
        </div>
    
    </div>

    <div id="History" class="dnnClear">
        <asp:GridView ID="GridViewDonations" runat="server" EnableModelValidation="False"
            DataKeyNames="DonationID" OnRowEditing="GridViewDonations_RowEditing" OnRowCommand="GridViewDonations_RowCommand" OnRowUpdating="GridViewDonations_RowUpdating"
            AutoGenerateColumns="False" GridLines="None" CssClass="dnnGrid" resourcekey="GridViewDonations" HorizontalAlign="Center">
            <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Center" />
            <RowStyle CssClass="dnnGridItem" />
            <AlternatingRowStyle CssClass="dnnGridAltItem" />
            <EditRowStyle CssClass="dnnFormInput" />
            <SelectedRowStyle CssClass="dnnFormError" />
            <FooterStyle CssClass="dnnGridFooter" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonEdit" CommandArgument='<%# Eval("DonationID") %>' CommandName="Edit"
                            runat="server">
                     Edit</asp:LinkButton>
                    </ItemTemplate>



                </asp:TemplateField>
                 <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonLetter" CommandArgument='<%# Eval("DonationID") %>' CommandName="Update" ToolTip="Generate a Thank You Letter/Email for this donation."
                            runat="server">
                     Thank You</asp:LinkButton>
                    </ItemTemplate>

                 </asp:TemplateField>


<asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" >
<ItemTemplate>

<asp:Image ID="Image1" runat="server" ToolTip="Previous Letter/Email Generated" ImageUrl='<%# (Eval("LetterGenerated").Equals(true) ? "~/DesktopModules/GIBS/DonationTracker/Images/yes.png" : "~/DesktopModules/GIBS/DonationTracker/Images/no.png")%>' />


</ItemTemplate>
</asp:TemplateField>


                <asp:BoundField HeaderText="Drive" DataField="DriveName" />
                <asp:BoundField HeaderText="Donation Date" DataField="DonationDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center">
                </asp:BoundField>

                <asp:TemplateField HeaderText="Pledge Date" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                    <asp:Label ID="myPledgeDate" runat="server" Text='<%# String.Format("{0:MM/dd/yyyy}", Eval("PledgeDate")) %>' Visible='<%# DataBinder.Eval(Container.DataItem, "PledgeDate").ToString() == "1/1/0001 12:00:00 AM" || DataBinder.Eval(Container.DataItem, "PledgeDate").ToString() == "1/1/1753 12:00:00 AM" || DataBinder.Eval(Container.DataItem, "PledgeDate").ToString() == "1/1/1900 12:00:00 AM" ? false : true %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                
                <asp:BoundField HeaderText="Amount" DataField="DonationAmount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                </asp:BoundField>
                <asp:BoundField HeaderText="Type" DataField="DonationType" Visible="false" />
                <asp:BoundField HeaderText="Notes" DataField="DonationNotes" />
                <asp:BoundField HeaderText="Followup" DataField="Followup" Visible="false" />
                <asp:BoundField HeaderText="Source" DataField="Source" Visible="true" />
                <asp:TemplateField HeaderText="Memory/Honor">
                    <ItemTemplate>
                    <asp:Label ID="myTypeOfDonation" runat="server" Text='<%# Eval("TypeOfDonation") + " " + Eval("DonationFrom") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Recorded By" DataField="CreatedByUserName" Visible="false" />
            </Columns>
        </asp:GridView>
    </div>
    <div id="AddDonation" class="dnnClear">
       
         <div style="float:right;">
            <asp:LinkButton CssClass="dnnPrimaryAction" ID="cmdUpdateDonation" 
                runat="server" Text="Add Donation" OnClick="cmdUpdateDonation_Click" CausesValidation="true"
                ValidationGroup="Donate" meta:resourcekey="cmdUpdateResource1"></asp:LinkButton><br />
            <asp:LinkButton CssClass="dnnSecondaryAction" ID="cmdDeleteDonation" resourcekey="cmdDeleteDonation"
                runat="server" Text="Delete" CausesValidation="False" OnClick="cmdDeleteDonation_Click" Visible="false"></asp:LinkButton>&nbsp;
        </div>      
       
        <div class="dnnForm" id="form-edit-donation">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDrive" runat="server" ControlName="ddlDrive" Suffix=":"></dnn:Label>
                    <asp:DropDownList ID="ddlDrive" runat="server" ValidationGroup="Donate">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlDrive" InitialValue="0" runat="server" 
                    ErrorMessage="Drive is Required!" ValidationGroup="Donate"></asp:RequiredFieldValidator>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDonationType" runat="server" ControlName="ddlDonationType" Suffix=":">
                    </dnn:Label>
                    <asp:DropDownList ID="ddlDonationType" runat="server">
                        <asp:ListItem Value="">-- Select --</asp:ListItem>
                        <asp:ListItem>Cash</asp:ListItem>
                        <asp:ListItem>Check</asp:ListItem>
                        <asp:ListItem>Credit Card</asp:ListItem>
                        <asp:ListItem>PayPal</asp:ListItem>
                        <asp:ListItem>Pledge</asp:ListItem>
                        <asp:ListItem>In-Kind</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDonationDate" runat="server" ControlName="txtDonationDate" Suffix=":"
                        CssClass="dnnFormLabel"></dnn:Label>
                    <asp:TextBox ID="txtDonationDate" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator runat="server" ID="reqDonationDate" ControlToValidate="txtDonationDate"
                        ErrorMessage="Please enter a donation date!" ValidationGroup="Donate" />
                </div>


 

                <div class="dnnFormItem">
                    <dnn:Label ID="lblDonationAmount" runat="server" ControlName="txtDonationAmount" Suffix=":" />
                    <asp:TextBox ID="txtDonationAmount" runat="server" ValidationGroup="Donate" />
                    <asp:RequiredFieldValidator runat="server" ID="reqDonationAmount" ControlToValidate="txtDonationAmount"
                        ValidationGroup="Donate" ErrorMessage="Please enter an amount!" Display="Dynamic" />
                </div>

                 <div class="dnnFormItem">
                    <dnn:Label ID="lblDonationSource" runat="server" ControlName="ddlDonationSource" Suffix=":" />
                    <asp:DropDownList ID="ddlDonationSource" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="reqDonationSource" ControlToValidate="ddlDonationSource" InitialValue=""
                        ValidationGroup="Donate" ErrorMessage="Please enter a source!" Display="Dynamic" />
                </div>	

                <div class="dnnFormItem">
                    <dnn:Label ID="lblTypeOfDonation" runat="server" ControlName="ddlTypeOfDonation" Suffix=":" />
                    <asp:DropDownList ID="ddlTypeOfDonation" runat="server">
                        <asp:ListItem Value="">-- Select --</asp:ListItem>
                        <asp:ListItem Value="Anonymous" Text="Anonymous" />
                        <asp:ListItem Value="In Memory Of" Text="In Memory Of" />
                        <asp:ListItem Value="In Honor Of" Text="In Honor Of" />

                    </asp:DropDownList>	
                    
                </div>
                <div class="dnnFormItem" id="DivDonationFromName">
                    <dnn:Label ID="lblDonationFromName" runat="server" ControlName="txtDonationFromName" Suffix=":"></dnn:Label>
                   <asp:TextBox ID="txtDonationFromName" runat="server" ValidationGroup="Donate" />
                </div>




                <div class="dnnFormItem">
                    <dnn:Label ID="lblFollowUp" runat="server" ControlName="cbxFollowup" Suffix=":"></dnn:Label>
                    <asp:CheckBox ID="cbxFollowup" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblDonationNotes" runat="server" ControlName="txtDonationNotes" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtDonationNotes" runat="server" Columns="30" Height="120px" TextMode="MultiLine"></asp:TextBox>
                </div>
            </fieldset>
        </div>
        <asp:HiddenField ID="DonationRecordID" runat="server" Value="" />
         <asp:HiddenField ID="hidDonationPledgeID" runat="server" Value="0" />	
        <asp:HiddenField ID="hidPledgeDate" runat="server" Value="" />
    </div>



    <div id="AddPledge" class="dnnClear">
       
 <div style="float:left;padding-left:30px;">
     <asp:Label ID="lblCurrentPledgeInfo" runat="server" Text="" />
 </div>

<asp:GridView ID="GridViewPledgeSchedule" runat="server" EnableModelValidation="False"
            DataKeyNames="ItemID" OnRowEditing="GridViewPledge_RowEditing" OnRowCommand="GridViewPledge_RowCommand" OnRowUpdating="GridViewPledge_RowUpdating"
            AutoGenerateColumns="False" GridLines="None" CssClass="dnnGrid" HorizontalAlign="Center">
            <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Center" />
            <RowStyle CssClass="dnnGridItem" />
            <AlternatingRowStyle CssClass="dnnGridAltItem" />
            <EditRowStyle CssClass="dnnFormInput" />
            <SelectedRowStyle CssClass="dnnFormError" />
            <FooterStyle CssClass="dnnGridFooter" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonEdit" CommandArgument='<%# Eval("PledgeID") %>' CommandName="Edit"
                            runat="server" Visible='<%# DataBinder.Eval(Container.DataItem, "DonationAmount").ToString() == "0" ? true : false %>'>
                     Record</asp:LinkButton><asp:HiddenField ID="HiddenField1" runat="server" 
                    Value='<%#Eval("PledgeID")%>'/>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField HeaderText="Drive" DataField="DriveName" />
                <asp:BoundField HeaderText="Pledge Date" DataField="PledgeDate" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Center" />
               
                <asp:BoundField HeaderText="Amount" DataField="DonationAmount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
              

            </Columns>
        </asp:GridView>


<br />&nbsp;

        <asp:GridView ID="GridViewPledges" runat="server" EnableModelValidation="False"
            DataKeyNames="PledgeID" OnRowEditing="GridViewPledges_RowEditing" OnRowCommand="GridViewPledges_RowCommand" OnRowUpdating="GridViewPledges_RowUpdating"
            AutoGenerateColumns="False" GridLines="None" CssClass="dnnGrid" HorizontalAlign="Center">
            <HeaderStyle CssClass="dnnGridHeader" HorizontalAlign="Center" />
            <RowStyle CssClass="dnnGridItem" />
            <AlternatingRowStyle CssClass="dnnGridAltItem" />
            <EditRowStyle CssClass="dnnFormInput" />
            <SelectedRowStyle CssClass="dnnFormError" />
            <FooterStyle CssClass="dnnGridFooter" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonEditPledge" CommandArgument='<%# Eval("PledgeID") %>' CommandName="Edit"
                            runat="server" ToolTip="Edit this Pledge">
                     Edit</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
				
                 <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonLoadPledge" CommandArgument='<%# Eval("PledgeID") %>' CommandName="Update"
                            runat="server" ToolTip="Load this Pledge above for recording">
                     Load</asp:LinkButton>
                    </ItemTemplate>
                 </asp:TemplateField>

                     <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonLetter" CommandArgument='<%# Eval("PledgeID") %>' CommandName="LoadLetter" ToolTip="Generate a Thank You Letter for this pledge"
                            runat="server">
                     Letter</asp:LinkButton>
                    </ItemTemplate>

                 </asp:TemplateField>
                  <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" >
<ItemTemplate>

<asp:Image ID="ImagePledgeLetter" runat="server" ToolTip="Previous Letter Generated" ImageUrl='<%# (Eval("LetterGenerated").Equals(true) ? "~/DesktopModules/GIBS/DonationTracker/Images/yes.png" : "~/DesktopModules/GIBS/DonationTracker/Images/no.png")%>' />


</ItemTemplate>
</asp:TemplateField>

                <asp:BoundField HeaderText="Drive" DataField="DriveName" />
                <asp:BoundField HeaderText="Start" DataField="StartDate" DataFormatString="{0:d}" />
				<asp:BoundField HeaderText="End" DataField="EndDate" DataFormatString="{0:d}" Visible="false" />
               
                <asp:BoundField HeaderText="Amount" DataField="PledgeAmount" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="Frequency">
                <ItemTemplate>
                    <asp:Label ID="lblFrequency" runat="server" Text='<%# GetFrequencyLookup(DataBinder.Eval(Container.DataItem, "Frequency").ToString()) %>'></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                
                 <asp:BoundField HeaderText="# Of Payments" DataField="NumberOfPayments" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField HeaderText="Notes" DataField="Notes" />
                <asp:BoundField HeaderText="Followup" DataField="Followup" Visible="false" />
                <asp:BoundField HeaderText="Recorded By" DataField="CreatedByUserName" Visible="false" />
                <asp:BoundField HeaderText="Last Update By" DataField="UpdatedByUserName" Visible="false" />
                <asp:BoundField HeaderText="Source" DataField="Source" Visible="true" />
                                <asp:TemplateField HeaderText="Memory/Honor">
                    <ItemTemplate>
                    <asp:Label ID="myTypeOfDonation" runat="server" Text='<%# Eval("TypeOfDonation") + " " + Eval("DonationFrom") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>




         <div style="float:right;">
            <asp:LinkButton CssClass="dnnPrimaryAction" ID="cmdUpdatePledge" 
                runat="server" Text="Add Pledge" OnClick="cmdUpdatePledge_Click" CausesValidation="true"
                ValidationGroup="Pledge" meta:resourcekey="cmdUpdatePledge"></asp:LinkButton><br />
            <asp:LinkButton CssClass="dnnSecondaryAction" ID="cmdDeletePledge" resourcekey="cmdDeletePledge"
                runat="server" Text="Delete" CausesValidation="False" OnClick="cmdDeletePledge_Click" Visible="false"></asp:LinkButton>&nbsp;
        </div>    
        <div class="dnnForm" id="form-edit-Pledge">
            <fieldset>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblPledgeDrive" runat="server" ControlName="ddlPledgeDrive" Suffix=":"></dnn:Label>
                    <asp:DropDownList ID="ddlPledgeDrive" runat="server" ValidationGroup="Pledge">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RFVddlPledgeDrive" ControlToValidate="ddlPledgeDrive" InitialValue="0" runat="server" 
                    ErrorMessage="Drive is Required!" ValidationGroup="Pledge"></asp:RequiredFieldValidator>
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblPledgeAmount" runat="server" ControlName="txtPledgeAmount" Suffix=":" />
                    <asp:TextBox ID="txtPledgeAmount" runat="server" ValidationGroup="Pledge" />
                    <asp:RequiredFieldValidator runat="server" ID="reqPledgeAmount" ControlToValidate="txtPledgeAmount"
                        ValidationGroup="Pledge" ErrorMessage="Please enter an amount!" Display="Dynamic" />
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblFrequency" runat="server" ControlName="ddlFrequency" Suffix=":">
                    </dnn:Label>
                    <asp:DropDownList ID="ddlFrequency" runat="server" onchange="CalculateDate();">
                        <asp:ListItem Value="0">-- Select --</asp:ListItem>
                        <asp:ListItem Value="m" Text="Monthly" />
                        <asp:ListItem Value="q" Text="Quarterly" />
						<asp:ListItem Value="s" Text="Semi-Annually" />
                        <asp:ListItem Value="a" Text="Annually" />

                    </asp:DropDownList><asp:RequiredFieldValidator ID="RFVddlFrequency" ControlToValidate="ddlFrequency" InitialValue="0" runat="server" 
                    ErrorMessage="Frequency is Required!" ValidationGroup="Pledge"></asp:RequiredFieldValidator>
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblPledgeDate" runat="server" ControlName="txtPledgeDate" Suffix=":"
                        CssClass="dnnFormLabel"></dnn:Label>
                    <asp:TextBox ID="txtPledgeDate" runat="server" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator runat="server" ID="reqPledgeDate" ControlToValidate="txtPledgeDate"
                        ErrorMessage="Please enter a pledge date!" Display="Dynamic" ValidationGroup="Pledge" />
                </div>

<script type="text/javascript" language="javascript">

   


    function CalculateDate() {
        var ControlName = document.getElementById('<%= ddlNumberOfPayments.ClientID %>');
        
 //       alert(ControlName.value);
        var x = ControlName.value;

   //     alert(x + ' x value');

       //  frequency 
        var e = document.getElementById('<%= ddlFrequency.ClientID %>');
        var frequency = e.options[e.selectedIndex].value;
  //      alert(frequency);

        switch (frequency) {
            case "m":
                x = (parseInt(x) * 1);
                break;
            case "q":
                x = (parseInt(x) * 3);
                break;
            case "s":
                x = (parseInt(x) * 6);
                break;
            case "a":
                x = (parseInt(x) * 12);
                break;

        }

  //      alert(x + ' x frequency value');


        var date = new Date($('#<%= txtPledgeDate.ClientID %>').val());
        date.setDate(date.getDate() + 1);

        var myNewDate = new Date(date).add(parseInt(x)).month();

     //   document.getElementById("txtPledgeDateEnd").readOnly = false;

        $("#txtPledgeDateEnd").datepicker("setDate", myNewDate);

    //    document.getElementById("txtPledgeDateEnd").readOnly = true;
    //    alert(myNewDate);


    } 


</script>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblNumberOfPayments" runat="server" ControlName="ddlNumberOfPayments" Suffix=":" CssClass="dnnFormLabel" />
                    <asp:DropDownList ID="ddlNumberOfPayments" runat="server"  onchange="CalculateDate();" >
                        <asp:ListItem Value="0">-- Select --</asp:ListItem>
                        <asp:ListItem Text="1" Value="1" />
                        <asp:ListItem Text="2" Value="2" />
                        <asp:ListItem Text="3" Value="3" />
                        <asp:ListItem Text="4" Value="4" />
                        <asp:ListItem Text="5" Value="5" />
                        <asp:ListItem Text="6" Value="6" />
                        <asp:ListItem Text="7" Value="7" />
                        <asp:ListItem Text="8" Value="8" />
                        <asp:ListItem Text="9" Value="9" />
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="11" Value="11" />
                        <asp:ListItem Text="12" Value="12" />
                    </asp:DropDownList><asp:RequiredFieldValidator ID="RFVddlNumberOfPayments" ControlToValidate="ddlNumberOfPayments" InitialValue="0" runat="server" 
                    ErrorMessage="Number of Payments is Required!" ValidationGroup="Pledge" />
                </div>

                <div class="dnnFormItem" style="display:none;">
                    <dnn:Label ID="lblPledgeDateEnd" runat="server" ControlName="txtPledgeDateEnd" Suffix=":"
                        CssClass="dnnFormLabel"></dnn:Label>
                    <asp:TextBox ID="txtPledgeDateEnd" runat="server" ClientIDMode="Static"  />
                    <asp:RequiredFieldValidator runat="server" ID="reqPledgeDateEnd" ControlToValidate="txtPledgeDateEnd"
                        ErrorMessage="Please enter an ending pledge date!" ValidationGroup="Pledge" Display="Dynamic" />
                </div>	


                <div class="dnnFormItem">
                    <dnn:Label ID="lblPledgeSource" runat="server" ControlName="ddlPledgeSource" Suffix=":" />
                    <asp:DropDownList ID="ddlPledgeSource" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="reqPledgeSource" ControlToValidate="ddlPledgeSource" InitialValue=""
                        ValidationGroup="Pledge" ErrorMessage="Please enter a source!" Display="Dynamic" />
                </div>	


                <div class="dnnFormItem">
                    <dnn:Label ID="lblPledgeTypeOfDonation" runat="server" ControlName="ddlPledgeTypeOfDonation" Suffix=":" />
                    <asp:DropDownList ID="ddlPledgeTypeOfDonation" runat="server">
                        <asp:ListItem Value="">-- Select --</asp:ListItem>
                        <asp:ListItem Value="Anonymous" Text="Anonymous" />
                        <asp:ListItem Value="In Memory Of" Text="In Memory Of" />
                        <asp:ListItem Value="In Honor Of" Text="In Honor Of" />

                    </asp:DropDownList>	
                    
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblPledgeDonationFromName" runat="server" ControlName="txtPledgeDonationFromName" Suffix=":" />
                   <asp:TextBox ID="txtPledgeDonationFromName" runat="server" ValidationGroup="Donate" />
                </div>


                <div class="dnnFormItem">
                    <dnn:Label ID="lblPledgeFollowUp" runat="server" ControlName="cbxPledgeFollowup" Suffix=":"></dnn:Label>
                    <asp:CheckBox ID="cbxPledgeFollowup" runat="server" />
                </div>
                <div class="dnnFormItem">
                    <dnn:Label ID="lblPledgeNotes" runat="server" ControlName="txtPledgeNotes" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtPledgeNotes" runat="server" Columns="30" Height="120px" TextMode="MultiLine"></asp:TextBox>
                </div>
            </fieldset>
        </div>
        <asp:HiddenField ID="hidPledgeID" runat="server" Value="" />

    </div>





    <div id="MailingLabels" class="dnnClear">


        <div class="dnnForm" id="form-mailing-labels">
            <fieldset>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblPrimaryAddress" runat="server" ControlName="txtPrimaryAddress" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtPrimaryAddress" runat="server" TextMode="MultiLine" Rows="5" ClientIDMode="Static" onClick="SelectAll('txtPrimaryAddress');" ></asp:TextBox>
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="lblAltAddress" runat="server" ControlName="txtAltAddress" Suffix=":">
                    </dnn:Label>
                    <asp:TextBox ID="txtAltAddress" runat="server" TextMode="MultiLine" Rows="5" ClientIDMode="Static" onClick="SelectAll('txtAltAddress');"></asp:TextBox>
                     <div style="text-align:center;">Click in the box and press <b>Ctrl-C</b> on your keyboard to copy to the Windows Clipboard. Use <b>Ctrl-V</b> to paste into Word.</div>
                </div>
               
            </fieldset>
        </div>    
    
    </div>

</div>

<div style="float:right; padding-top:10px;">
    <asp:LinkButton CssClass="dnnSecondaryAction" ID="cmdCancel" resourcekey="cmdCancel"
        runat="server" Text="Cancel" CausesValidation="False" OnClick="cmdCancel_Click"></asp:LinkButton>
</div>
