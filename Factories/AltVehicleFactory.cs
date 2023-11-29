using AltV.Atlas.Vehicles.Client.Base;
using AltV.Atlas.Vehicles.Client.Interfaces;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Factories;
using AltV.Net.Client.Elements.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace AltV.Atlas.Vehicles.Client.Factories;

/// <summary>
/// Entity factory for atlas vehicles
/// </summary>
public class AltVehicleFactory : IEntityFactory<IVehicle>
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates a new instance of ped factory
    /// </summary>
    /// <param name="serviceProvider">service provider that contains the AtlasPed class</param>
    public AltVehicleFactory( IServiceProvider serviceProvider )
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Create a new Atlas vehicle
    /// </summary>
    /// <param name="core">AltV Cor</param>
    /// <param name="entityPointer">Entity pointer</param>
    /// <param name="id">Id of the vehicle</param>
    /// <returns>A new atlas vehicle</returns>
    public IVehicle Create( ICore core, IntPtr entityPointer, uint id )
    {
        Alt.Log( "AltVehicleFactoryCreate" );
        return ActivatorUtilities.CreateInstance<AtlasVehicle>( _serviceProvider, core, entityPointer, id );
    }
}