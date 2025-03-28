
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Online_Job_Portal_MVC.Models
{
    public class RegisterModel
    {
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Username")]
        public string ?Username { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        public string ?Password { get; set; }

        [Required(ErrorMessage = "Please enter Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ?ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter Fullname")]
        public string ?Fullname { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        public string ?Address { get; set; }

        [Required(ErrorMessage = "Please enter Mobile No")]
        public string ? MobileNumber { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string ?Email { get; set; }

        [Required(ErrorMessage = "Please enter Country")]
        public string ?Country { get; set; }

        [Required(ErrorMessage = "Please enter Role")]
        public string ?Role { get;  set; }



        //Retrieve all records from a table
        public List<RegisterModel> getData()
        {
            List<RegisterModel> lstUser = new List<RegisterModel>();
            SqlDataAdapter apt = new SqlDataAdapter("select * from Register", con);
            DataSet ds = new DataSet();
            apt.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstUser.Add(new RegisterModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    Username = dr["Username"].ToString(),
                    Password = dr["Password"].ToString(),
                    ConfirmPassword = dr["ConfirmPassword"].ToString(),
                    Fullname = dr["Fullname"].ToString(),
                    Address = dr["Address"].ToString(),
                    MobileNumber = dr["MobileNumber"].ToString(),
                    Email = dr["Email"].ToString(),
                    Country = dr["Country"].ToString(),
                    Role = dr["Role"].ToString(),

                });
            }
            return lstUser;
        }


        //Insert a record into a database table
         public bool insert(RegisterModel re)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Register (Username, Password, ConfirmPassword, Fullname,Address, MobileNumber, Email, Country, Role) VALUES (@Username, @Password, @ConfirmPassword, @Fullname, @Address, @MobileNumber, @Email, @Country, @Role)", con);

                cmd.Parameters.AddWithValue("@Username", re.Username);
                cmd.Parameters.AddWithValue("@Password", re.Password);
                cmd.Parameters.AddWithValue("@ConfirmPassword", re.ConfirmPassword);
                cmd.Parameters.AddWithValue("@Fullname", re.Fullname);
                cmd.Parameters.AddWithValue("@Address", re.Address);
                cmd.Parameters.AddWithValue("@MobileNumber", re.MobileNumber);
                cmd.Parameters.AddWithValue("@Email", re.Email);
                cmd.Parameters.AddWithValue("@Country", re.Country);
                cmd.Parameters.AddWithValue("@Role", re.Role);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close(); 

                return i >= 1;
            }
        }


    }
}
