using AutoMapper;
using CourseSellingApp.Database.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CourseSellingApp.Utils
{
    /// <summary>
    /// Профиль AutoMapper для определения правил сопоставления между объектами-сущностями (Entities)
    /// и объектами модели представления (Models).
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Карта из сущности БД (Database.Entities.Course) в модель приложения (Models.Course)
            CreateMap<Database.Entities.Course, Models.Course>()
                // Специальная обработка для полей, хранящихся как JSON-строки в БД
                .ForMember(dest => dest.ImagePaths,
                           opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ImagePaths)
                               ? new List<string>()
                               : JsonConvert.DeserializeObject<List<string>>(src.ImagePaths) ?? new List<string>()))
                .ForMember(dest => dest.RelatedCoursesIds,
                           opt => opt.MapFrom(src => string.IsNullOrEmpty(src.RelatedCoursesIds)
                               ? new List<int>()
                               : JsonConvert.DeserializeObject<List<int>>(src.RelatedCoursesIds) ?? new List<int>()))
                // Игнорируем поля 'Category' и 'Author', т.к. они будут заполняться в сервисном слое
                // на основе данных из связанных таблиц (CategoryName, AuthorName), а не из самой сущности Course.
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore());


            // Карта из модели приложения (Models.Course) в сущность БД (Database.Entities.Course)
            CreateMap<Models.Course, Database.Entities.Course>()
                // Специальная обработка для полей, которые нужно сериализовать в JSON для хранения в БД
                .ForMember(dest => dest.ImagePaths,
                           opt => opt.MapFrom(src => src.ImagePaths != null && src.ImagePaths.Any()
                               ? JsonConvert.SerializeObject(src.ImagePaths)
                               : null))
                .ForMember(dest => dest.RelatedCoursesIds,
                           opt => opt.MapFrom(src => src.RelatedCoursesIds != null && src.RelatedCoursesIds.Any()
                               ? JsonConvert.SerializeObject(src.RelatedCoursesIds)
                               : null))
                // Игнорируем поля, которые не сопоставляются напрямую или устанавливаются другим способом.
                // CategoryId и AuthorId будут установлены в сервисе перед сохранением.
                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId, opt => opt.Ignore())
                // CreatedAt устанавливается базой данных по умолчанию.
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                // CoverImage (массив байт) будет обрабатываться отдельно в сервисе.
                .ForMember(dest => dest.CoverImage, opt => opt.Ignore());
        }
    }
}
