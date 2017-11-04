using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour {
	
	private bool generateTrees = true;
	
	public Texture2D grassTexture;
	public Texture2D stoneTexture;
	
	// Grass bounds
	
	float xMin = 200.0f;
	float xMax = 300.0f;
	float zMin = 200.0f;
	float zMax = 300.0f;
	
	// Grass variables
	private Material grassMat = null;
	
	private int grassCount = 0;
	private int grassLoadedPerUpdate = 100;
	private float grassDensity = 2.0f;
	private int grassMax = 0;
	private int grassMaxCap = 20000;
	
	// Rock variables
	private Material rockMat = null;
	
	private int rockCount = 0;
	private int rocksLoadedPerUpdate = 100;
	private float rockDensity = 0.1f;
	private int rockMax = 0;
	private int rockMaxCap = 5000;
	
	public void GenerateTerrain () {
		//Terrain terrain = gameObject.GetComponent("Terrain") as Terrain;
		Terrain terrain = Terrain.activeTerrain;
		int xSize = terrain.terrainData.heightmapWidth;
		int ySize = terrain.terrainData.heightmapHeight;
		float[,] heights = terrain.terrainData.GetHeights(0,0,xSize,ySize);
		
		// Fractal noise terrain
		heights = FractalNoise(xSize,ySize,new float[]{0.2f,0.2f,0.2f,0.2f},2.0f);
		//heights = Normalise2DArray(heights,xSize,ySize);
		//heights = Smooth2DArray(heights,xSize,ySize,10);
		
		// Perlin noise terrain
		
		for(int i = 0; i < xSize; i++) {
			for(int j = 0; j < ySize; j++) {
				float strength = 0.1f;
				float range = 5.0f;
				float noiseBig = Mathf.PerlinNoise(((float)i/(float)xSize)*range,((float)j/(float)ySize)*range)*strength;
				//float noiseSmall = Mathf.PerlinNoise(((float)i/(float)xSize)*range*10.0f,((float)j/(float)ySize)*range*10.0f)*strength;
				heights[i,j] += (noiseBig);
				
			}
		}
		
		terrain.terrainData.SetHeights(0,0,heights);
		
		
		float[,,] map = new float[terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight, 2];
		
		// For each point on the alphamap...
		for (int y = 0; y < terrain.terrainData.alphamapHeight; y++) {
			for (int x = 0; x < terrain.terrainData.alphamapWidth; x++) {
				// Get the normalized terrain coordinate that
				// corresponds to the the point.
				float normX = x * 1.0f / (float)(terrain.terrainData.alphamapWidth - 1);
				float normY = y * 1.0f / (float)(terrain.terrainData.alphamapHeight - 1);
				
				// Get the steepness value at the normalized coordinate.
				float angle = terrain.terrainData.GetSteepness(normX, normY);
				
				// Steepness is given as an angle, 0..90 degrees. Divide
				// by 90 to get an alpha blending value in the range 0..1.
				float frac = angle / 90.0f;
				//map[x, y, 0] = 1-frac;
				//map[x, y, 1] = frac;
				map[x, y, 0] = 1;
				map[x, y, 1] = 0;
			}
		}
		
		terrain.terrainData.SetAlphamaps(0, 0, map);
		
		// Set up grass growth
		grassMax = (int)Mathf.RoundToInt((float)(xMax-xMin)*(float)(zMax-zMin)*grassDensity*0.1f);
		GenerateGrassMaterial();
		
		// Set up rock placements
		rockMax = (int)Mathf.RoundToInt((float)(xMax-xMin)*(float)(zMax-zMin)*rockDensity*0.1f);
		GenerateRockMaterial();
	}
	
	void Update() {
		
		if (grassCount < grassMax && grassCount < grassMaxCap) {
			GenerateGrass(grassLoadedPerUpdate,xMin,xMax,zMin,zMax);
			grassCount += grassLoadedPerUpdate;
			Debug.Log("grassCount = " + grassCount);
		}
		/*
		if (rockCount < rockMax && rockCount < rockMaxCap) {
			GenerateRocks(rocksLoadedPerUpdate,xMin,xMax,zMin,zMax);
			rockCount += rocksLoadedPerUpdate;
			Debug.Log("rockCount = " + rockCount);
		}
		*/
		
	}
	
	private void GenerateGrass(int grassNum, float xMin, float xMax, float zMin, float zMax) {
		Terrain terrain = Terrain.activeTerrain;
		
		for(int i = 0; i < grassNum; i++) {
			GameObject newGrass = new GameObject("GrassInstance");
			newGrass.AddComponent<PGGrassStrip>();
			float randX = Random.Range(xMin,xMax);
			float randZ = Random.Range(zMin,zMax);
			float grassHeight = terrain.SampleHeight(new Vector3(randX,0.0f,randZ));
			newGrass.transform.position = new Vector3(randX,grassHeight,randZ);
			newGrass.AddComponent<MeshRenderer>();
			newGrass.GetComponent<Renderer>().material = grassMat;
		}
	}
	
	private void GenerateRocks(int rockNum, float xMin, float xMax, float zMin, float zMax) {
		Terrain terrain = Terrain.activeTerrain;
		for(int i = 0; i < rockNum; i++) {
			GameObject newRock = new GameObject("RockInstance");
			PGRock rockComp = newRock.AddComponent<PGRock>();
			rockComp.CreateObject();
			float randX = Random.Range(xMin,xMax);
			float randZ = Random.Range(zMin,zMax);
			float rockHeight = terrain.SampleHeight(new Vector3(randX,0.0f,randZ));
			newRock.transform.position = new Vector3(randX,rockHeight,randZ);
			newRock.GetComponent<Renderer>().material = rockMat;
		}
	}
	
	private void GenerateGrassMaterial() {
		TextureBuilder texturer = new TextureBuilder();
		Color grassColor = new Color(0.22f,0.64f,0.1f,1.0f);
		texturer.ColorTexture(256,grassColor);
		texturer.OverlayStripes(grassColor*0.8f,16,0.4f,0.0f,1.0f);
		texturer.CutOutGrassShape(32);
		Texture2D grassTex = texturer.GetTexture();
		grassMat = new Material("Default");
		grassMat.shader = Shader.Find("Transparent/Cutout/Diffuse");
		grassTex.mipMapBias = -0.5f;
		grassTex.Apply();
		grassMat.SetTexture("_MainTex",grassTex);
	}
	
	private void GenerateRockMaterial() {
		TextureBuilder texturer = new TextureBuilder();
		Color rockColor = new Color(0.4f,0.4f,0.4f,1.0f);
		texturer.ColorTexture(128,rockColor);
		texturer.OverlayStripes(rockColor*0.8f,16,0.4f,0.0f,1.0f);
		Texture2D rockTex = texturer.GetTexture();
		rockMat = new Material("Default");
		rockMat.shader = Shader.Find("Diffuse");
		rockTex.Apply();
		rockMat.SetTexture("_MainTex",rockTex);
	}
	
	public float[,] FractalNoise(int height, int width,float[] seeds, float bumpiness) {
		float[,] noise = new float[height,width];
		
		noise[0,0] = seeds[0];
		noise[0,width-1] = seeds[1];
		noise[height-1,0] = seeds[2];
		noise[height-1,width-1] = seeds[3];
		if(height>1) {
			noise[0,width/2] = (seeds[0]+seeds[1])/2.0f;
			noise[height/2,0] = (seeds[0]+seeds[2])/2.0f;
			noise[height-1,width/2] = (seeds[2]+seeds[3])/2.0f;
			noise[height/2,width-1] = (seeds[1]+seeds[3])/2.0f;
			noise[height/2,width/2] = (seeds[0]+seeds[1]+seeds[2]+seeds[3])/4.0f+Random.Range(-0.001f,0.001f)*height*Mathf.Pow(2.0f,-bumpiness);
			
			float[,] noiseTL = new float[height/2,width/2];
			float[,] noiseTR = new float[height/2,width/2];
			float[,] noiseBL = new float[height/2,width/2];
			float[,] noiseBR = new float[height/2,width/2];
			noiseTL = FractalNoise(height/2,width/2,new float[]{noise[0,0],noise[0,width/2],noise[height/2,0],noise[height/2,width/2]},bumpiness);
			noiseTR = FractalNoise(height/2,width/2,new float[]{noise[0,width/2],noise[0,width-1],noise[height/2,width/2],noise[height/2,width-1]},bumpiness);
			noiseBL = FractalNoise(height/2,width/2,new float[]{noise[height/2,0],noise[height/2,width/2],noise[height-1,0],noise[height-1,width/2]},bumpiness);
			noiseBR = FractalNoise(height/2,width/2,new float[]{noise[height/2,width/2],noise[height/2,width-1],noise[height-1,width/2],noise[height-1,width-1]},bumpiness);
			for(int i = 0; i < height/2; i++) {
				for(int j = 0; j < width/2; j++) {
					noise[i,j] = noiseTL[i,j];
					noise[i,j+width/2] = noiseTR[i,j];
					noise[i+height/2,j] = noiseBL[i,j];
					noise[i+height/2,j+width/2] = noiseBR[i,j];
				}
			}
		}
		
		return noise;
	}
	
	public float[,] Normalise2DArray(float[,]array, int arraySizeX, int arraySizeY) {
		float min = array[0,0];
		float max = array[0,0];
		
		float[,] nArray = new float[arraySizeX,arraySizeY];
		
		for(int i = 0; i < arraySizeX; i++) {
			for(int j = 0; j < arraySizeY; j++) {
				if(array[i,j] < min) {min = array[i,j];}
				if(array[i,j] > max) {max = array[i,j];}
			}
		}
		float range = max-min;
		for(int i = 0; i < arraySizeX; i++) {
			for(int j = 0; j < arraySizeY; j++) {
				nArray[i,j] = ((array[i,j]-min)/range);
			}
		}
		
		return nArray;
	}
	
	public float[,] Smooth2DArray(float[,] array, int sizeX, int sizeY, int smoothingLevel) {
		float[,] newArray = new float[sizeX,sizeY];
		newArray = array;
		/*
		for(int y = 0; y < sizeY; y++) {
			for(int x = 0; x < sizeX; x++) {
				float total = 0.0f;
				for(int s = -smoothingLevel; s <= smoothingLevel; s++) {
					int indexX = x+smoothingLevel;
					int indexY = y+smoothingLevel;
					// Clamp index between 0 and size-1
					if(indexX < 0) {indexX = 0;}
					if(indexX >= sizeX) {indexX = sizeX-1;}
					if(indexY < 0) {indexY = 0;}
					if(indexY >= sizeY) {indexY = sizeY-1;}
					total+=array[indexX,indexY];
				}
				newArray[x,y] = total/(float)(smoothingLevel*2+1);
			}
		}
		*/
		/*
		for(int x = 0; x < sizeX; x++) {
			for(int y = 0; y < sizeY; y++) {
				float total = 0.0f;
				for(int s = -smoothingLevel; s <= smoothingLevel; s++) {
					int index = y+smoothingLevel;
					// Clamp index between 0 and sizeX-1
					if(index < 0) {index = 0;}
					if(index >= sizeY) {index = sizeY-1;}
					total+=array[x,index];
				}
				newArray[x,y] = total/(float)(smoothingLevel*2+1);
			}
		}
		*/
		
		for(int i = 1; i < sizeX-1;i++) {
			for(int j = 1; j < sizeY-1;j++) {
				newArray[i,j] = (array[i,j] + array[i-1,j] + array[i+1,j] + array[i,j-1] + array[i,j+1])/5.0f;
			}
		}
		
		return newArray;
	}
}
