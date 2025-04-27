using Content.Server.StationEvents.Events;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(BureaucraticErrorRule))]
public sealed partial class BureaucraticErrorRuleComponent : Component
{
    /// <summary>
    /// The jobs that are ignored by this rule and won't have their slots changed.
    /// </summary>
    [DataField]
    public List<ProtoId<JobPrototype>> IgnoredJobs = new();

    // Start of Harmony change of Bureaucratic Error event
    /// <summary>
    /// The minimum number of jobs that can be set to unlimited slots.
    /// </summary>
    [DataField]
    public int MinimumJobs = 1;

    /// <summary>
    /// The maximum number of jobs that can be set to unlimited slots.
    /// </summary>
    [DataField]
    public int MaximumJobs = 4;
    // End of Harmony change of Bureaucratic Error event
}
