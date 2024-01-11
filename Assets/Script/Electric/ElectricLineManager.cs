using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLineManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _electricLinePrefab;
    [SerializeField]
    private Transform _electricLineContainer;
    private GameObject _currentElectricLine;
    private GameObject _currentElectricStart;
    private GameObject _currentElectricEnd;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _currentElectricLine == null)
        {
            GameObject spin = CheckSpinRay();
            if (spin == null)
            {
                return;
            }
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

        _currentElectricStart.transform.position = startPos;
        _currentElectricEnd.transform.position = endPos + (Vector2.left * 2f);
        _currentElectricLine = electricLine;
    }

    private void PlaceSpinController()
    {
        if (_currentElectricLine == null)
        {
            return;
        }
        GameObject spin = CheckSpinRay();
        if(spin == null)
        {
            return;
        }
        Vector2 spinPos = spin.transform.position;
        if (Input.GetMouseButtonDown(0) && spinPos != (Vector2)_currentElectricStart.transform.position)
        {
            PlaceSpinEnd(spinPos);
        }
    }

    private void PlaceSpinEnd(Vector2 pos)
    {
        if (_currentElectricLine == null)
        {
            return;
        }
        _currentElectricLine.transform.GetChild(1).position = pos;
        _currentElectricLine = null;
        _currentElectricEnd = null;
        _currentElectricStart = null;
    }

    private void UpdateElectricLineEnd()
    {
        if(_currentElectricLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currentElectricEnd.transform.position = mousePos;
        }
    }

    private GameObject CheckSpinRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }

        return null;
    }
}
