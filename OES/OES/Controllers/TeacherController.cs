using OES.Models;
using Questions.Models;
using Questions.Models.Exam;
using System;
using System.Linq;
using System.Web.Mvc;

namespace OES.Controllers
{
    public class TeacherController : Controller
    {
        OnlineExamDbContext db = new OnlineExamDbContext();

        // GET: Teacher
        public ActionResult TeacherDashboard(int teacherId)
        {
            var batches = db.CreateBatch.Where(e => e.TeacherId == teacherId).ToList();
            TempData["teacherid"] = teacherId;
            return View(batches);
        }

        [HttpPost]
        public ActionResult CreateBatch(string TeacherId, string BatchName, string InstitutesName)
        {
            CreateBatch createBatch = new CreateBatch
            {
                BatchName = BatchName,
                InstitutesName = InstitutesName
            };
            int tid = int.Parse(TeacherId);
            createBatch.TeacherId = tid;

            db.CreateBatch.Add(createBatch);
            var result = db.SaveChanges();

            if (result > 0)
            {
                TempData["Status"] = "Batch created successfully!";
            }
            else
            {
                TempData["Status"] = "There was an issue creating the batch. Please try again.";
            }
            return RedirectToAction("TeacherDashboard", "Teacher", new { teacherId = tid });
        }
        public ActionResult GoToBatch(int BatchId)
        {
            var batch = db.CreateBatch.FirstOrDefault(b => b.BatchId == BatchId);
            var studentsInBatch = db.AddStudentBatch.Where(e => e.BatchId == BatchId).Select(e => e.Student).ToList();

            var viewModel = new BatchDetailsViewModel
            {
                Batch = batch,
                Students = studentsInBatch
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult AddStudentToBatch(int StudentId, int batchid)
        {
            var CheckStudent = db.Students.FirstOrDefault(e => e.StudentId == StudentId);
            if (CheckStudent != null)
            {
                var student = db.AddStudentBatch.FirstOrDefault(e => e.StudentId == StudentId && e.BatchId == batchid);
                if (student == null)
                {
                    AddStudentBatch addStudentBatch = new AddStudentBatch()
                    {
                        StudentId = StudentId,
                        BatchId = batchid
                    };
                    db.AddStudentBatch.Add(addStudentBatch);
                    var result = db.SaveChanges();
                    if (result > 0)
                    {
                        TempData["Status"] = "Student added successfully!";
                    }
                    else
                    {
                        TempData["Status"] = "Unable to add student.";
                    }
                }
                else
                {
                    TempData["Status"] = "Student is already in the batch.";
                }
            }
            else
            {
                TempData["Status"] = "Student does not exist.";

            }
            return RedirectToAction("GoToBatch", new { BatchId = batchid });
        }
        [HttpPost]
        public ActionResult RemoveStudentFromBatch(int studentId, int batchid)
        {
            var studentBatchRecord = db.AddStudentBatch.FirstOrDefault(e => e.StudentId == studentId && e.BatchId == batchid);

            if (studentBatchRecord != null)
            {
                db.AddStudentBatch.Remove(studentBatchRecord);
                db.SaveChanges();

                TempData["Status"] = "Student removed successfully!";
            }
            else
            {
                TempData["Status"] = "Student not found in the batch.";
            }

            return RedirectToAction("GoToBatch", new { BatchId = batchid });
        }
        [HttpPost]
        public ActionResult CreateExam(int batchid, string ExamName, string ExamSubName, DateTime ExamDate, int ExamMarks, int ExamDuration)
        {
            var createExamInfo = new CreateExamInfo()
            {
                BatchId = batchid,
                ExamName = ExamName,
                ExamSubName = ExamSubName,
                ExamDate = ExamDate,
                ExamMarks = ExamMarks,
                ExamDuration = ExamDuration,
                ExamStatus = false
            };
            db.createExamInfo.Add(createExamInfo);
            db.SaveChanges();
            var examInfo = db.createExamInfo.FirstOrDefault(e => e.BatchId == batchid && e.ExamName == ExamName && e.ExamSubName == ExamSubName);
            return RedirectToAction("CreateExam2", new { examId = examInfo.ExamId });
        }
        public ActionResult CreateExam2(int examId)
        {
            var createExamInfo = db.createExamInfo.FirstOrDefault(e => e.ExamId == examId);
            var questions = db.questions.Where(q => q.ExamId == examId).ToList();

            var examAndQuestion = new ExamAndQuestion()
            {
                createExamInfo = createExamInfo,
                Questions = questions
            };
            return View(examAndQuestion);
        }
        [HttpPost]
        public ActionResult AddQuestion(int examId, int QuestionNo, string Question, string Option1, string Option2, string Option3, string Option4, string CorrectOption)
        {
            var question = new CreateQuestions()
            {
                ExamId = examId,
                QuestionNo = QuestionNo,
                Question = Question,
                Option1 = Option1,
                Option2 = Option2,
                Option3 = Option3,
                Option4 = Option4,
                CorrectOption = CorrectOption
            };
            db.questions.Add(question);
            db.SaveChanges();

            TempData["Status"] = "Question added successfully!";
            return RedirectToAction("CreateExam2", new { examId });
        }
        public ActionResult EditQuestion(int questionId)
        {
            var question = db.questions.FirstOrDefault(q => q.QuestionId == questionId);
            return View(question);
        }
        [HttpPost]
        public ActionResult UpdateQuestion(int questionId, int examId, int questionNo, string question, string option1, string option2, string option3, string option4, string correctOption)
        {
            var existingQuestion = db.questions.FirstOrDefault(q => q.QuestionId == questionId);

            if (existingQuestion != null)
            {
                existingQuestion.QuestionNo = questionNo;
                existingQuestion.Question = question;
                existingQuestion.Option1 = option1;
                existingQuestion.Option2 = option2;
                existingQuestion.Option3 = option3;
                existingQuestion.Option4 = option4;
                existingQuestion.CorrectOption = correctOption;

                db.SaveChanges();

                TempData["Status"] = "Question updated successfully!";
            }
            else
            {
                TempData["Status"] = "Question not found!";
            }

            return RedirectToAction("CreateExam2", new { examId });
        }
        public ActionResult GoToExams(int batchid)
        {
            var exams = db.createExamInfo.Where(e => e.BatchId == batchid).ToList();
            return View(exams);
        }
        public ActionResult StartExamExams(int examid)
        {
            var exam = db.createExamInfo.FirstOrDefault(e => e.ExamId == examid);
            exam.ExamStatus = true;
            db.SaveChanges();
            return RedirectToAction("GoToExams", new { batchid = exam.BatchId });
        }
        public ActionResult EditExamExams(int examid)
        {
            return RedirectToAction("Index", "Home");
        }
        public ActionResult DeleteExamExams(int examid)
        {
            var questionList = db.questions.Where(e => e.ExamId == examid).ToList();
            if (questionList != null && questionList.Any())
            {
                db.questions.RemoveRange(questionList);
                db.SaveChanges();
            }
            var exam = db.createExamInfo.FirstOrDefault(e => e.ExamId == examid);
            int batchID = exam.BatchId;
            if (exam != null)
            {
                db.createExamInfo.Remove(exam);
                db.SaveChanges();
            }
            TempData["Status"] = "Exam Delete Successfully";
            return RedirectToAction("GoToExams", new { batchid = batchID });
        }
    }
}
