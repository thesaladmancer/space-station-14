using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

/// <summary>
/// Preset used to define a crime, its criteria, and sentencing duration.
/// </summary>
[Prototype]
public sealed partial class CrimePresetPrototype : IPrototype, IInheritingPrototype
{
    /// </inheritdoc>
    [IdDataField]
    public string ID { get; } = default!;

    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<CrimePresetPrototype>))]
    public string[]? Parents { get; }

    [NeverPushInheritance]
    [AbstractDataField]
    public bool Abstract { get; }

    [DataField(required: true)]
    public LocId Name;

    [DataField(required: true)]
    public LocId Description;

    public TimeSpan Duration;

    public bool Permanent;
}
