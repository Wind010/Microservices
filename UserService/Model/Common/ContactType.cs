using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Services.User.Models.Rest
{
    public enum ContactCategory
    {
        Cellphone = 1,
        HomePhone,
        WorkPhone,
        Fax,
        Email,
        Twitter,
        Facebook,
        LinkedIn
    }

    [Serializable]
    [DataContract]
    public class ContactType
    {
        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public ContactCategory Contact { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
