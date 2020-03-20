using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DFC.App.MatchSkills.Application.Dysac.Models;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Services.Dysac
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<JobCategory, DysacJobCategory>()
                .ForMember(dest => dest.JobFamilyCode, opt => opt.MapFrom(src => src.JobFamilyCode))
                .ForMember(dest => dest.JobFamilyName, opt => opt.MapFrom(src => src.JobFamilyName))
                .ForMember(dest => dest.JobFamilyUrl, opt => opt.MapFrom(src => src.JobFamilyUrl))
                .ConstructUsing(dest => new DysacJobCategory())
                ;
        }
    }
}
