using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace Base {
    public class BaseObject {
        [JsonProperty("BaseInfo")]
        //<keyName, keysInfo>
        public Dictionary<string, BaseRefer> KeysInfo { get; set; }
    }
    public class BaseRefer {
        [JsonProperty("Base_value")]
        public double BaseValue { get; set; }
        [JsonProperty("Refered_key")]
        public int ReferedKey { get; set; }
        [JsonProperty("Roi")]
        public List<string> Rois { get; set; }
                
        [JsonConstructor]
        public BaseRefer(double baseValue, int referedKey) {
            BaseValue = baseValue;
            ReferedKey = referedKey;
            Rois = new List<string>();
        }
    }
}
