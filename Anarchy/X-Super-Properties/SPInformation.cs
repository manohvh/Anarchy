using System;
using System.Text;
using Newtonsoft.Json;

namespace Discord
{
    public class SPInformation
    {
        internal delegate void UpdatedHandler(object sender, SPUpdatedEventArgs args);
        internal event UpdatedHandler OnPropertiesUpdated;

        public string Base64
        {
            get
            {
                return Properties != null ? 
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Properties))) : null;
            }
            set
            {
                Properties = value != null ? 
                    Encoding.UTF8.GetString(Convert.FromBase64String(value)).Deserialize<SuperProperties>() : null;
            }
        }

        private SuperProperties _properties;
        public SuperProperties Properties
        {
            get { return _properties; }
            private set
            {
                _properties = value;
                OnPropertiesUpdated?.Invoke(this, new SPUpdatedEventArgs(this));
            }
        }


        public static implicit operator SuperProperties(SPInformation instance)
        {
            return instance.Properties;
        }
    }
}