using AutoMapper;
using HBS.Application.DTO.Room;
using HBS.Application.Exceptions;
using HBS.Application.Interfaces;
using HBS.Application.Interfaces.Repositories;
using HBS.Application.Wrappers;
using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.Features
{

    public class RoomsQuerys : IRoomQuery
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBookingDetailsRepository _bookingDetailsRepository;
        private readonly IRoomBookingRepository _roomBookingRepository;
        private readonly IRoomRepository _roomRepository;

        public RoomsQuerys(IUnitOfWork unitOfWork, IMapper mapper,
            IBookingDetailsRepository bookingDetailsRepository,
            IRoomBookingRepository roomBookingRepository,
            IRoomRepository roomRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bookingDetailsRepository = bookingDetailsRepository;
            _roomBookingRepository = roomBookingRepository;
            _roomRepository = roomRepository;
        }


        public async Task<Response<Rooms>> AddRoomsAsync(RoomViewModel rooms)
        {

            var addRoom = _mapper.Map<Rooms>(rooms);
            var allRooms = await _unitOfWork.Repository<Rooms>().GetAllAsync();

            var isRoomNumberAvailable = allRooms.Any(r => r.RoomNumber.ToLower().Equals(rooms.RoomNumber.ToLower()));

            if (isRoomNumberAvailable) throw new ApiException("Enter unique Room number");

            var hotel = await _unitOfWork.Repository<Hotel>().GetByIdAsync(rooms.HotelId);

            if (hotel == null) throw new ApiException("Hotel not found, Select another hotel");

            var result = await _unitOfWork.Repository<Rooms>().AddAsync(addRoom);
            return new Response<Rooms>(result);
        }

        public async Task<Response<IEnumerable<Rooms>>> GetAllRoomsAsync()
        {
            var result = await _unitOfWork.Repository<Rooms>().GetAllAsync();
            return new Response<IEnumerable<Rooms>>(result);
        }

        public async Task<Response<IEnumerable<BookingDetails>>> GetRoomsByDateAsync(DateTime bookingDate)
        {
            var result = await _bookingDetailsRepository.GetRoomsBookingByDateAsync(bookingDate);
            return new Response<IEnumerable<BookingDetails>>(result);
        }

        public async Task<Response<BookingDetails>> BookRoomAsync(RoomBookingViewModel roomBookingViewModel)
        {
            var checkAvailaibility = await GetAvailableRoomsAsync(roomBookingViewModel);
            if (checkAvailaibility == null || checkAvailaibility.Count == 0) throw new ApiException("Room not available");

            var roomTobeBooked = checkAvailaibility.FirstOrDefault();
            var bookRoom = _mapper.Map<RoomBooking>(roomBookingViewModel);
            bookRoom.Room = roomTobeBooked.Room;

            var bookedRoom = await _unitOfWork.Repository<RoomBooking>().AddAsync(bookRoom);
            if (bookedRoom == null) throw new ApiException("Problem in booking rooms, please tyr again later");

            var bookingDetails = new BookingDetails
            {
                RoomBooking = bookedRoom,
                UserEmail = roomBookingViewModel.EmailId
            };

            var result = await _bookingDetailsRepository.AddAsync(bookingDetails);
            return new Response<BookingDetails>(result);
        }

        public async Task<Response<IEnumerable<BookingDetails>>> GetCustomerRoomsBookingDetails(string userEmailId)
        {
            var result = await _bookingDetailsRepository.GetCustomerRoomsBookingDetails(userEmailId);
            return new Response<IEnumerable<BookingDetails>>(result);
        }

        public async Task<Response<IEnumerable<Rooms>>> GetRoomsBookingStatusAsync(DateTime bookingDate)
        {
            var roomStatus = await _roomRepository.GetRoomsBookingStatusAsync(bookingDate);
            return new Response<IEnumerable<Rooms>>(roomStatus);
        }


        private async Task<List<RoomBooking>> GetAvailableRoomsAsync(RoomBookingViewModel roomBookingViewModel)
        {
            var checkAvailaibility = await _roomBookingRepository.GetAvailableRoomsAsync(roomBookingViewModel.FromTime,
                                                                                        roomBookingViewModel.ToTime,
                                                                                        roomBookingViewModel.RoomType);
            return checkAvailaibility;
        }
    }
}
