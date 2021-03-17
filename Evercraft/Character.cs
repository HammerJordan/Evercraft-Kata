//https://github.com/PuttingTheDnDInTDD/EverCraft-Kata

using System;
using System.Collections.Generic;
using System.Linq;

namespace Evercraft
{
    public class Character
    {
        private const int XP_GAIN = 10;
        
        private int armorClass;
        private int maxHitPoints;
        
        public string Name { get; set; }
        public AlignmentType Alignment { get; set; }
        public int Xp { get; set; }
        public Dictionary<StatTypes, Stat> StatBlock { get; set; }

        public int ArmorClass
        {
            get => armorClass + StatBlock[StatTypes.Dexterity].Modifier;
            private set => armorClass = value;
        }

        public int MaxHitPoints
        {
            get => Math.Max(1, 
                maxHitPoints + StatBlock[StatTypes.Constitution].Modifier + ((Level - 1) * 5));
            private set => maxHitPoints = value;
        }
        
        public int Damaged { get; private set; }

        public int CurrentHitPoints => MaxHitPoints - Damaged;

        public bool IsDead => CurrentHitPoints <= 0;
        public int Level => 1 + Xp / 1000;

       

        public Character(CharacterBuilder builder)
        {
            Name = builder.Name;
            Alignment = builder.Alignment;
            armorClass = builder.ArmorClass;
            maxHitPoints = builder.MaxHitPoints;
            Xp = builder.Xp;

            StatBlock = Enum.GetValues<StatTypes>()
                .ToDictionary(x => x, x =>
                {
                    return x switch
                    {
                        StatTypes.Strength => new Stat(builder.Strength),
                        StatTypes.Dexterity => new Stat(builder.Dexterity),
                        StatTypes.Constitution => new Stat(builder.Constitution),
                        StatTypes.Wisdom => new Stat(builder.Wisdom),
                        StatTypes.Intelligence => new Stat(builder.Intelligence),
                        StatTypes.Charisma => new Stat(builder.Charisma),
                        _ => throw new ArgumentOutOfRangeException(nameof(x), x, null)
                    };
                });

            Damaged = 0;
        }

        public Character() : this(new CharacterBuilder())
        {
        }

        public bool AttackHits(DiceRoll roll)
        {
            return roll.IsNatural20 || roll.Total >= ArmorClass;
        }

        public void TakeDamage(int amount)
        {
            Damaged += amount;
        }

        public bool Attack(DiceRoll roll, Character target)
        {
            roll.Modifiers.Add(GetAttackRollModifier());

            if (!target.AttackHits(roll))
                return false;

            Xp += XP_GAIN;

            target.TakeDamage(GetDamage(roll.IsNatural20));

            return true;
        }

        public int GetAttackRollModifier()
        {
            return StatBlock[StatTypes.Strength].Modifier + (Level / 2);
        }

        private int GetDamage(bool crit)
        {
            int damage = 1;
            damage += StatBlock[StatTypes.Strength].Modifier;
            damage *= crit ? 2 : 1;

            return damage > 0 ? damage : 1;
        }
    }
}