using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SristyLearning.Objects
{
    public class ExamAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }
}
