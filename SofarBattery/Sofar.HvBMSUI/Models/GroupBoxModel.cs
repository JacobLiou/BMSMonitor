using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sofar.HvBMSUI.Models
{
    public class ActiveEquilibriumCheckBoxGroup
    {
        public string Label { get; set; }
        public CheckBoxItem ALL { get; set; }
        public ObservableCollection<BoxGruop> Items { get; set; }
    }

    public class PassiveEquilibriumCheckBoxGroup
    {
        public string Label { get; set; }
        public CheckBoxItem ALL { get; set; }
        public ObservableCollection<CheckBoxItem> Items { get; set; }

        public ICommand SelectCommand { get; set; }
    }

    public class BoxGruop
    {
        public CheckBoxItem A { get; set; }
        public CheckBoxItem B { get; set; }
    }

    public class CheckBoxItem : ObservableObject
    {
        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public string Label { get; set; } // 用于显示 CheckBox 的标签


    }
}
