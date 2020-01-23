using System.Linq;
using System.Reflection;
using AutoMapper;

namespace EFC.AgGateway.Integration.Mapping
{
    public static class AgGatewayMapping
    {
        private static IMapper _mapper = null;

        /// <summary>
        /// This method scans the assembly for any classes that inherit from
        ///     the `AutoMapper.Profile` class and adds each to an
        ///     `AutoMapper.MapperConfiguration` instance from which an
        ///     `AutoMapper.Mapper` instance is created.
        ///     
        /// It uses reflection, so for the sake of resources, it only will
        ///     run once per application instance and store the resulting 
        ///     `AutoMapper.Mapper` instance within this class.
        /// </summary>
        /// <returns>`AutoMapper.Mapper` instance needed for this library.</returns>
        public static IMapper GetMapper()
        {
            if (_mapper == null)
            {
                var mapperConfiguration = new MapperConfiguration(config =>
                {
                    var assembly = Assembly.GetAssembly(typeof(AgGatewayMapping));

                    var profileTypes = assembly.GetTypes().Where(type =>
                    {
                        return typeof(Profile).IsAssignableFrom(type);
                    });

                    foreach (var profileType in profileTypes)
                    {
                        config.AddProfile(profileType);
                    }
                });

                _mapper = new Mapper(mapperConfiguration);
            }

            return _mapper;
        } 
    }
}
