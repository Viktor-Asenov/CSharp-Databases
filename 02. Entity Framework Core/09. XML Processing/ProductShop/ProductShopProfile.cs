using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //this.CreateMap<ImportUserDto, User>();
            //
            //this.CreateMap<ImportProductDto, Product>();
            //
            //this.CreateMap<ImportCategoryDto, Category>();
            //
            //this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
            //
            //this.CreateMap<Product, ExportProductInRangeDto>()
            //    .ForMember(x => x.Buyer, y => y.MapFrom(s => s.Buyer.FirstName + " " + s.Buyer.LastName));
        }
    }
}
