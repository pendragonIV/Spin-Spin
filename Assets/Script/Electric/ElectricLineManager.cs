using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ElectricLineManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _electricLinePrefab;
    [SerializeField]
    private Transform _electricLineContainer;

    private GameObject _currentElectricLine;
    private GameObject _currentSpin;
    private GameObject _currentElectricStart;
    private GameObject _currentElectricEnd;
    private GameObject _currentElectricCenter;
    private LineRenderer _currentLineRenderer;

    [SerializeField]
    private Gradient unLinkable;
    [SerializeField]
    private Gradient linkable;

    private void Update()
    {
        if(GameManager.instance.IsThisGameFinalOrLose() || GameManager.instance.IsThisGameFinalOrWin())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && _currentElectricLine == null)
        {
            GameObject spin = CheckSpinRay();
            if (spin == null || !spin.GetComponent<Spin>().IsSpinCanLinkTo())
            {
                return;
            }
            _currentSpin = spin;
            _currentSpin.GetComponent<Spin>().StaticSpin();
            Vector2 spinPos = spin.transform.position;
            CreateNewElectricLine(spinPos, spinPos);
        }
        UpdateElectricLineEnd();
        PlaceSpinController();
    }

    public void CreateNewElectricLine(Vector2 startPos, Vector2 endPos)
    {
        GameObject electricLine = Instantiate(_electricLinePrefab, _electricLineContainer);
        _currentElectricEnd = electricLine.transform.GetChild(1).gameObject;
        _currentElectricStart = electricLine.transform.GetChild(0).gameObject;
        _currentElectricCenter = electricLine.transform.GetChild(2).gameObject;

        _currentElectricStart.transform.position = startPos;
        _currentElectricEnd.transform.position = endPos + (Vector2.left * 2f);
        _currentElectricLine = electricLine;
        _currentLineRenderer = electricLine.GetComponent<LineRenderer>();
    }

    private void PlaceSpinController()
    {
        if (_currentElectricLine == null)
        {
            return;
        }

        if(_currentElectricLine.GetComponent<ElectricLine>().IsElectricLineIntersect())
        {
            _currentLineRenderer.colorGradient = unLinkable;
            return;
        }else
        {
            _currentLineRenderer.colorGradient = linkable;
        }
        GameObject spin = CheckSpinRay();
        if (spin == null)
        {
            return;
        }

        Spin checkingSpin = spin.GetComponent<Spin>();
        if (checkingSpin.IsSpinLinkedTo(_currentSpin)
            || !checkingSpin.IsSpinCanLinkTo() || checkingSpin.IsSameSpinType(_currentSpin.GetComponent<Spin>().GetSpinType()))
        {
            _currentLineRenderer.colorGradient = unLinkable;
            return;
        }
        _currentLineRenderer.colorGradient = linkable;
        Vector2 spinPos = spin.transform.position;
        if (Input.GetMouseButtonDown(0) && spinPos != (Vector2)_currentElectricStart.transform.position)
        {
            PlaceSpinEnd(spinPos, spin);
        }
    }

    private void PlaceSpinEnd(Vector2 pos, GameObject spinToLink)
    {
        if (_currentElectricLine == null)
        {
            return;
        }
        SpinLinker(_currentSpin, spinToLink);
        _currentElectricEnd.transform.position = pos;
        GenerateElectricLineCollider(_currentElectricStart.transform.position
            , _currentElectricCenter.transform.position
            , pos);
        SetDefaultManager();

        if (SpinManager.instance.IsAllSpinLinked())
        {
            GameManager.instance.PlayerWinThisLevel();
        }
    }


    private void SpinLinker(GameObject linkingSpin, GameObject spinToLink)
    {
        if (linkingSpin == null || spinToLink == null)
        {
            return;
        }
        linkingSpin.GetComponent<Spin>().LinkSpin(spinToLink);
        spinToLink.GetComponent<Spin>().LinkSpin(linkingSpin);

    }

    private void UpdateElectricLineEnd()
    {
        if (_currentElectricLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currentElectricEnd.transform.position = mousePos;
            _currentElectricCenter.transform.position = Vector2.Lerp(_currentElectricStart.transform.position, _currentElectricEnd.transform.position, 0.5f);
            GenerateElectricLineCollider(_currentElectricStart.transform.position
                , _currentElectricCenter.transform.position
                , _currentElectricEnd.transform.position);
        }
    }

    private void GenerateElectricLineCollider(Vector2 start, Vector2 center, Vector2 end)
    {
        if (_currentElectricLine == null || _currentLineRenderer == null)
        {
            return;
        }
        SetColliderForElectricLine(start, center, end);
    }

    private void SetColliderForElectricLine(Vector2 start, Vector2 center, Vector2 end)
    {
        _currentElectricCenter.transform.localScale = new Vector2(Vector2.Distance(start, end) * .9f, .02f);
        float angle = Mathf.Atan2(center.y - start.y, center.x - start.x) * Mathf.Rad2Deg;
        _currentElectricCenter.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void SetDefaultManager()
    {
        _currentSpin = null;
        _currentElectricLine = null;
        _currentElectricEnd = null;
        _currentElectricStart = null;
        _currentElectricCenter = null;
        _currentLineRenderer = null;
    }
    #region CheckSpinLine
    private GameObject CheckSpinRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Spin"));
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    
    #endregion
}
