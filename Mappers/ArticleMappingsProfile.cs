using AutoMapper;
using UI_Museum.Models;

namespace Application.Mappers
{
    public class ArticleMappingsProfile : Profile
    {
        public ArticleMappingsProfile()
        {
            /*CreateMap<ArticleResponseViewModel, ArticleRequestViewModel>()
                .ForMember(dest => dest., opt => opt.MapFrom(src => src.IdMuseumNavigation.Name))
                .ReverseMap();

            /*CreateMap<Category, CategorySelectResponseViewModel>()
                .ForMember(x => x.CategoryId, x => x.MapFrom(y => y.Id))
                .ReverseMap();*/
        }
    }
}
