using Content.Server.Antag;
using Content.Server.Station.Components;
using Content.Server.StationEvents.Components;
using Content.Server.StationEvents.Events;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Map.Components;

namespace Content.Server._Harmony.StationEvents.Events;

/// <summary>
/// Station event component for spawning this rule's antags in space around a station.
/// </summary>
public sealed class DuelSpaceSpawnRuleSystem : StationEventSystem<DuelSpaceSpawnRuleComponent>
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DuelSpaceSpawnRuleComponent, AntagSelectLocationEvent>(OnSelectLocation);
    }

    protected override void Added(EntityUid uid, DuelSpaceSpawnRuleComponent comp, GameRuleComponent gameRule, GameRuleAddedEvent args)
    {
        base.Added(uid, comp, gameRule, args);

        if (!TryGetRandomStation(out var station))
        {
            ForceEndSelf(uid, gameRule);
            return;
        }

        var stationData = Comp<StationDataComponent>(station.Value);

        // Find a station grid
        var gridUid = StationSystem.GetLargestGrid(stationData);
        if (gridUid == null || !TryComp<MapGridComponent>(gridUid, out var grid))
        {
            Sawmill.Warning($"Chosen station has no grids, cannot pick location for {ToPrettyString(uid):rule}");
            ForceEndSelf(uid, gameRule);
            return;
        }

        // Get station AABB size and use that to determine how far the spawner should be
        var size = grid.LocalAABB.Size.Length() / 2;
        var distance = size + comp.SpawnDistance;
        var angle = RobustRandom.NextAngle();
    }

    private void OnSelectLocation(Entity<DuelSpaceSpawnRuleComponent> ent, ref AntagSelectLocationEvent args)
    {

    }
}
