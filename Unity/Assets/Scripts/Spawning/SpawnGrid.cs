using System.Collections.Generic;
using System.Linq;
using BeatMapper;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace Spawning
{
    /// <summary>
    ///     A grid of <see cref="SpawnPosition" /> object in the scene. Controls the local scale of the positions, and the
    ///     margin between rows and columns of the grid.
    /// </summary>
    public class SpawnGrid : MonoBehaviour
    {
        /// <summary>
        ///     Local scale of the <see cref="SpawnPosition" /> GameObjects
        /// </summary>
        [SerializeField] [Range(.05f, 1f)] private float localScale = 1f;

        /// <summary>
        ///     The space between columns of the grid.
        /// </summary>
        [SerializeField] [Range(.05f, 1f)] private float columnSpace = 1f;

        /// <summary>
        ///     The space between rows of the grid.
        /// </summary>
        [SerializeField] [Range(.05f, 1f)] private float rowSpace = 1;

        /// <summary>
        ///     The color in which gizmos of the SpawnPositions are displayed.
        /// </summary>
        [SerializeField] private Color gizmoColor;

        /// <summary>
        ///     All <see cref="SpawnPosition" /> objects in the first column of the grid.
        /// </summary>
        [SerializeField] [ReadOnly] private List<NotePositionContainer> firstColumn = new List<NotePositionContainer>();

        /// <summary>
        ///     All <see cref="SpawnPosition" /> objects in the second column of the grid.
        /// </summary>
        [SerializeField] [ReadOnly]
        private List<NotePositionContainer> secondColumn = new List<NotePositionContainer>();

        /// <summary>
        ///     All <see cref="SpawnPosition" /> objects in the third column of the grid.
        /// </summary>
        [SerializeField] [ReadOnly] private List<NotePositionContainer> thirdColumn = new List<NotePositionContainer>();

        /// <summary>
        ///     All <see cref="SpawnPosition" /> objects in the fourth column of the grid.
        /// </summary>
        [SerializeField] [ReadOnly]
        private List<NotePositionContainer> fourthColumn = new List<NotePositionContainer>();

        /// <summary>
        ///     Parent GameObject of the <see cref="SpawnPosition" /> objects in the first (topmost) row.
        /// </summary>
        [SerializeField] [ReadOnly] private GridRow topRow;

        /// <summary>
        ///     Parent GameObject of the <see cref="SpawnPosition" /> objects in the second (middle) row.
        /// </summary>
        [SerializeField] [ReadOnly] private GridRow middleRow;

        /// <summary>
        ///     Parent GameObject of the <see cref="SpawnPosition" /> objects in the third (bottommost) row.
        /// </summary>
        [SerializeField] [ReadOnly] private GridRow bottomRow;

        /// <summary>
        ///     Event fired whenever <see cref="localScale" /> changes.
        /// </summary>
        [SerializeField] private UnityEventVector3 onScaleChanged = new UnityEventVector3();

        /// <summary>
        ///     Event fired whenever the position of the first column should change. The change is based on changes in
        ///     <see cref="localScale" />, <see cref="columnSpace" />, and <see cref="rowSpace" />.
        /// </summary>
        [SerializeField] private UnityEventVector3 onFirstColumnPositionChanged = new UnityEventVector3();

        /// <summary>
        ///     Event fired whenever the position of the second column should change. The change is based on changes in
        ///     <see cref="localScale" />, <see cref="columnSpace" />, and <see cref="rowSpace" />.
        /// </summary>
        [SerializeField] private UnityEventVector3 onSecondColumnPositionChanged = new UnityEventVector3();

        /// <summary>
        ///     Event fired whenever the position of the third column should change. The change is based on changes in
        ///     <see cref="localScale" />, <see cref="columnSpace" />, and <see cref="rowSpace" />.
        /// </summary>
        [SerializeField] private UnityEventVector3 onThirdColumnPositionChanged = new UnityEventVector3();

        /// <summary>
        ///     Event fired whenever the position of the fourth column should change. The change is based on changes in
        ///     <see cref="localScale" />, <see cref="columnSpace" />, and <see cref="rowSpace" />.
        /// </summary>
        [SerializeField] private UnityEventVector3 onFourthColumnPositionChanged = new UnityEventVector3();


        private List<SpawnPosition> spawnPositions;

        /// <summary>
        ///     Assign the grid positions and scales
        /// </summary>
        private void OnValidate()
        {
            if (Application.isPlaying) return;
            topRow = GetRow(LineLayer.Top);
            middleRow = GetRow(LineLayer.Middle);
            bottomRow = GetRow(LineLayer.Bottom);
            firstColumn = GetColumnObjects(LineIndex.First).ToList();
            secondColumn = GetColumnObjects(LineIndex.Second).ToList();
            thirdColumn = GetColumnObjects(LineIndex.Third).ToList();
            fourthColumn = GetColumnObjects(LineIndex.Fourth).ToList();

            UpdateSpawnPositions();
            ApplyScale();
            ApplyColumnSpacing();
            ApplyRowSpacing();
        }

        /// <summary>
        ///     update all spawn positions objects from the rows of objects.
        /// </summary>
        private void UpdateSpawnPositions()
        {
            spawnPositions = firstColumn.Concat(secondColumn).Concat(thirdColumn).Concat(fourthColumn)
                .Select(np => np.GetComponent<SpawnPosition>()).ToList();
        }

        /// <summary>
        ///     apply <see cref="localScale" /> to all spawn positions.
        /// </summary>
        private void ApplyScale()
        {
            var scale = new Vector3(localScale, localScale, localScale);
            spawnPositions.ForEach(i
                =>
            {
                i.transform.localScale = scale;
                i.Color = gizmoColor;
            });
            onScaleChanged.Invoke(scale);
        }

        /// <summary>
        ///     Offsets the top and bottom row from the middle row based on <see cref="rowSpace" />.
        /// </summary>
        private void ApplyRowSpacing()
        {
            // make sure local scale never exceeds rowSpace so there is no overlapping
            //if (localScale > rowSpace) rowSpace = localScale;

            var middlePosition = middleRow.transform.position;
            topRow.transform.position = middlePosition + new Vector3(0, rowSpace + localScale, 0);
            bottomRow.transform.position = middlePosition - new Vector3(0, rowSpace + localScale, 0);
        }

        /// <summary>
        ///     Offset each column from the center.
        /// </summary>
        private void ApplyColumnSpacing()
        {
            var rows = new List<GridRow> {topRow, middleRow, bottomRow};

            // for the inner two columns
            var centerXOffset = localScale / 2 + columnSpace / 2;
            // for the outer two columns
            var outerXOffset = localScale / 2 + columnSpace / 2 + (localScale + columnSpace);

            // assign offsets to all child objects
            PositionColumn(firstColumn, outerXOffset, rows);
            onFirstColumnPositionChanged.Invoke(new Vector3(outerXOffset, 0, 0));

            PositionColumn(secondColumn, centerXOffset, rows);
            onSecondColumnPositionChanged.Invoke(new Vector3(centerXOffset, 0, 0));

            PositionColumn(thirdColumn, -centerXOffset, rows);
            onThirdColumnPositionChanged.Invoke(new Vector3(-centerXOffset, 0, 0));

            PositionColumn(fourthColumn, -outerXOffset, rows);
            onFourthColumnPositionChanged.Invoke(new Vector3(-outerXOffset, 0, 0));
        }

        /// <summary>
        ///     Sets the position of each <see cref="NotePositionContainer" /> in <see cref="positions" /> according to the
        ///     supplied offset and the row it's in.
        /// </summary>
        /// <param name="positions">The <see cref="NotePositionContainer" /> objects to place.</param>
        /// <param name="xOffset">The offset to apply on the x-axis.</param>
        /// <param name="rows">All rows in the grid.</param>
        private void PositionColumn(IEnumerable<NotePositionContainer> positions, float xOffset,
            IEnumerable<GridRow> rows)
        {
            positions.ToList().ForEach(np =>
            {
                var row = rows.Where(r => r.Layer == np.NotePosition.LineLayer).ToArray();
                var x = xOffset;
                var z = row.First().transform.position.z;
                var y = row.First().transform.position.y;
                var pos = new Vector3(x, y, z);
                np.transform.position = pos;
            });
        }

        /// <summary>
        ///     Returns the first gameobject with the assignes <see cref="LineLayer" />.
        /// </summary>
        /// <param name="layer">The <see cref="LineLayer" /> value to look for</param>
        /// <returns>A <see cref="GridRow" /> object of the supplied layer.</returns>
        private GridRow GetRow(LineLayer layer)
        {
            return GetComponentsInChildren<GridRow>().First(r => r.Layer == layer);
        }

        /// <summary>
        ///     Gets all objects of the specified LineIndex.
        /// </summary>
        /// <param name="lineIndex">The <see cref="LineIndex" /> of the column objects to return.</param>
        /// <returns>All <see cref="SpawnPosition" /> objects that have the supplied <see cref="LineIndex" /></returns>
        private IEnumerable<NotePositionContainer> GetColumnObjects(LineIndex lineIndex)
        {
            return GetComponentsInChildren<NotePositionContainer>()
                .Where(n => n.NotePosition.LineIndex == lineIndex);
        }
    }
}