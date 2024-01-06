using UnityEngine;

public class Define
{
    public enum GameMode
    {
        Preparation,
        Battle,
    }
    
    public enum SceneType
    {
        LobbyScene,
        GenerateCharacterScene,
        GameScene,
    }
    
    public enum ItemType
    {
        Equipment,
        Consumption,
    }
    
    public enum EquipType
    {
        Weapon,
        
    }
    
    public enum SkillType
    {
        None,
        Helmet,
        Armor,
        Shoes,
    }
    
    public enum CreatureState
    {
        Idle,
        Move,
        Attack,
        Dead,
    }

    public enum ArrowDirection
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4,
        TopRight = 5,
        BottomRight = 6,
        TopLeft = 7,
        BottomLeft = 8,
        UpFinished = 9,
        DownFinished = 10,
        LeftFinished = 11,
        RightFinished = 12
    }
}