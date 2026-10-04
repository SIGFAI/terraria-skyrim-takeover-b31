using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>An ancient dragon that circles the player, dives and breathes fire.</summary>
public class SkyDragon : ModNPC
{
    int side = 1, breath;
    float t;
    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 4;
    public override void SetDefaults()
    {
        NPC.scale = 1.6f; NPC.width = 190; NPC.height = 110; DrawOffsetY = 60;
        NPC.lifeMax = 900; NPC.damage = 30; NPC.defense = 6; NPC.knockBackResist = 0f; NPC.value = 20000;
        NPC.aiStyle = -1; NPC.noGravity = true; NPC.noTileCollide = true; NPC.boss = true;
        NPC.HitSound = SoundID.NPCHit7; NPC.DeathSound = SoundID.NPCDeath10;
    }
    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter += 1;
        NPC.frame.Y = (int)(NPC.frameCounter / 9 % 4) * frameHeight;
    }
    public override void OnSpawn(IEntitySource source)
    {
        Mix.Sound(SoundID.Roar, NPC.Center);
        Mix.Shake(10, 0.8);
        Mix.Title("A DRAGON!", "Dragons have returned to the land", 3);
    }
    public override void AI()
    {
        NPC.TargetClosest();
        var p = Main.player[NPC.target];
        t += 1;
        if (t % 420 == 0) side = -side;
        float wob = (float)Math.Sin(t / 70f);
        Vector2 hover = p.Center + new Vector2(side * (260 + 60 * wob), (t > 600 && Mix.IsDemo ? -125 : -170) + 35 * (float)Math.Sin(t / 130f));
        Vector2 want = (hover - NPC.Center);
        NPC.velocity = Vector2.Lerp(NPC.velocity, want.SafeNormalize(Vector2.Zero) * Math.Min(want.Length() * 0.06f, 7.5f), 0.06f);
        NPC.direction = NPC.spriteDirection = p.Center.X > NPC.Center.X ? 1 : -1;
        NPC.rotation = NPC.velocity.Y * 0.02f * NPC.direction * -1;

        // fire breath: 70 ticks of flames every ~5 s
        int cycle = (int)(t % 300);
        if (cycle == 200) { Mix.Sound(SoundID.Roar, NPC.Center); Mix.Shake(6, 0.5); breath = 70; }
        if (breath > 0)
        {
            breath--;
            Vector2 mouth = NPC.Center + new Vector2(NPC.direction * 110, 10);
            if (breath % 5 == 0)
            {
                Vector2 aim = Mix.Aim(mouth, p.Center + p.velocity * 8, 9f).RotatedByRandom(0.1);
                Mix.Shoot<DragonFire>(mouth, aim, NPC.damage, 3f, hostile: true);
            }
            Mix.Burst(mouth, DustID.Torch, 2, 6, 3);
        }
    }
    public override void HitEffect(NPC.HitInfo hit)
    {
        Mix.Burst(NPC.Center, DustID.Blood, NPC.life <= 0 ? 60 : 8, 50);
        if (NPC.life <= 0)
        {
            Mix.Burst(NPC.Center, DustID.Torch, 80, 60, 8);
            Mix.Burst(NPC.Center, DustID.Smoke, 40, 60, 5);
            Mix.Shake(14, 1.0);
        }
    }
    public override void OnKill()
    {
        Mix.Drop<Items.CheeseWheel>(NPC.Center, 5);
        Mix.Drop(ItemID.GoldCoin, NPC.Center, 25);
        if (Mix.Host != null)
        {
            Mix.Sound("levelup", Mix.Host.Center);
            Mix.Popup(Mix.Host.Top, "DRAGON SOUL ABSORBED", Color.Cyan);
            for (int i = 0; i < 70; i++)
            {
                Vector2 from = NPC.Center + Main.rand.NextVector2Circular(60, 40);
                var d = Dust.NewDustPerfect(from, DustID.IceTorch, (Mix.Host.Center - from) * 0.04f, 100, default, 2f);
                d.noGravity = true;
            }
        }
        Mix.Log("dragon slain"); Mix.Say("Dragon slain! Its soul flows into you.", Color.Cyan);
    }
}
