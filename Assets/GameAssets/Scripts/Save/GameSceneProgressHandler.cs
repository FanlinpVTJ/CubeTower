using System;
using CubeGame.Input;
using CubeGame.Tower;
using MessagePipe;
using Zenject;

namespace CubeGame.Save
{
    public sealed class GameSceneProgressHandler : IGameSceneProgressHandler, IInitializable, IDisposable
    {
        private readonly IGameSaver gameSaver;
        private readonly ITowerService towerService;
        private readonly ISubscriber<DragSessionPlacedMessage> dragSessionPlacedSubscriber;
        private readonly ISubscriber<DragSessionDisposedMessage> dragSessionDisposedSubscriber;

        private IDisposable dragSessionPlacedSubscription;
        private IDisposable dragSessionDisposedSubscription;

        public GameSceneProgressHandler(
            IGameSaver gameSaver,
            ITowerService towerService,
            ISubscriber<DragSessionPlacedMessage> dragSessionPlacedSubscriber,
            ISubscriber<DragSessionDisposedMessage> dragSessionDisposedSubscriber)
        {
            this.gameSaver = gameSaver;
            this.towerService = towerService;
            this.dragSessionPlacedSubscriber = dragSessionPlacedSubscriber;
            this.dragSessionDisposedSubscriber = dragSessionDisposedSubscriber;
        }

        public void Initialize()
        {
            RestoreProgress();

            if (dragSessionPlacedSubscriber != null)
            {
                dragSessionPlacedSubscription = dragSessionPlacedSubscriber.Subscribe(OnDragSessionPlaced);
            }

            if (dragSessionDisposedSubscriber != null)
            {
                dragSessionDisposedSubscription = dragSessionDisposedSubscriber.Subscribe(OnDragSessionDisposed);
            }
        }

        public void Dispose()
        {
            dragSessionPlacedSubscription?.Dispose();
            dragSessionPlacedSubscription = null;
            dragSessionDisposedSubscription?.Dispose();
            dragSessionDisposedSubscription = null;
        }

        private void OnDragSessionPlaced(DragSessionPlacedMessage message)
        {
            SaveProgress();
        }

        private void OnDragSessionDisposed(DragSessionDisposedMessage message)
        {
            SaveProgress();
        }

        public void SaveProgress()
        {
            if (gameSaver == null || towerService == null)
            {
                return;
            }

            TowerSnapshot towerSnapshot = towerService.GetSnapshot();
            GameSaveData saveData = new GameSaveData(towerSnapshot);
            gameSaver.SaveProgress(saveData);
        }

        private void RestoreProgress()
        {
            if (gameSaver == null || towerService == null)
            {
                return;
            }

            bool isLoaded = gameSaver.TryLoadProgress(out GameSaveData saveData);

            if (!isLoaded || saveData == null)
            {
                return;
            }

            towerService.Restore(saveData.TowerSnapshot);
        }
    }
}
