//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Data model of addresses.
// </summary>
//----------------------------------------------------------------------------------------------------------------------


using System;
using System.ComponentModel.DataAnnotations;

using Dapper.Contrib.Extensions;


namespace Services.User.Data.Repository.Models.DTO
{
    [Table("Address")]
    public class Address
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [StringLength(100)]
        public string AddressLine3 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public int? StateProvinceId { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }
    }

}
