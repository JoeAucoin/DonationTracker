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
    public partial class ReportSearch : PortalModuleBase, IActionable
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
                txtStartDate.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                txtEndDate.Text = DateTime.Today.ToShortDateString();
                txtDonationAmount.Text = "0.00";
                FillDetailGrid();

            }
        }


        public void FillDetailGrid()
        {

            try
            {

                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();

                items = controller.DonationTrackerReportSearch(this.ModuleId, txtStartDate.Text.ToString(), txtEndDate.Text.ToString(), double.Parse(txtDonationAmount.Text.ToString()));

                GridViewDetailReport.Columns[0].Visible = true;
                GridViewDetailReport.DataSource = items;
                GridViewDetailReport.DataBind();


                lblDetails.Text = "Searching From " + txtStartDate.Text.ToString() + " to " + txtEndDate.Text.ToString();
                //    GridViewDetailReport.Rows[0].Cells[0].Visible = false;
                GridViewDetailReport.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            FillDetailGrid();

        }



        protected void cmdExport_Click(object sender, EventArgs e)
        {
            ExportGridView();
        }

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

        private void ExportGridView()
        {
            string reportName = string.Format("rpt-{0:yyyy-MM-dd_hh-mm-ss-tt}.xls", DateTime.Now);
            GridViewExportUtil.Export(reportName.ToString(), this.GridViewDetailReport);


        }


        protected void GridViewSummaryReport_RowDataBound(object sender, GridViewRowEventArgs e)
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



                actions.Add(GetNextActionID(), Localization.GetString("ListDrives", this.LocalResourceFile),
 ModuleActionType.AddContent, "", "", EditUrl("ListDrives"), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
 true, false);

                return actions;
            }
        }

    }
}