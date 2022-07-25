using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SristyLearning.Objects
{
    public class ExamList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TotalQuestion { get; set; }
        public TimeSpan Duration { get; set; }
        public float PassMarkPercent { get; set; }
        public bool IsActive { get; set; }
    }
}
