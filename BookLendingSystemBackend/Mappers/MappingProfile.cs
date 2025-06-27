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

            // UserCreateDto  -> User
            CreateMap<UserCreateDto, User>(); 

            // User  -> UserCreateDto     
            CreateMap<User, UserReadDto>();

            CreateMap<UserUpdateDto, User>();

            CreateMap<LendingRecordCreateDto, LendingRecord>()
                .ForMember(dest => dest.BorrowDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => DateTime.UtcNow.AddDays(src.DaysToBorrow)))
                .ForMember(dest => dest.ReturnDate, opt => opt.Ignore());

            CreateMap<LendingRecord, LendingRecordReadDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.Name : "Unknown User"))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : "Unknown Book"));


        }
    }
}
