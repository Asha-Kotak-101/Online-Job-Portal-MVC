
using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;
namespace Online_Job_Portal_MVC.Models
{
    public class ContactModel
    {
        SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Fullname")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Please enter Message")]
        public string? Message { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter Subject")]
        public string? Subject { get; set; }


        //Retrieve all records from a table
        public List<ContactModel> getData()
        {
            List<ContactModel> lstUser = new List<ContactModel>();
            SqlDataAdapter apt = new SqlDataAdapter("select * from Contact", con);
            DataSet ds = new DataSet();
            apt.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lstUser.Add(new ContactModel
                {
                    Id = Convert.ToInt32(dr["Id"].ToString()),
                    FullName = dr["FullName"].ToString(),
                    Message = dr["Message"].ToString(),
                    Email = dr["Email"].ToString(),
                    Subject = dr["Subject"].ToString(),
                });
            }
            return lstUser;
        }


        //Insert a record into a database table
        public bool insert(ContactModel re)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=JobPortalDB;Integrated Security=True;"))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Contact (FullName, Message, Subject, Email) VALUES (@FullName, @Message, @Subject, @Email)", con);

                cmd.Parameters.AddWithValue("@FullName", re.FullName);
                cmd.Parameters.AddWithValue("@Message", re.Message);
                cmd.Parameters.AddWithValue("@Subject", re.Subject);
                cmd.Parameters.AddWithValue("@Email", re.Email);
               

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                return i >= 1;
            }
        }

    }
}
