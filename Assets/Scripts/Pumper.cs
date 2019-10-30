using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pumper : MonoBehaviour
{
    [Header("Setting")]
    public float perUnitTime = 1;

    public Transform itemHolder;
    public GameObject prefab_candy;


    private Player player;
    private Vector3[] path;
    private PickPoint targetPoint;
    private bool hasCandy;
    private bool isAction;

    protected Rigidbody2D m_rb;
    protected Animator m_anim;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(!isAction) {
            if (!hasCandy) {
                int pointIndex = PointManager.Instance.FindPickablePointIndex();
                if (pointIndex == -1)
                    return;
                targetPoint = PointManager.Instance.GetPickPoint(pointIndex);
                path = NavigationManager.Instance.CalculatePath(transform.position, targetPoint.transform.position);

            }
            else {
                path = NavigationManager.Instance.CalculatePath(transform.position, MarketPoint.Instance.transform.position);
            }

            StartCoroutine(MoveAlongPath());
            isAction = true;
        }
    }

    public void Register(Player p)
    {
        player = p;
    }

    IEnumerator MoveAlongPath()
    {
        m_anim.SetFloat("Walk", 1);
        transform.DOPath(path, path.Length*perUnitTime);
        yield return new WaitForSeconds(path.Length * perUnitTime);
        m_anim.SetFloat("Walk", 0);

        if (hasCandy) {
            MarketPoint.Instance.AddCandy();
            player.ChangeCoin(1);
            hasCandy = false;
            Destroy(itemHolder.GetChild(0).gameObject);
        }
        else {
            int cnt = 0;
            while(cnt == 0) {
                yield return new WaitForSeconds(1);
                cnt = targetPoint.TreatCandy(1);
            }
            hasCandy = true;
            GameObject go = Instantiate(prefab_candy, itemHolder);
            go.transform.localPosition = new Vector3(0, 0.3f, -0.1f);
            m_anim.SetTrigger("GetItem");
        }
        yield return new WaitForSeconds(1);

        isAction = false;
    }
}
