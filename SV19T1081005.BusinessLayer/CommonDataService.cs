using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SV19T1081005.DomainModel;
using System.Configuration;
using SV19T1081005.DataLayer;
using SV19T1081005.DataLayer.SQLServer;

namespace SV19T1081005.BusinessLayer
{
    /// <summary>
    /// Cung cấp chức năng xử lý dữ liệu chung
    /// </summary>
    public class CommonDataService
    {
        private static readonly ICommonDAL<Country> countryDB;
        private static readonly ICommonDAL<Category> categoryDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        /// <summary>
        /// Constructor
        /// </summary>
        //private static ICategoryDAL categoryDB;

        static CommonDataService()
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
                    countryDB = new DataLayer.SQLServer.CountryDAL(connectionString);
                    categoryDB = new DataLayer.SQLServer.CategoryDAL(connectionString);
                    customerDB = new DataLayer.SQLServer.CustomerDAL(connectionString);
                    supplierDB = new DataLayer.SQLServer.SupplierDAL(connectionString);
                    shipperDB = new DataLayer.SQLServer.ShipperDAL(connectionString);
                    employeeDB = new DataLayer.SQLServer.EmployeeDAL(connectionString);
                    break;
               
                default:
                    //categoryDB = new DataLayer.FakeDB.CategoryDAL();
                    break;
            }
            
            
        }
        #region chức năng liên quan đến quốc gia
        /// <summary>
        /// Danh sách quốc gia
        /// </summary>
        /// <returns></returns>
        public static List<Country> ListOfCountries()
        {
            return countryDB.List().ToList();
        }
        #endregion

        #region các chức năng liên quan đến khách hàng
        /// <summary>
        /// Lấy danh sách toàn bộ khách hàng
        /// </summary>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers()
        {
            return customerDB.List().ToList();
        }

        /// <summary>
        /// Tìm kiếm khách hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(int page
                                                    , int pageSize
                                                    , string searchValue
                                                    , out int rowCount)
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }

        /// <summary>
        /// Lấy Khách hàng thông qua ID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static Customer GetCustomer(int customerID)
        {
            return customerDB.Get(customerID);
        }
        /// <summary>
        /// Xóa một khách hàng thông qua ID
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int customerID)
        {
            if (customerDB.InUsed(customerID))
            {
                return false;
            }
            return customerDB.Delete(customerID);
        }
        /// <summary>
        /// Thêm khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }
        /// <summary>
        /// Cập nhật khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }
        /// <summary>
        /// Kiểm tra phụ thuộc của khách hàng
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public static bool InUsedCustomer(int customerID)
        {
            return customerDB.InUsed(customerID);
        }

        #endregion


        #region các chức năng liên quan đến người giao hàng
        /// <summary>
        /// Lấy danh sách tất cả người giao hàng
        /// </summary>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers()
        {
            return shipperDB.List().ToList();
        }
        /// <summary>
        /// Lấy danh sách người giao hàng theo phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(int page
                                                   , int pageSize
                                                   , string searchValue
                                                   , out int rowCount)
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin người giao hàng thông qua ID
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static Shipper GetShipper(int shipperID)
        {
            return shipperDB.Get(shipperID);
        }
        /// <summary>
        /// Xóa người giao hàng
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int shipperID)
        {
            return shipperDB.Delete(shipperID);
        }
        /// <summary>
        /// Bổ sung người giao hàng 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin người giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        /// <summary>
        /// Kiểm tra người giao hàng
        /// </summary>
        /// <param name="shipperID"></param>
        /// <returns></returns>
        public static bool InUsedShipper(int shipperID)
        {
            return shipperDB.InUsed(shipperID);
        }
        #endregion

        #region các chức năng liên quan đến loại hàng
        /// <summary>
        /// Danh sách tất cả loại hàng
        /// </summary>
        /// <returns></returns>
        public static List<Category> ListOfCategories()
        {
            return categoryDB.List().ToList();
        }

        /// <summary>
        /// Lấy ra danh sách các loại hàng
        /// </summary>
        /// <returns></returns>
        public static List<Category> ListOfCategories(int page
                                                   , int pageSize
                                                   , string searchValue
                                                   , out int rowCount)
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static Category GetCategory(int categoryID)
        {
            return categoryDB.Get(categoryID);
        }
        /// <summary>
        /// Xóa loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static bool DeleteCategory(int categoryID)
        {
            return categoryDB.Delete(categoryID);
        }
        /// <summary>
        /// Bổ sung loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }
        /// <summary>
        /// Kiểm tra loại hàng
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static bool InUsedCategory(int categoryID)
        {
            return categoryDB.InUsed(categoryID);
        }

        #endregion

        #region các chức năng liên quan đến nhà cung cấp
        /// <summary>
        /// Lấy tất cả nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers()
        {
            return supplierDB.List().ToList();
        }
        /// <summary>
        /// Lấy nhà cung cấp phân trang
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(int page
                                                    , int pageSize
                                                    , string searchValue
                                                    , out int rowCount)
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy nhà cung cấp
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static Supplier GetSupplier(int supplierID)
        {
            return supplierDB.Get(supplierID);
        }
        /// <summary>
        /// Xóa nhà cung cấp
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static bool DeleteSupplier(int supplierID)
        {
            return supplierDB.Delete(supplierID);
        }
        /// <summary>
        /// Bổ sung nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }
        /// <summary>
        /// Kiểm tra ràng buộc nhà cung cấp
        /// </summary>
        /// <param name="supplierID"></param>
        /// <returns></returns>
        public static bool InUsedSupplier(int supplierID)
        {
            return supplierDB.InUsed(supplierID);
        }

        #endregion

        #region các chức năng liên quan đến nhân viên
        /// <summary>
        /// Lấy tất cả nhân viên
        /// </summary>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees()
        {
            return employeeDB.List().ToList();
        }

        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(int page
                                                    , int pageSize
                                                    , string searchValue
                                                    , out int rowCount)
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>

        public static Employee GetEmployee(int employeeID)
        {
            return employeeDB.Get(employeeID);
        }
        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>

        public static bool DeleteEmployee(int employeeID)
        {
            return employeeDB.Delete(employeeID);
        }
        /// <summary>
        /// Bổ sung nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }
        /// <summary>
        /// cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        /// <summary>
        /// Kiểm tra ràng buộc nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>

        public static bool InUsedEmployee(int employeeID)
        {
            return employeeDB.InUsed(employeeID);
        }
        #endregion
    }
}
