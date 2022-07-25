using SristyLearning.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SristyLearning.Models
{
    public class StudentExamviewModel
    {
        public List<ExamQuestion> ExamQuestions{ get; set; }
        public List<ExamAnswer> ExamAnswers { get; set; }
        public string ExamDuration { get; set; }
    }
}
