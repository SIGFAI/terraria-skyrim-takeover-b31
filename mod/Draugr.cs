using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class Draugr : ModNPC
{
    public override void SetStaticDefaults() => Main.npcFrameCount[Type] = 4;
    public override void SetDefaults()
    {
        NPC.scale = 1.6f; NPC.width = 38; NPC.height = 64;
        NPC.lifeMax = 70; NPC.damage = 16; NPC.defense = 3; NPC.knockBackResist = 0.6f; NPC.value = 80;
        NPC.aiStyle = NPCAIStyleID.Fighter; AIType = NPCID.Zombie;
        NPC.HitSound = SoundID.NPCHit2; NPC.DeathSound = SoundID.NPCDeath2;
    }
    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter += Math.Abs(NPC.velocity.X) * 0.8 + 0.3;
        NPC.frame.Y = (int)(NPC.frameCounter / 6 % 4) * frameHeight;
    }
    public override void AI() => Mix.Light(NPC.Top + new Vector2(0, 8), new Color(80, 140, 255) * 0.5f);
    public override void HitEffect(NPC.HitInfo hit)
    {
        Mix.Burst(NPC.Center, DustID.Frost, NPC.life <= 0 ? 25 : 8);
        Mix.Burst(NPC.Center, DustID.Bone, NPC.life <= 0 ? 12 : 3);
    }
    public override void OnKill()
    {
        if (Main.rand.NextBool(3)) Mix.Drop<Items.CheeseWheel>(NPC.Center, 1);
        Mix.Drop(ItemID.SilverCoin, NPC.Center, 3);
    }
}
