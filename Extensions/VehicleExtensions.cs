using AltV.Net.Client;
using AltV.Net.Client.Elements.Entities;
using AltV.Net.Client.Elements.Interfaces;

namespace AltV.Atlas.Vehicles.Client.Extensions;

/// <summary>
/// Extension methods for vehicles
/// </summary>
public static class VehicleExtensions
{
    /// <summary>
    /// Set a ped into a vehicle at given seat
    /// </summary>
    /// <param name="ped">The ped to set into the vehicle</param>
    /// <param name="vehicleId">The vehicle ScriptId to put the ped into</param>
    /// <param name="seatIndex">The seat index to put the ped in to (uses natives, so start from -1)</param>
    /// <returns>True if succeeded, false if not</returns>
    public static Task<bool> SetIntoVehicle( this IPed ped, uint vehicleId, int seatIndex )
    {
        return SetIntoVehicle( ped.ScriptId, vehicleId, seatIndex );
    }
    
    /// <summary>
    /// Set a ped into a vehicle at given seat
    /// </summary>
    /// <param name="ped">The ped to set into the vehicle</param>
    /// <param name="vehicle">The vehicle to put the ped into</param>
    /// <param name="seatIndex">The seat index to put the ped in to (uses natives, so start from -1)</param>
    /// <returns>True if succeeded, false if not</returns>
    public static Task<bool> SetIntoVehicle( this IPed ped, IVehicle vehicle, int seatIndex )
    {
        return SetIntoVehicle( ped.ScriptId, vehicle.ScriptId, seatIndex );
    }
    
    /// <summary>
    /// Set a ped into a vehicle at given seat
    /// </summary>
    /// <param name="ped">The ped to set into the vehicle</param>
    /// <param name="vehicle">The vehicle to put the ped into</param>
    /// <param name="seatIndex">The seat index to put the ped in to (uses natives, so start from -1)</param>
    /// <returns>True if succeeded, false if not</returns>
    public static Task<bool> SetIntoVehicle( this IPed ped, IVehicle vehicle, int seatIndex, uint maxAttempts = 100, uint intervalMs = 50 )
    {
        return SetIntoVehicle( ped.ScriptId, vehicle.ScriptId, seatIndex, maxAttempts, intervalMs );
    }
    
    /// <summary>
    /// Set a player into a vehicle at given seat
    /// </summary>
    /// <param name="player">The player to set into the vehicle</param>
    /// <param name="vehicleId">The vehicle ScriptId to put the player into</param>
    /// <param name="seatIndex">The seat index to put the player in to (uses natives, so start from -1)</param>
    /// <returns>True if succeeded, false if not</returns>
    public static Task<bool> SetIntoVehicle( this IPlayer player, uint vehicleId, int seatIndex )
    {
        return SetIntoVehicle( player.ScriptId, vehicleId, seatIndex );
    }
    
    /// <summary>
    /// Set a player into a vehicle at given seat
    /// </summary>
    /// <param name="player">The player to set into the vehicle</param>
    /// <param name="vehicle">The vehicle to put the player into</param>
    /// <param name="seatIndex">The seat index to put the player in to (uses natives, so start from -1)</param>
    /// <returns>True if succeeded, false if not</returns>
    public static Task<bool> SetIntoVehicle( this IPlayer player, IVehicle vehicle, int seatIndex )
    {
        return SetIntoVehicle( player.ScriptId, vehicle.ScriptId, seatIndex );
    }
    
    /// <summary>
    /// Set a entity into a vehicle at given seat
    /// </summary>
    /// <param name="entityId">The ScriptId of the entity to set into the vehicle</param>
    /// <param name="vehicleId">The vehicle ScriptId to put the entity into</param>
    /// <param name="seatIndex">The seat index to put the entity in to (uses natives, so start from -1)</param>
    /// <returns>True if succeeded, false if not</returns>
    public static Task<bool> SetIntoVehicle( uint entityId, uint vehicleId, int seatIndex, uint maxAttempts = 100, uint intervalMs = 50 )
    {
        Alt.Natives.SetPedIntoVehicle( entityId, vehicleId, -1 );

        if( Alt.Natives.IsPedInVehicle( entityId, vehicleId, false ) )
            return Task.FromResult( true );

        uint interval = 0, attempts = 0;
        var taskCompletionSource = new TaskCompletionSource<bool>( );

        interval = Alt.SetInterval( ( ) =>
        {
            attempts++;
            if( attempts >= maxAttempts )
            {
                Alt.LogError( "[PED-TRAFFIC] Failed to set ped into vehicle" );
                taskCompletionSource.SetResult( false );
                Alt.ClearInterval( interval );
                return;
            }

            Alt.Natives.SetPedIntoVehicle(entityId, vehicleId, -1 );

            if( Alt.Natives.IsPedInVehicle( entityId, vehicleId, false ) )
            {
                taskCompletionSource.SetResult( true );
                Alt.ClearInterval( interval );
            }
        }, intervalMs );

        return taskCompletionSource.Task;
    }
}