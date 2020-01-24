using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace GIBS.DonationTracker.Components
{
    public class DonationTrackerController : IPortable
    {

        #region public method


        // PLEDGE
        public void DonationTrackerPledgeAdd(DonationTrackerInfo info)
        {
            //check we have some content to store
            if (info.PledgeAmount.ToString() != string.Empty)
            {
                DataProvider.Instance().DonationTrackerPledgeAdd(info.ModuleId, info.DonationUserID, info.DriveID, info.StartDate, info.EndDate,  info.PledgeAmount, info.Frequency, info.Followup, info.Notes, info.CreatedByUserID, info.NumberOfPayments, info.DonationFrom, info.TypeOfDonation, info.Source);
            }
        }

        public List<DonationTrackerInfo> DonationTrackerPledge_GetPledgeScheduleByUserID(int donationUserID)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerPledge_GetPledgeScheduleByUserID(donationUserID));
        }

        public List<DonationTrackerInfo> DonationTrackerPledge_GetPledgeScheduleByPledgeID(int pledgeID)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerPledge_GetPledgeScheduleByPledgeID(pledgeID));
        }

        //public DonationTrackerInfo DonationTrackerPledgeGetPledgeByID(int pledgeID)
        //{
        //    return (DonationTrackerInfo)CBO.FillObject(DataProvider.Instance().DonationTrackerPledgeGetPledgeByID(pledgeID), typeof(DonationTrackerInfo));
        //}
        public DonationTrackerInfo DonationTrackerPledgeGetPledgeByID(int pledgeID)
        {
            return CBO.FillObject<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerPledgeGetPledgeByID(pledgeID));
        }

        public List<DonationTrackerInfo> DonationTrackerPledgeGetPledgesByUserID(int moduleId, int donationUserID)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerPledgeGetPledgesByUserID(moduleId, donationUserID));
        }

        public void DonationTrackerPledgeUpdate(DonationTrackerInfo info)
        {
            //check we have some content to update
            if (info.PledgeID.ToString() != string.Empty)
            {
                DataProvider.Instance().DonationTrackerPledgeUpdate(info.PledgeID, info.DriveID, info.StartDate, info.EndDate, info.PledgeAmount, info.Frequency, info.Followup, info.Notes, info.UserID, info.NumberOfPayments, info.DonationFrom, info.TypeOfDonation, info.Source);
            }
        }

        public void DonationTrackerPledgeDelete(int pledgeID)
        {
            DataProvider.Instance().DonationTrackerPledgeDelete(pledgeID);
        }


        /// <summary>
        /// Gets all the DonationTrackerInfo objects for items matching the this moduleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<DonationTrackerInfo> DonationTrackerGetUserDonations(int moduleId, int donationUserID)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerGetUserDonations(moduleId, donationUserID));
        }

        /// <summary>
        /// Get an info object from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="DonationID"></param>
        /// <returns></returns>
        public DonationTrackerInfo DonationTrackerGetDonation(int moduleId, int DonationID)
        {
            return CBO.FillObject<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerGetDonation(moduleId, DonationID));
           
        }


        /// <summary>
        /// Adds a new DonationTrackerInfo object into the database
        /// </summary>
        /// <param name="info"></param>
        public void DonationTrackerAddDonation(DonationTrackerInfo info)
        {
            //check we have some content to store
            if (info.DonationAmount.ToString() != string.Empty)
            {
                DataProvider.Instance().DonationTrackerAddDonation(info.ModuleId, info.DonationUserID, info.DriveID, info.DonationDate, info.DonationAmount, info.DonationType, info.Followup, info.DonationNotes, info.CreatedByUserID, info.PledgeID, info.PledgeDate, info.DonationFrom, info.TypeOfDonation, info.Source);
            }
        }

        /// <summary>
        /// update a info object already stored in the database
        /// </summary>
        /// <param name="info"></param>
        public void DonationTrackerUpdateDonation(DonationTrackerInfo info)
        {
            //check we have some content to update
            if (info.DonationAmount.ToString() != string.Empty)
            {
                DataProvider.Instance().DonationTrackerUpdateDonation(info.ModuleId, info.DonationID, info.DriveID, info.DonationDate, info.DonationAmount, info.DonationType, info.Followup, info.DonationNotes, info.PledgeID, info.PledgeDate, info.DonationFrom, info.TypeOfDonation, info.Source);
            }
        }


        /// <summary>
        /// Delete a given item from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="DonationID"></param>
        public void DonationTrackerDeleteDonation(int moduleId, int DonationID)
        {
            DataProvider.Instance().DonationTrackerDeleteDonation(moduleId, DonationID);
        }



        public List<DonationTrackerInfo> DonationTrackerGetDrives(int moduleId, int isActive)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerGetDrives(moduleId, isActive));
        }

        public DonationTrackerInfo DonationTrackerGetDrive(int moduleId, int DriveID)
        {
            return CBO.FillObject<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerGetDrive(moduleId, DriveID));

        }

        public void DonationTrackerAddDrive(DonationTrackerInfo info)
        {
            //check we have some content to store
            if (info.DriveName != string.Empty)
            {
                DataProvider.Instance().DonationTrackerAddDrive(info.ModuleId, info.DriveName, info.DriveDate, info.Description, info.IsActive, info.CreatedByUserID, info.Reminder, info.PledgeLetter);
            }
        }

        public void DonationTrackerUpdateDrive(DonationTrackerInfo info)
        {
            //check we have some content to update
            if (info.DriveName != string.Empty)
            {
                DataProvider.Instance().DonationTrackerUpdateDrive(info.ModuleId, info.DriveID, info.DriveName, info.DriveDate, info.Description, info.IsActive, info.Reminder, info.PledgeLetter);
            }
        }	


        // LETTER TEMPLATES
        public void DonationTrackerLetterTemplate_Delete(int letterID)
        {
            DataProvider.Instance().DonationTrackerLetterTemplate_Delete(letterID);
        }

        public List<DonationTrackerInfo> DonationTrackerLetterTemplate_List(int moduleId, int isActive)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerLetterTemplate_List(moduleId, isActive));
        }

        public DonationTrackerInfo DonationTrackerLetterTemplate_Get(int letterID)
        {
            return CBO.FillObject<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerLetterTemplate_Get(letterID));
            //return (DonationTrackerInfo)CBO.FillObject(DataProvider.Instance().DonationTrackerLetterTemplate_Get(letterID), typeof(DonationTrackerInfo));
        }

        public void DonationTrackerLetterTemplate_Insert(DonationTrackerInfo info)
        {
            //check we have some content to store
            if (info.LetterName != string.Empty)
            {
                DataProvider.Instance().DonationTrackerLetterTemplate_Insert(info.ModuleId, info.LetterName, info.Letter, info.IsActive, info.CreatedByUserID);
            }
        }

        public void DonationTrackerLetterTemplate_Update(DonationTrackerInfo info)
        {
            //check we have some content to update
            if (info.LetterName != string.Empty)
            {
                DataProvider.Instance().DonationTrackerLetterTemplate_Update(info.LetterID, info.LetterName, info.Letter, info.IsActive);
            }
        }



        // REPORTS
        public List<DonationTrackerInfo> DonationTrackerReportSummary(int moduleId, string startDate, string endDate)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerReportSummary(moduleId, startDate, endDate));
        }


        public List<DonationTrackerInfo> DonationTrackerReportDetail(int moduleId, int driveID, string startDate, string endDate)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerReportDetail(moduleId, driveID, startDate, endDate));
        }

        public List<DonationTrackerInfo> DonationTrackerReportSearch(int moduleId, string startDate, string endDate, double donationAmount)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerReportSearch(moduleId, startDate, endDate, donationAmount));
        }


        //Joe This
        public List<DonationTrackerInfo> DonationTrackerUserFullListSearch(int PortalID, int PageIndex, int PageSize, string searchField, string searchCriteria, string orderByField, string OrderByDirection)
        {
            return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerUserFullListSearch( PortalID,  PageIndex,  PageSize,  searchField,  searchCriteria,  orderByField,  OrderByDirection));
        }

        public DonationTrackerInfo DonationTrackerUserSearchRecordCount(int PortalID, int PageIndex, int PageSize, string searchField, string searchCriteria, string orderByField, string OrderByDirection)
        {
            return CBO.FillObject<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerUserSearchRecordCount(PortalID, PageIndex, PageSize, searchField, searchCriteria, orderByField, OrderByDirection));
            //return (DonationTrackerInfo)CBO.FillObject(DataProvider.Instance().DonationTrackerUserSearchRecordCount(PortalID, PageIndex, PageSize, searchField, searchCriteria, orderByField, OrderByDirection), typeof(DonationTrackerInfo));
        }

        //public List<DonationTrackerInfo> DonationTrackerUserSearch(int PortalID, int PageIndex, int PageSize, string searchField, string searchCriteria, string orderByField, string OrderByDirection)
        //{
        //    return CBO.FillCollection<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerUserSearch(PortalID, PageIndex, PageSize, searchField, searchCriteria, orderByField, OrderByDirection));
        //}

        // THANK YOU LETTERS
        public int DonationTrackerLetterAdd(DonationTrackerInfo info)
        {
            if (info.Letter.ToString() != string.Empty)
            {
                return Convert.ToInt32(DataProvider.Instance().DonationTrackerLetterAdd(info.DonationID, info.Letter, info.CreatedByUserID, info.PledgeID));
            }
            else
            {
                return 0;
            }
        }

        public DonationTrackerInfo DonationTrackerLetterGet(int letterID)
        {
            //return (DonationTrackerInfo)CBO.FillObject(DataProvider.Instance().DonationTrackerLetterGet(letterID), typeof(DonationTrackerInfo));
            return CBO.FillObject<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerLetterGet(letterID));
        }

        public DonationTrackerInfo DonationTrackerLetterGet_ByDonationID(int donationID)
        {
            //return (DonationTrackerInfo)CBO.FillObject(DataProvider.Instance().DonationTrackerLetterGet_ByDonationID(donationID), typeof(DonationTrackerInfo));
            return CBO.FillObject<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerLetterGet_ByDonationID(donationID));
        }

        public DonationTrackerInfo DonationTrackerLetterGet_ByPledgeID(int pledgeID)
        {
            //return (DonationTrackerInfo)CBO.FillObject(DataProvider.Instance().DonationTrackerLetterGet_ByPledgeID(pledgeID), typeof(DonationTrackerInfo));
            return CBO.FillObject<DonationTrackerInfo>(DataProvider.Instance().DonationTrackerLetterGet_ByPledgeID(pledgeID));
        }

        public int DonationTracker_Merge(int MasterClientID, int ChildClientID)
        {

            return Convert.ToInt32(DataProvider.Instance().DonationTracker_Merge(MasterClientID, ChildClientID));

        }

        #endregion

        #region ISearchable Members

        /// <summary>
        /// Implements the search interface required to allow DNN to index/search the content of your
        /// module
        /// </summary>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        //public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        //{
        //    SearchItemInfoCollection searchItems = new SearchItemInfoCollection();

        //    List<DonationTrackerInfo> infos = GetDonationTrackers(modInfo.ModuleID, modInfo.don);

        //    foreach (DonationTrackerInfo info in infos)
        //    {
        //        SearchItemInfo searchInfo = new SearchItemInfo(modInfo.ModuleTitle, info.Notes, info.CreatedByUser, info.CreatedDate,
        //                                            modInfo.ModuleID, info.DonationID.ToString(), info.Notes, "Item=" + info.DonationID.ToString());
        //        searchItems.Add(searchInfo);
        //    }

        //    return searchItems;
        //}

        #endregion

        #region IPortable Members

        /// <summary>
        /// Exports a module to xml
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public string ExportModule(int moduleID)
        {
            StringBuilder sb = new StringBuilder();

            //List<DonationTrackerInfo> infos = GetDonationTrackers(moduleID, donationUserID);

            //if (infos.Count > 0)
            //{
            //    sb.Append("<DonationTrackers>");
            //    foreach (DonationTrackerInfo info in infos)
            //    {
            //        sb.Append("<DonationTracker>");
            //        sb.Append("<content>");
            //        sb.Append(XmlUtils.XMLEncode(info.Notes));
            //        sb.Append("</content>");
            //        sb.Append("</DonationTracker>");
            //    }
            //    sb.Append("</DonationTrackers>");
            //}

            return sb.ToString();
        }

        /// <summary>
        /// imports a module from an xml file
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="Content"></param>
        /// <param name="Version"></param>
        /// <param name="UserID"></param>
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            XmlNode infos = DotNetNuke.Common.Globals.GetContent(Content, "DonationTrackers");

            foreach (XmlNode info in infos.SelectNodes("DonationTracker"))
            {
                DonationTrackerInfo DonationTrackerInfo = new DonationTrackerInfo();
                DonationTrackerInfo.ModuleId = ModuleID;
                DonationTrackerInfo.Notes = info.SelectSingleNode("Notes").InnerText;
                DonationTrackerInfo.CreatedByUserID = UserID;

                DonationTrackerAddDonation(DonationTrackerInfo);
            }
        }

        #endregion
    }
}
