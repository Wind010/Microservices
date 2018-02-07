//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Data model of a User that maps to the UserProfile.User table.  Can split this model up for 
//      CQRS (Command Query Responsibility Segregation) when needed, now we stick to CRUD.
// </summary>
//----------------------------------------------------------------------------------------------------------------------


using System;
using System.ComponentModel.DataAnnotations;

using Dapper.Contrib.Extensions;

namespace Services.User.Data.Repository.Models.DTO
{
    [Table("User")]
    public class User
    {
        [Dapper.Contrib.Extensions.Key]
        public int Id { get; set; }

        public Guid? UserId { get; set; }

        public int TenantId { get; set; }

        [StringLength(1000)]
        public string FirstName { get; set; }

        [StringLength(1000)]
        public string MiddleName { get; set; }

        [StringLength(1000)]
        public string LastName { get; set; }

        [StringLength(10)]
        public string Title { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Modified { get; set; }
    }

}
