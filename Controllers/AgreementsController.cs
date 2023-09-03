using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgreementManagement.Data;
using AgreementManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AgreementManagement.Controllers
{
    public class AgreementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<IdentityUser> _signInManager;
        private int ProductGroupId = 0;

        public AgreementsController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Search(int draw, int start, int length, string searchValue, string sortColumn, string sortDirection)
        {
            try
            {

                var applicationDbContext = _context.Agreement.Include(a => a.Product).Include(a => a.User).Include(a => a.Product.ProductGroup)
                    .Where(x => !string.IsNullOrEmpty(searchValue) ?
                    x.User.UserName.Contains(searchValue) ||
                    x.Product.ProductNumber.Contains(searchValue) ||
                    x.Product.ProductGroup.GroupCode.Contains(searchValue) : true
                    );

                switch (sortColumn)
                {
                    case "0":
                        applicationDbContext = (sortDirection == "asc") ? applicationDbContext.OrderBy(x => x.User.UserName) :
                            applicationDbContext.OrderByDescending(x => x.User.UserName);
                        break;
                    case "1":
                        applicationDbContext = (sortDirection == "asc") ? applicationDbContext.OrderBy(x => x.Product.ProductGroup.GroupCode) :
                           applicationDbContext.OrderByDescending(x => x.Product.ProductGroup.GroupCode);
                        break;
                    case "2":
                        applicationDbContext = (sortDirection == "asc") ? applicationDbContext.OrderBy(x => x.Product.ProductNumber) :
                           applicationDbContext.OrderByDescending(x => x.Product.ProductNumber);
                        break;
                    case "3":
                        applicationDbContext = (sortDirection == "asc") ? applicationDbContext.OrderBy(x => x.EffectiveDate) :
                           applicationDbContext.OrderByDescending(x => x.EffectiveDate);
                        break;
                    case "4":
                        applicationDbContext = (sortDirection == "asc") ? applicationDbContext.OrderBy(x => x.ExpirationDate) :
                           applicationDbContext.OrderByDescending(x => x.ExpirationDate);
                        break;
                    case "5":
                        applicationDbContext = (sortDirection == "asc") ? applicationDbContext.OrderBy(x => x.Product.Price) :
                           applicationDbContext.OrderByDescending(x => x.Product.Price);
                        break;
                    case "6":
                        applicationDbContext = (sortDirection == "asc") ? applicationDbContext.OrderBy(x => x.NewPrice) :
                           applicationDbContext.OrderByDescending(x => x.NewPrice);
                        break;

                }


                var AgreementList = await applicationDbContext.ToListAsync();
                var result = new
                {
                    draw,
                    recordsTotal = AgreementList.Count,
                    recordsFiltered = AgreementList.Count,
                    data = AgreementList.Skip(start).Take(length).ToList()
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id)
        {
            try
            {
                if (id == null)
                {
                    var Agreementobj = new Agreement
                    {
                        EffectiveDate = DateTime.Now,
                        ExpirationDate = DateTime.Now,
                        UserId = _signInManager.UserManager.GetUserId(HttpContext.User)
                    };
                    ProductGroupId = _context.Set<ProductGroup>().First().Id;
                    ViewData["ProductGroupId"] = new SelectList(_context.Set<ProductGroup>(), "Id", "GroupCode", ProductGroupId);
                    ViewData["ProductId"] = new SelectList(_context.Set<Product>().Where(x => x.ProductGroupId == ProductGroupId), "Id", "ProductDescription", Agreementobj.ProductId);
                    return PartialView("_AddEditPartial", Agreementobj);
                }
                var agreement = _context.Agreement.Include(x => x.Product).First(x => x.Id == id);
                if (agreement == null)
                {
                    return NotFound();
                }
                ProductGroupId = agreement.Product.ProductGroupId;
                ViewData["ProductGroupId"] = new SelectList(_context.Set<ProductGroup>(), "Id", "GroupCode", ProductGroupId);
                ViewData["ProductId"] = new SelectList(_context.Set<Product>().Where(x => x.ProductGroupId == ProductGroupId), "Id", "ProductDescription", agreement.ProductId);
                return PartialView("_AddEditPartial", agreement);
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agreement agreement)
        {
            try
            {
                agreement.UserId = _signInManager.UserManager.GetUserId(HttpContext.User);
                if (ModelState.IsValid)
                {
                    if (agreement.Id == 0)
                    {
                        _context.Add(agreement);
                        ViewData["successMessage"] = "Data saved successfully.";
                    }
                    else
                    {
                        _context.Update(agreement);
                        ViewData["successMessage"] = "Data Updated successfully.";
                    }
                    await _context.SaveChangesAsync();
                    agreement = _context.Agreement.Include(x => x.Product).First(x => x.Id == agreement.Id);
                    ProductGroupId = agreement.Product.ProductGroupId;
                    ViewData["ProductGroupId"] = new SelectList(_context.Set<ProductGroup>(), "Id", "GroupCode", ProductGroupId);
                    ViewData["ProductId"] = new SelectList(_context.Set<Product>().Where(x => x.ProductGroupId == ProductGroupId), "Id", "ProductDescription", agreement.ProductId);
                    return PartialView("_AddEditPartial", agreement);
                }
                ViewData["ProductGroupId"] = new SelectList(_context.Set<ProductGroup>(), "Id", "GroupCode", ProductGroupId);
                ViewData["ProductId"] = new SelectList(_context.Set<Product>().Where(x => x.ProductGroupId == ProductGroupId), "Id", "ProductDescription", agreement.ProductId);
                return PartialView("_AddEditPartial", agreement);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetProduct(int ProductGroupId)
        {
            try
            {
                Dictionary<string, string> ProductDropdown = new Dictionary<string, string>();

                var product = _context.Set<Product>().Where(x => x.ProductGroupId == ProductGroupId);
                foreach (var item in product)
                {
                    ProductDropdown.Add(item.Id.ToString(), item.ProductDescription);
                }
                return Json(ProductDropdown);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var agreement = await _context.Agreement.FindAsync(id);
                _context.Agreement.Remove(agreement);
                await _context.SaveChangesAsync();
                return Json(new
                {
                    statusCode = 1,
                    Status = "Delete SucsessFully!!!"

                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    statusCode = -1,
                    Status = "Error on Delete!!!"

                });
            }
        }

        private bool AgreementExists(int id)
        {
            return _context.Agreement.Any(e => e.Id == id);
        }
    }
}
