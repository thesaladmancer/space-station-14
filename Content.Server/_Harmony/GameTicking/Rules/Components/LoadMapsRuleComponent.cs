using Content.Server.Maps;
using Content.Shared.GridPreloader.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._Harmony.GameTicking.Rules.Components;

/// <summary>
/// This is used for game rules that load multiple of the same map when activated.
/// Works with <see cref="RuleGridsComponent"/>
/// </summary>
[RegisterComponent, Access(typeof(LoadMapsRuleSystem))]
public sealed partial class LoadMapsRuleComponent : Component
{
    [DataField]
    public ProtoId<GameMapPrototype>? GameMap;

    [DataField]
    public ResPath? MapPath;

    [DataField]
    public ProtoId<PreloadedGridPrototype>? PreloadedGrid;
}
