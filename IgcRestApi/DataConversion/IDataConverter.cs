using System;

namespace IgcRestApi.DataConversion
{
    public interface IDataConverter
    {
        T Convert<T>(object src);

        object Convert(object source, Type sourceType, Type destinationType);

        TDest Convert<TSrc, TDest>(TSrc src);

        void AssertConfigurationIsValid();        

        TDest ConvertAndMerge<TDest>(object src, object destination) where TDest : class;
    }
}
