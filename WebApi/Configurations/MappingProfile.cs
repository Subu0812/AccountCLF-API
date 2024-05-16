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
        }
    }
}
