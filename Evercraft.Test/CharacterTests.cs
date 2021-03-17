using System;
using Xunit;
using FluentAssertions;
using FluentAssertions.Common;
using NSubstitute;

namespace Evercraft.Test
{
    public class CharacterTests
    {
        private readonly Character character;
        private Character enemy;

        public CharacterTests()
        {
            character = new Character();
            enemy = new Character();
        }

        [Fact]
        public void DefaultValues_AreSet()
        {
            character.Alignment.IsSameOrEqualTo(AlignmentType.Neutral);
            character.Name.IsSameOrEqualTo("Hero");
            character.ArmorClass.IsSameOrEqualTo(10);
            character.HitPoints.IsSameOrEqualTo(5);

            foreach (var stat in character.StatBlock.Values)
                stat.Value.Should().Be(10);
        }

        [Theory]
        [InlineData(5, false)]
        [InlineData(10, true)]
        [InlineData(11, true)]
        public void Attack_ReturnsTrueAndTargetTakesDamage_WhenRollMeetsOrBeatArmorClass(int roll, bool result)
        {
            var hit = character.Attack(new DiceRoll(roll, null), enemy);
            hit.Should().Be(result);
            enemy.HitPoints.Should().Be(result ? 4 : 5);
        }

        [Fact]
        public void Attack_HitsOnNat20_WhenArmorIsGreaterThen20()
        {
            enemy = new Character(armorClass: 22);
            var hit = character.Attack(new DiceRoll(20, null), enemy);
            hit.Should().BeTrue();
            enemy.HitPoints.Should().Be(3);
        }

        [Fact]
        public void IsDead_WhenNoLifeLeft()
        {
            enemy.IsDead.Should().BeFalse();
            character.Attack(new DiceRoll(20, null), enemy);
            character.Attack(new DiceRoll(20, null), enemy);
            enemy.IsDead.Should().BeFalse();
            character.Attack(new DiceRoll(20, null), enemy);
            enemy.IsDead.Should().BeTrue();
        }

        [Theory]
        [InlineData(9, 10, false)]
        [InlineData(9, 11, false)]
        [InlineData(9, 12, true)]
        [InlineData(10, 10, true)]
        [InlineData(10, 9, false)]
        [InlineData(5, 20, true)]
        [InlineData(14, 1, false)]
        public void Strength_ModifiesAttackRoll(int roll, int strength, bool expected)
        {
            character.StatBlock[StatTypes.Strength] = new Stat(strength);

            bool hit = character.Attack(new DiceRoll(roll, null), enemy);
            hit.Should().Be(expected);
        }
        
        [Theory]
        [InlineData(10, 1)]
        [InlineData(12, 2)]
        [InlineData(14, 3)]
        [InlineData(20, 6)]
        [InlineData(8, 1)]
        [InlineData(1, 1)]

        public void Strength_ModifiesAttackDamage(int strength, int expectedDamageDealt)
        {
            character.StatBlock[StatTypes.Strength] = new Stat(strength);
            int startHp = enemy.HitPoints;
            
            character.Attack(new DiceRoll(15, null), enemy);
            int damageDone = startHp - enemy.HitPoints;


            damageDone.Should().Be(expectedDamageDealt);
        }
        
        [Fact]
        
        public void Strength_ModifiesAttackDamage_OnCritMinIs1()
        {
            const int expectedDamageDealt = 1;
            character.StatBlock[StatTypes.Strength] = new Stat(1);
            int startHp = enemy.HitPoints;
            
            character.Attack(new DiceRoll(20, null), enemy);
            int damageDone = startHp - enemy.HitPoints;

            damageDone.Should().Be(expectedDamageDealt);
        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(12, 11)]
        [InlineData(20, 15)]
        [InlineData(1, 5)]
        public void Dexterity_ModifiesArmorClass(int dexAmount, int expectedArmorClass)
        {
            character.StatBlock[StatTypes.Dexterity] = new Stat(dexAmount);

            character.ArmorClass.Should().Be(expectedArmorClass);

        }
        
        [Theory]
        [InlineData(10, 5)]
        [InlineData(12, 6)]
        [InlineData(20, 10)]
        [InlineData(8, 4)]
        [InlineData(1, 1)]
        public void Constitution_ModifiesHp_MinOf1(int conAmount, int expectedHp)
        {
            character.StatBlock[StatTypes.Constitution] = new Stat(conAmount);

            character.HitPoints.Should().Be(expectedHp);
        }
        
        
        [Fact]
        public void Character_GainsXp_OnSuccessfulAttack()
        {
            character.XP.Should().Be(0);
            character.Attack(new DiceRoll(15),enemy);
            character.XP.Should().Be(10);
        }
        
        [Fact]
        public void Character_ShouldNotGainXp_OnUnSuccessfulAttack()
        {
            character.XP.Should().Be(0);
            character.Attack(new DiceRoll(8),enemy);
            character.XP.Should().Be(0);
        }
        
        
    }
}