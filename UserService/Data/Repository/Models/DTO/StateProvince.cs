//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Data model of StateProvince.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Dapper.Contrib.Extensions;

namespace Services.User.Data.Repository.Models.DTO
{
    [Table("StateProvince")]
    public class StateProvince
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(3)]
        public string Code { get; set; }

        [StringLength(60)]
        public string Country { get; set; }
    }

}
