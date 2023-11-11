using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DataLayer.SQLServer
{
    public class CountryDAL : _BaseDAL, ICommonDAL<Country>
    {
        public CountryDAL(string connectionString) : base(connectionString)
        {

        }

        public int Add(Country data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Country Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        //public IList<Country> List()
        //{
        //    List<Country> data = new List<Country>();

        //    //Tạo và mở kết nối
        //    using (SqlConnection cn = OpenConnection())
        //    {

        //        SqlCommand cmd = new SqlCommand();
        //        cmd.CommandText = "SELECT * FROM Countries";
        //        cmd.CommandType = System.Data.CommandType.Text;
        //        cmd.Connection = cn;

        //        var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

        //        while (dbReader.Read())
        //        {
        //            data.Add(new Country()
        //            {
        //                CountryName = Convert.ToString(dbReader["CountryName"])

        //            });
        //        }

        //        cn.Close();
        //    }
        //    return data;
        //}

        public IList<Country> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Country> data = new List<Country>();

            //Tạo và mở kết nối
            using (SqlConnection cn = OpenConnection())
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Countries";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;

                var dbReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (dbReader.Read())
                {
                    data.Add(new Country()
                    {
                        CountryName = Convert.ToString(dbReader["CountryName"])

                    });
                }

                cn.Close();
            }
            return data;
        }

        public bool Update(Country data)
        {
            throw new NotImplementedException();
        }
    }
}
