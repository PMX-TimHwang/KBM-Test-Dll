using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace Height {
    public class HeightObject {
        [JsonProperty("Height_Info")]
        public Dictionary<string, HeightLimit> HeightInfos { get; set; }
    }

    public class HeightLimit {
        [JsonProperty("Upper_limit")]
        public double UpperLimit { get; set; }
        [JsonProperty("Lower_limit")]
        public double LowerLimit { get; set; }
        [JsonProperty("Upper_tolerance")]
        public double UpperTolerance { get; set; }
        [JsonProperty("Lower_tolerance")]
        public double LowerTolerance { get; set; }

        [JsonConstructor]
        public HeightLimit(double lower, double upper) {
            UpperLimit = upper;
            LowerLimit = lower;
            UpperTolerance = 0.0;
            LowerTolerance = 0.0;
        }
        public HeightLimit(double lower, double upper, double lowerTolerance, double upperTolerance) {
            UpperLimit = upper;
            LowerLimit = lower;
            UpperTolerance = upperTolerance;
            LowerTolerance = lowerTolerance;
        }
    }
}
