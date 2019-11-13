using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiddleRPG.Core
{
    [Serializable]
    class DarkTemplar : Hero
    {
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
            Effect.Dead = ResourceMain.effect_dead_general;
        }
    }

    [Serializable]
    class HighTemplar : Hero
    {
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
            Effect.Dead = ResourceMain.effect_dead_general;
        }
    }

    [Serializable]
    class Archon : Hero
    {
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
            Effect.Dead = ResourceMain.effect_dead_general;
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
            Effect.Dead = ResourceMain.effect_dead_general;
        }
    }
}
