using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Drawing;
using TestSetting;

namespace Base {
    public class BaseConfig{

        private BaseObject baseSetting;
        public BaseObject BaseSetting {
            get {
                return baseSetting;
            }
            private set {
                baseSetting = value;
            }
        }
        
        private TestSettingConfig testSetting;

        public BaseConfig(string filePath, TestSettingConfig testSetting) {
            this.testSetting = testSetting;
            string jsonContect = File.ReadAllText(filePath);
            baseSetting = JsonConvert.DeserializeObject<BaseObject>(jsonContect);
        }

        public BaseConfig(TestSettingConfig testSetting) {
            this.testSetting = testSetting;
            baseSetting = new BaseObject();
            baseSetting.KeysInfo = new Dictionary<string, BaseRefer>();
            int keyCount = testSetting.KeyCount;
            for (int keyNum = 1; keyNum <= keyCount; keyNum++) {
                string keyName = testSetting.KeysName[keyNum - 1];
                baseSetting.KeysInfo.Add(keyName, new BaseRefer(0, keyNum));
                CreateRoiFromConfig(keyNum);
            }
        }

        private void CreateRoiFromConfig(int keyNum) {
            string keyName = testSetting.KeysName[keyNum - 1];
            AddRois(keyName, testSetting.KeysRoi[keyNum - 1]);

            int roiCount = testSetting.KeysRoi[keyNum - 1].Count();
            ROIRange[] rois = testSetting.KeysRoi[keyNum - 1];
            Point midLT = new Point();
            if (roiCount == 4)
                midLT = new Point((int)(rois[0].LeftTop.X + rois[2].LeftTop.X) / 2, (int)(rois[0].LeftTop.Y + rois[2].LeftTop.Y) / 2);
            else if (roiCount == 6)
                midLT = new Point((int)(rois[0].LeftTop.X + rois[3].LeftTop.X) / 2, (int)(rois[0].LeftTop.Y + rois[2].LeftTop.Y) / 2);
            string strLeftTop = midLT.X.ToString() + "," + midLT.Y.ToString();
            baseSetting.KeysInfo[keyName].Rois.Add(strLeftTop);
        }

        public void AddBaseValue(string keyName, double baseValue, ROIRange[] rois) {
            testSetting.AddKeyInfo(keyName, rois);
            int keyNum = testSetting.KeyCount;

            baseSetting.KeysInfo.Add(keyName, new BaseRefer(0, keyNum));
            CreateRoiFromConfig(keyNum);
        }
                
        public void AddRois(string keyName, ROIRange[] rois) {
            int keyCount = testSetting.KeyCount;
            foreach (ROIRange roi in rois) {
                string strLeftTop = roi.LeftTop.X.ToString() + "," + roi.LeftTop.Y.ToString();
                baseSetting.KeysInfo[keyName].Rois.Add(strLeftTop);
            }
        }

        public double GetBaseValue(int keyNum) {
            string keyName = testSetting.KeysName[keyNum - 1];
            int refer = baseSetting.KeysInfo[keyName].ReferedKey;
            keyName = testSetting.KeysName[refer - 1];
            return baseSetting.KeysInfo[keyName].BaseValue;
        }
        public double GetBaseValue(string keyName) {
            int refer = baseSetting.KeysInfo[keyName].ReferedKey;
            keyName = testSetting.KeysName[refer - 1];
            return baseSetting.KeysInfo[keyName].BaseValue;
        }

        public int GetRefer(int keyNum) {
            string keyName = testSetting.KeysName[keyNum - 1];
            return baseSetting.KeysInfo[keyName].ReferedKey;
        }
        public int GetRefer(string keyName) {
            return baseSetting.KeysInfo[keyName].ReferedKey;
        }

        public void SetBaseValue(int keyNum, double baseValue) {
            string keyName = testSetting.KeysName[keyNum - 1];
            baseSetting.KeysInfo[keyName].BaseValue = baseValue;
        }
        public void SetBaseValue(string keyName, double baseValue) {
            baseSetting.KeysInfo[keyName].BaseValue = baseValue;
        }

        public void SetReferTo(int keyNum, int referKey) {
            string keyName = testSetting.KeysName[keyNum - 1];
            baseSetting.KeysInfo[keyName].ReferedKey = referKey;
        }
        public void SetReferTo(string keyName, int referKey) {
            baseSetting.KeysInfo[keyName].ReferedKey = referKey;
        }

        public void RenameKeyName(string oldKeyName, string newKeyName) {
            baseSetting.KeysInfo.Add(newKeyName, baseSetting.KeysInfo[oldKeyName]);
            baseSetting.KeysInfo.Remove(oldKeyName);
            var l = baseSetting.KeysInfo.OrderBy(key => key.Key);
            baseSetting.KeysInfo = l.ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
        }

        public void SaveBaseSetting(string filePath) {
            string settingJson = JsonConvert.SerializeObject(baseSetting);
            using (StreamWriter writer = new StreamWriter(filePath, false)) {
                writer.Write(settingJson);
            }
        }
    }
}
