using AutoMapper;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Helpers;
using DFC.App.MatchSkills.Services.ServiceTaxonomy.Models;
using DFC.Personalisation.Common.Extensions;
using DFC.Personalisation.Domain.Models;
using System;
using System.Linq;
using DFC.App.MatchSkills.Application.ServiceTaxonomy.Models;
using Microsoft.Azure.Cosmos.Linq;

namespace DFC.App.MatchSkills.Services.ServiceTaxonomy
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
            CreateMap<StSkill, Skill>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill.FirstCharToUpper()))
                .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.AlternativeLabels))
                .ConstructUsing(dest => new Skill(dest.Uri, dest.Skill, (SkillType)Enum.Parse(typeof(SkillType), dest.SkillType, true)))
                ;
            CreateMap<StOccupation, Occupation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Occupation.FirstCharToUpper()))
                .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.AlternativeLabels))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ConstructUsing(dest => new Occupation(dest.Uri, dest.Occupation, dest.LastModified, dest.Description));

            CreateMap<StOccupationSearchResult.StsOccupation, Occupation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Occupation.FirstCharToUpper()))
                .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.AlternativeLabels))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ConstructUsing(dest => new Occupation(dest.Uri, dest.Occupation, dest.LastModified, dest.AlternativeLabels, dest.Description));

            CreateMap<StOccupationSkills.StOsSkill, Skill>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill.FirstCharToUpper()))
                .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.AlternativeLabels))
                .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.Type))

                .ConstructUsing(dest => new Skill(dest.Uri, dest.Skill, (SkillType)Enum.Parse(typeof(SkillType), dest.Type, true)));

            CreateMap<GetOccupationsWithMatchingSkillsResponse.MatchedOccupation, OccupationMatch>()
                .ForMember(dest => dest.JobProfileDescription, opt => opt.MapFrom(src => MappingHelper.StripHTML(src.JobProfileDescription)));

            CreateMap<StLabelSkill, Skill>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Uri))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill.FirstCharToUpper()))
                .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.AlternativeLabels))
                .ForMember(dest => dest.SkillType, opt => opt.MapFrom(src => src.SkillType))

                .ConstructUsing(dest => new Skill(dest.Uri, dest.Skill, (SkillType)Enum.Parse(typeof(SkillType), dest.SkillType, true)));

            CreateMap<SkillsGapAnalysis, SkillsGap>()
                .ForMember(dest => dest.CareerTitle, opt => opt.MapFrom(src => src.Occupation))
                .ForMember(dest => dest.MissingSkills, opt => opt.MapFrom(src => src.MissingSkills.Where(x => x.RelationshipType == RelationshipType.Essential).Select(x => x.Skill)))
                .ForMember(dest => dest.MatchingSkills, opt => opt.MapFrom(src => src.MatchingSkills.Where(x => x.RelationshipType == RelationshipType.Essential).Select(x => x.Skill)))
                .ForMember(dest => dest.OptionalMissingSkills, opt => opt.MapFrom(src => src.MissingSkills.Where(x => x.RelationshipType == RelationshipType.Optional).Select(x => x.Skill)))
                .ForMember(dest => dest.OptionalMatchingSkills, opt => opt.MapFrom(src => src.MatchingSkills.Where(x => x.RelationshipType == RelationshipType.Optional).Select(x => x.Skill)))
                .ForMember(dest => dest.CareerDescription, opt => opt.MapFrom(src => src.Description))
                .ConstructUsing(dest => new SkillsGap());
        }

    }

}
