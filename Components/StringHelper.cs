using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GIBS.DonationTracker.Components
{
    public static class StringHelper
    {

        public static string Shorten(this string name, int chars)
        {
            if (name.ToCharArray().Count() > chars)
            {
                return name.Substring(0, chars) + " . . .";
            }
            else return name;
        }

    }
}