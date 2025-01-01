using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OES.Models
{
    public class CreateBatch
    {
        [Key]
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public string InstitutesName { get; set; }
        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }
    }
}