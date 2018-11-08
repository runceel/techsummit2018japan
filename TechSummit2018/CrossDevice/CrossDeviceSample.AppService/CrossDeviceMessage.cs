using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace CrossDeviceSample.AppService
{
    class CrossDeviceMessage
    {
        // remote or local
        public string Command { get; set; }
        public string Value { get; set; }
        public CrossDeviceMessage()
        {

        }

        public CrossDeviceMessage(ValueSet message)
        {
            if (message.TryGetValue("command", out var command))
            {
                Command = command as string;
            }
            if (message.TryGetValue("value", out var value))
            {
                Value = value as string;
            }
        }

        public ValueSet ToValueSet()
        {
            var message = new ValueSet();
            message.Add("command", Command);
            message.Add("value", Value);
            return message;
        }
    }
}
