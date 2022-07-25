using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SristyLearning.Objects
{
    public class ExamResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TotalQuestion { get; set; }
        public int Answered { get; set; }
        public int NotAnswered { get; set; }
        public float TotalMark { get; set; }
        public float GetMark { get; set; }
        public string Result { get; set; }
    }
}
