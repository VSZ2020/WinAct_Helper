using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WinAct_Helper.Model;
using WinAct_Helper.Controller;

namespace WinAct_Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IUserPreferences preferences;
        public Radionuclides RadionuclidesProperty { get; set; }
        public InputFile InputFileProperty { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            InitializeProgram();
        }

        private void InitializeProgram()
        {
            preferences = new UserPreferencesController();
            preferences.LoadPreferences(preferences.PreferencesPath);
            RadionuclidesProperty = new Radionuclides();
            RadionuclidesProperty.ReadFromFile(new RadionuclideTextReader(), Radionuclides.RadionuclidesFilePath);
            LoadDefaultInput();
            SetMainWindowBindings();
            SetButtonEventHandlers();
        }

        private void LoadDefaultInput()
        {
            this.DataContext = null;
            InputFileProperty = (new DefaultDataController(RadionuclidesProperty)).CreateDefaultInput();
            this.DataContext = this;
            UpdateModifiedStatus(true);
        }
    }
}
