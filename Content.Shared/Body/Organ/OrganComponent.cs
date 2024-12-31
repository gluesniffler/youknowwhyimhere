using Content.Shared.Body.Systems;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;

// Shitmed Change
using Robust.Shared.Prototypes;
using Content.Shared._Shitmed.Medical.Surgery;
using Content.Shared._Shitmed.Medical.Surgery.Tools;
using Content.Shared._Shitmed.Body.BodyCapacity;
using Content.Shared._Shitmed.Body.Organ;

namespace Content.Shared.Body.Organ;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedBodySystem), typeof(SharedSurgerySystem))] // Shitmed Change
public sealed partial class OrganComponent : Component, ISurgeryToolComponent // Shitmed Change
{
    /// <summary>
    /// Relevant body this organ is attached to.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Body;

    /// <summary>
    ///     Shitmed Change:Relevant body this organ originally belonged to.
    ///     FOR WHATEVER FUCKING REASON AUTONETWORKING THIS CRASHES GIBTEST AAAAAAAAAAAAAAA
    /// </summary>
    [DataField]
    public EntityUid? OriginalBody;

    // Shitmed Change Start
    /// <summary>
    ///     Shitmed Change: Shitcodey solution to not being able to know what name corresponds to each organ's slot ID
    ///     without referencing the prototype or hardcoding.
    /// </summary>

    [DataField, AlwaysPushInheritance]
    public string SlotId = "";

    [DataField, AlwaysPushInheritance]
    public string ToolName { get; set; } = "An organ";

    [DataField, AlwaysPushInheritance]
    public float Speed { get; set; } = 1f;

    /// <summary>
    ///     Shitmed Change: If true, the organ will not heal an entity when transplanted into them.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool? Used { get; set; }


    /// <summary>
    ///     When attached, the organ will ensure these components on the entity, and delete them on removal.
    /// </summary>
    [DataField]
    public ComponentRegistry? OnAdd;

    /// <summary>
    ///     When removed, the organ will ensure these components on the entity, and delete them on insertion.
    /// </summary>
    [DataField]
    public ComponentRegistry? OnRemove;

    /// <summary>
    ///     Is this organ working or not?
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool Enabled = true;

    /// <summary>
    ///     Can this organ be enabled or disabled? Used mostly for prop, damaged or useless organs.
    /// </summary>
    [DataField]
    public bool CanEnable = true;

    /// <summary>
    ///     What is this organ's status?
    /// </summary>
    [DataField, AutoNetworkedField]
    public OrganStatus Status = OrganStatus.Healthy;

    /// <summary>
    ///     How much self healing this organ has.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float SelfHealingAmount = 5;

    /// <summary>
    ///     Delay between updates for the organ, used for updating the organ's status, and adding damage if its failing.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan UpdateDelay = TimeSpan.FromSeconds(2.0);

    /// <summary>
    ///     Next update time for the organ.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan NextUpdate;

    /// <summary>
    ///     On what OrganStatus should we start applying negative effects?
    /// </summary>
    [DataField, AutoNetworkedField]
    public OrganStatus DamagedStatus = OrganStatus.ModeratelyDamaged;

    [DataField, AutoNetworkedField]
    public Dictionary<OrganStatus, float> IntegrityThresholds = new()
    {
        { OrganStatus.Ruined, 90 },
        { OrganStatus.HeavilyDamaged, 75 },
        { OrganStatus.ModeratelyDamaged, 40},
        { OrganStatus.LightlyDamaged, 20 },
        { OrganStatus.Healthy, 10 },
    };

    // Shitmed Change End
}
