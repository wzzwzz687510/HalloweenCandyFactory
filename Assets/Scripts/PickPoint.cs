using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickPoint : MonoBehaviour
{
    [Header("Setting")]
    public int maxCandyCnt = 5;
    public float purchaseInterval = 10;
    public float pickableInterval = 20;
    [Range(0, 1)] public float colliderRadius = 0.2f;

    public int CandyCnt { get; private set; }
    public int purchasingPower { get; private set; }
    public bool isPickable { get; private set; }

    public delegate void Pickable(int index);
    public event Pickable pickableEvent;
    public event Pickable unpickableEvent;
    private int _index;

    public GameObject info;
    public GameObject prefab_candy;
    public SpriteRenderer candyRender;
    private float purchaseTimer;
    private float pickableTimer;

    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = colliderRadius;
        isPickable = true;
        purchasingPower = 1;
        CandyCnt = 5;
        purchaseTimer = purchaseInterval;
        pickableTimer = pickableInterval;
        
    }

    private void Update()
    {
        if(CandyCnt!= maxCandyCnt) {
            purchaseTimer -= Time.deltaTime;
            if (purchaseTimer < 0) {
                if (CandyCnt == 0)
                    info.SetActive(true);
                CandyCnt += MarketPoint.Instance.PurchaseCandy(Mathf.Min(purchasingPower, maxCandyCnt - CandyCnt));
                purchaseTimer = purchaseInterval;
                StartCoroutine(PurchaseAnim());
            }
        }
        else if(CandyCnt == 0) {
            info.SetActive(false);
        }

        if (!isPickable) {
            pickableTimer -= Time.deltaTime;
            if (pickableTimer < 0 && CandyCnt > 0) {
                isPickable = true;
                pickableTimer = pickableInterval;
                pickableEvent(_index);
                candyRender.DOColor(Color.white, 0.5f);
            }
        }
    }

    IEnumerator PurchaseAnim()
    {
        GameObject go = Instantiate(prefab_candy, transform);
        go.transform.localPosition = new Vector3(0,1,0);
        yield return new WaitForSeconds(0.5f);
        Destroy(go);
    }

    public int TreatCandy(int cnt)
    {
        if (!isPickable)
            return 0;
        int treatCnt = cnt;
        if(cnt > CandyCnt) {
            treatCnt = CandyCnt;
            info.SetActive(false);
        }

        CandyCnt -= treatCnt;
        isPickable = false;
        unpickableEvent(_index);
        candyRender.DOColor(Color.grey, 0.5f);

        return treatCnt;
    }

    public void RegisterIndex(int index)
    {
        _index = index;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, colliderRadius);
    //}
}
