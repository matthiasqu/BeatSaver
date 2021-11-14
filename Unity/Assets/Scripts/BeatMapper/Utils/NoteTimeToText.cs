using BeatMapper;
using UnityEngine;
using Utils;

public class NoteTimeToText : MonoBehaviour
{
    [SerializeField] private TextSetter noteTimeTextSetter;
    [SerializeField] private TextSetter lifetimeTextSetter;

    private float lifetime;

    private void Update()
    {
        lifetime += Time.deltaTime;
        lifetimeTextSetter.SetText(lifetime);
    }

    public void SetNoteTimeText(NoteEvent note)
    {
        noteTimeTextSetter.SetText(note._time);
    }
}