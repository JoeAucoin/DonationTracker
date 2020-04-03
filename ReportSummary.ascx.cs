using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using GIBS.DonationTracker.Components;
using DotNetNuke.Entities.Profile;
using Globals = DotNetNuke.Common.Globals;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.WebControls;
using System.IO;
using System.Web.UI.HtmlControls;
using GIBS.Modules.DonationTracker.Components;
using DotNetNuke.Framework.JavaScriptLibraries;  

namespace GIBS.Modules.DonationTracker
{
    public partial class ReportSummary : PortalModuleBase, IActionable
    {
       
        
        decimal total = 0;
        decimal totaldetail = 0;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);
       
           
        }


        protected void Page_Load(object sender, EventArgs e)
        {      

            if (!IsPostBack)
            {
                //DateTime dt = DateTime.Now;
                //dt.Subtract(
                txtStartDate.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                txtEndDate.Text = DateTime.Today.ToShortDateString();
                

                FillGrid();
               

            }

        }


        public void FillGrid()
        {

            try
            {

                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();

                items = controller.DonationTrackerReportSummary(this.ModuleId, txtStartDate.Text.ToString(), txtEndDate.Text.ToString());

                GridViewSummaryReport.DataSource = items;
                GridViewSummaryReport.DataBind();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public void FillDetailGrid(int driveID)
        {

            try
            {

                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();

                items = controller.DonationTrackerReportDetail(this.ModuleId, driveID, txtStartDate.Text.ToString(), txtEndDate.Text.ToString());

                GridViewDetailReport.Columns[0].Visible = true;
                GridViewDetailReport.DataSource = items;
                GridViewDetailReport.DataBind();


                lblDetails.Text = "Details for " + GridViewDetailReport.Rows[0].Cells[0].Text.ToString() + "<br />From " + txtStartDate.Text.ToString() + " to " + txtEndDate.Text.ToString();
            //    GridViewDetailReport.Rows[0].Cells[0].Visible = false;
                GridViewDetailReport.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        private void ExportGridView()
        {

            GridViewExportUtil.Export("DonationReport.xls", this.GridViewDetailReport);


        }


        protected  void GridViewSummaryReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {

                


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DonationAmount"));
                  //  grdTotal = grdTotal + rowTotal;

                }


                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lbl = (Label)e.Row.FindControl("lblTotal");
                    lbl.Text = total.ToString("c");
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        // DETAIL REPORT
        protected void GridViewDetailReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    totaldetail += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DonationAmount"));

                }


                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lbl = (Label)e.Row.FindControl("lblTotal");
                    lbl.Text = totaldetail.ToString("c");
                }


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        protected void GridViewDonations_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                PanelDetails.Visible = true;
                PanelSummary.Visible = false;
                
                int driveID = (int)GridViewSummaryReport.DataKeys[e.NewEditIndex].Value;
                FillDetailGrid(driveID);


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }


        }


        protected void GridViewDetail_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }


        }



        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillGrid();
        }

        protected void cmdReturnToSummary_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl("ReportSummary"));
        }

        protected void cmdExport_Click(object sender, EventArgs e)
        {
            ExportGridView();
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

        protected void GridViewDetailReport_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnReturn_Click(object sender, EventArgs e)
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

    }
}