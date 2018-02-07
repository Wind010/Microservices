using System;

namespace Services.User.Processors.Models.Domain
{
    public class Address
    {
        public string AddressLine1  { get; set; }

        public string AddressLine2  { get; set; }

        public string AddressLine3  { get; set; }

        public string City  { get; set; }

        public StateProvince StateProvince { get; set; }

        public string ZipCode  { get; set; }

        public DateTime? Created  { get; set; }

        public DateTime? Modified  { get; set; }
    }
}
