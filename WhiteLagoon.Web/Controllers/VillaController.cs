using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll(); //villa is the entity specified
            return View(villas);
        }


        //Right click Create below and add view. make sure its the same name as Action after IActionResult in this case its Create
        // This then creates the page for whats shown after clicking create. In  the below code the bage is return view and it
        // shows Createvi
        // ew as its the same name as action.   
        public IActionResult Create()
        {
            return View();
            //could enter Villa inbetween View brackets but because the Create view is using villa entities model we don't have to as it will
            //pick it up from the Create model. check top of page on create view for better understanding.
        }
        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            //Below is custom model validation example to stop Villa having same  name and description
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                if(obj.Image != null)
                {
                    string fileName= Guid.NewGuid().ToString()+ Path.GetExtension(obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                        obj.Image.CopyTo(fileStream);

                        obj.ImageUrl = @"\images\VillaImage\" + fileName;


                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }


				_unitOfWork.Villa.Add(obj);
				_unitOfWork.Save();
                TempData["success"] = "The villa has been created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa could not be created";
            return View(obj);
        }
        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId); //This code is saying give me the first or default where the Id matches what
            //is in the parameters. If matches multiple records then will bring back first. Also (u => u.Is == villaId) is a link expression.

            //Villa? obj - _db.Villas/Find(villaId);
            //var VillaLisr = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
                // above code says if no object is found then show the error view (error message page we made) "Home" is the controller this Error page is linked to.
                //The reason we have to put home is because we're currently in Villa controller and the error page is in home so need to specify where program can locate it.
            }
            return View(obj); //If villa found it will show villa with fields populated
        }
        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            //Below is custom model validation example to stop Villa having same  name and description
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Name", "The description cannot exactly match the Name.");
            }
            if (ModelState.IsValid && obj.Id > 0)
            {
				_unitOfWork.Villa.Update(obj);
				_unitOfWork.Save();
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa could not be updated";
            return View(obj);
        }
		public IActionResult Delete(int villaId)
		{
			Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId); 
			
			if (obj is null)
			{
				return RedirectToAction("Error", "Home");
				
			}
			return View(obj); 
		}
        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == obj.Id);
            if (objFromDb is not null)
            {
				_unitOfWork.Villa.Remove(objFromDb);
				_unitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa could not be deleted";
            return View(obj);
        }
    }
}

