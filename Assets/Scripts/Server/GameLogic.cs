// -------------------------------
// 📜 스크립트 이름: GameLogic.cs
// ⚔️ 기능: 무기 회전 각도를 바탕으로 적과 충돌 여부를 계산하고,
//        데미지를 적용하는 간단한 충돌 로직을 담당
// -------------------------------

using System;
using System.Collections.Generic;

public static class GameLogic
{
    public static void CheckCollisionAndApplyDamage(Player attacker, Dictionary<string, Player> players)
    {
        foreach (var target in players.Values)
        {
            if (target.id == attacker.id || target.hp <= 0)
                continue;

            // 💥 간단한 거리 기반 판정 (충돌 판단)
            float dx = target.x - attacker.x;
            float dy = target.y - attacker.y;
            float dist = MathF.Sqrt(dx * dx + dy * dy);

            if (dist < 1.5f)
            {
                target.hp -= 10;
                Console.WriteLine($"💥 {attacker.id}가 {target.id}를 공격함! HP: {target.hp}");
            }
        }
    }
}
