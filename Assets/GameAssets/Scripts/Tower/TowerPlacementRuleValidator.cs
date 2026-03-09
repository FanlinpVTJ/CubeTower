using System.Collections.Generic;

namespace CubeGame.Tower
{
    public sealed class TowerPlacementRuleValidator : ITowerPlacementRuleValidator
    {
        private readonly List<ITowerPlacementRule> rules;

        public TowerPlacementRuleValidator(List<ITowerPlacementRule> rules)
        {
            this.rules = rules;
        }

        public TowerPlacementFailureReasonType Validate(TowerPlacementContext context, TowerState towerState)
        {
            if (rules == null || rules.Count == 0)
            {
                return TowerPlacementFailureReasonType.None;
            }

            for (int i = 0; i < rules.Count; i++)
            {
                ITowerPlacementRule rule = rules[i];

                if (rule == null)
                {
                    continue;
                }

                TowerPlacementFailureReasonType failureReason = rule.Validate(context, towerState);

                if (failureReason != TowerPlacementFailureReasonType.None)
                {
                    return failureReason;
                }
            }

            return TowerPlacementFailureReasonType.None;
        }
    }
}
