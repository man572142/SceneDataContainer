using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace MiProduction.Extension
{
	public static class EditorScriptingExtension
	{
		/// <summary>
		/// 取得目前繪製的那一行的Rect，取完自動迭代至下行 (執行順序將會決定繪製的位置)
		/// </summary>
		/// <param name="drawer"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static Rect GetRectAndIterateLine(IEditorDrawer drawer, Rect position)
		{
			Rect newRect = new Rect(position.x, position.y + drawer.SingleLineSpace * drawer.DrawLineCount, position.width, EditorGUIUtility.singleLineHeight);
			drawer.DrawLineCount++;

			return newRect;
		}

		/// <summary>
		/// 將Rect依指定比例水平拆分為兩個Rect
		/// </summary>
		/// <param name="origin">原始Rect</param>
		/// <param name="firstRatio">第一個Rect的比例</param>
		/// <param name="gap">兩者的間隔</param>
		/// <param name="rect1">輸出的第一個Rect</param>
		/// <param name="rect2">輸出的第二個Recr</param>
		public static void SplitRectHorizontal(Rect origin, float firstRatio, float gap, out Rect rect1, out Rect rect2)
		{
			float halfGap = gap * 0.5f;
			rect1 = new Rect(origin.x, origin.y, origin.width * firstRatio - halfGap, origin.height);
			rect2 = new Rect(rect1.xMax + gap, origin.y, origin.width * (1 - firstRatio) - halfGap, origin.height);
		}

		/// <summary>
		/// 將Rect依指定比例水平拆分為相應數量的Rect
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="allRatio"></param>
		/// <param name="gap"></param>
		/// <param name="outputRects"></param>
		/// <returns></returns>
		public static bool TrySplitRectHorizontal(Rect origin, float[] allRatio, float gap, out Rect[] outputRects)
		{
			if (allRatio.Sum() != 1)
			{
				Debug.LogError("[Editor] Split ratio's sum should be 1");
				outputRects = null;
				return false;
			}

			Rect[] results = new Rect[allRatio.Length];

			for(int i = 0; i < results.Length;i++)
			{
				float offsetWidth = i == 0 || i == results.Length - 1 ? gap : gap * 0.5f;
				float newWidth = origin.width * allRatio[i] - offsetWidth;
				float newX = i > 0 ? results[i - 1].xMax + gap : origin.x;
				results[i] = new Rect(newX, origin.y, newWidth , origin.height);
			}
			outputRects = results;
			return true;
		}

		/// <summary>
		/// 將Rect依指定比例垂直拆分為兩個Rect
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="firstRatio"></param>
		/// <param name="gap"></param>
		/// <param name="rect1"></param>
		/// <param name="rect2"></param>
		public static void SplitRectVertical(Rect origin, float firstRatio, float gap, out Rect rect1, out Rect rect2)
		{
			float halfGap = gap * 0.5f;
			rect1 = new Rect(origin.x, origin.y, origin.width, origin.height * firstRatio - halfGap);
			rect2 = new Rect(origin.x, rect1.yMax + gap, origin.width, origin.height * (1 - firstRatio) - halfGap);
		}

		/// <summary>
		/// 將Rect依指定比例水平拆分為相應數量的Rect
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="allRatio"></param>
		/// <param name="gap"></param>
		/// <param name="outputRects"></param>
		/// <returns></returns>
		public static bool TrySplitRectVertical(Rect origin, float[] allRatio, float gap, out Rect[] outputRects)
		{
			if (allRatio.Sum() != 1)
			{
				Debug.LogError("[Editor] Split ratio's sum should be 1");
				outputRects = null;
				return false;
			}

			Rect[] results = new Rect[allRatio.Length];

			for (int i = 0; i < results.Length; i++)
			{
				float offsetHeight = i == 0 || i == results.Length - 1 ? gap : gap * 0.5f;
				float newHeight = origin.height * allRatio[i] - offsetHeight;
				float newY = i > 0 ? results[i - 1].yMax + gap : origin.y;
				results[i] = new Rect(origin.x,newY, origin.width,newHeight);
			}
			outputRects = results;
			return true;
		}

		/// <summary>
		/// 在原始Rect當中的指定比例位置顯示新的Rect(必須比原始Rect晚繪製)
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="dissolveRatio"></param>
		/// <returns></returns>
		public static Rect DissolveHorizontal(this Rect origin,float dissolveRatio)
		{
			return new Rect(origin.xMin + dissolveRatio * origin.width, origin.y, dissolveRatio * origin.width, origin.height);
		}

		public static bool TryGetPropertyObject<T>(this SerializedProperty sourceProperty, string propertyPath, out T newProperty) where T : class
		{
			newProperty = null;
			if (sourceProperty == null)
			{
				return false;
			}

			newProperty = sourceProperty.FindPropertyRelative(propertyPath)?.objectReferenceValue as T;
			return newProperty != null;
		}

		public static bool TryGetArrayElementAtIndex(this SerializedProperty sourceProperty,string arrayPropertyPath,int index,out SerializedProperty result)
		{
			result = null;
			SerializedProperty arrayProp = sourceProperty.FindPropertyRelative(arrayPropertyPath);
			if(arrayProp.isArray && arrayProp.arraySize > 0)
			{
				result = arrayProp.GetArrayElementAtIndex(index >= 0 ? index : 0);
				return true;
			}
			return false;
			
		}
	}

}