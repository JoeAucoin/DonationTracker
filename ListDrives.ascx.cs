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
using GIBS.Modules.DonationTracker;
using GIBS.DonationTracker;
using System.Data;
using DotNetNuke.UI.Skins;
using Globals = DotNetNuke.Common.Globals;
using DotNetNuke.Framework.JavaScriptLibraries;

namespace GIBS.Modules.DonationTracker
{
    public partial class ListDrives : PortalModuleBase, IActionable
    {



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            JavaScript.RequestRegistration(CommonJs.jQuery);
            JavaScript.RequestRegistration(CommonJs.jQueryUI);
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "InputMasks", (this.TemplateSourceDirectory + "/JavaScript/jquery.maskedinput-1.3.js"));
            Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "Watermark", (this.TemplateSourceDirectory + "/JavaScript/jquery.watermarkinput.js"));

        }
        
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                FillGrid();
                txtDriveDate.Text = DateTime.Today.ToShortDateString();
            }
            
           
        }



        public void FillGrid()
        {

            try
            {

 

                List<DonationTrackerInfo> items;
                DonationTrackerController controller = new DonationTrackerController();
              //  GiftCertController controller = new GiftCertController();

               // items = controller.GetGiftCerts(this.ModuleId);

                items = controller.DonationTrackerGetDrives(this.ModuleId, 0);

               // items = items.OrderBy("DriveDate","desc").ToList();
               // var so = from controller s in items orderby s.DriveDate select s;

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
            PanelDriveDetail.Visible = true;
            
            int itemID = (int)GridView1.DataKeys[e.NewEditIndex].Value;

            DonationTrackerController controller = new DonationTrackerController();
            DonationTrackerInfo item = controller.DonationTrackerGetDrive(this.ModuleId, itemID);

            if (item != null)
            {
                //  Response.Write(item);
                txtDriveName.Text = item.DriveName;
                txtDriveDate.Text = item.DriveDate.ToShortDateString();
                txtDriveNotes.Text = item.Description;
                txtReminder.Text = item.Reminder;
                txtPledgeLetter.Text = item.PledgeLetter;
                isActive.SelectedIndex = Convert.ToInt32(item.IsActive);
                HiddenItemID.Value = item.DriveID.ToString();

            }
            else
            {
                HiddenItemID.Value = "";
            }

        }



        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int itemID = (int)GridView1.DataKeys[e.RowIndex].Value;

            //GiftCertController controller = new GiftCertController();

            //controller.DeleteGiftCert(this.ModuleId, itemID);
            //FillGrid();

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
            PanelDriveDetail.Visible = true;
            //string path = DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtDriveDate);
            //CalendarImage.Attributes.Add("OnClick", path);
        }

        protected void cmdEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                DonationTrackerController controller = new DonationTrackerController();
                DonationTrackerInfo item = new DonationTrackerInfo();

                item.DriveName = txtDriveName.Text;

                DateTime dt = DateTime.Parse(txtDriveDate.Text);
                item.DriveDate = dt;

                item.Description = txtDriveNotes.Text;
                item.Reminder = txtReminder.Text;
                item.PledgeLetter = txtPledgeLetter.Text;
                item.CreatedByUserID = this.UserId;
                item.IsActive = Convert.ToBoolean(isActive.SelectedValue.ToString());
                item.ModuleId = this.ModuleId; 
                
                
                if (HiddenItemID.Value.Length > 0)
                {
                    item.DriveID = Int32.Parse(HiddenItemID.Value.ToString());
                    controller.DonationTrackerUpdateDrive(item);
                }
                else
                {
                    controller.DonationTrackerAddDrive(item);
                }

                //FillGrid();
                //PanelGrid.Visible = true;
                //PanelDriveDetail.Visible = false;
                Response.Redirect(EditUrl("ListDrives"));
                


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
                Response.Redirect(EditUrl("ListDrives"));
                //PanelGrid.Visible = true;
                //PanelDriveDetail.Visible = false;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
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