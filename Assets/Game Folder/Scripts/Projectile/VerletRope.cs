using UnityEngine;
using UnityEngine.UIElements;

public class VerletRope : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    public Transform _spaceshipAnchorTransform{private get; set;}

    private  struct RopeSegment
    {
        public Vector3 CurrentPosition;
        public Vector3 PreviousPosition;

        public RopeSegment(Vector3 position)
        {
            this.CurrentPosition = position;
            this.PreviousPosition = position;
        }
    }

    private RopeSegment[] _ropeSegmentsArray;
    [SerializeField] private int _numberOfRopeSegments = 50;
    [SerializeField] private float _lengthOfRopeSegment = 0.25f;
    [SerializeField] private Vector3 _tensionVector = new Vector3(0, -2, 0);
    [SerializeField] private int _numberOfConstraintRuns = 50;

    private Vector3[] _segmentPositions;

   void Start()
    {
        _lineRenderer.enabled = true;
        _ropeSegmentsArray = new RopeSegment[_numberOfRopeSegments];
        _segmentPositions = new Vector3[_numberOfRopeSegments];
        _lineRenderer.positionCount = _numberOfRopeSegments;

        VerletRopeSetup();
    }
    private void LateUpdate()
    {     
        DrawVerletRope();
    }
    

    private void FixedUpdate()
    {
        SimulateVerletRope();
    }

    private void VerletRopeSetup()
    {
        Vector3 startPosition = transform.position;

        for (int i = 0; i < _numberOfRopeSegments; i++)
        {
            _ropeSegmentsArray[i] = new RopeSegment(startPosition);
            startPosition.y -= _lengthOfRopeSegment;
        }
    }
    private void DrawVerletRope()
    {
        for (int i = 0; i < _numberOfRopeSegments; i++)
        {
            _segmentPositions[i] = _ropeSegmentsArray[i].CurrentPosition;
        }

        _lineRenderer.SetPositions(_segmentPositions);
    }

    private void SimulateVerletRope()
    {
        for (int i = 0; i < _numberOfRopeSegments; i++)
        {
            RopeSegment segment = _ropeSegmentsArray[i];
            Vector3 difference = segment.CurrentPosition - segment.PreviousPosition;
            segment.PreviousPosition = segment.CurrentPosition;
            segment.CurrentPosition += difference;
            segment.CurrentPosition += _tensionVector * Time.fixedDeltaTime;
            _ropeSegmentsArray[i] = segment;
        }

        for (int i = 0; i < _numberOfConstraintRuns; i++)
        {
            VerletConstraints();
        }
    }

    private void VerletConstraints()
    {
        RopeSegment _asteroidAnchor = _ropeSegmentsArray[0];
        _asteroidAnchor.CurrentPosition = transform.position;
        _ropeSegmentsArray[0] = _asteroidAnchor;

        RopeSegment _spaceshipAnchor = _ropeSegmentsArray[_numberOfRopeSegments - 1];
        _spaceshipAnchor.CurrentPosition = _spaceshipAnchorTransform.position;
        _ropeSegmentsArray[_numberOfRopeSegments - 1] = _spaceshipAnchor;

        for (int i = 0;i < _numberOfRopeSegments - 1; i++)
        {
            RopeSegment _nRopeSegment = this._ropeSegmentsArray[i];
            RopeSegment _n1RopeSegment = this._ropeSegmentsArray[i + 1];

            Vector3 _differenceVectorBetweenSegments = _nRopeSegment.CurrentPosition - _n1RopeSegment.CurrentPosition;

            float Distance = _differenceVectorBetweenSegments.magnitude;

            float Difference = Distance - _lengthOfRopeSegment;

            Vector3 Change_Vector = _differenceVectorBetweenSegments.normalized * Difference;

            if(i != 0)
            {
                _nRopeSegment.CurrentPosition -= Change_Vector * 0.5f;
                _n1RopeSegment.CurrentPosition += Change_Vector * 0.5f;
            }
            else
            {
                _n1RopeSegment.CurrentPosition += Change_Vector;
            }

            _ropeSegmentsArray[i] = _nRopeSegment;
            _ropeSegmentsArray[i + 1] = _n1RopeSegment;

        }
    }

}
