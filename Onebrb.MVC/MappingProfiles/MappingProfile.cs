using AutoMapper;
using Onebrb.MVC.Areas.Manager.Models;
using Onebrb.MVC.Areas.Manager.ViewModels.Company;
using Onebrb.MVC.Areas.Manager.ViewModels.Job;
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
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            CreateMap<Job, ViewSingleJobVM>()
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(x => x.CompanyLogoFullPath, opt => opt.MapFrom(src => src.Company.LogoPath));

            CreateMap<EditCompanyVM, Company>()
                .ReverseMap();
        }
    }
}
