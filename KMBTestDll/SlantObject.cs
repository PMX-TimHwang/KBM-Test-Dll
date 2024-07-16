using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace Slant {
    public class SlantObject {
        [JsonProperty("Slant_info")]
        public Dictionary<string, RoiPairs> SlantInfos { get; set; }
    }

    public class RoiPairs {
        [JsonProperty("Roi_pairs")]
        public Dictionary<string, SlantLimit> RoiPair { get; set; }
    }

    public class SlantLimit {
        [JsonProperty("Upper_limit")]
        public double UpperLimit { get; set; }
        [JsonProperty("Lower_limit")]
        public double LowerLimit { get; set; }
        [JsonProperty("Upper_tolerance")]
        public double UpperTolerance { get; set; }
        [JsonProperty("Lower_tolerance")]
        public double LowerTolerance { get; set; }
        
        [JsonConstructor]
        public SlantLimit(double lower, double upper) {
            UpperLimit = upper;
            LowerLimit = lower;
            UpperTolerance = 0.0;
            LowerTolerance = 0.0;
        }
        public SlantLimit(double lower, double upper, double lowerTolerance, double upperTolerance) {
            UpperLimit = upper;
            LowerLimit = lower;
            UpperTolerance = upperTolerance;
            LowerTolerance = lowerTolerance;
        }
    }
}
