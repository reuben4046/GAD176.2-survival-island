using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reuben/TerrainData", menuName = "Reuben/TerrainData")]
public class TerrainData : ScriptableObject {
    public float uniformScale = 5f;
    public bool useFalloff;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

}
