namespace SignTrainer.XR
{
    public interface IDeviceValidityProvider
    {
        bool LeftValid { get; }
        bool RightValid { get; }
        bool AnyValid => LeftValid || RightValid;
        bool BothValid => LeftValid && RightValid;
    }
}
