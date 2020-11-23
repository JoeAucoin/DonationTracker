using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

using GIBS.DonationTracker.Components;
using System.Collections;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Profile;

namespace GIBS.Modules.DonationTracker
{
    public partial class Settings : DonationTrackerSettings
    {

        /// <summary>
        /// handles the loading of the module setting for this
        /// control
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                if (!IsPostBack)
                {
                    GetRoles();
                    
                  //  DonationTrackerSettings settingsData = new DonationTrackerSettings(this.TabModuleId);

                 //   if (RoleName != null)
                    if (Settings.Contains("roleName"))
                        {
                        ddlRoles.SelectedValue = RoleName;
                    }
                    if (NumPerPage != null)
                    {
                        ddlNumPerPage.SelectedValue = NumPerPage;
                    }
                    if (EmailMessage != null)
                    {
                        txtEmailMessage.Text = EmailMessage;
                    }
                    if (EmailFrom != null)
                    {
                        txtEmailFrom.Text = EmailFrom;
                    }

                    if (EmailBCC != null)
                    {
                        txtEmailBCC.Text = EmailBCC;
                    }

                    if (EmailSubject != null)
                    {
                        txtEmailSubject.Text = EmailSubject;
                    }

                    if (ReportsRole != null || ReportsRole.Length > 0)
                    {
                        ddlReportsRoles.SelectedValue = ReportsRole;
                    }
                    
                    if (MergeRole != null)
                    {
                        ddlMergeRoles.SelectedValue = MergeRole;
                    }


                    if (Settings.Contains("showSendPassword"))
                    {
                        cbxShowSendPassword.Checked = Convert.ToBoolean(ShowSendPassword);
                    }

                    if (Settings.Contains("emailNewUserCredentials"))
                    {
                        cbxEmailNewUserCredentials.Checked = Convert.ToBoolean(EmailNewUserCredentials);
                    }

                    if (Settings.Contains("showDonationHistory"))
                    {
                        cbxShowDonationHistory.Checked = Convert.ToBoolean(ShowDonationHistory);
                    }

                    if (Settings.Contains("enableAddNewDonor"))
                    {
                        cbxEnableAddNewDonor.Checked = Convert.ToBoolean(EnableAddNewDonor);
                    }


                    if (ReportServerURL != null)
                    {
                        txturlReportServer.Text = ReportServerURL.ToString();
                    }

                    if (ReportPath != null)
                    {
                        txtReportPath.Text = ReportPath.ToString();
                    }

                    if (ReportCredentialsUserName != null)
                    {
                        txtRSCredentialsUserName.Text = ReportCredentialsUserName.ToString();
                    }

                    if (ReportCredentialsPassword != null)
                    {

                        txtRSCredentialsPassword.Text = ReportCredentialsPassword.ToString();
                    }
                    if (ReportCredentialsDomain != null)
                    {
                        txtRSCredentialsDomain.Text = ReportCredentialsDomain.ToString();
                    }

                    txtProfileCheckAnonymous.Text = CheckProfilePropertyExistsTrueFalse("Anonymous").ToString();
                    txtProfileCheckDoNotMail.Text = CheckProfilePropertyExistsTrueFalse("DoNotMail").ToString();

                    txtProfileCheckStreet2.Text = CheckProfilePropertyExistsText("Street2").ToString();

                    txtProfileCheckWorkPhone.Text = CheckProfilePropertyExistsText("WorkPhone").ToString();
                    txtProfileCheckAdditionalName.Text = CheckProfilePropertyExistsText("AdditionalName").ToString();
                    txtProfileCheckAdditionalFirstName.Text = CheckProfilePropertyExistsText("AdditionalFirstName").ToString();
                    txtProfileCheckAdditionalMI.Text = CheckProfilePropertyExistsText("AdditionalMI").ToString();
                    txtProfileCheckCapacityRating.Text = CheckProfilePropertyExistsText("CapacityRating").ToString();
                    txtProfileCheckInclinationRating.Text = CheckProfilePropertyExistsText("InclinationRating").ToString();
                    txtProfileCheckStage.Text = CheckProfilePropertyExistsText("Stage").ToString();
                    txtProfileCheckProspectResearch.Text = CheckProfilePropertyExistsText("ProspectResearch").ToString();
                    txtProfileCheckProspectManager.Text = CheckProfilePropertyExistsText("ProspectManager").ToString();
                    txtProfileCheckAge.Text = CheckProfilePropertyExistsText("Age").ToString();
                    txtProfileCheckComments.Text = CheckProfilePropertyExistsMultiLineText("Comments").ToString();

                    txtProfileCheckCompany.Text = CheckProfilePropertyExistsText("Company").ToString();
                    txtProfileCheckAltCity.Text = CheckProfilePropertyExistsText("AltCity").ToString();
                    txtProfileCheckAltPostalCode.Text = CheckProfilePropertyExistsText("AltPostalCode").ToString();
                    txtProfileCheckAltState.Text = CheckProfilePropertyExistsText("AltState").ToString();
                    txtProfileCheckAltStreet.Text = CheckProfilePropertyExistsText("AltStreet").ToString();
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void GetRoles()
        {
            DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();

            var myRoles = rc.GetRoles(this.PortalId);
            //  myRoles
            ddlRoles.DataSource = myRoles;
            ddlRoles.DataTextField = "RoleName";
            ddlRoles.DataValueField = "RoleName";
            ddlRoles.DataBind();

            // ADD FIRST (NULL) ITEM
            ListItem item = new ListItem();
            item.Text = "-- Select Role to Assign --";
            item.Value = "";
            ddlRoles.Items.Insert(0, item);
            // REMOVE DEFAULT ROLES
            ddlRoles.Items.Remove("Administrators");
            ddlRoles.Items.Remove("Registered Users");
            ddlRoles.Items.Remove("Subscribers");

            // REPORTS ROLE
            ddlReportsRoles.DataSource = myRoles;
            ddlReportsRoles.DataBind();

            // ADD FIRST (NULL) ITEM
            ListItem item1 = new ListItem();
            item1.Text = "-- Select Role to View Reports --";
            item1.Value = "";
            ddlReportsRoles.Items.Insert(0, item1);
            // REMOVE DEFAULT ROLES
            ddlReportsRoles.Items.Remove("Administrators");
            ddlReportsRoles.Items.Remove("Registered Users");
            ddlReportsRoles.Items.Remove("Subscribers");

            // MERGE ROLE

            ddlMergeRoles.DataSource = myRoles;
            ddlMergeRoles.DataBind();

            // ADD FIRST (NULL) ITEM
            item1.Value = "Select Role to Allow Merge";
            ddlMergeRoles.Items.Insert(0, item1);
            // REMOVE DEFAULT ROLES
            ddlMergeRoles.Items.Remove("Administrators");
            ddlMergeRoles.Items.Remove("Registered Users");
            ddlMergeRoles.Items.Remove("Subscribers");

        }

        public string CheckProfilePropertyExistsText(string propertyName)
        {
            string value = null;

            ProfilePropertyDefinition ppd = ProfileController.GetPropertyDefinitionByName(this.PortalId, propertyName.ToString());
            //  ProfileController.AddPropertyDefinition()
            if (ppd == null)
            {
                // IT DOESN'T EXIST - -  CREATE IT

                DotNetNuke.Common.Lists.ListController objListCtrl = new DotNetNuke.Common.Lists.ListController();

                DotNetNuke.Entities.Profile.ProfilePropertyDefinition objDef = new DotNetNuke.Entities.Profile.ProfilePropertyDefinition();
                DotNetNuke.Entities.Profile.ProfileController objProfileController = new DotNetNuke.Entities.Profile.ProfileController();

                objDef.DataType = objListCtrl.GetListEntryInfo("DataType", "Text").EntryID;
                objDef.Length = 50;
                objDef.PortalId = this.PortalId;
                objDef.PropertyName = propertyName.ToString(); // This is your property Name
                objDef.Required = false;
                objDef.DefaultValue = "";
                objDef.ViewOrder = -1;
                objDef.DefaultVisibility = DotNetNuke.Entities.Users.UserVisibilityMode.AdminOnly;
                objDef.Visible = true;
                objDef.PropertyCategory = "Donor";
                objDef.ReadOnly = false;

                DotNetNuke.Entities.Profile.ProfileController.AddPropertyDefinition(objDef);

                value = "Profile Property Created for " + propertyName.ToString() + "!";
            }
            else
            {
                value = propertyName.ToString() + " Profile Property Exists!";

            }
            return value;
        }

        public string CheckProfilePropertyExistsMultiLineText(string propertyName)
        {
            string value = null;

            ProfilePropertyDefinition ppd = ProfileController.GetPropertyDefinitionByName(this.PortalId, propertyName.ToString());
            //  ProfileController.AddPropertyDefinition()
            if (ppd == null)
            {
                // IT DOESN'T EXIST - -  CREATE IT

                DotNetNuke.Common.Lists.ListController objListCtrl = new DotNetNuke.Common.Lists.ListController();

                DotNetNuke.Entities.Profile.ProfilePropertyDefinition objDef = new DotNetNuke.Entities.Profile.ProfilePropertyDefinition();
                DotNetNuke.Entities.Profile.ProfileController objProfileController = new DotNetNuke.Entities.Profile.ProfileController();

                objDef.DataType = objListCtrl.GetListEntryInfo("DataType", "Multi-line Text").EntryID;
                objDef.Length = 0;
                objDef.PortalId = this.PortalId;
                objDef.PropertyName = propertyName.ToString(); // This is your property Name
                objDef.Required = false;
                objDef.DefaultValue = "";
                objDef.ViewOrder = -1;
                objDef.DefaultVisibility = DotNetNuke.Entities.Users.UserVisibilityMode.AdminOnly;
                objDef.Visible = true;
                objDef.PropertyCategory = "Donor";
                objDef.ReadOnly = false;

                DotNetNuke.Entities.Profile.ProfileController.AddPropertyDefinition(objDef);

                value = "Profile Property Created for " + propertyName.ToString() + "!";
            }
            else
            {
                value = propertyName.ToString() + " Profile Property Exists!";

            }
            return value;
        }

        public string CheckProfilePropertyExistsTrueFalse(string propertyName)
        {
            string value = null;

            ProfilePropertyDefinition ppd = ProfileController.GetPropertyDefinitionByName(this.PortalId, propertyName.ToString());
         //  ProfileController.AddPropertyDefinition()
            if (ppd == null)
            {
                // IT DOESN'T EXIST - -  CREATE IT

                DotNetNuke.Common.Lists.ListController objListCtrl = new DotNetNuke.Common.Lists.ListController();

                DotNetNuke.Entities.Profile.ProfilePropertyDefinition objDef = new DotNetNuke.Entities.Profile.ProfilePropertyDefinition();
                DotNetNuke.Entities.Profile.ProfileController objProfileController = new DotNetNuke.Entities.Profile.ProfileController();

                objDef.DataType = objListCtrl.GetListEntryInfo("DataType", "TrueFalse").EntryID;
                objDef.Length = 50;
                objDef.PortalId = this.PortalId;
                objDef.PropertyName = propertyName.ToString(); // This is your property Name
                objDef.Required = false;
                objDef.DefaultValue = "false";
                objDef.ViewOrder = -1;
                objDef.DefaultVisibility = DotNetNuke.Entities.Users.UserVisibilityMode.AdminOnly;
                objDef.Visible = true;
                objDef.PropertyCategory = "Donor";
                objDef.ReadOnly = false;
                
                DotNetNuke.Entities.Profile.ProfileController.AddPropertyDefinition(objDef);

                value = "Profile Property Created for " + propertyName.ToString() + "!";
            }
            else
            {
                value = propertyName.ToString() +  " Profile Property Exists!";

            }
            return value;
        }


        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings()
        {
            try
            {
            
                NumPerPage = ddlNumPerPage.SelectedValue;
                RoleName = ddlRoles.SelectedValue;
                EmailMessage = txtEmailMessage.Text;
                EmailFrom = txtEmailFrom.Text;
                EmailSubject = txtEmailSubject.Text;
                EmailBCC = txtEmailBCC.Text;
                ReportsRole = ddlReportsRoles.SelectedValue;
                MergeRole = ddlMergeRoles.SelectedValue;
                ShowSendPassword = cbxShowSendPassword.Checked.ToString();
                EmailNewUserCredentials = cbxEmailNewUserCredentials.Checked.ToString();
                ShowDonationHistory = cbxShowDonationHistory.Checked.ToString();
                EnableAddNewDonor = cbxEnableAddNewDonor.Checked.ToString();

                ReportCredentialsDomain = txtRSCredentialsDomain.Text.ToString();
                ReportCredentialsPassword = txtRSCredentialsPassword.Text.ToString();
                ReportCredentialsUserName = txtRSCredentialsUserName.Text.ToString();
                ReportPath = txtReportPath.Text.ToString();
                ReportServerURL = txturlReportServer.Text.ToString();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}