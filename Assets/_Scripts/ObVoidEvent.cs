using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewVoidEvent", menuName = "ObEvents/Void")]
public class ObVoidEvent : ScriptableObject
{
    public Action Message;
}
