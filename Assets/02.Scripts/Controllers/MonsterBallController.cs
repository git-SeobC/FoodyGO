using System.Collections;
using UnityEngine;


namespace FoodyGo.Controllers
{
    public class MonsterBallController : MonoBehaviour
    {
        GameObject _target;
        bool _isThrowing;
        [SerializeField] private float _radius = 0.7f;
        [SerializeField] private float _bounceDamping = 0.6f;
        [SerializeField] private LayerMask _targetMask;

        Vector3 _lastVelocity;

        public void Throw(GameObject target, float arcHeight, float duration)
        {
            if (_isThrowing) return;

            _target = target;
            StartCoroutine(C_Throw(arcHeight, duration));
        }

        IEnumerator C_Throw(float arcHeight, float duration)
        {
            _isThrowing = true;

            Vector3 startPos = transform.position;
            Vector3 endPos = _target.transform.position;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                Vector3 lastPosition = transform.position;
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                float ease = Mathf.Sin(t * Mathf.PI * 0.5f); // Ease in-out sine
                Vector3 lerp = Vector3.Lerp(startPos, endPos, ease);

                float heightOffset = arcHeight * Mathf.Sin(Mathf.PI * ease);
                Vector3 targetPos = new Vector3(lerp.x, lerp.y + heightOffset, lerp.z);
                transform.position = targetPos;

                _lastVelocity = (transform.position - lastPosition) / Time.deltaTime;

                if (Physics.SphereCast(lastPosition, _radius, _lastVelocity, out RaycastHit hit, maxDistance: Vector3.Distance(transform.position, lastPosition), _targetMask))
                {
                    if (hit.transform.TryGetComponent(out MonsterController monsterController))
                    {
                        monsterController.Damage(20f);
                        _isThrowing = false;
                        StartCoroutine(C_Bounce(hit.normal));
                    }
                    yield break;
                }

                yield return null;
            }

            transform.position = endPos;
            _isThrowing = false;
        }

        // 간단한 물리 계산은 코루틴으로 하는 것이 성능에 유리할 수 있음.
        IEnumerator C_Bounce(Vector3 normal)
        {
            Destroy(gameObject, 5.0f);
            // 벡터 방향 반사 값 = Reflect
            _lastVelocity = Vector3.Reflect(_lastVelocity, normal) * _bounceDamping;

            while (true)
            {
                _lastVelocity += Physics.gravity * Time.deltaTime;
                transform.position += _lastVelocity * Time.deltaTime;
                yield return null;
            }
        }
    }
}
