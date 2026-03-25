using AutoMapper;
using CourseSellingApp.Database.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CourseSellingApp.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Database.Entities.Course, Models.Course>()
                .ForMember(dest => dest.ImagePaths,
                           opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ImagePaths)
                               ? new List<string>()
                               : JsonConvert.DeserializeObject<List<string>>(src.ImagePaths) ?? new List<string>()))
                .ForMember(dest => dest.RelatedCoursesIds,
                           opt => opt.MapFrom(src => string.IsNullOrEmpty(src.RelatedCoursesIds)
                               ? new List<int>()
                               : JsonConvert.DeserializeObject<List<int>>(src.RelatedCoursesIds) ?? new List<int>()))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore());
            
            CreateMap<Models.Course, Database.Entities.Course>()
                .ForMember(dest => dest.ImagePaths,
                           opt => opt.MapFrom(src => src.ImagePaths != null && src.ImagePaths.Any()
                               ? JsonConvert.SerializeObject(src.ImagePaths)
                               : null))
                .ForMember(dest => dest.RelatedCoursesIds,
                           opt => opt.MapFrom(src => src.RelatedCoursesIds != null && src.RelatedCoursesIds.Any()
                               ? JsonConvert.SerializeObject(src.RelatedCoursesIds)
                               : null))
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CoverImage, opt => opt.Ignore());
        }
    }
}
