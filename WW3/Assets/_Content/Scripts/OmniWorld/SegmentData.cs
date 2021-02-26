using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniWorld
{
    public class SegmentData : MonoBehaviour
    {
        public Material material;
        
        [SerializeField] private Vector2 startPoint = Vector2.zero;
        [SerializeField] private Vector2 endPoint = Vector2.zero;
        [SerializeField] private Vector2 direction = Vector2.zero;

        private float startPointWidth;
        private float endPointWidth;
        
        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector2> uv = new List<Vector2>();
        private List<int> triangles = new List<int>();
        
        public void SetStartPoint(Vector2 pointPosition, float pointWidth)
        {
            this.startPoint = pointPosition;
            startPointWidth = pointWidth;
        }

        public void SetEndPoint(Vector2 pointPosition, float pointWidth)
        {
            this.endPoint = pointPosition;
            endPointWidth = pointWidth;
            
            CalculateSegmentMesh();
        }

        public Vector2 GetVerticePoint(int index)
        {
            return vertices[index];
        }

        public void ChangeVerticePoint(int index, Vector2 newPosition, bool divide)
        {
            if(divide)
                newPosition = (vertices[index] + new Vector3(newPosition.x, newPosition.y, 0)) / 2; 
            
            vertices[index] = newPosition;
        }

        private void CalculateSegmentMesh()
        {
            direction = startPoint - endPoint;

            Vector2 normal = OmniLib.Math.Vector2.PerpendicularClockwise(direction).normalized;
            
            vertices.Add(OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(startPoint, normal, new Vector2(startPointWidth, startPointWidth)));
            vertices.Add(OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(endPoint, normal, new Vector2(endPointWidth, endPointWidth)));
            vertices.Add(OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(startPoint, normal, new Vector2(0, 0)));
            vertices.Add(OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(endPoint, normal, new Vector2(0, 0)));
            
            // vertices.Add(OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(startPoint, normal, new Vector2(startPointWidth/2, startPointWidth/2)));
            // vertices.Add(OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(endPoint, normal, new Vector2(endPointWidth/2, endPointWidth/2)));
            // vertices.Add(OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(startPoint, normal, new Vector2(-startPointWidth/2, -startPointWidth/2)));
            // vertices.Add(OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(endPoint, normal, new Vector2(-endPointWidth/2, -endPointWidth/2)));


            // for (int i = 0; i < vertices.Count; i++)
            // {
            //     uv.Add(vertices[i]);
            // }
            
            uv.Add(new Vector2(0,1));
            uv.Add(new Vector2(1,1));
            uv.Add(new Vector2(0,0));
            uv.Add(new Vector2(1,0));
            

            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);
            triangles.Add(2);
            triangles.Add(1);
            triangles.Add(3);
        }

        public void GenerateMesh()
        {
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = triangles.ToArray();

            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            //gameObject.transform.localScale = new Vector3(1, 1, 1);
            gameObject.GetComponent<MeshFilter>().mesh = mesh;
            gameObject.GetComponent<MeshRenderer>().material = material;
        }
    }
}