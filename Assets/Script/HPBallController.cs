using UnityEngine;
using UnityEngine.UI;

public class HPBallController : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField] private float force;

    public Text damageText;
    public Text roundText;
    private int currentRound = 1;
    private int currentDamage = -30;
    private int initialDamage = -30;
    private bool isDeathBall = false;

    AudioSource audio;
    public AudioClip hitSound;
    public AudioClip whistleSound;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        UpdateBallSpeed();
        force = GameSettings.instance.ballSpeed;
        audio = GetComponent<AudioSource>();
        currentDamage = initialDamage;
        UpdateUI();
        LaunchBall();
        audio.PlayOneShot(whistleSound);
    }

    public void UpdateBallSpeed()
    {
        force = GameSettings.instance.ballSpeed;
        // If ball is moving, update its velocity to maintain direction but change speed
        if (rigid.velocity != Vector2.zero)
        {
            Vector2 direction = rigid.velocity.normalized;
            rigid.velocity = direction * force;
        }
    }

    void UpdateUI()
    {
        damageText.text = "Damage: " + currentDamage.ToString();
        roundText.text = "Round: " + currentRound.ToString();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        audio.PlayOneShot(hitSound);

        if (coll.gameObject.name == "TepiKanan" || coll.gameObject.name == "TepiKiri")
        {
            HPPaddleController hitPaddle = coll.gameObject.name == "TepiKanan" ?
                GameObject.Find("Pemukul2").GetComponent<HPPaddleController>() :
                GameObject.Find("Pemukul1").GetComponent<HPPaddleController>();

            HPPaddleController otherPaddle = coll.gameObject.name == "TepiKanan" ?
                GameObject.Find("Pemukul1").GetComponent<HPPaddleController>() :
                GameObject.Find("Pemukul2").GetComponent<HPPaddleController>();

            hitPaddle.TakeDamage(currentDamage);

            if (!isDeathBall && currentRound < 21)
            {
                otherPaddle.HealDamage(Mathf.Abs(currentDamage / 2));
            }

            currentRound++;
            currentDamage = initialDamage;
            ResetBall();

            if (currentRound == 21)
            {
                isDeathBall = true;
                currentDamage = -1000;
            }

            UpdateUI();
        }

        if ((coll.gameObject.name == "Pemukul1" || coll.gameObject.name == "Pemukul2") && !isDeathBall)
        {
            if (Mathf.Abs(currentDamage) < 300)
            {
                currentDamage -= 10;
            }

            float sudut = (transform.position.y - coll.transform.position.y) * 5f;
            Vector2 arah = new Vector2(rigid.velocity.x, sudut).normalized;
            rigid.velocity = new Vector2(0, 0);
            rigid.AddForce(arah * force);

            UpdateUI();
        }
    }

    void ResetBall()
    {
        transform.localPosition = new Vector3(0, 0);
        rigid.velocity = new Vector2(0, 0);
        LaunchBall();
    }

    void LaunchBall()
    {
        float xPos = Random.Range(0, 2) == 0 ? -1 : 1;
        float yPos = Random.Range(-0.5f, 0.5f);
        Vector2 direction = new Vector2(xPos, yPos).normalized;
        rigid.AddForce(direction * force);
    }
}
