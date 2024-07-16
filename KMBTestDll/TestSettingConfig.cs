using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Alignment;
using Slant;
using Base;

namespace TestSetting {
    public class TestSettingConfig {
        private TestSettingObject testSetting;

        #region Test Info
        private string modelName;
        public string ModelName {
            get { return testSetting.TestInfo.ModelName; }
            set { testSetting.TestInfo.ModelName = value; }
        }

        private int keyCount;
        public int KeyCount {
            get { return keyCount; }
            private set { keyCount = value; }
        }

        private int roiWidth;
        public int RoiWidth {
            get { return roiWidth; }
            set { roiWidth = value; }
        }
        private int roiHeight;
        public int RoiHeight {
            get { return roiHeight; }
            set { roiHeight = value; }
        }

        private double baseSpec;
        public double BaseSpec {
            get { return baseSpec; }
            set { baseSpec = value; }
        }

        private string[] slantSpecStr;
        private double[] slantSpec = new double[2];
        public double[] SlantSpec {
            get { return slantSpec; }
            private set { slantSpec = value; }
        }

        private int alignmentFindRange;
        public int AlignmentFindRange {
            get { return alignmentFindRange; }
            set { alignmentFindRange = value; }
        }

        private string[] alignmentSpecStr;
        private double[] alignmentSpec = new double[2];
        public double[] AlignmentSpec {
            get { return alignmentSpec; }
            private set { alignmentSpec = value; }
        }

        private string[] heightSpecStr;
        private double[] heightSpec = new double[2];
        public double[] HeightSpec {
            get { return heightSpec; }
            private set { heightSpec = value; }
        }

        private double heightTolerance;
        public double HeightTolerance {
            get { return heightTolerance; }
            set { heightTolerance = value; }
        }

        private string[] maxMinSpecStr;
        private double[] maxMinSpec = new double[2];
        public double[] MaxMinSpec {
            get { return maxMinSpec; }
            set { maxMinSpec = value;
            }
        }

        private string[] maxMinToleranceStr;
        private double[] maxMinTolerance = new double[2];
        public double[] MaxMinTolerance {
            get { return maxMinTolerance; }
            set { maxMinTolerance = value;
            }
        }
        #endregion

        #region Keys Info
        private string[] keysName;
        public string[] KeysName {
            get {
                return keysName;
            }
            private set {
                keysName = value;
            }
        }

        private ROIRange[][] keysRoi;
        public ROIRange[][] KeysRoi {
            get {
                return keysRoi;
            }
            private set {
                keysRoi = value;
            }
        }
        #endregion

        public TestSettingConfig(string modelName, int keyCount, int roiWide, int roiHeight, int alignmentFindRange) {
            testSetting = new TestSettingObject();

            testSetting.TestInfo = new TestInfo(modelName, keyCount, roiWide, roiHeight, alignmentFindRange);
            testSetting.KeysInfo = new Dictionary<string, KeyInfo>();
            
            keysName = new string[keyCount];
            for (int keyNum = 1; keyNum <= keyCount; keyNum++) {
                KeyInfo keyInfo = new KeyInfo();
                keyInfo.Name = keyNum.ToString();
                keyInfo.Rois = new List<string>();
                testSetting.KeysInfo.Add(keyNum.ToString(), keyInfo);
                keysName[keyNum - 1] = keyInfo.Name;
            }
            parsing();
        }

        public TestSettingConfig(string filePath) {
            string jsonContect = File.ReadAllText(filePath);
            testSetting = JsonConvert.DeserializeObject<TestSettingObject>(jsonContect);
            parsing();
        }

        private void parsing() {
            //Test Info
            ModelName = testSetting.TestInfo.ModelName;
            keyCount = testSetting.TestInfo.KeyCount;
            roiWidth = testSetting.TestInfo.RoiWidth;
            roiHeight = testSetting.TestInfo.RoiHeight;

            baseSpec = testSetting.TestInfo.BaseSpec;

            slantSpecStr = testSetting.TestInfo.SlantSpec.Split(',');
            slantSpec[0] = Convert.ToDouble(slantSpecStr[0]);
            slantSpec[1] = Convert.ToDouble(slantSpecStr[1]);

            alignmentFindRange = testSetting.TestInfo.AlignmentFindRange;

            alignmentSpecStr = testSetting.TestInfo.AlignmentSpec.Split(',');
            alignmentSpec[0] = Convert.ToDouble(alignmentSpecStr[0]);
            alignmentSpec[1] = Convert.ToDouble(alignmentSpecStr[1]);

            heightSpecStr = testSetting.TestInfo.HeightSpec.Split(',');
            heightSpec[0] = Convert.ToDouble(heightSpecStr[0]);
            heightSpec[1] = Convert.ToDouble(heightSpecStr[1]);
            heightTolerance = testSetting.TestInfo.HeightTolerance;

            maxMinSpecStr = testSetting.TestInfo.SpaceMaxminSpec.Split(',');
            maxMinSpec[0] = Convert.ToDouble(maxMinSpecStr[0]);
            maxMinSpec[1] = Convert.ToDouble(maxMinSpecStr[1]);
            maxMinToleranceStr = testSetting.TestInfo.SpaceMaxminTolerance.Split(',');
            maxMinTolerance[0] = Convert.ToDouble(maxMinToleranceStr[0]);
            maxMinTolerance[1] = Convert.ToDouble(maxMinToleranceStr[1]);
            //Keys Info
            SplitNameRoiFromTestSetting();
        }

        private void SplitNameRoiFromTestSetting() {
            KeyInfo thisKeyInfo = new KeyInfo();
            keysName = new string[keyCount];
            keysRoi = new ROIRange[keyCount][];

            for (int keyIndex = 0; keyIndex < keyCount; keyIndex++) {
                string keyNum = (keyIndex + 1).ToString();
                thisKeyInfo = testSetting.KeysInfo[keyNum];

                keysName[keyIndex] = thisKeyInfo.Name;
                keysRoi[keyIndex] = new ROIRange[thisKeyInfo.Rois.Count];
                for (int j = 0; j < thisKeyInfo.Rois.Count; j++)
                    keysRoi[keyIndex][j] = new ROIRange(thisKeyInfo.Rois[j], roiWidth, roiHeight);
            }
        }


        public void SetRoiSize(int roiWide, int roiHeight) {
            this.roiWidth = roiWide;
            testSetting.TestInfo.RoiWidth = roiWide;
            this.roiHeight = roiHeight;
            testSetting.TestInfo.RoiHeight = roiHeight;
        }
        public void SetAlignmentFindRange(int newdistance) {
            alignmentFindRange = newdistance;
            testSetting.TestInfo.AlignmentFindRange = newdistance;
        }

        public void SetMaxMinSpec(double lower, double upper) {
            testSetting.TestInfo.SpaceMaxminSpec = String.Format("{0},{1}", lower, upper);
        }
        public void SetMaxMinTolerance(double lower, double upper) {
            testSetting.TestInfo.SpaceMaxminTolerance = String.Format("{0},{1}", lower, upper);
        }


        public KeyInfo GetKeyInfo(int keyNum) {
            if (testSetting.KeysInfo.ContainsKey(keyNum.ToString())) {
                return testSetting.KeysInfo[keyNum.ToString()];
            }
            return null;
        }

        public void SetKeyInfo(int keyNum, string name, Point[] rois) {
            if (testSetting.KeysInfo.ContainsKey(keyNum.ToString())) {
                testSetting.KeysInfo[keyNum.ToString()].Name = name;
                testSetting.KeysInfo[keyNum.ToString()].Rois = new List<string>();
                keysName[keyNum - 1] = name;
                keysRoi[keyNum - 1] = new ROIRange[rois.Length];
                int roiIndex = 0;
                foreach (Point roi in rois) {
                    string strLeftTop = roi.X.ToString() + "," + roi.Y.ToString();
                    testSetting.KeysInfo[keyNum.ToString()].Rois.Add(strLeftTop);
                    ROIRange roiRane = new ROIRange(strLeftTop, roiWidth, roiHeight);
                    keysRoi[keyNum - 1][roiIndex] = roiRane;
                    roiIndex++;
                }
            }
        }

        public void AddKeyInfo(string name, Point[] rois) {
            int keysCount = testSetting.KeysInfo.Count;
            KeyInfo newKeyInfo = new KeyInfo();
            newKeyInfo.Name = name;
            newKeyInfo.Rois = new List<string>();
            foreach (Point roi in rois){
                string strLeftTop = roi.X.ToString() + "," + roi.Y.ToString();
                newKeyInfo.Rois.Add(strLeftTop);
            }

            int newKeyNum = keysCount + 1;
            testSetting.KeysInfo.Add(newKeyNum.ToString(), newKeyInfo);
            testSetting.TestInfo.KeyCount = testSetting.KeysInfo.Count;
            keyCount = testSetting.TestInfo.KeyCount;
            SplitNameRoiFromTestSetting();
        }

        public void AddKeyInfo(string name, ROIRange[] rois) {
            int keysCount = testSetting.KeysInfo.Count;
            KeyInfo newKeyInfo = new KeyInfo();
            newKeyInfo.Name = name;
            newKeyInfo.Rois = new List<string>();
            List<string> roiList = new List<string>();
            foreach (ROIRange roi in rois) {
                string strLeftTop = roi.LeftTop.X.ToString() + "," + roi.LeftTop.Y.ToString();
                newKeyInfo.Rois.Add(strLeftTop);
            }

            int newKeyNum = keysCount + 1;
            testSetting.KeysInfo.Add(newKeyNum.ToString(), newKeyInfo);
            testSetting.TestInfo.KeyCount = testSetting.KeysInfo.Count;
            keyCount = testSetting.TestInfo.KeyCount;
            SplitNameRoiFromTestSetting();
        }

        public void RemoveKeyInfo(int numIndex) {
            keysName = keysName.Where((val, idx) => idx != numIndex).ToArray();

            ROIRange[][] newKeysRoi = new ROIRange[keyCount - 1][];
            int offset = 0;
            for (int keyIndex = 0; keyIndex < keyCount; keyIndex++) {
                if (numIndex == keyIndex) {
                    offset = 1;
                    continue;
                }
                newKeysRoi[keyIndex - offset] = keysRoi[keyIndex];
            }
            keysRoi = new ROIRange[keyCount - 1][];
            keysRoi = newKeysRoi;
            keyCount = keysName.Count();
        }
        public void RemoveKeyInfo(string name) {
            keysName = keysName.Concat(new string[] { name }).ToArray();
            int numIndex = Array.IndexOf(keysName, name);

            keysName = keysName.Where((val, idx) => idx != numIndex).ToArray();

            ROIRange[][] newKeysRoi = new ROIRange[keyCount - 1][];

            for (int keyIndex = 0; keyIndex < keyCount; keyIndex++) {
                if (numIndex == keyIndex)
                    continue;
                newKeysRoi[keyIndex] = keysRoi[keyIndex];
            }
            keysRoi = new ROIRange[keyCount - 1][];
            keysRoi = newKeysRoi;
            keyCount = keysName.Count();    
        }
        
        public void RefreshKeysInfo(){
            testSetting.KeysInfo = new Dictionary<string, KeyInfo>();
            for (int keyIndex = 0; keyIndex < keyCount; keyIndex++) {
                KeyInfo keyInfo = new KeyInfo();

                //reset key name
                keyInfo.Name = keysName[keyIndex];
                
                //reset key rois
                keyInfo.Rois = new List<string>();
                ROIRange[] roiRanges = keysRoi[keyIndex];
                foreach (ROIRange roirange in roiRanges) {
                    string roi = roirange.LeftTop.Y.ToString() + "," + roirange.LeftTop.X.ToString();
                    keyInfo.Rois.Add(roi);
                }
                testSetting.KeysInfo.Add((keyIndex+1).ToString(), keyInfo);
            }
            testSetting.TestInfo.KeyCount = keyCount;
        }

        public void SaveSetting(string filePath) {
            string settingJson = JsonConvert.SerializeObject(testSetting);
            using (StreamWriter writer = new StreamWriter(filePath, false)) {
                writer.Write(settingJson);
            }
        }
    }
    public class ROIRange {
        private Point leftTop;
        public Point LeftTop {
            get {
                return leftTop;
            }
            set {
                leftTop = value;
            }
        }
        private Point rightBottom;
        public Point RightBottom {
            get {
                return rightBottom;
            }
            set {
                rightBottom = value;
            }
        }
        public ROIRange(string strLeftTop, int roiWidgh, int roiHeight) {
            string[] strLeftTopPoint = strLeftTop.Split(',');
            //setting  橫為X，直為Y
            leftTop = new Point(Convert.ToInt32(strLeftTopPoint[1]), Convert.ToInt32(strLeftTopPoint[0]));
            rightBottom = leftTop + new Size(roiHeight, roiWidgh);
        }
    }
}
