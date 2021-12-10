using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoDotnet.Data;
using DemoDotnet.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DemoDotnet.Controllers
{
    public class ProductListController : Controller
    {
        ExcelProcess excelPro = new ExcelProcess();
        public IConfiguration Configuration { get; }
        private readonly ApplicationDBContext _context;

        public ProductListController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: ProductList
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.Product.Include(p => p.Category);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: ProductList/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: ProductList/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID");
            return View();
        }

        // POST: ProductList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload!");
                }
                else
                {
                    //rename file when upload to server
                    //tao duong dan /Uploads/Excels de luu file upload len server
                    var fileName = "Ten file muon luu";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName + fileExtension);
                    var fileLocation = new FileInfo(filePath).ToString();

                    if (ModelState.IsValid)
                    {
                        //upload file to server
                        if (file.Length > 0)
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                //save file to server
                                await file.CopyToAsync(stream);
                                //read data from file and write to database
                                //_excelPro la doi tuong xu ly file excel ExcelProcess
                                var dt = excelPro.ExcelToDataTable(fileLocation);
                                //ghi du lieu datatable vao database
                                WriteDataTableToDatabase(dt);

                            }
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            return View(file);
        }

        private int WriteDataTableToDatabase(System.Data.DataTable dt)
        {
            try
            {
                var conn = Configuration.GetConnectionString("ApplicationDBContext");
                SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
                bulkCopy.DestinationTableName = "ProductID";
                bulkCopy.ColumnMappings.Add(0, "ProductName");
                bulkCopy.ColumnMappings.Add(1, "CategoryID");
                bulkCopy.ColumnMappings.Add(1, "UnitPrice");
                bulkCopy.ColumnMappings.Add(1, "Quantity");
                bulkCopy.WriteToServer(dt);
            }
            catch
            {
                return 0;
            }
            return dt.Rows.Count;
        }

        // GET: ProductList/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // POST: ProductList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProductID,ProductName,CategoryID,ItemID,UnitPrice,Quantity")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            return View(product);
        }

        // GET: ProductList/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: ProductList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
