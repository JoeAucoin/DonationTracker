using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using GIBS.DonationTracker.Components;
using DotNetNuke.Entities.Profile;
using Globals = DotNetNuke.Common.Globals;
//using DotNetNuke.Entities.Users;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Security.Roles;
using DotNetNuke.Common.Lists;
using System.Data;


namespace GIBS.Modules.DonationTracker
{
    public partial class ViewDonationTracker : PortalModuleBase, IActionable
    {
        private string _Filter = "";

        private int _CurrentPage = 1;
        private ArrayList _Users = new ArrayList();
        protected int TotalPages = -1;
        protected int TotalRecords = 0;
        static int PageSize;
        static string RoleName;
        public string _ReportsRole;
        protected bool SuppressPager = false;
        //       protected string RoleName = "Registered Users";

        public DataTable dt;

        protected int CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; }
        }
        protected string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }





        protected void Page_Init(Object sender, EventArgs e)
        {
            foreach (DataGridColumn column in grdUsers.Columns)
            {

                if (column.GetType() == typeof(ImageCommandColumn))
                {

                    ImageCommandColumn imageColumn = (ImageCommandColumn)column;

                    //Manage Edit Column NavigateURLFormatString                      
                    if (imageColumn.CommandName == "Edit")
                    {
                        //The Friendly URL parser does not like non-alphanumeric characters                          
                        //so first create the format string with a dummy value and then                          
                        //replace the dummy value with the FormatString place holder                          
                        string formatString = EditUrl("UserId", "KEYFIELD", "Edit", "TabFocus=DonorRecord");
                        formatString = formatString.Replace("KEYFIELD", "{0}");
                        imageColumn.NavigateURLFormatString = formatString;
                    }


                    if (imageColumn.CommandName == "Donate")
                    {
                        //The Friendly URL parser does not like non-alphanumeric characters                          
                        //so first create the format string with a dummy value and then                          
                        //replace the dummy value with the FormatString place holder                          
                        string formatString = EditUrl("UserId", "KEYFIELD", "Edit", "TabFocus=AddDonation");
                        formatString = formatString.Replace("KEYFIELD", "{0}");
                        imageColumn.NavigateURLFormatString = formatString;
                    }

                    if (imageColumn.CommandName == "Pledge")
                    {
                        //The Friendly URL parser does not like non-alphanumeric characters                          
                        //so first create the format string with a dummy value and then                          
                        //replace the dummy value with the FormatString place holder                          
                        string formatString = EditUrl("UserId", "KEYFIELD", "Edit", "TabFocus=AddPledge");
                        formatString = formatString.Replace("KEYFIELD", "{0}");
                        imageColumn.NavigateURLFormatString = formatString;
                    }

                    //Localize Image Column Text                     
                    if (!String.IsNullOrEmpty(imageColumn.CommandName))
                    {
                        imageColumn.Text = Localization.GetString(imageColumn.CommandName, this.LocalResourceFile);
                    }
                }
                //column.Visible = isVisible;              
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {



                if (!IsPostBack)
                {
                    txtSearch.Focus();

                    if (Request.QueryString["CurrentPage"] != null)
                    {
                        CurrentPage = Convert.ToInt32(Request.QueryString["CurrentPage"]);
                    }



                    if (Request.QueryString["SearchField"] != null)
                    {
                        ddlSearchType.SelectedValue = Request.QueryString["SearchField"];
                    }

                    if (Request.QueryString["OrderBy"] != null)
                    {
                        ddlOrderBy.SelectedValue = Request.QueryString["OrderBy"];
                    }



                    CreateLetterSearch();
                    //  LoadDropdowns();
                    LoadSettings();
                    SetReportsButtonView();


                    if (Request.QueryString["filter"] != null)
                    {
                        //   Filter = Request.QueryString["filter"];
                        txtSearch.Text = Request.QueryString["filter"].ToString();

                        BindData(txtSearch.Text.ToString(), ddlSearchType.SelectedValue.ToString(), ddlOrderBy.SelectedValue.ToString(), "asc");

                    }



                }

                else
                {

                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }




        public void LoadSettings()
        {
            try
            {
                
                if (Settings.Contains("numPerPage"))
                {
                    PageSize = Int32.Parse(Settings["numPerPage"].ToString());
                }

                if (Settings.Contains("roleName"))
                {
                    RoleName = Settings["roleName"].ToString();
                }

                if (Settings.Contains("reportsRole"))
                {
                    _ReportsRole = Settings["reportsRole"].ToString();
                }

                if (Settings.Contains("enableAddNewDonor"))
                {
                    if (Convert.ToBoolean(Settings["enableAddNewDonor"].ToString()) == true)
                    {
                        btnAddNewUser.Enabled = true;
                    }
                    else
                    {
                        btnAddNewUser.Enabled = false;
                    }
                }
                else
                {
                    btnAddNewUser.Enabled = false;
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        public void SetReportsButtonView()
        {
            try
            {

                var roleGroup = UserInfo.IsInRole(_ReportsRole);

                if (roleGroup == true)
                {
                    // TURN OFF LEGACY REPORTS
                    //btnReports.Visible = true;
                    //btnReportSearch.Visible = true;
                }
                else
                {
                    btnReports.Visible = false;
                    btnReportSearch.Visible = false;

                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        protected void btnSearch_Click(Object sender, ImageClickEventArgs e)
        {
            TotalRecords = 0;
            CurrentPage = 1;


            string _criteria = "";
            if (txtSearch.Text.ToString().Trim().Length == 0)
            {
                _criteria = "Return=AllRecords";
            }
            else
            {
                _criteria = "filter=" + txtSearch.Text.ToString();
            }


            string _URL = "";
            _URL = Globals.NavigateURL(TabId, "", _criteria.ToString(), "currentpage=1", "SearchField=" + ddlSearchType.SelectedValue, "OrderBy=" + ddlOrderBy.SelectedValue);
            Response.Redirect(_URL);


        }





        private void BindData()
        {
            BindData(null, null, null, null);
            ctlPagingControl.Visible = false;
        }



        public void BindData(string SearchText, string SearchField, string orderByField, string orderByDirection)
        {

            ctlPagingControl.Visible = true;

            orderByDirection = "asc";
            string strQuerystring = Null.NullString;


            //if (Filter == Localization.GetString("All"))
            //{
            //    SearchText = "";
            //}

            if (txtSearch.Text.ToString().Trim().Length > 0)
            {
                strQuerystring += "filter=" + txtSearch.Text.ToString();
            }

            strQuerystring += "&SearchField=" + SearchField.ToString();
            strQuerystring += "&OrderBy=" + orderByField.ToString();

            DonationTrackerController controller = new DonationTrackerController(); //ref TotalRecords,
            List<DonationTrackerInfo> Users11 = new List<DonationTrackerInfo>();

            //Joe this
            Users11 = controller.DonationTrackerUserFullListSearch(this.PortalId, CurrentPage, PageSize, SearchField, SearchText, orderByField, orderByDirection);


            DataTable dt = new DataTable();
            dt = Components.GridViewTools.ToDataTable(Users11);

            TotalRecords = (from DataRow dr in dt.Rows
                            select (int)dr["TotalRecords"]).FirstOrDefault();

            CurrentPage = (from DataRow dr in dt.Rows
                           select (int)dr["CurrentPage"]).FirstOrDefault();

            //        PageSize = (from DataRow dr in dt.Rows
            //                   select (int)dr["RecordsperPage"]).FirstOrDefault();

            lblDebug.Text = "Criteria: " + SearchText.ToString()
                + "  <br />Searching: " + ddlSearchType.SelectedItem.ToString()
                + "  <br />Order By: " + ddlOrderBy.SelectedItem.ToString()
                + "<br />Total Records: " + TotalRecords.ToString();

            //           	 Number Of Pages	
            grdUsers.DataSource = dt;
            grdUsers.DataBind();

            ctlPagingControl.TotalRecords = TotalRecords;
            ctlPagingControl.PageSize = PageSize;
            ctlPagingControl.CurrentPage = CurrentPage;
            ctlPagingControl.QuerystringParams = strQuerystring;
            ctlPagingControl.TabID = TabId;

        }


        protected void dnnGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {

            grdUsers.CurrentPageIndex = e.NewPageIndex;
            BindData();

        }



        protected string FilterURL(string Filter, string CurrentPage)
        {
            string _URL = Null.NullString;

            _URL = Globals.NavigateURL(TabId, "", "filter=" + Filter, "currentpage=1", "SearchField=" + ddlSearchType.SelectedValue.ToString(), "OrderBy=" + ddlOrderBy.SelectedValue.ToString());

            return _URL;
        }

        //public string DisplayAddress(object Unit, object Street, object City, object Region, object Country, object PostalCode) 
        //{              
        //    string _Address = Null.NullString;              
        //    try             
        //    {                  
        //        _Address = Globals.FormatAddress("", Street, City, Region, "", PostalCode);              
        //    }              
        //    catch (Exception exc) //Module failed to load             
        //    {                  
        //        Exceptions.ProcessModuleLoadException(this, exc);             
        //    }              
        //    return _Address;          
        //}  

        //public string DisplayEmail(string Email)          
        //{             
        //    string _Email = Null.NullString;             
        //    try             
        //    {                  
        //        if (Email  != null)                 
        //        {                     
        //            _Email = HtmlUtils.FormatEmail(Email);                 
        //        }              
        //    }              
        //    catch (Exception exc) //Module failed to load             
        //    {                  
        //        Exceptions.ProcessModuleLoadException(this, exc);              
        //    }             
        //    return _Email;          
        //}  

        //public string DisplayDate(System.DateTime UserDate)         
        //{              
        //    string _Date = Null.NullString;              
        //    try              
        //    {                  
        //        if (!(Null.IsNull(UserDate)))                  
        //        {                      
        //            _Date = UserDate.ToString();                  
        //        }                  
        //        else                 
        //        {                      
        //            _Date = "";                 
        //        }             
        //    }              
        //    catch (Exception exc) //Module failed to load             
        //    {                  
        //        Exceptions.ProcessModuleLoadException(this, exc);              
        //    }             
        //    return _Date;          
        //}




        protected string GetEditUrl(object donorID)
        {
            try
            {
                // build the URL and return it.
                string _navURL = Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "Edit", "mid=" + ModuleId.ToString() + "&UserId=" + donorID);
                return _navURL.ToString();
            }
            catch (Exception exc) //Module failed to load             
            {
                Exceptions.ProcessModuleLoadException(this, exc);
                return "";
            }
        }


        protected string GetStateLookup(object regionID)
        {
            try
            {
                string _state = "";
                int n;
                bool isNumeric = int.TryParse(regionID.ToString(), out n);

                if (isNumeric)
                {
                    // Get State from DNN Lists
                    ListController ctlList = new ListController();
                    ListEntryInfo vStateLookup = ctlList.GetListEntryInfo(Convert.ToInt32(regionID));

                    //_state = vStateLookup.Text.ToString();
                    _state = vStateLookup.Value.ToString();
                    return _state.ToString();
                }
                else
                {
                    return regionID.ToString();
                }


            }
            catch (Exception exc) //Module failed to load             
            {
                Exceptions.ProcessModuleLoadException(this, exc);
                return "ERROR ON LOOKUP";
            }
        }




        public void GoToEditPage(string _NavURL)
        {

            try
            {
                Response.Redirect(_NavURL.ToString());
            }
            catch (Exception exc) //Module failed to load             
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }

        }

        //public string GetProfileValue(int _portalID, int _userID)
        //{
        //    string _CompanyName = Null.NullString;
        //    try
        //    {
        //        UserInfo objInfo = new UserInfo(); // DNN Classes
        //        objInfo = UserController.GetUserById(_portalID, _userID);
        //        if (objInfo != null)
        //        {
        //            _CompanyName = objInfo.Profile.GetPropertyValue("Company");
        //        }
        //    }
        //    catch (Exception exc) //Module failed to load             
        //    {
        //        Exceptions.ProcessModuleLoadException(this, exc);
        //    }
        //    return _CompanyName;
        //}


        private void CreateLetterSearch()
        {
            string filters = Localization.GetString("Filter.Text", this.LocalResourceFile);
            //filters += "," + Localization.GetString("All");
            //filters += "," + Localization.GetString("Donors Only"); 
            //    filters += "," + Localization.GetString("OnLine"); 
            //   filters += "," + Localization.GetString("Unauthorized"); 
            string[] strAlphabet = filters.Split(',');
            rptLetterSearch.DataSource = strAlphabet;
            rptLetterSearch.DataBind();
        }


        private ListItem AddSearchItem(string name)
        {
            string propertyName = Null.NullString;
            if (Request.QueryString["filterProperty"] != null)
            {
                propertyName = Request.QueryString["filterProperty"];
            }
            string text = Localization.GetString(name, this.LocalResourceFile);
            if (text == "")
            { text = name; }
            ListItem li = new ListItem(text, name);
            if (name == propertyName)
            {
                li.Selected = true;
            }
            return li;
        }


        protected string FormatURL(string strKeyName, string strKeyValue)
        {
            string _URL = Null.NullString;
            try
            {
                if (Filter != "")
                {
                    _URL = EditUrl(strKeyName, strKeyValue, "", "filter=" + Filter);
                }
                else
                {
                    _URL = EditUrl(strKeyName, strKeyValue);
                }
            }
            catch (Exception exc) //Module failed to load              
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
            return _URL;
        }

        #region IActionable Members

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                //create a new action to add an item, this will be added to the controls
                //dropdown menu
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
                    ModuleActionType.AddContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                     true, false);

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

        protected void btnAddNewUser_Click(object sender, EventArgs e)
        {
            //  Response.Redirect(Globals.NavigateURL(this.TabId, EditUrl(), ""), true);
            Response.Redirect(EditUrl("TabFocus", "DonorRecord"));
        }

        protected void btnReports_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ReportSummary"));
        }

        protected void btnReportSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ReportSearch"));
        }

        protected void btnSources_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ListDrives"));
        }

        protected void btnLetters_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ListLetters"));
        }

        /// <summary>
        /// Handles the items being bound to the datalist control. In this method we merge the data with the
        /// template defined for this control to produce the result to display to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


    }
}