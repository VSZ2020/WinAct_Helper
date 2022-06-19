using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAct_Helper.Model
{
    internal interface IUserPreferences
    {
        const int MAX_RECENT_FILES_COUNT = 10;
        string PreferencesPath { get; set; }
        List<string> RecentFiles { get; set; }

        //TODO: There can be any program paramenters

        void SavePreferences(string path);
        void LoadPreferences(string path);
    }
}
