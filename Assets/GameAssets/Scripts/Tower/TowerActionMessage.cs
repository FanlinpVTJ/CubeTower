namespace CubeGame.Tower
{
    public readonly struct TowerActionMessage
    {
        public TowerActionMessage(TowerActionType actionType, string text)
        {
            ActionType = actionType;
            Text = text;
        }

        public TowerActionType ActionType { get; }
        public string Text { get; }
    }
}
