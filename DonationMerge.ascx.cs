using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Data;
using DotNetNuke.Framework.JavaScriptLibraries;
using GIBS.DonationTracker.Components;
using System.Text.RegularExpressions;

namespace GIBS.Modules.DonationTracker
{
    public partial class DonationMerge : PortalModuleBase
    {

        public DataTable dt;
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // lblMsg.Text = Request.QueryString["clientid"];
            try
            {
                if (IsPostBack == false)
                {
                    LoadDropdowns();
                    LoadSettings();
                    if (Request.QueryString["userid"] != null)
                    {
                        hidClientIDMaster.Value = Request.QueryString["userid"].ToString();


                    }
                }
                LoadRecord(Int32.Parse(hidClientIDMaster.Value.ToString()));
 //               LoadMaster();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);
            JavaScript.RequestRegistration(CommonJs.DnnPlugins);
        }
        public void LoadDropdowns()
        {
            try
            {
                ddlSearchType.Items.Add("FirstName");
                ddlSearchType.Items.Add("LastName");
                ddlSearchType.Items.Add("Company");
                ddlSearchType.Items.Add("City");
                ddlSearchType.Items.Add("State");

                ddlSearchType.SelectedValue = "LastName";
               

                ddlOrderBy.Items.Add("FirstName");
                ddlOrderBy.Items.Add("LastName");
                ddlOrderBy.Items.Add("Company");
                ddlOrderBy.Items.Add("City");
                ddlOrderBy.Items.Add("State");

                ddlOrderBy.SelectedValue = "LastName";
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        public void LoadRecord(int RecordID)
        {
            try
            {

                DotNetNuke.Entities.Users.UserInfo DonationUser = DotNetNuke.Entities.Users.UserController.GetUserById(this.PortalId, RecordID);

                SetPageName(DonationUser.Profile.GetPropertyValue("Company") + " - " + DonationUser.FirstName + " " + DonationUser.LastName);


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
                _PrimaryAddress = Regex.Replace(_PrimaryAddress.ToString(), @"\r\n?|\n", "<br />");
                lblMasterClient.Text = _PrimaryAddress.ToString();


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void SetPageName(string PageName)
        {
            try
            {
                this.ModuleConfiguration.ModuleControl.ControlTitle = PageName.ToString();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        //public void LoadChild()
        //{
        //    try
        //    {
        //        //ClientAge
        //        GridViewSearchChild.Columns[8].Visible = true;
        //        //ClientDOB
        //        GridViewSearchChild.Columns[9].Visible = true;
        //        List<DonationTrackerInfo> items;
        //        DonationTrackerController controller = new DonationTrackerController();
        //        //items = controller.FBClients_Search(this.PortalId, txtLastName.Text.ToString().Replace("'", "''").Trim(),
        //        //    txtClientIdCard.Text.ToString().Trim(),
        //        //    txtFirstName.Text.ToString().Replace("'", "''").Trim(),
        //        //    txtClientId.Text.ToString(),
        //        //    txtAddress.Text.ToString(),
        //        //    ddlCity.SelectedValue.ToString());
        //        //GridViewSearchChild.DataSource = items;
        //        //GridViewSearchChild.DataBind();
        //        ////ClAddFamMemFirstName
        //        //GridViewSearchChild.Columns[14].Visible = false;
        //        ////ClAddFamMemLastName
        //        //GridViewSearchChild.Columns[15].Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.ProcessModuleLoadException(this, ex);
        //    }

        //}



        public void BindData(string SearchText, string SearchField, string ordeByField, string orderByDirection)
        {

            orderByDirection = "asc";
            string strQuerystring = Null.NullString;



            //if (Filter != "")
            //{
            //    strQuerystring += "filter=" + Filter;
            //}

            DonationTrackerController controller = new DonationTrackerController(); //ref TotalRecords,
            List<DonationTrackerInfo> Users11 = new List<DonationTrackerInfo>();
            //  UserController Users = new UserController();

            //Joe this
            Users11 = controller.DonationTrackerUserFullListSearch(this.PortalId, 1, 1000, SearchField, SearchText, ordeByField, orderByDirection);

            //if (TotalRecords == 0)
            //{
            //    DonationTrackerInfo item = controller.DonationTrackerUserSearchRecordCount(DonorPortal, CurrentPage - 1, PageSize, SearchField, SearchText, ordeByField, orderByDirection);

            //    TotalRecords = item.TotalRecords;
            //}
            //

            lblSearchCriteria.Text = "Criteria: " + SearchText.ToString() + "  <br />SearchField: " + SearchField.ToString() + "  <br />Order By:" + ordeByField.ToString();


            dt = Components.GridViewTools.ToDataTable(Users11);



            //GridView1.DataSource = dt;

            //GridView1.DataBind();



            GridViewSearchChild.DataSource = dt;
            GridViewSearchChild.DataBind();

            //ctlPagingControl.TotalRecords = TotalRecords;
            //ctlPagingControl.PageSize = PageSize;
            //ctlPagingControl.CurrentPage = CurrentPage;
            //ctlPagingControl.QuerystringParams = strQuerystring;
            //ctlPagingControl.TabID = TabId;

        }



        protected void GridViewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DonationTrackerController controller = new DonationTrackerController();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblLastVisitDate = (Label)e.Row.FindControl("lblLastVisitDate");
                //FBClientsInfo item = controller.FBClients_Visit_GetClientLastVisitDate(_clientid);
                //if (item != null)
                //{
                //  lblLastVisitDate.Text = item.LastVisitDate.ToShortDateString();
                //}
            }
        }
        public void LoadSettings()
        {
            try
            {
                //FBClientsSettings settingsData = new FBClientsSettings(this.TabModuleId);
                //if (settingsData.FocusableControl != null)
                //{
                //    TextBox _SearchField = (TextBox)FindControl(settingsData.FocusableControl.ToString());
                //    if (_SearchField != null)
                //    {
                //        _SearchField.Focus();
                //    }
                //}

                //if (settingsData.ShowClientIdCard != null)
                //{
                //    divClientIDCard.Visible = Convert.ToBoolean(settingsData.ShowClientIdCard);
                //}
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        protected void GridViewSearch_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //try
            //{
            //    // USED TO GIVE A RETURN LINK TO THE SEARCH RESULTS
            //    string SearchParams = "";

            //    if (txtClientIdCard.Text.ToString().Length > 0)
            //    {
            //        SearchParams += "&cidcard=" + txtClientIdCard.Text.ToString().Trim();
            //    }
            //    if (txtFirstName.Text.ToString().Length > 0)
            //    {
            //        SearchParams = "&fname=" + txtFirstName.Text.ToString().Trim();
            //    }
            //    if (txtLastName.Text.ToString().Length > 0)
            //    {
            //        SearchParams = "&lname=" + txtLastName.Text.ToString().Trim();
            //    }
            //    if (txtAddress.Text.ToString().Length > 0)
            //    {
            //        SearchParams = "&address=" + txtAddress.Text.ToString().Trim();
            //    }

            //    if (txtClientId.Text.ToString().Length > 0)
            //    {
            //        SearchParams += "&clientid=" + txtClientId.Text.ToString().Trim();
            //    }
            //    int clientID = (int)GridViewSearchMaster.DataKeys[e.NewEditIndex].Value;
            //}
            //catch (Exception ex)
            //{
            //    Exceptions.ProcessModuleLoadException(this, ex);
            //}

        }
        protected void GridViewSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
              //  LoadChild();
                GridViewSearchChild.PageIndex = e.NewPageIndex;
                GridViewSearchChild.DataBind();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lblFormMessage.Visible = false;
                lblMsg.Text = "";
                BindData(txtSearch.Text.ToString(), ddlSearchType.SelectedValue.ToString(), ddlOrderBy.SelectedValue.ToString(), "asc");
             //   LoadChild();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
        protected void Merge(int MasterClientID, int ChildClientID)
        {

            DonationTrackerController controller = new DonationTrackerController();
            int result = controller.DonationTracker_Merge(MasterClientID, ChildClientID);
            if (result == 1)
            {

                lblMsg.Text = "Merge was done.";

            }
            else
            {
                lblMsg.Text = "Merge blew up.";


            }
        }
        protected void GridViewSearchChild_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //Get the button that raised the event
            Button btn = (Button)sender;
            
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //lblMsg.Text = gvr.Cells[1].Text;
            if (Convert.ToInt32(hidClientIDMaster.Value) != Convert.ToInt32(gvr.Cells[0].Text))
            {
                Merge(Convert.ToInt32(hidClientIDMaster.Value), Convert.ToInt32(gvr.Cells[0].Text));
                BindData(txtSearch.Text.ToString(), ddlSearchType.SelectedValue.ToString(), ddlOrderBy.SelectedValue.ToString(), "desc");
            }
            else
            {
                lblMsg.Text = "You can't merge a record unto itself. Nice try tho. Please select another record.";
            }
        }
        protected void GridViewSearchChild_SelectedIndexView(object sender, System.EventArgs e)
        {
            //Get the button that raised the event
            Button btn = (Button)sender;
            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            //lblMsg.Text = gvr.Cells[1].Text;
            // Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "EditClient", "mid=" + ModuleId.ToString() + "&cid=" + gvr.Cells[1].Text));
        }
   
    }
}