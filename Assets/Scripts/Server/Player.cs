// -------------------------------
// ?? 스크립트 이름: Player.cs
// ?? 기능: 플레이어의 위치, 무기 각도, HP 상태를 담고 관리하는 클래스
// -------------------------------

public class Player
{
    public string id;
    public float x;
    public float y;
    public float weaponAngle;
    public int hp = 100;

    public Player(string id)
    {
        this.id = id;
    }

    public void UpdateState(NetworkMessage msg)
    {
        this.x = msg.x;
        this.y = msg.y;
        this.weaponAngle = msg.weaponAngle;
    }
}
