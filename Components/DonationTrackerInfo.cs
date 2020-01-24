using System;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace GIBS.DonationTracker.Components
{
    public class DonationTrackerInfo
    {
        private int itemID;
        private int         moduleId;
        private int         donationID;
        private string      notes;
        
        private int         createdByUserID;
        private DateTime    createdDate;
        private string      createdByUserName = null;
        private string updatedByUserName;

        private int         driveID;
        private string      driveName;
        private DateTime    driveDate;
        
        private int         donationUserID;
        private DateTime    donationDate;
        private double      donationAmount = 0;
        private string      donationNotes;
        private string      donationType;
        private bool        followup;
        private string      description;
        private bool        isActive;
        private string donationFrom = "";
        private string typeOfDonation = "";
        private string source;

        private bool doNotMail;

        private string      prefix;
        private string      suffix;
        private string      firstName;
        private string      lastName;
        private string      middleName;
        private string      donationUserName;
        private string      street;
        private string      city;
        private string      state;
        private string      postalCode;
        private string      company;
        private int         userID;
        // LETTER
        private int         letterID;
        private string      letter;
        private string pledgeLetter;
        private string      pDFFile;
        private string letterName;
        private bool letterGenerated;


        private string      userName;
        private string      displayName;
        private string      email; 
        private string      country;
        private string      cell;
        private string      telephone;  
        private string      additionalFirstName = "";
        private string      additionalName;
        private string      capacityRating;
        private string      inclinationRating;
        private string      stage;
        private string      prospectResearch; 
        private string      prospectManager;
        
        // PLEDGE
        private int pledgeID = 0;
        private DateTime startDate;
        private DateTime endDate;
        private double pledgeAmount;
        private double totalAmountPledged;
        private string frequency;
        private int updatedByUserID;
        private DateTime pledgeDate;
        private int numberOfPayments;
        private string reminder;


        // PAGING
        private int totalRecords;
        private int recordsperPage;
        private int currentPage;
        private int pageSize;
		
        #region properties
        
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
             
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
                
        public string Cell
        {
            get { return cell; }
            set { cell = value; }
        }
                      
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }
                        
        public string AdditionalFirstName
        {
            get { return additionalFirstName; }
            set { additionalFirstName = value; }
        }
                            
        public string AdditionalName
        {
            get { return additionalName; }
            set { additionalName = value; }
        }
                                
        public string CapacityRating
        {
            get { return capacityRating; }
            set { capacityRating = value; }
        }
                                    
        public string InclinationRating
        {
            get { return inclinationRating; }
            set { inclinationRating = value; }
        }
                                        
        public string Stage
        {
            get { return stage; }
            set { stage = value; }
        }
                                             
        public string ProspectResearch
        {
            get { return prospectResearch; }
            set { prospectResearch = value; }
        }

        public string ProspectManager
        {
            get { return prospectManager; }
            set { prospectManager = value; }
        }

        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public int DonationID
        {
            get { return donationID; }
            set { donationID = value; }
        }


        public int DriveID
        {
            get { return driveID; }
            set { driveID = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }

        }

        public int DonationUserID
        {
            get { return donationUserID; }
            set { donationUserID = value; }
        }

        public string DriveName
        {
            get { return driveName; }
            set { driveName = value; }
        }


        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }


        public DateTime DriveDate
        {
            get { return driveDate; }
            set { driveDate = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public DateTime DonationDate
        {
            get { return donationDate; }
            set { donationDate = value; }
        }

        public double DonationAmount
        {
            get { return donationAmount; }
            set { donationAmount = value; }
        }
        //
        public string DonationFrom
        {
            get { return donationFrom; }
            set { donationFrom = value; }
        }

        //typeOfDonation
        public string TypeOfDonation
        {
            get { return typeOfDonation; }
            set { typeOfDonation = value; }
        }

        public string DonationType
        {
            get { return donationType; }
            set { donationType = value; }
        }

        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        //doNotMail
        public bool DoNotMail
        {
            get { return doNotMail; }
            set { doNotMail = value; }
        }

        public bool Followup
        {
            get { return followup; }
            set { followup = value; }
        }	

        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public string DonationNotes
        {
            get { return donationNotes; }
            set { donationNotes = value; }
        }

        public string Prefix
        {
            get { return prefix; }
            set { prefix = value; }
        }

        public string Suffix
        {
            get { return suffix; }
            set { suffix = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string MiddleName
        {
            get { return middleName; }
            set { middleName = value; }
        }

        public string DonationUserName
        {
            get { return donationUserName; }
            set { donationUserName = value; }
        }

        public string Street
        {
            get { return street; }
            set { street = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }


        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        public string Company
        {
            get { return company; }
            set { company = value; }
        }


        public int CreatedByUserID
        {
            get { return createdByUserID; }
            set { createdByUserID = value; }
        }
        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        // PLEDGE
        public int PledgeID
        {
            get { return pledgeID; }
            set { pledgeID = value; }
        }	

        public DateTime PledgeDate
        {
            get { return pledgeDate; }
            set { pledgeDate = value; }
        }	

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public double PledgeAmount
        {
            get { return pledgeAmount; }
            set { pledgeAmount = value; }
        }

        //totalAmountPledged

        public double TotalAmountPledged
        {
            get { return totalAmountPledged; }
            set { totalAmountPledged = value; }
        }


        public string Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        public string Reminder
        {
            get { return reminder; }
            set { reminder = value; }
        }

        public int UpdatedByUserID
        {
            get { return updatedByUserID; }
            set { updatedByUserID = value; }
        }

        //NumberOfPayments
        public int NumberOfPayments
        {
            get { return numberOfPayments; }
            set { numberOfPayments = value; }
        }


        // PAGING

        public int TotalRecords
        {
            get { return totalRecords; }
            set { totalRecords = value; }
        }

        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; }
        }

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        
        public int RecordsperPage
        {
            get { return recordsperPage; }
            set { recordsperPage = value; }
        }

        public string CreatedByUserName
        {

            get { return createdByUserName; }
            set { createdByUserName = value; }

        }

        public string UpdatedByUserName
        {
            get { return updatedByUserName; }
            set { updatedByUserName = value; }
        }		


        public int LetterID
        {
            get { return letterID; }
            set { letterID = value; }
        }


        public string Letter
        {
            get { return letter; }
            set { letter = value; }
        }
        //pledgeLetter
        public string PledgeLetter
        {
            get { return pledgeLetter; }
            set { pledgeLetter = value; }
        }


        //letterName
        public string LetterName
        {
            get { return letterName; }
            set { letterName = value; }
        }

        public bool LetterGenerated
        {
            get { return letterGenerated; }
            set { letterGenerated = value; }

        }

        public string PDFFile
        {
            get { return pDFFile; }
            set { pDFFile = value; }
        }	


        #endregion
    }
}
