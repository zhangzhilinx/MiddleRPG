using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiddleRPG.Core
{
    [Serializable]
    class DarkTemplar : Hero, ISuperAttack
    {
        private readonly int superAttackPower = 15;
        private readonly int superAttackProbabilityBonus = 5;
        private readonly double superAttackProbabilityCoe = 0.03;
        private readonly Random randomer = new Random();

        public DarkTemplar(string id) : base(
            id,
            "黑暗圣堂武士",
            ResourceMain.avatar_hero_darktemplar,
            120,
            40,
            40,
            125)
        {
            Effect.Attack = ResourceMain.effect_attack_darktemplar;
            Effect.SuperAttack = ResourceMain.effect_general_superattack;
            Effect.Dead = ResourceMain.effect_dead_general;
        }

        public override int Attack(BattleUnit enemy)
        {
            int damageNormal = base.Attack(enemy);
            int damageSuperAttack = 0;

            int b = superAttackProbabilityBonus;
            double c = superAttackProbabilityCoe;
            double x = (100 * Health / Life);
            double probabilityThreshold = 100 * Math.Pow(Math.E, c * (0.01 * b - 1) * x);

            if (randomer.Next(100) + 1 <= probabilityThreshold)
            {
                damageSuperAttack = SuperAttack(enemy);
            }

            return (damageNormal < 0 && damageSuperAttack < 0)
                ? -1
                : ((damageNormal < 0 ? 0 : damageNormal) +
                   (damageSuperAttack < 0 ? 0 : damageSuperAttack));
        }

        public int SuperAttack(BattleUnit enemy)
        {
            return enemy?.SufferedSuperAttack(this, superAttackPower) ?? -1;
        }
    }

    [Serializable]
    class HighTemplar : Hero, ISuperAttack
    {
        private readonly int superAttackPower = 10;
        private readonly int superAttackProbabilityBonus = 4;
        private readonly double superAttackProbabilityCoe = 0.03;
        private readonly Random randomer = new Random();

        public HighTemplar(string id) : base(
            id,
            "高阶圣堂武士",
            ResourceMain.avatar_hero_hightemplar,
            80,
            80,
            10,
            150)
        {
            Effect.Attack = ResourceMain.effect_attack_hightemplar;
            Effect.SuperAttack = ResourceMain.effect_general_superattack;
            Effect.Dead = ResourceMain.effect_dead_general;
        }

        public override int Attack(BattleUnit enemy)
        {
            int damageNormal = base.Attack(enemy);
            int damageSuperAttack = 0;

            int b = superAttackProbabilityBonus;
            double c = superAttackProbabilityCoe;
            double x = (100 * Health / Life);
            double probabilityThreshold = 100 * Math.Pow(Math.E, c * (0.01 * b - 1) * x);

            if (randomer.Next(100) + 1 <= probabilityThreshold)
            {
                damageSuperAttack = SuperAttack(enemy);
            }

            return (damageNormal < 0 && damageSuperAttack < 0)
                ? -1
                : ((damageNormal < 0 ? 0 : damageNormal) +
                   (damageSuperAttack < 0 ? 0 : damageSuperAttack));
        }

        public int SuperAttack(BattleUnit enemy)
        {
            return enemy?.SufferedSuperAttack(this, superAttackPower) ?? -1;
        }
    }

    [Serializable]
    class Archon : Hero, ISuperAttack
    {
        private readonly int superAttackPower = 20;
        private readonly int superAttackProbabilityBonus = 15;
        private readonly double superAttackProbabilityCoe = 0.03;
        private readonly Random randomer = new Random();

        public Archon(string id) : base(
            id,
            "执政官",
            ResourceMain.avatar_hero_archon,
            360,
            34,
            40,
            250)
        {
            Effect.Attack = ResourceMain.effect_attack_archon;
            Effect.SuperAttack = ResourceMain.effect_general_superattack;
            Effect.Dead = ResourceMain.effect_dead_general;
        }

        public override int Attack(BattleUnit enemy)
        {
            int damageNormal = base.Attack(enemy);
            int damageSuperAttack = 0;

            int b = superAttackProbabilityBonus;
            double c = superAttackProbabilityCoe;
            double x = (100 * Health / Life);
            double probabilityThreshold = 100 * Math.Pow(Math.E, c * (0.01 * b - 1) * x);

            if (randomer.Next(100) + 1 <= probabilityThreshold)
            {
                damageSuperAttack = SuperAttack(enemy);
            }

            return (damageNormal < 0 && damageSuperAttack < 0)
                ? -1
                : ((damageNormal < 0 ? 0 : damageNormal) +
                   (damageSuperAttack < 0 ? 0 : damageSuperAttack));
        }

        public int SuperAttack(BattleUnit enemy)
        {
            return enemy?.SufferedSuperAttack(this, superAttackPower) ?? -1;
        }
    }

    [Serializable]
    class Hydralisk : Monster
    {
        public Hydralisk(string id) : base(
            id,
            "刺蛇",
            ResourceMain.avatar_monster_hydralisk,
            80,
            25,
            30,
            50)
        {
            Effect.Attack = ResourceMain.effect_attack_hydralisk;
            Effect.SuperAttack = ResourceMain.effect_general_superattack;
            Effect.Dead = ResourceMain.effect_dead_general;
        }
    }

    [Serializable]
    class Queen : Monster
    {
        public Queen(string id) : base(
            id,
            "虫后",
            ResourceMain.avatar_monster_queen,
            175,
            32,
            10,
            150)
        {
            Effect.Attack = ResourceMain.effect_attack_queen;
            Effect.SuperAttack = ResourceMain.effect_general_superattack;
            Effect.Dead = ResourceMain.effect_dead_general;
        }
    }

    [Serializable]
    class Ultralisk : Monster
    {
        public Ultralisk(string id) : base(
            id,
            "雷兽",
            ResourceMain.avatar_monster_ultralisk,
            500,
            44,
            40,
            200)
        {
            Effect.Attack = ResourceMain.effect_attack_ultralisk;
            Effect.SuperAttack = ResourceMain.effect_general_superattack;
            Effect.Dead = ResourceMain.effect_dead_general;
        }
    }
}
