using System;
using UnityEngine;

namespace OmniWorld
{
    public class TestScript : MonoBehaviour
    {
        public GameObject startPointObject;
        public GameObject endPointObject;
        public GameObject offsetPointObject1;
        public GameObject offsetPointObject2;
        public GameObject offsetPointObject3;
        public GameObject offsetPointObject4;

        public Vector2 direction;
        public Vector2 normal;
        
        public Vector2 offsetValue;

        public float dotProduct;

        public Vector2 test;
        private void Start()
        {

        }

        private void Update()
        {
            Vector2 startPosition = startPointObject.transform.position;
            Vector2 endPosition = endPointObject.transform.position;

            direction = startPosition - endPosition;

            normal = OmniLib.Math.Vector2.PerpendicularClockwise(direction).normalized;


            offsetValue = new Vector2(2, 2);

            offsetPointObject1.transform.position =
                OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(startPosition, normal, offsetValue);
            offsetPointObject2.transform.position =
                OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(endPosition, normal, offsetValue);
            
            offsetValue = new Vector2(-2, -2);
            
            offsetPointObject3.transform.position =
                OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(startPosition, normal, offsetValue);
            offsetPointObject4.transform.position =
                OmniLib.Math.Vector2.GetOffsetPositionUsingNormal(endPosition, normal, offsetValue);
            
        }
    }
}