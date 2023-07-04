using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace WebApplication3.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {
        }
        [BindProperty, Required(ErrorMessage = "The First Name is required!")]
        [Display(Name ="First Name*")]
        public string FirstName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Last Name is required!")]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; } = "";

        [BindProperty, Required(ErrorMessage = "The Email is required!"),EmailAddress]
        [Display(Name = "Email*")]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Phone { get; set; } = "";

        [BindProperty,Required]
        [Display(Name = "Subject*")]
        public string Subject { get; set; } = "";
     
        [BindProperty, Required(ErrorMessage = "The Message is required!"),
            MinLength(5,ErrorMessage = "Message should be at least 5 characters"),
            MaxLength(1024, ErrorMessage = "Message should be less than 1024 characters")]
        [Display(Name = "Message*")]
        public string Message { get; set; } = "";

        public List<SelectListItem> SubjectList { get; } = new List<SelectListItem>
        {
            new SelectListItem {Value = "Order Status", Text = "Order Status"},
            new SelectListItem {Value = "Lectures", Text = "Lectures"},
            new SelectListItem {Value = "Internship Application", Text = "Internship Application"},
            new SelectListItem {Value = "Other", Text = "Other"},
        };

        public string SuccessMessage { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public void OnPost()
        {
   

            //required field is empty write this
            if(!ModelState.IsValid)
            {
                //Error message
                ErrorMessage = "please fill all required fields";
                return;
            }

            
            
            if (Phone == null) Phone = "";
            //adding messages to the databases and send confirmation email to the student
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESSNEW;Initial Catalog=bestshop;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO messsages" +
                        "(firstname, lastname, email, phone, subject, message) VALUES" +
                        "(@firstname, @lastname, @email, @phone, @subject, @message);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@firstname", FirstName);
                        command.Parameters.AddWithValue("@lastname", LastName);
                        command.Parameters.AddWithValue("@email", Email);
                        command.Parameters.AddWithValue("@phone", Phone);
                        command.Parameters.AddWithValue("@subject", Subject);
                        command.Parameters.AddWithValue("@message", Message);

                        command.ExecuteNonQuery();
                    }
                }
            }

            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return;
            }
            
            SuccessMessage = "Your message has been received correctly";

            FirstName = "";
            LastName = "";
            Email = "";
            Phone = "";
            Subject = "";
            Message = "";

            ModelState.Clear();
        }

    }
}
