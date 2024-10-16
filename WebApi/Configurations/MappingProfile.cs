using AccountCLF.Application.Contract.Entities;
using AccountCLF.Application.Contract.Entities.Logins;
using AccountCLF.Application.Contract.Locations;
using AccountCLF.Application.Contract.Masters;
using AccountCLF.Application.Contract.Masters.MasterType;
using AccountCLF.Application.Contract.TransFunds;
using AccountCLF.Application.Contract.TransFunds.FundTransPaymentDetails;
using AccountCLF.Application.Contract.TransFunds.Loan;
using AccountCLF.Application.Contract.TransFunds.TDSs;
using AutoMapper;
using Model;
using static AccountCLF.Application.Contract.Locations.GetLocationDto;

namespace WebApi.Configurations
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMasterTypeDto, MasterType>();
            CreateMap< MasterType, GetMasterTypeDto>();
            CreateMap< MasterTypeDetail, GetMasterTypeDto>();

            CreateMap<CreateMasterTypeDetailDto, MasterTypeDetail>();
            CreateMap< MasterTypeDetail,GetMasterTypeDetailsDto>();

            CreateMap<CreateLocationDto, Location>();
            CreateMap< Location,GetLocationDto>();
            CreateMap< Location, GetParentLocationDto>();

            CreateMap<CreatePaymentDetailsDto, TransFundPaymentDetail>();

            CreateMap<CreateTransFundTdsDto,TransFundTd>();
            CreateMap<TransFundTd, GetTransFundTdsDto>();

            CreateMap<TransFund, GetTransFundDto>().ReverseMap();
            CreateMap<CreateTransFundDto, TransFund>().ReverseMap();

            CreateMap<Entity,GetEntityDto>();
            CreateMap<Entity,GetParentEntityDto>();
            CreateMap<Entity,GetReferenceEntityDto>();
            CreateMap<BasicProfile,GetBasicProfileDto>();
            CreateMap<AccountGroup,GetAccountGroupDto>();
            CreateMap<LoanAccount,LoanAccountDto>();
           

        }
    }
}
