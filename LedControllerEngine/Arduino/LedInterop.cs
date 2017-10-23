using LedControllerEngine.Assets;
using LedControllerEngine.Assets.Effects;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace LedControllerEngine.Arduino
{
    public class LedInterop : IDisposable
    {
        private SerialPort serialPortChannel;

        /// <summary>
        /// Initializes a new instance of the <see cref="LedInterop"/> class.
        /// </summary>
        /// <param name="serialPort">The serial port.</param>
        public LedInterop(string serialPort) : this(serialPort, 115200) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LedInterop"/> class.
        /// </summary>
        /// <param name="serialPort">The serial port.</param>
        /// <param name="speed">The speed.</param>
        public LedInterop(string serialPort, int speed)
        {
            serialPortChannel = new SerialPort(serialPort, speed);
        }

        /// <summary>
        /// Sends the settings.
        /// </summary>
        /// <param name="effectId">The effect identifier.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="devices">The devices.</param>
        /// <param name="mode">The mode.</param>
        public void SendSettings(IEnumerable<EffectSetting> settings, IEnumerable<int> devices, TransferMode mode)
        {
            IEnumerable<string> commands = BuildCommands(settings, devices, mode);

            if (!serialPortChannel.IsOpen)
            {
                serialPortChannel.Open();
            }

            foreach (var command in commands)
            {
                serialPortChannel.Write(command);
            }
            serialPortChannel.WriteLine("");
        }

        /// <summary>
        /// Builds the commands.
        /// </summary>
        /// <param name="effectId">The effect identifier.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="devices">The devices.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        private IEnumerable<string> BuildCommands(IEnumerable<EffectSetting> settings, IEnumerable<int> devices, TransferMode mode)
        {
            string commandSymbol = (mode == TransferMode.Live)
                ? ">"
                : "^";

            var commandPattern = string.Join(",", settings.Select(s => $"{commandSymbol}" + "{0}." + $"{s.Code}.{s.Value}"));

            List<string> commands = new List<string>();
            foreach (var device in devices)
            {
                commands.Add(string.Format(commandPattern, device));
            }

            return commands;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (serialPortChannel.IsOpen)
            {
                serialPortChannel.Close();
            }

            serialPortChannel.Dispose();
        }
    }
}
