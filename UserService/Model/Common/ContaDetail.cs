using System;
using System.Runtime.Serialization;


namespace Services.User.Models.Rest
{
    [Serializable]
    [DataContract]
    public class ContactDetail
    {
        [DataMember]
        public ContactType Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
