using Newtonsoft.Json.Linq;

namespace Discord
{
    public class ControllableEx : Controllable
    {
        public delegate void JsonHandler(object sender, JObject json);
        public event JsonHandler JsonUpdated;

        private JObject _json;
        internal JObject Json
        {
            get
            {
                return _json;
            }
            set 
            {
                _json = value;

                JsonUpdated?.Invoke(this, value);
            }
        }
    }
}
