namespace DllTestForm {
    partial class Form1 {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent() {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.nudX = new System.Windows.Forms.NumericUpDown();
            this.nudY = new System.Windows.Forms.NumericUpDown();
            this.tbxValue = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.nudTLY = new System.Windows.Forms.NumericUpDown();
            this.nudTLX = new System.Windows.Forms.NumericUpDown();
            this.nudBRY = new System.Windows.Forms.NumericUpDown();
            this.nudBRX = new System.Windows.Forms.NumericUpDown();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.tbxFilePath = new System.Windows.Forms.TextBox();
            this.btnLoadData = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button7 = new System.Windows.Forms.Button();
            this.nudDistance = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnRefreshBase = new System.Windows.Forms.Button();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.nudRoiTop = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nudShrink = new System.Windows.Forms.NumericUpDown();
            this.nudRoiRight = new System.Windows.Forms.NumericUpDown();
            this.nudRoiBottom = new System.Windows.Forms.NumericUpDown();
            this.nudRoiLeft = new System.Windows.Forms.NumericUpDown();
            this.button6 = new System.Windows.Forms.Button();
            this.txbBarcode = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnLoadValue = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTLY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTLX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBRY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBRX)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoiTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudShrink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoiRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoiBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoiLeft)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(236, 342);
            this.textBox1.TabIndex = 0;
            // 
            // nudX
            // 
            this.nudX.Location = new System.Drawing.Point(6, 38);
            this.nudX.Maximum = new decimal(new int[] {
            84,
            0,
            0,
            0});
            this.nudX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudX.Name = "nudX";
            this.nudX.Size = new System.Drawing.Size(75, 22);
            this.nudX.TabIndex = 1;
            this.nudX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudX.ValueChanged += new System.EventHandler(this.nudX_ValueChanged);
            // 
            // nudY
            // 
            this.nudY.Location = new System.Drawing.Point(87, 39);
            this.nudY.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudY.Name = "nudY";
            this.nudY.Size = new System.Drawing.Size(75, 22);
            this.nudY.TabIndex = 1;
            this.nudY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudY.ValueChanged += new System.EventHandler(this.nudY_ValueChanged);
            // 
            // tbxValue
            // 
            this.tbxValue.Location = new System.Drawing.Point(87, 67);
            this.tbxValue.Name = "tbxValue";
            this.tbxValue.Size = new System.Drawing.Size(100, 22);
            this.tbxValue.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Average";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // nudTLY
            // 
            this.nudTLY.Location = new System.Drawing.Point(87, 38);
            this.nudTLY.Maximum = new decimal(new int[] {
            3199,
            0,
            0,
            0});
            this.nudTLY.Name = "nudTLY";
            this.nudTLY.Size = new System.Drawing.Size(75, 22);
            this.nudTLY.TabIndex = 4;
            this.nudTLY.ValueChanged += new System.EventHandler(this.nudTLY_ValueChanged);
            // 
            // nudTLX
            // 
            this.nudTLX.Location = new System.Drawing.Point(6, 38);
            this.nudTLX.Maximum = new decimal(new int[] {
            3999,
            0,
            0,
            0});
            this.nudTLX.Name = "nudTLX";
            this.nudTLX.Size = new System.Drawing.Size(75, 22);
            this.nudTLX.TabIndex = 5;
            this.nudTLX.ValueChanged += new System.EventHandler(this.nudTLX_ValueChanged);
            // 
            // nudBRY
            // 
            this.nudBRY.Location = new System.Drawing.Point(87, 80);
            this.nudBRY.Maximum = new decimal(new int[] {
            3199,
            0,
            0,
            0});
            this.nudBRY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBRY.Name = "nudBRY";
            this.nudBRY.Size = new System.Drawing.Size(75, 22);
            this.nudBRY.TabIndex = 6;
            this.nudBRY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudBRX
            // 
            this.nudBRX.Location = new System.Drawing.Point(6, 80);
            this.nudBRX.Maximum = new decimal(new int[] {
            3999,
            0,
            0,
            0});
            this.nudBRX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBRX.Name = "nudBRX";
            this.nudBRX.Size = new System.Drawing.Size(75, 22);
            this.nudBRX.TabIndex = 7;
            this.nudBRX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(87, 108);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 22);
            this.textBox2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "Key Index";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(85, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "Roi Index";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Top, Left";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Bottom, Right";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(87, 137);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 22);
            this.textBox3.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 137);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Deviation";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(254, 112);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(73, 22);
            this.button3.TabIndex = 3;
            this.button3.Text = "Test Flow";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(87, 95);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(100, 22);
            this.textBox4.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.nudX);
            this.groupBox1.Controls.Add(this.nudY);
            this.groupBox1.Controls.Add(this.tbxValue);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(801, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 124);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Get Height && Dev. by Index";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "Deviation:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "Height:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudTLX);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.nudBRY);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.nudBRX);
            this.groupBox2.Controls.Add(this.nudTLY);
            this.groupBox2.Location = new System.Drawing.Point(801, 159);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 164);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Get Height && Dev. form BMP";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(31, 360);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(186, 39);
            this.button4.TabIndex = 14;
            this.button4.Text = "Copy To Clip  Board";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(333, 112);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 22);
            this.textBox5.TabIndex = 2;
            this.textBox5.Text = "00:00:00.00 sec.";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(254, 12);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 15;
            this.btnOpenFile.Text = "Open Data";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // tbxFilePath
            // 
            this.tbxFilePath.Location = new System.Drawing.Point(254, 41);
            this.tbxFilePath.Multiline = true;
            this.tbxFilePath.Name = "tbxFilePath";
            this.tbxFilePath.Size = new System.Drawing.Size(179, 65);
            this.tbxFilePath.TabIndex = 16;
            // 
            // btnLoadData
            // 
            this.btnLoadData.Location = new System.Drawing.Point(333, 12);
            this.btnLoadData.Name = "btnLoadData";
            this.btnLoadData.Size = new System.Drawing.Size(75, 23);
            this.btnLoadData.TabIndex = 15;
            this.btnLoadData.Text = "Load Data";
            this.btnLoadData.UseVisualStyleBackColor = true;
            this.btnLoadData.Click += new System.EventHandler(this.btnLoadData_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(339, 459);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(254, 166);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 18;
            this.button7.Text = "roi distance";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // nudDistance
            // 
            this.nudDistance.Location = new System.Drawing.Point(333, 165);
            this.nudDistance.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudDistance.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDistance.Name = "nudDistance";
            this.nudDistance.Size = new System.Drawing.Size(100, 22);
            this.nudDistance.TabIndex = 19;
            this.nudDistance.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnRefreshBase);
            this.groupBox3.Controls.Add(this.textBox6);
            this.groupBox3.Controls.Add(this.button5);
            this.groupBox3.Controls.Add(this.nudRoiTop);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.nudShrink);
            this.groupBox3.Controls.Add(this.nudRoiRight);
            this.groupBox3.Controls.Add(this.nudRoiBottom);
            this.groupBox3.Controls.Add(this.nudRoiLeft);
            this.groupBox3.Location = new System.Drawing.Point(254, 222);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(179, 254);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "ROI Creator";
            // 
            // btnRefreshBase
            // 
            this.btnRefreshBase.Location = new System.Drawing.Point(6, 222);
            this.btnRefreshBase.Name = "btnRefreshBase";
            this.btnRefreshBase.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshBase.TabIndex = 20;
            this.btnRefreshBase.Text = "Refresh Base";
            this.btnRefreshBase.UseVisualStyleBackColor = true;
            this.btnRefreshBase.Click += new System.EventHandler(this.btnRefreshBase_Click);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(6, 157);
            this.textBox6.Multiline = true;
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(158, 58);
            this.textBox6.TabIndex = 19;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(89, 128);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 17;
            this.button5.Text = "Create";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // nudRoiTop
            // 
            this.nudRoiTop.Location = new System.Drawing.Point(8, 33);
            this.nudRoiTop.Maximum = new decimal(new int[] {
            3999,
            0,
            0,
            0});
            this.nudRoiTop.Name = "nudRoiTop";
            this.nudRoiTop.Size = new System.Drawing.Size(75, 22);
            this.nudRoiTop.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 113);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "Shrink";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "Bottom, Right";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "Top, Left";
            // 
            // nudShrink
            // 
            this.nudShrink.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudShrink.Location = new System.Drawing.Point(8, 128);
            this.nudShrink.Maximum = new decimal(new int[] {
            3199,
            0,
            0,
            0});
            this.nudShrink.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudShrink.Name = "nudShrink";
            this.nudShrink.Size = new System.Drawing.Size(75, 22);
            this.nudShrink.TabIndex = 0;
            this.nudShrink.Value = new decimal(new int[] {
            13,
            0,
            0,
            0});
            // 
            // nudRoiRight
            // 
            this.nudRoiRight.Location = new System.Drawing.Point(89, 75);
            this.nudRoiRight.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.nudRoiRight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRoiRight.Name = "nudRoiRight";
            this.nudRoiRight.Size = new System.Drawing.Size(75, 22);
            this.nudRoiRight.TabIndex = 16;
            this.nudRoiRight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudRoiBottom
            // 
            this.nudRoiBottom.Location = new System.Drawing.Point(8, 75);
            this.nudRoiBottom.Maximum = new decimal(new int[] {
            3999,
            0,
            0,
            0});
            this.nudRoiBottom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRoiBottom.Name = "nudRoiBottom";
            this.nudRoiBottom.Size = new System.Drawing.Size(75, 22);
            this.nudRoiBottom.TabIndex = 15;
            this.nudRoiBottom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudRoiLeft
            // 
            this.nudRoiLeft.Location = new System.Drawing.Point(89, 33);
            this.nudRoiLeft.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.nudRoiLeft.Name = "nudRoiLeft";
            this.nudRoiLeft.Size = new System.Drawing.Size(75, 22);
            this.nudRoiLeft.TabIndex = 14;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(31, 405);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(185, 41);
            this.button6.TabIndex = 22;
            this.button6.Text = "Reload Setting";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // txbBarcode
            // 
            this.txbBarcode.Location = new System.Drawing.Point(254, 137);
            this.txbBarcode.Name = "txbBarcode";
            this.txbBarcode.Size = new System.Drawing.Size(100, 22);
            this.txbBarcode.TabIndex = 23;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(439, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(346, 471);
            this.panel1.TabIndex = 24;
            // 
            // btnLoadValue
            // 
            this.btnLoadValue.Location = new System.Drawing.Point(333, 193);
            this.btnLoadValue.Name = "btnLoadValue";
            this.btnLoadValue.Size = new System.Drawing.Size(75, 23);
            this.btnLoadValue.TabIndex = 15;
            this.btnLoadValue.Text = "Load Value";
            this.btnLoadValue.UseVisualStyleBackColor = true;
            this.btnLoadValue.Click += new System.EventHandler(this.btnLoadValue_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(254, 193);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 15;
            this.button8.Text = "Open Value";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 507);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txbBarcode);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.nudDistance);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.tbxFilePath);
            this.Controls.Add(this.btnLoadValue);
            this.Controls.Add(this.btnLoadData);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTLY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTLX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBRY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBRX)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDistance)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoiTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudShrink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoiRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoiBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRoiLeft)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.NumericUpDown nudX;
        private System.Windows.Forms.NumericUpDown nudY;
        private System.Windows.Forms.TextBox tbxValue;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown nudTLY;
        private System.Windows.Forms.NumericUpDown nudTLX;
        private System.Windows.Forms.NumericUpDown nudBRY;
        private System.Windows.Forms.NumericUpDown nudBRX;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox tbxFilePath;
        private System.Windows.Forms.Button btnLoadData;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.NumericUpDown nudDistance;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.NumericUpDown nudRoiTop;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudShrink;
        private System.Windows.Forms.NumericUpDown nudRoiRight;
        private System.Windows.Forms.NumericUpDown nudRoiBottom;
        private System.Windows.Forms.NumericUpDown nudRoiLeft;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txbBarcode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnLoadValue;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btnRefreshBase;
    }
}

