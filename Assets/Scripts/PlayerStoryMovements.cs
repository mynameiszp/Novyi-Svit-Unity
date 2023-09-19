using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Drawing;
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

    private CapsuleCollider2D bodyCollider;
    private BoxCollider2D feetCollider;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private Vector2 moveInput;
    private DataLoader dataLoader;
    private bool canMove = true;
    private bool isInStoryMode = false;
    private int linesNumInScene;
    private int linesDisplayed = 0;
    private List<string> speakersInScene;
    private Rect rec;
    private Texture2D avatarTexture;
    private float opponentAvatarPosition;
    private GameObject doors;
    private GameObject mainCharacters;
    private GameObject followCamera;
    private LensSettings defaultFollowCameraLens;
    private Vector3 defaultFollowCameraOffset;

    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        doors = GameObject.FindWithTag("Exit");
        mainCharacters = GameObject.FindWithTag("MainCharacters");
        followCamera = GameObject.FindWithTag("FollowCamera");
        defaultFollowCameraLens = followCamera.GetComponent<CinemachineVirtualCamera>().m_Lens;
        defaultFollowCameraOffset = followCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        opponentAvatarPosition = opponentAvatar.transform.localPosition.x;
        dialogCanvas.SetActive(false);
        instructionCanvas.SetActive(false);
        yield return dataLoader;
        dataLoader = GameObject.FindWithTag("Player").GetComponent<DataLoader>();
        yield return dataLoader;
        doors.SetActive(false);
        linesNumInScene = dataLoader.GetLinesNumberInScene("StartStory");
        speakersInScene = dataLoader.GetSpeakersInScene("StartStory");
    }

    void Update()
    {
        Run();
        FlipPlayer();
    }

    void OnSkip(InputValue input)
    {
        if (linesDisplayed >= linesNumInScene)
        {
            isInStoryMode = false;
            dialogCanvas.SetActive(false);
            canMove = true;
            animator.speed = 1f;
            doors.SetActive(true);
            mainCharacters.SetActive(false);
            followCamera.GetComponent<CinemachineVirtualCamera>().m_Lens = defaultFollowCameraLens;
            followCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = defaultFollowCameraOffset;
        }
        if (isInStoryMode && linesDisplayed < linesNumInScene)
        {
            instructionCanvas.SetActive(false);
            dialogCanvas.SetActive(true);
            PlayText();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MainCharacters")
        {
            isInStoryMode = true;
            canMove = false;
            animator.speed = 0f;
            instructionCanvas.SetActive(true);
            followCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 4;
            followCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector2(3.4f, -0.3f);
        }
        if (other.tag == "Exit")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    private void PlayText()
    {
        avatarTexture = LoadPNG(dataLoader.GetSpeakerAvatarLink(linesDisplayed));
        rec = new Rect(0, 0, avatarTexture.width, avatarTexture.height);
        if (dataLoader.GetCurrentSpeaker(linesDisplayed) == "Player")
        {
            playerText.gameObject.SetActive(true);
            playerAvatar.gameObject.SetActive(true);
            playerName.gameObject.SetActive(true);
            opponentName.gameObject.SetActive(false);
            opponentText.gameObject.SetActive(false);
            opponentAvatar.gameObject.SetActive(false);
            playerText.text = dataLoader.GetSpeakerText(linesDisplayed);
            playerName.text = "Player";
            playerAvatar.GetComponent<Image>().sprite = Sprite.Create(avatarTexture, rec, new Vector2(0, 0));
        }
        else
        {
            playerText.gameObject.SetActive(false);
            playerAvatar.gameObject.SetActive(false);
            playerName.gameObject.SetActive(false);
            opponentName.gameObject.SetActive(true);
            opponentText.gameObject.SetActive(true);
            opponentAvatar.gameObject.SetActive(true);
            opponentText.text = dataLoader.GetSpeakerText(linesDisplayed);
            opponentName.text = dataLoader.GetCurrentSpeaker(linesDisplayed);
            opponentAvatar.GetComponent<Image>().sprite = Sprite.Create(avatarTexture, rec, new Vector2(0, 0));
            opponentAvatar.transform.localScale = new Vector2(-1, 1);
            opponentAvatar.transform.localPosition = new Vector2(opponentAvatarPosition - avatarTexture.width, opponentAvatar.transform.localPosition.y);
        }
        linesDisplayed++;
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
                transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
            }
        }
    }
}
