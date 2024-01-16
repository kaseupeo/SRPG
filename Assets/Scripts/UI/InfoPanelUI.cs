using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;

    public void UpdateInfo(Creature creature)
    {
        if (creature == null || creature.Name == null || creature.CurrentStat == null)
        {
            image.color = new Color(1, 1, 1, 0);
            text1.text = "이름 : \n이동력 : \n공격횟수 : ";
            text2.text = "체력 : \n공격력 : \n방어력 : ";
            return;
        }

        if (Managers.Game.GameMode == Define.GameMode.Preparation)
        {
            int level = creature.Level;
            image.color = new Color(1, 1, 1, 1);
            image.sprite = creature.Icon;
            text1.text = $"이름 : {creature.Name}\n레벨 : {creature.Level}\n이동력 : {creature.Stats[level].TurnCost}\n공격횟수 : {creature.Stats[level].AttackCost}";
            text2.text = $"체력 : {creature.Stats[level].HealthPoint}\n근접 공격력 : {creature.Stats[level].MeleeAttack}\n원거리 공격력 : {creature.Stats[level].LongRangeAttack}\n방어력 : {creature.Stats[level].Defence}";
        }
        else
        {
            int level = creature.Level;
            image.color = new Color(1, 1, 1, 1);
            image.sprite = creature.Icon;
            text1.text = $"이름 : {creature.Name}\n체력 : {creature.CurrentStat.HealthPoint}/{creature.Stats[level].HealthPoint}\n근접 공격력 : {creature.CurrentStat.MeleeAttack}\n공격횟수 : {creature.CurrentStat.AttackCost}";
            text2.text = $"레벨 : {creature.Level}\n이동력 : {creature.CurrentStat.TurnCost}\n원거리 공격력 : {creature.CurrentStat.LongRangeAttack}\n방어력 : {creature.CurrentStat.Defence}";

        }
    }
}