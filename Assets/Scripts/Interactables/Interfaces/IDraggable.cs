using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDraggable 
{
    public void BeginDrag();
    public void Dragging();
    public void EndDrag();
}
