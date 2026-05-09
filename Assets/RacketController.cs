using UnityEngine;

public class RacketRotation : MonoBehaviour
{
    public enum RotationAxis { X, Y, Z }

    [Header("どの軸を回転させる？")]
    [SerializeField] private RotationAxis axisToRotate = RotationAxis.X;

    [Header("1目盛りで何度動かすか")]
    [SerializeField] private float angleStep = 10f;

    private float currentAngle = 0f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // ホイールの回転方向に応じて角度を加減
            float direction = (scroll > 0) ? 1f : -1f;
            currentAngle += direction * angleStep;

            // 選んだ軸に対して回転を適用
            switch (axisToRotate)
            {
                case RotationAxis.X:
                    transform.localRotation = Quaternion.Euler(currentAngle, 0, 0);
                    break;
                case RotationAxis.Y:
                    transform.localRotation = Quaternion.Euler(0, currentAngle, 0);
                    break;
                case RotationAxis.Z:
                    transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
                    break;
            }
        }
    }
}