using UnityEngine;
using TMPro;

public class BulletCount:MonoBehaviour
{
    //Only for Display and debug this is a bad practice
    public TMP_Text BulletsInWorld;

    private void Update() {
        var bullets = FindObjectsOfType<BulletPooled>();
        if(bullets != null)
            BulletsInWorld.text ="In World = "+bullets.Length.ToString();

            
    }
}
