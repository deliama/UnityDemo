using Game.Gameplay.StateMachine.Character;
using UnityEngine;
using UnityEngine.InputSystem;
using GameDemo.Runtime.Gameplay.Character;
using GameDemo.Runtime.Gameplay.StateMachine.Character.Grounded;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character
{
    public sealed class CharacterStateMachineController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private bool enableInput = true;

        [Header("Movement")]
        [SerializeField] private float moveThreshold = 0.01f;

        [Header("Hit Debug")]
        [SerializeField] private float hitDuration = 0.35f;

        [Header("Runtime")]
        [SerializeField] private string currentStateName;
        [SerializeField] private Vector2 currentMoveInput;
        [SerializeField] private bool currentIsHit;

        private CharacterStateBlackboard _blackboard;
        private CharacterStateContext _context;
        private HfsmMachine _machine;

        private CharacterInputAdapter _inputAdapter;

        private IdleState _idleState;
        private MoveState _moveState;
        private HitState _hitState;

        public string CurrentStateName => _machine?.CurrentStateName ?? "None";
        public CharacterStateBlackboard Blackboard => _blackboard;

        private void Awake()
        {
            BuildContext();
            BuildStates();
            BuildMachine();
        }

        private void Start()
        {
            _machine.Start();
            SyncDebugFields();
        }

        private void Update()
        {
            UpdateInput();
            _machine.Tick(Time.deltaTime);
            SyncDebugFields();
        }

        private void OnDestroy()
        {
            _inputAdapter?.Dispose();
        }

        private void BuildContext()
        {
            _blackboard = new CharacterStateBlackboard();

            _context = new CharacterStateContext
            {
                Owner = gameObject,
                Transform = transform,
                Blackboard = _blackboard
            };

            if (enableInput)
            {
                if (inputActions == null)
                {
                    Debug.LogError("CharacterStateMachineController: inputActions is not assigned.", this);
                    enabled = false;
                    return;
                }

                _inputAdapter = new CharacterInputAdapter(inputActions);
            }
        }

        private void BuildStates()
        {
            _idleState = new IdleState(_context);
            _moveState = new MoveState(_context);
            _hitState = new HitState(_context, hitDuration);
        }

        private void BuildMachine()
        {
            _machine = new HfsmMachine();

            _machine.Register(_idleState);
            _machine.Register(_moveState);
            _machine.Register(_hitState);

            _machine.SetDefaultState(CharacterStateIds.Idle);

            float threshold = Mathf.Max(0f, moveThreshold);
            float sqrThreshold = threshold * threshold;

            _machine.AddTransition(
                CharacterStateIds.Idle,
                CharacterStateIds.Move,
                new FuncCondition(() => !_blackboard.IsHit && _blackboard.MoveInput.sqrMagnitude > sqrThreshold)
            );

            _machine.AddTransition(
                CharacterStateIds.Move,
                CharacterStateIds.Idle,
                new FuncCondition(() => !_blackboard.IsHit && _blackboard.MoveInput.sqrMagnitude <= sqrThreshold)
            );

            // 受击应可从任意状态打断；优先级提高以覆盖普通移动切换。
            _machine.AddAnyTransition(
                CharacterStateIds.Hit,
                new FuncCondition(() => _blackboard.HitRequested),
                priority: 100
            );

            _machine.AddTransition(
                CharacterStateIds.Hit,
                CharacterStateIds.Idle,
                new FuncCondition(() => _hitState.IsFinished())
            );
        }

        private void UpdateInput()
        {
            if (!enableInput || _inputAdapter == null)
            {
                _blackboard.MoveInput = Vector2.zero;
                _blackboard.HitRequested = false;
                return;
            }

            _inputAdapter.Tick();

            _blackboard.MoveInput = _inputAdapter.MoveInput;
            _blackboard.HitRequested = _inputAdapter.HitPressedThisFrame;
        }

        private void SyncDebugFields()
        {
            currentStateName = _machine.CurrentStateName;
            currentMoveInput = _blackboard.MoveInput;
            currentIsHit = _blackboard.IsHit;
        }
    }
}