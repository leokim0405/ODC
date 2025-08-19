using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HazardWall : MonoBehaviour
{
    [Header("데미지")]
    public int damage = 10;

    [Header("점멸 설정")]
    public Color hitFlashColor = new Color(1f, 0.3f, 0.3f); // 연분홍/빨강
    public float flashDuration = 0.08f;
    public int flashCount = 2;

    // Trigger를 쓰는 경우
    private void OnTriggerEnter(Collider other)
    {
        Apply(other.attachedRigidbody ? other.attachedRigidbody.gameObject : other.gameObject);
    }

    // 또는 물리 충돌을 쓰는 경우(콜라이더 IsTrigger 꺼짐)
    private void OnCollisionEnter(Collision collision)
    {
        Apply(collision.gameObject);
    }

    private void Apply(GameObject target)
    {
        if (target == null) return;

        var hp = target.GetComponent<StoneHealth>();
        if (hp == null) return;

        hp.TakeDamage(damage);
        hp.FlashHit(hitFlashColor, flashDuration, flashCount);
    }
}