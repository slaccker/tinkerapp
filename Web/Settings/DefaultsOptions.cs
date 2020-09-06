using Web.Defaults;

namespace Web.Settings
{
    public class DefaultsOptions
    {
        public const string Defaults = "Defaults";

        public int PerPageStories { get; set; } = PageSettingsDefaults.StoriesPerPage;
    }
}