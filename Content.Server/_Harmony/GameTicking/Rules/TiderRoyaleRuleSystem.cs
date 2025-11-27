using System.Linq;
using Content.Server._Harmony.GameTicking.Rules.Components;
using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Server.RoundEnd;
using Content.Server.Station.Systems;
using Content.Shared.GameTicking;
using Content.Shared.GameTicking.Components;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs;
using Content.Server.TiderRoyale;
using Content.Shared.CombatMode.Pacification;
using Robust.Shared.Timing;
using Content.Server.Station.Components;
using Content.Server.AlertLevel;
using Content.Server.Chat.Systems;
using Content.Server.Mind;

namespace Content.Server._Harmony.GameTicking.Rules;

/// <summary>
/// Manages <see cref="TiderRoyaleRuleComponent"/>
/// </summary>
public sealed class TiderRoyaleRuleSystem : GameRuleSystem<TiderRoyaleRuleComponent>
{
    [Dependency] private readonly AlertLevelSystem _alertLevel = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly RoundEndSystem _roundEnd = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;
    [Dependency] private readonly StationJobsSystem _stationJobs = default!;
    [Dependency] private readonly StationSpawningSystem _stationSpawning = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerBeforeSpawnEvent>(OnBeforeSpawn);

        SubscribeLocalEvent<TiderComponent, ComponentRemove>(OnComponentRemove);
        SubscribeLocalEvent<TiderComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    protected override void Started(EntityUid uid, TiderRoyaleRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        if (!TryGetRandomStation(out var station, HasComp<StationJobsComponent>))
            return;

        var jobList = _stationJobs.GetJobs(station.Value).Keys.ToList();

        if (jobList.Count == 0)
            return;

        foreach (var job in jobList)
            _stationJobs.MakeJobUnlimited(station.Value, job);

        component.GracePeriodStartedTime = _gameTiming.CurTime;
        Log.Debug("Tider Royale Rule component started.");
    }

    private void OnBeforeSpawn(PlayerBeforeSpawnEvent ev)
    {
        var query = EntityQueryEnumerator<TiderRoyaleRuleComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var tr, out var rule))
        {
            if (!GameTicker.IsGameRuleActive(uid, rule))
                continue;

            var newMind = _mind.CreateMind(ev.Player.UserId);
            _mind.SetUserId(newMind, ev.Player.UserId);

            var mobMaybe = _stationSpawning.SpawnPlayerCharacterOnStation(ev.Station, tr.Job, ev.Profile);
            var mob = mobMaybe!.Value;

            _mind.TransferTo(newMind, mob);

            EnsureComp<TiderComponent>(mob);

            if (!tr.IsGracePeriodOver)
                EnsureComp<PacifiedComponent>(mob);

            ev.Handled = true;
            break;
        }
    }

    private void OnComponentRemove(Entity<TiderComponent> ent, ref ComponentRemove args)
    {
        CheckRoundShouldEnd();
    }

    private void OnMobStateChanged(Entity<TiderComponent> ent, ref MobStateChangedEvent args)
    {
        CheckRoundShouldEnd();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<TiderRoyaleRuleComponent, GameRuleComponent>();
        while (query.MoveNext(out var uid, out var tr, out var rule))
        {
            if (_gameTiming.CurTime <= tr.GracePeriodStartedTime + tr.GracePeriod)
                continue;

            if (!tr.IsGracePeriodOver)
            {
                var message = Loc.GetString("tider-royale-announce-grace-period-over");
                var sender = Loc.GetString("tider-royale-announce-sender");
                var sound = tr.GracePeriodEndSound;
                var color = Color.Red;

                _chat.DispatchGlobalAnnouncement(message, sender, true, sound, color);

                if (TryGetRandomStation(out var station) && _alertLevel.GetLevel(station.Value) != tr.AlertLevelOnGracePeriodEnd)
                {
                    _alertLevel.SetLevel(station.Value, tr.AlertLevelOnGracePeriodEnd, false, false, true);
                }

                var tiders = EntityQueryEnumerator<TiderComponent, PacifiedComponent>();
                while (tiders.MoveNext(out var tideruid, out _, out _))
                {
                    if (HasComp<PacifiedComponent>(tideruid))
                        RemComp<PacifiedComponent>(tideruid);
                }

                tr.IsGracePeriodOver = true;
            }
        }
    }

    private void CheckRoundShouldEnd()
    {
        var query = QueryActiveRules();
        while (query.MoveNext(out var uid, out _, out var tiderRoyale, out _))
        {
            CheckRoundShouldEnd((uid, tiderRoyale));
        }
    }

    private void CheckRoundShouldEnd(Entity<TiderRoyaleRuleComponent> ent)
    {
        var players = EntityQuery<TiderComponent, MobStateComponent>(true);
        var playersAlive = players.Where(player => player.Item2.CurrentState == MobState.Alive || player.Item2.CurrentState == MobState.Critical);

        if (playersAlive.Count() > 1)
            return; // We are NOT done yet.
        else
        {
            _roundEnd.EndRound(ent.Comp.RestartDelay);
        }
    }

    protected override void AppendRoundEndText(EntityUid uid, TiderRoyaleRuleComponent component, GameRuleComponent gameRule, ref RoundEndTextAppendEvent args)
    {

    }
}
