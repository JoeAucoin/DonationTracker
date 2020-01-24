using System;
using System.Data;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace GIBS.DonationTracker.Components
{
    public class SqlDataProvider : DataProvider
    {


        #region vars

        private const string providerType = "data";
        private const string moduleQualifier = "GIBS_";

        private ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
        private string connectionString;
        private string providerPath;
        private string objectQualifier;
        private string databaseOwner;
     //   private string startDate;
     //   private string endDate;

        #endregion

        #region cstor

        /// <summary>
        /// cstor used to create the sqlProvider with required parameters from the configuration
        /// section of web.config file
        /// </summary>
        public SqlDataProvider()
        {
            Provider provider = (Provider)providerConfiguration.Providers[providerConfiguration.DefaultProvider];
            connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString();

            if (connectionString == string.Empty)
                connectionString = provider.Attributes["connectionString"];

            providerPath = provider.Attributes["providerPath"];

            objectQualifier = provider.Attributes["objectQualifier"];
            if (objectQualifier != string.Empty && !objectQualifier.EndsWith("_"))
                objectQualifier += "_";

            databaseOwner = provider.Attributes["databaseOwner"];
            if (databaseOwner != string.Empty && !databaseOwner.EndsWith("."))
                databaseOwner += ".";
        }

        #endregion

        #region properties

        public string ConnectionString
        {
            get { return connectionString; }
        }


        public string ProviderPath
        {
            get { return providerPath; }
        }

        public string ObjectQualifier
        {
            get { return objectQualifier; }
        }


        public string DatabaseOwner
        {
            get { return databaseOwner; }
        }

        #endregion

        #region private methods

        private string GetFullyQualifiedName(string name)
        {
            return DatabaseOwner + ObjectQualifier + moduleQualifier + name;
        }

        private object GetNull(object field)
        {
            return DotNetNuke.Common.Utilities.Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region override methods

        public override IDataReader DonationTrackerGetUserDonations(int moduleId, int donationUserID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerGetUserDonations"), moduleId, donationUserID);
        }

        public override IDataReader DonationTrackerGetDonation(int moduleId, int donationID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerGetDonation"), moduleId, donationID);
        }

        public override void DonationTrackerAddDonation(int moduleId, int donationUserID, int driveID, DateTime donationDate, double donationAmount, string donationType, bool followup, string donationNotes, int userId, int pledgeID, DateTime pledgeDate, string donationFrom, string typeOfDonation, string source)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerAddDonation"), moduleId, donationUserID, driveID, donationDate, donationAmount, donationType, followup, donationNotes, userId, pledgeID, pledgeDate, donationFrom, typeOfDonation, source);
        }

        public override void DonationTrackerUpdateDonation(int moduleId, int donationID, int driveID, DateTime donationDate, double donationAmount, string donationType, bool followup, string donationNotes, int pledgeID, DateTime pledgeDate, string donationFrom, string typeOfDonation, string source)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerUpdateDonation"), moduleId, donationID, driveID, donationDate, donationAmount, donationType, followup, donationNotes, pledgeID, pledgeDate, donationFrom, typeOfDonation, source);
        }

        public override void DonationTrackerDeleteDonation(int moduleId, int DonationID)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerDeleteDonation"), moduleId, DonationID);
        }

        // PLEDGE
        public override void DonationTrackerPledgeAdd(int moduleId, int donationUserID, int driveID, DateTime startDate, DateTime endDate, double pledgeAmount, string frequency, bool followup, string notes, int userId, int numberOfPayments, string donationFrom, string typeOfDonation, string source)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerPledgeAdd"), moduleId, donationUserID, driveID, startDate, endDate, pledgeAmount, frequency, followup, notes, userId, numberOfPayments, donationFrom, typeOfDonation, source);
        }

        public override IDataReader DonationTrackerPledge_GetPledgeScheduleByUserID(int donationUserID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerPledge_GetPledgeScheduleByUserID"), donationUserID);
        }

        public override IDataReader DonationTrackerPledge_GetPledgeScheduleByPledgeID(int pledgeID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerPledge_GetPledgeScheduleByPledgeID"), pledgeID);
        }		

        public override IDataReader DonationTrackerPledgeGetPledgeByID(int pledgeID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerPledgeGetPledgeByID"), pledgeID);
        }

        public override IDataReader DonationTrackerPledgeGetPledgesByUserID(int moduleId, int donationUserID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerPledgeGetPledgesByUserID"), moduleId, donationUserID);
        }

        public override void DonationTrackerPledgeUpdate(int pledgeID, int driveID, DateTime startDate, DateTime endDate, double pledgeAmount, string frequency, bool followup, string notes, int UserId, int numberOfPayments, string donationFrom, string typeOfDonation, string source)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerPledgeUpdate"), pledgeID, driveID, startDate, endDate, pledgeAmount, frequency, followup, notes, UserId, numberOfPayments, donationFrom, typeOfDonation, source);
        }

        public override void DonationTrackerPledgeDelete(int pledgeID)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerPledgeDelete"), pledgeID);
        }	


        //JOE DRIVES
        public override IDataReader DonationTrackerGetDrives(int moduleId, int isActive)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerGetDrives"), moduleId, isActive);
        }

        public override IDataReader DonationTrackerGetDrive(int moduleId, int DriveID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerGetDrive"), moduleId, DriveID);
        }

        public override void DonationTrackerAddDrive(int moduleId, string driveName, DateTime driveDate, string description, bool isActive, int userId, string reminder, string pledgeLetter)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerAddDrive"), moduleId, driveName, driveDate, description, isActive, userId, reminder, pledgeLetter);
        }

        public override void DonationTrackerUpdateDrive(int moduleId, int driveID, string driveName, DateTime driveDate, string description, bool isActive, string reminder, string pledgeLetter)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerUpdateDrive"), moduleId, driveID, driveName, driveDate, description, isActive, reminder, pledgeLetter);
        }


        // LETTER TEMPLATES
        public override IDataReader DonationTrackerLetterTemplate_List(int moduleId, int isActive)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerLetterTemplate_List"), moduleId, isActive);
        }

        public override IDataReader DonationTrackerLetterTemplate_Get(int letterID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerLetterTemplate_Get"), letterID);
        }

        public override void DonationTrackerLetterTemplate_Insert(int moduleId, string letterName, string letter, bool isActive, int userId)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerLetterTemplate_Insert"), moduleId, letterName, letter, isActive, userId);
        }

        public override void DonationTrackerLetterTemplate_Update(int letterID, string letterName, string letter, bool isActive)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerLetterTemplate_Update"), letterID, letterName, letter, isActive);
        }

        public override void DonationTrackerLetterTemplate_Delete(int letterID)
        {
            SqlHelper.ExecuteNonQuery(connectionString, GetFullyQualifiedName("DonationTrackerLetterTemplate_Delete"), letterID);
        }	

        //REPORTS
        public override IDataReader DonationTrackerReportSummary(int moduleId, string startDate, string endDate)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerReportSummary"), moduleId, startDate, endDate);
        }
        public override IDataReader DonationTrackerReportDetail(int moduleId, int driveID, string startDate, string endDate)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerReportDetail"), moduleId, driveID, startDate, endDate);
        }

        public override IDataReader DonationTrackerReportSearch(int moduleId, string startDate, string endDate, double donationAmount)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerReportSearch"), moduleId, startDate, endDate, donationAmount);
        }
        //Joe This
        public override IDataReader DonationTrackerUserFullListSearch(int PortalID, int PageIndex, int PageSize, string searchField, string searchCriteria, string orderByField, string OrderByDirection)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerUserFullListSearch"), PortalID, PageIndex, PageSize, searchField, searchCriteria, orderByField, OrderByDirection);
        }

        public override IDataReader DonationTrackerUserSearchRecordCount(int PortalID, int PageIndex, int PageSize, string searchField, string searchCriteria, string orderByField, string OrderByDirection)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerUserSearchRecordCount"), PortalID, PageIndex, PageSize, searchField, searchCriteria, orderByField, OrderByDirection);
        }

        //public override DataSet DonationTrackerUserSearch(int PortalID, int PageIndex, int PageSize, string searchField, string searchCriteria, string orderByField, string OrderByDirection)
        //{
        //    return (DataSet)SqlHelper.ExecuteDataset(connectionString, GetFullyQualifiedName("DonationTrackerUserFullListSearch"), PortalID, PageIndex, PageSize, searchField, searchCriteria, orderByField, OrderByDirection);
        //}	

        // THANK YOU LETTERS
        public override int DonationTrackerLetterAdd(int donationID, string letter, int createdByUser, int portalID)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, GetFullyQualifiedName("DonationTrackerLetterAdd"), donationID, letter, createdByUser, portalID));
        }

        public override IDataReader DonationTrackerLetterGet(int letterID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerLetterGet"), letterID);
        }

        public override IDataReader DonationTrackerLetterGet_ByDonationID(int donationID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerLetterGet_ByDonationID"), donationID);
        }

        public override IDataReader DonationTrackerLetterGet_ByPledgeID(int pledgeID)
        {
            return (IDataReader)SqlHelper.ExecuteReader(connectionString, GetFullyQualifiedName("DonationTrackerLetterGet_ByPledgeID"), pledgeID);
        }		

        public override int DonationTracker_Merge(int MasterClientID, int ChildClientID)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(connectionString, GetFullyQualifiedName("Donation_Merge"), MasterClientID, ChildClientID));
        }


        #endregion
    }
}
