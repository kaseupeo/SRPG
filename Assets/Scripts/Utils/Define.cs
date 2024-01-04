public class Define
{
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
}