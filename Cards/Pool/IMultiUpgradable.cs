namespace GrayTempest.Cards.Pool;

public interface IMultiUpgradable
{
    int UpgradeCount { get; }
    int MaxUpgrades { get; }
    void FakeUpgrade();
}