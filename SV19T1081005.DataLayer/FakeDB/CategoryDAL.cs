using SV19T1081005.DataLayer.SQLServer;
using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DataLayer.FakeDB
{
    /// <summary>
    /// 
    /// </summary>
    public class CategoryDAL : _BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string connectionString) : base(connectionString)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int Add(Category data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public bool Delete(int categoryID)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public Category Get(int categoryID)
        {
            throw new NotImplementedException();
        }

        public bool InUsed(int categoryID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Danh sách sản phẩm
        /// </summary>
        /// <returns></returns>
        public IList<Category> List()
        {
            List<Category> data = new List<Category>();

            data.Add(new Category() { 
                CategoryID = 1,
                CategoryName = "Mỹ phẩm",
                Description = "Giúp các cô nàng thêm xinh đẹp"
            });
            data.Add(new Category()
            {
                CategoryID = 2,
                CategoryName = "Bia rượu",
                Description = "Bản lĩnh đàn ông"
            });

            return data;
        }

        public IList<Category> List(int page, int pageSize, string searchValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Cập nhật sản phẩm từ mã sản phẩm
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update(Category data)
        {
            throw new NotImplementedException();
        }
    }
}
