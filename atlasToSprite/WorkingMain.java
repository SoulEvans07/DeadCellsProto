import java.io.*;
import java.util.*;

public class WorkingMain{
	public static int imgWidth;
	public static int imgHeight;
	public static int START_NUM = 21300000;
	public static String inputdir = "input";
	
	public static void main(String[] args) {
		if(args.length != 0)
			inputdir = args[0];

		for(File atlas : searchByExt(inputdir, ".atlas")){
			String baseName = getBaseName(atlas.getName());
			StringBuilder meta = new StringBuilder();
			meta.append(getMetaBase(inputdir, baseName+".png.meta"));
			generateSpriteAtlas(meta, loadSprites(inputdir, atlas.getName()));
			saveMeta(meta, inputdir, baseName+".png.meta");
		}
	}

	public static void saveMeta(StringBuilder meta, String dir, String file){
		try{
			BufferedWriter writer = new BufferedWriter(new FileWriter(dir+"/"+file));
			writer.write(meta.toString());
			writer.close();
		} catch(IOException e) {
			System.out.println(e);
		}
	}

	public static String getBaseName(String filename){
		String[] parts = filename.split("\\.");
		return parts[0];
	}

	public static File[] searchByExt(String dir, String fileExt){
		File folder = new File(dir);
		File[] matching = folder.listFiles(new FilenameFilter(){
			public boolean accept(File dir, String name){
				return name.endsWith(fileExt);
			}
		});
		return matching;
	}

	public static String getMetaBase(String dir, String metafile){
		String meta = getLines(dir, metafile);
		String[] split = meta.split("\r\n");
		StringBuilder base = new StringBuilder();
		for(int i = 0; i < 3; i++)
			base.append(split[i]).append('\n');
		return base.toString();
	}

	public static void log(Object o){
		System.out.print(o+"\n");
	}

	public static ArrayList<Sprite> loadSprites(String dir, String filename){
		ArrayList<Sprite> sprites = new ArrayList<>();
		String content = getLines(dir, filename);

		String[] split = content.split("\r\n");

		String imgSize = split[2].replace("size: ", "");
		imgWidth = Integer.parseInt(imgSize.split(",")[0]);
		imgHeight = Integer.parseInt(imgSize.split(",")[1]);

		for(int i = 6; i < split.length; i+=7){
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

		return sprites;
	}

	public static String getLines(String dirName, String fileName){
		String path = (dirName != null || dirName.equals("")) ? dirName + "/" + fileName : fileName;
		try (BufferedReader br = new BufferedReader(new FileReader(path))){
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

	public static void generateSpriteAtlas(StringBuilder meta, ArrayList<Sprite> sprites){
		meta.append("TextureImporter:").append('\n');
		meta.append("  fileIDToRecycleName:").append('\n');

		int i = 0;
		for(Sprite s : sprites){
			meta.append("    " + (START_NUM+i) + ": " + s.name).append('\n');
			i+=2;
		}

		meta.append("  serializedVersion: 4").append('\n');
		meta.append("  mipmaps:").append('\n');
		meta.append("    mipMapMode: 0").append('\n');
		meta.append("    enableMipMap: 0").append('\n');
		meta.append("    sRGBTexture: 1").append('\n');
		meta.append("    linearTexture: 0").append('\n');
		meta.append("    fadeOut: 0").append('\n');
		meta.append("    borderMipMap: 0").append('\n');
		meta.append("    mipMapsPreserveCoverage: 0").append('\n');
		meta.append("    alphaTestReferenceValue: 0.5").append('\n');
		meta.append("    mipMapFadeDistanceStart: 1").append('\n');
		meta.append("    mipMapFadeDistanceEnd: 3").append('\n');
		meta.append("  bumpmap:").append('\n');
		meta.append("    convertToNormalMap: 0").append('\n');
		meta.append("    externalNormalMap: 0").append('\n');
		meta.append("    heightScale: 0.25").append('\n');
		meta.append("    normalMapFilter: 0").append('\n');
		meta.append("  isReadable: 0").append('\n');
		meta.append("  grayScaleToAlpha: 0").append('\n');
		meta.append("  generateCubemap: 6").append('\n');
		meta.append("  cubemapConvolution: 0").append('\n');
		meta.append("  seamlessCubemap: 0").append('\n');
		meta.append("  textureFormat: 1").append('\n');
		meta.append("  maxTextureSize: 2048").append('\n');
		meta.append("  textureSettings:").append('\n');
		meta.append("    serializedVersion: 2").append('\n');
		meta.append("    filterMode: 0").append('\n');
		meta.append("    aniso: -1").append('\n');
		meta.append("    mipBias: -1").append('\n');
		meta.append("    wrapU: 1").append('\n');
		meta.append("    wrapV: 1").append('\n');
		meta.append("    wrapW: 1").append('\n');
		meta.append("  nPOTScale: 0").append('\n');
		meta.append("  lightmap: 0").append('\n');
		meta.append("  compressionQuality: 50").append('\n');
		meta.append("  spriteMode: 2").append('\n');
		meta.append("  spriteExtrude: 1").append('\n');
		meta.append("  spriteMeshType: 1").append('\n');
		meta.append("  alignment: 0").append('\n');
		meta.append("  spritePivot: {x: 0.5, y: 0.5}").append('\n');
		meta.append("  spriteBorder: {x: 0, y: 0, z: 0, w: 0}").append('\n');
		meta.append("  spritePixelsToUnits: 100").append('\n');
		meta.append("  alphaUsage: 1").append('\n');
		meta.append("  alphaIsTransparency: 1").append('\n');
		meta.append("  spriteTessellationDetail: -1").append('\n');
		meta.append("  textureType: 8").append('\n');
		meta.append("  textureShape: 1").append('\n');
		meta.append("  maxTextureSizeSet: 0").append('\n');
		meta.append("  compressionQualitySet: 0").append('\n');
		meta.append("  textureFormatSet: 0").append('\n');
		meta.append("  platformSettings:").append('\n');
		meta.append("  - buildTarget: DefaultTexturePlatform").append('\n');
		meta.append("    maxTextureSize: 8192").append('\n');
		meta.append("    textureFormat: -1").append('\n');
		meta.append("    textureCompression: 0").append('\n');
		meta.append("    compressionQuality: 50").append('\n');
		meta.append("    crunchedCompression: 0").append('\n');
		meta.append("    allowsAlphaSplitting: 0").append('\n');
		meta.append("    overridden: 0").append('\n');
		meta.append("  - buildTarget: Standalone").append('\n');
		meta.append("    maxTextureSize: 8192").append('\n');
		meta.append("    textureFormat: -1").append('\n');
		meta.append("    textureCompression: 0").append('\n');
		meta.append("    compressionQuality: 50").append('\n');
		meta.append("    crunchedCompression: 0").append('\n');
		meta.append("    allowsAlphaSplitting: 0").append('\n');
		meta.append("    overridden: 0").append('\n');
		meta.append("  spriteSheet:").append('\n');
		meta.append("    serializedVersion: 2").append('\n');
		meta.append("    sprites:").append('\n');

    	for(Sprite s : sprites){
    		meta.append("    - serializedVersion: 2").append('\n');
		    meta.append("      name: " + s.name).append('\n');
		    meta.append("      rect:").append('\n');
		    meta.append("        serializedVersion: 2").append('\n');
		    meta.append("        x: " + s.x).append('\n');
		    meta.append("        y: " + (imgHeight - s.y - s.h)).append('\n');
		    meta.append("        width: " + s.w).append('\n');
		    meta.append("        height: " + s.h).append('\n');
		    meta.append("      alignment: 0").append('\n');
		    meta.append("      pivot: {x: 0, y: 0}").append('\n');
		    meta.append("      border: {x: 0, y: 0, z: 0, w: 0}").append('\n');
		    meta.append("      outline: []").append('\n');
		    meta.append("      physicsShape: []").append('\n');
		    meta.append("      tessellationDetail: 0").append('\n');
    	}

    	meta.append("    outline: []").append('\n');
		meta.append("    physicsShape: []").append('\n');
		meta.append("  spritePackingTag: ").append('\n');
		meta.append("  userData: ").append('\n');
		meta.append("  assetBundleName: ").append('\n');
		meta.append("  assetBundleVariant: ").append('\n');
	}
}