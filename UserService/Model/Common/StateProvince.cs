using System;
using System.Runtime.Serialization;


namespace Services.User.Models.Rest
{
    [Serializable]
    [DataContract]
    public class StateProvince 
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Country { get; set; }
    }
}
