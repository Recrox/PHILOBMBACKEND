using AutoMapper;
using PHILOBMCore.Models;

namespace PHILOBMBAPI;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Client, PHILOBMDatabase.Models.Client>().ReverseMap();
        CreateMap<Car, PHILOBMDatabase.Models.Car>().ReverseMap();
        CreateMap<Invoice, PHILOBMDatabase.Models.Invoice>().ReverseMap();
        CreateMap<Service, PHILOBMDatabase.Models.Service>().ReverseMap();
        CreateMap<User, PHILOBMDatabase.Models.User>().ReverseMap();
    }
}