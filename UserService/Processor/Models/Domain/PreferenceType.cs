


namespace Services.User.Processors.Models.Domain
{
    public enum PreferenceCategory
    {
        Food = 1,
        Wine,
        Seating
    }

    public class PreferenceType
    {
        public PreferenceCategory Category { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
