using System;
using System.Xml.XPath;
using Xunit;
using FluentAssertions;
using FluentAssertions.Common;
using NSubstitute;

namespace Evercraft.Test
{
    public class CharacterTests
    {
        private const int XP_PER_LEVEL = 1000;
        
        private Character character;
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
            character.MaxHitPoints.IsSameOrEqualTo(5);

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
            enemy.CurrentHitPoints.Should().Be(result ? 4 : 5);
        }

        [Fact]
        public void Attack_HitsOnNat20_WhenArmorIsGreaterThen20()
        {
            enemy = new CharacterBuilder().SetArmorClass(22);
            var hit = character.Attack(new DiceRoll(20, null), enemy);
            hit.Should().BeTrue();
            enemy.CurrentHitPoints.Should().Be(3);
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
            int startHp = enemy.CurrentHitPoints;

            character.Attack(new DiceRoll(15, null), enemy);
            int damageDone = startHp - enemy.CurrentHitPoints;

            damageDone.Should().Be(expectedDamageDealt);
        }

        [Fact]
        public void Strength_ModifiesAttackDamage_OnCritMinIs1()
        {
            const int expectedDamageDealt = 1;
            character.StatBlock[StatTypes.Strength] = new Stat(1);
            int startHp = enemy.CurrentHitPoints;

            character.Attack(new DiceRoll(20, null), enemy);
            int damageDone = startHp - enemy.CurrentHitPoints;

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

            character.MaxHitPoints.Should().Be(expectedHp);
        }

        [Fact]
        public void Character_GainsXp_OnSuccessfulAttack()
        {
            character.Xp.Should().Be(0);
            character.Attack(new DiceRoll(15), enemy);
            character.Xp.Should().Be(10);
        }

        [Fact]
        public void Character_ShouldNotGainXp_OnUnSuccessfulAttack()
        {
            character.Xp.Should().Be(0);
            character.Attack(new DiceRoll(8), enemy);
            character.Xp.Should().Be(0);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(999, 1)]
        [InlineData(1000, 2)]
        [InlineData(1999, 2)]
        [InlineData(2000, 3)]
        [InlineData(3000, 4)]
        [InlineData(4000, 5)]
        public void Character_ShouldLevelUpOnXpGain(int xp, int expectedLevel)
        {
            character = new CharacterBuilder().SetXp(xp);

            character.Level.Should().Be(expectedLevel);
        }

        [Theory]
        [InlineData(1, 10, 5)]
        [InlineData(2, 10, 10)]
        [InlineData(2, 1, 5)]
        [InlineData(2, 20, 15)]
        [InlineData(3, 10, 15)]
        [InlineData(3, 1, 10)]
        [InlineData(3, 20, 20)]
        public void CharacterHitPoints_ShouldIncreaseBy5EachLevel(int level, int con, int expectedHp)
        {
            character = new CharacterBuilder()
                .SetStat(StatTypes.Constitution, con)
                .SetXp((level - 1) * XP_PER_LEVEL);

            character.MaxHitPoints.Should().Be(expectedHp);
        }
        
        [Theory]
        [InlineData(1, 10, 0)]
        [InlineData(1, 1, -5)]
        [InlineData(1, 20, 5)]
        [InlineData(2, 10, 1)]
        [InlineData(2, 20, 6)]
        [InlineData(2, 1, -4)]
        [InlineData(3, 10, 1)]
        [InlineData(4, 10, 2)]
        [InlineData(5, 10, 2)]
        [InlineData(6, 10, 3)]
        public void CharacterAttackRoll_ShouldIncreaseBy1EveryEvenLevel(int level, int strength, int expectedRollMod)
        {
            character = new CharacterBuilder()
                .SetStat(StatTypes.Strength, strength)
                .SetXp((level - 1) * XP_PER_LEVEL);

            character.GetAttackRollModifier().Should().Be(expectedRollMod);

        }
    }
}