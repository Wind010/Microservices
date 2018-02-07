//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Data model of contact types.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Dapper.Contrib.Extensions;

namespace Services.User.Data.Repository.Models.DTO
{
    [Table("ContactType")]
    public class ContactType
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }

}
