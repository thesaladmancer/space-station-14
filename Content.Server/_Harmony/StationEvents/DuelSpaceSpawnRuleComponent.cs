using Content.Server._Harmony.StationEvents;
using Content.Server._Harmony.StationEvents.Events;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server.StationEvents.Components;

/// <summary>
/// Component for spawning antags in space on opposite sides of a station.
/// Requires <c>AntagSelectionComponent</c>.
/// </summary>
[RegisterComponent, Access(typeof(DuelSpaceSpawnRuleSystem))]
public sealed partial class DuelSpaceSpawnRuleComponent : Component
{
    /// <summary>
    /// Distance that the entity spawns from the station's half AABB radius
    /// </summary>
    [DataField]
    public float SpawnDistance = 20f;

    /// <summary>
    /// Location that was picked.
    /// </summary>
    [DataField]
    public List<MapCoordinates> Coords;
}
