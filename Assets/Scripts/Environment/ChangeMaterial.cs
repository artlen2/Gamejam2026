using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public Material Material1;
    public Material Material2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<MeshRenderer>().material = Material1;
    }

    void ResetMats()
    {
        GetComponent<MeshRenderer>().material = Material1;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            CancelInvoke(); // cancel any invokes that are currently scheduled. we only need one to be active
            Invoke("ResetMats", 10);
            GetComponent<MeshRenderer>().material = Material2;
        }
    }
}
