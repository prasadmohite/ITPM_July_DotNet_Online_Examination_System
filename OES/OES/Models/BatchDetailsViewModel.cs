using Questions.Models;
using Questions.Models.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OES.Models
{
    public class BatchDetailsViewModel
    {
        public CreateBatch Batch { get; set; }
        public List<Student> Students { get; set; }
        
    }
}