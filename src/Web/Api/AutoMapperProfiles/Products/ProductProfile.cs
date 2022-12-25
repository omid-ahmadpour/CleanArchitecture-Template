using AutoMapper;
using CleanTemplate.Api.Controllers.v1.Products.Requests;
using CleanTemplate.Application.Products.Command.AddProduct;
using CleanTemplate.Application.Products.Query.GetProducts;

namespace CleanTemplate.Api.AutoMapperProfiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductRequest, AddProductCommand>();

            CreateMap<GetProductsRequest, GetProductsQuery>();
        }
    }
}
