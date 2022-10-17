using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        HeroController hero = collider.gameObject.GetComponent<HeroController>();
        if (hero)
        {
            hero.TurnOffSprings();

            var springJoints = hero.GetComponents<SpringJoint2D>();
            for (int i = 0; i < springJoints.Length; i++)
            {
                Destroy(springJoints[i]);
            }
            hero.enabled = false;

            Destroy(hero.GetComponent<Rigidbody2D>());

            TitleScript title = GameObject.FindObjectOfType<TitleScript>();
            title.ShowMenu("YOU LOSE", Color.red);
        }
    }
}
