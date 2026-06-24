using DG.Tweening;
using System;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{
    [SerializeField] private RectTransform Tracker;
    [SerializeField] private RectTransform[] _accuracyMarkers;
    [SerializeField] private Vector3[] _startPositionAccuracyMarkers;
    [SerializeField] private Vector3[] _endPositionAccuracyMarkers;
    [SerializeField] private float _moveDistanceAccuracyMarker = 0.1f;
    [SerializeField] private float _durationAccuracyMarker = 1;
    [SerializeField] private Ease _easeType;
    
    private float _accuracy;
    private int _accuracyTrackerCount;
    private bool _isTrackerActive = false;
    private Sequence _sequenceAccuracyMarker;


    public static Action<bool> OnTrackerActiveSwitch;
    public static Action<Vector2> OnTrackerSetPosition;
    public static Action<float> AccuracyGetter;

    private void OnEnable()
    {
        OnTrackerActiveSwitch += TrackerSetActive;
        OnTrackerSetPosition += TrackerSetPosition;
    }
    private void OnDisable()
    {
        OnTrackerActiveSwitch -= TrackerSetActive;
        OnTrackerSetPosition -= TrackerSetPosition;
    }

    private void Start()
    {
        _accuracyTrackerCount = _accuracyMarkers.Length;

        _startPositionAccuracyMarkers = new Vector3[_accuracyTrackerCount];
        _endPositionAccuracyMarkers = new Vector3[_accuracyTrackerCount];

        for (int i = 0; i < _accuracyTrackerCount; i++)
        {
            _startPositionAccuracyMarkers[i] = _accuracyMarkers[i].localPosition;

            _endPositionAccuracyMarkers[i] = _startPositionAccuracyMarkers[i];

            if (_startPositionAccuracyMarkers[i].x > 0)
            {
                _endPositionAccuracyMarkers[i].x -= _moveDistanceAccuracyMarker;
            }
            else
            {
                _endPositionAccuracyMarkers[i].x += _moveDistanceAccuracyMarker;
            }


            if (_startPositionAccuracyMarkers[i].y > 0)
            {
                _endPositionAccuracyMarkers[i].y -= _moveDistanceAccuracyMarker;
            }
            else
            {
                _endPositionAccuracyMarkers[i].y += _moveDistanceAccuracyMarker;
            }
        }
    }

    private void Update()
    {
        if (!_isTrackerActive) return;

        AccuracyGetter.Invoke(_accuracy);

    }

    private void StartMarkerAnimations()
    {
        _sequenceAccuracyMarker?.Kill();
        SetStartPositionAccuracyMarker();
        _sequenceAccuracyMarker = DOTween.Sequence();

        for (int i = 0; i < _accuracyTrackerCount; i++)
        {
            int index = i; 
            Tween tween = null;
            tween = _accuracyMarkers[i]
                .DOLocalMove(_endPositionAccuracyMarkers[i], _durationAccuracyMarker)
                .SetEase(_easeType)
                .OnUpdate(() => 
                {
                    if (tween.ElapsedPercentage() > 0.7f)
                    {
                        OnMarkerReachedEnd(index);
                    }
                });

            _sequenceAccuracyMarker.Append(tween);
        }

        _sequenceAccuracyMarker.AppendInterval(_durationAccuracyMarker);

        for (int i = 0; i < _accuracyTrackerCount; i++)
        {
            Tween tween = null;
            tween = _accuracyMarkers[i]
                .DOLocalMove(_startPositionAccuracyMarkers[i], _durationAccuracyMarker)
                .SetEase(_easeType).OnUpdate(() => 
                {
                    
                    if(tween.ElapsedPercentage() > 0.5f)
                    {
                        OnMarkerReachedEnd(-1);
                    }
                });

            _sequenceAccuracyMarker.Join(tween);
        }
        _sequenceAccuracyMarker.SetLoops(-1, LoopType.Restart);
    }

    private void OnMarkerReachedEnd(int markerIndex)
    {
        switch (markerIndex)
        {
            case 0: _accuracy = 30; break;
            case 1: _accuracy = 50; break;
            case 2: _accuracy = 70; break;
            case 3: _accuracy = 100; break;
            default: _accuracy = 0; break;
        }
    }
    private void SetStartPositionAccuracyMarker()
    {
        for (int i = 0; i < _accuracyTrackerCount; i++)
        {
            _accuracyMarkers[i].localPosition = _startPositionAccuracyMarkers[i];
        }
    }

    private void TrackerSetActive(bool boolean)
    {
        Tracker.gameObject.SetActive(boolean);
        _isTrackerActive = boolean;
        if (boolean)
        {
            StartMarkerAnimations();
        }
        else 
        {
            _sequenceAccuracyMarker.Kill();
            SetStartPositionAccuracyMarker();
        }

    
    }
    private void TrackerSetPosition(Vector2 position)
    {
        Tracker.position = position;
    }
}
