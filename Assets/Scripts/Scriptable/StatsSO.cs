using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats SO")]
public class StatsSO : ScriptableObject
{
        [SerializeField] 
        public Stats value;
        
        public Stats InitialValue { get; private set; }

        public void Init()
        {
            InitialValue = value;
        }

        public void ResetSelf()
        {
            value = InitialValue;
        }

        public bool IsDead => value.Hp <= 0;

       public void Hit(Stats hitStats)
       {
           value.Hp += hitStats.Hp;

           if (value.Hp > InitialValue.Hp)
           {
               value.Hp = InitialValue.Hp;
           }
       }
}
