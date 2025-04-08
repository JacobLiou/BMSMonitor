using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Sofar.BMSUI.Common;
using Sofar.BMSUI.Views;

namespace Sofar.BMSUI
{
    public enum MessageBoxType
    {
        Info,
        Success,
        Warning,
        Error,
        Ask
    }

    public enum ButtonType
    {
        OK = 0,
        OKCancel = 1,
        YesNoCancel = 3,
        YesNo = 4,
        None = 5,
        Custom
    }

    public class MessageBoxHelper
    {
        /// <summary>
        /// 信息提示
        /// </summary>
        /// <param name="content">提示信息</param>
        /// <param name="caption">标题</param>
        /// <param name="callback">返回结果</param>
        public static void Info(string content, string caption, Action<MessageBoxResult> callback = null, ButtonType button = ButtonType.OKCancel) => Show(content, caption, MessageBoxType.Info, button, callback);

        /// <summary>
        /// 成功信息提示
        /// </summary>
        /// <param name="content">提示信息</param>
        /// <param name="caption">标题</param>
        /// <param name="callback">返回结果</param>
        public static void Success(string content, string caption, Action<MessageBoxResult> callback = null, ButtonType button = ButtonType.OKCancel) => Show(content, caption, MessageBoxType.Success, button, callback);

        /// <summary>
        /// 警告信息提示
        /// </summary>
        /// <param name="content">提示信息</param>
        /// <param name="caption">标题</param>
        /// <param name="callback">返回结果</param>
        public static void Warning(string content, string caption, Action<MessageBoxResult> callback = null, ButtonType button = ButtonType.OKCancel) => Show(content, caption, MessageBoxType.Warning, button, callback);

        /// <summary>
        /// 错误信息提示
        /// </summary>
        /// <param name="content">提示信息</param>
        /// <param name="caption">标题</param>
        /// <param name="callback">返回结果</param>
        public static void Error(string content, string caption, Action<MessageBoxResult> callback = null, ButtonType button = ButtonType.OKCancel) => Show(content, caption, MessageBoxType.Error, button, callback);

        /// <summary>
        /// 询问信息提示
        /// </summary>
        /// <param name="content">提示信息</param>
        /// <param name="caption">标题</param>
        /// <param name="callback">返回结果</param>
        public static void Ask(string content, string caption, Action<MessageBoxResult> callback = null, ButtonType button = ButtonType.OKCancel) => Show(content, caption, MessageBoxType.Ask, button, callback);


        public static void Show(string messageBoxText, string caption, MessageBoxType type, ButtonType button = ButtonType.OKCancel, Action<MessageBoxResult> callback = null, string confirmName = "", string cannelName = "")
        {
            var window = new MessageView();
            window.Show(messageBoxText, caption, type, button, callback, confirmName, cannelName);
        }

        /// <summary>
        /// 弹窗提示
        /// </summary>
        /// <param name="message"></param>
        public static void ShowMessageBox(MessageBoxType type, string message, string caption = "提示")
        {
            BMSDef.Current.Dispatcher.Invoke(new Action(() =>
            {
                switch (type)
                {
                    case MessageBoxType.Info:
                        MessageBoxHelper.Info(message, caption, null, ButtonType.OK);
                        break;
                    case MessageBoxType.Success:
                        MessageBoxHelper.Success(message, caption, null, ButtonType.OK);
                        break;
                    case MessageBoxType.Error:
                        MessageBoxHelper.Error(message, caption, null, ButtonType.OK);
                        break;
                    case MessageBoxType.Warning:
                        MessageBoxHelper.Warning(message, caption, null, ButtonType.OK);
                        break;
                    case MessageBoxType.Ask:
                        MessageBoxHelper.Ask(message, caption, null, ButtonType.OK);
                        break;
                    default:
                        break;
                }
            }));
        }

        /// <summary>
        /// 弹窗提示
        /// </summary>
        /// <param name="message"></param>
        public static MessageBoxResult MessageBoxDialog(string message, string caption, Sofar.BMSUI.ButtonType button)
        {
            MessageBoxResult result = MessageBoxResult.None;
            BMSDef.Current.Dispatcher.Invoke(new Action(() =>
            {
                Action<MessageBoxResult> action = (a) =>
                {
                    if (a == MessageBoxResult.OK || a == MessageBoxResult.Yes)
                        result = MessageBoxResult.Yes;
                };
                MessageBoxHelper.Show(message, caption, MessageBoxType.Ask, button, action);
            }));
            return result;
        }
    }
}
