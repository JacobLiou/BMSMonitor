using Microsoft.Extensions.DependencyInjection;
using PowerKit.UI.Models;
using Sofar.BMSUI;
using Sofar.HvBMSUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
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
    /// EVBCM_BCU_Control.xaml 的交互逻辑
    /// </summary>
    public partial class EVBCM_BCU_Control : UserControl
    {
        public EVBCM_BCU_Control()
        {
            InitializeComponent();
            DataContext = BMSDef.MainAppServices.GetService<BCU_Control_ViewModel>();
            this.Loaded += Onloaded; // 注册 Unloaded 事件
            this.Unloaded += OnUnloaded; // 注册 Unloaded 事件
        }
     
        private void Onloaded(object sender, RoutedEventArgs e)
        {          
            if (DataContext is BCU_Control_ViewModel viewModel)
            {
                viewModel.cts = new CancellationTokenSource();
                viewModel.StartDataCollection();
                viewModel.UpdateTime();
            }
        }
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is BCU_Control_ViewModel viewModel)
            {
                viewModel.CancelOperation();
                viewModel.StopDataCollection();
            }
        }
    }
}

