using HBS.Client.Services.Interfaces;
using HBS.Client.Utilities;
using HBS.Client.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HBS.Client.Controllers
{
    public class RoomController : BaseController
    {
        private readonly IRoomBookingServices _roomBookingServices;
        private readonly ILogger<HomeController> _logger;


        public RoomController(IRoomBookingServices roomBookingServices, ILogger<HomeController> logger)
        {
            _roomBookingServices = roomBookingServices;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IActionResult BookRoom()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> BookRoom(BookRoomViewModel bookRoomViewModel)
        {
            if (ModelState.IsValid)
            {
                string email = HttpContext.Session.GetString(ApplicationsConstants.EMAIL);
                bookRoomViewModel.EmailId = email;
                bookRoomViewModel.jwtToken = HttpContext.Session.GetString(ApplicationsConstants.JWT_TOKEN);
                var bookedRoom = await _roomBookingServices.BookingRoom(bookRoomViewModel);
                return View(bookedRoom);
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string email = HttpContext.Session.GetString(ApplicationsConstants.EMAIL);
            string jwt = HttpContext.Session.GetString(ApplicationsConstants.JWT_TOKEN);
            var bookingHistory = await _roomBookingServices.GetBookingHistory(email, jwt);
            return View(bookingHistory);
        }
    }
}