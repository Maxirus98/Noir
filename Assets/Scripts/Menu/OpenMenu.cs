using UnityEngine;
using UnityEngine.Rendering;

public class OpenMenu : MonoBehaviour
{
    public GameObject menucanva; //canve menu
    public GameObject tuto; //canva tutoriel
    public GameObject cont; //canva controles
    public GameObject loup; //canva loupe
    public GameObject note;  //canva note
    public GameObject enq; //canva Résoudre enquête
    public void Menu()
    {
        menucanva.SetActive(true);
    }

    public void Tuto() 
    {
        tuto.SetActive(true);
    }

    public void Cont()
    {
        cont.SetActive(true);
    }

    public void Loupe() 
    {
        loup.SetActive(true);
    }

    public void Notes() 
    {
        note.SetActive(true);
    }

    public void Enq() 
    {
        enq.SetActive(true);
    }
}

