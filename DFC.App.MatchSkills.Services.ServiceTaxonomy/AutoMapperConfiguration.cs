using System;
using AutoMapper;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Extensions;
using DFC.Personalisation.Domain.Models;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
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
            CreateMap<StSkill, Skill>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill.FirstCharToUpper()))
                .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.AlternativeLabels))                     
                .ConstructUsing(dest => new Skill(dest.Uri, dest.Skill, (SkillType)Enum.Parse(typeof(SkillType),dest.SkillType,true)))
                ;
            CreateMap<StOccupation, Occupation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Occupation.FirstCharToUpper()))
                .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.AlternativeLabels))                     
                .ConstructUsing(dest => new Occupation(dest.Uri, dest.Occupation,dest.LastModified));
            
            CreateMap<StOccupationSearchResult.StsOccupation, Occupation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Occupation.FirstCharToUpper()))
                .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.AlternativeLabels))                     
                .ConstructUsing(dest => new Occupation(dest.Uri, dest.Occupation,dest.LastModified));
        }
    }
}
