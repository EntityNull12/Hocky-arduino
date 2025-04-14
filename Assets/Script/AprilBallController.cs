// AprilBallController.cs
using UnityEngine;
using UnityEngine.UI;

public class AprilBallController : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField] private float force;
    private int scoreP1;
    private int scoreP2;
    private int targetScore; // Skor yang harus dicapai untuk menang

    GameObject panelSelesai;
    Text txPemenang;
    AudioSource audio;
    public AudioClip hitSound;
    public AudioClip whistleSound;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        force = GameSettings.instance.ballSpeed;

        // Set target score random antara 5-15
        targetScore = Random.Range(5, 16);
        Debug.Log($"Target Score: {targetScore}");

        scoreP1 = 0;
        scoreP2 = 0;

        panelSelesai = GameObject.Find("PanelSelesai");
        panelSelesai.SetActive(false);
        audio = GetComponent<AudioSource>();

        Vector2 arah = new Vector2(2, 0).normalized;
        rigid.AddForce(arah * force);

        audio.PlayOneShot(whistleSound);

        // Atur ulang paddle settings untuk kedua pemain
        RefreshPaddleSettings("Pemukul1");
        RefreshPaddleSettings("Pemukul2");
    }

    void RefreshPaddleSettings(string paddleName)
    {
        GameObject paddle = GameObject.Find(paddleName);
        if (paddle != null)
        {
            // Atur kecepatan paddle
            float[] speedOptions = { 2f, 5f, 10f, 15f, 20f }; // terlalu lambat -> sangat cepat
            float selectedSpeed = speedOptions[Random.Range(0, speedOptions.Length)];

            // Atur skala paddle (panjang)
            float[] scaleOptions = { 4.5f, 3f, 2f }; // normal, pendek, sangat pendek
            float selectedScale = scaleOptions[Random.Range(0, scaleOptions.Length)];

            AprilPaddleController controller = paddle.GetComponent<AprilPaddleController>();
            if (controller != null)
            {
                controller.SetPaddleSpeed(selectedSpeed);
                paddle.transform.localScale = new Vector3(
                    paddle.transform.localScale.x,
                    selectedScale,
                    paddle.transform.localScale.z
                );
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        audio.PlayOneShot(hitSound);

        if (coll.gameObject.name == "TepiKanan")
        {
            scoreP1 += 1;
            if (scoreP1 >= targetScore)
            {
                EndGame("Player Kiri");
                return;
            }
            ResetBall();
            RefreshPaddleSettings("Pemukul1");
            RefreshPaddleSettings("Pemukul2");
            Vector2 arah = new Vector2(2, 0).normalized;
            rigid.AddForce(arah * force);
        }

        if (coll.gameObject.name == "TepiKiri")
        {
            scoreP2 += 1;
            if (scoreP2 >= targetScore)
            {
                EndGame("Player Kanan");
                return;
            }
            ResetBall();
            RefreshPaddleSettings("Pemukul1");
            RefreshPaddleSettings("Pemukul2");
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

    void EndGame(string winner)
    {
        panelSelesai.SetActive(true);
        txPemenang = GameObject.Find("Pemenang").GetComponent<Text>();
        txPemenang.text = winner + " Menang!";
        audio.PlayOneShot(whistleSound);
        Destroy(gameObject);
    }
}
