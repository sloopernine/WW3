using System.Collections.Generic;
using UnityEngine;

namespace OmniWorld
{
    public class Terrain : MonoBehaviour
    {
        public float radius = 22f;
        public float angle = 0.314f;
        public int amount = 20;
        public int segments;
        public float width = 2f;
        
        public Material material;

        [SerializeField] private PathData _pathData;
        [SerializeField] private List<SegmentData> _segmentData = new List<SegmentData>();
        
        private void Start()
        {
            Vector2 position = transform.position;

            _pathData = gameObject.AddComponent<PathData>();
            
            // for (int i = 0; i < amount; i++)
            // {
            //     Vector2 nextPoint = new Vector2(radius * Mathf.Sin(angle * i) + position.x, radius * Mathf.Cos(angle * i) + position.y); 
            //     _pathData.AddPoint(nextPoint);
            // }

            amount = segments;
            
            for (int i = 0; i < amount; i++)
            {
                float rad = Mathf.Deg2Rad * (i * 360f / segments);
                Vector2 newPoint = new Vector2((Mathf.Sin(rad) * radius) + position.x, (Mathf.Cos(rad) * radius) + position.y);
                _pathData.AddPoint(newPoint);
            }

            for (int i = 0; i < _pathData.GetLength(); i++)
            {
                GameObject newSegment = new GameObject("MeshSegment", typeof(SegmentData));
                newSegment.transform.parent = this.transform;
                SegmentData newSegmentData = newSegment.GetComponent<SegmentData>();

                if (i == _pathData.GetLength()-1)
                {
                    newSegmentData.SetStartPoint(_pathData.GetPoint(i), width);
                    newSegmentData.SetEndPoint(_pathData.GetPoint(0), width);
                    _segmentData.Add(newSegmentData);
                }
                else
                {
                    newSegmentData.SetStartPoint(_pathData.GetPoint(i), width);
                    newSegmentData.SetEndPoint(_pathData.GetPoint(i+1), width);
                    _segmentData.Add(newSegmentData);
                }

                newSegmentData.material = material;
            }
            
            for (int i = 0; i < _segmentData.Count; i++)
            {
                Vector2 newPosition = Vector2.zero;
                
                if (i == _pathData.GetLength()-1)
                {
                    newPosition = _segmentData[0].GetVerticePoint(0);
                }
                else
                {
                    newPosition = _segmentData[i + 1].GetVerticePoint(0);
                }
                
                _segmentData[i].ChangeVerticePoint(1, newPosition, true);
            }
            
            for (int i = 0; i < _segmentData.Count; i++)
            {
                Vector2 newPosition = Vector2.zero;
                
                if (i == 0)
                {
                    newPosition = _segmentData[_segmentData.Count-1].GetVerticePoint(1);
                }
                else
                {
                    newPosition = _segmentData[i - 1].GetVerticePoint(1);
                }
                
                _segmentData[i].ChangeVerticePoint(0, newPosition, false);
            }

            foreach (var segment in _segmentData)
            {
                segment.GenerateMesh();
            }
            
            // for (int i = 0; i < amount; i++)
            // {
            //     Vector2 nextPoint = new Vector2(radius * Mathf.Sin(angle * i) + position.x, radius * Mathf.Cos(angle * i) + position.y); 
            //     _pathData.AddPoint(nextPoint);
            // }
        }

        private void FixedUpdate()
        {
            Vector2 lastPoint = Vector2.zero;
        
            for (int i = 0; i < _pathData.GetLength(); i++)
            {
                if (i > 0)
                {
                    Debug.DrawLine(_pathData.GetPoint(i), lastPoint);
                }
                
                if(i == _pathData.GetLength() - 1)
                {
                    Debug.Log("Here");
                    Debug.DrawLine(_pathData.GetPoint(i), _pathData.GetPoint(0));
                }
        
                lastPoint = _pathData.GetPoint(i);
            }
        }
    }
}