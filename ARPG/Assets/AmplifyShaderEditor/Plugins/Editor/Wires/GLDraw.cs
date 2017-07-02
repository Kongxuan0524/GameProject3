using UnityEngine;
namespace AmplifyShaderEditor
{
	public class GLDraw
	{
		/*
		* Clipping code: http://forum.unity3d.com/threads/17066-How-to-draw-a-GUI-2D-quot-line-quot?p=230386#post230386
		* Thick line drawing code: http://unifycommunity.com/wiki/index.php?title=VectorLine
		*/
		public static Material LineMaterial = null;
		private static Shader m_shader = null;
		private static bool m_clippingEnabled;
		private static Rect m_clippingBounds;
		//private static Texture2D m_aaLineTex = null;
		//private static int m_shaderTextureId = -1;
		
		/* @ Credit: "http://cs-people.bu.edu/jalon/cs480/Oct11Lab/clip.c" */
		protected static bool ClipTest( float p, float q, ref float u1, ref float u2 )
		{
			float r;
			bool retval = true;

			if ( p < 0.0 )
			{
				r = q / p;
				if ( r > u2 )
					retval = false;
				else if ( r > u1 )
					u1 = r;
			}
			else if ( p > 0.0 )
			{
				r = q / p;
				if ( r < u1 )
					retval = false;
				else if ( r < u2 )
					u2 = r;
			}
			else if ( q < 0.0 )
				retval = false;

			return retval;
		}

		protected static bool SegmentRectIntersection( Rect bounds, ref Vector2 p1, ref Vector2 p2 )
		{
			float u1 = 0.0f, u2 = 1.0f, dx = p2.x - p1.x, dy;

			if ( ClipTest( -dx, p1.x - bounds.xMin, ref u1, ref u2 ) )
			{
				if ( ClipTest( dx, bounds.xMax - p1.x, ref u1, ref u2 ) )
				{
					dy = p2.y - p1.y;
					if ( ClipTest( -dy, p1.y - bounds.yMin, ref u1, ref u2 ) )
					{
						if ( ClipTest( dy, bounds.yMax - p1.y, ref u1, ref u2 ) )
						{
							if ( u2 < 1.0 )
							{
								p2.x = p1.x + u2 * dx;
								p2.y = p1.y + u2 * dy;
							}

							if ( u1 > 0.0 )
							{
								p1.x += u1 * dx;
								p1.y += u1 * dy;
							}
							return true;
						}
					}
				}
			}
			return false;
		}

		public static void BeginGroup( Rect position )
		{
			m_clippingEnabled = true;
			m_clippingBounds = new Rect( 0, 0, position.width, position.height );
			GUI.BeginGroup( position );
		}

		public static void EndGroup()
		{
			GUI.EndGroup();
			m_clippingBounds = new Rect( 0, 0, Screen.width, Screen.height );
			m_clippingEnabled = false;
		}

		public static void CreateMaterial()
		{
			if ( LineMaterial != null )
				return;

			m_shader = Shader.Find( "Unlit/Colored Transparent" );
			LineMaterial = new Material( m_shader );

			LineMaterial.hideFlags = HideFlags.HideAndDontSave;
			//m_shader.hideFlags = HideFlags.HideAndDontSave;

			//m_shaderTextureId = Shader.PropertyToID( "_MainTex" );
			//LineMaterial.SetPass( 0 );
			//Texture2D lineTex = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>( UnityEditor.AssetDatabase.GUIDToAssetPath( "02f71419854c0d845a930c9e0a0bf775" ) );
			//LineMaterial.SetTexture( m_shaderTextureId, lineTex );
		}

		public static void DrawLine( Vector2 start, Vector2 end, Color color, float width, Vector2 prev = default( Vector2 ), Vector2 next = default( Vector2 ), Color endColor = default( Color ) )
		{
			if ( Event.current == null || Event.current.type != EventType.repaint )
				return;

			if ( m_clippingEnabled )
				if ( !SegmentRectIntersection( m_clippingBounds, ref start, ref end ) )
					return;

			CreateMaterial();

			LineMaterial.SetPass( 0 );
			//Texture2D lineTex = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>( UnityEditor.AssetDatabase.GUIDToAssetPath( "02f71419854c0d845a930c9e0a0bf775" ) );
			//LineMaterial.SetTexture( m_shaderTextureId, lineTex );

			Vector3 startPt;
			Vector3 endPt;

			if ( width == 1 )
			{
				GL.Begin( GL.LINES );
				GL.Color( color );
				startPt = new Vector3( start.x, start.y, 0 );
				endPt = new Vector3( end.x, end.y, 0 );
				GL.Vertex( startPt );
				GL.Vertex( endPt );
			}
			else
			{
				GL.Begin( GL.QUADS );
				GL.Color( color );
				startPt = new Vector3( end.y, start.x, 0 );
				endPt = new Vector3( start.y, end.x, 0 );
				Vector3 perpendicular = ( startPt - endPt ).normalized * width;
				Vector3 v1 = new Vector3( start.x, start.y, 0 );
				Vector3 v2 = new Vector3( end.x, end.y, 0 );

				if ( prev != Vector2.zero )
				{
					startPt = new Vector3( start.y, prev.x, 0 );
					endPt = new Vector3( prev.y, start.x, 0 );
					Vector3 prevPerpendicular = ( startPt - endPt ).normalized * width;
					GL.TexCoord( new Vector3( 0, 0, 0 ) );
					GL.Vertex( v1 - Vector3.Lerp( perpendicular, prevPerpendicular, 0.5f ) );
					GL.TexCoord( new Vector3( 0, 1, 0 ) );
					GL.Vertex( v1 + Vector3.Lerp( perpendicular, prevPerpendicular, 0.5f ) );
				}
				else
				{
					GL.TexCoord( new Vector3( 0, 0, 0 ) );
					GL.Vertex( v1 - perpendicular );
					GL.TexCoord( new Vector3( 0, 1, 0 ) );
					GL.Vertex( v1 + perpendicular );
				}

				if ( endColor != default( Color ) )
					GL.Color( endColor );

				if ( next != Vector2.zero )
				{
					startPt = new Vector3( next.y, end.x, 0 );
					endPt = new Vector3( end.y, next.x, 0 );
					Vector3 nextPerpendicular = ( startPt - endPt ).normalized * width;
					GL.TexCoord( new Vector3( 1, 1, 0 ) );
					GL.Vertex( v2 + Vector3.Lerp( perpendicular, nextPerpendicular, 0.5f ) );
					GL.TexCoord( new Vector3( 1, 0, 0 ) );
					GL.Vertex( v2 - Vector3.Lerp( perpendicular, nextPerpendicular, 0.5f ) );
				}
				else
				{
					GL.TexCoord( new Vector3( 1, 1, 0 ) );
					GL.Vertex( v2 + perpendicular );
					GL.TexCoord( new Vector3( 1, 0, 0 ) );
					GL.Vertex( v2 - perpendicular );
				}
			}
			GL.End();
		}

		public static void DrawBox( Rect box, Color color, float width )
		{
			Vector2 p1 = new Vector2( box.xMin, box.yMin );
			Vector2 p2 = new Vector2( box.xMax, box.yMin );
			Vector2 p3 = new Vector2( box.xMax, box.yMax );
			Vector2 p4 = new Vector2( box.xMin, box.yMax );
			DrawLine( p1, p2, color, width );
			DrawLine( p2, p3, color, width );
			DrawLine( p3, p4, color, width );
			DrawLine( p4, p1, color, width );
		}

		public static void DrawBox( Vector2 topLeftCorner, Vector2 bottomRightCorner, Color color, float width )
		{
			Rect box = new Rect( topLeftCorner.x, topLeftCorner.y, bottomRightCorner.x - topLeftCorner.x, bottomRightCorner.y - topLeftCorner.y );
			DrawBox( box, color, width );
		}

		public static void DrawRoundedBox( Rect box, float radius, Color color, float width )
		{
			Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
			p1 = new Vector2( box.xMin + radius, box.yMin );
			p2 = new Vector2( box.xMax - radius, box.yMin );
			p3 = new Vector2( box.xMax, box.yMin + radius );
			p4 = new Vector2( box.xMax, box.yMax - radius );
			p5 = new Vector2( box.xMax - radius, box.yMax );
			p6 = new Vector2( box.xMin + radius, box.yMax );
			p7 = new Vector2( box.xMin, box.yMax - radius );
			p8 = new Vector2( box.xMin, box.yMin + radius );

			DrawLine( p1, p2, color, width );
			DrawLine( p3, p4, color, width );
			DrawLine( p5, p6, color, width );
			DrawLine( p7, p8, color, width );

			Vector2 t1, t2;
			float halfRadius = radius / 2;

			t1 = new Vector2( p8.x, p8.y + halfRadius );
			t2 = new Vector2( p1.x - halfRadius, p1.y );
			DrawBezier( p8, t1, p1, t2, color, width );

			t1 = new Vector2( p2.x + halfRadius, p2.y );
			t2 = new Vector2( p3.x, p3.y - halfRadius );
			DrawBezier( p2, t1, p3, t2, color, width );

			t1 = new Vector2( p4.x, p4.y + halfRadius );
			t2 = new Vector2( p5.x + halfRadius, p5.y );
			DrawBezier( p4, t1, p5, t2, color, width );

			t1 = new Vector2( p6.x - halfRadius, p6.y );
			t2 = new Vector2( p7.x, p7.y + halfRadius );
			DrawBezier( p6, t1, p7, t2, color, width );
		}

		public static void DrawConnectingCurve( Vector2 start, Vector2 end, Color color, float width )
		{
			Vector2 distance = start - end;

			Vector2 tangentA = start;
			tangentA.x -= ( distance / 2 ).x;
			Vector2 tangentB = end;
			tangentB.x += ( distance / 2 ).x;

			int segments = Mathf.FloorToInt( ( distance.magnitude / 20 ) * 3 );

			DrawBezier( start, tangentA, end, tangentB, color, width, segments );
		}

		public static Rect DrawBezier( Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width )
		{
			int segments = Mathf.FloorToInt( ( start - end ).magnitude / 20 ) * 3; // Three segments per distance of 20
			return DrawBezier( start, startTangent, end, endTangent, color, width, segments );
		}

		public static Rect DrawBezier( Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width, int segments )
		{
			return DrawBezier( start, startTangent, end, endTangent, color, color, width, segments );
		}

		public static Rect DrawBezier( Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color startColor, Color endColor, float width, int segments )
		{
			Vector2[] allPoints = new Vector2[ segments + 1 ];
			Color[] allColors = new Color[ segments + 1 ];

			allPoints[ 0 ] = CubeBezier( start, startTangent, end, endTangent, 0 );
			allColors[ 0 ] = startColor;

			float minX = allPoints[ 0 ].x;
			float minY = allPoints[ 0 ].y;
			float maxX = allPoints[ 0 ].x;
			float maxY = allPoints[ 0 ].y;

			for ( int i = 1; i < allPoints.Length; i++ )
			{
				allPoints[ i ] = CubeBezier( start, startTangent, end, endTangent, i / ( float ) segments );
				allColors[ i ] = Color.Lerp( startColor, endColor, i / ( float ) segments );

				minX = ( allPoints[ i ].x < minX ) ? allPoints[ i ].x : minX;
				minY = ( allPoints[ i ].y < minY ) ? allPoints[ i ].y : minY;
				maxX = ( allPoints[ i ].x > maxX ) ? allPoints[ i ].x : maxX;
				maxY = ( allPoints[ i ].y > maxY ) ? allPoints[ i ].y : maxY;
			}

			for ( int i = 0; i < allPoints.Length - 1; i++ )
			{
				if ( i == 0 )
					DrawLine( allPoints[ i ], allPoints[ i + 1 ], allColors[ i ], width, Vector2.zero, allPoints[ i + 2 ], allColors[ i + 1 ] );
				else if ( i == allPoints.Length - 2 )
					DrawLine( allPoints[ i ], allPoints[ i + 1 ], allColors[ i ], width, allPoints[ i - 1 ], Vector2.zero, allColors[ i + 1 ] );
				else
					DrawLine( allPoints[ i ], allPoints[ i + 1 ], allColors[ i ], width, allPoints[ i - 1 ], allPoints[ i + 2 ], allColors[ i + 1 ] );
			}
			return new Rect( minX, minY, ( maxX - minX ), ( maxY - minY ) );
		}

		private static Vector2 CubeBezier( Vector2 s, Vector2 st, Vector2 e, Vector2 et, float t )
		{
			float rt = 1 - t;
			float rtt = rt * t;
			return rt * rt * rt * s + 3 * rt * rtt * st + 3 * rtt * t * et + t * t * t * e;
		}

		public static void Destroy()
		{
			GameObject.DestroyImmediate( LineMaterial );
			LineMaterial = null;

			Resources.UnloadAsset( m_shader );
			m_shader = null;

			//GameObject.DestroyImmediate( m_aaLineTex );
			//m_aaLineTex = null;
		}

		//static Texture2D AALineTex
		//{
		//	get
		//	{
		//		if ( !m_aaLineTex )
		//		{
		//			m_aaLineTex = new Texture2D( 1, 3, TextureFormat.ARGB32, true );
		//			m_aaLineTex.SetPixel( 0, 0, new Color( 0, 0, 0, 0 ) );
		//			m_aaLineTex.SetPixel( 0, 1, Color.white );
		//			m_aaLineTex.SetPixel( 0, 2, new Color( 0, 0, 0, 0 ) );
		//			m_aaLineTex.Apply();
		//		}
		//		return m_aaLineTex;
		//	}
		//}
	}
}
