using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace BeatMapper.Utils
{
    /// <summary>
    ///     Extension methods used by the BeatMapper namespace.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Converts ticks from a BeatSaber song mapping to time in MS using the song's beats per minute.
        /// </summary>
        /// <param name="f">The float to convert.</param>
        /// <param name="BPM">The song's beats per minute.</param>
        /// <returns>The corresponding time in MS.</returns>
        private static float TickToMs(this float f, float BPM)
        {
            return (60 / BPM * f).SecondsToMs();
        }

        private static float TickToS(this float f, float BPM)
        {
            return f.TickToMs(BPM).MsToSeconds();
        }

        /// <summary>
        ///     Applies a given delay in MS to all _time fields in the given SongMapping.
        /// </summary>
        /// <param name="original">The original SongMapping</param>
        /// <param name="delay">The delay in MS to apply.</param>
        /// <returns>A new SongMapping with applied delay.</returns>
        public static SongMapping ApplyDelay(this SongMapping original, float delay)
        {
            var n = new SongMapping
            {
                Events = original.Events.ApplyDelay(delay).ToArray(),
                Notes = original.Notes.ApplyDelay(delay).ToArray(),
                Obstacles = original.Obstacles.ApplyDelay(delay).ToArray(),
                Version = original.Version.Clone() as string,
                CustomData = new CustomData
                {
                    Bookmarks = original.Bookmarks.ApplyDelay(delay).ToArray(),
                    BpmChanges = original.BpmChanges.ApplyDelay(delay).ToArray()
                }
            };
            return n;
        }

        /// <summary>
        ///     Converts a SongMapping to use MS instead of ticks in all _time fields.
        /// </summary>
        /// <param name="original">The original SongMapping to convert</param>
        /// <param name="bpm">The Beats Per Minute of the Song.</param>
        /// <returns>A new SongMapping with applied conversion.</returns>
        public static SongMapping ToTimedSongMapping(this SongMapping original, float bpm)
        {
            var n = new SongMapping
            {
                Notes = original.Notes.ToSeconds(bpm).ToArray(),
                Events = original.Events.ToSeconds(bpm).ToArray(),
                Obstacles = original.Obstacles.ToSeconds(bpm).ToArray(),
                Version = original.Version.Clone() as string,
                CustomData = new CustomData
                {
                    Bookmarks = original.Bookmarks.ToSeconds(bpm).ToArray(),
                    BpmChanges = original.BpmChanges.ToSeconds(bpm).ToArray()
                }
            };
            return n;
        }

        /// <summary>
        ///     Converts the ObstacleEvents duration in a supplied song mapping to space units.
        /// </summary>
        /// <param name="original">Original SongMapping to convert.</param>
        /// <param name="movementSpeed">The obstacle's movement speed for calculations.</param>
        /// <param name="bpm">The song's beats per minute for calculations.</param>
        /// <returns>A new song mapping with converted obstacle event durations.</returns>
        public static SongMapping ToLengthenedMapping(this SongMapping original, float movementSpeed, float bpm)
        {
            return new SongMapping
            {
                Notes = original.Notes.DeepCopy().ToArray(),
                Events = original.Events.DeepCopy().ToArray(),
                Obstacles = original.Obstacles.DurationToUnits(movementSpeed, bpm).ToArray(),
                Version = original.Version.Clone() as string,
                CustomData = new CustomData
                {
                    _bookmarks = original.Bookmarks.DeepCopy().ToArray(),
                    _BPMChanges = original.BpmChanges.DeepCopy().ToArray(),
                    _time = original.CustomData._time
                }
            };
        }

        /// <summary>
        ///     Turn the obstacles duration (in seconds!) into a length in units.
        /// </summary>
        /// <param name="obstacles">The obstacles to convert.</param>
        /// <param name="movementSpeed">The movement speed of the obstacle.</param>
        /// <returns>A converted enumerable with cloned values.</returns>
        public static IEnumerable<ObstacleEvent> DurationToUnits(this IEnumerable<ObstacleEvent> obstacles,
            float movementSpeed, float bpm)
        {
            var tickLength = TickToS(1, bpm);
            var singleTickLength = tickLength * movementSpeed;

            var obstacleEvents = obstacles as ObstacleEvent[] ?? obstacles.ToArray();
            var clone = obstacleEvents.DeepCopy().ToArray();
            foreach (var obstacle in clone) obstacle._duration *= singleTickLength;
            Debug.Log($"[DurationToUnity] Converted {obstacles.First()._duration} to {clone.First()._duration}");
            return clone;
        }

        /// <summary>
        ///     Applies the given delay in MS to all _time fields of the provided IEnumerable<TimedSongEvent>
        /// </summary>
        /// <param name="events">The events to delay</param>
        /// <param name="delay">The delay to apply in MS</param>
        /// <typeparam name="T">Objects of type TimedSongEvent implementing ICloneable</typeparam>
        /// <returns>A new List of TimedSongEvents with applied delay</returns>
        private static IEnumerable<T> ApplyDelay<T>(this IEnumerable<T> events, float delay)
            where T : TimedSongEvent, ICloneable
        {
            var clone = events.DeepCopy().ToArray();
            foreach (var c in clone) c._time += delay;
            return clone;
        }

        /// <summary>
        ///     Converts the _time field of an IEnumerable of TimedSongEvents from ticks to ms
        /// </summary>
        /// <param name="events">IEnumerable of TimedSongEvents</param>
        /// <param name="bpm">Beats Per minute of the song</param>
        /// <typeparam name="T">TimedSongEvent implementing ICloneable</typeparam>
        /// <returns>A deep copy of the IEnumerable provided with converted _time fields</returns>
        private static IEnumerable<T> ToMs<T>(this IEnumerable<T> events, float bpm)
            where T : TimedSongEvent, ICloneable
        {
            return events.DeepCopy().ForEach(clone => clone._time = clone._time.TickToMs(bpm));
        }

        /// <summary>
        ///     Converts the _time field of an IEnumerable of TimedSongEvents from ticks to ms
        /// </summary>
        /// <param name="events">IEnumerable of TimedSongEvents</param>
        /// <param name="bpm">Beats Per minute of the song</param>
        /// <typeparam name="T">TimedSongEvent implementing ICloneable</typeparam>
        /// <returns>A deep copy of the IEnumerable provided with converted _time fields</returns>
        private static IEnumerable<T> ToSeconds<T>(this IEnumerable<T> events, float bpm)
            where T : TimedSongEvent, ICloneable
        {
            var copy = events.DeepCopy().ToArray();
            foreach (var x in copy) x._time = x._time.TickToS(bpm);
            if (events.Any())
                Debug.Log($"[ToSeconds] Copy: {copy.Select(c => c._time.ToString()).Aggregate((x, y) => $"{x}, {y}")}");
            return copy;
        }

        /// <summary>
        ///     Returns a deep copy of the suuplied IEnumerable
        /// </summary>
        /// <param name="listToClone">Object that implements ICloneable</param>
        /// <typeparam name="T">ICloneable implementation</typeparam>
        /// <returns>A deep copy of the provided IEnumerable</returns>
        /// TODO: move this to the general Utils namespace.
        public static IEnumerable<T> DeepCopy<T>(this IEnumerable<T> listToClone) where T : class, ICloneable
        {
            var toClone = listToClone as T[] ?? listToClone.ToArray();
            if (!toClone.Any()) return new T[0];
            return toClone.Select(item => item.Clone() as T);
        }

        public static float MsToSeconds(this float f)
        {
            return f / 1000;
        }

        public static float SecondsToMs(this float f)
        {
            return f * 1000;
        }

        public static IEnumerable<T> GetCurrentEvents<T>(this IEnumerable<T> timedEvents, float time, float oldTime)
            where T : TimedSongEvent
        {
            var songEvents = timedEvents as T[] ?? timedEvents.ToArray();
            var matches = new List<T>();
            foreach (var @event in songEvents)
                if (@event._time > oldTime && @event._time <= time)
                    matches.Add(@event.Clone() as T);

            return matches;
        }

        /// <summary>
        ///     Converts a given <see cref="CutDirection" /> to a Vector3 for rotation purposes.
        /// </summary>
        /// <param name="cutDirection">The cut direction to convert.</param>
        /// <returns>A Vector3 where the z component is changed accordingly.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an invalid cut direction is supplied.</exception>
        public static Vector3 ToRotationVector(this CutDirection cutDirection)
        {
            var component = 0;
            switch (cutDirection)
            {
                case CutDirection.Down:
                    component = 45 * 0;
                    break;
                case CutDirection.DownLeft:
                    component = 45 * 1;
                    break;
                case CutDirection.Left:
                    component = 45 * 2;
                    break;
                case CutDirection.UpLeft:
                    component = 45 * 3;
                    break;
                case CutDirection.Up:
                    component = 45 * 4;
                    break;
                case CutDirection.UpRight:
                    component = 45 * 5;
                    break;
                case CutDirection.Right:
                    component = 45 * 6;
                    break;
                case CutDirection.DownRight:
                    component = 45 * 7;
                    break;
                case CutDirection.Any:
                    component = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cutDirection), cutDirection,
                        $"The supplied cut direction: {cutDirection} is not valid.");
            }

            return new Vector3(0, 0, component);
        }

        public static CutDirection ToCutDirection(this Vector3 noteRotation)
        {
            var z = (360 + noteRotation.z) % 360;
            /*if (Math.Abs(z - 45f * 0) < float.Epsilon) return CutDirection.Up;
            if (Math.Abs(z - 45f * 1) < float.Epsilon) return CutDirection.UpRight;
            if (Math.Abs(z - 45f * 2) < float.Epsilon) return CutDirection.Right;
            if (Math.Abs(z - 45f * 3) < float.Epsilon) return CutDirection.DownRight;
            if (Math.Abs(z - 45f * 4) < float.Epsilon) return CutDirection.Down;
            if (Math.Abs(z - 45f * 5) < float.Epsilon) return CutDirection.DownLeft;
            if (Math.Abs(z - 45f * 6) < float.Epsilon) return CutDirection.Left;
            if (Math.Abs(z - 45f * 7) < float.Epsilon) return CutDirection.UpLeft;*/

            if (Math.Abs(z - 45f * 0) < float.Epsilon) return CutDirection.Down;
            if (Math.Abs(z - 45f * 1) < float.Epsilon) return CutDirection.DownLeft;
            if (Math.Abs(z - 45f * 2) < float.Epsilon) return CutDirection.Left;
            if (Math.Abs(z - 45f * 3) < float.Epsilon) return CutDirection.UpLeft;
            if (Math.Abs(z - 45f * 4) < float.Epsilon) return CutDirection.Up;
            if (Math.Abs(z - 45f * 5) < float.Epsilon) return CutDirection.UpRight;
            if (Math.Abs(z - 45f * 6) < float.Epsilon) return CutDirection.Right;
            if (Math.Abs(z - 45f * 7) < float.Epsilon) return CutDirection.DownRight;
            throw new Exception("[ToCutDirection]Angle does not correspond to any CutDirection value");
        }

        public static CutDirection ToWorldCutDirection(this CutDirection innerCutDirection,
            CutDirection outerCutDirection)
        {
            var v1 = innerCutDirection.ToRotationVector().z;
            var v2 = outerCutDirection.ToRotationVector().z;
            var combinedRotations = v1 + v2;

            var rotation = Math.Round(combinedRotations) % 360;
            var dir = new Vector3(0, 0, (float) rotation).ToCutDirection();
            //Debug.Log($"[CutConverter] Inner: ({innerCutDirection}, {v1}), outer: ({outerCutDirection}), {v2}, combined: {combinedRotations}, new rotation: {rotation}, new direction: {dir}");

            return dir;
        }

        /// <summary>
        ///     Works like TakeWhile, but includes the first item that satisfies the condition.
        /// </summary>
        /// <param name="enumerable">The enumerable to take the items from</param>
        /// <param name="condition">When to stop taking items</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>a new IEnumerable including the first item that satisfies the given condition. Like TakeWhile + 1</returns>
        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> enumerable, Func<T, bool> condition)
        {
            var list = new List<T>();
            using var iter = enumerable.GetEnumerator();
            while (iter.MoveNext())
            {
                list.Add(iter.Current);
                if (condition(iter.Current)) return list;
            }

            return list;
        }

        /// <summary>
        ///     Like a Where().First() call.
        /// </summary>
        /// <param name="enumerable">The enumerable from which to take the item from</param>
        /// <param name="condition">The condition which determines the returned item</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>The first item that satisfies <see cref="condition" /></returns>
        /// <exception cref="Exception"></exception>
        public static T TakeFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> condition)
        {
            using var iter = enumerable.GetEnumerator();
            while (iter.MoveNext())
                if (condition(iter.Current))
                    return iter.Current;

            throw new Exception("No item satisfying the condition found!");
        }
    }
}