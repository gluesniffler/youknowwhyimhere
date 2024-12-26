using Robust.Shared.Serialization;
using Content.Shared.FixedPoint;
namespace Content.Shared._Shitmed.Body.Vascular;

[RegisterComponent]
public sealed partial class VascularComponent : Component
{
    /// <summary>
    ///     The current strain of the vascular system.
    /// </summary>
    [DataField]
    public float CurrentStrain = 0.0f;

    /// <summary>
    ///     The current heart rate of the vascular system.
    /// </summary>
    [DataField]
    public float HeartRate;

    /// <summary>
    ///     The speed at which the heart rate tries to rectify itself towards normality.
    /// </summary>
    [DataField]
    public float HeartRateRecoveryRate = 2.0f;

    /// <summary>
    ///     The maximum heart rate of the vascular system for this entity. Anything above this will be considered a heart failure.
    /// </summary>
    [DataField]
    public float HeartRateHighThreshold = 100.0f;

    /// <summary>
    ///     The minimum heart rate of the vascular system for this entity. Anything below this will be considered a heart failure.
    /// </summary>
    [DataField]
    public float HeartRateLowThreshold = 60.0f;

    /// <summary>
    ///     The current blood pressure of the vascular system.
    /// </summary>
    [DataField]
    public BloodPressure CurrentBloodPressure;

    /// <summary>
    ///     The speed at which the blood pressure tries to rectify itself towards normality.
    /// </summary>
    [DataField]
    public BloodPressure BloodPressureRecoveryRate = (1.0f, 1.0f);

    /// <summary>
    ///     The maximum blood pressure of the vascular system for this entity. Anything above this will be considered high pressure.
    /// </summary>
    [DataField]
    public BloodPressure BloodPressureHighThreshold = (100.0f, 100.0f);

    /// <summary>
    ///     The minimum blood pressure of the vascular system for this entity. Anything below this will be considered low pressure.
    /// </summary>
    [DataField]
    public BloodPressure BloodPressureLowThreshold = (100.0f, 100.0f);
}

[DataRecord, Serializable, NetSerializable]
public record struct BloodPressure(FixedPoint2 High, FixedPoint2 Low)
{
    public static implicit operator (FixedPoint2, FixedPoint2)(BloodPressure p) => (p.High, p.Low);
    public static implicit operator BloodPressure((FixedPoint2, FixedPoint2) p) => new(p.Item1, p.Item2);
}

/*

So for this system we need the following things:

1. Interface the blood pressure so that it directly correlates to heart rate. Heart rate can be increased by too low or too high pressure, as well as damage or stress.
2. If the heart rate is too fast, we will put more strain on the heart, if it goes over its maximum threshold, then we will start damaging the heart, which will decrease its capacity.
3. If the heart goes under capacity, then we will inflict a heart attack on the entity.
4. If the heart rate goes way too fast over the maximum threshold of the heart, then we will inflict a heart attack on the entity.
5. If the heart rate goes way too low under the minimum threshold, then we will make the entity lose consciousness and stop its heart eventually.
6. If the entity starts bleeding, with a low pressure this wont be too harmful, but with a higher one, they'll be gushing blood for the smallest of cuts.
7. The metabolism speed for chemicals will depend on the entity's blood pressure. Which will help entities that are damaged to heal faster, taking advantage of adrenaline.
8. We'll define thresholds for this heart failure within each heart directly, so that a felinid's heart could potentially be weaker than an oni's.
9. We also need a strain system in general to be able to track what things the entity is doing that puts strain on the heart.
  9.1. Things such as eating, running, being cold, taking hits, etc will pump your heart rate and blood pressure. We need to track these somehow though.
  9.2. Medications and other things with overlap can also alter the blood pressure, so we could try to make a pattern like a prefix such "vascular_strain_" followed by
  the name of the thing that is causing the strain. Though this will require us to keep careful track of what things are added so we can remove em.
10. What do we do with an entity that has a blood pressure or heart rate out of whack? You essentially stop their heart, and defib them so you can restore it to normality.

Afterwards, with Breathing we will do a simple copy of this system, where we'll have a minimum strain for the lungs that the organ has to meet based on the entity type.
If the entity doesnt meet this strain, the lungs will start taking damage, and their heart rate will start going up.

If the heart rate/blood pressure increases, the lungs strain will also go up. I'd kinda like for spacing to also force all the air out of your lungs and stuff.

*/
