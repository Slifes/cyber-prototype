using Godot;

partial class Stats : Node3D
{
    Player mainplayer;

    HealthStats stats;

    public override void _Ready()
    {
        stats = GetNode<HealthStats>("/root/World/UI/HealthStats");
    }

    public void setPlayer(Player player)
    {
        mainplayer = player;

        mainplayer.HealthStatusChanged += Mainplayer_HealthStatusChanged;
    }

    private void Mainplayer_HealthStatusChanged(int currentHP, int currentSP, int maxHP, int maxSP)
    {
        stats.SetCurrentHP(currentHP, maxHP);
    }
}