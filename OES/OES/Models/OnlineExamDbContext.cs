using OES.Models;
using Questions.Models.Exam;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Questions.Models
{
    public class OnlineExamDbContext:DbContext
    {   
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<CreateBatch> CreateBatch { get; set; }
        public DbSet<AddStudentBatch> AddStudentBatch { get; set; }
        public DbSet<CreateExamInfo> createExamInfo {  get; set; }  
        public DbSet<CreateQuestions> questions {  get; set; }  
    }
}