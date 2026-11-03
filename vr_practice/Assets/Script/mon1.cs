using UnityEngine;

public class mon1 : MonoBehaviour
{
    public Mon m;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print("Собщение от mon1");
        print ( m.GetName());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
