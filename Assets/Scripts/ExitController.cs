using UnityEngine;
using System.Collections;

public class ExitController : MonoBehaviour {
    
    private bool exitHit = false;

	void OnTriggerEnter2D(Collider2D collider)
    {
        if (!exitHit)
        {
            exitHit = true;

            HeroController hero = collider.gameObject.GetComponent<HeroController>();
            hero.TurnOffSprings();

            var springJoints = hero.GetComponents<SpringJoint2D>();
            for (int i = 0; i < springJoints.Length; i++)
            {
                Destroy(springJoints[i]);
            }
            hero.enabled = false;
            
            Destroy(hero.GetComponent<Rigidbody2D>());
            StartCoroutine("MoveToCenter", hero.transform);
        }
    }

    IEnumerator MoveToCenter(Transform target) 
    {
        Vector3 startPosition = target.position;
        for (int i = 0; i < 50; i++) 
        {
            target.position = Vector3.Lerp(startPosition, transform.position, i * 0.02f);
            yield return new WaitForSeconds(0.02f);
        }

        TitleScript title = GameObject.FindObjectOfType<TitleScript>();
        title.ShowMenu("YOU WIN!", new Color(40/255f, 1f, 0));
    }
}
