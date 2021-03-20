using System;
using System.Reflection.Metadata.Ecma335;

namespace Evercraft
{
    public class CharacterBuilder
    {
        public int ArmorClass { get; set; } = 10;
        public int MaxHitPoints { get; set; } = 5;
        public string Name { get; set; } = "Hero";
        public AlignmentType Alignment { get; set; } = AlignmentType.Neutral;
        public int Xp { get; set; } = 0;
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Constitution { get; set; } = 10;
        public int Wisdom { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public int Charisma { get; set; } = 10;

        public CharacterBuilder SetArmorClass(int armorClass)
        {
            ArmorClass = armorClass;
            return this;
        }

        public CharacterBuilder SetHp(int hp)
        {
            MaxHitPoints = hp;
            return this;
        }

        public CharacterBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public CharacterBuilder SetAlignment(AlignmentType alignment)
        {
            Alignment = alignment;
            return this;
        }

        public CharacterBuilder SetXp(int xp)
        {
            Xp = xp;
            return this;
        }

        public CharacterBuilder SetStrength(int value)
        {
            Strength = value;
            return this;
        }
        
        public CharacterBuilder SetDexterity(int value)
        {
            Dexterity = value;
            return this;
        }
        public CharacterBuilder SetConstitution(int value)
        {
            Constitution = value;
            return this;
        }
        public CharacterBuilder SetWisdom(int value)
        {
            Wisdom = value;
            return this;
        }
        public CharacterBuilder SetIntelligence(int value)
        {
            Strength = value;
            return this;
        }
        public CharacterBuilder SetCharisma(int value)
        {
            Strength = value;
            return this;
        }

        public Character Build()
        {
            return new Character(this);
        }
        
        public static implicit operator Character(CharacterBuilder builder)
        {
            return builder.Build();
        }
    }
}