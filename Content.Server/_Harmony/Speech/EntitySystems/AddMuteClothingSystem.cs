using Content.Server._Harmony.Speech.Components;
using Content.Shared.Clothing;
using Content.Shared.Speech.Muting;
using Content.Shared.StatusEffect;

namespace Content.Server._Harmony.Speech.EntitySystems;

public sealed class AddMuteClothingSystem : EntitySystem
{
    [Dependency] private readonly StatusEffectsSystem _statusEffectsSystem = default!;

    [ValidatePrototypeId<StatusEffectPrototype>]
    private const string MuteKey = "Muted";

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AddMuteClothingComponent, ClothingGotEquippedEvent>(OnGotEquipped);
        SubscribeLocalEvent<AddMuteClothingComponent, ClothingGotUnequippedEvent>(OnGotUnequipped);
    }

    private void OnGotEquipped(EntityUid uid, AddMuteClothingComponent component, ref ClothingGotEquippedEvent args)
    {
        if (_statusEffectsSystem.HasStatusEffect(args.Wearer, MuteKey))
            _statusEffectsSystem.TryRemoveStatusEffect(args.Wearer, MuteKey);

        if (HasComp<MutedComponent>(args.Wearer))
            return;

        AddComp<MutedComponent>(args.Wearer);
        component.IsActive = true;
    }

    private void OnGotUnequipped(EntityUid uid, AddMuteClothingComponent component, ref ClothingGotUnequippedEvent args)
    {
        if (!component.IsActive)
            return;

        if (!_statusEffectsSystem.HasStatusEffect(args.Wearer, MuteKey) && EntityManager.HasComponent<MutedComponent>(args.Wearer))
            EntityManager.RemoveComponent<MutedComponent>(args.Wearer);

        component.IsActive = false;
    }
}
