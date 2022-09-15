using UnityEngine;
using UnityEngine.Playables;

public class Cutscene_controller : MonoBehaviour
{
    PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            director.Play();
        }


    }
}
