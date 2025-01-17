﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBTBalance.Helpers;
using DBTBalance.Model;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using DBZGoatLib.Model;
using DBZGoatLib.Handlers;
using DBZGoatLib;
using Microsoft.Xna.Framework.Graphics;

namespace DBTBalance.Buffs
{
    public class LSSJ4Buff : Transformation
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Legendary Super Saiyan 4");
            if (BalanceConfigServer.Instance.SSJTweaks)
            {
                damageMulti = 1f;
                speedMulti = 1.5f;
                kiDrainRate = 4.5f;
                kiDrainRateWithMastery = 2.5f;
                attackDrainMulti = 1.70f;
                baseDefenceBonus = 22;
            }
            else
            {
                damageMulti = 3.8f;
                speedMulti = 3.50f;
                kiDrainRate = 4.5f;
                kiDrainRateWithMastery = 2.5f;
                attackDrainMulti = 1.70f;
                baseDefenceBonus = 41;
            }
            

            base.SetStaticDefaults();
        }

        public static TransformationInfo LSSJ4Info => 
            new TransformationInfo(ModContent.BuffType<LSSJ4Buff>(),
                "LSSJ4Buff", false, "Legendary Super Saiyan 4", 
                Color.HotPink, CanTransform , OnTransform, PostTransform,
                new AnimationData(new AuraData("DBTBalance/Assets/LSSJ4Aura", 4, BlendState.AlphaBlend,new Color(250, 74, 67)), true, 
                    new SoundData("DBZMODPORT/Sounds/SSJAscension", "DBZMODPORT/Sounds/SSJ3", 260), "DBTBalance/Assets/LSSJ4Hair"));
        
        public static bool CanTransform(Player player)
        {
            var modPlayer = player.GetModPlayer<BPlayer>();

            if (!modPlayer.Offset.HasValue)
                return false;

            return !player.HasBuff<LSSJ4Buff>() &&
                player.HasBuff(DBTBalance.DBZMOD.Find<ModBuff>("LSSJ3Buff").Type) &&
                modPlayer.LSSJ4Achieved &&
                (DateTime.Now - modPlayer.Offset.Value).TotalSeconds >= 0.8f;
        }
        public static void OnTransform(Player player)
        {
            var modPlayer = player.GetModPlayer<BPlayer>();
            modPlayer.LSSJ4Active = true;
            modPlayer.Offset = null;

            var transMenu = DBTBalance.DBZMOD.Code.DefinedTypes.First(x => x.Name.Equals("TransMenu"));
            var menuSelection = transMenu.GetField("menuSelection");
            var none = Enum.Parse(menuSelection.FieldType, "None");
            menuSelection.SetValue(null, none);

        }
        public static void PostTransform(Player player)
        {
            var modPlayer = player.GetModPlayer<BPlayer>();
            modPlayer.LSSJ4Active = false;

            var transMenu = DBTBalance.DBZMOD.Code.DefinedTypes.First(x => x.Name.Equals("TransMenu"));
            var menuSelection = transMenu.GetField("menuSelection");
            dynamic none = Enum.Parse(menuSelection.FieldType, "LSSJ3");
            menuSelection.SetValue(null, none);

            modPlayer.Offset = null;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            var transMenu = DBTBalance.DBZMOD.Code.DefinedTypes.First(x => x.Name.Equals("TransMenu"));
            var menuSelection = transMenu.GetField("menuSelection");
            dynamic none = Enum.Parse(menuSelection.FieldType, "None");
            menuSelection.SetValue(null, none);
            base.Update(player, ref buffIndex);
        }
    }
}
