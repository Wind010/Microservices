//----------------------------------------------------------------------------------------------------------------------
// <summary>
//     The REST model of a User.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Services.User.Models.Rest.Request
{ 
    // Use these attributes for this model with use with RawRabbit.
    // Determine if we need a RabbitMq model also.

    [Serializable]
    [DataContract]
    public class UserRequest : BaseRequest
    {
        // Prefer to have the separation of the REST model from the domain model.  
        // The REST model could contain the same data, but be structured differently.
        // The domain model be used for business logic which may contain sensitive data we don't want exposed inherently.

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "userId")]
        public Guid? UserId { get; set; }

        [DataMember]
        public int TenantId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public DateTime? Created { get; set; }

        [DataMember]
        public DateTime? Modified { get; set; }

        [DataMember]
        public List<Address> Addresses { get; set; }

        [DataMember]
        public List<ContactDetail> ContactDetails { get; set; }

        [DataMember]
        public List<string> Notes { get; set; }

        [DataMember]
        public List<Preference> Preferences { get; set; }

    }
}
