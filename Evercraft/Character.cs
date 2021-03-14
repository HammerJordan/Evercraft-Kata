using System;
//https://github.com/PuttingTheDnDInTDD/EverCraft-Kata
namespace Evercraft
{
    public class Character
    {
        private readonly IDice d20;
        public string Name { get; set; }
        public AlignmentType Alignment { get; set; }
        public int ArmorClass { get; private set; }
        public int HitPoints { get; private set; }

        public bool IsDead => HitPoints <= 0;

        public Character(
            string name = "Hero",
            AlignmentType alignment = AlignmentType.Neutral,
            int armorClass = 10,
            int hitPoints = 5,
            IDice d20 = null)
        {
            Name = name;
            Alignment = alignment;
            ArmorClass = armorClass;
            HitPoints = hitPoints;
            this.d20 = d20 ?? new D20();
        }

        public DiceRoll Roll()
        {
            return new DiceRoll(d20.Roll(), null);
        }

        public bool AttackHits(DiceRoll roll)
        {
            
            return roll.Natural20  || roll.Total >= ArmorClass;
        }

        public void BeAttacked(DiceRoll roll)
        {
            if (!AttackHits(roll))
                return;
            
            if (roll.Natural20)
            {
                HitPoints -= 2;
            }
            else
            {
                HitPoints -= 1;
            }
        }
    }
}