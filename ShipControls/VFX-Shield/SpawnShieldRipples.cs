using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SpawnShieldRipples : MonoBehaviour
{

    public GameObject shieldRipples;
    public GameObject shieldDown;
    private VisualEffect shieldRipplesVFX;
    private VisualEffect shieldDownVFX;
    private Collision collision;

    private void OnCollisionEnter(Collision c)
    {
       // collision = c;
        if (c.gameObject.tag == "Player Bullet")
        {
            collision = c;
            var ripples = Instantiate(shieldRipples, transform) as GameObject;
            shieldRipplesVFX = ripples.GetComponent<VisualEffect>();
            shieldRipplesVFX.SetVector3("SphereCenter", c.contacts[0].point);

            Destroy(ripples, 2);
        }
    }
    public void ShieldsDown()
    {
        var down = Instantiate(shieldDown, transform) as GameObject;
       // shieldDownVFX = down.GetComponent<VisualEffect>();
       // shieldDownVFX.SetVector3("SphereCenter", collision.contacts[0].point);
       

        Destroy(down, 4);
    }
}

