using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV19T1081005.DataLayer
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProductDAL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="searchCategoryID"></param>
        /// <param name="searchSupplierID"></param>
        /// <returns></returns>
        int Count(string searchValue = "", int CategoryID = 0, int SupplierID = 0);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Product Get(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Product data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Product data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="seacrchCategoriesID"></param>
        /// <param name="searchSupplierID"></param>
        /// <returns></returns>
        IList<Product> List(int page = 1, int pageSize = 0, string searchValue = ""
                            , int searchCategoryID = 0, int searchSupplierID = 0);
        
    }
}
