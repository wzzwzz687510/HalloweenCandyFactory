using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketPoint : MonoBehaviour
{
    public static MarketPoint Instance { get; private set; }

    [Header("Setting")]
    public int maxCandyCnt = 50;
    public int maxPumpkinCnt = 3;
    public int productCandyPerTime = 1;
    public float productionCandyInterval = 5;
    public float productionPumpkinInterval = 30;
    [Range(0, 1)] public float colliderSize = 0.2f;

    public int CandyCnt { get; private set; }
    public int PumpkinCnt { get; private set; }

    public GameObject info;
    public SpriteRenderer candyRender;
    public GameObject prefab_candy;
    public GameObject prefab_pumpkin;
    public GameObject prefab_coin;
    private float productionCandyTimer;
    private float productionPumpkinTimer;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    private void LateUpdate()
    {
        if (CandyCnt != maxCandyCnt) {
            productionCandyTimer -= Time.deltaTime;
            if (productionCandyTimer < 0) {
                CandyCnt += Mathf.Min(productCandyPerTime, maxCandyCnt - CandyCnt);
                productionCandyTimer = productionCandyInterval;
                StartCoroutine(ProduceAnim(prefab_candy));
            }
        }

        if (PumpkinCnt != maxPumpkinCnt) {
            productionPumpkinTimer -= Time.deltaTime;
            if (productionPumpkinTimer < 0) {
                if (PumpkinCnt == 0)
                    info.SetActive(true);
                PumpkinCnt++;
                productionPumpkinTimer = productionPumpkinInterval;
                StartCoroutine(ProduceAnim(prefab_pumpkin));
            }
        }
        else if (PumpkinCnt == 0) {
            info.SetActive(false);
        }
    }

    IEnumerator ProduceAnim(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, transform);
        go.transform.localPosition = new Vector3(0, 1, 0);
        yield return new WaitForSeconds(0.5f);
        Destroy(go);
    }

    public int PurchaseCandy(int cnt)
    {
        int purchaseCnt = cnt;
        if (cnt > CandyCnt)
            purchaseCnt = CandyCnt;
        CandyCnt -= purchaseCnt;
        StartCoroutine(ProduceAnim(prefab_coin));

        return purchaseCnt;
    }

    public int PurchasePumpkin(int cnt)
    {
        int purchaseCnt = cnt;
        if (cnt > PumpkinCnt) {
            purchaseCnt = PumpkinCnt;
            info.SetActive(false);
        }
        PumpkinCnt -= purchaseCnt;
        StartCoroutine(ProduceAnim(prefab_coin));

        return purchaseCnt;
    }

    public void AddCandy()
    {
        CandyCnt++;
        StartCoroutine(ProduceAnim(prefab_candy));
    }
}
