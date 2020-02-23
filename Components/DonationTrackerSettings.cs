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
                if (Settings.Contains("NumPerPage"))
                    return Settings["NumPerPage"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "NumPerPage", value.ToString());
            }
        }

        public string RoleName
        {
            get
            {
                if (Settings.Contains("RoleName"))
                    return Settings["RoleName"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "RoleName", value.ToString());
            }
        }

        public string ReportsRole
        {
            get
            {
                if (Settings.Contains("ReportsRole"))
                    return Settings["ReportsRole"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ReportsRole", value.ToString());
            }
        }

        public string MergeRole
        {
            get
            {
                if (Settings.Contains("MergeRole"))
                    return Settings["MergeRole"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "MergeRole", value.ToString());
            }
        }

        public string EmailFrom
        {
            get
            {
                if (Settings.Contains("EmailFrom"))
                    return Settings["EmailFrom"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "EmailFrom", value.ToString());
            }
        }

        public string EmailBCC
        {
            get
            {
                if (Settings.Contains("EmailBCC"))
                    return Settings["EmailBCC"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "EmailBCC", value.ToString());
            }
        }

        public string EmailSubject
        {
            get
            {
                if (Settings.Contains("EmailSubject"))
                    return Settings["EmailSubject"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "EmailSubject", value.ToString());
            }
        }

        public string EmailMessage
        {
            get
            {
                if (Settings.Contains("EmailMessage"))
                    return Settings["EmailMessage"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "EmailMessage", value.ToString());
            }
        }

        public string ShowSendPassword
        {
            get
            {
                if (Settings.Contains("ShowSendPassword"))
                    return Settings["ShowSendPassword"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ShowSendPassword", value.ToString());
            }
        }

        public string EmailNewUserCredentials
        {
            get
            {
                if (Settings.Contains("EmailNewUserCredentials"))
                    return Settings["EmailNewUserCredentials"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "EmailNewUserCredentials", value.ToString());
            }
        }

        public string ShowDonationHistory
        {
            get
            {
                if (Settings.Contains("ShowDonationHistory"))
                    return Settings["ShowDonationHistory"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ShowDonationHistory", value.ToString());
            }
        }

        public string EnableAddNewDonor
        {
            get
            {
                if (Settings.Contains("EnableAddNewDonor"))
                    return Settings["EnableAddNewDonor"].ToString();
                return "";
            }
            set
            { 
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "EnableAddNewDonor", value.ToString());
            }
        }

        public string ReportServerURL
        {
            get
            {
                if (Settings.Contains("ReportServerURL"))
                    return Settings["ReportServerURL"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ReportServerURL", value.ToString());
            }
        }

        public string ReportPath
        {
            get
            {
                if (Settings.Contains("ReportPath"))
                    return Settings["ReportPath"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ReportPath", value.ToString());
            }
        }

        public string ReportCredentialsUserName
        {
            get
            {
                if (Settings.Contains("ReportCredentialsUserName"))
                    return Settings["ReportCredentialsUserName"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ReportCredentialsUserName", value.ToString());
            }
        }

        public string ReportCredentialsPassword
        {
            get
            {
                if (Settings.Contains("ReportCredentialsPassword"))
                    return Settings["ReportCredentialsPassword"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ReportCredentialsPassword", value.ToString());
            }
        }

        public string ReportCredentialsDomain
        {
            get
            {
                if (Settings.Contains("ReportCredentialsDomain"))
                    return Settings["ReportCredentialsDomain"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "ReportCredentialsDomain", value.ToString());
            }
        }

        #endregion
    }
}
