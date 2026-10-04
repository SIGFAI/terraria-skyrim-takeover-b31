using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Whiterun guard: hunts monsters, and cannot stop talking about his knee.</summary>
public class Guard : ModNPC
{
    static readonly string[] Quips =
    {
        "I used to be an adventurer like you...", "Then I took an arrow to the knee.",
        "Let me guess: someone stole your sweetroll.", "Citizen, I'm watching you.",
        "Fus Ro WHAT?", "No lollygagging."
    };
    int hitCool, quipCool = 200;
    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 4;
    public override void SetDefaults()
    {
        NPC.scale = 1.6f; NPC.width = 40; NPC.height = 64; NPC.friendly = true;
        NPC.lifeMax = 400; NPC.damage = 0; NPC.defense = 20; NPC.knockBackResist = 0.2f;
        NPC.aiStyle = -1; NPC.HitSound = SoundID.NPCHit4; NPC.DeathSound = SoundID.NPCDeath1;
    }
    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter += Math.Abs(NPC.velocity.X) * 0.8 + 0.2;
        NPC.frame.Y = (int)(NPC.frameCounter / 6 % 4) * frameHeight;
    }
    public override void AI()
    {
        if (--quipCool <= 0)
        {
            quipCool = 420 + Main.rand.Next(300);
            Mix.Popup(NPC.Top, Quips[Main.rand.Next(Quips.Length)], Color.LightSteelBlue);
        }
        hitCool--;
        var foe = Mix.Nearest(NPC.Center, 40, n => n.active && !n.friendly && n.lifeMax > 5 && n.type != ModContent.NPCType<SkyDragon>());
        var host = Mix.Host;
        Vector2? goal = foe?.Center ?? (host != null && Vector2.Distance(host.Center, NPC.Center) > 160 ? host.Center : null);
        float dir = 0;
        if (goal.HasValue && Math.Abs(goal.Value.X - NPC.Center.X) > 22) dir = Math.Sign(goal.Value.X - NPC.Center.X);
        NPC.velocity.X = MathHelper.Lerp(NPC.velocity.X, dir * 2.6f, 0.15f);
        if (dir != 0) NPC.direction = NPC.spriteDirection = (int)dir;
        if (NPC.collideX && NPC.velocity.Y == 0) NPC.velocity.Y = -7.5f;
        if (foe != null && hitCool <= 0 && Math.Abs(foe.Center.X - NPC.Center.X) < 46 && Math.Abs(foe.Center.Y - NPC.Center.Y) < 50)
        {
            hitCool = 40;
            int d = NPC.Center.X < foe.Center.X ? 1 : -1;
            Main.player[Main.myPlayer].ApplyDamageToNPC(foe, 22, 5f, d, false);
            Mix.Burst(foe.Center, DustID.Torch, 8, 12, 4);
            Mix.Sound(SoundID.Item1, NPC.Center);
        }
    }
    public override void HitEffect(NPC.HitInfo hit) => Mix.Burst(NPC.Center, DustID.Iron, 6);
}
