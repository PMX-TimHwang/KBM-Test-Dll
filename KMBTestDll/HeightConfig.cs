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

namespace Height {
    public class HeightConfig {
        private HeightObject heightSetting;
        public HeightObject HeightSetting {
            get {
                return heightSetting;
            }
            private set {
                heightSetting = value;
            }
        }
        private TestSettingConfig testSetting;
        
        /// <summary>
        /// read exist json file
        /// </summary>
        /// <param name="filePath"></param>
        public HeightConfig(string filePath, TestSettingConfig testSetting) {
            this.testSetting = testSetting;
            string jsonContect = File.ReadAllText(filePath);
            heightSetting = JsonConvert.DeserializeObject<HeightObject>(jsonContect);
        }

        /// <summary>
        /// create AlignmentObject from config file automatically
        /// </summary>
        /// <param name="testSetting"></param>
        public HeightConfig(TestSettingConfig testSetting) {
            this.testSetting = testSetting;
            heightSetting = new HeightObject();
            heightSetting.HeightInfos = new Dictionary<string, HeightLimit>();
            CreateHeightSetting();
        }
        private void CreateHeightSetting() {
            int keyCount = testSetting.KeyCount;

            for (int keyNum = 1; keyNum <= keyCount; keyNum++) {
                string keyName = testSetting.KeysName[keyNum - 1];
                int roiCount = testSetting.KeysRoi[keyNum - 1].Count();
                for (int roiNum = 1; roiNum <= roiCount; roiNum++) {
                    string roiName = String.Format("{0}#{1}", keyName, roiNum);
                    heightSetting.HeightInfos.Add(roiName, new HeightLimit(testSetting.HeightSpec[0], testSetting.HeightSpec[1], -testSetting.HeightTolerance, testSetting.HeightTolerance));
                }
            }
        }

        public HeightLimit GetLimit(string keyRoiName) {
            return heightSetting.HeightInfos[keyRoiName];
        }
        public HeightLimit GetLimit(int keyNum, int roiNum) {
            string keyRoiName = GetKeyRoiName(keyNum, roiNum);
            return GetLimit(keyRoiName);
        }

        public bool SetLimit(string keyRoiName, double lowerLimit, double upperLimit) {
            if (heightSetting.HeightInfos.ContainsKey(keyRoiName)) {
                heightSetting.HeightInfos[keyRoiName].LowerLimit = lowerLimit;
                heightSetting.HeightInfos[keyRoiName].UpperLimit = upperLimit;
                return true;
            }
            return false;
        }
        public bool SetLimit(int tkeyNum, int roiNum, double lowerLimit, double upperLimit) {
            string keyRoiName = GetKeyRoiName(tkeyNum, roiNum);
            if (heightSetting.HeightInfos.ContainsKey(keyRoiName)) {
                heightSetting.HeightInfos[keyRoiName].LowerLimit = lowerLimit;
                heightSetting.HeightInfos[keyRoiName].UpperLimit = upperLimit;
                return true;
            }
            return false;
        }
        public bool SetTolerance(string keyRoiName, double lowTolerance, double upTolerance) {
            if (heightSetting.HeightInfos.ContainsKey(keyRoiName)) {
                heightSetting.HeightInfos[keyRoiName].LowerTolerance = lowTolerance;
                heightSetting.HeightInfos[keyRoiName].UpperTolerance = upTolerance;
                return true;
            }
            return false;
        }
        public bool SetTolerance(int keyNum, int roiNum, double lowTolerance, double upTolerance) {
            string keyRoiName = GetKeyRoiName(keyNum, roiNum);
            if (heightSetting.HeightInfos.ContainsKey(keyRoiName)) {
                heightSetting.HeightInfos[keyRoiName].LowerTolerance = lowTolerance;
                heightSetting.HeightInfos[keyRoiName].UpperTolerance = upTolerance;
                return true;
            }
            return false;
        }
        public void RenameKeyName(string oldKeyName, string newKeyName) {
            string[] keyRois = heightSetting.HeightInfos.Keys.ToArray();
            int itemCount = keyRois.Count();
            Dictionary<string, HeightLimit> tempHeightInfos = new Dictionary<string, HeightLimit>();

            for(int index = 0; index < itemCount; index++) {
                string oldKeyRoi = keyRois[index];
                if(oldKeyRoi.Contains(oldKeyName + "#")) {
                    string newKeyRoi = oldKeyRoi.Replace(oldKeyName + "#", newKeyName + "#");
                    tempHeightInfos.Add(newKeyRoi, heightSetting.HeightInfos[oldKeyRoi]);
                }
                else
                    tempHeightInfos.Add(oldKeyRoi, heightSetting.HeightInfos[oldKeyRoi]);
            }
            heightSetting.HeightInfos = tempHeightInfos;
        }

        public void SaveHeightSetting(string filePath) {
            string settingJson = JsonConvert.SerializeObject(heightSetting);
            using (StreamWriter writer = new StreamWriter(filePath, false)) {
                writer.Write(settingJson);
            }
        }
      
        public string GetKeyRoiName(int keyNum, int roiNum) {
            return String.Format("{0}#{1}", testSetting.KeysName[keyNum - 1], roiNum);
        }
        public int[] GetKeyRoiNum(string keyRoiName) {
            int[] result = new int[2];
            string[] splitKeyRoiName =  keyRoiName.Split('#');
            for (int keyNum = 1; keyNum <= testSetting.KeyCount; keyNum++) {
                if (splitKeyRoiName[0] == testSetting.KeysName[keyNum - 1]) {
                    result[0] = keyNum;
                    result[1] = Convert.ToInt32(splitKeyRoiName[1]);
                    break;
                }
            }
            return result;
        }
        public string GetKeyName(string keyRoiName) {
            string[] splitKeyRoiName = keyRoiName.Split('#');
            return splitKeyRoiName[0];
        }
        public int GetRoiNum(string keyRoiName) {
            int[] result = new int[2];
            string[] splitKeyRoiName = keyRoiName.Split('#');
            return Convert.ToInt32(splitKeyRoiName[1]);
        }


    }
}
