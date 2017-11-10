using SCHOTT.Core.Communication;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SCHOTT.KL2500LED.Communication
{
    /// <summary>
    /// 
    /// </summary>
    public class Protocol
    {
        private readonly ITextProtocol _port;
        private readonly bool _echoComTraffic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">The port for the Protocol to use.</param>
        /// <param name="echoComTraffic">When True, the protocol object will echo com traffic to the subscribed message functions.</param>
        public Protocol(ITextProtocol port, bool echoComTraffic = false)
        {
            _port = port;
            _echoComTraffic = echoComTraffic;
        }

        /// <summary>
        /// Process the Firmware Version of the currently connected unit.
        /// </summary>
        public string FirmwareVersion => !_port.SendCommandSingleTest("0ID?;", "0ID", out string workingLine, true) ? string.Empty : workingLine;

        /// <summary>
        /// Process the Firmware Version of the currently connected unit.
        /// </summary>
        public string ProtocolVersion => !_port.SendCommandSingleTest("0PV?;", "0PV", out string workingLine, true) ? string.Empty : workingLine;

        /// <summary>
        /// Process the Firmware Version of the currently connected unit.
        /// </summary>
        public double OutputPower
        {
            get
            {
                if (!_port.SendCommandSingleTest("0BR?", "0BR", out string workingLine, true))
                    return Double.NaN;

                if (!int.TryParse(workingLine, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int parsedResult))
                    return Double.NaN;

                return (parsedResult * 0.1);
            }
            set => _port.SendCommandSingle($"0BR{Math.Min(Math.Max((int)(value * 10.0), 0), 1000):x4}", echoComTraffic: _echoComTraffic);
        }

        /// <summary>
        /// 
        /// </summary>
        public enum SettingsLocations
        {
#pragma warning disable 1591
            PowerUp,
            M1,
            M2,
            M3,
            M4,
#pragma warning restore 1591
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SaveSettings(SettingsLocations location)
        {
            var command = $"0PS{(int) location + 1:0000}";
            return _port.SendCommandSingleTest(command, command, out string workingLine, echoComTraffic: _echoComTraffic);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool RestoreSettings(SettingsLocations location)
        {
            var command = $"0PR{(int)location + 1:0000}";
            return _port.SendCommandSingleTest(command, command, out string workingLine, echoComTraffic: _echoComTraffic);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShutterEnable
        {
            get
            {
                if (!_port.SendCommandSingleTest("0SH?", "0SH", out string workingLine, true))
                    return false;
                
                return workingLine == "0001";
            }
            set => _port.SendCommandSingle($"0SH{(value ? 1 : 0):0000}", echoComTraffic: _echoComTraffic);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool LockEnable
        {
            get
            {
                if (!_port.SendCommandSingleTest("0LK?", "0LK", out string workingLine, true))
                    return false;

                return workingLine == "0001";
            }
            set => _port.SendCommandSingle($"0LK{(value ? 1 : 0):0000}", echoComTraffic: _echoComTraffic);
        }

        /// <summary>
        /// 
        /// </summary>
        public enum RemoteTypes
        {
#pragma warning disable 1591
            Momentary,
            Toggle,
#pragma warning restore 1591
        }

        /// <summary>
        /// 
        /// </summary>
        public RemoteTypes RemoteType
        {
            get
            {
                var command = $"0SF?";
                var workingLine = _port.SendCommandSingle(command, true, prefixToRemove: "0SF",
                    echoComTraffic: _echoComTraffic);
                return (RemoteTypes)Enum.Parse(typeof(RemoteTypes), workingLine);
            }
            set
            {
                var command = $"0SF{(int)value:0000}";
                var result = _port.SendCommandSingleTest(command, command, out string workingLine, echoComTraffic: _echoComTraffic);
            }
        }

        /// <summary>
        /// Process the Firmware Version of the currently connected unit.
        /// </summary>
        public double TemperatureCelsius
        {
            get
            {
                if (!_port.SendCommandSingleTest("0TX?", "0TX", out string workingLine, true))
                    return Double.NaN;

                if (!int.TryParse(workingLine, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int parsedResult))
                    return Double.NaN;

                return parsedResult * 0.0625 - 273.15;
            }
        }

    }
}
