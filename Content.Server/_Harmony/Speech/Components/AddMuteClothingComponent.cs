using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server._Harmony.Speech.Components;

/// <summary>
///     Applies mute to a user while they wear entity as a clothing.
/// </summary>
[RegisterComponent]
public sealed partial class AddMuteClothingComponent : Component
{
    /// <summary>
    ///     Is that clothing is worn and making someone mute?
    /// </summary>
    public bool IsActive = false;
}
