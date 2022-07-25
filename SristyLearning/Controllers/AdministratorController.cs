using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using SristyLearning.Data;
using SristyLearning.Data.SqlDb;
using SristyLearning.Models;
using SristyLearning.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SristyLearning.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IDataAccess _dataAccess;

        public AdministratorController(IConfiguration configuration, IDataAccess dataAccess)
        {
            this.configuration = configuration;
            _dataAccess = dataAccess;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExamList()
        {
            //using (IDbConnection db = new SqlConnection(configuration.GetConnectionString("SristyLearningDBConnection")))
            //{

            //    examLists = db.Query<ExamList>("Select * From ExamList").ToList();
            //}
            //return View(examLists);

            ExamListViewModel model = new ExamListViewModel();
            List<ExamList> examLists = new List<ExamList>();

            examLists = _dataAccess.GetList<ExamList>(SpConstants.ExamListGet);
            model.AllExam = examLists;
            return View(model);
        }

        public IActionResult UpdateExamStatus(int examId, bool activeStatus)
        {
            UpdateExam updateExam = new UpdateExam();

            updateExam.ExamId = examId;

            if (activeStatus == true)
            {
                updateExam.IsActive = false;
            }
            else
            {
                updateExam.IsActive = true;
            }

            _dataAccess.Execute(SpConstants.UpdateExamStatus, updateExam);

            return RedirectToAction("ExamList");
        }

        public IActionResult AllStudentResult()
        {
            AllStudentResultViewModel model = new AllStudentResultViewModel();
            List<AllStudentResult> allStudents = new List<AllStudentResult>();

            allStudents = _dataAccess.GetList<AllStudentResult>(SpConstants.AllStudentResults);
            model.AllStudentResults = allStudents;
            return View(model);
        }
    }
}
