using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    public float speed = 3;
    public int pumpkinCost = 5;
    public int palCost = 3;

    [Header("Info")]
    public Text text_coin;
    public Text text_candy;
    public Text text_skull;
    public Text text_pumpkin;
    public GameObject marketInfo;
    public GameObject lvInfo;
    public GameObject pickInfo;

    [Header("Prefab")]
    public GameObject prefab_candy;
    public GameObject prefab_coin;
    public GameObject prefab_skull;
    public GameObject prefab_pumpkin;
    public GameObject prefab_pumper;

    public Transform itemHolder;

    protected Rigidbody2D m_rb;
    protected Animator m_anim;
    protected SpriteRenderer m_visual;
    protected Collider m_coll;

    private int face = 1;
    private int coinCnt = 0;
    private int pumpkinCnt = 0;
    private int candyCnt = 0;
    private int carryPower = 1;
    private int palCnt = 0;

    private bool isDisplayedInfo;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponentInChildren<Animator>();
        m_visual = GetComponentInChildren<SpriteRenderer>();
        m_coll = GetComponent<Collider>();
    }

    public void Update()
    {
        if (m_coll.onMarketPoint) {
            if (!isDisplayedInfo) {
                marketInfo.SetActive(true);
                isDisplayedInfo = true;
            }

            if (Input.GetButtonDown("Fire3")) {
                PurchaseCoin();
            }

            if (Input.GetButtonDown("Fire2")) {
                PurchasePumpkin();
            }

        }

        if (m_coll.onLvPoint) {
            if (!isDisplayedInfo) {
                lvInfo.SetActive(true);
                isDisplayedInfo = true;
            }

            if (Input.GetButtonDown("Fire3")) {
                if(candyCnt == carryPower) {
                    carryPower++;
                    ChangeCandyValue(-candyCnt);
                    for (int i = 0; i < itemHolder.childCount; i++) {
                        Destroy(itemHolder.GetChild(i).gameObject);
                    }
                    AudioManager.Instance.PlayCarryUp();
                }
            }

            if (Input.GetButtonDown("Fire2")) {
                if (candyCnt > 2) {
                    ChangeCandyValue(-palCost);
                    palCnt++;
                    text_skull.text = palCnt.ToString();
                    var ghost = Instantiate(prefab_skull, transform.position, Quaternion.identity).GetComponent<Ghost>();
                    ghost.RegisterGhost(this, palCnt);
                    for (int i = 0; i < 3; i++) {
                        Destroy(itemHolder.GetChild(candyCnt + i).gameObject);
                    }
                    AudioManager.Instance.PlayGhost();
                }
            }

            if (Input.GetButtonDown("Fire1")) {
                if (pumpkinCnt > 0) {
                    pumpkinCnt--;
                    text_pumpkin.text = pumpkinCnt.ToString();
                    Pumper pumper = Instantiate(prefab_pumper, (Vector2)transform.position + Vector2.one, Quaternion.identity).GetComponent<Pumper>();
                    pumper.Register(this);
                    AudioManager.Instance.PlayPumper();
                }
            }
        }

        if (m_coll.onPickPoint && candyCnt != carryPower) {
            if (!isDisplayedInfo) {
                pickInfo.SetActive(true);
                isDisplayedInfo = true;
            }

            if (Input.GetButtonDown("Fire3")) {
                int getCnt = m_coll.pickPoint.TreatCandy(Mathf.Min(palCnt + 1, carryPower - candyCnt));
                if (getCnt > 0) {  
                    m_anim.SetTrigger("GetItem");
                    for (int i = 0; i < getCnt; i++) {
                        GameObject go = Instantiate(prefab_candy, itemHolder);
                        go.transform.localPosition = new Vector3(0, 0.3f * (candyCnt + i + 1), -0.1f * (candyCnt + i));
                    }
                    ChangeCandyValue(getCnt);
                    //AudioManager.Instance.PlayCandy();
                    AudioManager.Instance.PlayCandyWithDelay(0.1f);
                }
            }
        }

        if (!m_coll.onCollider) {
            if (isDisplayedInfo) {
                marketInfo.SetActive(false);
                lvInfo.SetActive(false);
                pickInfo.SetActive(false);
                isDisplayedInfo = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xr = Input.GetAxisRaw("Horizontal");
        float yr = Input.GetAxisRaw("Vertical");

        Vector3 mv = new Vector3(x, y);
        Move(mv.normalized);

        face = xr * face >= 0 ? face : -face;
        m_visual.flipX = face == -1;
        m_anim.SetFloat("Walk", mv.magnitude / 1.414f);
    }

    void Move(Vector3 mov)
    {
        m_rb.velocity = speed * mov;
    }

    IEnumerator GetCoinAnim()
    {
        GameObject go = Instantiate(prefab_coin, itemHolder);
        go.transform.localPosition = new Vector3(0, 0.4f, 0);
        yield return new WaitForSeconds(0.4f);
        AudioManager.Instance.PlayCoin();
        Destroy(go);
    }

    IEnumerator GetPumpkinAnim()
    {
        GameObject go = Instantiate(prefab_pumpkin, itemHolder);
        go.transform.localPosition = new Vector3(0, 0.4f, 0);
        yield return new WaitForSeconds(0.4f);
        AudioManager.Instance.PlayCandy();
        Destroy(go);
    }

    private void ChangeCandyValue(int value)
    {
        candyCnt += value;
        text_candy.text = candyCnt.ToString() + "-" + carryPower;
    }

    public bool PurchaseCoin()
    {
        if (candyCnt == 0)
            return false;
        ChangeCoin(1);
        ChangeCandyValue(-1);
        MarketPoint.Instance.AddCandy();
        Destroy(itemHolder.GetChild(candyCnt).gameObject);
        return true;
    }

    public bool PurchasePumpkin()
    {
        if (pumpkinCost > coinCnt || MarketPoint.Instance.PurchasePumpkin(1) == 0)
            return false;
        ChangeCoin(-pumpkinCost);

        pumpkinCnt++;
        text_pumpkin.text = pumpkinCnt.ToString();
        StartCoroutine(GetPumpkinAnim());
        return true;
    }

    public void ChangeCoin(int value)
    {
        coinCnt += value;
        text_coin.text = (coinCnt * 100).ToString();
        if (value > 0)
            StartCoroutine(GetCoinAnim());
    }
}
