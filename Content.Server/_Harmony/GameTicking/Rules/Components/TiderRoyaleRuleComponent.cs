using Content.Server._Harmony.GameTicking.Rules;
using Content.Shared.Roles;
using Robust.Shared.Audio;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server._Harmony.GameTicking.Rules.Components;

/// <summary>
/// Gamerule that ends when only one player is left alive.
/// </summary>
[RegisterComponent, Access(typeof(TiderRoyaleRuleSystem))]
public sealed partial class TiderRoyaleRuleComponent : Component
{
    /// <summary>
    /// The amount of time before players are allowed to begin fighting.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan GracePeriod = TimeSpan.FromMinutes(10);

    /// <summary>
    /// The amount of time until the round restarts after ending.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan RestartDelay = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The job that every player will be forced as.
    /// </summary>
    [DataField]
    public ProtoId<JobPrototype> Job = new();

    /// <summary>
    /// The person who won.
    /// </summary>
    [DataField]
    public NetUserId? Victor;

    /// <summary>
    /// Indicates the time that the grace period started.
    /// </summary>
    [DataField]
    public TimeSpan GracePeriodStartedTime;

    /// <summary>
    /// Indicates whether or not the grace period has ended.
    /// </summary>
    [DataField]
    public bool IsGracePeriodOver = false;

    /// <summary>
    /// The alert level to set when the grace period has ended.
    /// </summary>
    [DataField]
    public string AlertLevelOnGracePeriodEnd = "red";

    /// <summary>
    /// The sound that plays globally when the grace period has ended.
    /// </summary>
    [DataField]
    public SoundPathSpecifier GracePeriodEndSound = new("/Audio/Misc/gamma.ogg");
}
