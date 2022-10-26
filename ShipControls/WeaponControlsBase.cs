public abstract class WeaponControlsBase : IWeaponControls
{
    // implementing interface
    public abstract bool PrimaryFired { get; }
    public abstract bool SecondaryFired { get; }
}