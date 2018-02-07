//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Data model of Contact.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Dapper.Contrib.Extensions;


namespace Services.User.Data.Repository.Models.DTO
{
    [Table("Contact")]
    public class Contact
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        public int ContactTypeId { get; set; }

        public int UserId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(75)]
        public string Value { get; set; }

    }

}
