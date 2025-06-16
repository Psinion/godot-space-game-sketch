namespace GodotSpaceGameSketch.Core.Utilities;

public class PID
{
    public float kp, ki, kd;
    private float previousError;

    public PID(float p, float i, float d)
    {
        kp = p;
        ki = i;
        kd = d;
    }

    public float ProcessByDeg(float currentAngle, float targetAngle, float deltaTime)
    {
        float currentError = AngleDifferenceByDeg(targetAngle, currentAngle);
        
        float p = currentError;
        float i = p * deltaTime;
        float d = -(p - previousError) / deltaTime;
        previousError = currentError;

        return p * kp + i * ki + d * kd;
    }
    
    public float ProcessByRad(float currentAngle, float targetAngle, float deltaTime)
    {
        float currentError = AngleDifferenceByRad(targetAngle, currentAngle) * PsiMath.Deg2Rad;
        
        float p = currentError;
        float i = p * deltaTime;
        float d = -(p - previousError) / deltaTime;
        previousError = currentError;

        return p * kp + i * ki + d * kd;
    }

    private float AngleDifferenceByDeg(float a, float b) {
        return (a - b + 540) % 360 - 180;   //calculate modular difference, and remap to [-180, 180]
    }
    
    private float AngleDifferenceByRad(float a, float b) {
        return ((a - b) * PsiMath.Rad2Deg + 540) % 360 - 180;
    }
}