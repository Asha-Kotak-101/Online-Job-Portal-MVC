
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Online_Job_Portal_MVC.Models
{
    public class LoginModel
    {
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        public string? Role { get; set; }


        //Retrieve all records from a table
        public List<LoginModel> getData()
        {
            List<LoginModel> lstUser = new List<LoginModel>();
            SqlDataAdapter apt = new SqlDataAdapter("select * from Register", con);
            DataSet ds = new DataSet();
            apt.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstUser.Add(new LoginModel
                {
                    Username = dr["Username"].ToString(),
                    Password = dr["Password"].ToString(),
                    Role = dr["Role"].ToString(),

                });
            }
            return lstUser;
        }


        //Insert a record into a database table
        public bool insert(LoginModel re)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Login (Username, Password, Role) VALUES (@Username, @Password, @Role)", con);

                cmd.Parameters.AddWithValue("@Username", re.Username);
                cmd.Parameters.AddWithValue("@Password", re.Password);
                cmd.Parameters.AddWithValue("@Role", re.Role);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
        }
    }
}
