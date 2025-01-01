using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OES.Models
{
    public class AddStudentBatch
    {
        [Key]
        public int Id { get; set; }
        public int BatchId { get; set; }
        [ForeignKey("BatchId")]
        public virtual CreateBatch CreateBatch { get; set; }
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}