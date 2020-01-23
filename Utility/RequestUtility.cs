using System;
using EFC.Data.Merchant.DTOs.AccountsPayable;

namespace EFC.AgGateway.Integration.Utility
{
    public static class RequestUtility
    {
        public static string GetSeedYear(Vendor vendor)
        {
            var today = DateTime.UtcNow;

            var seedMonth = vendor.SeedYearStartMonth;

            if (today.Month < seedMonth)
            {
                return today.Year.ToString();
            }
            else
            {
                return today.AddYears(1).Year.ToString();
            }
        }
    }
}
