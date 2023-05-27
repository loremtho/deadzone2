using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


namespace TempleRun.Player {
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        // �ʱ� �ӵ�, �ִ� �ӵ�, �ӵ� ���� ����, ���� ����, �ʱ� �߷� ��, �ٴ� ���̾�, ȸ�� ���̾� ���� ������ ����.
        [SerializeField]
        public float initialPlayerSpeed = 4f;
        [SerializeField]
        private float maximumPlayerSpeed = 30f;
        [SerializeField]
        private float playerSpeedIncreaseRate = .1f;
        [SerializeField]
        private float jumpHeight = 1.0f;
        [SerializeField]
        private float initialGravityValue = -9.8f;
        [SerializeField]
        private LayerMask groundLayer;
        [SerializeField]
        private LayerMask turnLayer;
        [SerializeField]
        private LayerMask obstacleLayer;
        [SerializeField]
        private AnimationClip slideAnimationClip;
        [SerializeField]
        private AnimationClip jumpClip;

        // �÷��̾��� �ӵ�, �߷�, �����̴� ����, �Է� ���� ���� ���� ����.
        [SerializeField]
        public float playerSpeed;

        [SerializeField]
        private float scoreMultiplier = 10f;

        // ȸ�� �̺�Ʈ�� ó���ϴ� ������ ����
        [SerializeField]
        private UnityEvent<Vector3> turnEvent;

        [SerializeField]
        private UnityEvent<int> gameOverEvent;

        [SerializeField]
        private UnityEvent<int> scoreUpdateEvent;

        private InterstitialAd interstitialAd;



        public float distanceTraveled = 0f;  //�̵� ���� ����
        public Vector3 lastPosition;
        public float destinationDistance = 15000f;
        public Slider distanceSlider;

        public int maxHealth = 5; // �ִ� ü��
        public int currentHealth; // ���� ü��
        //public Text healthText;       // ü���� ǥ���� Text UI ���
        public TextMeshProUGUI healthText;
        public Slider healthSlider;



        /*public Text distanceText; // �Ÿ� UI Text ������Ʈ
        public Text scoreText;  // ���ھ� UI Text*/
        public TextMeshProUGUI distanceText;
        public TextMeshProUGUI scoreText;


        // ����� ��ġ �����̵带 ����ϱ� ���� ����////////////////////////////////////////
        private InputAction swipeAction;
        private float moveDistance = 3f;
        /// //////////////////////////////////////////////////////////////////////////////
        /// </summary>


        private bool isPassingObstacle = false; // ��ֹ� ��� ���� ����
        private float passingObstacleTime = 0.4f; // ��ֹ� ��� �ð�
        private PlayerController playerController;

 
        private float gravity;
        private Vector3 movementDirection = Vector3.forward;

        private PlayerInput playerInput;
        private InputAction turnAction;
        private InputAction jumpAction;
        private InputAction slideAction;

        private AudioSource coinSound;

        private Vector3 playerVelocity;

        // ĳ���� ��Ʈ�ѷ�, �����̵� ���� ���� ������ ����
        private CharacterController controller;
        private Animator animator;
        private bool sliding = false;
        private float score = 0;

        private int slidingAnimationId;
        private int jumpAnimationId;
        private int leftrunAnimationld;
        private int rightAnimationld;
        private int stumbleAnimationId;
        private int dieAnimationId;

        private GameManager gameManager; //���� ���� ĵ����


        private bool isMagnetActive = false;
        private float magnetActiveDuration = 5f; // �ڼ� ȿ���� ���ӵǴ� �ð�
        private float magnetActiveTimer = 0f; // �ڼ� ȿ�� ���� �ð��� ������ Ÿ�̸�
        private float magnetDistance = 20f; // �ڼ� ������ ȿ���� ����� �ִ� �Ÿ�
        private float magnetSpeed = 30f; // �ڼ� ������ ȿ���� ���� ������ �̵��ϴ� �ӵ�

        private InputManager inputManager;


        public float moveSpeed = 5f; // �̵� �ӵ�
        public float moveDuration = 1f; // �̵� �ð�
        private Vector3 moveDirection = Vector3.left; // �̵� ����
        private float moveTimer = 0f; // �̵� Ÿ�̸�

        private CoinManager coinManager; //���� ������ ����
        private SoundManager soundManager;

        private int currentLaneIndex = 1;
        private float[] lanePositions = new float[] { -5f, 0f, 5f };
        private void Awake()
        {
            // �ʿ��� ������Ʈ���� ������
            playerInput = GetComponent<PlayerInput>();
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();

            slidingAnimationId = Animator.StringToHash("Sliding");
            jumpAnimationId = Animator.StringToHash("Jumping");
            leftrunAnimationld = Animator.StringToHash("LeftRun");
            rightAnimationld = Animator.StringToHash("RightRun");
            stumbleAnimationId = Animator.StringToHash("Stumble");
            dieAnimationId = Animator.StringToHash("Die");

            turnAction = playerInput.actions["Turn"];
            jumpAction = playerInput.actions["Jump"];
            slideAction = playerInput.actions["Slide"];
            inputManager = GetComponent<InputManager>();

        }

    
        private void OnEnable()
        {
            // �Է� �̺�Ʈ�� ���� ó��    
            slideAction.performed += PlayerSlide;
            jumpAction.performed += PlayerJump;
            inputManager.OnSwipeLeft += OnSwipeLeft;
            inputManager.OnSwipeRight += OnSwipeRight;
            inputManager.OnSwipeUp += OnSwipeUp;
            inputManager.OnSwipeDown += OnSwipeDown;


        }

        private void OnDisable()
        {
            // �Է� �̺�Ʈ�� ���� ó���� ����
            slideAction.performed -= PlayerSlide;
            jumpAction.performed -= PlayerJump;
            inputManager.OnSwipeLeft -= OnSwipeLeft;
            inputManager.OnSwipeRight -= OnSwipeRight;
            inputManager.OnSwipeUp -= OnSwipeUp;
            inputManager.OnSwipeDown -= OnSwipeDown;


        }

        private void OnSwipeLeft()
        {
            Debug.Log("���� �������� ����");


            if (currentLaneIndex > 0)
            {
                currentLaneIndex--;
            }

            Vector3 targetPosition = new Vector3(lanePositions[currentLaneIndex], transform.position.y, transform.position.z); //�÷��̾� ���� �̵�
            controller.Move(targetPosition - transform.position);

            animator.Play(leftrunAnimationld);

        }

        private void OnSwipeRight()
        {
            Debug.Log("������ �������� ����");

            if (currentLaneIndex < 2)
            {
                currentLaneIndex++;
            }

            Vector3 targetPosition = new Vector3(lanePositions[currentLaneIndex], transform.position.y, transform.position.z); //�÷��̾� ������ �̵�
            controller.Move(targetPosition - transform.position);

            animator.Play(rightAnimationld);

        }
        private void OnSwipeUp() { 
            Debug.Log("�� �������� ����");
            if (IsGrounded())
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * gravity * -5f); // ���� ���� ����
                controller.Move(playerVelocity * Time.deltaTime);
                animator.Play(jumpAnimationId);
            }
        }
        private void OnSwipeDown() { 
            Debug.Log("�Ʒ� �������� ����");

            if (!sliding && IsGrounded())
            {
                StartCoroutine(Slide());

            }

        }



        private void Start()
        {
  
            // �ʱ� �ӵ�, �߷� ���� ����
            playerSpeed = initialPlayerSpeed;
            gravity = initialGravityValue;

            gameManager = GameObject.FindObjectOfType<GameManager>(); //���� ���� ĵ���� ����

            /*
            rb = GetComponent<Rigidbody>();
            currentXPos = transform.position.x;
            */

            playerController = FindObjectOfType<PlayerController>(); //�÷��̾� ������Ʈ �Ÿ� �ʱ�ȭ

            lastPosition = transform.position; //�Ÿ� �ʱ�ȭ

            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>(); //���ھ�UI

            obstacleLayer = LayerMask.NameToLayer("Obstacle"); // "Obstacle" ���̾� ��������
            gameObject.layer = LayerMask.NameToLayer("Player"); // "Player" ���̾� ����
            
       
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            scoreText.text = "Score: 0";

            UpdateHealthText(); //ü�� UI
            currentHealth = maxHealth;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;

        }

   


        /*private void PlayerTurn(InputAction.CallbackContext context)
        {
            // ȸ�� ���� ���θ� üũ�ϰ� ȸ���� ó��/*
            Vector3? turnPosition = CheckTurn(context.ReadValue<float>());
            if(!turnPosition.HasValue)
            {
               // GameOver();
                return;
            }

            Vector3 targetDirection = Quaternion.AngleAxis(90 * context.ReadValue<float>(), Vector3.up)*
               movementDirection;
            turnEvent.Invoke(targetDirection);
            Turn(context.ReadValue<float>(),turnPosition.Value);
        }

        private Vector3? CheckTurn(float turnValue)
        {
            // ȸ�� ���� ���θ� üũ�ϰ� ȸ���� ��ġ�� ��ȯ
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, .1f, turnLayer);
            if (hitColliders.Length != 0)
            {
                Tile tile = hitColliders[0].transform.parent.GetComponent<Tile>();
                TileType type = tile.type;
                if((type == TileType.LEFT && turnValue == -1) || 
                   (type == TileType.RIGHT && turnValue == 1) ||
                   (type == TileType.SIDEWAYS))
                {
                    return tile.pivot.position;
                }
            }

            return null;
        }


        private void Turn(float turnValue, Vector3 turnPosition)
        {
            // �÷��̾��� y ��ǥ�� �״�� �ΰ�, x�� z ��ǥ�� �־��� ��ġ�� x�� z ��ǥ�� ����  //�翷 �̵��� �����ؾ���
            //Vector3 tempPlayerPosition = new Vector3(turnPosition.x, transform.position.y, turnPosition.z);
            //transform.position = tempPlayerPosition;        //�۵��� ������ ����
            

            controller.enabled = false; // ĳ���� ��Ʈ�ѷ��� ���� ��ġ�� ������ ��, �ٽ� �Ѽ� �̵��� ����
            controller.enabled = true;


            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90 * turnValue, 0); // �÷��̾ 90�� ȸ����Ű�� ���ʹϾ� ����
            transform.rotation = targetRotation;   // �÷��̾��� ȸ���� ����
            movementDirection = transform.forward.normalized;  // �÷��̾��� �̵� ���� ���͸� ���� �ٶ󺸴� �������� ����
        }*/

        private void PlayerSlide(InputAction.CallbackContext context) // �����̵� �Լ�: �����̵� ���� �ƴϰ� �ٴڿ� ������� ��쿡�� ����
        {
            if(!sliding && IsGrounded())
            {
                StartCoroutine(Slide());

            }

        }
        private IEnumerator Slide()
        {

            sliding = true;
            Vector3 originalControllerCenter = controller.center;
            Vector3 newControllerCenter = originalControllerCenter;
            controller.height /= 2;
            newControllerCenter.y -= controller.height /2;   //�����̵� ��
            controller.center = newControllerCenter;
         
            animator.Play(slidingAnimationId);  //�ִϸ��̼� ����

            yield return new WaitForSeconds(0.5f);

            controller.height *= 2;
            controller.center = originalControllerCenter;
            sliding = false;

        }


        private void PlayerJump(InputAction.CallbackContext context)  // ���� �Լ�: �ٴڿ� ������� ��쿡�� ����


        {
            if (IsGrounded()) 
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * gravity * -3f); // ���� ���� ����
                controller.Move(playerVelocity * Time.deltaTime);
                animator.Play(jumpAnimationId);
            }
        }

        private void Update()
        {
            moveTimer += Time.deltaTime;
            healthSlider.value = currentHealth;

            if (!IsGrounded(20f))
            {
                GameOver();
                return;
            }

            if(currentHealth>maxHealth)
            {
                currentHealth = maxHealth;
            }

            // �ڼ� ȿ���� Ȱ��ȭ�� ���¿��� Ÿ�̸Ӹ� ���ҽ�Ŵ
            if (isMagnetActive)
            {
                magnetActiveTimer -= Time.deltaTime;
                if (magnetActiveTimer <= 0f)
                {
                    isMagnetActive = false; // �ڼ� ȿ�� ��Ȱ��ȭ
                }
            }


            


            controller.Move(transform.forward * playerSpeed * Time.deltaTime);  // �÷��̾ ���� �ٶ󺸴� �������� playerSpeed ��ŭ �̵���Ŵ

            if (IsGrounded() && playerVelocity.y < 0)  // �ٴڿ� ��������鼭 �÷��̾��� ���� �ӵ��� 0���� ���� ���, y �ӵ��� 0���� ����
            {
                playerVelocity.y = 0f;
            }

            playerVelocity.y += gravity * Time.deltaTime;  // �÷��̾��� ���� �ӵ��� �߷� ���ӵ��� ������
            controller.Move(playerVelocity * Time.deltaTime);  // �÷��̾ ���� ���� �ӵ���ŭ �̵���Ŵ

            distanceTraveled += Vector3.Distance(transform.position, lastPosition); //�Ÿ�����
            lastPosition = transform.position;

            if (Time.timeSinceLevelLoad < 0.1f)  //ü�� ����UI �ʱ�ȭ
            {
                UpdateHealthText();
            }

            float distance = playerController.GetDistanceTraveled(); //�Ÿ����� UI
            distanceText.text = $"{distance:F1}m"; //�Է´� �Ÿ� ��� ��� ����  
            float playerDistance = playerController.GetDistanceTraveled();
            float remainingDistance = Mathf.Max(0f, destinationDistance - playerDistance);
            distanceSlider.value = 1f - (remainingDistance / destinationDistance);
        }

        public float GetDistanceTraveled()
        {
            return distanceTraveled;
        }

        private bool IsGrounded(float length = .2f)  // ĳ���� �� �Ʒ��� ���̸� ���� ���� �浹�ϴ��� �Ǵ��ϴ� �Լ� // length�� ������ ���̸� ��Ÿ����, �⺻���� 0.2f
        {   
             
            Vector3 raycastOriginFirst = transform.position;  // raycastOriginFirst ������ ĳ������ �� ���� ��Ÿ���� ����ĳ��Ʈ�� ����
            raycastOriginFirst.y -= controller.height / 2f;   // raycastOriginFirst�� y���� ĳ���� ������ ���ݸ�ŭ ������, �� ���� 0.1f��ŭ �÷���
            raycastOriginFirst.y += 1f;  // raycastOriginFirst�� y���� ĳ���� ������ ���ݸ�ŭ ������, �� ���� 0.1f��ŭ �÷��� �ݶ��̴��� y�� ��ŭ �÷��ָ� ��


            Vector3 ratcastOriginSecond = raycastOriginFirst; // ratcastOriginSecond ������ raycastOriginFirst�� ���� ��ġ����, ������ 0.2f��ŭ �� ���ư�
            raycastOriginFirst -= transform.forward * .2f;
            ratcastOriginSecond += transform.forward * .2f;

            /*
           Debug.DrawLine(raycastOriginFirst, Vector3.down, Color.green, 2f);
           Debug.DrawLine(raycastOriginFirst, Vector3.down, Color.red, 2f);
            */

            if (Physics.Raycast(raycastOriginFirst, Vector3.down, out RaycastHit hit, length, groundLayer) || Physics.
                Raycast(ratcastOriginSecond, Vector3.down, out RaycastHit hit2, length, groundLayer))
            {
                return true;
            }
            return false;

        }

        private void GameOver()
        {
            Debug.Log("Game Over");
            gameOverEvent.Invoke((int)score);
            gameObject.SetActive(false);
            //���� ���� ĵ���� Ȱ��ȭ 
            gameManager.gameOverCanvas.SetActive(true);

        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Obstacle")) //��ֹ��� �ε����� ü�� ���� ó��
            {
                TakeDamage(2);
            }

            if (other.gameObject.CompareTag("Portion")) //���ǿ� �ε����� ü�� ���� ó��
            {
                Destroy(other.gameObject);
                currentHealth += 20;
            }

            if (other.gameObject.CompareTag("Coin"))
            {
                Destroy(other.gameObject); // �浹�� ���� ������Ʈ �ı�
                score += 10;
                scoreText.text = "Score: " + score.ToString(); //��������
                //CoinManager.Instance.AddCoin(1); // ���� �Ŵ����� ���� �߰�
            }

            if(other.gameObject.CompareTag("Magnet")) //�ڼ� �����ۿ� �ε�����
            {
                isMagnetActive = true; // �ڼ� ȿ�� Ȱ��ȭ
                Destroy(other.gameObject); // �浹�� �ڼ� ������ ������Ʈ �ı�
                magnetActiveTimer = magnetActiveDuration; // �ڼ� ȿ�� ���� �ð� �ʱ�ȭ
            }

        }

        private void OnTriggerStay(Collider other) //�ڼ�
        {
            if (isMagnetActive && other.gameObject.CompareTag("Coin")) //�ڼ� ȿ���� Ȱ��ȭ�Ǿ� �ְ� ���ο� �ε�����
            {
                Vector3 direction = transform.position - other.gameObject.transform.position;
                float distance = direction.magnitude;

                // ������ �ڼ� ȿ�� �Ÿ� ���� ������ ������ �÷��̾� ĳ���� ������ �̵�
                if (distance < magnetDistance)
                {
                    other.gameObject.transform.position += direction.normalized * magnetSpeed * Time.deltaTime;
                }
            }
        }


        




        public void TakeDamage(int damage)
        {
            if (!isPassingObstacle)
            {
                currentHealth -= damage;
                if (currentHealth <= 0)
                {
                    
                    //GameOver();
                    //playerSpeed = 0f;
                    animator.Play(dieAnimationId);
                    StartCoroutine(WaitForAnimationToEnd());
                }
                else
                {
                    StartCoroutine(PassingObstacleCoroutine());
                }
                UpdateHealthText();
            }
        }

        // ��ֹ� ��� �ڷ�ƾ
        IEnumerator PassingObstacleCoroutine()
        {
            animator.Play(stumbleAnimationId);
            isPassingObstacle = true;
            yield return new WaitForSeconds(passingObstacleTime);
            isPassingObstacle = false;
        }

        void UpdateHealthText()
        {
            healthText.text = "" + currentHealth.ToString();
        }

        

        private IEnumerator WaitForAnimationToEnd()
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            gameObject.SetActive(false);
            gameManager.gameOverCanvas.SetActive(true);
            gameManager.interstitialAd.ShowAd();
            Debug.Log("00000");
        }


    }

}



