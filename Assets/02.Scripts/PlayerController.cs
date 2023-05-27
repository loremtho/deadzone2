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
        // 초기 속도, 최대 속도, 속도 증가 비율, 점프 높이, 초기 중력 값, 바닥 레이어, 회전 레이어 등의 변수를 선언.
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

        // 플레이어의 속도, 중력, 움직이는 방향, 입력 관련 변수 등을 선언.
        [SerializeField]
        public float playerSpeed;

        [SerializeField]
        private float scoreMultiplier = 10f;

        // 회전 이벤트를 처리하는 변수를 선언
        [SerializeField]
        private UnityEvent<Vector3> turnEvent;

        [SerializeField]
        private UnityEvent<int> gameOverEvent;

        [SerializeField]
        private UnityEvent<int> scoreUpdateEvent;

        private InterstitialAd interstitialAd;



        public float distanceTraveled = 0f;  //이동 측정 변수
        public Vector3 lastPosition;
        public float destinationDistance = 15000f;
        public Slider distanceSlider;

        public int maxHealth = 5; // 최대 체력
        public int currentHealth; // 현재 체력
        //public Text healthText;       // 체력을 표시할 Text UI 요소
        public TextMeshProUGUI healthText;
        public Slider healthSlider;



        /*public Text distanceText; // 거리 UI Text 컴포넌트
        public Text scoreText;  // 스코어 UI Text*/
        public TextMeshProUGUI distanceText;
        public TextMeshProUGUI scoreText;


        // 모바일 터치 슬라이드를 사용하기 위한 변수////////////////////////////////////////
        private InputAction swipeAction;
        private float moveDistance = 3f;
        /// //////////////////////////////////////////////////////////////////////////////
        /// </summary>


        private bool isPassingObstacle = false; // 장애물 통과 상태 여부
        private float passingObstacleTime = 0.4f; // 장애물 통과 시간
        private PlayerController playerController;

 
        private float gravity;
        private Vector3 movementDirection = Vector3.forward;

        private PlayerInput playerInput;
        private InputAction turnAction;
        private InputAction jumpAction;
        private InputAction slideAction;

        private AudioSource coinSound;

        private Vector3 playerVelocity;

        // 캐릭터 컨트롤러, 슬라이딩 여부 등의 변수를 선언
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

        private GameManager gameManager; //게임 오버 캔버스


        private bool isMagnetActive = false;
        private float magnetActiveDuration = 5f; // 자석 효과가 지속되는 시간
        private float magnetActiveTimer = 0f; // 자석 효과 지속 시간을 측정할 타이머
        private float magnetDistance = 20f; // 자석 아이템 효과가 적용될 최대 거리
        private float magnetSpeed = 30f; // 자석 아이템 효과로 인해 코인이 이동하는 속도

        private InputManager inputManager;


        public float moveSpeed = 5f; // 이동 속도
        public float moveDuration = 1f; // 이동 시간
        private Vector3 moveDirection = Vector3.left; // 이동 방향
        private float moveTimer = 0f; // 이동 타이머

        private CoinManager coinManager; //코인 데이터 저장
        private SoundManager soundManager;

        private int currentLaneIndex = 1;
        private float[] lanePositions = new float[] { -5f, 0f, 5f };
        private void Awake()
        {
            // 필요한 컴포넌트들을 가져옴
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
            // 입력 이벤트에 대한 처리    
            slideAction.performed += PlayerSlide;
            jumpAction.performed += PlayerJump;
            inputManager.OnSwipeLeft += OnSwipeLeft;
            inputManager.OnSwipeRight += OnSwipeRight;
            inputManager.OnSwipeUp += OnSwipeUp;
            inputManager.OnSwipeDown += OnSwipeDown;


        }

        private void OnDisable()
        {
            // 입력 이벤트에 대한 처리를 해제
            slideAction.performed -= PlayerSlide;
            jumpAction.performed -= PlayerJump;
            inputManager.OnSwipeLeft -= OnSwipeLeft;
            inputManager.OnSwipeRight -= OnSwipeRight;
            inputManager.OnSwipeUp -= OnSwipeUp;
            inputManager.OnSwipeDown -= OnSwipeDown;


        }

        private void OnSwipeLeft()
        {
            Debug.Log("왼쪽 스와이프 감지");


            if (currentLaneIndex > 0)
            {
                currentLaneIndex--;
            }

            Vector3 targetPosition = new Vector3(lanePositions[currentLaneIndex], transform.position.y, transform.position.z); //플레이어 왼쪽 이동
            controller.Move(targetPosition - transform.position);

            animator.Play(leftrunAnimationld);

        }

        private void OnSwipeRight()
        {
            Debug.Log("오른쪽 스와이프 감지");

            if (currentLaneIndex < 2)
            {
                currentLaneIndex++;
            }

            Vector3 targetPosition = new Vector3(lanePositions[currentLaneIndex], transform.position.y, transform.position.z); //플레이어 오른쪽 이동
            controller.Move(targetPosition - transform.position);

            animator.Play(rightAnimationld);

        }
        private void OnSwipeUp() { 
            Debug.Log("위 스와이프 감지");
            if (IsGrounded())
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * gravity * -5f); // 점프 동작 구현
                controller.Move(playerVelocity * Time.deltaTime);
                animator.Play(jumpAnimationId);
            }
        }
        private void OnSwipeDown() { 
            Debug.Log("아래 스와이프 감지");

            if (!sliding && IsGrounded())
            {
                StartCoroutine(Slide());

            }

        }



        private void Start()
        {
  
            // 초기 속도, 중력 값을 설정
            playerSpeed = initialPlayerSpeed;
            gravity = initialGravityValue;

            gameManager = GameObject.FindObjectOfType<GameManager>(); //게임 오버 캔버스 참조

            /*
            rb = GetComponent<Rigidbody>();
            currentXPos = transform.position.x;
            */

            playerController = FindObjectOfType<PlayerController>(); //플레이어 오브젝트 거리 초기화

            lastPosition = transform.position; //거리 초기화

            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>(); //스코어UI

            obstacleLayer = LayerMask.NameToLayer("Obstacle"); // "Obstacle" 레이어 가져오기
            gameObject.layer = LayerMask.NameToLayer("Player"); // "Player" 레이어 설정
            
       
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            scoreText.text = "Score: 0";

            UpdateHealthText(); //체력 UI
            currentHealth = maxHealth;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;

        }

   


        /*private void PlayerTurn(InputAction.CallbackContext context)
        {
            // 회전 가능 여부를 체크하고 회전을 처리/*
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
            // 회전 가능 여부를 체크하고 회전할 위치를 반환
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
            // 플레이어의 y 좌표는 그대로 두고, x와 z 좌표를 주어진 위치의 x와 z 좌표로 설정  //양옆 이동시 수정해야함
            //Vector3 tempPlayerPosition = new Vector3(turnPosition.x, transform.position.y, turnPosition.z);
            //transform.position = tempPlayerPosition;        //작동시 가운대로 고정
            

            controller.enabled = false; // 캐릭터 컨트롤러를 끄고 위치를 변경한 뒤, 다시 켜서 이동을 적용
            controller.enabled = true;


            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90 * turnValue, 0); // 플레이어를 90도 회전시키는 쿼터니언 생성
            transform.rotation = targetRotation;   // 플레이어의 회전을 적용
            movementDirection = transform.forward.normalized;  // 플레이어의 이동 방향 벡터를 현재 바라보는 방향으로 설정
        }*/

        private void PlayerSlide(InputAction.CallbackContext context) // 슬라이드 함수: 슬라이드 중이 아니고 바닥에 닿아있을 경우에만 동작
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
            newControllerCenter.y -= controller.height /2;   //슬라이딩 값
            controller.center = newControllerCenter;
         
            animator.Play(slidingAnimationId);  //애니메이션 미정

            yield return new WaitForSeconds(0.5f);

            controller.height *= 2;
            controller.center = originalControllerCenter;
            sliding = false;

        }


        private void PlayerJump(InputAction.CallbackContext context)  // 점프 함수: 바닥에 닿아있을 경우에만 동작


        {
            if (IsGrounded()) 
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * gravity * -3f); // 점프 동작 구현
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

            // 자석 효과가 활성화된 상태에서 타이머를 감소시킴
            if (isMagnetActive)
            {
                magnetActiveTimer -= Time.deltaTime;
                if (magnetActiveTimer <= 0f)
                {
                    isMagnetActive = false; // 자석 효과 비활성화
                }
            }


            


            controller.Move(transform.forward * playerSpeed * Time.deltaTime);  // 플레이어를 현재 바라보는 방향으로 playerSpeed 만큼 이동시킴

            if (IsGrounded() && playerVelocity.y < 0)  // 바닥에 닿아있으면서 플레이어의 수직 속도가 0보다 작을 경우, y 속도를 0으로 설정
            {
                playerVelocity.y = 0f;
            }

            playerVelocity.y += gravity * Time.deltaTime;  // 플레이어의 수직 속도에 중력 가속도를 더해줌
            controller.Move(playerVelocity * Time.deltaTime);  // 플레이어를 현재 수직 속도만큼 이동시킴

            distanceTraveled += Vector3.Distance(transform.position, lastPosition); //거리측정
            lastPosition = transform.position;

            if (Time.timeSinceLevelLoad < 0.1f)  //체력 측정UI 초기화
            {
                UpdateHealthText();
            }

            float distance = playerController.GetDistanceTraveled(); //거리측정 UI
            distanceText.text = $"{distance:F1}m"; //입력는 거리 출력 언어 설정  
            float playerDistance = playerController.GetDistanceTraveled();
            float remainingDistance = Mathf.Max(0f, destinationDistance - playerDistance);
            distanceSlider.value = 1f - (remainingDistance / destinationDistance);
        }

        public float GetDistanceTraveled()
        {
            return distanceTraveled;
        }

        private bool IsGrounded(float length = .2f)  // 캐릭터 발 아래에 레이를 쏴서 땅과 충돌하는지 판단하는 함수 // length는 레이의 길이를 나타내며, 기본값은 0.2f
        {   
             
            Vector3 raycastOriginFirst = transform.position;  // raycastOriginFirst 변수는 캐릭터의 발 밑을 나타내는 레이캐스트의 시작
            raycastOriginFirst.y -= controller.height / 2f;   // raycastOriginFirst의 y값을 캐릭터 높이의 절반만큼 내리고, 그 위로 0.1f만큼 올려줌
            raycastOriginFirst.y += 1f;  // raycastOriginFirst의 y값을 캐릭터 높이의 절반만큼 내리고, 그 위로 0.1f만큼 올려줌 콜라이더의 y축 만큼 올려주면 됨


            Vector3 ratcastOriginSecond = raycastOriginFirst; // ratcastOriginSecond 변수는 raycastOriginFirst와 같은 위치지만, 앞으로 0.2f만큼 더 나아감
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
            //게임 오버 캔버스 활성화 
            gameManager.gameOverCanvas.SetActive(true);

        }

        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Obstacle")) //장애물에 부딪히면 체력 감소 처리
            {
                TakeDamage(2);
            }

            if (other.gameObject.CompareTag("Portion")) //포션에 부딪히면 체력 증가 처리
            {
                Destroy(other.gameObject);
                currentHealth += 20;
            }

            if (other.gameObject.CompareTag("Coin"))
            {
                Destroy(other.gameObject); // 충돌한 코인 오브젝트 파괴
                score += 10;
                scoreText.text = "Score: " + score.ToString(); //점수증가
                //CoinManager.Instance.AddCoin(1); // 코인 매니저에 코인 추가
            }

            if(other.gameObject.CompareTag("Magnet")) //자석 아이템에 부딪히면
            {
                isMagnetActive = true; // 자석 효과 활성화
                Destroy(other.gameObject); // 충돌한 자석 아이템 오브젝트 파괴
                magnetActiveTimer = magnetActiveDuration; // 자석 효과 지속 시간 초기화
            }

        }

        private void OnTriggerStay(Collider other) //자석
        {
            if (isMagnetActive && other.gameObject.CompareTag("Coin")) //자석 효과가 활성화되어 있고 코인에 부딪히면
            {
                Vector3 direction = transform.position - other.gameObject.transform.position;
                float distance = direction.magnitude;

                // 코인이 자석 효과 거리 내에 있으면 코인을 플레이어 캐릭터 쪽으로 이동
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

        // 장애물 통과 코루틴
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



