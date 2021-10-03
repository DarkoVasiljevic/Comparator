using AutoMapper;
using Comparator.Base;
using Comparator.Dtos;
using Comparator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Comparator.Mapping
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Result, ResultResponse>()
                .ForMember(dest => dest.ResultType,
                           opt => opt.MapFrom(src => ConvertEnumToString(src.ResultType)));

            CreateMap<Diff, DiffResponse>();
            CreateMap<DataRequest, Left>();
            CreateMap<DataRequest, Right>();
        }

        private string ConvertEnumToString(TypeOfResult resultType)
        {
            switch (resultType)
            {
                case TypeOfResult.Equals:
                    return "Equals";
                case TypeOfResult.SizeDoNotMatch:
                    return "SizeDoNotMatch";
                case TypeOfResult.ContentDoNotMatch:
                    return "ContentDoNotMatch";
                default:
                    return string.Empty;
            }
        }
    }
}
