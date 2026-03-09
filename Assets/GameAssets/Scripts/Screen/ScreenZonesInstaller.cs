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
            GameObject leftZoneInstance = Spawn(leftZonePrefab, nameof(leftZonePrefab));
            GameObject rightZoneInstance = Spawn(rightZonePrefab, nameof(rightZonePrefab));
            GameObject scrollZoneInstance = Spawn(scrollZonePrefab, nameof(scrollZonePrefab));

            ILeftZone leftZone = Resolve<ILeftZone>(leftZoneInstance, nameof(leftZonePrefab));
            IHoleView holeView = Resolve<IHoleView>(leftZoneInstance, nameof(leftZonePrefab));
            IRightZone rightZone = Resolve<IRightZone>(rightZoneInstance, nameof(rightZonePrefab));
            IScrollZone scrollZone = Resolve<IScrollZone>(scrollZoneInstance, nameof(scrollZonePrefab));
            IScrollView scrollView = Resolve<IScrollView>(scrollZoneInstance, nameof(scrollZonePrefab));

            Container.Bind<ILeftZone>().FromInstance(leftZone).AsSingle();
            Container.Bind<IHoleView>().FromInstance(holeView).AsSingle();
            Container.Bind<IRightZone>().FromInstance(rightZone).AsSingle();
            Container.Bind<IScrollZone>().FromInstance(scrollZone).AsSingle();
            Container.Bind<IScrollView>().FromInstance(scrollView).AsSingle();
        }

        private GameObject Spawn(MonoBehaviour prefab, string fieldName)
        {
            if (prefab == null)
            {
                throw new ZenjectException($"[GameZoneInstaller] {fieldName} is not assigned.");
            }

            GameObject instance = Container.InstantiatePrefab(prefab);

            return instance;
        }

        private TZone Resolve<TZone>(GameObject instance, string fieldName) where TZone : class
        {
            TZone zone = instance.GetComponents<MonoBehaviour>().OfType<TZone>().FirstOrDefault();

            if (zone == null)
            {
                throw new ZenjectException(
                    $"[GameZoneInstaller] Prefab bound to '{fieldName}' does not contain component implementing {typeof(TZone).Name}.");
            }

            return zone;
        }
    }
}
