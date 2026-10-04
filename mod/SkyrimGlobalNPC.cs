using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Skyrim flavor on vanilla and modded monsters: skill-ups, the knee, and no more slimes.</summary>
public class SkyrimGlobalNPC : GlobalNPC
{
    static int skill = 15;
    public override void OnKill(NPC npc)
    {
        if (npc.friendly || npc.lifeMax <= 5) return;
        skill += 1;
        Mix.Popup(npc.Top + new Vector2(0, -24), "One-Handed increased to " + skill, Color.Gold);
        if (skill % 5 == 0 && Mix.Host != null) Mix.Sound("levelup", Mix.Host.Center, 0.6f);
    }
    public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
    {
        if (projectile.arrow && Main.rand.NextBool(3)) Mix.Popup(npc.Top, "Arrow to the knee!", Color.LightGray);
    }
}
