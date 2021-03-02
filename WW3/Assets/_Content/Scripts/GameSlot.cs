using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSlot : MonoBehaviour
{
    public TMP_Text gameName;

    public void CopyGameName()
    {
        UniClipboard.SetText(gameName.text);
    }
}
