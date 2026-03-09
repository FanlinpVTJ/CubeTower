namespace CubeGame.Save
{
    public sealed class MenuSceneProgressHandler : IMenuSceneProgressHandler
    {
        private readonly IGameSaver gameSaver;

        public MenuSceneProgressHandler(IGameSaver gameSaver)
        {
            this.gameSaver = gameSaver;
        }

        public bool HasSave
        {
            get
            {
                if (gameSaver == null)
                {
                    return false;
                }

                return gameSaver.HasSave;
            }
        }

        public void StartNewGame()
        {
            if (gameSaver == null)
            {
                return;
            }

            gameSaver.ClearProgress();
        }
    }
}
