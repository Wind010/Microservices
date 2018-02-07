//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Data model of Preference.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Dapper.Contrib.Extensions;

namespace Services.User.Data.Repository.Models.DTO
{
    [Table("Preference")]
    public class Preference
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PreferenceTypeId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Details { get; set; }

        public bool Like { get; set; }

    }

}
