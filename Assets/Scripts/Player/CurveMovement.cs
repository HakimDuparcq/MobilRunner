using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Curve Movement")]
public class CurveMovement : ScriptableObject
{
    [Range(0.1f, 8)] public float sideDistance;

    [Header("Switch Side Settings")]
    [Tooltip("X : Time, Y[0,1]* : Position X Player")]
    public AnimationCurve switchSideCurve;
    public float speedMax1Side, speedMax2Sides;
    [HideInInspector] public float switchSideTimer;

    [Header("Jump Settings")]
    [Tooltip("X : Time, Y : Position Y Player")]
    public AnimationCurve jumpCurve;
    [HideInInspector] public float jumpTimer;

    [Header("Down Settings")]
    public AnimationCurve downCurve;
    [HideInInspector] public float downTimer;

    [Header("Roll Settings")]
    public AnimationCurve rollCurve;
    [HideInInspector] public float rollTimer;



}
