using Indigo.Areas.Manage.ViewModels;
using Indigo.DAL;
using Indigo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Indigo.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductController : Controller
    {
        AppDbContext _context { get; set; }
        IWebHostEnvironment _environment { get; set; }

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> product = await _context.Products.ToListAsync();

            return View(product);
        }



        public async Task<IActionResult> Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProductVM)
        {

            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            Product product = new Product()
            {
                Description=createProductVM.ProductDescription,
                Name=createProductVM.Name,
                ImgUrl=createProductVM.ProductImage,
                //ImgUrl = createProductVM.ProductImage.Upload(_environment.WebRootPath, @"\Upload\Product\"),

            };
         
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));



        }
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Update(int id)
        {

            Product product = _context.Products.Find(id);
            UpdateProductVM updateProductVM = new UpdateProductVM()
            {
                Id = id,
                Name = product.Name,
                ProductDescription = product.Description
            };
            return View(updateProductVM);
        }



        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductVM updateProductVM)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }
            Product oldproduct = _context.Products.Find(updateProductVM.Id);
            oldproduct.Name = updateProductVM.ProductDescription;
            oldproduct.Description = updateProductVM.ProductDescription;
            oldproduct.ImgUrl = updateProductVM.ProductImage;
            _context.SaveChanges();
            return RedirectToAction("Index");
           
        }
    }
}
