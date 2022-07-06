﻿using SharpDX;

namespace Run8Tools.InternalClasses
{
	public struct VertexPositionNormalTexture : IEquatable<VertexPositionNormalTexture>
	{
		/// <summary>
		/// Initializes a new <see cref="T:SharpDX.Toolkit.Graphics.VertexPositionNormalTexture" /> instance.
		/// </summary>
		/// <param name="position">The position of this vertex.</param>
		/// <param name="normal">The vertex normal.</param>
		/// <param name="textureCoordinate">UV texture coordinates.</param>
		public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
		{
			this = default(VertexPositionNormalTexture);
			this.Position = position;
			this.Normal = normal;
			this.TextureCoordinate = textureCoordinate;
		}

		public bool Equals(VertexPositionNormalTexture other)
		{
			return this.Position.Equals(other.Position) && this.Normal.Equals(other.Normal) && this.TextureCoordinate.Equals(other.TextureCoordinate);
		}

		public override bool Equals(object obj)
		{
			return !object.ReferenceEquals(null, obj) && obj is VertexPositionNormalTexture && this.Equals((VertexPositionNormalTexture)obj);
		}

		public override int GetHashCode()
		{
			int num = this.Position.GetHashCode();
			num = (num * 397 ^ this.Normal.GetHashCode());
			return num * 397 ^ this.TextureCoordinate.GetHashCode();
		}

		public static bool operator ==(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(VertexPositionNormalTexture left, VertexPositionNormalTexture right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return string.Format("Position: {0}, Normal: {1}, Texcoord: {2}", this.Position, this.Normal, this.TextureCoordinate);
		}

		/// <summary>
		/// XYZ position.
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// The vertex normal.
		/// </summary>
		public Vector3 Normal;

		/// <summary>
		/// UV texture coordinates.
		/// </summary>
		public Vector2 TextureCoordinate;

		/// <summary>
		/// Defines structure byte size.
		/// </summary>
		// Token: 0x040003CA RID: 970
		public static readonly int Size = 32;
	}
}