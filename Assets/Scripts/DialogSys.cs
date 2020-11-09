using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSys : MonoBehaviour
{
    [SerializeField]
    GameObject DialogBox;
    [SerializeField]
    Text DialogText;

    [System.Serializable]
    public class dialog
    {
        [TextArea(5, 20)]
        public string dText;
        public float duration;
        public int next; // index of next dialog. -1 to end sequence
    }

    [SerializeField]
    public dialog[] Dialogs;

    IEnumerator ShowDialog(dialog d)
    {
        DialogBox.SetActive(true);
        DialogText.text = d.dText;
        yield return new WaitForSeconds(d.duration);
        if (d.next == -1)
        {
            DialogBox.SetActive(false);
        }
        else
        {
            callShowDialog(Dialogs[d.next]);
        }
    }

    public void callShowDialog(dialog d)
    {
        StartCoroutine(ShowDialog(d));
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
