using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.Models;
using WhiteLagoon.Web.ViewModels;


namespace WhiteLagoon.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            };
            return View(homeVM);
        }

        [HttpPost]
        public IActionResult Index(HomeVM homeVM)
        {
            homeVM.VillaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity");

            return View(homeVM);
        }
            
    



    public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
    {
            Thread.Sleep(500); // waits 2000ms so we can see spinner
        var villaList = _unitOfWork.Villa.GetAll(includeProperties: "VillaAmenity").ToList();
        foreach (var villa in villaList)
        {
            if (villa.Id % 2 == 0)
            {
                villa.IsAvailable = false;
            }
        }
        HomeVM homeVM = new()
        {
            CheckInDate = checkInDate,
            VillaList = villaList,
            Nights = nights
        };

        return PartialView("_VillaList",homeVM); //this only loads the one section and not refreshes page
            //in this instance it's refreshing the villas section after searching availability.
    }
		public IActionResult Privacy()
            {
                return View();
            }

            public IActionResult Error()
            {
                return View();
            }
        }
    }

