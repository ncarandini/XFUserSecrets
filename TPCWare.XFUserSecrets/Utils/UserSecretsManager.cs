using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace TPCWare.XFUserSecrets.Utils
{
    public class UserSecretsManager
    {
        private static UserSecretsManager _instance;
        private JObject _secrets;

        // Default namespace of the project
        private const string Namespace = "TPCWare.XFUserSecrets";

        // Filename of the embedded resource file
        private const string UserSecretsFileName = "secrets.json"; 

        private UserSecretsManager()
        {
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(UserSecretsManager)).Assembly;
                var stream = assembly.GetManifestResourceStream($"{Namespace}.{UserSecretsFileName}");
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    _secrets = JObject.Parse(json);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to load secrets file: {ex.Message}");
            }
        }

        public static UserSecretsManager Settings
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSecretsManager();
                }

                return _instance;
            }
        }

        public string this[string name]
        {
            get
            {
                try
                {
                    var path = name.Split(':');

                    JToken node = _secrets[path[0]];
                    for (int index = 1; index < path.Length; index++)
                    {
                        node = node[path[index]];
                    }

                    return node.ToString();
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Unable to retrieve secret '{name}'");
                    return string.Empty;
                }
            }
        }
    }
}
