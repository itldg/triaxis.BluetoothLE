﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace triaxis.Xamarin.BluetoothLE
{
    /// <summary>
    /// Represents an active connection to a Bluetooth LE Peripheral
    /// </summary>
    public interface IPeripheralConnection
    {
        /// <summary>
        /// Disconnects from the peripheral
        /// </summary>
        Task DisconnectAsync();

        /// <summary>
        /// Retrieves all services provided by the peripheral
        /// </summary>
        Task<IList<IService>> GetServicesAsync();

        /// <summary>
        /// Requests an increased MTU (maximum transfer unit) from the connected peripheral
        /// </summary>
        /// <returns>MTU confirmed by the connected peripheral</returns>
        Task<int> RequestMaximumWriteAsync(int request);

        /// <summary>
        /// Waits until the connection with the device is idle (no outstanding writes pending)
        /// </summary>
        Task IdleAsync();

        /// <summary>
        /// Event called when the connection is closed
        /// </summary>
        event EventHandler<Exception> Closed;
    }
}
