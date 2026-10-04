using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class DragonFire : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 22; Projectile.height = 22; Projectile.hostile = true; Projectile.friendly = false;
        Projectile.timeLeft = 90; Projectile.tileCollide = true; Projectile.penetrate = 1;
    }
    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation();
        Projectile.velocity.Y += 0.05f;
        Mix.Light(Projectile.Center, new Color(255, 150, 40));
        var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0, 0, 100, default, 1.8f);
        d.noGravity = true; d.velocity *= 0.3f;
        if (Main.rand.NextBool(3)) Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0, -1, 150, default, 1.2f).noGravity = true;
    }
    public override void OnKill(int timeLeft)
    {
        Mix.Burst(Projectile.Center, DustID.Torch, 20, 16, 4);
        Mix.Sound(SoundID.Item74, Projectile.Center);
    }
    public override bool PreDraw(ref Color lightColor) { lightColor = Color.White; return true; }
}
