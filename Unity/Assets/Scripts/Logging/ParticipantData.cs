using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Logging
{
    [Serializable]
    [CreateAssetMenu(order = 0, fileName = "ParticipantData", menuName = "_custom/ParticipantData")]
    public class ParticipantData : ScriptableObject
    {
        [UsedImplicitly] public int id;
        [UsedImplicitly] public string date = DateTime.Now.ToShortDateString();
        public List<Velocity> velocities;
        public List<LogBlock> detectedBlocks;

        [HideInInspector] public string group = "control";
        private Group _condition;

        [NonSerialized] public UnityEvent<Group> onGroupChanged = new UnityEvent<Group>();
        [NonSerialized] public UnityEventInt onIdChanged = new UnityEventInt();

        public Group Condition
        {
            get => _condition;
            set
            {
                _condition = value;
                PlayerPrefs.SetInt("Condition", (int) value);
                group = value.ToString(); // for serialization purposes
            }
        }

        public void AddBlock(GameObject n)
        {
            detectedBlocks ??= new List<LogBlock>(400);
            detectedBlocks.Add(new LogBlock(n));
        }

        public void AddVelocity(Hand hand, float velocity)
        {
            velocities ??= new List<Velocity>(19000);
            velocities.Add(new Velocity
                {hand = hand.ToString(), value = velocity, time = Time.timeSinceLevelLoad});
        }

        public void IncrementID()
        {
            id++;
            onIdChanged.Invoke(id);
            PlayerPrefs.SetInt("ID", id);
        }

        public void DecrementID()
        {
            id--;
            onIdChanged.Invoke(id);
            PlayerPrefs.SetInt("ID", id);
        }

        public void UpdateCondition(int condition)
        {
            Condition = (Group) condition;
            Debug.Log($"[ParticipantData] Group changed to {Condition}");
            onGroupChanged.Invoke(Condition);
        }

        public void Clear()
        {
            detectedBlocks.Clear();
        }
    }

    [Serializable]
    public class Velocity
    {
        public string hand;
        public float value;
        public float time;
    }

    [Serializable]
    public enum Hand
    {
        left,
        right
    }

    [Serializable]
    public enum Group
    {
        control = 0,
        experimental = 1
    }
}