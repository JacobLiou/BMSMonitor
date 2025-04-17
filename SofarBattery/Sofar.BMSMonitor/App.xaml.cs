using Microsoft.Extensions.DependencyInjection;
using Sofar.BMSUI;
using Sofar.HvBMSUI.Control.Pages;
using Sofar.HvBMSUI.ViewModels;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Sofar.BMSMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            //services.AddTransient<IStrategyService, SafetyIECService>();

            services.AddSingleton<MainWindow>();

            services.AddSingleton<BCU_ParamControl_ViewModel>();
            services.AddSingleton<BMU_ParamControl_ViewModel>();
            services.AddSingleton<BCU_Control_ViewModel>();
            services.AddSingleton<BMU_Control_ViewModel>();
            services.AddSingleton<UpgradeControl_BMS_ViewModel>();
            services.AddSingleton<FileTransmit_BMS_ViewModel>();

            services.AddSingleton<EVBCM_BCU_Control>();
            services.AddSingleton<EVBCM_BCU_ParamControl>();
            services.AddSingleton<EVBCM_BMU_Control>();
            services.AddSingleton<EVBCM_BMU_ParamControl>();
            services.AddSingleton<FileTransmit_BMS>();
            services.AddSingleton<UpgradeControl_BMS>();





            return services.BuildServiceProvider();
        }

        public App()
        {
            InitializeComponent();


            BMSDef.SetBMSConfig();

            Services = ConfigureServices();
            BMSDef.MainAppServices = Services;



            DispatcherUnhandledException += App_DispatcherUnhandledException;//UI线程异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;//非UI线程异常
        }
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }
        protected override void OnStartup(StartupEventArgs e)
        {
            // 从配置中加载保存的语言


            base.OnStartup(e);

            BMSDef.Connect();
        }


        

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //可以记录日志并转向错误bug窗口友好提示用户
            e.Handled = true;
            MessageBoxHelper.ShowMessageBox(MessageBoxType.Error, $"内部异常，请联系管理员！\r{e.Exception.Message.ToString()}", $"系统错误");
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //可以记录日志并转向错误bug窗口友好提示用户
            if (e.ExceptionObject is Exception ex)
            {
                MessageBoxHelper.ShowMessageBox(MessageBoxType.Error, $"内部异常，CurrentDomain_UnhandledException捕获异常！\r{ex.Message.ToString()}", $"系统错误");

            }
        }
    }

}
