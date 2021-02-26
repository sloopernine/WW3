using System.Collections.Generic;
using UnityEngine;

namespace OmniWorld
{
    public class PathData : MonoBehaviour
    {
        [SerializeField] private List<Vector2> pointData = new List<Vector2>();

        public void AddPoint(Vector2 pointPosition)
        {
            pointData.Add(pointPosition);
        }

        public int GetLength()
        {
            return pointData.Count;
        }

        public Vector2 GetPoint(int index)
        {
            return pointData[index];
        }

        public void DeletePoint(int index)
        {
            pointData.RemoveAt(index);
        }

        public void DeletePoint(Vector2 vector)
        {
            for (int i = 0; i < pointData.Count; i++)
            {
                if(vector == pointData[i])
                    pointData.RemoveAt(i);
            }
        }
    }
}