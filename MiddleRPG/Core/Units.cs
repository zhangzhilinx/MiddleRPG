using System;
using System.Collections.Generic;
using System.Reflection;

namespace MiddleRPG.Core
{
    public class UnitFactory
    {
        /// <summary>
        /// 记录所有英雄类型名，及其对应的运行时类型
        /// </summary>
        public static Dictionary<string, Type> kvHero = new Dictionary<string, Type>();
        /// <summary>
        /// 记录所有怪物类型名，及其对应的运行时类型
        /// </summary>
        public static Dictionary<string, Type> kvMonster = new Dictionary<string, Type>();

        /// <summary>
        /// UnitFactory的静态初始化
        /// 利用反射机制获取当前域内可以找到的所有英雄与怪物类
        /// </summary>
        static UnitFactory()
        {
            // 获取当前域下的所有程序集，以实现所需要的反射功能
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly asm in assemblies)
            {   // 遍历每个程序集获取所有类型
                Type[] types = asm.GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (types[j].IsSubclassOf(typeof(Hero)))
                    {
                        kvHero.Add(types[j].Name, types[j]);
                    }
                    else if (types[j].IsSubclassOf(typeof(Monster)))
                    {
                        kvMonster.Add(types[j].Name, types[j]);
                    }
                }
            }
        }

        /// <summary>
        /// 根据所需英雄类型获取对应的英雄对象
        /// </summary>
        /// <param name="typeHero">英雄类型</param>
        /// <param name="id">英雄对象唯一识别码</param>
        /// <returns>成功时返回对应的英雄对象，失败时返回null</returns>
        public static Hero CreateHero(string typeHero, string id = null)
        {
            if (kvHero.TryGetValue(typeHero, out Type classHero))
            {
                return Activator.CreateInstance(
                        classHero,
                        new object[] { id ?? Guid.NewGuid().ToString() }
                    ) as Hero;
            }
            return null;
        }

        /// <summary>
        /// 根据所需怪物类型获取对应的怪物对象
        /// </summary>
        /// <param name="typeMonster">怪物类型</param>
        /// <param name="id">怪物对象唯一识别码</param>
        /// <returns>成功时返回对应的怪物对象，失败时返回null</returns>
        public static Monster CreateMonster(string typeMonster, string id = null)
        {
            if (kvMonster.TryGetValue(typeMonster, out Type classMonster))
            {
                return Activator.CreateInstance(
                        classMonster,
                        new object[] { id ?? Guid.NewGuid().ToString() }
                    ) as Monster;
            }
            return null;
        }

        #region 早期简单的工厂模式实现
        public static DarkTemplar CreateDarkTemplar(string id = null)
        {
            return new DarkTemplar(id ?? Guid.NewGuid().ToString());
        }

        public static HighTemplar CreateHighTemplar(string id = null)
        {
            return new HighTemplar(id ?? Guid.NewGuid().ToString());
        }

        public static Archon CreateArchon(string id = null)
        {
            return new Archon(id ?? Guid.NewGuid().ToString());
        }

        public static Hydralisk CreateHydralisk(string id = null)
        {
            return new Hydralisk(id ?? Guid.NewGuid().ToString());
        }

        public static Queen CreateQueen(string id = null)
        {
            return new Queen(id ?? Guid.NewGuid().ToString());
        }

        public static Ultralisk CreateUltralisk(string id = null)
        {
            return new Ultralisk(id ?? Guid.NewGuid().ToString());
        }
        #endregion
    }

    [Serializable]
    public class DarkTemplar : Hero, ISuperAttack
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
    public class HighTemplar : Hero, ISuperAttack
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
    public class Archon : Hero, ISuperAttack
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
    public class Hydralisk : Monster
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
    public class Queen : Monster
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
    public class Ultralisk : Monster
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
