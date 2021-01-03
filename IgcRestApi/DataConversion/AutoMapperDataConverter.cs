using AutoMapper;
using System;

namespace IgcRestApi.DataConversion
{
    public class AutoMapperDataConverter : IDataConverter
    {
        private readonly IMapper _mapper;

        public AutoMapperDataConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public T Convert<T>(object src)
        {
            return _mapper.Map<T>(src);
        }

        public TDest Convert<TSrc, TDest>(TSrc src)
        {
            return _mapper.Map<TSrc, TDest>(src);
        }

        public object Convert(object source, Type sourceType, Type destinationType)
        {
            return _mapper.Map(source, sourceType, destinationType);
        }

        public void AssertConfigurationIsValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        public TDest ConvertAndMerge<TDest>(object src, object destination) where TDest : class
        {
            return _mapper.Map(src, destination, src.GetType(), destination.GetType()) as TDest;
        }


        /// <summary>
        /// GetDataConverter
        /// </summary>
        /// <returns></returns>
        public static IDataConverter GetDataConverter()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<IgcRestApiMappingProfile>());
            var mapper = mapperConfiguration.CreateMapper();
            return new AutoMapperDataConverter(mapper);
        }
    }
}
