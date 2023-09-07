using Zenject;
using UnityEngine;

public class PlantInstaller : MonoInstaller
{
    [SerializeField] private PlantSample[] _plantSamples;
    [SerializeField] private FieldPlanter _fieldPlanter;
    [SerializeField] private PlayerTester _playerTester;

    public override void InstallBindings()
    {
        var plantData = new PlantData(_plantSamples);
        var plantPoolManager = new PlantGameObjects(plantData);
        var plantFactory = new PlantFactory(plantData, plantPoolManager);

        Container.Bind<IPlantData>().FromInstance(plantData).AsSingle().NonLazy();
        Container.Bind<IPlantObjects>().FromInstance(plantPoolManager).AsSingle().NonLazy();
        Container.Bind<PlantFactory>().FromInstance(plantFactory).AsSingle().NonLazy();
        Container.Bind<FieldPlanter>().FromInstance(_fieldPlanter).AsSingle().NonLazy();
        Container.Bind<IPlayer>().FromInstance(_playerTester).AsSingle().NonLazy();
    }
}
