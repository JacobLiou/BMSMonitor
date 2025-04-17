using Microsoft.Extensions.DependencyInjection;
using Sofar.BMSLib;
using Sofar.BMSUI;
using Sofar.BMSUI.Common;
using Sofar.HvBMSUI.Control.Pages;
using Sofar.HvBMSUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Sofar.BMSMonitor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public RelayCommand<string> NavigateCommand { get; }

        public MainViewModel()
        {
            NavigateCommand = new RelayCommand<string>(Navigate);

            // 默认显示首页
            CurrentView = BMSDef.MainAppServices.GetService<EVBCM_BCU_Control>();
        }


        private void Navigate(string destination)
        {
            CurrentView = destination switch
            {
                "EVBCM_BCU_Control" => BMSDef.MainAppServices.GetService<EVBCM_BCU_Control>(),
                "EVBCM_BCU_ParamControl" => BMSDef.MainAppServices.GetService<EVBCM_BCU_ParamControl>(),
                "EVBCM_BMU_Control" => BMSDef.MainAppServices.GetService<EVBCM_BMU_Control>(),
                "EVBCM_BMU_ParamControl" => BMSDef.MainAppServices.GetService<EVBCM_BMU_ParamControl>(),
                "UpgradeControl_BMS" => BMSDef.MainAppServices.GetService<UpgradeControl_BMS>(),
                "FileTransmit_BMS" => BMSDef.MainAppServices.GetService<FileTransmit_BMS>()
            };
        }

       
    }
}
