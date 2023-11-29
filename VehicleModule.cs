using AltV.Atlas.Vehicles.Client.Base;
using AltV.Atlas.Vehicles.Client.Factories;
using AltV.Atlas.Vehicles.Client.Interfaces;
using AltV.Net.Client.Elements.Entities;
using AltV.Net.Client.Elements.Factories;
using AltV.Net.Client.Elements.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace AltV.Atlas.Vehicles.Client;

/// <summary>
/// Main entry point to client-side vehicle module
/// </summary>
public static class VehicleModule
{
    /// <summary>
    /// Registers the vehicle module and it's classes/interfaces
    /// </summary>
    /// <param name="serviceCollection">A service collection</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection RegisterVehicleModule( this IServiceCollection serviceCollection )
    {
        serviceCollection.AddTransient<IAtlasClientVehicle, AtlasVehicle>( );
        serviceCollection.AddTransient<IVehicle, Vehicle>( );

        serviceCollection.AddTransient<IEntityFactory<IVehicle>, AltVehicleFactory>( );

        return serviceCollection;
    }
}