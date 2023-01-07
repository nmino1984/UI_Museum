using AutoMapper;
using UI_Museum.Models;

namespace Application.Mappers
{
    public class MuseumMappingsProfile : Profile
    {
        public MuseumMappingsProfile()
        {
            CreateMap<MuseumResponseViewModel, MuseumRequestViewModel>()
                .ForMember(dest => dest.Theme, opt => opt.MapFrom(src => src.ThemeId))
                .ReverseMap();

        }
    }
}
