using Api.Controllers.v1.Products.Requests;
using Application.Products.Command.AddProduct;
using AutoMapper;

namespace Api.AutoMapperProfiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductRequest, AddProductCommand>();
        }
    }
}
