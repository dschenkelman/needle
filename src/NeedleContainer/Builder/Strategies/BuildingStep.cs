namespace Needle.Builder.Strategies
{
    public enum BuildingStep
    {
        PreBuilding = 0,
        ConstructorDetermination = 1,
        ConstructorDependenciesResolution = 2,
        PropertyDependenciesResolution = 3
    }
}
