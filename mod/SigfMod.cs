using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class SigfMod : ModSystem
{
    static bool snowed;

    public override void OnWorldLoad()
    {
        snowed = false;
        Mix.WanderTiles = 18;
        Mix.After(0.3, MakeSnow);
        Mix.Every(1, SwapSlimes);
        Mix.Every(5, () => { if (Mix.Hostiles(Mix.Host.Center, 40).Count < 6) Mix.Spawn<Draugr>(Mix.Host.Center + new Vector2(Main.rand.NextBool() ? 520 : -520, 0)); });
        Mix.Every(55, () => { if (NPC.CountNPCS(ModContent.NPCType<SkyDragon>()) == 0) SpawnDragon(); });
        Mix.Every(30, () => { if (NPC.CountNPCS(ModContent.NPCType<Guard>()) < 2) Mix.Spawn<Guard>(Mix.Host.Center + new Vector2(Main.rand.NextBool() ? 220 : -220, 0)); });

        Mix.Demo(0.5, () => { Mix.Title("SKYRIM TAKEOVER", "Hey, you. You're finally awake.", 4); Mix.Arm<Items.FusRoDah>(); });
        Mix.Demo(2, () => { Mix.Spawn<Draugr>(Mix.Ahead(9)); Mix.Spawn<Draugr>(Mix.Ahead(13)); Mix.Spawn<Guard>(Mix.Ahead(-6)); });
        Mix.Demo(8, () => Mix.Spawn<Draugr>(Mix.Ahead(7)));
        Mix.Demo(12, () => Mix.Arm<Items.CheeseWheel>());
        Mix.Demo(18, () => Mix.Arm<Items.Dragonbone>());
        Mix.Demo(22, () => SpawnDragon());
        Mix.Demo(24, () => Mix.Arm<Items.FusRoDah>());
    }

    static void SpawnDragon()
    {
        var h = Mix.Host;
        NPC.NewNPC(Mix.Source, (int)h.Center.X + 700, (int)h.Center.Y - 260, ModContent.NPCType<SkyDragon>());
    }

    static void SwapSlimes()
    {
        foreach (var n in Main.npc)
            if (n.active && !n.friendly && (n.type == NPCID.BlueSlime || n.type == NPCID.GreenSlime || n.type == NPCID.Zombie || n.type == NPCID.PurpleSlime))
            {
                var pos = n.Bottom; n.active = false;
                Mix.Spawn<Draugr>(pos);
            }
    }

    static void MakeSnow()
    {
        if (snowed) return; snowed = true;
        int cx = (int)(Mix.Center.X / 16), cy = (int)(Mix.Center.Y / 16);
        for (int x = cx - 90; x <= cx + 90; x++)
            for (int y = cy - 40; y <= cy + 40; y++)
            {
                if (x < 5 || y < 5 || x >= Main.maxTilesX - 5 || y >= Main.maxTilesY - 5) continue;
                var t = Main.tile[x, y];
                if (!t.HasTile) continue;
                if (t.TileType == TileID.Grass) { t.TileType = TileID.SnowBlock; }
                else if ((t.TileType == TileID.Plants || t.TileType == TileID.Plants2 || t.TileType == TileID.Sunflower) && Main.tile[x, y + 1].HasTile)
                    WorldGen.KillTile(x, y, false, false, true);
            }
        for (int x = cx - 90; x <= cx + 90; x += 8) for (int y = cy - 40; y <= cy + 40; y += 8) WorldGen.SquareTileFrame(x, y, true);
    }

    public override void PostUpdateEverything()
    {
        if (Main.dedServ || Main.gameMenu) return;
        for (int i = 0; i < 3; i++)
        {
            var pos = Main.screenPosition + new Vector2(Main.rand.Next(-200, Main.screenWidth + 200), -20);
            var d = Dust.NewDustPerfect(pos, DustID.SnowflakeIce, new Vector2(-1.2f + Main.rand.NextFloat(-0.4f, 0.4f), 2f + Main.rand.NextFloat(1.5f)), 0, Color.White, 1.0f);
            d.noGravity = true; d.noLight = true;
        }
    }
}
