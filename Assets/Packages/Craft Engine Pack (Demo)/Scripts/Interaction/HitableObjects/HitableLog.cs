using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableLog : HitableObject {

    public int m_hits = 12;//like a health of log
    public uint m_stickCount = 5;//drop count
    public GameObject m_stickPrefab;

    public override void HandleHit(ToolType toolType)
    {
        if (toolType == ToolType.Axe || toolType == ToolType.StoneFragment)
            if (m_audioSource)
                m_audioSource.Play();

        if (toolType == ToolType.Axe)//axe better chops
            m_hits -= 2;
        else if (toolType == ToolType.StoneFragment)
            m_hits -= 1;

        if(m_hits <= 0)
        {
            for (int i = 0; i < m_stickCount; ++i) // instantiate sticks
            {
                Vector2 randomCircle = Random.insideUnitCircle;
                Instantiate(m_stickPrefab, transform.position + new Vector3(randomCircle.x, 0.0f, randomCircle.y), transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
