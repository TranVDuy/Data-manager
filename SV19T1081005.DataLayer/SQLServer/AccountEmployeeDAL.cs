using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DataLayer.SQLServer
{
    public class AccountEmployeeDAL : _BaseDAL ,IAccountDAL
    {
        public AccountEmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public bool CheckEmail(string email)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT CASE WHEN EXISTS (SELECT * FROM Employees WHERE Email LIKE @email) THEN 1 ELSE 0 END";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@email", email);
                

                result = Convert.ToBoolean(cmd.ExecuteScalar());

                cn.Close();
            }

            return result;
        }

        public bool Check(string email, string password)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT CASE 
                                    WHEN EXISTS(
                                        SELECT Photo, Email, Password
                                        FROM Employees
                                        WHERE Email LIKE @email AND Password LIKE @password) THEN 1 ELSE 0 END";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                result = Convert.ToBoolean(cmd.ExecuteScalar());

                cn.Close();
            }

            return result;
        }

        public AccountEmployee Get(string email, string password)
        {
            AccountEmployee data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT Photo, Email, Password
                                    FROM Employees
                                    WHERE Email LIKE @email AND Password LIKE @password";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (dbReader.Read())
                {
                    data = new AccountEmployee()
                    {
                        Photo = Convert.ToString(dbReader["Photo"]),
                        Email = Convert.ToString(dbReader["Email"]),
                        Password = Convert.ToString(dbReader["Password"])

                    };
                }

                cn.Close();
            }

            return data;
        }

        public bool Update(AccountEmployee data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE Employees
                                    SET    Password = @password
                                    WHERE  Email LIKE @email";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@password", data.NewPassword);
                cmd.Parameters.AddWithValue("@email", data.Email);

                result = cmd.ExecuteNonQuery() > 0;


                cn.Close();
            }

            return result;
        }
    }
}
