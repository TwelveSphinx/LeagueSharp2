using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
namespace SFTemplate
{
    class Akali
    {
        public static string ChampName = "Akali";
        public static Orbwalking.Orbwalker Orbwalker;
        public static Obj_AI_Base Player = ObjectManager.Player; // Instead of typing ObjectManager.Player you can just type Player
        public static Spell Q, W, E, R;
        public static Items.Item DFG;
        public static Items.Item Hex;
        public static Items.Item Zhonya;

        public static Menu HH_Akali;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.BaseSkinName != ChampName) return;


            Q = new Spell(SpellSlot.Q, 625);
            W = new Spell(SpellSlot.W, 700);
            E = new Spell(SpellSlot.E, 300);
            R = new Spell(SpellSlot.R, 800);
            Hex = new Items.Item(3146, 700);
            DFG = new Items.Item(3128, 750);
            Zhonya = new Items.Item(3157, 0);


            HH_Akali = new Menu("HH Akali" + ChampName, ChampName, true);

            HH_Akali.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(HH_Akali.SubMenu("Orbwalker"));

            var ts = new Menu("Target Selector", "Target Selector");
            SimpleTs.AddToMenu(ts);
            HH_Akali.AddSubMenu(ts);

            HH_Akali.AddSubMenu(new Menu("Combo", "Combo"));
            HH_Akali.SubMenu("Combo").AddItem(new MenuItem("useQ", "Use Q?").SetValue(true));
            HH_Akali.SubMenu("Combo").AddItem(new MenuItem("useW", "Use W?").SetValue(true));
            HH_Akali.SubMenu("Combo").AddItem(new MenuItem("useE", "Use E?").SetValue(true));
            HH_Akali.SubMenu("Combo").AddItem(new MenuItem("useR", "Use R?").SetValue(true));
            HH_Akali.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));
            //Exploits
            HH_Akali.AddItem(new MenuItem("Zhonya", "AutoZhonya").SetValue(true));
            HH_Akali.AddToMainMenu();

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnGameUpdate += Game_OnGameUpdate;

            Game.PrintChat("Swag" + ChampName + " loaded! ");
        }

        static void Game_OnGameUpdate(EventArgs args)
        {
            while (Player.Health < 200)
            {
                Items.UseItem(3157);
            }

            if (HH_Akali.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
        }
        private static void Drawing_OnDraw(EventArgs args)
        {
            var menuItem = HH_Akali.Item("RRange").GetValue<Circle>();
            if (menuItem.Active) Utility.DrawCircle(Player.Position, R.Range, menuItem.Color);

            var menuItem2 = HH_Akali.Item("QRange").GetValue<Circle>();
            if (menuItem2.Active) Utility.DrawCircle(Player.Position, Q.Range, menuItem2.Color);
        }
        public static void Combo()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Magical);
            if (target == null) return;

            if (target.IsValidTarget(DFG.Range) && DFG.IsReady())
                DFG.Cast(target);
            if (target.IsValidTarget(Hex.Range) && Hex.IsReady())
            {

                DFG.Cast(target);
            }
            if (target.IsValidTarget(Q.Range) && Q.IsReady())
            {
                Q.Cast(target, HH_Akali.Item("NFE").GetValue<bool>());

            }
            if (target.IsValidTarget(W.Range) && W.IsReady())
            {
                W.Cast(target, HH_Akali.Item("NFE").GetValue<bool>());
            }
            if (target.IsValidTarget(E.Range) && E.IsReady())
            {
                E.Cast(target, HH_Akali.Item("NFE").GetValue<bool>());
            }
            if (target.IsValidTarget(R.Range) && R.IsReady())
            {
                R.Cast(target, HH_Akali.Item("NFE").GetValue<bool>());
            }

        }
    }
}