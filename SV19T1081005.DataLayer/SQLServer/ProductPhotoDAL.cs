using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081005.DomainModel;

namespace SV19T1081005.DataLayer.SQLServer
{
    public class ProductPhotoDAL : _BaseDAL, IProduct__DAL<ProductPhoto>
    {
        public ProductPhotoDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(ProductPhoto data)
        {
            int result = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO ProductPhotos
                                        (ProductID, Photo, Description, DisplayOrder, IsHidden)
                                    VALUES (@ProductID, @Photo, @Description, @DisplayOrder, @IsHidden)
                                    SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);
                cmd.Parameters.AddWithValue("@Description", data.Description);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@IsHidden", data.IsHidden);

                result = Convert.ToInt32(cmd.ExecuteScalar());

                cn.Close();
            }

            return result;
        }

        public bool Check(int DisplayOrder, int ProductID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT count(*) FROM ProductPhotos 
                                    WHERE DisplayOrder = @DisplayOrder 
                                    and ProductID = @ProductID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
                cmd.Parameters.AddWithValue("@ProductID", ProductID);

                result = Convert.ToInt32(cmd.ExecuteScalar()) == 0;

                cn.Close();
            }

            return result;
        }

        public bool Check(long ID, int DisplayOrder, int ProductID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT count(*) FROM ProductPhotos 
                                    WHERE DisplayOrder = @DisplayOrder 
                                    and ProductID = @ProductID and PhotoID != @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@PhotoID", ID);
                cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
                cmd.Parameters.AddWithValue("@ProductID", ProductID);

                result = Convert.ToInt32(cmd.ExecuteScalar()) == 0;

                cn.Close();
            }

            return result;
        }

        public bool Delete(long id)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from ProductPhotos where PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@PhotoID", id);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }

        public ProductPhoto Get(long id)
        {
            ProductPhoto data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ProductPhotos where PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@PhotoID", id);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (result.Read())
                {
                    data = new ProductPhoto()
                    {
                        PhotoID = Convert.ToInt32(result["PhotoID"]),
                        ProductID = Convert.ToInt32(result["ProductID"]),
                        Photo = Convert.ToString(result["Photo"]),
                        Description = Convert.ToString(result["Description"]),
                        DisplayOrder = Convert.ToInt32(result["DisplayOrder"]),
                        IsHidden = Convert.ToBoolean(result["IsHidden"]),
                    };
                }
                cn.Close();
            }

            return data;
        }

        public bool InUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<ProductPhoto> List(int id)
        {
            List<ProductPhoto> data = new List<ProductPhoto>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductPhotos WHERE ProductID = @productID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productID", id);
                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (result.Read())
                {
                    data.Add(new ProductPhoto()
                    {
                        PhotoID = Convert.ToInt64(result["PhotoID"]),
                        ProductID = Convert.ToInt32(result["ProductID"]),
                        Photo = Convert.ToString(result["Photo"]),
                        Description = Convert.ToString(result["Description"]),
                        DisplayOrder = Convert.ToInt32(result["DisplayOrder"]),
                        IsHidden = Convert.ToBoolean(result["IsHidden"])
                    });
                }
                cn.Close();
            }
            return data;
        }

        public bool Update(ProductPhoto data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE ProductPhotos
                                    SET 
                                        ProductID = @ProductID,  
                                        Photo = @Photo, 
                                        Description = @Description, 
                                        DisplayOrder = @DisplayOrder, 
                                        IsHidden = @IsHidden 
                                    WHERE PhotoID = @PhotoID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@Photo", data.Photo);
                cmd.Parameters.AddWithValue("@Description", data.Description);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@IsHidden", data.IsHidden);
                cmd.Parameters.AddWithValue("@PhotoID", data.PhotoID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
