using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content.Items;

/// <summary>Unrelenting Force: shout and blast everything away.</summary>
public class FusRoDah : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 30; Item.height = 32; Item.damage = 45; Item.DamageType = DamageClass.Magic; Item.knockBack = 14f;
        Item.useTime = 30; Item.useAnimation = 30; Item.useStyle = ItemUseStyleID.HoldUp; Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<ShoutWave>(); Item.shootSpeed = 15f; Item.rare = ItemRarityID.Cyan;
        Item.noMelee = true;
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Mix.Sound("thuum", player.Center);
        Mix.Shake(7, 0.3);
        Mix.Popup(player.Top + new Vector2(0, -10), Main.rand.NextBool(2) ? "FUS RO DAH!" : "FUS!", Color.Cyan);
        Mix.Burst(player.Center + velocity * 2f, DustID.IceTorch, 25, 20, 6);
        return true;
    }
}
