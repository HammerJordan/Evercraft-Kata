namespace Evercraft
{
    public class RogueCharacter: Character
    {
        public RogueCharacter(CharacterBuilder builder) : base(builder)
        {
        }

        protected override int GetDamage(bool crit)
        {
            var baseDamage = base.GetDamage(false);

            return crit ? baseDamage * 3 : baseDamage;
        }
    }
}