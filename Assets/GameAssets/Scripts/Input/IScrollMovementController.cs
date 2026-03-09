namespace CubeGame.Input
{
    public interface IScrollMovementController
    {
        float CurrentVelocityX { get; }
        void Stop();
        void Resume();
    }
}
