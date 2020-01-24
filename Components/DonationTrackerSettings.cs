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
    public class DonationTrackerSettings
    {
        ModuleController controller;
        int tabModuleId;

        public DonationTrackerSettings(int tabModuleId)
        {
            controller = new ModuleController();
            this.tabModuleId = tabModuleId;
        }

        protected T ReadSetting<T>(string settingName, T defaultValue)
        {
            Hashtable settings = controller.GetTabModuleSettings(this.tabModuleId);
         

            T ret = default(T);

            if (settings.ContainsKey(settingName))
            {
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                try
                {
                    ret = (T)tc.ConvertFrom(settings[settingName]);
                }
                catch
                {
                    ret = defaultValue;
                }
            }
            else
                ret = defaultValue;

            return ret;
        }

        protected void WriteSetting(string settingName, string value)
        {
            controller.UpdateTabModuleSetting(this.tabModuleId, settingName, value);
        }

        #region public properties

        /// <summary>
        /// get/set template used to render the module content
        /// to the user
        /// </summary>
        public string NumPerPage
        {
            get { return ReadSetting<string>("numPerPage", null); }
            set { WriteSetting("numPerPage", value); }
        }

        public string RoleName
        {
            get { return ReadSetting<string>("roleName", null); }
            set { WriteSetting("roleName", value); }
        }

        public string ReportsRole
        {
            get { return ReadSetting<string>("reportsRole", null); }
            set { WriteSetting("reportsRole", value); }
        }

        public string MergeRole
        {
            get { return ReadSetting<string>("mergeRole", null); }
            set { WriteSetting("mergeRole", value); }
        }	

        public string EmailFrom
        {
            get { return ReadSetting<string>("emailFrom", null); }
            set { WriteSetting("emailFrom", value); }
        }

        public string EmailBCC
        {
            get { return ReadSetting<string>("emailBCC", null); }
            set { WriteSetting("emailBCC", value); }
        }

        public string EmailSubject
        {
            get { return ReadSetting<string>("emailSubject", null); }
            set { WriteSetting("emailSubject", value); }
        }

        public string EmailMessage
        {
            get { return ReadSetting<string>("emailMessage", null); }
            set { WriteSetting("emailMessage", value); }
        }

        public string ShowSendPassword
        {
            get { return ReadSetting<string>("showSendPassword", null); }
            set { WriteSetting("showSendPassword", value); }
        }

        public string EmailNewUserCredentials
        {
            get { return ReadSetting<string>("emailNewUserCredentials", null); }
            set { WriteSetting("emailNewUserCredentials", value); }
        }

        public string ShowDonationHistory
        {
            get { return ReadSetting<string>("showDonationHistory", null); }
            set { WriteSetting("showDonationHistory", value); }
        }

        public string EnableAddNewDonor
        {
            get { return ReadSetting<string>("enableAddNewDonor", null); }
            set { WriteSetting("enableAddNewDonor", value); }
        }


        public string ReportServerURL
        {
            get { return ReadSetting<string>("reportServerURL", null); }
            set { WriteSetting("reportServerURL", value); }
        }

        public string ReportPath
        {
            get { return ReadSetting<string>("reportPath", null); }
            set { WriteSetting("reportPath", value); }
        }

        public string ReportCredentialsUserName
        {
            get { return ReadSetting<string>("reportCredentialsUserName", null); }
            set { WriteSetting("reportCredentialsUserName", value); }
        }

        public string ReportCredentialsPassword
        {
            get { return ReadSetting<string>("reportCredentialsPassword", null); }
            set { WriteSetting("reportCredentialsPassword", value); }
        }

        public string ReportCredentialsDomain
        {
            get { return ReadSetting<string>("reportCredentialsDomain", null); }
            set { WriteSetting("reportCredentialsDomain", value); }
        }


        #endregion
    }
}
