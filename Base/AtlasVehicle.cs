using System.Text.Json;
using AltV.Atlas.Vehicles.Client.Interfaces;
using AltV.Atlas.Vehicles.Shared;
using AltV.Atlas.Vehicles.Shared.Models;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Entities;
using AltV.Net.Client.Elements.Interfaces;
namespace AltV.Atlas.Vehicles.Client.Base;

/// <summary>
/// Client-side atlas vehicle
/// </summary>
public class AtlasVehicle : Vehicle, IAtlasClientVehicle
{
    /// <summary>
    /// Client-side atlas vehicle
    /// </summary>
    /// <param name="core">&gt;Alt core</param>
    /// <param name="vehiclePointer">Native pointer</param>
    /// <param name="id">ID of the vehicle</param>
    public AtlasVehicle( ICore core, IntPtr vehiclePointer, uint id ) : base( core, vehiclePointer, id )
    {
        Alt.OnGameEntityCreate += OnGameEntityCreate;
        Alt.OnStreamSyncedMetaChange += OnStreamSyncedMetaChange;
    }

    private void OnGameEntityCreate( IEntity entity )
    {
        if( entity is not IAtlasClientVehicle atlasVehicle || atlasVehicle.Id != Id )
            return;

        if( !atlasVehicle.HasStreamSyncedMetaData( VehicleConstants.ChangeWheelsMetaKey ) )
            return;

        atlasVehicle.GetStreamSyncedMetaData( VehicleConstants.ChangeWheelsMetaKey, out string result );
        ChangeWheels( result );

    }

    private void OnStreamSyncedMetaChange( IBaseObject target, string key, object value, object oldValue )
    {
        if( target is not IAtlasClientVehicle atlasVehicle || atlasVehicle.Id != Id )
            return;

        switch( key )
        {
            case VehicleConstants.ChangeWheelsMetaKey:
                if( value is string strValue )
                    ChangeWheels( strValue );
                break;
        }
    }
    /// <summary>
    /// Changes the vehicles wheel mods
    /// </summary>
    /// <param name="value"></param>
    protected virtual void ChangeWheels( string value )
    {
        var wheelData = JsonSerializer.Deserialize<List<WheelMod>>( value );

        if( wheelData == null )
            return;

        ChangeWheels( wheelData );
    }

    /// <summary>
    /// Changes the vehicles wheel mods
    /// </summary>
    /// <param name="wheelMods">The wheel mods to apply</param>
    protected virtual void ChangeWheels( List<WheelMod> wheelMods )
    {
        foreach( var wheelMod in wheelMods )
        {
            SetWheelCamber( wheelMod.Index, wheelMod.Camber );
            SetWheelHeight( wheelMod.Index, wheelMod.Height );
            SetWheelRimRadius( wheelMod.Index, wheelMod.RimRadius );
            SetWheelTrackWidth( wheelMod.Index, wheelMod.TrackWidth );
            SetWheelTyreRadius( wheelMod.Index, wheelMod.TyreRadius );
            SetWheelTyreWidth( wheelMod.Index, wheelMod.TyreWidth );
        }
    }
}