using System;
using System.Collections.Generic;
using SCHOTT.Core.Communication;
using SCHOTT.Core.Communication.Serial;
using SCHOTT.Core.Extensions;
using SCHOTT.Core.Threading;
using SCHOTT.Core.Utilities;
using SCHOTT.KL2500LED.Communication;

namespace SCHOTT.KL2500LED.Serial
{
    /// <summary>
    /// KL2500LED Implimentation of a ThreadedComPortBase. This port type should be used when the unit connection
    /// state is unknown. Functions are provided to be notified of connections and status changes. When using
    /// a static configuration, consider using the KL2500LEDComPort classes instead.
    /// </summary>
    public class KL2500LEDThreadedComPort : ThreadedComPortBase
    {
        #region Function Overrides for Derived Class

        /// <summary>
        /// Allows access to the CurrentConnection
        /// </summary>
        public new KL2500LEDComPort CurrentConnection => (KL2500LEDComPort)base.CurrentConnection;
        
        /// <summary>
        /// AutoConnect function called by the KL2500LEDThreadedComPort class. A derived class can override this function
        /// to return a different derived type of KL2500LEDComPort for the connect function. This allows for extension of
        /// the KL2500LEDThreadedComPort.
        /// </summary>
        /// <param name="portsToCheck">Which ports to check for a connection.</param>
        /// <param name="portParameters">Which parameters to use when checking ports.</param>
        /// <returns></returns>
        protected override ComPortBase AutoConnectComPort(List<string> portsToCheck, ComParameters portParameters)
        {
            return ComPortBase.AutoConnectComPort<KL2500LEDComPort>(portsToCheck, portParameters);
        }

        #endregion

        #region Initialization Functions

        /// <summary>
        /// Create a KL2500LEDThreadedComPort for a KL2500LED unit. This port type should be used when the unit connection
        /// state is unknown. Functions are provided to be notified of connections and status changes. When using
        /// a static configuration, consider using the KL2500LEDComPort classes instead.
        /// </summary>
        /// <param name="threadName">Name of the thread.</param>
        /// <param name="closingWorker">The ClosingWorker to add this thread too</param>
        public KL2500LEDThreadedComPort(string threadName, ClosingWorker closingWorker)
            : base(threadName, closingWorker, KL2500LEDComPort.ComParameters(), ConnectionMode.AnyCom)
        {
        }

        /// <summary>
        /// Create a KL2500LEDThreadedComPort for a KL2500LED unit. This port type should be used when the unit connection
        /// state is unknown. Functions are provided to be notified of connections and status changes. When using
        /// a static configuration, consider using the KL2500LEDComPort classes instead.
        /// </summary>
        /// <param name="threadName">Name of the thread.</param>
        /// <param name="closingWorker">The ClosingWorker to add this thread too</param>
        /// <param name="portName">The port to connect to in format 'COM#'</param>
        public KL2500LEDThreadedComPort(string threadName, ClosingWorker closingWorker, string portName)
            : base(threadName, closingWorker, KL2500LEDComPort.ComParameters(), ConnectionMode.SelectionRule, port => port.Port == portName)
        {
        }

        #endregion

        #region External Functions

        /// <summary>
        /// Protocol object to allow easy access of CVLS functions.
        /// NOTE: This can return null when not connected to a CVLSComPort.
        /// </summary>
        public Protocol Protocol => CurrentConnection?.Protocol;

        /// <summary>
        /// Protocol object to allow easy access of CVLS functions, echoing all com traffic to the message function.
        /// NOTE: This can return null when not connected to a CVLSComPort.
        /// </summary>
        public Protocol ProtocolEcho => CurrentConnection?.ProtocolEcho;

        /// <summary>
        /// Change the connection method of the KL2500LEDThreadedComPort to select any KL2500LED.
        /// </summary>
        public void ChangeMode()
        {
            ConnectionParameters.CopyFrom(KL2500LEDComPort.ComParameters());
            base.ChangeMode(ConnectionMode.AnyCom);
        }

        /// <summary>
        /// Change the connection method of the KL2500LEDThreadedComPort.
        /// </summary>
        /// <param name="portName">The port to connect to in format 'COM#'</param>
        public void ChangeMode(string portName)
        {
            ConnectionParameters.CopyFrom(KL2500LEDComPort.ComParameters());
            base.ChangeMode(ConnectionMode.SelectionRule, port => port.Port == portName);
        }
        
        #endregion

    }
}
