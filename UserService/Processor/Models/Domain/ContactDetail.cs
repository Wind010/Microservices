//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Domain model of ContactDetail.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

namespace Services.User.Processors.Models.Domain
{
    public class ContactDetail
    {
        public ContactType Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }
    }

}
