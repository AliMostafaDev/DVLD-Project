using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Business;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsCountry
    {
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode = enMode.AddNew;

        public int CountryID { get; set; }
        public string CountryName {  get; set; }

        public clsCountry()
        {
            Mode = enMode.AddNew;
            CountryID = -1;
            CountryName = string.Empty;
        }
        private clsCountry(int CountryID, string CountryName)
        {
            Mode = enMode.Update;
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }

        private bool _AddNewCountry()
        {
            CountryID = clsCountryData.AddNewCountry(CountryName);
            return CountryID != -1;
        }
        private bool _UpdateCountry()
        {
            return clsCountryData.UpdateCountry(CountryID, CountryName);
        }

        public static clsCountry Find(int CountryID)
        {
            string CountryName = string.Empty;

            bool isFound = clsCountryData.GetCountryInfoByID(CountryID, ref CountryName);

            if (isFound)
            {
                return new clsCountry(CountryID, CountryName);
            }

            return null;
        }
        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;

            bool isFound = clsCountryData.GetCountryInfoByName(CountryName, ref CountryID);

            if (isFound)
            {
                return new clsCountry(CountryID, CountryName);
            }

            return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCountry())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateCountry();
            }
            return false;
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }
        public static bool DeleteCountry(int CountryID)
        {
            return clsCountryData.DeleteCountry(CountryID);
        }

        public static bool IsCountryExist(int CountryID)
        {
            return clsPersonData.IsPersonExist(CountryID);
        }
        public static bool IsPersonExist(string CountryName)
        {
            return clsCountryData.IsCountryExist(CountryName);
        }

    }
}
