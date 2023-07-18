using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                string connectionString = "Data Source=.\\MSSQLSERVER01;Initial Catalog=MyStore;Integrated Security=True";
                using (SqlConnection sCon = new SqlConnection(connectionString))
                {
                    sCon.Open();
                    string sql = "Select * FROM clients WHERE id=@id";
                    using (SqlCommand sCmd = new SqlCommand(sql, sCon))
                    {
                        sCmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = sCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.id.Length == 0 || clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All fields are required!";
                return;
            }

            try
            {
                string connectionString = "Data Source=.\\MSSQLSERVER01;Initial Catalog=MyStore;Integrated Security=True";
                using (SqlConnection sCon = new SqlConnection(connectionString))
                {
                    sCon.Open();
                    string sql = "UPDATE clients " +
                                 " SET name=@name, email=@email, phone=@phone, address=@address " +
                                 " WHERE id=@id";

                    using (SqlCommand sCmd = new SqlCommand(sql, sCon))
                    {
                        sCmd.Parameters.AddWithValue("@name", clientInfo.name);
                        sCmd.Parameters.AddWithValue("@email", clientInfo.email);
                        sCmd.Parameters.AddWithValue("@phone", clientInfo.phone);
                        sCmd.Parameters.AddWithValue("@address", clientInfo.address);
                        sCmd.Parameters.AddWithValue("@id", clientInfo.id);

                        sCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Clients/Index");
        }
    }
}
