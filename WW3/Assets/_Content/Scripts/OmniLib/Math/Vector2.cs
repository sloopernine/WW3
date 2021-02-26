using UnityEngine;

namespace OmniLib.Math
{
    public class Vector2 : MonoBehaviour
    {
        ///<summary>
        ///Get normal from direction vector2 clockwise. You might want to normalize the result!
        ///</summary>
        public static UnityEngine.Vector2 PerpendicularClockwise(UnityEngine.Vector2 vector2)
        {
            return new UnityEngine.Vector2(vector2.y, -vector2.x);
        }
        ///<summary>
        ///Get normal from direction vector2 counter clockwise. You might want to normalize the result!
        ///</summary>        
        public static UnityEngine.Vector2 PerpendicularCounterClockwise(UnityEngine.Vector2 vector2)
        {
            return new UnityEngine.Vector2(-vector2.y, vector2.x);
        }
        ///<summary>
        ///Get direction between 2 vector2.
        ///</summary>        
        public static UnityEngine.Vector2 GetDirectionBetween(UnityEngine.Vector2 vector2A, UnityEngine.Vector2 vector2B)
        {
            return vector2A - vector2B;
        }

        public static UnityEngine.Vector2 GetOffsetPositionUsingNormal(UnityEngine.Vector2 position, UnityEngine.Vector2 normal, UnityEngine.Vector2 offset)
        {
            return position + (normal * offset);
        }
        
        ///<summary>
        ///Convert angle value.
        ///</summary>   
        public static float WrapAngle(float angle)
        {
            angle%=360;
            if(angle >180)
                return angle - 360;
 
            return angle;
        }
 
        ///<summary>
        ///Convert angle value.
        ///</summary>   
        public static float UnwrapAngle(float angle)
        {
            if(angle >=0)
                return angle;
        
            angle = -angle%360;
        
            return 360-angle;
        }
    }
}