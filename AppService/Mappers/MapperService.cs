﻿using AppDomain.Entities;
using AppService.Dtos.Accounts;
using AppService.Dtos.Users;
using AutoMapper;

namespace AppService.Mappers
{
    internal static class MapperService
    {
        private static readonly object _locker = new object();
        private static readonly MapperConfiguration _mapperConfig;
        static MapperService()
        {
            _mapperConfig = new MapperConfiguration(config =>
            {
                CreateUserMappers(config);
                CreateAccountMappers(config);
            });
        }
        public static IMapper Mapper
        {
            get
            {
                IMapper result;

                lock(_locker)
                {
                    result = _mapperConfig.CreateMapper();
                }

                return result;
            }
        }

        private static void CreateUserMappers(IMapperConfigurationExpression config)
        {
            config.CreateMap<User, UserDto>().ReverseMap();
        }

        private static void CreateAccountMappers(IMapperConfigurationExpression config)
        {
            config.CreateMap<AccountCreatingDto, Account>().ReverseMap();
            config.CreateMap<AccountCreatingDto, User>().ReverseMap();
        }
    }
}
