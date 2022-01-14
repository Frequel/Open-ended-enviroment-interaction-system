using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiquidDensityInteractable
{ 
    public float getDensity();
    public void postionLiquidInContainer(setPositionInSpace father_sPiS, LiquidDensityInteractor father_ldi);
}
