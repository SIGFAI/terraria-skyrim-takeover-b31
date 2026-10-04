using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content.Items;

public class Dragonbone : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32; Item.height = 32; Item.damage = 52; Item.DamageType = DamageClass.Melee; Item.knockBack = 6f;
        Item.useTime = 22; Item.useAnimation = 22; Item.useStyle = ItemUseStyleID.Swing; Item.autoReuse = true;
        Item.UseSound = SoundID.Item1; Item.rare = ItemRarityID.Red; Item.scale = 1.4f;
    }
    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.OnFire, 180);
        Mix.Burst(target.Center, DustID.Torch, 14, 14, 4);
    }
}
