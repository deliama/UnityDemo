using Game.Gameplay.StateMachine.Character;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character.Grounded
{
    /// <summary>
    /// Grounded 父状态：承载地面态公共逻辑，并在内部驱动 Idle / Move 子状态。
    /// </summary>
    public sealed class GroundedState : CharacterStateBase
    {
        private readonly IdleState _idleState;
        private readonly MoveState _moveState;
        private readonly HfsmMachine _subMachine;

        public GroundedState(CharacterStateContext context, float moveThreshold)
            : base(CharacterStateIds.Grounded, context)
        {
            var threshold = moveThreshold < 0f ? 0f : moveThreshold;
            var sqrThreshold = threshold * threshold;

            _idleState = new IdleState(context);
            _moveState = new MoveState(context);
            _subMachine = new HfsmMachine();

            _subMachine.Register(_idleState);
            _subMachine.Register(_moveState);
            _subMachine.SetDefaultState(CharacterStateIds.Idle);

            _subMachine.AddTransition(
                CharacterStateIds.Idle,
                CharacterStateIds.Move,
                new FuncCondition(() => !BB.IsHit && !BB.AttackPressed && BB.MoveInput.sqrMagnitude > sqrThreshold));

            _subMachine.AddTransition(
                CharacterStateIds.Move,
                CharacterStateIds.Idle,
                new FuncCondition(() => !BB.IsHit && !BB.AttackPressed && BB.MoveInput.sqrMagnitude <= sqrThreshold));
        }

        /// <summary> 父状态不独立计时，计时由当前子状态负责。 </summary>
        protected override bool TrackStateTime => false;

        public string ActiveChildStateName => _subMachine.CurrentStateName;

        protected override void OnStateEnter()
        {
            BB.IsGrounded = true;
            BB.CanMove = true;
            BB.CanRotate = true;
            _subMachine.Start();
        }

        protected override void OnStateTick(float deltaTime)
        {
            _subMachine.Tick(deltaTime);
        }

        protected override void OnStateExit()
        {
            BB.IsGrounded = false;
            _subMachine.Stop();
        }
    }
}