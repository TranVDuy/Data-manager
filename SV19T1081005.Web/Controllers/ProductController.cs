using SV19T1081005.BusinessLayer;
using SV19T1081005.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SV19T1081005.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [RoutePrefix("product")]
    public class ProductController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Lấy điều kiện tìm kiếm từ trong session
            Models.ProductSearchInput model = Session["PRODUCT_SEARCH"] as Models.ProductSearchInput;

            //Nếu không có điều kiện tìm kiếm thì tạo điều kiện mặc định
            if (model == null)
                model = new Models.ProductSearchInput()
                {
                    Page = 1,
                    PageSize = 10,
                    SearchValue = "",
                    
                };

            return View(model);
        }
        public ActionResult Search(Models.ProductSearchInput input)
        {
            int rowCount = 0;

            var data = BusinessLayer.ProductDataService.ListOfProducts(input.Page,
                                                                       input.PageSize,
                                                                       input.SearchValue,
                                                                       input.CategoryID,
                                                                       input.SupplierID,
                                                                       out rowCount);
            //ktra câu sql thử ok
            Models.ProductPaginationResult model = new Models.ProductPaginationResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue,
                CategoryID = input.CategoryID,
                SupplierID = input.SupplierID,
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiện tìm kiếm vào session
            Session["PRODUCT_SEARCH"] = input;

            return View(model);
        }
        [Route("viewdetailproduct/{method}/{id}")]
        public ActionResult ViewDetailProduct(string method, int ID)
        {
            switch (method)
            {
                case "category":
                    Category ct = CommonDataService.GetCategory(ID);
                    Session["PRODUCT_SEARCH"] = new Models.ProductSearchInput()
                    {
                        Page = 1,
                        PageSize = 10,
                        SearchValue = "",
                        CategoryID = ct.CategoryID,
                        SupplierID = 0
                    };
                    break;
                case "supplier":
                    Supplier sp = CommonDataService.GetSupplier(ID);
                    Session["PRODUCT_SEARCH"] = new Models.ProductSearchInput()
                    {
                        Page = 1,
                        PageSize = 10,
                        SearchValue = "",
                        CategoryID = 0,
                        SupplierID = sp.SupplierID
                    };
                    break;
                

                default:
                    return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Product model = new Product()
            {
                ProductID = 0
            };
            ViewBag.Title = "Bổ sung mặt hàng";
            return View(model);
        }
     
        public ActionResult Save(Product model, HttpPostedFileBase uploadPhoto)
        {
            if (string.IsNullOrWhiteSpace(model.ProductName))
            {
                ModelState.AddModelError("ProductName", "Tên mặt hàng không được trống");
            }


            if (string.IsNullOrWhiteSpace(model.Unit))
            {
                model.Unit = "";
            }

            if (string.IsNullOrWhiteSpace(model.Price))
            {
                model.Price = "0.00";
            }

            if(model.CategoryID == 0)
            {
                ModelState.AddModelError("CategoryID", "Vui lòng chọn loại hàng!");
            }
            if(model.SupplierID == 0)
            {
                ModelState.AddModelError("SupplierID", "Vui lòng chọn nhà cung cấp");
            }

            //Xử lý ảnh

            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/images/products");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string uploadFilePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(uploadFilePath);
                model.Photo = $"/images/products/{fileName}";
            }
           
            
            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật mặt hàng";
                return View("Create", model);
            }




            if (model.ProductID == 0)
            {             
                ProductDataService.AddProduct(model);

            }
            else
            {
                ProductDataService.UpdateProduct(model);
            }

            Session["PRODUCT_SEARCH"] = new Models.ProductSearchInput()
            {
                SearchValue = model.ProductName,
                Page = 1,
                PageSize = 5
            };

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("edit/{productID}")]
        public ActionResult Edit(int productID)
        {
            Product model = ProductDataService.GetProduct(productID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Title = "Cập nhật mặt hàng";
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [Route("delete/{productID}")]
        public ActionResult Delete(int productID)
        {
            if (Request.HttpMethod == "POST")
            {
                ProductDataService.DeleteProduct(productID);
                return RedirectToAction("Index");
            }

            Product model = ProductDataService.GetProduct(productID);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            Supplier nhaCungCap = CommonDataService.GetSupplier(model.SupplierID);
            Category loaiHang = CommonDataService.GetCategory(model.CategoryID);

            ViewBag.TenNhaCungCap = nhaCungCap.SupplierName;
            ViewBag.LoaiHang = loaiHang.CategoryName;

            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="photoID"></param>
        /// <returns></returns>
        [Route("photo/{method}/{productID}/{photoID?}")]
        public ActionResult Photo(string method, int productID, int? photoID)
        {
            ProductPhoto model = new ProductPhoto();
            switch (method)
            {
                case "add":
                    model.PhotoID = 0;
                    model.ProductID = productID;
                    ViewBag.Title = "Bổ sung ảnh";
                    break;
                case "edit":
                    model = Product__DataService.GetProductPhoto(photoID.GetValueOrDefault());
                    if (model == null)
                    {
                        return RedirectToAction("edit/"+model.ProductID);
                    }
                    ViewBag.Title = "Thay đổi ảnh";
                    break;
                case "delete":
                    model = Product__DataService.GetProductPhoto(photoID.GetValueOrDefault());
                    if (model == null)
                        return RedirectToAction("edit/"+model.ProductID);

                    if (System.IO.File.Exists("E:\\C#\\Lập trình web\\Solution\\SV19T1081005\\SV19T1081005.Web" + model.Photo))
                    {
                        try
                        {
                            System.IO.File.Delete("E:\\C#\\Lập trình web\\Solution\\SV19T1081005\\SV19T1081005.Web" + model.Photo);
                        }
                        catch (System.IO.IOException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    Product__DataService.DeleteProductPhoto(model.PhotoID);
                    return RedirectToAction("Edit", new { productID = productID });
                    
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult PhotoSave(ProductPhoto model, HttpPostedFileBase uploadPhoto)
        {
            if (String.IsNullOrWhiteSpace(model.Description))
            {
                ModelState.AddModelError("Description", "Mô tả không được để trống");
            }
            //Kiểm tra thứ tự hiển thị
            if (model.PhotoID == 0)
            {
                if (!Product__DataService.CheckProductPhoto(model.DisplayOrder, model.ProductID))
                {
                    ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị đã tồn tại");
                }
            }
            else
            {
                if (!Product__DataService.CheckProductPhoto(model.PhotoID, model.DisplayOrder, model.ProductID))
                {
                    ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị đã tồn tại trong 1 ảnh khác");
                }
            }
            if (String.IsNullOrWhiteSpace(model.Photo))
            {
                model.Photo = "";
            }


            //Xử lý upload ảnh

            if (uploadPhoto != null)
            {
                string path = Server.MapPath("~/images/productphotos");
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string uploadFilePath = System.IO.Path.Combine(path, fileName);
                uploadPhoto.SaveAs(uploadFilePath);
                model.Photo = $"/images/productphotos/{fileName}";
            }

            if (!ModelState.IsValid) 
            {
                ViewBag.Title = model.PhotoID == 0 ? "Bổ sung ảnh sản phẩm" : "Cập nhật ảnh sản phẩm";
                return View("Photo", model);
            }

            if (model.PhotoID > 0)
            {
                Product__DataService.UpdateProductPhoto(model);
            }
            else
            {
                Product__DataService.AddProductPhoto(model);
            }
            return RedirectToAction("edit/" + model.ProductID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="productID"></param>
        /// <param name="attributeID"></param>
        /// <returns></returns>
        [Route("attribute/{method}/{productID}/{attributeID?}")]
        public ActionResult Attribute(string method, int productID, int? attributeID)
        {
            ProductAttribute model = new ProductAttribute();
            switch (method)
            {
                case "add":
                    model.AttributeID = 0;
                    model.ProductID = productID;
                    ViewBag.Title = "Bổ sung thuộc tính";
                    break;
                case "edit":
                    model = Product__DataService.GetProductAttribute(attributeID.GetValueOrDefault());
                    if(model == null)
                    {
                        return RedirectToAction("edit/" + model.ProductID);
                    }
                    ViewBag.Title = "Thay đổi thuộc tính";
                    break;
                case "delete":
                    model = Product__DataService.GetProductAttribute(attributeID.GetValueOrDefault());
                    if (model == null)
                        return RedirectToAction("edit/" + model.ProductID);

                    Product__DataService.DeleteProductAttribute(model.AttributeID);
                    return RedirectToAction("Edit", new { productID = productID });
                    
                default:
                    return RedirectToAction("Index");
            }
            
            return View(model);
        }
        /// <summary>
        /// Lưu thông tin ProductAttributes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 

        [Route("attribute/attributesave")]
        [HttpPost]
        public ActionResult AttributeSave(ProductAttribute model)
        {
            if (string.IsNullOrWhiteSpace(model.AttributeName))
            {
                ModelState.AddModelError("AttributeName", "Tên thuộc tính không được trống");
            }

            if (model.AttributeID == 0)
            { 
                if (!Product__DataService.CheckDisplayOrderProductAttribute(model.DisplayOrder, model.ProductID))
                {
                    ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị đã tồn tại");
                }
            }
            else
            {
                if (!Product__DataService.CheckProductAttribute(model.AttributeID, model.DisplayOrder, model.ProductID))
                {
                    ModelState.AddModelError("DisplayOrder", "Thứ tự hiển thị đã tồn tại");
                }
            }

            if (string.IsNullOrWhiteSpace(model.AttributeValue))
                model.AttributeValue = "";

            if (!ModelState.IsValid)
            {
                ViewBag.Title = model.AttributeID == 0 ? "Bổ sung thuộc tính" : "Cập nhật thuộc tính";
                return View("Attribute", model);

            }

            if (model.AttributeID == 0)
            {
                Product__DataService.AddProductAttribute(model);
            }
            else
            {
                Product__DataService.UpdateProductAttribute(model);
            }
            return RedirectToAction("edit/"+model.ProductID);
        }
    }
}