using OrderSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSystem.App_Start
{
    public class AutoMapperConfig : AutoMapper.Profile
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperConfig>();
            });
        }

        public AutoMapperConfig()
        {
            CreateMap<Orders, OrderViewModel>();
            //CreateMap<OrderViewModel, Orders>();
            //...
        }

    }
}