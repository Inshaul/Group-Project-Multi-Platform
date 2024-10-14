using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitableStone : HitableObject {

    public int m_rocks = 10;// count of rocks that are in stone
    public int m_hitsPerRock = 100;
    public int m_hammerHitValue = 10;
    public int m_rockHitValue = 5;
    public Transform m_rockSpawn;
    public GameObject m_rockPrefab;

    int m_hits = 0;
    public override void HandleHit(ToolType toolType)
    {
        if (toolType == ToolType.Hammer || toolType == ToolType.Rock)
            if (m_audioSource)
                m_audioSource.Play();

        if (toolType == ToolType.Hammer)//hammer hits better
            m_hits += m_hammerHitValue;
        else if (toolType == ToolType.Rock)
            m_hits += m_rockHitValue;
        if(m_hits >= m_hitsPerRock)
        {
            if(m_rocks == 2)//last hit -> instantiate 2 rocks and destroy self
            {
                Instantiate(m_rockPrefab, m_rockSpawn.position, m_rockSpawn.rotation);
                Instantiate(m_rockPrefab, m_rockSpawn.position, m_rockSpawn.rotation);
                Destroy(gameObject);
                return;
            }
            Instantiate(m_rockPrefab, m_rockSpawn.position, m_rockSpawn.rotation);
            m_hits = 0;
            --m_rocks;
        }
    }
}
