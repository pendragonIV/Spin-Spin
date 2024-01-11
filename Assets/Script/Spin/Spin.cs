using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    private GameObject _firstLinkedSpin;
    private GameObject _secondLinkedSpin;

    public bool IsSpinLinkedTo(GameObject spin)
    {
        if (_firstLinkedSpin == null || _secondLinkedSpin == null)
        {
            return false;
        }
        return spin == _firstLinkedSpin || spin == _secondLinkedSpin;
    }

    public bool LinkSpin(GameObject spin)
    {
        if (_firstLinkedSpin == null)
        {
            _firstLinkedSpin = spin;
            return true;
        }
        else if (_secondLinkedSpin == null)
        {
            _secondLinkedSpin = spin;
            return true;
        }
        return false;
    }
}
