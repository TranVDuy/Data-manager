using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081005.DomainModel;

namespace SV19T1081005.DataLayer.SQLServer
{
    public class ProductAttributeDAL : _BaseDAL, IProduct__DAL<ProductAttribute>
    {
        public ProductAttributeDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(ProductAttribute data)
        {
            int result = 0;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"INSERT INTO ProductAttributes
                                            (ProductID, AttributeName, AttributeValue, DisplayOrder)
                                    VALUES (@productID,@attributeName,@attributeValue,@displayOrder);
                                    SELECT @@IDENTITY;";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productID", data.ProductID);
                cmd.Parameters.AddWithValue("@attributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("@attributeValue", data.AttributeValue);
                cmd.Parameters.AddWithValue("@displayOrder", data.DisplayOrder);


                result = Convert.ToInt32(cmd.ExecuteScalar());


                cn.Close();
            }

            return result;
        }
        /// <summary>
        /// Kiểm tra tồn tại ví trí hiển thị
        /// </summary>
        /// <param name="DisplayOrder"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public bool Check(int DisplayOrder, int ProductID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {   
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT count(*) FROM ProductAttributes 
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
        /// <summary>
        /// Kiểm tra tồn tại vị trí hiển thị khi chỉnh sửa thuộc tính
        /// </summary>
        /// <param name="AttributeID"></param>
        /// <param name="DisplayOrder"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        /*public bool Check(long AttributeID, int DisplayOrder, int ProductID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT count(*) FROM ProductAttributes 
                                    WHERE DisplayOrder = @DisplayOrder 
                                    and ProductID = @ProductID and AttributeID != @AttributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@AttributeID", AttributeID);
                cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
                cmd.Parameters.AddWithValue("@ProductID", ProductID);

                result = Convert.ToInt32(cmd.ExecuteScalar()) == 0;

                cn.Close();
            }

            return result;
        }*/

        public bool Check(long ID, int DisplayOrder, int ProductID)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT count(*) FROM ProductAttributes 
                                    WHERE DisplayOrder = @DisplayOrder 
                                    and ProductID = @ProductID and AttributeID != @AttributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@AttributeID", ID);
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
                cmd.CommandText = "DELETE FROM ProductAttributes WHERE AttributeID = @AttributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@AttributeID", id);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }

        public ProductAttribute Get(long id)
        {
            ProductAttribute data = null;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ProductAttributes where AttributeID = @attributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@attributeID", id);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                if (result.Read())
                {
                    data = new ProductAttribute()
                    {
                        AttributeID = Convert.ToInt32(result["AttributeID"]),
                        ProductID = Convert.ToInt32(result["ProductID"]),
                        AttributeName = Convert.ToString(result["AttributeName"]),
                        AttributeValue = Convert.ToString(result["AttributeValue"]),
                        DisplayOrder = Convert.ToInt32(result["DisplayOrder"]),
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

        public IList<ProductAttribute> List(int id)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"SELECT * FROM ProductAttributes WHERE ProductID = @productID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@productID", id);

                var result = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (result.Read())
                {
                    data.Add(new ProductAttribute()
                    {
                        AttributeID = Convert.ToInt64(result["AttributeID"]),
                        ProductID = Convert.ToInt32(result["ProductID"]),
                        AttributeName = Convert.ToString(result["AttributeName"]),
                        AttributeValue = Convert.ToString(result["AttributeValue"]),
                        DisplayOrder = Convert.ToInt32(result["DisplayOrder"])
                    });
                }
                cn.Close();
            }
            return data;
        }

        public bool Update(ProductAttribute data)
        {
            bool result = false;

            using (SqlConnection cn = OpenConnection())
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = @"UPDATE ProductAttributes
                                    SET 
                                        ProductID = @ProductID, 
                                        AttributeName = @AttributeName, 
                                        AttributeValue = @AttributeValue, 
                                        DisplayOrder = @DisplayOrder
                                    WHERE AttributeID = @AttributeID";
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = cn;
                cmd.Parameters.AddWithValue("@ProductID", data.ProductID);
                cmd.Parameters.AddWithValue("@AttributeName", data.AttributeName);
                cmd.Parameters.AddWithValue("@AttributeValue", data.AttributeValue);
                cmd.Parameters.AddWithValue("@DisplayOrder", data.DisplayOrder);
                cmd.Parameters.AddWithValue("@AttributeID", data.AttributeID);

                result = cmd.ExecuteNonQuery() > 0;

                cn.Close();
            }

            return result;
        }
    }
}
