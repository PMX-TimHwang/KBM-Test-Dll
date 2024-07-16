using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KBMTestDll;
using System.IO;
using Height;

namespace DllTestForm {
    public partial class Form1 : Form {
        string AppFolder = System.IO.Directory.GetCurrentDirectory();
        KBMTester kbmTester;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            kbmTester = new KBMTester();
            string settingFilePath = AppFolder + @"\TestData\test setting.json";
            string baseFilePath = AppFolder + @"\TestData\base setting.json";
            string slantFilePath = AppFolder + @"\TestData\slant setting.json";
            string alignmentFilePath = AppFolder + @"\TestData\alignment setting.json";
            string heightFilePath = AppFolder + @"\TestData\height setting.json";
            this.Text = "KBM DLL [" + kbmTester.GetVersion() + "]";
            textBox1.Text += kbmTester.Initialize(settingFilePath, alignmentFilePath, slantFilePath, baseFilePath, heightFilePath) + "\r\n";

            //textBox1.Text += "=====\r\n";
            //HeightLimit limit = kbmTester.heightSetting.GetLimit("K1#1");
            //textBox1.Text += limit.UpperLimit + "~" + limit.LowerLimit + "\r\n";
            //textBox1.Text += limit.UpperTolerance + "~" + limit.LowerTolerance + "\r\n";

            //limit = kbmTester.heightSetting.GetLimit("K1#4");
            //textBox1.Text += limit.UpperLimit + "~" + limit.LowerLimit + "\r\n";
            //textBox1.Text += limit.UpperTolerance + "~" + limit.LowerTolerance + "\r\n";

            for (int keyNum = 1; keyNum <= kbmTester.GetKeyCount(); keyNum++) {
                textBox1.Text += "=====\r\n";
                textBox1.Text += "Roi_count:" + (keyNum + 1).ToString() + ":" + kbmTester.GetRoiCount(keyNum).ToString() + "\r\n";
                for (int roiNum = 1; roiNum <= kbmTester.GetRoiCount(keyNum); roiNum++) {
                    textBox1.Text += kbmTester.GetRoiTopLeft(keyNum, roiNum) + "," + kbmTester.GetRoiBottomRight(keyNum, roiNum) + "\r\n";
                }
            }
            nudX.ValueChanged -= nudX_ValueChanged;
            nudX.Maximum = kbmTester.GetKeyCount();
            nudX.ValueChanged += nudX_ValueChanged;
        }

        private void nudX_ValueChanged(object sender, EventArgs e) {
            int x, roiCount, y;
            x = (int)nudX.Value;
            roiCount = kbmTester.TestSetting.KeysRoi[x - 1].Count();
            nudY.Maximum = roiCount;
            y = (int)nudY.Value;
            if (y > roiCount) {
                y = roiCount;
                nudX.Value = y;
            }

            tbxValue.Text = kbmTester.GetDeepthValue(x, y).ToString();
            textBox4.Text = kbmTester.GetRoiDeviation(x, y).ToString();
        }

        private void nudY_ValueChanged(object sender, EventArgs e) {
            int x, y;
            x = (int)nudX.Value;
            y = (int)nudY.Value;
            //tbxValue.Text = kmbTester.GetDeepthValue(x, y).ToString();
            //Point p = new Point(x, y);
            tbxValue.Text = kbmTester.GetDeepthValue(x, y).ToString();
            textBox4.Text = kbmTester.GetRoiDeviation(x, y).ToString();
        }

        private void button1_Click(object sender, EventArgs e) {
            textBox2.Text = TestGetDeepthValue();
        }
        public string TestGetDeepthValue() {
            double sumValue = 0;
            double avgValue = 0;
            Point leftTop, rightBottom;

            leftTop = new Point((int)nudTLX.Value, (int)nudTLY.Value);
            rightBottom = new Point((int)nudBRX.Value, (int)nudBRY.Value);

            int count = 0;
            for (int i = leftTop.X; i < rightBottom.X; i++) {
                for (int j = leftTop.Y; j < rightBottom.Y; j++) {
                    Point point = new Point(i, j);
                    count++;
                    sumValue += kbmTester.GetDeepthValue(point);
                }
            }
            avgValue = sumValue / count;
            return avgValue.ToString();
        }

        private void button2_Click(object sender, EventArgs e) {
            textBox3.Text = TestGetDeviationValue();
        }
        public string TestGetDeviationValue() {
            double avgValue = Convert.ToDouble(TestGetDeepthValue());
            double devValue = 0;
            Point leftTop, rightBottom;

            leftTop = new Point((int)nudTLX.Value, (int)nudTLY.Value);
            rightBottom = new Point((int)nudBRX.Value, (int)nudBRY.Value);

            int count = 0;
            for (int i = leftTop.X; i < rightBottom.X; i++)
                for (int j = leftTop.Y; j < rightBottom.Y; j++) {
                    count++;
                    Point point = new Point(i, j);
                    devValue += Math.Pow((kbmTester.GetDeepthValue(point) - avgValue), (double)2);
                }
            devValue = Math.Sqrt(devValue / count);
            return devValue.ToString();
        }
        
        private void button3_Click(object sender, EventArgs e) {
            TimeSpan ts1 = new TimeSpan(DateTime.Now.Ticks);
            Size roiOffset = new Size(0, 0);
            string result = kbmTester.KmbMainTest(txbBarcode.Text, tbxFilePath.Text, AppFolder + String.Format(@"\original value"), roiOffset);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            textBox5.Text = string.Format("{0:00} : {1:00} : {2:00}.{3:00} sec.", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

            //Picture Process
            if (MyImage != null) {
                MyImage.Dispose();
            }
            MyImage = new Bitmap(tbxFilePath.Text);
            Graphics g = Graphics.FromImage(MyImage);
            for (int keyNum = 1; keyNum <= kbmTester.GetKeyCount(); keyNum++) {
                int roiCount = kbmTester.GetRoiCount(keyNum);
                bool totalSlantResult = true;
                foreach (bool slantresult in kbmTester.TestResult.ResultValue[keyNum - 1].SlantResult)
                    totalSlantResult = totalSlantResult & slantresult;

                if (roiCount == 4) {
                    Point tl = kbmTester.GetRoiTopLeft(keyNum, 1, roiOffset);
                    Point topLeft = new Point(tl.Y, tl.X);
                    Point br = kbmTester.GetRoiBottomRight(keyNum, 3, roiOffset);
                    Size recsize = new Size(Point.Subtract(br, (Size)tl).Y, Point.Subtract(br, (Size)tl).X);
                    Rectangle slantRec = new Rectangle(topLeft, recsize);
                    Brush bb = totalSlantResult ? new SolidBrush(Color.Transparent) : new SolidBrush(Color.Red);
                    g.FillRectangle(bb, slantRec);
                } else if (roiCount == 6) {
                    Point tl = kbmTester.GetRoiTopLeft(keyNum, 1, roiOffset);
                    Point topLeft = new Point(tl.Y, tl.X);
                    Point br = kbmTester.GetRoiBottomRight(keyNum, 4, roiOffset);
                    Size recsize = new Size(Point.Subtract(br, (Size)tl).Y, Point.Subtract(br, (Size)tl).X);
                    Rectangle slantRec = new Rectangle(topLeft, recsize);
                    Brush bb = totalSlantResult ? new SolidBrush(Color.Transparent) : new SolidBrush(Color.Red);
                    g.FillRectangle(bb, slantRec);
                }

                for (int roiNum = 1; roiNum <= roiCount; roiNum++) {
                    Point roiPosition = kbmTester.GetRoiTopLeft(keyNum, roiNum, roiOffset);
                    Point drawRoiPoint = new Point(roiPosition.Y, roiPosition.X);
                    Rectangle rec = new Rectangle(drawRoiPoint, new Size(40, 40));
                    Brush bb = kbmTester.TestResult.ResultValue[keyNum - 1].AlignmentResult[roiNum - 1] ? new SolidBrush(Color.Green) : new SolidBrush(Color.Red);
                    g.FillRectangle(bb, rec);
                }

                for (int roiNum = 1; roiNum <= roiCount; roiNum++) {
                    Point roiPosition = kbmTester.GetRoiTopLeft(keyNum, roiNum, roiOffset);
                    Point drawRoiPoint = new Point(roiPosition.Y, roiPosition.X);
                    Rectangle rec = new Rectangle(drawRoiPoint, new Size(30, 30));

                    Brush bb = kbmTester.TestResult.ResultValue[keyNum - 1].KeyHeightResult[roiNum - 1] ? new SolidBrush(Color.Green) : new SolidBrush(Color.Orange);
                    g.FillRectangle(bb, rec);
                }
            }
            g.DrawImage(MyImage, 0, 0);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = (Image)MyImage;
            g.Dispose();
        }

        private void button4_Click(object sender, EventArgs e) {
            Clipboard.SetText(textBox1.Text);
        }

        private void btnOpenFile_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "bmp files (*.*)|*.bmp|tiff files (*.*)|*.tif";
            if (dialog.ShowDialog() == DialogResult.OK) {
                tbxFilePath.Text=dialog.FileName;
            }
        }
        private Bitmap MyImage;
        private void btnLoadData_Click(object sender, EventArgs e) {
            kbmTester.LoadBmpDeepFile(tbxFilePath.Text);
            double[] value = kbmTester.DeepthValue;
            //picture process
            if (MyImage != null) {
                MyImage.Dispose();
            }
            MyImage = new Bitmap(tbxFilePath.Text);
            nudTLX.Maximum = MyImage.Height - 2;
            nudTLY.Maximum = MyImage.Width - 2;
            nudBRX.Maximum = MyImage.Height - 1;
            nudBRY.Maximum = MyImage.Width - 1;
            Graphics g = Graphics.FromImage(MyImage);
            Brush bOK = new SolidBrush(Color.Green);
            Brush bNG = new SolidBrush(Color.Red);
            for (int keyNum = 1; keyNum <= kbmTester.GetKeyCount();keyNum++ ) {
                int roiCount = kbmTester.GetRoiCount(keyNum);
                for (int roiNum = 1; roiNum <= roiCount; roiNum++) {
                    Point roiPosition = kbmTester.GetRoiTopLeft(keyNum, roiNum);
                    Point drawRoiPoint = new Point(roiPosition.Y, roiPosition.X);
                    Rectangle rec = new Rectangle(drawRoiPoint, new Size(20, 20));
                    g.FillRectangle(bOK, rec);
                }
            }
            g.DrawImage(MyImage, 0, 0);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = (Image)MyImage;
            g.Dispose();
        }

        private void nudTLX_ValueChanged(object sender, EventArgs e) {
            nudBRX.Value = nudTLX.Value + 1;
        }

        private void nudTLY_ValueChanged(object sender, EventArgs e) {
            nudBRY.Value = nudTLY.Value + 1;
        }

        private void button7_Click(object sender, EventArgs e) {
            int distance = (int)(nudDistance.Value);
            kbmTester.ChangeAlignmentFindRange(distance);
            kbmTester.SaveAlignmentSetting(AppFolder + @"\Test Data\alignment setting.json");
        }

        private void button5_Click(object sender, EventArgs e) {
            Point tl = new Point((int)(nudRoiTop.Value), (int)(nudRoiLeft.Value));
            Point br = new Point((int)(nudRoiBottom.Value), (int)(nudRoiRight.Value));
            Point[] rois = kbmTester.Create4Rois(tl, br, (int)(nudShrink.Value));
            textBox6.Text = "[";
            foreach (Point p in rois)
                textBox6.Text += String.Format("\"{0},{1}\",",p.X ,p.Y);
            textBox6.Text = textBox6.Text.TrimEnd(',');
            textBox6.Text += "],";
        }

        private void button6_Click(object sender, EventArgs e) {
            textBox1.Text = kbmTester.Initialize(AppFolder + @"\Test Data\test setting.json", AppFolder + @"\Test Data\alignment setting.json", AppFolder + @"\Test Data\slant setting.json", AppFolder + @"\Test Data\base setting.json", AppFolder + @"\Test Data\height setting.json") + "\r\n";
            for (int keyNum = 1; keyNum <= kbmTester.GetKeyCount(); keyNum++) {
                textBox1.Text += "=====\r\n";
                textBox1.Text += "Roi_count:" + (keyNum + 1).ToString() + ":" + kbmTester.GetRoiCount(keyNum).ToString() + "\r\n";
                for (int roiNum = 1; roiNum <= kbmTester.GetRoiCount(keyNum); roiNum++) {
                    textBox1.Text += kbmTester.GetRoiTopLeft(keyNum, roiNum) + "," + kbmTester.GetRoiBottomRight(keyNum, roiNum) + "\r\n";
                }
            }
        }

        private void btnLoadValue_Click(object sender, EventArgs e) {
            kbmTester.LoadDataValue(value);
        }

        List<ushort> value = new List<ushort>();
        string[] strValue;
        private void button8_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == DialogResult.OK) {
                tbxFilePath.Text = dialog.FileName;
                textBox1.Text =  File.ReadAllText(dialog.FileName).Replace("\r\n","\t").TrimEnd();
                string text = textBox1.Text;
                strValue = text.Split(',');
                foreach(string v in strValue)
                    value.Add((ushort)Convert.ToUInt32(v));
            }
        }

        private void btnRefreshBase_Click(object sender, EventArgs e) {
            kbmTester.RefreshBaseFile(tbxFilePath.Text);
        }
        
    }
}
