using UnityEngine;

public class Rotator : MonoBehaviour
{
    public Vector2 FacingDirection { get; private set; }
    
    public void FaceDirection(Vector2 direction)
    {
        if (direction.x > 0)
            FaceLeft();
        else if (direction.x < 0)
            FaceRight();
    }
    
    public void FaceRight()
    {
        FacingDirection = Vector2.right;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    
    public void FaceLeft()
    {
        FacingDirection = Vector2.left;
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}