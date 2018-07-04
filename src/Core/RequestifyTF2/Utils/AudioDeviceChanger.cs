namespace RequestifyTF2.Utils
{
    // Code From https://github.com/aifdsc/AudioChanger
    // Thanks Stephan for your work
    internal static class AudioDeviceChanger
    {
        private static bool setDefaultAudioDevice(string deviceId)
        {
            var client = new CPolicyConfigClient();
            client.SetDefaultDevice(deviceId);
            var vclient = new CPolicyConfigVistaClient();
            vclient.SetDefaultDevice(deviceId);
            return true;
        }

        public static bool SetDefaultInputDevice(string deviceId)
        {
            var result = setDefaultAudioDevice(deviceId);
            return result;
        }
    }
}