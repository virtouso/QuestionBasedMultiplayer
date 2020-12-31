using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<NetworkManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerNetworkStateHolder>().AsSingle();
        Container.Bind<PlayerDataModel>().AsSingle();
        Container.Bind<RunningMatchData>().AsSingle();

    }
}