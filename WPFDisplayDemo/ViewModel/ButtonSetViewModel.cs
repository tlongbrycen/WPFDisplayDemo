using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFDisplayDemo.View;

namespace WPFDisplayDemo.ViewModel
{
    class ButtonSetViewModel
    {
        public MyICommand<CheckBox> ClickCommand { get; set; }

        public ButtonSetViewModel()
        {
            ClickCommand = new MyICommand<CheckBox>(OnClick);
        }

        private void OnClick(CheckBox checkBox)
        {
            if((bool)checkBox.IsChecked)
            {
                Debug.WriteLine("CheckBox is checked");
            }
            else
            {
                Debug.WriteLine("CheckBox is not checked");
            }
            ThermalWindow thermalWindow = new ThermalWindow();
            thermalWindow.Show();
        }
    }
}
