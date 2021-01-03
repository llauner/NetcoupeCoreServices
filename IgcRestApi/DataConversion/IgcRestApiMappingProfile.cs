using AutoMapper;
using IgcRestApi.Dto;
using IgcRestApi.Exceptions;
using IgcRestApi.Models;
using System;
using System.Net;

namespace IgcRestApi.DataConversion
{
    public class IgcRestApiMappingProfile : Profile
    {
        /// <summary>
        /// IgcRestApiMappingProfile
        /// </summary>
        public IgcRestApiMappingProfile()
        {
            // Storage
            CreateMap<Google.Apis.Storage.v1.Data.Object, IgcFlightDto>()
                .ForMember(to => to.Id, opt => opt.Ignore())
                .ForMember(to => to.ZipFileName, opt => opt.Ignore())
                .ForMember(to => to.Status, opt => opt.Ignore())
                .ForMember(to => to.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

            // Exception --> Models
            CreateMap<CoreApiException, CoreApiExceptionModel>()
                .ReverseMap();

            CreateMap<Exception, CoreApiExceptionModel>()
                .ForMember(to => to.DateTime, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(to => to.StatusCode, opt => opt.MapFrom(src => HttpStatusCode.InternalServerError))
                ;

            // Dto -> Model
            CreateMap<IgcFlightDto, IgcFlightModel>()
                .ReverseMap();

            CreateMap<CumulativeTracksStatDto, CumulativeTracksStatModel>()
                .ReverseMap();
        }


    }
}
