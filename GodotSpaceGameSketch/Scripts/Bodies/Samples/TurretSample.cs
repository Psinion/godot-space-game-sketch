using Godot;
using GodotSpaceGameSketch.Bodies.Enums;
using GodotSpaceGameSketch.Core;
using GodotSpaceGameSketch.Core.Utilities;
using GodotSpaceGameSketch.Pooling.Enums;

namespace GodotSpaceGameSketch.Bodies.Samples;

public class TurretSample
{
    public WeaponType weaponType;
    public TurretType turretType;
    public PackedScene sample;

    public int attackRadius;
    public float fireRate;
    public float rotationSpeed;
    
    public float projectileSpeed;
    
    // На сколько времени предсказываем траекторию цели
    public float leadPredictionFactor;
    
    // Максимальный угол для предсказания
    public float maxLeadAngle;

    public FlyweightType flyweightType;
    
    public bool hasConstraintsHorizontal;
    public float minPivotAngleHorizontal;
    public float maxPivotAngleHorizontal;
    
    public bool hasConstraintsVertical;
    public float minPivotAngleVertical;
    public float maxPivotAngleVertical;

    public int health;
    
    public TurretSample(
        WeaponType weaponType, 
        TurretType turretType, 
        PackedScene sample, 
        int attackRadius, 
        float fireRate,
        float rotationSpeed,
        float projectileSpeed,
        float leadPredictionFactor,
        float maxLeadAngle,
        int health,
        int minPivotAngleHorizontal = 180,
        int maxPivotAngleHorizontal = 180,
        int minPivotAngleVertical = 180,
        int maxPivotAngleVertical = 180
        )
    {
        this.weaponType = weaponType;
        this.turretType = turretType;
        this.sample = sample;
        this.attackRadius = attackRadius;
        this.fireRate = fireRate;
        this.rotationSpeed = rotationSpeed * GlobalConstants.GodotCoordModifier;
        
        this.projectileSpeed = projectileSpeed;
        this.leadPredictionFactor = leadPredictionFactor;
        this.maxLeadAngle = maxLeadAngle * PsiMath.Deg2Rad;

        this.health = health;
        
        this.hasConstraintsHorizontal = minPivotAngleHorizontal != 180 || maxPivotAngleHorizontal != 180;
        this.minPivotAngleHorizontal = minPivotAngleHorizontal * PsiMath.Deg2Rad;
        this.maxPivotAngleHorizontal = maxPivotAngleHorizontal * PsiMath.Deg2Rad;
        this.hasConstraintsVertical = minPivotAngleVertical != 180 || maxPivotAngleVertical != 180;
        this.minPivotAngleVertical = minPivotAngleVertical * PsiMath.Deg2Rad;
        this.maxPivotAngleVertical = maxPivotAngleVertical * PsiMath.Deg2Rad;
    }
}