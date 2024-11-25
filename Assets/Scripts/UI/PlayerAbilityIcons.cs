using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityIcons : MonoBehaviour
{
    public List<GameObject> playerAbilityIcons;
    public Sprite defaultAbilityIcon;

    void Awake()
    {
        if(playerAbilityIcons == null)
        {
            playerAbilityIcons = new List<GameObject>();

            foreach(Transform child in transform)
            {
                if(!playerAbilityIcons.Contains(child.gameObject))
                {
                    playerAbilityIcons.Add(child.gameObject);
                }
            }
        }
    }

    public void SetAbilityIcon(Ability ability)
    {
        if(ability is PlayerRangedAttack)
        {
            playerAbilityIcons[0].GetComponent<Image>().sprite = ability.icon;
        }
        else if(ability is PlayerDash)
        {
            playerAbilityIcons[1].GetComponent<Image>().sprite = ability.icon;
        }
    }

    public void ChangeAbilityIcon(Ability ability)
    {
        int abilityIndex = 0;
        
        if(ability is PlayerRangedAttack)
        {
            abilityIndex = 0;
        }
        else if(ability is PlayerDash)
        {
            abilityIndex = 1;
        }

        switch(ability.state)
        {
            case Ability.AbilityState.ready:
                playerAbilityIcons[abilityIndex].GetComponent<Image>().fillAmount = 1;
                playerAbilityIcons[abilityIndex].GetComponent<Image>().color = new Color(1,1,1,1);
            break;

            case Ability.AbilityState.active:
                playerAbilityIcons[abilityIndex].GetComponent<Image>().color = new Color(0.2f,0.2f,0.2f,1f);
            break;
        }
    }

    public void ChangeAbilityIconCooldownPercentage(Ability ability, float percentage)
    {
        if(ability is PlayerRangedAttack)
        {
            playerAbilityIcons[0].GetComponent<Image>().fillAmount = percentage;
        }
        else if(ability is PlayerDash)
        {
            playerAbilityIcons[1].GetComponent<Image>().fillAmount = percentage;
        }
    }
}
