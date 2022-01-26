using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiquidDensityInteractable
{ 
    public float getDensity();
    public void postionLiquidInContainer(setPositionOnZ father_sPoZ, LiquidDensityInteractor father_ldi);
}
