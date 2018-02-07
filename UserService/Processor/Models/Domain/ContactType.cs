


namespace Services.User.Processors.Models.Domain
{
    // TODO:  Create enumeration from database values. 
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

    public class ContactType
    {
        public ContactCategory Contact { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
