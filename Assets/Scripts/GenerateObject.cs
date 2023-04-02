using UnityEngine;

public class GenerateObject : MonoBehaviour
{
    [SerializeField] private GameObject treePref;
    [SerializeField] private GameObject mountPref;
    [SerializeField] private GameObject hightMountPref;
    [SerializeField] private GameObject snowPeakPref;
    [SerializeField] Buildings _buildings;

    public GameObject ObjectSpawner(int x, int z, TypeOfSpawn spawn)
    {
        
        if (spawn == TypeOfSpawn.Tree)
            return Instantiate(treePref, new Vector3(x, 2, z), Quaternion.identity);
        else if (spawn == TypeOfSpawn.Mount)
            return Instantiate(mountPref, new Vector3(x, 2, z), Quaternion.identity);
        else if (spawn == TypeOfSpawn.HightMount)
            return Instantiate(hightMountPref, new Vector3(x, 2.5f, z), Quaternion.identity);
        else if (spawn == TypeOfSpawn.Snow)
            return Instantiate(snowPeakPref, new Vector3(x, 1.5f, z), Quaternion.identity);
            
        return null;
    }
}
