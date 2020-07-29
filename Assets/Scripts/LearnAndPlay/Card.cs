using JetBrains.Annotations;

using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public new string name;
    [CanBeNull]
    public string animalType;
    public int animalTypeID;
    [CanBeNull]
    public int legs;
    [CanBeNull]
    public bool isFlying;
    [CanBeNull]
    public string animalColor;

    public Sprite animalSprite;


    public override string ToString()
    {
        return name + ", " + animalType + ", legs: " + legs.ToString() + ", Is Flying: " + isFlying.ToString() + ", " + animalColor;
    }





}
