using System.Collections.Generic;

using WinAct_Helper.Model;

namespace WinAct_Helper.Controller
{
	internal class UserPreferencesController : IUserPreferences
    {
        public const string PreferencesFileName = "";
        public string PreferencesPath { get; set; }
        public List<string> RecentFiles { get; set; }

        public UserPreferencesController()
        {
            RecentFiles = new List<string>();
            PreferencesPath = "prefs\\userprefs.xml";
        }

        public void LoadPreferences(string path)
        {
            //throw new NotImplementedException();
        }

        public void SavePreferences(string path)
        {
            //throw new NotImplementedException();
        }
    }
}
