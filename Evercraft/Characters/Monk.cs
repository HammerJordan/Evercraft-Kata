namespace Evercraft
{
    public class Monk : Character
    {
        public Monk(CharacterBuilder builder) : base(builder)
        {
        }

        protected override int GetHitPointModifier()
        {
            return Constitution.Modifier + ((Level - 1) * 6) + 1;
        }

        public override int GetDamage(bool crit)
        {
            return base.GetDamage(crit) + 2;
        }

        protected override int GetArmorClassModifier()
        {
            if (Wisdom.Modifier > 0)
                return base.GetArmorClassModifier() + Wisdom.Modifier;
            else
                return base.GetArmorClassModifier();
        }

        public override int GetAttackRollModifier()
        {
            return base.GetAttackRollModifier() + (Level / 3);
        }
    }
}