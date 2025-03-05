using Content.Server.Antag;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.GridPreloader;
using Content.Server.StationEvents.Events;
using Content.Shared.GameTicking.Components;
using Robust.Server.GameObjects;
using Robust.Server.Maps;
using Robust.Shared.Prototypes;
using Content.Server._Harmony.GameTicking.Rules.Components;

namespace Content.Server._Harmony.GameTicking.Rules;

public sealed class LoadMapsRuleSystem : StationEventSystem<LoadMapsRuleComponent>
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly MapSystem _map = default!;
    [Dependency] private readonly MapLoaderSystem _mapLoader = default!;
    [Dependency] private readonly TransformSystem _transform = default!;
    [Dependency] private readonly GridPreloaderSystem _gridPreloader = default!;

    protected override void Added(EntityUid uid, LoadMapsRuleComponent comp, GameRuleComponent rule, GameRuleAddedEvent args)
    {
        if (comp.PreloadedGrid != null && !_gridPreloader.PreloadingEnabled)
        {
            // Preloading is disabled, return
            Log.Debug($"Immediately ending {ToPrettyString(uid):rule} as preloading grids is disabled by cvar.");
            ForceEndSelf(uid, rule);
            return;
        }

        // Grid preloading needs map to init after moving it
        // TO DO: DON'T FUCKING DO THIS
        var mapUid = _map.CreateMap()

        var ev = new RuleLoadedGridsEvent(mapId);
        base.Added(uid, comp, rule, args);
    }
}
