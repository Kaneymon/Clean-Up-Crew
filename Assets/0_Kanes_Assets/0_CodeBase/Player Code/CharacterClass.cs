using System;
using Unity.Mathematics;

public class CharacterClass
{
    // Fields
    public float Health { get; private set; }
    public string Name { get; private set; }
    public float Armor { get; private set; }
    public float WalkSpeed { get; private set; }
    public float RunSpeed { get; private set; }
    public float JumpForce { get; private set; }
    public int InventorySize { get; private set; }
    public float Strength { get; private set; }

    // Parameterless constructor required for deserialization, i should probably write a serializer if i dont want ServerRpcs default the class to 0 parameters. NOTSURE if tahts how that works though.
    //public CharacterClass() { }

    // Constructor
    public CharacterClass(float health, string name, float armor, float walkSpeed, float runSpeed, float jumpForce, int inventorySize, float strength)
    {
        Health = health;
        Name = name;
        Armor = armor;
        WalkSpeed = walkSpeed;
        RunSpeed = runSpeed;
        JumpForce = jumpForce;
        InventorySize = inventorySize;
        Strength = strength;
    }

    // Preset Classes
    public static readonly CharacterClass Gunner = new CharacterClass(100,"Gunna", 50, 6f, 9.5f, 35f, 5, 10f);
    public static readonly CharacterClass Mopper = new CharacterClass(120,"Marcus Mop", 30, 6f, 10f, 45f, 6, 8f);
    public static readonly CharacterClass Scientist = new CharacterClass(75,"Ellie", 20, 6f, 8.5f, 40f, 4, 6f);
    public static readonly CharacterClass Engineer = new CharacterClass(90, "Davis", 40, 6f, 9f, 30f, 5, 9f);


    public static CharacterClass GetRandomClass()
    {
        int ran = UnityEngine.Random.Range(0, 4);
        switch (ran)
        {
            case 0:
                return CharacterClass.Engineer;
            case 1:
                return CharacterClass.Gunner;
            case 2:
                return CharacterClass.Scientist;
            case 3:
                return CharacterClass.Mopper;
            default:
                return CharacterClass.Gunner;
        }
    }
    public static CharacterClass EnumToCharacterClass(DefaultClasses characterClass)
    {
        switch (characterClass)
        {
            case DefaultClasses.Scientist:
                //Debug.Log($"class choice {CharacterClass.Scientist.Name}");
                return CharacterClass.Scientist;
            case DefaultClasses.Engineer:
                //Debug.Log($"class choice {CharacterClass.Engineer.Name}");
                return CharacterClass.Engineer;
            case DefaultClasses.Gunner:
                //Debug.Log($"class choice {CharacterClass.Gunner.Name}");
                return CharacterClass.Gunner;
            case DefaultClasses.Mopper:
                //Debug.Log($"class choice {CharacterClass.Mopper.Name}");
                return CharacterClass.Mopper;
            default:
                //Debug.Log("could not convert class from enum to CharacteClass. classchoicetube.cs");
                return null;
        }
    }

}
public enum DefaultClasses
{
    Gunner,
    Mopper,
    Scientist,
    Engineer
}
