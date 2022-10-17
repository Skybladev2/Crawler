using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;
using System;

public class HeroController : MonoBehaviour
{
    private const int MAX_DISTANCE = 40;
    public SpringController[] SpringControllers;
    public LayerMask defaultLayerMask;
    public LayerMask springLayerMask;
    public bool clickHandled = false;

    // Use this for initialization
    void Start()
    {
        Analytics.CustomEvent("GameStart", null);

        EjectSpring(new Vector2(10, 10));
        EjectSpring(new Vector2(-1, 1));
        EjectSpring(new Vector2(0, -1));
    }

    private void EjectSpring(Vector2 direction)
    {
        SpringController freeSpringController = GetFreeSpringController();

        if (freeSpringController != null)
        {
            TryEjectSpring(freeSpringController, direction);
        }
    }

    private SpringController GetFreeSpringController()
    {
        for (int i = 0; i < SpringControllers.Length; i++)
        {
            if (!SpringControllers[i].SpringJoint.enabled)
                return SpringControllers[i];
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Analytics.CustomEvent("GameQuit", null);
            Application.Quit();
        }

        SpringController freeSpringController = GetFreeSpringController();

        if (Input.GetMouseButtonUp(0))
        {
            ResetHandleClick();
        }

        if (clickHandled)
        {
            return;
        }

        if (freeSpringController == null)
        {
            if (!Input.GetMouseButton(0))
            {
                return;
            }

            if (!TryRetractSpring())
            {
                return;
            }

            HandleClick();
        }
        else
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            TryEjectSpring(freeSpringController, direction);
            HandleClick();
        }
    }

    private void ResetHandleClick()
    {
        clickHandled = false;
    }

    private void HandleClick()
    {
        clickHandled = true;
    }

    private bool TryRetractSpring()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, springLayerMask);

        if (hit.collider)
        {
            hit.collider.transform.parent.GetComponent<SpringController>().RetractSpring();
            return true;
        }

        return false;
    }

    private void TryEjectSpring(SpringController freeSpringController, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, HeroController.MAX_DISTANCE, defaultLayerMask);

        if (hit.collider)
        {
            freeSpringController.EjectSpring(hit.point);
        }
    }

    public void TurnOffSprings()
    {
        for (int i = 0; i < SpringControllers.Length; i++)
        {
            SpringControllers[i].gameObject.SetActive(false);
        }
    }
}
