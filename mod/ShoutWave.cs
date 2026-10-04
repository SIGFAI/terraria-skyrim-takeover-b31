using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class ShoutWave : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 44; Projectile.height = 44; Projectile.friendly = true; Projectile.DamageType = DamageClass.Magic;
        Projectile.penetrate = -1; Projectile.timeLeft = 38; Projectile.tileCollide = false; Projectile.ignoreWater = true;
        Projectile.usesLocalNPCImmunity = true; Projectile.localNPCHitCooldown = 30; Projectile.alpha = 40;
    }
    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        Projectile.scale = 0.8f + (38 - Projectile.timeLeft) * 0.04f;
        Projectile.alpha = System.Math.Min(255, Projectile.alpha + 3);
        Mix.Light(Projectile.Center, new Color(120, 190, 255));
        var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0, 0, 100, default, 1.5f);
        d.noGravity = true; d.velocity = Projectile.velocity * 0.2f;
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Mix.Burst(target.Center, DustID.IceTorch, 18, 14, 5);
        Mix.Sound(SoundID.Item70, target.Center);
        Mix.Shake(4, 0.15);
        target.velocity.X += Projectile.velocity.X * 0.5f * target.knockBackResist;
        target.velocity.Y -= 3f * target.knockBackResist;
    }
    public override bool PreDraw(ref Color lightColor) { lightColor = Color.White * Projectile.Opacity; return true; }
}
