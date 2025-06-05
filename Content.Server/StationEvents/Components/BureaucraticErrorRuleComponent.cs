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
    /// The probability that a single job will be set to infinite slots instead of adding slots to multiple jobs.
    /// </summary>
    [DataField]
    public float InfiniteJobProbability = 0.2f;

    /// <summary>
    /// The minimum percentage of station jobs that will be affected.
    /// </summary>
    [DataField]
    public float MinAffectedJobs = 0.1f;

    /// <summary>
    /// The maximum percentage of station jobs that will be affected.
    /// </summary>
    [DataField]
    public float MaxAffectedJobs = 0.3f;

    /// <summary>
    /// The minimum number of slots that can be added to a job.
    /// </summary>
    [DataField]
    public int MinFiniteAddedSlots = 1;

    /// <summary>
    /// The maximum number of slots that can be added to a job.
    /// </summary>
    [DataField]
    public int MaxFiniteAddedSlots = 4;
    // End of Harmony change of Bureaucratic Error event
}
