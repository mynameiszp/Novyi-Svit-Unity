using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using Cinemachine;

public class PlayerStoryMovements : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float runSpeed = 10f;
    [Header("Dialog")]
    [SerializeField] GameObject dialogCanvas;
    [SerializeField] GameObject instructionCanvas;
    [SerializeField] TextMeshProUGUI playerText;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI opponentText;
    [SerializeField] TextMeshProUGUI opponentName;
    [SerializeField] Image playerAvatar;
    [SerializeField] Image opponentAvatar;
    [Header("Camera")]
    [SerializeField] Vector2 cameraOffset;
    [SerializeField] float cameraLensInStory;

    private CapsuleCollider2D bodyCollider;
    private BoxCollider2D feetCollider;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private Vector2 moveInput;
    private LevelStoryData storyData;
    private bool canMove = true;
    private bool isInStoryMode = false;
    private int linesNumInScene;
    private int linesDisplayed;
    private List<string> speakersInScene;
    private Rect rec;
    private Texture2D avatarTexture;
    private float opponentAvatarPosition;
    private GameObject doors;
    private GameObject mainCharacters;
    private GameObject followCamera;
    private LensSettings defaultFollowCameraLens;
    private Vector3 defaultFollowCameraOffset;
    private string currentScene;
    private Vector3 initialScale;
    private GameObject warningCanvas;
    private GameObject exitButton;
    private NextLevelNoExit nextLevelScript;

    private void Awake()
    {
        feetCollider = GetComponent<BoxCollider2D>();
        initialScale = transform.localScale;
        currentScene = SceneManager.GetActiveScene().name;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        exitButton = GameObject.FindWithTag("ExitButton");
        warningCanvas = GameObject.FindWithTag("WarningCanvas");
        if (warningCanvas != null) warningCanvas.SetActive(false);
        doors = GameObject.FindWithTag("Exit");
        if (doors != null) doors.SetActive(false);
        mainCharacters = GameObject.FindWithTag("MainCharacters");
        followCamera = GameObject.FindWithTag("FollowCamera");
        defaultFollowCameraLens = followCamera.GetComponent<CinemachineVirtualCamera>().m_Lens;
        defaultFollowCameraOffset = followCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        opponentAvatarPosition = opponentAvatar.transform.localPosition.x;
        dialogCanvas.SetActive(false);
        instructionCanvas.SetActive(false);
        nextLevelScript = GetComponent<NextLevelNoExit>();
        storyData = GameObject.FindWithTag("Player").GetComponent<LevelStoryData>();
    }

    IEnumerator Start()
    {
        yield return storyData;
        
            linesDisplayed = storyData.GetStartLineNumber(currentScene);
            linesNumInScene = storyData.GetLinesNumberInScene(currentScene) + linesDisplayed;
            speakersInScene = storyData.GetSpeakersInScene(currentScene);
        
    }

    void Update()
    {
        Run();
        FlipPlayer();
    }

    void OnSkip(InputValue input)
    {
        
            if (isInStoryMode && linesDisplayed >= linesNumInScene)
            {
                dialogCanvas.SetActive(false);
                mainCharacters.SetActive(false);
                ExitStoryMode();
                if (doors == null && exitButton == null) nextLevelScript.LoadNextLevel(SceneManager.GetActiveScene().buildIndex + 1);
                else if (doors != null && exitButton == null) doors.SetActive(true);
            }
            if (isInStoryMode && linesDisplayed < linesNumInScene)
            {
                instructionCanvas.SetActive(false);
                dialogCanvas.SetActive(true);
                PlayDialog();
            }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MainCharacters")
        {
            EnterStoryMode();
            instructionCanvas.SetActive(true);
        }
    }

    private void EnterStoryMode()
    {
        isInStoryMode = true;
        canMove = false;
        animator.speed = 0f;
        followCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = cameraLensInStory;
        followCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = cameraOffset;
    }

    private void ExitStoryMode()
    {
        isInStoryMode = false;
        canMove = true;
        animator.speed = 1f;
        followCamera.GetComponent<CinemachineVirtualCamera>().m_Lens = defaultFollowCameraLens;
        followCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = defaultFollowCameraOffset;
        if (warningCanvas != null && exitButton != null) ShowWarning();
    }

    private void ShowWarning()
    {
        warningCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    private void PlayDialog()
    {
        avatarTexture = LoadPNG(storyData.GetSpeakerAvatarLink(linesDisplayed));
        rec = new Rect(0, 0, avatarTexture.width, avatarTexture.height);
        if (storyData.GetCurrentSpeaker(linesDisplayed) == "Player")
        {
            SetPlayerDialogPanel();
            playerText.text = storyData.GetSpeakerText(linesDisplayed);
            playerName.text = "Player";
            playerAvatar.GetComponent<Image>().sprite = Sprite.Create(avatarTexture, rec, new Vector2(0, 0));
        }
        else
        {
            SetOpponentDialogPanel();
            opponentText.text = storyData.GetSpeakerText(linesDisplayed);
            opponentName.text = storyData.GetCurrentSpeaker(linesDisplayed);
            opponentAvatar.GetComponent<Image>().sprite = Sprite.Create(avatarTexture, rec, new Vector2(0, 0));
            opponentAvatar.transform.localScale = new Vector2(-1, 1);
            opponentAvatar.transform.localPosition = new Vector2(opponentAvatarPosition - avatarTexture.width, opponentAvatar.transform.localPosition.y);
        }
        linesDisplayed++;
    }

    private void SetPlayerDialogPanel()
    {
        playerText.gameObject.SetActive(true);
        playerAvatar.gameObject.SetActive(true);
        playerName.gameObject.SetActive(true);
        opponentName.gameObject.SetActive(false);
        opponentText.gameObject.SetActive(false);
        opponentAvatar.gameObject.SetActive(false);
    }

    private void SetOpponentDialogPanel()
    {
        playerText.gameObject.SetActive(false);
        playerAvatar.gameObject.SetActive(false);
        playerName.gameObject.SetActive(false);
        opponentName.gameObject.SetActive(true);
        opponentText.gameObject.SetActive(true);
        opponentAvatar.gameObject.SetActive(true);
    }

    public Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        return tex;
    }

    void OnMove(InputValue input)
    {
        moveInput = input.Get<Vector2>();
    }

    void Run()
    {
        if (canMove)
        {
            transform.Translate(moveInput * runSpeed * Time.deltaTime);
            bool isMoving = Mathf.Abs(moveInput.x) > 0;
            animator.SetBool("isRunning", isMoving);
        }
    }

    void FlipPlayer()
    {
        if (canMove)
        {
            bool isMoving = Mathf.Abs(moveInput.x) > 0;
            if (isMoving)
            {
                transform.localScale = new Vector2(Mathf.Sign(moveInput.x) * initialScale.x, initialScale.y);
            }
        }
    }
}