using AutoMapper;
using PersonsHandBook.Domain.Models.Entity;
using PersonsHandBook.Models;

namespace PersonsHandBook.Services.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreatePersonModel, Person>();
            CreateMap<AddContactModel, Contact>();
            CreateMap<UpdateContactModel, Contact>();
            CreateMap<UploadPhotoModel, Photo>();

        }
    }
}
