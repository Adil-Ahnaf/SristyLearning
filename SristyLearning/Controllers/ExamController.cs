using AspNetCore.Identity.Dapper.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SristyLearning.Data;
using SristyLearning.Data.SqlDb;
using SristyLearning.Helpers;
using SristyLearning.Models;
using SristyLearning.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SristyLearning.Controllers
{
    public class ExamController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDataAccess _dataAccess;
        private readonly IEmailHelper emailHelper;

        public ExamController(IConfiguration configuration, UserManager<ApplicationUser> userManager, IDataAccess dataAccess, IEmailHelper emailHelper)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            _dataAccess = dataAccess;
            this.emailHelper = emailHelper;
        }

        [Authorize]
        public IActionResult Index()
        {
            ExamListViewModel model = new ExamListViewModel();
            List<ExamList> examLists = new List<ExamList>();

            Student student = new Student();
            student.StudentId = userManager.GetUserId(HttpContext.User);

            examLists = _dataAccess.GetList<ExamList>(SpConstants.AvaliableExamList, student);
            model.AllExam = examLists;

            return View(model);
        }

        [Authorize]
        public IActionResult ExamResult()
        {
            ExamResultViewModel model = new ExamResultViewModel();
            List<ExamResult> examResults = new List<ExamResult>();

            Student student = new Student();
            student.StudentId = userManager.GetUserId(HttpContext.User);

            examResults = _dataAccess.GetList<ExamResult>(SpConstants.GetExamResult, student);
            model.AllResult = examResults;
            return View(model);
        }

        public IActionResult DetailedResult(int examId)
        {
            DetailedResultViewModel model = new DetailedResultViewModel();
            List<DetailedResult> detailedResults = new List<DetailedResult>();

            StudentDetailedResult student = new StudentDetailedResult();
            student.StudentId = userManager.GetUserId(HttpContext.User);
            student.ExamId = examId;

            detailedResults = _dataAccess.GetList<DetailedResult>(SpConstants.GetResultDetails, student);
            model.AllDetails = detailedResults;
            return View(model);
        }

        public IActionResult ExamInProcessing(string examTime, int examId)
        {
            ExamQuestionId exam = new ExamQuestionId();
            exam.ExamId = examId;

            //ExamListViewModel model = new ExamListViewModel();
            StudentExamviewModel model = new StudentExamviewModel();

            //List<ExamQuestion> questions = new List<ExamQuestion>();
            //List<ExamAnswer> answers = new List<ExamAnswer>();

            model.ExamQuestions = _dataAccess.GetList<ExamQuestion>(SpConstants.GetQuestion, exam);
            model.ExamAnswers = _dataAccess.GetList<ExamAnswer>(SpConstants.GetOption, exam);


            //string stringjson = JsonConvert.SerializeObject(questions);

            //string path = @"D:\Intern Codes\SristyLearningAppSln\SristyLearning\Json\exam_question.json";


            ////Delete old file
            //string rootFolderPath = @"D:\Intern Codes\SristyLearningAppSln\SristyLearning\Json";
            //string filesToDelete = @"exam_question.json"; 
            //string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
            //foreach (string file in fileList)
            //{
            //    //System.Diagnostics.Debug.WriteLine(file + "will be deleted");
            //    System.IO.File.Delete(file);
            //}

            ////Write into new JSON file
            //using (var tw = new StreamWriter(path, true))
            //{
            //    tw.WriteLine(stringjson.ToString());
            //    tw.Close();
            //}

            var minute = examTime.Substring(3, 2);
            ViewBag.ExamId = examId;
            model.ExamDuration = minute;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignExam(int examId, int[] questionId, string[] answer)
        {
            try
            {


                //for (int i = 0; i < questionId.Length; i++)
                //{
                //    SubmitExam submitExam = new SubmitExam();
                //    submitExam.ExamId = examId;
                //    submitExam.StudentId = userManager.GetUserId(HttpContext.User);
                //    submitExam.QuestionId = questionId[i];
                //    submitExam.AnswerId = answer[i];

                //    _dataAccess.Execute(SpConstants.AssignExam, submitExam);
                //}

                Student student = new Student();
                student.StudentId = userManager.GetUserId(HttpContext.User);


                //_dataAccess.Execute(SpConstants.SubmitAnswer, student);


                //create a pdf file
                var fileName = User.Identity.Name + '_' + ".pdf";
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();


                ExamResultViewModel model = new ExamResultViewModel();
                List<ExamResult> examResults = new List<ExamResult>();


                examResults = _dataAccess.GetList<ExamResult>(SpConstants.GetExamResult, student);
                model.AllResult = examResults;

                /*doc.Add(new Paragraph("Hello"));*/
                doc.Add(new Paragraph("Your Exam Result in Below:"));
                doc.Add(new Paragraph("Exam Id: " + model.AllResult[0].Id));
                doc.Add(new Paragraph("Exam Title: " + model.AllResult[0].Title));
                doc.Add(new Paragraph("TotalQuestion: " + model.AllResult[0].TotalQuestion));
                doc.Add(new Paragraph("Answered: " + model.AllResult[0].Answered));
                doc.Add(new Paragraph("NotAnswered: " + model.AllResult[0].NotAnswered));
                doc.Add(new Paragraph("TotalMark: " + model.AllResult[0].TotalMark));
                doc.Add(new Paragraph("GetMark: " + model.AllResult[0].GetMark));
                doc.Add(new Paragraph("Result: " + model.AllResult[0].Result));

                var Filepath = Path.Combine(@"D:\Intern Codes\SristyLearningAppSln\SristyLearning\SristyLearning\" + fileName);

                var userid = userManager.GetUserId(HttpContext.User);
                ApplicationUser user = userManager.FindByIdAsync(userid).Result;
                var studentemail = user.Email;



                await emailHelper.SendAsync("learners.acdemy123@gmail.com",
                    studentemail,
                    "Exam Submit Successfully",
                    string.Format("Congratulations." +
                    "We are very happy that you have successfully give your online Exam." +
                    "Your exam result attached in this PDF below" +
                    "Done"), null
                    //"Click <a href='{0}'>here</a> to Download", Url.ActionLink(Filepath)), Filepath
                    );

            }
            catch(Exception ex) {
                var mesage = ex.Message;

            }
            return RedirectToAction("Index", "Exam");

        }
    }
}
