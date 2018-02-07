using System;
using System.Runtime.Serialization;

namespace Services.User.Models.Rest
{
    [Serializable]
    [DataContract]
    public class Address
    {
        [DataMember]
        public string AddressLine1 { get; set; }

        [DataMember]
        public string AddressLine2 { get; set; }

        [DataMember]
        public string AddressLine3 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public StateProvince StateProvince { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public DateTime? Created { get; set; }

        [DataMember]
        public DateTime? Modified { get; set; }
    }
}
