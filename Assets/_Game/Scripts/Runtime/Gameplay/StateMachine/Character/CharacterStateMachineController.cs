using Game.Gameplay.StateMachine.Character;
using UnityEngine;
using UnityEngine.InputSystem;
using GameDemo.Runtime.Gameplay.Character;
using GameDemo.Runtime.Gameplay.StateMachine.Character.Combat;
using GameDemo.Runtime.Gameplay.StateMachine.Character.Grounded;
using GameDemo.Runtime.Gameplay.Cameras;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character
{
    public sealed class CharacterStateMachineController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private bool enableInput = true;

        [Header("Movement")]
        [SerializeField] private float moveThreshold = 0.01f;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 100f;

        [Header("Debug")]
        [SerializeField] private float hitDuration = 0.35f;
        [SerializeField] private bool attackPressed;
        [SerializeField] private float attackStartupDuration = 0.12f;
        [SerializeField] private float attackActiveDuration = 0.08f;
        [SerializeField] private float attackRecoveryDuration = 0.20f;

        [Header("Runtime")]
        [SerializeField] private string currentParentStateName;
        [SerializeField] private string currentLeafStateName;
        [SerializeField] private Vector2 currentMoveInput;
        [SerializeField] private bool currentIsHit;
        [SerializeField] private bool currentIsDead;
        
        [Header("References")]
        [SerializeField] private Animator animator;
        [SerializeField] private PlayerCameraController cameraController;

        private CharacterStateBlackboard _blackboard;
        private CharacterStateContext _context;
        private HfsmMachine _machine;

        private CharacterInputAdapter _inputAdapter;

        private GroundedState _groundedState;
        private HitState _hitState;
        private DeadState _deadState;
        private AttackState _attackState;
        private float _sqrMoveThreshold;

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

            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }

            if (cameraController == null)
            {
                cameraController = FindFirstObjectByType<PlayerCameraController>();
            }
            
            _context = new CharacterStateContext
            {
                Owner = gameObject,
                Transform = transform,
                Blackboard = _blackboard,
                Animator = new CharacterAnimatorController(animator),
                Motor = new CharacterMotor(transform, cameraController,moveSpeed,rotationSpeed)
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
            var threshold = Mathf.Max(0f, moveThreshold);
            _sqrMoveThreshold = threshold * threshold;

            _groundedState = new GroundedState(_context, threshold);
            _hitState = new HitState(_context, hitDuration);
            _deadState = new DeadState(_context);
            _attackState = new AttackState(
                _context,
                attackStartupDuration,
                attackActiveDuration,
                attackRecoveryDuration);
        }

        private void BuildMachine()
        {
            _machine = new HfsmMachine();

            _machine.Register(_groundedState);
            _machine.Register(_hitState);
            _machine.Register(_deadState);
            _machine.Register(_attackState);

            _machine.SetDefaultState(CharacterStateIds.Grounded);

            _machine.AddTransition(
                CharacterStateIds.Grounded,
                CharacterStateIds.Attack,
                new FuncCondition(() => _blackboard.AttackPressed),
                priority: 10
            );

            _machine.AddTransition(
                CharacterStateIds.Attack,
                CharacterStateIds.Grounded,
                new FuncCondition(() => _attackState.ShouldExitToMove(_sqrMoveThreshold))
            );

            _machine.AddTransition(
                CharacterStateIds.Attack,
                CharacterStateIds.Grounded,
                new FuncCondition(() => _attackState.ShouldExitToIdle(_sqrMoveThreshold))
            );

            _machine.AddAnyTransition(
                CharacterStateIds.Dead,
                new FuncCondition(() => _blackboard.DeadRequested || _blackboard.IsDead),
                priority: 1000
            );

            _machine.AddAnyTransition(
                CharacterStateIds.Hit,
                new FuncCondition(() => !_blackboard.IsDead && _blackboard.HitRequested),
                priority: 100
            );

            _machine.AddTransition(
                CharacterStateIds.Hit,
                CharacterStateIds.Grounded,
                new FuncCondition(() => _hitState.IsFinished())
            );
        }

        private void UpdateInput()
        {
            if (!enableInput || _inputAdapter == null)
            {
                _blackboard.MoveInput = Vector2.zero;
                _blackboard.HitRequested = false;
                _blackboard.DeadRequested = false;
                _blackboard.AttackPressed = false;
                return;
            }

            _inputAdapter.Tick();

            _blackboard.MoveInput = _inputAdapter.MoveInput;
            _blackboard.HitRequested = _inputAdapter.HitPressedThisFrame;
            _blackboard.DeadRequested = _inputAdapter.DeadPressedThisFrame;
            _blackboard.AttackPressed = _inputAdapter.AttackPressedThisFrame;
        }

        private void SyncDebugFields()
        {
            currentParentStateName = _machine.CurrentStateName;
            currentLeafStateName = ResolveLeafStateName();
            currentMoveInput = _blackboard.MoveInput;
            currentIsHit = _blackboard.IsHit;
            currentIsDead = _blackboard.IsDead;
            attackPressed = _blackboard.AttackPressed;
        }

        private string ResolveLeafStateName()
        {
            if (_machine == null)
            {
                return "None";
            }

            if (_machine.CurrentStateName == CharacterStateIds.Grounded)
            {
                return _groundedState.ActiveChildStateName;
            }

            if (_machine.CurrentStateName == CharacterStateIds.Attack)
            {
                return _attackState.ActiveChildStateName;
            }

            return _machine.CurrentStateName;
        }
    }
}