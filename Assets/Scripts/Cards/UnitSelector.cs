using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelector : MonoBehaviour
{
    public List<Cards> cardsList = new List<Cards>();
    public List<Button> _buttons = new List<Button>();
    
    private Player player;

    public void Init(Player _player)
    {
        player = _player;
        
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (i >= cardsList.Count)
                break;

            Cards card = cardsList[i];
            Button button = _buttons[i];

            button.onClick.AddListener(() => { player.ChangeSelectedUnit(card);});
            button.GetComponentInChildren<TMP_Text>().text = card.ToString();
        }
    }
}

