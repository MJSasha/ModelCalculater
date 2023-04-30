using UI.Localization;

namespace UI.Utils
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class LocalizationPropsAttribute : Attribute
    {
        public string DisplayName { get; set; }
        public string FolderName { get; set; }

        public LocalizationPropsAttribute(string displayName, string folderName)
        {
            FolderName = folderName;
            DisplayName = displayName;
        }
    }
}
