using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableRock : HitableObject {
    public int m_hits = 4;
    public GameObject m_fragmentPrefab;
    public override void HandleHit(ToolType toolType)
    {
        if (toolType == ToolType.Hammer || toolType == ToolType.Rock)
            if (m_audioSource)
                m_audioSource.Play();

        if (toolType == ToolType.Hammer) // hammer better hits
            m_hits -= 2;
        else if (toolType == ToolType.Rock)
            m_hits -= 1;
        if (m_hits <= 0)
        {// instantiate two stone fragments and destroy self
            Instantiate(m_fragmentPrefab, transform.position, transform.rotation);
            Instantiate(m_fragmentPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
