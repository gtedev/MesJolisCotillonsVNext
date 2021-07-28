using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.IO;

namespace MesJolisCotillons.Models
{
    public class Util_Repository : Repository
    {
        public IQueryable<Country> FindAllCountry()
        {
            return db.Countries;
        }
        public byte[] GetImageFromPath(string path)
        {
            byte[] imageBytes = File.ReadAllBytes(path);
            return imageBytes;
        }

        public Country FindCountry(int Country_ID)
        {
            return db.Countries.Where(item => item.Country_ID == Country_ID).FirstOrDefault();
        }

        public Country FindCountryByName(string countryName)
        {
            return db.Countries
                .Where(item => item.Name == countryName)
                .FirstOrDefault();
        }

        public IQueryable<Address> FindAllAddress()
        {
            return db.Addresses;
        }

        public Blob GetBlob(int blob_ID)
        {
            var blob = db.Blobs.Where(item => item.Blob_ID == blob_ID).FirstOrDefault();
            return blob;
        }
    }
}