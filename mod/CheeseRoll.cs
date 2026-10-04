using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>A thrown wheel of cheese that rolls, bounces and bowls enemies over.</summary>
public class CheeseRoll : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 26; Projectile.height = 24; Projectile.friendly = true; Projectile.DamageType = DamageClass.Ranged;
        Projectile.penetrate = 6; Projectile.timeLeft = 240; Projectile.usesLocalNPCImmunity = true; Projectile.localNPCHitCooldown = 20;
    }
    public override void AI()
    {
        Projectile.rotation += Projectile.velocity.X * 0.08f;
        Projectile.velocity.Y += 0.35f;
        if (Projectile.velocity.Y > 12) Projectile.velocity.Y = 12;
    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X * 0.8f;
        if (Projectile.velocity.Y != oldVelocity.Y && oldVelocity.Y > 1.5f) Projectile.velocity.Y = -oldVelocity.Y * 0.55f;
        return false;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Mix.Burst(target.Center, DustID.GoldCoin, 14, 14, 4);
        Mix.Popup(target.Top, "CHEESE!", Color.Yellow);
        Projectile.velocity.X *= 0.8f;
    }
}
