using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;


public static class DateExtension
{
    public static string ToFrenchMonthName(this int str)
    {
        var result = new DateTime(2017, str, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("FR"));
        return result;
    }
}
