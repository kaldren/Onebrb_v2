using AutoMapper;
using Onebrb.MVC.Areas.Manager.Models;
using Onebrb.MVC.Areas.Manager.ViewModels.Company;
using Onebrb.MVC.Areas.Manager.ViewModels.Job;
using Onebrb.MVC.Areas.Message.Models;
using Onebrb.MVC.Areas.Message.ViewModels.Message;
using Onebrb.MVC.Areas.Search.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onebrb.MVC.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Job, ViewAllJobsByCompanyVM>()
                .ForMember(x => x.ManagerUserName, opt => opt.MapFrom(src => src.Company.Manager.UserName))
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            CreateMap<Job, ViewSingleJobVM>()
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(x => x.CompanyLogoFullPath, opt => opt.MapFrom(src => src.Company.LogoPath));

            CreateMap<EditCompanyVM, Company>()
                .ReverseMap();

            CreateMap<EditJobOfferVM, Job>()
                .ReverseMap();

            CreateMap<CreateMessageVM, Message>()
                .ReverseMap();

            CreateMap<ViewMessageVM, Message>()
                .ReverseMap();

            CreateMap<Job, SearchResultsVM>()
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ReverseMap();
        }
    }
}
