using System;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;
using OES.Models;
using Questions.Models;
using System.Linq;

namespace OES.Controllers
{
    public class HomeController : Controller
    {
        OnlineExamDbContext db = new OnlineExamDbContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string Password, string Role)
        {
            if (Role == "Teacher")
            {
                var teacher = db.Teachers.FirstOrDefault(e => e.TeacherEmail == Email && e.TeacherPassword == Password);
                if (teacher != null)
                {
                    int teacherId = teacher.TeacherId;
                    return RedirectToAction("TeacherDashboard", "Teacher", new { teacherId = teacherId });
                }
                else
                {
                    ViewBag.Error = "Invalid Teacher Credentials.";
                }
            }
            else if (Role == "Student")
            {
                //if (Role == "Student")
                //{
                //    var student = db.Students.FirstOrDefault(e => e.StudentEmail == Email && e.StudentPassword == Password);
                //    if (student != null)
                //    {
                //        int StudentId = student.StudentId;
                //        return RedirectToAction("TeacherDashboard", "Teacher", new { teacherId =StudentId });
                //    }
                //    else
                //    {
                //        ViewBag.Error = "Invalid Teacher Credentials.";
                //    }
                //}
            }
            else
            {
                // Handle admin login
            }
            return View();
        }



        // Student Signup
        [HttpGet]
        public ActionResult StudentSignup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentSignup(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                ViewBag.SuccessMessage = "Student registration successful!";
            }
            else
            {
                ViewBag.ErrorMessage = "Registration failed. Please check the entered information.";
            }
            return View(student);
        }

        // Teacher Signup
        [HttpGet]
        public ActionResult TeacherSignup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TeacherSignup(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Teachers.Add(teacher);
                db.SaveChanges();
                ViewBag.SuccessMessage = "Teacher registration successful!";
            }
            else
            {
                ViewBag.ErrorMessage = "Registration failed. Please check the entered information.";
            }
            return View(teacher);
        }

        // Recruiter Signup (Implement similarly)
        //[HttpGet]
        //public ActionResult RecruiterSignup()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult RecruiterSignup(Recruiter recruiter)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Recruiters.Add(recruiter);
        //        db.SaveChanges();
        //        ViewBag.SuccessMessage = "Recruiter registration successful!";
        //    }
        //    else
        //    {
        //        ViewBag.ErrorMessage = "Registration failed. Please check the entered information.";
        //    }
        //    return View(recruiter);
        //}


        [HttpPost]
        public JsonResult CreateOtp(string FullName, string Email, string Mobile, string UserName, string Password)
        {
            try
            {
                // Generate OTP
                Random random = new Random();
                string otp = random.Next(100000, 999999).ToString();

                // Store OTP in Session
                Session["GeneratedOtp"] = otp;
                Session["Email"] = Email;

                // Send OTP Email
                //MailMessage message = new MailMessage
                //{
                //    From = new MailAddress("tusharsutar799@gmail.com"),
                //    Subject = "Your OTP Code",
                //    Body = $"Hello {FullName},\n\nYour OTP is: {otp}. Please use this code to verify your identity.\n\nThank you!",
                //};
                MailMessage message = new MailMessage
                {
                    From = new MailAddress("tusharsutar799@gmail.com"),
                    Subject = "Your OTP Code",
                    Body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    line-height: 1.6;
                    background-color: #f9f9f9;
                    color: #333;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #fff;
                    border: 1px solid #ddd;
                    border-radius: 8px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    padding: 20px;
                }}
                .header {{
                    text-align: center;
                    padding: 10px 0;
                    background-color: #4CAF50;
                    color: white;
                    font-size: 20px;
                    border-radius: 8px 8px 0 0;
                }}
                .content {{
                    text-align: center;
                    margin: 20px 0;
                }}
                .otp {{
                    display: inline-block;
                    padding: 10px 20px;
                    background-color: #4CAF50;
                    color: white;
                    font-size: 24px;
                    font-weight: bold;
                    border-radius: 5px;
                    margin: 10px 0;
                }}
                .footer {{
                    text-align: center;
                    font-size: 12px;
                    color: #999;
                    margin-top: 20px;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    Your OTP Code
                </div>
                <div class='content'>
                    <p>Hello <strong>{FullName}</strong>,</p>
                    <p>Your OTP is:</p>
                    <div class='otp'>{otp}</div>
                    <p>Please use this code to verify your identity.</p>
                    <p>Thank you!</p>
                </div>
                <div class='footer'>
                    <p>Team Examino</p>
                </div>
            </div>
        </body>
        </html>",
                    IsBodyHtml = true // Enable HTML in the email body
                };

                message.To.Add(Email);

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("tusharsutar799@gmail.com", "jnsfjrhp rsaz ayut"),
                    EnableSsl = true
                };
                client.Send(message);

                return Json(new { success = true, message = "OTP sent successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to send OTP: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult VerifyOtp(string[] Otp)
        {
            string enteredOtp = string.Join("", Otp);
            string storedOtp = Session["GeneratedOtp"] as string;

            if (!string.IsNullOrEmpty(storedOtp) && enteredOtp == storedOtp)
            {
                TempData["Message"] = "OTP verified successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Invalid OTP. Please try again.";
                return RedirectToAction("TeacherDashboard", "Teacher");
            }
        }


    }
}
