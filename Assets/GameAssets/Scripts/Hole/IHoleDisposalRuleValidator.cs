namespace CubeGame.Hole
{
    public interface IHoleDisposalRuleValidator
    {
        HoleDisposalFailureReasonType Validate(HoleDisposalContext context);
    }
}
