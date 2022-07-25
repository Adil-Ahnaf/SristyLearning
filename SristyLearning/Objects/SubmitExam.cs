using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SristyLearning.Objects
{
    public class SubmitExam
    {
        public int ExamId { get; set; }
        public string StudentId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerId { get; set; }
    }
}
