public abstract class MovementControlsBase : IMovementControls
{
    // implementing interface
    public abstract float YawAmount { get; }
    public abstract float PitchAmount { get; }
    public abstract float RollAmount { get; }
    public abstract float ThrustAmount { get; }

}
