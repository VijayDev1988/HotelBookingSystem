using AutoMapper;
using HBS.Application.Wrappers;
using HBS.Client.Enums;
using HBS.Client.Models;
using HBS.Client.Services.Interfaces;
using HBS.Client.Utilities;
using HBS.Client.ViewModel;
using HBS.Domain.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace HBS.Client.Services.Concreate
{
    public class RoomBookingServices : IRoomBookingServices
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RoomBookingServices(HttpClient httpClient, IMapper mapper, ILogger logger)
        {
            _httpClient = httpClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookingHistoryViewModel>> GetBookingHistory(string userEmail, string jwtToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var URI = $"{ApplicationsConstants.BOOKING_HISTORY_API}{userEmail}";
                var output = await _httpClient.GetAsync(URI);
                var bookingHistory = await output.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response<List<BookingDetails>>>(bookingHistory);

                if (result.Succeeded)
                {
                    var bookingDetails = _mapper.Map<IEnumerable<BookingHistoryViewModel>>(result.Data);
                    return bookingDetails;
                }

                _logger.LogInformation($"No booking details found for {userEmail}");

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to fetch Booking history for user {userEmail}");
                return null;
            }
        }

        public async Task<BookRoomViewModel> BookingRoom(BookRoomViewModel bookRoomViewModel)
        {
            try
            {
                var bookRoomModel = new BookRoomModel
                {
                    FromTime = bookRoomViewModel.FromDate,
                    ToTime = bookRoomViewModel.ToDate,
                    RoomType = (int)bookRoomViewModel.RoomType,
                    EmailId = bookRoomViewModel.EmailId,
                    Status = Enum.GetName(BookingStatus.Booked)
                };
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bookRoomViewModel.jwtToken);

                var json = JsonConvert.SerializeObject(bookRoomModel);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var URI = $"{ApplicationsConstants.BOOK_ROOM_API}";
                var output = await _httpClient.PostAsync(URI, stringContent);

                var bookedRoom = await output.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response<BookingDetails>>(bookedRoom);

                BookRoomViewModel bookRoomResult = new BookRoomViewModel();

                if (result.Succeeded)
                {
                    var bookingDetails = result.Data;
                    bookRoomResult.BookingId = bookingDetails.RoomBooking.Id;
                    bookRoomResult.RoomNumber = bookingDetails.RoomBooking.Room.RoomNumber;
                    bookRoomResult.Message = result.Message;

                    _logger.LogInformation($"Room booked for user {bookRoomViewModel.EmailId}. Booking Id {bookRoomResult.BookingId}");

                    return bookRoomResult;
                };

                _logger.LogError($"Unable to book room for user {bookRoomViewModel.EmailId}. Message {result.Message}");

                bookRoomResult.Message = result.Message;
                return bookRoomResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to book the room for user {bookRoomViewModel.EmailId}");
                return null;
            }
        }
    }
}
