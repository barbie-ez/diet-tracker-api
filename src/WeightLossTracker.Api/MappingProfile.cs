using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeightLossTracker.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            //CreateMap<Author, AuthorDTO>()
            //    .ForMember(dest => dest.Name,
            //                            opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            //    .ForMember(dest => dest.Age,
            //                            opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));
            //CreateMap<Book, BookDTO>();
    
        }
    }
}
