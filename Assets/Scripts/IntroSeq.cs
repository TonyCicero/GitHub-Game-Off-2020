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
    Image Fade;
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

    IEnumerator ScreenFade(float speed = 2f, int n=1)
    {
        float t = 0;
        float a = 0;
        float b = 1;
        int count = 0;
        while (count < n) {
            if (t >= 1)
            {
                t = 0;
                float tmp = a;
                a = b;
                b = tmp;
                count++;
            }
            t += speed * Time.deltaTime;
            Fade.color = new Color(0, 0, 0, Mathf.Lerp(a, b, t));
            yield return null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        float step = camSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        if (Vector3.Distance(transform.position, target.position) < distLimit+50)
        {
            StartCoroutine(ScreenFade());
        }

        if (Vector3.Distance(transform.position, target.position) < distLimit)
        {
            playerObj.SetActive(true);
            UItxt.text = "";
            dialog.callShowDialog(dialog.Dialogs[0]);
            cAud.playClip(1);
            Fade.color = new Color(0, 0, 0, 0);
            this.gameObject.SetActive(false);
        }

    }
}
