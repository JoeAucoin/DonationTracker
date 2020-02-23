using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

using DotNetNuke.Entities.Modules.Definitions;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Modules.Communications;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;
using GIBS.DonationTracker.Components;
using GIBS.Modules.DonationTracker.Components;
using GIBS.Modules.DonationTracker;
using GIBS.DonationTracker;
using System.Data;
using DotNetNuke.UI.Skins;
using Globals = DotNetNuke.Common.Globals;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace GIBS.Modules.DonationTracker
{
    public partial class ListLetters : PortalModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                FillGrid();
              
            }
        }




        public void FillGrid()
        {

            try
            {
                int showInactive = 1;
                if (cbxShowInActive.Checked == true)
                {
                    showInactive  = 0;
                }

                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();
                
                items = controller.DonationTrackerLetterTemplate_List(this.ModuleId, showInactive);


                //bind the data
                GridView1.DataSource = items;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        // GridView1_RowEditing
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            PanelGrid.Visible = false;
            PanelLetterDetail.Visible = true;
            
            int itemID = (int)GridView1.DataKeys[e.NewEditIndex].Value;

            DonationTrackerController controller = new DonationTrackerController();
            DonationTrackerInfo item = controller.DonationTrackerLetterTemplate_Get(itemID);

            if (item != null)
            {
                //  Response.Write(item);

                txtLetterName.Text = item.LetterName;
                txtLetter.Text = item.Letter;

                isActive.SelectedIndex = Convert.ToInt32(item.IsActive);
                HiddenLetterID.Value = item.LetterID.ToString();

            }
            else
            {
                HiddenLetterID.Value = "";
                
            }

        }



        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int itemID = (int)GridView1.DataKeys[e.RowIndex].Value;

            DonationTrackerController controller = new DonationTrackerController();
            controller.DonationTrackerLetterTemplate_Delete(itemID);



        }


        // PRINT VIEW   
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int itemID = (int)GridView1.DataKeys[e.RowIndex].Value;
            //     Response.Redirect(Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "Edit", "SkinSrc=[G]" + Globals.QueryStringEncode(SkinController.RootSkin + "/" + Globals.glbHostSkinFolder + "/" + "No Skin") + "&mid=" + ModuleId.ToString() + "&ItemId=" + itemID), true);

            // string MyURL =  Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "Edit", "SkinSrc=[G]" + Globals.QueryStringEncode(SkinController.RootSkin + "/" + Globals.glbHostSkinFolder + "/" + "No Skin") + "&mid=" + ModuleId.ToString() + "&ItemId=" + itemID);

        }

        protected void cmdAddRecord_Click(object sender, EventArgs e)
        {
            PanelGrid.Visible = false;
            PanelLetterDetail.Visible = true;
            //string path = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtDriveDate);
            //CalendarImage.Attributes.Add("OnClick", path);
        }

        protected void cmdEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = new DonationTrackerInfo();

                item.LetterName = txtLetterName.Text.ToString();



                item.Letter = txtLetter.Text.ToString();

                item.CreatedByUserID = this.UserId;
                item.IsActive = Convert.ToBoolean(isActive.SelectedValue.ToString());
               


                if (HiddenLetterID.Value.Length > 0)
                {
                    item.LetterID = Int32.Parse(HiddenLetterID.Value.ToString());
                    controller.DonationTrackerLetterTemplate_Update(item);
                }
                else
                {
                    item.ModuleId = this.ModuleId;
                    controller.DonationTrackerLetterTemplate_Insert(item);
                }

                //FillGrid();
                //PanelGrid.Visible = true;
                //PanelDriveDetail.Visible = false;
                Response.Redirect(EditUrl("ListLetters"));



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

        protected void cmdAddCancel_Click(object sender, EventArgs e)
        {

            try
            {
                Response.Redirect(EditUrl("ListLetters"));
                //PanelGrid.Visible = true;
                //PanelDriveDetail.Visible = false;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        protected void cbxShowInActive_CheckedChanged(object sender, EventArgs e)
        {
            FillGrid();
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



                return actions;
            }
        }

        #endregion
    }
}