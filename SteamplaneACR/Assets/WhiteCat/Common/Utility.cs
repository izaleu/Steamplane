using System;
using System.IO;
using System.Collections;
using UnityEngine;

namespace WhiteCat
{
	/// <summary>
	/// 实用工具
	/// </summary>
	public struct Utility
	{
		/// <summary> 2 的平方根 </summary>
		public const float sqrt2 = 1.414214f;

		/// <summary> 3 的平方根 </summary>
		public const float sqrt3 = 1.732051f;

		/// <summary> 单位矩阵 </summary>
		public static readonly Matrix4x4 identityMatrix = Matrix4x4.identity;


		/// <summary> 交换两个变量的值 </summary>
		public static void Swap<T>(ref T a, ref T b)
		{
			T c = a; a = b; b = c;
		}


		/// <summary> 判断集合是否为 null 或元素个数为 0 </summary>
		public static bool IsNullOrEmpty(ICollection collection)
		{
			return collection == null || collection.Count == 0;
		}


		/// <summary> 将欧拉角转换到 [0, 360) </summary>
		public static float StandardizeEulerAngle(float a)
		{
			return (360 + a % 360) % 360;
		}


		/// <summary>
		/// 根据指定的插值方法执行插值
		/// </summary>
		public static float Interpolate(float from, float to, float t, Func<float, float> interpolate)
		{
			return interpolate(Mathf.Clamp01(t)) * (to - from) + from;
		}


		/// <summary>
		/// 根据指定的插值方法执行插值
		/// </summary>
		public static Vector3 Interpolate(Vector3 from, Vector3 to, float t, Func<float, float> interpolate)
		{
			return interpolate(Mathf.Clamp01(t)) * (to - from) + from;
		}


		/// <summary> 分三阶段的插值 </summary>
		/// <param name="t"> [0, 1] 范围的值 </param>
		/// <param name="t1"> 第一个时间点 </param>
		/// <param name="t2"> 第二个时间点 </param>
		/// <param name="interpolate"> 传入一个在 [0, 1] 上的插值方法 </param>
		/// <returns> [0, t1) 返回插值方法前一半，(t2, 1] 返回插值方法后一半，[t1, t2] 返回 0.5 处的值 </returns>
		public static float InterpolateInThreePhases(float t, float t1, float t2, Func<float, float> interpolate)
		{
			if (t2 < t1) Utility.Swap(ref t1, ref t2);

			if (t < t1) return interpolate(t / t1 * 0.5f);

			if (t > t2) return interpolate((t - t2) / (1.0f - t2) * 0.5f + 0.5f);

			return interpolate(0.5f);
		}


		/// <summary>
		/// 计算基数样条插值
		/// </summary>
		public static float CardinalSpline(float p0, float p1, float p2, float p3, float t, float tension = 0.5f)
		{
			return p1
				+ (p2 - p0) * tension * t
				+ ((p2 - p1) * 3 - (p3 - p1) * tension - (p2 - p0) * tension * 2) * t * t
				+ ((p3 - p1) * tension - (p2 - p1) * 2 + (p2 - p0) * tension) * t * t * t;
		}


		/// <summary> 同时设置 Unity 时间缩放和 FixedUpdate 频率 </summary>
		/// <param name="timeScale">要设置的时间缩放</param>
		/// <param name="fixedFrequency">要设置的 FixedUpdate 频率</param>
		public static void SetTimeScaleAndFixedFrequency(float timeScale, float fixedFrequency)
		{
			Time.timeScale = timeScale;
			Time.fixedDeltaTime = timeScale / fixedFrequency;
		}


		/// <summary> 将 RGBA 格式的整数转换为 Color 类型 </summary>
		public static Color IntRGBAToColor(int rgba)
		{
			return new Color(
				(rgba >> 24) / 255.0f,
				((rgba >> 16) & 0xFF) / 255.0f,
				((rgba >> 8) & 0xFF) / 255.0f,
				(rgba & 0xFF) / 255.0f);
		}


		/// <summary> 将 RGB 格式的整数转换为 Color 类型 </summary>
		public static Color IntRGBToColor(int rgb)
		{
			return new Color(
				((rgb >> 16) & 0xFF) / 255.0f,
				((rgb >> 8) & 0xFF) / 255.0f,
				(rgb & 0xFF) / 255.0f);
		}


		#region 矩阵操作

		/// <summary> 从矩阵中获取位置 </summary>
		public static Vector3 GetPositionOfMatrix(ref Matrix4x4 matrix)
		{
			return new Vector3(matrix.m03, matrix.m13, matrix.m23);
		}


		/// <summary> 设置矩阵的位置 </summary>
		public static void SetPositionOfMatrix(ref Matrix4x4 matrix, Vector3 position)
		{
			matrix.m03 = position.x;
			matrix.m13 = position.y;
			matrix.m23 = position.z;
		}


		/// <summary> 从矩阵中获取旋转 </summary>
		public static Quaternion GetRotationOfMatrix(ref Matrix4x4 matrix)
		{
			return Quaternion.LookRotation(
				new Vector3(matrix.m02, matrix.m12, matrix.m22),
				new Vector3(matrix.m01, matrix.m11, matrix.m21)
				);
		}


		/// <summary> 从矩阵中获取缩放 </summary>
		public static Vector3 GetScaleOfMatrix(ref Matrix4x4 matrix)
		{
			return new Vector3(
				new Vector4(matrix.m00, matrix.m10, matrix.m20, matrix.m30).magnitude,
				new Vector4(matrix.m01, matrix.m11, matrix.m21, matrix.m31).magnitude,
				new Vector4(matrix.m02, matrix.m12, matrix.m22, matrix.m32).magnitude
				);
		}

		#endregion
	}
}
