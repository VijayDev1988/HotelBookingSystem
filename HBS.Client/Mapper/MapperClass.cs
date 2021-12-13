using AutoMapper;
using HBS.Application.DTO.Account;
using HBS.Client.ViewModel;
using HBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBS.Client.Mapper
{
    public class MapperClass : Profile
    {
        public MapperClass()
        {
            CreateMap<List<BookingHistoryViewModel>, List<BookingDetails>>().ReverseMap();
            CreateMap<BookingHistoryViewModel, BookingDetails>().ReverseMap();
            CreateMap<RegisterViewModel, RegisterRequest>().ReverseMap();

        }
    }
}
