namespace OES.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateALL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AddStudentBatches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BatchId = c.Int(nullable: false),
                        StudentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CreateBatches", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.BatchId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.CreateBatches",
                c => new
                    {
                        BatchId = c.Int(nullable: false, identity: true),
                        BatchName = c.String(),
                        InstitutesName = c.String(),
                        TeacherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BatchId)
                .ForeignKey("dbo.Teachers", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        TeacherId = c.Int(nullable: false, identity: true),
                        TeacherEmail = c.String(),
                        TeacherPassword = c.String(),
                    })
                .PrimaryKey(t => t.TeacherId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.StudentId);
            
            CreateTable(
                "dbo.CreateExamInfoes",
                c => new
                    {
                        ExamId = c.Int(nullable: false, identity: true),
                        ExamName = c.String(),
                        ExamSubName = c.String(),
                        ExamDate = c.DateTime(nullable: false),
                        ExamMarks = c.Int(nullable: false),
                        ExamDuration = c.Int(nullable: false),
                        ExamStatus = c.Boolean(nullable: false),
                        BatchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExamId)
                .ForeignKey("dbo.CreateBatches", t => t.BatchId, cascadeDelete: true)
                .Index(t => t.BatchId);
            
            CreateTable(
                "dbo.CreateQuestions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        ExamId = c.Int(nullable: false),
                        QuestionNo = c.Int(nullable: false),
                        Question = c.String(),
                        Option1 = c.String(),
                        Option2 = c.String(),
                        Option3 = c.String(),
                        Option4 = c.String(),
                        CorrectOption = c.String(),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.CreateExamInfoes", t => t.ExamId, cascadeDelete: true)
                .Index(t => t.ExamId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreateQuestions", "ExamId", "dbo.CreateExamInfoes");
            DropForeignKey("dbo.CreateExamInfoes", "BatchId", "dbo.CreateBatches");
            DropForeignKey("dbo.AddStudentBatches", "StudentId", "dbo.Students");
            DropForeignKey("dbo.AddStudentBatches", "BatchId", "dbo.CreateBatches");
            DropForeignKey("dbo.CreateBatches", "TeacherId", "dbo.Teachers");
            DropIndex("dbo.CreateQuestions", new[] { "ExamId" });
            DropIndex("dbo.CreateExamInfoes", new[] { "BatchId" });
            DropIndex("dbo.CreateBatches", new[] { "TeacherId" });
            DropIndex("dbo.AddStudentBatches", new[] { "StudentId" });
            DropIndex("dbo.AddStudentBatches", new[] { "BatchId" });
            DropTable("dbo.CreateQuestions");
            DropTable("dbo.CreateExamInfoes");
            DropTable("dbo.Students");
            DropTable("dbo.Teachers");
            DropTable("dbo.CreateBatches");
            DropTable("dbo.AddStudentBatches");
        }
    }
}
