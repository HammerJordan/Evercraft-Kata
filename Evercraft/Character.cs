/*https://github.com/PuttingTheDnDInTDD/EverCraft-Kata */

using System;

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

        public Stat Strength { get; set; }
        public Stat Dexterity { get; set; }
        public Stat Constitution { get; set; }
        public Stat Wisdom { get; set; }
        public Stat Intelligence { get; set; }
        public Stat Charisma { get; set; }

        public int ArmorClass
        {
            get => armorClass + Dexterity.Modifier;
            private set => armorClass = value;
        }

        public int MaxHitPoints
        {
            get => Math.Max(1,
                maxHitPoints + Constitution.Modifier + ((Level - 1) * 5));
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

            Strength = new Stat(builder.Strength);
            Dexterity = new Stat(builder.Dexterity);
            Constitution = new Stat(builder.Constitution);
            Wisdom = new Stat(builder.Wisdom);
            Intelligence = new Stat(builder.Intelligence);
            Charisma = new Stat(builder.Charisma);

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
            return Strength.Modifier + (Level / 2);
        }

        private int GetDamage(bool crit)
        {
            int damage = 1;
            damage += Strength.Modifier;
            damage *= crit ? 2 : 1;

            return damage > 0 ? damage : 1;
        }
    }
}