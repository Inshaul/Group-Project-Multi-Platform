using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellOnCursor : MonoBehaviour {
    RectTransform m_rtransform;
    void Start()
    {
        m_rtransform = GetComponent<RectTransform>();
    }
	void Update () {
        m_rtransform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_rtransform.position.z);
	}
}
