// <summary>
//      Domain model of Preference.
// </summary>
//----------------------------------------------------------------------------------------------------------------------

namespace Services.User.Processors.Models.Domain
{
    public class Preference
    {
        public PreferenceType Type { get; set; }
        
        public string Name { get; set; }

        public string Details { get; set; }

        public bool Like { get; set; }
    }

}
