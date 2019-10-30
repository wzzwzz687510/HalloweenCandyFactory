using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    public Transform selector;
    public Transform[] selectSlots;

    public AudioClip select;
    public AudioClip click;

    protected AudioSource m_as;
    private AsyncOperation ao;
    private int selectIndex = 0;

    private void Awake()
    {
        m_as = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (selectSlots.Length == 0)
            Debug.LogError("Slot null");
        StartCoroutine(AsyncLoadScene());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Vertical")) {
            m_as.PlayOneShot(select);
            if (Input.GetAxisRaw("Vertical") > 0) {
                selectIndex = Mathf.Max(selectIndex - 1, 0);              
            }
            else {
                selectIndex = Mathf.Min(selectIndex + 1, selectSlots.Length - 1);
            }
            selector.SetParent(selectSlots[selectIndex]);
            selector.localPosition = Vector3.zero;
        }

        if (Input.GetButtonDown("Fire3")) {
            m_as.PlayOneShot(click);
            if (selectIndex == 0)
                Button_Start();
            else
                Button_Quit();
        }
    }

    private void FixedUpdate()
    {
        selector.Rotate(Vector3.right * Time.deltaTime * 180);
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

    public void Button_Start()
    {
        ao.allowSceneActivation = true;
    }

    private IEnumerator AsyncLoadScene()
    {
        ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;

        while (!ao.isDone) {
            yield return null;
        }
    }
}
