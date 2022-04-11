using System.Net.Security;
using FluentAssertions;
using Xunit;

namespace Evercraft.Test
{
    public class RougeTests
    {
        private Character character;
        private Character enemy;

        public RougeTests()
        {
            enemy = new Character();
            character = new CharacterBuilder().AsRogue();
        }

        [Theory]
        [InlineData(1,3)]
        [InlineData(2,6)]
        [InlineData(3,6)]
        [InlineData(4,9)]
        public void DoesTripleDamage_WhenHitIsCritical(int level, int expectedCrit)
        {
            character = new CharacterBuilder().SetLevel(level).AsRogue();
            
            var startHp = enemy.CurrentHitPoints;
            character.Attack(new DiceRoll(20), enemy);
            var damageDone = startHp - enemy.CurrentHitPoints;

            damageDone.Should().Be(expectedCrit);
        }

        [Fact]
        public void IgnoresOpponentsDexterityModifer_ToArmorClassWhenAttacking()
        {
            enemy = new CharacterBuilder().SetDexterity(16);

            character.Attack(new DiceRoll(11), enemy);

            enemy.CurrentHitPoints.Should().NotBe(enemy.MaxHitPoints);
        }
        
        
        
    }
}