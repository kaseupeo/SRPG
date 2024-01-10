using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;

    private void Update()
    {
        UpdateInfo(Managers.Game.SelectedCharacter);
    }

    public void UpdateInfo(Creature creature)
    {
        if (creature == null)
            return;
        
        int level = creature.Level;
        
        image = image;
        text1.text = $"이름 : {creature.Name}\n이동력 : {creature.Stats[level].TurnCost}\n공격횟수 : {creature.Stats[level].AttackCost}";
        text2.text = $"체력 : {creature.Stats[level].HealthPoint}\n공격력 : {creature.Stats[level].Attack}\n방어력 : {creature.Stats[level].Defence}";
    }
}