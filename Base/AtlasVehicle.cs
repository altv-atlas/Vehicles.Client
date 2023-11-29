using AltV.Atlas.Vehicles.Client.Interfaces;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Entities;
using AltV.Net.Client.Elements.Interfaces;
using AltV.Net.Elements.Entities;
namespace AltV.Atlas.Vehicles.Client.Base;

public class AtlasVehicle : Vehicle, IAtlasClientVehicle
{
    public AtlasVehicle( ICore core, IntPtr vehiclePointer, uint id ) : base( core, vehiclePointer, id )
    {
        Alt.Log( "atlas vehicle ctor" );
        Alt.OnGameEntityCreate += OnGameEntityCreate;
        Alt.OnStreamSyncedMetaChange += OnStreamSyncedMetaChange;
    }

    private void OnGameEntityCreate( IEntity entity )
    {
        if( entity is not IAtlasClientVehicle atlasVehicle || atlasVehicle.Id != Id )
            return;

        Alt.Log( "WE GOT HERE" );

        if( !atlasVehicle.HasStreamSyncedMetaData( "changeWheels" ) )
            return;

        atlasVehicle.GetStreamSyncedMetaData( "changeWheels", out float result );
        ChangeWheels( result );

    }

    private void OnStreamSyncedMetaChange( IBaseObject target, string key, object value, object oldValue )
    {
        if( target is not IAtlasClientVehicle atlasVehicle || atlasVehicle.Id != Id )
            return;

        switch( key )
        {
            case "changeWheels":
                if( value is float floatValue )
                    ChangeWheels( floatValue );
                break;
        }
    }

    protected virtual void ChangeWheels( float value )
    {
        Alt.LogError( "changewheels" );

        for( byte i = 0; i < WheelsCount; i++ )
        {
            SetWheelCamber( i, value );
        }

        // if( value is not WheelMod)
        //     return;
        //
        //
        // SetWheelCamber( 0, 1.5f );
        // SetWheelHeight(  );
        // SetWheelRimRadius(  );
        // SetWheelTrackWidth(  );
        // SetWheelTyreRadius(  );

    }

}