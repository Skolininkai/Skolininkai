using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    public void ClearHighlight()
    {
        if (highlight != null)
        {
            Outline outline = highlight.gameObject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            highlight = null;
        }
    }

    

    void Update()
    {
        // Clear previous highlight
        ClearHighlight();

        // Create a ray from the center of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, 4.5f))
        {
            highlight = raycastHit.transform;
            if (highlight != null && highlight.CompareTag("canPickUp") && highlight != selection)
            {
                Outline outline = highlight.gameObject.GetComponent<Outline>();
                if (outline == null)
                {
                    outline = highlight.gameObject.AddComponent<Outline>();
                }
                outline.enabled = true;
            }
            else
            {
                highlight = null;
            }
        }

        // Selection
        /*if (Input.GetMouseButtonDown(0))
        {
            if (highlight != null)
            {
                if (selection != null)
                {
                    Outline selectionOutline = selection.gameObject.GetComponent<Outline>();
                    if (selectionOutline != null)
                    {
                        selectionOutline.enabled = false;
                    }
                }
                selection = raycastHit.transform;
                Outline newOutline = selection.gameObject.GetComponent<Outline>();
                if (newOutline != null)
                {
                    newOutline.enabled = true;
                }
                highlight = null;
            }
            else
            {
                if (selection != null)
                {
                    Outline selectionOutline = selection.gameObject.GetComponent<Outline>();
                    if (selectionOutline != null)
                    {
                        selectionOutline.enabled = false;
                    }
                    selection = null;
                }
            }
        }*/
    }
}