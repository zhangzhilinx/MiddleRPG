using System;
using System.Drawing;

namespace MiddleRPG.Core
{
    [Serializable]
    public class BattleUnit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Bitmap Avatar { get; set; }
        public int Life { get; private set; }
        public int Power { get; private set; }
        public int Agility { get; private set; }
        public int Intelligence { get; private set; }
        public int Health { get; private set; }
        public int Defense { get { return Agility / 10 + Intelligence / 50; } }
        public bool IsAlive { get { return Health > 0; } }

        [Serializable]
        public class BitmapEffect
        {
            public Bitmap Attack { get; set; }
            public Bitmap Dead { get; set; }
        };
        public BitmapEffect Effect { get; set; } = new BitmapEffect();

        public delegate void OnUnderAttackHandler(BattleUnit attacker, int power);
        public delegate void OnHealthChangedHandler(int life);
        [field: NonSerialized]
        public event OnUnderAttackHandler UnderAttack;
        [field: NonSerialized]
        public event OnHealthChangedHandler HealthChanged;

        public BattleUnit(string id, string name, Bitmap avatar, int life, int power, int agility, int intelligence)
        {
            Id = id;
            Name = name;
            Avatar = avatar;
            Health = Life = life;
            Power = power;
            Agility = agility;
            Intelligence = intelligence;
        }

        public int Attack(BattleUnit enemy)
        {
            //return (enemy != null) ? enemy.Injured(this, Power) : -1;
            return enemy?.Injured(this, Power) ?? -1;
        }

        public int Injured(BattleUnit attacker, int power)
        {
            int damage = power - Defense;
            if (damage < Health)
            {
                Health -= damage;
                UnderAttack(attacker, damage);
                HealthChanged(Health);
            }
            else
            {
                Health = 0;
                UnderAttack(attacker, damage);
                HealthChanged(Health);
            }
            return Health;
        }
    }

    [Serializable]
    class Hero : BattleUnit
    {
        public Hero(string id, string name, Bitmap avatar, int life, int power, int agility, int intelligence)
            : base(id, name, avatar, life, power, agility, intelligence)
        {
        }
    }

    [Serializable]
    class Monster : BattleUnit
    {
        public Monster(string id, string name, Bitmap avatar, int life, int power, int agility, int intelligence)
            : base(id, name, avatar, life, power, agility, intelligence)
        {
        }
    }
}
