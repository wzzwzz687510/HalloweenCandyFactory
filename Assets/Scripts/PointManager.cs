using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance { get; private set; }

    protected PickPoint[] pickPoints;
    private HashSet<int> pickablePoint = new HashSet<int>();

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    private void Start()
    {
        pickPoints = GetComponentsInChildren<PickPoint>();
        if (pickPoints.Length != 0) {
            for (int i = 0; i < pickPoints.Length; i++) {
                pickablePoint.Add(i);
                pickPoints[i].RegisterIndex(i);
                pickPoints[i].pickableEvent += PointPickable;
                pickPoints[i].unpickableEvent += PointUnpickable;
            }
        }
    }

    void PointPickable(int index)
    {
        pickablePoint.Add(index);
    }

    void PointUnpickable(int index)
    {
        pickablePoint.Remove(index);
    }

    public int FindPickablePointIndex()
    {
        for (int i = 0; i < pickPoints.Length; i++) {
            if (pickablePoint.Contains(i)) {
                pickablePoint.Remove(i);
                return i;
            }                
        }

        return -1;
    }

    public PickPoint GetPickPoint(int index)
    {
        if (index < 0 || index > pickPoints.Length - 1)
            return null;
        return pickPoints[index];
    }
}
