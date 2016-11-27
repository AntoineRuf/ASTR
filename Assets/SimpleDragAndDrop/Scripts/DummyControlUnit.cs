using UnityEngine;
using System.Collections;

/// <summary>
/// Example of control unit for drag and drop events handle
/// </summary>
public class DummyControlUnit : MonoBehaviour
{
    void OnItemPlace(DragAndDropCell.DropDescriptor desc)
    {
        DummyControlUnit sourceSheet = desc.sourceCell.GetComponentInParent<DummyControlUnit>();
        DummyControlUnit destinationSheet = desc.destinationCell.GetComponentInParent<DummyControlUnit>();
        // If item dropped between different sheets
        if (destinationSheet != sourceSheet)
        {
            Debug.Log(desc.item.name + " is dropped from " + sourceSheet.name + " to " + destinationSheet.name);
        }
        // Delete double items
        DragAndDropCell[] liste_cell = destinationSheet.GetComponentsInChildren<DragAndDropCell>();
        foreach (DragAndDropCell c in liste_cell)
        {
            DragAndDropItem[] i = c.GetComponentsInChildren<DragAndDropItem>();
            if (i.Length>0)
            {
                if((i[0].name==desc.item.name) && (c.name !=desc.destinationCell.name))
                {
                    Debug.Log(desc.item.name + " de " + desc.destinationCell.name +" n'est pas le meme que " + i[0].name +" de " + c.name);
                    c.RemoveItem();
                }
            }
        }
    }
}
