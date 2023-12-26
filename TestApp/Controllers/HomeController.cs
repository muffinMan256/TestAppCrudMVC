using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Data.Core.IRepository;
using Microsoft.EntityFrameworkCore;
using TestApp.Models;
using Data.Core.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestApp.Helper;
using X.PagedList;


namespace TestApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonalRepository _personalRepository;

        public HomeController(IPersonalRepository personalRepository)
        {
            _personalRepository = personalRepository;
        }


        //metoda asincrona 
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int page = 1, int pageSize=10)
        {

            string nume = "Hello Andrei";
            ViewBag.NumePrenume = nume;
            ViewData["NumePrenume"] = nume;


            //ordinea de sortare a functieti mele

            if (string.IsNullOrWhiteSpace(sortOrder))
            {
                sortOrder = "sort1_desc";
            }

           

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                page = 1;
            }

        

            //await asteapta dupa rezultat 
            var lst =  _personalRepository.FindAllAsync();

            switch (sortOrder)
            {
                case "sort1_asc": lst = lst.OrderBy(a => a.Valid);
                    break;
                case "sort1_desc": lst = lst.OrderByDescending(a => a.Valid);
                    break;
                case "sort2_asc":
                    lst = lst.OrderBy(a => a.Nume).ThenBy(a=>a.Prenume);
                    break;
                case "sort2_desc":
                    lst = lst.OrderByDescending(a => a.Nume).ThenByDescending(a => a.Prenume);
                    break;
                case "sort3_asc":
                    lst = lst.OrderBy(a => a.Birthdate);
                    break;
                case "sort3_desc":
                    lst = lst.OrderByDescending(a => a.Birthdate);
                    break;
                default:
                    lst = lst.OrderBy(a => a.Valid);
                    break;
            }

            var model = lst.Select(a => new PersonalModel()
            {
                FullName = a.FullName,
                Valid = a.Valid,
                UserId = a.UserId,
                Birthday = a.Birthdate,
            });

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentPageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.CurrentFilter = searchString;

            ViewBag.Sort1Parm = sortOrder == "sort1_desc" ? "sort1_asc" : "sort1_desc";
            ViewBag.Sort2Parm = sortOrder == "sort2_asc" ? "sort2_desc" : "sort2_asc";
            ViewBag.Sort3Parm = sortOrder == "sort3_asc" ? "sort3_desc" : "sort3_asc";

           

            var items = Utils.RecordListItems();
            ViewBag.PageSizeList = new SelectList(items, "Value", "Text", pageSize);


            //pager 
            var lstmodel = model.ToPagedList(page, pageSize);
            return View(lstmodel);
        }

        [HttpGet]
        public  IActionResult Create()
        {
            var model = new PersonalModel()
            {
                UserId = Guid.NewGuid()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonalModel model)
        {
            //server validation

            if (ModelState.IsValid)
            {
                var newpers = new Personal()
                {
                    Nume = model.Nume,
                    Prenume = model.Prenume,
                    Valid = model.Valid,
                    UserId = model.UserId,
                };

                await _personalRepository.AddAsync(newpers);

               return RedirectToAction("Index", "Home");

            }


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid userId)
        {
            var getmodel= await _personalRepository.FindByIdAsync(userId);

            if (getmodel == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new PersonalModel()
            {
                UserId = getmodel.UserId,
                Nume = getmodel.Nume,
                Prenume = getmodel.Prenume,
                Valid = getmodel.Valid,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PersonalModel model)
        {
            //server validation

            if (ModelState.IsValid)
            {
                var getmodel = await _personalRepository.FindByIdAsync(model.UserId);

                if (getmodel == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                    getmodel.Nume = model.Nume;
                  getmodel.Prenume = model.Prenume;
                  getmodel.Valid = model.Valid;

                await _personalRepository.UpdateAsync(getmodel);

                return RedirectToAction("Index", "Home");

            }


            return View(model);
        }


        public async Task<IActionResult> Delete(Guid userId)
        {
            var getmodel = await _personalRepository.FindByIdAsync(userId);

            if (getmodel == null)
            {
                return RedirectToAction("Index", "Home");
            }

            await _personalRepository.DeleteAsync(getmodel);
        
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(Guid userId)
        {
            var getmodel = await _personalRepository.FindByIdAsync(userId);

            if (getmodel == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var model = new PersonalModel()
            {
                UserId = getmodel.UserId,
                Nume = getmodel.Nume,
                Prenume = getmodel.Prenume,
                Valid = getmodel.Valid,
            };
            return View(model);
        }


        [HttpGet]
        public async Task<RedirectToActionResult> GenerateData()
        {

            for (int i = 0; i < 100; i++)
            {
                var model = new Personal()
                {
                    UserId = Guid.NewGuid(),
                    Valid = true,
                    Nume = "TestNume" + i.ToString(),
                    Prenume = "TestPrenume" + i.ToString(),
                };

              await  _personalRepository.AddAsync(model);

            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
