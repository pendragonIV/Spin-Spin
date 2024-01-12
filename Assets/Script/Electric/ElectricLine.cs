using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLine : MonoBehaviour
{
    private List<GameObject> _electricTriggerList = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ElectricLine")
        {
            _electricTriggerList.Add(collision.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ElectricLine")
        {
            if (!_electricTriggerList.Contains(collision.transform.parent.gameObject))
            {
                return;
            }
            _electricTriggerList.Remove(collision.transform.parent.gameObject);
        }
    }

    public bool IsElectricLineIntersect()
    {
        if (_electricTriggerList.Count == 0)
        {
            return false;
        }
        for (int i = 0; i < _electricTriggerList.Count; i++)
        {
            Vector2 firstLineStart = _electricTriggerList[i].transform.GetChild(0).position;
            Vector2 firstLineEnd = _electricTriggerList[i].transform.GetChild(1).position;
            Vector2 secondLineStart = transform.GetChild(0).position;
            Vector2 secondLineEnd = transform.GetChild(1).position;

            if (IsElectricLineIntersection(firstLineStart, firstLineEnd, secondLineStart, secondLineEnd))
            {
                return true;
            }
        }
        return false;
    }

    static bool IsElectricLineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {

        Vector2 a = p2 - p1;
        Vector2 b = p3 - p4;
        Vector2 c = p1 - p3;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float alphaDenominator = a.y * b.x - a.x * b.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float betaDenominator = a.y * b.x - a.x * b.y;

        bool doIntersect = true;

        if (alphaDenominator == 0 || betaDenominator == 0)
        {
            doIntersect = false;
        }
        else
        {

            if (alphaDenominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                {
                    doIntersect = false;

                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
            {
                doIntersect = false;
            }

            if (doIntersect && betaDenominator > 0)
            {
                if (betaNumerator < 0 || betaNumerator > betaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (betaNumerator > 0 || betaNumerator < betaDenominator)
            {
                doIntersect = false;
            }
        }

        return doIntersect;
    }
}
