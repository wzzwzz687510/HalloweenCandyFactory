using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider : MonoBehaviour
{
    [Header("Setting")]
    public Vector2 hitOffset = new Vector2(0, 0.22f);
    public Vector2 hitBoxSize = new Vector2(0.28f,0.45f);
    public LayerMask collLayer;

    public bool onCollider { get; private set; }
    public bool onPickPoint { get; private set; }
    public bool onMarketPoint { get; private set; }
    public bool onLvPoint { get; private set; }
    private bool collChanged = true;

    public PickPoint pickPoint { get; private set; }
    public MarketPoint marketPoint { get; private set; }

    private void Update()
    {
        var obj = Physics2D.OverlapBox((Vector2)transform.position - hitOffset, hitBoxSize,0, collLayer);
        if (obj && collChanged) {
            collChanged = false;

            onPickPoint = obj.CompareTag("PickPoint");
            if (onPickPoint)
                pickPoint = obj.GetComponent<PickPoint>();
            onMarketPoint = obj.CompareTag("MarketPoint");
            if (onMarketPoint)
                marketPoint = obj.GetComponent<MarketPoint>();
            onLvPoint = obj.CompareTag("LvPoint");
        }
         if(!obj && !collChanged) {
            collChanged = true;
            onPickPoint = false;
            onMarketPoint = false;
            onLvPoint = false;
        }

        onCollider = onPickPoint || onMarketPoint || onLvPoint;
    }
}
