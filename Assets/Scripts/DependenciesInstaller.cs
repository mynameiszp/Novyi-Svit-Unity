using UnityEngine;
using Zenject;

public class DependenciesInstaller : MonoInstaller
{
    [SerializeField] private AvatarsManager managerPrefab;
    public override void InstallBindings()
    {
        BindAvatarsManager();
    }

    private void BindAvatarsManager()
    {
        var manager = Container.InstantiatePrefabForComponent<AvatarsManager>(managerPrefab);

        Container
            .Bind<AvatarsManager>()
            .FromInstance(manager)
            .AsCached()
            .Lazy();
    }
}