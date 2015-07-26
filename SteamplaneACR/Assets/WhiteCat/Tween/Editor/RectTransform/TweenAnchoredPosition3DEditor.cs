﻿using UnityEditor;
using UnityEngine;
using WhiteCat.Internal;

namespace WhiteCat
{
	[CustomEditor(typeof(TweenAnchoredPosition3D))]
	class TweenAnchoredPosition3DEditor : TweenEditor<TweenAnchoredPosition3D>
	{
		int mask;
		Vector3 from, to;


		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			mask = target.mask;
			from = target.from;
			to = target.to;

			EditorGUI.BeginChangeCheck();
			DrawVector3Channels(ref mask, ref from, ref to);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, undoString);
				target.mask = mask;
				target.from = from;
				target.to = to;
				EditorUtility.SetDirty(target);
			}
		}
	}
}
