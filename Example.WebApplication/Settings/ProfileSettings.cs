namespace Example.WebApplication.Settings
{
    using System.Diagnostics.CodeAnalysis;

    public class ProfileSettings
    {
        [AllowNull]
        public string[] Genders { get; set; }
    }
}
