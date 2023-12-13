using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Data.Core.IRepository;
using Microsoft.EntityFrameworkCore;
using TestApp.Models;
using Data.Core.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestApp.Helper;


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
        public async Task<IActionResult> Index(int? pageSize)
        {

            string nume = "Hello Andrei";
            ViewBag.NumePrenume = nume;
            ViewData["NumePrenume"] = nume;

            //await asteapta dupa rezultat 
            var lst = await _personalRepository.FindAllAsync();

            var model = lst.Select(a => new PersonalModel()
            {
                Nume = a.Nume,
                Prenume = a.Prenume,
                Valid = a.Valid,
                UserId = a.UserId,
            });

            var items = Utils.RecordListItems();
            ViewBag.PageSizeList = new SelectList(items, "Value", "Text", pageSize);

            return View(model);
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
    }
}
