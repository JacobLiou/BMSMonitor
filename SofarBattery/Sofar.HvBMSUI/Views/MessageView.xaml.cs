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
using System.Windows.Shapes;

namespace Sofar.BMSUI.Views
{
    /// <summary>
    /// MessageView.xaml 的交互逻辑
    /// </summary>
    public partial class MessageView : Window
    {
        private ButtonType currentButtonStyle;

        private Action<MessageBoxResult> resultAction;
        
        public MessageView()
        {
            InitializeComponent();

            foreach (Window item in Application.Current.Windows)
            {
                if (item.IsActive)
                {
                    Owner = item;
                    break;
                }
            }
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseEvent));
        }

        private void CloseEvent(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
            resultAction?.Invoke(MessageBoxResult.None);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (currentButtonStyle == ButtonType.OKCancel || currentButtonStyle == ButtonType.Custom)
            {
                resultAction?.Invoke(MessageBoxResult.OK);
            }
            else if (currentButtonStyle == ButtonType.YesNoCancel || currentButtonStyle == ButtonType.YesNo)
            {
                resultAction?.Invoke(MessageBoxResult.Yes);
            };
        }

        private void Cannel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (currentButtonStyle == ButtonType.OKCancel || currentButtonStyle == ButtonType.Custom)
            {
                resultAction?.Invoke(MessageBoxResult.Cancel);
            }
            else if (currentButtonStyle == ButtonType.YesNoCancel || currentButtonStyle == ButtonType.YesNo)
            {
                resultAction?.Invoke(MessageBoxResult.No);
            };
        }

        public void Show(string messageBoxText, string caption, MessageBoxType type = MessageBoxType.Info, ButtonType buttonType = ButtonType.OKCancel, Action<MessageBoxResult> callback = null, string confirmName = "", string cannelName = "")
        {
            this.Content.Text = messageBoxText;
            this.Title = caption;
            this.currentButtonStyle = buttonType;
            switch (buttonType)
            {
                case ButtonType.OK:
                    Cannel.Visibility = Visibility.Collapsed;
                    break;
                case ButtonType.OKCancel:
                    Cannel.Visibility = Visibility.Visible;
                    Confirm.Visibility = Visibility.Visible;
                    Confirm.Content = "确定(Enter)";
                    Cannel.Content = "取消(ESC)";
                    break;
                case ButtonType.YesNoCancel:
                case ButtonType.YesNo:
                    Cannel.Visibility = Visibility.Visible;
                    Confirm.Visibility = Visibility.Visible;
                    Confirm.Content = "是(Yes)";
                    Cannel.Content = "否(No)";
                    break;
                case ButtonType.None:
                    Cannel.Visibility = Visibility.Collapsed;
                    Confirm.Visibility = Visibility.Collapsed;
                    break;
                case ButtonType.Custom:
                    Cannel.Visibility = Visibility.Visible;
                    Confirm.Visibility = Visibility.Visible;
                    Cannel.Content = cannelName;
                    Confirm.Content = confirmName;
                    break;
                default:
                    break;
            }
            SetIcon(type);
            this.resultAction = callback;
            this.ShowDialog();
        }

        private void SetIcon(MessageBoxType type)
        {
            string iconText = "";
            switch (type)
            {
                case MessageBoxType.Info:
                    icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00bcd4"));
                    iconText = "\xe844";
                    break;
                case MessageBoxType.Success:
                    icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2db84d"));
                    iconText = "\xe68b";
                    break;
                case MessageBoxType.Warning:
                    icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#e9af20"));
                    iconText = "\xe61c";
                    break;
                case MessageBoxType.Error:
                    icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#db3340"));
                    iconText = "\xe62b";
                    break;
                case MessageBoxType.Ask:
                    icon.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8a2be2"));
                    iconText = "\xe60c";
                    break;
                default:
                    break;
            }
            icon.Text = iconText;
        }
    }
}
