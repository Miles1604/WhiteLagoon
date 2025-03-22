using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
    
{
    [Authorize(Roles = SD.Role_Admin)]
    public class AmenityController : Controller
    {
		private readonly IUnitOfWork _unitOfWork;

		public AmenityController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
        {
            var Amenity = _unitOfWork.Amenity.GetAll(includeProperties:"Villa");
            return View(Amenity);
        }


        //Right click Create below and add view. make sure its the same name as Action after IActionResult in this case its Create
        // This then creates the page for whats shown after clicking create. In  the below code the bage is return view and it
        // shows Createvi
        // ew as its the same name as action.   
        public IActionResult Create()
        {
            AmenityVM amenityVM = new()
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
            return View(amenityVM);
            //could enter Villa inbetween View brackets but because the Create view is using villa entities model we don't have to as it will
            //pick it up from the Create model. check top of page on create view for better understanding.
        }
        //View data is know to transfer data drom controller to the view. **abo
        [HttpPost]
        public IActionResult Create(AmenityVM obj)
        {
			if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "Amenity has been created successfully";
                return RedirectToAction(nameof(Index)); //name of doesn't work if diff controller
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
            });

			return View(obj);
        }
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
                }),
                Amenity = _unitOfWork.Amenity.GetAll().FirstOrDefault(u=>u.Id== amenityId)

			};

			if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
                // above code says if no object is found then show the error view (error message page we made) "Home" is the controller this Error page is linked to.
                //The reason we have to put home is because we're currently in Villa controller and the error page is in home so need to specify where program can locate it.
            }
            return View(amenityVM); //If villa found it will show villa with fields populated
        }
        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVM.Amenity);
				_unitOfWork.Save();
				TempData["success"] = "The Amenity has been updated successfully";
                return RedirectToAction(nameof(Index)); //clean code
            }

            amenityVM.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
            });

            return View(amenityVM);
        }
          
			public IActionResult Delete(int amenityId)
		{
			AmenityVM amenityVM = new()
			{
				VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()   //This code is creating a dropdownlist of villa ID and Name
				}),
				Amenity = _unitOfWork.Amenity.Get(u => u.Id == amenityId)

			};

			if (amenityVM.Amenity == null)
			{
				return RedirectToAction("Error", "Home");
				// above code says if no object is found then show the error view (error message page we made) "Home" is the controller this Error page is linked to.
				//The reason we have to put home is because we're currently in Villa controller and the error page is in home so need to specify where program can locate it.
			}
			return View(amenityVM); //If villa found it will show villa with fields populated
		}
        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? objFromDb = _unitOfWork.Amenity
                .Get(u => u.Id == amenityVM.Amenity.Id);
            if (objFromDb is not null)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Amenity has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Amenity could not be deleted";
            return View();
        }
    }
}

