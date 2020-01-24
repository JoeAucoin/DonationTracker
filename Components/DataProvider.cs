using System;
using System.Data;
using DotNetNuke;
using DotNetNuke.Framework;

namespace GIBS.DonationTracker.Components
{
    public abstract class DataProvider
    {

        #region common methods

        /// <summary>
        /// var that is returned in the this singleton
        /// pattern
        /// </summary>
        private static DataProvider instance = null;

        /// <summary>
        /// private static cstor that is used to init an
        /// instance of this class as a singleton
        /// </summary>
        static DataProvider()
        {
            instance = (DataProvider)Reflection.CreateObject("data", "GIBS.DonationTracker.Components", "");
        }

        /// <summary>
        /// Exposes the singleton object used to access the database with
        /// the conrete dataprovider
        /// </summary>
        /// <returns></returns>
        public static DataProvider Instance()
        {
            return instance;
        }

        #endregion


        #region Abstract methods

        /* implement the methods that the dataprovider should */

        public abstract IDataReader DonationTrackerGetUserDonations(int moduleId, int donationUserID);
        public abstract IDataReader DonationTrackerGetDonation(int moduleId, int donationID);
        public abstract void DonationTrackerAddDonation(int moduleId, int donationUserID, int driveID, DateTime donationDate, double donationAmount, string donationType, bool followup, string donationNotes, int userId, int pledgeID, DateTime pledgeDate, string donationFrom, string typeOfDonation, string source);
        public abstract void DonationTrackerUpdateDonation(int moduleId, int donationID, int driveID, DateTime donationDate, double donationAmount, string donationType, bool followup, string donationNotes, int pledgeID, DateTime pledgeDate, string donationFrom, string typeOfDonation, string source);
        public abstract void DonationTrackerDeleteDonation(int moduleId, int donationID);
        // DRIVES
        public abstract IDataReader DonationTrackerGetDrives(int moduleId, int isActive);
        public abstract IDataReader DonationTrackerGetDrive(int moduleId, int driveID);
        public abstract void DonationTrackerAddDrive(int moduleId, string driveName, DateTime driveDate, string description, bool isActive, int userId, string reminder, string pledgeLetter);
        public abstract void DonationTrackerUpdateDrive(int moduleId, int driveID, string driveName, DateTime driveDate, string description, bool isActive, string reminder, string pledgeLetter);
        
        public abstract IDataReader DonationTrackerReportSummary(int moduleId, string startDate, string endDate);
        public abstract IDataReader DonationTrackerReportDetail(int moduleId, int driveID, string startDate, string endDate);
        public abstract IDataReader DonationTrackerReportSearch(int moduleId, string startDate, string endDate, double donationAmount);
        
        // PLEDGE
        public abstract void DonationTrackerPledgeAdd(int moduleId, int donationUserID, int driveID, DateTime startDate, DateTime endDate, double pledgeAmount, string frequency, bool followup, string notes, int userId, int numberOfPayments, string donationFrom, string typeOfDonation, string source);
        public abstract IDataReader DonationTrackerPledge_GetPledgeScheduleByUserID(int donationUserID);
        public abstract IDataReader DonationTrackerPledge_GetPledgeScheduleByPledgeID(int pledgeID);
        public abstract IDataReader DonationTrackerPledgeGetPledgeByID(int pledgeID);
        public abstract IDataReader DonationTrackerPledgeGetPledgesByUserID(int moduleId, int donationUserID);
        public abstract void DonationTrackerPledgeUpdate(int pledgeID, int driveID, DateTime startDate, DateTime endDate, double pledgeAmount, string frequency, bool followup, string notes, int UserId, int numberOfPayments, string donationFrom, string typeOfDonation, string source);
        public abstract void DonationTrackerPledgeDelete(int pledgeID);

        // LETTER TEMPLATES
        public abstract IDataReader DonationTrackerLetterTemplate_List(int moduleId, int isActive);
        public abstract void DonationTrackerLetterTemplate_Insert(int moduleId, string letterName, string letter, bool isActive, int userId);
        public abstract void DonationTrackerLetterTemplate_Update(int letterID, string letterName, string letter, bool isActive);
        public abstract IDataReader DonationTrackerLetterTemplate_Get(int letterID);
        public abstract void DonationTrackerLetterTemplate_Delete(int letterID);

        // SEARCH
        public abstract IDataReader DonationTrackerUserFullListSearch(int PortalID, int PageIndex, int PageSize, string searchField, string searchCriteria, string orderByField, string OrderByDirection);

        public abstract IDataReader DonationTrackerUserSearchRecordCount(int PortalID, int PageIndex, int PageSize, string searchField, string searchCriteria, string orderByField, string OrderByDirection);
       
        // THANK YOU LETTERS
        public abstract int DonationTrackerLetterAdd(int donationID, string letter, int createdByUser, int portalID);
        public abstract IDataReader DonationTrackerLetterGet(int letterID);
        public abstract IDataReader DonationTrackerLetterGet_ByDonationID(int donationID);
        public abstract IDataReader DonationTrackerLetterGet_ByPledgeID(int pledgeID);
        //Client Merge
        public abstract int DonationTracker_Merge(int MasterClientID, int ChildClientID);

        #endregion

    }



}
