using System;
using System.Runtime.Serialization;

namespace Services.User.Models.Rest
{
    public enum PreferenceCategory
    {
        Food = 1,
        Wine,
        Seating
    }

    [Serializable]
    [DataContract]
    public class Preference
    {
        [DataMember]
        public PreferenceCategory Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Details { get; set; }

        [DataMember]
        public bool Like { get; set; }
    }
}
