using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Questions.Models.Exam
{
    public class CreateQuestions
    {
        [Key]
        public int QuestionId { get; set; }
        public int ExamId {  set; get; }

        [ForeignKey("ExamId")]  
        public virtual CreateExamInfo CreateExamInfo { get; set; }
        public int QuestionNo { get; set; }
        public String Question { get; set; }
        public String Option1 { get; set; }
        public String Option2 { get; set; }
        public String Option3 { get; set; }
        public String Option4 { get; set; }
        public string CorrectOption { get; set; }
    }
}