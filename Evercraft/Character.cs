//https://github.com/PuttingTheDnDInTDD/EverCraft-Kata

using System;
using System.Collections.Generic;
using System.Linq;

namespace Evercraft
{
    public class Character
    {
        private int armorClass;
        private int hitPoints;
        public string Name { get; set; }
        public AlignmentType Alignment { get; set; }

        public int ArmorClass
        {
            get => armorClass + StatBlock[StatTypes.Dexterity].Modifier;
            private set => armorClass = value;
        }

        public int HitPoints
        {
            get => hitPoints + (StatBlock[StatTypes.Constitution].Modifier < -4 ?
                -4 :
                StatBlock[StatTypes.Constitution].Modifier);
            private set => hitPoints = value;
        }

        public Dictionary<StatTypes, Stat> StatBlock { get; set; }

        public bool IsDead => HitPoints <= 0;

        public int XP { get; private set; }

        public Character(
            string name = "Hero",
            AlignmentType alignment = AlignmentType.Neutral,
            int armorClass = 10,
            int hitPoints = 5)
        {
            Name = name;
            Alignment = alignment;
            this.armorClass = armorClass;
            HitPoints = hitPoints;
            XP = 0;

            StatBlock = Enum.GetValues<StatTypes>()
                .ToDictionary(x => x, _ => new Stat(10));
        }

        public bool AttackHits(DiceRoll roll)
        {
            return roll.IsNatural20 || roll.Total >= ArmorClass;
        }

        public void TakeDamage(int amount)
        {
            HitPoints -= amount;
        }

        public bool Attack(DiceRoll roll, Character target)
        {
            roll.Modifiers.Add(StatBlock[StatTypes.Strength].Modifier);

            if (!target.AttackHits(roll))
                return false;

            XP += 10;
            
            target.TakeDamage(GetDamage(roll.IsNatural20));

            return true;
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