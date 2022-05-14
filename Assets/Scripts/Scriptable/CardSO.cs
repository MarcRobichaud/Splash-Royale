using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card SO")]
public class CardSO : ScriptableObject
{
    [SerializeField] 
    public int manaCost;
    [SerializeField]
    private int quantity;
}
