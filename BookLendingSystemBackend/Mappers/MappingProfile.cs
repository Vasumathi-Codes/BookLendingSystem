using AutoMapper;
using BookLendingSystem.DTOs;
using BookLendingSystem.Models;

namespace BookLendingSystem.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // BookCreateDto  -> Book
            CreateMap<BookCreateDto, Book>()
                .ForMember(dest => dest.AvailableCopies, opt => opt.MapFrom(src => src.TotalCopies));

            // Book ->  BookReadDto
            CreateMap<Book, BookReadDto>();

            // BookUpdateDto -> Book
            CreateMap<BookUpdateDto, Book>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
