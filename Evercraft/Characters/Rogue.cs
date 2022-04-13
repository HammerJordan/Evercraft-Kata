using System;

namespace Evercraft
{
    public class Rogue: Character
    {
        protected override bool IgnoreTargetsDexterity => true;

        public Rogue(CharacterBuilder builder) : base(builder)
        {
            if (builder.Alignment == AlignmentType.Good)
            {
                throw new ArgumentException("Rogue Cant have good alignment");
            }
        }

        public override int GetDamage(bool crit)
        {
            var baseDamage = base.GetDamage(false);
            return crit ? baseDamage * 3 : baseDamage;
        }

        public override int GetAttackRollModifier()
        {
            return Dexterity.Modifier + (Level / 2);
        }
    }
}