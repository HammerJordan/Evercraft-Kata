using FluentAssertions;
using Xunit;

namespace Evercraft.Test
{
    public class MonkTests
    {
        private Character character;
        private Character enemy;

        public MonkTests()
        {
            enemy = new Character();
            character = new CharacterBuilder().AsMonk();
        }

        [Theory]
        [InlineData(1,6)]
        [InlineData(2,12)]
        [InlineData(3,18)]
        public void Has6HitPoints_PerLevel(int level, int expectedHP)
        {
            character = new CharacterBuilder().SetLevel(level).AsMonk();

            character.MaxHitPoints.Should().Be(expectedHP);
        }

        [Fact]
        public void Does3DamagePerAttack_InsteadOf1()
        {
            character.Attack(new DiceRoll(15), enemy);

            int damageDone = enemy.Damaged;

            damageDone.Should().Be(3);
        }

        [Theory]
        [InlineData(10,10,10)]
        [InlineData(10,8,9)]
        [InlineData(10,12,11)]
        [InlineData(12,10,11)]
        [InlineData(8,10,10)]
        [InlineData(20,20,20)]
        public void AddsPositiveWisdomModifierToArmorClass(int wisdom, int dex, int expectedArmor)
        {
            character = new CharacterBuilder()
                .SetDexterity(dex)
                .SetWisdom(wisdom)
                .AsMonk();

            character.ArmorClass.Should().Be(expectedArmor);
        }

        [Theory]
        [InlineData(1,0)]
        [InlineData(2,1)]
        [InlineData(3,2)]
        [InlineData(4,3)]
        [InlineData(5,3)]
        [InlineData(6,5)]
        public void AttackRollIsIncreasedEverySecondAndThirdLevel(int level, int expectedAttack)
        {
            character = new CharacterBuilder().SetLevel(level).AsMonk();

            character.GetAttackRollModifier().Should().Be(expectedAttack);
        }
        
  
    }
}