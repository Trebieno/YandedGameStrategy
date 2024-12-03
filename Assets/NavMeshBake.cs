using UnityEngine;
using NavMeshPlus.Components;

public class NavMeshBake : MonoBehaviour
{

    private void Start()
    {
        ObjectStorage.Instance.NavMeshSurface = GetComponent<NavMeshSurface>();
    }
}
