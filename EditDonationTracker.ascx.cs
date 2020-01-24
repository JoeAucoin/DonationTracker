using System;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using GIBS.DonationTracker.Components;
using DotNetNuke.Common.Lists;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Globalization;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Framework.JavaScriptLibraries;
using System.Web;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;





namespace GIBS.Modules.DonationTracker
{
    public partial class EditDonationTracker : PortalModuleBase, IActionable
    {

        private GridViewHelper helper;
        // To show custom operations...
        private List<int> mQuantities = new List<int>();

        protected string RoleName = "Registered Users";
        int DonationID = Null.NullInteger;
        int DonationUserId = Null.NullInteger;
        public int DonorPortal;
        public bool _EmailNewUserCredentials = false;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);
            Page.ClientScript.RegisterClientScriptInclude("MyDateJS", "https://storage.googleapis.com/google-code-archive-downloads/v2/code.google.com/datejs/date.js");
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "InputMasks", (this.TemplateSourceDirectory + "/JavaScript/jquery.maskedinput-1.3.js"));
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "Watermark", (this.TemplateSourceDirectory + "/JavaScript/jquery.watermarkinput.js"));
            
       //     Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "Style", ("https://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/redmond/jquery-ui.css"));

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                
                
                DonorPortal = this.PortalId;
                
                LoadSettings();
                

                if (Request.QueryString["DonationID"] != null)
                {
                    DonationID = Int32.Parse(Request.QueryString["DonationID"]);
                }







                if (!IsPostBack)
                {

                    txtDonationDate.Text = DateTime.Today.ToShortDateString();

                    if (Request.QueryString["TabFocus"] != null)
                    {
                        HttpCookie myCookie = new HttpCookie("dnnTabs-tabs-donors");
                        myCookie.HttpOnly = false;
                        switch (Request.QueryString["TabFocus"])
                        {
                            case "DonorRecord":
                                myCookie.Value = "";
                                break;

                            case "History":
                                myCookie.Value = "3";
                                break;

                            case "AddDonation":
                                myCookie.Value = "4";
                                break;
                            case "AddPledge":
                                myCookie.Value = "5";
                                break;

                            default:
                                myCookie.Value = "";
                                break;
                        }
                        Response.Cookies.Add(myCookie);
                    }


                    //load the data into the control the first time
                    //we hit this page
                    CreatePrefixSuffixDropdowns();
                    GetStates();
                    FillDropDown();



                    if (Request.QueryString["UserId"] != null)
                    {
                        DonationUserId = Int32.Parse(Request.QueryString["UserId"]);
                        hidDonationUserId.Value = DonationUserId.ToString();
                        LoadRecord(DonationUserId);
                        GroupIt();
                        FillDonationGridAndPledgeGrid();
                        AddTotalToPledgeGrid();
                        FillPledgeGrid();
                    }
                    else
                    {
                        this.ModuleConfiguration.ModuleControl.ControlTitle = "Add New Record";
                        LinkButtonMerge.Visible = false;
                    }


                    if (Request.QueryString["Status"] != null)
                    {
                        if (Request.QueryString["Status"] == "Success")
                        {
                            //AddNewDonationRecord();
                            // SET FOCUS TO ADD DONATION TAB
                            lblStatus.Text = "Record Successfully Inserted";
                        }
                    }

                    cmdDeleteDonation.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");




                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void LinkButtonMerge_Click(object sender, EventArgs e)
        {

            DonationTrackerController controller = new DonationTrackerController();

            DonationTrackerInfo item = new DonationTrackerInfo();

            if (DonationUserId.ToString().Length > 0)
            {
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "DonationMerge", "mid=" + ModuleId.ToString() + "&userid=" + hidDonationUserId.Value.ToString()));

                //item.ClientID = Int32.Parse(hidClientID.Value.ToString());

                //controller.DeleteFBClients(item.ClientID);
                // controller.FBClients_Update(item);

            }

        }

        public void AddTotalToPledgeGrid()
        {

            try
            {

                helper = new GridViewHelper(this.GridViewPledgeSchedule);



                helper.RegisterSummary("DonationAmount", SummaryOperation.Sum);

                helper.GeneralSummary += new FooterEvent(helper_GeneralSummary);


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public void GroupIt()
        {

            try
            {

                helper = new GridViewHelper(this.GridViewDonations);
               


                helper.RegisterSummary("DonationAmount", SummaryOperation.Sum);

                helper.GeneralSummary += new FooterEvent(helper_GeneralSummary);


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        void helper_GeneralSummary(GridViewRow row)
        {

            row.Cells[0].HorizontalAlign = HorizontalAlign.Right;

            row.Cells[0].Text = "Grand Total: ";
            row.BackColor = Color.BlanchedAlmond;
            row.ForeColor = Color.Black;

        }

        private void SaveQuantity(string column, string group, object value)
        {
            mQuantities.Add(Convert.ToInt32(value));
        }

        private object GetMinQuantity(string column, string group)
        {
            int[] qArray = new int[mQuantities.Count];
            mQuantities.CopyTo(qArray);
            Array.Sort(qArray);
            return qArray[0];
        }




        public void LoadSettings()
        {
            try
            {
                DonationTrackerSettings settingsData = new DonationTrackerSettings(this.TabModuleId);

                if (settingsData.RoleName != null)
                {
                    RoleName = settingsData.RoleName;
                }
                if (settingsData.ShowSendPassword != null)
                {
                    if (Convert.ToBoolean(settingsData.ShowSendPassword) == true)
                    {
                        cmdSendCredentials.Visible = true;
                    }
                    else
                    {
                        cmdSendCredentials.Visible = false;
                    }
                
                }

                if (settingsData.ShowDonationHistory != null)
                {
                    if (Convert.ToBoolean(settingsData.ShowDonationHistory) == true)
                    {
                        GridViewDonations.Visible = true;
                    }
                    else
                    {
                        GridViewDonations.Visible = false;
                        
                    }

                }
                //else
                //{
                //    GridViewDonations.Visible = false;
                //}

                if (settingsData.ReportsRole != null)
                {
                    string _ReportsRole = settingsData.ReportsRole;
                    var roleGroup = UserInfo.IsInRole(_ReportsRole);

                    if (roleGroup == true)
                    {
                        GridViewDonations.Visible = true;
                    }
                    else
                    {
                        GridViewDonations.Visible = false;
                    }

                }


                if (settingsData.MergeRole != null)
                {
                    string _ReportsRole = settingsData.MergeRole;
                    var roleGroup = UserInfo.IsInRole(_ReportsRole);

                    if (roleGroup == true)
                    {
                        LinkButtonMerge.Visible = true;
                        
                    }
                    else
                    {
                        LinkButtonMerge.Visible = false;

                    }

                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        //public void SetPageName(string PageName)
        //{
        //    try
        //    {
        //        Page.Title = PageName.ToString();
        //        this.ModuleConfiguration.ModuleTitle = PageName.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.ProcessModuleLoadException(this, ex);
        //    }
        //}

        public void LoadRecord(int RecordID)
        {
            try
            {

                DotNetNuke.Entities.Users.UserInfo DonationUser = DotNetNuke.Entities.Users.UserController.GetUserById(DonorPortal, RecordID);

              //  SetPageName(DonationUser.Profile.GetPropertyValue("Company") + " - " + DonationUser.FirstName + " " + DonationUser.LastName);

                LabelDonorName.Text = DonationUser.Profile.GetPropertyValue("Company") + " - " + DonationUser.FirstName + " " + DonationUser.LastName;

          //      this.ModuleConfiguration.ModuleControl.ControlTitle = DonationUser.Profile.GetPropertyValue("Company") + " - " + DonationUser.DisplayName;

                cbxDoNotMail.Checked = Convert.ToBoolean(DonationUser.Profile.GetPropertyValue("DoNotMail"));
                cbxAnonymous.Checked = Convert.ToBoolean(DonationUser.Profile.GetPropertyValue("Anonymous"));
                if (Convert.ToBoolean(DonationUser.Profile.GetPropertyValue("DoNotMail")))
                {
                    ImageDoNotMail.Visible = true;
                 //   lblStatus.Text = "<IMG SRC='" + this.TemplateSourceDirectory.ToString() + "/images/No-mail.png' alt='Do Not Mail' />";
                }

                if (Convert.ToBoolean(DonationUser.Profile.GetPropertyValue("Anonymous")))
                {
                    ddlTypeOfDonation.SelectedValue = "Anonymous";
                }

                    txtFirstName.Text = DonationUser.FirstName;
                txtLastName.Text = DonationUser.LastName;
                txtMiddleName.Text = DonationUser.Profile.GetPropertyValue("MiddleName");
                txtCompany.Text = DonationUser.Profile.GetPropertyValue("Company");
                
                txtAdditionalName.Text = DonationUser.Profile.GetPropertyValue("AdditionalName");
                txtAdditionalFirstName.Text = DonationUser.Profile.GetPropertyValue("AdditionalFirstName");
                txtAdditionalMI.Text = DonationUser.Profile.GetPropertyValue("AdditionalMI");

                ListItem LIdonortype = ddlDonorType.Items.FindByValue(DonationUser.Profile.GetPropertyValue("DonorType"));
                if (LIdonortype != null)
                {
                    // value found
                    ddlDonorType.SelectedValue = DonationUser.Profile.GetPropertyValue("DonorType");
                }
                else
                {
                    //Value not found
                }
                

                txtCapacityRating.Text = DonationUser.Profile.GetPropertyValue("CapacityRating");
                txtInclinationRating.Text = DonationUser.Profile.GetPropertyValue("InclinationRating");
                txtStage.Text = DonationUser.Profile.GetPropertyValue("Stage");
                txtProspectResearch.Text = DonationUser.Profile.GetPropertyValue("ProspectResearch");
                txtProspectManager.Text = DonationUser.Profile.GetPropertyValue("ProspectManager");
                txtAge.Text = DonationUser.Profile.GetPropertyValue("Age");


                txtEmail.Text = DonationUser.Email;

                ListItem litown = ddlPrefix.Items.FindByValue(DonationUser.Profile.GetPropertyValue("Prefix"));
                if (litown != null)
                {
                    // value found
                    ddlPrefix.SelectedValue = DonationUser.Profile.GetPropertyValue("Prefix");
                }
                else
                {
                    //Value not found
                    //   ddlTown.SelectedValue = item.ClientTown;
                }

                
                txtStreet.Text = DonationUser.Profile.Street;
                txtStreet2.Text = DonationUser.Profile.GetPropertyValue("Street2");
                txtCity.Text = DonationUser.Profile.City;
                txtWorkPhone.Text = DonationUser.Profile.GetPropertyValue("WorkPhone");
                txtTelephone.Text = DonationUser.Profile.Telephone;
                txtCellPhone.Text = DonationUser.Profile.Cell;
                txtFax.Text = DonationUser.Profile.Fax;
                txtZip.Text = DonationUser.Profile.PostalCode;
                
             //   ddlState.SelectedValue = DonationUser.Profile.Region;
           //     lblDebug.Text = DonationUser.Profile.Region;
             
                ListItem liregion = ddlState.Items.FindByText(DonationUser.Profile.Region);      //.FindByValue(DonationUser.Profile.Region);
                if (liregion != null)
                {
                    // value found
                    ddlState.SelectedValue = ddlState.Items.FindByText(DonationUser.Profile.Region).Value;    //DonationUser.Profile.Region;
                }
                else
                {
                    //Value not found
                    //   ddlTown.SelectedValue = item.ClientTown;
                    lblStatus.Text += "<br />STATE NOT FOUND";
                }


                ddlSuffix.SelectedValue = DonationUser.Profile.GetPropertyValue("Suffix");
         //       txtNotes.Text = DonationUser.Profile.GetPropertyValue("Biography");
                txtAltStreet.Text = DonationUser.Profile.GetPropertyValue("AltStreet");
                txtAltCity.Text = DonationUser.Profile.GetPropertyValue("AltCity");
                txtAltState.Text = DonationUser.Profile.GetPropertyValue("AltState");
                txtAltZip.Text = DonationUser.Profile.GetPropertyValue("AltPostalCode");
                txtComments.Text = DonationUser.Profile.GetPropertyValue("Comments");



                string _PrimaryAddress = DonationUser.Profile.GetPropertyValue("Prefix") + " " 
                    + DonationUser.FirstName + " "
                    + DonationUser.Profile.GetPropertyValue("MiddleName") + " " 
                    + DonationUser.LastName + " "
                    + DonationUser.Profile.GetPropertyValue("Suffix") + Environment.NewLine
                    + DonationUser.Profile.GetPropertyValue("AdditionalName") + Environment.NewLine
                    + DonationUser.Profile.GetPropertyValue("Company") + Environment.NewLine
                    + DonationUser.Profile.Street + Environment.NewLine
                    + DonationUser.Profile.GetPropertyValue("Street2") + Environment.NewLine
                    + DonationUser.Profile.City + ", "
                    + DonationUser.Profile.Region + " " 
                    + DonationUser.Profile.PostalCode;
             //   REMOVE EMPTY LINES
                _PrimaryAddress = Regex.Replace(_PrimaryAddress.ToString(), @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                
                txtPrimaryAddress.Text = _PrimaryAddress.ToString().TrimStart().TrimEnd().Replace("  ", " ");

                string _AltAddress = DonationUser.Profile.GetPropertyValue("Prefix") + " "
                    + DonationUser.FirstName + " "
                    + DonationUser.Profile.GetPropertyValue("MiddleName") + " "
                    + DonationUser.LastName + " "
                    + DonationUser.Profile.GetPropertyValue("Suffix") + Environment.NewLine
                    + DonationUser.Profile.GetPropertyValue("AdditionalName") + Environment.NewLine
                    + DonationUser.Profile.GetPropertyValue("Company") + Environment.NewLine
                    + DonationUser.Profile.GetPropertyValue("AltStreet") + Environment.NewLine
                    + DonationUser.Profile.GetPropertyValue("AltCity") + ", "
                    + DonationUser.Profile.GetPropertyValue("AltState") + " "
                    + DonationUser.Profile.GetPropertyValue("AltPostalCode");
                //   REMOVE EMPTY LINES
                _AltAddress = Regex.Replace(_AltAddress.ToString(), @"^\s+$[\r\n]*", "", RegexOptions.Multiline);

                txtAltAddress.Text = _AltAddress.ToString().TrimStart().TrimEnd().Replace("  ", " ");
                

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void FillDonationGridAndPledgeGrid()
        {

            try
            {

                DonationUserId = Int32.Parse(Request.QueryString["UserId"]);


                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();

                items = controller.DonationTrackerGetUserDonations(this.ModuleId, DonationUserId);

                GridViewDonations.DataSource = items;
                GridViewDonations.DataBind();

                List<DonationTrackerInfo> pledgeitems;
                pledgeitems = controller.DonationTrackerPledgeGetPledgesByUserID(this.ModuleId, DonationUserId);
                GridViewPledges.DataSource = pledgeitems;
                GridViewPledges.DataBind();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public void FillPledgeGridByPledgeID(int pledgeID)
        {

            try
            {

             

                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();

                items = controller.DonationTrackerPledge_GetPledgeScheduleByPledgeID(pledgeID);

                //   items.Find(

                GridViewPledgeSchedule.DataSource = items;
                GridViewPledgeSchedule.DataBind();

                if (items.Count > 0)
                {

                    GetCurrentPledgeInfo(pledgeID);
                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }	

        public void FillPledgeGrid()
        {

            try
            {

                DonationUserId = Int32.Parse(Request.QueryString["UserId"]);


                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();

                items = controller.DonationTrackerPledge_GetPledgeScheduleByUserID(DonationUserId);

             //   items.Find(

                GridViewPledgeSchedule.DataSource = items;
                GridViewPledgeSchedule.DataBind();

                if (items.Count > 0)
                {
                    // GET THE PLEDGEID TO LOOKUP DETAILS
                    HiddenField field = GridViewPledgeSchedule.Rows[0].FindControl("HiddenField1") as HiddenField;
                    int pledgeID = Int32.Parse(field.Value.ToString());

                    GetCurrentPledgeInfo(pledgeID);
                }
                
                

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public void GetCurrentPledgeInfo(int _pledgeID)
        {

            try
            {
                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = controller.DonationTrackerPledgeGetPledgeByID(_pledgeID);

                if (item != null)
                {

                    //      double.Parse((GetNumberOfMonths(item.Frequency) * item.PledgeAmount).ToString(), NumberStyles.Currency);
                    string _frequencyAmount = item.PledgeAmount.ToString("C2");
                    string _totalPledgeAmount =((MonthDifference(item.EndDate, item.StartDate) / GetNumberOfMonths(item.Frequency)) * item.PledgeAmount).ToString("C2");
                        //(GetNumberOfMonths(item.Frequency) * item.PledgeAmount).ToString("C2");

                    string myPledgeContent = "<b>" + item.DriveName + "</b><br />"
                        + "Starting: " + item.StartDate.ToShortDateString() + "<br />"
                     //   + "Ending: " + item.EndDate.ToShortDateString() + "<br />"
                        + "Frequency: " + _frequencyAmount.ToString() + " " + GetFrequencyLookup(item.Frequency) + "<br />"
                        + "Repeat: " + item.NumberOfPayments.ToString() + " Times<br />"
                        + "Total Amount Pledged: " + _totalPledgeAmount.ToString();

                    lblCurrentPledgeInfo.Text = myPledgeContent.ToString();
                    //lblCurrentPledgeInfo.Text +=  "<br />MonthDiff:" + MonthDifference(item.EndDate, item.StartDate).ToString();
                    //lblCurrentPledgeInfo.Text += "<br />GetNumberOfMonths:" + GetNumberOfMonths(item.Frequency).ToString();

                }
                else
                {
                    lblStatus.Text = "Error on loading pledge info.";
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }

        public int GetNumberOfMonths(string _frequency)
        {

            try
            {
                int _numMonths;

                switch (_frequency)
                {
                    case "m":
                        _numMonths = 1;
                        break;
                    case "q":
                        _numMonths = 3;
                        break;
                    case "s":
                        _numMonths = 6;
                        break;
                    case "a":
                        _numMonths = 12;
                        break;

                    default:
                        _numMonths = 1;
                        break;
                }

                return _numMonths;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return 0;
            }

        }

        public string GetFrequencyLookup(string _frequency)
        {

            try
            {
                string _numMonths = "";

                switch (_frequency)
                {
                    case "m":
                        _numMonths = "Monthly";
                        break;
                    case "q":
                        _numMonths = "Quarterly";
                        break;
                    case "s":
                        _numMonths = "Semi-Annually";
                        break;
                    case "a":
                        _numMonths = "Annually";
                        break;

                    default:
                        _numMonths = "Unknown";
                        break;
                }

                return _numMonths;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return "ERROR on GetFrequencyLookup";
            }

        }	

        private void CreatePrefixSuffixDropdowns()
        {
            string prefix = Localization.GetString("Prefix.Text", this.LocalResourceFile);

            //    filters += "," + Localization.GetString("OnLine"); 
            //   filters += "," + Localization.GetString("Unauthorized"); 
            string[] strPrefix = prefix.Split(',');
            ddlPrefix.DataSource = strPrefix;
            ddlPrefix.DataBind();

            //string suffix = Localization.GetString("Suffix.Text", this.LocalResourceFile);

            ////    filters += "," + Localization.GetString("OnLine"); 
            ////   filters += "," + Localization.GetString("Unauthorized"); 
            //string[] strSuffix = suffix.Split(',');
            //ddlSuffix.DataSource = strSuffix;
            //ddlSuffix.DataBind();

            //DonorType
            var DonorType = new ListController().GetListEntryInfoItems("DonorType", "", this.PortalId);

            ddlDonorType.DataTextField = "Text";
            ddlDonorType.DataValueField = "Value";
            ddlDonorType.DataSource = DonorType;
            ddlDonorType.DataBind();
            ddlDonorType.Items.Insert(0, new ListItem("--", ""));

            //_DonationSource
            var _DonationSource = new ListController().GetListEntryInfoItems("DonationSource", "", this.PortalId);

            ddlDonationSource.DataTextField = "Text";
            ddlDonationSource.DataValueField = "Value";
            ddlDonationSource.DataSource = _DonationSource;
            ddlDonationSource.DataBind();
            ddlDonationSource.Items.Insert(0, new ListItem("--", ""));

            ddlPledgeSource.DataTextField = "Text";
            ddlPledgeSource.DataValueField = "Value";
            ddlPledgeSource.DataSource = _DonationSource;
            ddlPledgeSource.DataBind();
            ddlPledgeSource.Items.Insert(0, new ListItem("--", ""));	


        }


        public void FillDropDown()
        {

            try
            {
               // CreatePrefixSuffixDropdowns();

                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();


                items = controller.DonationTrackerGetDrives(this.ModuleId, 1);

                ddlDrive.DataTextField = "DriveName";
                ddlDrive.DataValueField = "DriveID";
                ddlDrive.DataSource = items;
                ddlDrive.DataBind();
                ddlDrive.Items.Insert(0, new ListItem("-- Select --", "0"));


                // PLEDGE

                ddlPledgeDrive.DataTextField = "DriveName";
                ddlPledgeDrive.DataValueField = "DriveID";
                ddlPledgeDrive.DataSource = items;
                ddlPledgeDrive.DataBind();
                ddlPledgeDrive.Items.Insert(0, new ListItem("-- Select --", "0"));

                //bind the data
                // GridView1.DataSource = items;
                //  GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public void UpdateRecord(int RecordID)
        {
            try
            {
                //  DotNetNuke.Entities.Users.UserInfo uUser = DotNetNuke.Entities.Users.UserController.GetUserById(PortalSettings.PortalId, RecordID);

                UserController objUserController = new UserController();
                UserInfo uUser = objUserController.GetUser(DonorPortal, RecordID);

                uUser.FirstName = txtFirstName.Text.Trim().ToString();
                uUser.LastName = txtLastName.Text.Trim().ToString();
                uUser.Email = txtEmail.Text.ToString();

                uUser.Profile.Street = txtStreet.Text.Trim().ToString();
                uUser.Profile.City = txtCity.Text.Trim().ToString();
                
                uUser.Profile.SetProfileProperty("Prefix", ddlPrefix.SelectedValue.ToString());
                uUser.Profile.SetProfileProperty("MiddleName", txtMiddleName.Text.Trim().ToString());
                
                //doNotMail  item.Followup = Convert.ToBoolean(cbxFollowup.Checked); 
                uUser.Profile.SetProfileProperty("DoNotMail", cbxDoNotMail.Checked.ToString());
                uUser.Profile.SetProfileProperty("Anonymous", cbxAnonymous.Checked.ToString());

                uUser.Profile.SetProfileProperty("DonorType", ddlDonorType.SelectedValue.ToString());


                uUser.Profile.SetProfileProperty("AdditionalName", txtAdditionalName.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("AdditionalFirstName", txtAdditionalFirstName.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("AdditionalMI", txtAdditionalMI.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("CapacityRating", txtCapacityRating.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("InclinationRating", txtInclinationRating.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("Stage", txtStage.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("ProspectResearch", txtProspectResearch.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("ProspectManager", txtProspectManager.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("Age", txtAge.Text.Trim().ToString());




                uUser.Profile.SetProfileProperty("Company", txtCompany.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("Suffix", ddlSuffix.SelectedValue.ToString());

                uUser.Profile.SetProfileProperty("Street2", txtStreet2.Text.Trim().ToString());
                uUser.Profile.Telephone = txtTelephone.Text.ToString();
                uUser.Profile.SetProfileProperty("WorkPhone", txtWorkPhone.Text.ToString());
                uUser.Profile.Cell = txtCellPhone.Text.ToString();
                uUser.Profile.Fax = txtFax.Text.ToString();
                uUser.Profile.PostalCode = txtZip.Text.ToString();
                uUser.Profile.Region = ddlState.SelectedValue.ToString();

                uUser.Profile.SetProfileProperty("AltStreet", txtAltStreet.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("AltCity", txtAltCity.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("AltState", txtAltState.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("AltPostalCode", txtAltZip.Text.Trim().ToString());
                uUser.Profile.SetProfileProperty("Comments", txtComments.Text.ToString());

                UserController.UpdateUser(DonorPortal, uUser);

                // RELOAD RECORD
                LoadRecord(RecordID);

                lblStatus.Text = "Record Successully Updated";

          
                ImageDoNotMail.Visible = Convert.ToBoolean(cbxDoNotMail.Checked);
               
           


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void GetStates()
        {

            try
            {
                // Recipient State
                //ListController ctlList = new ListController();
                //ListEntryInfo vStates = ctlList.GetListEntryInfoCollection("Region", "Country.US", this.PortalId);

                var _states = new ListController().GetListEntryInfoItems("Region", "Country.US", this.PortalId);
                
                //ddlState.DataTextField = "Value";

                ddlState.DataTextField = "Text";
                ddlState.DataValueField = "EntryID";
                ddlState.DataSource = _states;
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("--", ""));


                //ListItem liregion = ddlState.Items.FindByValue("MA");
                //if (liregion != null)
                //{
                //    // value found
                //    ddlState.SelectedValue = "MA";
                //}
                //else
                //{
                //    //Value not found
                //    //   ddlTown.SelectedValue = item.ClientTown;
                //}

            //    ddlState.SelectedValue = "MA";

                var _suffix = new ListController().GetListEntryInfoItems("Suffix", "", this.PortalId);

                ddlSuffix.DataTextField = "Text";
                ddlSuffix.DataValueField = "Value";
                ddlSuffix.DataSource = _suffix;
                ddlSuffix.DataBind();
                ddlSuffix.Items.Insert(0, new ListItem("", ""));

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        //INSERT NEW USER BUTTON CLICK
        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                if (Request.QueryString["UserId"] != null)
                {
                    DonationUserId = Int32.Parse(Request.QueryString["UserId"]);
                    UpdateRecord(DonationUserId);
                }
                else
                {
                    CreateNewUser(RoleName);
                }



                //  Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(Globals.NavigateURL(), true);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Null.IsNull(DonationID))
                {
                    DonationTrackerController controller = new DonationTrackerController();
                    controller.DonationTrackerDeleteDonation(this.ModuleId, DonationID);
                    Response.Redirect(Globals.NavigateURL(), true);
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void CreateNewUser(string AddUserRole)
        {

            try
            {

                string vPassword = GenerateRandomString(7);

                UserInfo oUser = new UserInfo();

                oUser.PortalID = DonorPortal;
                oUser.IsSuperUser = false;
                oUser.FirstName = txtFirstName.Text.Trim().ToString();
                oUser.LastName = txtLastName.Text.Trim().ToString();
                oUser.Email = txtEmail.Text.Trim().ToString();
                //
                string NewUserName = "";
                if (txtEmail.Text.ToString().Length >= 7)
                {
                    NewUserName = txtEmail.Text.ToString();
                }
                else if (txtCompany.Text.ToString().Length >=6 )
                {
                    NewUserName = txtCompany.Text.ToString().Trim().Replace(" ", "").Replace("'", "").Replace(",", "");
                }
                else
                {
                    NewUserName = txtLastName.Text.ToString().Trim().Replace(" ", "").Replace("'", "").Replace(",", "") + txtFirstName.Text.ToString().Trim().Replace(" ", "").Replace("'", "").Replace(",", "");
                }
                if (NewUserName.ToString().Length < 7)
                {
                    NewUserName += "!!!";
                }

                oUser.Username = NewUserName.ToString();

                oUser.DisplayName = txtFirstName.Text.Trim().ToString() + " " + txtLastName.Text.Trim().ToString();

                //Fill MINIMUM Profile Items (KEY PIECE)
                oUser.Profile.PreferredLocale = PortalSettings.DefaultLanguage;
                oUser.Profile.PreferredTimeZone = PortalSettings.TimeZone;
                oUser.Profile.FirstName = oUser.FirstName;
                oUser.Profile.LastName = oUser.LastName;

                
                oUser.Profile.Street = txtStreet.Text.ToString();
                oUser.Profile.City = txtCity.Text.ToString();
                oUser.Profile.SetProfileProperty("Suffix", ddlSuffix.SelectedValue.ToString());
                oUser.Profile.SetProfileProperty("Prefix", ddlPrefix.SelectedValue.ToString());
                oUser.Profile.SetProfileProperty("Street2", txtStreet2.Text.ToString());
                oUser.Profile.SetProfileProperty("MiddleName", txtMiddleName.Text.Trim().ToString());
                oUser.Profile.SetProfileProperty("Company", txtCompany.Text.Trim().ToString());

                oUser.Profile.SetProfileProperty("DonorType", ddlDonorType.SelectedValue.ToString());

                oUser.Profile.SetProfileProperty("DoNotMail", cbxDoNotMail.Checked.ToString());
                oUser.Profile.SetProfileProperty("Anonymous", cbxAnonymous.Checked.ToString());

                //                +DonationUser.Profile.GetPropertyValue("AltStreet").ToString() + Environment.NewLine
                //+ DonationUser.Profile.GetPropertyValue("AltCity").ToString() + ", "
                //+ DonationUser.Profile.GetPropertyValue("AltState").ToString() + " "
                //+ DonationUser.Profile.GetPropertyValue("AltPostalCode").ToString();
                //txtAltStreet.Text = DonationUser.Profile.GetPropertyValue("AltStreet");
                //txtAltCity.Text = DonationUser.Profile.GetPropertyValue("AltCity");
                //txtAltState.Text = DonationUser.Profile.GetPropertyValue("AltState");
                //txtAltZip.Text = DonationUser.Profile.GetPropertyValue("AltPostalCode");
                //txtComments.Text = DonationUser.Profile.GetPropertyValue("Comments");

                oUser.Profile.SetProfileProperty("AltStreet", txtAltStreet.Text.ToString());
                oUser.Profile.SetProfileProperty("AltCity", txtAltCity.Text.ToString());
                oUser.Profile.SetProfileProperty("AltState", txtAltState.Text.ToString());
                oUser.Profile.SetProfileProperty("AltPostalCode", txtAltZip.Text.ToString());
                oUser.Profile.SetProfileProperty("Comments", txtComments.Text.ToString());

           //     oUser.Profile.SetProfileProperty("Biography", txtNotes.Text.ToString());
                oUser.Profile.Country = "221";
                oUser.Profile.Telephone = txtTelephone.Text.ToString();
                oUser.Profile.Cell = txtCellPhone.Text.ToString();
                oUser.Profile.Fax = txtFax.Text.ToString();
                oUser.Profile.PostalCode = txtZip.Text.ToString();
                oUser.Profile.Region = ddlState.SelectedValue.ToString();

                //Set Membership
                UserMembership oNewMembership = new UserMembership(oUser);
                oNewMembership.Approved = true;
                oNewMembership.CreatedDate = System.DateTime.Now;

         //       oNewMembership.Email = oUser.Email;
                oNewMembership.IsOnLine = false;
         //       oNewMembership.Username = oUser.Username;
                oNewMembership.Password = vPassword.ToString();

                //Bind membership to user
                oUser.Membership = oNewMembership;

                //Add the user, ensure it was successful 
                if (DotNetNuke.Security.Membership.UserCreateStatus.Success == UserController.CreateUser(ref oUser))
                {
                    //Add Role if passed something from module settings

                    if (AddUserRole.Length > 0)
                    {
                        DotNetNuke.Security.Roles.RoleController rc = new DotNetNuke.Security.Roles.RoleController();
                        //retrieve role
                        string groupName = AddUserRole;
                        DotNetNuke.Security.Roles.RoleInfo ri = rc.GetRoleByName(DonorPortal, groupName);
                        rc.AddUserRole(DonorPortal, oUser.UserID, ri.RoleID,DotNetNuke.Security.Roles.RoleStatus.Approved,false, DateTime.Today, Null.NullDate);
                    }

                    //string EmailContent = settingsData.EmailMessage + content;
                    DonationTrackerSettings settingsData = new DonationTrackerSettings(this.TabModuleId);

                    if (settingsData.EmailNewUserCredentials != null)
                    {
                        if (Convert.ToBoolean(settingsData.EmailNewUserCredentials) == true)
                        {
                            string EmailContent = settingsData.EmailMessage + "<p>UserName: " + txtEmail.Text.ToString() + "<br />";
                            EmailContent += "Password: " + vPassword.ToString() + "<br />";
                            EmailContent += "Site: http://" + Request.Url.Host + "</p>";

                            EmailContent = EmailContent.ToString().Replace("[FULLNAME]", oUser.DisplayName);

                            // SEND EMAIL
                            EmailNotification(EmailContent.ToString(), txtEmail.Text.ToString());
                        }
                        else
                        {
                            // DO NOT SEND EMAIL
                        }

                    }




                    // THIS URL WILL GIVE YOU THE ADD NEW DONATION PANEL
                    string newURL = Globals.NavigateURL(this.TabId, "", "UserId=" + oUser.UserID, "ctl=Edit", "mid=" + this.ModuleId, "Status=Success");

                    // THIS URL WILL GIVE YOU A BLANK FORM TO ADD A NEW USER RECORD
                    //string newURL = Globals.NavigateURL(this.TabId, "", "ctl=Edit", "mid=" + this.ModuleId, "Status=Success");

                    Response.Redirect(newURL, false);

                }
                else
                {
                    DotNetNuke.Entities.Users.UserInfo DonationUser = DotNetNuke.Entities.Users.UserController.GetUserByName(oUser.Username);
                    LoadRecord(DonationUser.UserID);
                    lblStatus.Text = "Error on Insert. Thay user already exists . . . I've loaded the record for you!";
                }






            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public void EmailNotification(string content, string toEmail)
        {

            try
            {
                DonationTrackerSettings settingsData = new DonationTrackerSettings(this.TabModuleId);
                // BUILD E-MAIL BODY

                string EmailContent = content;
                string subject = settingsData.EmailSubject.ToString();
                
                // LOOK FOR THE FROM EMAIL ADDRESS
                string EmailFrom = "";
                if (settingsData.EmailFrom.Length > 1)
                {
                    EmailFrom = settingsData.EmailFrom.ToString();
                }
                else
                {
                    EmailFrom = PortalSettings.Email.ToString();
                }
                // LOOK FOR BCC ADDRESS
                string EmailBCC = "";
                if (settingsData.EmailBCC.Length > 1)
                {
                    EmailBCC = settingsData.EmailBCC;
                }
                else
                {
                    EmailBCC = "";
                }
                //  SEND THE EMAIL
                DotNetNuke.Services.Mail.Mail.SendMail(EmailFrom.ToString(), toEmail, EmailBCC.ToString(), subject, EmailContent.ToString(), "", "HTML", "", "", "", "");

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }


        }


        public static string GenerateRandomString(int length)
        {
            //Removed O, o, 0, l, 1 
            string allowedLetterChars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string allowedNumberChars = "23456789";
            char[] chars = new char[length];
            Random rd = new Random();

            bool useLetter = true;
            for (int i = 0; i < length; i++)
            {
                if (useLetter)
                {
                    chars[i] = allowedLetterChars[rd.Next(0, allowedLetterChars.Length)];
                    useLetter = false;
                }
                else
                {
                    chars[i] = allowedNumberChars[rd.Next(0, allowedNumberChars.Length)];
                    useLetter = true;
                }

            }

            return new string(chars);
        }

        protected void cmdUpdateDonation_Click(object sender, EventArgs e)
        {

            try
            {
                

                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = new DonationTrackerInfo();

                item.ModuleId = this.ModuleId;
                item.DonationUserID = Int32.Parse(Request.QueryString["UserId"]);
                item.DriveID = Int32.Parse(ddlDrive.SelectedValue.ToString());
                
                item.DonationDate = Convert.ToDateTime(txtDonationDate.Text.ToString());  
                item.DonationAmount = double.Parse(txtDonationAmount.Text, NumberStyles.Currency);
                
                item.DonationType = ddlDonationType.SelectedValue.ToString();
                
                item.Followup = Convert.ToBoolean(cbxFollowup.Checked);
                item.DonationNotes = txtDonationNotes.Text.ToString();
                item.CreatedByUserID = this.UserId;
                item.PledgeID = Int32.Parse(hidDonationPledgeID.Value);
                //SqlDateTime sqldatenull;
                //sqldatenull = SqlDateTime.Null;
                item.TypeOfDonation = ddlTypeOfDonation.SelectedValue.ToString();
                item.DonationFrom = txtDonationFromName.Text.ToString();



                item.Source = ddlDonationSource.SelectedValue.ToString();

                if (hidPledgeDate.Value != "" && hidPledgeDate.Value != "1/1/0001")
                {
                    item.PledgeDate = DateTime.Parse(hidPledgeDate.Value.ToString());
                }
                else
                {
                    item.PledgeDate = (DateTime)System.Data.SqlTypes.SqlDateTime.Null;
                }
                
                if (DonationRecordID.Value.Length > 0)
                {
                    item.DonationID = Int32.Parse(DonationRecordID.Value.ToString());
                    controller.DonationTrackerUpdateDonation(item);

                    lblStatus.Text = "Donation Record Successfully Updated";
                }
                else
                {
                    controller.DonationTrackerAddDonation(item);
   
                    lblStatus.Text = "Donation Record Successfully Added";
                }

                // FOCUS TO ADD HISTORY TAB
                HttpCookie myCookie = new HttpCookie("dnnTabs-tabs-donors");
                myCookie.HttpOnly = false;
                myCookie.Value = "3";
                Response.Cookies.Add(myCookie);

                GridViewDonations.DataSource = null;
                GroupIt();
                FillDonationGridAndPledgeGrid();

            // CLEAN UP - RESET FORM FIELDS
                ddlDonationSource.ClearSelection();
                ddlDonationType.ClearSelection();
                ddlDrive.ClearSelection();
                txtDonationDate.Text = DateTime.Today.ToShortDateString();

                ddlTypeOfDonation.ClearSelection();
                txtDonationFromName.Text = "";

                txtDonationAmount.Text = "";
                cbxFollowup.Checked = false;
                txtDonationNotes.Text = "";

                DonationRecordID.Value = "";
                hidDonationPledgeID.Value = "0";
                hidPledgeDate.Value = "";
                cmdDeleteDonation.Visible = false;
                cmdUpdateDonation.Text = "Add Donation";


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }


        }



        protected void cmdDeleteDonation_Click(object sender, EventArgs e)
        {
            DonationTrackerController controller = new DonationTrackerController();
            DonationTrackerInfo item = new DonationTrackerInfo();
            controller.DonationTrackerDeleteDonation(this.ModuleId, Int32.Parse(DonationRecordID.Value.ToString()));


            lblStatus.Text = "Donation Record Deleted";

            // CLEAN UP - RESET FORM FIELDS

            txtDonationFromName.Text = "";
            ddlTypeOfDonation.ClearSelection();
            ddlDonationSource.ClearSelection();
            ddlDonationType.ClearSelection();
            ddlDrive.ClearSelection();
            txtDonationDate.Text = DateTime.Today.ToShortDateString();
            txtDonationAmount.Text = "";
            cbxFollowup.Checked = false;
            txtDonationNotes.Text = "";
            DonationRecordID.Value = "";
            hidDonationPledgeID.Value = "0";
            hidPledgeDate.Value = "";
            GridViewDonations.DataSource = null;
            GroupIt();
            FillDonationGridAndPledgeGrid();
        }



        protected void cmdUpdatePledge_Click(object sender, EventArgs e)
        {

            try
            {


                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = new DonationTrackerInfo();
                
                item.ModuleId = this.ModuleId;
                item.Source = ddlPledgeSource.SelectedValue.ToString();
                item.DonationUserID = Int32.Parse(Request.QueryString["UserId"]);
                item.DriveID = Int32.Parse(ddlPledgeDrive.SelectedValue.ToString());
                item.StartDate = Convert.ToDateTime(txtPledgeDate.Text.ToString());
                item.EndDate = Convert.ToDateTime(txtPledgeDateEnd.Text.ToString());
                item.PledgeAmount = double.Parse(txtPledgeAmount.Text, NumberStyles.Currency);
                item.Frequency = ddlFrequency.SelectedValue;
                item.Followup = Convert.ToBoolean(cbxPledgeFollowup.Checked);
                item.Notes = txtPledgeNotes.Text.ToString();
                item.CreatedByUserID = this.UserId;
                item.UserID = this.UserId;
                item.NumberOfPayments = Int32.Parse(ddlNumberOfPayments.SelectedValue.ToString());

                item.DonationFrom = txtPledgeDonationFromName.Text.ToString();
                item.TypeOfDonation = ddlPledgeTypeOfDonation.SelectedValue.ToString();
                
                if (hidPledgeID.Value.Length > 0)
                {
                    item.PledgeID = Int32.Parse(hidPledgeID.Value.ToString());
                    controller.DonationTrackerPledgeUpdate(item);

                    lblStatus.Text = "Pledge Record Successfully Updated";
                }
                else
                {
                    controller.DonationTrackerPledgeAdd(item);

                    lblStatus.Text = "Pledge Record Successfully Added";
                }


                //// NEED TO REPLACE WITH PROJECTED DONATION DATES
                //GridViewPledgeSchedule.DataSource = null;
                //AddTotalToPledgeGrid();
                //FillPledgeGrid();

                //GroupIt();
                //FillDonationGridAndPledgeGrid();
                
                // CLEAN UP - RESET FORM FIELDS
                ddlFrequency.ClearSelection();
                ddlPledgeDrive.ClearSelection();
                ddlPledgeSource.ClearSelection();
                ddlNumberOfPayments.ClearSelection();
                txtPledgeDate.Text = "";
                txtPledgeDateEnd.Text = "";
                txtPledgeAmount.Text = "";
                cbxPledgeFollowup.Checked = false;
                txtPledgeNotes.Text = "";
                txtPledgeDonationFromName.Text = "";
                ddlPledgeTypeOfDonation.ClearSelection();
                hidPledgeID.Value = "";

                cmdUpdatePledge.Text = "Add Pledge";
                cmdDeletePledge.Visible = false;
           
                // RELOAD DONATIONS
                GridViewDonations.DataSource = null;
                GroupIt();
                FillDonationGridAndPledgeGrid();

                // RELOAD PledgeSchedule
                GridViewPledgeSchedule.DataSource = null;
                AddTotalToPledgeGrid();
                FillPledgeGrid();


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }


        }



        protected void cmdDeletePledge_Click(object sender, EventArgs e)
        {
            DonationTrackerController controller = new DonationTrackerController();
         //   DonationTrackerInfo item = new DonationTrackerInfo();
            int _pledgeID = Int32.Parse(hidPledgeID.Value.ToString());
            //int myresult = -1;
            controller.DonationTrackerPledgeDelete(_pledgeID);


            lblStatus.Text = "Pledge Record Deleted";

       
            // CLEAN UP - RESET FORM FIELDS

            ddlPledgeDrive.ClearSelection();
            txtPledgeAmount.Text = "";

            ddlFrequency.ClearSelection();
            txtPledgeDate.Text="";

            ddlNumberOfPayments.ClearSelection();
            ddlPledgeSource.ClearSelection();
            ddlPledgeTypeOfDonation.ClearSelection();

            txtPledgeDonationFromName.Text = "";

            cbxPledgeFollowup.Checked = false;
            txtPledgeNotes.Text = "";
            hidPledgeID.Value = "";
            cmdDeletePledge.Visible = false;
            cmdUpdatePledge.Text = "Add Pledge";


            // RELOAD DONATIONS
            GridViewDonations.DataSource = null;
            GroupIt();
            FillDonationGridAndPledgeGrid();

            // RELOAD PledgeSchedule
            GridViewPledgeSchedule.DataSource = null;
            AddTotalToPledgeGrid();
            FillPledgeGrid();


        }



        protected void GridViewDonations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName.Equals("Edit"))
            //{
            //    //Perform validation & cancel update if the validation fails.
            //}
            
           
            if (e.CommandName.ToString() == "Update")
            {
              //  FillGrid();


                lblDebug.Text = "Need Command!";    // +e.CommandArgument.ToString();
            }

            return;
        }


        protected void GridViewDonations_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            e.Cancel = true;
            
          
            int donationID = Int32.Parse(GridViewDonations.DataKeys[e.RowIndex].Value.ToString());


       //     lblDebug.Text = "Need Command! " + donationID.ToString();
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "ThankYouLetter", "mid=" + ModuleId.ToString() + "&DonationID=" + donationID.ToString()));



        }


        protected void GridViewDonations_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                // PREVENT TEXTBOXES IN GRID
                e.Cancel = true;


                // FOCUS TO ADD DONATION TAB
                HttpCookie myCookie = new HttpCookie("dnnTabs-tabs-donors");
                myCookie.HttpOnly = false;
                myCookie.Value = "4";
                Response.Cookies.Add(myCookie);




                cmdDeleteDonation.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteDonationItemMessage.Confirm", this.LocalResourceFile) + "');");

                //string path = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtDonationDate);
                //CalendarImage.Attributes.Add("OnClick", path);

                int donationID = (int)GridViewDonations.DataKeys[e.NewEditIndex].Value;

                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = controller.DonationTrackerGetDonation(this.ModuleId, donationID);

                lblDebug.Text = donationID.ToString() + " HERE";

                if (item != null)
                {
                    cmdDeleteDonation.Visible = true;
                    ddlDonationType.SelectedValue = item.DonationType;
                  
                    ListItem lisource = ddlDonationSource.Items.FindByValue(item.Source);
                    if (lisource != null)
                    {
                        // value found
                        ddlDonationSource.SelectedValue = item.Source;
                    }
                    else
                    {
                        //Value not found - add it and select it
                        ddlDonationSource.Items.Insert(1, new ListItem(item.Source, item.Source));
                        ddlDonationSource.SelectedValue = item.Source;
                    }



                    ddlDrive.SelectedValue = item.DriveID.ToString();
                    txtDonationAmount.Text = item.DonationAmount.ToString("C2");
                    txtDonationDate.Text = item.DonationDate.ToShortDateString();

                    cbxFollowup.Checked = item.Followup;
                    txtDonationNotes.Text = item.DonationNotes;

                    DonationRecordID.Value = item.DonationID.ToString();
                    ddlTypeOfDonation.SelectedValue = item.TypeOfDonation;
                    txtDonationFromName.Text = item.DonationFrom;

                    string _pledgeID = item.PledgeID.ToString();
                    int num1 = 0;
                    bool successfullyParsed = int.TryParse(_pledgeID.ToString(), out num1);
                    if (successfullyParsed)
                    {
                        hidDonationPledgeID.Value = item.PledgeID.ToString();
                    }
                    else
                    {
                        hidDonationPledgeID.Value = num1.ToString();
                    }

                    hidPledgeDate.Value = item.PledgeDate.ToShortDateString();




                    lblStatus.Text = "Donation Record Loaded for Editing";
                    cmdUpdateDonation.Text = "Update Donation";

                }
                else
                {
                    DonationRecordID.Value = "";
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }



        }




        protected void GridViewPledge_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName.Equals("Edit"))
            //{
            //    //Perform validation & cancel update if the validation fails.
            //}


            if (e.CommandName.ToString() == "Update")
            {
                //  FillGrid();


                lblDebug.Text = "Need Command!";    // +e.CommandArgument.ToString();
            }

            return;
        }


        protected void GridViewPledge_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            e.Cancel = true;


            //int donationID = Int32.Parse(GridViewDonations.DataKeys[e.RowIndex].Value.ToString());


            ////     lblDebug.Text = "Need Command! " + donationID.ToString();
            //Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "ThankYouLetter", "mid=" + ModuleId.ToString() + "&DonationID=" + donationID.ToString()));



        }


        protected void GridViewPledge_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                
               
                // PREVENT TEXTBOXES IN GRID
                e.Cancel = true;

               
               
                int itemID = (int)GridViewPledgeSchedule.DataKeys[e.NewEditIndex].Value;
              //  string pledgeDate = (string)GridViewPledgeSchedule.DataKeys[e.NewEditIndex].Value;

                
                string donationDate = GridViewPledgeSchedule.Rows[itemID - 1].Cells[2].Text.ToString();
                // LOCK IT
                txtDonationDate.ReadOnly = true;

             
                // GET THE PLEDGEID TO LOOKUP DETAILS
                HiddenField field = GridViewPledgeSchedule.Rows[itemID - 1].FindControl("HiddenField1") as HiddenField;
                int pledgeID = Int32.Parse(field.Value.ToString());
              
                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = controller.DonationTrackerPledgeGetPledgeByID(pledgeID);

                if (item != null)
                {

                DonationTrackerInfo itemPledgeInsert = new DonationTrackerInfo();

                itemPledgeInsert.ModuleId = this.ModuleId;
                itemPledgeInsert.DonationUserID = Int32.Parse(Request.QueryString["UserId"]);
                itemPledgeInsert.DriveID = item.DriveID;
                DateTime dt = DateTime.Parse(donationDate.ToString());
                itemPledgeInsert.PledgeDate = dt;
                itemPledgeInsert.DonationDate = DateTime.Parse(DateTime.Today.ToShortDateString());
                itemPledgeInsert.DonationAmount = item.PledgeAmount;
                
                itemPledgeInsert.DonationType = "";
                itemPledgeInsert.DonationFrom = item.DonationFrom.ToString();
                itemPledgeInsert.TypeOfDonation = item.TypeOfDonation.ToString();
                itemPledgeInsert.Source = item.Source.ToString();
                itemPledgeInsert.Followup = item.Followup;
                itemPledgeInsert.DonationNotes = "Pledge Payment - " + item.Notes.ToString();
                itemPledgeInsert.CreatedByUserID = this.UserId;
                itemPledgeInsert.PledgeID = pledgeID;
                controller.DonationTrackerAddDonation(itemPledgeInsert);

                lblStatus.Text = "Pledge Donation Record Successfully Added ";
                
                // RELOAD DONATIONS
                GridViewDonations.DataSource = null;
                GroupIt();
                FillDonationGridAndPledgeGrid();

                // RELOAD PledgeSchedule
                GridViewPledgeSchedule.DataSource = null;
                AddTotalToPledgeGrid();                    
             //   FillPledgeGrid();
                FillPledgeGridByPledgeID(pledgeID);



                }
                else
                {
                    lblStatus.Text = "Error on loading pledge.";
                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }



        }



        protected void GridViewPledges_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                // PREVENT TEXTBOXES IN GRID
                e.Cancel = true;


                int pledgeID = (int)GridViewPledges.DataKeys[e.NewEditIndex].Value;

                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = controller.DonationTrackerPledgeGetPledgeByID(pledgeID);

                lblCurrentPledgeInfo.Text += "<br />PledgeID: " + pledgeID.ToString();

                if (item != null)
                {
                    cmdDeleteDonation.Visible = true;
                    ddlPledgeDrive.SelectedValue = item.DriveID.ToString();
                    ddlFrequency.SelectedValue = item.Frequency;
                    ddlDrive.SelectedValue = item.DriveID.ToString();
                    txtPledgeAmount.Text = item.PledgeAmount.ToString("C2");
                    txtPledgeDate.Text = item.StartDate.ToString("yyyy-MM-dd");
                    txtPledgeDateEnd.Text = item.EndDate.ToString("yyyy-MM-dd");
                    cbxPledgeFollowup.Checked = item.Followup;
                    txtPledgeNotes.Text = item.DonationNotes;
                    ddlNumberOfPayments.SelectedValue = item.NumberOfPayments.ToString();

                    txtPledgeDonationFromName.Text = item.DonationFrom.ToString();
                    ddlPledgeTypeOfDonation.SelectedValue = item.TypeOfDonation;
              
                    ListItem lisource = ddlPledgeSource.Items.FindByValue(item.Source);
                    if (lisource != null)
                    {
                        // value found
                        ddlPledgeSource.SelectedValue = item.Source;
                    }
                    else
                    {
                        //Value not found - add it and select it
                        ddlPledgeSource.Items.Insert(1, new ListItem(item.Source, item.Source));
                        ddlPledgeSource.SelectedValue = item.Source;
                    }


                    hidPledgeID.Value = pledgeID.ToString();

                    lblStatus.Text = "Pledge Record Loaded for Editing";
                    cmdUpdatePledge.Text = "Update Pledge";
                    cmdDeletePledge.Visible = true;
                }
                else
                {
                    hidPledgeID.Value = "";
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }



        }


        protected void GridViewPledges_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName.ToString() == "LoadLetter")
            {
               
                lblDebug.Text = "Need Command!";    // +e.CommandArgument.ToString();

                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "ThankYouLetter", "mid=" + ModuleId.ToString() + "&PledgeID=" + e.CommandArgument.ToString()));


            }

            return;
        }


        protected void GridViewPledges_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int pledgeID = Int32.Parse(GridViewPledges.DataKeys[e.RowIndex].Value.ToString());

            lblDebug.Text = "Need Command Update! " + pledgeID.ToString();
            //CommandEventArgs.

            

            FillPledgeGridByPledgeID(pledgeID);

            e.Cancel = true;

        }




        #region IActionable Members

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                //create a new action to add an item, this will be added to the controls
                //dropdown menu
                ModuleActionCollection actions = new ModuleActionCollection();

                actions.Add(GetNextActionID(), Localization.GetString("ReportSummary", this.LocalResourceFile),
                    ModuleActionType.AddContent, "", "", EditUrl("ReportSummary"), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                    true, false);
                //ReportSearch
                actions.Add(GetNextActionID(), Localization.GetString("btnReportSearch", this.LocalResourceFile),
                    ModuleActionType.AddContent, "", "", EditUrl("ReportSearch"), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                    true, false);


                actions.Add(GetNextActionID(), Localization.GetString("ListDrives", this.LocalResourceFile),
                     ModuleActionType.AddContent, "", "", EditUrl("ListDrives"), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                     true, false);

                return actions;
            }
        }

        #endregion

        protected void cmdSendCredentials_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtEmail.Text.ToString().Length > 7)
                {
                    
                    UserInfo u = new UserInfo();
                   
                    u = UserController.GetUserById(DonorPortal, Int32.Parse(Request.QueryString["UserId"]));
               //     u.Membership.v
                    u.Membership.Password = UserController.GetPassword(ref u, "");
                 //   System.Web.Security.Membership.ValidateUser(u.Username, u.Profile.p);
                    DotNetNuke.Services.Mail.Mail.SendMail(u, DotNetNuke.Services.Mail.MessageType.PasswordUpdated, PortalSettings);
                    cmdSendCredentials.Visible = false;
                    lblSendCredentials.Text = "Password Successfully Sent!";

                }
                else
                {
                    lblSendCredentials.Text = "A valid e-mail address is required to send a password!";
                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }





    }
}