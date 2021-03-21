using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniLib
{
    public static class Tools
    {
        public static float WrapAngle(float angle)
        {
            angle%=360;
            if(angle >180)
                return angle - 360;
 
            return angle;
        }

        // public static float UnwrapAngle(float angle)
        // {
        //     if(angle >=0)
        //         return angle;
        //
        //     angle = -angle%360;
        //
        //     return 360-angle;
        // }

        public static string ToHex(int value)
        {
            return value.ToString("X");
        }

        public static int FromHex(string value)
        {
            return int.Parse(value, System.Globalization.NumberStyles.HexNumber);
        }
        
        public static void CopyToClipboard(this string s)
        {
            TextEditor te = new TextEditor();
            te.text = s;
            te.SelectAll();
            te.Copy();
        }
    }
}

