//----------------------------------------------------------------------------------------------------------------------
// <summary>
//      Data model of notes.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

using Dapper.Contrib.Extensions;


namespace Services.User.Data.Repository.Models.DTO
{
    [Table("Note")]
    public class Note
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Notes { get; set; }

    }

}
