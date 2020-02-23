using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common;

namespace GIBS.DonationTracker.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class DonationTrackerSettings: ModuleSettingsBase 
    {
   

        #region public properties

        /// <summary>
        /// get/set template used to render the module content
        /// to the user
        /// </summary>



        public string NumPerPage
        {
            get
            {
                if (Settings.Contains("numPerPage"))
                    return Settings["numPerPage"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "numPerPage", value.ToString());
            }
        }

        public string RoleName
        {
            get
            {
                if (Settings.Contains("roleName"))
                    return Settings["roleName"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "roleName", value.ToString());
            }
        }

        public string ReportsRole
        {
            get
            {
                if (Settings.Contains("reportsRole"))
                    return Settings["reportsRole"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "reportsRole", value.ToString());
            }
        }

        public string MergeRole
        {
            get
            {
                if (Settings.Contains("mergeRole"))
                    return Settings["mergeRole"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "mergeRole", value.ToString());
            }
        }

        public string EmailFrom
        {
            get
            {
                if (Settings.Contains("emailFrom"))
                    return Settings["emailFrom"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "emailFrom", value.ToString());
            }
        }

        public string EmailBCC
        {
            get
            {
                if (Settings.Contains("emailBCC"))
                    return Settings["emailBCC"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "emailBCC", value.ToString());
            }
        }

        public string EmailSubject
        {
            get
            {
                if (Settings.Contains("emailSubject"))
                    return Settings["emailSubject"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "emailSubject", value.ToString());
            }
        }

        public string EmailMessage
        {
            get
            {
                if (Settings.Contains("emailMessage"))
                    return Settings["emailMessage"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "emailMessage", value.ToString());
            }
        }

        public string ShowSendPassword
        {
            get
            {
                if (Settings.Contains("showSendPassword"))
                    return Settings["showSendPassword"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "showSendPassword", value.ToString());
            }
        }

        public string EmailNewUserCredentials
        {
            get
            {
                if (Settings.Contains("emailNewUserCredentials"))
                    return Settings["emailNewUserCredentials"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "emailNewUserCredentials", value.ToString());
            }
        }

        public string ShowDonationHistory
        {
            get
            {
                if (Settings.Contains("showDonationHistory"))
                    return Settings["showDonationHistory"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "showDonationHistory", value.ToString());
            }
        }

        public string EnableAddNewDonor
        {
            get
            {
                if (Settings.Contains("enableAddNewDonor"))
                    return Settings["enableAddNewDonor"].ToString();
                return "";
            }
            set
            { 
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "enableAddNewDonor", value.ToString());
            }
        }

        public string ReportServerURL
        {
            get
            {
                if (Settings.Contains("reportServerURL"))
                    return Settings["reportServerURL"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "reportServerURL", value.ToString());
            }
        }

        public string ReportPath
        {
            get
            {
                if (Settings.Contains("reportPath"))
                    return Settings["reportPath"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "reportPath", value.ToString());
            }
        }

        public string ReportCredentialsUserName
        {
            get
            {
                if (Settings.Contains("reportCredentialsUserName"))
                    return Settings["reportCredentialsUserName"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "reportCredentialsUserName", value.ToString());
            }
        }

        public string ReportCredentialsPassword
        {
            get
            {
                if (Settings.Contains("reportCredentialsPassword"))
                    return Settings["reportCredentialsPassword"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "reportCredentialsPassword", value.ToString());
            }
        }

        public string ReportCredentialsDomain
        {
            get
            {
                if (Settings.Contains("reportCredentialsDomain"))
                    return Settings["reportCredentialsDomain"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateTabModuleSetting(TabModuleId, "reportCredentialsDomain", value.ToString());
            }
        }

        #endregion
    }
}
