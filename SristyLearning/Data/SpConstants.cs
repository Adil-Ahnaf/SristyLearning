using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SristyLearning.Data
{
    public static class SpConstants
    {
        public static string ExamListGet = "usp_ViewAvaliableExam";
        public static string UpdateExamStatus = "usp_UpdateExamStatus";
        public static string AvaliableExamList = "usp_ViewAvaliableExamForStudent";
        public static string GetQuestion = "usp_ViewQuestions";
        public static string GetOption = "usp_ViewExamQuestionOptions";
        public static string GetExamResult = "usp_ViewResult";
        public static string GetResultDetails = "usp_ViewDetailedResult";
        public static string AllStudentResults = "usp_AllStudentResult";
        public static string AssignExam = "usp_AssignExam";
        public static string SubmitAnswer = "usp_SubmitAnswer";
    }
}
