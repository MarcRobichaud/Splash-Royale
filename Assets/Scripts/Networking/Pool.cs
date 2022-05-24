using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.AI;

public class Pool : NetworkBehaviour
{
    #region Singleton
    private static Pool instance = null;
    public static Pool Instance => instance;

    private Pool()
    {
    }
    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } 
        else 
        {
            instance = this;
        }
    }
    
    public bool IsInit { get; private set; }

    private const int InitialSpawnNumber = 5;

    public Dictionary<Cards, CardSO> CardSos = new Dictionary<Cards, CardSO>();
        
    private Dictionary<Cards, Unit> unitResources = new Dictionary<Cards, Unit>();
    private Dictionary<Cards, Stack<Unit>> availableCardList = new Dictionary<Cards, Stack<Unit>>();

    private string prefabsPath = "prefabs/Units/";
    private string cardsPath = "Scriptable Objects/Cards/";

    public void Init(List<Cards> cardsList)
    {
        IsInit = true;
        
        foreach (var card in cardsList)
        {
            Debug.Log("prefabs/" + card);
            unitResources.Add(card, Resources.Load<Unit>(prefabsPath + card));
            CardSos.Add(card, Resources.Load<CardSO>( cardsPath + card));
        }

        foreach (var unitResource in unitResources)
        {
            availableCardList.Add(unitResource.Key, new Stack<Unit>());
            
            for (int i = 0; i < InitialSpawnNumber; i++)
            {
                availableCardList[unitResource.Key].Push(InstantiateNewUnit(unitResource.Key));
            }
        }
    }

    private Unit InstantiateNewUnit(Cards card)
    {
        Unit unit = GameObject.Instantiate(unitResources[card], Vector3.zero, Quaternion.identity);
        
        unit.NetworkObject.Spawn();
        unit.ServerInit();
        unit.enabled = false;
        unit.GetComponent<NavMeshAgent>().enabled = false;
        unit.gameObject.SetActive(false);
        SetGameObjectActiveClientRpc(unit.NetworkObjectId, false);

        return unit;
    }

    public Unit GetNewUnit(Cards card, Vector3 position, ulong id)
    {
        Unit unit;
        
        unit = availableCardList[card].Count == 0 ? InstantiateNewUnit(card) : availableCardList[card].Pop();

        SetGameObjectActiveClientRpc(unit.NetworkObjectId, true);
        unit.GetComponent<NetworkTransform>().SetState(position, unit.transform.rotation, unit.transform.localScale);
        unit.IdInit(id);
        StartCoroutine((workAround(unit, position)));

        return unit;
    }

    private IEnumerator workAround(Unit u, Vector3 position)
    {
        yield return null;
        yield return null;
        u.gameObject.SetActive(true);
        u.enabled = true;
        u.GetComponent<NavMeshAgent>().enabled = true;
    }

    public void PoolUnit(Unit unit)
    {
        ResetUnit(unit);
        availableCardList[unit.unitSO.Card].Push(unit);
    }

    private void ResetUnit(Unit unit)
    {
        unit.ResetSelf();
        unit.enabled = false;
        unit.GetComponent<NavMeshAgent>().enabled = false;
        unit.gameObject.SetActive(false);
        SetGameObjectActiveClientRpc(unit.NetworkObjectId, false);
    }

    [ClientRpc]
    private void SetGameObjectActiveClientRpc(ulong objID, bool isActive)
    {
        if (!IsHost)
        {
            GameObject go = NetworkManager.SpawnManager.SpawnedObjects[objID].gameObject;

            Unit u = go.GetComponent<Unit>();
        
            if (!isActive)
                u.SetSkinnedMeshRenderers(false);

            go.SetActive(isActive);

            if (isActive)
                StartCoroutine(SetRenderer(u));
        }
    }
    
    private IEnumerator SetRenderer(Unit u)
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        u.SetSkinnedMeshRenderers(true);
    }
}
