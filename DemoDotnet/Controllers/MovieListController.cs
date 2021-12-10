using System.Data;
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
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace DemoDotnet.Controllers
{
    public class MovieListController : Controller
    {
        ExcelProcess excelPro = new ExcelProcess();
        public IConfiguration Configuration { get; }
        // public Test(IConfiguration config){
        //     Configuration = config;
        // }
        private readonly ApplicationDBContext _context;

        public MovieListController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: MovieList
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movie.ToListAsync());
        }

        // GET: MovieList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: MovieList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MovieList/Create
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
                    var fileName = "Excel Demo";
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
            return View();
        }
        // GET: MovieList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: MovieList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            return View(movie);
        }

        // GET: MovieList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: MovieList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private int WriteDataTableToDatabase(DataTable dt)
        {
            try
            {
                var conn = Configuration.GetConnectionString("ApplicationDBContext");
                SqlBulkCopy bulkCopy = new SqlBulkCopy(conn);
                bulkCopy.DestinationTableName = "Movie";
                bulkCopy.ColumnMappings.Add(0, "Title");
                bulkCopy.ColumnMappings.Add(1, "ReleaseDate");
                bulkCopy.ColumnMappings.Add(2, "Genre");
                bulkCopy.ColumnMappings.Add(3, "Price");
                bulkCopy.ColumnMappings.Add(4, "Rating");
                bulkCopy.WriteToServer(dt);
            }
            catch
            {
                return 0;
            }
            return dt.Rows.Count;
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
