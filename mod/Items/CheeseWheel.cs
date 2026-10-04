using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content.Items;

public class CheeseWheel : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32; Item.height = 28; Item.damage = 30; Item.DamageType = DamageClass.Ranged; Item.knockBack = 7f;
        Item.useTime = 26; Item.useAnimation = 26; Item.useStyle = ItemUseStyleID.Swing; Item.autoReuse = true; Item.noUseGraphic = true;
        Item.noMelee = true; Item.UseSound = SoundID.Item1; Item.rare = ItemRarityID.Yellow;
        Item.shoot = ModContent.ProjectileType<CheeseRoll>(); Item.shootSpeed = 9f; Item.maxStack = 99;
    }
}
