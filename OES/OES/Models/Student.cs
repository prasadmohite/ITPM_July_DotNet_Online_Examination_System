using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OES.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        public string Name { get; set; }

      



    }
}