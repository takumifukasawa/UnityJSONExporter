using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

[TrackClipType(typeof(SourceClip))] // ← このトラックが受け付けるクリップ型を宣言
[DisplayName("Source Track")] // メニューに出る名前
public class SourceTrack : TrackAsset
{
    // ランタイムで何か流したいならここでミキサー作成も可
}
