using System.Linq;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using GrayTempest.Cards.Keywords;
using GrayTempest.Character;
using GrayTempest.Orbs;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace GrayTempest.Cards.Rare;

[RegisterCard(typeof(GrayTempestCardPool))]
public class OrbitalDebrisScattering : ModCardTemplate
{
    public OrbitalDebrisScattering() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }
    public override IEnumerable<CardKeyword> CanonicalKeywords => [GrayTempestKeywords.Burst];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10m, ValueProp.Move)];
    public override CardAssetProfile AssetProfile => new(GrayTempestConst.Paths.CardPlaceholder, GrayTempestConst.Paths.CardPlaceholder);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var debris = PileType.Hand.GetPile(Owner).Cards
            .Concat(PileType.Draw.GetPile(Owner).Cards)
            .Concat(PileType.Discard.GetPile(Owner).Cards)
            .Where(c => c.Type == CardType.Status || c.Type == CardType.Curse)
            .ToList();

        int count = 0;
        foreach (var card in debris)
        {
            await CardCmd.Exhaust(choiceContext, card);
            count++;
        }

        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this, play)
            .TargetingAllOpponents(CombatState)
            .Execute(choiceContext);

        for (int i = 0; i < count; i++)
            await OrbCmd.Channel<OrbitalBombardmentUnit>(choiceContext, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
        EnergyCost.UpgradeBy(-1);
    }
}
