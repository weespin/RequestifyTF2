using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestifyTF2.Utils
{
    // Code From https://github.com/aifdsc/AudioChanger
    // Thanks Stephan for your work
 static   class AudioDeviceChanger
    {

        private static bool setDefaultAudioDevice(string deviceId)
        {
            CPolicyConfigClient client = new CPolicyConfigClient();
            client.SetDefaultDevice(deviceId);
            CPolicyConfigVistaClient vclient = new CPolicyConfigVistaClient();
            vclient.SetDefaultDevice(deviceId);
            return true;
        }
        public static bool SetDefaultInputDevice(string deviceId)
        {
            bool result = setDefaultAudioDevice(deviceId);
            return result;
        }
    }
}
