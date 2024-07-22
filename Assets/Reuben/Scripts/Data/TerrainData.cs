using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reuben/TerrainData", menuName = "Reuben/TerrainData")]
public class TerrainData : UpdateableData {
    public float uniformScale = 5f;
    public bool useFalloff;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public float minHeight {
        get {
            return uniformScale * meshHeightMultiplier * meshHeightCurve.Evaluate(0);
        }
    }
    
    public float maxHeight {
        get { 
            return uniformScale * meshHeightCurve.Evaluate(1);
        }
    }
}
