using System;
using BeatMapper;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Game.Tutorial
{
    public class TutorialController : SerializedMonoBehaviour
    {
        [TabGroup("General")] [SerializeField] private TextAsset[] instructions;
        [TabGroup("General")] [SerializeField] private TextAsset failInstruction;
        [TabGroup("General")] [SerializeField] private TutorialSection prefab;
        [TabGroup("General")] [SerializeField] private Transform instructionParent;
        [TabGroup("General")] [SerializeField] private bool canEnableAudioEffects;
        [TabGroup("General")] [SerializeField] private int maxFailedObjects;


        [TabGroup("General")] [SerializeField] private UnityEvent onPauseSpawning = new UnityEvent();
        [TabGroup("General")] [SerializeField] private UnityEvent onResumeSpawning = new UnityEvent();
        [TabGroup("General")] [SerializeField] private UnityEvent onDeactivateSlowdown = new UnityEvent();
        [TabGroup("General")] [SerializeField] private UnityEvent onActivateSlowdown = new UnityEvent();
        [TabGroup("General")] [SerializeField] private UnityEvent onTutorialFinished = new UnityEvent();
        [TabGroup("General")] [SerializeField] private UnityEvent onTutorialSectionFailed = new UnityEvent();
        [TabGroup("General")] [SerializeField] private UnityEvent onSpeedupObjects = new UnityEvent();
        [TabGroup("General")] [SerializeField] private UnityEventString onNextInstruction = new UnityEventString();
        [TabGroup("General")] [SerializeField] private UnityEventString onPreviousInstruction = new UnityEventString();
        [TabGroup("General")] [SerializeField] private UnityEvent onDisableEffects = new UnityEvent();
        [TabGroup("General")] [SerializeField] private UnityEvent onEnableEffects = new UnityEvent();


        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private int currentInstructionIndex;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private string instructionTitle;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private int expectedSpawnings;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private int receivedSpawnings;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private int notesToDetect;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private int notesDetected;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private int obstaclesToDetect;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private int obstaclesDetected;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private bool mustPass;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private bool hasFailed;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private bool hasFinished;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private int failedObstacles;

        [TabGroup("Debug")] [SerializeField] [ReadOnly]
        private int failedBlocks;

        [TabGroup("Debug")] [SerializeField] [ReadOnly] [UsedImplicitly]
        private TutorialInstruction currentinstruction;

        private void Awake()
        {
            onPauseSpawning.AddListener(() => Debug.Log("[TutorialController] OnPauseSpawning called"));
            onResumeSpawning.AddListener(() => Debug.Log("[TutorialController] OnResumeSpawning called"));
            onDeactivateSlowdown.AddListener(() => Debug.Log("[TutorialController] OnDeactivateSlowdown called"));
            onActivateSlowdown.AddListener(() => Debug.Log("[TutorialController] OnActivateSlowdown called"));
            onTutorialFinished.AddListener(() => Debug.Log("[TutorialController] OnTutorialFinished was invoked!"));
            onTutorialSectionFailed.AddListener(() => Debug.Log("[TutorialController] OnTutorialSectionFailed called"));
            onSpeedupObjects.AddListener(() => Debug.Log("[TutorialController] OnSpeedupObjects called"));
            onNextInstruction.AddListener(s => Debug.Log($"[TutorialController] OnNextInstruction called\n{s}"));
            onPreviousInstruction.AddListener(s =>
                Debug.Log($"[TutorialController] OnPreviousInstruction called\n{s}"));
            onDisableEffects.AddListener(() => Debug.Log("[TutorialController] OnDisableEffects called"));
            onEnableEffects.AddListener(() => Debug.Log("[TutorialController] OnEnableEffects called"));

            onTutorialFinished.AddListener(() => hasFinished = true);
        }

        private void Start()
        {
            currentInstructionIndex--;
            NextInstruction();
        }

        [Button]
        private void PreviousInstruction()
        {
            --currentInstructionIndex;
            var instruction = JsonUtility.FromJson<TutorialInstruction>(instructions[currentInstructionIndex].text);

            onPreviousInstruction.Invoke(instruction.bookmarkName);
            InstantiateInstruction(instruction);
        }

        private void NextInstruction()
        {
            ++currentInstructionIndex;
            if (TutorialFinished()) return;

            var instruction = JsonUtility.FromJson<TutorialInstruction>(instructions[currentInstructionIndex].text);
            onNextInstruction.Invoke(instruction.bookmarkName);
            InstantiateInstruction(instruction);
        }

        private void InstantiateInstruction(TutorialInstruction tutorialInstruction)
        {
            onActivateSlowdown.Invoke();

            notesToDetect = notesDetected = obstaclesDetected =
                obstaclesToDetect = receivedSpawnings = failedBlocks = failedObstacles = 0;
            hasFailed = false;

            if (TutorialFinished()) return;

            var tutorialSection = SpawnCanvas();
            var textSetter = tutorialSection.GetComponentInChildren<TextSetter>();

            expectedSpawnings = tutorialInstruction.expectedSpawnings;
            mustPass = tutorialInstruction.mustPass;
            instructionTitle = tutorialInstruction.bookmarkName;

            if (tutorialInstruction.audioEffectsEnabled && canEnableAudioEffects) onEnableEffects.Invoke();
            else onDisableEffects.Invoke();

            tutorialSection.ShouldReleaseTutobjects = expectedSpawnings > 0;
            if (tutorialSection.ShouldReleaseTutobjects)
            {
                tutorialSection.OnContinue.AddListener(onSpeedupObjects.Invoke);
                tutorialSection.OnContinue.AddListener(onDeactivateSlowdown.Invoke);
                onResumeSpawning.Invoke();
            }
            else
            {
                tutorialSection.OnContinue.AddListener(NextInstruction);
                onPauseSpawning.Invoke();
            }

            textSetter.SetText(tutorialInstruction.text);
        }

        private TutorialSection SpawnCanvas()
        {
            return Instantiate(prefab, instructionParent.position, prefab.transform.rotation, instructionParent);
        }

        private bool TutorialFinished()
        {
            if (currentInstructionIndex <= instructions.Length - 1) return false;
            Debug.Log("[TutorialController] Tutorial finished!");
            onTutorialFinished.Invoke();
            return true;
        }

        public void AcknowledgeSpawnedMapping(SongMapping mapping)
        {
            receivedSpawnings += 1;
            notesToDetect += mapping.Notes.Length;
            obstaclesToDetect += mapping.Obstacles.Length;
            if (expectedSpawnings == receivedSpawnings) onPauseSpawning.Invoke();
        }

        public void AckBlock()
        {
            notesDetected++;
            Debug.Log("[TutorialController] Note acknowledged.");
            CheckSectionFinished();
        }

        public void AckObstacle()
        {
            obstaclesDetected++;
            Debug.Log("[TutorialController] Obstacle acknowledged.");
            CheckSectionFinished();
        }

        public void AddSingleScore(int score)
        {
            if (score == 0 && mustPass)
            {
                failedBlocks++;

                if (failedBlocks + failedObstacles == maxFailedObjects)
                {
                    Debug.Log("[TutorialController] Detected block with score of 0, tutorial failed!");
                    hasFailed = true;
                }
            }
        }

        public void AddObstacleStatus(GameObject obstacle)
        {
            var headDetected = obstacle.GetComponent<HeadDetector>()?.HeadDetected;
            if (mustPass && headDetected.GetValueOrDefault())
            {
                failedObstacles++;
                if (failedObstacles + failedBlocks == maxFailedObjects)
                {
                    Debug.Log("[TutorialController] Detected Obstacle with detected head, tutorial failed!");
                    hasFailed = true;
                }
            }
        }

        public void PlaySpecificTutorialSection(TextAsset asset)
        {
            var instruction = JsonUtility.FromJson<TutorialInstruction>(asset.text);
            var bookmarkName = instruction.bookmarkName;
            currentInstructionIndex = -2;
            onNextInstruction.Invoke(bookmarkName);
            InstantiateInstruction(instruction);
        }

        private void CheckSectionFinished()
        {
            //TODO: Introduce Threshold until sections fails
            if (obstaclesDetected == obstaclesToDetect && notesDetected == notesToDetect)
            {
                if (!hasFinished && (mustPass && !hasFailed || !mustPass))
                    NextInstruction();
                else if (!hasFinished)
                    RegisterFailedTut();
                else if (hasFinished)
                    onTutorialFinished.Invoke();
            }
        }

        private void RegisterFailedTut()
        {
            onTutorialSectionFailed.Invoke();
            currentInstructionIndex--;

            var instruction = JsonUtility.FromJson<TutorialInstruction>(failInstruction.text);
            InstantiateInstruction(instruction);
        }
    }

    [UsedImplicitly]
    [Serializable]
    internal class TutorialInstruction
    {
        public string text;
        public string bookmarkName;
        public int expectedSpawnings;
        public bool mustPass;
        public bool audioEffectsEnabled;

        public override string ToString()
        {
            return $"Bookmark: {bookmarkName}" +
                   $"\n\tSpawniongs: {expectedSpawnings}" +
                   $"\n\tMust pass: {mustPass}" +
                   $"\n\tAudio effects enabled: {audioEffectsEnabled}" +
                   $"\n\tInstruction: {text}";
        }
    }
}