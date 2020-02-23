using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using GIBS.DonationTracker.Components;
using GIBS.Modules.DonationTracker.Components;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace GIBS.Modules.DonationTracker
{
    public partial class ThankYouLetter : PortalModuleBase
    {

        int DonationID = Null.NullInteger;
        int PledgeID = Null.NullInteger;

        public string _ReportCredentialsDomain = "";        //"ORLEANS";
        public string _ReportCredentialsPassword = "";        //"CapeCod1";
        public string _ReportCredentialsUserName = "";        //"inetFamilyPantry";
        public string _ReportPath = "";        //"/FamilyPantry/DonationThankYouLetter";
        public string _ReportServerURL = "";        //"http://orleans.gibs.com:8081/Reportserver";

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                LoadSettings();

                if (Request.QueryString["DonationID"] != null)
                {
                    DonationID = Int32.Parse(Request.QueryString["DonationID"]);
                    hidDonationID.Value = DonationID.ToString();
                }
                if (Request.QueryString["PledgeID"] != null)
                {
                    PledgeID = Int32.Parse(Request.QueryString["PledgeID"]);
                    hidPledgeID.Value = PledgeID.ToString();

                }

                if (!IsPostBack)
                {
                    if (DonationID != Null.NullInteger)
                    {
                        GetDonation(DonationID);
                    }

                    if (PledgeID != Null.NullInteger)
                    {
                        string _returnURL = Request.UrlReferrer.ToString();
                        HyperLinkReturnToDonor.NavigateUrl = _returnURL.ToString();
                        // lblDebug.Text = "Still working on this part . . . Will be ready for next release!";
                        GetPledge(PledgeID);
                    }

                    LoadTemplates();
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        //ddlLetterTemplates




        public void LoadSettings()
        {
            try
            {
                //  DonationTrackerSettings settingsData = new DonationTrackerSettings(this.TabModuleId);
                if (Settings.Contains("ReportCredentialsDomain"))
                {
                    _ReportCredentialsDomain = Settings["ReportCredentialsDomain"].ToString();
                }

                if (Settings.Contains("ReportCredentialsPassword"))
                {
                    _ReportCredentialsPassword = Settings["ReportCredentialsPassword"].ToString();
                }

                if (Settings.Contains("ReportCredentialsUserName"))
                {
                    _ReportCredentialsUserName = Settings["ReportCredentialsUserName"].ToString();
                }

                if (Settings.Contains("ReportPath"))
                {
                    _ReportPath = Settings["ReportPath"].ToString();
                }

                if (Settings.Contains("ReportServerURL"))
                {
                    _ReportServerURL = Settings["ReportServerURL"].ToString();
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void GetDonationTemplate(int _DonationID, int _LetterID)
        {
            try
            {
                string _myOriginalLetter = "";
                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = controller.DonationTrackerGetDonation(this.ModuleId, _DonationID);

                if (_LetterID != 0)
                {
                    DonationTrackerInfo itemTemplate = controller.DonationTrackerLetterTemplate_Get(_LetterID);
                    _myOriginalLetter = itemTemplate.Letter.ToString();

                    if (item != null)
                    {

                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[DriveName]", item.DriveName.ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[FirstName]", item.FirstName.ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[LastName]", item.LastName.ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[DonationAmount]", String.Format("{0:C}", item.DonationAmount).ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[PrimaryAddress]", GetDonorAddress(item.DonationUserID));
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[DonationDate]", item.DonationDate.ToLongDateString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[FirstNames]", GetBothNames(item.DonationUserID));
                        //_myOriginalLetter = FixTokens(_myOriginalLetter, "[DriveName]", item.DriveName.ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[PledgeAmount]", String.Format("{0:C}", item.PledgeAmount).ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[PledgeStartDate]", item.StartDate.ToLongDateString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[NumberOfPayments]", item.NumberOfPayments.ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[TotalAmountPledged]", String.Format("{0:C}", item.TotalAmountPledged).ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[Frequency]", GetFrequencyLookup(item.Frequency.ToString()));

                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[InHonorOf]", (item.TypeOfDonation.ToString().ToLower() + " " + item.DonationFrom.ToString()).Trim().ToString());

                        txtLetter.Text = _myOriginalLetter.ToString();

                        // SET RETURN TO DONOR LINK
                        string formatString = EditUrl("UserId", item.DonationUserID.ToString(), "Edit");
                        HyperLinkReturnToDonor.NavigateUrl = formatString.ToString();
                        // SET NEW SEARCH LINK
                        string newSearch = Globals.NavigateURL();
                        HyperLinkNewSearch.NavigateUrl = newSearch.ToString();
                    }
                    else
                    {
                        lblDebug.Text = "ERROR ON RECORD LOAD";
                    }

                }
                else
                {
                    GetDonation(DonationID);
                }




            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }




        public string GetBothNames(int RecordID)
        {
            try
            {

                DotNetNuke.Entities.Users.UserInfo DonationUser = DotNetNuke.Entities.Users.UserController.GetUserById(this.PortalId, RecordID);



                string BothNames = "";
                if (DonationUser.Profile.GetPropertyValue("AdditionalFirstName") != null && DonationUser.Profile.GetPropertyValue("AdditionalFirstName").ToString().Length > 0)
                {
                    BothNames = DonationUser.FirstName + " & " + DonationUser.Profile.GetPropertyValue("AdditionalFirstName").ToString();
                }
                else
                {
                    BothNames = DonationUser.FirstName;
                }




                return BothNames.ToString();



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return ex.ToString();
            }
        }








        public void GetPledgeTemplate(int _PledgeID, int _LetterID)
        {
            try
            {
                string _myOriginalLetter = "";
                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = controller.DonationTrackerPledgeGetPledgeByID(_PledgeID);

                if (_LetterID != 0)
                {
                    DonationTrackerInfo itemTemplate = controller.DonationTrackerLetterTemplate_Get(_LetterID);
                    _myOriginalLetter = itemTemplate.Letter.ToString();

                    if (item != null)
                    {


                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[DriveName]", item.DriveName.ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[FirstName]", item.FirstName.ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[LastName]", item.LastName.ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[DonationAmount]", String.Format("{0:C}", item.DonationAmount).ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[PrimaryAddress]", GetDonorAddress(item.UserID));
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[DonationDate]", item.DonationDate.ToLongDateString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[FirstNames]", GetBothNames(item.UserID));
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[PledgeAmount]", String.Format("{0:C}", item.PledgeAmount).ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[PledgeStartDate]", item.StartDate.ToLongDateString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[NumberOfPayments]", item.NumberOfPayments.ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[TotalAmountPledged]", String.Format("{0:C}", item.TotalAmountPledged).ToString());
                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[Frequency]", GetFrequencyLookup(item.Frequency.ToString()));

                        _myOriginalLetter = FixTokens(_myOriginalLetter, "[InHonorOf]", (item.TypeOfDonation.ToString().ToLower() + " " + item.DonationFrom.ToString()).Trim().ToString());

                        txtLetter.Text = _myOriginalLetter.ToString();

                        // SET RETURN TO DONOR LINK
                        string formatString = EditUrl("UserId", item.DonationUserID.ToString(), "Edit");
                        HyperLinkReturnToDonor.NavigateUrl = formatString.ToString();
                        // SET NEW SEARCH LINK
                        string newSearch = Globals.NavigateURL();
                        HyperLinkNewSearch.NavigateUrl = newSearch.ToString();
                        lblDebug.Text = "Template Loaded";
                    }
                    else
                    {
                        lblDebug.Text = "ERROR ON RECORD LOAD";
                    }

                }
                else
                {
                    GetDonation(DonationID);
                }




            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public void GetPledge(int _PledgeID)
        {
            try
            {
                string _myOriginalLetter = "";
                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = controller.DonationTrackerPledgeGetPledgeByID(_PledgeID);
                //controller.DonationTrackerGetDonation(this.ModuleId, _DonationID);


                if (item != null)
                {
                    if (item.LetterGenerated)
                    {
                        DonationTrackerInfo itemPreviousLetter = controller.DonationTrackerLetterGet_ByPledgeID(_PledgeID);
                        _myOriginalLetter = itemPreviousLetter.Letter.ToString();
                        lblDebug.Text = "You are viewing a previously sent letter generated on " + itemPreviousLetter.CreatedDate.ToShortDateString();
                    }

                    else
                    {
                        _myOriginalLetter = item.PledgeLetter.ToString();
                        lblDebug.Text = "You are viewing the default template!";

                    }



                    //TotalAmountPledged

                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[DriveName]", item.DriveName.ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[FirstName]", item.FirstName.ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[LastName]", item.LastName.ToString());

                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[PrimaryAddress]", GetDonorAddress(item.UserID));
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[FirstNames]", GetBothNames(item.UserID));
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[PledgeAmount]", String.Format("{0:C}", item.PledgeAmount).ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[PledgeStartDate]", item.StartDate.ToLongDateString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[NumberOfPayments]", item.NumberOfPayments.ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[TotalAmountPledged]", String.Format("{0:C}", item.TotalAmountPledged).ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[Frequency]", GetFrequencyLookup(item.Frequency.ToString()));

                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[InHonorOf]", (item.TypeOfDonation.ToString().ToLower() + " " + item.DonationFrom.ToString()).Trim().ToString());


                    txtLetter.Text = _myOriginalLetter.ToString();

                    // SET RETURN TO DONOR LINK
                    string formatString = EditUrl("UserId", item.UserID.ToString(), "Edit", "TabFocus=History");
                    HyperLinkReturnToDonor.NavigateUrl = formatString.ToString();
                    // SET NEW SEARCH LINK
                    string newSearch = Globals.NavigateURL();
                    HyperLinkNewSearch.NavigateUrl = newSearch.ToString();
                }
                else
                {
                    lblDebug.Text = "ERROR ON PLEDGE RECORD LOAD";
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
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
                        _numMonths = "months";
                        break;
                    case "q":
                        _numMonths = "quarterly";
                        break;
                    case "s":
                        _numMonths = "semi-annually";
                        break;
                    case "a":
                        _numMonths = "years";
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

        public void GetDonation(int _DonationID)
        {
            try
            {
                string _myOriginalLetter = "";
                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = controller.DonationTrackerGetDonation(this.ModuleId, _DonationID);


                if (item != null)
                {
                    if (item.LetterGenerated)
                    {
                        DonationTrackerInfo itemPreviousLetter = controller.DonationTrackerLetterGet_ByDonationID(DonationID);
                        _myOriginalLetter = itemPreviousLetter.Letter.ToString();
                        hidLetterID.Value = itemPreviousLetter.LetterID.ToString();
                        lblDebug.Text = "You are viewing a previously sent letter generated on " + itemPreviousLetter.CreatedDate.ToShortDateString();
                    }

                    else
                    {
                        _myOriginalLetter = item.Description.ToString();

                    }




                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[DriveName]", item.DriveName.ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[FirstName]", item.FirstName.ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[LastName]", item.LastName.ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[DonationAmount]", String.Format("{0:C}", item.DonationAmount).ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[PrimaryAddress]", GetDonorAddress(item.DonationUserID));
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[DonationDate]", item.DonationDate.ToLongDateString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[FirstNames]", GetBothNames(item.DonationUserID));
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[PledgeAmount]", String.Format("{0:C}", item.PledgeAmount).ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[PledgeStartDate]", item.StartDate.ToLongDateString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[NumberOfPayments]", item.NumberOfPayments.ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[TotalAmountPledged]", String.Format("{0:C}", item.TotalAmountPledged).ToString());
                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[Frequency]", GetFrequencyLookup(item.Frequency.ToString()));

                    _myOriginalLetter = FixTokens(_myOriginalLetter, "[InHonorOf]", (item.TypeOfDonation.ToString().ToLower() + " " + item.DonationFrom.ToString()).Trim().ToString());

                    txtLetter.Text = _myOriginalLetter.ToString();

                    // SET RETURN TO DONOR LINK
                    string formatString = EditUrl("UserId", item.DonationUserID.ToString(), "Edit", "TabFocus=DonorRecord");
                    HyperLinkReturnToDonor.NavigateUrl = formatString.ToString();
                    // SET NEW SEARCH LINK
                    string newSearch = Globals.NavigateURL();
                    HyperLinkNewSearch.NavigateUrl = newSearch.ToString();
                }
                else
                {
                    lblDebug.Text = "ERROR ON DONATION RECORD LOAD";
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        private void ShowReport()
        {
            try
            {
                ReportViewer1.Visible = true;
                string urlReportServer = _ReportServerURL.ToString();   // "http://orleans:8081/Reportserver"; //get from web config
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote; // ProcessingMode will be Either Remote or Local
                ReportViewer1.ServerReport.ReportServerUrl = new Uri(urlReportServer); //Set the ReportServer Url
                ReportViewer1.ServerReport.ReportPath = _ReportPath.ToString(); //  get from the query string                

                ////Creating an ArrayList for combine the Parameters which will be passed into SSRS Report
                ArrayList reportParam = new ArrayList();
                reportParam = ReportDefaultParam();
                ReportParameter[] param = new ReportParameter[reportParam.Count];
                for (int k = 0; k < reportParam.Count; k++)
                {
                    param[k] = (ReportParameter)reportParam[k];
                }
                //

                //ReportViewer1.ServerReport.ReportServerCredentials = new ReportServerCredentials("Administrator", "Bouyea9213", "Orleans");
                ReportViewer1.ServerReport.ReportServerCredentials = new ReportServerCredentials(_ReportCredentialsUserName.ToString(), _ReportCredentialsPassword.ToString(), _ReportCredentialsDomain.ToString());
                //pass parmeters to report
                ReportViewer1.ServerReport.SetParameters(param); //Set Report Parameters
                ReportViewer1.ServerReport.Refresh();


                //Creating a PDF from a RDLC Report in the Background

                //Warning[] warnings;
                //string[] streamids;
                //string mimeType;
                //string encoding;
                //string filenameExtension;

                //byte[] bytes = ReportViewer1.LocalReport.Render(
                //    "PDF", null, out mimeType, out encoding, out filenameExtension,
                //    out streamids, out warnings);

                //using (FileStream fs = new FileStream("output.pdf", FileMode.Create))
                //{
                //    fs.Write(bytes, 0, bytes.Length);
                //}

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void LoadTemplates()
        {
            try
            {
                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();
                items = controller.DonationTrackerLetterTemplate_List(this.ModuleId, 1);

                if (items != null)
                {

                    ddlLetterTemplates.DataTextField = "LetterName";
                    ddlLetterTemplates.DataValueField = "LetterID";
                    ddlLetterTemplates.DataSource = items;
                    ddlLetterTemplates.DataBind();
                    ddlLetterTemplates.Items.Insert(0, new ListItem("-- Select Template --", "0"));


                }
                else
                {
                    lblDebug.Text = "ERROR ON RECORD LOAD";
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }



        public string GetStateByName(string name)
        {
            switch (name.ToUpper())
            {
                case "ALABAMA":
                    return "AL";

                case "ALASKA":
                    return "AK";

                case "AMERICAN SAMOA":
                    return "AS";

                case "ARIZONA":
                    return "AZ";

                case "ARKANSAS":
                    return "AR";

                case "CALIFORNIA":
                    return "CA";

                case "COLORADO":
                    return "CO";

                case "CONNECTICUT":
                    return "CT";

                case "DELAWARE":
                    return "DE";

                case "DISTRICT OF COLUMBIA":
                    return "DC";

                case "FEDERATED STATES OF MICRONESIA":
                    return "FM";

                case "FLORIDA":
                    return "FL";

                case "GEORGIA":
                    return "GA";

                case "GUAM":
                    return "GU";

                case "HAWAII":
                    return "HI";

                case "IDAHO":
                    return "ID";

                case "ILLINOIS":
                    return "IL";

                case "INDIANA":
                    return "IN";

                case "IOWA":
                    return "IA";

                case "KANSAS":
                    return "KS";

                case "KENTUCKY":
                    return "KY";

                case "LOUISIANA":
                    return "LA";

                case "MAINE":
                    return "ME";

                case "MARSHALL ISLANDS":
                    return "MH";

                case "MARYLAND":
                    return "MD";

                case "MASSACHUSETTS":
                    return "MA";

                case "MICHIGAN":
                    return "MI";

                case "MINNESOTA":
                    return "MN";

                case "MISSISSIPPI":
                    return "MS";

                case "MISSOURI":
                    return "MO";

                case "MONTANA":
                    return "MT";

                case "NEBRASKA":
                    return "NE";

                case "NEVADA":
                    return "NV";

                case "NEW HAMPSHIRE":
                    return "NH";

                case "NEW JERSEY":
                    return "NJ";

                case "NEW MEXICO":
                    return "NM";

                case "NEW YORK":
                    return "NY";

                case "NORTH CAROLINA":
                    return "NC";

                case "NORTH DAKOTA":
                    return "ND";

                case "NORTHERN MARIANA ISLANDS":
                    return "MP";

                case "OHIO":
                    return "OH";

                case "OKLAHOMA":
                    return "OK";

                case "OREGON":
                    return "OR";

                case "PALAU":
                    return "PW";

                case "PENNSYLVANIA":
                    return "PA";

                case "PUERTO RICO":
                    return "PR";

                case "RHODE ISLAND":
                    return "RI";

                case "SOUTH CAROLINA":
                    return "SC";

                case "SOUTH DAKOTA":
                    return "SD";

                case "TENNESSEE":
                    return "TN";

                case "TEXAS":
                    return "TX";

                case "UTAH":
                    return "UT";

                case "VERMONT":
                    return "VT";

                case "VIRGIN ISLANDS":
                    return "VI";

                case "VIRGINIA":
                    return "VA";

                case "WASHINGTON":
                    return "WA";

                case "WEST VIRGINIA":
                    return "WV";

                case "WISCONSIN":
                    return "WI";

                case "WYOMING":
                    return "WY";
                default:
                    return "";
            }

            throw new Exception("Unavailable");
        }

        public string GetDonorAddress(int RecordID)
        {
            try
            {

                DotNetNuke.Entities.Users.UserInfo DonationUser = DotNetNuke.Entities.Users.UserController.GetUserById(this.PortalId, RecordID);



                string _FullName = DonationUser.Profile.GetPropertyValue("Prefix") + " "
                    + DonationUser.FirstName + " "
                    + DonationUser.Profile.GetPropertyValue("MiddleName") + " "
                    + DonationUser.LastName + " "
                    + DonationUser.Profile.GetPropertyValue("Suffix");


                string _AdditionalName = DonationUser.Profile.GetPropertyValue("AdditionalFirstName") + " "
                    + DonationUser.Profile.GetPropertyValue("AdditionalMI") + " "
                    + DonationUser.Profile.GetPropertyValue("AdditionalName");


                string _Region = "";

                if (DonationUser.Profile.Region != null)
                {
                    _Region = DonationUser.Profile.Region;
                }



                // GET RID OF DOUBLE SPACES
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"[ ]{2,}", options);
                _FullName = regex.Replace(_FullName, @" ");
                _AdditionalName = regex.Replace(_AdditionalName, @" ");

                //  _FullName = _FullName.ToString().Replace("  ", " ");
                //AdditionalFirstName AdditionalMI
                string _PrimaryAddress = _FullName.ToString().Trim() + Environment.NewLine
                    + _AdditionalName.ToString().Trim() + Environment.NewLine
                    + DonationUser.Profile.GetPropertyValue("Company") + Environment.NewLine
                    + DonationUser.Profile.Street + Environment.NewLine
                    + DonationUser.Profile.GetPropertyValue("Street2") + Environment.NewLine
                    + DonationUser.Profile.City + ", "
                    + GetStateByName(_Region.ToString()) + " "
                    + DonationUser.Profile.PostalCode;
                //   REMOVE EMPTY LINES
                _PrimaryAddress = Regex.Replace(_PrimaryAddress.ToString(), @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
                _PrimaryAddress = _PrimaryAddress.TrimEnd(',').ToString();
                return _PrimaryAddress.ToString();



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return ex.ToString();
            }
        }


        public string FixTokens(string _myOriginal, string _myToken, string _myReplacement)
        {

            try
            {
                string _ReturnValue = "";

                _ReturnValue = _myOriginal.ToString().Replace(_myToken, _myReplacement.ToString()).ToString();

                return _ReturnValue;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return ex.ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                lblDebug.Text = "";

                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = new DonationTrackerInfo();

                item.DonationID = Convert.ToInt32(hidDonationID.Value.ToString());

                item.Letter = txtLetter.Text.ToString();
                item.CreatedByUserID = this.UserId;
                int _MyNewetterID = 0;
                item.PledgeID = Convert.ToInt32(hidPledgeID.Value.ToString());
                _MyNewetterID = controller.DonationTrackerLetterAdd(item);

                hidLetterID.Value = _MyNewetterID.ToString();
                txtLetter.Visible = false;
                btnSave.Visible = false;
                ddlLetterTemplates.Visible = false;

                //  lblDebug.Text = hidLetterID.Value.ToString();
                lblDebug.Text = "Letter Successfully Generated!";

                HyperLinkReturnToDonor.Visible = true;
                HyperLinkNewSearch.Visible = true;

                ShowReport();


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void DisableUnwantedExportFormats(ServerReport rvServer)
        {
            foreach (RenderingExtension extension in rvServer.ListRenderingExtensions())
            {
                //if (extension.Name == "XML" || extension.Name == "TIFF" || extension.Name == "MHTML" || extension.Name == "EXCEL" || extension.Name == "CSV")
                if (extension.Name == "XML" || extension.Name == "IMAGE" || extension.Name == "MHTML" || extension.Name == "CSV" || extension.Name == "EXCEL")
                {
                    ReflectivelySetVisibilityFalse(extension);
                }
            }
        }


        private void ReflectivelySetVisibilityFalse(RenderingExtension extension)
        {
            FieldInfo info = extension.GetType().GetField("m_isVisible", BindingFlags.NonPublic | BindingFlags.Instance);


            if (info != null)
            {
                info.SetValue(extension, false);
            }
        }


        protected void ReportViewer1_PreRender(object sender, EventArgs e)
        {
            DisableUnwantedExportFormats(ReportViewer1.ServerReport);
        }

        private ArrayList ReportDefaultParam()
        {

            ArrayList arrLstDefaultParam = new ArrayList();

            arrLstDefaultParam.Add(CreateReportParameter("LetterID", hidLetterID.Value.ToString()));// get this from mycontext

            //     arrLstDefaultParam.Add(CreateReportParameter("DatabaseServer", "Orleans")); //get this from web.config
            // All report parameters will be passed in the QueryString. 
            // The name - key will be prefaced by p and the name will match the p
            // EXample : businessnet.pureaxess.com/Admin/reports/SSRSReportViewer.aspx?reportname=" + reportName + '&p_locationid=' + plocationid;
            //  EFT_Batch_Breakdown
            //// We will lop over the query string and pull out the p-keys/values
            //string sKey;
            //string sValue;
            //foreach (String key in Request.QueryString.AllKeys)
            //{

            //    sKey = key;

            //    //  Response.Write("Key: " + key + " Value: " + Request.QueryString[key] + " -||| " + sKey.Substring(0, 2));

            //    sValue = Request.QueryString[key];
            //    if (sKey.Substring(0, 2).ToString() == "p_")
            //    {
            //        sKey = sKey.Substring(2, sKey.Length - 2);
            //        arrLstDefaultParam.Add(CreateReportParameter(sKey, sValue));    // get this from the query string

            //    }

            //}
            return arrLstDefaultParam;
        }
        private ReportParameter CreateReportParameter(string paramName, string pramValue)
        {
            ReportParameter aParam = new ReportParameter(paramName, pramValue);
            return aParam;
        }

        protected void ddlLetterTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblDebug.Text = "";


            if (ddlLetterTemplates.SelectedValue.ToString() == "0")
            {

                if (Request.QueryString["DonationID"] != null)
                {
                    GetDonation(DonationID);
                }

                if (Request.QueryString["PledgeID"] != null)
                {
                    GetPledge(PledgeID);
                }
            }
            else
            {
                if (Request.QueryString["DonationID"] != null)
                {
                    GetDonationTemplate(DonationID, Convert.ToInt32(ddlLetterTemplates.SelectedValue.ToString()));
                }

                if (Request.QueryString["PledgeID"] != null)
                {
                    GetPledgeTemplate(Convert.ToInt32(hidPledgeID.Value.ToString()), Convert.ToInt32(ddlLetterTemplates.SelectedValue.ToString()));
                }
            }


        }


    }
}