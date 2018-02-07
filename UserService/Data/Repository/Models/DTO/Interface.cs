//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Data model of Interfaces.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Dapper.Contrib.Extensions;

namespace Services.User.Data.Repository.Models.DTO
{
    [Table("Interface")]
    public class Interface
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

    }

}
