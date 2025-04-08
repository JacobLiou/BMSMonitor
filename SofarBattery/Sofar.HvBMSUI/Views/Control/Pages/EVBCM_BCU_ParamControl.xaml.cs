using Microsoft.Extensions.DependencyInjection;
using Sofar.BMSUI;
using Sofar.HvBMSUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Sofar.HvBMSUI.Control.Pages
{
    /// <summary>
    /// EVBCM_BCU_ParamControl.xaml 的交互逻辑
    /// </summary>
    public partial class EVBCM_BCU_ParamControl : UserControl
    {
        public EVBCM_BCU_ParamControl()
        {
            InitializeComponent();
            DataContext = BMSDef.MainAppServices.GetService<BCU_ParamControl_ViewModel>();
            this.Loaded += Onloaded; // 注册 Unloaded 事件
            this.Unloaded += OnUnloaded; // 注册 Unloaded 事件
        }

        private void Onloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is BCU_ParamControl_ViewModel viewModel)
            {
                viewModel.cts = new CancellationTokenSource();
                viewModel.Load();
            }
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is BCU_ParamControl_ViewModel viewModel)
            {
                viewModel.CancelOperation();
            }
        }
    }
}

