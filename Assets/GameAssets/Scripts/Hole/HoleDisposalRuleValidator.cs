using System.Collections.Generic;

namespace CubeGame.Hole
{
    public sealed class HoleDisposalRuleValidator : IHoleDisposalRuleValidator
    {
        private readonly List<IHoleDisposalRule> rules;

        public HoleDisposalRuleValidator(List<IHoleDisposalRule> rules)
        {
            this.rules = rules;
        }

        public HoleDisposalFailureReasonType Validate(HoleDisposalContext context)
        {
            if (rules == null || rules.Count == 0)
            {
                return HoleDisposalFailureReasonType.None;
            }

            for (int i = 0; i < rules.Count; i++)
            {
                IHoleDisposalRule rule = rules[i];

                if (rule == null)
                {
                    continue;
                }

                HoleDisposalFailureReasonType failureReason = rule.Validate(context);

                if (failureReason != HoleDisposalFailureReasonType.None)
                {
                    return failureReason;
                }
            }

            return HoleDisposalFailureReasonType.None;
        }
    }
}
