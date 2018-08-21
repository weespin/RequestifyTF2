using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RequestifyTF2.API.Permission
{
   public static class  PermissionCfg
    {
        public static void Save()
        {
            File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/permcfg.json", JsonConvert.SerializeObject(Permissions._users));
        }

        public static void Load()
        {
            if (Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/config/"))
            {
                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "/config/permcfg.json"))
                {
                    Permissions._users = JsonConvert.DeserializeObject<Dictionary<string,Group>>(
                        File.ReadAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json"));
                    
                }
                else
                {
                    File.WriteAllText(
                        Path.GetDirectoryName(Application.ExecutablePath) + "/config/config.json",
                        JsonConvert.SerializeObject(Permissions._users));
                }
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) + "/config/");
                File.WriteAllText(Path.GetDirectoryName(Application.ExecutablePath) + "/config/permcfg.json", JsonConvert.SerializeObject(Permissions._users));
             
            }
        }
        
    }
}
