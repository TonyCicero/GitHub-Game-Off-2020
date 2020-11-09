using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSeq : MonoBehaviour
{
    [SerializeField]
    DialogSys dialog;
    [SerializeField]
    computerAudio cAud;


    [SerializeField]
    Text UItxt;
    [SerializeField]
    Transform target;
    [SerializeField]
    float camSpeed = 1;
    [TextArea(5, 20)]
    [SerializeField]
    string txt2show;
    [SerializeField]
    float distLimit = 50;

    [SerializeField]
    GameObject playerObj;

    IEnumerator textEffect()
    {
        UItxt.text = "";
        for(int i = 0; i < txt2show.Length; i++)
        {
            cAud.playClip(2);
            UItxt.text += txt2show[i];
            yield return new WaitForSeconds(Random.Range(0.05f,.25f));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(textEffect());
    }


    // Update is called once per frame
    void Update()
    {
        float step = camSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        if (Vector3.Distance(transform.position, target.position) < distLimit)
        {
            playerObj.SetActive(true);
            UItxt.text = "";
            dialog.callShowDialog(dialog.Dialogs[0]);
            cAud.playClip(1);
            this.gameObject.SetActive(false);
        }

    }
}
