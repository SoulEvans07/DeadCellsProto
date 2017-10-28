import java.io.*;
import java.util.*;

public class WorkingMain{
	public static int imageWidth = 2048;
	public static int imageHeight = 4096;
	public static int startnum = 21300000;
	public static String fname = "simple.txt";
	public static ArrayList<Sprite> sprites = new ArrayList<>();
	
	public static void main(String[] args) {
		loadSprites();
		generateSpriteAtlas();
	}



	public static void log(Object o){
		System.out.print(o+"\n");
	}

	public static void loadSprites(){
//		try {
//			String content = new Scanner(new File(fname)).useDelimiter("\\Z").next();
			String content = getLines(fname);

			String[] split = content.split("\r\n");

			for(int i = 0; i < split.length; i+=7){
				Sprite tmp = new Sprite();
				tmp.name = split[i];				

				String rotate = split[i+1].replace("  rotate: ", "");
				tmp.rotate = Boolean.parseBoolean(rotate);

				String xy = split[i+2].replace("  xy: ", "");
				tmp.x = Integer.parseInt(xy.split(", ")[0]);
				tmp.y = Integer.parseInt(xy.split(", ")[1]);

				String size = split[i+3].replace("  size: ", "");
				tmp.w = Integer.parseInt(size.split(", ")[0]);
				tmp.h = Integer.parseInt(size.split(", ")[1]);

				String orig = split[i+4].replace("  orig: ", "");
				tmp.origX = Integer.parseInt(orig.split(", ")[0]);
				tmp.origY = Integer.parseInt(orig.split(", ")[1]);

				String off = split[i+5].replace("  offset: ", "");
				tmp.offX = Integer.parseInt(off.split(", ")[0]);
				tmp.offY = Integer.parseInt(off.split(", ")[1]);

				String index = split[i+6].replace("  index: ", "");
				tmp.index = Integer.parseInt(index);

				sprites.add(tmp);
			}
//		} catch (FileNotFoundException e) {
//			e.printStackTrace();
//		}
	}

	public static String getLines(String fileName){
		try (BufferedReader br = new BufferedReader(new FileReader(fileName))){
		    StringBuilder sb = new StringBuilder();
		    String line = br.readLine();

		    while (line != null) {
		        sb.append(line);
		        sb.append(System.lineSeparator());
		        line = br.readLine();
		    }
		    String everything = sb.toString();
		    return everything;
		} catch(Exception e) {
			System.out.println(e);
		}
		return null;
	}

	private static class Sprite{
		String name;
		boolean rotate;
		int x, y;
		int w, h;
		int origX, origY;
		int offX, offY;
		int index;

		@Override
		public String toString(){
			return 
			"name: " + name + "\n" + 
			"rotate: " + rotate + "\n" + 
			"xy: " + x + ", " + y + "\n" + 
			"size: " + w + ", " + h + "\n" + 
			"orig: " + origX + ", " + origY + "\n" +
			"offset: " + offX  + ", " + offY + "\n" +
			"index: " + index + "\n";
		}
	}

	public static void generateSpriteAtlas(){
		log("fileFormatVersion: 2");
		log("guid: 8a438e08ceed56b4eb9353b25c127d1c");
		log("timeCreated: 1508015774");
		log("licenseType: Free");
		log("TextureImporter:");
		log("  fileIDToRecycleName:");

		int i = 0;
		for(Sprite s : sprites){
			log("    " + (startnum+i) + ": " + s.name);
			i+=2;
		}

		log("  serializedVersion: 4");
		log("  mipmaps:");
		log("    mipMapMode: 0");
		log("    enableMipMap: 0");
		log("    sRGBTexture: 1");
		log("    linearTexture: 0");
		log("    fadeOut: 0");
		log("    borderMipMap: 0");
		log("    mipMapsPreserveCoverage: 0");
		log("    alphaTestReferenceValue: 0.5");
		log("    mipMapFadeDistanceStart: 1");
		log("    mipMapFadeDistanceEnd: 3");
		log("  bumpmap:");
		log("    convertToNormalMap: 0");
		log("    externalNormalMap: 0");
		log("    heightScale: 0.25");
		log("    normalMapFilter: 0");
		log("  isReadable: 0");
		log("  grayScaleToAlpha: 0");
		log("  generateCubemap: 6");
		log("  cubemapConvolution: 0");
		log("  seamlessCubemap: 0");
		log("  textureFormat: 1");
		log("  maxTextureSize: 2048");
		log("  textureSettings:");
		log("    serializedVersion: 2");
		log("    filterMode: 0");
		log("    aniso: -1");
		log("    mipBias: -1");
		log("    wrapU: 1");
		log("    wrapV: 1");
		log("    wrapW: 1");
		log("  nPOTScale: 0");
		log("  lightmap: 0");
		log("  compressionQuality: 50");
		log("  spriteMode: 2");
		log("  spriteExtrude: 1");
		log("  spriteMeshType: 1");
		log("  alignment: 0");
		log("  spritePivot: {x: 0.5, y: 0.5}");
		log("  spriteBorder: {x: 0, y: 0, z: 0, w: 0}");
		log("  spritePixelsToUnits: 100");
		log("  alphaUsage: 1");
		log("  alphaIsTransparency: 1");
		log("  spriteTessellationDetail: -1");
		log("  textureType: 8");
		log("  textureShape: 1");
		log("  maxTextureSizeSet: 0");
		log("  compressionQualitySet: 0");
		log("  textureFormatSet: 0");
		log("  platformSettings:");
		log("  - buildTarget: DefaultTexturePlatform");
		log("    maxTextureSize: 8192");
		log("    textureFormat: -1");
		log("    textureCompression: 0");
		log("    compressionQuality: 50");
		log("    crunchedCompression: 0");
		log("    allowsAlphaSplitting: 0");
		log("    overridden: 0");
		log("  - buildTarget: Standalone");
		log("    maxTextureSize: 8192");
		log("    textureFormat: -1");
		log("    textureCompression: 0");
		log("    compressionQuality: 50");
		log("    crunchedCompression: 0");
		log("    allowsAlphaSplitting: 0");
		log("    overridden: 0");
		log("  spriteSheet:");
		log("    serializedVersion: 2");
		log("    sprites:");

    	for(Sprite s : sprites){
    		log("    - serializedVersion: 2");
		    log("      name: " + s.name);
		    log("      rect:");
		    log("        serializedVersion: 2");
		    log("        x: " + s.x);
		    log("        y: " + (imageHeight - s.y - s.h));
		    log("        width: " + s.w);
		    log("        height: " + s.h);
		    log("      alignment: 0");
		    log("      pivot: {x: 0, y: 0}");
		    log("      border: {x: 0, y: 0, z: 0, w: 0}");
		    log("      outline: []");
		    log("      physicsShape: []");
		    log("      tessellationDetail: 0");
    	}

    	log("    outline: []");
		log("    physicsShape: []");
		log("  spritePackingTag: ");
		log("  userData: ");
		log("  assetBundleName: ");
		log("  assetBundleVariant: ");
	}
}