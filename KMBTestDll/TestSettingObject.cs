using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace TestSetting {
    public class TestSettingObject {
        [JsonProperty("TestInfo")]
        public TestInfo TestInfo { get; set; }
        [JsonProperty("KeysInfo")]
        public Dictionary<string, KeyInfo> KeysInfo { get; set; }
    }

    public class TestInfo {
        [JsonProperty("Model_name")]
        public string ModelName { get; set; }
        [JsonProperty("Key_count")]
        public int KeyCount { get; set; }
        [JsonProperty("Roi_w")]
        public int RoiWidth { get; set; }
        [JsonProperty("Roi_h")]
        public int RoiHeight { get; set; }
        [JsonProperty("Base_spec")]
        public double BaseSpec { get; set; }
        [JsonProperty("Slant_spec")]
        public string SlantSpec { get; set; }
        [JsonProperty("Alignment_find_range")]
        public int AlignmentFindRange { get; set; }
        [JsonProperty("Alignment_spec")]
        public string AlignmentSpec { get; set; }
        [JsonProperty("Height_spec")]
        public string HeightSpec { get; set; }
        [JsonProperty("Height_target")]
        public double HeightTarget { get; set; }
        [JsonProperty("Height_tolerance")]
        public double HeightTolerance { get; set; }
        [JsonProperty("Space_maxmin_spec")]
        public string SpaceMaxminSpec { get; set; }
        [JsonProperty("Space_maxmin_tolerance")]
        public string SpaceMaxminTolerance { get; set; }

        public TestInfo(string modelName, int keyCount, int roiWide, int roiHeight, int alignmentFindRange) {
            this.ModelName = modelName;
            this.KeyCount = keyCount;
            this.RoiWidth = roiWide;
            this.RoiHeight = roiHeight;
            BaseSpec = -0.5;
            SlantSpec = "-0.2,0.3";
            AlignmentFindRange = alignmentFindRange;
            AlignmentSpec = "-0.35,0.35";
            HeightSpec = "3.8,4.0";       // 2021.03.17 [James] Add for Key Height Function
            HeightTarget = 2.75;
            HeightTolerance = 1.0;
            SpaceMaxminSpec = "-0.2,0.3";
            SpaceMaxminTolerance = "-0.1,0.1";
        }
    }

    public class KeyInfo {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Roi")]
        public List<string> Rois { get; set; }
    }
}
