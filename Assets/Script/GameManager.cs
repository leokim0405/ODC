using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 필요합니다.

public class GameManager : MonoBehaviour
{
    // 게임이 시작되고 돌이 한 번이라도 있었는지 확인하는 변수
    private bool hasStonesExisted = false;

    void Update()
    {
        // "Stone" 태그를 가진 모든 게임 오브젝트를 찾습니다.
        GameObject[] stones = GameObject.FindGameObjectsWithTag("Stone");

        // 처음 돌을 발견하면, 게임이 정상적으로 시작되었다고 판단합니다.
        if (stones.Length > 0)
        {
            hasStonesExisted = true;
        }

        // 돌이 한 번이라도 존재했었는데, 이제 하나도 없다면 게임 종료!
        if (hasStonesExisted && stones.Length == 0)
        {
            // EndScene을 로드합니다.
            SceneManager.LoadScene("EndScene");
        }
    }
}
