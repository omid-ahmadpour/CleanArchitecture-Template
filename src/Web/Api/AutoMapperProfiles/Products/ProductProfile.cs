using AutoMapper;
using CleanTemplate.Api.Controllers.v1.Products.Requests;
using CleanTemplate.Application.Products.Command.AddProduct;

namespace CleanTemplate.Api.AutoMapperProfiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductRequest, AddProductCommand>();
        }
    }
}
