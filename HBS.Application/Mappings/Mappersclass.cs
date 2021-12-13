using AutoMapper;
using HBS.Application.DTO.Room;
using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBS.Application.Mappings
{
    public class Mappersclass : Profile
    {
        public Mappersclass()
        {
            CreateMap<RoomViewModel, Rooms>().ReverseMap();
            CreateMap<RoomBookingViewModel, RoomBooking>().ReverseMap();

            
            //CreateMap<CreateProductCommand, Product>();
            //CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
