using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081005.DataLayer;
using SV19T1081005.DomainModel;

namespace SV19T1081005.BusinessLayer
{
    /// <summary>
    /// Cung cấp chức năng xử lý dữ liệu về tài khoản
    /// </summary>
    public class AccountDataService
    {
        private static readonly IAccountDAL accountDB;

        static AccountDataService()
        {
            string provider = ConfigurationManager
                                    .ConnectionStrings["DB"]
                                    .ProviderName;
            string connectionString = ConfigurationManager
                                            .ConnectionStrings["DB"]
                                            .ConnectionString;

            switch (provider)
            {
                case "SQLServer":
                    accountDB = new DataLayer.SQLServer.AccountEmployeeDAL(connectionString);
                    break;

                default:
                    //categoryDB = new DataLayer.FakeDB.CategoryDAL();
                    break;
            }
        }

        #region các chức năng liên quan đến tài khoản
        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateAccount(AccountEmployee data)
        {
            return accountDB.Update(data);
        }
        /// <summary>
        /// Lấy thông tin tài khoản
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static AccountEmployee GetAccount(string email, string password)
        {
            return accountDB.Get(email, password);
        }
        /// <summary>
        /// Kiểm tra đăng nhập
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Check(string email, string password)
        {
            return accountDB.Check(email, password);
        }
        public static bool CheckEmailExits(string email)
        {
            return accountDB.CheckEmail(email);
        }
        #endregion

    }
}
