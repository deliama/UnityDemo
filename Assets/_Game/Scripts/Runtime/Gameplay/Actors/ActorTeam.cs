namespace GameDemo.Runtime.Gameplay.Actors
{
    /// <summary>
    /// 阵营枚举：用于命中判定时区分敌友（同阵营不造成伤害）。
    /// </summary>
    public enum ActorTeam
    {
        Neutral = 0,
        Player = 1,
        Enemy = 2
    }
}

