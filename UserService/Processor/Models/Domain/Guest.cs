//----------------------------------------------------------------------------------------------------------------------
// <summary>
//     The domain model of a User.  Aggregates other models.  Make setters private when needed.
//     Prefer to have the UserProcessor do the work, but would this be an anemic domain model anti-pattern that
//     some consider a pattern now?
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Services.User.Processors.Models.Domain
{

    public class User 
    {
        public int Id  { get; set; }

        public Guid? UserId  { get; set; }

        public int TenantId  { get; set; }

        public string FirstName  { get; set; }

        public string MiddleName  { get; set; }

        public string LastName  { get; set; }

        public string Title  { get; set; }

        public DateTime? Created  { get; set; }

        public DateTime? Modified  { get; set; }


        public List<Address> Addresses { get; set; }

        public List<ContactDetail> ContactDetails { get; set; }

        public List<string> Notes { get; set; }

        public List<Preference> Preferences { get; set; }


    }
}
