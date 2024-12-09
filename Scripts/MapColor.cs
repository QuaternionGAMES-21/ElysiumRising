using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapColor : MonoBehaviour
{
    public GameObject qwe;
    private void OnEnable() {
        Image mapColor = qwe.GetComponent<Image>();
        mapColor.color = new Color(1f, 1f, 1f);
    }
}
