using System.Linq;
using UnityEngine;
using Zenject;

namespace CubeGame.Screen
{
    public sealed class ScreenZonesInstaller : MonoInstaller<ScreenZonesInstaller>
    {
        [Header("Zone Prefabs")]
        [SerializeField] private ScreenZoneBase leftZonePrefab;
        [SerializeField] private ScreenZoneBase rightZonePrefab;
        [SerializeField] private ScreenZoneBase scrollZonePrefab;

        public override void InstallBindings()
        {
            var leftZone = SpawnAndResolve<ILeftZone>(leftZonePrefab, nameof(leftZonePrefab));
            var rightZone = SpawnAndResolve<IRightZone>(rightZonePrefab, nameof(rightZonePrefab));
            var scrollZone = SpawnAndResolve<IScrollZone>(scrollZonePrefab, nameof(scrollZonePrefab));

            Container.Bind<ILeftZone>().FromInstance(leftZone).AsSingle();
            Container.Bind<IRightZone>().FromInstance(rightZone).AsSingle();
            Container.Bind<IScrollZone>().FromInstance(scrollZone).AsSingle();

            if (scrollZone is IScrollView scrollView)
            {
                Container.Bind<IScrollView>().FromInstance(scrollView).AsSingle();
            }
            else
            {
                throw new ZenjectException("[ScreenZonesInstaller] Scroll zone prefab must implement IScrollView.");
            }
        }

        private TZone SpawnAndResolve<TZone>(MonoBehaviour prefab, string fieldName) where TZone : class
        {
            if (prefab == null)
            {
                throw new ZenjectException($"[GameZoneInstaller] {fieldName} is not assigned.");
            }

            var instance = Container.InstantiatePrefab(prefab);
            var zone = instance.GetComponents<MonoBehaviour>().OfType<TZone>().FirstOrDefault();

            if (zone == null)
            {
                throw new ZenjectException(
                    $"[GameZoneInstaller] Prefab '{prefab.name}' does not contain component implementing {typeof(TZone).Name}.");
            }

            return zone;
        }
    }
}
