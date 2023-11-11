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
    public interface IProduct__DAL<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(long id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(long id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<T> List(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DisplayOrder"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        bool Check(int DisplayOrder, int ProductID);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DisplayOrder"></param>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        bool Check(long ID, int DisplayOrder, int ProductID);
    }
}
