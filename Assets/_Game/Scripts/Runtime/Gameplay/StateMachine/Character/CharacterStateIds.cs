namespace GameDemo.Runtime.Gameplay.StateMachine.Character
{
    public static class CharacterStateIds
    {
        //组织状态
        public const string Grounded = "Grounded";
        public const string Combat = "Combat";
        public const string Common = "Common";
        
        //Grounded的子状态
        public const string Idle = "Idle";
        public const string Move = "Move";
        public const string Dash = "Dash";

        //Attack的子状态
        public const string Attack = "Attack";
        public const string AttackStartup = "AttackStartup";
        public const string AttackActive = "AttackActive";
        public const string AttackRecovery = "AttackRecovery";
        
        //Common或者打断的子状态
        public const string Hit = "Hit";
        public const string Dead = "Dead";
    }
}