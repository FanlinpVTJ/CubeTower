namespace CubeGame.Hole
{
    public interface IHoleDisposalRule
    {
        HoleDisposalFailureReasonType Validate(HoleDisposalContext context);
    }
}
