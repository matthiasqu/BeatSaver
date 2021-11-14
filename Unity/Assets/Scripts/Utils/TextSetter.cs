using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    /// <summary>
    ///     Sets text the text content on either a TMP_Text or UI Text asset.
    /// </summary>
    public class TextSetter : MonoBehaviour
    {
        /// <summary>
        ///     Whether or not to use a TextMeshPro asset.
        /// </summary>
        [SerializeField] private bool useTMP;

        /// <summary>
        ///     The TextMeshPro asset to set.
        /// </summary>
        [SerializeField] [ShowIf("@useTMP")] private TMP_Text tmpText;

        /// <summary>
        ///     The standard UnityUI text asset to set.
        /// </summary>
        [SerializeField] [ShowIf("@!useTMP")] private Text textElement;

        /// <summary>
        ///     Whether to prepend new incoming texts to the existing or to replace it.
        /// </summary>
        [SerializeField]
        [Tooltip("Choose whether or not to add all incoming text as new lines at the top of the field.")]
        private bool incremental;

        /// <summary>
        ///     Whether to append rather than prepend new text.
        /// </summary>
        [SerializeField] [Tooltip("Choose this if you wish to append the existing text rather than prepending it.")]
        private bool append;

        /// <summary>
        ///     Maximum characters present in the text asset's text field. Any overflowing characters are discarded.
        /// </summary>
        [SerializeField] private int maxRetainedCharacters = 500;

        /// <summary>
        ///     The default text to set to the asset.
        /// </summary>
        [SerializeField] private string text = "Default Text";


        /// <summary>
        ///     Sets the text representation of <see cref="text" /> to the assigned text component.
        /// </summary>
        /// <param name="text">The value to set.</param>
        public void SetText(string text)
        {
            Set(text);
        }

        /// <summary>
        ///     Sets the text representation of <see cref="text" /> to the assigned text component.
        /// </summary>
        /// <param name="text">The value to set.</param>
        public void SetText(int text)
        {
            Set(text.ToString());
        }

        /// <summary>
        ///     Sets the text representation of <see cref="text" /> to the assigned text component.
        /// </summary>
        /// <param name="text">The value to set.</param>
        public void SetText(float text)
        {
            Set(text.ToString());
        }

        /// <summary>
        ///     Sets text when the application is playing. Decides upon the usage of a TMP_Text or UnityUI Text asset.
        /// </summary>
        /// <param name="s"></param>
        private void Set(string s)
        {
            if (!Application.isPlaying) return;
            if (useTMP)
            {
                if (tmpText == null)
                {
                    Debug.LogWarning("[TextSetter] Should use TMP, but none is assigned");
                    tmpText = GetComponentInChildren<TMP_Text>();
                    if (tmpText == null)
                        Debug.LogError(
                            $"[TexteStter] Can't find a tmp text asset on {gameObject.name} or its children.");
                }
            }
            else if (textElement == null)
            {
                Debug.LogWarning("[TextSetter] Should use UI text, but none is assigned");
                textElement = GetComponentInChildren<Text>();
                if (textElement == null)
                    Debug.LogError($"[TextSetter] Can't find a UI text asset on {gameObject.name} or its children.");
                return;
            }

            var oldText = useTMP ? tmpText.text : textElement.text;

            var newText = "";
            if (incremental)
                newText =
                    $"{s}\n{oldText.Substring(0, oldText.Length < maxRetainedCharacters ? oldText.Length - 1 : maxRetainedCharacters)}";
            else newText = $"{text}{s}";

            if (useTMP) tmpText.text = newText;
            else textElement.text = newText;
        }
    }
}