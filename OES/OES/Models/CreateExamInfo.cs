using OES.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Questions.Models
{
    public class CreateExamInfo
    {
        [Key]
        public int ExamId {  get; set; }
        public string ExamName { get; set; }
        public string ExamSubName { get; set; }
        public DateTime ExamDate { get; set; }
        public int ExamMarks { get; set; }
        public int ExamDuration {  get; set; }
        public Boolean ExamStatus { get; set; }
        public int BatchId { get; set; }
        [ForeignKey("BatchId")]
        public virtual CreateBatch CreateBatch { get; set; }

    }
}