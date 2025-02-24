using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
		private readonly IUnitOfWork _unitOfWork;

		public VillaNumberController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties:"Villa");
            return View(villaNumbers);
        }


        //Right click Create below and add view. make sure its the same name as Action after IActionResult in this case its Create
        // This then creates the page for whats shown after clicking create. In  the below code the bage is return view and it
        // shows Createvi
        // ew as its the same name as action.   
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
                })

            };
            /* IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text= u.Name,
                Value= u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
            }); */

            //ViewBag.VillaList = list;

            //Viewbag is a temp way and works same //ViewData["VillaList"] = list;
            //Viewdata is a method to transfer data from controller to view.
            return View(villaNumberVM);
            //could enter Villa inbetween View brackets but because the Create view is using villa entities model we don't have to as it will
            //pick it up from the Create model. check top of page on create view for better understanding.
        }
        //View data is know to transfer data drom controller to the view. **abo
        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            //True or false if any records in db with villa number being entered

			if (ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa Number has been created successfully";
                return RedirectToAction(nameof(Index)); //name of doesn't work if diff controller
            }
            if(roomNumberExists)
            {
                TempData["error"] = "The villa number already exists.";
            }

            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
            });

			return View(obj);
        }
        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
                }),
                VillaNumber = _unitOfWork.VillaNumber.GetAll().FirstOrDefault(u=>u.Villa_Number== villaNumberId)

			};

			if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
                // above code says if no object is found then show the error view (error message page we made) "Home" is the controller this Error page is linked to.
                //The reason we have to put home is because we're currently in Villa controller and the error page is in home so need to specify where program can locate it.
            }
            return View(villaNumberVM); //If villa found it will show villa with fields populated
        }
        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
				_unitOfWork.Save();
				TempData["success"] = "The Villa Number has been updated successfully";
                return RedirectToAction(nameof(Index)); //clean code
            }

            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
            });

            return View(villaNumberVM);
        }
          
			public IActionResult Delete(int villaNumberId)
		{
			VillaNumberVM villaNumberVM = new()
			{
				VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
				}),
				VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)

			};

			if (villaNumberVM.VillaNumber == null)
			{
				return RedirectToAction("Error", "Home");
				// above code says if no object is found then show the error view (error message page we made) "Home" is the controller this Error page is linked to.
				//The reason we have to put home is because we're currently in Villa controller and the error page is in home so need to specify where program can locate it.
			}
			return View(villaNumberVM); //If villa found it will show villa with fields populated
		}
        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (objFromDb is not null)
            {
                _unitOfWork.VillaNumber.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa number could not be deleted";
            return View();
        }
    }
}

