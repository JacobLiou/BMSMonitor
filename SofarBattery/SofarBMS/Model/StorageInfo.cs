using SofarBMS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SofarBMS.Model
{

    public class StorageInfo
    {
        public string OccurredDate
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell1");


        public string SerialNumber
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell2");


        public string FaultByte1
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell3");


        public string FaultByte2
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell4");


        public string FaultByte3
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell5");


        public string FaultByte4
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell6");


        public string FaultByte5
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell7");


        public string FaultByte6
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell8");


        public string FaultByte7
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell9");


        public string FaultByte8
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell10");


        public string BatteryStatus
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell11");


        public string ChargingMOS
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell12");


        public string DischargeMOS
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell13");


        public string PreChargedMOS
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell14");


        public string ChargingEmergencyStop
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell15");


        public string HeatedMOS
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell16");


        public string BatteryVoltage
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell17");


        public string LoadVoltage
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell18");


        public string BatteryCurrent
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell19");


        public string SOC
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell20");


        public string SOH
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell21");


        public string ChargeCurrentLimit
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell22");


        public string DischargeCurrentLimit
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell23");


        public string CumulativeChargingCapacity
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell24");


        public string Cumulativedischargecapacity
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell25");


        public string Maximumcellvoltage
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell26");


        public string Maximumcellvoltagenumber
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell27");


        public string Minimumcellvoltage
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell28");


        public string Minimumcellvoltagenumber
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell29");


        public string Voltage1
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell30");


        public string Voltage2
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell31");


        public string Voltage3
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell32");


        public string Voltage4
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell33");


        public string Voltage5
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell34");


        public string Voltage6
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell35");


        public string Voltage7
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell36");


        public string Voltage8
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell37");


        public string Voltage9
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell38");


        public string Voltage10
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell39");


        public string Voltage11
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell40");


        public string Voltage12
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell41");


        public string Voltage13
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell42");


        public string Voltage14
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell43");


        public string Voltage15
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell44");


        public string Voltage16
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell45");


        public string AmbientTemperature
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell46");


        public string MosTemperature
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell47");


        public string Maximumcelltemperature
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell48");


        public string Maximumcelltemperaturenumber
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell49");


        public string Minimumcelltemperature
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell50");


        public string Minimumcelltemperaturenumber
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell51");


        public string Temperature1
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell52");


        public string Temperature2
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell53");


        public string Temperature3
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell54");


        public string Temperature4
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell55");


        public string Temperature5
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell56");


        public string Temperature6
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell57");


        public string Temperature7
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell58");


        public string Temperature8
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell59");


        public string RemainingCapacity
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell60");


        public string FullChargeCapacity
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell61");


        public string EquilibriumState
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell62");


        public string EquilibriumTemperature1
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell63");


        public string EquilibriumTemperature2
        {
            get;
            set;
        } = LanguageHelper.GetString("Cell64");

    }
}
