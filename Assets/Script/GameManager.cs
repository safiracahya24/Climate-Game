using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Pengaturan Halaman Topik")]
    public string halaman_kuis_pertama;
    public string halaman_hasil;


    [Header("Pengaturan Halaman Kuis")]
    public string jawabanBenar;
    public int nilaiJawabanBenar;
    public string halamanSelanjutnya;
    public AudioSource suaraBenar;
    public AudioSource suaraSalah;

    [Header("Pengaturan Halaman Hasil")]
    public Text text_nilai;
    public GameObject[] bintang;
    public int batas_bintang_1;
    public int batas_bintang_2;
    public int batas_bintang_3;

    public int nilai;
    Scene activeScene;
    // Start is called before the first frame update
    void Start()
    {
    // Mengambil nilai dari PlayerPrefs
    nilai = PlayerPrefs.GetInt("nilai");
    
    // Mengambil referensi ke scene aktif
    activeScene = SceneManager.GetActiveScene();
    
    // Jika scene adalah "Topik", set beberapa PlayerPrefs dan reset nilai ke awal
    if(activeScene.name == "Topik")
    {
        PlayerPrefs.SetString("hasil", halaman_hasil);
        PlayerPrefs.SetString("halaman_hasil", halaman_hasil);
        PlayerPrefs.SetString("halaman_kuis", halaman_kuis_pertama);

        // Reset nilai ke awal (misalnya, 0)
        nilai = 0;
        PlayerPrefs.SetInt("nilai", nilai);
        PlayerPrefs.Save();  // Simpan perubahan ke PlayerPrefs
        
    }
    else if(activeScene.name == PlayerPrefs.GetString("halaman_kuis"))
    {
        // Jika di halaman kuis, hapus kunci "nilai" dan log nilai yang ada
        PlayerPrefs.DeleteKey("nilai");
        Debug.Log("Ok" + PlayerPrefs.GetInt("nilai"));

        // Set nilai ke awal setelah menghapus kunci
        nilai = 0;
        PlayerPrefs.SetInt("nilai", nilai);
        PlayerPrefs.Save();  // Simpan perubahan ke PlayerPrefs
    }
    else if (activeScene.name == PlayerPrefs.GetString("halaman_hasil"))
    {    
            if(nilai <= batas_bintang_1)
            {
                bintang[0].SetActive(true);
                bintang[1].SetActive(false);
                bintang[2].SetActive(false);
            }
            else if(nilai <= batas_bintang_2)
            {
                bintang[0].SetActive(true);
                bintang[1].SetActive(true);
                bintang[2].SetActive(false);
            }
            else if(nilai <= batas_bintang_3)
            {
                bintang[0].SetActive(true);
                bintang[1].SetActive(true);
                bintang[2].SetActive(true);
            }
            text_nilai.text = nilai.ToString();
            
        }
    }


    public void PindahHalaman(string halaman)
    {
        SceneManager.LoadScene(halaman);
    }

    public void Buka_Popup(GameObject gameobject)
    {
        gameobject.SetActive(true);
    }

    public void Tutup_Popup(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void Jawaban_User(string jawaban)
    {
        StartCoroutine(Cek_Jawaban(jawaban));
    }

    IEnumerator Cek_Jawaban(string jawaban)
    {
        if (jawabanBenar == jawaban)
        {
            nilai = nilai + nilaiJawabanBenar;
            PlayerPrefs.SetInt("nilai", nilai);
            suaraBenar.Play();
        }
        else
        {
            suaraSalah.Play();
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(halamanSelanjutnya);

    }

    public void Keluar_Aplikasi()
    {
        Application.Quit();
    }
}
