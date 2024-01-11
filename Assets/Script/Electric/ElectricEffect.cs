using UnityEngine;
using System.Collections.Generic;

public enum ElectricMode
{
    None,
    Random,
    Loop,
    PingPong
}
public class ElectricEffect : MonoBehaviour
{
    public GameObject _startPositionOfElectric;
    public GameObject _endPositionOfElectric;

    [Range(0, 8)]
    public int _numberOfGenericLine = 6;

    [Range(0.01f, 1.0f)]
    public float _existTimeOfElectric = 0.05f;
    private float _timerForElectric;

    [Range(0.0f, 1.0f)]
    public float _messyVolume = 0.15f;
    public bool _defaultModeOfElectric;

    [Range(1, 64)]
    public int _numberOfLine = 1;

    [Range(1, 64)]
    public int _numberOfCollum = 1;
    public ElectricMode _electricMode;
    [System.NonSerialized]
    public System.Random _randomGenerator = new System.Random();

    private LineRenderer _lineRenderer;
    private List<KeyValuePair<Vector3, Vector3>> _segments = new List<KeyValuePair<Vector3, Vector3>>();
    private int _startIndex;
    private Vector2 _size;
    private Vector2[] _offsets;
    private int _animationOffsetIndex;
    private int _animationPingPongDirection = 1;
    private bool _orthoGraphic;

    private void Start()
    {
        _orthoGraphic = (Camera.main != null && Camera.main.orthographic);
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
        UpdateFromMaterialChange();
    }

    private void Update()
    {
        _orthoGraphic = (Camera.main != null && Camera.main.orthographic);
        if (_timerForElectric <= 0.0f)
        {
            if (_defaultModeOfElectric)
            {
                _timerForElectric = _existTimeOfElectric;
                _lineRenderer.positionCount = 0;
            }
        }
        _timerForElectric -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_timerForElectric > 0.0f)
        {
            return;
        }
        
        if(!_defaultModeOfElectric)
        {
            Trigger();
        }
    }

    public void Trigger()
    {
        Vector3 start, end;
        _timerForElectric = _existTimeOfElectric + Mathf.Min(0.0f, _timerForElectric);
        start = _startPositionOfElectric.transform.position;
        end = _endPositionOfElectric.transform.position;
        _startIndex = 0;
        GenerateLightningBolt(start, end, _numberOfGenericLine, 0.0f);
        UpdateLineRenderer();
    }

    private void GetPerpendicularVector(ref Vector3 directionNormalized, out Vector3 side)
    {
        if (directionNormalized == Vector3.zero)
        {
            side = Vector3.right;
        }
        else
        {
            float x = directionNormalized.x;
            float y = directionNormalized.y;
            float z = directionNormalized.z;
            float px, py, pz;
            float ax = Mathf.Abs(x), ay = Mathf.Abs(y), az = Mathf.Abs(z);
            if (ax >= ay && ay >= az)
            {
                py = 1.0f;
                pz = 1.0f;
                px = -(y * py + z * pz) / x;
            }
            else if (ay >= az)
            {
                px = 1.0f;
                pz = 1.0f;
                py = -(x * px + z * pz) / y;
            }
            else
            {
                px = 1.0f;
                py = 1.0f;
                pz = -(x * px + y * py) / z;
            }
            side = new Vector3(px, py, pz).normalized;
        }
    }

    private void GenerateLightningBolt(Vector3 start, Vector3 end, int generation, float offsetAmount)
    {
        if (generation < 0 || generation > 8)
        {
            return;
        }
        else if (_orthoGraphic)
        {
            start.z = end.z = Mathf.Min(start.z, end.z);
        }

        _segments.Add(new KeyValuePair<Vector3, Vector3>(start, end));
        if (generation == 0)
        {
            return;
        }

        Vector3 randomVector;
        if (offsetAmount <= 0.0f)
        {
            offsetAmount = (end - start).magnitude * _messyVolume;
        }

        while (generation-- > 0)
        {
            int previousStartIndex = _startIndex;
            _startIndex = _segments.Count;
            for (int i = previousStartIndex; i < _startIndex; i++)
            {
                start = _segments[i].Key;
                end = _segments[i].Value;
                Vector3 midPoint = (start + end) * 0.5f;
                RandomVector(ref start, ref end, offsetAmount, out randomVector);
                midPoint += randomVector;
                _segments.Add(new KeyValuePair<Vector3, Vector3>(start, midPoint));
                _segments.Add(new KeyValuePair<Vector3, Vector3>(midPoint, end));
            }
            offsetAmount *= 0.5f;
        }
    }

    public void RandomVector(ref Vector3 start, ref Vector3 end, float offsetAmount, out Vector3 result)
    {
        if (_orthoGraphic)
        {
            Vector3 directionNormalized = (end - start).normalized;
            Vector3 side = new Vector3(-directionNormalized.y, directionNormalized.x, directionNormalized.z);
            float distance = ((float)_randomGenerator.NextDouble() * offsetAmount * 2.0f) - offsetAmount;
            result = side * distance;
        }
        else
        {
            Vector3 directionNormalized = (end - start).normalized;
            Vector3 side;
            GetPerpendicularVector(ref directionNormalized, out side);
            float distance = (((float)_randomGenerator.NextDouble() + 0.1f) * offsetAmount);
            float rotationAngle = ((float)_randomGenerator.NextDouble() * 360.0f);
            result = Quaternion.AngleAxis(rotationAngle, directionNormalized) * side * distance;
        }
    }

    private void SelectOffsetFromAnimationMode()
    {
        int index;

        if (_electricMode == ElectricMode.None)
        {
            _lineRenderer.material.mainTextureOffset = _offsets[0];
            return;
        }
        else if (_electricMode == ElectricMode.PingPong)
        {
            index = _animationOffsetIndex;
            _animationOffsetIndex += _animationPingPongDirection;
            if (_animationOffsetIndex >= _offsets.Length)
            {
                _animationOffsetIndex = _offsets.Length - 2;
                _animationPingPongDirection = -1;
            }
            else if (_animationOffsetIndex < 0)
            {
                _animationOffsetIndex = 1;
                _animationPingPongDirection = 1;
            }
        }
        else if (_electricMode == ElectricMode.Loop)
        {
            index = _animationOffsetIndex++;
            if (_animationOffsetIndex >= _offsets.Length)
            {
                _animationOffsetIndex = 0;
            }
        }
        else
        {
            index = _randomGenerator.Next(0, _offsets.Length);
        }

        if (index >= 0 && index < _offsets.Length)
        {
            _lineRenderer.material.mainTextureOffset = _offsets[index];
        }
        else
        {
            _lineRenderer.material.mainTextureOffset = _offsets[0];
        }
    }

    private void UpdateLineRenderer()
    {
        int segmentCount = (_segments.Count - _startIndex) + 1;
        _lineRenderer.positionCount = segmentCount;

        if (segmentCount < 1)
        {
            return;
        }

        int index = 0;
        _lineRenderer.SetPosition(index++, _segments[_startIndex].Key);

        for (int i = _startIndex; i < _segments.Count; i++)
        {
            _lineRenderer.SetPosition(index++, _segments[i].Value);
        }

        _segments.Clear();

        SelectOffsetFromAnimationMode();
    }

    public void UpdateFromMaterialChange()
    {
        _size = new Vector2(1.0f / (float)_numberOfCollum, 1.0f / (float)_numberOfLine);
        _lineRenderer.material.mainTextureScale = _size;
        _offsets = new Vector2[_numberOfLine * _numberOfCollum];
        for (int y = 0; y < _numberOfLine; y++)
        {
            for (int x = 0; x < _numberOfCollum; x++)
            {
                _offsets[x + (y * _numberOfCollum)] = new Vector2((float)x / _numberOfCollum, (float)y / _numberOfLine);
            }
        }
    }
}