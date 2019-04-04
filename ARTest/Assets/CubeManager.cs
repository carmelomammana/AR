using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public List<GameObject> waypoints;
    public GameObject cube;

    private float timer;
    private float timeToMove;
    // Start is called before the first frame update
    void Start()
    {
        waypoints = GameObject.FindGameObjectsWithTag("Player").ToList();
        timeToMove = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> temp = GameObject.FindGameObjectsWithTag("Player").ToList();

        if (waypoints.Count < temp.Count)
        {
            waypoints = GameObject.FindGameObjectsWithTag("Player").ToList();
        }
        foreach (GameObject waypoint in waypoints)
        {
            timer += Time.deltaTime;
            if (timer > timeToMove)
            {
                cube.transform.position = Vector3.MoveTowards(cube.transform.position, waypoint.transform.position, 1.0f);
                timer = 0;
            }
        }
    }
}
