using AccountCLF.Application.Contract.Locations;
using AccountCLF.Application.Contract.Masters;
using AccountCLF.Application.Contract.Masters.MasterType;
using AutoMapper;
using Model;

namespace WebApi.Configurations
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMasterTypeDto, MasterType>();
            CreateMap< MasterType, GetMasterTypeDto>();

            CreateMap<CreateMasterTypeDetailDto, MasterTypeDetail>();
            CreateMap< MasterTypeDetail,GetMasterTypeDetailsDto>();

            CreateMap<CreateLocationDto, Location>();
        }
    }
}
