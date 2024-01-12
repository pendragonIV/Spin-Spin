using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinManager : MonoBehaviour
{
    public static SpinManager instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    [SerializeField]
    private Transform _spinContainer;

    public bool IsAllSpinLinked()
    {
        foreach(Transform spin in _spinContainer)
        {
            if (!spin.GetComponent<Spin>().IsSpinFullyCharge())
            {
                return false;
            }
        }
        return true;
    }
}
