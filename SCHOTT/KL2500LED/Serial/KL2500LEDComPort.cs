using System;
using System.Linq;
using SCHOTT.Core.Communication;
using SCHOTT.Core.Communication.Serial;
using SCHOTT.Core.Utilities;
using SCHOTT.KL2500LED.Communication;

namespace SCHOTT.KL2500LED.Serial
{
    /// <summary>
    /// KL2500LEDComPort extenstion class to add simplified connection methods for KL2500LED units.
    /// </summary>
    public class KL2500LEDComPort : ComPortBase
    {
        /// <summary>
        /// Protocol object to allow easy access of CVLS functions
        /// </summary>
        public Protocol Protocol { get; }

        /// <summary>
        /// Protocol object to allow easy access of CVLS functions, echoing all com traffic to the message function.
        /// </summary>
        public Protocol ProtocolEcho { get; }
        
        #region Initialization Functions

        /// <summary>
        /// Initialize a KL2500LEDComPort using the supplied parameters.
        /// </summary>
        /// <param name="portName">The port name to connect too.</param>
        /// <param name="portParameters">The parameters to use when setting up the KL2500LEDComPort</param>
        public KL2500LEDComPort(string portName, ComParameters portParameters) : base(portName, portParameters)
        {
            Protocol = new Protocol(this);
            ProtocolEcho = new Protocol(this, true);
        }

        #endregion

        #region External Functions
        
        #endregion

        #region Static Functions
        
        /// <summary>
        /// Find and connect to any KL2500LED unit.
        /// </summary>
        public static KL2500LEDComPort AutoConnectComPort()
        {
            var parameters = ComParameters();
            var comPorts = ComPortInfo.GetDescriptions();
            return comPorts.Any() ? AutoConnectComPort<KL2500LEDComPort>(comPorts.Select(p => p.Port).ToList(), parameters) : null;
        }

        /// <summary>
        /// Find and connect to any KL2500LED unit with the given parameters.
        /// </summary>
        /// <param name="portName">The port to connect to in format 'COM#'</param>
        public static KL2500LEDComPort AutoConnectComPort(string portName)
        {
            var parameters = ComParameters();
            var comPorts = ComPortInfo.GetDescriptions().Where(p => p.Port == portName).ToList();
            return comPorts.Any() ? AutoConnectComPort<KL2500LEDComPort>(comPorts.Select(p => p.Port).ToList(), parameters) : null;
        }

        /// <summary>
        /// Default ComParameters to use for KL2500LED units.
        /// </summary>
        /// <returns>new ComParameter object</returns>
        public static ComParameters ComParameters()
        {
            return new ComParameters { Command = "0ID?", ExpectedResponce = "0IDKL 2500 LED V", TerminationChar = ';', TimeoutMilliseconds = 250 };
        }

        #endregion

    }
}
