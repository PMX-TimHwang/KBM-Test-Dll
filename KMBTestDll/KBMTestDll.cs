using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using Alignment;
using Slant;
using Height;
using Base;
using TestSetting;
using Result;

namespace KBMTestDll {
    public class KBMTester {
        const int BmpHead = 66;
        const int DataWidth = 3200;
        int dataHeight;
        string AppFolder = System.IO.Directory.GetCurrentDirectory();
        public TestSettingConfig TestSetting;
        public SlantConfig slantSetting;
        public AlignmentConfig alignmentSetting;
        public string BaseFilePath;
        public BaseConfig baseSetting;
        public HeightConfig heightSetting;
        private string logFolder;
        private string deepLogPath;
        private string preSlantLogPath;
        private string preAlignmentLogPath;
        private string preHeightLogPath;
        private string newSlantLogPath;
        private string newAlignmentLogPath;
        private string newHeightLogPath;
        private string resultPath;
        private string space14TestLogPath;

        public ResultObject TestResult;
        private List<ResultValue> resultValue = new List<ResultValue>();

        #region General Function

        public string GetVersion() {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public string Initialize(string filePath, string alignmentFilePath, string slantFilePath, string baseFilePath, string heightFilePath) {
            TestSetting = new TestSettingConfig(filePath);

            if (!File.Exists(slantFilePath)) {
                slantSetting = new SlantConfig(TestSetting);
                slantSetting.SaveSlantSetting(slantFilePath);
            } else
                slantSetting = new SlantConfig(slantFilePath, TestSetting);

            if (!File.Exists(alignmentFilePath)) {
                alignmentSetting = new AlignmentConfig(TestSetting);
                alignmentSetting.SaveAlignmentSetting(alignmentFilePath);
            } else
                alignmentSetting = new AlignmentConfig(alignmentFilePath, TestSetting);

            if(!File.Exists(baseFilePath)) {
                baseSetting = new BaseConfig(TestSetting);
                baseSetting.SaveBaseSetting(baseFilePath);
                BaseFilePath = baseFilePath;
            }
            else {
                baseSetting = new BaseConfig(baseFilePath, TestSetting);
                BaseFilePath = baseFilePath;
            }

            if (!File.Exists(heightFilePath)) {
                heightSetting = new HeightConfig(TestSetting);
                heightSetting.SaveHeightSetting(heightFilePath);
            } else
                heightSetting = new HeightConfig(heightFilePath, TestSetting);

            TestResult = new ResultObject();

            logFolder = AppFolder + String.Format(@"\ZHight Log\{0}\", DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);
            return TestSetting.ModelName;
        }
        public int GetKeyCount() {
            return TestSetting.KeyCount;
        }
        public int GetRoiCount(int keyNum) {
            return TestSetting.KeysRoi[keyNum - 1].Length;
        }

        public Point GetRoiTopLeft(int keyNum, int roiNum) {
            return TestSetting.KeysRoi[keyNum - 1][roiNum - 1].LeftTop;
        }
        public Point GetRoiTopLeft(int keyNum, int roiNum, Size roiOffset) {
            return Point.Add(TestSetting.KeysRoi[keyNum - 1][roiNum - 1].LeftTop, roiOffset);
        }

        public Point GetRoiBottomRight(int keyNum, int roiNum) {
            return TestSetting.KeysRoi[keyNum - 1][roiNum - 1].RightBottom;
        }
        public Point GetRoiBottomRight(int keyNum, int roiNum, Size roiOffset) {
            return Point.Add(TestSetting.KeysRoi[keyNum - 1][roiNum - 1].RightBottom, roiOffset);
        }

        public double GetDeepthValue(Point point) {
            int index = (dataHeight - 1 - point.X) * DataWidth + point.Y;
            return Math.Round(deepthValue[index], 4, MidpointRounding.AwayFromZero);
        }
        public double GetDeepthValue(Point topLeft, Point bottomRight) {
            double sumValue = 0;
            double avgValue = 0;

            int count = 0;
            for (int i = topLeft.X; i < bottomRight.X; i++) {
                for (int j = topLeft.Y; j < bottomRight.Y; j++) {
                    count++;
                    sumValue += GetDeepthValue(new Point(i, j));
                }
            }
            avgValue = sumValue / count;
            return Math.Round(avgValue, 4, MidpointRounding.AwayFromZero);
        }
        public double GetDeepthValue(int keyNum, int roiNum) {
            double sumValue = 0;
            double avgValue = 0;
            Point topLeft, bottomRight;

            topLeft = GetRoiTopLeft(keyNum, roiNum);
            bottomRight = GetRoiBottomRight(keyNum, roiNum);

            int count = 0;
            for (int i = topLeft.X; i < bottomRight.X; i++) {
                for (int j = topLeft.Y; j < bottomRight.Y; j++) {
                    count++;
                    sumValue += GetDeepthValue(new Point(i, j));
                }
            }
            avgValue = sumValue / count;
            return Math.Round(avgValue, 4, MidpointRounding.AwayFromZero);
        }
        public double GetDeepthValue(int keyNum, int roiNum, Size roiOffset) {
            double sumValue = 0;
            double avgValue = 0;
            Point topLeft, bottomRight;

            topLeft = GetRoiTopLeft(keyNum, roiNum, roiOffset);
            bottomRight = GetRoiBottomRight(keyNum, roiNum, roiOffset);

            int count = 0;
            for (int i = topLeft.X; i < bottomRight.X; i++) {
                for (int j = topLeft.Y; j < bottomRight.Y; j++) {
                    count++;
                    sumValue += GetDeepthValue(new Point(i, j));
                }
            }
            avgValue = sumValue / count;
            return Math.Round(avgValue, 4, MidpointRounding.AwayFromZero);
        }

        public double GetHeightValue(int keyNum, int roiNum) {
            double sumValue = 0;
            double avgValue = 0;
            Point topLeft, bottomRight;

            topLeft = GetRoiTopLeft(keyNum, roiNum);
            bottomRight = GetRoiBottomRight(keyNum, roiNum);

            int count = 0;
            for (int i = topLeft.X; i < bottomRight.X; i++) {
                for (int j = topLeft.Y; j < bottomRight.Y; j++) {
                    count++;
                    sumValue += GetDeepthValue(new Point(i, j));
                }
            }
            avgValue = sumValue / count;
            avgValue -= baseSetting.GetBaseValue(keyNum);
            return Math.Round(avgValue, 4, MidpointRounding.AwayFromZero);
        }
        public double GetHeightValue(int keyNum, int roiNum, Size roiOffset) {
            double sumValue = 0;
            double avgValue = 0;
            Point topLeft, bottomRight;

            topLeft = GetRoiTopLeft(keyNum, roiNum, roiOffset);
            bottomRight = GetRoiBottomRight(keyNum, roiNum, roiOffset);

            int count = 0;
            for (int i = topLeft.X; i < bottomRight.X; i++) {
                for (int j = topLeft.Y; j < bottomRight.Y; j++) {
                    count++;
                    sumValue += GetDeepthValue(new Point(i, j));
                }
            }
            avgValue = sumValue / count;
            avgValue -= baseSetting.GetBaseValue(keyNum);
            return Math.Round(avgValue, 4, MidpointRounding.AwayFromZero);
        }

        public double GetRoiDeviation(Point roiTopLeft) {
            double sumValue = 0;
            double avgValue = 0;
            double devValue = 0;
            Point topLeft, bottomRight;
            topLeft = roiTopLeft;
            bottomRight = Point.Add(roiTopLeft, new Size(TestSetting.RoiHeight, TestSetting.RoiWidth));

            int count = 0;
            sumValue = 0;
            for (int i = topLeft.X - 1; i < bottomRight.X - 1; i++) {
                for (int j = topLeft.Y - 1; j < bottomRight.Y - 1; j++) {
                    count++;
                    Point point = new Point(i, j);
                    sumValue += GetDeepthValue(point);
                }
            }
            avgValue = sumValue / count;

            devValue = 0;
            for (int i = topLeft.X - 1; i < bottomRight.X - 1; i++)
                for (int j = topLeft.Y - 1; j < bottomRight.Y - 1; j++)
                    devValue += Math.Pow((GetDeepthValue(new Point(i, j)) - avgValue), (double)2);
            devValue = Math.Sqrt(devValue / count);
            return Math.Round(devValue, 4, MidpointRounding.AwayFromZero);
        }
        public double GetRoiDeviation(int keyNum, int roiNum) {
            double avgValue = 0;
            double devValue = 0;
            Point topLeft, bottomRight;

            avgValue = GetDeepthValue(keyNum, roiNum);

            int count = 0;
            topLeft = GetRoiTopLeft(keyNum, roiNum);
            bottomRight = GetRoiBottomRight(keyNum, roiNum);
            devValue = 0;
            for (int i = topLeft.X - 1; i < bottomRight.X - 1; i++)
                for (int j = topLeft.Y - 1; j < bottomRight.Y - 1; j++) {
                    count++;
                    devValue += Math.Pow((GetDeepthValue(new Point(i, j)) - avgValue), (double)2);
                }
            devValue = Math.Sqrt(devValue / count);
            return Math.Round(devValue, 4, MidpointRounding.AwayFromZero);
        }
        public double GetRoiDeviation(int keyNum, int roiNum, Size roiOffset) {
            double avgValue = 0;
            double devValue = 0;
            Point topLeft, bottomRight;

            avgValue = GetDeepthValue(keyNum, roiNum);

            int count = 0;
            topLeft = GetRoiTopLeft(keyNum, roiNum, roiOffset);
            bottomRight = GetRoiBottomRight(keyNum, roiNum, roiOffset);
            devValue = 0;
            for (int i = topLeft.X - 1; i < bottomRight.X - 1; i++)
                for (int j = topLeft.Y - 1; j < bottomRight.Y - 1; j++) {
                    count++;
                    devValue += Math.Pow((GetDeepthValue(new Point(i, j)) - avgValue), (double)2);
                }
            devValue = Math.Sqrt(devValue / count);
            return Math.Round(devValue, 4, MidpointRounding.AwayFromZero);
        }
        public double GetRoiDeviation(Point topLeft, Point bottomRight) {
            double sumValue = 0;
            double avgValue = 0;
            double devValue = 0;

            int count = 0;
            sumValue = 0;
            for (int i = topLeft.X - 1; i < bottomRight.X - 1; i++) {
                for (int j = topLeft.Y - 1; j < bottomRight.Y - 1; j++) {
                    count++;
                    sumValue += GetDeepthValue(new Point(i, j));
                }
            }
            avgValue = sumValue / count;

            devValue = 0;
            for (int i = topLeft.X - 1; i < bottomRight.X - 1; i++)
                for (int j = topLeft.Y - 1; j < bottomRight.Y - 1; j++) {
                    devValue += Math.Pow((GetDeepthValue(new Point(i, j)) - avgValue), (double)2);
                }
            devValue = Math.Sqrt(devValue / count);
            return Math.Round(devValue, 4, MidpointRounding.AwayFromZero);
        }

        private void WriteLog(string filePath, string content) {
            using (StreamWriter srOutFile = File.AppendText(filePath)) {
                srOutFile.WriteLine(content);
                srOutFile.Flush();
                srOutFile.Close();
            }
        }
        #endregion

        #region Create Key Module Setting
        public string Initialize(string filePath, string modelName, int keyCount, int roiWide, int roiHeight, int alignmentFindRange) {
            TestSetting = new TestSettingConfig(modelName, keyCount, roiWide, roiHeight, alignmentFindRange);
            SaveTestSetting(filePath);
            logFolder = AppFolder + @"\Log\";
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);
            return TestSetting.ModelName;
        }

        public Point[] Create4Rois(Point leftTop, Point rightBottom, int shrinkSize) {
            Point[] rois = new Point[4];
            rois[0] = new Point(leftTop.X + shrinkSize, leftTop.Y + shrinkSize);
            rois[1] = new Point(leftTop.X + shrinkSize, rightBottom.Y - shrinkSize - TestSetting.RoiHeight);
            rois[2] = new Point(rightBottom.X - shrinkSize - TestSetting.RoiWidth, rightBottom.Y - shrinkSize - TestSetting.RoiHeight);
            rois[3] = new Point(rightBottom.X - shrinkSize - TestSetting.RoiWidth, leftTop.Y + shrinkSize);
            return rois;
        }
        public Point[] Create5Rois(Point leftTop, Point rightBottom, int shrinkSize) {
            Point[] rois = new Point[5];
            rois[0] = new Point(leftTop.X + shrinkSize, leftTop.Y + shrinkSize);
            rois[1] = new Point(leftTop.X + shrinkSize, rightBottom.Y - shrinkSize - TestSetting.RoiHeight);
            rois[2] = new Point(rightBottom.X - shrinkSize - TestSetting.RoiWidth, rightBottom.Y - shrinkSize - TestSetting.RoiHeight);
            rois[3] = new Point(rightBottom.X - shrinkSize - TestSetting.RoiWidth, leftTop.Y + shrinkSize);
            rois[4] = new Point((int)((leftTop.X + rightBottom.X) / 2), (int)((leftTop.Y + rightBottom.Y) / 2));
            return rois;
        }
        public Point[] Create6Rois(Point leftTop, Point rightBottom, int shrinkSize) {
            Point[] rois = new Point[6];
            rois[0] = new Point(leftTop.X + shrinkSize, leftTop.Y + shrinkSize);
            rois[2] = new Point(leftTop.X + shrinkSize, rightBottom.Y - shrinkSize - TestSetting.RoiHeight);
            rois[3] = new Point(rightBottom.X - shrinkSize - TestSetting.RoiWidth, rightBottom.Y - shrinkSize - TestSetting.RoiHeight);
            rois[5] = new Point(rightBottom.X - shrinkSize - TestSetting.RoiWidth, leftTop.Y + shrinkSize);
            rois[1] = new Point((int)((rois[0].X + rois[2].X) / 2), (int)((rois[0].Y + rois[2].Y) / 2));
            rois[4] = new Point((int)((rois[3].X + rois[5].X) / 2), (int)((rois[3].Y + rois[5].Y) / 2));
            return rois;
        }
        public Point[] Create7Rois(Point leftTop, Point rightBottom, int shrinkSize) {
            Point[] rois = new Point[7];
            rois[0] = new Point(leftTop.X + shrinkSize, leftTop.Y + shrinkSize);
            rois[2] = new Point(leftTop.X + shrinkSize, rightBottom.Y - shrinkSize - TestSetting.RoiHeight);
            rois[3] = new Point(rightBottom.X - shrinkSize - TestSetting.RoiWidth, rightBottom.Y - shrinkSize - TestSetting.RoiHeight);
            rois[5] = new Point(rightBottom.X - shrinkSize - TestSetting.RoiWidth, leftTop.Y + shrinkSize);
            rois[1] = new Point((int)((rois[0].X + rois[2].X) / 2), (int)((rois[0].Y + rois[2].Y) / 2));
            rois[4] = new Point((int)((rois[3].X + rois[5].X) / 2), (int)((rois[3].Y + rois[5].Y) / 2));
            rois[7] = new Point((int)((rois[1].X + rois[4].X) / 2), (int)((rois[1].Y + rois[4].Y) / 2));
            return rois;
        }

        public double GetDeepthValue(Point[] points) {
            double sumValue = 0;
            double avgValue = 0;

            int count = 0;
            foreach (Point p in points) {
                for (int i = p.X; i < p.X + TestSetting.RoiWidth; i++) {
                    for (int j = p.Y; j < p.Y + TestSetting.RoiHeight; j++) {
                        count++;
                        sumValue += GetDeepthValue(new Point(i, j));
                    }
                }
            }
            avgValue = sumValue / count;
            return avgValue;
        }

        public void SetKeyInfo(int keyNum, string name, Point[] rois) {
            TestSetting.SetKeyInfo(keyNum, name, rois);
        }

        public void AddKeyInfo(string name, Point[] rois) {
            TestSetting.AddKeyInfo(name, rois);
        }

        public void SaveTestSetting(string filePath) {
            TestSetting.SaveSetting(filePath);
        }
        #endregion

        #region Alignment Function

        public void InitialAlignmentSetting(string filePath) {
            alignmentSetting = new AlignmentConfig(TestSetting);
            alignmentSetting.SaveAlignmentSetting(filePath);
        }

        public void ChangeAlignmentFindRange(int newdistance) {
            TestSetting.SetAlignmentFindRange(newdistance);
            alignmentSetting.ChangeDistance(newdistance);
        }

        public void SaveAlignmentSetting(string filePath) {
            alignmentSetting.SaveAlignmentSetting(filePath);
        }

        #endregion

        #region Slant Function
        public void InitailSlantSetting(string filePath) {
            slantSetting = new SlantConfig(TestSetting);
            slantSetting.SaveSlantSetting(filePath);
        }

        public void SaveSlantSetting(string filePath) {
            slantSetting.SaveSlantSetting(filePath);
        }

        public int[] GetPairRoiNum(string roiName) {
            return slantSetting.GetPairRoiNum(roiName);
        }

        #endregion

        #region Base setting
        BaseConfig virtualBaseConfig;
        
        private double[] baseValue;

        public void LoadBaseValue(string bmpDataPath) {
            // Read raw image into byte array
            MemoryStream memoryStream = new MemoryStream(800000);
            FileStream fs = new FileStream(bmpDataPath, FileMode.Open);
            fs.CopyTo(memoryStream);
            byteDataBuffer = memoryStream.ToArray();
            if(baseValue != null)
                baseValue = null;
            baseValue = new double[(byteDataBuffer.Length - BmpHead) / 2];
            dataHeight = baseValue.Length / DataWidth;
            for(int i = 0; i < baseValue.Length - 1; i++) {
                baseValue[i] = (((double)byteDataBuffer[2 * i + BmpHead + 1] * 256 + (double)byteDataBuffer[2 * i + BmpHead]) - 32768) * 8 / 1000;
            }
            fs.Close();
            memoryStream.Flush();
        }
        public void LoadBaseValue(List<ushort> dataValue) {
            if(baseValue != null)
                baseValue = null;
            baseValue = new double[dataValue.Count];
            dataHeight = baseValue.Length / DataWidth;
            for(int h = 0; h < dataHeight; h++) {
                for(int w = 0; w < DataWidth; w++) {
                    int index = (dataHeight - 1 - h) * DataWidth + w;
                    baseValue[index] = ((double)dataValue[h * DataWidth + w] - 32768) * 8 / 1000;
                }
            }
        }

        private double GetBaseHeight(int keyIndex) {
            int keyNum = keyIndex + 1;

            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
            double keyDeepValue = 0;
            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                double sumValue = 0;
                double avgValue = 0;
                Point topLeft, bottomRight;

                topLeft = GetRoiTopLeft(keyNum, roiNum);
                bottomRight = GetRoiBottomRight(keyNum, roiNum);

                int count = 0;
                for(int i = topLeft.X; i < bottomRight.X; i++) {
                    for(int j = topLeft.Y; j < bottomRight.Y; j++) {
                        count++;
                        int index = (dataHeight - 1 - i) * DataWidth + j;
                        sumValue += Math.Round(baseValue[index], 4, MidpointRounding.AwayFromZero);
                    }
                }
                avgValue = sumValue / count;
                keyDeepValue += Math.Round(avgValue, 4, MidpointRounding.AwayFromZero);
            }
            keyDeepValue = keyDeepValue / roiCount;
            return keyDeepValue;
        }

        public void RefreshBaseFile(string bmpDataPath) {
            LoadBaseValue(bmpDataPath);
            virtualBaseConfig = baseSetting;

            int keyCount = TestSetting.KeyCount;
            for(int keyIndex = 0; keyIndex < keyCount; keyIndex++) {
                double keyDeepValue = GetBaseHeight(keyIndex);
                virtualBaseConfig.SetBaseValue(TestSetting.KeysName[keyIndex], keyDeepValue);
            }
            virtualBaseConfig.SaveBaseSetting(BaseFilePath);
        }

        public void RefreshBaseFile(List<ushort> dataValue) {
            LoadBaseValue(dataValue);
            virtualBaseConfig = baseSetting;

            int keyCount = TestSetting.KeyCount;
            for(int keyIndex = 0; keyIndex < keyCount; keyIndex++) {
                double keyDeepValue = GetBaseHeight(keyIndex);
                virtualBaseConfig.SetBaseValue(TestSetting.KeysName[keyIndex], keyDeepValue);
            }
        }

        public void InitialBaseSetting(string filePath) {
            baseSetting = new BaseConfig(TestSetting);
            baseSetting.SaveBaseSetting(filePath);
        }

        public double GetBase(int keyNum) {
            return baseSetting.GetBaseValue(keyNum);
        }
        public void SetBase(int keyNum, double baseValue) {
            baseSetting.SetBaseValue(keyNum.ToString(), baseValue);
        }
        public void SetBase(string keyName, double baseValue) {
            baseSetting.SetBaseValue(keyName, baseValue);
        }
        public void SaveBaseSetting(string filePath) {
            baseSetting.SaveBaseSetting(filePath);
        }
        #endregion

        #region Height setting
        public void InitialHeightSetting(string filePath) {
            heightSetting = new HeightConfig(TestSetting);
            heightSetting.SaveHeightSetting(filePath);
        }
        public void SaveHHeightSetting(string filePath) {
            heightSetting.SaveHeightSetting(filePath);
        }
        #endregion

        #region Test Function
        private byte[] byteDataBuffer;
        private double[] deepthValue;
        public double[] DeepthValue {
            get {
                return deepthValue;
            }
            private set {
                deepthValue = value;
            }
        }

        public void LoadBmpDeepFile(string bmpDataPath) {
            // Read raw image into byte array
            MemoryStream memoryStream = new MemoryStream(800000);
            FileStream fs = new FileStream(bmpDataPath, FileMode.Open);
            fs.CopyTo(memoryStream);
            byteDataBuffer = memoryStream.ToArray();
            if(deepthValue != null)
                deepthValue = null;
            deepthValue = new double[(byteDataBuffer.Length - BmpHead) / 2];
            dataHeight = deepthValue.Length / DataWidth;
            for(int i = 0; i < deepthValue.Length - 1; i++) {
                deepthValue[i] = (((double)byteDataBuffer[2 * i + BmpHead + 1] * 256 + (double)byteDataBuffer[2 * i + BmpHead]) - 32768) * 8 / 1000;
            }
            fs.Close();
            memoryStream.Flush();
        }

        public void LoadDataValue(List<ushort> dataValue) {
            if(deepthValue != null)
                deepthValue = null;
            deepthValue = new double[dataValue.Count];
            dataHeight = deepthValue.Length / DataWidth;
            for(int h = 0 ; h < dataHeight; h++) {
                for(int w = 0; w < DataWidth; w++) {
                    int index = (dataHeight - 1 - h) * DataWidth + w;
                    deepthValue[index] = ((double)dataValue[h * DataWidth + w] - 32768) * 8 / 1000;
                }
            }
        }
        string rawLogPath;
        bool deepSaved = false;
        void SaveDeepValue(string filePath) {
            string contents = "";
            for(int h = 0; h < dataHeight; h += 5) {
                contents = "";
                for(int w = 0; w < DataWidth; w += 5)
                    contents += deepthValue[h * DataWidth + w].ToString("0.00") + ',';
                File.AppendAllText(filePath, contents + "\r\n");
            }
        }

        public Bitmap GetDeepBmp(double max, double min) {
            byte[] data = null;
            //建立副本
            data = (byte[])byteDataBuffer.Clone();
            double intervel = max - min;
            for (int i = 0; i < deepthValue.Length - 1; i++) {
                if (deepthValue[i] > min && deepthValue[i] < max) {
                    byte tempValue = (byte)((deepthValue[i] - min) / intervel * 32);
                    byte byteLow = (byte)((byte)(tempValue << 6) + tempValue);
                    byte byteHigh = (byte)((byte)(tempValue >> 2) + tempValue * 8);
                    data[2 * i + BmpHead + 1] = byteHigh;
                    data[2 * i + BmpHead] = byteLow;
                } else {
                    data[2 * i + BmpHead + 1] = 0;
                    data[2 * i + BmpHead] = 0;
                }
            }
            System.Drawing.Image returnImage;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data)) {
                returnImage = System.Drawing.Image.FromStream(ms);
                ms.Flush();
            }

            //return oImage;
            return new Bitmap(returnImage);
        }

        //Load bmp
        public string KmbMainTest(string dataFilePath, string originalFolder) {
            LoadBmpDeepFile(dataFilePath);
            spaceKeyName = "";
            for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                int keyIndex = keyNum - 1;
                string keyName = TestSetting.KeysName[keyIndex];
                if(keyName.Contains('_')) {
                    string[] nameSplit = keyName.Split('_');
                    spaceKeyName = nameSplit[0];
                    spaceIndex = Convert.ToInt16(nameSplit[1]) - 1;
                    spaceIndexs[spaceIndex] = keyIndex;
                }
            }
            if(spaceKeyName == "")
                NewTestFlow("", originalFolder, new Size(0, 0));
            else
                TestFlowSpace14("", originalFolder, new Size(0, 0));
            string resultJson = JsonConvert.SerializeObject(TestResult);
            return resultJson;
        }
        public string KmbMainTest(string barcode, string dataFilePath, string originalFolder) {
            LoadBmpDeepFile(dataFilePath);
            spaceKeyName = "";
            for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                int keyIndex = keyNum - 1;
                string keyName = TestSetting.KeysName[keyIndex];
                if(keyName.Contains('_')) {
                    string[] nameSplit = keyName.Split('_');
                    spaceKeyName = nameSplit[0];
                    spaceIndex = Convert.ToInt16(nameSplit[1]) - 1;
                    spaceIndexs[spaceIndex] = keyIndex;
                }
            }
            if(spaceKeyName == "")
                NewTestFlow(barcode, originalFolder, new Size(0, 0));
            else
                TestFlowSpace14(barcode, originalFolder, new Size(0, 0));
            string resultJson = JsonConvert.SerializeObject(TestResult);
            return resultJson;
        }
        public string KmbMainTest(string dataFilePath, string originalFolder, Size roiOffset) {
            LoadBmpDeepFile(dataFilePath);
            spaceKeyName = "";
            for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                int keyIndex = keyNum - 1;
                string keyName = TestSetting.KeysName[keyIndex];
                if(keyName.Contains('_')) {
                    string[] nameSplit = keyName.Split('_');
                    spaceKeyName = nameSplit[0];
                    spaceIndex = Convert.ToInt16(nameSplit[1]) - 1;
                    spaceIndexs[spaceIndex] = keyIndex;
                }
            }
            if(spaceKeyName == "")
                NewTestFlow("", originalFolder, roiOffset);
            else
                TestFlowSpace14("", originalFolder, roiOffset);
            string resultJson = JsonConvert.SerializeObject(TestResult);
            return resultJson;
        }
        public string KmbMainTest(string barcode, string dataFilePath, string originalFolder, Size roiOffset) {
            LoadBmpDeepFile(dataFilePath);
            spaceKeyName = "";
            for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                int keyIndex = keyNum - 1;
                string keyName = TestSetting.KeysName[keyIndex];
                if(keyName.Contains('_')) {
                    string[] nameSplit = keyName.Split('_');
                    spaceKeyName = nameSplit[0];
                    spaceIndex = Convert.ToInt16(nameSplit[1]) - 1;
                    spaceIndexs[spaceIndex] = keyIndex;
                }
            }
            if(spaceKeyName == "")
                NewTestFlow(barcode, originalFolder, roiOffset);
            else
                TestFlowSpace14(barcode, originalFolder, roiOffset);
            string resultJson = JsonConvert.SerializeObject(TestResult);
            return resultJson;
        }

        //Load dataValue
        public string KmbMainTest(List<ushort> dataValue, string originalFolder) {
            LoadDataValue(dataValue);
            spaceKeyName = "";
            for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                int keyIndex = keyNum - 1;
                string keyName = TestSetting.KeysName[keyIndex];
                if(keyName.Contains('_')) {
                    string[] nameSplit = keyName.Split('_');
                    spaceKeyName = nameSplit[0];
                    spaceIndex = Convert.ToInt16(nameSplit[1]) - 1;
                    spaceIndexs[spaceIndex] = keyIndex;
                }
            }
            if(spaceKeyName =="")
                NewTestFlow("", originalFolder, new Size(0, 0));
            else
                TestFlowSpace14("", originalFolder, new Size(0, 0));
            string resultJson = JsonConvert.SerializeObject(TestResult);
            return resultJson;
        }
        public string KmbMainTest(string barcode, List<ushort> dataValue, string originalFolder) {
            LoadDataValue(dataValue);
            spaceKeyName = "";
            for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                int keyIndex = keyNum - 1;
                string keyName = TestSetting.KeysName[keyIndex];
                if(keyName.Contains('_')) {
                    string[] nameSplit = keyName.Split('_');
                    spaceKeyName = nameSplit[0];
                    spaceIndex = Convert.ToInt16(nameSplit[1]) - 1;
                    spaceIndexs[spaceIndex] = keyIndex;
                }
            }
            if(spaceKeyName == "")
                NewTestFlow(barcode, originalFolder, new Size(0, 0));
            else
                TestFlowSpace14(barcode, originalFolder, new Size(0, 0));
            string resultJson = JsonConvert.SerializeObject(TestResult);
            return resultJson;
        }
        public string KmbMainTest(List<ushort> dataValue, string originalFolder, Size roiOffset) {
            LoadDataValue(dataValue);
            spaceKeyName = "";
            for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                int keyIndex = keyNum - 1;
                string keyName = TestSetting.KeysName[keyIndex];
                if(keyName.Contains('_')) {
                    string[] nameSplit = keyName.Split('_');
                    spaceKeyName = nameSplit[0];
                    spaceIndex = Convert.ToInt16(nameSplit[1]) - 1;
                    spaceIndexs[spaceIndex] = keyIndex;
                }
            }
            if(spaceKeyName == "")
                NewTestFlow("", originalFolder, roiOffset);
            else
                TestFlowSpace14("", originalFolder, roiOffset);
            string resultJson = JsonConvert.SerializeObject(TestResult);
            return resultJson;
        }
        public string KmbMainTest(string barcode, List<ushort> dataValue, string originalFolder, Size roiOffset) {
            LoadDataValue(dataValue);
            spaceKeyName = "";
            for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                int keyIndex = keyNum - 1;
                string keyName = TestSetting.KeysName[keyIndex];
                if(keyName.Contains('_')) {
                    string[] nameSplit = keyName.Split('_'); 
                    spaceKeyName = nameSplit[0];
                    spaceIndex = Convert.ToInt16(nameSplit[1]) - 1;
                    spaceIndexs[spaceIndex] = keyIndex;
                }
            }
            if(spaceKeyName == "")
                NewTestFlow(barcode, originalFolder, roiOffset);
            else
                TestFlowSpace14(barcode, originalFolder, roiOffset);
            string resultJson = JsonConvert.SerializeObject(TestResult);
            return resultJson;
        }

        //Deep
        private List<double> deepValues = new List<double>();   //oiginal value
        private double heightMax;
        private double heightMin;
        List<Dictionary<int, ResultValue.ResultStatus>> keyStatus = new List<Dictionary<int, ResultValue.ResultStatus>>();

        //Height
        private List<double> keyHeightValue = new List<double>(); //from base
        private List<bool> keyHeightResult = new List<bool>();
        private List<double> spaceHeightValues = new List<double>();
        private List<bool> spaceHeightResult = new List<bool>();

        
        //Slant
        private List<double> keySlantValue = new List<double>();
        private List<bool> keySlantResult = new List<bool>();

        //Alignment
        private List<Dictionary<string, double>> keyAlignmentValue = new List<Dictionary<string, double>>();
        private Dictionary<string, double> alignmentValues = new Dictionary<string, double>();
        private List<Dictionary<string, bool>> keyAlignmentStatus = new List<Dictionary<string, bool>>();
        private List<bool> keyAlignmentResult = new List<bool>();

        //TotalStatus
        public List<bool> keyTestStatus = new List<bool>();

        //Space
        string spaceKeyName="";
        int[] spaceIndexs = new int[4];   //key name:key index
        int spaceIndex;
        private enum TestSteps {
            Initial,
            CreateTestLog,
            GetDeepValue,
            SlantTest,
            AligmentTest,
            HeightTest,
            NewSlantTest,
            NewAligmentTest,
            LogNewHeight,
            EndTest
        };
        private void TestFlow(string barcode, string originalFolder, Size roiOffset) {
            TestSteps testStep = TestSteps.Initial;
            while(testStep != TestSteps.EndTest) {
                switch(testStep) {
                    case TestSteps.Initial:
                        TestResult = new ResultObject();
                        resultValue = new List<ResultValue>();
                        TestResult.Barcode = barcode;
                        TestResult.ModelName = TestSetting.ModelName;
                        TestResult.KeyCount = TestSetting.KeyCount;
                        testStep = TestSteps.CreateTestLog;
                        break;
                    case TestSteps.CreateTestLog:
                        #region CreateTestLog
                        originalFolder = originalFolder + String.Format(@"\ZHight Log\{0}\", DateTime.Now.ToString("yyyy-MM-dd"));
                        if(!Directory.Exists(originalFolder))
                            Directory.CreateDirectory(originalFolder);

                        logFolder = AppFolder + String.Format(@"\ZHight Log\{0}\", DateTime.Now.ToString("yyyy-MM-dd"));
                        if(!Directory.Exists(logFolder))
                            Directory.CreateDirectory(logFolder);

                        string prefix = "";
                        if(barcode != "")
                            prefix = string.Format(@"{0}_{1}", barcode, DateTime.Now.ToString("HH-mm-ss-ff"));
                        else
                            prefix = string.Format(@"{0}", DateTime.Now.ToString("HH-mm-ss-ff"));

                        deepLogPath = string.Format(@"{0}\{1}_Deep.log", originalFolder, prefix);
                        preHeightLogPath = string.Format(@"{0}\{1}_Height.log", originalFolder, prefix);
                        preSlantLogPath = string.Format(@"{0}\{1}_Slant.log", originalFolder, prefix);
                        preAlignmentLogPath = string.Format(@"{0}\{1}_Alignment.log", originalFolder, prefix);
                        newSlantLogPath = string.Format(@"{0}\{1}_Slant.log", logFolder, prefix);
                        newAlignmentLogPath = string.Format(@"{0}\{1}_Alignment.log", logFolder, prefix);
                        newHeightLogPath = string.Format(@"{0}\{1}_Height.log", logFolder, prefix);
                        resultPath = string.Format(@"{0}\{1}_Result.json", logFolder, prefix);

                        WriteLog(deepLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(preSlantLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(preAlignmentLogPath, "KeyRoiName\tKeyRoiName\tValue\tPF");
                        WriteLog(preHeightLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(newSlantLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(newAlignmentLogPath, "KeyRoiName\tKeyRoiName\tValue\tPF");
                        WriteLog(newHeightLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        #endregion
                        testStep = TestSteps.GetDeepValue;
                        break;

                    case TestSteps.GetDeepValue:
                        #region GetDeepValue
                        heightMax = -10000;
                        heightMin = 10000;
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            string keyName = TestSetting.KeysName[keyIndex];
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            deepValues = new List<double>();
                            keyHeightValue = new List<double>();
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                deepValues.Add(GetDeepthValue(keyNum, roiNum, roiOffset));
                                keyHeightValue.Add(GetHeightValue(keyNum, roiNum, roiOffset));
                            }
                            heightMax = keyHeightValue.Max() > heightMax ? keyHeightValue.Max() : heightMax;
                            heightMin = keyHeightValue.Min() < heightMin ? keyHeightValue.Min() : heightMin;

                            resultValue.Add(new ResultValue(keyNum, keyName, roiCount, deepValues, keyHeightValue, new List<double>(), new List<bool>(), new List<Dictionary<string, double>>(), new List<bool>(), new List<double>(), new List<double>(), new List<bool>(), new List<ResultValue.ResultStatus>()));
                            resultValue[keyIndex].KeyHeights = keyHeightValue;
                            resultValue[keyIndex].NewKeyHeights = keyHeightValue;
                                                                         
                            if(roiCount == 4) {
                                WriteLog(deepLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    deepValues[0], deepValues[1],
                                    deepValues[2], deepValues[3]));
                                WriteLog(preHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3]));
                            }
                            else if(roiCount == 6) {
                                WriteLog(deepLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    deepValues[0], deepValues[1],
                                    deepValues[2], deepValues[3],
                                    deepValues[4], deepValues[5]));
                                WriteLog(preHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3],
                                    keyHeightValue[4], keyHeightValue[5]));
                            }
                        }
                        #endregion
                        testStep = TestSteps.SlantTest;
                        break;

                    case TestSteps.SlantTest:
                        #region SlantTest
                        string preSlantLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            string keyName = TestSetting.KeysName[keyIndex];
                            keySlantValue = new List<double>();
                            Dictionary<string, SlantLimit> slantPairs = slantSetting.SlantSetting.SlantInfos[keyName].RoiPair;
                            preSlantLog = String.Format("{0}\t{1}", resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name);
                            for(int pairIndex = 0; pairIndex < slantPairs.Count; pairIndex++) {
                                string pairName = slantPairs.Keys.ToArray<string>()[pairIndex];
                                SlantLimit limit = slantPairs[pairName];
                                int[] roiPair = slantSetting.GetPairRoiNum(pairName);
                                double height1 = resultValue[keyIndex].Heights[roiPair[0] - 1], height2 = resultValue[keyIndex].Heights[roiPair[1] - 1];
                                resultValue[keyIndex].NewKeyHeights[roiPair[0] - 1] = height1;
                                resultValue[keyIndex].NewKeyHeights[roiPair[1] - 1] = height2;
                                double slantValue = Math.Round(height1 - height2, 3, MidpointRounding.AwayFromZero);
                                string keyRoiName = heightSetting.GetKeyRoiName(keyNum, roiPair[0]);
                                HeightLimit heightLimit = heightSetting.HeightSetting.HeightInfos[keyRoiName];

                                if(slantValue < limit.LowerLimit + limit.LowerTolerance || limit.UpperLimit + limit.UpperTolerance < slantValue) {
                                    //超出工廠範圍
                                    preSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);
                                }
                                else if((limit.LowerLimit + limit.LowerTolerance < slantValue && slantValue < limit.LowerLimit) ||  //工廠與客戶範圍內，下極限
                                        (limit.UpperLimit < slantValue && slantValue < limit.UpperLimit + limit.UpperTolerance)) {  //工廠與客戶範圍內，上極限
                                    preSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);

                                    double upGap1 = height1 - limit.UpperLimit;
                                    double upGap2 = height2 - limit.UpperLimit;
                                    double downGap1 = limit.LowerLimit - height1;
                                    double downGap2 = limit.LowerLimit - height2;

                                    int maxCase = 0;
                                    if(upGap1 > upGap2 && upGap1 > downGap1 && upGap1 > downGap2)
                                        maxCase = 1;
                                    else if(upGap2 > upGap1 && upGap2 > downGap1 && upGap2 > downGap2)
                                        maxCase = 2;
                                    else if(downGap1 > upGap1 && downGap1 > upGap2 && downGap1 > downGap2)
                                        maxCase = 3;
                                    else if(downGap2 > upGap1 && downGap2 > upGap2 && downGap2 > downGap1)
                                        maxCase = 4;

                                    switch(maxCase) {
                                        case 1:
                                            while(height1 > heightLimit.UpperLimit) {
                                                height1 -= 0.03;
                                            }
                                            resultValue[keyIndex].NewKeyHeights[roiPair[0] - 1] = height1;
                                            break;
                                        case 2:
                                            while(height2 > heightLimit.UpperLimit) {
                                                height2 -= 0.03;
                                            }
                                            resultValue[keyIndex].NewKeyHeights[roiPair[1] - 1] = height2;
                                            break;
                                        case 3:
                                            while(height1 < heightLimit.LowerLimit) {
                                                height1 += 0.03;
                                            }
                                            resultValue[keyIndex].NewKeyHeights[roiPair[0] - 1] = height1;
                                            break;
                                        case 4:
                                            while(height2 < heightLimit.LowerLimit) {
                                                height2 += 0.03;
                                            }
                                            resultValue[keyIndex].NewKeyHeights[roiPair[1] - 1] = height2;
                                            break;
                                    }
                                }
                                else {
                                    //在客戶範圍內
                                    preSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);
                                }
                            }
                            WriteLog(preSlantLogPath, preSlantLog);
                        }
                        #endregion
                        testStep = TestSteps.AligmentTest;
                        break;

                    case TestSteps.AligmentTest:
                        #region AligmentTest
                        string preAlignmentLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            keyAlignmentValue = new List<Dictionary<string, double>>();

                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                alignmentValues = new Dictionary<string, double>();
                                string thisKeyRoiName = alignmentSetting.GetKeyRoiName(keyNum, roiNum);
                                Dictionary<string, AlignmentLimit> alignmentPairs = alignmentSetting.AlignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair;
                                bool alignmentResult = true;
                                for(int otherKeyRioIndex = 0; otherKeyRioIndex < alignmentPairs.Count(); otherKeyRioIndex++) {
                                    string otherKeyRoiName = alignmentPairs.Keys.ToArray()[otherKeyRioIndex];
                                    int[] otherKeyRoiNum = alignmentSetting.GetKeyRoiNum(otherKeyRoiName);
                                    int otherKeyIndex = otherKeyRoiNum[0] - 1;
                                    int otherRoiIndex = otherKeyRoiNum[1] - 1;
                                    double thisHeight = resultValue[keyIndex].Heights[roiNum - 1];
                                    double otherHeight = resultValue[otherKeyIndex].Heights[otherRoiIndex];
                                    resultValue[keyIndex].NewKeyHeights[roiNum - 1] = thisHeight;
                                    //resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex] = otherHeight;
                                    AlignmentLimit limit = alignmentSetting.GetLimit(thisKeyRoiName, otherKeyRoiName);
                                    double alignmentValue = Math.Round(thisHeight - otherHeight, 3, MidpointRounding.AwayFromZero);

                                    if(alignmentValue < limit.LowerLimit + limit.LowerTolerance || limit.UpperLimit + limit.UpperTolerance < alignmentValue) {
                                        //超出工廠範圍
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult &= false;
                                        preAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + false + "\r\n";
                                    }
                                    else if((limit.LowerLimit + limit.LowerTolerance <= alignmentValue && alignmentValue < limit.LowerLimit) || //工廠與客戶範圍內，下極限
                                          (limit.UpperLimit < alignmentValue && alignmentValue <= limit.UpperLimit + limit.UpperTolerance)) { //工廠與客戶範圍內，上極限
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult &= true;
                                        preAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + true + "\r\n";

                                        HeightLimit heightLimit = heightSetting.HeightSetting.HeightInfos[thisKeyRoiName];

                                        double upGap1 = thisHeight - limit.UpperLimit;
                                        double upGap2 = otherHeight - limit.UpperLimit;
                                        double downGap1 = limit.LowerLimit - thisHeight;
                                        double downGap2 = limit.LowerLimit - otherHeight;

                                        int maxCase = 0;
                                        if(upGap1 > upGap2 && upGap1 > downGap1 && upGap1 > downGap2)
                                            maxCase = 1;
                                        else if(upGap2 > upGap1 && upGap2 > downGap1 && upGap2 > downGap2)
                                            maxCase = 2;
                                        else if(downGap1 > upGap1 && downGap1 > upGap2 && downGap1 > downGap2)
                                            maxCase = 3;
                                        else if(downGap2 > upGap1 && downGap2 > upGap2 && downGap2 > downGap1)
                                            maxCase = 4;

                                        switch(maxCase) {
                                            case 1:
                                                while(thisHeight > heightLimit.UpperLimit) {
                                                    thisHeight -= 0.03;
                                                }
                                                resultValue[keyIndex].NewKeyHeights[roiNum - 1] = thisHeight;
                                                break;
                                            case 2:
                                                while(otherHeight > heightLimit.UpperLimit) {
                                                    otherHeight -= 0.03;
                                                }
                                                resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex] = otherHeight;
                                                break;
                                            case 3:
                                                while(thisHeight < heightLimit.LowerLimit) {
                                                    thisHeight += 0.03;
                                                }
                                                resultValue[keyIndex].NewKeyHeights[roiNum - 1] = thisHeight;
                                                break;
                                            case 4:
                                                while(otherHeight < heightLimit.LowerLimit) {
                                                    otherHeight += 0.03;
                                                }
                                                resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex] = otherHeight;
                                                break;
                                        }
                                    }
                                    else {
                                        //在客戶範圍內
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult &= true;
                                        preAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + true + "\r\n";
                                    }
                                }//pair
                                keyAlignmentValue.Add(alignmentValues);
                            }//roi
                            resultValue[keyIndex].Alignment = keyAlignmentValue;
                        }//key
                        WriteLog(preAlignmentLogPath, preAlignmentLog);
                        #endregion
                        testStep = TestSteps.HeightTest;
                        break;

                    case TestSteps.HeightTest:
                        #region HeightTest
                        string heightLog = "";

                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            string keyName = TestSetting.KeysName[keyIndex];
                            heightLog = String.Format("{0}\t{1}", resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name);
                            keyHeightValue = new List<double>();
                            keyHeightResult = new List<bool>();
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                string keyNameRoi = String.Format("{0}#{1}", keyName, roiNum);
                                HeightLimit limit = heightSetting.HeightSetting.HeightInfos[keyNameRoi];
                                double heightValue = resultValue[keyIndex].NewKeyHeights[roiNum - 1];
                                if(heightValue < limit.LowerLimit + limit.LowerTolerance || limit.UpperLimit + limit.UpperTolerance < heightValue) {
                                    //超出工廠範圍 ，補空
                                    double interpLow = (limit.LowerLimit - heightMin) / ((limit.LowerLimit + limit.LowerTolerance) - heightMin);
                                    double interpUp = (heightMax - limit.UpperLimit) / (heightMax - (limit.UpperLimit + limit.UpperTolerance));
                                    if(heightValue < limit.LowerLimit + limit.LowerTolerance) {
                                        heightValue = limit.LowerLimit - ((limit.LowerLimit + limit.LowerTolerance) - heightValue) * interpLow;
                                        heightValue = Math.Round(heightValue, 3, MidpointRounding.AwayFromZero);
                                    }
                                    else if(heightValue > limit.UpperLimit + limit.UpperTolerance) {
                                        heightValue = (heightValue - (limit.UpperLimit + limit.UpperTolerance)) * interpUp + limit.UpperLimit;
                                        heightValue = Math.Round(heightValue, 3, MidpointRounding.AwayFromZero);
                                    }
                                    keyHeightValue.Add(heightValue);
                                    keyHeightResult.Add(false);
                                }
                                else if((limit.LowerLimit + limit.LowerTolerance < heightValue && heightValue < limit.LowerLimit) ||    //工廠與客戶範圍內，下極限
                                        (limit.UpperLimit < heightValue && heightValue < limit.UpperLimit + limit.UpperTolerance)) {    //工廠與客戶範圍內，上極限
                                    double upGap1 = heightValue - limit.UpperLimit;
                                    double upGap2 = heightValue - limit.LowerLimit;

                                    if(heightValue < limit.LowerLimit) {
                                        while(heightValue - limit.LowerLimit < 0) {
                                            heightValue += 0.03;
                                        };
                                    } else if(limit.UpperLimit < heightValue) {
                                        while(heightValue - limit.UpperLimit > 0) {
                                            heightValue -= 0.03;
                                        }
                                    }
                                    keyHeightValue.Add(heightValue);
                                    keyHeightResult.Add(true);
                                }
                                else {
                                    //在客戶範圍內
                                    keyHeightValue.Add(heightValue);
                                    keyHeightResult.Add(true);
                                }
                                
                                    heightLog += keyNameRoi + "\t" + heightValue.ToString("0.00") + "\r\n";
                            }
                            resultValue[keyIndex].KeyHeightResult = keyHeightResult;
                            resultValue[keyIndex].NewKeyHeights = keyHeightValue;
                            if(roiCount == 4)
                                WriteLog(newHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3]));
                            else if(roiCount == 6)
                                WriteLog(newHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3],
                                    keyHeightValue[4], keyHeightValue[5]));
                        }

                        TestResult.ResultValue = resultValue;
                        #endregion
                        testStep = TestSteps.NewSlantTest;
                        break;

                    case TestSteps.NewSlantTest:
                        #region NewSlantTest
                        string newSlantLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            string keyName = TestSetting.KeysName[keyIndex];
                            keySlantValue = new List<double>();
                            keySlantResult = new List<bool>();
                            Dictionary<string, SlantLimit> slantPairs = slantSetting.SlantSetting.SlantInfos[keyName].RoiPair;
                            newSlantLog = String.Format("{0}\t{1}", resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name);
                            for(int pairIndex = 0; pairIndex < slantPairs.Count; pairIndex++) {
                                string pairName = slantPairs.Keys.ToArray<string>()[pairIndex];
                                SlantLimit limit = slantPairs[pairName];
                                int[] roiPair = slantSetting.GetPairRoiNum(pairName);
                                double height1 = resultValue[keyIndex].NewKeyHeights[roiPair[0] - 1], height2 = resultValue[keyIndex].NewKeyHeights[roiPair[1] - 1];
                                double slantValue = Math.Round(height1 - height2, 3, MidpointRounding.AwayFromZero);

                                if(slantValue < limit.LowerLimit || limit.UpperLimit < slantValue) {
                                    //超出客戶範圍;
                                    newSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);
                                    keySlantResult.Add(false);
                                }
                                else {
                                    //客戶範圍內
                                    keySlantValue.Add(slantValue);
                                    keySlantResult.Add(true);
                                    newSlantLog += "\t" + slantValue.ToString();
                                }
                            }
                            resultValue[keyIndex].Slant = keySlantValue;
                            resultValue[keyIndex].SlantResult = keySlantResult;

                            //"Index\tName\t1\t2\t3\t4\t5\t6\r\n"
                            if(keySlantValue.Count() == 4)
                                WriteLog(newSlantLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keySlantValue[0], keySlantValue[1],
                                    keySlantValue[2], keySlantValue[3]));
                            else if(keySlantValue.Count() == 6)
                                WriteLog(newSlantLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keySlantValue[0], keySlantValue[1],
                                    keySlantValue[2], keySlantValue[3],
                                    keySlantValue[4], keySlantValue[5]));
                        }
                        TestResult.ResultValue = resultValue;
                        #endregion
                        testStep = TestSteps.NewAligmentTest;
                        break;

                    case TestSteps.NewAligmentTest:
                        #region NewAligmentTest
                        string newAlignmentLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            keyAlignmentValue = new List<Dictionary<string, double>>();
                            keyAlignmentResult = new List<bool>();

                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                keyAlignmentResult.Add(true);
                            }
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                alignmentValues = new Dictionary<string, double>();
                                string thisKeyRoiName = alignmentSetting.GetKeyRoiName(keyNum, roiNum);
                                Dictionary<string, AlignmentLimit> alignmentPairs = alignmentSetting.AlignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair;
                                bool alignmentResult = true;
                                for(int otherKeyRioIndex = 0; otherKeyRioIndex < alignmentPairs.Count(); otherKeyRioIndex++) {
                                    string otherKeyRoiName = alignmentPairs.Keys.ToArray()[otherKeyRioIndex];
                                    int[] otherKeyRoiNum = alignmentSetting.GetKeyRoiNum(otherKeyRoiName);
                                    int otherKeyIndex = otherKeyRoiNum[0] - 1;
                                    int otherRoiIndex = otherKeyRoiNum[1] - 1;
                                    double thisHeight = resultValue[keyIndex].NewKeyHeights[roiNum - 1];
                                    double otherHeight = resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex];
                                    AlignmentLimit limit = alignmentSetting.GetLimit(thisKeyRoiName, otherKeyRoiName);
                                    double alignmentValue = Math.Round(thisHeight - otherHeight, 3, MidpointRounding.AwayFromZero);

                                    if(alignmentValue < limit.LowerLimit || limit.UpperLimit < alignmentValue) {
                                        //超出客戶範圍
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult &= false;
                                        newAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + false + "\r\n";
                                    }
                                    else {
                                        //客戶範圍內
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult &= true;
                                        newAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + true + "\r\n";
                                    }
                                }//pair
                                keyAlignmentValue.Add(alignmentValues);
                                keyAlignmentResult[roiNum - 1] = alignmentResult;
                            }//roi
                            resultValue[keyIndex].Alignment = keyAlignmentValue;
                            resultValue[keyIndex].AlignmentResult = keyAlignmentResult;
                        }//key
                        WriteLog(newAlignmentLogPath, newAlignmentLog);
                        
                        TestResult.ResultValue = resultValue;
                        string resultJson = JsonConvert.SerializeObject(TestResult);
                        WriteLog(resultPath, resultJson);
                        #endregion
                        testStep = TestSteps.EndTest;
                        break;
                }
            }
        }

        private enum NewTestSteps {
            Initial,
            CreateTestLog,
            GetDeepValue,
            GetMaxMin,
            SlantTest,
            AligmentTest,
            HeightTest,
            CheckSlantResult,
            CheckAligmentResult,
            CheckHeightResult,
            NewSlantTest,
            NewAligmentTest,
            NewHeightTest,
            LogNewHeight,
            EndTest
        };

        private void NewTestFlow(string barcode, string originalFolder, Size roiOffset) {
            NewTestSteps testStep = NewTestSteps.Initial;
            while(testStep != NewTestSteps.EndTest) {
                switch(testStep) {
                    case NewTestSteps.Initial:
                        TestResult = new ResultObject();
                        resultValue = new List<ResultValue>();
                        TestResult.Barcode = barcode;
                        TestResult.ModelName = TestSetting.ModelName;
                        TestResult.KeyCount = TestSetting.KeyCount;

                        testStep = NewTestSteps.CreateTestLog;
                        break;
                    case NewTestSteps.CreateTestLog:
                        #region CreateTestLog
                        originalFolder = originalFolder + String.Format(@"\ZHight Log\{0}\", DateTime.Now.ToString("yyyy-MM-dd"));
                        if(!Directory.Exists(originalFolder))
                            Directory.CreateDirectory(originalFolder);

                        logFolder = AppFolder + String.Format(@"\ZHight Log\{0}\", DateTime.Now.ToString("yyyy-MM-dd"));
                        if(!Directory.Exists(logFolder))
                            Directory.CreateDirectory(logFolder);

                        string prefix = "";
                        if(barcode != "")
                            prefix = string.Format(@"{0}_{1}", barcode, DateTime.Now.ToString("HH-mm-ss-ff"));
                        else
                            prefix = string.Format(@"{0}", DateTime.Now.ToString("HH-mm-ss-ff"));

                        deepLogPath = string.Format(@"{0}\{1}_Deep.log", originalFolder, prefix);
                        preHeightLogPath = string.Format(@"{0}\{1}_Height.log", originalFolder, prefix);
                        preSlantLogPath = string.Format(@"{0}\{1}_Slant.log", originalFolder, prefix);
                        preAlignmentLogPath = string.Format(@"{0}\{1}_Alignment.log", originalFolder, prefix);
                        rawLogPath = string.Format(@"{0}\{1}_Raw.log", logFolder, prefix);
                        newSlantLogPath = string.Format(@"{0}\{1}_Slant.log", logFolder, prefix);
                        newAlignmentLogPath = string.Format(@"{0}\{1}_Alignment.log", logFolder, prefix);
                        newHeightLogPath = string.Format(@"{0}\{1}_Height.log", logFolder, prefix);
                        resultPath = string.Format(@"{0}\{1}_Result.json", logFolder, prefix);

                        WriteLog(deepLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(preSlantLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(preAlignmentLogPath, "KeyRoiName\tKeyRoiName\tValue\tPF");
                        WriteLog(preHeightLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(newSlantLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(newAlignmentLogPath, "KeyRoiName\tKeyRoiName\tValue\tPF");
                        WriteLog(newHeightLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        #endregion
                        testStep = NewTestSteps.GetDeepValue;
                        break;

                    case NewTestSteps.GetDeepValue:
                        #region GetDeepValue
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            string keyName = TestSetting.KeysName[keyIndex];
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            deepValues = new List<double>();
                            keyHeightValue = new List<double>();
                            List<ResultValue.ResultStatus> status = new List<ResultValue.ResultStatus>();
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                deepValues.Add(GetDeepthValue(keyNum, roiNum, roiOffset));
                                double hightValue = GetHeightValue(keyNum, roiNum, roiOffset);
                                keyHeightValue.Add(hightValue);
                                if(!deepSaved & hightValue < -7) {
                                    deepSaved = true;
                                    SaveDeepValue(rawLogPath);
                                }
                                status.Add(ResultValue.ResultStatus.Ingauge);
                            }
                            deepSaved = false;

                            resultValue.Add(new ResultValue(keyNum, keyName, roiCount, deepValues, keyHeightValue, new List<double>(), new List<bool>(), new List<Dictionary<string, double>>(), new List<bool>(), new List<double>(), new List<double>(), new List<bool>(), new List<ResultValue.ResultStatus>()));
                            resultValue[keyIndex].KeyHeights = keyHeightValue;
                            resultValue[keyIndex].NewKeyHeights = keyHeightValue;
                            resultValue[keyIndex].KeyStatus = status;

                            if(roiCount == 4) {
                                WriteLog(deepLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    deepValues[0], deepValues[1],
                                    deepValues[2], deepValues[3]));
                                WriteLog(preHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3]));
                            }
                            else if(roiCount == 6) {
                                WriteLog(deepLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    deepValues[0], deepValues[1],
                                    deepValues[2], deepValues[3],
                                    deepValues[4], deepValues[5]));
                                WriteLog(preHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3],
                                    keyHeightValue[4], keyHeightValue[5]));
                            }
                        }
                        #endregion
                        testStep = NewTestSteps.GetMaxMin;
                        break;

                    case NewTestSteps.GetMaxMin:
                        heightMax = -10000;
                        heightMin = 10000;
                        heightMax = keyHeightValue.Max() > heightMax ? keyHeightValue.Max() : heightMax;
                        heightMin = keyHeightValue.Min() < heightMin ? keyHeightValue.Min() : heightMin;
                        testStep = NewTestSteps.SlantTest;
                        break;

                    case NewTestSteps.SlantTest:
                        #region SlantTest
                        string preSlantLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            string keyName = TestSetting.KeysName[keyIndex];
                            keySlantValue = new List<double>();
                            keySlantResult = new List<bool>();
                            Dictionary<string, SlantLimit> slantPairs = slantSetting.SlantSetting.SlantInfos[keyName].RoiPair;
                            preSlantLog = String.Format("{0}\t{1}", resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name);
                            for(int pairIndex = 0; pairIndex < slantPairs.Count; pairIndex++) {
                                string pairName = slantPairs.Keys.ToArray<string>()[pairIndex];
                                SlantLimit limit = slantPairs[pairName];
                                int[] roiPair = slantSetting.GetPairRoiNum(pairName);
                                int thisRoiIndex = roiPair[0] - 1, otherRoiIndex = roiPair[1] - 1;
                                double height1 = resultValue[keyIndex].Heights[thisRoiIndex], height2 = resultValue[keyIndex].Heights[otherRoiIndex];
                                resultValue[keyIndex].NewKeyHeights[thisRoiIndex] = height1;
                                resultValue[keyIndex].NewKeyHeights[otherRoiIndex] = height2;
                                double slantValue = Math.Round(height1 - height2, 3, MidpointRounding.AwayFromZero);
                                string keyRoiName = heightSetting.GetKeyRoiName(keyNum, roiPair[0]);

                                if(slantValue < limit.LowerLimit + limit.LowerTolerance || limit.UpperLimit + limit.UpperTolerance < slantValue) {
                                    //超出工廠範圍
                                    preSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);
                                    keySlantResult.Add(false);
                                    resultValue[keyIndex].KeyStatus[thisRoiIndex] = ResultValue.ResultStatus.NG;
                                }
                                else if((limit.LowerLimit + limit.LowerTolerance < slantValue && slantValue < limit.LowerLimit) ||  //工廠與客戶範圍內，下極限
                                        (limit.UpperLimit < slantValue && slantValue < limit.UpperLimit + limit.UpperTolerance)) {  //工廠與客戶範圍內，上極限
                                    preSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);
                                    keySlantResult.Add(true);
                                    resultValue[keyIndex].KeyStatus[thisRoiIndex] = ResultValue.ResultStatus.OK;
                                }
                                else {
                                    //在客戶範圍內
                                    preSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);
                                    keySlantResult.Add(true);
                                    resultValue[keyIndex].KeyStatus[thisRoiIndex] = ResultValue.ResultStatus.Ingauge;
                                }
                            }//pair
                            resultValue[keyIndex].SlantResult = keySlantResult;
                            WriteLog(preSlantLogPath, preSlantLog);
                        }//key
                        #endregion
                        testStep = NewTestSteps.AligmentTest;
                        break;

                    case NewTestSteps.AligmentTest:
                        #region AligmentTest
                        string preAlignmentLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            keyAlignmentValue = new List<Dictionary<string, double>>();
                                keyAlignmentResult = new List<bool>();

                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                int thisRoiIndex = roiNum - 1;
                                alignmentValues = new Dictionary<string, double>();
                                bool aligmentResult = true;
                                string thisKeyRoiName = alignmentSetting.GetKeyRoiName(keyNum, roiNum);
                                Dictionary<string, AlignmentLimit> alignmentPairs = alignmentSetting.AlignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair;

                                for(int otherKeyRioIndex = 0; otherKeyRioIndex < alignmentPairs.Count(); otherKeyRioIndex++) {
                                    string otherKeyRoiName = alignmentPairs.Keys.ToArray()[otherKeyRioIndex];
                                    int[] otherKeyRoiNum = alignmentSetting.GetKeyRoiNum(otherKeyRoiName);
                                    int otherKeyIndex = otherKeyRoiNum[0] - 1;
                                    int otherRoiIndex = otherKeyRoiNum[1] - 1;
                                    double thisHeight = resultValue[keyIndex].Heights[thisRoiIndex];
                                    double otherHeight = resultValue[otherKeyIndex].Heights[otherRoiIndex];
                                    resultValue[keyIndex].NewKeyHeights[thisRoiIndex] = thisHeight;
                                    //resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex] = otherHeight;
                                    AlignmentLimit limit = alignmentSetting.GetLimit(thisKeyRoiName, otherKeyRoiName);
                                    double alignmentValue = Math.Round(thisHeight - otherHeight, 3, MidpointRounding.AwayFromZero);

                                    if(alignmentValue < limit.LowerLimit + limit.LowerTolerance || limit.UpperLimit + limit.UpperTolerance < alignmentValue) {
                                        //超出工廠範圍
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        aligmentResult &= false;
                                        resultValue[keyIndex].KeyStatus[thisRoiIndex] = ResultValue.ResultStatus.NG;
                                        preAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + false + "\r\n";
                                    }
                                    else if((limit.LowerLimit + limit.LowerTolerance <= alignmentValue && alignmentValue < limit.LowerLimit) || //工廠與客戶範圍內，下極限
                                          (limit.UpperLimit < alignmentValue && alignmentValue <= limit.UpperLimit + limit.UpperTolerance)) { //工廠與客戶範圍內，上極限
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        aligmentResult &= true;
                                        if(resultValue[keyIndex].KeyStatus[thisRoiIndex] < ResultValue.ResultStatus.NG)
                                            resultValue[keyIndex].KeyStatus[thisRoiIndex] = ResultValue.ResultStatus.OK;
                                        preAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + true + "\r\n";
                                        
                                    }
                                    else {
                                        //在客戶範圍內
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        aligmentResult &= true;
                                        //KeyStatus unchange
                                        preAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + true + "\r\n";
                                    }
                                }//pair
                                keyAlignmentValue.Add(alignmentValues);
                                keyAlignmentResult.Add(aligmentResult);
                            }//roi
                            resultValue[keyIndex].Alignment = keyAlignmentValue;
                            resultValue[keyIndex].AlignmentResult = keyAlignmentResult;
                        }//key
                        WriteLog(preAlignmentLogPath, preAlignmentLog);
                        #endregion
                        testStep = NewTestSteps.HeightTest;
                        break;

                    case NewTestSteps.HeightTest:
                        #region HeightTest
                        string heightLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            string keyName = TestSetting.KeysName[keyIndex];
                            heightLog = String.Format("{0}\t{1}", resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name);
                            keyHeightValue = new List<double>();
                            keyHeightResult = new List<bool>();
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                int thisRoiIndex = roiNum - 1;
                                string keyNameRoi = String.Format("{0}#{1}", keyName, roiNum);
                                HeightLimit limit = heightSetting.HeightSetting.HeightInfos[keyNameRoi];
                                double heightValue = resultValue[keyIndex].NewKeyHeights[roiNum - 1];
                                if(heightValue < limit.LowerLimit + limit.LowerTolerance || limit.UpperLimit + limit.UpperTolerance < heightValue) {
                                    keyHeightValue.Add(heightValue);
                                    keyHeightResult.Add(false);
                                    resultValue[keyIndex].KeyStatus[thisRoiIndex] = ResultValue.ResultStatus.NG;
                                }
                                else if((limit.LowerLimit + limit.LowerTolerance < heightValue && heightValue < limit.LowerLimit) ||    //工廠與客戶範圍內，下極限
                                        (limit.UpperLimit < heightValue && heightValue < limit.UpperLimit + limit.UpperTolerance)) {    //工廠與客戶範圍內，上極限
                                    keyHeightValue.Add(heightValue);
                                    keyHeightResult.Add(true);
                                    if(resultValue[keyIndex].KeyStatus[thisRoiIndex] < ResultValue.ResultStatus.NG)
                                        resultValue[keyIndex].KeyStatus[thisRoiIndex] = ResultValue.ResultStatus.OK;
                                }
                                else {
                                    //在客戶範圍內
                                    keyHeightValue.Add(heightValue);
                                    keyHeightResult.Add(true);
                                    //KeyStatus unchange
                                }
                                heightLog += keyNameRoi + "\t" + heightValue.ToString("0.00") + "\r\n";
                            }
                            resultValue[keyIndex].KeyHeightResult = keyHeightResult;
                            //if(roiCount == 4) {
                            //    WriteLog(newHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                            //        resultValue[keyIndex].KeyIndex, keyName,
                            //        keyHeightValue[0], keyHeightValue[1],
                            //        keyHeightValue[2], keyHeightValue[3]));
                            //}
                            //else if(roiCount == 6) {
                            //    WriteLog(newHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                            //        resultValue[keyIndex].KeyIndex, keyName,
                            //        keyHeightValue[0], keyHeightValue[1],
                            //        keyHeightValue[2], keyHeightValue[3],
                            //        keyHeightValue[4], keyHeightValue[5]));
                            //}
                        }
                        TestResult.ResultValue = resultValue;
                        #endregion
                        testStep = NewTestSteps.CheckSlantResult;
                        break;

                    case NewTestSteps.CheckSlantResult:
                        #region  CheckSlantResult
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            string keyName = TestSetting.KeysName[keyIndex];
                            keySlantValue = new List<double>();
                            keySlantResult = new List<bool>();
                            Dictionary<string, SlantLimit> slantPairs = slantSetting.SlantSetting.SlantInfos[keyName].RoiPair;
                            for(int pairIndex = 0; pairIndex < slantPairs.Count; pairIndex++) {
                                string pairName = slantPairs.Keys.ToArray<string>()[pairIndex];
                                SlantLimit limit = slantPairs[pairName];
                                int[] roiPair = slantSetting.GetPairRoiNum(pairName);
                                string keyRoiName = heightSetting.GetKeyRoiName(keyNum, roiPair[0]);
                                HeightLimit heightLimit = heightSetting.HeightSetting.HeightInfos[keyRoiName];
                                int roi1 = roiPair[0] - 1, roi2 = roiPair[1] - 1;
                                if(resultValue[keyIndex].KeyStatus[roi1] == ResultValue.ResultStatus.OK) {
                                    double height1 = resultValue[keyIndex].Heights[roi1], height2 = resultValue[keyIndex].Heights[roi2];
                                    double middle = (heightLimit.UpperLimit + heightLimit.LowerLimit) / 2;
                                    double Gap1 = Math.Abs(height1 - middle);
                                    double Gap2 = Math.Abs(height2 - middle);
                                    if(Gap1 > Gap2) {
                                        while(height1 > heightLimit.UpperLimit) {
                                            height1 -= 0.03;
                                        }
                                        while(height1 < heightLimit.LowerLimit) {
                                            height1 += 0.03;
                                        }
                                        resultValue[keyIndex].NewKeyHeights[roi1] = height1;
                                        resultValue[keyIndex].KeyStatus[roi1] = ResultValue.ResultStatus.Ingauge;
                                    }
                                    if(Gap1 < Gap2) {
                                        while(height2 > heightLimit.UpperLimit) {
                                            height2 -= 0.03;
                                        }
                                        while(height2 < heightLimit.LowerLimit) {
                                            height2 += 0.03;
                                        }
                                        resultValue[keyIndex].NewKeyHeights[roi2] = height2;
                                        resultValue[keyIndex].KeyStatus[roi2] = ResultValue.ResultStatus.Ingauge;
                                    }
                                }
                            }
                        }
                        #endregion
                        testStep = NewTestSteps.CheckAligmentResult;
                        break;
                    case NewTestSteps.CheckAligmentResult:
                        #region  CheckAligmentResult
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int thisKeyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[thisKeyIndex].Length;
                            keyAlignmentValue = new List<Dictionary<string, double>>();

                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                int thisRoiIndex = roiNum - 1;
                                if(resultValue[thisKeyIndex].KeyStatus[thisRoiIndex] == ResultValue.ResultStatus.OK) {
                                    alignmentValues = new Dictionary<string, double>();
                                    keyAlignmentResult = new List<bool>();
                                    string thisKeyRoiName = alignmentSetting.GetKeyRoiName(keyNum, roiNum);
                                    Dictionary<string, AlignmentLimit> alignmentPairs = alignmentSetting.AlignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair;

                                    for(int otherKeyRioIndex = 0; otherKeyRioIndex < alignmentPairs.Count(); otherKeyRioIndex++) {
                                        string otherKeyRoiName = alignmentPairs.Keys.ToArray()[otherKeyRioIndex];
                                        int[] otherKeyRoiNum = alignmentSetting.GetKeyRoiNum(otherKeyRoiName);
                                        int otherKeyIndex = otherKeyRoiNum[0] - 1;
                                        int otherRoiIndex = otherKeyRoiNum[1] - 1;
                                        if(thisKeyIndex < otherKeyIndex) {
                                            string keyRoiName = heightSetting.GetKeyRoiName(keyNum, roiNum);
                                            HeightLimit heightLimit = heightSetting.HeightSetting.HeightInfos[keyRoiName];
                                            if(resultValue[thisKeyIndex].KeyStatus[thisRoiIndex] == ResultValue.ResultStatus.OK) {
                                                double height1 = resultValue[thisKeyIndex].Heights[thisRoiIndex], height2 = resultValue[otherKeyIndex].Heights[otherRoiIndex];
                                                double middle = (heightLimit.UpperLimit + heightLimit.LowerLimit) / 2;
                                                double Gap1 = Math.Abs(height1 - middle);
                                                double Gap2 = Math.Abs(height2 - middle);
                                                if(Gap1 > Gap2) {
                                                    while(height1 > heightLimit.UpperLimit) {
                                                        height1 -= 0.03;
                                                    }
                                                    while(height1 < heightLimit.LowerLimit) {
                                                        height1 += 0.03;
                                                    }
                                                    resultValue[thisKeyIndex].NewKeyHeights[thisRoiIndex] = height1;
                                                    resultValue[thisKeyIndex].KeyStatus[thisRoiIndex] = ResultValue.ResultStatus.Ingauge;
                                                }
                                                if(Gap1 < Gap2) {
                                                    while(height2 > heightLimit.UpperLimit) {
                                                        height2 -= 0.03;
                                                    }
                                                    while(height2 < heightLimit.LowerLimit) {
                                                        height2 += 0.03;
                                                    }
                                                    resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex] = height2;
                                                    resultValue[otherKeyIndex].KeyStatus[otherRoiIndex] = ResultValue.ResultStatus.Ingauge;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        testStep = NewTestSteps.CheckHeightResult;
                        break;
                    case NewTestSteps.CheckHeightResult:
                        #region  CheckHeightResult
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            string keyName = TestSetting.KeysName[keyIndex];
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                int roiIndex = roiNum - 1;
                                string keyNameRoi = String.Format("{0}#{1}", keyName, roiNum);
                                HeightLimit limit = heightSetting.HeightSetting.HeightInfos[keyNameRoi];
                                double height = resultValue[keyIndex].NewKeyHeights[roiNum - 1];
                                double interpLow = (limit.LowerLimit - heightMin) / ((limit.LowerLimit + limit.LowerTolerance) - heightMin);
                                double interpUp = (heightMax - limit.UpperLimit) / (heightMax - (limit.UpperLimit + limit.UpperTolerance));
                                if(resultValue[keyIndex].KeyStatus[roiIndex] == ResultValue.ResultStatus.OK) {
                                    while(height > limit.UpperLimit) {
                                        height -= 0.03;
                                    }
                                    while(height < limit.LowerLimit) {
                                        height += 0.03;
                                    }
                                    resultValue[keyIndex].NewKeyHeights[roiIndex] = height;
                                    resultValue[keyIndex].KeyStatus[roiIndex] = ResultValue.ResultStatus.Ingauge;
                                }else if(resultValue[keyIndex].KeyStatus[roiIndex] == ResultValue.ResultStatus.NG) {
                                    if(height < limit.LowerLimit + limit.LowerTolerance) {
                                        height = limit.LowerLimit - ((limit.LowerLimit + limit.LowerTolerance) - height) * interpLow;
                                        height = Math.Round(height, 3, MidpointRounding.AwayFromZero);
                                    }
                                    else if(height > limit.UpperLimit + limit.UpperTolerance) {
                                        height = (height - (limit.UpperLimit + limit.UpperTolerance)) * interpUp + limit.UpperLimit;
                                        height = Math.Round(height, 3, MidpointRounding.AwayFromZero);
                                    }
                                    resultValue[keyIndex].NewKeyHeights[roiIndex] = height;
                                    keyHeightResult.Add(false);
                                }
                            }
                            if (roiCount == 4) {
                                WriteLog(newHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    resultValue[keyIndex].NewKeyHeights[0], resultValue[keyIndex].NewKeyHeights[1],
                                    resultValue[keyIndex].NewKeyHeights[2], resultValue[keyIndex].NewKeyHeights[3]));
                            } else if (roiCount == 6) {
                                WriteLog(newHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    resultValue[keyIndex].NewKeyHeights[0], resultValue[keyIndex].NewKeyHeights[1],
                                    resultValue[keyIndex].NewKeyHeights[2], resultValue[keyIndex].NewKeyHeights[3],
                                    resultValue[keyIndex].NewKeyHeights[4], resultValue[keyIndex].NewKeyHeights[5]));
                            }
                        }
                        #endregion
                        testStep = NewTestSteps.NewSlantTest;
                        break;

                    case NewTestSteps.NewSlantTest:
                        #region NewSlantTest
                        string newSlantLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            string keyName = TestSetting.KeysName[keyIndex];
                            keySlantValue = new List<double>();
                            keySlantResult = new List<bool>();
                            Dictionary<string, SlantLimit> slantPairs = slantSetting.SlantSetting.SlantInfos[keyName].RoiPair;
                            newSlantLog = String.Format("{0}\t{1}", resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name);
                            for(int pairIndex = 0; pairIndex < slantPairs.Count; pairIndex++) {
                                string pairName = slantPairs.Keys.ToArray<string>()[pairIndex];
                                SlantLimit limit = slantPairs[pairName];
                                int[] roiPair = slantSetting.GetPairRoiNum(pairName);
                                double height1 = resultValue[keyIndex].NewKeyHeights[roiPair[0] - 1], height2 = resultValue[keyIndex].NewKeyHeights[roiPair[1] - 1];
                                double slantValue = Math.Round(height1 - height2, 3, MidpointRounding.AwayFromZero);

                                newSlantLog += "\t" + slantValue.ToString();
                                keySlantValue.Add(slantValue);
                            }
                            resultValue[keyIndex].Slant = keySlantValue;
                            //resultValue[keyIndex].SlantResult = keySlantResult;

                            //"Index\tName\t1\t2\t3\t4\t5\t6\r\n"
                            if(keySlantValue.Count() == 4)
                                WriteLog(newSlantLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keySlantValue[0], keySlantValue[1],
                                    keySlantValue[2], keySlantValue[3]));
                            else if(keySlantValue.Count() == 6)
                                WriteLog(newSlantLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keySlantValue[0], keySlantValue[1],
                                    keySlantValue[2], keySlantValue[3],
                                    keySlantValue[4], keySlantValue[5]));
                        }
                        TestResult.ResultValue = resultValue;
                        #endregion
                        testStep = NewTestSteps.NewAligmentTest;
                        break;

                    case NewTestSteps.NewAligmentTest:
                        #region NewAligmentTest
                        string newAlignmentLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            keyAlignmentValue = new List<Dictionary<string, double>>();
                            keyAlignmentResult = new List<bool>();

                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                keyAlignmentResult.Add(true);
                            }
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                alignmentValues = new Dictionary<string, double>();
                                string thisKeyRoiName = alignmentSetting.GetKeyRoiName(keyNum, roiNum);
                                Dictionary<string, AlignmentLimit> alignmentPairs = alignmentSetting.AlignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair;
                                for(int otherKeyRioIndex = 0; otherKeyRioIndex < alignmentPairs.Count(); otherKeyRioIndex++) {
                                    string otherKeyRoiName = alignmentPairs.Keys.ToArray()[otherKeyRioIndex];
                                    int[] otherKeyRoiNum = alignmentSetting.GetKeyRoiNum(otherKeyRoiName);
                                    int otherKeyIndex = otherKeyRoiNum[0] - 1;
                                    int otherRoiIndex = otherKeyRoiNum[1] - 1;
                                    double thisHeight = resultValue[keyIndex].NewKeyHeights[roiNum - 1];
                                    double otherHeight = resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex];
                                    AlignmentLimit limit = alignmentSetting.GetLimit(thisKeyRoiName, otherKeyRoiName);
                                    double alignmentValue = Math.Round(thisHeight - otherHeight, 3, MidpointRounding.AwayFromZero);
                                    alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                    newAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + false + "\r\n";
                                    //if(alignmentValue < limit.LowerLimit || limit.UpperLimit < alignmentValue) {
                                    //    //超出客戶範圍
                                    //    alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                    //    newAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + false + "\r\n";
                                    //}
                                    //else {
                                    //    //客戶範圍內
                                    //    alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                    //    newAlignmentLog += thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + true + "\r\n";
                                    //}
                                }//pair
                                keyAlignmentValue.Add(alignmentValues);
                            }//roi
                            resultValue[keyIndex].Alignment = keyAlignmentValue;
                        }//key
                        WriteLog(newAlignmentLogPath, newAlignmentLog);

                        TestResult.ResultValue = resultValue;
                        string resultJson = JsonConvert.SerializeObject(TestResult);
                        WriteLog(resultPath, resultJson);
                        #endregion
                        testStep = NewTestSteps.EndTest;
                        break;
                }
            }
        }


        private enum Space14TestSteps {
            Initial,
            CreateTestLog,
            GetDeepValue,
            GetSpaceDeepValue,
            SlantTest,
            GetSpaceSlant,
            AligmentTest,
            GetSpaceAligment,
            HeightTest,
            GetNewSpaceHeight,
            NewSlantTest,
            GetNewSpaceSlant,
            NewAligmentTest,
            GetNewSpaceAligment,
            MaxMinTest,
            EndTest
        };
        private void TestFlowSpace14(string barcode, string originalFolder, Size roiOffset) {
            Space14TestSteps testStep = Space14TestSteps.Initial;
            while(testStep != Space14TestSteps.EndTest) {
                switch(testStep) {
                    case Space14TestSteps.Initial:
                        deepSaved = false;
                        TestResult = new ResultObject();
                        resultValue = new List<ResultValue>();
                        TestResult.Barcode = barcode;
                        TestResult.ModelName = TestSetting.ModelName;
                        TestResult.KeyCount = TestSetting.KeyCount;

                        testStep = Space14TestSteps.CreateTestLog;
                        break;
                    case Space14TestSteps.CreateTestLog:
                        originalFolder = originalFolder + String.Format(@"\ZHight Log\{0}\", DateTime.Now.ToString("yyyy-MM-dd"));
                        if(!Directory.Exists(originalFolder))
                            Directory.CreateDirectory(originalFolder);

                        logFolder = AppFolder + String.Format(@"\ZHight Log\{0}\", DateTime.Now.ToString("yyyy-MM-dd"));
                        if(!Directory.Exists(logFolder))
                            Directory.CreateDirectory(logFolder);

                        string prefix = "";
                        if(barcode != "")
                            prefix = string.Format(@"{0}_{1}", barcode, DateTime.Now.ToString("HH-mm-ss-ff"));
                        else
                            prefix = string.Format(@"{0}", DateTime.Now.ToString("HH-mm-ss-ff"));

                        deepLogPath = string.Format(@"{0}\{1}_Deep.log", originalFolder, prefix);
                        preHeightLogPath = string.Format(@"{0}\{1}_Height.log", originalFolder, prefix);
                        preSlantLogPath = string.Format(@"{0}\{1}_Slant.log", originalFolder, prefix);
                        preAlignmentLogPath = string.Format(@"{0}\{1}_Alignment.log", originalFolder, prefix);
                        newSlantLogPath = string.Format(@"{0}\{1}_Slant.log", logFolder, prefix);
                        newAlignmentLogPath = string.Format(@"{0}\{1}_Alignment.log", logFolder, prefix);
                        newHeightLogPath = string.Format(@"{0}\{1}_Height.log", logFolder, prefix);
                        resultPath = string.Format(@"{0}\{1}_Result.json", logFolder, prefix);

                        WriteLog(deepLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(preSlantLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(preAlignmentLogPath, "KeyRoiName\tKeyRoiName\tValue\tPF");
                        WriteLog(preHeightLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(newSlantLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        WriteLog(newAlignmentLogPath, "KeyRoiName\tKeyRoiName\tValue\tPF");
                        WriteLog(newHeightLogPath, "Index\tName\t1\t2\t3\t4\t5\t6");
                        testStep = Space14TestSteps.GetDeepValue;
                        break;

                    case Space14TestSteps.GetDeepValue:
                        heightMax = -10000;
                        heightMin = 10000;
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;

                            string keyName = TestSetting.KeysName[keyIndex];
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            deepValues = new List<double>();
                            keyHeightValue = new List<double>();
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                deepValues.Add(GetDeepthValue(keyNum, roiNum, roiOffset));
                                double hightValue = GetHeightValue(keyNum, roiNum, roiOffset);
                                keyHeightValue.Add(hightValue);
                                if(hightValue < 0 && !deepSaved ) {
                                    deepSaved = true;
                                    SaveDeepValue(rawLogPath);
                                }
                            }
                            heightMax = keyHeightValue.Max() > heightMax ? keyHeightValue.Max() : heightMax;
                            heightMin = keyHeightValue.Min() < heightMin ? keyHeightValue.Min() : heightMin;

                            resultValue.Add(new ResultValue(keyNum, keyName, roiCount, deepValues, keyHeightValue, new List<double>(), new List<bool>(), new List<Dictionary<string, double>>(), new List<bool>(), new List<double>(), new List<double>(), new List<bool>(), new List<ResultValue.ResultStatus>()));
                            resultValue[keyIndex].KeyHeights = keyHeightValue;
                            resultValue[keyIndex].NewKeyHeights = keyHeightValue;

                            if(roiCount == 4) {
                                WriteLog(deepLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    deepValues[0], deepValues[1],
                                    deepValues[2], deepValues[3]));
                                WriteLog(preHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3]));
                            }
                            else if(roiCount == 6) {
                                WriteLog(deepLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    deepValues[0], deepValues[1],
                                    deepValues[2], deepValues[3],
                                    deepValues[4], deepValues[5]));
                                WriteLog(preHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, keyName,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3],
                                    keyHeightValue[4], keyHeightValue[5]));
                            }
                        }

                        testStep = Space14TestSteps.GetSpaceDeepValue;
                        break;

                    case Space14TestSteps.GetSpaceDeepValue:
                        int spaceIndex = TestSetting.KeyCount;
                        deepValues = new List<double>();
                        spaceHeightValues = new List<double>();
                        //01 02 05 06 09 10 13 14
                        //04 03 08 07 12 11 16 15
                        deepValues.Add(resultValue[spaceIndexs[0]].Deeps[0]);
                        deepValues.Add(resultValue[spaceIndexs[0]].Deeps[1]);
                        deepValues.Add(resultValue[spaceIndexs[1]].Deeps[0]);
                        deepValues.Add(resultValue[spaceIndexs[1]].Deeps[1]);

                        deepValues.Add(resultValue[spaceIndexs[2]].Deeps[0]);
                        deepValues.Add(resultValue[spaceIndexs[2]].Deeps[1]);
                        deepValues.Add(resultValue[spaceIndexs[3]].Deeps[0]);
                        deepValues.Add(resultValue[spaceIndexs[3]].Deeps[1]);

                        deepValues.Add(resultValue[spaceIndexs[3]].Deeps[2]);
                        deepValues.Add(resultValue[spaceIndexs[3]].Deeps[3]);
                        deepValues.Add(resultValue[spaceIndexs[2]].Deeps[2]);
                        deepValues.Add(resultValue[spaceIndexs[2]].Deeps[3]);

                        deepValues.Add(resultValue[spaceIndexs[1]].Deeps[2]);
                        deepValues.Add(resultValue[spaceIndexs[1]].Deeps[3]);
                        deepValues.Add(resultValue[spaceIndexs[0]].Deeps[2]);
                        deepValues.Add(resultValue[spaceIndexs[0]].Deeps[3]);

                        spaceHeightValues.Add(resultValue[spaceIndexs[0]].Heights[0]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[0]].Heights[1]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[1]].Heights[0]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[1]].Heights[1]);

                        spaceHeightValues.Add(resultValue[spaceIndexs[2]].Heights[0]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[2]].Heights[1]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[3]].Heights[0]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[3]].Heights[1]);

                        spaceHeightValues.Add(resultValue[spaceIndexs[3]].Heights[2]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[3]].Heights[3]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[2]].Heights[2]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[2]].Heights[3]);

                        spaceHeightValues.Add(resultValue[spaceIndexs[1]].Heights[2]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[1]].Heights[3]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[0]].Heights[2]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[0]].Heights[3]);

                        resultValue.Add(new ResultValue(spaceIndex + 1, spaceKeyName, 16, deepValues, spaceHeightValues, new List<double>(), new List<bool>(), new List<Dictionary<string, double>>(), new List<bool>(), new List<double>(), new List<double>(), new List<bool>(), new List<ResultValue.ResultStatus>()));
                        resultValue[spaceIndex].KeyHeights = spaceHeightValues;
                        resultValue[spaceIndex].NewKeyHeights = spaceHeightValues;

                        WriteLog(deepLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}\t{8:0.00}\t{9:0.00}\t{10:0.00}\t{11:0.00}\t{12:0.00}\t{13:0.00}\t{14:0.00}\t{15:0.00}",
                            resultValue[spaceIndex].KeyIndex, spaceKeyName,
                            deepValues[0], deepValues[1], deepValues[2], deepValues[3], deepValues[4], deepValues[5], deepValues[6],
                            deepValues[7], deepValues[8], deepValues[9], deepValues[10], deepValues[11], deepValues[12], deepValues[13]));

                        WriteLog(preHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}\t{8:0.00}\t{9:0.00}\t{10:0.00}\t{11:0.00}\t{12:0.00}\t{13:0.00}\t{14:0.00}\t{15:0.00}",
                            resultValue[spaceIndex].KeyIndex, spaceKeyName,
                            spaceHeightValues[0], spaceHeightValues[1], spaceHeightValues[2], spaceHeightValues[3], spaceHeightValues[4], spaceHeightValues[5], spaceHeightValues[6],
                            spaceHeightValues[7], spaceHeightValues[8], spaceHeightValues[9], spaceHeightValues[10], spaceHeightValues[11], spaceHeightValues[12], spaceHeightValues[13]));

                        testStep = Space14TestSteps.SlantTest;
                        break;

                    case Space14TestSteps.SlantTest:
                        string preSlantLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            string keyName = TestSetting.KeysName[keyIndex];
                            keySlantValue = new List<double>();
                            Dictionary<string, SlantLimit> slantPairs = slantSetting.SlantSetting.SlantInfos[keyName].RoiPair;
                            preSlantLog = String.Format("{0}\t{1}", resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name);
                            for(int pairIndex = 0; pairIndex < slantPairs.Count; pairIndex++) {
                                string pairName = slantPairs.Keys.ToArray<string>()[pairIndex];
                                SlantLimit limit = slantPairs[pairName];
                                int[] roiPair = slantSetting.GetPairRoiNum(pairName);
                                double height1 = resultValue[keyIndex].Heights[roiPair[0] - 1], height2 = resultValue[keyIndex].Heights[roiPair[1] - 1];
                                resultValue[keyIndex].NewKeyHeights[roiPair[0] - 1] = height1;
                                resultValue[keyIndex].NewKeyHeights[roiPair[1] - 1] = height2;
                                double slantValue = Math.Round(height1 - height2, 3, MidpointRounding.AwayFromZero);
                                string keyRoiName = heightSetting.GetKeyRoiName(keyNum, roiPair[0]);
                                HeightLimit heightLimit = heightSetting.HeightSetting.HeightInfos[keyRoiName];

                                if(slantValue < limit.LowerLimit + limit.LowerTolerance || limit.UpperLimit + limit.UpperTolerance < slantValue) {
                                    //超出工廠範圍
                                    preSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);
                                }
                                else if((limit.LowerLimit + limit.LowerTolerance < slantValue && slantValue < limit.LowerLimit) ||  //工廠與客戶範圍內，下極限
                                        (limit.UpperLimit < slantValue && slantValue < limit.UpperLimit + limit.UpperTolerance)) {  //工廠與客戶範圍內，上極限
                                    preSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);

                                    double upGap1 = height1 - limit.UpperLimit;
                                    double upGap2 = height2 - limit.UpperLimit;
                                    double downGap1 = limit.LowerLimit - height1;
                                    double downGap2 = limit.LowerLimit - height2;

                                    int maxCase = 0;
                                    if(upGap1 > upGap2 && upGap1 > downGap1 && upGap1 > downGap2)
                                        maxCase = 1;
                                    else if(upGap2 > upGap1 && upGap2 > downGap1 && upGap2 > downGap2)
                                        maxCase = 2;
                                    else if(downGap1 > upGap1 && downGap1 > upGap2 && downGap1 > downGap2)
                                        maxCase = 3;
                                    else if(downGap2 > upGap1 && downGap2 > upGap2 && downGap2 > downGap1)
                                        maxCase = 4;

                                    switch(maxCase) {
                                        case 1:
                                            while(height1 > heightLimit.UpperLimit) {
                                                height1 -= 0.03;
                                            }
                                            resultValue[keyIndex].NewKeyHeights[roiPair[0] - 1] = height1;
                                            break;
                                        case 2:
                                            while(height2 > heightLimit.UpperLimit) {
                                                height2 -= 0.03;
                                            }
                                            resultValue[keyIndex].NewKeyHeights[roiPair[1] - 1] = height2;
                                            break;
                                        case 3:
                                            while(height1 < heightLimit.LowerLimit) {
                                                height1 += 0.03;
                                            }
                                            resultValue[keyIndex].NewKeyHeights[roiPair[0] - 1] = height1;
                                            break;
                                        case 4:
                                            while(height2 < heightLimit.LowerLimit) {
                                                height2 += 0.03;
                                            }
                                            resultValue[keyIndex].NewKeyHeights[roiPair[1] - 1] = height2;
                                            break;
                                    }
                                }
                                else {
                                    //在客戶範圍內
                                    preSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);
                                }
                            }

                            //"Index\tName\t1\t2\t3\t4\t5\t6\r\n"
                            if(keySlantValue.Count() == 4)
                                WriteLog(preSlantLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keySlantValue[0], keySlantValue[1],
                                    keySlantValue[2], keySlantValue[3]));
                            else if(keySlantValue.Count() == 6)
                                WriteLog(preSlantLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keySlantValue[0], keySlantValue[1],
                                    keySlantValue[2], keySlantValue[3],
                                    keySlantValue[4], keySlantValue[5]));
                        }
                        testStep = Space14TestSteps.GetSpaceSlant;
                        break;

                    case Space14TestSteps.GetSpaceSlant:
                        spaceIndex = TestSetting.KeyCount;
                        preSlantLog = String.Format("{0}\t{1}", resultValue[spaceIndex].KeyIndex, spaceKeyName);
                        double max1 = resultValue[spaceIndexs[0]].Heights.Max();
                        double max2 = resultValue[spaceIndexs[1]].Heights.Max();
                        double max3 = resultValue[spaceIndexs[2]].Heights.Max();
                        double max4 = resultValue[spaceIndexs[3]].Heights.Max();
                        double min1 = resultValue[spaceIndexs[0]].Heights.Min();
                        double min2 = resultValue[spaceIndexs[1]].Heights.Min();
                        double min3 = resultValue[spaceIndexs[2]].Heights.Min();
                        double min4 = resultValue[spaceIndexs[3]].Heights.Min();
                        resultValue[spaceIndex].Slant = new List<double>();

                        resultValue[spaceIndex].Slant.Add(max1 - min2);
                        resultValue[spaceIndex].Slant.Add(max2 - min1);
                        resultValue[spaceIndex].Slant.Add(max2 - min3);
                        resultValue[spaceIndex].Slant.Add(max3 - min2);
                        resultValue[spaceIndex].Slant.Add(max3 - min4);
                        resultValue[spaceIndex].Slant.Add(max4 - min3);

                        preSlantLog += String.Format("\t{0:0.00}", max1 - min2);
                        preSlantLog += String.Format("\t{0:0.00}", max2 - min1);
                        preSlantLog += String.Format("\t{0:0.00}", max2 - min3);
                        preSlantLog += String.Format("\t{0:0.00}", max3 - min2);
                        preSlantLog += String.Format("\t{0:0.00}", max3 - min4);
                        preSlantLog += String.Format("\t{0:0.00}", max4 - min3);

                        WriteLog(preSlantLogPath, preSlantLog);
                        testStep = Space14TestSteps.AligmentTest;
                        break;

                    case Space14TestSteps.AligmentTest:
                        string preAlignmentLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            keyAlignmentValue = new List<Dictionary<string, double>>();
                            keyAlignmentResult = new List<bool>();
                            for(int roiNum = 0; roiNum <= roiCount; roiNum++) {
                                keyAlignmentResult.Add(true);
                            }
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                alignmentValues = new Dictionary<string, double>();
                                string thisKeyRoiName = alignmentSetting.GetKeyRoiName(keyNum, roiNum);
                                Dictionary<string, AlignmentLimit> alignmentPairs = alignmentSetting.AlignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair;
                                bool alignmentResult = true;
                                HeightLimit heightLimit = heightSetting.HeightSetting.HeightInfos[thisKeyRoiName];

                                for(int otherKeyRioIndex = 0; otherKeyRioIndex < alignmentPairs.Count(); otherKeyRioIndex++) {
                                    string otherKeyRoiName = alignmentPairs.Keys.ToArray()[otherKeyRioIndex];
                                    int[] otherKeyRoiNum = alignmentSetting.GetKeyRoiNum(otherKeyRoiName);
                                    int otherKeyIndex = otherKeyRoiNum[0] - 1;
                                    int otherRoiIndex = otherKeyRoiNum[1] - 1;
                                    double thisHeight = resultValue[keyIndex].Heights[roiNum - 1];
                                    double otherHeight = resultValue[otherKeyIndex].Heights[otherRoiIndex];
                                    resultValue[keyIndex].NewKeyHeights[roiNum - 1] = thisHeight;
                                    resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex] = otherHeight;
                                    AlignmentLimit limit = alignmentSetting.GetLimit(thisKeyRoiName, otherKeyRoiName);
                                    double alignmentValue = Math.Round(thisHeight - otherHeight, 3, MidpointRounding.AwayFromZero);
                                    Console.WriteLine(alignmentValue);
                                    if(alignmentValue < limit.LowerLimit + limit.LowerTolerance || limit.UpperLimit + limit.UpperTolerance < alignmentValue) {
                                        //超出工廠範圍
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult &= false;
                                        preAlignmentLog = thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + false;
                                        WriteLog(preAlignmentLogPath, preAlignmentLog);
                                    }
                                    else if((limit.LowerLimit + limit.LowerTolerance <= alignmentValue && alignmentValue < limit.LowerLimit) || //工廠與客戶範圍內，下極限
                                            (limit.UpperLimit < alignmentValue && alignmentValue <= limit.UpperLimit + limit.UpperTolerance)) { //工廠與客戶範圍內，上極限
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult &= true;
                                        preAlignmentLog = thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + true;
                                        WriteLog(preAlignmentLogPath, preAlignmentLog);
                                        double upGap1 = thisHeight - limit.UpperLimit;
                                        double upGap2 = otherHeight - limit.UpperLimit;
                                        double downGap1 = limit.LowerLimit - thisHeight;
                                        double downGap2 = limit.LowerLimit - otherHeight;

                                        int maxCase = 0;
                                        if(upGap1 > upGap2 && upGap1 > downGap1 && upGap1 > downGap2)
                                            maxCase = 1;
                                        else if(upGap2 > upGap1 && upGap2 > downGap1 && upGap2 > downGap2)
                                            maxCase = 2;
                                        else if(downGap1 > upGap1 && downGap1 > upGap2 && downGap1 > downGap2)
                                            maxCase = 3;
                                        else if(downGap2 > upGap1 && downGap2 > upGap2 && downGap2 > downGap1)
                                            maxCase = 4;

                                        switch(maxCase) {
                                            case 1:
                                                while(thisHeight > heightLimit.UpperLimit) {
                                                    thisHeight -= 0.03;
                                                }
                                                resultValue[keyIndex].NewKeyHeights[roiNum - 1] = thisHeight;
                                                break;
                                            case 2:
                                                while(otherHeight > heightLimit.UpperLimit) {
                                                    otherHeight -= 0.03;
                                                }
                                                resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex] = otherHeight;
                                                break;
                                            case 3:
                                                while(thisHeight < heightLimit.LowerLimit) {
                                                    thisHeight += 0.03;
                                                }
                                                resultValue[keyIndex].NewKeyHeights[roiNum - 1] = thisHeight;
                                                break;
                                            case 4:
                                                while(otherHeight < heightLimit.LowerLimit) {
                                                    otherHeight += 0.03;
                                                }
                                                resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex] = otherHeight;
                                                break;
                                        }
                                    }
                                    else {
                                        //在客戶範圍內
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult &= true;
                                        preAlignmentLog = thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + true;
                                        WriteLog(preAlignmentLogPath, preAlignmentLog);
                                    }
                                }//pair
                                keyAlignmentValue.Add(alignmentValues);
                                keyAlignmentResult[roiNum - 1] = alignmentResult;
                            }//roi
                            resultValue[keyIndex].Alignment = keyAlignmentValue;
                            resultValue[keyIndex].AlignmentResult = keyAlignmentResult;
                        }//key
                        //WriteLog(preAlignmentLogPath, preAlignmentLog);

                        testStep = Space14TestSteps.GetSpaceAligment;
                        break;

                    case Space14TestSteps.GetSpaceAligment:
                        spaceIndex = TestSetting.KeyCount;
                        preAlignmentLog = "";
                        keyAlignmentValue = new List<Dictionary<string, double>>();
                        keyAlignmentValue.Add(resultValue[spaceIndexs[0]].Alignment[0]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[0])
                            preAlignmentLog += spaceKeyName + "#1\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[0] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[0]].Alignment[1]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[1])
                            preAlignmentLog += spaceKeyName + "#2\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[1] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[1]].Alignment[0]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[1]].Alignment[0])
                            preAlignmentLog += spaceKeyName + "#3\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[1]].AlignmentResult[0] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[1]].Alignment[1]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[1]].Alignment[1])
                            preAlignmentLog += spaceKeyName + "#4A\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[1]].AlignmentResult[1] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[2]].Alignment[0]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[2]].Alignment[0])
                            preAlignmentLog += spaceKeyName + "#4B\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[2]].AlignmentResult[0] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[2]].Alignment[1]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[2]].Alignment[1])
                            preAlignmentLog += spaceKeyName + "#5\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[2]].AlignmentResult[1] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[3]].Alignment[0]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[3]].Alignment[0])
                            preAlignmentLog += spaceKeyName + "#6\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[3]].AlignmentResult[0] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[3]].Alignment[1]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[3]].Alignment[1])
                            preAlignmentLog += spaceKeyName + "#7\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[3]].AlignmentResult[1] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[3]].Alignment[2]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[2])
                            preAlignmentLog += spaceKeyName + "#8\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[2] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[3]].Alignment[3]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[3])
                            preAlignmentLog += spaceKeyName + "#9\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[3] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[2]].Alignment[2]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[2]].Alignment[2])
                            preAlignmentLog += spaceKeyName + "#10\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[2]].AlignmentResult[2] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[2]].Alignment[3]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[2]].Alignment[3])
                            preAlignmentLog += spaceKeyName + "#11B\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[2]].AlignmentResult[3] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[1]].Alignment[2]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[1]].Alignment[2])
                            preAlignmentLog += spaceKeyName + "#11A\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[1]].AlignmentResult[2] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[1]].Alignment[3]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[1]].Alignment[3])
                            preAlignmentLog += spaceKeyName + "#12\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[1]].AlignmentResult[3] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[0]].Alignment[2]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[2])
                            preAlignmentLog += spaceKeyName + "#13\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[2] + "\r\n";
                        keyAlignmentValue.Add(resultValue[spaceIndexs[0]].Alignment[3]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[3])
                            preAlignmentLog += spaceKeyName + "#14\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[3] + "\r\n";
                        WriteLog(preAlignmentLogPath, preAlignmentLog);

                        testStep = Space14TestSteps.HeightTest;
                        break;

                    case Space14TestSteps.HeightTest:
                        string heightLog = "";

                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            string keyName = TestSetting.KeysName[keyIndex];
                            keyHeightValue = new List<double>();
                            keyHeightResult = new List<bool>();
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                string keyNameRoi = String.Format("{0}#{1}", keyName, roiNum);
                                HeightLimit limit = heightSetting.HeightSetting.HeightInfos[keyNameRoi];
                                double heightValue = resultValue[keyIndex].KeyHeights[roiNum - 1];
                                if(heightValue < limit.LowerLimit + limit.LowerTolerance || limit.UpperLimit + limit.UpperTolerance < heightValue) {
                                    //超出工廠範圍 ，補空
                                    double interpLow = (limit.LowerLimit - heightMin) / ((limit.LowerLimit + limit.LowerTolerance) - heightMin);
                                    double interpUp = (heightMax - limit.UpperLimit) / (heightMax - (limit.UpperLimit + limit.UpperTolerance));
                                    if(heightValue < limit.LowerLimit + limit.LowerTolerance) {
                                        heightValue = limit.LowerLimit - ((limit.LowerLimit + limit.LowerTolerance) - heightValue) * interpLow;
                                        heightValue = Math.Round(heightValue, 3, MidpointRounding.AwayFromZero);
                                    }
                                    else if(heightValue > limit.UpperLimit + limit.UpperTolerance) {
                                        heightValue = (heightValue - (limit.UpperLimit + limit.UpperTolerance)) * interpUp + limit.UpperLimit;
                                        heightValue = Math.Round(heightValue, 3, MidpointRounding.AwayFromZero);
                                    }
                                    keyHeightValue.Add(heightValue);
                                    keyHeightResult.Add(false);
                                }
                                else if((limit.LowerLimit + limit.LowerTolerance < heightValue && heightValue < limit.LowerLimit) ||    //工廠與客戶範圍內，下極限
                                        (limit.UpperLimit < heightValue && heightValue < limit.UpperLimit + limit.UpperTolerance)) {    //工廠與客戶範圍內，上極限
                                    double upGap1 = heightValue - limit.UpperLimit;
                                    double upGap2 = heightValue - limit.LowerLimit;

                                    if(heightValue < limit.LowerLimit) {
                                        while(heightValue < limit.LowerLimit) {
                                            heightValue += 0.03;
                                        };
                                    }
                                    else if(limit.UpperLimit < heightValue) {
                                        while(heightValue - limit.UpperLimit > 0) {
                                            heightValue -= 0.03;
                                        }
                                    }
                                    keyHeightValue.Add(heightValue);
                                    keyHeightResult.Add(true);
                                }
                                else {
                                    //在客戶範圍內
                                    keyHeightValue.Add(heightValue);
                                    keyHeightResult.Add(true);
                                }
                                heightLog += keyNameRoi + "\t" + heightValue.ToString("0.00") + "\r\n";
                            }
                            resultValue[keyIndex].KeyHeightResult = keyHeightResult;
                            resultValue[keyIndex].NewKeyHeights = keyHeightValue;
                            if (roiCount == 4) {
                                WriteLog(newHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3]));
                                WriteLog("D:\\123.txt", String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3]));
                            } else if (roiCount == 6)
                                WriteLog(newHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}",
                                    resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name,
                                    keyHeightValue[0], keyHeightValue[1],
                                    keyHeightValue[2], keyHeightValue[3],
                                    keyHeightValue[4], keyHeightValue[5]));
                        }

                        TestResult.ResultValue = resultValue;
                        testStep = Space14TestSteps.GetNewSpaceHeight;
                        break;

                    case Space14TestSteps.GetNewSpaceHeight:
                        spaceIndex = TestSetting.KeyCount;
                        spaceHeightValues = new List<double>();
                        //01 02 05 06 09 10 13 14
                        //04 03 08 07 12 11 16 15
                        spaceHeightValues.Add(resultValue[spaceIndexs[0]].NewKeyHeights[0]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[0]].NewKeyHeights[1]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[1]].NewKeyHeights[0]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[1]].NewKeyHeights[1]);

                        spaceHeightValues.Add(resultValue[spaceIndexs[2]].NewKeyHeights[0]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[2]].NewKeyHeights[1]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[3]].NewKeyHeights[0]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[3]].NewKeyHeights[1]);

                        spaceHeightValues.Add(resultValue[spaceIndexs[3]].NewKeyHeights[2]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[3]].NewKeyHeights[3]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[2]].NewKeyHeights[2]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[2]].NewKeyHeights[3]);

                        spaceHeightValues.Add(resultValue[spaceIndexs[1]].NewKeyHeights[2]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[1]].NewKeyHeights[3]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[0]].NewKeyHeights[2]);
                        spaceHeightValues.Add(resultValue[spaceIndexs[0]].NewKeyHeights[3]);

                        resultValue[spaceIndex].NewKeyHeights = spaceHeightValues;

                        WriteLog(newHeightLogPath, String.Format("{0}\t{1}\t{2:0.00}\t{3:0.00}\t{4:0.00}\t{5:0.00}\t{6:0.00}\t{7:0.00}\t{8:0.00}\t{9:0.00}\t{10:0.00}\t{11:0.00}\t{12:0.00}\t{13:0.00}\t{14:0.00}\t{15:0.00}",
                            resultValue[spaceIndex].KeyIndex, spaceKeyName,
                            spaceHeightValues[0], spaceHeightValues[1], spaceHeightValues[2], spaceHeightValues[3], spaceHeightValues[4], spaceHeightValues[5], spaceHeightValues[6],
                            spaceHeightValues[7], spaceHeightValues[8], spaceHeightValues[9], spaceHeightValues[10], spaceHeightValues[11], spaceHeightValues[12], spaceHeightValues[13]));

                        testStep = Space14TestSteps.NewSlantTest;
                        break;

                    case Space14TestSteps.NewSlantTest:
                        string newSlantLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            string keyName = TestSetting.KeysName[keyIndex];
                            keySlantValue = new List<double>();
                            keySlantResult = new List<bool>();
                            Dictionary<string, SlantLimit> slantPairs = slantSetting.SlantSetting.SlantInfos[keyName].RoiPair;
                            newSlantLog = String.Format("{0}\t{1}", resultValue[keyIndex].KeyIndex, resultValue[keyIndex].Name);
                            for(int pairIndex = 0; pairIndex < slantPairs.Count; pairIndex++) {
                                string pairName = slantPairs.Keys.ToArray<string>()[pairIndex];
                                SlantLimit limit = slantPairs[pairName];
                                int[] roiPair = slantSetting.GetPairRoiNum(pairName);
                                double height1 = resultValue[keyIndex].NewKeyHeights[roiPair[0] - 1], height2 = resultValue[keyIndex].NewKeyHeights[roiPair[1] - 1];
                                double slantValue = Math.Round(height1 - height2, 3, MidpointRounding.AwayFromZero);

                                if(slantValue < limit.LowerLimit || limit.UpperLimit < slantValue) {
                                    //超出客戶範圍;
                                    newSlantLog += "\t" + slantValue.ToString();
                                    keySlantValue.Add(slantValue);
                                    keySlantResult.Add(false);
                                }
                                else {
                                    //客戶範圍內
                                    keySlantValue.Add(slantValue);
                                    keySlantResult.Add(true);
                                    newSlantLog += "\t" + slantValue.ToString();
                                }
                            }
                            resultValue[keyIndex].Slant = keySlantValue;
                            resultValue[keyIndex].SlantResult = keySlantResult;

                            WriteLog(newSlantLogPath, newSlantLog);
                        }
                        TestResult.ResultValue = resultValue;
                        testStep = Space14TestSteps.GetNewSpaceSlant;
                        break;

                    case Space14TestSteps.GetNewSpaceSlant:
                        spaceIndex = TestSetting.KeyCount;
                        newSlantLog = String.Format("{0}\t{1}", resultValue[spaceIndex].KeyIndex, spaceKeyName);
                        max1 = resultValue[spaceIndexs[0]].NewKeyHeights.Max();
                        max2 = resultValue[spaceIndexs[1]].NewKeyHeights.Max();
                        max3 = resultValue[spaceIndexs[2]].NewKeyHeights.Max();
                        max4 = resultValue[spaceIndexs[3]].NewKeyHeights.Max();
                        min1 = resultValue[spaceIndexs[0]].NewKeyHeights.Min();
                        min2 = resultValue[spaceIndexs[1]].NewKeyHeights.Min();
                        min3 = resultValue[spaceIndexs[2]].NewKeyHeights.Min();
                        min4 = resultValue[spaceIndexs[3]].NewKeyHeights.Min();
                        resultValue[spaceIndex].Slant = new List<double>();

                        resultValue[spaceIndex].Slant.Add(max1 - min2);
                        resultValue[spaceIndex].Slant.Add(max2 - min1);
                        resultValue[spaceIndex].Slant.Add(max2 - min3);
                        resultValue[spaceIndex].Slant.Add(max3 - min2);
                        resultValue[spaceIndex].Slant.Add(max3 - min4);
                        resultValue[spaceIndex].Slant.Add(max4 - min3);

                        newSlantLog += String.Format("\t{0:0.00}", (max1 - min2));
                        newSlantLog += String.Format("\t{0:0.00}", (max2 - min1));
                        newSlantLog += String.Format("\t{0:0.00}", (max2 - min3));
                        newSlantLog += String.Format("\t{0:0.00}", (max3 - min2));
                        newSlantLog += String.Format("\t{0:0.00}", (max3 - min4));
                        newSlantLog += String.Format("\t{0:0.00}", (max4 - min3));

                        WriteLog(newSlantLogPath, newSlantLog);
                        testStep = Space14TestSteps.NewAligmentTest;
                        break;

                    case Space14TestSteps.NewAligmentTest:
                        string newAlignmentLog = "";
                        for(int keyNum = 1; keyNum <= TestSetting.KeyCount; keyNum++) {
                            int keyIndex = keyNum - 1;
                            int roiCount = TestSetting.KeysRoi[keyIndex].Length;
                            keyAlignmentValue = new List<Dictionary<string, double>>();
                            keyAlignmentResult = new List<bool>();
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                keyAlignmentResult.Add(true);
                            }
                            for(int roiNum = 1; roiNum <= roiCount; roiNum++) {
                                alignmentValues = new Dictionary<string, double>();
                                string thisKeyRoiName = alignmentSetting.GetKeyRoiName(keyNum, roiNum);
                                Dictionary<string, AlignmentLimit> alignmentPairs = alignmentSetting.AlignmentSetting.AlignmentInfos[thisKeyRoiName].RoiPair;
                                bool alignmentResult = true;
                                for(int otherKeyRioIndex = 0; otherKeyRioIndex < alignmentPairs.Count(); otherKeyRioIndex++) {
                                    string otherKeyRoiName = alignmentPairs.Keys.ToArray()[otherKeyRioIndex];
                                    int[] otherKeyRoiNum = alignmentSetting.GetKeyRoiNum(otherKeyRoiName);
                                    int otherKeyIndex = otherKeyRoiNum[0] - 1;
                                    int otherRoiIndex = otherKeyRoiNum[1] - 1;
                                    double thisHeight = resultValue[keyIndex].NewKeyHeights[roiNum - 1];
                                    double otherHeight = resultValue[otherKeyIndex].NewKeyHeights[otherRoiIndex];
                                    AlignmentLimit limit = alignmentSetting.GetLimit(thisKeyRoiName, otherKeyRoiName);
                                    double alignmentValue = Math.Round(thisHeight - otherHeight, 3, MidpointRounding.AwayFromZero);

                                    if(alignmentValue < limit.LowerLimit || limit.UpperLimit < alignmentValue) {
                                        //超出客戶範圍
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult = false;
                                        newAlignmentLog = thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + alignmentResult;
                                        WriteLog(newAlignmentLogPath, newAlignmentLog);
                                    }
                                    else {
                                        //客戶範圍內
                                        alignmentValues.Add(otherKeyRoiName, alignmentValue);
                                        alignmentResult = true;
                                        newAlignmentLog = thisKeyRoiName + "\t" + otherKeyRoiName + "\t" + alignmentValue.ToString("0.00") + "\t" + alignmentResult;
                                        WriteLog(newAlignmentLogPath, newAlignmentLog);
                                    }
                                    keyAlignmentValue.Add(alignmentValues);
                                    keyAlignmentResult[roiNum - 1] = alignmentResult;
                                }
                            }
                            resultValue[keyIndex].Alignment = keyAlignmentValue;
                            resultValue[keyIndex].AlignmentResult = keyAlignmentResult;
                        }
                        testStep = Space14TestSteps.GetNewSpaceAligment;
                        break;

                    case Space14TestSteps.GetNewSpaceAligment:

                        spaceIndex = TestSetting.KeyCount;
                        newAlignmentLog = "";
                        keyAlignmentValue = new List<Dictionary<string, double>>();
                        keyAlignmentResult = new List<bool>();

                        keyAlignmentValue.Add(resultValue[spaceIndexs[0]].Alignment[0]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[0]].AlignmentResult[0]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[0])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#1\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[0] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[0]].Alignment[1]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[0]].AlignmentResult[1]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[1])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#2\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[1] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[1]].Alignment[0]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[1]].AlignmentResult[0]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[1]].Alignment[0])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#3\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[1]].AlignmentResult[0] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[1]].Alignment[1]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[1]].AlignmentResult[1]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[1]].Alignment[1])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#4A\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[1]].AlignmentResult[1] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[2]].Alignment[0]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[2]].AlignmentResult[0]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[2]].Alignment[0])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#4B\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[2]].AlignmentResult[0] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[2]].Alignment[1]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[2]].AlignmentResult[1]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[2]].Alignment[1])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#5\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[2]].AlignmentResult[1] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[3]].Alignment[0]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[3]].AlignmentResult[0]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[3]].Alignment[0])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#6\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[3]].AlignmentResult[0] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[3]].Alignment[1]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[3]].AlignmentResult[1]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[3]].Alignment[1])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#7\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[3]].AlignmentResult[1] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[3]].Alignment[2]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[3]].AlignmentResult[2]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[2])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#8\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[2] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[3]].Alignment[3]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[3]].AlignmentResult[3]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[3])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#9\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[3] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[2]].Alignment[2]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[2]].AlignmentResult[2]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[2]].Alignment[2])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#10\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[2]].AlignmentResult[2] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[2]].Alignment[3]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[2]].AlignmentResult[3]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[2]].Alignment[3])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#11A\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[2]].AlignmentResult[3] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[1]].Alignment[2]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[1]].AlignmentResult[2]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[1]].Alignment[2])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#11B\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[1]].AlignmentResult[2] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[1]].Alignment[3]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[1]].AlignmentResult[3]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[1]].Alignment[3])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#12\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[1]].AlignmentResult[3] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[0]].Alignment[2]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[0]].AlignmentResult[2]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[2])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#13\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[2] + "\r\n";

                        keyAlignmentValue.Add(resultValue[spaceIndexs[0]].Alignment[3]);
                        resultValue[spaceIndexs[0]].AlignmentResult.Add(resultValue[spaceIndexs[0]].AlignmentResult[3]);
                        foreach(KeyValuePair<string, double> alignmentValue in resultValue[spaceIndexs[0]].Alignment[3])
                            newAlignmentLog += resultValue[spaceIndexs[0]].Name + "#14\t" + alignmentValue.Key + "\t" + alignmentValue.Value.ToString("0.00") + "\t" + resultValue[spaceIndexs[0]].AlignmentResult[3] + "\r\n";

                        resultValue[spaceIndex].Alignment = keyAlignmentValue;
                        resultValue[spaceIndex].AlignmentResult = keyAlignmentResult;
                        WriteLog(newAlignmentLogPath, newAlignmentLog);

                        TestResult.ResultValue = resultValue;
                        string resultJson = JsonConvert.SerializeObject(TestResult);
                        WriteLog(resultPath, resultJson);

                        testStep = Space14TestSteps.EndTest;
                        break;
                        
                }
            }
        }        
        #endregion
    }
}
