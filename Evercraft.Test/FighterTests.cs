using FluentAssertions;
using Xunit;

namespace Evercraft.Test
{
    public class FighterTests
    {
        private Character character;
        private Character enemy;

        public FighterTests()
        {
            enemy = new Character();
            character = new CharacterBuilder().AsFighter();
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        [InlineData(5, 5)]
        public void AttackRoll_IncreasesBy1EachLevel(int level, int expcted)
        {
            character = new CharacterBuilder()
                .SetLevel(level)
                .AsFighter();

            character.GetAttackRollModifier().Should().Be(expcted);
        }

        [Theory]
        [InlineData(1, 5)]
        [InlineData(2, 15)]
        [InlineData(3, 25)]
        [InlineData(4, 35)]
        [InlineData(5, 45)]
        public void HitPoints_IncreasesBy10EachLevel(int level, int expcted)
        {
            character = new CharacterBuilder()
                .SetLevel(level)
                .AsFighter();

            character.MaxHitPoints.Should().Be(expcted);
        }
    }
}