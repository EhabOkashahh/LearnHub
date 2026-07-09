using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Cart;
using Shared.DTOS.Cart;

namespace Services.Mapping
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<CartDto , Cart>().ReverseMap();
            CreateMap<CartItemDto,CartItem>().ReverseMap();
        }
    }
}