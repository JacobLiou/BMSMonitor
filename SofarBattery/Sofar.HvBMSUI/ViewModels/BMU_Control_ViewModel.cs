using CommunityToolkit.Mvvm.ComponentModel;
using PowerKit.UI.Common;
using PowerKit.UI.Models;
using Sofar.BMSLib;
using Sofar.BMSUI;
using Sofar.BMSUI.Common;
using Sofar.HvBMSLib;
using Sofar.HvBMSUI.Models;
using Sofar.ProtocolLib;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static PowerKit.UI.Models.RealtimeData_BMS1500V_BMU;

namespace Sofar.HvBMSUI.ViewModels
{
    public partial class BMU_Control_ViewModel : ObservableObject
    {
        private string _dataCollectionInterval = "1000";
        /// <summary>
        /// 数据采集间隔(ms)
        /// </summary>
        public string DataCollectionInterval
        {
            get => _dataCollectionInterval;
            set => SetProperty(ref _dataCollectionInterval, value);
        }

        private bool _IsStartDataCollectionEnabled;
        /// <summary>
        /// 是否启用启动数据采集按钮
        /// </summary>
        public bool IsStartDataCollectionEnabled
        {
            get => _IsStartDataCollectionEnabled;
            set => SetProperty(ref _IsStartDataCollectionEnabled, value);
        }

        private bool _IsStopDataCollectionEnabled = true;
        /// <summary>
        /// 是否启用停止数据采集按钮
        /// </summary>
        public bool IsStopDataCollectionEnabled
        {
            get => _IsStopDataCollectionEnabled;
            set => SetProperty(ref _IsStopDataCollectionEnabled, value);
        }

        /// <summary>
        /// 是否启动数据保存
        /// </summary>
        public bool _isChecked_StartDataSaving;
        public bool IsChecked_StartDataSaving
        {
            get => _isChecked_StartDataSaving;
            set
            {
                SetProperty(ref _isChecked_StartDataSaving, value);
                if (value)
                {
                    DataSavingTimer = new System.Threading.Timer(TimerCallBack_DataSaving, 0, 1000, Convert.ToInt32(DataStorageInterval));
                }
                else
                {
                    DataSavingTimer.Dispose();
                }
            }
        }

        private string _dataStorageInterval = "1000";
        /// <summary>
        /// 数据保存间隔(ms)
        /// </summary>
        public string DataStorageInterval
        {
            get => _dataStorageInterval;
            set => SetProperty(ref _dataStorageInterval, value);
        }

        private string _batteryVolt;
        /// <summary>
        /// 电池电压
        /// </summary>
        public string BatteryVolt
        {
            get => _batteryVolt;
            set => SetProperty(ref _batteryVolt, value);
        }

        private string _loadVolt;
        /// <summary>
        /// 负载电压
        /// </summary>
        public string LoadVolt
        {
            get => _loadVolt;
            set => SetProperty(ref _loadVolt, value);
        }

        private string _batteryCurrent;
        /// <summary>
        /// 电池电流
        /// </summary>
        public string BatteryCurrent
        {
            get => _batteryCurrent;
            set => SetProperty(ref _batteryCurrent, value);
        }

        private string _SOC;
        /// <summary>
        /// 电池剩余容量
        /// </summary>
        public string SOC
        {
            get => _SOC;
            set => SetProperty(ref _SOC, value);
        }

        private string _SOH;
        /// <summary>
        /// 电池健康程度
        /// </summary>
        public string SOH
        {
            get => _SOH;
            set => SetProperty(ref _SOH, value);
        }

        private string _remainingCapacity;
        /// <summary>
        /// 剩余容量
        /// </summary>
        public string RemainingCapacity
        {
            get => _remainingCapacity;
            set => SetProperty(ref _remainingCapacity, value);
        }

        private string _fullCapacity;
        /// <summary>
        /// 满充容量
        /// </summary>
        public string FullCapacity
        {
            get => _fullCapacity;
            set => SetProperty(ref _fullCapacity, value);
        }

        private string _batteryStatus;
        /// <summary>
        /// 电池状态
        /// </summary>
        public string BatteryStatus
        {
            get => _batteryStatus;
            set => SetProperty(ref _batteryStatus, value);
        }

        private string _bmsStatus;
        /// <summary>
        /// BMS状态
        /// </summary>
        public string BmsStatus
        {
            get => _bmsStatus;
            set => SetProperty(ref _bmsStatus, value);
        }

        private string _syncFallSoc;
        /// <summary>
        /// 主动均衡SOC
        /// </summary>
        public string SyncFallSoc
        {
            get => _syncFallSoc;
            set => SetProperty(ref _syncFallSoc, value);
        }

        private string _activeBalanceStatus;
        /// <summary>
        /// 主动均衡状态
        /// </summary>
        public string ActiveBalanceStatus
        {
            get => _activeBalanceStatus;
            set => SetProperty(ref _activeBalanceStatus, value);
        }

        private string _chargeCurrentLimitation;
        /// <summary>
        /// 充电电流上限
        /// </summary>
        public string ChargeCurrentLimitation
        {
            get => _chargeCurrentLimitation;
            set => SetProperty(ref _chargeCurrentLimitation, value);
        }

        private string _dischargeCurrentLimitation;
        /// <summary>   
        /// 放电电流上限
        /// </summary>
        public string DischargeCurrentLimitation
        {
            get => _dischargeCurrentLimitation;
            set => SetProperty(ref _dischargeCurrentLimitation, value);
        }

        //故障等级
        private bool _alarmLevelBit0;
        public bool AlarmLevelBit0
        {
            get => _alarmLevelBit0;
            set => SetProperty(ref _alarmLevelBit0, value);
        }
        private bool _alarmLevelBit1;
        public bool AlarmLevelBit1
        {
            get => _alarmLevelBit1;
            set => SetProperty(ref _alarmLevelBit1, value);
        }
        private bool _alarmLevelBit2;
        public bool AlarmLevelBit2
        {
            get => _alarmLevelBit2;
            set => SetProperty(ref _alarmLevelBit2, value);
        }
        private bool _alarmLevelBit3;
        public bool AlarmLevelBit3
        {
            get => _alarmLevelBit3;
            set => SetProperty(ref _alarmLevelBit3, value);
        }

        private string _cumulativeDischargeCapacity;
        /// <summary>
        /// 累计放电量
        /// </summary>
        public string CumulativeDischargeCapacity
        {
            get => _cumulativeDischargeCapacity;
            set => SetProperty(ref _cumulativeDischargeCapacity, value);
        }

        private string _cumulativeChargeCapacity;
        /// <summary>
        /// 累计充电量
        /// </summary>
        public string CumulativeChargeCapacity
        {
            get => _cumulativeChargeCapacity;
            set => SetProperty(ref _cumulativeChargeCapacity, value);
        }

        private string _totalChgCap;
        /// <summary>
        /// 累计充电容量
        /// </summary>
        public string TotalChgCap
        {
            get => _totalChgCap;
            set => SetProperty(ref _totalChgCap, value);
        }

        private string _totalDsgCap;
        /// <summary>
        /// 累计放电容量
        /// </summary>
        public string TotalDsgCap
        {
            get => _totalDsgCap;
            set => SetProperty(ref _totalDsgCap, value);
        }

        private string _loadVoltN;
        /// <summary>
        /// P-对B-电压
        /// </summary>
        public string LOAD_VOLT_N
        {
            get => _loadVoltN;
            set => SetProperty(ref _loadVoltN, value);
        }

        private string _cycleTime;
        /// <summary>
        /// 循环次数
        /// </summary>
        public string CycleTime
        {
            get => _cycleTime;
            set => SetProperty(ref _cycleTime, value);
        }

        private string _batMaxCellVolt;
        /// <summary>
        /// 最高单体电压
        /// </summary>
        public string BatMaxCellVolt
        {
            get => _batMaxCellVolt;
            set => SetProperty(ref _batMaxCellVolt, value);
        }

        private string _batMaxCellVoltNum;
        /// <summary>
        /// 最高单体电压编号
        /// </summary>
        public string BatMaxCellVoltNum
        {
            get => _batMaxCellVoltNum;
            set => SetProperty(ref _batMaxCellVoltNum, value);
        }

        private string _batMinCellVolt;
        /// <summary>
        /// 最低单体电压
        /// </summary>
        public string BatMinCellVolt
        {
            get => _batMinCellVolt;
            set => SetProperty(ref _batMinCellVolt, value);
        }

        private string _batMinCellVoltNum;
        /// <summary>
        /// 最低单体电压编号
        /// </summary>
        public string BatMinCellVoltNum
        {
            get => _batMinCellVoltNum;
            set => SetProperty(ref _batMinCellVoltNum, value);
        }

        private string _batDiffCellVolt;
        /// <summary>
        /// 单体电压压差
        /// </summary>
        public string BatDiffCellVolt
        {
            get => _batDiffCellVolt;
            set => SetProperty(ref _batDiffCellVolt, value);
        }

        /// <summary>
        /// 平均单体电压
        /// </summary>
        private string _batAvgCellVolt;
        public string BatAvgCellVolt
        {
            get => _batAvgCellVolt;
            set => SetProperty(ref _batAvgCellVolt, value);
        }

        private string _cellVoltage1;
        /// <summary>
        /// 电压1
        /// </summary>
        public string CellVoltage1
        {
            get => _cellVoltage1;
            set => SetProperty(ref _cellVoltage1, value);
        }

        private string _cellVoltage2;
        /// <summary>
        /// 电压2
        /// </summary>
        public string CellVoltage2
        {
            get => _cellVoltage2;
            set => SetProperty(ref _cellVoltage2, value);
        }

        private string _cellVoltage3;
        /// <summary>
        /// 电压3
        /// </summary>
        public string CellVoltage3
        {
            get => _cellVoltage3;
            set => SetProperty(ref _cellVoltage3, value);
        }

        private string _cellVoltage4;
        /// <summary>
        /// 电压4
        /// </summary>
        public string CellVoltage4
        {
            get => _cellVoltage4;
            set => SetProperty(ref _cellVoltage4, value);
        }

        private string _cellVoltage5;
        /// <summary>
        /// 电压5
        /// </summary>
        public string CellVoltage5
        {
            get => _cellVoltage5;
            set => SetProperty(ref _cellVoltage5, value);
        }

        private string _cellVoltage6;
        /// <summary>
        /// 电压6
        /// </summary>
        public string CellVoltage6
        {
            get => _cellVoltage6;
            set => SetProperty(ref _cellVoltage6, value);
        }

        private string _cellVoltage7;
        /// <summary>
        /// 电压7
        /// </summary>
        public string CellVoltage7
        {
            get => _cellVoltage7;
            set => SetProperty(ref _cellVoltage7, value);
        }

        private string _cellVoltage8;
        /// <summary>
        /// 电压8
        /// </summary>
        public string CellVoltage8
        {
            get => _cellVoltage8;
            set => SetProperty(ref _cellVoltage8, value);
        }

        private string _cellVoltage9;
        /// <summary>
        /// 电压9
        /// </summary>
        public string CellVoltage9
        {
            get => _cellVoltage9;
            set => SetProperty(ref _cellVoltage9, value);
        }

        private string _cellVoltage10;
        /// <summary>
        /// 电压10
        /// </summary>
        public string CellVoltage10
        {
            get => _cellVoltage10;
            set => SetProperty(ref _cellVoltage10, value);
        }

        private string _cellVoltage11;
        /// <summary>
        /// 电压11
        /// </summary>
        public string CellVoltage11
        {
            get => _cellVoltage11;
            set => SetProperty(ref _cellVoltage11, value);
        }

        private string _cellVoltage12;
        /// <summary>
        /// 电压12
        /// </summary>
        public string CellVoltage12
        {
            get => _cellVoltage12;
            set => SetProperty(ref _cellVoltage12, value);
        }

        private string _cellVoltage13;
        /// <summary>
        /// 电压13
        /// </summary>
        public string CellVoltage13
        {
            get => _cellVoltage13;
            set => SetProperty(ref _cellVoltage13, value);
        }

        private string _cellVoltage14;
        /// <summary>
        /// 电压14
        /// </summary>
        public string CellVoltage14
        {
            get => _cellVoltage14;
            set => SetProperty(ref _cellVoltage14, value);
        }

        private string _cellVoltage15;
        /// <summary>
        /// 电压15
        /// </summary>
        public string CellVoltage15
        {
            get => _cellVoltage15;
            set => SetProperty(ref _cellVoltage15, value);
        }

        private string _cellVoltage16;
        /// <summary>
        /// 电压16
        /// </summary>
        public string CellVoltage16
        {
            get => _cellVoltage16;
            set => SetProperty(ref _cellVoltage16, value);
        }

        private string _fault;
        /// <summary>
        /// 故障状态
        /// </summary>
        public string Fault
        {
            get => _fault;
            set => SetProperty(ref _fault, value);
        }

        private string _warning;
        /// <summary>
        /// 告警状态
        /// </summary>
        public string Warning
        {
            get => _warning;
            set => SetProperty(ref _warning, value);
        }

        private string _protection;
        /// <summary>
        /// 保护状态
        /// </summary>
        public string Protection
        {
            get => _protection;
            set => SetProperty(ref _protection, value);
        }

        private string _fault2;
        /// <summary>
        /// 故障状态
        /// </summary>
        public string Fault2
        {
            get => _fault2;
            set => SetProperty(ref _fault2, value);
        }

        private string _warning2;
        /// <summary>
        /// 告警状态
        /// </summary>
        public string Warning2
        {
            get => _warning2;
            set => SetProperty(ref _warning2, value);
        }

        private string _protection2;
        /// <summary>
        /// 保护状态
        /// </summary>
        public string Protection2
        {
            get => _protection2;
            set => SetProperty(ref _protection2, value);
        }

        private string _chargeMosEnable;
        /// <summary>
        /// 充电MOS
        /// </summary>
        public string ChargeMosEnable
        {
            get => _chargeMosEnable;
            set => SetProperty(ref _chargeMosEnable, value);
        }

        private string _dischargeMosEnable;
        /// <summary>
        /// 放电MOS
        /// </summary>
        public string DischargeMosEnable
        {
            get => _dischargeMosEnable;
            set => SetProperty(ref _dischargeMosEnable, value);
        }

        private string _prechgMosEnable;
        /// <summary>
        /// 预充MOS
        /// </summary>
        public string PrechgMosEnable
        {
            get => _prechgMosEnable;
            set => SetProperty(ref _prechgMosEnable, value);
        }

        private string _stopChgEnable;
        /// <summary>
        /// 充电急停
        /// </summary>
        public string StopChgEnable
        {
            get => _stopChgEnable;
            set => SetProperty(ref _stopChgEnable, value);
        }

        private string _heatEnable;
        /// <summary>
        /// 加热MOS
        /// </summary>
        public string HeatEnable
        {
            get => _heatEnable;
            set => SetProperty(ref _heatEnable, value);
        }

        //private string _equaState;
        ///// <summary>
        ///// 均衡状态—低8串电池均衡状态 高8串电池均衡状态 新协议已删除
        ///// </summary>
        //public string EquaState
        //{
        //    get => _equaState;
        //    set => SetProperty(ref _equaState, value);
        //}

        private string _heatRequest;
        /// <summary>
        /// 加热请求
        /// </summary>
        public string HeatRequest
        {
            get => _heatRequest;
            set => SetProperty(ref _heatRequest, value);
        }

        private string _chargeEnable;
        /// <summary>
        /// 允许充电
        /// </summary>
        public string ChargeEnable
        {
            get => _chargeEnable;
            set => SetProperty(ref _chargeEnable, value);
        }

        private string _dischargeEnable;
        /// <summary>
        /// 允许放电
        /// </summary>
        public string DischargeEnable
        {
            get => _dischargeEnable;
            set => SetProperty(ref _dischargeEnable, value);
        }

        private string _bmuCutOffRequest;
        /// <summary>
        /// 切断继电器
        /// </summary>
        public string BmuCutOffRequest
        {
            get => _bmuCutOffRequest;
            set => SetProperty(ref _bmuCutOffRequest, value);
        }

        private string _bmuPowOffRequest;
        /// <summary>
        /// BMU关机
        /// </summary>
        public string BmuPowOffRequest
        {
            get => _bmuPowOffRequest;
            set => SetProperty(ref _bmuPowOffRequest, value);
        }

        private string _forceChrgRequest;
        /// <summary>
        /// 请求强充
        /// </summary>
        public string ForceChrgRequest
        {
            get => _forceChrgRequest;
            set => SetProperty(ref _forceChrgRequest, value);
        }

        private string _chagreStatus;
        /// <summary>
        /// 充满
        /// </summary>
        public string ChagreStatus
        {
            get => _chagreStatus;
            set => SetProperty(ref _chagreStatus, value);
        }

        private string _dischargeStatus;
        /// <summary>
        /// 放空
        /// </summary>
        public string DischargeStatus
        {
            get => _dischargeStatus;
            set => SetProperty(ref _dischargeStatus, value);
        }

        private string _diIO;
        /// <summary>
        /// 编址输入电平
        /// </summary>
        public string DiIO
        {
            get => _diIO;
            set => SetProperty(ref _diIO, value);
        }

        private string _chargeIO;
        /// <summary>
        /// 补电输入电平
        /// </summary>
        public string ChargeIO
        {
            get => _chargeIO;
            set => SetProperty(ref _chargeIO, value);
        }

        private string _balanceBusVoltage;
        /// <summary>
        /// 均衡母线电压
        /// </summary>
        public string BalanceBusVoltage
        {
            get => _balanceBusVoltage;
            set => SetProperty(ref _balanceBusVoltage, value);
        }

        private string _balanceCurrent;
        /// <summary>
        /// 均衡电流
        /// </summary>
        public string BalanceCurrent
        {
            get => _balanceCurrent;
            set => SetProperty(ref _balanceCurrent, value);
        }

        private string _activeBalanceMaxCellVolt;
        /// <summary>
        /// 主动均衡最大单体电压
        /// </summary>
        public string ActiveBalanceMaxCellVolt
        {
            get => _activeBalanceMaxCellVolt;
            set => SetProperty(ref _activeBalanceMaxCellVolt, value);
        }

        private string _batAverageTemp;
        /// <summary>
        /// 电芯平均温度
        /// </summary>
        public string BatAverageTemp
        {
            get => _batAverageTemp;
            set => SetProperty(ref _batAverageTemp, value);
        }

        private string _batDiffCellTemp;
        /// <summary>
        /// 电芯温差
        /// </summary>
        public string BatDiffCellTemp
        {
            get => _batDiffCellTemp;
            set => SetProperty(ref _batDiffCellTemp, value);
        }

        private string _activeBalanceCellSoc;
        /// <summary>
        /// 主动均衡SOC
        /// </summary>
        public string ActiveBalanceCellSoc
        {
            get => _activeBalanceCellSoc;
            set => SetProperty(ref _activeBalanceCellSoc, value);
        }

        private string _activeBalanceAccCap;
        /// <summary>
        /// 主动均衡累计容量
        /// </summary>
        public string ActiveBalanceAccCap
        {
            get => _activeBalanceAccCap;
            set => SetProperty(ref _activeBalanceAccCap, value);
        }

        private string _activeBalanceRemainCap;
        /// <summary>
        /// 主动均衡剩余容量
        /// </summary>
        public string ActiveBalanceRemainCap
        {
            get => _activeBalanceRemainCap;
            set => SetProperty(ref _activeBalanceRemainCap, value);
        }

        private string _bmuSoftwareVersion;
        /// <summary>
        /// BMU软件版本（0x06A）
        /// </summary>
        public string BMUSaftwareVersion
        {
            get => _bmuSoftwareVersion;
            set => SetProperty(ref _bmuSoftwareVersion, value);
        }

        private string _bmuHardwareVersion;
        /// <summary>
        /// BMU硬件版本（0x06A）
        /// </summary>
        public string BMUHardwareVersion
        {
            get => _bmuHardwareVersion;
            set => SetProperty(ref _bmuHardwareVersion, value);
        }

        private string _bmuCanVersion;
        /// <summary>
        /// BMU-CAN版本
        /// </summary>
        public string BMUCanVersion
        {
            get => _bmuCanVersion;
            set => SetProperty(ref _bmuCanVersion, value);
        }

        private string _batNominalCapacity;
        /// <summary>
        /// 标定容量
        /// </summary>
        public string BatNominalCapacity
        {
            get => _batNominalCapacity;
            set => SetProperty(ref _batNominalCapacity, value);
        }

        private string _registerName;
        /// <summary>
        /// 供应商厂家名称
        /// </summary>
        public string RegisterName
        {
            get => _registerName;
            set => SetProperty(ref _registerName, value);
        }

        private string _batType;
        /// <summary>
        /// 锂电池类型
        /// </summary>
        public string BatType
        {
            get => _batType;
            set => SetProperty(ref _batType, value);
        }

        private string _manufacturerName;
        /// <summary>
        /// 厂家名称（0x06C）
        /// </summary>
        public string ManufacturerName
        {
            get => _manufacturerName;
            set => SetProperty(ref _manufacturerName, value);
        }

        private string _resetMode;
        /// <summary>
        /// 复位方式
        /// </summary>
        public string ResetMode
        {
            get => _resetMode;
            set => SetProperty(ref _resetMode, value);
        }

        private string _auxVolt;
        /// <summary>
        /// 辅源电压
        /// </summary>
        public string AuxVolt
        {
            get => _auxVolt;
            set => SetProperty(ref _auxVolt, value);
        }

        private string _envTemperature;
        /// <summary>
        /// 环境温度
        /// </summary>
        public string EnvTemperature
        {
            get => _envTemperature;
            set => SetProperty(ref _envTemperature, value);
        }

        private string _mosTemperature;
        /// <summary>
        /// Mos温度
        /// </summary>
        public string MosTemperature
        {
            get => _mosTemperature;
            set => SetProperty(ref _mosTemperature, value);
        }

        private string _balanceTemperature1;
        /// <summary>
        /// 被动均衡温度1
        /// </summary>
        public string BalanceTemperature1
        {
            get => _balanceTemperature1;
            set => SetProperty(ref _balanceTemperature1, value);
        }

        private string _balanceTemperature2;
        /// <summary>
        /// 被动均衡温度2
        /// </summary>
        public string BalanceTemperature2
        {
            get => _balanceTemperature2;
            set => SetProperty(ref _balanceTemperature2, value);
        }

        private string _dcdcTemperature1;
        /// <summary>
        /// 主动均衡温度1
        /// </summary>
        public string DcdcTemperature1
        {
            get => _dcdcTemperature1;
            set => SetProperty(ref _dcdcTemperature1, value);
        }

        private string _dcdcTemperature2;
        /// <summary>
        /// 主动均衡温度2
        /// </summary>
        public string DcdcTemperature2
        {
            get => _dcdcTemperature2;
            set => SetProperty(ref _dcdcTemperature2, value);
        }

        private string _batMaxCellTemp;
        /// <summary>
        /// 最高单体温度
        /// </summary>
        public string BatMaxCellTemp
        {
            get => _batMaxCellTemp;
            set => SetProperty(ref _batMaxCellTemp, value);
        }

        private string _batMaxCellTempNum;
        /// <summary>
        /// 最高单体温度编号
        /// </summary>
        public string BatMaxCellTempNum
        {
            get => _batMaxCellTempNum;
            set => SetProperty(ref _batMaxCellTempNum, value);
        }

        private string _batMinCellTemp;
        /// <summary>
        /// 最低单体温度
        /// </summary>
        public string BatMinCellTemp
        {
            get => _batMinCellTemp;
            set => SetProperty(ref _batMinCellTemp, value);
        }

        private string _batMinCellTempNum;
        /// <summary>
        /// 最低单体温度编号
        /// </summary>
        public string BatMinCellTempNum
        {
            get => _batMinCellTempNum;
            set => SetProperty(ref _batMinCellTempNum, value);
        }

        private string _cellTemperature1;
        /// <summary>
        /// 温度1
        /// </summary>
        public string CellTemperature1
        {
            get => _cellTemperature1;
            set => SetProperty(ref _cellTemperature1, value);
        }

        private string _cellTemperature2;
        /// <summary>
        /// 温度2
        /// </summary>
        public string CellTemperature2
        {
            get => _cellTemperature2;
            set => SetProperty(ref _cellTemperature2, value);
        }

        private string _cellTemperature3;
        /// <summary>
        /// 温度3
        /// </summary>
        public string CellTemperature3
        {
            get => _cellTemperature3;
            set => SetProperty(ref _cellTemperature3, value);
        }

        private string _cellTemperature4;
        /// <summary>
        /// 温度4
        /// </summary>
        public string CellTemperature4
        {
            get => _cellTemperature4;
            set => SetProperty(ref _cellTemperature4, value);
        }

        private string _cellTemperature5;
        /// <summary>
        /// 温度4
        /// </summary>
        public string CellTemperature5
        {
            get => _cellTemperature5;
            set => SetProperty(ref _cellTemperature5, value);
        }

        private string _cellTemperature6;
        /// <summary>
        /// 温度6
        /// </summary>
        public string CellTemperature6
        {
            get => _cellTemperature6;
            set => SetProperty(ref _cellTemperature6, value);
        }

        private string _cellTemperature7;
        /// <summary>
        /// 温度7
        /// </summary>
        public string CellTemperature7
        {
            get => _cellTemperature7;
            set => SetProperty(ref _cellTemperature7, value);
        }

        private string _cellTemperature8;
        /// <summary>
        /// 温度8
        /// </summary>
        public string CellTemperature8
        {
            get => _cellTemperature8;
            set => SetProperty(ref _cellTemperature8, value);
        }

        private string _powerTemperture1;
        /// <summary>
        /// 功率正端子温度
        /// </summary>
        public string PowerTemperture1
        {
            get => _powerTemperture1;
            set => SetProperty(ref _powerTemperture1, value);
        }

        private string _powerTemperture2;
        /// <summary>
        /// 功率负端子温度
        /// </summary>
        public string PowerTemperture2
        {
            get => _powerTemperture2;
            set => SetProperty(ref _powerTemperture2, value);
        }

        private string _heatCur;
        /// <summary>
        /// 加热膜电流
        /// </summary>
        public string HeatCur
        {
            get => _heatCur;
            set => SetProperty(ref _heatCur, value);
        }

        private string _heatRelayVol;
        /// <summary>
        /// 加热膜电压
        /// </summary>
        public string HeatRelayVol
        {
            get => _heatRelayVol;
            set => SetProperty(ref _heatRelayVol, value);
        }

        private string _chgCurOffsetVolt;
        /// <summary>
        /// 充电电流偏压
        /// </summary>
        public string ChgCurOffsetVolt
        {
            get => _chgCurOffsetVolt;
            set => SetProperty(ref _chgCurOffsetVolt, value);
        }

        private string _dsgCurOffsetVolt;
        /// <summary>
        /// 放电电流偏压
        /// </summary>
        public string DsgCurOffsetVolt
        {
            get => _dsgCurOffsetVolt;
            set => SetProperty(ref _dsgCurOffsetVolt, value);
        }

        private string _bmsSoftwareVersion;
        /// <summary>
        /// BMS软件版本
        /// </summary>
        public string BMSSoftwareVersion
        {
            get => _bmsSoftwareVersion;
            set => SetProperty(ref _bmsSoftwareVersion, value);
        }

        private string _bmsHardwareVersion;
        /// <summary>
        /// BMS硬件版本
        /// </summary>
        public string BMSHardwareVersion
        {
            get => _bmsHardwareVersion;
            set => SetProperty(ref _bmsHardwareVersion, value);
        }

        private string _sn;
        /// <summary>
        /// 序列号
        /// </summary>
        public string SN
        {
            get => _sn;
            set => SetProperty(ref _sn, value);
        }

        private string _flashData;
        /// <summary>
        /// ATE Flash测试：0x101FXXE0 Flash数据
        /// </summary>
        public string FlashData
        {
            get => _flashData;
            set => SetProperty(ref _flashData, value);
        }

        private bool _ischargeEnable;
        /// <summary>
        /// 是否允许充电
        /// </summary>
        public bool IsChargeEnable
        {
            get { return _ischargeEnable; }
            set
            {
                _ischargeEnable = value;
                OnPropertyChanged(nameof(IsChargeEnable));
            }
        }

        private bool _isDischargeEnable;
        /// <summary>
        /// 是否允许放电
        /// </summary>
        public bool IsDischargeEnable
        {
            get { return _isDischargeEnable; }
            set
            {
                _isDischargeEnable = value;
                OnPropertyChanged(nameof(IsDischargeEnable));
            }
        }

        private bool _isBmuCutOffRequest;
        /// <summary>
        /// 是否切断继电器
        /// </summary>
        public bool IsBmuCutOffRequest
        {
            get { return _isBmuCutOffRequest; }
            set
            {
                _isBmuCutOffRequest = value;
                OnPropertyChanged(nameof(IsBmuCutOffRequest));
            }
        }

        private bool _isBmuPowOffRequest;
        /// <summary>
        /// 是否BMU关机
        /// </summary>
        public bool IsBmuPowOffRequest
        {
            get { return _isBmuPowOffRequest; }
            set
            {
                _isBmuPowOffRequest = value;
                OnPropertyChanged(nameof(IsBmuPowOffRequest));
            }
        }

        private bool _isForceChrgRequest;
        /// <summary>
        /// 是否强制充电
        /// </summary>
        public bool IsForceChrgRequest
        {
            get { return _isForceChrgRequest; }
            set
            {
                _isForceChrgRequest = value;
                OnPropertyChanged(nameof(IsForceChrgRequest));
            }
        }

        private bool _isChagreStatus;
        /// <summary>
        /// 是否充满
        /// </summary>
        public bool IsChagreStatus
        {
            get { return _isChagreStatus; }
            set
            {
                _isChagreStatus = value;
                OnPropertyChanged(nameof(IsChagreStatus));
            }
        }

        private bool _isDisChagreStatus;
        /// <summary>
        /// 是否放空
        /// </summary>
        public bool IsDisChargeStatus
        {
            get { return _isDisChagreStatus; }
            set
            {
                _isDisChagreStatus = value;
                OnPropertyChanged(nameof(IsDisChargeStatus));
            }
        }

        private bool _isDiIO;
        /// <summary>
        /// 是否编址输入电平
        /// </summary>
        public bool IsDiIO
        {
            get { return _isDiIO; }
            set
            {
                _isDiIO = value;
                OnPropertyChanged(nameof(IsDiIO));
            }
        }

        private bool _isChargeIO;
        /// <summary>
        /// 是否补电输入电平
        /// </summary>
        public bool IsChargeIO
        {
            get { return _isChargeIO; }
            set
            {
                _isChargeIO = value;
                OnPropertyChanged(nameof(IsChargeIO));
            }
        }

        /// <summary>
        /// 电解液漏液浓度
        /// </summary>
        private string _electrolyteLeakageConcentration;
        public string ElectrolyteLeakageConcentration
        {
            get => _electrolyteLeakageConcentration;
            set => SetProperty(ref _electrolyteLeakageConcentration, value);
        }

        /// <summary>
        /// 电解液漏液浓度(电池包1)
        /// </summary>
        private string _electrolyteLeakageConcentrationPack1;
        public string ElectrolyteLeakageConcentrationPack1
        {
            get => _electrolyteLeakageConcentrationPack1;
            set => SetProperty(ref _electrolyteLeakageConcentrationPack1, value);
        }
        /// <summary>
        /// 电解液漏液浓度(电池包2)
        /// </summary>
        private string _electrolyteLeakageConcentrationPack2;
        public string ElectrolyteLeakageConcentrationPack2
        {
            get => _electrolyteLeakageConcentrationPack2;
            set => SetProperty(ref _electrolyteLeakageConcentrationPack2, value);
        }
        /// <summary>
        /// 电解液漏液浓度(电池包3)
        /// </summary>
        private string _electrolyteLeakageConcentrationPack3;
        public string ElectrolyteLeakageConcentrationPack3
        {
            get => _electrolyteLeakageConcentrationPack3;
            set => SetProperty(ref _electrolyteLeakageConcentrationPack3, value);
        }
        /// <summary>
        /// 电解液漏液浓度(电池包4)
        /// </summary>
        private string _electrolyteLeakageConcentrationPack4;
        public string ElectrolyteLeakageConcentrationPack4
        {
            get => _electrolyteLeakageConcentrationPack4;
            set => SetProperty(ref _electrolyteLeakageConcentrationPack4, value);
        }

        // 记录报警信息的数据列表
        public ObservableCollection<RealtimeData_BMS1500V_BMU.AlarmMessageData> _alarmMessageDataList;
        public ObservableCollection<RealtimeData_BMS1500V_BMU.AlarmMessageData> AlarmMessageDataList
        {
            get { return _alarmMessageDataList; }
            set
            {
                _alarmMessageDataList = value;
                OnPropertyChanged(nameof(AlarmMessageDataList));
            }
        }

        // 老化模式请求列表
        public ObservableCollection<string> Request1List { get; } = new ObservableCollection<string>
        {
            "0x00：查询当前老化模式开启状态",
            "0xAA：开启老化模式",
            "0x55：关闭老化模式"

        };

        private string _selectedRequest1;
        /// <summary>
        /// 老化模式请求  
        /// 0x00：查询
        /// 0xAA：开启
        /// 0x55：关闭
        /// </summary>
        public string SelectedRequest1
        {
            get { return _selectedRequest1; }
            set
            {
                _selectedRequest1 = value;
                OnPropertyChanged(nameof(SelectedRequest1));
            }
        }

        // 标定模式请求列表
        public ObservableCollection<string> Request2List { get; } = new ObservableCollection<string>
        {
            "0x00：查询当前标定模式开启状态",
            "0xAA：开启标定模式",
            "0x55：关闭标定模式"

        };

        private string _selectedRequest2;
        /// <summary>
        /// 标定模式请求  
        /// 0x00：查询
        /// 0xAA：开启
        /// 0x55：关闭
        /// </summary>
        public string SelectedRequest2
        {
            get { return _selectedRequest2; }
            set
            {
                _selectedRequest2 = value;
                OnPropertyChanged(nameof(SelectedRequest2));
            }
        }

        // ATE测试模式请求列表
        public ObservableCollection<string> Request3List { get; } = new ObservableCollection<string>
        {
            "0x00：查询当前ATE测试模式开启状态",
            "0xAA：开启当前ATE测试模式",
            "0x55：关闭当前ATE测试模式"

        };

        private string _selectedRequest3;
        /// <summary>
        /// ATE测试模式请求  
        /// 0x00：查询
        /// 0xAA：开启
        /// 0x55：关闭
        /// </summary>
        public string SelectedRequest3
        {
            get { return _selectedRequest3; }
            set
            {
                _selectedRequest3 = value;
                OnPropertyChanged(nameof(SelectedRequest3));
            }
        }

        // PCU老化模式请求列表
        public ObservableCollection<string> Request4List { get; } = new ObservableCollection<string>
        {
            "0x00：查询当前PCU老化模式开启状态",
            "0xAA：开启当前PCU老化模式",
            "0x55：关闭当前PCU老化模式"

        };

        private string _selectedRequest4;
        /// <summary>
        /// PCU老化模式请求
        /// 0x00：查询
        /// 0xAA：开启
        /// 0x55：关闭
        /// </summary>
        public string SelectedRequest4
        {
            get { return _selectedRequest4; }
            set
            {
                _selectedRequest4 = value;
                OnPropertyChanged(nameof(SelectedRequest4));
            }
        }

        // 强制控制开关列表
        public ObservableCollection<string> Request5List { get; } = new ObservableCollection<string>
        {
            "0x00：无效操作",
            "0xAA：强制控制开启",
            "0x55：强制控制关闭"
        };

        private string _selectedRequest5;
        /// <summary>
        /// 强制控制开关
        /// 0x00：无效
        /// 0xAA：开启
        /// 0x55：关闭
        /// </summary>
        public string SelectedRequest5
        {
            get { return _selectedRequest5; }
            set
            {
                _selectedRequest5 = value;
                OnPropertyChanged(nameof(SelectedRequest5));
            }
        }

        // 标定参数恢复列表
        public ObservableCollection<string> Request6List { get; } = new ObservableCollection<string>
        {
            "0x00：无效操作",
            "0xAA：恢复所有默认设置",
            "0x55：其他"
        };

        private string _selectedRequest6;
        /// <summary>
        /// 标定参数恢复
        /// 0x00：无效
        /// 0xAA：开启
        /// 0x55：其他
        /// </summary>
        public string SelectedRequest6
        {
            get { return _selectedRequest6; }
            set
            {
                _selectedRequest6 = value;
                OnPropertyChanged(nameof(SelectedRequest6));
            }
        }

        private string _packCurrent;
        /// <summary>
        /// 电池包电流设置
        /// 充电正，放电负       
        /// </summary>
        public string PackCurrent
        {
            get { return _packCurrent; }
            set
            {
                _packCurrent = value;
                OnPropertyChanged(nameof(PackCurrent));
            }
        }

        // 充放电状态列表
        public ObservableCollection<string> BatteryStatusList { get; } = new ObservableCollection<string>
        {
            "0x00：待机状态",
            "0x01：充电状态",
            "0x02：放电状态"
        };

        private string _selectedBatteryStatus;
        /// <summary>
        /// 充放电状态设置
        /// 0x0：待机状态
        /// 0x1：充电状态
        /// 0x2：放电状态
        /// </summary>
        public string SelectedBatteryStatus
        {
            get { return _selectedBatteryStatus; }
            set
            {
                _selectedBatteryStatus = value;
                OnPropertyChanged(nameof(SelectedBatteryStatus));
            }
        }

        // 充满状态列表
        public ObservableCollection<string> FullChagreStatusList { get; } = new ObservableCollection<string>
        {
            "0x00：正常",
            "0x01：充满"
        };

        private string _selectedFullChagreStatus;
        /// <summary>
        /// 充满状态设置
        /// 0x00：正常
        /// 0x01：充满     
        /// </summary>
        public string SelectedFullChagreStatus
        {
            get { return _selectedFullChagreStatus; }
            set
            {
                _selectedFullChagreStatus = value;
                OnPropertyChanged(nameof(SelectedFullChagreStatus));
            }
        }

        // 放空状态列表
        public ObservableCollection<string> FullDischargeStatusList { get; } = new ObservableCollection<string>
        {
            "0x00：正常",
            "0x01：放空"
        };

        private string _selectedFullDischargeStatus;
        /// <summary>
        /// 放空状态设置
        /// 0x00：正常
        /// 0x01：放空     
        /// </summary>
        public string SelectedFullDischargeStatus
        {
            get { return _selectedFullDischargeStatus; }
            set
            {
                _selectedFullDischargeStatus = value;
                OnPropertyChanged(nameof(SelectedFullDischargeStatus));
            }
        }

        // 关机状态列表
        public ObservableCollection<string> ShutDownList { get; } = new ObservableCollection<string>
        {
            "0x00：正常",
            "0x01：关机"
        };

        private string _selectedShutDown;
        /// <summary>
        /// 关机状态设置
        /// 0x00：正常
        /// 0x01：关机     
        /// </summary>
        public string SelectedShutDown
        {
            get { return _selectedShutDown; }
            set
            {
                _selectedShutDown = value;
                OnPropertyChanged(nameof(SelectedShutDown));
            }
        }

        // 均衡补电标志列表
        public ObservableCollection<string> BalRechargeList { get; } = new ObservableCollection<string>
        {
            "0x00：正常",
            "0x01：补电"
        };

        private string _selectedBalRecharge;
        /// <summary>
        /// 均衡补电标志设置
        /// 0x00：正常
        /// 0x01：补电     
        /// </summary>
        public string SelectedBalRecharge
        {
            get { return _selectedBalRecharge; }
            set
            {
                _selectedBalRecharge = value;
                OnPropertyChanged(nameof(SelectedBalRecharge));
            }
        }

        // 均衡补电标志列表
        public ObservableCollection<string> RecoverFlagList { get; } = new ObservableCollection<string>
        {
            "0x01：复归"
        };

        private string _selectedRecoverFlag;
        /// <summary>
        /// 复归标志设置   
        /// 0x01：复归     
        /// </summary>
        public string SelectedRecoverFlag
        {
            get { return _selectedRecoverFlag; }
            set
            {
                _selectedRecoverFlag = value;
                OnPropertyChanged(nameof(SelectedRecoverFlag));
            }
        }

        private string _sync_Fall_Soc;
        /// <summary>
        /// 同步下降soc 
        /// </summary>
        public string Sync_Fall_Soc
        {
            get { return _sync_Fall_Soc; }
            set
            {
                _sync_Fall_Soc = value;
                OnPropertyChanged(nameof(Sync_Fall_Soc));
            }
        }

        // 主动均衡充电使能列表
        public ObservableCollection<string> ActiveBalanceCtrlList { get; } = new ObservableCollection<string>
        {
            "0x00：禁止",
            "0x01：放电",
            "0x02：充电",
            "0xff：无效"
        };

        private string _selectedActiveBalanceCtrl;
        /// <summary>
        /// 主动均衡充电使能        
        ///  0x00：禁止
        ///  0x01：放电
        ///  0x02：充电
        ///  0xff：无效
        /// </summary>
        public string SelectedActiveBalanceCtrl
        {
            get { return _selectedActiveBalanceCtrl; }
            set
            {
                _selectedActiveBalanceCtrl = value;
                OnPropertyChanged(nameof(SelectedActiveBalanceCtrl));
            }
        }

        private string _pack_Active_Balance_Cur;
        /// <summary>
        /// 设置主动均衡电流 
        /// 默认10000      
        /// </summary>
        public string Pack_Active_Balance_Cur
        {
            get { return _pack_Active_Balance_Cur; }
            set
            {
                _pack_Active_Balance_Cur = value;
                OnPropertyChanged(nameof(Pack_Active_Balance_Cur));
            }
        }

        private string _pack_Active_Balance_Cap;
        /// <summary>
        /// 设置主动均衡电流 
        /// 默认10000      
        /// </summary>
        public string Pack_Active_Balance_Cap
        {
            get { return _pack_Active_Balance_Cap; }
            set
            {
                _pack_Active_Balance_Cap = value;
                OnPropertyChanged(nameof(_pack_Active_Balance_Cap));
            }
        }

        private string _Bal_open_volt;
        /// <summary>
        /// 均衡开启电压(mV)
        /// </summary>
        public string Bal_open_volt
        {
            get { return _Bal_open_volt; }
            set
            {
                _Bal_open_volt = value;
                OnPropertyChanged(nameof(Bal_open_volt));
            }
        }

        private string _Bal_open_volt_diff;
        /// <summary>
        /// 均衡开启压差(mV)
        /// </summary>
        public string Bal_open_volt_diff
        {
            get { return _Bal_open_volt_diff; }
            set
            {
                _Bal_open_volt_diff = value;
                OnPropertyChanged(nameof(Bal_open_volt_diff));
            }
        }

        private string _Full_chg_volt;
        /// <summary>
        /// 满充电压(mV)
        /// </summary>
        public string Full_chg_volt
        {
            get { return _Full_chg_volt; }
            set
            {
                _Full_chg_volt = value;
                OnPropertyChanged(nameof(Full_chg_volt));
            }
        }

        private string _Heating_film_open_temp;
        /// <summary>
        /// 加热模开启温度
        /// </summary>
        public string Heating_film_open_temp
        {
            get { return _Heating_film_open_temp; }
            set
            {
                _Heating_film_open_temp = value;
                OnPropertyChanged(nameof(Heating_film_open_temp));
            }
        }

        private string _HeatingHeating_film_close_temp;
        /// <summary>
        /// 加热模关闭温度
        /// </summary>
        public string HeatingHeating_film_close_temp
        {
            get { return _HeatingHeating_film_close_temp; }
            set
            {
                _HeatingHeating_film_close_temp = value;
                OnPropertyChanged(nameof(HeatingHeating_film_close_temp));
            }
        }

        private string _pack_stop_volt;
        /// <summary>
        /// 电池包截止电压
        /// </summary>
        public string Pack_stop_volt
        {
            get { return _pack_stop_volt; }
            set
            {
                _pack_stop_volt = value;
                OnPropertyChanged(nameof(Pack_stop_volt));
            }
        }

        private string _Pack_stop_cur;
        /// <summary>
        /// 电池包截止电流
        /// </summary>
        public string Pack_stop_cur
        {
            get { return _Pack_stop_cur; }
            set
            {
                _Pack_stop_cur = value;
                OnPropertyChanged(nameof(Pack_stop_cur));
            }
        }

        private string _Rated_capacity;
        /// <summary>
        /// 参数标定0x1023XXE0—额定容量
        /// </summary>
        public string Rated_capacity
        {
            get { return _Rated_capacity; }
            set
            {
                _Rated_capacity = value;
                OnPropertyChanged(nameof(Rated_capacity));
            }
        }

        private string _Cell_volt_num;
        /// <summary>
        /// 参数标定0x1023XXE0—单体电压个数
        /// </summary>
        public string Cell_Volt_Num
        {
            get { return _Cell_volt_num; }
            set
            {
                _Cell_volt_num = value;
                OnPropertyChanged(nameof(Cell_Volt_Num));
            }
        }

        private string _Cell_temp_num;
        /// <summary>
        /// 参数标定0x1023XXE0—单体温度个数
        /// </summary>
        public string Cell_temp_num
        {
            get { return _Cell_temp_num; }
            set
            {
                _Cell_temp_num = value;
                OnPropertyChanged(nameof(Cell_temp_num));
            }
        }

        private string _Cumulative_chg_capacity;
        /// <summary>
        /// 参数标定0x1024XXE0—累计充电容量
        /// </summary>
        public string Cumulative_Chg_Capacity
        {
            get { return _Cumulative_chg_capacity; }
            set
            {
                _Cumulative_chg_capacity = value;
                OnPropertyChanged(nameof(Cumulative_Chg_Capacity));
            }
        }

        private string _Cumulative_dsg_capacity;
        /// <summary>
        /// 参数标定0x1024XXE0—累计放电容量
        /// </summary>
        public string Cumulative_dsg_capacity
        {
            get { return _Cumulative_dsg_capacity; }
            set
            {
                _Cumulative_dsg_capacity = value;
                OnPropertyChanged(nameof(Cumulative_dsg_capacity));
            }
        }

        private string _soc;
        /// <summary>
        /// 参数标定0x1025XXE0—SOC
        /// </summary>
        public string soc
        {
            get { return _soc; }
            set
            {
                _soc = value;
                OnPropertyChanged(nameof(soc));
            }
        }
        private string _Full_chg_capacity;
        /// <summary>
        /// 参数标定0x1025XXE0—满充容量
        /// </summary>
        public string Full_chg_capacity
        {
            get { return _Full_chg_capacity; }
            set
            {
                _Full_chg_capacity = value;
                OnPropertyChanged(nameof(Full_chg_capacity));
            }
        }

        private string _Surplus_capacity;
        /// <summary>
        /// 参数标定0x1025XXE0—剩余容量
        /// </summary>
        public string Surplus_capacity
        {
            get { return _Surplus_capacity; }
            set
            {
                _Surplus_capacity = value;
                OnPropertyChanged(nameof(Surplus_capacity));
            }
        }

        private string _soh;
        /// <summary>
        /// 参数标定0x1025XXE0—SOH
        /// </summary>
        public string soh
        {
            get { return _soh; }
            set
            {
                _soh = value;
                OnPropertyChanged(nameof(soh));
            }
        }
        /// <summary>
        /// 时
        /// </summary>
        public ObservableCollection<string> HourList { get; } = new ObservableCollection<string>(Enumerable.Range(0, 24).Select(x => x.ToString("D2")));

        /// <summary>
        /// 分
        /// </summary>
        public ObservableCollection<string> MinuteList { get; } = new ObservableCollection<string>(Enumerable.Range(0, 60).Select(x => x.ToString("D2")));

        /// <summary>
        /// 秒
        /// </summary>
        public ObservableCollection<string> SecondList { get; } = new ObservableCollection<string>(Enumerable.Range(0, 60).Select(x => x.ToString("D2")));

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged(); }
        }

        private string _selectedHour;
        public string SelectedHour
        {
            get { return _selectedHour; }
            set { _selectedHour = value; OnPropertyChanged(); }
        }

        private string _selectedMinute;
        public string SelectedMinute
        {
            get { return _selectedMinute; }
            set { _selectedMinute = value; OnPropertyChanged(); }
        }

        private string _selectedSecond;
        public string SelectedSecond
        {
            get { return _selectedSecond; }
            set { _selectedSecond = value; OnPropertyChanged(); }
        }

        private string _PACK_SN;
        /// <summary>
        /// PACK条形码
        /// </summary>
        public string PACK_SN
        {
            get { return _PACK_SN; }
            set { _PACK_SN = value; OnPropertyChanged(nameof(PACK_SN)); }
        }

        private string _BOARD_SN;
        /// <summary>
        /// BOARD条形码
        /// </summary>
        public string BOARD_SN
        {
            get { return _BOARD_SN; }
            set { _BOARD_SN = value; OnPropertyChanged(nameof(BOARD_SN)); }
        }

        // 电芯品牌列表
        public ObservableCollection<string> Cell_brandList { get; } = new ObservableCollection<string>
        {
            "1：宁德时代",
            "2：亿纬锂能",
            "3：其他"
        };

        private string _selectedCell_brand;
        /// <summary>
        /// 电芯品牌
        /// 1：宁德时代
        /// 2：亿纬锂能    
        /// 3：其他
        /// </summary>
        public string SelectedCell_brand
        {
            get { return _selectedCell_brand; }
            set
            {
                _selectedCell_brand = value;
                OnPropertyChanged(nameof(SelectedCell_brand));
            }
        }

        //电芯型号编号列表
        public ObservableCollection<int> Cell_model_numList { get; } = new ObservableCollection<int>(Enumerable.Range(1, 255));
        private int _SelectedCell_model_num;
        /// <summary>
        /// 电芯型号编号 1-255     
        /// </summary>
        public int SelectedCell_model_num
        {
            get { return _SelectedCell_model_num; }
            set
            {
                _SelectedCell_model_num = value;
                OnPropertyChanged(nameof(SelectedCell_model_num));
            }
        }

        // 清除FLASH标志功能码列表
        public ObservableCollection<string> Clr_FLASH_flag_funcList { get; } = new ObservableCollection<string>
        {
            "0：无",
            "1：清除重置真实容量标志"
        };

        private string _selectedClr_FLASH_flag_func;
        /// <summary>
        /// 清除FLASH标志功能码
        /// 0：无
        /// 1：清除重置真实容量标志    
        /// </summary>
        public string SelectedClr_FLASH_flag_func
        {
            get { return _selectedClr_FLASH_flag_func; }
            set
            {
                _selectedClr_FLASH_flag_func = value;
                OnPropertyChanged(nameof(SelectedClr_FLASH_flag_func));
            }
        }

        // 查询FLASH标志功能码列表
        public ObservableCollection<string> Query_FLASH_flag_funcList { get; } = new ObservableCollection<string>
        {
            "0：无",
            "1：查询重置真实容量标志"
        };

        private string _selectedQuery_FLASH_flag_func;
        /// <summary>
        /// 清除FLASH标志功能码
        /// 0：无
        /// 1：查询重置真实容量标志
        /// </summary>
        public string SelectedQuery_FLASH_flag_func
        {
            get { return _selectedQuery_FLASH_flag_func; }
            set
            {
                _selectedQuery_FLASH_flag_func = value;
                OnPropertyChanged(nameof(SelectedQuery_FLASH_flag_func));
            }
        }

        private string _Cali_items_1;
        /// <summary>
        /// 校准总压(0-1000V)
        /// </summary>
        public string Cali_items_1
        {
            get { return _Cali_items_1; }
            set
            {
                _Cali_items_1 = value;
                OnPropertyChanged(nameof(Cali_items_1));
            }
        }

        private string _Cali_items_2;
        /// <summary>
        /// 校准负载电压(0-1000V)
        /// </summary>
        public string Cali_items_2
        {
            get { return _Cali_items_2; }
            set
            {
                _Cali_items_2 = value;
                OnPropertyChanged(nameof(Cali_items_2));
            }
        }
        private string _Cali_items_3;
        /// <summary>
        /// 校准充电电流(0-50A)
        /// </summary>
        public string Cali_items_3
        {
            get { return _Cali_items_3; }
            set
            {
                _Cali_items_3 = value;
                OnPropertyChanged(nameof(Cali_items_3));
            }
        }

        private string _Cali_items_4;
        /// <summary>
        /// 校准充电小电流(0-5A)
        /// </summary>
        public string Cali_items_4
        {
            get { return _Cali_items_4; }
            set
            {
                _Cali_items_4 = value;
                OnPropertyChanged(nameof(Cali_items_4));
            }
        }

        private string _Cali_items_5;
        /// <summary>
        /// 校准放电电流(0-50A)
        /// </summary>
        public string Cali_items_5
        {
            get { return _Cali_items_5; }
            set
            {
                _Cali_items_5 = value;
                OnPropertyChanged(nameof(Cali_items_5));
            }
        }

        private string _Cali_items_6;
        /// <summary>
        /// 校准放电小电流(0-5A)
        /// </summary>
        public string Cali_items_6
        {
            get { return _Cali_items_6; }
            set
            {
                _Cali_items_6 = value;
                OnPropertyChanged(nameof(Cali_items_6));
            }
        }

        public ObservableCollection<int> Request7List { get; } = new ObservableCollection<int>(Enumerable.Range(1, 255));
        private int _selectedRequest7 = 1;
        /// <summary>
        /// 地址设置
        /// </summary>
        public int SelectedRequest7
        {
            get { return _selectedRequest7; }
            set
            {
                _selectedRequest7 = value;
                AlarmMessageDataList.Clear();
                FaultInfo.FaultInfos1.ForEach(x => x.State = 0);
                FaultInfo.FaultInfos2.ForEach(x => x.State = 0);
                FaultInfo.FaultInfos3.ForEach(x => x.State = 0);

                ClearBatteryList(0);

                OnPropertyChanged(nameof(SelectedRequest7));
            }
        }

        // 强制休眠列表
        public ObservableCollection<string> Force_sleepList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：正常",
            "2：休眠"
        };

        private string _SelectedForce_sleep;
        /// <summary>
        /// BMS功能开关0x102BXXE0—强制休眠
        /// 0：解除控制
        /// 1：正常
        /// 2：休眠
        /// </summary>
        public string SelectedForce_sleep
        {
            get { return _SelectedForce_sleep; }
            set
            {
                _SelectedForce_sleep = value;
                OnPropertyChanged(nameof(SelectedForce_sleep));
            }
        }

        // 告警保护蜂鸣器功能列表
        public ObservableCollection<string> Alarm_protection_beepList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：开启"
        };

        private string _SelectedAlarm_protection_beep;
        /// <summary>
        /// BMS功能开关0x102BXXE0—告警保护蜂鸣器功能
        /// 0：解除控制
        /// 1：关闭
        /// 2：开启
        /// </summary>
        public string SelectedAlarm_protection_beep
        {
            get { return _SelectedAlarm_protection_beep; }
            set
            {
                _SelectedAlarm_protection_beep = value;
                OnPropertyChanged(nameof(SelectedAlarm_protection_beep));
            }
        }

        // 告警保护指示灯功能列表
        public ObservableCollection<string> Alarm_protection_ledList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：开启"
        };

        private string _SelectedAlarm_protection_led;
        /// <summary>
        /// BMS功能开关0x102BXXE0—告警保护指示灯功能
        /// 0：解除控制
        /// 1：关闭
        /// 2：开启
        /// </summary>
        public string SelectedAlarm_protection_led
        {
            get { return _SelectedAlarm_protection_led; }
            set
            {
                _SelectedAlarm_protection_led = value;
                OnPropertyChanged(nameof(SelectedAlarm_protection_led));
            }
        }

        // 加热功能列表
        public ObservableCollection<string> HeatList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：开启"
        };

        private string _SelectedHeat;
        /// <summary>
        /// BMS功能开关0x102BXXE0—加热功能
        /// 0：解除控制
        /// 1：关闭
        /// 2：开启
        /// </summary>
        public string SelectedHeat
        {
            get { return _SelectedHeat; }
            set
            {
                _SelectedHeat = value;
                OnPropertyChanged(nameof(SelectedHeat));
            }
        }

        // 清除系统锁列表
        public ObservableCollection<string> Clr_system_lockList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：开启"
        };

        private string _SelectedClr_system_lock;
        /// <summary>
        /// BMS功能开关0x102BXXE0—清除系统锁
        /// 0：解除控制
        /// 1：关闭
        /// 2：开启
        /// </summary>
        public string SelectedClr_system_lock
        {
            get { return _SelectedClr_system_lock; }
            set
            {
                _SelectedClr_system_lock = value;
                OnPropertyChanged(nameof(SelectedClr_system_lock));
            }
        }

        // 强制控制开关列表
        public ObservableCollection<string> Force_ctrl_switchList { get; } = new ObservableCollection<string>
        {
            "0x00：查询控制状态",
            "0xAA：强制控制"
        };

        private string _SelectedForce_ctrl_switch;
        /// <summary>
        /// BMS功能开关0x102BXXE0—强制控制开关
        /// 0x00：查询控制状态
        /// 0xAA：强制控制
        /// </summary>
        public string SelectedForce_ctrl_switch
        {
            get { return _SelectedForce_ctrl_switch; }
            set
            {
                _SelectedForce_ctrl_switch = value;
                OnPropertyChanged(nameof(SelectedForce_ctrl_switch));
            }
        }

        // 充电Mos控制列表
        public ObservableCollection<string> chg_mos_ctrlList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：断开(低)",
            "2：闭合(高)"
        };

        private string _Selectedchg_mos_ctrl;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—充电Mos控制
        /// 0：解除控制
        /// 1：断开(低)
        /// 2：闭合(高)
        /// </summary>
        public string Selectedchg_mos_ctrl
        {
            get { return _Selectedchg_mos_ctrl; }
            set
            {
                _Selectedchg_mos_ctrl = value;
                OnPropertyChanged(nameof(Selectedchg_mos_ctrl));
            }
        }

        // 放电Mos控制列表
        public ObservableCollection<string> dsg_mos_ctrlList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：断开(高)",
            "2：闭合(低)"
        };

        private string _Selecteddsg_mos_ctrl;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—放电Mos控制
        /// 0：解除控制
        /// 1：断开(高)
        /// 2：闭合(低)
        /// </summary>
        public string Selecteddsg_mos_ctrl
        {
            get { return _Selecteddsg_mos_ctrl; }
            set
            {
                _Selecteddsg_mos_ctrl = value;
                OnPropertyChanged(nameof(Selecteddsg_mos_ctrl));
            }
        }

        // 预充Mos控制列表
        public ObservableCollection<string> pre_chg_mos_ctrlList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：断开(低)",
            "2：闭合(高)"
        };

        private string _Selectedpre_chg_mos_ctrl;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—预充Mos控制
        /// 0：解除控制
        /// 1：断开(低)
        /// 2：闭合(高)
        /// </summary>
        public string Selectedpre_chg_mos_ctrl
        {
            get { return _Selectedpre_chg_mos_ctrl; }
            set
            {
                _Selectedpre_chg_mos_ctrl = value;
                OnPropertyChanged(nameof(Selectedpre_chg_mos_ctrl));
            }
        }

        // 加热膜控制列表
        public ObservableCollection<string> heating_film_ctrlList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：断开",
            "2：闭合"
        };

        private string _Selectedheating_film_ctrl;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—加热膜控制
        /// 0：解除控制
        /// 1：断开
        /// 2：闭合
        /// </summary>
        public string Selectedheating_film_ctrl
        {
            get { return _Selectedheating_film_ctrl; }
            set
            {
                _Selectedheating_film_ctrl = value;
                OnPropertyChanged(nameof(Selectedheating_film_ctrl));
            }
        }

        // 强制休眠列表
        public ObservableCollection<string> force_sleepList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：正常",
            "2：休眠"
        };

        private string _Selectedforce_sleep;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—强制休眠
        /// 0：解除控制
        /// 1：正常
        /// 2：休眠
        /// </summary>
        public string Selectedforce_sleep
        {
            get { return _Selectedforce_sleep; }
            set
            {
                _Selectedforce_sleep = value;
                OnPropertyChanged(nameof(Selectedforce_sleep));
            }
        }

        // 蜂鸣器开启列表
        public ObservableCollection<string> beep_openList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：开启"
        };

        private string _Selectedbeep_open;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—蜂鸣器开启
        /// 0：解除控制
        /// 1：关闭
        /// 2：开启
        /// </summary>
        public string Selectedbeep_open
        {
            get { return _Selectedbeep_open; }
            set
            {
                _Selectedbeep_open = value;
                OnPropertyChanged(nameof(Selectedbeep_open));
            }
        }

        // 充电急停控制列表
        public ObservableCollection<string> chg_stop_ctrlList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：急停(高)",
            "2：正常(低)"
        };

        private string _Selectedchg_stop_ctrl;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—充电急停控制
        /// 0：解除控制
        /// 1：急停(高)
        /// 2：正常(低)
        /// </summary>
        public string Selectedchg_stop_ctrl
        {
            get { return _Selectedchg_stop_ctrl; }
            set
            {
                _Selectedchg_stop_ctrl = value;
                OnPropertyChanged(nameof(Selectedchg_stop_ctrl));
            }
        }

        // 加热继电器IOS状态列表
        public ObservableCollection<string> heating_relay_drive_I0SList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：正常"
        };

        private string _Selectedheating_relay_drive_I0S;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—加热继电器IOS状态
        /// 0：解除控制
        /// 1：关闭
        /// 2：正常
        /// </summary>
        public string Selectedheating_relay_drive_I0S
        {
            get { return _Selectedheating_relay_drive_I0S; }
            set
            {
                _Selectedheating_relay_drive_I0S = value;
                OnPropertyChanged(nameof(Selectedheating_relay_drive_I0S));
            }
        }

        // ID_OP控制列表
        public ObservableCollection<string> ID_OP_ctrlList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：无效(高)",
            "2：有效(低)"
        };

        private string _SelectedID_OP_ctrl;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—ID_OP控制
        /// 0：解除控制
        /// 1：无效(高)
        /// 2：有效(低)
        /// </summary>
        public string SelectedID_OP_ctrl
        {
            get { return _SelectedID_OP_ctrl; }
            set
            {
                _SelectedID_OP_ctrl = value;
                OnPropertyChanged(nameof(SelectedID_OP_ctrl));
            }
        }

        // ID_IN状态列表
        public ObservableCollection<string> ID_IN_stateList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：无效(高)",
            "2：有效(低)"
        };

        private string _SelectedID_IN_state;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—ID_IN状态
        /// 0：解除控制
        /// 1：无效(高)
        /// 2：有效(低)
        /// </summary>
        public string SelectedID_IN_state
        {
            get { return _SelectedID_IN_state; }
            set
            {
                _SelectedID_IN_state = value;
                OnPropertyChanged(nameof(SelectedID_IN_state));
            }
        }

        // IDM_OP控制列表
        public ObservableCollection<string> IDM_OP_ctrlList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：无效(高)",
            "2：有效(低)"
        };

        private string _SelectedIDM_OP_ctrl;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—IDM_OP控制
        /// 0：解除控制
        /// 1：无效(高)
        /// 2：有效(低)
        /// </summary>
        public string SelectedIDM_OP_ctrl
        {
            get { return _SelectedIDM_OP_ctrl; }
            set
            {
                _SelectedIDM_OP_ctrl = value;
                OnPropertyChanged(nameof(SelectedIDM_OP_ctrl));
            }
        }

        // IDM_IN状态列表
        public ObservableCollection<string> IDM_IN_stateList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：无效(高)",
            "2：有效(低)"
        };

        private string _SelectedIDM_IN_state;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—IDM_IN状态
        /// 0：解除控制
        /// 1：无效(高)
        /// 2：有效(低)
        /// </summary>
        public string SelectedIDM_IN_state
        {
            get { return _SelectedIDM_IN_state; }
            set
            {
                _SelectedIDM_IN_state = value;
                OnPropertyChanged(nameof(SelectedIDM_IN_state));
            }
        }

        private string _force_bal_ctrl_1_8;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—强制均衡控制1-8
        /// 开启ATE测试指令和强制控制开关时生效
        /// </summary>
        public string force_bal_ctrl_1_8
        {
            get { return _force_bal_ctrl_1_8; }
            set
            {
                _force_bal_ctrl_1_8 = value;
                OnPropertyChanged(nameof(force_bal_ctrl_1_8));
            }
        }

        private string _force_bal_ctrl_9_16;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—强制均衡控制9-16
        /// 开启ATE测试指令和强制控制开关时生效
        /// </summary>
        public string force_bal_ctrl_9_16
        {
            get { return _force_bal_ctrl_9_16; }
            set
            {
                _force_bal_ctrl_9_16 = value;
                OnPropertyChanged(nameof(force_bal_ctrl_9_16));
            }
        }

        // 强制控制开关列表
        public ObservableCollection<string> force_ctrl_switchList { get; } = new ObservableCollection<string>
        {
            "0x00：查询控制状态",
            "0xAA：强制控制"
        };

        private string _Selectedforce_ctrl_switch;
        /// <summary>
        /// ATE强制控制指令0x102FXXE0—强制控制开关
        /// 0x00：查询控制状态
        /// 0xAA：强制控制
        /// </summary>
        public string Selectedforce_ctrl_switch
        {
            get { return _Selectedforce_ctrl_switch; }
            set
            {
                _Selectedforce_ctrl_switch = value;
                OnPropertyChanged(nameof(Selectedforce_ctrl_switch));
            }
        }

        // llc强制放电列表
        public ObservableCollection<string> llc_force_dsgList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：启动"
        };

        private string _Selectedllc_force_dsg;
        /// <summary>
        /// ATE强制控制指令0x101EXXE0—llc强制放电
        /// 0：解除控制
        /// 1：关闭
        /// 2：启动
        /// </summary>
        public string Selectedllc_force_dsg
        {
            get { return _Selectedllc_force_dsg; }
            set
            {
                _Selectedllc_force_dsg = value;
                OnPropertyChanged(nameof(Selectedllc_force_dsg));
            }
        }

        // llc强制充电列表
        public ObservableCollection<string> llc_force_chgList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：启动"
        };

        private string _Selectedllc_force_chg;
        /// <summary>
        /// ATE强制控制指令0x101EXXE0—llc强制充电
        /// 0：解除控制
        /// 1：关闭
        /// 2：启动
        /// </summary>
        public string Selectedllc_force_chg
        {
            get { return _Selectedllc_force_chg; }
            set
            {
                _Selectedllc_force_chg = value;
                OnPropertyChanged(nameof(Selectedllc_force_chg));
            }
        }

        // LED灯控制列表
        public ObservableCollection<string> LED_ctrlList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：恢复",
            "2：闪烁"
        };

        private string _SelectedLED_ctrl;
        /// <summary>
        /// ATE强制控制指令0x101EXXE0—LED灯控制
        /// 0：解除控制
        /// 1：恢复
        /// 2：闪烁
        /// </summary>
        public string SelectedLED_ctrl
        {
            get { return _SelectedLED_ctrl; }
            set
            {
                _SelectedLED_ctrl = value;
                OnPropertyChanged(nameof(SelectedLED_ctrl));
            }
        }

        // 硬件看门狗控制列表
        public ObservableCollection<string> hw_wdg_ctrlList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：正常"
        };

        private string _Selectedhw_wdg_ctrl;
        /// <summary>
        /// ATE强制控制指令0x101EXXE0—硬件看门狗控制
        /// 0：解除控制
        /// 1：关闭
        /// 2：正常
        /// </summary>
        public string Selectedhw_wdg_ctrl
        {
            get { return _Selectedhw_wdg_ctrl; }
            set
            {
                _Selectedhw_wdg_ctrl = value;
                OnPropertyChanged(nameof(Selectedhw_wdg_ctrl));
            }
        }

        // 强制控制开关列表
        public ObservableCollection<string> ForceCtrlSwitchList { get; } = new ObservableCollection<string>
        {
            "0：解除控制",
            "1：关闭",
            "2：正常"
        };

        private string _SelectedForceCtrlSwitch;
        /// <summary>
        /// ATE强制控制指令0x101EXXE0—强制控制开关
        /// 0xAA：强制控制
        /// 其他：查询控制状态
        /// </summary>
        public string SelectedForceCtrlSwitch
        {
            get { return _SelectedForceCtrlSwitch; }
            set
            {
                _SelectedForceCtrlSwitch = value;
                OnPropertyChanged(nameof(SelectedForceCtrlSwitch));
            }
        }


        // 被动均衡状态设置-主从控制标志列表
        public ObservableCollection<string> MasterSlaveControlFlagList { get; } = new ObservableCollection<string>
        {
            "0：默认自控制",
            "1：主从控制"
        };

        private string _selectedMasterSlaveControlFlag = "0：默认自控制";
        /// <summary>
        /// 被动均衡状态设置-主从控制标志 
        /// 0：默认自控制
        /// 1：主从控制
        /// </summary>
        public string SelectedMasterSlaveControlFlag
        {
            get { return _selectedMasterSlaveControlFlag; }
            set
            {
                _selectedMasterSlaveControlFlag = value;
                OnPropertyChanged(nameof(SelectedMasterSlaveControlFlag));
            }
        }

        public ObservableCollection<int> PackageNumberList { get; } = new ObservableCollection<int>(Enumerable.Range(1, 60));
        private int _selectedPackageNumber = 1;
        /// <summary>
        /// 被动均衡状态设置-包序号N
        /// </summary>
        public int SelectedPackageNumber
        {
            get { return _selectedPackageNumber; }
            set
            {
                if (_selectedPackageNumber != value)
                {
                    _selectedPackageNumber = value;
                    OnPropertyChanged(nameof(SelectedPackageNumber));
                    PackageNumber_SelectedIndexChanged(value); // 处理选项变化
                }
            }
        }

        private string _PassiveEquilibriumState = "电芯1~48被动均衡状态";

        /// <summary>
        /// 电芯1~64被动均衡状态
        /// </summary>
        public string PassiveEquilibriumState
        {
            get { return _PassiveEquilibriumState; }
            set
            {
                _PassiveEquilibriumState = value;
                OnPropertyChanged(nameof(PassiveEquilibriumState));
            }
        }


        private ObservableCollection<IBatteryData> _batteryData_BMU_List;
        // BMU电池数据
        public ObservableCollection<IBatteryData> BatteryData_BMU_List
        {
            get => _batteryData_BMU_List;
            set
            {
                _batteryData_BMU_List = value;
                OnPropertyChanged(nameof(BatteryData_BMU_List)); // 通知界面更新
            }
        }

        private ObservableCollection<batteryVoltageData> batteryVoltageDataList;
        private ObservableCollection<batterySocData> batterySocDataList;
        private ObservableCollection<batterySohData> batterySohDataList;
        private ObservableCollection<batteryTemperatureData> batteryTemperatureDataList;
        private ObservableCollection<batteryEquilibriumStateData> batteryEquilibriumStateDataList;
        private ObservableCollection<batteryEquilibriumTemperatureData> batteryEquilibriumTemperatureDataList;

        private bool _isBatteryVoltageChecked;
        /// <summary>
        /// 电池电压
        /// </summary>
        public bool IsChecked_BatteryVoltage
        {
            get => _isBatteryVoltageChecked;
            set
            {
                SetProperty(ref _isBatteryVoltageChecked, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池电压
                    batteryVoltageTimer = new System.Threading.Timer(TimerCallBack_batteryVoltage, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    //    BatteryDataList = new ObservableCollection<IBatteryData>(UpdateDataGrid(batteryVoltageDataList.ToList(), new[] { "节号", "电压" }, data => data.SectionNumber, data => data.Voltage));
                    //});
                }
                else
                {
                    batteryVoltageTimer.Dispose();
                }
            }
        }

        private bool _isBatterySOCChecked;

        /// <summary>
        /// 电池SOC
        /// </summary>
        public bool IsChecked_BatterySOC
        {
            get => _isBatterySOCChecked;
            set
            {
                SetProperty(ref _isBatterySOCChecked, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池SOC
                    batterySocTimer = new System.Threading.Timer(TimerCallBack_batterySoc, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    //    BatteryDataList = new ObservableCollection<IBatteryData>(UpdateDataGrid(batterySocDataList.ToList(), new[] { "节号", "SOC" }, data => data.SectionNumber, data => data.SOC));
                    //});
                }
                else
                {
                    batterySocTimer.Dispose();
                }

            }
        }

        private bool _isBatterySOHChecked;
        /// <summary>
        /// 电池SOH
        /// </summary>
        public bool IsChecked_BatterySOH
        {
            get => _isBatterySOHChecked;
            set
            {
                SetProperty(ref _isBatterySOHChecked, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池SOH
                    batterySohTimer = new System.Threading.Timer(TimerCallBack_batterySoh, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    //    BatteryDataList = new ObservableCollection<IBatteryData>(UpdateDataGrid(batterySohDataList.ToList(), new[] { "节号", "SOH" }, data => data.SectionNumber, data => data.SOH));
                    //});
                }
                else
                {
                    batterySohTimer.Dispose();
                }
            }
        }

        private bool _isBatteryTemperatureChecked;
        /// <summary>
        /// 电池温度
        /// </summary>
        public bool IsChecked_BatteryTemperature
        {
            get => _isBatteryTemperatureChecked;
            set
            {
                SetProperty(ref _isBatteryTemperatureChecked, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池温度
                    batteryTemperatureTimer = new System.Threading.Timer(TimerCallBack_batteryTemperature, 0, 1000, Convert.ToInt32(DataCollectionInterval));
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    //    BatteryDataList = new ObservableCollection<IBatteryData>(UpdateDataGrid(batteryTemperatureDataList.ToList(), new[] { "序号", "温度" }, data => data.CellNumber, data => data.Temperature));
                    //});
                }
                else
                {
                    batteryTemperatureTimer.Dispose();
                }
            }
        }

        private bool _isChecked_BatteryEquilibriumState;
        /// <summary>
        /// 电池均衡状态
        /// </summary>
        public bool IsChecked_BatteryEquilibriumState
        {
            get => _isChecked_BatteryEquilibriumState;
            set
            {
                SetProperty(ref _isChecked_BatteryEquilibriumState, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池均衡状态
                    equilibriumStateTimer = new System.Threading.Timer(TimerCallBack_BatteryEquilibriumState, 0, 1000, Convert.ToInt32(DataCollectionInterval));


                }
                else
                {
                    equilibriumStateTimer.Dispose();
                }
            }
        }

        private bool _isChecked_BatteryEquilibriumTemperature;
        /// <summary>
        /// 电池均衡温度
        /// </summary>
        public bool IsChecked_BatteryEquilibriumTemperature
        {
            get => _isChecked_BatteryEquilibriumTemperature;
            set
            {
                SetProperty(ref _isChecked_BatteryEquilibriumTemperature, value);
                if (value)
                {
                    // 更新 DataGrid 数据为电池均衡温度
                    equilibriumTemperatureTimer = new System.Threading.Timer(TimerCallBack_BatteryEquilibriumTemperature, 0, 1000, Convert.ToInt32(DataCollectionInterval));


                }
                else
                {
                    equilibriumTemperatureTimer.Dispose();
                }
            }
        }

        public CancellationTokenSource cts = null;

        RealtimeData_BMS1500V_BMU model = new RealtimeData_BMS1500V_BMU();

        string[] packSN = new string[3];
        string[] bcuCode = new string[3];
        string[] boardCode = new string[3];
        private Timer timer = null;
        private Timer DataSavingTimer = null;
        private Timer batteryVoltageTimer = null;
        private Timer batteryTemperatureTimer = null;
        private Timer batterySocTimer = null;
        private Timer batterySohTimer = null;
        private Timer equilibriumStateTimer = null;
        private Timer equilibriumTemperatureTimer = null;
        BaseCanHelper baseCanHelper = null;
        public BMU_Control_ViewModel()
        {
            baseCanHelper = new CommandOperation(BMSConfig.ConfigManager).baseCanHelper;


            cts = new CancellationTokenSource();
            if (batteryVoltageDataList == null) batteryVoltageDataList = new ObservableCollection<RealtimeData_BMS1500V_BMU.batteryVoltageData>();
            if (batteryTemperatureDataList == null) batteryTemperatureDataList = new ObservableCollection<RealtimeData_BMS1500V_BMU.batteryTemperatureData>();
            if (batterySocDataList == null) batterySocDataList = new ObservableCollection<RealtimeData_BMS1500V_BMU.batterySocData>();
            if (batterySohDataList == null) batterySohDataList = new ObservableCollection<RealtimeData_BMS1500V_BMU.batterySohData>();
            if (batteryEquilibriumStateDataList == null) batteryEquilibriumStateDataList = new ObservableCollection<RealtimeData_BMS1500V_BMU.batteryEquilibriumStateData>();
            if (batteryEquilibriumTemperatureDataList == null) batteryEquilibriumTemperatureDataList = new ObservableCollection<RealtimeData_BMS1500V_BMU.batteryEquilibriumTemperatureData>();
            if (AlarmMessageDataList == null) AlarmMessageDataList = new ObservableCollection<RealtimeData_BMS1500V_BMU.AlarmMessageData>();

            // 初始化更新电池电压数据
            IsChecked_BatteryVoltage = true;

            //添加电池电压编号
            for (int i = 1; i <= ConstantDef.PageMaxBatteryCellNumber; i++)
            {
                batteryVoltageDataList.Add(new RealtimeData_BMS1500V_BMU.batteryVoltageData
                {
                    SectionNumber = i.ToString() + "#"
                });
            }

            // 添加SOC编号
            for (int i = 1; i <= ConstantDef.PageMaxBatteryCellNumber; i++)
            {
                batterySocDataList.Add(new RealtimeData_BMS1500V_BMU.batterySocData
                {
                    SectionNumber = i.ToString() + "#"
                });
            }

            //添加SOH编号
            for (int i = 1; i <= ConstantDef.PageMaxBatteryCellNumber; i++)
            {
                batterySohDataList.Add(new RealtimeData_BMS1500V_BMU.batterySohData
                {
                    SectionNumber = i.ToString() + "#"
                });
            }

            //添加温度编号
            for (int i = 1; i <= ConstantDef.PageMaxBatteryCellNumber; i++)
            {
                batteryTemperatureDataList.Add(new RealtimeData_BMS1500V_BMU.batteryTemperatureData
                {
                    CellNumber = i.ToString() + "#"
                });
            }

            //添加均衡状态编号
            for (int i = 1; i <= ConstantDef.PageMaxBatteryCellNumber; i++)
            {
                batteryEquilibriumStateDataList.Add(new RealtimeData_BMS1500V_BMU.batteryEquilibriumStateData
                {
                    CellNumber = i.ToString() + "#",
                });
            }

            //添加均衡温度编号
            for (int i = 1; i <= ConstantDef.PageMaxBatteryCellNumber; i++)
            {
                batteryEquilibriumTemperatureDataList.Add(new RealtimeData_BMS1500V_BMU.batteryEquilibriumTemperatureData
                {
                    CellNumber = i.ToString() + "#",
                });
            }
            ChangeEquilibriumList(48, 1);


        }
        private void ChangeEquilibriumList(int batterySeries, int packageNumber)
        {
            int offset = 48 * (packageNumber - 1);
            if (PassiveEquilibriumCheckBoxItems == null)
            {
                PassiveEquilibriumCheckBoxItems = new ObservableCollection<CheckBoxItem>();
            }
            else
            {
                PassiveEquilibriumCheckBoxItems.Clear();
            }



            // 一帧只能发送48个bit
            for (int i = 0; i < 48; i++)
            {
                PassiveEquilibriumCheckBoxItems.Add(new CheckBoxItem
                {
                    Label = $"电芯 {(i + 1) + offset}",
                    IsChecked = false // 默认未选中
                });

            }

        }

        public ObservableCollection<CheckBoxItem> PassiveEquilibriumCheckBoxItems { get; set; }
        public void UpdateTime()
        {
            DateTime now = DateTime.Now;
            SelectedDate = now.Date;
            SelectedHour = now.Hour.ToString("D2");
            SelectedMinute = now.Minute.ToString("D2");
            SelectedSecond = now.Second.ToString("D2");
        }

        /// <summary>
        /// 更新电池电压数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_batteryVoltage(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryData_BMU_List = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(batteryVoltageDataList.ToList(),
                    new[] { "节号", "电压" },
                    data => data.SectionNumber,
                    data => data.Voltage,
                    ConstantDef.PageMaxBatteryCellNumber));
            });
        }

        /// <summary>
        /// 更新电池温度数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_batteryTemperature(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryData_BMU_List = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(batteryTemperatureDataList.ToList(),
                    new[] { "序号", "温度" },
                    data => data.CellNumber,
                    data => data.Temperature,
                    ConstantDef.PageMaxBatteryCellNumber));
            });
        }

        /// <summary>
        /// 更新电池SOC数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_batterySoc(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryData_BMU_List = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(batterySocDataList.ToList(),
                    new[] { "节号", "SOC" },
                    data => data.SectionNumber,
                    data => data.SOC,
                    ConstantDef.PageMaxBatteryCellNumber));
            });
        }

        /// <summary>
        /// 更新电池SOH数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_batterySoh(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryData_BMU_List = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(batterySohDataList.ToList(),
                    new[] { "节号", "SOH" },
                    data => data.SectionNumber,
                    data => data.SOH,
                    ConstantDef.PageMaxBatteryCellNumber));
            });
        }

        /// <summary>
        /// 更新电池均衡状态数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_BatteryEquilibriumState(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryData_BMU_List = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(batteryEquilibriumStateDataList.ToList(),
                    new[] { "序号", "均衡状态" },
                    data => data.CellNumber,
                    data => data.BatteryEquilibriumState,
                    ConstantDef.PageMaxBatteryCellNumber));
            });
        }

        /// <summary>
        /// 更新电池均衡温度数据
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_BatteryEquilibriumTemperature(object obj)
        {
            Application.Current.Dispatcher.BeginInvoke(() =>
            {
                BatteryData_BMU_List = new ObservableCollection<IBatteryData>(
                    UpdateDataGrid(batteryEquilibriumTemperatureDataList.ToList(),
                    new[] { "序号", "均衡温度" },
                    data => data.CellNumber,
                    data => data.BatteryEquilibriumTemperature,
                    ConstantDef.PageMaxBatteryCellNumber));
            });
        }

        /// <summary>
        /// 更新数据表网格数据(泛型方法)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="headers"></param>
        /// <param name="getPrimaryValue"></param>
        /// <param name="getSecondaryValue"></param>
        /// <param name="maxSectionsPerGroup"></param>
        /// <returns></returns>
        //private List<T> UpdateDataGrid<T>(List<T> dataList, string[] headers, Func<T, string> getPrimaryValue, Func<T, string> getSecondaryValue, int maxSectionsPerGroup = 48) where T : IBatteryData, new()
        //{
        //    var tempDataList = new List<T>(dataList);
        //    dataList.Clear();
        //    if (tempDataList.Count == 0)
        //    {
        //        return dataList;
        //    }
        //    int emptyRowsCount = 8 - (maxSectionsPerGroup / 16) * 2;

        //    for (int i = 0; i < 480; i += 8)
        //    {
        //        int bmmIndex = (i / 8) + 1;
        //        string dataName = $"BMM{bmmIndex}";

        //        for (int j = 0; j < 8 - emptyRowsCount; j++)
        //        {
        //            var dataRow = new T
        //            {
        //                DataName = dataName,
        //                IndexName = (j % 2 == 0) ? headers[0] : headers[1]
        //            };

        //            int voltageIndex = (j / 2) * 16 + (8 - emptyRowsCount) * i;

        //            if (voltageIndex < tempDataList.Count - 15)
        //            {
        //                if (maxSectionsPerGroup > 0 && maxSectionsPerGroup < 16)
        //                {
        //                    for (int k = 0; k < maxSectionsPerGroup; k++)
        //                    {
        //                        string value = (j % 2 == 0)
        //                            ? getPrimaryValue(tempDataList[voltageIndex + k])
        //                            : getSecondaryValue(tempDataList[voltageIndex + k]);
        //                        dataRow.GetType().GetProperty($"Value{k + 1}").SetValue(dataRow, value);
        //                    }
        //                }
        //                else
        //                {
        //                    for (int k = 0; k < 16; k++)
        //                    {
        //                        string value = (j % 2 == 0)
        //                            ? getPrimaryValue(tempDataList[voltageIndex + k])
        //                            : getSecondaryValue(tempDataList[voltageIndex + k]);
        //                        dataRow.GetType().GetProperty($"Value{k + 1}").SetValue(dataRow, value);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (maxSectionsPerGroup > 0 && maxSectionsPerGroup < 16)
        //                {
        //                    for (int k = maxSectionsPerGroup; k < 16 - maxSectionsPerGroup; k++)
        //                    {
        //                        dataRow.GetType().GetProperty($"Value{k + 1}").SetValue(dataRow, null);
        //                    }
        //                }
        //                else
        //                {
        //                    for (int k = 0; k < 16; k++)
        //                    {
        //                        dataRow.GetType().GetProperty($"Value{k + 1}").SetValue(dataRow, null);
        //                    }
        //                }
        //            }

        //            dataList.Add(dataRow);
        //        }

        //        for (int k = 0; k < emptyRowsCount; k++)
        //        {
        //            var emptyRow = new T
        //            {
        //                DataName = dataName,
        //                IndexName = (k % 2 == 0) ? headers[0] : headers[1]
        //            };


        //            if (maxSectionsPerGroup > 0 && maxSectionsPerGroup < 16)
        //            {
        //                for (int m = maxSectionsPerGroup; m < 16 - maxSectionsPerGroup; m++)
        //                {
        //                    emptyRow.GetType().GetProperty($"Value{m + 1}").SetValue(emptyRow, null);
        //                }
        //            }
        //            else
        //            {
        //                for (int m = 0; m < 16; m++)
        //                {
        //                    emptyRow.GetType().GetProperty($"Value{m + 1}").SetValue(emptyRow, null);
        //                }
        //            }



        //            dataList.Add(emptyRow);
        //        }
        //    }

        //    return dataList;
        //}

        /// <summary>
        /// 更新数据表网格数据(泛型方法)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="headers"></param>
        /// <param name="getPrimaryValue"></param>
        /// <param name="getSecondaryValue"></param>
        /// <param name="maxSectionsPerGroup"></param>
        /// <returns></returns>
        private List<T> UpdateDataGrid<T>(List<T> dataList, string[] headers, Func<T, string> getPrimaryValue, Func<T, string> getSecondaryValue, int maxSectionsPerGroup = 64) where T : IBatteryData, new()
        {

            var tempDataList = new List<T>(dataList);
            dataList.Clear();

            if (tempDataList.Count == 0)
            {
                return dataList;
            }
            //16列
            int totalColumns = 16;
            //BMU只有8行
            int totalRows = 8;
            string[,] values = new string[totalColumns, totalRows];
            string[] dataName = new string[totalRows];

            //BMU数据只有一个,编号由BMU地址确定
            for (int i = 0; i < totalRows; i++)
            {
                dataName[i] = $"BMU{SelectedRequest7}";
            }

            for (int currentIndex = 0; currentIndex < tempDataList.Count; currentIndex++)
            {
                var data = tempDataList[currentIndex];
                int groupIndex = currentIndex / maxSectionsPerGroup;
                int startingRow = groupIndex * 8;
                int rowInGroup = (currentIndex % maxSectionsPerGroup) / totalColumns;
                int col = (currentIndex % maxSectionsPerGroup) % totalColumns;

                try
                {
                    values[col, startingRow + rowInGroup * 2] = getPrimaryValue(data);
                    values[col, startingRow + rowInGroup * 2 + 1] = getSecondaryValue(data);
                }
                catch (Exception ex)
                {


                }



            }

            for (int i = 0; i < totalRows; i++)
            {
                dataList.Add(new T
                {
                    DataName = dataName[i],
                    IndexName = (i % 2 == 0) ? headers[0] : headers[1],
                    Value1 = values[0, i],
                    Value2 = values[1, i],
                    Value3 = values[2, i],
                    Value4 = values[3, i],
                    Value5 = values[4, i],
                    Value6 = values[5, i],
                    Value7 = values[6, i],
                    Value8 = values[7, i],
                    Value9 = values[8, i],
                    Value10 = values[9, i],
                    Value11 = values[10, i],
                    Value12 = values[11, i],
                    Value13 = values[12, i],
                    Value14 = values[13, i],
                    Value15 = values[14, i],
                    Value16 = values[15, i]
                });
            }

            return dataList;
        }

        public ICommand RefreshAlarmMessageCmd => new RelayCommand(RefreshAlarmMessage);

        public void RefreshAlarmMessage()
        {
            //if (AlarmMessageDataList != null) AlarmMessageDataList.Clear();

            if (AlarmMessageDataList != null)
            {
                // 使用LINQ 筛选出已结束的报警信息
                var endedAlarms = AlarmMessageDataList.Where(a => a.isEnd == "是").ToList();

                // 移除所有已结束的报警信息
                foreach (var alarm in endedAlarms)
                {
                    AlarmMessageDataList.Remove(alarm);
                }
            }
        }

        public void CancelOperation()
        {
            if (cts != null)
            {
                cts.Cancel();
                cts.Dispose();
            }
        }

        public ICommand StartDataCollectionCmd => new RelayCommand(StartDataCollection);
        /// <summary>
        /// 进入界面或者手动点击启动数据采集按钮
        /// </summary>
        public void StartDataCollection()
        {
            //禁用启动数据采集按钮
            IsStartDataCollectionEnabled = false;
            //启用停止数据采集按钮
            IsStopDataCollectionEnabled = true;
            //启动数据采集定时器
            timer = new System.Threading.Timer(TimerCallBack, 0, 1000, Convert.ToInt32(DataCollectionInterval));
            //默认选择展示电池电压数据
            batteryVoltageTimer = new System.Threading.Timer(TimerCallBack_batteryVoltage, 0, 1000, Convert.ToInt32(DataCollectionInterval));
        }

        public ICommand StopDataCollectionCmd => new RelayCommand(StopDataCollection);
        /// <summary>
        /// 离开界面或者手动点击停止数据采集按钮
        /// </summary>
        public void StopDataCollection()
        {
            //启用启动数据采集按钮
            IsStartDataCollectionEnabled = true;
            //禁用停止数据采集按钮
            IsStopDataCollectionEnabled = false;

            //关闭数据采集定时器
            if (timer != null) timer.Dispose();
            //关闭数据存储定时器
            if (DataSavingTimer != null) DataSavingTimer.Dispose();
            //关闭BMS实时信息相关定时器
            if (batteryVoltageTimer != null) batteryVoltageTimer.Dispose();
            if (batteryTemperatureTimer != null) batteryTemperatureTimer.Dispose();
            if (batterySocTimer != null) batterySocTimer.Dispose();
            if (batterySohTimer != null) batterySohTimer.Dispose();
            if (equilibriumStateTimer != null) equilibriumStateTimer.Dispose();
            if (equilibriumTemperatureTimer != null) equilibriumTemperatureTimer.Dispose();
        }

        public ICommand WriteFlashDataCmd => new RelayCommand(WriteFlashData);

        /// <summary>
        /// 写入Flash数据
        /// </summary>
        public void WriteFlashData()
        {
            //FrmMain.BMU_ID = 0x01;
            byte[] can_id = new byte[4] { 0xE0, Convert.ToByte(SelectedRequest7), 0x1F, 0x10 };
            byte[] data = new byte[8];

            data[0] = 0x01;//0x00:读取Flash    0x01:写入Flash      
            byte[] data1 = Encoding.Default.GetBytes(FlashData);
            int lengthToCopy = Math.Min(data1.Length, data.Length - 1);
            Array.Copy(data1, 0, data, 1, lengthToCopy);

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand WriteRequestDataCmd => new RelayCommand(WriteRequestData);
        /// <summary>
        /// 写入请求数据——0x020:上位机控制
        /// </summary>
        public void WriteRequestData()
        {
            //FrmMain.BMU_ID = 0x01;
            byte[] can_id = new byte[4] { 0xE0, Convert.ToByte(SelectedRequest7), 0x20, 0x10 };
            byte[] data = new byte[8];

            int Request1_Index = Request1List.IndexOf(SelectedRequest1);
            int Request2_Index = Request2List.IndexOf(SelectedRequest2);
            int Request3_Index = Request3List.IndexOf(SelectedRequest3);
            int Request4_Index = Request4List.IndexOf(SelectedRequest4);
            int Request5_Index = Request5List.IndexOf(SelectedRequest5);
            int Request6_Index = Request6List.IndexOf(SelectedRequest6);

            data[0] = (byte)(GetValue(Request1_Index) & 0xff);
            data[1] = (byte)(GetValue(Request2_Index) & 0xff);
            data[2] = (byte)(GetValue(Request3_Index) & 0xff);
            data[3] = (byte)(GetValue(Request4_Index) & 0xff);
            data[4] = (byte)(GetValue(Request5_Index) & 0xff);
            data[5] = (byte)(GetValue(Request6_Index) & 0xff);

            int GetValue(int index)
            {
                switch (index)
                {
                    case 0: return 0x00;
                    case 1: return 0xAA;
                    case 2: return 0x55;
                    default: return 0x00;
                }
            }

            data[6] = Convert.ToByte(SelectedRequest7);
            byte crc8 = (byte)(0x10 + 0x20 + data[6] + 0xE0);
            for (int i = 0; i < data.Length - 1; i++)
            {
                crc8 += data[i];
            }

            data[7] = crc8;
            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);

        }

        public ICommand Write_0x21_Cmd => new RelayCommand(Write_0x21);
        /// <summary>
        /// 参数标定0x1021XXE0
        /// </summary>
        public void Write_0x21()
        {
            //byte[] can_id = new byte[] { 0xE0, 0x81, 0x21, 0x10 };
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x21, 0x10 };
            byte[] data = new byte[8];

            byte[] bal_open_volt = Uint16ToBytes(Convert.ToUInt32(float.Parse(Bal_open_volt)));
            byte[] bal_open_volt_diff = Uint16ToBytes(Convert.ToUInt32(float.Parse(Bal_open_volt_diff)));
            byte[] full_chg_volt = Uint16ToBytes(Convert.ToUInt32(float.Parse(Full_chg_volt)));

            data[0] = bal_open_volt[0];
            data[1] = bal_open_volt[1];
            data[2] = bal_open_volt_diff[0];
            data[3] = bal_open_volt_diff[1];
            data[4] = full_chg_volt[0];
            data[5] = full_chg_volt[1];
            data[6] = Convert.ToByte(Convert.ToInt32(Heating_film_open_temp) + 40);
            data[7] = Convert.ToByte(Convert.ToInt32(HeatingHeating_film_close_temp) + 40);

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand Write_0x22_Cmd => new RelayCommand(Write_0x22);
        /// <summary>
        /// 参数标定0x1022XXE0
        /// </summary>
        public void Write_0x22()
        {
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x22, 0x10 };
            //byte[] can_id = new byte[] { 0xE0, 0x81, 0x22, 0x10 };
            byte[] data = new byte[8];

            byte[] pack_stop_volt = Uint16ToBytes(Convert.ToUInt32(float.Parse(Pack_stop_volt)));
            byte[] pack_stop_cur = Uint16ToBytes(Convert.ToUInt32(float.Parse(Pack_stop_cur)));

            data[0] = pack_stop_volt[0];
            data[1] = pack_stop_volt[1];
            data[2] = pack_stop_cur[0];
            data[3] = pack_stop_cur[1];

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand Write_0x23_Cmd => new RelayCommand(Write_0x23);
        /// <summary>
        /// 参数标定0x1023XXE0
        /// </summary>
        public void Write_0x23()
        {
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x23, 0x10 };
            //byte[] can_id = new byte[] { 0xE0, 0x81, 0x23, 0x10 };
            byte[] data = new byte[8];

            byte[] rated_capacity = Uint16ToBytes(Convert.ToUInt32(float.Parse(Rated_capacity)));
            byte[] cell_Volt_Num = Uint16ToBytes(Convert.ToUInt32(float.Parse(Cell_Volt_Num)));
            byte[] cell_temp_num = Uint16ToBytes(Convert.ToUInt32(float.Parse(Cell_temp_num)));

            data[0] = rated_capacity[0];
            data[1] = rated_capacity[1];
            data[2] = cell_Volt_Num[0];
            data[3] = cell_Volt_Num[1];
            data[4] = cell_temp_num[0];
            data[5] = cell_temp_num[1];

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand Write_0x24_Cmd => new RelayCommand(Write_0x24);
        /// <summary>
        /// 参数标定0x1024XXE0
        /// </summary>
        public void Write_0x24()
        {
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x24, 0x10 };
            //byte[] can_id = new byte[] { 0xE0, 0x81, 0x24, 0x10 };

            byte[] buf1 = new byte[4];
            ConvertIntToByteArray(Convert.ToInt32(Cumulative_Chg_Capacity), ref buf1);
            byte[] buf2 = new byte[4];
            ConvertIntToByteArray(Convert.ToInt32(Cumulative_dsg_capacity), ref buf2);

            byte[] data = new byte[] { buf1[0], buf1[1], buf1[2], buf1[3], buf2[0], buf2[1], buf2[2], buf2[3] };


            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand Write_0x25_Cmd => new RelayCommand(Write_0x25);
        /// <summary>
        /// 参数标定0x1025XXE0
        /// </summary>
        public void Write_0x25()
        {
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x25, 0x10 };
            //byte[] can_id = new byte[] { 0xE0, 0x81, 0x25, 0x10 };
            byte[] data = new byte[8];

            byte[] _soc = Uint16ToBytes(Convert.ToUInt32(float.Parse(soc)));
            byte[] full_chg_capacity = Uint16ToBytes(Convert.ToUInt32(float.Parse(Full_chg_capacity)));
            byte[] surplus_capacity = Uint16ToBytes(Convert.ToUInt32(float.Parse(Surplus_capacity)));
            byte[] _soh = Uint16ToBytes(Convert.ToUInt32(float.Parse(soh)));

            data[0] = _soc[0];
            data[1] = _soc[1];
            data[2] = full_chg_capacity[0];
            data[3] = full_chg_capacity[1];
            data[4] = surplus_capacity[0];
            data[5] = surplus_capacity[1];
            data[6] = _soh[0];
            data[7] = _soh[1];

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }
        public ICommand Write_0x26_Cmd => new RelayCommand(Write_0x26);
        /// <summary>
        /// 时间标定0x1026FFE0
        /// </summary>
        public void Write_0x26()
        {
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x26, 0x10 };
            //byte[] can_id = new byte[] { 0xE0, 0x81, 0x26, 0x10 };

            var SelectedDate1 = DateTime.Now;
            DateTime SystemTime = new DateTime(SelectedDate1.Year, SelectedDate1.Month, SelectedDate1.Day,
                                 SelectedDate1.Hour, SelectedDate1.Minute, SelectedDate1.Second);
            string[] date = SystemTime.ToString("yyyy-MM-dd HH:mm:ss").Split(new char[] { ' ', '-', ':' });
            byte[] bytes = new byte[] {
                                    Convert.ToByte(Convert.ToInt32(date[0]) - 2000),
                                    Convert.ToByte(Convert.ToInt32(date[1])),
                                    Convert.ToByte(Convert.ToInt32(date[2])),
                                    Convert.ToByte(Convert.ToInt32(date[3])),
                                    Convert.ToByte(Convert.ToInt32(date[4])),
                                    Convert.ToByte(Convert.ToInt32(date[5])),
                                    0x00, 0x00 };

            if (baseCanHelper.Send(bytes, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand Write_0x27_Cmd => new RelayCommand(Write_0x27);
        /// <summary>
        ///PACK_SN标定0x1027FFE0
        /// </summary>
        public void Write_0x27()
        {
            SetSN(PACK_SN);
        }

        public ICommand Write_0x28_Cmd => new RelayCommand(Write_0x28);
        /// <summary>
        ///BOARD_SN标定0x1028FFE0
        /// </summary>
        public void Write_0x28()
        {
            SetSN(BOARD_SN, true);
        }

        /// <summary>
        /// 设置SN序列号
        /// </summary>
        /// <param name="packSn"></param>
        /// <param name="identity">false=PACK_SN设置，true=单板SN设置</param>
        public void SetSN(string packSn, bool identity = false)
        {
            bool flag_WriteSuccess = false;
            if (packSn == null)
            {
                MessageBoxHelper.Warning("输入的序列号不能为空", "提示", null, ButtonType.OK);
                return;
            }

            // 去掉前导和尾部空白字符，并只保留字母和数字
            packSn = Regex.Replace(packSn.Trim(), "[^a-zA-Z0-9]", "");

            if (packSn.Length != 21)
            {
                MessageBoxHelper.Warning("SN序列号长度不等于21！输入的序列号长度为" + packSn.Length.ToString() + "位", "提示", null, ButtonType.OK);
                //MessageBox.Show("SN序列号长度不等于20！输入的序列号长度为" + packSn.Length.ToString() + "位");
                return;
            }


            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x27, 0x10 };
            if (identity)
            {
                can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x28, 0x10 };
            }

            string[] strs = new string[packSn.Length];
            for (int j = 0; j < packSn.Length; j++)
            {
                strs[j] = ((int)packSn[j]).ToString("X2");
            }

            for (int j = 0; j < strs.Length; j++)
            {
                int i = 0;
                byte[] data = new byte[8];

                data[i++] = Convert.ToByte((j / 7).ToString(), 16);//条形码组号n n=0,1,2
                data[i++] = Convert.ToByte(strs[j++], 16);                                  //第7* n+1位成品条形码，如'A'
                data[i++] = Convert.ToByte(strs[j++], 16);                                  //第7* n+2位成品条形码，如'A'
                data[i++] = Convert.ToByte(strs[j++], 16);                                  //第7* n+3位成品条形码，如'A'
                data[i++] = Convert.ToByte(strs[j++], 16);                                  //第7* n+4位成品条形码，如'A'
                data[i++] = Convert.ToByte(strs[j++], 16);                                  //第7* n+5位成品条形码，如'A'
                data[i++] = Convert.ToByte(strs[j++], 16);                                  //第7* n+6位成品条形码，如'A'
                data[i++] = j == strs.Length ? (byte)0x00 : Convert.ToByte(strs[j], 16);    //第7* n+7位成品条形码，如'A'
                if (baseCanHelper.Send(data, can_id))
                {
                    flag_WriteSuccess = true;
                }
            }

            if (flag_WriteSuccess) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        public ICommand Write_0x29_Cmd => new RelayCommand(Write_0x29);
        /// <summary>
        ///设置电池信息0x1029FFE0
        /// </summary>
        public void Write_0x29()
        {
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x29, 0x10 };
            byte[] data = new byte[8];

            data[0] = (byte)(Cell_brandList.IndexOf(SelectedCell_brand) + 1);
            data[1] = (byte)(Cell_model_numList.IndexOf(SelectedCell_model_num) + 1);
            data[2] = (byte)Clr_FLASH_flag_funcList.IndexOf(SelectedClr_FLASH_flag_func);
            data[3] = (byte)Query_FLASH_flag_funcList.IndexOf(SelectedQuery_FLASH_flag_func);

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        private void Calibration(byte firstData, int CalibrationVal)
        {
            //byte[] can_id = new byte[] { 0xE0, FrmMain.BMS_ID, 0x2A, 0x10 };
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x2A, 0x10 };

            byte[] data = new byte[8] { firstData, Convert.ToByte(CalibrationVal & 0xff), Convert.ToByte(CalibrationVal >> 8), 0x00, 0x00, 0x00, 0x00, 0x00 };

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }
        public ICommand WriteCali_items_1_Cmd => new RelayCommand(WriteCali_items_1);
        /// <summary>
        ///校准系数标定0x102AXXE0
        /// 0x01:校准总压(0-1000V)  0.1V
        /// </summary>
        public void WriteCali_items_1()
        {
            int val = Convert.ToInt32(Convert.ToDouble(Cali_items_1) / 0.1);
            Calibration(0x01, val);
        }

        public ICommand WriteCali_items_2_Cmd => new RelayCommand(WriteCali_items_2);
        /// <summary>
        ///校准系数标定0x102AXXE0
        /// 0x02:校准负载电压(0-1000V) 0.1V
        /// </summary>
        public void WriteCali_items_2()
        {
            int val = Convert.ToInt32(Convert.ToDouble(Cali_items_2) / 0.1);
            Calibration(0x02, val);
        }

        public ICommand WriteCali_items_3_Cmd => new RelayCommand(WriteCali_items_3);
        /// <summary>
        ///校准系数标定0x102AXXE0
        /// 0x03:校准充电电流(0-50A) 0.01A
        /// </summary>
        public void WriteCali_items_3()
        {
            int val = Convert.ToInt32(Convert.ToDouble(Cali_items_3) / 0.01);
            Calibration(0x03, val);
        }

        public ICommand WriteCali_items_4_Cmd => new RelayCommand(WriteCali_items_4);
        /// <summary>
        ///校准系数标定0x102AXXE0
        /// 0x04:校准充电小电流(0-5A) 0.01A
        /// </summary>
        public void WriteCali_items_4()
        {
            int val = Convert.ToInt32(Convert.ToDouble(Cali_items_4) / 0.01);
            Calibration(0x04, val);
        }

        public ICommand WriteCali_items_5_Cmd => new RelayCommand(WriteCali_items_5);
        /// <summary>
        ///校准系数标定0x102AXXE0
        /// 0x05:校准放电电流(0-50A) 0.01A
        /// </summary>
        public void WriteCali_items_5()
        {
            int val = Convert.ToInt32(Convert.ToDouble(Cali_items_5) / 0.01);
            Calibration(0x05, val);
        }

        public ICommand WriteCali_items_6_Cmd => new RelayCommand(WriteCali_items_6);
        /// <summary>
        ///校准系数标定0x102AXXE0
        /// 0x06:校准放电小电流(0-5A) 0.01A
        /// </summary>
        public void WriteCali_items_6()
        {
            int val = Convert.ToInt32(Convert.ToDouble(Cali_items_6) / 0.01);
            Calibration(0x06, val);
        }

        public ICommand Write_0x2B_Cmd => new RelayCommand(Write_0x2B);
        /// <summary>
        ///BMS功能开关0x102BXXE0
        /// </summary>
        public void Write_0x2B()
        {
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x2B, 0x10 };
            byte[] data = new byte[8];

            if (SelectedForce_sleep == null || SelectedAlarm_protection_beep == null || SelectedAlarm_protection_led == null || SelectedHeat == null || SelectedClr_system_lock == null || SelectedForce_ctrl_switch == null)
            {
                MessageBoxHelper.Warning("选项不能为空", "提示", null, ButtonType.OK);
                return;
            }

            byte data0_BIT_0_1 = (byte)(Force_sleepList.IndexOf(SelectedForce_sleep) & 0x03); // 保留低2位 data[0] BIT[0:1]
            byte data0_BIT_2_3 = (byte)((Alarm_protection_beepList.IndexOf(SelectedAlarm_protection_beep) & 0x03) << 2); // 保留低2位并左移2位 data[0] BIT[2:3]
            byte data0_BIT_4_5 = (byte)((Alarm_protection_ledList.IndexOf(SelectedAlarm_protection_led) & 0x03) << 4); // 保留低2位并左移4位 data[0] BIT[4:5]
            byte data0_BIT_6_7 = (byte)((HeatList.IndexOf(SelectedHeat) & 0x03) << 6); // 保留低2位并左移6位 BIT[6:7]

            byte data1_BIT_0_1 = (byte)(Clr_system_lockList.IndexOf(SelectedClr_system_lock) & 0x03); // 保留低2位


            data[0] = (byte)(data0_BIT_0_1 | data0_BIT_2_3 | data0_BIT_4_5 | data0_BIT_6_7);
            data[1] = data1_BIT_0_1; // data[1] 只需要用低2位 data[1] BIT[0:1]

            if (Force_ctrl_switchList.IndexOf(SelectedForce_ctrl_switch) == 0) data[6] = 0x00;
            else if (Force_ctrl_switchList.IndexOf(SelectedForce_ctrl_switch) == 1) data[6] = 0xAA;

            //byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x2B, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            byte[] crcData = new byte[11] { 0xE0, 0x01, 0x2B, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);

        }

        public ICommand Write_0x2F_Cmd => new RelayCommand(Write_0x2F);
        /// <summary>
        //ATE强制控制指令0x102FXXE0
        /// </summary>
        public void Write_0x2F()
        {

            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x2F, 0x10 };
            byte[] data = new byte[8];

            if (Selectedchg_mos_ctrl == null || Selecteddsg_mos_ctrl == null || Selectedpre_chg_mos_ctrl == null || Selectedheating_film_ctrl == null
                || Selectedforce_sleep == null || Selectedbeep_open == null || Selectedchg_stop_ctrl == null || Selectedheating_relay_drive_I0S == null
                || SelectedID_OP_ctrl == null || SelectedID_IN_state == null || SelectedIDM_OP_ctrl == null || SelectedIDM_IN_state == null
                || force_bal_ctrl_1_8 == null || force_bal_ctrl_9_16 == null || Selectedforce_ctrl_switch == null)

            {
                MessageBoxHelper.Warning("选项不能为空", "提示", null, ButtonType.OK);
                return;
            }

            byte data0_BIT_0_1 = (byte)(chg_mos_ctrlList.IndexOf(Selectedchg_mos_ctrl) & 0x03); // 保留低2位 data[0] BIT[0:1]
            byte data0_BIT_2_3 = (byte)((dsg_mos_ctrlList.IndexOf(Selecteddsg_mos_ctrl) & 0x03) << 2); // 保留低2位并左移2位 data[0] BIT[2:3]
            byte data0_BIT_4_5 = (byte)((pre_chg_mos_ctrlList.IndexOf(Selectedpre_chg_mos_ctrl) & 0x03) << 4); // 保留低2位并左移4位 data[0] BIT[4:5]
            byte data0_BIT_6_7 = (byte)((heating_film_ctrlList.IndexOf(Selectedheating_film_ctrl) & 0x03) << 6); // 保留低2位并左移6位 data[0] BIT[6:7]

            byte data1_BIT_0_1 = (byte)(force_sleepList.IndexOf(Selectedforce_sleep) & 0x03);
            byte data1_BIT_2_3 = (byte)((beep_openList.IndexOf(Selectedbeep_open) & 0x03) << 2);
            byte data1_BIT_4_5 = (byte)((chg_stop_ctrlList.IndexOf(Selectedchg_stop_ctrl) & 0x03) << 4);
            byte data1_BIT_6_7 = (byte)((heating_relay_drive_I0SList.IndexOf(Selectedheating_relay_drive_I0S) & 0x03) << 6);

            byte data2_BIT_0_1 = (byte)(ID_OP_ctrlList.IndexOf(SelectedID_OP_ctrl) & 0x03);
            byte data2_BIT_2_3 = (byte)((ID_IN_stateList.IndexOf(SelectedID_IN_state) & 0x03) << 2);
            byte data2_BIT_4_5 = (byte)((IDM_OP_ctrlList.IndexOf(SelectedIDM_OP_ctrl) & 0x03) << 4);
            byte data2_BIT_6_7 = (byte)((IDM_IN_stateList.IndexOf(SelectedIDM_IN_state) & 0x03) << 6);

            data[0] = (byte)(data0_BIT_0_1 | data0_BIT_2_3 | data0_BIT_4_5 | data0_BIT_6_7);
            data[1] = (byte)(data1_BIT_0_1 | data1_BIT_2_3 | data1_BIT_4_5 | data1_BIT_6_7);
            data[2] = (byte)(data2_BIT_0_1 | data2_BIT_2_3 | data2_BIT_4_5 | data2_BIT_6_7);

            data[4] = Convert.ToByte(force_bal_ctrl_1_8);
            data[5] = Convert.ToByte(force_bal_ctrl_9_16);

            if (force_ctrl_switchList.IndexOf(Selectedforce_ctrl_switch) == 0) data[6] = 0x00;
            else if (force_ctrl_switchList.IndexOf(Selectedforce_ctrl_switch) == 1) data[6] = 0xAA;

            //byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x2F, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            byte[] crcData = new byte[11] { 0xE0, Convert.ToByte(SelectedRequest7), 0x2F, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);

        }

        public ICommand Write_0x1E_Cmd => new RelayCommand(Write_0x1E);
        /// <summary>
        ///ATE强制控制指令0x101EXXE0
        /// </summary>
        public void Write_0x1E()
        {
            byte[] can_id = new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x1E, 0x10 };
            byte[] data = new byte[8];

            if (Selectedllc_force_dsg == null || Selectedllc_force_chg == null || SelectedLED_ctrl == null || Selectedhw_wdg_ctrl == null || SelectedForceCtrlSwitch == null)
            {
                MessageBoxHelper.Warning("选项不能为空", "提示", null, ButtonType.OK);
                return;
            }

            byte data0_BIT_0_1 = (byte)(llc_force_dsgList.IndexOf(Selectedllc_force_dsg) & 0x03); // 保留低2位 data[0] BIT[0:1]
            byte data0_BIT_2_3 = (byte)((llc_force_chgList.IndexOf(Selectedllc_force_chg) & 0x03) << 2); // 保留低2位并左移2位 data[0] BIT[2:3]
            byte data0_BIT_4_5 = (byte)((LED_ctrlList.IndexOf(SelectedLED_ctrl) & 0x03) << 4); // 保留低2位并左移4位 data[0] BIT[4:5]
            byte data0_BIT_6_7 = (byte)((hw_wdg_ctrlList.IndexOf(Selectedhw_wdg_ctrl) & 0x03) << 6); // 保留低2位并左移6位 BIT[6:7]

            data[0] = (byte)(data0_BIT_0_1 | data0_BIT_2_3 | data0_BIT_4_5 | data0_BIT_6_7);

            if (Force_ctrl_switchList.IndexOf(SelectedForceCtrlSwitch) == 0) data[6] = 0xAA;
            else if (Force_ctrl_switchList.IndexOf(SelectedForceCtrlSwitch) == 1) data[6] = 0x00;

            //byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x1E, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            byte[] crcData = new byte[11] { 0xE0, Convert.ToByte(SelectedRequest7), 0x1E, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);

        }

        private bool ConvertIntToByteArray(Int32 m, ref byte[] arry)
        {
            if (arry == null) return false;
            if (arry.Length < 4) return false;

            arry[0] = (byte)(m & 0xFF);
            arry[1] = (byte)((m & 0xFF00) >> 8);
            arry[2] = (byte)((m & 0xFF0000) >> 16);
            arry[3] = (byte)((m >> 24) & 0xFF);

            return true;
        }
        public byte[] Uint16ToBytes(uint ivalue)
        {
            byte[] bytes = new byte[2];
            bytes[1] = (byte)(ivalue >> 8);
            bytes[0] = (byte)(ivalue & 0xff);

            return bytes;
        }

        /// <summary>
        /// 启动数据存储
        /// </summary>
        /// <param name="obj"></param>
        private void TimerCallBack_DataSaving(object obj)
        {
            model.PackID = Convert.ToByte(SelectedRequest7).ToString();
            model.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (model != null)
            {
                var folderPath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}//StoreData_BMS1500V//BMU";
                var filePath = $"{folderPath}//BMU实时数据_地址{SelectedRequest7}_{DateTime.Now.ToString("yyyy-MM-dd")}.csv";

                // 检查文件夹是否存在，不存在则创建
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 创建临时电池电压数据列表的副本，重新设置SectionNumber
                var tempbatteryVoltageDataList = batteryVoltageDataList.Select(b => new batteryVoltageData
                {
                    Voltage = b.Voltage,
                    SectionNumber = $"BMU{(SelectedRequest7)}电池电压(V) " + b.SectionNumber
                }).ToList();


                // 创建临时电池温度数据列表的副本，重新设置CellNumber
                var tempbatteryTemperatureDataList = batteryTemperatureDataList.Select(b => new batteryTemperatureData
                {
                    Temperature = b.Temperature,
                    CellNumber = $"BMU{(SelectedRequest7)}电池温度(℃) " + b.CellNumber
                }).ToList();

                // 创建临时电池温度数据列表的副本，重新设置CellNumber
                var tempbatterySocDataList = batterySocDataList.Select(b => new batterySocData
                {
                    SOC = b.SOC,
                    SectionNumber = $"BMU{(SelectedRequest7)}电池SOC(%) " + b.SectionNumber
                }).ToList();

                // 如果文件不存在，则写入表头
                if (!File.Exists(filePath))
                {
                    // 写入表头，拼接各部分
                    string header = model.GetHeader() + "," +// 分隔符                                 
                                    string.Join(",", tempbatteryVoltageDataList.Select(b => b.SectionNumber)) + "," + string.Join(",", tempbatteryTemperatureDataList.Select(b => b.CellNumber)) + "," + string.Join(",", tempbatterySocDataList.Select(b => b.SectionNumber)) + "\r\n";
                    File.AppendAllText(filePath, header);
                }

                // 写入数据
                var values = model.GetValue() + "," +
                             string.Join(",", tempbatteryVoltageDataList.Select(b => b.Voltage)) + "," + string.Join(",", tempbatteryTemperatureDataList.Select(b => b.Temperature)) + string.Join(",", tempbatterySocDataList.Select(b => b.SOC)) + "\r\n";
                File.AppendAllText(filePath, values);
            }
        }

        private void ClearBatteryList(int startIndex)
        {


            for (int i = startIndex; i < batteryVoltageDataList.Count; i++)
            {
                batteryVoltageDataList[i].Voltage = "";
            }
            for (int i = startIndex; i < batterySocDataList.Count; i++)
            {
                batterySocDataList[i].SOC = "";
            }
            for (int i = startIndex; i < batterySohDataList.Count; i++)
            {
                batterySohDataList[i].SOH = "";
            }
            for (int i = startIndex; i < batteryTemperatureDataList.Count; i++)
            {
                batteryTemperatureDataList[i].Temperature = "";
            }
            for (int i = startIndex; i < batteryEquilibriumStateDataList.Count; i++)
            {
                batteryEquilibriumStateDataList[i].BatteryEquilibriumState = "";
            }
            for (int i = startIndex; i < batteryEquilibriumTemperatureDataList.Count; i++)
            {
                batteryEquilibriumTemperatureDataList[i].BatteryEquilibriumTemperature = "";
            }


        }

        private void TimerCallBack(object obj)
        {
            if (baseCanHelper.IsConnection)
            {

                if (ConstantDef.BatteryCellNumber == 48)
                {
                    ClearBatteryList(48);
                }

                //获取实时数据指令       0x072：BMS电池均衡状态 0x0A0：BMS单电芯的SOC 0x0A1：BMS单电芯的SOH
                baseCanHelper.Send(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                               , new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x2C, 0x10 });
                //BMS电池温度
                baseCanHelper.Send(new byte[] { 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                              , new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x2C, 0x10 });


                if (baseCanHelper.CommunicationType == "Ecan")
                {

                    while (EcanHelper._task.Count > 0 && !cts.IsCancellationRequested)
                    {
                        if (EcanHelper._task.TryDequeue(out CAN_OBJ ch))
                        {
                            Application.Current.Dispatcher.Invoke(() => { AnalysisData(ch.ID, ch.Data); });
                            //Log.Info($"{System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")} 接收CAN数据:{BitConverter.ToString(ch.Data).Replace("-", " ")}  帧ID:{ch.ID.ToString("X8")}");
                        }
                    }

                }
                else
                {
                    lock (ControlcanHelper._locker)
                    {
                        while (ControlcanHelper._task.Count > 0 && !cts.IsCancellationRequested)
                        {
                            if (ControlcanHelper._task.TryDequeue(out VCI_CAN_OBJ ch))
                            {
                                Application.Current.Dispatcher.Invoke(() => { AnalysisData(ch.ID, ch.Data); });
                            }
                        }
                    }
                }
            }
        }

        public ICommand ReadAllParameterCmd => new RelayCommand(ReadAllParameter);
        /// <summary>
        /// 读取BMU参数设置—一键操作标定参数0x102EXXE0 0x01:读取所有参数
        /// </summary>
        public void ReadAllParameter()
        {
            if (baseCanHelper.IsConnection)
            {
                //读取BMU参数 0x01:读取所有参数
                try
                {
                    baseCanHelper.Send(new byte[8] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }
                                , new byte[] { 0xE0, Convert.ToByte(SelectedRequest7), 0x2E, 0x10 });
                }
                catch (Exception ex)
                {
                    MessageBoxHelper.Warning("读取失败！", "警告", null, ButtonType.OK);
                }

            }
        }


        public void AnalysisData(uint canID, byte[] data)
        {

            byte Address_BMU = Convert.ToByte(SelectedRequest7);

            byte[] canid = BitConverter.GetBytes(canID);

            if ((canID & 0xff) != Address_BMU)
                return;
            //if (canid[0] != Address_BMU || !(canid[0] == Address_BMU && canid[1] == 0x81 /*&& canid[3] == 0x18*/)) 
            //    return;
            if (baseCanHelper.CommunicationType == "Ecan")
            {
                EcanHelper.Instance().GetLogAction()?.Invoke(1, HexDataHelper.GetDebugByteString(data, "Recv：0x" + canID.ToString("X")));
            }

            if (model == null) model = new RealtimeData_BMS1500V_BMU();
            string[] strs;

            //string[] batteryVoltages = new string[3840];
            //string[] batterySocs = new string[3840];
            //string[] batterySohs = new string[3840];
            //string[] batteryTemperatures = new string[3840];
            //string[] batteryEquilibriumStates = new string[3840];

            try
            {
                switch (canID | 0xff)
                {
                    case 0x1004FFFF:
                    case 0x1004E0FF:
                        strs = new string[4] { "0.1", "0.1", "0.01", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        BatteryVolt = strs[0];
                        LoadVolt = strs[1];
                        BatteryCurrent = strs[2];
                        SOC = strs[3];

                        model.BatteryVolt = Convert.ToDouble(strs[0]);
                        model.LoadVolt = Convert.ToDouble(strs[1]);
                        model.BatteryCurrent = Convert.ToDouble(strs[2]);
                        model.SOC = Convert.ToDouble(strs[3]);
                        break;
                    case 0x1005FFFF:
                    case 0x1005E0FF:
                        strs = new string[6];
                        strs[0] = BytesToIntger(data[1], data[0]);
                        strs[1] = BytesToIntger(0x00, data[2]);
                        strs[2] = BytesToIntger(data[4], data[3]);
                        strs[3] = BytesToIntger(0x00, data[5]);
                        strs[4] = (Convert.ToInt32(strs[0]) - Convert.ToInt32(strs[2])).ToString();
                        strs[5] = BytesToIntger(data[7], data[6]);//精度待确认0.001

                        BatMaxCellVolt = strs[0];
                        BatMaxCellVoltNum = strs[1];
                        BatMinCellVolt = strs[2];
                        BatMinCellVoltNum = strs[3];
                        BatDiffCellVolt = strs[4];
                        BatAvgCellVolt = strs[5];

                        model.BatMaxCellVolt = Convert.ToUInt16(strs[0]);
                        model.BatMaxCellVoltNum = Convert.ToUInt16(strs[1]);
                        model.BatMinCellVolt = Convert.ToUInt16(strs[2]);
                        model.BatMinCellVoltNum = Convert.ToUInt16(strs[3]);
                        model.BatDiffCellVolt = Convert.ToUInt16(strs[4]);
                        //缺少平均单体电压
                        break;
                    case 0x1006FFFF:
                    case 0x1006E0FF:
                        strs = new string[5] { "0.1", "1", "0.1", "1", "0.1" };
                        strs[0] = BytesToIntger(data[1], data[0], 0.1);
                        strs[1] = BytesToIntger(0x00, data[2]);
                        strs[2] = BytesToIntger(data[4], data[3], 0.1);
                        strs[3] = BytesToIntger(0x00, data[5]);
                        strs[4] = BytesToIntger(data[7], data[6], 0.1);

                        BatMaxCellTemp = strs[0];
                        BatMaxCellTempNum = strs[1];
                        BatMinCellTemp = strs[2];
                        BatMinCellTempNum = strs[3];
                        BatAverageTemp = strs[4];
                        BatDiffCellTemp = (decimal.Parse(BatMaxCellTemp) - decimal.Parse(BatMinCellTemp)).ToString();

                        model.BatMaxCellTemp = Convert.ToDouble(strs[0]);
                        model.BatMaxCellTempNum = Convert.ToUInt16(strs[1]);
                        model.BatMinCellTemp = Convert.ToDouble(strs[2]);
                        model.BatMinCellTempNum = Convert.ToUInt16(strs[3]);
                        model.BatAverageTemp = Convert.ToDouble(strs[4]);
                        break;
                    case 0x1007FFFF:
                    case 0x1007E0FF:
                        TotalChgCap = (Convert.ToDouble(((data[3] << 24) + (data[2] << 16) + (data[1] << 8) + (data[0] & 0xff)) * 0.001)).ToString();
                        TotalDsgCap = (Convert.ToDouble(((data[7] << 24) + (data[6] << 16) + (data[5] << 8) + (data[4] & 0xff)) * 0.001)).ToString();

                        model.TotalChgCap = Convert.ToDouble(TotalChgCap);
                        model.TotalDsgCap = Convert.ToDouble(TotalDsgCap);
                        break;
                    case 0x1008FFFF:
                    case 0x1008E0FF:
                        //报警信息                     
                        AnalysisLog(data, 1);
                        break;
                    case 0x100DFFFF:
                    case 0x100DE0FF:
                        strs = new string[4] { "0.1", "0.1", "0.1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        CellTemperature1 = strs[0];
                        CellTemperature2 = strs[1];
                        CellTemperature3 = strs[2];
                        CellTemperature4 = strs[3];
                        model.CellTemperature1 = Convert.ToDouble(strs[0]);
                        model.CellTemperature2 = Convert.ToDouble(strs[1]);
                        model.CellTemperature3 = Convert.ToDouble(strs[2]);
                        model.CellTemperature4 = Convert.ToDouble(strs[3]);
                        break;
                    case 0x100EFFFF:
                    case 0x100EE0FF:
                        strs = new string[3] { "0.1", "0.1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        //低8串电池均衡状态 高8串电池均衡状态 新协议已删除
                        //string strAdd = "[1~16]:";
                        //for (int i = 6; i < 8; i++)
                        //{
                        //    for (short j = 0; j < 8; j++)
                        //    {
                        //        if (GetBit(data[i], j) == 1)
                        //        {

                        //            strAdd += "1";
                        //        }
                        //        else
                        //        {

                        //            strAdd = strAdd + "0";
                        //        }
                        //        if (j != 0)
                        //        {
                        //            if (j + 1 % 8 == 0)
                        //                strAdd += ",";
                        //            else if (j + 1 % 4 == 0)
                        //                strAdd += " ";
                        //        }
                        //    }
                        //}
                        MosTemperature = strs[0];
                        EnvTemperature = strs[1];
                        SOH = strs[2];
                        //EquaState = strAdd;//低8串电池均衡状态 高8串电池均衡状态 新协议已删除

                        model.MosTemperature = Convert.ToDouble(strs[0]);
                        model.EnvTemperature = Convert.ToDouble(strs[1]);
                        model.SOH = Convert.ToDouble(strs[2]);
                        //model.EquaState = strAdd;//低8串电池均衡状态 高8串电池均衡状态 新协议已删除
                        break;
                    case 0x100FFFFF:
                    case 0x100FE0FF:
                        RemainingCapacity = BytesToIntger(data[1], data[0], 0.1);
                        FullCapacity = BytesToIntger(data[3], data[2], 0.1);
                        CycleTime = BytesToIntger(data[5], data[4]);

                        model.RemainingCapacity = RemainingCapacity;
                        model.FullCapacity = FullCapacity;
                        model.CycleTime = Convert.ToUInt16(CycleTime);
                        break;
                    case 0x1040FFFF:
                    case 0x1040E0FF:
                        CumulativeDischargeCapacity = (((data[3] << 24) + (data[2] << 16) + (data[1] << 8) + (data[0] & 0xff))).ToString();
                        CumulativeChargeCapacity = (((data[7] << 24) + (data[6] << 16) + (data[5] << 8) + (data[4] & 0xff))).ToString(); ;

                        model.CumulativeDischargeCapacity = CumulativeDischargeCapacity;
                        model.CumulativeChargeCapacity = CumulativeChargeCapacity;
                        break;
                    case 0x1041FFFF:
                    case 0x1041E0FF:
                        //此功能码调整被删除
                        //BalanceTemperature1 = BytesToIntger(data[1], data[0], 0.1);
                        //BalanceTemperature2 = BytesToIntger(data[3], data[2], 0.1);
                        //DcdcTemperature1 = BytesToIntger(data[5], data[4], 0.1);
                        //DcdcTemperature2 = BytesToIntger(data[7], data[6], 0.1);

                        //model.BalanceTemperature1 = BalanceTemperature1;
                        //model.BalanceTemperature2 = BalanceTemperature2;
                        //model.DcdcTemperature1 = DcdcTemperature1;
                        //model.DcdcTemperature2 = DcdcTemperature2;
                        break;
                    case 0x1042FFFF:
                    case 0x1042E0FF:
                        strs = new string[4] { "0.1", "0.1", "0.1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        CellTemperature5 = strs[0];
                        CellTemperature6 = strs[1];
                        CellTemperature7 = strs[2];
                        CellTemperature8 = strs[3];
                        model.CellTemperature5 = Convert.ToDouble(CellTemperature5);
                        model.CellTemperature6 = Convert.ToDouble(CellTemperature6);
                        model.CellTemperature7 = Convert.ToDouble(CellTemperature7);
                        model.CellTemperature8 = Convert.ToDouble(CellTemperature8);
                        break;
                    case 0x1045FFFF:
                    case 0x1045E0FF:
                        AnalysisLog(data, 2);
                        break;
                    case 0x104BFFFF:
                    case 0x104BE0FF:
                        AnalysisLog(data, 3);

                        //故障等级 U8,byte[7],故障等级(bit0：提示告警;bit1：一般告警;bit2：严重告警;bit3:硬件故障)
                        AlarmLevelBit0 = (data[7] & 0x01) == 0x01 ? true : false;
                        AlarmLevelBit1 = (data[7] & 0x02) == 0x02 ? true : false;
                        AlarmLevelBit2 = (data[7] & 0x04) == 0x04 ? true : false;
                        AlarmLevelBit3 = (data[7] & 0x08) == 0x08 ? true : false;
                        break;
                    case 0x104AFFFF:
                    case 0x104AE0FF:
                        strs = new string[4] { "0.1", "0.1", "0.1", "0.1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }
                        //功率正端子温度
                        PowerTemperture1 = strs[0];
                        //功率负端子温度
                        PowerTemperture2 = strs[1];

                        //加热膜电流->主动均衡温度1
                        //HeatCur = strs[2];
                        DcdcTemperature1 = strs[2];
                        //加热膜电压->主动均衡温度2
                        //HeatRelayVol = strs[3];
                        DcdcTemperature2 = strs[3];

                        model.PowerTemperture1 = Convert.ToDouble(PowerTemperture1);
                        model.PowerTemperture2 = Convert.ToDouble(PowerTemperture2);
                        model.DcdcTemperature1 = Convert.ToDouble(DcdcTemperature1);
                        model.DcdcTemperature2 = Convert.ToDouble(DcdcTemperature2);

                        //model.HeatCur = Convert.ToDouble(HeatCur);
                        //model.HeatRelayVol = Convert.ToDouble(HeatRelayVol);
                        break;
                    case 0x1027E0FF:
                        string strSn = GetPackSN(data);
                        if (!string.IsNullOrEmpty(strSn)) SN = strSn;

                        break;
                    case 0x1046FFFF:
                    case 0x1046E0FF:
                        //加热请求
                        HeatRequest = "0:无请求无禁止 1:请求加热 2:禁止加热";
                        switch (data[1] & 0x03)
                        {
                            case 0:
                                HeatRequest = "NONE";
                                break;
                            case 1:
                                HeatRequest = "请求加热";
                                break;
                            case 2:
                                HeatRequest = "禁止加热";
                                break;
                            default:
                                break;
                        }

                        IsChargeEnable = (GetBit(data[0], 0) == 1);
                        IsDischargeEnable = (GetBit(data[0], 1) == 1);
                        IsBmuCutOffRequest = (GetBit(data[0], 2) == 1);
                        IsBmuPowOffRequest = (GetBit(data[0], 3) == 1);
                        IsForceChrgRequest = (GetBit(data[0], 4) == 1);

                        IsChagreStatus = (GetBit(data[2], 0) == 1);
                        IsDisChargeStatus = (GetBit(data[2], 1) == 1);

                        IsDiIO = (GetBit(data[6], 0) == 1);
                        IsChargeIO = (GetBit(data[6], 1) == 1);


                        model.ChargeEnable = (ushort)GetBit(data[0], 0);
                        model.DischargeEnable = (ushort)GetBit(data[0], 1);
                        model.BmuCutOffRequest = (ushort)GetBit(data[0], 2);
                        model.BmuPowOffRequest = (ushort)GetBit(data[0], 3);
                        model.ForceChrgRequest = (ushort)GetBit(data[0], 4);
                        model.ChagreStatus = (ushort)GetBit(data[2], 0);
                        model.DischargeStatus = (ushort)GetBit(data[2], 1);
                        model.DiIO = (ushort)GetBit(data[6], 0);
                        model.ChargeIO = (ushort)GetBit(data[6], 1);
                        model.HeatRequest = HeatRequest;
                        break;
                    case 0x1047FFFF:
                    case 0x1047E0FF:
                        SyncFallSoc = Convert.ToInt32(data[0].ToString("X2"), 16).ToString();
                        BmsStatus = Enum.Parse(typeof(BMSState), (Convert.ToInt32(data[1].ToString("X2"), 16) & 0x0f).ToString()).ToString();
                        string balanceStatus = "";
                        switch ((Convert.ToInt32(data[2].ToString("X2"), 16) & 0x0f))
                        {
                            case 0: balanceStatus = "禁止"; break;
                            case 1: balanceStatus = "放电"; break;
                            case 2: balanceStatus = "充电"; break;
                            default:
                                break;
                        }
                        ActiveBalanceStatus = balanceStatus;
                        ChargeCurrentLimitation = BytesToIntger(data[5], data[4], 0.01);
                        DischargeCurrentLimitation = BytesToIntger(data[7], data[6], 0.01);

                        model.SyncFallSoc = SyncFallSoc;
                        model.BmsStatus = BmsStatus;
                        model.ActiveBalanceStatus = ActiveBalanceStatus;
                        model.ChargeCurrentLimitation = Convert.ToDouble(ChargeCurrentLimitation);
                        model.DischargeCurrentLimitation = Convert.ToDouble(DischargeCurrentLimitation);
                        break;
                    case 0x1048FFFF:
                    case 0x1048E0FF:
                        strs = new string[3] { "0.1", "0.1", "1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        BalanceBusVoltage = strs[0];
                        BalanceCurrent = strs[1];
                        ActiveBalanceMaxCellVolt = strs[2];
                        //BatAverageTemp = strs[3]; //挪到0x006

                        model.BalanceBusVoltage = Convert.ToDouble(strs[0]);
                        model.BalanceCurrent = Convert.ToDouble(strs[1]);
                        model.ActiveBalanceMaxCellVolt = Convert.ToDouble(strs[2]);
                        //model.BatAverageTemp = Convert.ToDouble(strs[3]);
                        break;
                    case 0x1049FFFF:
                    case 0x1049E0FF:
                        strs = new string[3] { "0.01", "1", "1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        ActiveBalanceCellSoc = strs[0];
                        ActiveBalanceAccCap = strs[1];
                        ActiveBalanceRemainCap = strs[2];
                        model.ActiveBalanceCellSoc = Convert.ToDouble(ActiveBalanceCellSoc);
                        model.ActiveBalanceAccCap = ActiveBalanceRemainCap;
                        model.ActiveBalanceRemainCap = ActiveBalanceRemainCap;
                        break;
                    case 0x104CFFFF:
                    case 0x104CE0FF:
                        switch (data[0])
                        {
                            case 0x01:
                            case 0x02:
                                //用于集储中的soc校准
                                break;
                            case 0x03:
                                //电解液漏液浓度
                                ElectrolyteLeakageConcentration = (((data[1] << 8) | data[2])).ToString();
                                break;
                            case 0x05:
                                strs = new string[3];
                                for (int i = 0; i < strs.Length; i++)
                                {
                                    strs[i] = BytesToIntger(data[i * 2 + 2], data[i * 2 + 1]);
                                }

                                //电池包1~3电解液漏液浓度
                                ElectrolyteLeakageConcentrationPack1 = strs[0];
                                ElectrolyteLeakageConcentrationPack2 = strs[1];
                                ElectrolyteLeakageConcentrationPack3 = strs[2];
                                break;
                            case 0x06:
                                //电池包4电解液漏液浓度
                                ElectrolyteLeakageConcentrationPack4 = BytesToIntger(data[2], data[1]);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 0x106AFFFF:
                    case 0x106AE0FF:
                        string[] softwareVersion = new string[3];
                        for (int i = 0; i < 3; i++)
                        {
                            softwareVersion[i] = data[i + 1].ToString().PadLeft(2, '0');
                        }
                        BMUSaftwareVersion = Encoding.ASCII.GetString(new byte[] { data[0] }) + string.Join("", softwareVersion);
                        BMUCanVersion = Encoding.ASCII.GetString(new byte[] { data[5], data[6] });
                        BMUHardwareVersion = $"{Convert.ToInt16(data[4])}";
                        model.BMUSaftwareVersion = BMUSaftwareVersion;
                        model.BMUCanVersion = BMUCanVersion;
                        break;
                    case 0x106BFFFF:
                    case 0x106BE0FF:
                        strs = new string[3] { "0.1", "1", "1" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        BatNominalCapacity = strs[0];
                        RegisterName = strs[1];
                        BatType = strs[2];
                        model.BatNominalCapacity = BatNominalCapacity;
                        model.RegisterName = RegisterName;
                        model.BatType = BatType;
                        break;
                    case 0x106CFFFF:
                    case 0x106CE0FF:
                        //厂家信息
                        StringBuilder manufacturerName = new StringBuilder();
                        manufacturerName.Append(Encoding.ASCII.GetString(data));
                        ManufacturerName = manufacturerName.ToString();
                        model.ManufacturerName = ManufacturerName;
                        break;
                    case 0x106DFFFF:
                    case 0x106DE0FF:
                        strs = new string[3] { "0.001", "0.001", "0.001" };
                        for (int i = 0; i < strs.Length; i++)
                        {
                            strs[i] = BytesToIntger(data[i * 2 + 1], data[i * 2], Convert.ToDouble(strs[i]));
                        }

                        ResetMode = Enum.Parse(typeof(Reset_Mode), (Convert.ToInt32(data[6].ToString("X2"), 16) & 0x0f).ToString()).ToString();

                        AuxVolt = strs[0];
                        ChgCurOffsetVolt = strs[1];
                        DsgCurOffsetVolt = strs[2];
                        model.ResetMode = ResetMode;
                        model.AuxVolt = AuxVolt;
                        model.ChgCurOffsetVolt = ChgCurOffsetVolt;
                        model.DsgCurOffsetVolt = DsgCurOffsetVolt;
                        break;
                    case 0x1070E0FF://0x070:BMS电池单体电压(M - 1)* 30+ (N - 1)*3 ~  17 +(M - 1)* 30+ (N)*3 
                        // Byte 1:帧序号M  0x1-0x16    一帧3个电池电压数据，0x16帧共66个电池电压数据
                        int frameNumber = data[0];
                        // Byte 2: 包序号N 8包 1包66个电池电压数据  8包共528个电池电压数据
                        int sequenceNumber = data[1];

                        //调试时只有一个模块 共64个电池电压数据
                        if (sequenceNumber >= 0x01)
                        {
                            int startBatteryIndex = GetBatteryStartIndex(sequenceNumber, frameNumber);
                            ProcessBatteryData(startBatteryIndex, data);

                        }

                        int GetBatteryStartIndex(int sequenceNumber, int frameNumber)
                        {
                            // 每帧3个电池电压数据                        
                            return (frameNumber - 1) * 3;
                        }

                        void ProcessBatteryData(int startBatteryIndex, byte[] data)
                        {
                            var batteryVoltages = new string[3 + startBatteryIndex];

                            for (int k = 0; k < 3; k++)
                            {
                                int byteIndex = 2 + k * 2;
                                double voltageRaw = (data[byteIndex + 1] << 8) | data[byteIndex]; // [3:2] 2:低位，3:高位
                                if ((startBatteryIndex + k) < ConstantDef.PageMaxBatteryCellNumber)                                                              //  if (startBatteryIndex + k < ConstantDef.MaxBatteryCell)
                                {
                                    batteryVoltages[startBatteryIndex + k] = (voltageRaw * 0.001).ToString("F3");// 电压数据 0.001V/bit 保留3位小数
                                }

                            }




                            //batteryVoltageDataList.Clear();
                            //for (int i = startBatteryIndex; i < batteryVoltages.Length; i++)
                            //{
                            //    batteryVoltageDataList.Add(new RealtimeData_BMS1500V_BMU.batteryVoltageData
                            //    {
                            //        SectionNumber = (i + 1).ToString() + "#",
                            //        Voltage = batteryVoltages[i]
                            //    });
                            //}

                            for (int i = startBatteryIndex; i < batteryVoltages.Length; i++)
                            {

                                string sectionNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batteryVoltageDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的电压值
                                    existingData.Voltage = batteryVoltages[i];
                                }


                            }
                        }
                        break;
                    case 0x1071E0FF: //0x071：BMS电池温度+(M - 1)* 30+ (N - 1)*3 ~ 17+(M - 1)* 30 + (N)*3 
                        // Byte 1:帧序号 1~10 10帧 1帧3个电池温度数据
                        frameNumber = data[0];
                        // Byte 2: 包序号 1~N（电压按 30节电池划分的包数） 
                        sequenceNumber = data[1];

                        //if (sequenceNumber >= 0x01 && sequenceNumber <= 0x04)
                        //{
                        //    if (frameNumber >= 0x01 && frameNumber <= 0x0A)
                        //    {
                        //        int startBatteryIndex = GetbatteryTemperaturesStartIndex(sequenceNumber, frameNumber);
                        //        ProcessBatteryTemperature(startBatteryIndex, data);
                        //    }
                        //}
                        //实际调试30个电池温度数据

                        if (sequenceNumber >= 0x01)
                        {
                            int startBatteryIndex = GetbatteryTemperaturesStartIndex(sequenceNumber, frameNumber);
                            ProcessBatteryTemperature(startBatteryIndex, data);
                        }

                        int GetbatteryTemperaturesStartIndex(int sequenceNumber, int frameNumber)
                        {
                            // 每个包有30个电池温度，每帧 3个电池温度数据                       
                            return (sequenceNumber - 1) * 30 + (frameNumber - 1) * 3;
                        }

                        void ProcessBatteryTemperature(int startBatteryIndex, byte[] data)
                        {

                            var batteryTemperatures = new string[3 + startBatteryIndex];
                            for (int k = 0; k < 3; k++)
                            {
                                int byteIndex = 2 + k * 2;
                                Int16 temperatureRaw = (short)((data[byteIndex + 1] << 8) | data[byteIndex]); // [3:2] 2:低位，3:高位
                                                                                                              // 温度数据 0.1/bit 
                                if (startBatteryIndex + k < ConstantDef.PageMaxBatteryCellNumber)
                                {
                                    batteryTemperatures[startBatteryIndex + k] = (temperatureRaw * 0.1).ToString("F1");// 保留1位小数
                                }

                            }

                            //for (int k = 0; k < 3; k++)
                            //{
                            //    int byteIndex = 2 + k * 2;
                            //    double temperatureRaw = (data[byteIndex + 1] << 8) | data[byteIndex]; // [3:2] 2:低位，3:高位

                            //    // 检查温度值是否为65535，无效值不添加
                            //    if (temperatureRaw != 65535)
                            //    {
                            //        // 温度数据 0.1/bit 
                            //        batteryTemperatures[startBatteryIndex + k] = (temperatureRaw * 0.1).ToString("F1"); // 保留1位小数
                            //    }
                            //    else
                            //    {
                            //        // 无效值显示“-”
                            //        batteryTemperatures[startBatteryIndex + k] = "-"; // 或者其他处理逻辑
                            //    }
                            //}


                            for (int i = startBatteryIndex; i < batteryTemperatures.Length; i++)
                            {

                                // 如果索引为28或29，直接跳过当前循环
                                //if (i == 28 || i == 29) continue;
                                string cellNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batteryTemperatureDataList.FirstOrDefault(data => data.CellNumber == cellNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的均衡状态值
                                    existingData.Temperature = batteryTemperatures[i];
                                }

                            }
                        }

                        break;
                    case 0x1072E0FF://0x072：BMS电池均衡状态 (电芯id>16)
                        // Byte 1:包序号 1~N  1包56个电池均衡状态数据
                        frameNumber = data[0];

                        var batteryEquilibriumStates = new string[ConstantDef.PageMaxBatteryCellNumber]; // 创建一个数组来存储当前 frame 的状态

                        int startBatteryIndex1 = GetbatteryEquilibriumStateDataStartIndex(frameNumber);
                        ProcessBatteryEquilibriumStateData(startBatteryIndex1, data);

                        //实际调试一包只有48个电池均衡状态数据
                        // 获取电池均衡状态数据的起始索引
                        int GetbatteryEquilibriumStateDataStartIndex(int frameNumber)
                        {
                            return (frameNumber - 1) * 48;
                        }

                        // 处理电池均衡状态数据
                        void ProcessBatteryEquilibriumStateData(int startBatteryIndex1, byte[] data)
                        {



                            //解析新数据
                            for (int j = 0; j < 6; j++)
                            {
                                for (int k = 0; k < 8; k++)
                                {
                                    if (startBatteryIndex1 + k < ConstantDef.PageMaxBatteryCellNumber)
                                    {
                                        batteryEquilibriumStates[startBatteryIndex1 + k] = GetBit(data[j + 1], (short)k).ToString();
                                    }

                                }

                                for (int i = startBatteryIndex1; i < batteryEquilibriumStates.Length; i++)
                                {


                                    string cellNumber = (i + 1).ToString() + "#";

                                    // 查找是否已存在该编号的记录
                                    var existingData = batteryEquilibriumStateDataList.FirstOrDefault(data => data.CellNumber == cellNumber);
                                    if (existingData != null)
                                    {
                                        // 更新已有记录的均衡状态值
                                        existingData.BatteryEquilibriumState = batteryEquilibriumStates[i];
                                    }



                                }
                                startBatteryIndex1 += 8;
                            }
                        }
                        break;
                    case 0x1073E0FF://0x073：BMS均衡温度 N1=1+(N-1)*30+3*(M-1)
                        // Byte 1:帧序号 1~10 10帧 1帧3个电池温度数据
                        frameNumber = data[0];
                        // Byte 2: 包序号 1~N
                        sequenceNumber = data[1];

                        if (sequenceNumber >= 0x01)
                        {
                            if (frameNumber >= 0x01 && frameNumber <= 0x0A)
                            {
                                int startBatteryIndex = GetEquilibriumStartIndex(sequenceNumber, frameNumber);
                                ProcessEquilibriumTemperature(startBatteryIndex, data);
                            }
                        }
                        int GetEquilibriumStartIndex(int sequenceNumber, int frameNumber)
                        {
                            // 每个包有30个电池温度，每帧 3个电池温度数据                       
                            return (sequenceNumber - 1) * 30 + (frameNumber - 1) * 3;
                        }

                        void ProcessEquilibriumTemperature(int startBatteryIndex, byte[] data)
                        {


                            var batteryEquilibriumTemperatures = new string[3 + startBatteryIndex];
                            for (int k = 0; k < 3; k++)
                            {
                                int byteIndex = 2 + k * 2;
                                Int16 equilibriumTemperatureRaw = (short)((data[byteIndex + 1] << 8) | data[byteIndex]); // [3:2] 2:低位，3:高位
                                // 温度数据 0.1/bit 
                                if (startBatteryIndex + k < ConstantDef.PageMaxBatteryCellNumber)
                                {
                                    batteryEquilibriumTemperatures[startBatteryIndex + k] = (equilibriumTemperatureRaw * 0.1).ToString("F1");// 保留1位小数
                                }

                            }


                            for (int i = startBatteryIndex; i < batteryEquilibriumTemperatures.Length; i++)
                            {

                                string cellNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batteryEquilibriumTemperatureDataList.FirstOrDefault(data => data.CellNumber == cellNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的均衡温度值
                                    existingData.BatteryEquilibriumTemperature = batteryEquilibriumTemperatures[i];
                                }


                            }
                        }
                        break;
                    case 0x10A0FFFF:// 0x0A0：BMS单电芯的SOC
                    case 0x10A0E0FF:
                        // Byte 1:单电芯数据包组号(0~((PACK电芯个数/7) - 1))  224/7-1=31
                        frameNumber = data[0];

                        //if (frameNumber >= 0x00 && frameNumber <= 0x20)
                        //{
                        //    int startBatteryIndex = GetbatterySOCStartIndex(frameNumber);
                        //    ProcessBatterySOC(startBatteryIndex, data);
                        //}
                        //
                        var batterySocs = new string[ConstantDef.PageMaxBatteryCellNumber];
                        //调试数据 实际7包共48个电池SOC数据,最后一帧是48 % 7 = 6
                        if (frameNumber >= 0x00 && frameNumber <= 0x10)
                        {
                            int startBatteryIndex = GetbatterySOCStartIndex(frameNumber);
                            ProcessBatterySOC(startBatteryIndex, data);
                        }



                        int GetbatterySOCStartIndex(int frameNumber)
                        {
                            // 每个包有7个电池SOC数据                     
                            return frameNumber * 7;
                        }

                        void ProcessBatterySOC(int startBatteryIndex, byte[] data)
                        {
                            for (int k = 0; k < 7; k++)
                            {
                                int byteIndex = k + 1;
                                double SocRaw = data[byteIndex];
                                // SOC数据 1/bit 范围：0-255
                                if (startBatteryIndex + k < ConstantDef.PageMaxBatteryCellNumber)
                                {
                                    batterySocs[startBatteryIndex + k] = SocRaw.ToString();
                                }

                            }



                            for (int i = startBatteryIndex; i < batterySocs.Length; i++)
                            {

                                string sectionNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batterySocDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的SOC值
                                    existingData.SOC = batterySocs[i];
                                }

                            }
                        }

                        break;
                    case 0x10A1FFFF:// 0x0A0：BMS单电芯的SOH
                    case 0x10A1E0FF:
                        // Byte 1:单电芯数据包组号(0~((PACK电芯个数/7) - 1))  224/7-1=31
                        frameNumber = data[0];
                        var batterySohs = new string[ConstantDef.PageMaxBatteryCellNumber];
                        //if (frameNumber >= 0x01 && frameNumber <= 0x20)
                        //{
                        //    int startBatteryIndex = GetbatterySOHStartIndex(frameNumber);
                        //    ProcessBatterySOH(startBatteryIndex, data);
                        //}
                        //调试数据 实际7包共48个电池SOH数据,最后一帧是48 % 7 = 6
                        if (frameNumber >= 0x00 && frameNumber <= 0x10)
                        {
                            int startBatteryIndex = GetbatterySOHStartIndex(frameNumber);
                            ProcessBatterySOH(startBatteryIndex, data);
                        }


                        int GetbatterySOHStartIndex(int frameNumber)
                        {
                            // 每个包有7个电池SOH数据                     
                            return frameNumber * 7;
                        }

                        void ProcessBatterySOH(int startBatteryIndex, byte[] data)
                        {



                            for (int k = 0; k < 7; k++)
                            {
                                int byteIndex = k + 1;
                                double SohRaw = data[byteIndex];
                                // SOH数据 1/bit 范围：0-255
                                if (startBatteryIndex + k < ConstantDef.PageMaxBatteryCellNumber)
                                {
                                    batterySohs[startBatteryIndex + k] = SohRaw.ToString();
                                }

                            }


                            for (int i = startBatteryIndex; i < batterySohs.Length; i++)
                            {

                                string sectionNumber = (i + 1).ToString() + "#";

                                // 查找是否已存在该编号的记录
                                var existingData = batterySohDataList.FirstOrDefault(data => data.SectionNumber == sectionNumber);
                                if (existingData != null)
                                {
                                    // 更新已有记录的SOH值
                                    existingData.SOH = batterySohs[i];
                                }

                            }
                        }

                        break;

                }

                int[] numbers_bit = BytesToBit(data);
                int[] numbers = BytesToUint16(data);

                switch (canid[2])
                {
                    case 0x1E:
                        Selectedllc_force_dsg = llc_force_dsgList[data[0] & 0x03];
                        Selectedllc_force_chg = llc_force_chgList[(data[0] >> 2) & 0x03];
                        SelectedLED_ctrl = LED_ctrlList[(data[0] >> 4) & 0x03];
                        Selectedhw_wdg_ctrl = hw_wdg_ctrlList[(data[0] >> 6) & 0x03];
                        if (data[6] == 0xAA) SelectedForceCtrlSwitch = Force_ctrl_switchList[0];
                        else SelectedForceCtrlSwitch = Force_ctrl_switchList[1];
                        break;
                    //Flash数据
                    case 0x1F:
                        if (data[0] == 0x00)
                        {
                            FlashData = Encoding.Default.GetString(data).Substring(1);
                        }
                        else
                        {
                            FlashData = "";
                        }
                        break;
                    //0x020:上位机控制
                    case 0x20:

                        SelectedRequest1 = Request1List[GetValue(0)];
                        SelectedRequest2 = Request2List[GetValue(1)];
                        SelectedRequest3 = Request3List[GetValue(2)];
                        SelectedRequest4 = Request4List[GetValue(3)];
                        SelectedRequest5 = Request5List[GetValue(4)];
                        SelectedRequest6 = Request6List[GetValue(5)];
                        SelectedRequest7 = Request7List[Convert.ToInt32(data[6]) - 1];

                        int GetValue(int index)
                        {
                            int value = 0;
                            switch (data[index])
                            {
                                case 0x00:
                                    value = 0;
                                    break;
                                case 0xAA:
                                    value = 1;
                                    break;
                                case 0x55:
                                    value = 2;
                                    break;
                                default:
                                    break;

                            }
                            //if (index == 4||index==5)
                            //{
                            //    switch (data[index])
                            //    {
                            //        case 0xAA:
                            //            value = 0;
                            //            break;
                            //        case 0x55:
                            //            value = 1;
                            //            break;
                            //        default:
                            //            break;
                            //    }
                            //}

                            return value;
                        }
                        break;
                    //参数标定0x1021XXE0 //混合（均衡-满充电压-加热膜）
                    case 0x21:
                        Bal_open_volt = numbers[0].ToString();
                        Bal_open_volt_diff = numbers[1].ToString();
                        Full_chg_volt = numbers[2].ToString();
                        Heating_film_open_temp = (numbers_bit[6] - 40).ToString();
                        HeatingHeating_film_close_temp = (numbers_bit[7] - 40).ToString();
                        break;
                    //参数标定0x1022XXE0 //电池包
                    case 0x22:
                        Pack_stop_volt = (numbers[0] * 0.1).ToString("F1");
                        Pack_stop_cur = (numbers[1] * 0.01).ToString("F2");
                        break;
                    //参数标定0x1023XXE0 
                    case 0x23:
                        Rated_capacity = (numbers[0] * 0.1).ToString();
                        Cell_Volt_Num = numbers[1].ToString();
                        Cell_temp_num = numbers[2].ToString();
                        break;
                    //参数标定0x1024XXE0 
                    case 0x24:
                        Cumulative_Chg_Capacity = (data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24)).ToString();
                        Cumulative_dsg_capacity = (data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24)).ToString();
                        break;
                    //参数标定0x1025XXE0 
                    case 0x25:
                        soc = (numbers[0] * 0.1).ToString();
                        Full_chg_capacity = (numbers[1] * 0.1).ToString("F1");
                        Surplus_capacity = (numbers[2] * 0.1).ToString("F1");
                        soh = (numbers[3] * 0.1).ToString();
                        break;
                    //时间标定0x1026FFE0
                    case 0x26:
                        SelectedDate = new DateTime(numbers_bit[0] + 2000, numbers_bit[1], numbers_bit[2]);
                        SelectedHour = numbers_bit[3].ToString("D2");
                        SelectedMinute = numbers_bit[4].ToString("D2");
                        SelectedSecond = numbers_bit[5].ToString("D2");
                        break;
                    //PACK_SN
                    case 0x27:
                        switch (data[0])
                        {
                            case 0:
                                bcuCode[0] = Encoding.Default.GetString(data).Substring(1);
                                break;
                            case 1:
                                bcuCode[1] = Encoding.Default.GetString(data).Substring(1);
                                break;
                            case 2:
                                bcuCode[2] = Encoding.Default.GetString(data).Substring(1);
                                PACK_SN = String.Join("", bcuCode).Trim();
                                bcuCode = new string[3];
                                break;
                                //default:
                                //    File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/data.log", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + data[0].ToString() + Environment.NewLine);
                                //    break;
                        }
                        break;
                    //BOARD_SN
                    case 0x28:
                        switch (data[0])
                        {
                            case 0:
                                boardCode[0] = Encoding.Default.GetString(data).Substring(1);
                                break;
                            case 1:
                                boardCode[1] = Encoding.Default.GetString(data).Substring(1);
                                break;
                            case 2:
                                boardCode[2] = Encoding.Default.GetString(data).Substring(1);
                                BOARD_SN = String.Join("", boardCode).Trim();
                                boardCode = new string[3];
                                break;
                                //default:
                                //    File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/data.log", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + data[0].ToString() + Environment.NewLine);
                                // break;
                        }
                        break;
                    //电池信息0x1029FFE0
                    case 0x29:
                        SelectedCell_brand = Cell_brandList[Convert.ToInt32(data[0]) - 1];
                        SelectedCell_model_num = Cell_model_numList[Convert.ToInt32(data[1]) - 1];
                        SelectedClr_FLASH_flag_func = Clr_FLASH_flag_funcList[Convert.ToInt32(data[2])];
                        SelectedQuery_FLASH_flag_func = Query_FLASH_flag_funcList[Convert.ToInt32(data[3])];
                        break;
                    case 0x2A:
                        switch (data[0])
                        {
                            case 0x01:
                                Cali_items_1 = ((data[1] + (data[2] << 8)) * 0.1).ToString("F1");
                                break;
                            case 0x02:
                                Cali_items_2 = ((data[1] + (data[2] << 8)) * 0.1).ToString("F1");
                                break;
                            case 0x03:
                                Cali_items_3 = ((data[1] + (data[2] << 8)) * 0.01).ToString("F2");
                                break;
                            case 0x04:
                                Cali_items_4 = ((data[1] + (data[2] << 8)) * 0.01).ToString("F2");
                                break;
                            case 0x05:
                                Cali_items_5 = ((data[1] + (data[2] << 8)) * 0.01).ToString("F2");
                                break;
                            case 0x06:
                                Cali_items_6 = ((data[1] + (data[2] << 8)) * 0.01).ToString("F2");
                                break;
                        }
                        break;
                    case 0x2B:
                        SelectedForce_sleep = Force_sleepList[data[0] & 0x03];
                        SelectedAlarm_protection_beep = Alarm_protection_beepList[(data[0] >> 2) & 0x03];
                        SelectedAlarm_protection_led = Alarm_protection_ledList[(data[0] >> 4) & 0x03];
                        SelectedHeat = HeatList[(data[0] >> 6) & 0x03];
                        SelectedClr_system_lock = Clr_system_lockList[data[1] & 0x03];

                        if (data[6] == 0x00) SelectedForce_ctrl_switch = Force_ctrl_switchList[0];
                        else if (data[6] == 0xAA) SelectedForce_ctrl_switch = Force_ctrl_switchList[1];

                        break;
                    case 0x2F:
                        Selectedchg_mos_ctrl = chg_mos_ctrlList[data[0] & 0x03];
                        Selecteddsg_mos_ctrl = dsg_mos_ctrlList[(data[0] >> 2) & 0x03];
                        Selectedpre_chg_mos_ctrl = pre_chg_mos_ctrlList[(data[0] >> 4) & 0x03];
                        Selectedheating_film_ctrl = heating_film_ctrlList[(data[0] >> 6) & 0x03];
                        Selectedforce_sleep = force_sleepList[data[1] & 0x03];
                        Selectedbeep_open = beep_openList[(data[1] >> 2) & 0x03];
                        Selectedchg_stop_ctrl = chg_stop_ctrlList[(data[1] >> 4) & 0x03];
                        Selectedheating_relay_drive_I0S = heating_relay_drive_I0SList[(data[1] >> 6) & 0x03];
                        SelectedID_OP_ctrl = ID_OP_ctrlList[data[2] & 0x03];
                        SelectedID_IN_state = ID_IN_stateList[(data[2] >> 2) & 0x03];
                        SelectedIDM_OP_ctrl = IDM_OP_ctrlList[(data[2] >> 4) & 0x03];
                        SelectedIDM_IN_state = IDM_IN_stateList[(data[2] >> 6) & 0x03];
                        force_bal_ctrl_1_8 = data[4].ToString("");
                        force_bal_ctrl_9_16 = data[5].ToString("");

                        if (data[6] == 0x00) Selectedforce_ctrl_switch = force_ctrl_switchList[0];
                        else if (data[6] == 0xAA) Selectedforce_ctrl_switch = force_ctrl_switchList[1];
                        break;

                }
            }
            catch (Exception)
            {

            }
        }

        private int[] BytesToBit(byte[] data)
        {
            int[] numbers = new int[8];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = Convert.ToInt32(data[i].ToString("X2"), 16);
            }
            return numbers;
        }
        int[] BytesToUint16(byte[] data)
        {
            int[] numbers = new int[4];
            for (int i = 0; i < data.Length; i += 2)
            {
                numbers[i / 2] = Convert.ToInt32(data[i + 1].ToString("X2") + data[i].ToString("X2"), 16);
            }
            return numbers;
        }

        public void PackageNumber_SelectedIndexChanged(int index)
        {
            int offset = 48 * (index - 1);

            PassiveEquilibriumState = $"电芯{1 + offset}~{48 + offset}的被动均衡状态";

            ChangeEquilibriumList(48, index);

        }

        public ICommand Write0x060_Cmd => new RelayCommand(Write0x060);
        public void Write0x060()
        {
            //byte[] can_id = new byte[4] { 0xE0, FrmMain.BMS_ID, 0x60, 0x10 };
            //byte[] can_id = new byte[4] { 0x81, 0x1F, 0x60, 0x10 };
            byte[] can_id = new byte[4] { 0xE0, Convert.ToByte(SelectedRequest7), 0x60, 0x10 };
            byte[] data = new byte[8];

            int current = Convert.ToInt32(PackCurrent);
            data[0] = (byte)(current & 0xff);

            data[1] = (byte)(current >> 8);

            int value = BatteryStatusList.IndexOf(SelectedBatteryStatus) & 0x03;

            if (FullChagreStatusList.IndexOf(SelectedFullChagreStatus) == 1)
            {
                value = value | 0x4;
            }
            if (FullDischargeStatusList.IndexOf(SelectedFullDischargeStatus) == 1)
            {
                value = value | 0x8;
            }
            if (ShutDownList.IndexOf(SelectedShutDown) == 1)
            {
                value = value | 0x10;
            }
            data[2] = Convert.ToByte(value);

            data[3] = Convert.ToByte(Convert.ToInt32(SyncFallSoc, 16));//低字节

            //byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x60, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            //byte[] crcData = new byte[11] { 0x81, 0x1F, 0x60, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            byte[] crcData = new byte[11] { 0xE0, Convert.ToByte(SelectedRequest7), 0x60, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };

            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);

        }

        public ICommand Write0x061_Cmd => new RelayCommand(Write0x061);
        public void Write0x061()
        {
            //byte[] can_id = new byte[4] { 0xE0, FrmMain.BMS_ID, 0x61, 0x10 };
            //byte[] can_id = new byte[4] { 0x81, 0x1F, 0x61, 0x10 };
            byte[] can_id = new byte[4] { 0xE0, Convert.ToByte(SelectedRequest7), 0x61, 0x10 };

            byte[] data = new byte[8];
            //主动均衡充电使能
            data[0] = (byte)Convert.ToByte(ActiveBalanceCtrlList.IndexOf(SelectedActiveBalanceCtrl));
            //主动均衡电流
            int current = Convert.ToInt32(Pack_Active_Balance_Cur);
            data[1] = (byte)(current & 0xff);
            data[2] = (byte)(current >> 8);
            //主动均衡容量
            int capacity = Convert.ToInt32(Pack_Active_Balance_Cap);
            data[3] = (byte)(capacity & 0xff);
            data[4] = (byte)(capacity >> 8);

            //byte[] crcData = new byte[11] { 0xE0, FrmMain.BMS_ID, 0x61, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            //byte[] crcData = new byte[11] { 0x81, 0x1F, 0x61, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            byte[] crcData = new byte[11] { 0xE0, Convert.ToByte(SelectedRequest7), 0x61, 0x10, data[0], data[1], data[2], data[3], data[4], data[5], data[6] };
            data[7] = (byte)(Crc8_8210_nBytesCalculate(crcData, 11, 0) & 0xff);

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        #region CRC校验
        /*******************************************************************************
        * Function Name  : Crc8_8210_nBytesCalculate
        * Description    : CRC校验,多项式为0x2F
        *******************************************************************************/
        public static uint Crc8_8210_nBytesCalculate(byte[] pBuff, uint bLen, uint bCrsMask)
        {
            uint i;
            for (int k = 0; k < bLen; k++)
            {
                for (i = 0x80; i > 0; i >>= 1)
                {
                    if (0 != (bCrsMask & 0x80))
                    {
                        bCrsMask <<= 1;
                        bCrsMask ^= 0x2F;
                    }
                    else
                    {
                        bCrsMask <<= 1;
                    }
                    if (0 != (pBuff[k] & i))
                    {
                        bCrsMask ^= 0x2F;
                    }
                }
            }
            return bCrsMask;
        }

        #endregion
        public ICommand WritePassiveEquilibriumState_Cmd => new RelayCommand(WritePassiveEquilibriumState);

        public void WritePassiveEquilibriumState()
        {
            //byte[] can_id = new byte[4] { 0x81, 0x1F, 0x63, 0x10 };
            byte[] can_id = new byte[4] { 0xE0, Convert.ToByte(SelectedRequest7), 0x63, 0x10 };

            byte[] data = new byte[8];

            data[0] = Convert.ToByte(MasterSlaveControlFlagList.IndexOf(SelectedMasterSlaveControlFlag));
            data[1] = Convert.ToByte(SelectedPackageNumber);

            for (int i = 0; i < PassiveEquilibriumCheckBoxItems.Count; i++)
            {
                int index = 2 + (i / 8);
                int bitIndex = i % 8;
                if (PassiveEquilibriumCheckBoxItems[i].IsChecked)
                {
                    data[index] |= (byte)(1 << bitIndex);
                }

            }

            if (baseCanHelper.Send(data, can_id)) MessageBoxHelper.Success("写入成功！", "提示", null, ButtonType.OK);
            else MessageBoxHelper.Warning("写入失败！", "提示", null, ButtonType.OK);
        }

        private string BytesToIntger(byte high, byte low = 0x00, double unit = 1)
        {
            string value = Convert.ToInt16(high.ToString("X2") + low.ToString("X2"), 16).ToString();
            if (unit == 10)
            {
                value = (long.Parse(value) * 10).ToString();
            }
            else if (unit == 0.1)
            {
                value = (long.Parse(value) / 10.0f).ToString();
            }
            else if (unit == 0.01)
            {
                value = (long.Parse(value) / 100.0f).ToString();
            }
            else if (unit == 0.001)
            {
                value = (long.Parse(value) / 1000.0f).ToString();
            }
            else if (unit == 0.0001)
            {
                value = (long.Parse(value) / 10000).ToString();
            }

            return value;
        }

        /// <summary>
        /// 获取字节中的指定Bit的值
        /// </summary>
        /// <param name="b">字节</param>
        /// <param name="index">Bit的索引值(0-7)</param>
        /// <returns></returns>
        public static int GetBit(byte b, short index)
        {
            byte _byte = 0x01;
            switch (index)
            {
                case 0: { _byte = 0x01; } break;
                case 1: { _byte = 0x02; } break;
                case 2: { _byte = 0x04; } break;
                case 3: { _byte = 0x08; } break;
                case 4: { _byte = 0x10; } break;
                case 5: { _byte = 0x20; } break;
                case 6: { _byte = 0x40; } break;
                case 7: { _byte = 0x80; } break;
                default: { return 0; }
            }
            int x = (b & _byte) == _byte ? 1 : 0;

            return (b & _byte) == _byte ? 1 : 0;
        }

        private string GetPackSN(byte[] data)
        {
            //条形码解析
            switch (data[0])
            {
                case 0:
                    packSN[0] = Encoding.Default.GetString(data).Substring(1);
                    break;
                case 1:
                    packSN[1] = Encoding.Default.GetString(data).Substring(1);
                    break;
                case 2:
                    packSN[2] = Encoding.Default.GetString(data).Substring(1);
                    break;
            }

            //判断sn是否接收完成
            if (packSN[0] != null && packSN[1] != null && packSN[2] != null)
            {
                string strSN = String.Join("", packSN);
                //return strSN.Substring(0, strSN.Length - 1);
                return strSN;//序列号为21位，无需去除最后一个字符
            }
            else
            {
                return string.Empty;
            }
        }

        private void AnalysisLog(byte[] data, int faultNum)
        {
            string[] msg = new string[2];
            string alarmLevel = "";
            for (int i = 0; i < data.Length; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    if (GetBit(data[i], j) == 1)
                    {
                        // 报警状态为激活
                        getLog(out msg, i, j, faultNum, 0);//state=0 未激活
                        //alarmLevel = Enum.Parse(typeof(Alarm_Level), (Convert.ToInt32(msg[1]) - 1).ToString()).ToString();
                        alarmLevel = ((Alarm_Level)(Convert.ToInt32(msg[1]) - 1)).ToString();

                        string StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        // 检查是否已有激活报警记录
                        if (msg[0] != null)
                        {
                            if (!AlarmMessageDataList.Any(x => x.AlarmMessage.Contains(msg[0]) && x.isEnd == "否"
                                && x.ID == SelectedRequest7.ToString()))
                            {
                                AlarmMessageDataList.Insert(0, new RealtimeData_BMS1500V_BMU.AlarmMessageData
                                {
                                    AlarmNumber = (AlarmMessageDataList.Count + 1).ToString(),
                                    AlarmStartTime = StartTime,
                                    AlarmLevel = alarmLevel,
                                    ID = SelectedRequest7.ToString(),
                                    AlarmMessage = msg[0],
                                    AlarmStatus = "【异常报警🚨】",
                                    isEnd = "否"
                                });
                            }
                        }
                    }
                    else
                    {
                        // 解除报警状态
                        getLog(out msg, i, j, faultNum, 1);//state=1 可解除
                        string StopTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        // 查找激活报警记录以确认解除
                        if (msg[0] != null)
                        {
                            var activeAlarm = AlarmMessageDataList.FirstOrDefault(x => x.AlarmMessage.Contains(msg[0]) && x.isEnd == "否");
                            if (activeAlarm != null)
                            {
                                activeAlarm.AlarmStopTime = StopTime;
                                activeAlarm.AlarmMessage = msg[0];
                                activeAlarm.AlarmStatus = "【报警解除🆗】";
                                activeAlarm.isEnd = "是";
                            }
                        }
                    }
                }
            }


            var alarmMessageDataList = AlarmMessageDataList.Where(x => x.isEnd == "否"
                && (x.AlarmLevel == "一般报警" || x.AlarmLevel == "轻微报警" || x.AlarmLevel == "严重报警" || x.AlarmLevel == "设备硬件故障")).ToList();

            model.Warning = "";
            model.Protection = "";
            model.Fault = "";
            model.Prompt = "";
            if (alarmMessageDataList.Any())
            {
                foreach (var alarmMessageData in alarmMessageDataList)
                {
                    switch (alarmMessageData.AlarmLevel)
                    {
                        case "告警":
                            model.Warning += alarmMessageData.AlarmMessage + ";";
                            break;
                        case "保护":
                            model.Protection += alarmMessageData.AlarmMessage + ";";
                            break;
                        case "故障":
                            model.Fault += alarmMessageData.AlarmMessage + ";";
                            break;
                        case "提示":
                            model.Prompt += alarmMessageData.AlarmMessage + ";";
                            break;
                    }
                }
            }

        }

        public string[] getLog(out string[] msg, int row, int column, int faultNum, int state)
        {
            msg = new string[2];
            List<FaultInfo> FaultInfos = new List<FaultInfo>();
            switch (faultNum)
            {
                case 1:
                    FaultInfos = FaultInfo.FaultInfos1;
                    break;
                case 2:
                    FaultInfos = FaultInfo.FaultInfos2;
                    break;
                case 3:
                    FaultInfos = FaultInfo.FaultInfos3;
                    break;
            }
            FaultInfo faultInfo = FaultInfos.FirstOrDefault(f => f.Byte == row && f.Bit == column && f.State == state);

            if (faultInfo != null)
            {
                msg[0] = $"{faultInfo.Content.Split(',')[0]}({faultInfo.Content.Split(',')[1]})";//中文(英文或者代码)
                //msg[0] = faultInfo.Content.Trim();  
                msg[1] = faultInfo.Type.ToString();
                faultInfo.State = state == 1 ? 0 : 1; // 更新状态

            }

            return msg;
        }

        public enum BMSState
        {
            开机自检 = 0,
            运行 = 1,
            故障 = 2,
            升级 = 3,
            关机 = 4
        }

        public enum Reset_Mode
        {
            LowPower复位 = 0,
            WWDG复位 = 1,
            IWDG复位 = 2,
            软件复位 = 3,
            上电或者掉电复位 = 4,
            Pin引脚复位 = 5,
            MMU复位 = 6
        }
        public enum Alarm_Level
        {
            告警 = 0,
            保护 = 1,
            故障 = 2,
            提示 = 3
        }
    }
}
