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

namespace Alignment {
    public class AlignmentConfig {
        private AlignmentObject alignmentSetting;
        public AlignmentObject AlignmentSetting {
            get {
                return alignmentSetting;
            }
            private set {
                alignmentSetting = value;
            }
        }
        private TestSettingConfig testSetting;
        
        /// <summary>
        /// read exist json file
        /// </summary>
        /// <param name="filePath"></param>
        public AlignmentConfig(string filePath, TestSettingConfig testSetting) {
            this.testSetting = testSetting;
            string jsonContect = File.ReadAllText(filePath);
            alignmentSetting = JsonConvert.DeserializeObject<AlignmentObject>(jsonContect);
        }

        /// <summary>
        /// create AlignmentObject from config file automatically
        /// </summary>
        /// <param name="testSetting"></param>
        public AlignmentConfig(TestSettingConfig testSetting) {
            this.testSetting = testSetting;
            alignmentSetting = new AlignmentObject();
            alignmentSetting.AlignmentInfos = new Dictionary<string, RoiPairs>();
            CreateAlignmentSetting();
        }
        private void CreateAlignmentSetting() {
            int keyCount = testSetting.KeyCount;

            for (int keyNum = 1; keyNum <= keyCount; keyNum++) {
                string keyName = testSetting.KeysName[keyNum - 1];
                int roiCount = testSetting.KeysRoi[keyNum - 1].Count();
                for (int roiNum = 1; roiNum <= roiCount; roiNum++) {
                    string roiName = String.Format("{0}#{1}", keyName, roiNum);
                    alignmentSetting.AlignmentInfos.Add(roiName, CreateRoiPairFromConfig(keyNum, roiNum));
                }
            }
        }
        public RoiPairs CreateRoiPairFromConfig(int thisKeyNum, int thisRoiNum) {
            int keyCount = testSetting.KeyCount;
            RoiPairs roiPairs = new RoiPairs();
            roiPairs.RoiPair = new Dictionary<string, AlignmentLimit>();
            for (int otherKeyNum = 1; otherKeyNum <= keyCount; otherKeyNum++) {
                if (thisKeyNum == otherKeyNum) continue;
                string keyName = testSetting.KeysName[otherKeyNum - 1];
                int roiCount = testSetting.KeysRoi[otherKeyNum - 1].Count();
                for (int roiNum = 1; roiNum <= roiCount; roiNum++) {
                    double distance = GetRoiDistance(testSetting.KeysRoi[thisKeyNum - 1][thisRoiNum - 1], testSetting.KeysRoi[otherKeyNum - 1][roiNum - 1]);
                    if (distance < testSetting.AlignmentFindRange) {
                        roiPairs.RoiPair.Add(String.Format("{0}#{1}", keyName, roiNum), new AlignmentLimit(testSetting.AlignmentSpec[0], testSetting.AlignmentSpec[1]));
                    }
                }
            }
            return roiPairs;
        }
        
        public void ChangeDistance(int newDistance) {
            int keyCount = testSetting.KeyCount;

            for (int keyNum = 1; keyNum <= keyCount; keyNum++) {
                string keyName = testSetting.KeysName[keyNum - 1];
                int roiCount = testSetting.KeysRoi[keyNum - 1].Count();
                for (int roiNum = 1; roiNum <= roiCount; roiNum++) {
                    string thisKeyRoiName = String.Format("{0}#{1}", keyName, roiNum);
                    alignmentSetting.AlignmentInfos[thisKeyRoiName] = GetCheckRoiPairFromNewDistance(keyNum, roiNum, newDistance);
                }
            }
            testSetting.SetAlignmentFindRange(newDistance);
        }

        private RoiPairs GetCheckRoiPairFromNewDistance(int thisKeyNum, int thisRoiNum, int newDistance) {
            int keyCount = testSetting.KeyCount;
            string thisKeyName = testSetting.KeysName[thisKeyNum - 1];
            string thisKeyRoiName = String.Format("{0}#{1}", thisKeyName, thisRoiNum);
            RoiPairs oldPairs = alignmentSetting.AlignmentInfos[thisKeyRoiName];
            RoiPairs newPairs = new RoiPairs();
            newPairs.RoiPair = new Dictionary<string, AlignmentLimit>();
            for (int otherKeyNum = 1; otherKeyNum <= keyCount; otherKeyNum++) {
                if (thisKeyNum == otherKeyNum) continue;
                string otherkeyName = testSetting.KeysName[otherKeyNum - 1];
                int roiCount = testSetting.KeysRoi[otherKeyNum - 1].Count();
                for (int roiNum = 1; roiNum <= roiCount; roiNum++) {
                    double distance = GetRoiDistance(testSetting.KeysRoi[thisKeyNum - 1][thisRoiNum - 1], testSetting.KeysRoi[otherKeyNum - 1][roiNum - 1]);
                    string otherKeyRoiName = String.Format("{0}#{1}", otherkeyName, roiNum);
                    if (distance < newDistance) {
                        newPairs.RoiPair.Add(otherKeyRoiName, new AlignmentLimit(testSetting.AlignmentSpec[0], testSetting.AlignmentSpec[1]));
                        if (oldPairs.RoiPair.Keys.Contains(otherKeyRoiName))
                            newPairs.RoiPair[otherKeyRoiName] = oldPairs.RoiPair[otherKeyRoiName];                            
                    }
                }
            }
            return newPairs;
        }

        public AlignmentLimit GetLimit(string thisKeyRoiName, string otherKeyRoiName) {
            return alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName];
        }
        public AlignmentLimit GetLimit(int thisKeyNum, int thisRoiNum, int otherKeyNum, int otherRoiNum) {
            string thisKeyRoiName = GetKeyRoiName(thisKeyNum, thisRoiNum);
            string otherKeyRoiName = GetKeyRoiName(otherKeyNum, otherRoiNum);
            return alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName];
        }

        public bool SetLimit(string thisKeyRoiName, string otherKeyRoiName, double lowerLimit, double upperLimit) {
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName)) {
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName].LowerLimit = lowerLimit;
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName].UpperLimit = upperLimit;
                return true;
            }
            return false;
        }
        public bool SetLimit(int thisKeyNum, int thisRoiNum, int otherKeyNum, int otherRoiNum, double lowerLimit, double upperLimit) {
            string thisKeyRoiName = GetKeyRoiName(thisKeyNum, thisRoiNum);
            string otherKeyRoiName = GetKeyRoiName(otherKeyNum, otherRoiNum);
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName)) {
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName].LowerLimit = lowerLimit;
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName].UpperLimit = upperLimit;
                return true;
            }
            return false;
        }
        public bool SetTolerance(string thisKeyRoiName, string otherKeyRoiName,  double lowTolerance, double upTolerance) {
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName)) {
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName].LowerTolerance = lowTolerance;
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName].UpperTolerance = upTolerance;
                return true;
            }
            return false;
        }
        public bool SetTolerance(int thisKeyNum, int thisRoiNum, int otherKeyNum, int otherRoiNum, double lowTolerance, double upTolerance) {
            string thisKeyRoiName = GetKeyRoiName(thisKeyNum, thisRoiNum);
            string otherKeyRoiName = GetKeyRoiName(otherKeyNum, otherRoiNum);
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName)) {
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName].LowerTolerance = lowTolerance;
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair[otherKeyRoiName].UpperTolerance = upTolerance;
                return true;
            }
            return false;
        }

        public void AddRoiPair(string thisKeyRoiName, string otherKeyRoiName, double upperLimit, double lowerLimit) {
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName))
                SetLimit(thisKeyRoiName, otherKeyRoiName, lowerLimit, upperLimit);
            else
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.Add(otherKeyRoiName, new AlignmentLimit(lowerLimit, upperLimit));
        }
        public void AddRoiPair(int thisKeyNum, int thisRoiNum, int otherKeyNum, int otherRoiNum, double upperLimit, double lowerLimit) {
            string thisKeyRoiName = GetKeyRoiName(thisKeyNum, thisRoiNum);
            string otherKeyRoiName = GetKeyRoiName(otherKeyNum, otherRoiNum);
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName))
                SetLimit(thisKeyRoiName, otherKeyRoiName, lowerLimit, upperLimit);
            else
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.Add(otherKeyRoiName, new AlignmentLimit(lowerLimit, upperLimit));
        }
        public void AddRoiPair(string thisKeyRoiName, string otherKeyRoiName, double upperLimit, double lowerLimit, double lowTolerance, double upTolerance) {
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName)) {
                SetLimit(thisKeyRoiName, otherKeyRoiName, lowerLimit, upperLimit);
                SetTolerance(thisKeyRoiName, otherKeyRoiName, lowTolerance, upTolerance);
            } else
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.Add(otherKeyRoiName, new AlignmentLimit(lowerLimit, upperLimit, lowTolerance, upTolerance));
        }
        public void AddRoiPair(int thisKeyNum, int thisRoiNum, int otherKeyNum, int otherRoiNum, double upperLimit, double lowerLimit, double lowTolerance, double upTolerance) {
            string thisKeyRoiName = GetKeyRoiName(thisKeyNum, thisRoiNum);
            string otherKeyRoiName = GetKeyRoiName(otherKeyNum, otherRoiNum);
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName)) {
                SetLimit(thisKeyRoiName, otherKeyRoiName, lowerLimit, upperLimit);
                SetTolerance(thisKeyRoiName, otherKeyRoiName, lowTolerance, upTolerance);
            } else
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.Add(otherKeyRoiName, new AlignmentLimit(lowerLimit, upperLimit, lowTolerance, upTolerance));
        }

        public void RemoveRoiPair(string thisKeyRoiName, string otherKeyRoiName) {
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName))
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.Remove(otherKeyRoiName);
            if (alignmentSetting.AlignmentInfos[otherKeyRoiName].RoiPair.ContainsKey(thisKeyRoiName))
                alignmentSetting.AlignmentInfos[otherKeyRoiName].RoiPair.Remove(thisKeyRoiName);
        }
        public void RemoveRoiPair(int thisKeyNum, int thisRoiNum, int otherKeyIndex, int otherRoiNum) {
            string thisKeyRoiName = GetKeyRoiName(thisKeyNum, thisRoiNum);
            string otherKeyRoiName = GetKeyRoiName(otherKeyIndex, otherRoiNum);
            if (alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.ContainsKey(otherKeyRoiName))
                alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.Remove(otherKeyRoiName);
            if (alignmentSetting.AlignmentInfos[otherKeyRoiName].RoiPair.ContainsKey(thisKeyRoiName))
                alignmentSetting.AlignmentInfos[otherKeyRoiName].RoiPair.Remove(thisKeyRoiName);
        }

        public void RenameKeyName(string oldKeyName, string newKeyName) {
            AlignmentObject tempAlignmentConfig  = new AlignmentObject();
            tempAlignmentConfig.AlignmentInfos = new Dictionary<string, RoiPairs>();
            int itemCount = alignmentSetting.AlignmentInfos.Count;
            string thisKeyRoiName;
            string thisKeyName;
            int thisRoiNum;
            RoiPairs thisPair;
            for (int keyIndex = 0; keyIndex < itemCount; keyIndex++) {
                thisKeyRoiName = alignmentSetting.AlignmentInfos.Keys.ToArray()[keyIndex];
                thisKeyName = GetKeyName(thisKeyRoiName);
                thisRoiNum = GetRoiNum(thisKeyRoiName);
                thisPair = alignmentSetting.AlignmentInfos.Values.ToArray()[keyIndex];
                if (thisKeyName == oldKeyName) {
                    string newKeyRoiName = String.Format("{0}#{1}", newKeyName, thisRoiNum);
                    tempAlignmentConfig.AlignmentInfos.Add(newKeyRoiName, thisPair);
                } else {
                    int pairsCount = thisPair.RoiPair.Count;
                    string otherKeyRoiName;
                    string otherKeyName;
                    int otherRoiNum;
                    RoiPairs otherPair = new RoiPairs();
                    otherPair.RoiPair = new Dictionary<string, AlignmentLimit>();
                    AlignmentLimit otherLimit;
                    for (int pairIndex = 0; pairIndex < pairsCount; pairIndex++) {
                        otherKeyRoiName = alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.Keys.ToArray()[pairIndex];
                        otherKeyName = GetKeyName(otherKeyRoiName);
                        otherRoiNum = GetRoiNum(otherKeyRoiName);
                        otherLimit = alignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair.Values.ToArray()[pairIndex];
                        if (otherKeyName == oldKeyName) {
                            string newKeyRoiName = String.Format("{0}#{1}", newKeyName, otherRoiNum);
                            otherPair.RoiPair.Add(newKeyRoiName, otherLimit);
                        } else
                            otherPair.RoiPair.Add(otherKeyRoiName, otherLimit);
                    }
                    tempAlignmentConfig.AlignmentInfos.Add(thisKeyRoiName, otherPair);
                }
            }
            alignmentSetting = tempAlignmentConfig;
        }

        public void SaveAlignmentSetting(string filePath) {
            string settingJson = JsonConvert.SerializeObject(alignmentSetting);
            using (StreamWriter writer = new StreamWriter(filePath, false)) {
                writer.Write(settingJson);
            }
        }

        public double GetRoiDistance(ROIRange roi1, ROIRange roi2) {
            return Math.Sqrt(Math.Pow((roi1.LeftTop.X - roi2.LeftTop.X), 2) + Math.Pow((roi1.LeftTop.Y - roi2.LeftTop.Y), 2));
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
