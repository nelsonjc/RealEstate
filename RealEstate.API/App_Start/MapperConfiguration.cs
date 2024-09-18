using AutoMapper;
using RealEstate.Core.DTOs;
using RealEstate.Core.Entities;
using RealEstate.Core.Interfaces.Utils;

namespace RealEstate.API.App_Start
{
    public static class MapperConfig
    {
        internal static IServiceCollection AddUtoMapperConfig(this IServiceCollection services)
        {
            // Agregar AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Configurar Mapper con el servicio de File
            services.AddSingleton(provider =>
            {
                var fileService = provider.GetRequiredService<IFileService>();
                var mapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MappingProfile(fileService));
                });
                return mapperConfig.CreateMapper();
            });

            return services;
        }

        public class MappingProfile : Profile
        {
            private readonly IFileService _fileService;

            public MappingProfile(IFileService fileService)
            {
                _fileService = fileService;

                CreateMap<PropertyCreationRequestDto, Property>();
                CreateMap<PropertyTraceCreationRequestDto, PropertyTrace>();
                CreateMap<PropertyImageCreationRequestDto, PropertyImage>();

                CreateMap<Property, PropertyDto>().ForMember(x => x.IdProperty, o => o.MapFrom(s => s.Id));
                CreateMap<Owner, OwnerDto>().ForMember(x => x.IdOwner, o => o.MapFrom(s => s.Id));
                CreateMap<PropertyTrace, PropertyTraceDto>().ForMember(x => x.IdPropertyTrace, o => o.MapFrom(s => s.Id));

                CreateMap<PropertyImage, PropertyImageDto>()
                 .ForMember(x => x.IdPropertyImage, o => o.MapFrom(s => s.Id))
                 .ForMember(x => x.FileUrl, o => o.MapFrom(s => _fileService.GetFullPath(s.FileUrl)));
            }
        }
    }
}
