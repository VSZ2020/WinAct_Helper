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
using WinAct_Helper.Forms;
using WinAct_Helper.ViewModels;
using WinAct_Helper.Services;
using System.Diagnostics;

namespace WinAct_Helper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            vm = new MainWindowVM();
            DataContext = vm;

            InitializeComponent();
        }

        private MainWindowVM vm;

        private void OnBtnAboutClick(object sender, RoutedEventArgs e)
        {
            About formAbout = new About();
            formAbout.ShowDialog();            
        }

        private void MenuHelp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = "https://github.com/VSZ2020/WinAct_Helper/wiki" });
        }
    }
}
