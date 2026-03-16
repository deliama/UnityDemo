using UnityEngine;
using Game.Gameplay.StateMachine.Character;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character.Combat
{
    /// <summary>
    /// Attack 父状态：统一攻击期间公共能力锁，并在内部驱动 Startup/Active/Recovery。
    /// </summary>
    public sealed class AttackState : CharacterStateBase
    {
        private readonly AttackStartupState _startupState;
        private readonly AttackActiveState _activeState;
        private readonly AttackRecoveryState _recoveryState;
        private readonly HfsmMachine _subMachine;

        public AttackState(
            CharacterStateContext context,
            float startupDuration,
            float activeDuration,
            float recoveryDuration)
            : base(CharacterStateIds.Attack, context)
        {
            _startupState = new AttackStartupState(context, startupDuration);
            _activeState = new AttackActiveState(context, activeDuration);
            _recoveryState = new AttackRecoveryState(context, recoveryDuration);

            _subMachine = new HfsmMachine();
            _subMachine.Register(_startupState);
            _subMachine.Register(_activeState);
            _subMachine.Register(_recoveryState);
            _subMachine.SetDefaultState(CharacterStateIds.AttackStartup);

            _subMachine.AddTransition(
                CharacterStateIds.AttackStartup,
                CharacterStateIds.AttackActive,
                new FuncCondition(() => _startupState.IsFinished()));

            _subMachine.AddTransition(
                CharacterStateIds.AttackActive,
                CharacterStateIds.AttackRecovery,
                new FuncCondition(() => _activeState.IsFinished()));

            _subMachine.AddTransition(
                CharacterStateIds.AttackRecovery,
                CharacterStateIds.AttackStartup,
                new FuncCondition(() => BB.AttackPressed));
        }

        /// <summary> 父状态不独立计时，计时由当前子状态负责。 </summary>
        protected override bool TrackStateTime => false;

        public string ActiveChildStateName => _subMachine.CurrentStateName;

        public bool ShouldExitToMove(float sqrMoveThreshold)
        {
            return _subMachine.CurrentStateName == CharacterStateIds.AttackRecovery &&
                   _recoveryState.IsFinished() &&
                   !BB.AttackPressed &&
                   BB.MoveInput.sqrMagnitude > sqrMoveThreshold;
        }

        public bool ShouldExitToIdle(float sqrMoveThreshold)
        {
            return _subMachine.CurrentStateName == CharacterStateIds.AttackRecovery &&
                   _recoveryState.IsFinished() &&
                   !BB.AttackPressed &&
                   BB.MoveInput.sqrMagnitude <= sqrMoveThreshold;
        }

        protected override void OnStateEnter()
        {
            // 进入攻击父状态时消耗输入脉冲，避免重复触发。
            BB.AttackPressed = false;
            BB.CanMove = false;
            BB.CanRotate = true;
            BB.CanBeInterrupted = true;

            BB.AttackPerformed = false;
            BB.AttackWindowOpened = false;

            BB.DesiredMoveDirection = Vector3.zero;
            BB.MoveSpeed = 0f;

            _subMachine.Start(CharacterStateIds.AttackStartup);
        }

        protected override void OnStateTick(float deltaTime)
        {
            _subMachine.Tick(deltaTime);
        }

        protected override void OnStateExit()
        {
            _subMachine.Stop();

            BB.CanMove = true;
            BB.CanRotate = true;

            BB.AttackPressed = false;
            BB.AttackWindowOpened = false;
            BB.AttackPerformed = false;
        }
    }
}