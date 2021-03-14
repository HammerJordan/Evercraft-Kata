using System;
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace Evercraft.Test
{
    public class CharacterTests
    {
        [Fact]
        public void Name_ShouldBeAssigned()
        {
            const string name = "Wizard";
            var character = new Character() {Name = name};

            character.Name.Should().BeEquivalentTo(name);
        }
        
        [Fact]
        public void AlignmentType_ShouldBeAssigned()
        {
            var character = new Character {Alignment = AlignmentType.Evil};

            character.Alignment.Should().Be(AlignmentType.Evil);
        }
        
        [Fact]
        public void DefaultCharacter_ShouldHave10ArmorAnd5HP()
        {
            var character = new Character();
            character.ArmorClass.Should().Be(10);
            character.HitPoints.Should().Be(5);
        }
        
        [Fact]
        public void Roll_Returns1To20()
        {
            var character = new Character();
            character.Roll().Roll.Should().BeInRange(1, 20);
        }
        [Fact]
        public void InjectDice_ReturnsDiceValue()
        {
            var dice = NSubstitute.Substitute.For<IDice>();
            dice.Roll().Returns(20);
            var character = new Character(d20:dice);

            character.Roll().Roll.Should().Be(20);
        }
        
        [Theory]
        [InlineData(5,false)]
        [InlineData(10,true)]
        [InlineData(11,true)]
        
        public void AttackHits_ReturnsTrue_WhenRollMeetsOrBeatArmorClass(int roll, bool result)
        {
            var character = new Character();
            var hit = character.AttackHits(new DiceRoll(roll,null));
            hit.Should().Be(result);

        }
        [Fact]
        public void AttackHits_ReturnsTrue_OnNat20()
        {
            var character = new Character();
            var hit = character.AttackHits(new DiceRoll(20,null));
            hit.Should().BeTrue();
        }
        
        [Fact]
        public void BeAttacked_WhenRollHitsAC_Lose1Life()
        {
            var character = new Character();
            character.BeAttacked(new DiceRoll(10,null));
            character.HitPoints.Should().Be(4);
        }
        
        [Fact]
        public void BeAttacked_WhenRollNat20_Lose2Life()
        {
            var character = new Character();
            character.BeAttacked(new DiceRoll(20,null));
            character.HitPoints.Should().Be(3);
        }
        
        [Fact]
        public void IsDead_WhenNoLifeLeft()
        {
            var character = new Character();
            character.BeAttacked(new DiceRoll(20,null));
            character.BeAttacked(new DiceRoll(20,null));
            character.BeAttacked(new DiceRoll(20,null));
            character.IsDead.Should().BeTrue();
        }
        
        
        
        
    }
}