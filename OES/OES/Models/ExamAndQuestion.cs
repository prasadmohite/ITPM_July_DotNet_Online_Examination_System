using Questions.Models;
using Questions.Models.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OES.Models
{
    public class ExamAndQuestion
    {
        public CreateExamInfo createExamInfo { get; set; }
        public List<CreateQuestions> Questions { get; set; }
    }
}