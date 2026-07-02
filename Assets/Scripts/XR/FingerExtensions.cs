namespace SignTrainer.XR
{
    /// <summary>
    /// Per-finger extension amounts for one hand: 0 = fully curled, 1 = straight.
    /// Order: thumb, index, middle, ring, pinky.
    /// </summary>
    public struct FingerExtensions
    {
        public bool isTracked;
        public float thumb, index, middle, ring, pinky;

        public float Get(int finger) => finger switch
        {
            0 => thumb,
            1 => index,
            2 => middle,
            3 => ring,
            _ => pinky,
        };
    }
}
