using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


public static class CustomHmtlHelpers
{
    public static MvcHtmlString CountryDropdownList(this HtmlHelper helper, string fieldName = "", int? countryIdSelected = null, string cls = "")
    {
        var utilRepository = new Util_Repository();
        var countries = utilRepository.FindAllCountry();
        var html = "<select name=\"" + fieldName + "\" class=\"" + cls + "\">";

        foreach (var item in countries)
        {
            if (countryIdSelected != null && item.Country_ID == countryIdSelected)
            {
                html += "<option selected =\"selected\" value =\"" + item.Country_ID + "\">" + item.Name + "</option>";
            }
            else
            {
                html += "<option value=\"" + item.Country_ID + "\">" + item.Name + "</option>";
            }
        }
        html += "</select>";

        //var html = "<select>" + countries.Select(item => "<option value=\"" + item.Name + "\">" + item.Name + "</option>") +
        //           "</select>";

        return new MvcHtmlString(html);
    }
}
