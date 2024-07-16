using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Result {
    public class ResultObject {
        public string ModelName { get; set; }
        public string Barcode { get; set; }
        public int KeyCount { get; set; }
        public List<ResultValue> ResultValue { get; set; }
    }
    
    public class ResultValue {
        public enum ResultStatus {
            Ingauge,
            OK,
            NG
        }
        public int KeyIndex { get; set; }
        public string Name { get; set; }
        public int RoiCount { get; set; }
        public List<double> Deeps { get; set; }
        public List<double> Heights { get; set; }
        public List<ResultStatus> KeyStatus;
        public List<double> Slant { get; set; }
        public List<bool> SlantResult;
        public List<Dictionary<string, double>> Alignment{ get; set; }
        public List<bool> AlignmentResult;
        public List<double> KeyHeights { get; set; }
        public List<double> NewKeyHeights { get; set; }
        public List<bool> KeyHeightResult { get; set; }

        public ResultValue(int KeyIndex, string Name, int RoiCount, List<double> Deeps, List<double> Heights, List<double> Slant, List<bool> SlantResult, List<Dictionary<string, double>> Alignment, List<bool> AlignmentResult, List<double> KeyHeights, List<double> NewKeyHeights ,List<bool> KeyHeightResult, List<ResultStatus> KeyStatus) {
            this.KeyIndex = KeyIndex;
            this.Name = Name;
            this.RoiCount = RoiCount;
            this.Deeps = Deeps;
            this.Heights = Heights;
            this.Slant = Slant;
            this.SlantResult = SlantResult;
            this.Alignment = Alignment;
            this.AlignmentResult = AlignmentResult;
            this.KeyHeights = KeyHeights;
            this.NewKeyHeights = NewKeyHeights;
            this.KeyHeightResult = KeyHeightResult;
            this.SlantResult = SlantResult;
            this.KeyStatus = KeyStatus;
        }
    }


}
