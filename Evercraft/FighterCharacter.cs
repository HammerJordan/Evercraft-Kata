namespace Evercraft
{
    public class FighterCharacter : Character
    {
        public FighterCharacter(CharacterBuilder builder) : base(builder)
        {
        }


        public override int GetAttackRollModifier()
        {
            return Strength.Modifier + Level;
        }

        protected override int GetHitPointModifier()
        {
            return Constitution.Modifier + ((Level - 1) * 10);
        }
    }
}