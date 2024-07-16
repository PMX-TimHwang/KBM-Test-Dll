using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using KBMTestDll;
using TestSetting;

namespace Slant {
    public class SlantConfig {
        private SlantObject slantSetting;
        public SlantObject SlantSetting {
            get {
                return slantSetting;
            }
            private set {
                slantSetting = value;
            }
        }

        private TestSettingConfig testSetting;
                
        /// <summary>
        /// read exist json file
        /// </summary>
        /// <param name="filePath"></param>
        public SlantConfig(string filePath, TestSettingConfig testSetting) {
            this.testSetting = testSetting;
            string jsonContect = File.ReadAllText(filePath);
            slantSetting = JsonConvert.DeserializeObject<SlantObject>(jsonContect);
        }

        /// <summary>
        /// create slantObject from config file automatically
        /// </summary>
        /// <param name="testSetting"></param>
        public SlantConfig(TestSettingConfig testSetting) {
            this.testSetting = testSetting;
            slantSetting = new SlantObject();
            CreateSlantSetting();
        }
        private void CreateSlantSetting() {
            int keyCount = testSetting.KeyCount;
            slantSetting.SlantInfos = new Dictionary<string, RoiPairs>();
            for (int keyNum = 1; keyNum <= keyCount; keyNum++) {
                int roiCount = testSetting.KeysRoi[keyNum - 1].Count();
                string keyName = testSetting.KeysName[keyNum - 1];
                slantSetting.SlantInfos.Add(keyName, CreateRoiPairFromConfig(roiCount));
            }
        }

        public void ChangeSlantSetting(int keyNum, int roiCount) {
            int keyCount = testSetting.KeyCount;
            string keyName = testSetting.KeysName[keyNum - 1];
            slantSetting.SlantInfos[keyName] = CreateRoiPairFromConfig(roiCount);
        }

        public RoiPairs CreateRoiPairFromConfig(int roiCount) {
            RoiPairs roiPairs = new RoiPairs();
            roiPairs.RoiPair = new Dictionary<string, SlantLimit>();
            if (roiCount == 4) {
                roiPairs.RoiPair.Add("1#3", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
                roiPairs.RoiPair.Add("1#4", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
                roiPairs.RoiPair.Add("2#3", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
                roiPairs.RoiPair.Add("2#4", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
                roiPairs.RoiPair.Add("1#2", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
                roiPairs.RoiPair.Add("4#3", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
            }
            else if (roiCount == 6) {
                roiPairs.RoiPair.Add("1#2", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
                roiPairs.RoiPair.Add("3#2", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
                roiPairs.RoiPair.Add("4#5", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
                roiPairs.RoiPair.Add("6#5", new SlantLimit(testSetting.SlantSpec[0], testSetting.SlantSpec[1]));
            }
            return roiPairs;
        }

        public SlantLimit GetLimit(string keyName, string roiName) {
            return slantSetting.SlantInfos[keyName].RoiPair[roiName];
        }
        public SlantLimit GetLimit(int keyNum, int roiNum1, int roiNum2) {
            string keyName = testSetting.KeysName[keyNum - 1];
            string roiName = GetPairName(roiNum1, roiNum2);
            return slantSetting.SlantInfos[keyName].RoiPair[roiName];
        }

        public bool SetLimit(string keyName, string roiName, double lowerLimit, double upperLimit) {
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName)) {
                slantSetting.SlantInfos[keyName].RoiPair[roiName].LowerLimit = lowerLimit;
                slantSetting.SlantInfos[keyName].RoiPair[roiName].UpperLimit = upperLimit;
                return true;
            } else
                AddRoiPair(keyName, roiName, upperLimit, lowerLimit);
            return false;
        }
        public bool SetLimit(int keyNum, int roiNum1, int roiNum2, double lowerLimit, double upperLimit) {
            string keyName = testSetting.KeysName[keyNum - 1];
            string roiName = GetPairName(roiNum1, roiNum2);
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName)) {
                slantSetting.SlantInfos[keyName].RoiPair[roiName].LowerLimit = lowerLimit;
                slantSetting.SlantInfos[keyName].RoiPair[roiName].UpperLimit = upperLimit;
                return true;
            } else
                AddRoiPair(keyName, roiName, upperLimit, lowerLimit);
            return false;
        }
        public bool SetTolerance(string keyName, string roiName, double lowerTolerance, double upperTolerance) {
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName)) {
                slantSetting.SlantInfos[keyName].RoiPair[roiName].LowerTolerance = lowerTolerance;
                slantSetting.SlantInfos[keyName].RoiPair[roiName].UpperTolerance = upperTolerance;
                return true;
            } else
                return false;
        }
        public bool SetTolerance(int keyNum, int roiNum1, int roiNum2, double lowerTolerance, double upperTolerance) {
            string keyName = testSetting.KeysName[keyNum - 1];
            string roiName = GetPairName(roiNum1, roiNum2);
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName)) {
                slantSetting.SlantInfos[keyName].RoiPair[roiName].LowerTolerance = lowerTolerance;
                slantSetting.SlantInfos[keyName].RoiPair[roiName].UpperTolerance = upperTolerance;
                return true;
            } else
                return false;
        }

        public void AddRoiPair(string keyName, string roiName, double lowerLimit, double upperLimit) {
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName))
                SetLimit(keyName, roiName, lowerLimit, upperLimit);
            else
                slantSetting.SlantInfos[keyName].RoiPair.Add(roiName, new SlantLimit(lowerLimit, upperLimit));
        }
        public void AddRoiPair(int keyNum, int roiNum1, int roiNum2, double lowerLimit, double upperLimit) {
            string keyName = testSetting.KeysName[keyNum - 1];
            string roiName = GetPairName(roiNum1, roiNum2);
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName))
                SetLimit(keyName, roiName, lowerLimit, upperLimit);
            else
                slantSetting.SlantInfos[keyName].RoiPair.Add(roiName, new SlantLimit(lowerLimit, upperLimit));
        }
        public void AddRoiPair(string keyName, string roiName, double lowerLimit, double upperLimit, double lowerTolerance, double upperTolerance) {
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName)) {
                SetLimit(keyName, roiName, lowerLimit, upperLimit);
                SetTolerance(keyName, roiName, lowerTolerance, upperTolerance);
            } else
                slantSetting.SlantInfos[keyName].RoiPair.Add(roiName, new SlantLimit(lowerLimit, upperLimit, lowerTolerance, upperTolerance));
        }
        public void AddRoiPair(int keyNum, int roiNum1, int roiNum2, double lowerLimit, double upperLimit, double lowerTolerance, double upperTolerance) {
            string keyName = testSetting.KeysName[keyNum - 1];
            string roiName = GetPairName(roiNum1, roiNum2);
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName)) {
                SetLimit(keyName, roiName, lowerLimit, upperLimit);
                SetTolerance(keyName, roiName, lowerTolerance, upperTolerance);
            } else
                slantSetting.SlantInfos[keyName].RoiPair.Add(roiName, new SlantLimit(lowerLimit, upperLimit, lowerTolerance, upperTolerance));
        }

        public void RemoveRoiPair(string keyName) {
            slantSetting.SlantInfos.Remove(keyName);
        }
        public void RemoveRoiPair(string keyName, string roiName) {
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName))
                slantSetting.SlantInfos[keyName].RoiPair.Remove(roiName);
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName))
                slantSetting.SlantInfos[keyName].RoiPair.Remove(roiName);
        }
        public void RemoveRoiPair(int keyNum, int roiNum1, int roiNum2) {
            string keyName = testSetting.KeysName[keyNum - 1];
            string roiName = GetPairName(roiNum1, roiNum2);
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName))
                slantSetting.SlantInfos[keyName].RoiPair.Remove(roiName);
            if (slantSetting.SlantInfos[keyName].RoiPair.ContainsKey(roiName))
                slantSetting.SlantInfos[keyName].RoiPair.Remove(roiName);
        }

        public void RenameKeyName(string oldKeyName, string newKeyName) {
            SlantObject tempSlantSetting = new SlantObject();
            tempSlantSetting.SlantInfos = new Dictionary<string, RoiPairs>();
            int itemCount = slantSetting.SlantInfos.Count;
            string thisKeyName;
            RoiPairs thisPair;
            for (int index = 0; index < itemCount; index++ ) {
                thisKeyName = slantSetting.SlantInfos.Keys.ToArray()[index];
                thisPair = slantSetting.SlantInfos.Values.ToArray()[index];
                if (thisKeyName == oldKeyName)
                    tempSlantSetting.SlantInfos.Add(newKeyName, thisPair);
                else
                    tempSlantSetting.SlantInfos.Add(thisKeyName, thisPair);
            }
            slantSetting = tempSlantSetting;
        }

        public void SaveSlantSetting(string filePath) {
            string settingJson = JsonConvert.SerializeObject(slantSetting);
            using (StreamWriter writer = new StreamWriter(filePath, false)) {
                writer.Write(settingJson);
            }
        }

        public string GetPairName(int roiNum1, int roiNum2) {
            return String.Format("{0}#{1}", roiNum1, roiNum2);
        }
        public int[] GetPairRoiNum(string roiName) {
            int[] result = new int[2];
            string[] splitRoiName = roiName.Split('#');
            result[0] = Convert.ToInt16(splitRoiName[0]);
            result[1] = Convert.ToInt16(splitRoiName[1]);
            return result;
        }
    }
}
