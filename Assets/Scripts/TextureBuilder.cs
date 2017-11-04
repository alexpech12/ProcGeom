using UnityEngine;
using System.Collections;

public class TextureBuilder {
	
	private Texture2D texture;
	
	public struct LeafCurvePoint{
		public Vector2 start;
		public Vector2 mid1;
		public Vector2 mid2;
		public Vector2 end;
	}
	
	public void SetFilterMode(FilterMode filterMode) {
		texture.filterMode = filterMode;
	}
	
	public void RandomTexture(int size) {
		texture = new Texture2D(size,size);
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				Color color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f));
				texture.SetPixel(i,j,color);
			}
		}
	}
	
	public void ColorTexture(int size, Color color) {
		texture = new Texture2D(size,size);
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				texture.SetPixel(i,j,color);
			}
		}
	}
	
	public void Color4GridTexture(int size, Color[] colors) {
		texture = new Texture2D(size,size);
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				if(i<size/2&&j<size/2) { // Quadrant 1
					texture.SetPixel(i,j,colors[0]);
				} else if(i>=size/2&&j<size/2) { // Quadrant 2
					texture.SetPixel(i,j,colors[1]);
				} else if(i<size/2&&j>=size/2) { // Quadrant 3
					texture.SetPixel(i,j,colors[2]);
				} else if(i>=size/2&&j>=size/2) { // Quadrant 4
					texture.SetPixel(i,j,colors[3]);
				}
			}
		}
	}
	
	public void AddPerlinNoise(float range, float opacity, float offset) {
		int size = texture.width;
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				Color color = texture.GetPixel(i,j);
				float noise = Mathf.PerlinNoise(((float)i/(float)size)*range,((float)j/(float)size)*range)*opacity;
				color += new Color(offset+noise,offset+noise,offset+noise,1.0f);
				texture.SetPixel(i,j,color);
			}
		}
	}
	
	public void OverlaySolidNoise(Color color,float threshold,float noiseRange,Vector2 noiseOffset,float opacity) {
		// Clamps noise to 100% above threshold and 0% below threshold.
		// Then sets 100% noise to color arg and 0% to transparent and overlays with existing texture.
		int size = texture.width;
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				float noise = Mathf.PerlinNoise(((float)i/(float)size)*noiseRange+noiseOffset.x,((float)j/(float)size)*noiseRange+noiseOffset.y);
				if (noise > threshold) {
					Color oldColor = texture.GetPixel(i,j);
					texture.SetPixel(i,j,oldColor*(1-opacity) + color*opacity);
				}
				
			}
		}
	}
	
	public void OverlayStripes(Color color, float frequency, float ratio, float angle, float opacity) {
		int size = texture.width;
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				float u = (float)i/(float)size;
				float v = (float)j/(float)size;
				bool drawColor = Mathf.Sin(frequency*2*Mathf.PI*(Mathf.Cos(angle*Mathf.Deg2Rad)*u+Mathf.Sin(angle*Mathf.Deg2Rad)*v)) + 2*ratio-1 > 0;
				
				if (drawColor) {
					Color oldColor = texture.GetPixel(i,j);
					texture.SetPixel(i,j,oldColor*(1-opacity) + color*opacity);
				}
				
			}
		}
	}
	
	public void OverlaySpots(Color color, float frequency, float ratio, float opacity) {
		int size = texture.width;
		float dotSize = (float)size/frequency;
		float dotRadius = dotSize/2.0f;
		
		
		for (int i = 0; i < frequency; i++) {
			for (int j = 0; j < frequency; j++) {
				for (int iDot = 0; iDot < dotSize; iDot++) {
					for (int jDot = 0; jDot < dotSize; jDot++) {
						bool drawColor = Mathf.Pow(dotRadius*ratio-iDot,2)+Mathf.Pow(dotRadius*ratio-jDot,2) < dotRadius*ratio*dotRadius*ratio;
						if (drawColor) {
							int u = i*(int)dotSize+iDot;
							int v = j*(int)dotSize+jDot;
							Color oldColor = texture.GetPixel(u,v);
							texture.SetPixel(u,v,oldColor*(1-opacity) + color*opacity);
						}
					}
				}
			}
		}
	}
	
	public void CutOutLeafShape(int detailMin, int detailMax,bool assumeLeafVariation) {
		// Randomize leafPoints
		// Select between 1 and 4 bezier curves to use
		int curveNum = Random.Range(detailMin,detailMax);
		//Debug.Log("curveNum = " + curveNum);
		LeafCurvePoint[] leafCurves = new LeafCurvePoint[curveNum];
		// First loop: Place start and end points
		leafCurves[0].start = new Vector2(0.5f,0.0f);
		leafCurves[curveNum-1].end = new Vector2(0.5f,1.0f);
		for (int curve = 1; curve < curveNum; curve++) {
			float randX = Random.Range(0.05f,0.40f);
			//float minY = (curve-1)/curveNum;
			//float maxY = (curve)/curveNum;
			float randY = Random.Range((float)(curve-1)/(float)(curveNum-1),(float)curve/(float)(curveNum-1));
			Vector2 newPoint = new Vector2(randX,randY);
			leafCurves[curve-1].end = newPoint;
			leafCurves[curve].start = newPoint;
		}
		// Second loop: Place midpoints
		float startRadius = Random.Range(0.1f,0.4f);
		float startAngle = Random.Range(-90.0f,90.0f)*Mathf.Deg2Rad;
		leafCurves[0].mid1 = new Vector2(Mathf.Sin(startAngle),Mathf.Cos(startAngle))*startRadius;
		float endRadius = Random.Range(0.1f,0.4f);
		float endAngle = Random.Range(-90.0f,90.0f)*Mathf.Deg2Rad;
		leafCurves[curveNum-1].mid2 = leafCurves[curveNum-1].end+new Vector2(Mathf.Sin(endAngle),-Mathf.Cos(endAngle))*endRadius;
		for (int curve = 1; curve < curveNum; curve++) {
			
			Vector2 currentPoint = leafCurves[curve-1].end;
			
			float randRadius1 = Random.Range(0.1f,0.4f);
			// Decrease angle the further out the leaf point goes, to prevent overlaps
			float randAngle1 = Random.Range(-180.0f+(0.5f-currentPoint.x)*180.0f,180.0f-(0.5f-currentPoint.x)*180.0f)*Mathf.Deg2Rad;
			Vector2 newPoint1 = new Vector2(Mathf.Cos(randAngle1),Mathf.Sin(randAngle1))*randRadius1;
			leafCurves[curve-1].mid2 = currentPoint+newPoint1;
			
			float randRadius2 = Random.Range(0.1f,0.4f);
			// Decrease angle the further out the leaf point goes, to prevent overlaps
			float randAngle2 = Random.Range(-180.0f+(0.5f-currentPoint.x)*180.0f,180.0f-(0.5f-currentPoint.x)*180.0f)*Mathf.Deg2Rad;
			Vector2 newPoint2 = new Vector2(Mathf.Cos(randAngle2),Mathf.Sin(randAngle2))*randRadius2;
			leafCurves[curve].mid1 = currentPoint+newPoint2;
		}
		
		CutOutLeafShape(leafCurves,curveNum,assumeLeafVariation);
	}
	
	public void CutOutLeafShape(LeafCurvePoint[] leafPoints,int pointNum,bool assumeLeafVariation) {
		int size = texture.width;
		
		int cutNum = assumeLeafVariation ? 4 : 1;
		for (int l = 0; l < cutNum; l++) {
			// Set cut bounds
			int cutSize = assumeLeafVariation ? size/2 : size;
			int offsetX = l==1||l==3 ? size/2 : 0;
			int offsetY = l==2||l==3 ? size/2 : 0;
			for(int c = 0; c < pointNum; c++) {
				for(float t = 0.0f; t < 1.0f; t+=1/((float)cutSize*2)) {
					Vector2 bezier = Bezier2D(leafPoints[c].start*cutSize,leafPoints[c].mid1*cutSize,leafPoints[c].mid2*cutSize,leafPoints[c].end*cutSize,t);
					bezier = new Vector2(Mathf.RoundToInt(bezier.x),Mathf.RoundToInt(bezier.y));
					for(int i = 0; i < cutSize;i++) {
						Color newColor = texture.GetPixel(i+offsetX,(int)bezier.y+offsetY);
						newColor.a = 0.0f;
						if(i<bezier.x || i>cutSize-bezier.x) {
							texture.SetPixel(i+offsetX,(int)bezier.y+offsetY,newColor);
						}
					}
				}
			}
		}
	}
	
	public void CutOutGrassShape(int frequency) {
		int size = texture.width;
		
		int bladeNum = frequency;
		Debug.Log ("Cutting out grass...");
		for(int i = 0; i < size;) {
			float randHeight = Random.Range(0,size/2);
			for(int j = 0; j < (size/bladeNum)/2; j++) {
				for(int pixel = 0; pixel < size; pixel++) {
					Color newColor = texture.GetPixel(i,pixel);
					if(pixel > j*bladeNum-randHeight) {
						//Debug.Log ("Cutting at ("+i+","+pixel+")...");
						newColor.a = 0.0f;
					}
					texture.SetPixel(i,pixel,newColor);
				}
				i++;
			}
			for(int j = ((size/bladeNum)/2) - 1; j >= 0 ; j--) {
				for(int pixel = 0; pixel < size; pixel++) {
					Color newColor = texture.GetPixel(i,pixel);
					if(pixel > j*bladeNum-randHeight) {
						newColor.a = 0.0f;
						//Debug.Log ("Cutting at ("+i+","+pixel+")...");
					}
					texture.SetPixel(i,pixel,newColor);
				}
				i++;
			}
		}
	}
	
	public void SmoothAlpha() {
		int size = texture.width;
		for(int i = 0; i < size;i++) {
			for(int j = 0; j < size;j++) {
				float newAlpha = 0.0f;
				if(!(i==0||i==size||j==0||j==size)) {
					newAlpha = (texture.GetPixel(i,j).a + texture.GetPixel(i-1,j).a +
								texture.GetPixel(i+1,j).a + texture.GetPixel(i,j-1).a +
								texture.GetPixel(i,j+1).a)/5.0f;
				}
				Color color = texture.GetPixel(i,j);
				color.a = newAlpha;
				texture.SetPixel(i,j,color);
			}
		}
	}
	
	public void LeafTexture(int size, Color color) {
		texture = new Texture2D(size,size);
		
		// Define boundary of leaf
		
		for (int i = 0; i < size; i++) {
			for (int j = 0; j < size; j++) {
				bool inCircle = Mathf.Pow((float)size/2-i,2)+Mathf.Pow((float)size/2-j,2) < (size/2)*(size/2);
				float alpha = 0.0f;
				if(inCircle) {
					alpha = 1.0f;
				}
				texture.SetPixel(i,j,new Color(color.r,color.g,color.b,alpha));
			}
		}
	}
	
	public void AssignTexture(GameObject target, Material mat) {
		target.GetComponent<Renderer>().material = mat;
		AssignTexture(target,mat.shader);
	}
	
	public void AssignTexture(GameObject target, Shader shader) {
		if(target.GetComponent<Renderer>().material == null) {
			target.GetComponent<Renderer>().material = new Material(shader);
		} else {
			target.GetComponent<Renderer>().material.shader = shader;
		}
		AssignTexture(target);
	}
	public void AssignTexture(GameObject target) {
		texture.Apply();
		
		target.GetComponent<Renderer>().material.SetTexture("_MainTex",texture);
	}
	
	public void AssignTexture(GameObject target, Texture2D newTexture) {
		texture = newTexture;
		AssignTexture(target);
	}
	
	public Texture2D GetTexture() {
		return texture;
	}
	
	public Vector2 Bezier2D(Vector2 start, Vector2 controlMid1, Vector2 controlMid2, Vector2 end, float t)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		float mt = 1 - t;
		float mt2 = mt * mt;
		float mt3 = mt2 * mt;

		return start * mt3 + controlMid1 * mt2 * t * 3.0f + controlMid2 * mt * t2 * 3.0f + end * t3;
	}

	public Vector2 Bezier2DTangent(Vector2 start, Vector2 controlMid1, Vector2 controlMid2, Vector2 end, float t)
	{
		float t2 = t * t;

		float mt = 1 - t;
		float mt2 = mt * mt;

		float mid = 2.0f * t * mt;

		Vector3 tangent = start * -mt2 + controlMid1 * (mt2 - mid) + controlMid2 * (-t2 + mid) + end * t2;

		return tangent.normalized;
	}
	
	public Vector3 RGBtoHSV(float r, float g, float b) {
		throw new System.NotImplementedException();
	}
	
	public Color HSVtoRGB(float h, float s, float v) {
		throw new System.NotImplementedException();
	}
}