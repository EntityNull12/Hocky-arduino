using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public int force;
    Rigidbody2D rigid;
    int scoreP1;
    int scoreP2;
    Text scoreUIP1;
    Text scoreUIP2;

    GameObject panelSelesai;
    Text txPemenang;
    AudioSource audio;
    public AudioClip hitSound;
    public AudioClip whistleSound; // Whistle sound

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Vector2 arah = new Vector2(2, 0).normalized;
        rigid.AddForce(arah * force);
        scoreP1 = 0;
        scoreP2 = 0;
        scoreUIP1 = GameObject.Find("Score1").GetComponent<Text>();
        scoreUIP2 = GameObject.Find("Score2").GetComponent<Text>();

        panelSelesai = GameObject.Find("PanelSelesai");
        panelSelesai.SetActive(false);
        audio = GetComponent<AudioSource>();

        audio.PlayOneShot(whistleSound); // Play whistle sound at the start
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        audio.PlayOneShot(hitSound);
        if (coll.gameObject.name == "TepiKanan")
        {
            scoreP1 += 1;
            TampilkanScore();
            if (scoreP1 == 7)
            {
                EndGame("Player Merah Pemenang");
                return;
            }
            ResetBall();
            Vector2 arah = new Vector2(2, 0).normalized;
            rigid.AddForce(arah * force);
        }
        if (coll.gameObject.name == "TepiKiri")
        {
            scoreP2 += 1;
            TampilkanScore();
            if (scoreP2 == 7)
            {
                EndGame("Player Kuning Pemenang");
                return;
            }
            ResetBall();
            Vector2 arah = new Vector2(-2, 0).normalized;
            rigid.AddForce(arah * force);
        }

        if (coll.gameObject.name == "Pemukul1" || coll.gameObject.name == "Pemukul2")
        {
            float sudut = (transform.position.y - coll.transform.position.y) * 5f;
            Vector2 arah = new Vector2(rigid.velocity.x, sudut).normalized;
            rigid.velocity = new Vector2(0, 0);
            rigid.AddForce(arah * force * 2);
        }
    }

    void ResetBall()
    {
        transform.localPosition = new Vector3(0, 0);
        rigid.velocity = new Vector2(0, 0);
    }

    void TampilkanScore()
    {
        Debug.Log("Score P1 : " + scoreP1 + " Score P2 :  " + scoreP2);
        scoreUIP1.text = scoreP1.ToString();
        scoreUIP2.text = scoreP2.ToString();
    }

    void EndGame(string winner)
    {
        panelSelesai.SetActive(true);
        txPemenang = GameObject.Find("Pemenang").GetComponent<Text>();
        txPemenang.text = winner;
        audio.PlayOneShot(whistleSound); // Play whistle sound at the end
        Destroy(gameObject);
    }
}
